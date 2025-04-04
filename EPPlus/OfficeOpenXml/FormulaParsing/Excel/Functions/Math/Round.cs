// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Round
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Round : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      double num = this.ArgToDecimal(arguments, 0);
      int digits = this.ArgToInt(arguments, 1);
      if (digits >= 0)
        return this.CreateResult((object) System.Math.Round(num, digits), DataType.Decimal);
      int y = digits * -1;
      return this.CreateResult((object) (num - num % System.Math.Pow(10.0, (double) y)), DataType.Integer);
    }
  }
}
