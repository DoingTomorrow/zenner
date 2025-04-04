// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.SdpDiscoveryRecordsBufferBase
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal abstract class SdpDiscoveryRecordsBufferBase : ISdpDiscoveryRecordsBuffer, IDisposable
  {
    protected readonly ServiceDiscoveryParams m_request;
    private List<ServiceRecord> m_records;

    protected SdpDiscoveryRecordsBufferBase(ServiceDiscoveryParams query) => this.m_request = query;

    public abstract int RecordCount { get; }

    public abstract int[] Hack_GetPorts();

    public abstract int[] Hack_GetPsms();

    public ServiceRecord[] GetServiceRecords()
    {
      if (this.m_records == null)
      {
        this.m_records = new List<ServiceRecord>();
        foreach (SdpDiscoveryRecordsBufferBase.SimpleInfo simpleInfo in this.GetSimpleInfo())
        {
          int num1;
          int num2 = num1 = 0;
          ServiceRecordBuilder serviceRecordBuilder = new ServiceRecordBuilder();
          serviceRecordBuilder.AddCustomAttribute(new ServiceAttribute((ServiceAttributeId) -1, new ServiceElement(ElementType.TextString, (object) "<partial Widcomm decode>")));
          serviceRecordBuilder.AddServiceClass(simpleInfo.serviceUuid);
          if (this.m_request.serviceGuid == simpleInfo.serviceUuid)
            ++num2;
          if (simpleInfo.serviceNameWchar != IntPtr.Zero)
          {
            string stringUni = Marshal.PtrToStringUni(simpleInfo.serviceNameWchar);
            if (stringUni.Length != 0)
              serviceRecordBuilder.ServiceName = stringUni;
          }
          else if (simpleInfo.serviceNameChar != IntPtr.Zero)
          {
            string stringAnsi = Marshal.PtrToStringAnsi(simpleInfo.serviceNameChar);
            if (stringAnsi.Length != 0)
              serviceRecordBuilder.ServiceName = stringAnsi;
          }
          if (simpleInfo.scn == -1)
            serviceRecordBuilder.ProtocolType = BluetoothProtocolDescriptorType.None;
          switch (serviceRecordBuilder.ProtocolType)
          {
            case BluetoothProtocolDescriptorType.None:
              if (this.m_request.serviceGuid == BluetoothService.L2CapProtocol)
              {
                ++num1;
                break;
              }
              break;
            case BluetoothProtocolDescriptorType.Rfcomm:
              if (this.m_request.serviceGuid == BluetoothService.RFCommProtocol)
                ++num1;
              if (this.m_request.serviceGuid == BluetoothService.L2CapProtocol)
              {
                ++num1;
                break;
              }
              break;
            case BluetoothProtocolDescriptorType.GeneralObex:
              if (this.m_request.serviceGuid == BluetoothService.ObexProtocol)
              {
                ++num1;
                goto case BluetoothProtocolDescriptorType.Rfcomm;
              }
              else
                goto case BluetoothProtocolDescriptorType.Rfcomm;
          }
          ServiceRecord serviceRecord = serviceRecordBuilder.ServiceRecord;
          if (simpleInfo.scn != -1)
            ServiceRecordHelper.SetRfcommChannelNumber(serviceRecord, checked ((byte) simpleInfo.scn));
          if (num2 > 0 || num1 > 0)
          {
            MiscUtils.Trace_WriteLine("Adding record");
            this.m_records.Add(serviceRecord);
          }
          else
            MiscUtils.Trace_WriteLine("Skipping record");
        }
      }
      return this.m_records.ToArray();
    }

    protected abstract SdpDiscoveryRecordsBufferBase.SimpleInfo[] GetSimpleInfo();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected abstract void Dispose(bool disposing);

    protected abstract void EnsureNotDisposed();

    internal struct SimpleInfo
    {
      internal readonly Guid serviceUuid;
      internal readonly IntPtr serviceNameWchar;
      internal readonly IntPtr serviceNameChar;
      internal readonly int scn;

      public SimpleInfo(int TheseFieldsAreSetByPInvoke)
      {
        this.serviceUuid = Guid.Empty;
        this.serviceNameWchar = IntPtr.Zero;
        this.serviceNameChar = IntPtr.Zero;
        this.scn = -1;
      }

      internal SimpleInfo(Guid serviceUuid, int scn)
      {
        this.serviceUuid = serviceUuid;
        this.scn = scn;
        this.serviceNameWchar = IntPtr.Zero;
        this.serviceNameChar = IntPtr.Zero;
      }
    }
  }
}
