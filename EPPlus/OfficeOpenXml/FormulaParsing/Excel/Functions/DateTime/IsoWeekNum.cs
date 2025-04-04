// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.IsoWeekNum
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class IsoWeekNum : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      return this.CreateResult((object) this.WeekNumber(System.DateTime.FromOADate((double) this.ArgToInt(arguments, 0))), DataType.Integer);
    }

    private int WeekNumber(System.DateTime fromDate)
    {
      System.DateTime dateTime1 = fromDate.AddDays((double) (-fromDate.Day + 1)).AddMonths(-fromDate.Month + 1);
      System.DateTime dateTime2 = dateTime1.AddYears(1).AddDays(-1.0);
      int[] numArray = new int[7]{ 6, 7, 8, 9, 10, 4, 5 };
      int num = (fromDate.Subtract(dateTime1).Days + numArray[(int) dateTime1.DayOfWeek]) / 7;
      switch (num)
      {
        case 0:
          return this.WeekNumber(dateTime1.AddDays(-1.0));
        case 53:
          return dateTime2.DayOfWeek < DayOfWeek.Thursday ? 1 : num;
        default:
          return num;
      }
    }
  }
}
