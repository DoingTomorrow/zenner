// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.IdBagMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Persister.Collection;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class IdBagMapper : 
    IIdBagPropertiesMapper,
    ICollectionPropertiesMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    ICollectionSqlsMapper
  {
    private readonly IAccessorPropertyMapper entityPropertyMapper;
    private readonly KeyMapper keyMapper;
    private readonly HbmIdbag mapping;
    private ICacheMapper cacheMapper;
    private readonly CollectionIdMapper idMapper;

    public IdBagMapper(System.Type ownerType, System.Type elementType, HbmIdbag mapping)
      : this(ownerType, elementType, (IAccessorPropertyMapper) new AccessorPropertyMapper(ownerType, mapping.Name, (Action<string>) (x => mapping.access = x)), mapping)
    {
    }

    public IdBagMapper(
      System.Type ownerType,
      System.Type elementType,
      IAccessorPropertyMapper accessorMapper,
      HbmIdbag mapping)
    {
      if (ownerType == null)
        throw new ArgumentNullException(nameof (ownerType));
      if (elementType == null)
        throw new ArgumentNullException(nameof (elementType));
      if (mapping == null)
        throw new ArgumentNullException(nameof (mapping));
      this.OwnerType = ownerType;
      this.ElementType = elementType;
      this.mapping = mapping;
      if (mapping.Key == null)
        mapping.key = new HbmKey();
      this.keyMapper = new KeyMapper(ownerType, mapping.Key);
      if (mapping.collectionid == null)
        mapping.collectionid = new HbmCollectionId();
      this.idMapper = new CollectionIdMapper(mapping.collectionid);
      this.entityPropertyMapper = accessorMapper;
    }

    public System.Type OwnerType { get; private set; }

    public System.Type ElementType { get; private set; }

    public void Inverse(bool value) => this.mapping.inverse = value;

    public void Mutable(bool value) => this.mapping.mutable = value;

    public void Where(string sqlWhereClause) => this.mapping.where = sqlWhereClause;

    public void BatchSize(int value)
    {
      if (value > 0)
      {
        this.mapping.batchsize = value;
        this.mapping.batchsizeSpecified = true;
      }
      else
      {
        this.mapping.batchsize = 0;
        this.mapping.batchsizeSpecified = false;
      }
    }

    public void Lazy(CollectionLazy collectionLazy)
    {
      this.mapping.lazySpecified = true;
      switch (collectionLazy)
      {
        case CollectionLazy.Lazy:
          this.mapping.lazy = HbmCollectionLazy.True;
          break;
        case CollectionLazy.NoLazy:
          this.mapping.lazy = HbmCollectionLazy.False;
          break;
        case CollectionLazy.Extra:
          this.mapping.lazy = HbmCollectionLazy.Extra;
          break;
      }
    }

    public void Key(Action<IKeyMapper> keyMapping) => keyMapping((IKeyMapper) this.keyMapper);

    public void OrderBy(MemberInfo property) => this.mapping.orderby = property.Name;

    public void OrderBy(string sqlOrderByClause) => this.mapping.orderby = sqlOrderByClause;

    public void Sort()
    {
    }

    public void Sort<TComparer>()
    {
    }

    public void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle)
    {
      this.mapping.cascade = cascadeStyle.ToCascadeString();
    }

    public void Type<TCollection>() where TCollection : IUserCollectionType
    {
      this.mapping.collectiontype = typeof (TCollection).AssemblyQualifiedName;
    }

    public void Type(System.Type collectionType)
    {
      if (collectionType == null)
        throw new ArgumentNullException(nameof (collectionType));
      this.mapping.collectiontype = typeof (IUserCollectionType).IsAssignableFrom(collectionType) ? collectionType.AssemblyQualifiedName : throw new ArgumentOutOfRangeException(nameof (collectionType), string.Format("The collection type should be an implementation of IUserCollectionType.({0})", (object) collectionType));
    }

    public void Table(string tableName) => this.mapping.table = tableName;

    public void Catalog(string catalogName) => this.mapping.catalog = catalogName;

    public void Schema(string schemaName) => this.mapping.schema = schemaName;

    public void Cache(Action<ICacheMapper> cacheMapping)
    {
      if (this.cacheMapper == null)
      {
        HbmCache cacheMapping1 = new HbmCache();
        this.mapping.cache = cacheMapping1;
        this.cacheMapper = (ICacheMapper) new CacheMapper(cacheMapping1);
      }
      cacheMapping(this.cacheMapper);
    }

    public void Filter(string filterName, Action<IFilterMapper> filterMapping)
    {
      if (filterMapping == null)
        filterMapping = (Action<IFilterMapper>) (x => { });
      HbmFilter filter = new HbmFilter();
      FilterMapper filterMapper = new FilterMapper(filterName, filter);
      filterMapping((IFilterMapper) filterMapper);
      Dictionary<string, HbmFilter> dictionary = this.mapping.filter != null ? ((IEnumerable<HbmFilter>) this.mapping.filter).ToDictionary<HbmFilter, string, HbmFilter>((Func<HbmFilter, string>) (f => f.name), (Func<HbmFilter, HbmFilter>) (f => f)) : new Dictionary<string, HbmFilter>(1);
      dictionary[filterName] = filter;
      this.mapping.filter = dictionary.Values.ToArray<HbmFilter>();
    }

    public void Fetch(CollectionFetchMode fetchMode)
    {
      if (fetchMode == null)
        return;
      this.mapping.fetch = fetchMode.ToHbm();
      this.mapping.fetchSpecified = this.mapping.fetch != HbmCollectionFetchMode.Select;
    }

    public void Persister(System.Type persister)
    {
      if (persister == null)
        throw new ArgumentNullException(nameof (persister));
      this.mapping.persister = typeof (ICollectionPersister).IsAssignableFrom(persister) ? persister.AssemblyQualifiedName : throw new ArgumentOutOfRangeException(nameof (persister), "Expected type implementing ICollectionPersister.");
    }

    public void Id(Action<ICollectionIdMapper> idMapping)
    {
      idMapping((ICollectionIdMapper) this.idMapper);
    }

    public void Access(Accessor accessor) => this.entityPropertyMapper.Access(accessor);

    public void Access(System.Type accessorType) => this.entityPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this.mapping.optimisticlock = takeInConsiderationForOptimisticLock;
    }

    public void Loader(string namedQueryReference)
    {
      if (this.mapping.SqlLoader == null)
        this.mapping.loader = new HbmLoader();
      this.mapping.loader.queryref = namedQueryReference;
    }

    public void SqlInsert(string sql)
    {
      if (this.mapping.SqlInsert == null)
        this.mapping.sqlinsert = new HbmCustomSQL();
      this.mapping.sqlinsert.Text = new string[1]{ sql };
    }

    public void SqlUpdate(string sql)
    {
      if (this.mapping.SqlUpdate == null)
        this.mapping.sqlupdate = new HbmCustomSQL();
      this.mapping.sqlupdate.Text = new string[1]{ sql };
    }

    public void SqlDelete(string sql)
    {
      if (this.mapping.SqlDelete == null)
        this.mapping.sqldelete = new HbmCustomSQL();
      this.mapping.sqldelete.Text = new string[1]{ sql };
    }

    public void SqlDeleteAll(string sql)
    {
      if (this.mapping.SqlDeleteAll == null)
        this.mapping.sqldeleteall = new HbmCustomSQL();
      this.mapping.sqldeleteall.Text = new string[1]{ sql };
    }

    public void Subselect(string sql)
    {
      if (this.mapping.Subselect == null)
        this.mapping.subselect = new HbmSubselect();
      this.mapping.subselect.Text = new string[1]{ sql };
    }
  }
}
