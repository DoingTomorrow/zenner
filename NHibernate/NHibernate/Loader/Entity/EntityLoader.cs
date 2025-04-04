// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Entity.EntityLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Entity
{
  public class EntityLoader : AbstractEntityLoader
  {
    private readonly bool batchLoader;

    public EntityLoader(
      IOuterJoinLoadable persister,
      LockMode lockMode,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : this(persister, 1, lockMode, factory, enabledFilters)
    {
    }

    public EntityLoader(
      IOuterJoinLoadable persister,
      int batchSize,
      LockMode lockMode,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : this(persister, persister.IdentifierColumnNames, persister.IdentifierType, batchSize, lockMode, factory, enabledFilters)
    {
    }

    public EntityLoader(
      IOuterJoinLoadable persister,
      string[] uniqueKey,
      IType uniqueKeyType,
      int batchSize,
      LockMode lockMode,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(persister, uniqueKeyType, factory, enabledFilters)
    {
      this.InitFromWalker((JoinWalker) new EntityJoinWalker(persister, uniqueKey, batchSize, lockMode, factory, enabledFilters));
      this.PostInstantiate();
      this.batchLoader = batchSize > 1;
      AbstractEntityLoader.log.Debug((object) ("Static select for entity " + this.entityName + ": " + (object) this.SqlString));
    }

    public object LoadByUniqueKey(ISessionImplementor session, object key)
    {
      return this.Load(session, key, (object) null, (object) null);
    }

    protected override bool IsSingleRowLoader => !this.batchLoader;
  }
}
