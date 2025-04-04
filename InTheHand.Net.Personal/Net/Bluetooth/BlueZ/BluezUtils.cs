// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueZ.BluezUtils
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueZ
{
  internal static class BluezUtils
  {
    private const int SocketError_NotSocket = 10038;

    internal static bool IsSuccess(BluezError ret) => ret >= (BluezError) 0;

    [DebuggerNonUserCode]
    internal static void Assert(BluezError ret, string descr)
    {
    }

    [DebuggerNonUserCode]
    internal static void CheckAndThrow(BluezError ret, string descr)
    {
      if (ret >= (BluezError) 0)
        return;
      BluezUtils.Throw(ret, descr);
    }

    [DebuggerNonUserCode]
    internal static void Throw(BluezError ret0, string descr)
    {
      int lastWin32Error = Marshal.GetLastWin32Error();
      int? nullable = new int?();
      if (lastWin32Error == 112)
        nullable = new int?(10064);
      if (!nullable.HasValue)
        nullable = new int?(10038);
      string str = "<disabled>";
      Console.WriteLine("BluezUtils.Throw: '" + descr + "', ret: " + (object) ret0 + " at: " + str);
      throw new SocketException(nullable.Value);
    }

    internal static BluetoothAddress ToBluetoothAddress(byte[] address_)
    {
      return BluetoothAddress.CreateFromLittleEndian(address_);
    }

    internal static byte[] FromBluetoothAddress(BluetoothAddress address)
    {
      return address.ToByteArrayLittleEndian();
    }

    internal static string FromNameString(byte[] arr, int? bufferLen)
    {
      int num = Array.IndexOf<byte>(arr, (byte) 0);
      int count = num != -1 ? num : arr.Length;
      if (bufferLen.HasValue && bufferLen.Value < count)
        count = bufferLen.Value;
      return Encoding.UTF8.GetString(arr, 0, count);
    }

    internal static string FromNameString(byte[] arr) => BluezUtils.FromNameString(arr, new int?());

    internal static byte[] ToNameString(string name)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(name);
      byte[] numArray = bytes.Length <= 248 ? new byte[bytes.Length + 1] : throw new ArgumentException("Name is too long, must be 248 chars or less (actually 248 bytes in UTF8 encoding).");
      bytes.CopyTo((Array) numArray, 0);
      return numArray.Length <= 249 ? numArray : throw new ArgumentException("Name is too long, must be 248 chars or less (actually 248 bytes in UTF8 encoding).");
    }

    internal static ClassOfDevice ToClassOfDevice(byte[] p)
    {
      return new ClassOfDevice((uint) p[0] + (uint) (256 * ((int) p[1] + 256 * (int) p[2])));
    }

    internal static IntPtr malloc(int size) => Marshal.AllocHGlobal(size);

    internal static void free(IntPtr p) => Marshal.FreeHGlobal(p);

    internal static void close(int sock)
    {
    }

    internal static IntPtr sdp_list_append<T>(IntPtr list, T val, List<IntPtr> listAllocs) where T : struct
    {
      if ((ValueType) val is IntPtr)
        throw new RankException("1");
      if (typeof (T) == typeof (IntPtr))
        throw new RankException("2");
      IntPtr d = BluezUtils.Malloc<T>(ref val, listAllocs);
      IntPtr num = NativeMethods.sdp_list_append(list, d);
      return !(num == IntPtr.Zero) ? num : throw new InvalidOperationException("sdp_list_append(STRU)");
    }

    internal static IntPtr Malloc<T>(ref T val, List<IntPtr> listAllocs) where T : struct
    {
      IntPtr ptr = BluezUtils.malloc(Marshal.SizeOf((object) val));
      if (ptr == IntPtr.Zero)
        throw new InvalidOperationException("malloc");
      listAllocs.Add(ptr);
      Marshal.StructureToPtr((object) val, ptr, false);
      Marshal.ReadInt64(ptr);
      return ptr;
    }
  }
}
