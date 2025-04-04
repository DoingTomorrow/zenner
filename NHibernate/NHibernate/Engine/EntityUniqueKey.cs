// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.EntityUniqueKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class EntityUniqueKey
  {
    private readonly string entityName;
    private readonly string uniqueKeyName;
    private readonly object key;
    private readonly IType keyType;
    private readonly int hashCode;
    private readonly EntityMode entityMode;

    public EntityUniqueKey(
      string entityName,
      string uniqueKeyName,
      object semiResolvedKey,
      IType keyType,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      if (string.IsNullOrEmpty(entityName))
        throw new ArgumentNullException(nameof (entityName));
      if (string.IsNullOrEmpty(uniqueKeyName))
        throw new ArgumentNullException(nameof (entityName));
      if (semiResolvedKey == null)
        throw new ArgumentNullException(nameof (semiResolvedKey));
      if (keyType == null)
        throw new ArgumentNullException(nameof (keyType));
      this.entityName = entityName;
      this.uniqueKeyName = uniqueKeyName;
      this.key = semiResolvedKey;
      this.keyType = keyType.GetSemiResolvedType(factory);
      this.entityMode = entityMode;
      this.hashCode = this.GenerateHashCode(factory);
    }

    public int GenerateHashCode(ISessionFactoryImplementor factory)
    {
      return 37 * (37 * (37 * 17 + this.entityName.GetHashCode()) + this.uniqueKeyName.GetHashCode()) + this.keyType.GetHashCode(this.key, this.entityMode, factory);
    }

    public string EntityName => this.entityName;

    public object Key => this.key;

    public string UniqueKeyName => this.uniqueKeyName;

    public override int GetHashCode() => this.hashCode;

    public override bool Equals(object obj)
    {
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as EntityUniqueKey);
    }

    public bool Equals(EntityUniqueKey that)
    {
      return that != null && that.EntityName.Equals(this.entityName) && that.UniqueKeyName.Equals(this.uniqueKeyName) && this.keyType.IsEqual(that.key, this.key, this.entityMode);
    }

    public override string ToString()
    {
      return string.Format("EntityUniqueKey{0}", (object) MessageHelper.InfoString(this.entityName, this.uniqueKeyName, this.key));
    }
  }
}
