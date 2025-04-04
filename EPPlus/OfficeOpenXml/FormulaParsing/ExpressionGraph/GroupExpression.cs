// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.GroupExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class GroupExpression : Expression
  {
    private readonly IExpressionCompiler _expressionCompiler;

    public GroupExpression()
      : this((IExpressionCompiler) new ExpressionCompiler())
    {
    }

    public GroupExpression(IExpressionCompiler expressionCompiler)
    {
      this._expressionCompiler = expressionCompiler;
    }

    public override CompileResult Compile() => this._expressionCompiler.Compile(this.Children);

    public override bool IsGroupedExpression => true;
  }
}
