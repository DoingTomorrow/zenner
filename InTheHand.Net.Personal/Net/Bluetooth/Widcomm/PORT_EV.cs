// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.PORT_EV
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [Flags]
  internal enum PORT_EV : uint
  {
    RXCHAR = 1,
    RXFLAG = 2,
    TXEMPTY = 4,
    CTS = 8,
    DSR = 16, // 0x00000010
    RLSD = 32, // 0x00000020
    BREAK = 64, // 0x00000040
    ERR = 128, // 0x00000080
    RING = 256, // 0x00000100
    CTSS = 1024, // 0x00000400
    DSRS = 2048, // 0x00000800
    RLSDS = 4096, // 0x00001000
    OVERRUN = 8192, // 0x00002000
    TXCHAR = 16384, // 0x00004000
    CONNECTED = 512, // 0x00000200
    CONNECT_ERR = 32768, // 0x00008000
    FC = 65536, // 0x00010000
    FCS = 131072, // 0x00020000
  }
}
