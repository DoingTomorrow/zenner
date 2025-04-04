// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.HciVersion
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public enum HciVersion : byte
  {
    v1_0_b = 0,
    v1_1 = 1,
    v1_2 = 2,
    v2_0wEdr = 3,
    v2_1wEdr = 4,
    v3_0wHS = 5,
    v4_0 = 6,
    Unknown = 255, // 0xFF
  }
}
