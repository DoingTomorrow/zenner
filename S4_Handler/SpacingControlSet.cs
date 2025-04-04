// Decompiled with JetBrains decompiler
// Type: S4_Handler.SpacingControlSet
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  internal enum SpacingControlSet
  {
    Absolute = 0,
    Seconds = 0,
    Minutes = 16, // 0x00000010
    SixMonth = 16, // 0x00000010
    Hours = 32, // 0x00000020
    ThreeMonth = 32, // 0x00000020
    Days = 48, // 0x00000030
    HalfMonth = 48, // 0x00000030
    OneMonth = 48, // 0x00000030
    Increment = 64, // 0x00000040
    Decrement = 128, // 0x00000080
    Diff = 192, // 0x000000C0
  }
}
