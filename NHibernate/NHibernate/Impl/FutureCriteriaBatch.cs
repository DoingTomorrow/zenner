// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.FutureCriteriaBatch
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Impl
{
  public class FutureCriteriaBatch(SessionImpl session) : FutureBatch<ICriteria, IMultiCriteria>(session)
  {
    protected override IMultiCriteria CreateMultiApproach(bool isCacheable, string cacheRegion)
    {
      return this.session.CreateMultiCriteria().SetCacheable(isCacheable).SetCacheRegion(cacheRegion);
    }

    protected override void AddTo(IMultiCriteria multiApproach, ICriteria query, Type resultType)
    {
      multiApproach.Add(resultType, query);
    }

    protected override IList GetResultsFrom(IMultiCriteria multiApproach) => multiApproach.List();

    protected override void ClearCurrentFutureBatch()
    {
      this.session.FutureCriteriaBatch = (FutureCriteriaBatch) null;
    }

    protected override bool IsQueryCacheable(ICriteria query) => ((CriteriaImpl) query).Cacheable;

    protected override string CacheRegion(ICriteria query) => ((CriteriaImpl) query).CacheRegion;
  }
}
