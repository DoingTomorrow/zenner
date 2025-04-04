// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.StdevP
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class StdevP : HiddenValuesHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this.CreateResult((object) StdevP.StandardDeviation(this.ArgsToDoubleEnumerable(this.IgnoreHiddenValues, false, arguments, context)), DataType.Decimal);
    }

    private static double StandardDeviation(IEnumerable<double> values)
    {
      double avg = values.Average();
      return System.Math.Sqrt(values.Average<double>((Func<double, double>) (v => System.Math.Pow(v - avg, 2.0))));
    }
  }
}
