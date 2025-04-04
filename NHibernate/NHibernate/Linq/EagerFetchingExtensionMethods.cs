// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.EagerFetchingExtensionMethods
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq
{
  public static class EagerFetchingExtensionMethods
  {
    public static INhFetchRequest<TOriginating, TRelated> Fetch<TOriginating, TRelated>(
      this IQueryable<TOriginating> query,
      Expression<Func<TOriginating, TRelated>> relatedObjectSelector)
    {
      ArgumentUtility.CheckNotNull<IQueryable<TOriginating>>(nameof (query), query);
      ArgumentUtility.CheckNotNull<Expression<Func<TOriginating, TRelated>>>(nameof (relatedObjectSelector), relatedObjectSelector);
      return EagerFetchingExtensionMethods.CreateFluentFetchRequest<TOriginating, TRelated>(((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TOriginating), typeof (TRelated)), query, (LambdaExpression) relatedObjectSelector);
    }

    public static INhFetchRequest<TOriginating, TRelated> FetchMany<TOriginating, TRelated>(
      this IQueryable<TOriginating> query,
      Expression<Func<TOriginating, IEnumerable<TRelated>>> relatedObjectSelector)
    {
      ArgumentUtility.CheckNotNull<IQueryable<TOriginating>>(nameof (query), query);
      ArgumentUtility.CheckNotNull<Expression<Func<TOriginating, IEnumerable<TRelated>>>>(nameof (relatedObjectSelector), relatedObjectSelector);
      return EagerFetchingExtensionMethods.CreateFluentFetchRequest<TOriginating, TRelated>(((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TOriginating), typeof (TRelated)), query, (LambdaExpression) relatedObjectSelector);
    }

    public static INhFetchRequest<TQueried, TRelated> ThenFetch<TQueried, TFetch, TRelated>(
      this INhFetchRequest<TQueried, TFetch> query,
      Expression<Func<TFetch, TRelated>> relatedObjectSelector)
    {
      ArgumentUtility.CheckNotNull<INhFetchRequest<TQueried, TFetch>>(nameof (query), query);
      ArgumentUtility.CheckNotNull<Expression<Func<TFetch, TRelated>>>(nameof (relatedObjectSelector), relatedObjectSelector);
      return EagerFetchingExtensionMethods.CreateFluentFetchRequest<TQueried, TRelated>(((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TQueried), typeof (TFetch), typeof (TRelated)), (IQueryable<TQueried>) query, (LambdaExpression) relatedObjectSelector);
    }

    public static INhFetchRequest<TQueried, TRelated> ThenFetchMany<TQueried, TFetch, TRelated>(
      this INhFetchRequest<TQueried, TFetch> query,
      Expression<Func<TFetch, IEnumerable<TRelated>>> relatedObjectSelector)
    {
      ArgumentUtility.CheckNotNull<INhFetchRequest<TQueried, TFetch>>(nameof (query), query);
      ArgumentUtility.CheckNotNull<Expression<Func<TFetch, IEnumerable<TRelated>>>>(nameof (relatedObjectSelector), relatedObjectSelector);
      return EagerFetchingExtensionMethods.CreateFluentFetchRequest<TQueried, TRelated>(((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TQueried), typeof (TFetch), typeof (TRelated)), (IQueryable<TQueried>) query, (LambdaExpression) relatedObjectSelector);
    }

    private static INhFetchRequest<TOriginating, TRelated> CreateFluentFetchRequest<TOriginating, TRelated>(
      MethodInfo currentFetchMethod,
      IQueryable<TOriginating> query,
      LambdaExpression relatedObjectSelector)
    {
      return (INhFetchRequest<TOriginating, TRelated>) new NhFetchRequest<TOriginating, TRelated>(query.Provider, (Expression) Expression.Call(currentFetchMethod, query.Expression, (Expression) relatedObjectSelector));
    }
  }
}
