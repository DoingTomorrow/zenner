// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Projections
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Impl;
using NHibernate.Type;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  public class Projections
  {
    private Projections()
    {
    }

    public static IProjection Distinct(IProjection proj) => (IProjection) new NHibernate.Criterion.Distinct(proj);

    public static NHibernate.Criterion.ProjectionList ProjectionList() => new NHibernate.Criterion.ProjectionList();

    public static IProjection RowCount() => (IProjection) new RowCountProjection();

    public static IProjection RowCountInt64() => (IProjection) new RowCountInt64Projection();

    public static CountProjection Count(IProjection projection) => new CountProjection(projection);

    public static CountProjection Count(string propertyName) => new CountProjection(propertyName);

    public static CountProjection CountDistinct(string propertyName)
    {
      return new CountProjection(propertyName).SetDistinct();
    }

    public static AggregateProjection Max(string propertyName)
    {
      return new AggregateProjection("max", propertyName);
    }

    public static AggregateProjection Max(IProjection projection)
    {
      return new AggregateProjection("max", projection);
    }

    public static AggregateProjection Min(string propertyName)
    {
      return new AggregateProjection("min", propertyName);
    }

    public static AggregateProjection Min(IProjection projection)
    {
      return new AggregateProjection("min", projection);
    }

    public static AggregateProjection Avg(string propertyName)
    {
      return (AggregateProjection) new AvgProjection(propertyName);
    }

    public static AggregateProjection Avg(IProjection projection)
    {
      return (AggregateProjection) new AvgProjection(projection);
    }

    public static AggregateProjection Sum(string propertyName)
    {
      return new AggregateProjection("sum", propertyName);
    }

    public static AggregateProjection Sum(IProjection projection)
    {
      return new AggregateProjection("sum", projection);
    }

    public static IProjection SqlProjection(string sql, string[] columnAliases, IType[] types)
    {
      return (IProjection) new SQLProjection(sql, columnAliases, types);
    }

    public static IProjection SqlGroupProjection(
      string sql,
      string groupBy,
      string[] columnAliases,
      IType[] types)
    {
      return (IProjection) new SQLProjection(sql, groupBy, columnAliases, types);
    }

    public static PropertyProjection GroupProperty(string propertyName)
    {
      return new PropertyProjection(propertyName, true);
    }

    public static GroupedProjection GroupProperty(IProjection projection)
    {
      return new GroupedProjection(projection);
    }

    public static PropertyProjection Property(string propertyName)
    {
      return new PropertyProjection(propertyName);
    }

    public static IdentifierProjection Id() => new IdentifierProjection();

    public static IProjection Alias(IProjection projection, string alias)
    {
      return (IProjection) new AliasedProjection(projection, alias);
    }

    public static IProjection Cast(IType type, IProjection projection)
    {
      return (IProjection) new CastProjection(type, projection);
    }

    public static IProjection Constant(object obj) => (IProjection) new ConstantProjection(obj);

    public static IProjection Constant(object obj, IType type)
    {
      return (IProjection) new ConstantProjection(obj, type);
    }

    public static IProjection SqlFunction(
      string functionName,
      IType type,
      params IProjection[] projections)
    {
      return (IProjection) new SqlFunctionProjection(functionName, type, projections);
    }

    public static IProjection SqlFunction(
      ISQLFunction function,
      IType type,
      params IProjection[] projections)
    {
      return (IProjection) new SqlFunctionProjection(function, type, projections);
    }

    public static IProjection Conditional(
      ICriterion criterion,
      IProjection whenTrue,
      IProjection whenFalse)
    {
      return (IProjection) new ConditionalProjection(criterion, whenTrue, whenFalse);
    }

    public static IProjection SubQuery(DetachedCriteria detachedCriteria)
    {
      return (IProjection) new SubqueryProjection(new SelectSubqueryExpression(detachedCriteria));
    }

    public static AggregateProjection Avg<T>(Expression<Func<T, object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Avg), new Func<IProjection, AggregateProjection>(Projections.Avg));
    }

    public static AggregateProjection Avg(Expression<Func<object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Avg), new Func<IProjection, AggregateProjection>(Projections.Avg));
    }

    public static CountProjection Count<T>(Expression<Func<T, object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<CountProjection>(new Func<string, CountProjection>(Projections.Count), new Func<IProjection, CountProjection>(Projections.Count));
    }

    public static CountProjection Count(Expression<Func<object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<CountProjection>(new Func<string, CountProjection>(Projections.Count), new Func<IProjection, CountProjection>(Projections.Count));
    }

    public static CountProjection CountDistinct<T>(Expression<Func<T, object>> expression)
    {
      return Projections.CountDistinct(ExpressionProcessor.FindMemberExpression(expression.Body));
    }

    public static CountProjection CountDistinct(Expression<Func<object>> expression)
    {
      return Projections.CountDistinct(ExpressionProcessor.FindMemberExpression(expression.Body));
    }

    public static PropertyProjection Group<T>(Expression<Func<T, object>> expression)
    {
      return Projections.GroupProperty(ExpressionProcessor.FindMemberExpression(expression.Body));
    }

    public static PropertyProjection Group(Expression<Func<object>> expression)
    {
      return Projections.GroupProperty(ExpressionProcessor.FindMemberExpression(expression.Body));
    }

    public static AggregateProjection Max<T>(Expression<Func<T, object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Max), new Func<IProjection, AggregateProjection>(Projections.Max));
    }

    public static AggregateProjection Max(Expression<Func<object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Max), new Func<IProjection, AggregateProjection>(Projections.Max));
    }

    public static AggregateProjection Min<T>(Expression<Func<T, object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Min), new Func<IProjection, AggregateProjection>(Projections.Min));
    }

    public static AggregateProjection Min(Expression<Func<object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Min), new Func<IProjection, AggregateProjection>(Projections.Min));
    }

    public static PropertyProjection Property<T>(Expression<Func<T, object>> expression)
    {
      return Projections.Property(ExpressionProcessor.FindMemberExpression(expression.Body));
    }

    public static PropertyProjection Property(Expression<Func<object>> expression)
    {
      return Projections.Property(ExpressionProcessor.FindMemberExpression(expression.Body));
    }

    public static IProjection SubQuery<T>(QueryOver<T> detachedQueryOver)
    {
      return Projections.SubQuery(detachedQueryOver.DetachedCriteria);
    }

    public static AggregateProjection Sum<T>(Expression<Func<T, object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Sum), new Func<IProjection, AggregateProjection>(Projections.Sum));
    }

    public static AggregateProjection Sum(Expression<Func<object>> expression)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).Create<AggregateProjection>(new Func<string, AggregateProjection>(Projections.Sum), new Func<IProjection, AggregateProjection>(Projections.Sum));
    }

    public static string Concat(params string[] strings)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessConcat(MethodCallExpression methodCallExpression)
    {
      NewArrayExpression newArrayExpression = (NewArrayExpression) methodCallExpression.Arguments[0];
      IProjection[] projectionArray = new IProjection[newArrayExpression.Expressions.Count];
      for (int index = 0; index < newArrayExpression.Expressions.Count; ++index)
        projectionArray[index] = ExpressionProcessor.FindMemberProjection(newArrayExpression.Expressions[index]).AsProjection();
      return Projections.SqlFunction("concat", (IType) NHibernateUtil.String, projectionArray);
    }
  }
}
