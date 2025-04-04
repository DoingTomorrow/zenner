// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Trunc
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Trunc : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      double d = this.ArgToDecimal(arguments, 0);
      if (arguments.Count<FunctionArgument>() == 1)
        return this.CreateResult((object) System.Math.Truncate(d), DataType.Decimal);
      this.ArgToInt(arguments, 1);
      return context.Configuration.FunctionRepository.GetFunction("rounddown").Execute(arguments, context);
    }
  }
}
