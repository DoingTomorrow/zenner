// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.PropertyContainerCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class PropertyContainerCustomizer<TEntity> where TEntity : class
  {
    private readonly IModelExplicitDeclarationsHolder explicitDeclarationsHolder;

    public PropertyContainerCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder,
      PropertyPath propertyPath)
    {
      this.explicitDeclarationsHolder = explicitDeclarationsHolder != null ? explicitDeclarationsHolder : throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      this.CustomizersHolder = customizersHolder;
      this.PropertyPath = propertyPath;
    }

    protected internal ICustomizersHolder CustomizersHolder { get; private set; }

    protected internal PropertyPath PropertyPath { get; private set; }

    protected internal IModelExplicitDeclarationsHolder ExplicitDeclarationsHolder
    {
      get => this.explicitDeclarationsHolder;
    }

    public void Property<TProperty>(Expression<Func<TEntity, TProperty>> property)
    {
      this.Property<TProperty>(property, (Action<IPropertyMapper>) (x => { }));
    }

    public void Property<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IPropertyMapper> mapping)
    {
      this.RegisterPropertyMapping<TProperty>(property, mapping);
    }

    protected virtual void RegisterPropertyMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IPropertyMapper> mapping)
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property);
      this.RegistePropertyMapping(mapping, memberInfo1, memberInfo2);
    }

    public void Property(string notVisiblePropertyOrFieldName, Action<IPropertyMapper> mapping)
    {
      this.RegisterNoVisiblePropertyMapping(notVisiblePropertyOrFieldName, mapping);
    }

    protected virtual void RegisterNoVisiblePropertyMapping(
      string notVisiblePropertyOrFieldName,
      Action<IPropertyMapper> mapping)
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegistePropertyMapping(mapping, matchingNameOrThrow, fromReflectedType);
    }

    protected void RegistePropertyMapping(
      Action<IPropertyMapper> mapping,
      params MemberInfo[] members)
    {
      foreach (MemberInfo member in members)
      {
        this.CustomizersHolder.AddCustomizer(new PropertyPath(this.PropertyPath, member), mapping);
        this.explicitDeclarationsHolder.AddAsProperty(member);
      }
    }

    public void Component<TComponent>(
      Expression<Func<TEntity, TComponent>> property,
      Action<IComponentMapper<TComponent>> mapping)
      where TComponent : class
    {
      this.RegisterComponentMapping<TComponent>(property, mapping);
    }

    public void Component<TComponent>(Expression<Func<TEntity, TComponent>> property) where TComponent : class
    {
      this.RegisterComponentMapping<TComponent>(property, (Action<IComponentMapper<TComponent>>) (x => { }));
    }

    protected virtual void RegisterComponentMapping<TComponent>(
      Expression<Func<TEntity, TComponent>> property,
      Action<IComponentMapper<TComponent>> mapping)
      where TComponent : class
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TComponent>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TComponent>(property);
      this.RegisterComponentMapping<TComponent>(mapping, memberInfo1, memberInfo2);
    }

    protected void RegisterComponentMapping<TComponent>(
      Action<IComponentMapper<TComponent>> mapping,
      params MemberInfo[] members)
      where TComponent : class
    {
      foreach (MemberInfo member in members)
        mapping((IComponentMapper<TComponent>) new ComponentCustomizer<TComponent>(this.explicitDeclarationsHolder, this.CustomizersHolder, new PropertyPath(this.PropertyPath, member)));
    }

    public void Component<TComponent>(
      Expression<Func<TEntity, IDictionary>> property,
      TComponent dynamicComponentTemplate,
      Action<IDynamicComponentMapper<TComponent>> mapping)
      where TComponent : class
    {
      this.RegisterDynamicComponentMapping<TComponent>(property, mapping);
    }

    protected virtual void RegisterDynamicComponentMapping<TComponent>(
      Expression<Func<TEntity, IDictionary>> property,
      Action<IDynamicComponentMapper<TComponent>> mapping)
      where TComponent : class
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, IDictionary>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, IDictionary>(property);
      this.RegisterDynamicComponentMapping<TComponent>(mapping, memberInfo1, memberInfo2);
    }

    protected void RegisterDynamicComponentMapping<TComponent>(
      Action<IDynamicComponentMapper<TComponent>> mapping,
      params MemberInfo[] members)
      where TComponent : class
    {
      foreach (MemberInfo member in members)
        mapping((IDynamicComponentMapper<TComponent>) new DynamicComponentCustomizer<TComponent>(this.explicitDeclarationsHolder, this.CustomizersHolder, new PropertyPath(this.PropertyPath, member)));
    }

    public void ManyToOne<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IManyToOneMapper> mapping)
      where TProperty : class
    {
      this.RegisterManyToOneMapping<TProperty>(property, mapping);
    }

    protected virtual void RegisterManyToOneMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IManyToOneMapper> mapping)
      where TProperty : class
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property);
      this.RegisterManyToOneMapping<TProperty>(mapping, memberInfo1, memberInfo2);
    }

    protected void RegisterManyToOneMapping<TProperty>(
      Action<IManyToOneMapper> mapping,
      params MemberInfo[] members)
      where TProperty : class
    {
      foreach (MemberInfo member in members)
      {
        this.CustomizersHolder.AddCustomizer(new PropertyPath(this.PropertyPath, member), mapping);
        this.explicitDeclarationsHolder.AddAsManyToOneRelation(member);
      }
    }

    public void ManyToOne<TProperty>(Expression<Func<TEntity, TProperty>> property) where TProperty : class
    {
      this.ManyToOne<TProperty>(property, (Action<IManyToOneMapper>) (x => { }));
    }

    public void OneToOne<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IOneToOneMapper> mapping)
      where TProperty : class
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property);
      this.RegisterOneToOneMapping<TProperty>(mapping, memberInfo1, memberInfo2);
    }

    protected void RegisterOneToOneMapping<TProperty>(
      Action<IOneToOneMapper> mapping,
      params MemberInfo[] members)
      where TProperty : class
    {
      foreach (MemberInfo member in members)
      {
        this.CustomizersHolder.AddCustomizer(new PropertyPath(this.PropertyPath, member), mapping);
        this.explicitDeclarationsHolder.AddAsOneToOneRelation(member);
      }
    }

    public void Any<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Type idTypeOfMetaType,
      Action<IAnyMapper> mapping)
      where TProperty : class
    {
      this.RegisterAnyMapping<TProperty>(property, idTypeOfMetaType, mapping);
    }

    protected virtual void RegisterAnyMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Type idTypeOfMetaType,
      Action<IAnyMapper> mapping)
      where TProperty : class
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property);
      this.RegisterAnyMapping<TProperty>(mapping, idTypeOfMetaType, memberInfo1, memberInfo2);
    }

    protected void RegisterAnyMapping<TProperty>(
      Action<IAnyMapper> mapping,
      Type idTypeOfMetaType,
      params MemberInfo[] members)
      where TProperty : class
    {
      foreach (MemberInfo member in members)
      {
        this.CustomizersHolder.AddCustomizer(new PropertyPath(this.PropertyPath, member), (Action<IAnyMapper>) (am => am.IdType(idTypeOfMetaType)));
        this.CustomizersHolder.AddCustomizer(new PropertyPath(this.PropertyPath, member), mapping);
        this.explicitDeclarationsHolder.AddAsAny(member);
      }
    }

    public void Set<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.RegisterSetMapping<TElement>(property, collectionMapping, mapping);
    }

    public void Set<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.Set<TElement>(property, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    protected virtual void RegisterSetMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, IEnumerable<TElement>>(property);
      this.RegisterSetMapping<TElement>(collectionMapping, mapping, memberInfo1, memberInfo2);
    }

    protected void RegisterSetMapping<TElement>(
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping,
      params MemberInfo[] members)
    {
      foreach (MemberInfo member in members)
      {
        collectionMapping((ISetPropertiesMapper<TEntity, TElement>) new SetPropertiesCustomizer<TEntity, TElement>(this.explicitDeclarationsHolder, new PropertyPath((PropertyPath) null, member), this.CustomizersHolder));
        mapping((ICollectionElementRelation<TElement>) new CollectionElementRelationCustomizer<TElement>(this.explicitDeclarationsHolder, new PropertyPath(this.PropertyPath, member), this.CustomizersHolder));
      }
    }

    public void Bag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.RegisterBagMapping<TElement>(property, collectionMapping, mapping);
    }

    public void Bag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.Bag<TElement>(property, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    protected virtual void RegisterBagMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, IEnumerable<TElement>>(property);
      this.RegisterBagMapping<TElement>(collectionMapping, mapping, memberInfo1, memberInfo2);
    }

    protected void RegisterBagMapping<TElement>(
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping,
      params MemberInfo[] members)
    {
      foreach (MemberInfo member in members)
      {
        collectionMapping((IBagPropertiesMapper<TEntity, TElement>) new BagPropertiesCustomizer<TEntity, TElement>(this.explicitDeclarationsHolder, new PropertyPath((PropertyPath) null, member), this.CustomizersHolder));
        mapping((ICollectionElementRelation<TElement>) new CollectionElementRelationCustomizer<TElement>(this.explicitDeclarationsHolder, new PropertyPath(this.PropertyPath, member), this.CustomizersHolder));
      }
    }

    public void List<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.RegisterListMapping<TElement>(property, collectionMapping, mapping);
    }

    public void List<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.List<TElement>(property, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    protected virtual void RegisterListMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, IEnumerable<TElement>>(property);
      this.RegisterListMapping<TElement>(collectionMapping, mapping, memberInfo1, memberInfo2);
    }

    protected void RegisterListMapping<TElement>(
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping,
      params MemberInfo[] members)
    {
      foreach (MemberInfo member in members)
      {
        collectionMapping((IListPropertiesMapper<TEntity, TElement>) new ListPropertiesCustomizer<TEntity, TElement>(this.explicitDeclarationsHolder, new PropertyPath((PropertyPath) null, member), this.CustomizersHolder));
        mapping((ICollectionElementRelation<TElement>) new CollectionElementRelationCustomizer<TElement>(this.explicitDeclarationsHolder, new PropertyPath(this.PropertyPath, member), this.CustomizersHolder));
      }
    }

    public void Map<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<IMapKeyRelation<TKey>> keyMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.RegisterMapMapping<TKey, TElement>(property, collectionMapping, keyMapping, mapping);
    }

    public void Map<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping)
    {
      this.Map<TKey, TElement>(property, collectionMapping, (Action<IMapKeyRelation<TKey>>) (keyMapping => { }), (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    protected virtual void RegisterMapMapping<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<IMapKeyRelation<TKey>> keyMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, IDictionary<TKey, TElement>>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, IDictionary<TKey, TElement>>(property);
      this.RegisterMapMapping<TKey, TElement>(collectionMapping, keyMapping, mapping, memberInfo1, memberInfo2);
    }

    protected virtual void RegisterMapMapping<TKey, TElement>(
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<IMapKeyRelation<TKey>> keyMapping,
      Action<ICollectionElementRelation<TElement>> mapping,
      params MemberInfo[] members)
    {
      foreach (MemberInfo member in members)
      {
        PropertyPath propertyPath = new PropertyPath(this.PropertyPath, member);
        collectionMapping((IMapPropertiesMapper<TEntity, TKey, TElement>) new MapPropertiesCustomizer<TEntity, TKey, TElement>(this.explicitDeclarationsHolder, propertyPath, this.CustomizersHolder));
        keyMapping((IMapKeyRelation<TKey>) new MapKeyRelationCustomizer<TKey>(this.explicitDeclarationsHolder, propertyPath, this.CustomizersHolder));
        mapping((ICollectionElementRelation<TElement>) new CollectionElementRelationCustomizer<TElement>(this.explicitDeclarationsHolder, propertyPath, this.CustomizersHolder));
      }
    }

    public void Map<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.Map<TKey, TElement>(property, collectionMapping, (Action<IMapKeyRelation<TKey>>) (keyMapping => { }), mapping);
    }

    public void IdBag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.RegisterIdBagMapping<TElement>(property, collectionMapping, mapping);
    }

    public void IdBag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.RegisterIdBagMapping<TElement>(property, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    protected virtual void RegisterIdBagMapping<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, IEnumerable<TElement>>(property);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, IEnumerable<TElement>>(property);
      this.RegisterIdBagMapping<TElement>(collectionMapping, mapping, memberInfo1, memberInfo2);
    }

    protected virtual void RegisterIdBagMapping<TElement>(
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping,
      params MemberInfo[] members)
    {
      foreach (MemberInfo member in members)
      {
        collectionMapping((IIdBagPropertiesMapper<TEntity, TElement>) new IdBagPropertiesCustomizer<TEntity, TElement>(this.explicitDeclarationsHolder, new PropertyPath((PropertyPath) null, member), this.CustomizersHolder));
        mapping((ICollectionElementRelation<TElement>) new CollectionElementRelationCustomizer<TElement>(this.explicitDeclarationsHolder, new PropertyPath(this.PropertyPath, member), this.CustomizersHolder));
      }
    }

    public void Set<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type collectionElementType = matchingNameOrThrow.GetPropertyOrFieldType().DetermineCollectionElementType();
      if (!typeof (TElement).Equals(collectionElementType))
        throw new MappingException(string.Format("Wrong collection element type. For the property/field '{0}' of {1} was expected a collection of {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TElement).Name, (object) collectionElementType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterSetMapping<TElement>(collectionMapping, mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void Set<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.Set<TElement>(notVisiblePropertyOrFieldName, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    public void Bag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type collectionElementType = matchingNameOrThrow.GetPropertyOrFieldType().DetermineCollectionElementType();
      if (!typeof (TElement).Equals(collectionElementType))
        throw new MappingException(string.Format("Wrong collection element type. For the property/field '{0}' of {1} was expected a collection of {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TElement).Name, (object) collectionElementType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterBagMapping<TElement>(collectionMapping, mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void Bag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.Bag<TElement>(notVisiblePropertyOrFieldName, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    public void List<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type collectionElementType = matchingNameOrThrow.GetPropertyOrFieldType().DetermineCollectionElementType();
      if (!typeof (TElement).Equals(collectionElementType))
        throw new MappingException(string.Format("Wrong collection element type. For the property/field '{0}' of {1} was expected a collection of {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TElement).Name, (object) collectionElementType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterListMapping<TElement>(collectionMapping, mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void List<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.List<TElement>(notVisiblePropertyOrFieldName, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    public void Map<TKey, TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<IMapKeyRelation<TKey>> keyMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type propertyOrFieldType = matchingNameOrThrow.GetPropertyOrFieldType();
      Type dictionaryKeyType = propertyOrFieldType.DetermineDictionaryKeyType();
      Type dictionaryValueType = propertyOrFieldType.DetermineDictionaryValueType();
      if (!typeof (TElement).Equals(dictionaryValueType) || !typeof (TKey).Equals(dictionaryKeyType))
        throw new MappingException(string.Format("Wrong collection element type. For the property/field '{0}' of {1} was expected a dictionary of {2}/{3} but was {4}/{5}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TKey).Name, (object) dictionaryKeyType.Name, (object) typeof (TElement).Name, (object) dictionaryValueType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterMapMapping<TKey, TElement>(collectionMapping, keyMapping, mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void Map<TKey, TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      this.Map<TKey, TElement>(notVisiblePropertyOrFieldName, collectionMapping, (Action<IMapKeyRelation<TKey>>) (x => { }), mapping);
    }

    public void Map<TKey, TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping)
    {
      this.Map<TKey, TElement>(notVisiblePropertyOrFieldName, collectionMapping, (Action<IMapKeyRelation<TKey>>) (x => { }), (Action<ICollectionElementRelation<TElement>>) (y => { }));
    }

    public void IdBag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping)
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type collectionElementType = matchingNameOrThrow.GetPropertyOrFieldType().DetermineCollectionElementType();
      if (!typeof (TElement).Equals(collectionElementType))
        throw new MappingException(string.Format("Wrong collection element type. For the property/field '{0}' of {1} was expected a collection of {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TElement).Name, (object) collectionElementType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterIdBagMapping<TElement>(collectionMapping, mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void IdBag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping)
    {
      this.IdBag<TElement>(notVisiblePropertyOrFieldName, collectionMapping, (Action<ICollectionElementRelation<TElement>>) (x => { }));
    }

    public void ManyToOne<TProperty>(
      string notVisiblePropertyOrFieldName,
      Action<IManyToOneMapper> mapping)
      where TProperty : class
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type propertyOrFieldType = matchingNameOrThrow.GetPropertyOrFieldType();
      if (!typeof (TProperty).Equals(propertyOrFieldType))
        throw new MappingException(string.Format("Wrong relation type. For the property/field '{0}' of {1} was expected a many-to-one with {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TProperty).Name, (object) propertyOrFieldType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterManyToOneMapping<TProperty>(mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void Component<TComponent>(
      string notVisiblePropertyOrFieldName,
      Action<IComponentMapper<TComponent>> mapping)
      where TComponent : class
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type propertyOrFieldType = matchingNameOrThrow.GetPropertyOrFieldType();
      if (!typeof (TComponent).Equals(propertyOrFieldType))
        throw new MappingException(string.Format("Wrong relation type. For the property/field '{0}' of {1} was expected a component of {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TComponent).Name, (object) propertyOrFieldType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterComponentMapping<TComponent>(mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void Component<TComponent>(string notVisiblePropertyOrFieldName) where TComponent : class
    {
      this.Component<TComponent>(notVisiblePropertyOrFieldName, (Action<IComponentMapper<TComponent>>) (x => { }));
    }

    public void Component<TComponent>(
      string notVisiblePropertyOrFieldName,
      TComponent dynamicComponentTemplate,
      Action<IDynamicComponentMapper<TComponent>> mapping)
      where TComponent : class
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterDynamicComponentMapping<TComponent>(mapping, matchingNameOrThrow, fromReflectedType);
    }

    public void Any<TProperty>(
      string notVisiblePropertyOrFieldName,
      Type idTypeOfMetaType,
      Action<IAnyMapper> mapping)
      where TProperty : class
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type propertyOrFieldType = matchingNameOrThrow.GetPropertyOrFieldType();
      if (!typeof (TProperty).Equals(propertyOrFieldType))
        throw new MappingException(string.Format("Wrong relation type. For the property/field '{0}' of {1} was expected a heterogeneous (any) of type {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TProperty).Name, (object) propertyOrFieldType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterAnyMapping<TProperty>(mapping, idTypeOfMetaType, matchingNameOrThrow, fromReflectedType);
    }

    public void OneToOne<TProperty>(
      string notVisiblePropertyOrFieldName,
      Action<IOneToOneMapper> mapping)
      where TProperty : class
    {
      MemberInfo matchingNameOrThrow = PropertyContainerCustomizer<TEntity>.GetPropertyOrFieldMatchingNameOrThrow(notVisiblePropertyOrFieldName);
      Type propertyOrFieldType = matchingNameOrThrow.GetPropertyOrFieldType();
      if (!typeof (TProperty).Equals(propertyOrFieldType))
        throw new MappingException(string.Format("Wrong relation type. For the property/field '{0}' of {1} was expected a one-to-one with {2} but was {3}", (object) notVisiblePropertyOrFieldName, (object) typeof (TEntity).FullName, (object) typeof (TProperty).Name, (object) propertyOrFieldType.Name));
      MemberInfo fromReflectedType = matchingNameOrThrow.GetMemberFromReflectedType(typeof (TEntity));
      this.RegisterOneToOneMapping<TProperty>(mapping, matchingNameOrThrow, fromReflectedType);
    }

    public static MemberInfo GetPropertyOrFieldMatchingNameOrThrow(string memberName)
    {
      return typeof (TEntity).GetPropertyOrFieldMatchingName(memberName) ?? throw new MappingException(string.Format("Member not found. The member '{0}' does not exists in type {1}", (object) memberName, (object) typeof (TEntity).FullName));
    }
  }
}
