// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Days360
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Days360 : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      double d1 = this.ArgToDecimal(arguments, 0);
      double d2 = this.ArgToDecimal(arguments, 1);
      System.DateTime dateTime1 = System.DateTime.FromOADate(d1);
      System.DateTime dateTime2 = System.DateTime.FromOADate(d2);
      Days360.Days360Calctype days360Calctype = Days360.Days360Calctype.Us;
      if (arguments.Count<FunctionArgument>() > 2 && this.ArgToBool(arguments, 2))
        days360Calctype = Days360.Days360Calctype.European;
      int year1 = dateTime1.Year;
      int month1 = dateTime1.Month;
      int num1 = dateTime1.Day;
      int year2 = dateTime2.Year;
      int month2 = dateTime2.Month;
      int num2 = dateTime2.Day;
      if (days360Calctype == Days360.Days360Calctype.European)
      {
        if (num1 == 31)
          num1 = 30;
        if (num2 == 31)
          num2 = 30;
      }
      else
      {
        int num3 = new GregorianCalendar().IsLeapYear(dateTime1.Year) ? 29 : 28;
        if (month1 == 2 && num1 == num3 && month2 == 2 && num2 == num3)
          num2 = 30;
        if (month1 == 2 && num1 == num3)
          num1 = 30;
        if (num2 == 31 && (num1 == 30 || num1 == 31))
          num2 = 30;
        if (num1 == 31)
          num1 = 30;
      }
      return this.CreateResult((object) (year2 * 12 * 30 + month2 * 30 + num2 - (year1 * 12 * 30 + month1 * 30 + num1)), DataType.Integer);
    }

    private int GetNumWholeMonths(System.DateTime dt1, System.DateTime dt2)
    {
      System.DateTime dateTime1 = new System.DateTime(dt1.Year, dt1.Month, 1).AddMonths(1);
      System.DateTime dateTime2 = new System.DateTime(dt2.Year, dt2.Month, 1);
      return (dateTime2.Year - dateTime1.Year) * 12 + (dateTime2.Month - dateTime1.Month);
    }

    private enum Days360Calctype
    {
      European,
      Us,
    }
  }
}
