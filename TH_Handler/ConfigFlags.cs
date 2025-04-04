// Decompiled with JetBrains decompiler
// Type: TH_Handler.ConfigFlags
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using System;

#nullable disable
namespace TH_Handler
{
  [Flags]
  public enum ConfigFlags : ushort
  {
    SLEEP = 1,
    ENABLE_LCD = 2,
    ENABLE_RADIO = 4,
    ENABLE_LOG = 8,
    ENABLE_SHT2X = 16, // 0x0010
    ENABLE_NTC = 32, // 0x0020
    ENABLE_T = 64, // 0x0040
    ENABLE_RH = 128, // 0x0080
    FAHRENHEIT = 256, // 0x0100
  }
}
