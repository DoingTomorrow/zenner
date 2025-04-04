// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.DetachedNamedQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public class DetachedNamedQuery : AbstractDetachedQuery
  {
    private readonly string queryName;
    private bool cacheableWasSet;
    private bool cacheModeWasSet;
    private bool cacheRegionWasSet;
    private bool readOnlyWasSet;
    private bool timeoutWasSet;
    private bool fetchSizeWasSet;
    private bool commentWasSet;
    private bool flushModeWasSet;

    public DetachedNamedQuery(string queryName) => this.queryName = queryName;

    public string QueryName => this.queryName;

    public override IQuery GetExecutableQuery(ISession session)
    {
      IQuery namedQuery = session.GetNamedQuery(this.queryName);
      this.SetDefaultProperties((ISessionFactoryImplementor) session.SessionFactory);
      this.SetQueryProperties(namedQuery);
      return namedQuery;
    }

    private void SetDefaultProperties(ISessionFactoryImplementor factory)
    {
      NamedQueryDefinition namedQueryDefinition = factory.GetNamedQuery(this.queryName) ?? (NamedQueryDefinition) factory.GetNamedSQLQuery(this.queryName);
      if (!this.cacheableWasSet)
        this.cacheable = namedQueryDefinition.IsCacheable;
      if (!this.cacheRegionWasSet)
        this.cacheRegion = namedQueryDefinition.CacheRegion;
      if (!this.timeoutWasSet && namedQueryDefinition.Timeout != -1)
        this.selection.Timeout = namedQueryDefinition.Timeout;
      if (!this.fetchSizeWasSet && namedQueryDefinition.FetchSize != -1)
        this.selection.FetchSize = namedQueryDefinition.FetchSize;
      if (!this.cacheModeWasSet && namedQueryDefinition.CacheMode.HasValue)
        this.cacheMode = new CacheMode?(namedQueryDefinition.CacheMode.Value);
      if (!this.readOnlyWasSet)
        this.readOnly = namedQueryDefinition.IsReadOnly;
      if (!this.commentWasSet && namedQueryDefinition.Comment != null)
        this.comment = namedQueryDefinition.Comment;
      if (this.flushModeWasSet)
        return;
      this.flushMode = namedQueryDefinition.FlushMode;
    }

    public DetachedNamedQuery Clone()
    {
      DetachedNamedQuery destination = new DetachedNamedQuery(this.queryName);
      this.CopyTo((IDetachedQuery) destination);
      return destination;
    }

    public override IDetachedQuery SetCacheable(bool cacheable)
    {
      this.cacheableWasSet = true;
      return base.SetCacheable(cacheable);
    }

    public override IDetachedQuery SetCacheMode(CacheMode cacheMode)
    {
      this.cacheModeWasSet = true;
      return base.SetCacheMode(cacheMode);
    }

    public override IDetachedQuery SetCacheRegion(string cacheRegion)
    {
      this.cacheRegionWasSet = true;
      return base.SetCacheRegion(cacheRegion);
    }

    public override IDetachedQuery SetReadOnly(bool readOnly)
    {
      this.readOnlyWasSet = true;
      return base.SetReadOnly(readOnly);
    }

    public override IDetachedQuery SetTimeout(int timeout)
    {
      this.timeoutWasSet = true;
      return base.SetTimeout(timeout);
    }

    public override IDetachedQuery SetFetchSize(int fetchSize)
    {
      this.fetchSizeWasSet = true;
      return base.SetFetchSize(fetchSize);
    }

    public override IDetachedQuery SetComment(string comment)
    {
      this.commentWasSet = true;
      return base.SetComment(comment);
    }

    public override IDetachedQuery SetFlushMode(FlushMode flushMode)
    {
      this.flushModeWasSet = true;
      return base.SetFlushMode(flushMode);
    }
  }
}
