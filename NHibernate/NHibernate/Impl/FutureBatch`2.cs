// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.FutureBatch`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Impl
{
  public abstract class FutureBatch<TQueryApproach, TMultiApproach>
  {
    private readonly List<TQueryApproach> queries = new List<TQueryApproach>();
    private readonly IList<Type> resultTypes = (IList<Type>) new List<Type>();
    private int index;
    private IList results;
    private bool isCacheable = true;
    private string cacheRegion;
    protected readonly SessionImpl session;

    protected FutureBatch(SessionImpl session) => this.session = session;

    public IList Results
    {
      get
      {
        if (this.results == null)
          this.GetResults();
        return this.results;
      }
    }

    public void Add<TResult>(TQueryApproach query)
    {
      if (this.queries.Count == 0)
        this.cacheRegion = this.CacheRegion(query);
      this.queries.Add(query);
      this.resultTypes.Add(typeof (TResult));
      this.index = this.queries.Count - 1;
      this.isCacheable = this.isCacheable && this.IsQueryCacheable(query);
      this.isCacheable = this.isCacheable && this.cacheRegion == this.CacheRegion(query);
    }

    public void Add(TQueryApproach query) => this.Add<object>(query);

    public IFutureValue<TResult> GetFutureValue<TResult>()
    {
      int currentIndex = this.index;
      return (IFutureValue<TResult>) new FutureValue<TResult>((FutureValue<TResult>.GetResult) (() => this.GetCurrentResult<TResult>(currentIndex)));
    }

    public IEnumerable<TResult> GetEnumerator<TResult>()
    {
      int currentIndex = this.index;
      return (IEnumerable<TResult>) new DelayedEnumerator<TResult>((DelayedEnumerator<TResult>.GetResult) (() => this.GetCurrentResult<TResult>(currentIndex)));
    }

    private void GetResults()
    {
      TMultiApproach multiApproach = this.CreateMultiApproach(this.isCacheable, this.cacheRegion);
      for (int index = 0; index < this.queries.Count; ++index)
        this.AddTo(multiApproach, this.queries[index], this.resultTypes[index]);
      this.results = this.GetResultsFrom(multiApproach);
      this.ClearCurrentFutureBatch();
    }

    private IEnumerable<TResult> GetCurrentResult<TResult>(int currentIndex)
    {
      return ((IEnumerable) this.Results[currentIndex]).Cast<TResult>();
    }

    protected abstract TMultiApproach CreateMultiApproach(bool isCacheable, string cacheRegion);

    protected abstract void AddTo(
      TMultiApproach multiApproach,
      TQueryApproach query,
      Type resultType);

    protected abstract IList GetResultsFrom(TMultiApproach multiApproach);

    protected abstract void ClearCurrentFutureBatch();

    protected abstract bool IsQueryCacheable(TQueryApproach query);

    protected abstract string CacheRegion(TQueryApproach query);
  }
}
