// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.SumProduct
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class SumProduct : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      double result = 0.0;
      List<List<double>> source = new List<List<double>>();
      foreach (FunctionArgument functionArgument1 in arguments)
      {
        source.Add(new List<double>());
        List<double> currentResult = source.Last<List<double>>();
        if (functionArgument1.Value is IEnumerable<FunctionArgument>)
        {
          foreach (FunctionArgument functionArgument2 in (IEnumerable<FunctionArgument>) functionArgument1.Value)
            this.AddValue(functionArgument2.Value, currentResult);
        }
        else if (functionArgument1.Value is FunctionArgument)
          this.AddValue(functionArgument1.Value, currentResult);
        else if (functionArgument1.IsExcelRange)
        {
          ExcelDataProvider.IRangeInfo valueAsRangeInfo = functionArgument1.ValueAsRangeInfo;
          for (int fromCol = valueAsRangeInfo.Address._fromCol; fromCol <= valueAsRangeInfo.Address._toCol; ++fromCol)
          {
            for (int fromRow = valueAsRangeInfo.Address._fromRow; fromRow <= valueAsRangeInfo.Address._toRow; ++fromRow)
              this.AddValue(valueAsRangeInfo.GetValue(fromRow, fromCol), currentResult);
          }
        }
      }
      int count = source.First<List<double>>().Count;
      foreach (List<double> doubleList in source)
      {
        if (doubleList.Count != count)
          throw new ExcelErrorValueException(ExcelErrorValue.Create(eErrorType.Value));
      }
      for (int index1 = 0; index1 < count; ++index1)
      {
        double num = 1.0;
        for (int index2 = 0; index2 < source.Count; ++index2)
          num *= source[index2][index1];
        result += num;
      }
      return this.CreateResult((object) result, DataType.Decimal);
    }

    private void AddValue(object convertVal, List<double> currentResult)
    {
      if (this.IsNumeric(convertVal))
      {
        currentResult.Add(Convert.ToDouble(convertVal));
      }
      else
      {
        if (convertVal is ExcelErrorValue)
          throw new ExcelErrorValueException((ExcelErrorValue) convertVal);
        currentResult.Add(0.0);
      }
    }
  }
}
