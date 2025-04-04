// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.AverageA
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class AverageA : HiddenValuesHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1, eErrorType.Div0);
      double nValues = 0.0;
      double retVal = 0.0;
      foreach (FunctionArgument functionArgument in arguments)
        this.Calculate(functionArgument, context, ref retVal, ref nValues);
      return this.CreateResult((object) (retVal / nValues), DataType.Decimal);
    }

    private void Calculate(
      FunctionArgument arg,
      ParsingContext context,
      ref double retVal,
      ref double nValues,
      bool isInArray = false)
    {
      if (this.ShouldIgnore(arg))
        return;
      if (arg.Value is IEnumerable<FunctionArgument>)
      {
        foreach (FunctionArgument functionArgument in (IEnumerable<FunctionArgument>) arg.Value)
          this.Calculate(functionArgument, context, ref retVal, ref nValues, true);
      }
      else if (arg.IsExcelRange)
      {
        foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) arg.ValueAsRangeInfo)
        {
          if (!this.ShouldIgnore(cellInfo, context))
          {
            this.CheckForAndHandleExcelError(cellInfo);
            if (this.IsNumeric(cellInfo.Value))
            {
              ++nValues;
              retVal += cellInfo.ValueDouble;
            }
            else if (cellInfo.Value is bool)
            {
              ++nValues;
              retVal += (bool) cellInfo.Value ? 1.0 : 0.0;
            }
            else if (cellInfo.Value is string)
              ++nValues;
          }
        }
      }
      else
      {
        double? numericValue = this.GetNumericValue(arg.Value, isInArray);
        if (numericValue.HasValue)
        {
          ++nValues;
          retVal += numericValue.Value;
        }
        else if (arg.Value is string && !ConvertUtil.IsNumericString(arg.Value))
        {
          if (isInArray)
            ++nValues;
          else
            this.ThrowExcelErrorValueException(eErrorType.Value);
        }
      }
      this.CheckForAndHandleExcelError(arg);
    }

    private double? GetNumericValue(object obj, bool isInArray)
    {
      if (this.IsNumeric(obj))
        return new double?(ConvertUtil.GetValueDouble(obj));
      return obj is bool ? (isInArray ? new double?() : new double?(ConvertUtil.GetValueDouble(obj))) : (ConvertUtil.IsNumericString(obj) ? new double?(double.Parse(obj.ToString(), (IFormatProvider) CultureInfo.InvariantCulture)) : new double?());
    }
  }
}
