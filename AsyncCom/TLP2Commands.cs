// Decompiled with JetBrains decompiler
// Type: AsyncCom.TLP2Commands
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

#nullable disable
namespace AsyncCom
{
  public enum TLP2Commands
  {
    lp2Alive = 0,
    lp2Ver = 1,
    lp2SetComParams = 2,
    lp2GSetComParams = 3,
    lp2SetWakeupParams = 4,
    lp2GetWakeupParams = 5,
    lp2WakeUp = 6,
    lp2ReqUD2 = 7,
    lp2SendData = 8,
    lp2ResetTemic = 9,
    lp2ReqUD2_Flash = 10, // 0x0000000A
    lp2SendData_Flash = 11, // 0x0000000B
    lp2ReqUD2_x = 12, // 0x0000000C
    lp2SendChipconParams1 = 101, // 0x00000065
    lp2SendChipconParams2 = 102, // 0x00000066
    lp2SetRF2Mode = 105, // 0x00000069
    lp2SetRF3Mode = 106, // 0x0000006A
    lp2Transparent = 150, // 0x00000096
    lp2LPM1 = 151, // 0x00000097
    lp2LPM2 = 152, // 0x00000098
    lp2LPM3 = 153, // 0x00000099
    lp2LPM4 = 154, // 0x0000009A
    lp2SetPWRTicker = 155, // 0x0000009B
    lp2GetPWRTicker = 156, // 0x0000009C
    lp2SetBooster = 157, // 0x0000009D
    lp2SleepOFF = 158, // 0x0000009E
    lp2PortBit = 159, // 0x0000009F
    lp2GetImpulse = 160, // 0x000000A0
    lp2ClearImpulse = 161, // 0x000000A1
    lp2EchoIrDa = 162, // 0x000000A2
    lp2RFModul = 163, // 0x000000A3
    lp2TransparentSD = 164, // 0x000000A4
    lpUnknownCommand = 255, // 0x000000FF
  }
}
