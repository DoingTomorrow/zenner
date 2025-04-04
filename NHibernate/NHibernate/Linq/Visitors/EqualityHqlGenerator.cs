// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.EqualityHqlGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class EqualityHqlGenerator
  {
    private readonly HqlTreeBuilder _hqlTreeBuilder;
    private readonly VisitorParameters _parameters;

    public EqualityHqlGenerator(VisitorParameters parameters)
    {
      this._parameters = parameters;
      this._hqlTreeBuilder = new HqlTreeBuilder();
    }

    public HqlBooleanExpression Visit(Expression innerKeySelector, Expression outerKeySelector)
    {
      return !(innerKeySelector is NewExpression) || !(outerKeySelector is NewExpression) ? (HqlBooleanExpression) this.GenerateEqualityNode(innerKeySelector, outerKeySelector) : this.VisitNew((NewExpression) innerKeySelector, (NewExpression) outerKeySelector);
    }

    private HqlBooleanExpression VisitNew(
      NewExpression innerKeySelector,
      NewExpression outerKeySelector)
    {
      if (innerKeySelector.Arguments.Count != outerKeySelector.Arguments.Count)
        throw new NotSupportedException();
      HqlBooleanExpression lhs = (HqlBooleanExpression) this.GenerateEqualityNode(innerKeySelector, outerKeySelector, 0);
      for (int index = 1; index < innerKeySelector.Arguments.Count; ++index)
        lhs = (HqlBooleanExpression) this._hqlTreeBuilder.BooleanAnd(lhs, (HqlBooleanExpression) this.GenerateEqualityNode(innerKeySelector, outerKeySelector, index));
      return lhs;
    }

    private HqlEquality GenerateEqualityNode(
      NewExpression innerKeySelector,
      NewExpression outerKeySelector,
      int index)
    {
      return this.GenerateEqualityNode(innerKeySelector.Arguments[index], outerKeySelector.Arguments[index]);
    }

    private HqlEquality GenerateEqualityNode(Expression leftExpr, Expression rightExpr)
    {
      HqlGeneratorExpressionTreeVisitor expressionTreeVisitor1 = new HqlGeneratorExpressionTreeVisitor(this._parameters);
      HqlGeneratorExpressionTreeVisitor expressionTreeVisitor2 = new HqlGeneratorExpressionTreeVisitor(this._parameters);
      return this._hqlTreeBuilder.Equality(expressionTreeVisitor1.Visit(leftExpr).AsExpression(), expressionTreeVisitor2.Visit(rightExpr).AsExpression());
    }
  }
}
