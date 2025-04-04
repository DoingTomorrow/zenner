// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Information.IsError
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Information
{
  public class IsError : ErrorHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      if (arguments == null || arguments.Count<FunctionArgument>() == 0)
        return this.CreateResult((object) false, DataType.Boolean);
      foreach (FunctionArgument functionArgument in arguments)
      {
        if (functionArgument.Value is ExcelDataProvider.IRangeInfo)
        {
          ExcelDataProvider.IRangeInfo rangeInfo = (ExcelDataProvider.IRangeInfo) functionArgument.Value;
          if (ExcelErrorValue.Values.IsErrorValue(rangeInfo.GetValue(rangeInfo.Address._fromRow, rangeInfo.Address._fromCol)))
            return this.CreateResult((object) true, DataType.Boolean);
        }
        else if (ExcelErrorValue.Values.IsErrorValue(functionArgument.Value))
          return this.CreateResult((object) true, DataType.Boolean);
      }
      return this.CreateResult((object) false, DataType.Boolean);
    }

    public override CompileResult HandleError(string errorCode)
    {
      return this.CreateResult((object) true, DataType.Boolean);
    }
  }
}
