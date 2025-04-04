// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Sum
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Sum : HiddenValuesHandlingFunction
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

    private double Calculate(FunctionArgument arg, ParsingContext context)
    {
      double num = 0.0;
      if (this.ShouldIgnore(arg))
        return num;
      if (arg.Value is IEnumerable<FunctionArgument>)
      {
        foreach (FunctionArgument functionArgument in (IEnumerable<FunctionArgument>) arg.Value)
          num += this.Calculate(functionArgument, context);
      }
      else if (arg.Value is ExcelDataProvider.IRangeInfo)
      {
        foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) arg.Value)
        {
          if (!this.ShouldIgnore(cellInfo, context))
          {
            this.CheckForAndHandleExcelError(cellInfo);
            num += cellInfo.ValueDouble;
          }
        }
      }
      else
      {
        this.CheckForAndHandleExcelError(arg);
        num += ConvertUtil.GetValueDouble(arg.Value, true);
      }
      return num;
    }
  }
}
