// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.PartialEvaluationExceptionExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.Expressions
{
  public class PartialEvaluationExceptionExpression : ExtensionExpression
  {
    public const ExpressionType ExpressionType = (ExpressionType) 100004;
    private readonly Exception _exception;
    private readonly Expression _evaluatedExpression;

    public PartialEvaluationExceptionExpression(Exception exception, Expression evaluatedExpression)
      : base(ArgumentUtility.CheckNotNull<Expression>(nameof (evaluatedExpression), evaluatedExpression).Type, (ExpressionType) 100004)
    {
      ArgumentUtility.CheckNotNull<Exception>(nameof (exception), exception);
      this._exception = exception;
      this._evaluatedExpression = evaluatedExpression;
    }

    public Exception Exception => this._exception;

    public Expression EvaluatedExpression => this._evaluatedExpression;

    public override bool CanReduce => true;

    public override Expression Reduce() => this._evaluatedExpression;

    protected internal override Expression VisitChildren(ExpressionTreeVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionTreeVisitor>(nameof (visitor), visitor);
      Expression evaluatedExpression = visitor.VisitExpression(this._evaluatedExpression);
      return evaluatedExpression != this._evaluatedExpression ? (Expression) new PartialEvaluationExceptionExpression(this._exception, evaluatedExpression) : (Expression) this;
    }

    public override Expression Accept(ExpressionTreeVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionTreeVisitor>(nameof (visitor), visitor);
      return visitor is IPartialEvaluationExceptionExpressionVisitor expressionVisitor ? expressionVisitor.VisitPartialEvaluationExceptionExpression(this) : base.Accept(visitor);
    }

    public override string ToString()
    {
      return string.Format("PartialEvalException ({0} (\"{1}\"), {2})", (object) this._exception.GetType().Name, (object) this._exception.Message, (object) FormattingExpressionTreeVisitor.Format(this._evaluatedExpression));
    }
  }
}
