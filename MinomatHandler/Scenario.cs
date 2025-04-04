// Decompiled with JetBrains decompiler
// Type: MinomatHandler.Scenario
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public enum Scenario : byte
  {
    M1_Mesco = 0,
    M1_HCM = 1,
    M2 = 2,
    Invalid = 3,
    M2_1plus = 4,
    Standalone_M1_Mesco = 5,
    Standalone_M1_HCM = 6,
    Standalone_M2 = 7,
    M1_Fast_HCM = 8,
    M1_Fast_Mesco = 9,
    M1_Fast_HCM_Mesco = 10, // 0x0A
    M1_HCM_Mesco = 11, // 0x0B
    Standalone_M1_HCM_Mesco = 12, // 0x0C
    Radio3_Range_Test = 20, // 0x14
  }
}
