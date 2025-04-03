// Decompiled with JetBrains decompiler
// Type: S3_Handler.ParamCode
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;

#nullable disable
namespace S3_Handler
{
  [Serializable]
  internal enum ParamCode
  {
    None = 0,
    Date = 16, // 0x00000010
    DateTime = 32, // 0x00000020
    LogDate = 48, // 0x00000030
    LogDateTime = 64, // 0x00000040
    LogValue = 80, // 0x00000050
    SimFunc0 = 96, // 0x00000060
  }
}
