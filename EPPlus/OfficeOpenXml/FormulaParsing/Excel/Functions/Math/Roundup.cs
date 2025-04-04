// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Roundup
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Roundup : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      double num = this.ArgToDecimal(arguments, 0);
      int y = this.ArgToInt(arguments, 1);
      return this.CreateResult((object) (num >= 0.0 ? System.Math.Ceiling(num * System.Math.Pow(10.0, (double) y)) / System.Math.Pow(10.0, (double) y) : System.Math.Floor(num * System.Math.Pow(10.0, (double) y)) / System.Math.Pow(10.0, (double) y)), DataType.Decimal);
    }
  }
}
