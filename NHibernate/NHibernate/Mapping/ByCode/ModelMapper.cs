// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ModelMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode.Impl;
using NHibernate.Mapping.ByCode.Impl.CustomizersImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class ModelMapper
  {
    private readonly ICustomizersHolder customizerHolder;
    private readonly IModelExplicitDeclarationsHolder explicitDeclarationsHolder;
    private readonly ICandidatePersistentMembersProvider membersProvider;
    private readonly IModelInspector modelInspector;

    public ModelMapper()
      : this((IModelInspector) new ExplicitlyDeclaredModel())
    {
    }

    public ModelMapper(IModelInspector modelInspector)
      : this(modelInspector, modelInspector as IModelExplicitDeclarationsHolder)
    {
    }

    public ModelMapper(
      IModelInspector modelInspector,
      ICandidatePersistentMembersProvider membersProvider)
      : this(modelInspector, modelInspector as IModelExplicitDeclarationsHolder, (ICustomizersHolder) new CustomizersHolder(), membersProvider)
    {
    }

    public ModelMapper(
      IModelInspector modelInspector,
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder)
      : this(modelInspector, explicitDeclarationsHolder, (ICustomizersHolder) new CustomizersHolder(), (ICandidatePersistentMembersProvider) new DefaultCandidatePersistentMembersProvider())
    {
    }

    public ModelMapper(
      IModelInspector modelInspector,
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizerHolder,
      ICandidatePersistentMembersProvider membersProvider)
    {
      if (modelInspector == null)
        throw new ArgumentNullException(nameof (modelInspector));
      if (customizerHolder == null)
        throw new ArgumentNullException(nameof (customizerHolder));
      if (membersProvider == null)
        throw new ArgumentNullException(nameof (membersProvider));
      this.modelInspector = modelInspector;
      this.customizerHolder = customizerHolder;
      this.explicitDeclarationsHolder = explicitDeclarationsHolder ?? (IModelExplicitDeclarationsHolder) new FakeModelExplicitDeclarationsHolder();
      this.membersProvider = membersProvider;
    }

    public event RootClassMappingHandler BeforeMapClass;

    public event SubclassMappingHandler BeforeMapSubclass;

    public event JoinedSubclassMappingHandler BeforeMapJoinedSubclass;

    public event UnionSubclassMappingHandler BeforeMapUnionSubclass;

    public event PropertyMappingHandler BeforeMapProperty;

    public event ManyToOneMappingHandler BeforeMapManyToOne;

    public event OneToOneMappingHandler BeforeMapOneToOne;

    public event AnyMappingHandler BeforeMapAny;

    public event ComponentMappingHandler BeforeMapComponent;

    public event SetMappingHandler BeforeMapSet;

    public event BagMappingHandler BeforeMapBag;

    public event IdBagMappingHandler BeforeMapIdBag;

    public event ListMappingHandler BeforeMapList;

    public event MapMappingHandler BeforeMapMap;

    public event ManyToManyMappingHandler BeforeMapManyToMany;

    public event ElementMappingHandler BeforeMapElement;

    public event OneToManyMappingHandler BeforeMapOneToMany;

    public event MapKeyManyToManyMappingHandler BeforeMapMapKeyManyToMany;

    public event MapKeyMappingHandler BeforeMapMapKey;

    public event RootClassMappingHandler AfterMapClass;

    public event SubclassMappingHandler AfterMapSubclass;

    public event JoinedSubclassMappingHandler AfterMapJoinedSubclass;

    public event UnionSubclassMappingHandler AfterMapUnionSubclass;

    public event PropertyMappingHandler AfterMapProperty;

    public event ManyToOneMappingHandler AfterMapManyToOne;

    public event OneToOneMappingHandler AfterMapOneToOne;

    public event AnyMappingHandler AfterMapAny;

    public event ComponentMappingHandler AfterMapComponent;

    public event SetMappingHandler AfterMapSet;

    public event BagMappingHandler AfterMapBag;

    public event IdBagMappingHandler AfterMapIdBag;

    public event ListMappingHandler AfterMapList;

    public event MapMappingHandler AfterMapMap;

    public event ManyToManyMappingHandler AfterMapManyToMany;

    public event ElementMappingHandler AfterMapElement;

    public event OneToManyMappingHandler AfterMapOneToMany;

    public event MapKeyManyToManyMappingHandler AfterMapMapKeyManyToMany;

    public event MapKeyMappingHandler AfterMapMapKey;

    private void InvokeBeforeMapUnionSubclass(
      Type type,
      IUnionSubclassAttributesMapper unionsubclasscustomizer)
    {
      UnionSubclassMappingHandler mapUnionSubclass = this.BeforeMapUnionSubclass;
      if (mapUnionSubclass == null)
        return;
      mapUnionSubclass(this.ModelInspector, type, unionsubclasscustomizer);
    }

    private void InvokeBeforeMapJoinedSubclass(
      Type type,
      IJoinedSubclassAttributesMapper joinedsubclasscustomizer)
    {
      JoinedSubclassMappingHandler mapJoinedSubclass = this.BeforeMapJoinedSubclass;
      if (mapJoinedSubclass == null)
        return;
      mapJoinedSubclass(this.ModelInspector, type, joinedsubclasscustomizer);
    }

    private void InvokeBeforeMapSubclass(Type type, ISubclassAttributesMapper subclasscustomizer)
    {
      SubclassMappingHandler beforeMapSubclass = this.BeforeMapSubclass;
      if (beforeMapSubclass == null)
        return;
      beforeMapSubclass(this.ModelInspector, type, subclasscustomizer);
    }

    private void InvokeBeforeMapClass(Type type, IClassAttributesMapper classcustomizer)
    {
      RootClassMappingHandler beforeMapClass = this.BeforeMapClass;
      if (beforeMapClass == null)
        return;
      beforeMapClass(this.ModelInspector, type, classcustomizer);
    }

    private void InvokeBeforeMapProperty(PropertyPath member, IPropertyMapper propertycustomizer)
    {
      PropertyMappingHandler beforeMapProperty = this.BeforeMapProperty;
      if (beforeMapProperty == null)
        return;
      beforeMapProperty(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapManyToOne(PropertyPath member, IManyToOneMapper propertycustomizer)
    {
      ManyToOneMappingHandler beforeMapManyToOne = this.BeforeMapManyToOne;
      if (beforeMapManyToOne == null)
        return;
      beforeMapManyToOne(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapOneToOne(PropertyPath member, IOneToOneMapper propertycustomizer)
    {
      OneToOneMappingHandler beforeMapOneToOne = this.BeforeMapOneToOne;
      if (beforeMapOneToOne == null)
        return;
      beforeMapOneToOne(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapAny(PropertyPath member, IAnyMapper propertycustomizer)
    {
      AnyMappingHandler beforeMapAny = this.BeforeMapAny;
      if (beforeMapAny == null)
        return;
      beforeMapAny(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapComponent(
      PropertyPath member,
      IComponentAttributesMapper propertycustomizer)
    {
      ComponentMappingHandler beforeMapComponent = this.BeforeMapComponent;
      if (beforeMapComponent == null)
        return;
      beforeMapComponent(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapSet(PropertyPath member, ISetPropertiesMapper propertycustomizer)
    {
      SetMappingHandler beforeMapSet = this.BeforeMapSet;
      if (beforeMapSet == null)
        return;
      beforeMapSet(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapBag(PropertyPath member, IBagPropertiesMapper propertycustomizer)
    {
      BagMappingHandler beforeMapBag = this.BeforeMapBag;
      if (beforeMapBag == null)
        return;
      beforeMapBag(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapIdBag(
      PropertyPath member,
      IIdBagPropertiesMapper propertycustomizer)
    {
      IdBagMappingHandler beforeMapIdBag = this.BeforeMapIdBag;
      if (beforeMapIdBag == null)
        return;
      beforeMapIdBag(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapList(PropertyPath member, IListPropertiesMapper propertycustomizer)
    {
      ListMappingHandler beforeMapList = this.BeforeMapList;
      if (beforeMapList == null)
        return;
      beforeMapList(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapMap(PropertyPath member, IMapPropertiesMapper propertycustomizer)
    {
      MapMappingHandler beforeMapMap = this.BeforeMapMap;
      if (beforeMapMap == null)
        return;
      beforeMapMap(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeBeforeMapManyToMany(
      PropertyPath member,
      IManyToManyMapper collectionrelationmanytomanycustomizer)
    {
      ManyToManyMappingHandler beforeMapManyToMany = this.BeforeMapManyToMany;
      if (beforeMapManyToMany == null)
        return;
      beforeMapManyToMany(this.ModelInspector, member, collectionrelationmanytomanycustomizer);
    }

    private void InvokeBeforeMapElement(
      PropertyPath member,
      IElementMapper collectionrelationelementcustomizer)
    {
      ElementMappingHandler beforeMapElement = this.BeforeMapElement;
      if (beforeMapElement == null)
        return;
      beforeMapElement(this.ModelInspector, member, collectionrelationelementcustomizer);
    }

    private void InvokeBeforeMapOneToMany(
      PropertyPath member,
      IOneToManyMapper collectionrelationonetomanycustomizer)
    {
      OneToManyMappingHandler beforeMapOneToMany = this.BeforeMapOneToMany;
      if (beforeMapOneToMany == null)
        return;
      beforeMapOneToMany(this.ModelInspector, member, collectionrelationonetomanycustomizer);
    }

    private void InvokeBeforeMapMapKeyManyToMany(
      PropertyPath member,
      IMapKeyManyToManyMapper mapkeymanytomanycustomizer)
    {
      MapKeyManyToManyMappingHandler mapKeyManyToMany = this.BeforeMapMapKeyManyToMany;
      if (mapKeyManyToMany == null)
        return;
      mapKeyManyToMany(this.ModelInspector, member, mapkeymanytomanycustomizer);
    }

    private void InvokeBeforeMapMapKey(PropertyPath member, IMapKeyMapper mapkeyelementcustomizer)
    {
      MapKeyMappingHandler beforeMapMapKey = this.BeforeMapMapKey;
      if (beforeMapMapKey == null)
        return;
      beforeMapMapKey(this.ModelInspector, member, mapkeyelementcustomizer);
    }

    private void InvokeAfterMapUnionSubclass(
      Type type,
      IUnionSubclassAttributesMapper unionsubclasscustomizer)
    {
      UnionSubclassMappingHandler mapUnionSubclass = this.AfterMapUnionSubclass;
      if (mapUnionSubclass == null)
        return;
      mapUnionSubclass(this.ModelInspector, type, unionsubclasscustomizer);
    }

    private void InvokeAfterMapJoinedSubclass(
      Type type,
      IJoinedSubclassAttributesMapper joinedsubclasscustomizer)
    {
      JoinedSubclassMappingHandler mapJoinedSubclass = this.AfterMapJoinedSubclass;
      if (mapJoinedSubclass == null)
        return;
      mapJoinedSubclass(this.ModelInspector, type, joinedsubclasscustomizer);
    }

    private void InvokeAfterMapSubclass(Type type, ISubclassAttributesMapper subclasscustomizer)
    {
      SubclassMappingHandler afterMapSubclass = this.AfterMapSubclass;
      if (afterMapSubclass == null)
        return;
      afterMapSubclass(this.ModelInspector, type, subclasscustomizer);
    }

    private void InvokeAfterMapClass(Type type, IClassAttributesMapper classcustomizer)
    {
      RootClassMappingHandler afterMapClass = this.AfterMapClass;
      if (afterMapClass == null)
        return;
      afterMapClass(this.ModelInspector, type, classcustomizer);
    }

    private void InvokeAfterMapProperty(PropertyPath member, IPropertyMapper propertycustomizer)
    {
      PropertyMappingHandler afterMapProperty = this.AfterMapProperty;
      if (afterMapProperty == null)
        return;
      afterMapProperty(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapManyToOne(PropertyPath member, IManyToOneMapper propertycustomizer)
    {
      ManyToOneMappingHandler afterMapManyToOne = this.AfterMapManyToOne;
      if (afterMapManyToOne == null)
        return;
      afterMapManyToOne(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapOneToOne(PropertyPath member, IOneToOneMapper propertycustomizer)
    {
      OneToOneMappingHandler afterMapOneToOne = this.AfterMapOneToOne;
      if (afterMapOneToOne == null)
        return;
      afterMapOneToOne(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapAny(PropertyPath member, IAnyMapper propertycustomizer)
    {
      AnyMappingHandler afterMapAny = this.AfterMapAny;
      if (afterMapAny == null)
        return;
      afterMapAny(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapComponent(
      PropertyPath member,
      IComponentAttributesMapper propertycustomizer)
    {
      ComponentMappingHandler afterMapComponent = this.AfterMapComponent;
      if (afterMapComponent == null)
        return;
      afterMapComponent(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapSet(PropertyPath member, ISetPropertiesMapper propertycustomizer)
    {
      SetMappingHandler afterMapSet = this.AfterMapSet;
      if (afterMapSet == null)
        return;
      afterMapSet(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapBag(PropertyPath member, IBagPropertiesMapper propertycustomizer)
    {
      BagMappingHandler afterMapBag = this.AfterMapBag;
      if (afterMapBag == null)
        return;
      afterMapBag(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapIdBag(PropertyPath member, IIdBagPropertiesMapper propertycustomizer)
    {
      IdBagMappingHandler afterMapIdBag = this.AfterMapIdBag;
      if (afterMapIdBag == null)
        return;
      afterMapIdBag(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapList(PropertyPath member, IListPropertiesMapper propertycustomizer)
    {
      ListMappingHandler afterMapList = this.AfterMapList;
      if (afterMapList == null)
        return;
      afterMapList(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapMap(PropertyPath member, IMapPropertiesMapper propertycustomizer)
    {
      MapMappingHandler afterMapMap = this.AfterMapMap;
      if (afterMapMap == null)
        return;
      afterMapMap(this.ModelInspector, member, propertycustomizer);
    }

    private void InvokeAfterMapManyToMany(
      PropertyPath member,
      IManyToManyMapper collectionrelationmanytomanycustomizer)
    {
      ManyToManyMappingHandler afterMapManyToMany = this.AfterMapManyToMany;
      if (afterMapManyToMany == null)
        return;
      afterMapManyToMany(this.ModelInspector, member, collectionrelationmanytomanycustomizer);
    }

    private void InvokeAfterMapElement(
      PropertyPath member,
      IElementMapper collectionrelationelementcustomizer)
    {
      ElementMappingHandler afterMapElement = this.AfterMapElement;
      if (afterMapElement == null)
        return;
      afterMapElement(this.ModelInspector, member, collectionrelationelementcustomizer);
    }

    private void InvokeAfterMapOneToMany(
      PropertyPath member,
      IOneToManyMapper collectionrelationonetomanycustomizer)
    {
      OneToManyMappingHandler afterMapOneToMany = this.AfterMapOneToMany;
      if (afterMapOneToMany == null)
        return;
      afterMapOneToMany(this.ModelInspector, member, collectionrelationonetomanycustomizer);
    }

    private void InvokeAfterMapMapKeyManyToMany(
      PropertyPath member,
      IMapKeyManyToManyMapper mapkeymanytomanycustomizer)
    {
      MapKeyManyToManyMappingHandler mapKeyManyToMany = this.AfterMapMapKeyManyToMany;
      if (mapKeyManyToMany == null)
        return;
      mapKeyManyToMany(this.ModelInspector, member, mapkeymanytomanycustomizer);
    }

    private void InvokeAfterMapMapKey(PropertyPath member, IMapKeyMapper mapkeyelementcustomizer)
    {
      MapKeyMappingHandler afterMapMapKey = this.AfterMapMapKey;
      if (afterMapMapKey == null)
        return;
      afterMapMapKey(this.ModelInspector, member, mapkeyelementcustomizer);
    }

    public IModelInspector ModelInspector => this.modelInspector;

    protected ICandidatePersistentMembersProvider MembersProvider => this.membersProvider;

    public void Class<TRootEntity>(Action<IClassMapper<TRootEntity>> customizeAction) where TRootEntity : class
    {
      ClassCustomizer<TRootEntity> classCustomizer = new ClassCustomizer<TRootEntity>(this.explicitDeclarationsHolder, this.customizerHolder);
      customizeAction((IClassMapper<TRootEntity>) classCustomizer);
    }

    public void Subclass<TEntity>(Action<ISubclassMapper<TEntity>> customizeAction) where TEntity : class
    {
      SubclassCustomizer<TEntity> subclassCustomizer = new SubclassCustomizer<TEntity>(this.explicitDeclarationsHolder, this.customizerHolder);
      customizeAction((ISubclassMapper<TEntity>) subclassCustomizer);
    }

    public void JoinedSubclass<TEntity>(
      Action<IJoinedSubclassMapper<TEntity>> customizeAction)
      where TEntity : class
    {
      JoinedSubclassCustomizer<TEntity> subclassCustomizer = new JoinedSubclassCustomizer<TEntity>(this.explicitDeclarationsHolder, this.customizerHolder);
      customizeAction((IJoinedSubclassMapper<TEntity>) subclassCustomizer);
    }

    public void UnionSubclass<TEntity>(
      Action<IUnionSubclassMapper<TEntity>> customizeAction)
      where TEntity : class
    {
      UnionSubclassCustomizer<TEntity> subclassCustomizer = new UnionSubclassCustomizer<TEntity>(this.explicitDeclarationsHolder, this.customizerHolder);
      customizeAction((IUnionSubclassMapper<TEntity>) subclassCustomizer);
    }

    public void Component<TComponent>(
      Action<IComponentMapper<TComponent>> customizeAction)
      where TComponent : class
    {
      ComponentCustomizer<TComponent> componentCustomizer = new ComponentCustomizer<TComponent>(this.explicitDeclarationsHolder, this.customizerHolder);
      customizeAction((IComponentMapper<TComponent>) componentCustomizer);
    }

    public HbmMapping CompileMappingFor(IEnumerable<Type> types)
    {
      HashSet<Type> source = types != null ? new HashSet<Type>(types) : throw new ArgumentNullException(nameof (types));
      string str1 = (string) null;
      string str2 = (string) null;
      Type firstType = source.FirstOrDefault<Type>();
      if (firstType != null && source.All<Type>((Func<Type, bool>) (t => t.Assembly.Equals((object) firstType.Assembly))))
        str1 = firstType.Assembly.GetName().Name;
      if (firstType != null && source.All<Type>((Func<Type, bool>) (t => t.Namespace.Equals(firstType.Namespace))))
        str2 = firstType.Namespace;
      HbmMapping mapping = new HbmMapping()
      {
        assembly = str1,
        @namespace = str2
      };
      foreach (Type rootClass in this.RootClasses((IEnumerable<Type>) source))
        this.MapRootClass(rootClass, mapping);
      foreach (Type subclass in this.Subclasses((IEnumerable<Type>) source))
        this.AddSubclassMapping(mapping, subclass);
      return mapping;
    }

    public IEnumerable<HbmMapping> CompileMappingForEach(IEnumerable<Type> types)
    {
      HashSet<Type> typeToMap = types != null ? new HashSet<Type>(types) : throw new ArgumentNullException(nameof (types));
      foreach (Type type in this.RootClasses((IEnumerable<Type>) typeToMap))
      {
        HbmMapping mapping = new HbmMapping()
        {
          assembly = type.Assembly.GetName().Name,
          @namespace = type.Namespace
        };
        this.MapRootClass(type, mapping);
        yield return mapping;
      }
      foreach (Type type in this.Subclasses((IEnumerable<Type>) typeToMap))
      {
        HbmMapping mapping = new HbmMapping()
        {
          assembly = type.Assembly.GetName().Name,
          @namespace = type.Namespace
        };
        this.AddSubclassMapping(mapping, type);
        yield return mapping;
      }
    }

    private IEnumerable<Type> Subclasses(IEnumerable<Type> types)
    {
      return types.Where<Type>((Func<Type, bool>) (type => this.modelInspector.IsEntity(type) && !this.modelInspector.IsRootEntity(type)));
    }

    private IEnumerable<Type> RootClasses(IEnumerable<Type> types)
    {
      return types.Where<Type>((Func<Type, bool>) (type => this.modelInspector.IsEntity(type) && this.modelInspector.IsRootEntity(type)));
    }

    private void AddSubclassMapping(HbmMapping mapping, Type type)
    {
      if (this.modelInspector.IsTablePerClassHierarchy(type))
        this.MapSubclass(type, mapping);
      else if (this.modelInspector.IsTablePerClass(type))
      {
        this.MapJoinedSubclass(type, mapping);
      }
      else
      {
        if (!this.modelInspector.IsTablePerConcreteClass(type))
          return;
        this.MapUnionSubclass(type, mapping);
      }
    }

    private void MapUnionSubclass(Type type, HbmMapping mapping)
    {
      UnionSubclassMapper unionSubclassMapper = new UnionSubclassMapper(type, mapping);
      IEnumerable<MemberInfo> memberInfos = (IEnumerable<MemberInfo>) null;
      if (!this.modelInspector.IsEntity(type.BaseType))
      {
        Type entityBaseType = this.GetEntityBaseType(type);
        if (entityBaseType != null)
        {
          unionSubclassMapper.Extends(entityBaseType);
          memberInfos = this.membersProvider.GetSubEntityMembers(type, entityBaseType);
        }
      }
      IEnumerable<MemberInfo> propertiesToMap = (memberInfos ?? this.membersProvider.GetSubEntityMembers(type, type.BaseType)).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsPersistentProperty(p) && !this.modelInspector.IsPersistentId(p)));
      this.InvokeBeforeMapUnionSubclass(type, (IUnionSubclassAttributesMapper) unionSubclassMapper);
      this.customizerHolder.InvokeCustomizers(type, (IUnionSubclassAttributesMapper) unionSubclassMapper);
      this.InvokeAfterMapUnionSubclass(type, (IUnionSubclassAttributesMapper) unionSubclassMapper);
      this.MapProperties(type, propertiesToMap, (IPropertyContainerMapper) unionSubclassMapper);
    }

    private void MapSubclass(Type type, HbmMapping mapping)
    {
      SubclassMapper subclassMapper = new SubclassMapper(type, mapping);
      IEnumerable<MemberInfo> memberInfos1 = (IEnumerable<MemberInfo>) null;
      if (!this.modelInspector.IsEntity(type.BaseType))
      {
        Type entityBaseType = this.GetEntityBaseType(type);
        if (entityBaseType != null)
        {
          subclassMapper.Extends(entityBaseType);
          memberInfos1 = this.membersProvider.GetSubEntityMembers(type, entityBaseType);
        }
      }
      IEnumerable<MemberInfo> source = memberInfos1 ?? this.membersProvider.GetSubEntityMembers(type, type.BaseType);
      this.InvokeBeforeMapSubclass(type, (ISubclassAttributesMapper) subclassMapper);
      this.customizerHolder.InvokeCustomizers(type, (ISubclassMapper) subclassMapper);
      foreach (IJoinMapper mapper in subclassMapper.JoinMappers.Values)
        this.customizerHolder.InvokeCustomizers(type, (IJoinAttributesMapper) mapper);
      IEnumerable<string> propertiesSplits = this.modelInspector.GetPropertiesSplits(type);
      IEnumerable<MemberInfo> memberInfos2 = source.Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsPersistentProperty(p) && !this.modelInspector.IsPersistentId(p)));
      HashSet<MemberInfo> second = new HashSet<MemberInfo>();
      foreach (string str in propertiesSplits)
      {
        string groupId = str;
        List<MemberInfo> list = memberInfos2.Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsTablePerClassSplit(type, (object) groupId, p))).ToList<MemberInfo>();
        IJoinMapper propertiesContainer;
        if (list.Count > 0 && subclassMapper.JoinMappers.TryGetValue(groupId, out propertiesContainer))
        {
          this.MapSplitProperties(type, (IEnumerable<MemberInfo>) list, propertiesContainer);
          second.UnionWith((IEnumerable<MemberInfo>) list);
        }
      }
      this.MapProperties(type, memberInfos2.Except<MemberInfo>((IEnumerable<MemberInfo>) second), (IPropertyContainerMapper) subclassMapper);
      this.InvokeAfterMapSubclass(type, (ISubclassAttributesMapper) subclassMapper);
    }

    private void MapJoinedSubclass(Type type, HbmMapping mapping)
    {
      JoinedSubclassMapper joinedSubclassMapper = new JoinedSubclassMapper(type, mapping);
      IEnumerable<MemberInfo> memberInfos = (IEnumerable<MemberInfo>) null;
      if (!this.modelInspector.IsEntity(type.BaseType))
      {
        Type baseType = this.GetEntityBaseType(type);
        if (baseType != null)
        {
          joinedSubclassMapper.Extends(baseType);
          joinedSubclassMapper.Key((Action<IKeyMapper>) (km => km.Column(baseType.Name.ToLowerInvariant() + "_key")));
          memberInfos = this.membersProvider.GetSubEntityMembers(type, baseType);
        }
      }
      IEnumerable<MemberInfo> propertiesToMap = (memberInfos ?? this.membersProvider.GetSubEntityMembers(type, type.BaseType)).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsPersistentProperty(p) && !this.modelInspector.IsPersistentId(p)));
      this.InvokeBeforeMapJoinedSubclass(type, (IJoinedSubclassAttributesMapper) joinedSubclassMapper);
      this.customizerHolder.InvokeCustomizers(type, (IJoinedSubclassAttributesMapper) joinedSubclassMapper);
      this.InvokeAfterMapJoinedSubclass(type, (IJoinedSubclassAttributesMapper) joinedSubclassMapper);
      this.MapProperties(type, propertiesToMap, (IPropertyContainerMapper) joinedSubclassMapper);
    }

    private Type GetEntityBaseType(Type type)
    {
      Type type1 = type;
      while (type1 != null && type1 != typeof (object))
      {
        type1 = type1.BaseType;
        if (this.modelInspector.IsEntity(type1))
          return type1;
      }
      return ((IEnumerable<Type>) type.GetInterfaces()).FirstOrDefault<Type>((Func<Type, bool>) (i => this.modelInspector.IsEntity(i)));
    }

    private void MapRootClass(Type type, HbmMapping mapping)
    {
      MemberInfo poidPropertyOrField = this.membersProvider.GetEntityMembersForPoid(type).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (mi => this.modelInspector.IsPersistentId(mi)));
      ClassMapper classMapper = new ClassMapper(type, mapping, poidPropertyOrField);
      if (this.modelInspector.IsTablePerClassHierarchy(type))
        classMapper.Discriminator((Action<IDiscriminatorMapper>) (x => { }));
      MemberInfo[] array = this.membersProvider.GetRootEntityMembers(type).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsPersistentProperty(p) && !this.modelInspector.IsPersistentId(p))).ToArray<MemberInfo>();
      this.InvokeBeforeMapClass(type, (IClassAttributesMapper) classMapper);
      this.InvokeClassCustomizers(type, classMapper);
      if (poidPropertyOrField != null && this.modelInspector.IsComponent(poidPropertyOrField.GetPropertyOrFieldType()))
        classMapper.ComponentAsId(poidPropertyOrField, (Action<IComponentAsIdMapper>) (compoAsId =>
        {
          PropertyPath propertyPath = new PropertyPath((PropertyPath) null, poidPropertyOrField);
          ComponentAsIdLikeComponentAttributesMapper attributesMapper = new ComponentAsIdLikeComponentAttributesMapper(compoAsId);
          this.InvokeBeforeMapComponent(propertyPath, (IComponentAttributesMapper) attributesMapper);
          Type propertyOrFieldType = poidPropertyOrField.GetPropertyOrFieldType();
          IEnumerable<MemberInfo> memberInfos = this.membersProvider.GetComponentMembers(propertyOrFieldType).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsPersistentProperty(p)));
          this.customizerHolder.InvokeCustomizers(propertyOrFieldType, (IComponentAttributesMapper) attributesMapper);
          this.ForEachMemberPath(poidPropertyOrField, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, (IComponentAsIdAttributesMapper) compoAsId)));
          this.InvokeAfterMapComponent(propertyPath, (IComponentAttributesMapper) attributesMapper);
          foreach (MemberInfo localMember in memberInfos)
            this.MapComposedIdProperties((IMinimalPlainPropertyContainerMapper) compoAsId, new PropertyPath(propertyPath, localMember));
        }));
      MemberInfo[] composedIdPropeties = ((IEnumerable<MemberInfo>) array).Where<MemberInfo>((Func<MemberInfo, bool>) (mi => this.modelInspector.IsMemberOfComposedId(mi))).ToArray<MemberInfo>();
      if (composedIdPropeties.Length > 0)
        classMapper.ComposedId((Action<IComposedIdMapper>) (composedIdMapper =>
        {
          foreach (MemberInfo localMember in composedIdPropeties)
            this.MapComposedIdProperties((IMinimalPlainPropertyContainerMapper) composedIdMapper, new PropertyPath((PropertyPath) null, localMember));
        }));
      MemberInfo[] naturalIdPropeties = ((IEnumerable<MemberInfo>) array).Except<MemberInfo>((IEnumerable<MemberInfo>) composedIdPropeties).Where<MemberInfo>((Func<MemberInfo, bool>) (mi => this.modelInspector.IsMemberOfNaturalId(mi))).ToArray<MemberInfo>();
      if (naturalIdPropeties.Length > 0)
        classMapper.NaturalId((Action<INaturalIdMapper>) (naturalIdMapper =>
        {
          foreach (MemberInfo property in naturalIdPropeties)
            this.MapNaturalIdProperties(type, naturalIdMapper, property);
        }));
      IEnumerable<string> propertiesSplits = this.modelInspector.GetPropertiesSplits(type);
      List<MemberInfo> list1 = ((IEnumerable<MemberInfo>) array).Except<MemberInfo>((IEnumerable<MemberInfo>) naturalIdPropeties).Except<MemberInfo>((IEnumerable<MemberInfo>) composedIdPropeties).Where<MemberInfo>((Func<MemberInfo, bool>) (mi => !this.modelInspector.IsVersion(mi) && !this.modelInspector.IsVersion(mi.GetMemberFromDeclaringType()))).ToList<MemberInfo>();
      HashSet<MemberInfo> second = new HashSet<MemberInfo>();
      foreach (string str in propertiesSplits)
      {
        string groupId = str;
        List<MemberInfo> list2 = list1.Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsTablePerClassSplit(type, (object) groupId, p))).ToList<MemberInfo>();
        IJoinMapper propertiesContainer;
        if (list2.Count > 0 && classMapper.JoinMappers.TryGetValue(groupId, out propertiesContainer))
        {
          this.MapSplitProperties(type, (IEnumerable<MemberInfo>) list2, propertiesContainer);
          second.UnionWith((IEnumerable<MemberInfo>) list2);
        }
      }
      this.MapProperties(type, list1.Except<MemberInfo>((IEnumerable<MemberInfo>) second), (IPropertyContainerMapper) classMapper);
      this.InvokeAfterMapClass(type, (IClassAttributesMapper) classMapper);
    }

    private void MapSplitProperties(
      Type propertiesContainerType,
      IEnumerable<MemberInfo> propertiesToMap,
      IJoinMapper propertiesContainer)
    {
      foreach (MemberInfo propertiesTo in propertiesToMap)
      {
        MemberInfo memberInfo = propertiesTo;
        Type propertyOrFieldType = propertiesTo.GetPropertyOrFieldType();
        PropertyPath propertyPath = new PropertyPath((PropertyPath) null, memberInfo);
        if (this.modelInspector.IsProperty(memberInfo))
          this.MapProperty(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) propertiesContainer);
        else if (this.modelInspector.IsAny(memberInfo))
          this.MapAny(memberInfo, propertyPath, (IBasePlainPropertyContainerMapper) propertiesContainer);
        else if (this.modelInspector.IsManyToOne(propertiesTo))
          this.MapManyToOne(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) propertiesContainer);
        else if (this.modelInspector.IsSet(propertiesTo))
          this.MapSet(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
        else if (this.modelInspector.IsDictionary(propertiesTo))
        {
          this.MapDictionary(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
        }
        else
        {
          if (this.modelInspector.IsArray(propertiesTo))
            throw new NotSupportedException();
          if (this.modelInspector.IsList(propertiesTo))
            this.MapList(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
          else if (this.modelInspector.IsIdBag(propertiesTo))
            this.MapIdBag(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
          else if (this.modelInspector.IsBag(propertiesTo))
            this.MapBag(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
          else if (this.modelInspector.IsComponent(propertyOrFieldType))
            this.MapComponent(memberInfo, propertyPath, propertyOrFieldType, (IBasePlainPropertyContainerMapper) propertiesContainer, propertiesContainerType);
          else
            this.MapProperty(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) propertiesContainer);
        }
      }
    }

    private void InvokeClassCustomizers(Type type, ClassMapper classMapper)
    {
      this.InvokeAncestorsCustomizers((IEnumerable<Type>) type.GetInterfaces(), (IClassMapper) classMapper);
      this.InvokeAncestorsCustomizers(type.GetHierarchyFromBase(), (IClassMapper) classMapper);
      this.customizerHolder.InvokeCustomizers(type, (IClassMapper) classMapper);
      foreach (IJoinMapper mapper in classMapper.JoinMappers.Values)
        this.customizerHolder.InvokeCustomizers(type, (IJoinAttributesMapper) mapper);
    }

    private void InvokeAncestorsCustomizers(
      IEnumerable<Type> typeAncestors,
      IClassMapper classMapper)
    {
      foreach (Type type in typeAncestors.Where<Type>((Func<Type, bool>) (t => !this.modelInspector.IsEntity(t))))
        this.customizerHolder.InvokeCustomizers(type, classMapper);
    }

    private void MapComposedIdProperties(
      IMinimalPlainPropertyContainerMapper composedIdMapper,
      PropertyPath propertyPath)
    {
      MemberInfo localMember = propertyPath.LocalMember;
      Type propertyOrFieldType = localMember.GetPropertyOrFieldType();
      PropertyPath propertyPath1 = propertyPath;
      if (this.modelInspector.IsProperty(localMember))
        this.MapProperty(localMember, propertyPath1, composedIdMapper);
      else if (this.modelInspector.IsManyToOne(localMember))
      {
        this.MapManyToOne(localMember, propertyPath1, composedIdMapper);
      }
      else
      {
        if (this.modelInspector.IsAny(localMember) || this.modelInspector.IsComponent(propertyOrFieldType) || this.modelInspector.IsOneToOne(localMember) || this.modelInspector.IsSet(localMember) || this.modelInspector.IsDictionary(localMember) || this.modelInspector.IsArray(localMember) || this.modelInspector.IsList(localMember) || this.modelInspector.IsBag(localMember))
          throw new ArgumentOutOfRangeException(nameof (propertyPath), string.Format("The property {0} of {1} can't be part of composite-id.", (object) localMember.Name, (object) localMember.DeclaringType));
        this.MapProperty(localMember, propertyPath1, composedIdMapper);
      }
    }

    private void MapNaturalIdProperties(
      Type rootEntityType,
      INaturalIdMapper naturalIdMapper,
      MemberInfo property)
    {
      MemberInfo memberInfo = property;
      Type propertyOrFieldType = property.GetPropertyOrFieldType();
      PropertyPath propertyPath = new PropertyPath((PropertyPath) null, memberInfo);
      if (this.modelInspector.IsProperty(memberInfo))
        this.MapProperty(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) naturalIdMapper);
      else if (this.modelInspector.IsAny(memberInfo))
        this.MapAny(memberInfo, propertyPath, (IBasePlainPropertyContainerMapper) naturalIdMapper);
      else if (this.modelInspector.IsManyToOne(memberInfo))
        this.MapManyToOne(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) naturalIdMapper);
      else if (this.modelInspector.IsComponent(propertyOrFieldType))
      {
        this.MapComponent(memberInfo, propertyPath, propertyOrFieldType, (IBasePlainPropertyContainerMapper) naturalIdMapper, rootEntityType);
      }
      else
      {
        if (this.modelInspector.IsOneToOne(memberInfo) || this.modelInspector.IsSet(property) || this.modelInspector.IsDictionary(property) || this.modelInspector.IsArray(property) || this.modelInspector.IsList(property) || this.modelInspector.IsBag(property))
          throw new ArgumentOutOfRangeException(nameof (property), string.Format("The property {0} of {1} can't be part of natural-id.", (object) property.Name, (object) property.DeclaringType));
        this.MapProperty(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) naturalIdMapper);
      }
    }

    private void MapProperties(
      Type propertiesContainerType,
      IEnumerable<MemberInfo> propertiesToMap,
      IPropertyContainerMapper propertiesContainer)
    {
      this.MapProperties(propertiesContainerType, propertiesToMap, propertiesContainer, (PropertyPath) null);
    }

    private void MapProperties(
      Type propertiesContainerType,
      IEnumerable<MemberInfo> propertiesToMap,
      IPropertyContainerMapper propertiesContainer,
      PropertyPath path)
    {
      foreach (MemberInfo propertiesTo in propertiesToMap)
      {
        MemberInfo memberInfo = propertiesTo;
        Type propertyOrFieldType = propertiesTo.GetPropertyOrFieldType();
        PropertyPath propertyPath = new PropertyPath(path, memberInfo);
        if (this.modelInspector.IsProperty(memberInfo))
          this.MapProperty(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) propertiesContainer);
        else if (this.modelInspector.IsAny(memberInfo))
          this.MapAny(memberInfo, propertyPath, (IBasePlainPropertyContainerMapper) propertiesContainer);
        else if (this.modelInspector.IsManyToOne(propertiesTo))
          this.MapManyToOne(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) propertiesContainer);
        else if (this.modelInspector.IsOneToOne(propertiesTo))
          this.MapOneToOne(memberInfo, propertyPath, (IPlainPropertyContainerMapper) propertiesContainer);
        else if (this.modelInspector.IsDynamicComponent(propertiesTo))
          this.MapDynamicComponent(memberInfo, propertyPath, propertyOrFieldType, propertiesContainer);
        else if (this.modelInspector.IsSet(propertiesTo))
          this.MapSet(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
        else if (this.modelInspector.IsDictionary(propertiesTo))
        {
          this.MapDictionary(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
        }
        else
        {
          if (this.modelInspector.IsArray(propertiesTo))
            throw new NotSupportedException();
          if (this.modelInspector.IsList(propertiesTo))
            this.MapList(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
          else if (this.modelInspector.IsIdBag(propertiesTo))
            this.MapIdBag(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
          else if (this.modelInspector.IsBag(propertiesTo))
            this.MapBag(memberInfo, propertyPath, propertyOrFieldType, (ICollectionPropertiesContainerMapper) propertiesContainer, propertiesContainerType);
          else if (this.modelInspector.IsComponent(propertyOrFieldType))
            this.MapComponent(memberInfo, propertyPath, propertyOrFieldType, (IBasePlainPropertyContainerMapper) propertiesContainer, propertiesContainerType);
          else
            this.MapProperty(memberInfo, propertyPath, (IMinimalPlainPropertyContainerMapper) propertiesContainer);
        }
      }
    }

    private void MapDynamicComponent(
      MemberInfo member,
      PropertyPath memberPath,
      Type propertyType,
      IPropertyContainerMapper propertiesContainer)
    {
      propertiesContainer.Component(member, (Action<IDynamicComponentMapper>) (componentMapper =>
      {
        IEnumerable<MemberInfo> componentMembers = this.membersProvider.GetComponentMembers(this.modelInspector.GetDynamicComponentTemplate(member));
        this.ForEachMemberPath(member, memberPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, (IDynamicComponentAttributesMapper) componentMapper)));
        this.MapProperties(propertyType, componentMembers, (IPropertyContainerMapper) componentMapper, memberPath);
      }));
    }

    private void MapAny(
      MemberInfo member,
      PropertyPath memberPath,
      IBasePlainPropertyContainerMapper propertiesContainer)
    {
      propertiesContainer.Any(member, typeof (int), (Action<IAnyMapper>) (anyMapper =>
      {
        this.InvokeBeforeMapAny(memberPath, anyMapper);
        MemberInfo propertyOrField = this.membersProvider.GetEntityMembersForPoid(memberPath.GetRootMember().DeclaringType).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (mi => this.modelInspector.IsPersistentId(mi)));
        if (propertyOrField != null)
          anyMapper.IdType(propertyOrField.GetPropertyOrFieldType());
        this.ForEachMemberPath(member, memberPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, anyMapper)));
        this.InvokeAfterMapAny(memberPath, anyMapper);
      }));
    }

    private void MapProperty(
      MemberInfo member,
      PropertyPath propertyPath,
      IMinimalPlainPropertyContainerMapper propertiesContainer)
    {
      propertiesContainer.Property(member, (Action<IPropertyMapper>) (propertyMapper =>
      {
        this.InvokeBeforeMapProperty(propertyPath, propertyMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, propertyMapper)));
        this.InvokeAfterMapProperty(propertyPath, propertyMapper);
      }));
    }

    protected void ForEachMemberPath(
      MemberInfo member,
      PropertyPath progressivePath,
      Action<PropertyPath> invoke)
    {
      HashSet<PropertyPath> propertyPathSet = new HashSet<PropertyPath>();
      foreach (MemberInfo propertyFromInterface in member.GetPropertyFromInterfaces())
      {
        PropertyPath propertyPath = new PropertyPath((PropertyPath) null, propertyFromInterface);
        invoke(propertyPath);
      }
      PropertyPath propertyPath1 = new PropertyPath((PropertyPath) null, member.GetMemberFromDeclaringType());
      PropertyPath other = new PropertyPath((PropertyPath) null, member);
      invoke(propertyPath1);
      propertyPathSet.Add(propertyPath1);
      if (!propertyPath1.Equals(other))
      {
        invoke(other);
        propertyPathSet.Add(other);
      }
      foreach (PropertyPath propertyPath2 in progressivePath.InverseProgressivePath())
      {
        if (!propertyPathSet.Contains(propertyPath2))
        {
          invoke(propertyPath2);
          propertyPathSet.Add(propertyPath2);
        }
      }
    }

    private void MapComponent(
      MemberInfo member,
      PropertyPath memberPath,
      Type propertyType,
      IBasePlainPropertyContainerMapper propertiesContainer,
      Type propertiesContainerType)
    {
      propertiesContainer.Component(member, (Action<IComponentMapper>) (componentMapper =>
      {
        this.InvokeBeforeMapComponent(memberPath, (IComponentAttributesMapper) componentMapper);
        Type type = propertyType;
        IEnumerable<MemberInfo> memberInfos = this.membersProvider.GetComponentMembers(type).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.modelInspector.IsPersistentProperty(p)));
        MemberInfo parentReferenceProperty = this.GetComponentParentReferenceProperty(memberInfos, propertiesContainerType);
        if (parentReferenceProperty != null)
          componentMapper.Parent(parentReferenceProperty);
        this.customizerHolder.InvokeCustomizers(type, (IComponentAttributesMapper) componentMapper);
        this.ForEachMemberPath(member, memberPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, (IComponentAttributesMapper) componentMapper)));
        this.InvokeAfterMapComponent(memberPath, (IComponentAttributesMapper) componentMapper);
        this.MapProperties(propertyType, memberInfos.Where<MemberInfo>((Func<MemberInfo, bool>) (pi => pi != parentReferenceProperty)), (IPropertyContainerMapper) componentMapper, memberPath);
      }));
    }

    protected MemberInfo GetComponentParentReferenceProperty(
      IEnumerable<MemberInfo> persistentProperties,
      Type propertiesContainerType)
    {
      return !this.modelInspector.IsComponent(propertiesContainerType) ? (MemberInfo) null : persistentProperties.FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (pp => pp.GetPropertyOrFieldType() == propertiesContainerType));
    }

    private void MapBag(
      MemberInfo member,
      PropertyPath propertyPath,
      Type propertyType,
      ICollectionPropertiesContainerMapper propertiesContainer,
      Type propertiesContainerType)
    {
      Type elementTypeOrThrow = this.GetCollectionElementTypeOrThrow(propertiesContainerType, member, propertyType);
      ModelMapper.ICollectionElementRelationMapper cert = this.DetermineCollectionElementRelationType(member, propertyPath, elementTypeOrThrow);
      propertiesContainer.Bag(member, (Action<IBagPropertiesMapper>) (collectionPropertiesMapper =>
      {
        cert.MapCollectionProperties((ICollectionPropertiesMapper) collectionPropertiesMapper);
        this.InvokeBeforeMapBag(propertyPath, collectionPropertiesMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, collectionPropertiesMapper)));
        this.InvokeAfterMapBag(propertyPath, collectionPropertiesMapper);
      }), new Action<ICollectionElementRelation>(cert.Map));
    }

    private void MapList(
      MemberInfo member,
      PropertyPath propertyPath,
      Type propertyType,
      ICollectionPropertiesContainerMapper propertiesContainer,
      Type propertiesContainerType)
    {
      Type elementTypeOrThrow = this.GetCollectionElementTypeOrThrow(propertiesContainerType, member, propertyType);
      ModelMapper.ICollectionElementRelationMapper cert = this.DetermineCollectionElementRelationType(member, propertyPath, elementTypeOrThrow);
      propertiesContainer.List(member, (Action<IListPropertiesMapper>) (collectionPropertiesMapper =>
      {
        cert.MapCollectionProperties((ICollectionPropertiesMapper) collectionPropertiesMapper);
        this.InvokeBeforeMapList(propertyPath, collectionPropertiesMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, collectionPropertiesMapper)));
        this.InvokeAfterMapList(propertyPath, collectionPropertiesMapper);
      }), new Action<ICollectionElementRelation>(cert.Map));
    }

    private void MapDictionary(
      MemberInfo member,
      PropertyPath propertyPath,
      Type propertyType,
      ICollectionPropertiesContainerMapper propertiesContainer,
      Type propertiesContainerType)
    {
      ModelMapper.IMapKeyRelationMapper mapKeyRelationType = this.DetermineMapKeyRelationType(member, propertyPath, propertyType.DetermineDictionaryKeyType() ?? throw new NotSupportedException(string.Format("Can't determine collection element relation (property {0} in {1})", (object) member.Name, (object) propertiesContainerType)));
      Type dictionaryValueType = propertyType.DetermineDictionaryValueType();
      ModelMapper.ICollectionElementRelationMapper cert = this.DetermineCollectionElementRelationType(member, propertyPath, dictionaryValueType);
      propertiesContainer.Map(member, (Action<IMapPropertiesMapper>) (collectionPropertiesMapper =>
      {
        cert.MapCollectionProperties((ICollectionPropertiesMapper) collectionPropertiesMapper);
        this.InvokeBeforeMapMap(propertyPath, collectionPropertiesMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, collectionPropertiesMapper)));
        this.InvokeAfterMapMap(propertyPath, collectionPropertiesMapper);
      }), new Action<IMapKeyRelation>(mapKeyRelationType.Map), new Action<ICollectionElementRelation>(cert.Map));
    }

    private void MapSet(
      MemberInfo member,
      PropertyPath propertyPath,
      Type propertyType,
      ICollectionPropertiesContainerMapper propertiesContainer,
      Type propertiesContainerType)
    {
      Type elementTypeOrThrow = this.GetCollectionElementTypeOrThrow(propertiesContainerType, member, propertyType);
      ModelMapper.ICollectionElementRelationMapper cert = this.DetermineCollectionElementRelationType(member, propertyPath, elementTypeOrThrow);
      propertiesContainer.Set(member, (Action<ISetPropertiesMapper>) (collectionPropertiesMapper =>
      {
        cert.MapCollectionProperties((ICollectionPropertiesMapper) collectionPropertiesMapper);
        this.InvokeBeforeMapSet(propertyPath, collectionPropertiesMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, collectionPropertiesMapper)));
        this.InvokeAfterMapSet(propertyPath, collectionPropertiesMapper);
      }), new Action<ICollectionElementRelation>(cert.Map));
    }

    private void MapIdBag(
      MemberInfo member,
      PropertyPath propertyPath,
      Type propertyType,
      ICollectionPropertiesContainerMapper propertiesContainer,
      Type propertiesContainerType)
    {
      Type elementTypeOrThrow = this.GetCollectionElementTypeOrThrow(propertiesContainerType, member, propertyType);
      ModelMapper.ICollectionElementRelationMapper cert = this.DetermineCollectionElementRelationType(member, propertyPath, elementTypeOrThrow);
      if (cert is ModelMapper.OneToManyRelationMapper)
        throw new NotSupportedException("id-bag does not suppot one-to-many relation");
      propertiesContainer.IdBag(member, (Action<IIdBagPropertiesMapper>) (collectionPropertiesMapper =>
      {
        cert.MapCollectionProperties((ICollectionPropertiesMapper) collectionPropertiesMapper);
        this.InvokeBeforeMapIdBag(propertyPath, collectionPropertiesMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, collectionPropertiesMapper)));
        this.InvokeAfterMapIdBag(propertyPath, collectionPropertiesMapper);
      }), new Action<ICollectionElementRelation>(cert.Map));
    }

    private void MapOneToOne(
      MemberInfo member,
      PropertyPath propertyPath,
      IPlainPropertyContainerMapper propertiesContainer)
    {
      propertiesContainer.OneToOne(member, (Action<IOneToOneMapper>) (oneToOneMapper =>
      {
        this.InvokeBeforeMapOneToOne(propertyPath, oneToOneMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, oneToOneMapper)));
        this.InvokeAfterMapOneToOne(propertyPath, oneToOneMapper);
      }));
    }

    private void MapManyToOne(
      MemberInfo member,
      PropertyPath propertyPath,
      IMinimalPlainPropertyContainerMapper propertiesContainer)
    {
      propertiesContainer.ManyToOne(member, (Action<IManyToOneMapper>) (manyToOneMapper =>
      {
        this.InvokeBeforeMapManyToOne(propertyPath, manyToOneMapper);
        this.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizerHolder.InvokeCustomizers(pp, manyToOneMapper)));
        this.InvokeAfterMapManyToOne(propertyPath, manyToOneMapper);
      }));
    }

    private Type GetCollectionElementTypeOrThrow(Type type, MemberInfo property, Type propertyType)
    {
      return propertyType.DetermineCollectionElementType() ?? throw new NotSupportedException(string.Format("Can't determine collection element relation (property {0} in {1})", (object) property.Name, (object) type));
    }

    protected virtual ModelMapper.ICollectionElementRelationMapper DetermineCollectionElementRelationType(
      MemberInfo property,
      PropertyPath propertyPath,
      Type collectionElementType)
    {
      Type reflectedType = property.ReflectedType;
      if (this.modelInspector.IsOneToMany(property))
        return (ModelMapper.ICollectionElementRelationMapper) new ModelMapper.OneToManyRelationMapper(propertyPath, reflectedType, collectionElementType, this.modelInspector, this.customizerHolder, this);
      if (this.modelInspector.IsManyToMany(property))
        return (ModelMapper.ICollectionElementRelationMapper) new ModelMapper.ManyToManyRelationMapper(propertyPath, this.customizerHolder, this);
      if (this.modelInspector.IsComponent(collectionElementType))
        return (ModelMapper.ICollectionElementRelationMapper) new ModelMapper.ComponentRelationMapper(property, reflectedType, collectionElementType, this.membersProvider, this.modelInspector, this.customizerHolder, this);
      return this.modelInspector.IsManyToAny(property) ? (ModelMapper.ICollectionElementRelationMapper) new ModelMapper.ManyToAnyRelationMapper(propertyPath, this.customizerHolder, this) : (ModelMapper.ICollectionElementRelationMapper) new ModelMapper.ElementRelationMapper(propertyPath, this.customizerHolder, this);
    }

    private ModelMapper.IMapKeyRelationMapper DetermineMapKeyRelationType(
      MemberInfo member,
      PropertyPath propertyPath,
      Type dictionaryKeyType)
    {
      if (this.modelInspector.IsEntity(dictionaryKeyType))
        return (ModelMapper.IMapKeyRelationMapper) new ModelMapper.KeyManyToManyRelationMapper(propertyPath, this.customizerHolder, this);
      return this.modelInspector.IsComponent(dictionaryKeyType) ? (ModelMapper.IMapKeyRelationMapper) new ModelMapper.KeyComponentRelationMapper(dictionaryKeyType, propertyPath, this.membersProvider, this.modelInspector, this.customizerHolder, this) : (ModelMapper.IMapKeyRelationMapper) new ModelMapper.KeyElementRelationMapper(propertyPath, this.customizerHolder, this);
    }

    public void AddMapping<T>() where T : IConformistHoldersProvider, new()
    {
      this.AddMapping((IConformistHoldersProvider) new T());
    }

    public void AddMapping(IConformistHoldersProvider mapping)
    {
      if (!(this.customizerHolder is CustomizersHolder customizerHolder))
        throw new NotSupportedException("To merge 'conformist' mappings, the instance of ICustomizersHolder, provided in the ModelMapper constructor, have to be a CustomizersHolder instance.");
      if (!(mapping.CustomizersHolder is CustomizersHolder customizersHolder))
        throw new NotSupportedException("The mapping class have to provide a CustomizersHolder instance.");
      customizerHolder.Merge(customizersHolder);
      this.explicitDeclarationsHolder.Merge(mapping.ExplicitDeclarationsHolder);
    }

    public void AddMapping(Type type)
    {
      object instance;
      try
      {
        instance = Activator.CreateInstance(type);
      }
      catch (Exception ex)
      {
        throw new MappingException("Unable to instantiate mapping class (see InnerException): " + (object) type, ex);
      }
      if (!(instance is IConformistHoldersProvider mapping))
        throw new ArgumentOutOfRangeException(nameof (type), "The mapping class must be an implementation of IConformistHoldersProvider.");
      this.AddMapping(mapping);
    }

    public void AddMappings(IEnumerable<Type> types)
    {
      if (types == null)
        throw new ArgumentNullException(nameof (types));
      foreach (Type type in types.Where<Type>((Func<Type, bool>) (x => typeof (IConformistHoldersProvider).IsAssignableFrom(x) && !x.IsGenericTypeDefinition)))
        this.AddMapping(type);
    }

    public HbmMapping CompileMappingForAllExplicitlyAddedEntities()
    {
      return this.CompileMappingFor(this.customizerHolder.GetAllCustomizedEntities());
    }

    public IEnumerable<HbmMapping> CompileMappingForEachExplicitlyAddedEntity()
    {
      return this.CompileMappingForEach(this.customizerHolder.GetAllCustomizedEntities());
    }

    protected interface ICollectionElementRelationMapper
    {
      void Map(ICollectionElementRelation relation);

      void MapCollectionProperties(ICollectionPropertiesMapper mapped);
    }

    private class ComponentRelationMapper : ModelMapper.ICollectionElementRelationMapper
    {
      private readonly MemberInfo collectionMember;
      private readonly Type componentType;
      private readonly ICustomizersHolder customizersHolder;
      private readonly IModelInspector domainInspector;
      private readonly ICandidatePersistentMembersProvider membersProvider;
      private readonly ModelMapper modelMapper;
      private readonly Type ownerType;

      public ComponentRelationMapper(
        MemberInfo collectionMember,
        Type ownerType,
        Type componentType,
        ICandidatePersistentMembersProvider membersProvider,
        IModelInspector domainInspector,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.collectionMember = collectionMember;
        this.ownerType = ownerType;
        this.componentType = componentType;
        this.membersProvider = membersProvider;
        this.domainInspector = domainInspector;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(ICollectionElementRelation relation)
      {
        relation.Component((Action<IComponentElementMapper>) (x =>
        {
          IEnumerable<MemberInfo> persistentProperties = this.GetPersistentProperties(this.componentType);
          MemberInfo parentReferenceProperty = this.modelMapper.GetComponentParentReferenceProperty(persistentProperties, this.ownerType);
          if (parentReferenceProperty != null)
            x.Parent(parentReferenceProperty);
          this.customizersHolder.InvokeCustomizers(this.componentType, (IComponentAttributesMapper) x);
          this.MapProperties(this.componentType, new PropertyPath((PropertyPath) null, this.collectionMember), x, persistentProperties.Where<MemberInfo>((Func<MemberInfo, bool>) (pi => pi != parentReferenceProperty)));
        }));
      }

      public void MapCollectionProperties(ICollectionPropertiesMapper mapped)
      {
      }

      private IEnumerable<MemberInfo> GetPersistentProperties(Type type)
      {
        return this.membersProvider.GetComponentMembers(type).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.domainInspector.IsPersistentProperty(p)));
      }

      private void MapProperties(
        Type type,
        PropertyPath memberPath,
        IComponentElementMapper propertiesContainer,
        IEnumerable<MemberInfo> persistentProperties)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ModelMapper.ComponentRelationMapper.\u003C\u003Ec__DisplayClass8f cDisplayClass8f1 = new ModelMapper.ComponentRelationMapper.\u003C\u003Ec__DisplayClass8f();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass8f1.type = type;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass8f1.\u003C\u003E4__this = this;
        foreach (MemberInfo persistentProperty in persistentProperties)
        {
          MemberInfo member = persistentProperty;
          Type propertyType = persistentProperty.GetPropertyOrFieldType();
          PropertyPath propertyPath = new PropertyPath(memberPath, member);
          if (this.domainInspector.IsManyToOne(member))
            propertiesContainer.ManyToOne(member, (Action<IManyToOneMapper>) (manyToOneMapper =>
            {
              // ISSUE: variable of a compiler-generated type
              ModelMapper.ComponentRelationMapper.\u003C\u003Ec__DisplayClass8f cDisplayClass8f = cDisplayClass8f1;
              IManyToOneMapper manyToOneMapper1 = manyToOneMapper;
              this.modelMapper.InvokeBeforeMapManyToOne(propertyPath, manyToOneMapper1);
              // ISSUE: reference to a compiler-generated field
              this.modelMapper.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => cDisplayClass8f.\u003C\u003E4__this.customizersHolder.InvokeCustomizers(pp, manyToOneMapper1)));
              this.modelMapper.InvokeAfterMapManyToOne(propertyPath, manyToOneMapper1);
            }));
          else if (this.domainInspector.IsComponent(propertyType))
            propertiesContainer.Component(member, (Action<IComponentElementMapper>) (x =>
            {
              // ISSUE: variable of a compiler-generated type
              ModelMapper.ComponentRelationMapper.\u003C\u003Ec__DisplayClass8f cDisplayClass8f = cDisplayClass8f1;
              IComponentElementMapper x1 = x;
              this.modelMapper.InvokeBeforeMapComponent(propertyPath, (IComponentAttributesMapper) x1);
              Type propertiesContainerType = type;
              Type type1 = propertyType;
              IEnumerable<MemberInfo> persistentProperties1 = this.GetPersistentProperties(type1);
              MemberInfo parentReferenceProperty = this.modelMapper.GetComponentParentReferenceProperty(persistentProperties1, propertiesContainerType);
              if (parentReferenceProperty != null)
                x1.Parent(parentReferenceProperty);
              this.customizersHolder.InvokeCustomizers(type1, (IComponentAttributesMapper) x1);
              // ISSUE: reference to a compiler-generated field
              this.modelMapper.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => cDisplayClass8f.\u003C\u003E4__this.customizersHolder.InvokeCustomizers(pp, (IComponentAttributesMapper) x1)));
              this.modelMapper.InvokeAfterMapComponent(propertyPath, (IComponentAttributesMapper) x1);
              this.MapProperties(type1, propertyPath, x1, persistentProperties1.Where<MemberInfo>((Func<MemberInfo, bool>) (pi => pi != parentReferenceProperty)));
            }));
          else
            propertiesContainer.Property(member, (Action<IPropertyMapper>) (propertyMapper =>
            {
              // ISSUE: variable of a compiler-generated type
              ModelMapper.ComponentRelationMapper.\u003C\u003Ec__DisplayClass8f cDisplayClass8f = cDisplayClass8f1;
              IPropertyMapper propertyMapper1 = propertyMapper;
              this.modelMapper.InvokeBeforeMapProperty(propertyPath, propertyMapper1);
              // ISSUE: reference to a compiler-generated field
              this.modelMapper.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => cDisplayClass8f.\u003C\u003E4__this.customizersHolder.InvokeCustomizers(pp, propertyMapper1)));
              this.modelMapper.InvokeAfterMapProperty(propertyPath, propertyMapper1);
            }));
        }
      }
    }

    private class ElementRelationMapper : ModelMapper.ICollectionElementRelationMapper
    {
      private readonly ICustomizersHolder customizersHolder;
      private readonly ModelMapper modelMapper;
      private readonly PropertyPath propertyPath;

      public ElementRelationMapper(
        PropertyPath propertyPath,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.propertyPath = propertyPath;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(ICollectionElementRelation relation)
      {
        relation.Element((Action<IElementMapper>) (x =>
        {
          this.modelMapper.InvokeBeforeMapElement(this.propertyPath, x);
          this.customizersHolder.InvokeCustomizers(this.propertyPath, x);
          this.modelMapper.InvokeAfterMapElement(this.propertyPath, x);
        }));
      }

      public void MapCollectionProperties(ICollectionPropertiesMapper mapped)
      {
      }
    }

    protected interface IMapKeyRelationMapper
    {
      void Map(IMapKeyRelation relation);
    }

    private class KeyComponentRelationMapper : ModelMapper.IMapKeyRelationMapper
    {
      private readonly ICustomizersHolder customizersHolder;
      private readonly Type dictionaryKeyType;
      private readonly IModelInspector domainInspector;
      private readonly ICandidatePersistentMembersProvider membersProvider;
      private readonly ModelMapper modelMapper;
      private readonly PropertyPath propertyPath;

      public KeyComponentRelationMapper(
        Type dictionaryKeyType,
        PropertyPath propertyPath,
        ICandidatePersistentMembersProvider membersProvider,
        IModelInspector domainInspector,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.dictionaryKeyType = dictionaryKeyType;
        this.propertyPath = propertyPath;
        this.membersProvider = membersProvider;
        this.domainInspector = domainInspector;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(IMapKeyRelation relation)
      {
        relation.Component((Action<IComponentMapKeyMapper>) (x =>
        {
          IEnumerable<MemberInfo> persistentProperties = this.GetPersistentProperties(this.dictionaryKeyType);
          this.MapProperties(x, persistentProperties);
        }));
      }

      private IEnumerable<MemberInfo> GetPersistentProperties(Type type)
      {
        return this.membersProvider.GetComponentMembers(type).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.domainInspector.IsPersistentProperty(p)));
      }

      private void MapProperties(
        IComponentMapKeyMapper propertiesContainer,
        IEnumerable<MemberInfo> persistentProperties)
      {
        foreach (MemberInfo persistentProperty in persistentProperties)
        {
          MemberInfo member = persistentProperty;
          if (this.domainInspector.IsManyToOne(member))
            propertiesContainer.ManyToOne(member, (Action<IManyToOneMapper>) (manyToOneMapper =>
            {
              PropertyPath propertyPath = new PropertyPath(this.propertyPath, member);
              this.modelMapper.InvokeBeforeMapManyToOne(propertyPath, manyToOneMapper);
              this.modelMapper.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizersHolder.InvokeCustomizers(pp, manyToOneMapper)));
              this.modelMapper.InvokeAfterMapManyToOne(propertyPath, manyToOneMapper);
            }));
          else
            propertiesContainer.Property(member, (Action<IPropertyMapper>) (propertyMapper =>
            {
              PropertyPath propertyPath = new PropertyPath(this.propertyPath, member);
              this.modelMapper.InvokeBeforeMapProperty(propertyPath, propertyMapper);
              this.modelMapper.ForEachMemberPath(member, propertyPath, (Action<PropertyPath>) (pp => this.customizersHolder.InvokeCustomizers(pp, propertyMapper)));
              this.modelMapper.InvokeAfterMapProperty(propertyPath, propertyMapper);
            }));
        }
      }
    }

    private class KeyElementRelationMapper : ModelMapper.IMapKeyRelationMapper
    {
      private readonly ICustomizersHolder customizersHolder;
      private readonly ModelMapper modelMapper;
      private readonly PropertyPath propertyPath;

      public KeyElementRelationMapper(
        PropertyPath propertyPath,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.propertyPath = propertyPath;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(IMapKeyRelation relation)
      {
        relation.Element((Action<IMapKeyMapper>) (x =>
        {
          this.modelMapper.InvokeBeforeMapMapKey(this.propertyPath, x);
          this.customizersHolder.InvokeCustomizers(this.propertyPath, x);
          this.modelMapper.InvokeAfterMapMapKey(this.propertyPath, x);
        }));
      }
    }

    private class KeyManyToManyRelationMapper : ModelMapper.IMapKeyRelationMapper
    {
      private readonly ICustomizersHolder customizersHolder;
      private readonly ModelMapper modelMapper;
      private readonly PropertyPath propertyPath;

      public KeyManyToManyRelationMapper(
        PropertyPath propertyPath,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.propertyPath = propertyPath;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(IMapKeyRelation relation)
      {
        relation.ManyToMany((Action<IMapKeyManyToManyMapper>) (x =>
        {
          this.modelMapper.InvokeBeforeMapMapKeyManyToMany(this.propertyPath, x);
          this.customizersHolder.InvokeCustomizers(this.propertyPath, x);
          this.modelMapper.InvokeAfterMapMapKeyManyToMany(this.propertyPath, x);
        }));
      }
    }

    private class ManyToManyRelationMapper : ModelMapper.ICollectionElementRelationMapper
    {
      private readonly ICustomizersHolder customizersHolder;
      private readonly ModelMapper modelMapper;
      private readonly PropertyPath propertyPath;

      public ManyToManyRelationMapper(
        PropertyPath propertyPath,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.propertyPath = propertyPath;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(ICollectionElementRelation relation)
      {
        relation.ManyToMany((Action<IManyToManyMapper>) (x =>
        {
          this.modelMapper.InvokeBeforeMapManyToMany(this.propertyPath, x);
          this.customizersHolder.InvokeCustomizers(this.propertyPath, x);
          this.modelMapper.InvokeAfterMapManyToMany(this.propertyPath, x);
        }));
      }

      public void MapCollectionProperties(ICollectionPropertiesMapper mapped)
      {
      }
    }

    private class ManyToAnyRelationMapper : ModelMapper.ICollectionElementRelationMapper
    {
      private readonly ICustomizersHolder customizersHolder;
      private readonly ModelMapper modelMapper;
      private readonly PropertyPath propertyPath;

      public ManyToAnyRelationMapper(
        PropertyPath propertyPath,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.propertyPath = propertyPath;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(ICollectionElementRelation relation)
      {
        relation.ManyToAny(typeof (int), (Action<IManyToAnyMapper>) (x => this.customizersHolder.InvokeCustomizers(this.propertyPath, x)));
      }

      public void MapCollectionProperties(ICollectionPropertiesMapper mapped)
      {
      }
    }

    private class OneToManyRelationMapper : ModelMapper.ICollectionElementRelationMapper
    {
      private const BindingFlags FlattenHierarchyBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
      private readonly Type collectionElementType;
      private readonly ICustomizersHolder customizersHolder;
      private readonly IModelInspector domainInspector;
      private readonly ModelMapper modelMapper;
      private readonly Type ownerType;
      private readonly PropertyPath propertyPath;

      public OneToManyRelationMapper(
        PropertyPath propertyPath,
        Type ownerType,
        Type collectionElementType,
        IModelInspector domainInspector,
        ICustomizersHolder customizersHolder,
        ModelMapper modelMapper)
      {
        this.propertyPath = propertyPath;
        this.ownerType = ownerType;
        this.collectionElementType = collectionElementType;
        this.domainInspector = domainInspector;
        this.customizersHolder = customizersHolder;
        this.modelMapper = modelMapper;
      }

      public void Map(ICollectionElementRelation relation)
      {
        relation.OneToMany((Action<IOneToManyMapper>) (x =>
        {
          this.modelMapper.InvokeBeforeMapOneToMany(this.propertyPath, x);
          this.customizersHolder.InvokeCustomizers(this.propertyPath, x);
          this.modelMapper.InvokeAfterMapOneToMany(this.propertyPath, x);
        }));
      }

      public void MapCollectionProperties(ICollectionPropertiesMapper mapped)
      {
        string parentColumnNameInChild = this.GetParentColumnNameInChild();
        if (parentColumnNameInChild == null)
          return;
        mapped.Key((Action<IKeyMapper>) (k => k.Column(parentColumnNameInChild)));
      }

      private string GetParentColumnNameInChild()
      {
        return ((IEnumerable<PropertyInfo>) this.collectionElementType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.PropertyType.IsAssignableFrom(this.ownerType) && this.domainInspector.IsPersistentProperty((MemberInfo) p)))?.Name;
      }
    }
  }
}
