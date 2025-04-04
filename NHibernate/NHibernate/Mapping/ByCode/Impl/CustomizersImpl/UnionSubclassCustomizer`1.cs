// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.UnionSubclassCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class UnionSubclassCustomizer<TEntity> : 
    PropertyContainerCustomizer<TEntity>,
    IUnionSubclassMapper<TEntity>,
    IUnionSubclassAttributesMapper<TEntity>,
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
    public UnionSubclassCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder)
      : base(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsTablePerConcreteClassEntity(typeof (TEntity));
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => { }));
    }

    public void EntityName(string value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.EntityName(value)));
    }

    public void Proxy(Type proxy)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Proxy(proxy)));
    }

    public void Lazy(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Lazy(value)));
    }

    public void DynamicUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.DynamicUpdate(value)));
    }

    public void DynamicInsert(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.DynamicInsert(value)));
    }

    public void BatchSize(int value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.BatchSize(value)));
    }

    public void SelectBeforeUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.SelectBeforeUpdate(value)));
    }

    public void Persister<T>() where T : IEntityPersister
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Persister<T>()));
    }

    public void Synchronize(params string[] table)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Synchronize(table)));
    }

    public void Loader(string namedQueryReference)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Loader(namedQueryReference)));
    }

    public void SqlInsert(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.SqlInsert(sql)));
    }

    public void SqlUpdate(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.SqlUpdate(sql)));
    }

    public void SqlDelete(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.SqlDelete(sql)));
    }

    public void Subselect(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Subselect(sql)));
    }

    public void Table(string tableName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Table(tableName)));
    }

    public void Catalog(string catalogName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Catalog(catalogName)));
    }

    public void Schema(string schemaName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IUnionSubclassAttributesMapper>) (m => m.Schema(schemaName)));
    }

    ICustomizersHolder IConformistHoldersProvider.CustomizersHolder => this.CustomizersHolder;

    IModelExplicitDeclarationsHolder IConformistHoldersProvider.ExplicitDeclarationsHolder
    {
      get => this.ExplicitDeclarationsHolder;
    }
  }
}
