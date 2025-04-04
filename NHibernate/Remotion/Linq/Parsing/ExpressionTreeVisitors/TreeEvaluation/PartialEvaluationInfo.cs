// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.TreeEvaluation.PartialEvaluationInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.TreeEvaluation
{
  public class PartialEvaluationInfo
  {
    private readonly HashSet<Expression> _evaluatableExpressions = new HashSet<Expression>();

    public int Count => this._evaluatableExpressions.Count;

    public void AddEvaluatableExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      this._evaluatableExpressions.Add(expression);
    }

    public bool IsEvaluatableExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this._evaluatableExpressions.Contains(expression);
    }
  }
}
