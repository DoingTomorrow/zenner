// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.L2CapPort
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class L2CapPort : IRfcommPort
  {
    private IRfCommIf _intf;
    private IntPtr m_pPort;
    private WidcommRfcommStreamBase m_parent;
    private WidcommRfcommPort.NativeBits.RfcommPort_DataReceivedCallbackDelegate m_handleDataReceived;
    private WidcommL2CapClient.L2CapConn_EventReceivedCallbackDelegate m_handleEvent;

    internal L2CapPort(IRfCommIf intf) => this._intf = intf;

    public void SetParentStream(WidcommRfcommStreamBase parent)
    {
      this.m_parent = this.m_parent == null ? parent : throw new InvalidOperationException("Can only have one parent.");
    }

    public void Create()
    {
      this.m_handleDataReceived = new WidcommRfcommPort.NativeBits.RfcommPort_DataReceivedCallbackDelegate(this.HandleDataReceived);
      this.m_handleEvent = new WidcommL2CapClient.L2CapConn_EventReceivedCallbackDelegate(this.HandleEventReceived);
      WidcommL2CapClient.NativeMethods.L2CapConn_Create(out this.m_pPort, this.m_handleDataReceived, this.m_handleEvent);
      MiscUtils.Trace_WriteLine("WidcommRfcommPort.Create'd: " + this.DebugId);
      if (this.m_pPort == IntPtr.Zero)
        throw new InvalidOperationException("Native object creation failed.");
    }

    public string DebugId
    {
      get
      {
        if (this.m_pPort == IntPtr.Zero)
          MiscUtils.Trace_WriteLine("Can't call get_DebugId before initialised.");
        return this.m_pPort.ToInt64().ToString("X");
      }
    }

    private void HandleDataReceived(IntPtr buffer, ushort len)
    {
      MiscUtils.Trace_WriteLine("HandleReceive: len: {0}", (object) len);
      this.m_parent.HandlePortReceive(WidcommUtils.GetByteArray(buffer, (int) len), (IRfcommPort) this);
    }

    private void HandleEventReceived(uint eventId0, uint data)
    {
      WidcommL2CapClient.MyL2CapEvent eventId = (WidcommL2CapClient.MyL2CapEvent) eventId0;
      PORT_EV? nullable = L2CapPort.ConvertEvent(eventId, data);
      MiscUtils.Trace_WriteLine("{0} HandleEvent: was: '{1}'=0x{2:X} data: 0x{3:X}, mapped to: '{4}'", (object) DateTime.Now.TimeOfDay.ToString(), (object) eventId, (object) eventId0, (object) data, (object) nullable);
      if (eventId == WidcommL2CapClient.MyL2CapEvent.IncomingConnection_Pending)
        this.Accept();
      if (!nullable.HasValue)
        return;
      this.m_parent.HandlePortEvent(nullable.Value, (IRfcommPort) this);
    }

    private static PORT_EV? ConvertEvent(WidcommL2CapClient.MyL2CapEvent eventId, uint data)
    {
      PORT_EV? nullable = new PORT_EV?();
      switch (eventId)
      {
        case WidcommL2CapClient.MyL2CapEvent.Connected:
          nullable = new PORT_EV?(PORT_EV.CONNECTED);
          break;
        case WidcommL2CapClient.MyL2CapEvent.CongestionStatus:
          if (!Convert.ToBoolean(data))
          {
            nullable = new PORT_EV?(PORT_EV.TXEMPTY);
            break;
          }
          break;
        case WidcommL2CapClient.MyL2CapEvent.RemoteDisconnected:
          nullable = new PORT_EV?(PORT_EV.CONNECT_ERR);
          MiscUtils.Trace_WriteLine("  RemoteDisconnected reason: " + (object) (L2CapPort.L2CapDisconnectReason) data);
          break;
      }
      return nullable;
    }

    public PORT_RETURN_CODE OpenClient(int scn__, byte[] address)
    {
      if (address == null || address.Length != 6)
        throw new ArgumentException("Parameter 'address' must be non-null and six-bytes long.");
      PORT_RETURN_CODE portReturnCode = L2CapPort.ConvertResult(WidcommL2CapClient.NativeMethods.L2CapConn_Connect(this.m_pPort, this._intf.PObject, address));
      MiscUtils.Trace_WriteLine("WidcommL2CapClient.NativeMethods.L2CapConn_OpenClient ret: {0}=0x{0:X}", (object) portReturnCode);
      return portReturnCode;
    }

    public PORT_RETURN_CODE OpenServer(int scn__)
    {
      PORT_RETURN_CODE portReturnCode = L2CapPort.ConvertResult(WidcommL2CapClient.NativeMethods.L2CapConn_Listen(this.m_pPort, this._intf.PObject));
      MiscUtils.Trace_WriteLine("WidcommL2CapClient.NativeMethods.L2CapConn_OpenServer ret: {0}=0x{0:X}", (object) portReturnCode);
      return portReturnCode;
    }

    private static PORT_RETURN_CODE ConvertResult(bool p)
    {
      return p ? PORT_RETURN_CODE.SUCCESS : PORT_RETURN_CODE.UNKNOWN_ERROR;
    }

    public void Accept() => WidcommL2CapClient.NativeMethods.L2CapConn_Accept(this.m_pPort);

    public PORT_RETURN_CODE Write(byte[] data, ushort lenToWrite, out ushort lenWritten)
    {
      return L2CapPort.ConvertResult(WidcommL2CapClient.NativeMethods.L2CapConn_Write(this.m_pPort, data, lenToWrite, out lenWritten));
    }

    public bool IsConnected(out BluetoothAddress p_remote_bdaddr)
    {
      byte[] numArray = new byte[6];
      WidcommL2CapClient.NativeMethods.L2CapConn_GetRemoteBdAddr(this.m_pPort, numArray, numArray.Length);
      p_remote_bdaddr = WidcommUtils.ToBluetoothAddress(numArray);
      return true;
    }

    public PORT_RETURN_CODE Close()
    {
      MiscUtils.Trace_WriteLine("L2CapPort.Close(): " + this.DebugId);
      WidcommL2CapClient.NativeMethods.L2CapConn_Disconnect(this.m_pPort);
      return PORT_RETURN_CODE.SUCCESS;
    }

    public void Destroy()
    {
      MiscUtils.Trace_WriteLine("L2CapPort.Destroy(): " + this.DebugId);
      if (!(this.m_pPort != IntPtr.Zero))
        return;
      WidcommL2CapClient.NativeMethods.L2CapConn_Destroy(this.m_pPort);
      this.m_pPort = IntPtr.Zero;
    }

    internal int GetMtu()
    {
      ushort pMtu;
      WidcommL2CapClient.NativeMethods.L2CapConn_GetProperties(this.m_pPort, out bool _, out pMtu);
      return (int) pMtu;
    }

    private enum L2CapDisconnectReason
    {
      DisconnectInd = 0,
      XXPending = 1,
      PsmNotSupported = 2,
      SecurityBlock = 3,
      NoResources = 4,
      CfgUnacceptable_params = 5,
      CfgFailedNoReason = 6,
      CfgUnknownOptions = 7,
      NoLink = 255, // 0x000000FF
      Timeout = 61166, // 0x0000EEEE
      LocalPowerOff = 65534, // 0x0000FFFE
    }
  }
}
