// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.BasicCollectionLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.SqlCommand;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Collection
{
  public class BasicCollectionLoader : CollectionLoader
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (BasicCollectionLoader));

    public BasicCollectionLoader(
      IQueryableCollection collectionPersister,
      ISessionFactoryImplementor session,
      IDictionary<string, IFilter> enabledFilters)
      : this(collectionPersister, 1, session, enabledFilters)
    {
    }

    public BasicCollectionLoader(
      IQueryableCollection collectionPersister,
      int batchSize,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : this(collectionPersister, batchSize, (SqlString) null, factory, enabledFilters)
    {
    }

    protected BasicCollectionLoader(
      IQueryableCollection collectionPersister,
      int batchSize,
      SqlString subquery,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(collectionPersister, factory, enabledFilters)
    {
      this.InitializeFromWalker(collectionPersister, subquery, batchSize, enabledFilters, factory);
    }

    protected virtual void InitializeFromWalker(
      IQueryableCollection collectionPersister,
      SqlString subquery,
      int batchSize,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
    {
      this.InitFromWalker((JoinWalker) new BasicCollectionJoinWalker(collectionPersister, batchSize, subquery, factory, enabledFilters));
      this.PostInstantiate();
      BasicCollectionLoader.log.Debug((object) ("Static select for collection " + collectionPersister.Role + ": " + (object) this.SqlString));
    }
  }
}
