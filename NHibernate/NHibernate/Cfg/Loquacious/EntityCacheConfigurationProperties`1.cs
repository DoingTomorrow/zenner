// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.EntityCacheConfigurationProperties`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class EntityCacheConfigurationProperties<TEntity> : 
    IEntityCacheConfigurationProperties<TEntity>
    where TEntity : class
  {
    private readonly Dictionary<string, IEntityCollectionCacheConfigurationProperties> collections;

    public EntityCacheConfigurationProperties()
    {
      this.collections = new Dictionary<string, IEntityCollectionCacheConfigurationProperties>(10);
      this.Strategy = new EntityCacheUsage?();
    }

    public EntityCacheUsage? Strategy { set; get; }

    public string RegionName { set; get; }

    public void Collection<TCollection>(
      Expression<Func<TEntity, TCollection>> collectionProperty,
      Action<IEntityCollectionCacheConfigurationProperties> collectionCacheConfiguration)
      where TCollection : IEnumerable
    {
      MemberInfo memberInfo = collectionProperty != null ? ExpressionsHelper.DecodeMemberAccessExpression<TEntity, TCollection>(collectionProperty) : throw new ArgumentNullException(nameof (collectionProperty));
      if (memberInfo.DeclaringType != typeof (TEntity))
        throw new ArgumentOutOfRangeException(nameof (collectionProperty), "Collection not owned by " + typeof (TEntity).FullName);
      EntityCollectionCacheConfigurationProperties configurationProperties = new EntityCollectionCacheConfigurationProperties();
      collectionCacheConfiguration((IEntityCollectionCacheConfigurationProperties) configurationProperties);
      this.collections.Add(typeof (TEntity).FullName + "." + memberInfo.Name, (IEntityCollectionCacheConfigurationProperties) configurationProperties);
    }

    public IDictionary<string, IEntityCollectionCacheConfigurationProperties> Collections
    {
      get => (IDictionary<string, IEntityCollectionCacheConfigurationProperties>) this.collections;
    }
  }
}
