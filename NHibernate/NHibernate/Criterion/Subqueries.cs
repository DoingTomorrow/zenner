// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Subqueries
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion.Lambda;
using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  public class Subqueries
  {
    public static AbstractCriterion Exists(DetachedCriteria dc)
    {
      return (AbstractCriterion) new ExistsSubqueryExpression("exists", dc);
    }

    public static AbstractCriterion NotExists(DetachedCriteria dc)
    {
      return (AbstractCriterion) new ExistsSubqueryExpression("not exists", dc);
    }

    public static AbstractCriterion PropertyEqAll(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "=", "all", dc);
    }

    public static AbstractCriterion PropertyIn(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "in", (string) null, dc);
    }

    public static AbstractCriterion PropertyNotIn(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "not in", (string) null, dc);
    }

    public static AbstractCriterion PropertyEq(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "=", (string) null, dc);
    }

    public static AbstractCriterion PropertyNe(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "<>", (string) null, dc);
    }

    public static AbstractCriterion PropertyGt(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, ">", (string) null, dc);
    }

    public static AbstractCriterion PropertyLt(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "<", (string) null, dc);
    }

    public static AbstractCriterion PropertyGe(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, ">=", (string) null, dc);
    }

    public static AbstractCriterion PropertyLe(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "<=", (string) null, dc);
    }

    public static AbstractCriterion PropertyGtAll(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, ">", "all", dc);
    }

    public static AbstractCriterion PropertyLtAll(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "<", "all", dc);
    }

    public static AbstractCriterion PropertyGeAll(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, ">=", "all", dc);
    }

    public static AbstractCriterion PropertyLeAll(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "<=", "all", dc);
    }

    public static AbstractCriterion PropertyGtSome(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, ">", "some", dc);
    }

    public static AbstractCriterion PropertyLtSome(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "<", "some", dc);
    }

    public static AbstractCriterion PropertyGeSome(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, ">=", "some", dc);
    }

    public static AbstractCriterion PropertyLeSome(string propertyName, DetachedCriteria dc)
    {
      return (AbstractCriterion) new PropertySubqueryExpression(propertyName, "<=", "some", dc);
    }

    public static AbstractCriterion EqAll(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "=", "all", dc);
    }

    public static AbstractCriterion In(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "in", (string) null, dc);
    }

    public static AbstractCriterion NotIn(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "not in", (string) null, dc);
    }

    public static AbstractCriterion Eq(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "=", (string) null, dc);
    }

    public static AbstractCriterion Gt(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, ">", (string) null, dc);
    }

    public static AbstractCriterion Lt(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "<", (string) null, dc);
    }

    public static AbstractCriterion Ge(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, ">=", (string) null, dc);
    }

    public static AbstractCriterion Le(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "<=", (string) null, dc);
    }

    public static AbstractCriterion Ne(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "<>", (string) null, dc);
    }

    public static AbstractCriterion GtAll(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, ">", "all", dc);
    }

    public static AbstractCriterion LtAll(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "<", "all", dc);
    }

    public static AbstractCriterion GeAll(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, ">=", "all", dc);
    }

    public static AbstractCriterion LeAll(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "<=", "all", dc);
    }

    public static AbstractCriterion GtSome(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, ">", "some", dc);
    }

    public static AbstractCriterion LtSome(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "<", "some", dc);
    }

    public static AbstractCriterion GeSome(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, ">=", "some", dc);
    }

    public static AbstractCriterion LeSome(object value, DetachedCriteria dc)
    {
      return (AbstractCriterion) new SimpleSubqueryExpression(value, "<=", "some", dc);
    }

    public static AbstractCriterion Select(DetachedCriteria detachedCriteria)
    {
      return (AbstractCriterion) new SelectSubqueryExpression(detachedCriteria);
    }

    public static LambdaSubqueryBuilder WhereProperty<T>(Expression<Func<T, object>> expression)
    {
      return new LambdaSubqueryBuilder(ExpressionProcessor.FindMemberExpression(expression.Body), (object) null);
    }

    public static LambdaSubqueryBuilder WhereProperty(Expression<Func<object>> expression)
    {
      return new LambdaSubqueryBuilder(ExpressionProcessor.FindMemberExpression(expression.Body), (object) null);
    }

    public static LambdaSubqueryBuilder WhereValue(object value)
    {
      return new LambdaSubqueryBuilder((string) null, value);
    }

    public static AbstractCriterion Where<T>(Expression<Func<T, bool>> expression)
    {
      return ExpressionProcessor.ProcessSubquery<T>(LambdaSubqueryType.Exact, expression);
    }

    public static AbstractCriterion Where(Expression<Func<bool>> expression)
    {
      return ExpressionProcessor.ProcessSubquery(LambdaSubqueryType.Exact, expression);
    }

    public static AbstractCriterion WhereAll<T>(Expression<Func<T, bool>> expression)
    {
      return ExpressionProcessor.ProcessSubquery<T>(LambdaSubqueryType.All, expression);
    }

    public static AbstractCriterion WhereAll(Expression<Func<bool>> expression)
    {
      return ExpressionProcessor.ProcessSubquery(LambdaSubqueryType.All, expression);
    }

    public static AbstractCriterion WhereSome<T>(Expression<Func<T, bool>> expression)
    {
      return ExpressionProcessor.ProcessSubquery<T>(LambdaSubqueryType.Some, expression);
    }

    public static AbstractCriterion WhereSome(Expression<Func<bool>> expression)
    {
      return ExpressionProcessor.ProcessSubquery(LambdaSubqueryType.Some, expression);
    }

    public static AbstractCriterion WhereExists<U>(QueryOver<U> detachedQuery)
    {
      return Subqueries.Exists(detachedQuery.DetachedCriteria);
    }

    public static AbstractCriterion WhereNotExists<U>(QueryOver<U> detachedQuery)
    {
      return Subqueries.NotExists(detachedQuery.DetachedCriteria);
    }

    public static AbstractCriterion IsNull(DetachedCriteria dc)
    {
      return (AbstractCriterion) new NullSubqueryExpression("IS NULL", dc);
    }

    public static AbstractCriterion IsNotNull(DetachedCriteria dc)
    {
      return (AbstractCriterion) new NullSubqueryExpression("IS NOT NULL", dc);
    }
  }
}
