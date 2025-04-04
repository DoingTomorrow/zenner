// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Fact
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Fact : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      double number = this.ArgToDecimal(arguments, 0);
      this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() => number < 0.0), eErrorType.NA);
      double result = 1.0;
      for (int index = 1; (double) index < number; ++index)
        result *= (double) index;
      return this.CreateResult((object) result, DataType.Integer);
    }
  }
}
