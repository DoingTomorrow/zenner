// Decompiled with JetBrains decompiler
// Type: EDC_Handler.RuntimeFlags
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  [Flags]
  public enum RuntimeFlags : ushort
  {
    MBUS_RECEIVED = 1,
    MBUS_SEND = 2,
    MBUS_POWER = 8,
    MBUS_POWERCHECK = 16, // 0x0010
    IRDA_RECEIVED = 32, // 0x0020
    IRDA_SEND = 64, // 0x0040
    POST_POLL = 256, // 0x0100
    PULSE_TASK = 512, // 0x0200
    PULSE_FLOWCHECK = 1024, // 0x0400
    WARNING = 2048, // 0x0800
    MBUS_KEEPALIVE = 16384, // 0x4000
    IRDA_KEEPALIVE = 32768, // 0x8000
  }
}
