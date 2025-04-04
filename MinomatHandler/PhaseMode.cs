// Decompiled with JetBrains decompiler
// Type: MinomatHandler.PhaseMode
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public enum PhaseMode : byte
  {
    Init = 0,
    Setup = 1,
    Mur = 2,
    NetConst = 3,
    DatForwarding = 4,
    Standby = 5,
    CfgForwarding = 6,
    Netext = 7,
    Deactivate = 8,
    Unspecified = 255, // 0xFF
  }
}
