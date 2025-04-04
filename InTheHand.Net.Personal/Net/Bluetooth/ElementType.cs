// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ElementType
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public enum ElementType
  {
    Unknown = 0,
    Nil = 20, // 0x00000014
    UInt8 = 21, // 0x00000015
    UInt16 = 22, // 0x00000016
    UInt32 = 23, // 0x00000017
    UInt64 = 24, // 0x00000018
    UInt128 = 25, // 0x00000019
    Int8 = 30, // 0x0000001E
    Int16 = 31, // 0x0000001F
    Int32 = 32, // 0x00000020
    Int64 = 33, // 0x00000021
    Int128 = 34, // 0x00000022
    Uuid16 = 40, // 0x00000028
    Uuid32 = 41, // 0x00000029
    Uuid128 = 42, // 0x0000002A
    TextString = 43, // 0x0000002B
    Boolean = 44, // 0x0000002C
    ElementSequence = 45, // 0x0000002D
    ElementAlternative = 46, // 0x0000002E
    Url = 47, // 0x0000002F
  }
}
