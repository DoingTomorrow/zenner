// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommSdpServiceCreator
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal class WidcommSdpServiceCreator : ServiceRecordCreator
  {
    private ServiceRecord m_record;
    private ISdpService m_sdpSvc;
    private bool aboveBase;
    private bool isAttrId;
    private bool isInSeqOrAlt;
    private bool needWrite;
    private ServiceAttributeId attrId;
    private SdpService.DESC_TYPE dt;
    private byte[] value;
    private static byte[] s_dummy = new byte[1024];
    private static readonly ServiceAttributeId[] MustWellKnownWriteAttributes = new ServiceAttributeId[1]
    {
      (ServiceAttributeId) 1
    };

    public void CreateServiceRecord(ServiceRecord record, ISdpService sdpService)
    {
      if (this.m_record != null)
        throw new InvalidOperationException("One at a time please.");
      Interlocked.Exchange<ServiceRecord>(ref this.m_record, record);
      try
      {
        this.m_sdpSvc = sdpService;
        this.CreateServiceRecord(record, WidcommSdpServiceCreator.s_dummy);
      }
      finally
      {
        Interlocked.Exchange<ServiceRecord>(ref this.m_record, (ServiceRecord) null);
      }
    }

    protected override void WriteAttribute(ServiceAttribute attr, byte[] buffer, ref int offset)
    {
      if (this.WriteWellKnownAttribute(attr, this.m_sdpSvc))
        return;
      if (Array.IndexOf<ServiceAttributeId>(WidcommSdpServiceCreator.MustWellKnownWriteAttributes, attr.Id) != -1)
        throw new NotImplementedException("MustWellKnownWriteAttributes: " + attr.Id.ToString("X") + ".");
      base.WriteAttribute(attr, buffer, ref offset);
      this.needWrite = false;
      this.m_sdpSvc.AddAttribute((ushort) this.attrId, this.dt, this.value != null ? this.value.Length : 0, this.value);
    }

    protected override int CreateAttrId(ServiceAttributeId attrId, byte[] buf, int offset)
    {
      this.attrId = attrId;
      this.isAttrId = true;
      int attrId1 = base.CreateAttrId(attrId, buf, offset);
      this.isAttrId = false;
      return attrId1;
    }

    protected override void WriteFixedLength(
      ServiceElement element,
      byte[] valueBytes,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      if (!this.isAttrId && !this.isInSeqOrAlt)
      {
        this.value = (byte[]) valueBytes.Clone();
        this.dt = WidcommSdpServiceCreator.ToDESC_TYPE(element.ElementTypeDescriptor);
        this.needWrite = true;
      }
      base.WriteFixedLength(element, valueBytes, buf, ref offset, out totalLength);
    }

    protected override void WriteVariableLength(
      ServiceElement element,
      byte[] valueBytes,
      byte[] buf,
      ref int offset,
      out int totalLength)
    {
      if (!this.isAttrId && !this.isInSeqOrAlt)
      {
        this.value = (byte[]) valueBytes.Clone();
        this.dt = WidcommSdpServiceCreator.ToDESC_TYPE(element.ElementTypeDescriptor);
        this.needWrite = true;
      }
      base.WriteVariableLength(element, valueBytes, buf, ref offset, out totalLength);
    }

    protected override int MakeVariableLengthHeader(
      byte[] buf,
      int offset,
      ElementTypeDescriptor etd,
      out ServiceRecordCreator.HeaderWriteState headerState)
    {
      bool flag = false;
      if (!this.aboveBase)
        this.aboveBase = true;
      else if (!this.isInSeqOrAlt && (etd == ElementTypeDescriptor.ElementSequence || etd == ElementTypeDescriptor.ElementAlternative))
        flag = true;
      int num = base.MakeVariableLengthHeader(buf, offset, etd, out headerState);
      if (flag)
      {
        this.isInSeqOrAlt = true;
        headerState.widcommNeedsStoring = true;
      }
      return num;
    }

    protected override void CompleteHeaderWrite(
      ServiceRecordCreator.HeaderWriteState headerState,
      byte[] buf,
      int offsetAtEndOfWritten,
      out int totalLength)
    {
      base.CompleteHeaderWrite(headerState, buf, offsetAtEndOfWritten, out totalLength);
      if (!headerState.widcommNeedsStoring)
        return;
      this.isInSeqOrAlt = false;
      int sourceIndex = headerState.HeaderOffset + headerState.HeaderLength;
      byte[] destinationArray = new byte[offsetAtEndOfWritten - sourceIndex];
      Array.Copy((Array) buf, sourceIndex, (Array) destinationArray, 0, destinationArray.Length);
      this.value = destinationArray;
      this.dt = WidcommSdpServiceCreator.ToDESC_TYPE(headerState.Etd);
      this.needWrite = true;
    }

    private static SdpService.DESC_TYPE ToDESC_TYPE(ElementTypeDescriptor elementTypeDescriptor)
    {
      switch (elementTypeDescriptor)
      {
        case ElementTypeDescriptor.UnsignedInteger:
          return SdpService.DESC_TYPE.UINT;
        case ElementTypeDescriptor.TwosComplementInteger:
          return SdpService.DESC_TYPE.TWO_COMP_INT;
        case ElementTypeDescriptor.Uuid:
          return SdpService.DESC_TYPE.UUID;
        case ElementTypeDescriptor.TextString:
          return SdpService.DESC_TYPE.TEXT_STR;
        case ElementTypeDescriptor.Boolean:
          return SdpService.DESC_TYPE.BOOLEAN;
        case ElementTypeDescriptor.ElementSequence:
          return SdpService.DESC_TYPE.DATA_ELE_SEQ;
        case ElementTypeDescriptor.ElementAlternative:
          return SdpService.DESC_TYPE.DATA_ELE_ALT;
        case ElementTypeDescriptor.Url:
          return SdpService.DESC_TYPE.URL;
        default:
          throw new ArgumentException("ToDESC_TYPE(" + (object) elementTypeDescriptor + ")");
      }
    }

    private bool WriteWellKnownAttribute(ServiceAttribute attr, ISdpService sdpService)
    {
      switch (attr.Id)
      {
        case (ServiceAttributeId) 1:
          sdpService.AddServiceClassIdList(WidcommSdpServiceCreator.ServiceRecordHelper_GetServiceClassIdList(attr));
          return true;
        case (ServiceAttributeId) 4:
          bool? isSimpleRfcomm;
          ServiceElement channelElement = ServiceRecordHelper.GetChannelElement(attr, BluetoothProtocolDescriptorType.Rfcomm, out isSimpleRfcomm);
          if (channelElement == null)
            return false;
          int rfcommChannelNumber = ServiceRecordHelper.GetRfcommChannelNumber(channelElement);
          if (rfcommChannelNumber == -1)
            return false;
          bool? nullable = isSimpleRfcomm;
          if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
            return false;
          sdpService.AddRFCommProtocolDescriptor(checked ((byte) rfcommChannelNumber));
          return true;
        default:
          return false;
      }
    }

    private static IList<Guid> ServiceRecordHelper_GetServiceClassIdList(ServiceAttribute attr)
    {
      ServiceElement serviceElement1 = attr.Value;
      IList<ServiceElement> serviceElementList = serviceElement1.ElementType == ElementType.ElementSequence ? serviceElement1.GetValueAsElementList() : throw new ArgumentException("ServiceClassIdList needs ElementSequence at base.");
      List<Guid> serviceClassIdList = new List<Guid>();
      foreach (ServiceElement serviceElement2 in (IEnumerable<ServiceElement>) serviceElementList)
      {
        if (serviceElement2.ElementTypeDescriptor != ElementTypeDescriptor.Uuid)
          throw new ArgumentException("ServiceClassIdList contains a " + (object) serviceElement2.ElementType + " element.");
        serviceClassIdList.Add(serviceElement2.GetValueAsUuid());
      }
      return (IList<Guid>) serviceClassIdList;
    }
  }
}
