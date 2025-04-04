// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NestedSelects.SelectClauseRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq.Clauses.Expressions;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.NestedSelects
{
  internal class SelectClauseRewriter : NhExpressionTreeVisitor
  {
    private readonly Dictionary<Expression, Expression> _dictionary;
    private readonly ICollection<ExpressionHolder> expressions;
    private readonly Expression parameter;
    private readonly int tuple;

    public SelectClauseRewriter(
      Expression parameter,
      ICollection<ExpressionHolder> expressions,
      Expression expression,
      Dictionary<Expression, Expression> dictionary)
      : this(parameter, expressions, expression, 0, dictionary)
    {
    }

    public SelectClauseRewriter(
      Expression parameter,
      ICollection<ExpressionHolder> expressions,
      Expression expression,
      int tuple,
      Dictionary<Expression, Expression> dictionary)
    {
      this.expressions = expressions;
      this.parameter = parameter;
      this.tuple = tuple;
      this.expressions.Add(new ExpressionHolder()
      {
        Expression = expression,
        Tuple = tuple
      });
      this._dictionary = dictionary;
    }

    public override Expression VisitExpression(Expression expression)
    {
      if (expression == null)
        return (Expression) null;
      Expression expression1;
      return this._dictionary.TryGetValue(expression, out expression1) ? expression1 : base.VisitExpression(expression);
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      return this.AddAndConvertExpression((Expression) expression);
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return this.AddAndConvertExpression((Expression) expression);
    }

    private Expression AddAndConvertExpression(Expression expression)
    {
      this.expressions.Add(new ExpressionHolder()
      {
        Expression = expression,
        Tuple = this.tuple
      });
      return (Expression) Expression.Convert((Expression) Expression.ArrayIndex((Expression) Expression.Property(this.parameter, Tuple.ItemsProperty), (Expression) Expression.Constant((object) (this.expressions.Count - 1))), expression.Type);
    }
  }
}
