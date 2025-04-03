// Decompiled with JetBrains decompiler
// Type: S3_Handler.ControlLogger
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

#nullable disable
namespace S3_Handler
{
  internal enum ControlLogger : ushort
  {
    None = 0,
    LoggerReset = 4096, // 0x1000
    LoggerDueDateReset = 8192, // 0x2000
    LoggerNext = 12288, // 0x3000
    LoggerChangeChanal = 16384, // 0x4000
    LoggerNextChangeChanal = 20480, // 0x5000
  }
}
