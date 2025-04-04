// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.EntityKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public sealed class EntityKey
  {
    private readonly object identifier;
    private readonly string rootEntityName;
    private readonly string entityName;
    private readonly IType identifierType;
    private readonly bool isBatchLoadable;
    [NonSerialized]
    private ISessionFactoryImplementor factory;
    private int hashCode;
    private readonly EntityMode entityMode;

    public EntityKey(object id, IEntityPersister persister, EntityMode entityMode)
      : this(id, persister.RootEntityName, persister.EntityName, persister.IdentifierType, persister.IsBatchLoadable, persister.Factory, entityMode)
    {
    }

    private EntityKey(
      object identifier,
      string rootEntityName,
      string entityName,
      IType identifierType,
      bool batchLoadable,
      ISessionFactoryImplementor factory,
      EntityMode entityMode)
    {
      this.identifier = identifier != null ? identifier : throw new AssertionFailure("null identifier");
      this.rootEntityName = rootEntityName;
      this.entityName = entityName;
      this.identifierType = identifierType;
      this.isBatchLoadable = batchLoadable;
      this.factory = factory;
      this.entityMode = entityMode;
      this.hashCode = this.GenerateHashCode();
    }

    public bool IsBatchLoadable => this.isBatchLoadable;

    public object Identifier => this.identifier;

    public string EntityName => this.entityName;

    public override bool Equals(object other)
    {
      return other is EntityKey entityKey && entityKey.rootEntityName.Equals(this.rootEntityName) && this.identifierType.IsEqual(entityKey.Identifier, this.Identifier, this.entityMode, this.factory);
    }

    private int GenerateHashCode()
    {
      return 37 * (37 * 17 + this.rootEntityName.GetHashCode()) + this.identifierType.GetHashCode(this.identifier, this.entityMode, this.factory);
    }

    public override int GetHashCode() => this.hashCode;

    public override string ToString()
    {
      return nameof (EntityKey) + MessageHelper.InfoString(this.factory.GetEntityPersister(this.EntityName), this.Identifier, this.factory);
    }

    internal void SetSessionFactory(ISessionFactoryImplementor sessionFactory)
    {
      this.factory = sessionFactory;
      this.hashCode = this.GetHashCode();
    }
  }
}
