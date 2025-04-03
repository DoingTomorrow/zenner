// Decompiled with JetBrains decompiler
// Type: PDC_Handler.RadioFlagsPDCwMBus
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;

#nullable disable
namespace PDC_Handler
{
  [Flags]
  public enum RadioFlagsPDCwMBus : byte
  {
    CONFIG_RADIO_LONGHEADER = 1,
    CONFIG_RADIO_ENCRYPT = 2,
    CONFIG_RADIO_SYNCHRONOUS = 4,
    CONFIG_RADIO_INSTALL = 8,
    CONFIG_RADIO_CHANNEL_A = 16, // 0x10
    CONFIG_RADIO_CHANNEL_B = 32, // 0x20
    CONFIG_RADIO_ENCRYPT_7 = 64, // 0x40
  }
}
