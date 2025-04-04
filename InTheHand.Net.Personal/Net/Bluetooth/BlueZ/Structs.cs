// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueZ.Structs
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Sockets;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueZ
{
  internal static class Structs
  {
    internal const string SDP_UNIX_PATH = "/var/run/sdp";
    internal const int SDP_REQ_BUFFER_SIZE = 2048;
    internal const int SDP_RSP_BUFFER_SIZE = 65535;
    internal const int SDP_PDU_CHUNK_SIZE = 1024;

    internal struct inquiry_info
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
      internal readonly byte[] bdaddr;
      internal readonly byte pscan_rep_mode;
      internal readonly byte pscan_period_mode;
      internal readonly byte pscan_mode;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      internal readonly byte[] dev_class;
      internal readonly ushort clock_offset;
    }

    internal struct hci_version
    {
      internal readonly ushort manufacturer;
      internal readonly byte hci_ver;
      internal readonly ushort hci_rev;
      internal readonly byte lmp_ver;
      internal readonly ushort lmp_subver;

      internal hci_version(HciVersion fake)
        : this()
      {
        DEV_VER_INFO.SetManufacturerAndVersionsToUnknown(out this.manufacturer, out this.hci_ver, out this.lmp_ver);
      }
    }

    internal struct uuid_t
    {
      private const int PADDING_SIZEa = 3;
      private const int ValueLength = 16;
      internal readonly StackConsts.SdpType_uint8_t type;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      private byte[] PADDING;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
      internal readonly byte[] value;

      internal uuid_t(Guid uuid)
      {
        this.type = StackConsts.SdpType_uint8_t.UUID128;
        this.PADDING = new byte[3];
        this.value = BluetoothListener.HostToNetworkOrder(uuid).ToByteArray();
      }

      internal uuid_t(uint uuid)
      {
        this.type = StackConsts.SdpType_uint8_t.UUID32;
        this.PADDING = new byte[3];
        byte[] bytes = BitConverter.GetBytes(uuid);
        this.value = new byte[16];
        Array.Copy((Array) bytes, (Array) this.value, bytes.Length);
      }

      internal uuid_t(ushort uuid)
      {
        this.type = StackConsts.SdpType_uint8_t.UUID16;
        this.PADDING = new byte[3];
        byte[] bytes = BitConverter.GetBytes(uuid);
        this.value = new byte[16];
        Array.Copy((Array) bytes, (Array) this.value, bytes.Length);
      }
    }

    private static class TestUuids
    {
      private static bool _TestUuidsOnce;

      internal static void Test()
      {
        if (Structs.TestUuids._TestUuidsOnce)
          return;
        Structs.TestUuids._TestUuidsOnce = true;
        Structs.TestUuids.DumpUuidt(new Structs.uuid_t((ushort) 256));
        Structs.TestUuids.DumpUuidt(new Structs.uuid_t(256U));
        Structs.TestUuids.DumpUuidt(new Structs.uuid_t(BluetoothService.L2CapProtocol));
      }

      internal static void DumpUuidt(Structs.uuid_t uuid)
      {
        int cb = Marshal.SizeOf((object) uuid);
        IntPtr num = Marshal.AllocHGlobal(cb);
        Marshal.StructureToPtr((object) uuid, num, false);
        try
        {
          byte[] destination = new byte[cb];
          Marshal.Copy(num, destination, 0, destination.Length);
          Console.WriteLine("uuid_t: {0} (len: {1})", (object) BitConverter.ToString(destination), (object) cb);
        }
        finally
        {
          Marshal.FreeHGlobal(num);
        }
      }
    }

    internal struct sdp_list_t
    {
      internal readonly IntPtr next;
      internal readonly IntPtr data;
    }

    private struct sdp_lang_attr_t
    {
      internal readonly ushort code_ISO639;
      internal readonly ushort encoding;
      internal readonly ushort base_offset;
    }

    internal struct sdp_record_t
    {
      internal readonly uint handle;
      internal readonly IntPtr pattern;
      internal readonly IntPtr attrlist;
    }

    internal struct sdp_data_struct__Bytes
    {
      internal const int SizeOfVal = 20;
      internal readonly StackConsts.SdpType_uint8_t dtd;
      internal readonly ushort attrId;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
      internal readonly byte[] val;
      internal readonly IntPtr next;
      internal readonly int unitSize;

      static sdp_data_struct__Bytes()
      {
        Structs.sdp_data_struct__Bytes.AssertAll(typeof (Structs.sdp_data_struct__Bytes));
      }

      internal static void AssertSize(Type type)
      {
        int num1 = 32 + 2 * (IntPtr.Size - 4);
        int num2 = Marshal.SizeOf(type);
        if (num2 != num1)
          throw new InvalidOperationException("Wrong size of type " + type.Name + " expected: " + (object) num1 + " but was: " + (object) num2);
      }

      internal static void AssertAll(Type type)
      {
        Structs.sdp_data_struct__Bytes.AssertSize(type);
        \u003C\u003Ef__AnonymousType0<string, int>[] dataArray = new \u003C\u003Ef__AnonymousType0<string, int>[5]
        {
          new{ Name = "dtd", Offset = 0 },
          new{ Name = "attrId", Offset = 2 },
          new{ Name = "val", Offset = 4 },
          new{ Name = "next", Offset = 24 },
          new{ Name = "unitSize", Offset = 28 }
        };
        foreach (var data in dataArray)
        {
          long int64 = Marshal.OffsetOf(type, data.Name).ToInt64();
          if (int64 != (long) data.Offset)
            throw new InvalidOperationException("Wrong field offset in type " + type.Name + " expected: " + (object) data.Offset + " but was: " + (object) int64);
        }
      }

      internal IntPtr ReadIntPtr()
      {
        IntPtr num;
        switch (IntPtr.Size)
        {
          case 4:
            num = new IntPtr(BitConverter.ToInt32(this.val, 0));
            break;
          case 8:
            num = new IntPtr(BitConverter.ToInt64(this.val, 0));
            break;
          default:
            throw new NotImplementedException("Pointer size is not 4 or 8 bytes: " + (object) IntPtr.Size + ".");
        }
        return num;
      }
    }

    internal struct sdp_data_struct__Debug
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
      internal readonly byte[] all;

      static sdp_data_struct__Debug()
      {
        Structs.sdp_data_struct__Bytes.AssertSize(typeof (Structs.sdp_data_struct__Debug));
      }
    }

    internal struct sdp_data_struct__uuid_t
    {
      internal readonly StackConsts.SdpType_uint8_t dtd;
      internal readonly ushort attrId;
      internal readonly Structs.uuid_t val;
      internal readonly IntPtr next;
      internal readonly int unitSize;

      static sdp_data_struct__uuid_t()
      {
        Structs.sdp_data_struct__Bytes.AssertAll(typeof (Structs.sdp_data_struct__uuid_t));
      }
    }

    private struct sdp_session_t_Real
    {
      private int sock;
      private int state;
      private int local;
      private int flags;
      private ushort tid;
      private IntPtr priv;
    }

    private struct rfcomm_conninfo
    {
      private ushort hci_handle;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
      private byte[] dev_class;
    }

    private static class StructUtils
    {
      internal static void AssertSize<T>(ref T stru, int expectedSize) where T : struct
      {
        int num = Marshal.SizeOf((object) stru);
        if (num != expectedSize)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Wrong STRUCT size {0}, expected {1} but was {2}.", (object) stru.GetType().Name, (object) expectedSize, (object) num));
      }
    }
  }
}
