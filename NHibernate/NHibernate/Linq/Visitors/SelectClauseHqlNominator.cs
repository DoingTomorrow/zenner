// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.SelectClauseHqlNominator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Functions;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  internal class SelectClauseHqlNominator : NhExpressionTreeVisitor
  {
    private readonly ILinqToHqlGeneratorsRegistry _functionRegistry;
    private HashSet<Expression> _candidates;
    private bool _canBeCandidate;
    private Stack<bool> _stateStack;

    public SelectClauseHqlNominator(VisitorParameters parameters)
    {
      this._functionRegistry = parameters.SessionFactory.Settings.LinqToHqlGeneratorsRegistry;
    }

    internal HashSet<Expression> Nominate(Expression expression)
    {
      this._candidates = new HashSet<Expression>();
      this._canBeCandidate = true;
      this._stateStack = new Stack<bool>();
      this._stateStack.Push(false);
      this.VisitExpression(expression);
      return this._candidates;
    }

    public override Expression VisitExpression(Expression expression)
    {
      try
      {
        bool projectConstantsInHql = this._stateStack.Peek();
        if (!projectConstantsInHql && expression != null && this.IsRegisteredFunction(expression))
          projectConstantsInHql = true;
        this._stateStack.Push(projectConstantsInHql);
        if (expression == null)
          return (Expression) null;
        bool canBeCandidate = this._canBeCandidate;
        this._canBeCandidate = true;
        if (SelectClauseHqlNominator.CanBeEvaluatedInHqlStatementShortcut(expression))
        {
          this._candidates.Add(expression);
          return expression;
        }
        base.VisitExpression(expression);
        if (this._canBeCandidate)
        {
          if (this.CanBeEvaluatedInHqlSelectStatement(expression, projectConstantsInHql))
            this._candidates.Add(expression);
          else
            this._canBeCandidate = false;
        }
        this._canBeCandidate &= canBeCandidate;
      }
      finally
      {
        this._stateStack.Pop();
      }
      return expression;
    }

    private bool IsRegisteredFunction(Expression expression)
    {
      if (expression.NodeType == ExpressionType.Call)
      {
        MethodCallExpression methodCallExpression = (MethodCallExpression) expression;
        if (this._functionRegistry.TryGetGenerator(methodCallExpression.Method, out IHqlGeneratorForMethod _))
          return methodCallExpression.Object == null || methodCallExpression.Object.NodeType != ExpressionType.Constant;
      }
      return false;
    }

    private bool CanBeEvaluatedInHqlSelectStatement(
      Expression expression,
      bool projectConstantsInHql)
    {
      if (expression.NodeType == ExpressionType.MemberInit || expression.NodeType == ExpressionType.New)
        return false;
      if (expression.NodeType == ExpressionType.Constant)
        return projectConstantsInHql;
      return expression.NodeType != ExpressionType.Call || this.IsRegisteredFunction(expression);
    }

    private static bool CanBeEvaluatedInHqlStatementShortcut(Expression expression)
    {
      return expression.NodeType == (ExpressionType) 10004;
    }
  }
}
