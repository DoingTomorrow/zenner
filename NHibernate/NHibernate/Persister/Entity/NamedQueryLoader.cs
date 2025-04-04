// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.NamedQueryLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader.Entity;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public class NamedQueryLoader : IUniqueEntityLoader
  {
    private readonly string queryName;
    private readonly IEntityPersister persister;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (NamedQueryLoader));

    public NamedQueryLoader(string queryName, IEntityPersister persister)
    {
      this.queryName = queryName;
      this.persister = persister;
    }

    public object Load(object id, object optionalObject, ISessionImplementor session)
    {
      if (NamedQueryLoader.log.IsDebugEnabled)
        NamedQueryLoader.log.Debug((object) string.Format("loading entity: {0} using named query: {1}", (object) this.persister.EntityName, (object) this.queryName));
      AbstractQueryImpl namedQuery = (AbstractQueryImpl) session.GetNamedQuery(this.queryName);
      if (namedQuery.HasNamedParameters)
        namedQuery.SetParameter(namedQuery.NamedParameters[0], id, this.persister.IdentifierType);
      else
        namedQuery.SetParameter(0, id, this.persister.IdentifierType);
      namedQuery.SetOptionalId(id);
      namedQuery.SetOptionalEntityName(this.persister.EntityName);
      namedQuery.SetOptionalObject(optionalObject);
      namedQuery.SetFlushMode(FlushMode.Never);
      namedQuery.List();
      return session.PersistenceContext.GetEntity(new EntityKey(id, this.persister, session.EntityMode));
    }
  }
}
