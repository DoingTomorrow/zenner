// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.FutureQueryBatch
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Impl
{
  public class FutureQueryBatch(SessionImpl session) : FutureBatch<IQuery, IMultiQuery>(session)
  {
    protected override IMultiQuery CreateMultiApproach(bool isCacheable, string cacheRegion)
    {
      return this.session.CreateMultiQuery().SetCacheable(isCacheable).SetCacheRegion(cacheRegion);
    }

    protected override void AddTo(IMultiQuery multiApproach, IQuery query, Type resultType)
    {
      multiApproach.Add(resultType, query);
    }

    protected override IList GetResultsFrom(IMultiQuery multiApproach) => multiApproach.List();

    protected override void ClearCurrentFutureBatch()
    {
      this.session.FutureQueryBatch = (FutureQueryBatch) null;
    }

    protected override bool IsQueryCacheable(IQuery query) => ((AbstractQueryImpl) query).Cacheable;

    protected override string CacheRegion(IQuery query) => ((AbstractQueryImpl) query).CacheRegion;
  }
}
