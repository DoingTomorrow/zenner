// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ICollectionPropertiesContainerMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface ICollectionPropertiesContainerMapper<TEntity> where TEntity : class
  {
    void Set<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Set<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping);

    void Set<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Set<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<ISetPropertiesMapper<TEntity, TElement>> collectionMapping);

    void Bag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Bag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping);

    void Bag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Bag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping);

    void List<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void List<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping);

    void List<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void List<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IListPropertiesMapper<TEntity, TElement>> collectionMapping);

    void Map<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<IMapKeyRelation<TKey>> keyMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Map<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Map<TKey, TElement>(
      Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping);

    void Map<TKey, TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<IMapKeyRelation<TKey>> keyMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Map<TKey, TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void Map<TKey, TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IMapPropertiesMapper<TEntity, TKey, TElement>> collectionMapping);

    void IdBag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void IdBag<TElement>(
      Expression<Func<TEntity, IEnumerable<TElement>>> property,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping);

    void IdBag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping,
      Action<ICollectionElementRelation<TElement>> mapping);

    void IdBag<TElement>(
      string notVisiblePropertyOrFieldName,
      Action<IIdBagPropertiesMapper<TEntity, TElement>> collectionMapping);
  }
}
