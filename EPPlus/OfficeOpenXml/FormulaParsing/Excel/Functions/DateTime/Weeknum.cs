// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Weeknum
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
  public class Weeknum : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1, eErrorType.Value);
      System.DateTime time = System.DateTime.FromOADate(this.ArgToDecimal(arguments, 0));
      DayOfWeek firstDayOfWeek = DayOfWeek.Sunday;
      if (arguments.Count<FunctionArgument>() > 1)
      {
        switch (this.ArgToInt(arguments, 1))
        {
          case 1:
            firstDayOfWeek = DayOfWeek.Sunday;
            break;
          case 2:
          case 11:
            firstDayOfWeek = DayOfWeek.Monday;
            break;
          case 12:
            firstDayOfWeek = DayOfWeek.Tuesday;
            break;
          case 13:
            firstDayOfWeek = DayOfWeek.Wednesday;
            break;
          case 14:
            firstDayOfWeek = DayOfWeek.Thursday;
            break;
          case 15:
            firstDayOfWeek = DayOfWeek.Friday;
            break;
          case 16:
            firstDayOfWeek = DayOfWeek.Saturday;
            break;
          default:
            this.ThrowExcelErrorValueException(eErrorType.Num);
            break;
        }
      }
      if (DateTimeFormatInfo.CurrentInfo == null)
        throw new InvalidOperationException("Could not execute Weeknum function because DateTimeFormatInfo.CurrentInfo was null");
      return this.CreateResult((object) DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, firstDayOfWeek), DataType.Integer);
    }
  }
}
