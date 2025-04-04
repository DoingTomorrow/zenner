// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Workday
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Workday : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      System.DateTime dateTime1 = System.DateTime.FromOADate((double) this.ArgToInt(arguments, 0));
      int num1 = this.ArgToInt(arguments, 1);
      System.DateTime minValue = System.DateTime.MinValue;
      int num2 = 0;
      System.DateTime date;
      for (date = dateTime1; date.DayOfWeek != DayOfWeek.Monday && num1 - num2 > 0; date = date.AddDays(1.0))
      {
        if (!this.IsHoliday(date))
          ++num2;
      }
      int num3 = (num1 - num2) / 5;
      System.DateTime dateTime2 = date.AddDays((double) (num3 * 7));
      int num4 = num2 + num3 * 5;
      while (num4 < num1)
      {
        dateTime2 = dateTime2.AddDays(1.0);
        if (!this.IsHoliday(dateTime2))
          ++num4;
      }
      return this.CreateResult((object) this.AdjustResultWithHolidays(dateTime2, arguments).ToOADate(), DataType.Date);
    }

    private System.DateTime AdjustResultWithHolidays(
      System.DateTime resultDate,
      IEnumerable<FunctionArgument> arguments)
    {
      if (arguments.Count<FunctionArgument>() == 2)
        return resultDate;
      if (arguments.ElementAt<FunctionArgument>(2).Value is IEnumerable<FunctionArgument> functionArguments)
      {
        foreach (FunctionArgument functionArgument in functionArguments)
        {
          if (ConvertUtil.IsNumeric(functionArgument.Value) && !this.IsHoliday(System.DateTime.FromOADate(ConvertUtil.GetValueDouble(functionArgument.Value))))
            resultDate = resultDate.AddDays(1.0);
        }
      }
      else if (arguments.ElementAt<FunctionArgument>(2).Value is ExcelDataProvider.IRangeInfo rangeInfo)
      {
        foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) rangeInfo)
        {
          if (ConvertUtil.IsNumeric(cellInfo.Value) && !this.IsHoliday(System.DateTime.FromOADate(ConvertUtil.GetValueDouble(cellInfo.Value))))
            resultDate = resultDate.AddDays(1.0);
        }
      }
      return resultDate;
    }

    private bool IsHoliday(System.DateTime date)
    {
      return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
  }
}
