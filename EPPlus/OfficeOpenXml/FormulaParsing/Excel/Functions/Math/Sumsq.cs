// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Sumsq
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Sumsq : HiddenValuesHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      double result = 0.0;
      if (arguments != null)
      {
        foreach (FunctionArgument functionArgument in arguments)
          result += this.Calculate(functionArgument, context);
      }
      return this.CreateResult((object) result, DataType.Decimal);
    }

    private double Calculate(FunctionArgument arg, ParsingContext context, bool isInArray = false)
    {
      double num = 0.0;
      if (this.ShouldIgnore(arg))
        return num;
      if (arg.Value is IEnumerable<FunctionArgument>)
      {
        foreach (FunctionArgument functionArgument in (IEnumerable<FunctionArgument>) arg.Value)
          num += this.Calculate(functionArgument, context, true);
      }
      else if (arg.Value is ExcelDataProvider.IRangeInfo rangeInfo)
      {
        foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) rangeInfo)
        {
          if (!this.ShouldIgnore(cellInfo, context))
          {
            this.CheckForAndHandleExcelError(cellInfo);
            num += System.Math.Pow(cellInfo.ValueDouble, 2.0);
          }
        }
      }
      else
      {
        this.CheckForAndHandleExcelError(arg);
        if (this.IsNumericString(arg.Value) && !isInArray)
          return System.Math.Pow(ConvertUtil.GetValueDouble(arg.Value), 2.0);
        bool ignoreBool = isInArray;
        num += System.Math.Pow(ConvertUtil.GetValueDouble(arg.Value, ignoreBool), 2.0);
      }
      return num;
    }
  }
}
