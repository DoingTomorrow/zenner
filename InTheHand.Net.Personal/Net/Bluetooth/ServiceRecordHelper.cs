// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceRecordHelper
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public static class ServiceRecordHelper
  {
    public static ServiceElement GetRfcommChannelElement(ServiceRecord record)
    {
      return ServiceRecordHelper.GetChannelElement(record, BluetoothProtocolDescriptorType.Rfcomm);
    }

    public static ServiceElement GetL2CapChannelElement(ServiceRecord record)
    {
      return ServiceRecordHelper.GetChannelElement(record, BluetoothProtocolDescriptorType.L2Cap);
    }

    private static ServiceElement GetChannelElement(
      ServiceRecord record,
      BluetoothProtocolDescriptorType proto)
    {
      return record.Contains((ServiceAttributeId) 4) ? ServiceRecordHelper.GetChannelElement(record.GetAttributeById((ServiceAttributeId) 4), proto, out bool? _) : (ServiceElement) null;
    }

    internal static ServiceElement GetChannelElement(
      ServiceAttribute attr,
      BluetoothProtocolDescriptorType proto,
      out bool? isSimpleRfcomm)
    {
      if (proto != BluetoothProtocolDescriptorType.L2Cap && proto != BluetoothProtocolDescriptorType.Rfcomm)
        throw new ArgumentException("Can only fetch RFCOMM or L2CAP element.");
      isSimpleRfcomm = new bool?(true);
      ServiceElement serviceElement = attr.Value;
      if (serviceElement.ElementType == ElementType.ElementAlternative)
        Trace.WriteLine("Don't support ElementAlternative ProtocolDescriptorList values.");
      else if (serviceElement.ElementType != ElementType.ElementSequence)
      {
        Trace.WriteLine("Bad ProtocolDescriptorList base element.");
      }
      else
      {
        IEnumerator<ServiceElement> enumerator = serviceElement.GetValueAsElementList().GetEnumerator();
        if (!enumerator.MoveNext())
        {
          Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Protocol stack truncated before {0}.", (object) "L2CAP"));
        }
        else
        {
          IList<ServiceElement> valueAsElementList1 = enumerator.Current.GetValueAsElementList();
          if (valueAsElementList1[0].GetValueAsUuid() != BluetoothService.L2CapProtocol)
          {
            Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Bad protocol stack, layer {0} is not {1}.", (object) 1, (object) "L2CAP"));
          }
          else
          {
            bool flag1 = valueAsElementList1.Count != 1;
            isSimpleRfcomm = new bool?(isSimpleRfcomm.Value && !flag1);
            ServiceElement channelElement;
            if (proto == BluetoothProtocolDescriptorType.L2Cap)
            {
              if (valueAsElementList1.Count < 2)
              {
                Trace.WriteLine("L2CAP PSM element was requested but the L2CAP layer in this case hasn't a second element.");
                goto label_24;
              }
              else
                channelElement = valueAsElementList1[1];
            }
            else if (!enumerator.MoveNext())
            {
              Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Protocol stack truncated before {0}.", (object) "RFCOMM"));
              goto label_24;
            }
            else
            {
              IList<ServiceElement> valueAsElementList2 = enumerator.Current.GetValueAsElementList();
              if (valueAsElementList2[0].GetValueAsUuid() != BluetoothService.RFCommProtocol)
              {
                Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Bad protocol stack, layer {0} is not {1}.", (object) 2, (object) "RFCOMM"));
                goto label_24;
              }
              else if (valueAsElementList2.Count < 2)
              {
                Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Bad protocol stack, layer {0} hasn't a second element.", (object) 2));
                goto label_24;
              }
              else
              {
                channelElement = valueAsElementList2[1];
                if (channelElement.ElementType != ElementType.UInt8)
                {
                  Trace.WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Bad protocol stack, layer {0} is not UInt8.", (object) 2));
                  goto label_24;
                }
                else
                {
                  bool flag2 = enumerator.MoveNext();
                  isSimpleRfcomm = new bool?(isSimpleRfcomm.Value && !flag2);
                }
              }
            }
            return channelElement;
          }
        }
      }
label_24:
      isSimpleRfcomm = new bool?();
      return (ServiceElement) null;
    }

    public static int GetRfcommChannelNumber(ServiceRecord record)
    {
      ServiceElement rfcommChannelElement = ServiceRecordHelper.GetRfcommChannelElement(record);
      return rfcommChannelElement == null ? -1 : ServiceRecordHelper.GetRfcommChannelNumber(rfcommChannelElement);
    }

    internal static int GetRfcommChannelNumber(ServiceElement channelElement)
    {
      return (int) (byte) channelElement.Value;
    }

    public static int GetL2CapChannelNumber(ServiceRecord record)
    {
      ServiceElement capChannelElement = ServiceRecordHelper.GetL2CapChannelElement(record);
      return capChannelElement == null ? -1 : ServiceRecordHelper.GetL2CapChannelNumber(capChannelElement);
    }

    internal static int GetL2CapChannelNumber(ServiceElement channelElement)
    {
      return (int) (ushort) channelElement.Value;
    }

    public static void SetRfcommChannelNumber(ServiceRecord record, byte channelNumber)
    {
      (ServiceRecordHelper.GetRfcommChannelElement(record) ?? throw new InvalidOperationException("ProtocolDescriptorList element does not exist or is not in the RFCOMM format.")).SetValue((object) channelNumber);
    }

    public static void SetL2CapPsmNumber(ServiceRecord record, int psm)
    {
      ushort num = psm >= 0 && psm <= (int) ushort.MaxValue ? checked ((ushort) psm) : throw new ArgumentOutOfRangeException(nameof (psm), "A PSM is a UInt16 value.");
      ServiceRecordHelper.GetRfcommChannelElement(record);
      ServiceElement channelElement = ServiceRecordHelper.GetChannelElement(record, BluetoothProtocolDescriptorType.L2Cap);
      if (channelElement == null || channelElement.ElementType != ElementType.UInt16)
        throw new InvalidOperationException("ProtocolDescriptorList element does not exist, is not in the L2CAP format, or it the L2CAP layer has no PSM element.");
      channelElement.SetValue((object) num);
    }

    public static ServiceElement CreateL2CapProtocolDescriptorList()
    {
      return ServiceRecordHelper.CreateL2CapProtocolDescriptorListWithUpperLayers();
    }

    public static ServiceElement CreateRfcommProtocolDescriptorList()
    {
      return ServiceRecordHelper.CreateRfcommProtocolDescriptorListWithUpperLayers();
    }

    public static ServiceElement CreateGoepProtocolDescriptorList()
    {
      return ServiceRecordHelper.CreateRfcommProtocolDescriptorListWithUpperLayers(ServiceRecordHelper.CreatePdlLayer((ushort) 8));
    }

    private static ServiceElement CreateRfcommProtocolDescriptorListWithUpperLayers(
      params ServiceElement[] upperLayers)
    {
      IList<ServiceElement> childElements = (IList<ServiceElement>) new List<ServiceElement>();
      childElements.Add(ServiceRecordHelper.CreatePdlLayer((ushort) 256));
      childElements.Add(ServiceRecordHelper.CreatePdlLayer((ushort) 3, new ServiceElement(ElementType.UInt8, (object) (byte) 0)));
      foreach (ServiceElement upperLayer in upperLayers)
      {
        if (upperLayer.ElementType != ElementType.ElementSequence)
          throw new ArgumentException("Each layer in a ProtocolDescriptorList must be an ElementSequence.");
        childElements.Add(upperLayer);
      }
      return new ServiceElement(ElementType.ElementSequence, childElements);
    }

    public static ServiceElement CreateL2CapProtocolDescriptorListWithUpperLayers(
      params ServiceElement[] upperLayers)
    {
      IList<ServiceElement> childElements = (IList<ServiceElement>) new List<ServiceElement>();
      childElements.Add(ServiceRecordHelper.CreatePdlLayer((ushort) 256, new ServiceElement(ElementType.UInt16, (object) (ushort) 0)));
      foreach (ServiceElement upperLayer in upperLayers)
      {
        if (upperLayer.ElementType != ElementType.ElementSequence)
          throw new ArgumentException("Each layer in a ProtocolDescriptorList must be an ElementSequence.");
        childElements.Add(upperLayer);
      }
      return new ServiceElement(ElementType.ElementSequence, childElements);
    }

    private static ServiceElement CreatePdlLayer(ushort uuid, params ServiceElement[] data)
    {
      IList<ServiceElement> childElements = (IList<ServiceElement>) new List<ServiceElement>();
      ServiceElement serviceElement1 = new ServiceElement(ElementType.Uuid16, (object) uuid);
      childElements.Add(serviceElement1);
      foreach (ServiceElement serviceElement2 in data)
        childElements.Add(serviceElement2);
      return new ServiceElement(ElementType.ElementSequence, childElements);
    }

    internal static Guid _GetPrimaryServiceClassId(ServiceRecord sr)
    {
      return sr.GetAttributeById((ServiceAttributeId) 1).Value.GetValueAsElementList()[0].GetValueAsUuid();
    }
  }
}
