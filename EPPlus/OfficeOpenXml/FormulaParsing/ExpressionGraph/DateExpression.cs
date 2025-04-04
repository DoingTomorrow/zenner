// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.DateExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class DateExpression(string expression) : AtomicExpression(expression)
  {
    public override CompileResult Compile()
    {
      return new CompileResult((object) DateTime.FromOADate(double.Parse(this.ExpressionString, (IFormatProvider) CultureInfo.InvariantCulture)), DataType.Date);
    }
  }
}
