// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBluetoothClient : CommonBluetoothClient
  {
    private readonly WidcommBluetoothFactoryBase m_factory;
    private readonly WidcommRfcommStreamBase m_connRef;
    private string m_passcode;
    private static bool s_useRegistryForGetKnownRemoteDevice = true;

    internal WidcommBluetoothClient(WidcommBluetoothFactoryBase factory)
      : this(factory, factory.GetWidcommRfcommStream())
    {
    }

    internal WidcommBluetoothClient(
      WidcommBluetoothFactoryBase factory,
      WidcommRfcommStreamBase conn)
      : base((BluetoothFactory) factory, (CommonRfcommStream) conn)
    {
      this.m_factory = factory;
      this.m_connRef = conn;
    }

    internal WidcommBluetoothClient(BluetoothEndPoint localEP, WidcommBluetoothFactoryBase factory)
      : this(factory)
    {
      throw new NotSupportedException("Don't support binding to a particular local address/port.");
    }

    internal WidcommBluetoothClient(
      WidcommRfcommStreamBase strm,
      WidcommBluetoothFactoryBase factory)
      : base((BluetoothFactory) factory, (CommonRfcommStream) strm)
    {
      this.m_factory = factory;
    }

    private WidcommBtInterface BtIf
    {
      [DebuggerStepThrough] get => this.m_factory.GetWidcommBtInterface();
    }

    private IAsyncResult ConnBeginConnect(
      BluetoothEndPoint remoteEP,
      AsyncCallback requestCallback,
      object state)
    {
      return this.m_connRef.BeginConnect(remoteEP, this.m_passcode, requestCallback, state);
    }

    public override IAsyncResult BeginServiceDiscovery(
      BluetoothAddress address,
      Guid serviceGuid,
      AsyncCallback asyncCallback,
      object state)
    {
      return this.BtIf.BeginServiceDiscovery(address, serviceGuid, SdpSearchScope.ServiceClassOnly, asyncCallback, state);
    }

    public override List<int> EndServiceDiscovery(IAsyncResult ar)
    {
      using (ISdpDiscoveryRecordsBuffer discoveryRecordsBuffer = this.BtIf.EndServiceDiscovery(ar))
      {
        List<int> intList = new List<int>();
        int[] ports = discoveryRecordsBuffer.Hack_GetPorts();
        MiscUtils.Trace_WriteLine("_GetPorts, got {0} records.", (object) discoveryRecordsBuffer.RecordCount);
        for (int index = ports.Length - 1; index >= 0; --index)
        {
          int num = ports[index];
          intList.Add(num);
        }
        return intList;
      }
    }

    public override void SetPin(string pin) => this.m_passcode = pin;

    public override void SetPin(BluetoothAddress device, string pin)
    {
      throw new NotImplementedException("Use this.SetPin or BluetoothSecurity.PairRequest...");
    }

    internal static bool ReadKnownDeviceFromTheRegistry
    {
      set => WidcommBluetoothClient.s_useRegistryForGetKnownRemoteDevice = value;
      get => WidcommBluetoothClient.s_useRegistryForGetKnownRemoteDevice;
    }

    protected override List<IBluetoothDeviceInfo> GetKnownRemoteDeviceEntries()
    {
      return !WidcommBluetoothClient.s_useRegistryForGetKnownRemoteDevice ? this.BtIf.GetKnownRemoteDeviceEntries() : this.BtIf.ReadKnownDevicesFromRegistry();
    }

    protected override void BeginInquiry(
      int maxDevices,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      DiscoDevsParams args)
    {
      this.BtIf.BeginInquiry(maxDevices, this.InquiryLength, callback, state, liveDiscoHandler, liveDiscoState, args);
    }

    protected override List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar)
    {
      return this.BtIf.EndInquiry(ar);
    }
  }
}
