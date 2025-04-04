// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.EnumerableExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class EnumerableExpression : Expression
  {
    private readonly IExpressionCompiler _expressionCompiler;

    public EnumerableExpression()
      : this((IExpressionCompiler) new ExpressionCompiler())
    {
    }

    public EnumerableExpression(IExpressionCompiler expressionCompiler)
    {
      this._expressionCompiler = expressionCompiler;
    }

    public override bool IsGroupedExpression => false;

    public override Expression PrepareForNextChild() => (Expression) this;

    public override CompileResult Compile()
    {
      List<object> result = new List<object>();
      foreach (Expression child in this.Children)
        result.Add(this._expressionCompiler.Compile((IEnumerable<Expression>) new List<Expression>()
        {
          child
        }).Result);
      return new CompileResult((object) result, DataType.Enumerable);
    }
  }
}
