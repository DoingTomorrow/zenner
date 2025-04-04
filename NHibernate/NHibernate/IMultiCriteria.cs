// Decompiled with JetBrains decompiler
// Type: NHibernate.IMultiCriteria
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;

#nullable disable
namespace NHibernate
{
  public interface IMultiCriteria
  {
    IList List();

    IMultiCriteria Add(Type resultGenericListType, ICriteria criteria);

    IMultiCriteria Add<T>(ICriteria criteria);

    IMultiCriteria Add<T>(string key, ICriteria criteria);

    IMultiCriteria Add<T>(DetachedCriteria detachedCriteria);

    IMultiCriteria Add<T>(string key, DetachedCriteria detachedCriteria);

    IMultiCriteria Add(ICriteria criteria);

    IMultiCriteria Add(string key, ICriteria criteria);

    IMultiCriteria Add(DetachedCriteria detachedCriteria);

    IMultiCriteria Add(string key, DetachedCriteria detachedCriteria);

    IMultiCriteria Add(Type resultGenericListType, IQueryOver queryOver);

    IMultiCriteria Add<T>(IQueryOver<T> queryOver);

    IMultiCriteria Add<U>(IQueryOver queryOver);

    IMultiCriteria Add<T>(string key, IQueryOver<T> queryOver);

    IMultiCriteria Add<U>(string key, IQueryOver queryOver);

    IMultiCriteria SetCacheable(bool cachable);

    IMultiCriteria SetCacheRegion(string region);

    IMultiCriteria ForceCacheRefresh(bool forceRefresh);

    IMultiCriteria SetResultTransformer(IResultTransformer resultTransformer);

    object GetResult(string key);
  }
}
