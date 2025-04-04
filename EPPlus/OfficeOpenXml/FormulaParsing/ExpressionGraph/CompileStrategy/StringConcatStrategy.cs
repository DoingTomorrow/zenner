// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileStrategy.StringConcatStrategy
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileStrategy
{
  public class StringConcatStrategy(Expression expression) : OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileStrategy.CompileStrategy(expression)
  {
    public override Expression Compile()
    {
      StringExpression stringExpression = ExpressionConverter.Instance.ToStringExpression(this._expression);
      stringExpression.Prev = this._expression.Prev;
      stringExpression.Next = this._expression.Next;
      if (this._expression.Prev != null)
        this._expression.Prev.Next = (Expression) stringExpression;
      if (this._expression.Next != null)
        this._expression.Next.Prev = (Expression) stringExpression;
      return stringExpression.MergeWithNext();
    }
  }
}
