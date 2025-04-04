// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommRfcommPort
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommRfcommPort : IRfcommPort
  {
    private IntPtr m_pRfcommPort;
    private WidcommRfcommStreamBase m_parent;
    private WidcommRfcommPort.NativeBits.RfcommPort_DataReceivedCallbackDelegate m_handleDataReceived;
    private WidcommRfcommPort.NativeBits.RfcommPort_EventReceivedCallbackDelegate m_handleEvent;

    public void SetParentStream(WidcommRfcommStreamBase parent)
    {
      this.m_parent = this.m_parent == null ? parent : throw new InvalidOperationException("Can only have one parent.");
    }

    public void Create()
    {
      this.m_handleDataReceived = new WidcommRfcommPort.NativeBits.RfcommPort_DataReceivedCallbackDelegate(this.HandleDataReceived);
      this.m_handleEvent = new WidcommRfcommPort.NativeBits.RfcommPort_EventReceivedCallbackDelegate(this.HandleEventReceived);
      WidcommRfcommPort.NativeMethods.RfcommPort_Create(out this.m_pRfcommPort, this.m_handleDataReceived, this.m_handleEvent);
      MiscUtils.Trace_WriteLine("WidcommRfcommPort.Create'd: " + this.DebugId);
      if (this.m_pRfcommPort == IntPtr.Zero)
        throw new InvalidOperationException("Native object creation failed.");
    }

    public string DebugId
    {
      get
      {
        if (this.m_pRfcommPort == IntPtr.Zero)
          MiscUtils.Trace_WriteLine("Can't call get_DebugId before initialised.");
        return this.m_pRfcommPort.ToInt64().ToString("X");
      }
    }

    private void HandleDataReceived(IntPtr buffer, ushort len)
    {
      this.m_parent.HandlePortReceive(WidcommUtils.GetByteArray(buffer, (int) len), (IRfcommPort) this);
    }

    private void HandleEventReceived(uint eventId)
    {
      MiscUtils.Trace_WriteLine("{2} HandleEvent: {0}=0x{0:X}={1}", (object) eventId, (object) (PORT_EV) eventId, (object) DateTime.Now.TimeOfDay.ToString());
      this.m_parent.HandlePortEvent((PORT_EV) eventId, (IRfcommPort) this);
    }

    public PORT_RETURN_CODE OpenClient(int scn, byte[] address)
    {
      byte scn1 = checked ((byte) scn);
      if (scn1 < (byte) 1 || scn1 == byte.MaxValue || scn1 > (byte) 30)
        throw new ArgumentOutOfRangeException(nameof (scn), "Should be >0 and <31, is: " + (object) scn1 + ".");
      if (address == null || address.Length != 6)
        throw new ArgumentException("Parameter 'address' must be non-null and six-bytes long.");
      PORT_RETURN_CODE portReturnCode = WidcommRfcommPort.NativeMethods.RfcommPort_OpenClient(this.m_pRfcommPort, (int) scn1, address);
      MiscUtils.Trace_WriteLine("NativeMethods.RfcommPort_OpenClient ret: {0}=0x{0:X}", (object) portReturnCode);
      return portReturnCode;
    }

    public PORT_RETURN_CODE OpenServer(int scn)
    {
      byte scn1 = checked ((byte) scn);
      PORT_RETURN_CODE portReturnCode = scn1 >= (byte) 1 && scn1 != byte.MaxValue && scn1 <= (byte) 30 ? WidcommRfcommPort.NativeMethods.RfcommPort_OpenServer(this.m_pRfcommPort, (int) scn1) : throw new ArgumentOutOfRangeException(nameof (scn), "Should be >0 and <31, is: " + (object) scn1 + ".");
      MiscUtils.Trace_WriteLine("NativeMethods.RfcommPort_OpenServer ret: {0}=0x{0:X}", (object) portReturnCode);
      return portReturnCode;
    }

    public PORT_RETURN_CODE Write(byte[] data, ushort lenToWrite, out ushort lenWritten)
    {
      return WidcommRfcommPort.NativeMethods.RfcommPort_Write(this.m_pRfcommPort, data, lenToWrite, out lenWritten);
    }

    public bool IsConnected(out BluetoothAddress p_remote_bdaddr)
    {
      byte[] numArray = new byte[6];
      bool flag = WidcommRfcommPort.NativeMethods.RfcommPort_IsConnected(this.m_pRfcommPort, numArray, numArray.Length);
      p_remote_bdaddr = WidcommUtils.ToBluetoothAddress(numArray);
      return flag;
    }

    public PORT_RETURN_CODE Close()
    {
      return WidcommRfcommPort.NativeMethods.RfcommPort_Close(this.m_pRfcommPort);
    }

    public void Destroy()
    {
      if (!(this.m_pRfcommPort != IntPtr.Zero))
        return;
      WidcommRfcommPort.NativeMethods.RfcommPort_Destroy(this.m_pRfcommPort);
      this.m_pRfcommPort = IntPtr.Zero;
    }

    internal static class NativeBits
    {
      internal const string WidcommDll = "32feetWidcomm";

      internal delegate void RfcommPort_DataReceivedCallbackDelegate(IntPtr data, ushort len);

      internal delegate void RfcommPort_EventReceivedCallbackDelegate(uint data);
    }

    private static class NativeMethods
    {
      [DllImport("32feetWidcomm")]
      internal static extern IntPtr RfcommPort_Create(
        out IntPtr ppRfcommPort,
        WidcommRfcommPort.NativeBits.RfcommPort_DataReceivedCallbackDelegate handleDataReceived,
        WidcommRfcommPort.NativeBits.RfcommPort_EventReceivedCallbackDelegate handleEvent);

      [DllImport("32feetWidcomm")]
      internal static extern void RfcommPort_Destroy(IntPtr pRfcommPort);

      [DllImport("32feetWidcomm")]
      internal static extern PORT_RETURN_CODE RfcommPort_OpenClient(
        IntPtr pRfcommPort,
        int scn,
        byte[] address);

      [DllImport("32feetWidcomm")]
      internal static extern PORT_RETURN_CODE RfcommPort_OpenServer(IntPtr pRfcommPort, int scn);

      [DllImport("32feetWidcomm")]
      internal static extern PORT_RETURN_CODE RfcommPort_Write(
        IntPtr pRfcommPort,
        byte[] p_data,
        ushort len_to_write,
        out ushort p_len_written);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool RfcommPort_IsConnected(
        IntPtr pObj,
        [Out] byte[] p_remote_bdaddr,
        int bufLen);

      [DllImport("32feetWidcomm")]
      internal static extern PORT_RETURN_CODE RfcommPort_Close(IntPtr pRfcommPort);
    }
  }
}
