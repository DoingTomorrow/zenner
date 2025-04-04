// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.JoinMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class JoinMapper : 
    AbstractPropertyContainerMapper,
    IJoinMapper,
    IJoinAttributesMapper,
    IEntitySqlsMapper,
    ICollectionPropertiesContainerMapper,
    IBasePlainPropertyContainerMapper,
    IMinimalPlainPropertyContainerMapper
  {
    private readonly HbmJoin hbmJoin;
    private readonly KeyMapper keyMapper;

    public JoinMapper(Type container, string splitGroupId, HbmJoin hbmJoin, HbmMapping mapDoc)
      : base(container, mapDoc)
    {
      if (splitGroupId == null)
        throw new ArgumentNullException(nameof (splitGroupId));
      this.hbmJoin = hbmJoin != null ? hbmJoin : throw new ArgumentNullException(nameof (hbmJoin));
      this.hbmJoin.table = splitGroupId.Trim();
      if (string.IsNullOrEmpty(this.hbmJoin.table))
        throw new ArgumentOutOfRangeException(nameof (splitGroupId), "The table-name cant be empty.");
      if (hbmJoin.key == null)
        hbmJoin.key = new HbmKey()
        {
          column1 = container.Name.ToLowerInvariant() + "_key"
        };
      this.keyMapper = new KeyMapper(container, hbmJoin.key);
    }

    public event TableNameChangedHandler TableNameChanged;

    private void InvokeTableNameChanged(TableNameChangedEventArgs e)
    {
      TableNameChangedHandler tableNameChanged = this.TableNameChanged;
      if (tableNameChanged == null)
        return;
      tableNameChanged((IJoinMapper) this, e);
    }

    protected override void AddProperty(object property)
    {
      object[] second = property != null ? new object[1]
      {
        property
      } : throw new ArgumentNullException(nameof (property));
      this.hbmJoin.Items = this.hbmJoin.Items == null ? second : ((IEnumerable<object>) this.hbmJoin.Items).Concat<object>((IEnumerable<object>) second).ToArray<object>();
    }

    public void Loader(string namedQueryReference)
    {
    }

    public void SqlInsert(string sql)
    {
      if (this.hbmJoin.SqlInsert == null)
        this.hbmJoin.sqlinsert = new HbmCustomSQL();
      this.hbmJoin.sqlinsert.Text = new string[1]{ sql };
    }

    public void SqlUpdate(string sql)
    {
      if (this.hbmJoin.SqlUpdate == null)
        this.hbmJoin.sqlupdate = new HbmCustomSQL();
      this.hbmJoin.sqlupdate.Text = new string[1]{ sql };
    }

    public void SqlDelete(string sql)
    {
      if (this.hbmJoin.SqlDelete == null)
        this.hbmJoin.sqldelete = new HbmCustomSQL();
      this.hbmJoin.sqldelete.Text = new string[1]{ sql };
    }

    public void Subselect(string sql)
    {
      if (this.hbmJoin.Subselect == null)
        this.hbmJoin.subselect = new HbmSubselect();
      this.hbmJoin.subselect.Text = new string[1]{ sql };
    }

    public void Table(string tableName)
    {
      string newName = tableName != null ? tableName.Trim() : throw new ArgumentNullException(nameof (tableName));
      if (string.IsNullOrEmpty(newName))
        throw new ArgumentOutOfRangeException(nameof (tableName), "The table-name cant be empty.");
      string table = this.hbmJoin.table;
      this.hbmJoin.table = newName;
      if (newName.Equals(table))
        return;
      this.InvokeTableNameChanged(new TableNameChangedEventArgs(table, newName));
    }

    public void Catalog(string catalogName) => this.hbmJoin.catalog = catalogName;

    public void Schema(string schemaName) => this.hbmJoin.schema = schemaName;

    public void Key(Action<IKeyMapper> keyMapping) => keyMapping((IKeyMapper) this.keyMapper);

    public void Inverse(bool value) => this.hbmJoin.inverse = value;

    public void Optional(bool isOptional) => this.hbmJoin.optional = isOptional;

    public void Fetch(FetchKind fetchMode) => this.hbmJoin.fetch = fetchMode.ToHbmJoinFetch();
  }
}
