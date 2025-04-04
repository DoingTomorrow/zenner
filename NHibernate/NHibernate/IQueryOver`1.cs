// Decompiled with JetBrains decompiler
// Type: NHibernate.IQueryOver`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate
{
  public interface IQueryOver<TRoot> : IQueryOver
  {
    IList<TRoot> List();

    IList<U> List<U>();

    IQueryOver<TRoot, TRoot> ToRowCountQuery();

    IQueryOver<TRoot, TRoot> ToRowCountInt64Query();

    int RowCount();

    long RowCountInt64();

    TRoot SingleOrDefault();

    U SingleOrDefault<U>();

    IEnumerable<TRoot> Future();

    IEnumerable<U> Future<U>();

    IFutureValue<TRoot> FutureValue();

    IFutureValue<U> FutureValue<U>();

    IQueryOver<TRoot, TRoot> Clone();

    IQueryOver<TRoot> ClearOrders();

    IQueryOver<TRoot> Skip(int firstResult);

    IQueryOver<TRoot> Take(int maxResults);

    IQueryOver<TRoot> Cacheable();

    IQueryOver<TRoot> CacheMode(NHibernate.CacheMode cacheMode);

    IQueryOver<TRoot> CacheRegion(string cacheRegion);

    IQueryOver<TRoot> ReadOnly();
  }
}
