// Decompiled with JetBrains decompiler
// Type: MinolHandler.ReadMode
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using System;

#nullable disable
namespace MinolHandler
{
  [Flags]
  public enum ReadMode
  {
    Ident = 1,
    LoggerMonth = 2,
    LoggerDay = 4,
    LoggerHour = 8,
    Logger = LoggerHour | LoggerDay | LoggerMonth, // 0x0000000E
    Complete = Logger | Ident, // 0x0000000F
  }
}
