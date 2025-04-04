// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Count
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Count : HiddenValuesHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      double nItems = 0.0;
      this.Calculate(arguments, ref nItems, context, Count.ItemContext.SingleArg);
      return this.CreateResult((object) nItems, DataType.Integer);
    }

    private void Calculate(
      IEnumerable<FunctionArgument> items,
      ref double nItems,
      ParsingContext context,
      Count.ItemContext itemContext)
    {
      foreach (FunctionArgument functionArgument in items)
      {
        if (functionArgument.Value is ExcelDataProvider.IRangeInfo rangeInfo)
        {
          foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) rangeInfo)
          {
            this._CheckForAndHandleExcelError(cellInfo, context);
            if (!this.ShouldIgnore(cellInfo, context) && this.ShouldCount(cellInfo.Value, Count.ItemContext.InRange))
              ++nItems;
          }
        }
        else if (functionArgument.Value is IEnumerable<FunctionArgument> items1)
        {
          this.Calculate(items1, ref nItems, context, Count.ItemContext.InArray);
        }
        else
        {
          this._CheckForAndHandleExcelError(functionArgument, context);
          if (!this.ShouldIgnore(functionArgument) && this.ShouldCount(functionArgument.Value, itemContext))
            ++nItems;
        }
      }
    }

    private void _CheckForAndHandleExcelError(FunctionArgument arg, ParsingContext context)
    {
    }

    private void _CheckForAndHandleExcelError(
      ExcelDataProvider.ICellInfo cell,
      ParsingContext context)
    {
    }

    private bool ShouldCount(object value, Count.ItemContext context)
    {
      switch (context)
      {
        case Count.ItemContext.InRange:
          return this.IsNumeric(value);
        case Count.ItemContext.InArray:
          return this.IsNumeric(value) || this.IsNumericString(value);
        case Count.ItemContext.SingleArg:
          return this.IsNumeric(value) || this.IsNumericString(value);
        default:
          throw new ArgumentException("Unknown ItemContext:" + context.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    private enum ItemContext
    {
      InRange,
      InArray,
      SingleArg,
    }
  }
}
