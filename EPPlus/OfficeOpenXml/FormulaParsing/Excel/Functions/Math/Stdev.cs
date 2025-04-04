// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Stdev
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
  public class Stdev : HiddenValuesHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      return this.CreateResult((object) Stdev.StandardDeviation(this.ArgsToDoubleEnumerable(arguments, context, false)), DataType.Decimal);
    }

    private static double StandardDeviation(IEnumerable<double> values)
    {
      double num = 0.0;
      if (values.Count<double>() > 0)
      {
        double avg = values.Count<double>() != 1 ? values.Average() : throw new ExcelErrorValueException(eErrorType.Div0);
        num = System.Math.Sqrt(values.Sum<double>((Func<double, double>) (d => System.Math.Pow(d - avg, 2.0))) / (double) (values.Count<double>() - 1));
      }
      return num;
    }
  }
}
