// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_BaseUnitSupport
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;

#nullable disable
namespace S4_Handler
{
  public class S4_BaseUnitSupport
  {
    public const double Qm_to_US_GAL_Factor = 264.17205235814839;
    public const double Qm_to_Qf_Factor = 35.314666721488592;

    public S4_BaseUnits BaseUnit { get; private set; }

    public S4_BaseUnitSupport(S4_BaseUnits baseUnit)
    {
      this.BaseUnit = Enum.IsDefined(typeof (S4_BaseUnits), (object) baseUnit) ? baseUnit : throw new ArgumentOutOfRangeException("Illegal BaseUnit");
    }

    public string VolumeUnitString
    {
      get
      {
        switch (this.BaseUnit)
        {
          case S4_BaseUnits.Qm:
            return "m\u00B3";
          case S4_BaseUnits.US_GAL:
            return "US_GAL";
          default:
            return "feet\u00B3";
        }
      }
    }

    public string FlowUnitString
    {
      get
      {
        switch (this.BaseUnit)
        {
          case S4_BaseUnits.Qm:
            return "m\u00B3/h";
          case S4_BaseUnits.US_GAL:
            return "US_GAL/Min";
          default:
            return "US_GAL/Min";
        }
      }
    }

    public double GetDisplayVolume(double storedVolume)
    {
      switch (this.BaseUnit)
      {
        case S4_BaseUnits.Qm:
          return storedVolume;
        case S4_BaseUnits.US_GAL:
          return S4_BaseUnitSupport.QmToUS_GAL(storedVolume);
        default:
          return S4_BaseUnitSupport.QmToQf(storedVolume);
      }
    }

    public float GetDisplayFlow(float storedFlow)
    {
      switch (this.BaseUnit)
      {
        case S4_BaseUnits.Qm:
          return storedFlow;
        case S4_BaseUnits.US_GAL:
          return S4_BaseUnitSupport.QmphToUS_GALperMinute(storedFlow);
        default:
          return S4_BaseUnitSupport.QmphToUS_GALperMinute(storedFlow);
      }
    }

    public static float CelsiusToFahrenheit(float celsiusValue)
    {
      return (float) ((double) celsiusValue * 1.8 + 32.0);
    }

    public static float FahrenheitToCelsius(float fahrenheitValue)
    {
      return (float) (((double) fahrenheitValue - 32.0) / 1.8);
    }

    public static double QmToUS_GAL(double qmValue) => qmValue * 264.17205235814839;

    public static double US_GALToQm(double usGalValue) => usGalValue / 264.17205235814839;

    public static double QmToQf(double qmValue) => qmValue * 35.314666721488592;

    public static double QfToQm(double qfValue) => qfValue / 35.314666721488592;

    public static float QmphToUS_GALperMinute(float qmphValue)
    {
      return (float) ((double) qmphValue * 264.17205235814839 / 60.0);
    }

    public static float US_GALperMinuteToQmph(float US_GALperMinuteValue)
    {
      return (float) ((double) US_GALperMinuteValue * 60.0 / 264.17205235814839);
    }
  }
}
