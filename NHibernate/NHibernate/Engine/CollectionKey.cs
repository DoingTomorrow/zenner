// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.CollectionKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public sealed class CollectionKey
  {
    private readonly string role;
    private readonly object key;
    private readonly IType keyType;
    [NonSerialized]
    private readonly ISessionFactoryImplementor factory;
    private readonly int hashCode;
    private readonly EntityMode entityMode;

    public CollectionKey(ICollectionPersister persister, object key, EntityMode entityMode)
      : this(persister.Role, key, persister.KeyType, entityMode, persister.Factory)
    {
    }

    private CollectionKey(
      string role,
      object key,
      IType keyType,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      this.role = role;
      this.key = key;
      this.keyType = keyType;
      this.entityMode = entityMode;
      this.factory = factory;
      this.hashCode = this.GenerateHashCode();
    }

    public override bool Equals(object obj)
    {
      CollectionKey collectionKey = (CollectionKey) obj;
      return collectionKey.role.Equals(this.role) && this.keyType.IsEqual(collectionKey.key, this.key, this.entityMode, this.factory);
    }

    public override int GetHashCode() => this.hashCode;

    private int GenerateHashCode()
    {
      return 37 * (37 * 17 + this.role.GetHashCode()) + this.keyType.GetHashCode(this.key, this.entityMode, this.factory);
    }

    public string Role => this.role;

    public object Key => this.key;

    public override string ToString()
    {
      return nameof (CollectionKey) + MessageHelper.InfoString(this.factory.GetCollectionPersister(this.role), this.key, this.factory);
    }
  }
}
