// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.DecimalExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class DecimalExpression : AtomicExpression
  {
    private readonly double? _compiledValue;
    private readonly bool _negate;

    public DecimalExpression(string expression)
      : this(expression, false)
    {
    }

    public DecimalExpression(string expression, bool negate)
      : base(expression)
    {
      this._negate = negate;
    }

    public DecimalExpression(double compiledValue)
      : base(compiledValue.ToString((IFormatProvider) CultureInfo.InvariantCulture))
    {
      this._compiledValue = new double?(compiledValue);
    }

    public override CompileResult Compile()
    {
      double num = this._compiledValue.HasValue ? this._compiledValue.Value : double.Parse(this.ExpressionString, (IFormatProvider) CultureInfo.InvariantCulture);
      return new CompileResult((object) (this._negate ? num * -1.0 : num), DataType.Decimal);
    }
  }
}
