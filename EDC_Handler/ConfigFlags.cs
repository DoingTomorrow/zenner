// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ConfigFlags
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  [Flags]
  public enum ConfigFlags : ushort
  {
    CONFIG_ENABLE_PULSE = 1,
    CONFIG_ENABLE_RADIO = 2,
    CONFIG_LOG_ENABLED = 4,
    CONFIG_MAGNET_MASK = 8,
    CONFIG_FLOW_CHECK = 16, // 0x0010
    CONFIG_MAGNET_FOUND = 32, // 0x0020
  }
}
