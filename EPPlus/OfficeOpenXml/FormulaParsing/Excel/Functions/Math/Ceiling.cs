// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Ceiling
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Ceiling : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      double num1 = this.ArgToDecimal(arguments, 0);
      double sign = this.ArgToDecimal(arguments, 1);
      this.ValidateNumberAndSign(num1, sign);
      if (sign < 1.0 && sign > 0.0)
      {
        double num2 = System.Math.Floor(num1);
        int num3 = (int) ((num1 - num2) / sign) + 1;
        return this.CreateResult((object) (num2 + (double) num3 * sign), DataType.Decimal);
      }
      return sign == 1.0 ? this.CreateResult((object) System.Math.Ceiling(num1), DataType.Decimal) : this.CreateResult((object) (num1 - num1 % sign + sign), DataType.Decimal);
    }

    private void ValidateNumberAndSign(double number, double sign)
    {
      if (number > 0.0 && sign < 0.0)
        throw new InvalidOperationException("Ceiling cannot handle a negative significance when the number is positive" + string.Format("num: {0}, sign: {1}", (object) number, (object) sign));
    }
  }
}
