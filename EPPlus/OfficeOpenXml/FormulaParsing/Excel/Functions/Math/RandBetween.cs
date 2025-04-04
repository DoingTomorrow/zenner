// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.RandBetween
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class RandBetween : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      double low = this.ArgToDecimal(arguments, 0);
      double high = this.ArgToDecimal(arguments, 1);
      object result = new Rand().Execute((IEnumerable<FunctionArgument>) new FunctionArgument[0], context).Result;
      double num = System.Math.Floor(this.CalulateDiff(high, low) * (double) result + 1.0);
      return this.CreateResult((object) (low + num), DataType.Integer);
    }

    private double CalulateDiff(double high, double low)
    {
      if (high > 0.0 && low < 0.0)
        return high + low * -1.0;
      return high < 0.0 && low < 0.0 ? high * -1.0 - low * -1.0 : high - low;
    }
  }
}
