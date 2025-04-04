// Decompiled with JetBrains decompiler
// Type: TH_Handler.RadioPower
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

#nullable disable
namespace TH_Handler
{
  public enum RadioPower : byte
  {
    Minus30dB = 0,
    Minus20dB = 1,
    Minus10dB = 2,
    Minus5dB = 3,
    Plus0dB = 4,
    Plus5dB = 5,
    Plus7dB = 6,
    Plus10dB = 7,
    None = 255, // 0xFF
  }
}
