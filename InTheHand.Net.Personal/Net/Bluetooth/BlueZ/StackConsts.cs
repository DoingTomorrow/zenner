// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueZ.StackConsts
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueZ
{
  internal static class StackConsts
  {
    internal const int BdaddrSize = 6;
    internal const SocketOptionLevel SOL_HCI = SocketOptionLevel.IP;
    internal const SocketOptionLevel SOL_L2CAP = SocketOptionLevel.Tcp;
    internal const SocketOptionLevel SOL_SCO = SocketOptionLevel.Udp;
    internal const SocketOptionLevel SOL_RFCOMM = (SocketOptionLevel) 18;
    internal const SocketOptionLevel SOL_BLUETOOTH = (SocketOptionLevel) 274;
    internal const int RFCOMM_DEFAULT_MTU = 127;
    internal const int RFCOMM_PSM = 3;
    internal const SocketOptionName so_RFCOMM_CONNINFO = SocketOptionName.AcceptConnection;
    internal const SocketOptionName so_RFCOMM_LM = SocketOptionName.TypeOfService;
    internal static readonly byte[] BDADDR_ANY = new byte[6];
    internal static readonly byte[] BDADDR_ALL = new byte[6]
    {
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue
    };
    internal static readonly byte[] BDADDR_LOCAL = new byte[6]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue
    };

    internal enum IREQ_int
    {
      IREQ_CACHE_FLUSH = 1,
    }

    internal enum SdpType_uint8_t : byte
    {
      DATA_NIL = 0,
      UINT8 = 8,
      UINT16 = 9,
      UINT32 = 10, // 0x0A
      UINT64 = 11, // 0x0B
      UINT128 = 12, // 0x0C
      INT8 = 16, // 0x10
      INT16 = 17, // 0x11
      INT32 = 18, // 0x12
      INT64 = 19, // 0x13
      INT128 = 20, // 0x14
      UUID_UNSPEC = 24, // 0x18
      UUID16 = 25, // 0x19
      UUID32 = 26, // 0x1A
      UUID128 = 28, // 0x1C
      TEXT_STR_UNSPEC = 32, // 0x20
      TEXT_STR8 = 37, // 0x25
      TEXT_STR16 = 38, // 0x26
      TEXT_STR32 = 39, // 0x27
      BOOL = 40, // 0x28
      SEQ_UNSPEC = 48, // 0x30
      SEQ8 = 53, // 0x35
      SEQ16 = 54, // 0x36
      SEQ32 = 55, // 0x37
      ALT_UNSPEC = 56, // 0x38
      ALT8 = 61, // 0x3D
      ALT16 = 62, // 0x3E
      ALT32 = 63, // 0x3F
      URL_STR_UNSPEC = 64, // 0x40
      URL_STR8 = 69, // 0x45
      URL_STR16 = 70, // 0x46
      URL_STR32 = 71, // 0x47
    }

    [Flags]
    internal enum SdpRecordRegisterFlags
    {
      SDP_RECORD_PERSIST = 1,
      SDP_DEVICE_RECORD = 2,
    }

    [Flags]
    internal enum SdpConnectFlags
    {
      SDP_RETRY_IF_BUSY = 1,
      SDP_WAIT_ON_CLOSE = 2,
      SDP_NON_BLOCKING = 4,
    }

    internal enum sdp_attrreq_type_t
    {
      SDP_ATTR_REQ_INDIVIDUAL = 1,
      SDP_ATTR_REQ_RANGE = 2,
    }

    [Flags]
    internal enum RFCOMM_LM
    {
      Master = 1,
      Auth = 2,
      Encrypt = 4,
      Trusted = 8,
      Reliable = 16, // 0x00000010
      Secure = 32, // 0x00000020
    }
  }
}
