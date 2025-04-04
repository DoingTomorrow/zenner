// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.PORT_RETURN_CODE
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal enum PORT_RETURN_CODE
  {
    SUCCESS = 0,
    UNKNOWN_ERROR = 1,
    ALREADY_OPENED = 2,
    NOT_OPENED = 3,
    LINE_ERR = 4,
    START_FAILED = 5,
    PAR_NEG_FAILED = 6,
    PORT_NEG_FAILED = 7,
    PEER_CONNECTION_FAILED = 8,
    PEER_TIMEOUT = 9,
    INVALID_PARAMETER = 10, // 0x0000000A
    NotSet = 32768, // 0x00008000
  }
}
