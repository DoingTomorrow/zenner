// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.SubclassCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class SubclassCustomizer<TEntity> : 
    PropertyContainerCustomizer<TEntity>,
    ISubclassMapper<TEntity>,
    ISubclassAttributesMapper<TEntity>,
    IEntityAttributesMapper,
    IEntitySqlsMapper,
    IPropertyContainerMapper<TEntity>,
    ICollectionPropertiesContainerMapper<TEntity>,
    IPlainPropertyContainerMapper<TEntity>,
    IBasePlainPropertyContainerMapper<TEntity>,
    IMinimalPlainPropertyContainerMapper<TEntity>,
    IConformistHoldersProvider
    where TEntity : class
  {
    private Dictionary<string, IJoinMapper<TEntity>> joinCustomizers;

    public SubclassCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder)
      : base(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsTablePerClassHierarchyEntity(typeof (TEntity));
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => { }));
    }

    public void DiscriminatorValue(object value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.DiscriminatorValue(value)));
    }

    private Dictionary<string, IJoinMapper<TEntity>> JoinCustomizers
    {
      get
      {
        return this.joinCustomizers ?? (this.joinCustomizers = new Dictionary<string, IJoinMapper<TEntity>>());
      }
    }

    public void Join(string splitGroupId, Action<IJoinMapper<TEntity>> splitMapping)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.Join(splitGroupId, (Action<IJoinMapper>) (j => { }))));
      IJoinMapper<TEntity> joinMapper;
      if (!this.JoinCustomizers.TryGetValue(splitGroupId, out joinMapper))
      {
        joinMapper = (IJoinMapper<TEntity>) new JoinCustomizer<TEntity>(splitGroupId, this.ExplicitDeclarationsHolder, this.CustomizersHolder);
        this.JoinCustomizers.Add(splitGroupId, joinMapper);
      }
      splitMapping(joinMapper);
    }

    public void EntityName(string value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.EntityName(value)));
    }

    public void Proxy(Type proxy)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.Proxy(proxy)));
    }

    public void Lazy(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.Lazy(value)));
    }

    public void DynamicUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.DynamicUpdate(value)));
    }

    public void DynamicInsert(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.DynamicInsert(value)));
    }

    public void BatchSize(int value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.BatchSize(value)));
    }

    public void SelectBeforeUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.SelectBeforeUpdate(value)));
    }

    public void Persister<T>() where T : IEntityPersister
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.Persister<T>()));
    }

    public void Synchronize(params string[] table)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.Synchronize(table)));
    }

    public void Loader(string namedQueryReference)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.Loader(namedQueryReference)));
    }

    public void SqlInsert(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.SqlInsert(sql)));
    }

    public void SqlUpdate(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.SqlUpdate(sql)));
    }

    public void SqlDelete(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.SqlDelete(sql)));
    }

    public void Subselect(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<ISubclassMapper>) (m => m.Subselect(sql)));
    }

    ICustomizersHolder IConformistHoldersProvider.CustomizersHolder => this.CustomizersHolder;

    IModelExplicitDeclarationsHolder IConformistHoldersProvider.ExplicitDeclarationsHolder
    {
      get => this.ExplicitDeclarationsHolder;
    }
  }
}
