// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.ClassCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class ClassCustomizer<TEntity> : 
    PropertyContainerCustomizer<TEntity>,
    IClassMapper<TEntity>,
    IClassAttributesMapper<TEntity>,
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

    public ClassCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder)
      : base(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsRootEntity(typeof (TEntity));
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => { }));
    }

    private Dictionary<string, IJoinMapper<TEntity>> JoinCustomizers
    {
      get
      {
        return this.joinCustomizers ?? (this.joinCustomizers = new Dictionary<string, IJoinMapper<TEntity>>());
      }
    }

    public void Id<TProperty>(Expression<Func<TEntity, TProperty>> idProperty)
    {
      this.Id<TProperty>(idProperty, (Action<IIdMapper>) (x => { }));
    }

    public void Id<TProperty>(
      Expression<Func<TEntity, TProperty>> idProperty,
      Action<IIdMapper> idMapper)
    {
      MemberInfo member = (MemberInfo) null;
      if (idProperty != null)
      {
        member = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(idProperty);
        this.ExplicitDeclarationsHolder.AddAsPoid(member);
      }
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Id(member, idMapper)));
    }

    public void Id(string notVisiblePropertyOrFieldName, Action<IIdMapper> idMapper)
    {
      MemberInfo member = (MemberInfo) null;
      if (notVisiblePropertyOrFieldName != null)
      {
        member = typeof (TEntity).GetPropertyOrFieldMatchingName(notVisiblePropertyOrFieldName);
        this.ExplicitDeclarationsHolder.AddAsPoid(member);
      }
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Id(member, idMapper)));
    }

    public void ComponentAsId<TComponent>(Expression<Func<TEntity, TComponent>> idProperty) where TComponent : class
    {
      this.ComponentAsId<TComponent>(idProperty, (Action<IComponentAsIdMapper<TComponent>>) (x => { }));
    }

    public void ComponentAsId<TComponent>(
      Expression<Func<TEntity, TComponent>> idProperty,
      Action<IComponentAsIdMapper<TComponent>> idMapper)
      where TComponent : class
    {
      PropertyPath propertyPath = new PropertyPath((PropertyPath) null, TypeExtensions.DecodeMemberAccessExpression<TEntity, TComponent>(idProperty));
      idMapper((IComponentAsIdMapper<TComponent>) new ComponentAsIdCustomizer<TComponent>(this.ExplicitDeclarationsHolder, this.CustomizersHolder, propertyPath));
    }

    public void ComponentAsId<TComponent>(string notVisiblePropertyOrFieldName) where TComponent : class
    {
      this.ComponentAsId<TComponent>(notVisiblePropertyOrFieldName, (Action<IComponentAsIdMapper<TComponent>>) (x => { }));
    }

    public void ComponentAsId<TComponent>(
      string notVisiblePropertyOrFieldName,
      Action<IComponentAsIdMapper<TComponent>> idMapper)
      where TComponent : class
    {
      PropertyPath propertyPath = new PropertyPath((PropertyPath) null, typeof (TEntity).GetPropertyOrFieldMatchingName(notVisiblePropertyOrFieldName));
      idMapper((IComponentAsIdMapper<TComponent>) new ComponentAsIdCustomizer<TComponent>(this.ExplicitDeclarationsHolder, this.CustomizersHolder, propertyPath));
    }

    public void ComposedId(
      Action<IComposedIdMapper<TEntity>> idPropertiesMapping)
    {
      idPropertiesMapping((IComposedIdMapper<TEntity>) new ComposedIdCustomizer<TEntity>(this.ExplicitDeclarationsHolder, this.CustomizersHolder));
    }

    public void Discriminator(Action<IDiscriminatorMapper> discriminatorMapping)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Discriminator(discriminatorMapping)));
    }

    public void DiscriminatorValue(object value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.DiscriminatorValue(value)));
    }

    public void Table(string tableName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Table(tableName)));
    }

    public void Catalog(string catalogName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Catalog(catalogName)));
    }

    public void Schema(string schemaName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Schema(schemaName)));
    }

    public void Mutable(bool isMutable)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Mutable(isMutable)));
    }

    public void Version<TProperty>(
      Expression<Func<TEntity, TProperty>> versionProperty,
      Action<IVersionMapper> versionMapping)
    {
      MemberInfo member = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(versionProperty);
      this.ExplicitDeclarationsHolder.AddAsVersionProperty(member);
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Version(member, versionMapping)));
    }

    public void Version(string notVisiblePropertyOrFieldName, Action<IVersionMapper> versionMapping)
    {
      MemberInfo member = typeof (TEntity).GetPropertyOrFieldMatchingName(notVisiblePropertyOrFieldName);
      this.ExplicitDeclarationsHolder.AddAsVersionProperty(member);
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Version(member, versionMapping)));
    }

    public void NaturalId(
      Action<IBasePlainPropertyContainerMapper<TEntity>> naturalIdPropertiesMapping,
      Action<INaturalIdAttributesMapper> naturalIdMapping)
    {
      naturalIdPropertiesMapping((IBasePlainPropertyContainerMapper<TEntity>) new NaturalIdCustomizer<TEntity>(this.ExplicitDeclarationsHolder, this.CustomizersHolder));
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.NaturalId((Action<INaturalIdMapper>) (nidm => naturalIdMapping((INaturalIdAttributesMapper) nidm)))));
    }

    public void NaturalId(
      Action<IBasePlainPropertyContainerMapper<TEntity>> naturalIdPropertiesMapping)
    {
      this.NaturalId(naturalIdPropertiesMapping, (Action<INaturalIdAttributesMapper>) (mapAttr => { }));
    }

    public void Cache(Action<ICacheMapper> cacheMapping)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Cache(cacheMapping)));
    }

    public void Filter(string filterName, Action<IFilterMapper> filterMapping)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Filter(filterName, filterMapping)));
    }

    public void Where(string whereClause)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Where(whereClause)));
    }

    public void SchemaAction(NHibernate.Mapping.ByCode.SchemaAction action)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.SchemaAction(action)));
    }

    public void Join(string splitGroupId, Action<IJoinMapper<TEntity>> splitMapping)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Join(splitGroupId, (Action<IJoinMapper>) (j => { }))));
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
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.EntityName(value)));
    }

    public void Proxy(Type proxy)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Proxy(proxy)));
    }

    public void Lazy(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Lazy(value)));
    }

    public void DynamicUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.DynamicUpdate(value)));
    }

    public void DynamicInsert(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.DynamicInsert(value)));
    }

    public void BatchSize(int value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.BatchSize(value)));
    }

    public void SelectBeforeUpdate(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.SelectBeforeUpdate(value)));
    }

    public void Persister<T>() where T : IEntityPersister
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Persister<T>()));
    }

    public void Synchronize(params string[] table)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Synchronize(table)));
    }

    public void Loader(string namedQueryReference)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Loader(namedQueryReference)));
    }

    public void SqlInsert(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.SqlInsert(sql)));
    }

    public void SqlUpdate(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.SqlUpdate(sql)));
    }

    public void SqlDelete(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.SqlDelete(sql)));
    }

    public void Subselect(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IClassMapper>) (m => m.Subselect(sql)));
    }

    ICustomizersHolder IConformistHoldersProvider.CustomizersHolder => this.CustomizersHolder;

    IModelExplicitDeclarationsHolder IConformistHoldersProvider.ExplicitDeclarationsHolder
    {
      get => this.ExplicitDeclarationsHolder;
    }
  }
}
