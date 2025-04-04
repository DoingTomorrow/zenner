// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.JoinedSubclassCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class JoinedSubclassCustomizer<TEntity> : 
    PropertyContainerCustomizer<TEntity>,
    IJoinedSubclassMapper<TEntity>,
    IJoinedSubclassAttributesMapper<TEntity>,
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
    private readonly IKeyMapper<TEntity> keyMapper;

    public JoinedSubclassCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder)
      : base(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsTablePerClassEntity(typeof (TEntity));
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => { }));
      this.keyMapper = (IKeyMapper<TEntity>) new JoinedSubclassKeyCustomizer<TEntity>(customizersHolder);
    }

    public void Key(Action<IKeyMapper<TEntity>> keyMapping) => keyMapping(this.keyMapper);

    public void SchemaAction(NHibernate.Mapping.ByCode.SchemaAction action)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.SchemaAction(action)));
    }

    public void EntityName(string value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.EntityName(value)));
    }

    public void Proxy(Type proxy)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Proxy(proxy)));
    }

    public void Lazy(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Lazy(value)));
    }

    public void DynamicUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.DynamicUpdate(value)));
    }

    public void DynamicInsert(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.DynamicInsert(value)));
    }

    public void BatchSize(int value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.BatchSize(value)));
    }

    public void SelectBeforeUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.SelectBeforeUpdate(value)));
    }

    public void Persister<T>() where T : IEntityPersister
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Persister<T>()));
    }

    public void Synchronize(params string[] table)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Synchronize(table)));
    }

    public void Loader(string namedQueryReference)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Loader(namedQueryReference)));
    }

    public void SqlInsert(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.SqlInsert(sql)));
    }

    public void SqlUpdate(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.SqlUpdate(sql)));
    }

    public void SqlDelete(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.SqlDelete(sql)));
    }

    public void Subselect(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Subselect(sql)));
    }

    public void Table(string tableName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Table(tableName)));
    }

    public void Catalog(string catalogName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Catalog(catalogName)));
    }

    public void Schema(string schemaName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinedSubclassAttributesMapper>) (m => m.Schema(schemaName)));
    }

    ICustomizersHolder IConformistHoldersProvider.CustomizersHolder => this.CustomizersHolder;

    IModelExplicitDeclarationsHolder IConformistHoldersProvider.ExplicitDeclarationsHolder
    {
      get => this.ExplicitDeclarationsHolder;
    }
  }
}
