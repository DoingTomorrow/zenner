// Decompiled with JetBrains decompiler
// Type: S4_Handler.DiagnosticTemperatureEntry
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;

#nullable disable
namespace S4_Handler
{
  internal class DiagnosticTemperatureEntry
  {
    internal float Last;
    internal float SecondLast;
    internal float ThirdLast;
    internal string UnitString;
    internal bool threeValueLogger = false;

    internal DiagnosticTemperatureEntry GetValuesForBaseUnit(S4_BaseUnits baseUnit)
    {
      if (this.UnitString != null)
        throw new Exception("Multiple unit translation not allowed.");
      DiagnosticTemperatureEntry valuesForBaseUnit = new DiagnosticTemperatureEntry();
      valuesForBaseUnit.Last = this.Last;
      valuesForBaseUnit.SecondLast = this.SecondLast;
      if (this.threeValueLogger)
        valuesForBaseUnit.ThirdLast = this.ThirdLast;
      valuesForBaseUnit.UnitString = " °C";
      return valuesForBaseUnit;
    }
  }
}
