// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Information.IsErr
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Information
{
  public class IsErr : ErrorHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      CompileResult compileResult = new IsError().Execute(arguments, context);
      if ((bool) compileResult.Result)
      {
        object firstValue = this.GetFirstValue(arguments);
        switch (firstValue)
        {
          case ExcelDataProvider.IRangeInfo _:
            ExcelDataProvider.IRangeInfo rangeInfo = (ExcelDataProvider.IRangeInfo) firstValue;
            if (rangeInfo.GetValue(rangeInfo.Address._fromRow, rangeInfo.Address._fromCol) is ExcelErrorValue excelErrorValue && excelErrorValue.Type == eErrorType.NA)
              return this.CreateResult((object) false, DataType.Boolean);
            break;
          case ExcelErrorValue _:
            if (((ExcelErrorValue) firstValue).Type == eErrorType.NA)
              return this.CreateResult((object) false, DataType.Boolean);
            break;
        }
      }
      return compileResult;
    }

    public override CompileResult HandleError(string errorCode)
    {
      return this.CreateResult((object) true, DataType.Boolean);
    }
  }
}
