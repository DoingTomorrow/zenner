// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Information.IsBlank
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Information
{
  public class IsBlank : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      if (arguments == null || arguments.Count<FunctionArgument>() == 0)
        return this.CreateResult((object) true, DataType.Boolean);
      bool result = true;
      foreach (FunctionArgument functionArgument in arguments)
      {
        if (functionArgument.Value is ExcelDataProvider.IRangeInfo)
        {
          ExcelDataProvider.IRangeInfo rangeInfo = (ExcelDataProvider.IRangeInfo) functionArgument.Value;
          if (rangeInfo.GetValue(rangeInfo.Address._fromRow, rangeInfo.Address._fromCol) != null)
            result = false;
        }
        else if (functionArgument.Value != null && functionArgument.Value.ToString() != string.Empty)
        {
          result = false;
          break;
        }
      }
      return this.CreateResult((object) result, DataType.Boolean);
    }
  }
}
