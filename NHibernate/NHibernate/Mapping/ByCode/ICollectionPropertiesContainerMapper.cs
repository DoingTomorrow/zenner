// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ICollectionPropertiesContainerMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface ICollectionPropertiesContainerMapper
  {
    void Set(
      MemberInfo property,
      Action<ISetPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping);

    void Bag(
      MemberInfo property,
      Action<IBagPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping);

    void List(
      MemberInfo property,
      Action<IListPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping);

    void Map(
      MemberInfo property,
      Action<IMapPropertiesMapper> collectionMapping,
      Action<IMapKeyRelation> keyMapping,
      Action<ICollectionElementRelation> mapping);

    void IdBag(
      MemberInfo property,
      Action<IIdBagPropertiesMapper> collectionMapping,
      Action<ICollectionElementRelation> mapping);
  }
}
