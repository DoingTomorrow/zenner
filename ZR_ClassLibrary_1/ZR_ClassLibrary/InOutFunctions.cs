// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.InOutFunctions
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum InOutFunctions
  {
    IO_EnumError = 0,
    IO1_Off = 1,
    IO1_Input = 2,
    IO1_Energy = 3,
    IO1_Volume = 4,
    IO1_CEnergy = 5,
    IO1_Error = 6,
    IO1_Special = 7,
    IO1_Mask = 15, // 0x0000000F
    IO2_Off = 16, // 0x00000010
    IO2_Input = 32, // 0x00000020
    IO2_Energy = 48, // 0x00000030
    IO2_Volume = 64, // 0x00000040
    IO2_CEnergy = 80, // 0x00000050
    IO2_Error = 96, // 0x00000060
    IO2_Special = 112, // 0x00000070
    IO2_Mask = 240, // 0x000000F0
  }
}
