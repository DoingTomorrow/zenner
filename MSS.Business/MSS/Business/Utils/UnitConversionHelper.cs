// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.UnitConversionHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

#nullable disable
namespace MSS.Business.Utils
{
  public static class UnitConversionHelper
  {
    public static double? ConvertValueToImpulses(
      double? startValue,
      string readingUnit,
      double? impValue,
      string impUnit)
    {
      impUnit = impUnit.ToUpper();
      readingUnit = readingUnit.ToUpper();
      if (impUnit == "L" && (readingUnit == "M3" || readingUnit == "M\u00B3"))
      {
        double? nullable1 = startValue;
        double num = 1000.0;
        double? nullable2 = nullable1.HasValue ? new double?(nullable1.GetValueOrDefault() * num) : new double?();
        double? nullable3 = impValue;
        double? impulses;
        if (!(nullable2.HasValue & nullable3.HasValue))
        {
          nullable1 = new double?();
          impulses = nullable1;
        }
        else
          impulses = new double?(nullable2.GetValueOrDefault() / nullable3.GetValueOrDefault());
        return impulses;
      }
      if (impUnit == "KWH" && readingUnit == "KWH")
      {
        double? nullable4 = startValue;
        double? nullable5 = impValue;
        return nullable4.HasValue & nullable5.HasValue ? new double?(nullable4.GetValueOrDefault() / nullable5.GetValueOrDefault()) : new double?();
      }
      if (impUnit == "KWH" && readingUnit == "MWH")
      {
        double? nullable6 = startValue;
        double num = 1000.0;
        double? nullable7 = nullable6.HasValue ? new double?(nullable6.GetValueOrDefault() * num) : new double?();
        double? nullable8 = impValue;
        double? impulses;
        if (!(nullable7.HasValue & nullable8.HasValue))
        {
          nullable6 = new double?();
          impulses = nullable6;
        }
        else
          impulses = new double?(nullable7.GetValueOrDefault() / nullable8.GetValueOrDefault());
        return impulses;
      }
      if (impUnit == "MWH" && readingUnit == "KWH")
      {
        double? nullable9 = startValue;
        double? nullable10 = impValue;
        double? nullable11 = nullable9.HasValue & nullable10.HasValue ? new double?(nullable9.GetValueOrDefault() / nullable10.GetValueOrDefault()) : new double?();
        double num = 1000.0;
        return nullable11.HasValue ? new double?(nullable11.GetValueOrDefault() / num) : new double?();
      }
      double? nullable12 = startValue;
      double? nullable13 = impValue;
      return nullable12.HasValue & nullable13.HasValue ? new double?(nullable12.GetValueOrDefault() / nullable13.GetValueOrDefault()) : new double?();
    }
  }
}
