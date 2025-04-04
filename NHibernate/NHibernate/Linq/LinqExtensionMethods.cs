// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.LinqExtensionMethods
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using Remotion.Linq;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public static class LinqExtensionMethods
  {
    public static IQueryable<T> Query<T>(this ISession session)
    {
      return (IQueryable<T>) new NhQueryable<T>(session.GetSessionImplementation());
    }

    public static IQueryable<T> Query<T>(this IStatelessSession session)
    {
      return (IQueryable<T>) new NhQueryable<T>(session.GetSessionImplementation());
    }

    public static IQueryable<T> Cacheable<T>(this IQueryable<T> query)
    {
      MethodCallExpression methodCallExpression = Expression.Call(ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).Cacheable<object>())).MakeGenericMethod(typeof (T)), query.Expression);
      return (IQueryable<T>) new NhQueryable<T>(query.Provider, (Expression) methodCallExpression);
    }

    public static IQueryable<T> CacheMode<T>(this IQueryable<T> query, NHibernate.CacheMode cacheMode)
    {
      MethodCallExpression methodCallExpression = Expression.Call(ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).CacheMode<object>(NHibernate.CacheMode.Normal))).MakeGenericMethod(typeof (T)), query.Expression, (Expression) Expression.Constant((object) cacheMode));
      return (IQueryable<T>) new NhQueryable<T>(query.Provider, (Expression) methodCallExpression);
    }

    public static IQueryable<T> CacheRegion<T>(this IQueryable<T> query, string region)
    {
      MethodCallExpression methodCallExpression = Expression.Call(ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).CacheRegion<object>(default (string)))).MakeGenericMethod(typeof (T)), query.Expression, (Expression) Expression.Constant((object) region));
      return (IQueryable<T>) new NhQueryable<T>(query.Provider, (Expression) methodCallExpression);
    }

    public static IQueryable<T> Timeout<T>(this IQueryable<T> query, int timeout)
    {
      MethodCallExpression methodCallExpression = Expression.Call(ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).Timeout<object>(0))).MakeGenericMethod(typeof (T)), query.Expression, (Expression) Expression.Constant((object) timeout));
      return (IQueryable<T>) new NhQueryable<T>(query.Provider, (Expression) methodCallExpression);
    }

    public static IEnumerable<T> ToFuture<T>(this IQueryable<T> query)
    {
      if (!(query is QueryableBase<T> queryableBase))
        throw new NotSupportedException("Query needs to be of type QueryableBase<T>");
      return (IEnumerable<T>) ((INhQueryProvider) queryableBase.Provider).ExecuteFuture(queryableBase.Expression);
    }

    public static IFutureValue<T> ToFutureValue<T>(this IQueryable<T> query)
    {
      if (!(query is QueryableBase<T> queryableBase))
        throw new NotSupportedException("Query needs to be of type QueryableBase<T>");
      object future = ((INhQueryProvider) queryableBase.Provider).ExecuteFuture(queryableBase.Expression);
      return future is IEnumerable<T> ? (IFutureValue<T>) new FutureValue<T>((FutureValue<T>.GetResult) (() => (IEnumerable<T>) future)) : (IFutureValue<T>) future;
    }

    public static IFutureValue<TResult> ToFutureValue<T, TResult>(
      this IQueryable<T> query,
      Expression<Func<IQueryable<T>, TResult>> selector)
    {
      if (!(query is QueryableBase<T>))
        throw new NotSupportedException("Query needs to be of type QueryableBase<T>");
      return (IFutureValue<TResult>) ((INhQueryProvider) query.Provider).ExecuteFuture(ReplacingExpressionTreeVisitor.Replace((Expression) selector.Parameters.Single<ParameterExpression>(), query.Expression, selector.Body));
    }
  }
}
