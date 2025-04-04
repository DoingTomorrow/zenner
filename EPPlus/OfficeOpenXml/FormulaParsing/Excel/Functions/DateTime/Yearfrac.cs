// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Yearfrac
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Yearfrac : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      if (!(arguments is FunctionArgument[] functionArgumentArray1))
        functionArgumentArray1 = arguments.ToArray<FunctionArgument>();
      FunctionArgument[] functionArgumentArray2 = functionArgumentArray1;
      this.ValidateArguments((IEnumerable<FunctionArgument>) functionArgumentArray2, 2);
      double d1 = this.ArgToDecimal((IEnumerable<FunctionArgument>) functionArgumentArray2, 0);
      double d2 = this.ArgToDecimal((IEnumerable<FunctionArgument>) functionArgumentArray2, 1);
      if (d1 > d2)
      {
        double num = d1;
        d1 = d2;
        d2 = num;
        FunctionArgument functionArgument = functionArgumentArray2[1];
        functionArgumentArray2[1] = functionArgumentArray2[0];
        functionArgumentArray2[0] = functionArgument;
      }
      System.DateTime dt1 = System.DateTime.FromOADate(d1);
      System.DateTime dt2 = System.DateTime.FromOADate(d2);
      int basis = 0;
      if (((IEnumerable<FunctionArgument>) functionArgumentArray2).Count<FunctionArgument>() > 2)
      {
        basis = this.ArgToInt((IEnumerable<FunctionArgument>) functionArgumentArray2, 2);
        this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() => basis < 0 || basis > 4), eErrorType.Num);
      }
      ExcelFunction function = context.Configuration.FunctionRepository.GetFunction("days360");
      GregorianCalendar gregorianCalendar = new GregorianCalendar();
      switch (basis)
      {
        case 0:
          double num1 = Math.Abs(function.Execute((IEnumerable<FunctionArgument>) functionArgumentArray2, context).ResultNumeric);
          if (dt1.Month == 2 && dt2.Day == 31)
          {
            int num2 = gregorianCalendar.IsLeapYear(dt1.Year) ? 29 : 28;
            if (dt1.Day == num2)
              ++num1;
          }
          return this.CreateResult((object) (num1 / 360.0), DataType.Decimal);
        case 1:
          return this.CreateResult((object) Math.Abs((dt2 - dt1).TotalDays / this.CalculateAcutalYear(dt1, dt2)), DataType.Decimal);
        case 2:
          return this.CreateResult((object) Math.Abs((dt2 - dt1).TotalDays / 360.0), DataType.Decimal);
        case 3:
          return this.CreateResult((object) Math.Abs((dt2 - dt1).TotalDays / 365.0), DataType.Decimal);
        case 4:
          List<FunctionArgument> list = ((IEnumerable<FunctionArgument>) functionArgumentArray2).ToList<FunctionArgument>();
          list.Add(new FunctionArgument((object) true));
          return this.CreateResult((object) new double?(Math.Abs(function.Execute((IEnumerable<FunctionArgument>) list, context).ResultNumeric / 360.0)).Value, DataType.Decimal);
        default:
          return (CompileResult) null;
      }
    }

    private double CalculateAcutalYear(System.DateTime dt1, System.DateTime dt2)
    {
      GregorianCalendar gregorianCalendar = new GregorianCalendar();
      double num1 = 0.0;
      int num2 = dt2.Year - dt1.Year + 1;
      for (int year = dt1.Year; year <= dt2.Year; ++year)
        num1 += gregorianCalendar.IsLeapYear(year) ? 366.0 : 365.0;
      if (new System.DateTime(dt1.Year + 1, dt1.Month, dt1.Day) >= dt2)
      {
        num2 = 1;
        num1 = 365.0;
        if (gregorianCalendar.IsLeapYear(dt1.Year) && dt1.Month <= 2)
          num1 = 366.0;
        else if (gregorianCalendar.IsLeapYear(dt2.Year) && dt2.Month > 2)
          num1 = 366.0;
        else if (dt2.Month == 2 && dt2.Day == 29)
          num1 = 366.0;
      }
      return num1 / (double) num2;
    }
  }
}
