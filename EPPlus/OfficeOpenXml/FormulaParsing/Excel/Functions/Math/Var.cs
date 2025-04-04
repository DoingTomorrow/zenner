// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Var
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Var : HiddenValuesHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      IEnumerable<double> doubleEnumerable = this.ArgsToDoubleEnumerable(this.IgnoreHiddenValues, false, arguments, context);
      double avg = doubleEnumerable.Average();
      double num = doubleEnumerable.Aggregate<double, double>(0.0, (Func<double, double, double>) ((total, next) => total += System.Math.Pow(next - avg, 2.0))) / (double) (doubleEnumerable.Count<double>() - 1);
      return !double.IsNaN(num) ? new CompileResult((object) num, DataType.Decimal) : throw new ExcelErrorValueException(eErrorType.Div0);
    }
  }
}
