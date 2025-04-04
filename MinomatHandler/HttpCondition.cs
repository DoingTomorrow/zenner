// Decompiled with JetBrains decompiler
// Type: MinomatHandler.HttpCondition
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;

#nullable disable
namespace MinomatHandler
{
  [Flags]
  public enum HttpCondition : byte
  {
    None = 0,
    HttpActiv = 128, // 0x80
    HttpCanceled = 64, // 0x40
    Bit5 = 32, // 0x20
    Bit4 = 16, // 0x10
    Bit3 = 8,
    Bit2 = 4,
    Bit1 = 2,
    Bit0 = 1,
  }
}
