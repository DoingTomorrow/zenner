// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Information.IsNa
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Information
{
  public class IsNa : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      if (arguments == null || arguments.Count<FunctionArgument>() == 0)
        return this.CreateResult((object) false, DataType.Boolean);
      object firstValue = this.GetFirstValue(arguments);
      return firstValue is ExcelErrorValue && ((ExcelErrorValue) firstValue).Type == eErrorType.NA ? this.CreateResult((object) true, DataType.Boolean) : this.CreateResult((object) false, DataType.Boolean);
    }
  }
}
