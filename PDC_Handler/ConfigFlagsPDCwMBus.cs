// Decompiled with JetBrains decompiler
// Type: PDC_Handler.ConfigFlagsPDCwMBus
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;

#nullable disable
namespace PDC_Handler
{
  [Flags]
  public enum ConfigFlagsPDCwMBus : ushort
  {
    CONFIG_ENABLE_PULSE = 1,
    CONFIG_ENABLE_RADIO = 2,
    CONFIG_LOG_ENABLED = 4,
    CONFIG_FLOW_CHECK = 8,
    CONFIG_DISABLE_A = 256, // 0x0100
    CONFIG_DISABLE_B = 512, // 0x0200
    CONFIG_AUTO_RADIO_A = 1024, // 0x0400
    CONFIG_AUTO_RADIO_B = 2048, // 0x0800
  }
}
