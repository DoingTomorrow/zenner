// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ICollectionPropertiesMapper`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Collection;
using NHibernate.UserTypes;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface ICollectionPropertiesMapper<TEntity, TElement> : 
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    ICollectionSqlsMapper
    where TEntity : class
  {
    void Inverse(bool value);

    void Mutable(bool value);

    void Where(string sqlWhereClause);

    void BatchSize(int value);

    void Lazy(CollectionLazy collectionLazy);

    void Key(Action<IKeyMapper<TEntity>> keyMapping);

    void OrderBy<TProperty>(Expression<Func<TElement, TProperty>> property);

    void OrderBy(string sqlOrderByClause);

    void Sort();

    void Sort<TComparer>();

    void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle);

    void Type<TCollection>() where TCollection : IUserCollectionType;

    void Type(System.Type collectionType);

    void Table(string tableName);

    void Catalog(string catalogName);

    void Schema(string schemaName);

    void Cache(Action<ICacheMapper> cacheMapping);

    void Filter(string filterName, Action<IFilterMapper> filterMapping);

    void Fetch(CollectionFetchMode fetchMode);

    void Persister<TPersister>() where TPersister : ICollectionPersister;
  }
}
