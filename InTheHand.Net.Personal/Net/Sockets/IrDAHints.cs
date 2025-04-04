// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.IrDAHints
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Sockets
{
  [Flags]
  public enum IrDAHints
  {
    None = 0,
    PnP = 1,
    PdaAndPalmtop = 2,
    Computer = 4,
    Printer = 8,
    Modem = 16, // 0x00000010
    Fax = 32, // 0x00000020
    LanAccess = 64, // 0x00000040
    Extension = 128, // 0x00000080
    Telephony = 256, // 0x00000100
    FileServer = 512, // 0x00000200
    IrCOMM = 1024, // 0x00000400
    Obex = 8192, // 0x00002000
  }
}
