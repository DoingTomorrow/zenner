// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.AbstractStatus
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using System;

#nullable disable
namespace NFCL_Handler
{
  [Flags]
  internal enum AbstractStatus : ushort
  {
    OK = 0,
    LOW_BAT_TIME = 1,
    LOW_BAT_VOLTAGE = 2,
    LOW_BAT_ERROR = 4,
    ACCURACY = 8,
    HARDWARE_ERROR = 16, // 0x0010
    EMPTY_TUBE = 32, // 0x0020
    FLOW_OUT_OF_RANGE = 64, // 0x0040
    DEVICE_IN_SLEEP = 128, // 0x0080
    NOT_PROTECTED = 256, // 0x0100
  }
}
