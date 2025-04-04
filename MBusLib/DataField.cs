// Decompiled with JetBrains decompiler
// Type: MBusLib.DataField
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

#nullable disable
namespace MBusLib
{
  public enum DataField : byte
  {
    NoData,
    Integer8bit,
    Integer16bit,
    Integer24bit,
    Integer32bit,
    Real32bit,
    Integer48bit,
    Integer64bit,
    SelectionForReadout,
    Bcd2Digit,
    Bcd4Digit,
    Bcd6Digit,
    Bcd8Digit,
    VariableLength,
    Bcd12Digit,
    SpecialFunctions,
  }
}
