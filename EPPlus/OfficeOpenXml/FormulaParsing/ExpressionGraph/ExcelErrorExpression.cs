// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.ExcelErrorExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class ExcelErrorExpression : Expression
  {
    private ExcelErrorValue _error;

    public ExcelErrorExpression(string expression, ExcelErrorValue error)
      : base(expression)
    {
      this._error = error;
    }

    public override bool IsGroupedExpression => false;

    public override CompileResult Compile()
    {
      return new CompileResult((object) this._error, DataType.ExcelError);
    }
  }
}
