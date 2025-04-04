// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.TransparentIdentifierRemovingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors
{
  public class TransparentIdentifierRemovingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    public static Expression ReplaceTransparentIdentifiers(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      Expression expression1 = expression;
      Expression expression2;
      do
      {
        expression2 = expression1;
        expression1 = new TransparentIdentifierRemovingExpressionTreeVisitor().VisitExpression(expression1);
      }
      while (expression1 != expression2);
      return expression1;
    }

    private TransparentIdentifierRemovingExpressionTreeVisitor()
    {
    }

    protected override Expression VisitMemberExpression(MemberExpression memberExpression)
    {
      IEnumerable<Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding> createdByExpression = this.GetMemberBindingsCreatedByExpression(memberExpression.Expression);
      if (createdByExpression == null)
        return base.VisitMemberExpression(memberExpression);
      Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding memberBinding = createdByExpression.Where<Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding>((Func<Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding, bool>) (binding => binding.MatchesReadAccess(memberExpression.Member))).LastOrDefault<Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding>();
      return memberBinding == null ? base.VisitMemberExpression(memberExpression) : memberBinding.AssociatedExpression;
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(TransparentIdentifierRemovingExpressionTreeVisitor.ReplaceTransparentIdentifiers));
      return (Expression) expression;
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression;
    }

    private IEnumerable<Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding> GetMemberBindingsCreatedByExpression(
      Expression expression)
    {
      switch (expression)
      {
        case MemberInitExpression memberInitExpression:
          return memberInitExpression.Bindings.Where<System.Linq.Expressions.MemberBinding>((Func<System.Linq.Expressions.MemberBinding, bool>) (binding => binding is MemberAssignment)).Select<System.Linq.Expressions.MemberBinding, Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding>((Func<System.Linq.Expressions.MemberBinding, Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding>) (assignment => Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding.Bind(assignment.Member, ((MemberAssignment) assignment).Expression)));
        case NewExpression newExpression:
          if (newExpression.Members != null)
            return this.GetMemberBindingsForNewExpression(newExpression);
          break;
      }
      return (IEnumerable<Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding>) null;
    }

    private IEnumerable<Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding> GetMemberBindingsForNewExpression(
      NewExpression newExpression)
    {
      for (int i = 0; i < newExpression.Members.Count; ++i)
        yield return Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding.Bind(newExpression.Members[i], newExpression.Arguments[i]);
    }
  }
}
