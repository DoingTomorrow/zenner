// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ClassMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ClassMapper : 
    AbstractPropertyContainerMapper,
    IClassMapper,
    IClassAttributesMapper,
    IEntityAttributesMapper,
    IEntitySqlsMapper,
    IPropertyContainerMapper,
    ICollectionPropertiesContainerMapper,
    IPlainPropertyContainerMapper,
    IBasePlainPropertyContainerMapper,
    IMinimalPlainPropertyContainerMapper
  {
    private readonly HbmClass classMapping;
    private readonly IIdMapper idMapper;
    private bool simpleIdPropertyWasUsed;
    private bool composedIdWasUsed;
    private bool componentAsIdWasUsed;
    private Dictionary<string, IJoinMapper> joinMappers;
    private ICacheMapper cacheMapper;
    private IDiscriminatorMapper discriminatorMapper;
    private INaturalIdMapper naturalIdMapper;
    private IVersionMapper versionMapper;

    public ClassMapper(Type rootClass, HbmMapping mapDoc, MemberInfo idProperty)
      : base(rootClass, mapDoc)
    {
      this.classMapping = new HbmClass();
      HbmClass[] second = new HbmClass[1]
      {
        this.classMapping
      };
      this.classMapping.name = rootClass.GetShortClassName(mapDoc);
      if (rootClass.IsAbstract)
      {
        this.classMapping.@abstract = true;
        this.classMapping.abstractSpecified = true;
      }
      HbmId hbmId = new HbmId();
      this.classMapping.Item = (object) hbmId;
      this.idMapper = (IIdMapper) new IdMapper(idProperty, hbmId);
      mapDoc.Items = mapDoc.Items == null ? (object[]) second : ((IEnumerable<object>) mapDoc.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }

    protected override void AddProperty(object property)
    {
      object[] second = property != null ? new object[1]
      {
        property
      } : throw new ArgumentNullException(nameof (property));
      this.classMapping.Items = this.classMapping.Items == null ? second : ((IEnumerable<object>) this.classMapping.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }

    public Dictionary<string, IJoinMapper> JoinMappers
    {
      get => this.joinMappers ?? (this.joinMappers = new Dictionary<string, IJoinMapper>());
    }

    public void Id(Action<IIdMapper> mapper) => mapper(this.idMapper);

    public void Id(MemberInfo idProperty, Action<IIdMapper> mapper)
    {
      if (!(this.classMapping.Item is HbmId hbmId))
        throw new MappingException(string.Format("Ambiguous mapping of {0} id. A ComponentAsId or a ComposedId was used and you are trying to map the property{1} as id.", (object) this.Container.FullName, idProperty != null ? (object) (" '" + idProperty.Name + "'") : (object) ", with generator, "));
      mapper((IIdMapper) new IdMapper(idProperty, hbmId));
      if (idProperty == null)
        return;
      this.simpleIdPropertyWasUsed = true;
    }

    public void ComponentAsId(MemberInfo idProperty, Action<IComponentAsIdMapper> mapper)
    {
      if (idProperty == null)
        return;
      if (!this.IsMemberSupportedByMappedContainer(idProperty))
        throw new ArgumentOutOfRangeException(nameof (idProperty), "Can't use, as component id property, a property of another graph");
      if (this.composedIdWasUsed)
        throw new MappingException(string.Format("Ambiguous mapping of {0} id. A composed id was defined and you are trying to map the component {1}, of property '{2}', as id for {0}.", (object) this.Container.FullName, (object) idProperty.GetPropertyOrFieldType().FullName, (object) idProperty.Name));
      if (this.simpleIdPropertyWasUsed)
        throw new MappingException(string.Format("Ambiguous mapping of {0} id. An id property, with generator, was defined and you are trying to map the component {1}, of property '{2}', as id for {0}.", (object) this.Container.FullName, (object) idProperty.GetPropertyOrFieldType().FullName, (object) idProperty.Name));
      if (!(this.classMapping.Item is HbmCompositeId id))
      {
        id = new HbmCompositeId();
        this.classMapping.Item = (object) id;
      }
      mapper((IComponentAsIdMapper) new ComponentAsIdMapper(idProperty.GetPropertyOrFieldType(), idProperty, id, this.mapDoc));
      this.componentAsIdWasUsed = true;
    }

    public void ComposedId(Action<IComposedIdMapper> idPropertiesMapping)
    {
      if (this.componentAsIdWasUsed)
        throw new MappingException(string.Format("Ambiguous mapping of {0} id. A Component as id was used and you are trying to map an id composed by various properties of {0}.", (object) this.Container.FullName));
      if (this.simpleIdPropertyWasUsed)
        throw new MappingException(string.Format("Ambiguous mapping of {0} id. An id property, with generator, was defined and you are trying to map an id composed by various properties of {0}.", (object) this.Container.FullName));
      if (!(this.classMapping.Item is HbmCompositeId id))
      {
        id = new HbmCompositeId();
        this.classMapping.Item = (object) id;
      }
      idPropertiesMapping((IComposedIdMapper) new ComposedIdMapper(this.Container, id, this.mapDoc));
      this.composedIdWasUsed = true;
    }

    public void Discriminator(Action<IDiscriminatorMapper> discriminatorMapping)
    {
      if (this.discriminatorMapper == null)
      {
        HbmDiscriminator discriminatorMapping1 = new HbmDiscriminator();
        this.classMapping.discriminator = discriminatorMapping1;
        this.discriminatorMapper = (IDiscriminatorMapper) new DiscriminatorMapper(discriminatorMapping1);
      }
      discriminatorMapping(this.discriminatorMapper);
    }

    public void DiscriminatorValue(object value)
    {
      if (value != null)
      {
        this.classMapping.discriminatorvalue = value.ToString();
        this.Discriminator((Action<IDiscriminatorMapper>) (x => { }));
        Type type = value.GetType();
        if (type == typeof (string))
          return;
        this.classMapping.discriminator.type = type.GetNhTypeName();
      }
      else
        this.classMapping.discriminatorvalue = "null";
    }

    public void Table(string tableName) => this.classMapping.table = tableName;

    public void Catalog(string catalogName) => this.classMapping.catalog = catalogName;

    public void Schema(string schemaName) => this.classMapping.schema = schemaName;

    public void Mutable(bool isMutable) => this.classMapping.mutable = isMutable;

    public void Version(MemberInfo versionProperty, Action<IVersionMapper> versionMapping)
    {
      if (this.versionMapper == null)
      {
        HbmVersion hbmVersion = new HbmVersion();
        this.classMapping.Item1 = (object) hbmVersion;
        this.versionMapper = (IVersionMapper) new VersionMapper(versionProperty, hbmVersion);
      }
      versionMapping(this.versionMapper);
    }

    public void NaturalId(Action<INaturalIdMapper> naturalIdMapping)
    {
      if (this.naturalIdMapper == null)
        this.naturalIdMapper = (INaturalIdMapper) new NaturalIdMapper(this.Container, this.classMapping, this.MapDoc);
      naturalIdMapping(this.naturalIdMapper);
    }

    public void Cache(Action<ICacheMapper> cacheMapping)
    {
      if (this.cacheMapper == null)
      {
        HbmCache cacheMapping1 = new HbmCache();
        this.classMapping.cache = cacheMapping1;
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
      Dictionary<string, HbmFilter> dictionary = this.classMapping.filter != null ? ((IEnumerable<HbmFilter>) this.classMapping.filter).ToDictionary<HbmFilter, string, HbmFilter>((Func<HbmFilter, string>) (f => f.name), (Func<HbmFilter, HbmFilter>) (f => f)) : new Dictionary<string, HbmFilter>(1);
      dictionary[filterName] = filter;
      this.classMapping.filter = dictionary.Values.ToArray<HbmFilter>();
    }

    public void Where(string whereClause) => this.classMapping.where = whereClause;

    public void SchemaAction(NHibernate.Mapping.ByCode.SchemaAction action)
    {
      this.classMapping.schemaaction = action.ToSchemaActionString();
    }

    public void Join(string splitGroupId, Action<IJoinMapper> splitMapping)
    {
      IJoinMapper joinMapper;
      if (!this.JoinMappers.TryGetValue(splitGroupId, out joinMapper))
      {
        HbmJoin hbmJoin = new HbmJoin();
        joinMapper = (IJoinMapper) new JoinMapper(this.Container, splitGroupId, hbmJoin, this.MapDoc);
        HbmJoin[] second = new HbmJoin[1]{ hbmJoin };
        this.JoinMappers.Add(splitGroupId, joinMapper);
        this.classMapping.Items1 = this.classMapping.Items1 == null ? (object[]) second : ((IEnumerable<object>) this.classMapping.Items1).Concat<object>((IEnumerable<object>) second).ToArray<object>();
      }
      splitMapping(joinMapper);
    }

    public void EntityName(string value) => this.classMapping.entityname = value;

    public void Proxy(Type proxy)
    {
      this.classMapping.proxy = this.Container.IsAssignableFrom(proxy) || proxy.IsAssignableFrom(this.Container) ? proxy.GetShortClassName(this.MapDoc) : throw new MappingException("Not compatible proxy for " + (object) this.Container);
    }

    public void Lazy(bool value)
    {
      this.classMapping.lazy = value;
      this.classMapping.lazySpecified = !value;
    }

    public void DynamicUpdate(bool value) => this.classMapping.dynamicupdate = value;

    public void DynamicInsert(bool value) => this.classMapping.dynamicinsert = value;

    public void BatchSize(int value)
    {
      if (value > 0)
      {
        this.classMapping.batchsize = value;
        this.classMapping.batchsizeSpecified = true;
      }
      else
      {
        this.classMapping.batchsize = 0;
        this.classMapping.batchsizeSpecified = false;
      }
    }

    public void SelectBeforeUpdate(bool value) => this.classMapping.selectbeforeupdate = value;

    public void Persister<T>() where T : IEntityPersister
    {
      this.classMapping.persister = typeof (T).GetShortClassName(this.MapDoc);
    }

    public void Synchronize(params string[] table)
    {
      if (table == null)
        return;
      HashSet<string> existingSyncs = new HashSet<string>(this.classMapping.synchronize != null ? ((IEnumerable<HbmSynchronize>) this.classMapping.synchronize).Select<HbmSynchronize, string>((Func<HbmSynchronize, string>) (x => x.table)) : Enumerable.Empty<string>());
      System.Array.ForEach<string>(((IEnumerable<string>) table).Where<string>((Func<string, bool>) (x => x != null)).Select<string, string>((Func<string, string>) (tableName => tableName.Trim())).Where<string>((Func<string, bool>) (cleanedName => !"".Equals(cleanedName))).ToArray<string>(), (Action<string>) (x => existingSyncs.Add(x.Trim())));
      this.classMapping.synchronize = existingSyncs.Select<string, HbmSynchronize>((Func<string, HbmSynchronize>) (x => new HbmSynchronize()
      {
        table = x
      })).ToArray<HbmSynchronize>();
    }

    public void Loader(string namedQueryReference)
    {
      if (this.classMapping.SqlLoader == null)
        this.classMapping.loader = new HbmLoader();
      this.classMapping.loader.queryref = namedQueryReference;
    }

    public void SqlInsert(string sql)
    {
      if (this.classMapping.SqlInsert == null)
        this.classMapping.sqlinsert = new HbmCustomSQL();
      this.classMapping.sqlinsert.Text = new string[1]
      {
        sql
      };
    }

    public void SqlUpdate(string sql)
    {
      if (this.classMapping.SqlUpdate == null)
        this.classMapping.sqlupdate = new HbmCustomSQL();
      this.classMapping.sqlupdate.Text = new string[1]
      {
        sql
      };
    }

    public void SqlDelete(string sql)
    {
      if (this.classMapping.SqlDelete == null)
        this.classMapping.sqldelete = new HbmCustomSQL();
      this.classMapping.sqldelete.Text = new string[1]
      {
        sql
      };
    }

    public void Subselect(string sql)
    {
      if (this.classMapping.Subselect == null)
        this.classMapping.subselect = new HbmSubselect();
      this.classMapping.subselect.Text = new string[1]
      {
        sql
      };
    }
  }
}
