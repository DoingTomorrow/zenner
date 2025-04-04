// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.JoinCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class JoinCustomizer<TEntity> : 
    PropertyContainerCustomizer<TEntity>,
    IJoinMapper<TEntity>,
    IJoinAttributesMapper<TEntity>,
    IEntitySqlsMapper,
    ICollectionPropertiesContainerMapper<TEntity>,
    IBasePlainPropertyContainerMapper<TEntity>,
    IMinimalPlainPropertyContainerMapper<TEntity>
    where TEntity : class
  {
    private readonly string splitGroupId;
    private readonly IKeyMapper<TEntity> keyMapper;

    public JoinCustomizer(
      string splitGroupId,
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder)
      : base(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      this.splitGroupId = splitGroupId;
      this.keyMapper = (IKeyMapper<TEntity>) new JoinKeyCustomizer<TEntity>(customizersHolder);
    }

    public void Loader(string namedQueryReference)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Loader(namedQueryReference)));
    }

    public void SqlInsert(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.SqlInsert(sql)));
    }

    public void SqlUpdate(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.SqlUpdate(sql)));
    }

    public void SqlDelete(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.SqlDelete(sql)));
    }

    public void Subselect(string sql)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Subselect(sql)));
    }

    public void Table(string tableName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Table(tableName)));
    }

    public void Catalog(string catalogName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Catalog(catalogName)));
    }

    public void Schema(string schemaName)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Schema(schemaName)));
    }

    public void Key(Action<IKeyMapper<TEntity>> keyMapping) => keyMapping(this.keyMapper);

    public void Inverse(bool value)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Inverse(value)));
    }

    public void Optional(bool isOptional)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Optional(isOptional)));
    }

    public void Fetch(FetchKind fetchMode)
    {
      this.CustomizersHolder.AddCustomizer(typeof (TEntity), (Action<IJoinAttributesMapper>) (m => m.Fetch(fetchMode)));
    }

    protected override void RegisterSetMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property)));
      base.RegisterSetMapping<TElement>(property, collectionMapping, mapping);
    }

    protected override void RegisterBagMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property)));
      base.RegisterBagMapping<TElement>(property, collectionMapping, mapping);
    }

    protected override void RegisterListMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property)));
      base.RegisterListMapping<TElement>(property, collectionMapping, mapping);
    }

    protected override void RegisterMapMapping<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<IMapKeyRelation<TKey>> keyMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, IDictionary<TKey, TElement>>(property)));
      base.RegisterMapMapping<TKey, TElement>(property, collectionMapping, keyMapping, mapping);
    }

    protected override void RegisterPropertyMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IPropertyMapper> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property)));
      base.RegisterPropertyMapping<TProperty>(property, mapping);
    }

    protected override void RegisterNoVisiblePropertyMapping(
      string notVisiblePropertyOrFieldName,
      Action<IPropertyMapper> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, typeof (TEntity).GetPropertyOrFieldMatchingName(notVisiblePropertyOrFieldName)));
      base.RegisterNoVisiblePropertyMapping(notVisiblePropertyOrFieldName, mapping);
    }

    protected override void RegisterComponentMapping<TComponent>(
      Expression<Func<TEntity, TComponent>> property,
      Action<IComponentMapper<TComponent>> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, TComponent>(property)));
      base.RegisterComponentMapping<TComponent>(property, mapping);
    }

    protected override void RegisterManyToOneMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IManyToOneMapper> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property)));
      base.RegisterManyToOneMapping<TProperty>(property, mapping);
    }

    protected override void RegisterAnyMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Type idTypeOfMetaType,
      Action<IAnyMapper> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property)));
      base.RegisterAnyMapping<TProperty>(property, idTypeOfMetaType, mapping);
    }

    protected override void RegisterIdBagMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPropertySplit(new SplitDefinition(typeof (TEntity), this.splitGroupId, TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property)));
      base.RegisterIdBagMapping<TElement>(property, collectionMapping, mapping);
    }
  }
}
