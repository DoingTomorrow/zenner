// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.SelectClauseVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class SelectClauseVisitor : ExpressionTreeVisitor
  {
    private HashSet<Expression> _hqlNodes;
    private readonly ParameterExpression _inputParameter;
    private readonly VisitorParameters _parameters;
    private int _iColumn;
    private List<HqlExpression> _hqlTreeNodes = new List<HqlExpression>();

    public SelectClauseVisitor(Type inputType, VisitorParameters parameters)
    {
      this._inputParameter = Expression.Parameter(inputType, "input");
      this._parameters = parameters;
    }

    public LambdaExpression ProjectionExpression { get; private set; }

    public IEnumerable<HqlExpression> GetHqlNodes()
    {
      return (IEnumerable<HqlExpression>) this._hqlTreeNodes;
    }

    public void Visit(Expression expression)
    {
      this._hqlNodes = new SelectClauseHqlNominator(this._parameters).Nominate(expression);
      Expression body = this.VisitExpression(expression);
      if (body != expression && !this._hqlNodes.Contains(expression))
        this.ProjectionExpression = Expression.Lambda(body, this._inputParameter);
      this._hqlTreeNodes = BooleanToCaseConvertor.Convert((IEnumerable<HqlExpression>) this._hqlTreeNodes).ToList<HqlExpression>();
    }

    public override Expression VisitExpression(Expression expression)
    {
      if (expression == null)
        return (Expression) null;
      if (!this._hqlNodes.Contains(expression))
        return base.VisitExpression(expression);
      this._hqlTreeNodes.Add(new HqlGeneratorExpressionTreeVisitor(this._parameters).Visit(expression).AsExpression());
      return (Expression) Expression.Convert((Expression) Expression.ArrayIndex((Expression) this._inputParameter, (Expression) Expression.Constant((object) this._iColumn++)), expression.Type);
    }
  }
}
