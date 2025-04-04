// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.TreeEvaluation.EvaluatableTreeFindingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.TreeEvaluation
{
  public class EvaluatableTreeFindingExpressionTreeVisitor : 
    ExpressionTreeVisitor,
    IPartialEvaluationExceptionExpressionVisitor
  {
    private readonly PartialEvaluationInfo _partialEvaluationInfo = new PartialEvaluationInfo();
    private bool _isCurrentSubtreeEvaluatable;

    public static PartialEvaluationInfo Analyze(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      EvaluatableTreeFindingExpressionTreeVisitor expressionTreeVisitor = new EvaluatableTreeFindingExpressionTreeVisitor();
      expressionTreeVisitor.VisitExpression(expressionTree);
      return expressionTreeVisitor._partialEvaluationInfo;
    }

    public override Expression VisitExpression(Expression expression)
    {
      if (expression == null)
        return base.VisitExpression(expression);
      bool subtreeEvaluatable = this._isCurrentSubtreeEvaluatable;
      this._isCurrentSubtreeEvaluatable = ExpressionTreeVisitor.IsSupportedStandardExpression(expression);
      Expression expression1 = base.VisitExpression(expression);
      if (this._isCurrentSubtreeEvaluatable)
        this._partialEvaluationInfo.AddEvaluatableExpression(expression);
      this._isCurrentSubtreeEvaluatable &= subtreeEvaluatable;
      return expression1;
    }

    protected override Expression VisitParameterExpression(ParameterExpression expression)
    {
      this._isCurrentSubtreeEvaluatable = false;
      return base.VisitParameterExpression(expression);
    }

    protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
    {
      if (this.IsQueryableExpression(expression.Object))
        this._isCurrentSubtreeEvaluatable = false;
      for (int index = 0; index < expression.Arguments.Count && this._isCurrentSubtreeEvaluatable; ++index)
      {
        if (this.IsQueryableExpression(expression.Arguments[index]))
          this._isCurrentSubtreeEvaluatable = false;
      }
      return base.VisitMethodCallExpression(expression);
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      if (this.IsQueryableExpression(expression.Expression))
        this._isCurrentSubtreeEvaluatable = false;
      return base.VisitMemberExpression(expression);
    }

    protected override Expression VisitMemberInitExpression(MemberInitExpression expression)
    {
      ArgumentUtility.CheckNotNull<MemberInitExpression>(nameof (expression), expression);
      this.VisitMemberBindingList(expression.Bindings);
      if (!this._isCurrentSubtreeEvaluatable)
        return (Expression) expression;
      this.VisitExpression((Expression) expression.NewExpression);
      return (Expression) expression;
    }

    protected override Expression VisitListInitExpression(ListInitExpression expression)
    {
      ArgumentUtility.CheckNotNull<ListInitExpression>(nameof (expression), expression);
      this.VisitElementInitList(expression.Initializers);
      if (!this._isCurrentSubtreeEvaluatable)
        return (Expression) expression;
      this.VisitExpression((Expression) expression.NewExpression);
      return (Expression) expression;
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression;
    }

    private bool IsQueryableExpression(Expression expression)
    {
      return expression != null && typeof (IQueryable).IsAssignableFrom(expression.Type);
    }

    public Expression VisitPartialEvaluationExceptionExpression(
      PartialEvaluationExceptionExpression partialEvaluationExceptionExpression)
    {
      this._isCurrentSubtreeEvaluatable = false;
      return (Expression) partialEvaluationExceptionExpression;
    }
  }
}
