// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ElementTypeDescriptor
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public enum ElementTypeDescriptor
  {
    Unknown = -1, // 0xFFFFFFFF
    Nil = 0,
    UnsignedInteger = 1,
    TwosComplementInteger = 2,
    Uuid = 3,
    TextString = 4,
    Boolean = 5,
    ElementSequence = 6,
    ElementAlternative = 7,
    Url = 8,
  }
}
