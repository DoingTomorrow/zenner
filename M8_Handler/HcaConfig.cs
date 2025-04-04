// Decompiled with JetBrains decompiler
// Type: M8_Handler.HcaConfig
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

using System;

#nullable disable
namespace M8_Handler
{
  [Flags]
  public enum HcaConfig : ushort
  {
    HCA_PSCALE = 2,
    HCA_SUMMER_OFF = 16, // 0x0010
    HCA_AUTOCAL = 32, // 0x0020
    HCA_SHOW_NTC = 64, // 0x0040
    HCA_XT_SUMMER = 128, // 0x0080
  }
}
