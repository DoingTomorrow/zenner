// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommL2CapClient
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal class WidcommL2CapClient : 
    CommonBluetoothClient,
    IL2CapClient,
    IBluetoothClient,
    IDisposable
  {
    private readonly WidcommBluetoothFactoryBase m_factory;
    private readonly WidcommL2CapClient.WidcommL2CapStream m_connRef;

    public static IL2CapClient Create()
    {
      return (IL2CapClient) new WidcommL2CapClient(WidcommBluetoothFactory.GetWidcommIfExists());
    }

    private static WidcommL2CapClient.WidcommL2CapStream factory_GetWidcommL2CapStream(
      WidcommBluetoothFactoryBase fcty)
    {
      IRfCommIf widcommL2CapIf = WidcommL2CapClient.GetWidcommL2CapIf(fcty);
      return new WidcommL2CapClient.WidcommL2CapStream(WidcommL2CapClient.GetWidcommL2CapPort(widcommL2CapIf), widcommL2CapIf, fcty);
    }

    internal static WidcommL2CapClient.WidcommL2CapStream GetWidcommL2CapStreamWithThisIf(
      WidcommBluetoothFactoryBase fcty,
      IRfCommIf intf)
    {
      return new WidcommL2CapClient.WidcommL2CapStream(WidcommL2CapClient.GetWidcommL2CapPort(intf), (IRfCommIf) null, fcty);
    }

    internal static IRfCommIf GetWidcommL2CapIf(WidcommBluetoothFactoryBase fcty)
    {
      L2CapIf child = new L2CapIf();
      return (IRfCommIf) new WidcommStRfCommIf(fcty, (IRfCommIf) child);
    }

    private static IRfcommPort GetWidcommL2CapPort(IRfCommIf intf)
    {
      return (IRfcommPort) new L2CapPort(intf);
    }

    internal static IBluetoothClient factory_DoGetBluetoothClientForListener(
      WidcommBluetoothFactoryBase fcty,
      CommonRfcommStream strm)
    {
      return (IBluetoothClient) new WidcommL2CapClient(fcty, strm);
    }

    internal WidcommL2CapClient(WidcommBluetoothFactoryBase factory)
      : this(factory, (CommonRfcommStream) WidcommL2CapClient.factory_GetWidcommL2CapStream(factory))
    {
    }

    internal WidcommL2CapClient(WidcommBluetoothFactoryBase factory, BluetoothEndPoint localEP)
      : this(factory)
    {
      throw new NotSupportedException("Don't support binding to a particular local address/port.");
    }

    internal WidcommL2CapClient(WidcommBluetoothFactoryBase factory, CommonRfcommStream conn)
      : base((BluetoothFactory) factory, conn)
    {
      this.m_factory = factory;
      this.m_connRef = (WidcommL2CapClient.WidcommL2CapStream) conn;
    }

    private WidcommBtInterface BtIf
    {
      [DebuggerStepThrough] get => this.m_factory.GetWidcommBtInterface();
    }

    public int GetMtu() => this.m_connRef.GetMtu();

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
        int[] psms = discoveryRecordsBuffer.Hack_GetPsms();
        MiscUtils.Trace_WriteLine("_GetPorts, got {0} records.", (object) discoveryRecordsBuffer.RecordCount);
        for (int index = psms.Length - 1; index >= 0; --index)
        {
          int num = psms[index];
          intList.Add(num);
        }
        return intList;
      }
    }

    public override void SetPin(string pin) => throw new NotImplementedException();

    public override void SetPin(BluetoothAddress device, string pin)
    {
      throw new NotImplementedException("Use this.SetPin or BluetoothSecurity.PairRequest...");
    }

    protected override List<IBluetoothDeviceInfo> GetKnownRemoteDeviceEntries()
    {
      throw new NotSupportedException();
    }

    protected override void BeginInquiry(
      int maxDevices,
      AsyncCallback callback,
      object state,
      BluetoothClient.LiveDiscoveryCallback liveDiscoHandler,
      object liveDiscoState,
      DiscoDevsParams args)
    {
      throw new NotSupportedException();
    }

    protected override List<IBluetoothDeviceInfo> EndInquiry(IAsyncResult ar)
    {
      throw new NotSupportedException();
    }

    internal delegate void L2CapConn_EventReceivedCallbackDelegate(uint eventId, uint data);

    internal enum MyL2CapEvent
    {
      IncomingConnection_Pending,
      ConnectPendingReceived,
      Connected,
      CongestionStatus,
      RemoteDisconnected,
    }

    internal static class NativeMethods
    {
      [DllImport("32feetWidcomm")]
      internal static extern void L2CapIf_Create(out IntPtr ppObj);

      [DllImport("32feetWidcomm")]
      internal static extern void L2CapIf_Destroy(IntPtr pObj);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool L2CapIf_AssignPsmValue(
        IntPtr pObj,
        ref Guid p_service_guid,
        ushort psm);

      [DllImport("32feetWidcomm")]
      internal static extern ushort L2CapIf_GetPsm(IntPtr pObj);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool L2CapIf_SetSecurityLevel(
        IntPtr pObj,
        string p_service_name,
        BTM_SEC security_level,
        [MarshalAs(UnmanagedType.Bool)] bool is_server);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool L2CapIf_Register(IntPtr pObj);

      [DllImport("32feetWidcomm")]
      internal static extern void L2CapIf_Deregister(IntPtr pObj);

      [DllImport("32feetWidcomm")]
      internal static extern void L2CapConn_Create(
        out IntPtr ppObj,
        WidcommRfcommPort.NativeBits.RfcommPort_DataReceivedCallbackDelegate handleDataReceived,
        WidcommL2CapClient.L2CapConn_EventReceivedCallbackDelegate handleEvent);

      [DllImport("32feetWidcomm")]
      internal static extern void L2CapConn_Destroy(IntPtr pObj);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool L2CapConn_Connect(IntPtr pObj, IntPtr pIf, byte[] pAddr);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool L2CapConn_Listen(IntPtr pObj, IntPtr pIf);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool L2CapConn_Accept(IntPtr _pIf);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool L2CapConn_Write(
        IntPtr pObj,
        byte[] p_data,
        ushort len_to_write,
        out ushort p_len_written);

      [DllImport("32feetWidcomm")]
      internal static extern void L2CapConn_Disconnect(IntPtr pObj);

      [DllImport("32feetWidcomm")]
      internal static extern void L2CapConn_GetRemoteBdAddr(
        IntPtr pObj,
        byte[] p_remote_bdaddr,
        int bufLen);

      [DllImport("32feetWidcomm")]
      internal static extern void L2CapConn_GetProperties(
        IntPtr pObj,
        [MarshalAs(UnmanagedType.Bool)] out bool pIsCongested,
        out ushort pMtu);
    }

    internal sealed class WidcommL2CapStream : WidcommRfcommStreamBase
    {
      private readonly L2CapPort _portRef;

      internal WidcommL2CapStream(
        IRfcommPort port,
        IRfCommIf rfCommIf,
        WidcommBluetoothFactoryBase factory)
        : base(port, rfCommIf, factory)
      {
        this._portRef = port != null ? (L2CapPort) port : throw new ArgumentNullException(nameof (port));
      }

      protected override void VerifyPortIsInRange(BluetoothEndPoint bep)
      {
        int port = (int) checked ((ushort) bep.Port);
      }

      internal int GetMtu() => this._portRef.GetMtu();
    }
  }
}
