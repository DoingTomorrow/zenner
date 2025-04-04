// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ResolutionData
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ResolutionData
  {
    public string resolutionString;
    public string baseUnitString;
    public string displayUnitString;
    public Decimal baseUnitFactor;
    public Decimal displayFactor;
    public int mbusVIF;
    public string PulsValueUnitString;
    public Decimal MeterPulsValue_To_DisplayPulsValue_Factor;
    public Decimal DisplayPulsValue_To_MeterPulsValue_Factor;

    public ResolutionData(
      string resolutionString,
      string baseUnitString,
      Decimal baseUnitFactor,
      Decimal displayFactor,
      int mbusVIF)
    {
      this.resolutionString = resolutionString;
      this.baseUnitString = baseUnitString;
      this.displayUnitString = "";
      this.baseUnitFactor = baseUnitFactor;
      this.displayFactor = displayFactor;
      this.mbusVIF = mbusVIF;
      for (int index = 0; index < resolutionString.Length; ++index)
      {
        if (resolutionString[index] != '0' && resolutionString[index] != '.')
        {
          this.displayUnitString = resolutionString.Substring(index);
          break;
        }
      }
      this.DisplayPulsValue_To_MeterPulsValue_Factor = displayFactor;
      this.PulsValueUnitString = this.displayUnitString.Length != 0 ? this.displayUnitString + "/Imp" : this.displayUnitString + "1/Imp";
      if (this.displayUnitString == "m\u00B3")
      {
        this.PulsValueUnitString = "l/Imp";
        this.DisplayPulsValue_To_MeterPulsValue_Factor = displayFactor / 1000M;
      }
      else if (this.displayUnitString == "MWh")
      {
        this.PulsValueUnitString = "kWh/Imp";
        this.DisplayPulsValue_To_MeterPulsValue_Factor = displayFactor / 1000M;
      }
      else if (this.displayUnitString == "GJ")
      {
        this.PulsValueUnitString = "MJ/Imp";
        this.DisplayPulsValue_To_MeterPulsValue_Factor = displayFactor / 1000M;
      }
      this.MeterPulsValue_To_DisplayPulsValue_Factor = 1M / this.DisplayPulsValue_To_MeterPulsValue_Factor;
    }
  }
}
