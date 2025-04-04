// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.NamedQueryCollectionInitializer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader.Collection;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public class NamedQueryCollectionInitializer : ICollectionInitializer
  {
    private readonly string queryName;
    private readonly ICollectionPersister persister;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (NamedQueryCollectionInitializer));

    public NamedQueryCollectionInitializer(string queryName, ICollectionPersister persister)
    {
      this.queryName = queryName;
      this.persister = persister;
    }

    public void Initialize(object key, ISessionImplementor session)
    {
      if (NamedQueryCollectionInitializer.log.IsDebugEnabled)
        NamedQueryCollectionInitializer.log.Debug((object) string.Format("initializing collection: {0} using named query: {1}", (object) this.persister.Role, (object) this.queryName));
      AbstractQueryImpl namedSqlQuery = (AbstractQueryImpl) session.GetNamedSQLQuery(this.queryName);
      if (namedSqlQuery.NamedParameters.Length > 0)
        namedSqlQuery.SetParameter(namedSqlQuery.NamedParameters[0], key, this.persister.KeyType);
      else
        namedSqlQuery.SetParameter(0, key, this.persister.KeyType);
      namedSqlQuery.SetCollectionKey(key).SetFlushMode(FlushMode.Never).List();
    }
  }
}
