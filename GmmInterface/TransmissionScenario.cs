// Decompiled with JetBrains decompiler
// Type: ZENNER.TransmissionScenario
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using System;

#nullable disable
namespace ZENNER
{
  [Serializable]
  public enum TransmissionScenario : byte
  {
    Scenario1_Monthly = 1,
    Scenario2_Daily = 2,
    Scenario3_Hourly = 3,
    WMBusFormatA = 10, // 0x0A
    WMBusFormatB = 11, // 0x0B
  }
}
