// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.JoinedSubclassMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class JoinedSubclassMapper : 
    AbstractPropertyContainerMapper,
    IJoinedSubclassMapper,
    IJoinedSubclassAttributesMapper,
    IEntityAttributesMapper,
    IEntitySqlsMapper,
    IPropertyContainerMapper,
    ICollectionPropertiesContainerMapper,
    IPlainPropertyContainerMapper,
    IBasePlainPropertyContainerMapper,
    IMinimalPlainPropertyContainerMapper
  {
    private readonly HbmJoinedSubclass classMapping = new HbmJoinedSubclass();
    private readonly KeyMapper keyMapper;

    public JoinedSubclassMapper(Type subClass, HbmMapping mapDoc)
      : base(subClass, mapDoc)
    {
      HbmJoinedSubclass[] second = new HbmJoinedSubclass[1]
      {
        this.classMapping
      };
      this.classMapping.name = subClass.GetShortClassName(mapDoc);
      this.classMapping.extends = subClass.BaseType.GetShortClassName(mapDoc);
      if (this.classMapping.key == null)
        this.classMapping.key = new HbmKey()
        {
          column1 = subClass.BaseType.Name.ToLowerInvariant() + "_key"
        };
      this.keyMapper = new KeyMapper(subClass, this.classMapping.key);
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
      this.classMapping.batchsize = value > 0 ? value.ToString() : (string) null;
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

    public void Table(string tableName) => this.classMapping.table = tableName;

    public void Catalog(string catalogName) => this.classMapping.catalog = catalogName;

    public void Schema(string schemaName) => this.classMapping.schema = schemaName;

    public void Key(Action<IKeyMapper> keyMapping) => keyMapping((IKeyMapper) this.keyMapper);

    public void Extends(Type baseType)
    {
      if (baseType == null)
        throw new ArgumentNullException(nameof (baseType));
      this.classMapping.extends = this.Container.GetBaseTypes().Contains<Type>(baseType) ? baseType.GetShortClassName(this.MapDoc) : throw new ArgumentOutOfRangeException(nameof (baseType), string.Format("{0} is a valid super-class of {1}", (object) baseType, (object) this.Container));
    }

    public void SchemaAction(NHibernate.Mapping.ByCode.SchemaAction action)
    {
      this.classMapping.schemaaction = action.ToSchemaActionString();
    }
  }
}
