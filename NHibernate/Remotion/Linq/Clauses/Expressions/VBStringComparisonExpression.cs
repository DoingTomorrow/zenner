// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.VBStringComparisonExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.Expressions
{
  public class VBStringComparisonExpression : ExtensionExpression
  {
    public const ExpressionType ExpressionType = (ExpressionType) 100003;
    private readonly Expression _comparison;
    private readonly bool _textCompare;

    public VBStringComparisonExpression(Expression comparison, bool textCompare)
      : base(comparison.Type, (ExpressionType) 100003)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (comparison), comparison);
      this._comparison = comparison;
      this._textCompare = textCompare;
    }

    public Expression Comparison => this._comparison;

    public bool TextCompare => this._textCompare;

    public override bool CanReduce => true;

    public override Expression Reduce() => this._comparison;

    protected internal override Expression VisitChildren(ExpressionTreeVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionTreeVisitor>(nameof (visitor), visitor);
      Expression comparison = visitor.VisitExpression(this._comparison);
      return comparison != this._comparison ? (Expression) new VBStringComparisonExpression(comparison, this._textCompare) : (Expression) this;
    }

    public override Expression Accept(ExpressionTreeVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionTreeVisitor>(nameof (visitor), visitor);
      return visitor is IVBSpecificExpressionVisitor expressionVisitor ? expressionVisitor.VisitVBStringComparisonExpression(this) : base.Accept(visitor);
    }

    public override string ToString()
    {
      return string.Format("VBCompareString({0}, {1})", (object) FormattingExpressionTreeVisitor.Format(this.Comparison), (object) this.TextCompare);
    }
  }
}
