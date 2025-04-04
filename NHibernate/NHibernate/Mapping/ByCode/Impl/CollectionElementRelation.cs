// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CollectionElementRelation
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class CollectionElementRelation : ICollectionElementRelation
  {
    private readonly Type collectionElementType;
    private readonly Action<object> elementRelationshipAssing;
    private readonly HbmMapping mapDoc;

    public CollectionElementRelation(
      Type collectionElementType,
      HbmMapping mapDoc,
      Action<object> elementRelationshipAssing)
    {
      this.collectionElementType = collectionElementType;
      this.mapDoc = mapDoc;
      this.elementRelationshipAssing = elementRelationshipAssing;
    }

    public void Element(Action<IElementMapper> mapping)
    {
      HbmElement elementMapping = new HbmElement();
      mapping((IElementMapper) new ElementMapper(this.collectionElementType, elementMapping));
      this.elementRelationshipAssing((object) elementMapping);
    }

    public void OneToMany(Action<IOneToManyMapper> mapping)
    {
      HbmOneToMany oneToManyMapping = new HbmOneToMany()
      {
        @class = this.collectionElementType.GetShortClassName(this.mapDoc)
      };
      mapping((IOneToManyMapper) new OneToManyMapper(this.collectionElementType, oneToManyMapping, this.mapDoc));
      this.elementRelationshipAssing((object) oneToManyMapping);
    }

    public void ManyToMany(Action<IManyToManyMapper> mapping)
    {
      HbmManyToMany manyToMany = new HbmManyToMany()
      {
        @class = this.collectionElementType.GetShortClassName(this.mapDoc)
      };
      mapping((IManyToManyMapper) new ManyToManyMapper(this.collectionElementType, manyToMany, this.mapDoc));
      this.elementRelationshipAssing((object) manyToMany);
    }

    public void Component(Action<IComponentElementMapper> mapping)
    {
      HbmCompositeElement component = new HbmCompositeElement()
      {
        @class = this.collectionElementType.GetShortClassName(this.mapDoc)
      };
      mapping((IComponentElementMapper) new ComponentElementMapper(this.collectionElementType, this.mapDoc, component));
      this.elementRelationshipAssing((object) component);
    }

    public void ManyToAny(Type idTypeOfMetaType, Action<IManyToAnyMapper> mapping)
    {
      HbmManyToAny manyToAny = new HbmManyToAny();
      mapping((IManyToAnyMapper) new ManyToAnyMapper(this.collectionElementType, idTypeOfMetaType, manyToAny, this.mapDoc));
      this.elementRelationshipAssing((object) manyToAny);
    }
  }
}
