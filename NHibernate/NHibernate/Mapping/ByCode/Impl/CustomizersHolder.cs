// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersHolder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class CustomizersHolder : ICustomizersHolder
  {
    private readonly Dictionary<PropertyPath, List<Action<IAnyMapper>>> anyCustomizers = new Dictionary<PropertyPath, List<Action<IAnyMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IBagPropertiesMapper>>> bagCustomizers = new Dictionary<PropertyPath, List<Action<IBagPropertiesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IIdBagPropertiesMapper>>> idBagCustomizers = new Dictionary<PropertyPath, List<Action<IIdBagPropertiesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>> collectionCustomizers = new Dictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IElementMapper>>> collectionRelationElementCustomizers = new Dictionary<PropertyPath, List<Action<IElementMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IManyToManyMapper>>> collectionRelationManyToManyCustomizers = new Dictionary<PropertyPath, List<Action<IManyToManyMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IManyToAnyMapper>>> collectionRelationManyToAnyCustomizers = new Dictionary<PropertyPath, List<Action<IManyToAnyMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IOneToManyMapper>>> collectionRelationOneToManyCustomizers = new Dictionary<PropertyPath, List<Action<IOneToManyMapper>>>();
    private readonly Dictionary<Type, List<Action<IComponentAttributesMapper>>> componentClassCustomizers = new Dictionary<Type, List<Action<IComponentAttributesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IComponentAttributesMapper>>> componentPropertyCustomizers = new Dictionary<PropertyPath, List<Action<IComponentAttributesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IComponentAsIdAttributesMapper>>> componentAsIdPropertyCustomizers = new Dictionary<PropertyPath, List<Action<IComponentAsIdAttributesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IDynamicComponentAttributesMapper>>> dynamicComponentCustomizers = new Dictionary<PropertyPath, List<Action<IDynamicComponentAttributesMapper>>>();
    private readonly Dictionary<Type, List<Action<IJoinedSubclassAttributesMapper>>> joinedClassCustomizers = new Dictionary<Type, List<Action<IJoinedSubclassAttributesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IListPropertiesMapper>>> listCustomizers = new Dictionary<PropertyPath, List<Action<IListPropertiesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IManyToOneMapper>>> manyToOneCustomizers = new Dictionary<PropertyPath, List<Action<IManyToOneMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IMapPropertiesMapper>>> mapCustomizers = new Dictionary<PropertyPath, List<Action<IMapPropertiesMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IMapKeyMapper>>> mapKeyElementCustomizers = new Dictionary<PropertyPath, List<Action<IMapKeyMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IMapKeyManyToManyMapper>>> mapKeyManyToManyCustomizers = new Dictionary<PropertyPath, List<Action<IMapKeyManyToManyMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IOneToOneMapper>>> oneToOneCustomizers = new Dictionary<PropertyPath, List<Action<IOneToOneMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<IPropertyMapper>>> propertyCustomizers = new Dictionary<PropertyPath, List<Action<IPropertyMapper>>>();
    private readonly Dictionary<Type, List<Action<IClassMapper>>> rootClassCustomizers = new Dictionary<Type, List<Action<IClassMapper>>>();
    private readonly Dictionary<PropertyPath, List<Action<ISetPropertiesMapper>>> setCustomizers = new Dictionary<PropertyPath, List<Action<ISetPropertiesMapper>>>();
    private readonly Dictionary<Type, List<Action<ISubclassMapper>>> subclassCustomizers = new Dictionary<Type, List<Action<ISubclassMapper>>>();
    private readonly Dictionary<Type, List<Action<IUnionSubclassAttributesMapper>>> unionClassCustomizers = new Dictionary<Type, List<Action<IUnionSubclassAttributesMapper>>>();
    private readonly Dictionary<Type, List<Action<IJoinAttributesMapper>>> joinCustomizers = new Dictionary<Type, List<Action<IJoinAttributesMapper>>>();

    public void AddCustomizer(Type type, Action<IClassMapper> classCustomizer)
    {
      this.AddCustomizer<Type, IClassMapper>((IDictionary<Type, List<Action<IClassMapper>>>) this.rootClassCustomizers, type, classCustomizer);
    }

    public void AddCustomizer(Type type, Action<ISubclassMapper> classCustomizer)
    {
      this.AddCustomizer<Type, ISubclassMapper>((IDictionary<Type, List<Action<ISubclassMapper>>>) this.subclassCustomizers, type, classCustomizer);
    }

    public void AddCustomizer(
      Type type,
      Action<IJoinedSubclassAttributesMapper> classCustomizer)
    {
      this.AddCustomizer<Type, IJoinedSubclassAttributesMapper>((IDictionary<Type, List<Action<IJoinedSubclassAttributesMapper>>>) this.joinedClassCustomizers, type, classCustomizer);
    }

    public void AddCustomizer(
      Type type,
      Action<IUnionSubclassAttributesMapper> classCustomizer)
    {
      this.AddCustomizer<Type, IUnionSubclassAttributesMapper>((IDictionary<Type, List<Action<IUnionSubclassAttributesMapper>>>) this.unionClassCustomizers, type, classCustomizer);
    }

    public void AddCustomizer(Type type, Action<IComponentAttributesMapper> classCustomizer)
    {
      this.AddCustomizer<Type, IComponentAttributesMapper>((IDictionary<Type, List<Action<IComponentAttributesMapper>>>) this.componentClassCustomizers, type, classCustomizer);
    }

    public void AddCustomizer(Type type, Action<IJoinAttributesMapper> joinCustomizer)
    {
      this.AddCustomizer<Type, IJoinAttributesMapper>((IDictionary<Type, List<Action<IJoinAttributesMapper>>>) this.joinCustomizers, type, joinCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IPropertyMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IPropertyMapper>((IDictionary<PropertyPath, List<Action<IPropertyMapper>>>) this.propertyCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IManyToOneMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IManyToOneMapper>((IDictionary<PropertyPath, List<Action<IManyToOneMapper>>>) this.manyToOneCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IOneToOneMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IOneToOneMapper>((IDictionary<PropertyPath, List<Action<IOneToOneMapper>>>) this.oneToOneCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IAnyMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IAnyMapper>((IDictionary<PropertyPath, List<Action<IAnyMapper>>>) this.anyCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<ISetPropertiesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, ISetPropertiesMapper>((IDictionary<PropertyPath, List<Action<ISetPropertiesMapper>>>) this.setCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IBagPropertiesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IBagPropertiesMapper>((IDictionary<PropertyPath, List<Action<IBagPropertiesMapper>>>) this.bagCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IListPropertiesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IListPropertiesMapper>((IDictionary<PropertyPath, List<Action<IListPropertiesMapper>>>) this.listCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IMapPropertiesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IMapPropertiesMapper>((IDictionary<PropertyPath, List<Action<IMapPropertiesMapper>>>) this.mapCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IIdBagPropertiesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IIdBagPropertiesMapper>((IDictionary<PropertyPath, List<Action<IIdBagPropertiesMapper>>>) this.idBagCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<ICollectionPropertiesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, ICollectionPropertiesMapper>((IDictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>>) this.collectionCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IComponentAttributesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IComponentAttributesMapper>((IDictionary<PropertyPath, List<Action<IComponentAttributesMapper>>>) this.componentPropertyCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IComponentAsIdAttributesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IComponentAsIdAttributesMapper>((IDictionary<PropertyPath, List<Action<IComponentAsIdAttributesMapper>>>) this.componentAsIdPropertyCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IDynamicComponentAttributesMapper> propertyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IDynamicComponentAttributesMapper>((IDictionary<PropertyPath, List<Action<IDynamicComponentAttributesMapper>>>) this.dynamicComponentCustomizers, member, propertyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IManyToManyMapper> collectionRelationManyToManyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IManyToManyMapper>((IDictionary<PropertyPath, List<Action<IManyToManyMapper>>>) this.collectionRelationManyToManyCustomizers, member, collectionRelationManyToManyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IElementMapper> collectionRelationElementCustomizer)
    {
      this.AddCustomizer<PropertyPath, IElementMapper>((IDictionary<PropertyPath, List<Action<IElementMapper>>>) this.collectionRelationElementCustomizers, member, collectionRelationElementCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IOneToManyMapper> collectionRelationOneToManyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IOneToManyMapper>((IDictionary<PropertyPath, List<Action<IOneToManyMapper>>>) this.collectionRelationOneToManyCustomizers, member, collectionRelationOneToManyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IManyToAnyMapper> collectionRelationManyToAnyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IManyToAnyMapper>((IDictionary<PropertyPath, List<Action<IManyToAnyMapper>>>) this.collectionRelationManyToAnyCustomizers, member, collectionRelationManyToAnyCustomizer);
    }

    public void AddCustomizer(
      PropertyPath member,
      Action<IMapKeyManyToManyMapper> mapKeyManyToManyCustomizer)
    {
      this.AddCustomizer<PropertyPath, IMapKeyManyToManyMapper>((IDictionary<PropertyPath, List<Action<IMapKeyManyToManyMapper>>>) this.mapKeyManyToManyCustomizers, member, mapKeyManyToManyCustomizer);
    }

    public void AddCustomizer(PropertyPath member, Action<IMapKeyMapper> mapKeyElementCustomizer)
    {
      this.AddCustomizer<PropertyPath, IMapKeyMapper>((IDictionary<PropertyPath, List<Action<IMapKeyMapper>>>) this.mapKeyElementCustomizers, member, mapKeyElementCustomizer);
    }

    public void InvokeCustomizers(PropertyPath member, IManyToAnyMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IManyToAnyMapper>((IDictionary<PropertyPath, List<Action<IManyToAnyMapper>>>) this.collectionRelationManyToAnyCustomizers, member, mapper);
    }

    public IEnumerable<Type> GetAllCustomizedEntities()
    {
      return this.rootClassCustomizers.Keys.Concat<Type>((IEnumerable<Type>) this.subclassCustomizers.Keys).Concat<Type>((IEnumerable<Type>) this.joinedClassCustomizers.Keys).Concat<Type>((IEnumerable<Type>) this.unionClassCustomizers.Keys);
    }

    public void InvokeCustomizers(Type type, IClassMapper mapper)
    {
      this.InvokeCustomizers<Type, IClassMapper>((IDictionary<Type, List<Action<IClassMapper>>>) this.rootClassCustomizers, type, mapper);
    }

    public void InvokeCustomizers(Type type, ISubclassMapper mapper)
    {
      this.InvokeCustomizers<Type, ISubclassMapper>((IDictionary<Type, List<Action<ISubclassMapper>>>) this.subclassCustomizers, type, mapper);
    }

    public void InvokeCustomizers(Type type, IJoinedSubclassAttributesMapper mapper)
    {
      this.InvokeCustomizers<Type, IJoinedSubclassAttributesMapper>((IDictionary<Type, List<Action<IJoinedSubclassAttributesMapper>>>) this.joinedClassCustomizers, type, mapper);
    }

    public void InvokeCustomizers(Type type, IUnionSubclassAttributesMapper mapper)
    {
      this.InvokeCustomizers<Type, IUnionSubclassAttributesMapper>((IDictionary<Type, List<Action<IUnionSubclassAttributesMapper>>>) this.unionClassCustomizers, type, mapper);
    }

    public void InvokeCustomizers(Type type, IComponentAttributesMapper mapper)
    {
      this.InvokeCustomizers<Type, IComponentAttributesMapper>((IDictionary<Type, List<Action<IComponentAttributesMapper>>>) this.componentClassCustomizers, type, mapper);
    }

    public void InvokeCustomizers(Type type, IJoinAttributesMapper mapper)
    {
      this.InvokeCustomizers<Type, IJoinAttributesMapper>((IDictionary<Type, List<Action<IJoinAttributesMapper>>>) this.joinCustomizers, type, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IPropertyMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IPropertyMapper>((IDictionary<PropertyPath, List<Action<IPropertyMapper>>>) this.propertyCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IManyToOneMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IManyToOneMapper>((IDictionary<PropertyPath, List<Action<IManyToOneMapper>>>) this.manyToOneCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IOneToOneMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IOneToOneMapper>((IDictionary<PropertyPath, List<Action<IOneToOneMapper>>>) this.oneToOneCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IAnyMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IAnyMapper>((IDictionary<PropertyPath, List<Action<IAnyMapper>>>) this.anyCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, ISetPropertiesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, ICollectionPropertiesMapper>((IDictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>>) this.collectionCustomizers, member, (ICollectionPropertiesMapper) mapper);
      this.InvokeCustomizers<PropertyPath, ISetPropertiesMapper>((IDictionary<PropertyPath, List<Action<ISetPropertiesMapper>>>) this.setCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IBagPropertiesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, ICollectionPropertiesMapper>((IDictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>>) this.collectionCustomizers, member, (ICollectionPropertiesMapper) mapper);
      this.InvokeCustomizers<PropertyPath, IBagPropertiesMapper>((IDictionary<PropertyPath, List<Action<IBagPropertiesMapper>>>) this.bagCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IListPropertiesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, ICollectionPropertiesMapper>((IDictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>>) this.collectionCustomizers, member, (ICollectionPropertiesMapper) mapper);
      this.InvokeCustomizers<PropertyPath, IListPropertiesMapper>((IDictionary<PropertyPath, List<Action<IListPropertiesMapper>>>) this.listCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IMapPropertiesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, ICollectionPropertiesMapper>((IDictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>>) this.collectionCustomizers, member, (ICollectionPropertiesMapper) mapper);
      this.InvokeCustomizers<PropertyPath, IMapPropertiesMapper>((IDictionary<PropertyPath, List<Action<IMapPropertiesMapper>>>) this.mapCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IIdBagPropertiesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, ICollectionPropertiesMapper>((IDictionary<PropertyPath, List<Action<ICollectionPropertiesMapper>>>) this.collectionCustomizers, member, (ICollectionPropertiesMapper) mapper);
      this.InvokeCustomizers<PropertyPath, IIdBagPropertiesMapper>((IDictionary<PropertyPath, List<Action<IIdBagPropertiesMapper>>>) this.idBagCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IComponentAttributesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IComponentAttributesMapper>((IDictionary<PropertyPath, List<Action<IComponentAttributesMapper>>>) this.componentPropertyCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IComponentAsIdAttributesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IComponentAsIdAttributesMapper>((IDictionary<PropertyPath, List<Action<IComponentAsIdAttributesMapper>>>) this.componentAsIdPropertyCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IDynamicComponentAttributesMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IDynamicComponentAttributesMapper>((IDictionary<PropertyPath, List<Action<IDynamicComponentAttributesMapper>>>) this.dynamicComponentCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IManyToManyMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IManyToManyMapper>((IDictionary<PropertyPath, List<Action<IManyToManyMapper>>>) this.collectionRelationManyToManyCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IElementMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IElementMapper>((IDictionary<PropertyPath, List<Action<IElementMapper>>>) this.collectionRelationElementCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IOneToManyMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IOneToManyMapper>((IDictionary<PropertyPath, List<Action<IOneToManyMapper>>>) this.collectionRelationOneToManyCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IMapKeyManyToManyMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IMapKeyManyToManyMapper>((IDictionary<PropertyPath, List<Action<IMapKeyManyToManyMapper>>>) this.mapKeyManyToManyCustomizers, member, mapper);
    }

    public void InvokeCustomizers(PropertyPath member, IMapKeyMapper mapper)
    {
      this.InvokeCustomizers<PropertyPath, IMapKeyMapper>((IDictionary<PropertyPath, List<Action<IMapKeyMapper>>>) this.mapKeyElementCustomizers, member, mapper);
    }

    public void Merge(CustomizersHolder source)
    {
      if (source == null)
        return;
      this.MergeDictionary<Type, IClassMapper>(this.rootClassCustomizers, source.rootClassCustomizers);
      this.MergeDictionary<Type, ISubclassMapper>(this.subclassCustomizers, source.subclassCustomizers);
      this.MergeDictionary<Type, IJoinedSubclassAttributesMapper>(this.joinedClassCustomizers, source.joinedClassCustomizers);
      this.MergeDictionary<Type, IUnionSubclassAttributesMapper>(this.unionClassCustomizers, source.unionClassCustomizers);
      this.MergeDictionary<Type, IComponentAttributesMapper>(this.componentClassCustomizers, source.componentClassCustomizers);
      this.MergeDictionary<Type, IJoinAttributesMapper>(this.joinCustomizers, source.joinCustomizers);
      this.MergeDictionary<PropertyPath, IPropertyMapper>(this.propertyCustomizers, source.propertyCustomizers);
      this.MergeDictionary<PropertyPath, IManyToOneMapper>(this.manyToOneCustomizers, source.manyToOneCustomizers);
      this.MergeDictionary<PropertyPath, IOneToOneMapper>(this.oneToOneCustomizers, source.oneToOneCustomizers);
      this.MergeDictionary<PropertyPath, IAnyMapper>(this.anyCustomizers, source.anyCustomizers);
      this.MergeDictionary<PropertyPath, ISetPropertiesMapper>(this.setCustomizers, source.setCustomizers);
      this.MergeDictionary<PropertyPath, IBagPropertiesMapper>(this.bagCustomizers, source.bagCustomizers);
      this.MergeDictionary<PropertyPath, IListPropertiesMapper>(this.listCustomizers, source.listCustomizers);
      this.MergeDictionary<PropertyPath, IMapPropertiesMapper>(this.mapCustomizers, source.mapCustomizers);
      this.MergeDictionary<PropertyPath, IIdBagPropertiesMapper>(this.idBagCustomizers, source.idBagCustomizers);
      this.MergeDictionary<PropertyPath, ICollectionPropertiesMapper>(this.collectionCustomizers, source.collectionCustomizers);
      this.MergeDictionary<PropertyPath, IComponentAttributesMapper>(this.componentPropertyCustomizers, source.componentPropertyCustomizers);
      this.MergeDictionary<PropertyPath, IManyToManyMapper>(this.collectionRelationManyToManyCustomizers, source.collectionRelationManyToManyCustomizers);
      this.MergeDictionary<PropertyPath, IElementMapper>(this.collectionRelationElementCustomizers, source.collectionRelationElementCustomizers);
      this.MergeDictionary<PropertyPath, IOneToManyMapper>(this.collectionRelationOneToManyCustomizers, source.collectionRelationOneToManyCustomizers);
      this.MergeDictionary<PropertyPath, IMapKeyManyToManyMapper>(this.mapKeyManyToManyCustomizers, source.mapKeyManyToManyCustomizers);
      this.MergeDictionary<PropertyPath, IMapKeyMapper>(this.mapKeyElementCustomizers, source.mapKeyElementCustomizers);
      this.MergeDictionary<PropertyPath, IDynamicComponentAttributesMapper>(this.dynamicComponentCustomizers, source.dynamicComponentCustomizers);
      this.MergeDictionary<PropertyPath, IComponentAsIdAttributesMapper>(this.componentAsIdPropertyCustomizers, source.componentAsIdPropertyCustomizers);
    }

    private void MergeDictionary<TSubject, TCustomizable>(
      Dictionary<TSubject, List<Action<TCustomizable>>> destination,
      Dictionary<TSubject, List<Action<TCustomizable>>> source)
    {
      foreach (KeyValuePair<TSubject, List<Action<TCustomizable>>> keyValuePair in source)
      {
        List<Action<TCustomizable>> actionList;
        if (!destination.TryGetValue(keyValuePair.Key, out actionList))
        {
          actionList = new List<Action<TCustomizable>>();
          destination[keyValuePair.Key] = actionList;
        }
        actionList.AddRange((IEnumerable<Action<TCustomizable>>) keyValuePair.Value);
      }
    }

    private void AddCustomizer<TSubject, TCustomizable>(
      IDictionary<TSubject, List<Action<TCustomizable>>> customizers,
      TSubject member,
      Action<TCustomizable> customizer)
    {
      List<Action<TCustomizable>> actionList;
      if (!customizers.TryGetValue(member, out actionList))
      {
        actionList = new List<Action<TCustomizable>>();
        customizers[member] = actionList;
      }
      actionList.Add(customizer);
    }

    private void InvokeCustomizers<TSubject, TCustomizable>(
      IDictionary<TSubject, List<Action<TCustomizable>>> customizers,
      TSubject member,
      TCustomizable customizable)
    {
      List<Action<TCustomizable>> actionList;
      if (!customizers.TryGetValue(member, out actionList))
        return;
      foreach (Action<TCustomizable> action in actionList)
        action(customizable);
    }
  }
}
