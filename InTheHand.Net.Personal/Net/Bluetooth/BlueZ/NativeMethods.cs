// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueZ.NativeMethods
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueZ
{
  internal static class NativeMethods
  {
    private const string LibraryName = "bluetooth";

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern int hci_open_dev(int dev_id);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern int hci_close_dev(int dd);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern int hci_inquiry(
      int dev_id,
      int len,
      int num_rsp,
      IntPtr lap,
      ref IntPtr ppii,
      StackConsts.IREQ_int flags);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern int hci_inquiry(
      int dev_id,
      int len,
      int num_rsp,
      byte[] lap,
      ref IntPtr ppii,
      StackConsts.IREQ_int flags);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError hci_read_local_name(int dd, int len, byte[] name, int to);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError hci_write_local_name(int dd, byte[] name, int to);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError hci_read_remote_name(
      int dd,
      byte[] bdaddr,
      int len,
      byte[] nameBuf,
      int to);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError hci_read_local_version(
      int dd,
      ref Structs.hci_version ver,
      int to);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError hci_read_bd_addr(int dd, byte[] bdaddr, int to);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError hci_read_class_of_dev(int dd, byte[] cls, int to);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError hci_read_rssi(int dd, ushort handle, ref byte rssi, int to);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern int hci_get_route(IntPtr pbdaddr);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern int hci_get_route(byte[] bdaddr);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern IntPtr sdp_list_append(IntPtr list, IntPtr d);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern NativeMethods.SdpSessionSafeHandle sdp_connect(
      byte[] src,
      byte[] dst,
      StackConsts.SdpConnectFlags flags);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern int sdp_close(IntPtr session);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError sdp_service_search_attr_req(
      NativeMethods.SdpSessionSafeHandle session,
      IntPtr search,
      StackConsts.sdp_attrreq_type_t reqtype,
      IntPtr attrid_list,
      out IntPtr ppRsp_list);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern IntPtr sdp_extract_pdu(byte[] pdata, int len, out int scanned);

    [DllImport("bluetooth", SetLastError = true)]
    internal static extern BluezError sdp_record_register(
      NativeMethods.SdpSessionSafeHandle session,
      IntPtr rec,
      byte flags);

    internal delegate void sdp_free_func_t(IntPtr p);

    internal delegate int sdp_comp_func_t(IntPtr p1, IntPtr p2);

    internal delegate void sdp_callback_t(
      byte type,
      ushort status,
      ref byte rsp,
      IntPtr size,
      IntPtr udata);

    internal class SdpSessionSafeHandle : SafeHandle
    {
      public SdpSessionSafeHandle()
        : base(IntPtr.Zero, true)
      {
      }

      public override bool IsInvalid => this.handle == IntPtr.Zero;

      protected override bool ReleaseHandle() => NativeMethods.sdp_close(this.handle) >= 0;
    }
  }
}
