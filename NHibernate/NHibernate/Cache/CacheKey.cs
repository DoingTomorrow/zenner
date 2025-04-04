// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.CacheKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Cache
{
  [Serializable]
  public class CacheKey
  {
    private readonly object key;
    private readonly IType type;
    private readonly string entityOrRoleName;
    private readonly int hashCode;
    private readonly EntityMode entityMode;

    public CacheKey(
      object id,
      IType type,
      string entityOrRoleName,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      this.key = id;
      this.type = type;
      this.entityOrRoleName = entityOrRoleName;
      this.entityMode = entityMode;
      this.hashCode = type.GetHashCode(this.key, entityMode, factory);
    }

    public override string ToString() => this.entityOrRoleName + (object) '#' + this.key;

    public override bool Equals(object obj)
    {
      return obj is CacheKey cacheKey && this.entityOrRoleName.Equals(cacheKey.entityOrRoleName) && this.type.IsEqual(this.key, cacheKey.key, this.entityMode);
    }

    public override int GetHashCode() => this.hashCode;

    public object Key => this.key;

    public string EntityOrRoleName => this.entityOrRoleName;
  }
}
