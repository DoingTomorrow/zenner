// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.TransmissionScenario
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

#nullable disable
namespace EDCL_Handler
{
  public enum TransmissionScenario : byte
  {
    Scenario1 = 1,
    Scenario2 = 2,
    Scenario3 = 3,
    WMBusFormatA = 10, // 0x0A
    WMBusFormatB = 11, // 0x0B
    WMBusFormatC = 12, // 0x0C
    NBIoTScenario1 = 21, // 0x15
    NBIoTScenario2 = 22, // 0x16
    NBIoTScenario3 = 23, // 0x17
    NBIoTScenario4 = 24, // 0x18
    NBIoTScenario5 = 25, // 0x19
    NBIoTScenario6 = 26, // 0x1A
  }
}
