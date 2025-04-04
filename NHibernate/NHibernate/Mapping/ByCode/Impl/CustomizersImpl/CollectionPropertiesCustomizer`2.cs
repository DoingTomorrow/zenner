// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.CollectionPropertiesCustomizer`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Collection;
using NHibernate.UserTypes;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class CollectionPropertiesCustomizer<TEntity, TElement> : 
    ICollectionPropertiesMapper<TEntity, TElement>,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    ICollectionSqlsMapper
    where TEntity : class
  {
    private readonly IKeyMapper<TEntity> keyMapper;

    public CollectionPropertiesCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      this.PropertyPath = propertyPath;
      this.CustomizersHolder = customizersHolder;
      this.keyMapper = (IKeyMapper<TEntity>) new CollectionKeyCustomizer<TEntity>(explicitDeclarationsHolder, propertyPath, customizersHolder);
    }

    public ICustomizersHolder CustomizersHolder { get; private set; }

    public PropertyPath PropertyPath { get; private set; }

    public void Inverse(bool value)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Inverse(value)));
    }

    public void Mutable(bool value)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Mutable(value)));
    }

    public void Where(string sqlWhereClause)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Where(sqlWhereClause)));
    }

    public void BatchSize(int value)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.BatchSize(value)));
    }

    public void Lazy(CollectionLazy collectionLazy)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Lazy(collectionLazy)));
    }

    public void Key(Action<IKeyMapper<TEntity>> keyMapping) => keyMapping(this.keyMapper);

    public void OrderBy<TProperty>(Expression<Func<TElement, TProperty>> property)
    {
      MemberInfo member = TypeExtensions.DecodeMemberAccessExpression<TElement, TProperty>(property);
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.OrderBy(member)));
    }

    public void OrderBy(string sqlOrderByClause)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.OrderBy(sqlOrderByClause)));
    }

    public void Sort()
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Sort()));
    }

    public void Sort<TComparer>()
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Sort<TComparer>()));
    }

    public void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Cascade(cascadeStyle)));
    }

    public void Type<TCollection>() where TCollection : IUserCollectionType
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Type<TCollection>()));
    }

    public void Type(System.Type collectionType)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Type(collectionType)));
    }

    public void Table(string tableName)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Table(tableName)));
    }

    public void Catalog(string catalogName)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Catalog(catalogName)));
    }

    public void Schema(string schemaName)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Schema(schemaName)));
    }

    public void Cache(Action<ICacheMapper> cacheMapping)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Cache(cacheMapping)));
    }

    public void Filter(string filterName, Action<IFilterMapper> filterMapping)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Filter(filterName, filterMapping)));
    }

    public void Fetch(CollectionFetchMode fetchMode)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Fetch(fetchMode)));
    }

    public void Persister<TPersister>() where TPersister : ICollectionPersister
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Persister(typeof (TPersister))));
    }

    public void Access(Accessor accessor)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Access(accessor)));
    }

    public void Access(System.Type accessorType)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Access(accessorType)));
    }

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.OptimisticLock(takeInConsiderationForOptimisticLock)));
    }

    public void Loader(string namedQueryReference)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Loader(namedQueryReference)));
    }

    public void SqlInsert(string sql)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.SqlInsert(sql)));
    }

    public void SqlUpdate(string sql)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.SqlUpdate(sql)));
    }

    public void SqlDelete(string sql)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.SqlDelete(sql)));
    }

    public void SqlDeleteAll(string sql)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.SqlDeleteAll(sql)));
    }

    public void Subselect(string sql)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<ICollectionPropertiesMapper>) (x => x.Subselect(sql)));
    }
  }
}
