// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ModuleTypeValues
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum ModuleTypeValues
  {
    NoValue = 0,
    Inp1 = 1,
    Out1 = 2,
    IO1Mask = 3,
    InOut1 = 3,
    Inp2 = 4,
    Out2 = 8,
    IO2Mask = 12, // 0x0000000C
    InOut2 = 12, // 0x0000000C
    MBus = 16, // 0x00000010
    ZRBus = 32, // 0x00000020
    BusMask = 48, // 0x00000030
    RS232 = 48, // 0x00000030
  }
}
