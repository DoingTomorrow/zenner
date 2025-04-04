// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.AbstractPropertyContainerMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public abstract class AbstractPropertyContainerMapper(Type container, HbmMapping mapDoc) : 
    AbstractBasePropertyContainerMapper(container, mapDoc),
    IPropertyContainerMapper,
    ICollectionPropertiesContainerMapper,
    IPlainPropertyContainerMapper,
    IBasePlainPropertyContainerMapper,
    IMinimalPlainPropertyContainerMapper
  {
    public virtual void OneToOne(MemberInfo property, Action<IOneToOneMapper> mapping)
    {
      HbmOneToOne hbmOneToOne = new HbmOneToOne()
      {
        name = property.Name
      };
      mapping((IOneToOneMapper) new OneToOneMapper(property, hbmOneToOne));
      this.AddProperty((object) hbmOneToOne);
    }

    public virtual void Set(
      MemberInfo property,
      Action<ISetPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmSet hbm = new HbmSet() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((ISetPropertiesMapper) new SetMapper(this.container, collectionElementType, hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item = rel)));
      this.AddProperty((object) hbm);
    }

    public virtual void Bag(
      MemberInfo property,
      Action<IBagPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmBag hbm = new HbmBag() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((IBagPropertiesMapper) new BagMapper(this.container, collectionElementType, hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item = rel)));
      this.AddProperty((object) hbm);
    }

    public virtual void List(
      MemberInfo property,
      Action<IListPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmList hbm = new HbmList() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((IListPropertiesMapper) new ListMapper(this.container, collectionElementType, hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item1 = rel)));
      this.AddProperty((object) hbm);
    }

    public virtual void Map(
      MemberInfo property,
      Action<IMapPropertiesMapper> collectionMapping,
      Action<IMapKeyRelation> keyMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmMap hbm = new HbmMap() { name = property.Name };
      Type propertyOrFieldType = property.GetPropertyOrFieldType();
      Type dictionaryKeyType = propertyOrFieldType.DetermineDictionaryKeyType();
      Type dictionaryValueType = propertyOrFieldType.DetermineDictionaryValueType();
      collectionMapping((IMapPropertiesMapper) new MapMapper(this.container, dictionaryKeyType, dictionaryValueType, hbm, this.mapDoc));
      keyMapping((IMapKeyRelation) new MapKeyRelation(dictionaryKeyType, hbm, this.mapDoc));
      mapping((ICollectionElementRelation) new CollectionElementRelation(dictionaryValueType, this.MapDoc, (Action<object>) (rel => hbm.Item1 = rel)));
      this.AddProperty((object) hbm);
    }

    public virtual void IdBag(
      MemberInfo property,
      Action<IIdBagPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping)
    {
      HbmIdbag hbm = new HbmIdbag() { name = property.Name };
      Type collectionElementType = property.GetPropertyOrFieldType().DetermineCollectionElementType();
      collectionMapping((IIdBagPropertiesMapper) new IdBagMapper(this.container, collectionElementType, hbm));
      mapping((ICollectionElementRelation) new CollectionElementRelation(collectionElementType, this.MapDoc, (Action<object>) (rel => hbm.Item = rel)));
      this.AddProperty((object) hbm);
    }
  }
}
