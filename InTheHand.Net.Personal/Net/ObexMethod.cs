// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexMethod
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net
{
  public enum ObexMethod : byte
  {
    Put = 2,
    Get = 3,
    Connect = 128, // 0x80
    Disconnect = 129, // 0x81
    PutFinal = 130, // 0x82
    SetPath = 133, // 0x85
  }
}
