// Decompiled with JetBrains decompiler
// Type: MinolHandler.ValueInfo
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using System;

#nullable disable
namespace MinolHandler
{
  [Flags]
  internal enum ValueInfo
  {
    IsNotDefined = 256, // 0x00000100
    IsModified = 512, // 0x00000200
    IsFlashAddress = 1024, // 0x00000400
  }
}
