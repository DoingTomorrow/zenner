// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.SumIf
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.Utilities;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class SumIf : HiddenValuesHandlingFunction
  {
    private readonly NumericExpressionEvaluator _evaluator;

    public SumIf()
      : this(new NumericExpressionEvaluator())
    {
    }

    public SumIf(NumericExpressionEvaluator evaluator)
    {
      OfficeOpenXml.FormulaParsing.Utilities.Require.That<NumericExpressionEvaluator>(evaluator).Named(nameof (evaluator)).IsNotNull<NumericExpressionEvaluator>();
      this._evaluator = evaluator;
    }

    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      ExcelDataProvider.IRangeInfo range = arguments.ElementAt<FunctionArgument>(0).Value as ExcelDataProvider.IRangeInfo;
      object criteria = arguments.ElementAt<FunctionArgument>(1).Value;
      this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() => criteria == null || criteria.ToString().Length > (int) byte.MaxValue), eErrorType.Value);
      double result;
      if (arguments.Count<FunctionArgument>() > 2)
      {
        ExcelDataProvider.IRangeInfo sumRange = arguments.ElementAt<FunctionArgument>(2).Value as ExcelDataProvider.IRangeInfo;
        result = this.CalculateWithSumRange(range, criteria.ToString(), sumRange, context);
      }
      else
        result = range == null ? this.CalculateSingleRange(arguments.ElementAt<FunctionArgument>(0).Value as IEnumerable<FunctionArgument>, criteria.ToString(), context) : this.CalculateSingleRange(range, criteria.ToString(), context);
      return this.CreateResult((object) result, DataType.Decimal);
    }

    private double CalculateWithSumRange(
      IEnumerable<FunctionArgument> range,
      string criteria,
      IEnumerable<FunctionArgument> sumRange,
      ParsingContext context)
    {
      double withSumRange = 0.0;
      IEnumerable<double> doubleEnumerable1 = this.ArgsToDoubleEnumerable(range, context);
      IEnumerable<double> doubleEnumerable2 = this.ArgsToDoubleEnumerable(sumRange, context);
      for (int index = 0; index < doubleEnumerable1.Count<double>(); ++index)
      {
        double num = doubleEnumerable2.ElementAt<double>(index);
        if (this._evaluator.Evaluate((object) doubleEnumerable1.ElementAt<double>(index), criteria))
          withSumRange += num;
      }
      return withSumRange;
    }

    private double CalculateWithSumRange(
      ExcelDataProvider.IRangeInfo range,
      string criteria,
      ExcelDataProvider.IRangeInfo sumRange,
      ParsingContext context)
    {
      double withSumRange = 0.0;
      foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) range)
      {
        if (this._evaluator.Evaluate(cellInfo.Value, criteria))
        {
          int rowOffset = cellInfo.Row - range.Address._fromRow;
          int colOffset = cellInfo.Column - range.Address._fromCol;
          if (sumRange.Address._fromRow + rowOffset <= sumRange.Address._toRow && sumRange.Address._fromCol + colOffset <= sumRange.Address._toCol)
          {
            object offset = sumRange.GetOffset(rowOffset, colOffset);
            if (offset is ExcelErrorValue)
              throw new ExcelErrorValueException((ExcelErrorValue) offset);
            withSumRange += ConvertUtil.GetValueDouble(offset, true);
          }
        }
      }
      return withSumRange;
    }

    private double CalculateSingleRange(
      IEnumerable<FunctionArgument> args,
      string expression,
      ParsingContext context)
    {
      double singleRange = 0.0;
      foreach (double left in this.ArgsToDoubleEnumerable(args, context))
      {
        if (this._evaluator.Evaluate((object) left, expression))
          singleRange += left;
      }
      return singleRange;
    }

    private double CalculateSingleRange(
      ExcelDataProvider.IRangeInfo range,
      string expression,
      ParsingContext context)
    {
      double singleRange = 0.0;
      foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) range)
      {
        if (this._evaluator.Evaluate(cellInfo.Value, expression))
        {
          if (cellInfo.IsExcelError)
            throw new ExcelErrorValueException((ExcelErrorValue) cellInfo.Value);
          singleRange += cellInfo.ValueDouble;
        }
      }
      return singleRange;
    }
  }
}
