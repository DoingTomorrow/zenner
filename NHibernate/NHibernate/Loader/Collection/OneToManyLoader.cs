// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.OneToManyLoader
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
  public class OneToManyLoader : CollectionLoader
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (OneToManyLoader));

    public OneToManyLoader(
      IQueryableCollection oneToManyPersister,
      ISessionFactoryImplementor session,
      IDictionary<string, IFilter> enabledFilters)
      : this(oneToManyPersister, 1, session, enabledFilters)
    {
    }

    public OneToManyLoader(
      IQueryableCollection oneToManyPersister,
      int batchSize,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : this(oneToManyPersister, batchSize, (SqlString) null, factory, enabledFilters)
    {
    }

    public OneToManyLoader(
      IQueryableCollection oneToManyPersister,
      int batchSize,
      SqlString subquery,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(oneToManyPersister, factory, enabledFilters)
    {
      this.InitializeFromWalker(oneToManyPersister, subquery, batchSize, enabledFilters, factory);
    }

    protected virtual void InitializeFromWalker(
      IQueryableCollection oneToManyPersister,
      SqlString subquery,
      int batchSize,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
    {
      this.InitFromWalker((JoinWalker) new OneToManyJoinWalker(oneToManyPersister, batchSize, subquery, factory, enabledFilters));
      this.PostInstantiate();
      OneToManyLoader.log.Debug((object) ("Static select for one-to-many " + oneToManyPersister.Role + ": " + (object) this.SqlString));
    }
  }
}
