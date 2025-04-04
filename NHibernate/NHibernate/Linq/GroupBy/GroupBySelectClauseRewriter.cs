// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupBy.GroupBySelectClauseRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.GroupBy
{
  internal class GroupBySelectClauseRewriter : NhExpressionTreeVisitor
  {
    private readonly GroupResultOperator _groupBy;
    private readonly QueryModel _model;

    public static Expression ReWrite(
      Expression expression,
      GroupResultOperator groupBy,
      QueryModel model)
    {
      return new GroupBySelectClauseRewriter(groupBy, model).VisitExpression(expression);
    }

    private GroupBySelectClauseRewriter(GroupResultOperator groupBy, QueryModel model)
    {
      this._groupBy = groupBy;
      this._model = model;
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return expression.ReferencedQuerySource == this._groupBy ? this._groupBy.ElementSelector : base.VisitQuerySourceReferenceExpression(expression);
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      Expression result;
      if (expression.Expression is MemberExpression expression1 && expression1.Member.Name == "Key" && this.IsMemberOfModel(expression1) && GroupBySelectClauseRewriter.TryFindTransparentPropertyOfAnonymousObject(this._groupBy.KeySelector, expression.Member.Name, out result))
        return result;
      if (!this.IsMemberOfModel(expression))
        return base.VisitMemberExpression(expression);
      if (expression.Member.Name == "Key")
        return this._groupBy.KeySelector;
      Expression elementSelector = this._groupBy.ElementSelector;
      if (elementSelector is MemberExpression || elementSelector is QuerySourceReferenceExpression)
        return base.VisitMemberExpression(expression);
      if (GroupBySelectClauseRewriter.TryFindTransparentPropertyOfAnonymousObject(elementSelector, expression.Member.Name, out result))
        return result;
      throw new NotImplementedException();
    }

    private static bool TryFindTransparentPropertyOfAnonymousObject(
      Expression expression,
      string memberName,
      out Expression result)
    {
      if (expression is NewExpression newExpression)
      {
        int index = 0;
        foreach (MemberInfo member in newExpression.Members)
        {
          if (member.Name == "get_" + memberName)
          {
            result = newExpression.Arguments[index];
            return true;
          }
          ++index;
        }
      }
      result = (Expression) null;
      return false;
    }

    private bool IsMemberOfModel(MemberExpression expression)
    {
      if (!(expression.Expression is QuerySourceReferenceExpression expression1) || !(expression1.ReferencedQuerySource is FromClauseBase referencedQuerySource))
        return false;
      if (referencedQuerySource.FromExpression is SubQueryExpression fromExpression1)
        return fromExpression1.QueryModel == this._model;
      return referencedQuerySource.FromExpression is QuerySourceReferenceExpression fromExpression2 && (fromExpression2.ReferencedQuerySource as FromClauseBase).FromExpression is SubQueryExpression fromExpression3 && fromExpression3.QueryModel == this._model;
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      return GroupBySelectClauseRewriter.ReWrite(expression.QueryModel.SelectClause.Selector, this._groupBy, this._model);
    }
  }
}
