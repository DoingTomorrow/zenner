// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.RadioMode
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

#nullable disable
namespace SmokeDetectorHandler
{
  public enum RadioMode : byte
  {
    NormalOperatingMode_RX_TX = 0,
    ContinuousWaveLowerCarrier_TX_868_3_MHz = 1,
    ContinuousWaveUpperCarrier_TX_868_3_MHz = 2,
    PN9_TestPatternTransmitting_TX_868_3_MHz = 3,
    TestTelegramTransmitting_TX_868_3_MHz = 4,
    ContinuousWaveLowerCarrier_TX_869_85_MHz = 5,
    ContinuousWaveUpperCarrier_TX_869_85_MHz = 6,
    PN9_TestPatternTransmitting_TX_869_85_MHz = 7,
    TestTelegramTransmitting_TX_869_85_MHz = 8,
    Continuous_Receiving_RX_869_85_MHz = 16, // 0x10
  }
}
