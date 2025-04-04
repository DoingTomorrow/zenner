// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.DynamicComponentMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class DynamicComponentMapper : 
    IDynamicComponentMapper,
    IDynamicComponentAttributesMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IPropertyContainerMapper,
    ICollectionPropertiesContainerMapper,
    IPlainPropertyContainerMapper,
    IBasePlainPropertyContainerMapper,
    IMinimalPlainPropertyContainerMapper
  {
    private readonly HbmDynamicComponent component;
    private readonly IAccessorPropertyMapper accessorPropertyMapper;

    public DynamicComponentMapper(
      HbmDynamicComponent component,
      MemberInfo declaringTypeMember,
      HbmMapping mapDoc)
      : this(component, declaringTypeMember, (IAccessorPropertyMapper) new AccessorPropertyMapper(declaringTypeMember.DeclaringType, declaringTypeMember.Name, (Action<string>) (x => component.access = x)), mapDoc)
    {
    }

    private DynamicComponentMapper(
      HbmDynamicComponent component,
      MemberInfo declaringTypeMember,
      IAccessorPropertyMapper accessorMapper,
      HbmMapping mapDoc)
    {
      if (mapDoc == null)
        throw new ArgumentNullException(nameof (mapDoc));
      this.Container = declaringTypeMember.DeclaringType;
      this.MapDoc = mapDoc;
      this.component = component;
      this.accessorPropertyMapper = accessorMapper;
    }

    private Type Container { get; set; }

    private HbmMapping MapDoc { get; set; }

    protected void AddProperty(object property)
    {
      object[] second = property != null ? new object[1]
      {
        property
      } : throw new ArgumentNullException(nameof (property));
      this.component.Items = this.component.Items == null ? second : ((IEnumerable<object>) this.component.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }

    public void Property(MemberInfo property, Action<IPropertyMapper> mapping)
    {
      HbmProperty hbmProperty = new HbmProperty()
      {
        name = property.Name,
        type1 = property.GetPropertyOrFieldType().GetNhTypeName()
      };
      mapping((IPropertyMapper) new PropertyMapper(property, hbmProperty, (IAccessorPropertyMapper) new NoMemberPropertyMapper()));
      this.AddProperty((object) hbmProperty);
    }

    public void Component(MemberInfo property, Action<IComponentMapper> mapping)
    {
      HbmComponent hbmComponent = new HbmComponent()
      {
        name = property.Name
      };
      mapping((IComponentMapper) new ComponentMapper(hbmComponent, property.GetPropertyOrFieldType(), (IAccessorPropertyMapper) new NoMemberPropertyMapper(), this.MapDoc));
      this.AddProperty((object) hbmComponent);
    }

    public void Component(MemberInfo property, Action<IDynamicComponentMapper> mapping)
    {
      HbmDynamicComponent dynamicComponent = new HbmDynamicComponent()
      {
        name = property.Name
      };
      mapping((IDynamicComponentMapper) new DynamicComponentMapper(dynamicComponent, property, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), this.MapDoc));
      this.AddProperty((object) dynamicComponent);
    }

    public void ManyToOne(MemberInfo property, Action<IManyToOneMapper> mapping)
    {
      HbmManyToOne hbmManyToOne = new HbmManyToOne()
      {
        name = property.Name
      };
      mapping((IManyToOneMapper) new ManyToOneMapper(property, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbmManyToOne, this.MapDoc));
      this.AddProperty((object) hbmManyToOne);
    }

    public void Any(MemberInfo property, Type idTypeOfMetaType, Action<IAnyMapper> mapping)
    {
      HbmAny hbmAny = new HbmAny() { name = property.Name };
      mapping((IAnyMapper) new AnyMapper(property, idTypeOfMetaType, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbmAny, this.MapDoc));
      this.AddProperty((object) hbmAny);
    }

    public void OneToOne(MemberInfo property, Action<IOneToOneMapper> mapping)
    {
      HbmOneToOne hbmOneToOne = new HbmOneToOne()
      {
        name = property.Name
      };
      mapping((IOneToOneMapper) new OneToOneMapper(property, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbmOneToOne));
      this.AddProperty((object) hbmOneToOne);
    }

    public void Bag(
      MemberInfo property,
      Action<IBagPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmBag hbm = new HbmBag() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((IBagPropertiesMapper) new BagMapper(this.Container, collectionElementType, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item = rel)));
      this.AddProperty((object) hbm);
    }

    public void Set(
      MemberInfo property,
      Action<ISetPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmSet hbm = new HbmSet() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((ISetPropertiesMapper) new SetMapper(this.Container, collectionElementType, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item = rel)));
      this.AddProperty((object) hbm);
    }

    public void List(
      MemberInfo property,
      Action<IListPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmList hbm = new HbmList() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((IListPropertiesMapper) new ListMapper(this.Container, collectionElementType, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item1 = rel)));
      this.AddProperty((object) hbm);
    }

    public void Map(
      MemberInfo property,
      Action<IMapPropertiesMapper> collectionMapping,
      Action<IMapKeyRelation> keyMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmMap hbm = new HbmMap() { name = property.Name };
      Type propertyOrFieldType = property.GetPropertyOrFieldType();
      Type dictionaryKeyType = propertyOrFieldType.DetermineDictionaryKeyType();
      Type dictionaryValueType = propertyOrFieldType.DetermineDictionaryValueType();
      collectionMapping((IMapPropertiesMapper) new MapMapper(this.Container, dictionaryKeyType, dictionaryValueType, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbm, this.MapDoc));
      keyMapping((IMapKeyRelation) new MapKeyRelation(dictionaryKeyType, hbm, this.MapDoc));
      mapping((ICollectionElementRelation) new CollectionElementRelation(dictionaryValueType, this.MapDoc, (Action<object>) (rel => hbm.Item1 = rel)));
      this.AddProperty((object) hbm);
    }

    public void IdBag(
      MemberInfo property,
      Action<IIdBagPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmIdbag hbm = new HbmIdbag() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((IIdBagPropertiesMapper) new IdBagMapper(this.Container, collectionElementType, (IAccessorPropertyMapper) new NoMemberPropertyMapper(), hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item = rel)));
      this.AddProperty((object) hbm);
    }

    public void Access(Accessor accessor) => this.accessorPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this.accessorPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this.component.optimisticlock = takeInConsiderationForOptimisticLock;
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this.component.update = consideredInUpdateQuery;
    }

    public void Insert(bool consideredInInsertQuery)
    {
      this.component.insert = consideredInInsertQuery;
    }

    public void Unique(bool unique) => this.component.unique = unique;
  }
}
