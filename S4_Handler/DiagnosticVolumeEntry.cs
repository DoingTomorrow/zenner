// Decompiled with JetBrains decompiler
// Type: S4_Handler.DiagnosticVolumeEntry
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;

#nullable disable
namespace S4_Handler
{
  public class DiagnosticVolumeEntry
  {
    internal double Last;
    internal double SecondLast;
    internal double ThirdLast;
    internal string VolumeUnitString;
    internal string FlowUnitString;

    internal DiagnosticVolumeEntry GetValuesForBaseUnit(S4_BaseUnits baseUnit)
    {
      if (this.VolumeUnitString != null)
        throw new Exception("Multiple unit translation not allowed.");
      DiagnosticVolumeEntry valuesForBaseUnit = new DiagnosticVolumeEntry();
      switch (baseUnit)
      {
        case S4_BaseUnits.Qm:
          valuesForBaseUnit.Last = this.Last;
          valuesForBaseUnit.SecondLast = this.SecondLast;
          valuesForBaseUnit.VolumeUnitString = " m\u00B3";
          valuesForBaseUnit.FlowUnitString = " m\u00B3/h";
          break;
        case S4_BaseUnits.US_GAL:
          valuesForBaseUnit.Last = S4_BaseUnitSupport.QmToUS_GAL(this.Last);
          valuesForBaseUnit.SecondLast = S4_BaseUnitSupport.QmToUS_GAL(this.SecondLast);
          valuesForBaseUnit.VolumeUnitString = " US_GAL";
          valuesForBaseUnit.FlowUnitString = " US_GAL/min";
          break;
        case S4_BaseUnits.Qft:
          valuesForBaseUnit.Last = S4_BaseUnitSupport.QmToQf(this.Last);
          valuesForBaseUnit.SecondLast = S4_BaseUnitSupport.QmToQf(this.SecondLast);
          valuesForBaseUnit.VolumeUnitString = " f\u00B3";
          valuesForBaseUnit.FlowUnitString = " US_GAL/min";
          break;
        default:
          throw new Exception("Illegal base unit");
      }
      return valuesForBaseUnit;
    }
  }
}
