// Decompiled with JetBrains decompiler
// Type: HandlerLib.LoRaPackets
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum LoRaPackets
  {
    SP1 = 1,
    SP2 = 2,
    SP3 = 3,
    SP4 = 4,
    START_JOINING = 5,
    TEMPERATURE = 6,
    AP1 = 7,
    SP9 = 8,
    CMD = 9,
    IK = 10, // 0x0000000A
    SP0 = 11, // 0x0000000B
    SP5 = 12, // 0x0000000C
    SP6 = 13, // 0x0000000D
    SP7 = 14, // 0x0000000E
    SP8 = 15, // 0x0000000F
    ASYNC_TELEGRAM = 16, // 0x00000010
    SP12 = 17, // 0x00000011
    STRESS = 18, // 0x00000012
    FE = 254, // 0x000000FE
  }
}
