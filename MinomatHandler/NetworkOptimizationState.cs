// Decompiled with JetBrains decompiler
// Type: MinomatHandler.NetworkOptimizationState
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public enum NetworkOptimizationState : byte
  {
    Error = 0,
    Unknown = 0,
    Successful = 1,
    IsSlave = 253, // 0xFD
    InvalidNetworkPhase = 254, // 0xFE
    NotAvailable = 255, // 0xFF
  }
}
