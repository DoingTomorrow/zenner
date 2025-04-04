// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.BooleanExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class BooleanExpression : AtomicExpression
  {
    private bool? _precompiledValue;

    public BooleanExpression(string expression)
      : base(expression)
    {
    }

    public BooleanExpression(bool value)
      : base(value ? "true" : "false")
    {
      this._precompiledValue = new bool?(value);
    }

    public override CompileResult Compile()
    {
      return new CompileResult((object) (((int) this._precompiledValue ?? (bool.Parse(this.ExpressionString) ? 1 : 0)) != 0), DataType.Boolean);
    }
  }
}
