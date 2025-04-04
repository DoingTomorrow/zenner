// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Rounddown
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Rounddown : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      double num1 = this.ArgToDecimal(arguments, 0);
      int nDecimals = this.ArgToInt(arguments, 1);
      int num2 = num1 < 0.0 ? -1 : 1;
      double num3 = num1 * (double) num2;
      double num4;
      if (nDecimals > 0)
      {
        num4 = Rounddown.RoundDownDecimalNumber(num3, nDecimals);
      }
      else
      {
        double num5 = (double) (int) System.Math.Floor(num3);
        num4 = num5 - num5 % System.Math.Pow(10.0, (double) (nDecimals * -1));
      }
      return this.CreateResult((object) (num4 * (double) num2), DataType.Decimal);
    }

    private static double RoundDownDecimalNumber(double number, int nDecimals)
    {
      double num1 = System.Math.Floor(number);
      double num2 = number - num1;
      double num3 = System.Math.Truncate(System.Math.Pow(10.0, (double) nDecimals) * num2) / System.Math.Pow(10.0, (double) nDecimals);
      return num1 + num3;
    }
  }
}
