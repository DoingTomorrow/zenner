// Decompiled with JetBrains decompiler
// Type: MinomatHandler.LED
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;

#nullable disable
namespace MinomatHandler
{
  [Flags]
  public enum LED : byte
  {
    None = 0,
    D1_Red = 1,
    D2_Yellow = 2,
    D3_Green = 4,
    D4_Green = 8,
  }
}
