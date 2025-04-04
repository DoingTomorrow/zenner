// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.AbstractTableMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public abstract class AbstractTableMetadata : ITableMetadata
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ITableMetadata));
    private string catalog;
    private string schema;
    private string name;
    private readonly Dictionary<string, IColumnMetadata> columns = new Dictionary<string, IColumnMetadata>();
    private readonly Dictionary<string, IForeignKeyMetadata> foreignKeys = new Dictionary<string, IForeignKeyMetadata>();
    private readonly Dictionary<string, IIndexMetadata> indexes = new Dictionary<string, IIndexMetadata>();

    public AbstractTableMetadata(DataRow rs, IDataBaseSchema meta, bool extras)
    {
      this.ParseTableInfo(rs);
      this.InitColumns(meta);
      if (extras)
      {
        this.InitForeignKeys(meta);
        this.InitIndexes(meta);
      }
      string str1 = this.catalog == null ? "" : this.catalog + (object) '.';
      string str2 = this.schema == null ? "" : this.schema + (object) '.';
      AbstractTableMetadata.log.Info((object) ("table found: " + str1 + str2 + this.name));
      AbstractTableMetadata.log.Info((object) ("columns: " + StringHelper.CollectionToString((ICollection) this.columns.Keys)));
      if (!extras)
        return;
      AbstractTableMetadata.log.Info((object) ("foreign keys: " + StringHelper.CollectionToString((ICollection) this.foreignKeys.Keys)));
      AbstractTableMetadata.log.Info((object) ("indexes: " + StringHelper.CollectionToString((ICollection) this.indexes.Keys)));
    }

    protected abstract void ParseTableInfo(DataRow rs);

    protected abstract string GetConstraintName(DataRow rs);

    protected abstract string GetColumnName(DataRow rs);

    protected abstract string GetIndexName(DataRow rs);

    protected abstract IColumnMetadata GetColumnMetadata(DataRow rs);

    protected abstract IForeignKeyMetadata GetForeignKeyMetadata(DataRow rs);

    protected abstract IIndexMetadata GetIndexMetadata(DataRow rs);

    public string Name
    {
      get => this.name;
      protected set => this.name = value;
    }

    public string Catalog
    {
      get => this.catalog;
      protected set => this.catalog = value;
    }

    public string Schema
    {
      get => this.schema;
      protected set => this.schema = value;
    }

    public override string ToString() => "TableMetadata(" + this.name + (object) ')';

    public IColumnMetadata GetColumnMetadata(string columnName)
    {
      IColumnMetadata columnMetadata;
      this.columns.TryGetValue(columnName.ToLowerInvariant(), out columnMetadata);
      return columnMetadata;
    }

    public IForeignKeyMetadata GetForeignKeyMetadata(string keyName)
    {
      IForeignKeyMetadata foreignKeyMetadata;
      this.foreignKeys.TryGetValue(keyName.ToLowerInvariant(), out foreignKeyMetadata);
      return foreignKeyMetadata;
    }

    public IIndexMetadata GetIndexMetadata(string indexName)
    {
      IIndexMetadata indexMetadata;
      this.indexes.TryGetValue(indexName.ToLowerInvariant(), out indexMetadata);
      return indexMetadata;
    }

    public virtual bool NeedPhysicalConstraintCreation(string fkName)
    {
      return this.GetIndexMetadata(fkName) == null;
    }

    private void AddForeignKey(DataRow rs, IDataBaseSchema meta)
    {
      string constraintName = this.GetConstraintName(rs);
      if (string.IsNullOrEmpty(constraintName))
        return;
      IForeignKeyMetadata foreignKeyMetadata = this.GetForeignKeyMetadata(constraintName);
      if (foreignKeyMetadata == null)
      {
        foreignKeyMetadata = this.GetForeignKeyMetadata(rs);
        this.foreignKeys[foreignKeyMetadata.Name.ToLowerInvariant()] = foreignKeyMetadata;
      }
      foreach (DataRow row in (InternalDataCollectionBase) meta.GetIndexColumns(this.catalog, this.schema, this.name, constraintName).Rows)
        foreignKeyMetadata.AddColumn(this.GetColumnMetadata(this.GetColumnName(row)));
    }

    private void AddIndex(DataRow rs, IDataBaseSchema meta)
    {
      string indexName = this.GetIndexName(rs);
      if (string.IsNullOrEmpty(indexName))
        return;
      IIndexMetadata indexMetadata = this.GetIndexMetadata(indexName);
      if (indexMetadata == null)
      {
        indexMetadata = this.GetIndexMetadata(rs);
        this.indexes[indexMetadata.Name.ToLowerInvariant()] = indexMetadata;
      }
      foreach (DataRow row in (InternalDataCollectionBase) meta.GetIndexColumns(this.catalog, this.schema, this.name, indexName).Rows)
        indexMetadata.AddColumn(this.GetColumnMetadata(this.GetColumnName(row)));
    }

    private void AddColumn(DataRow rs)
    {
      string columnName = this.GetColumnName(rs);
      if (string.IsNullOrEmpty(columnName) || this.GetColumnMetadata(columnName) != null)
        return;
      IColumnMetadata columnMetadata = this.GetColumnMetadata(rs);
      this.columns[columnMetadata.Name.ToLowerInvariant()] = columnMetadata;
    }

    private void InitForeignKeys(IDataBaseSchema meta)
    {
      foreach (DataRow row in (InternalDataCollectionBase) meta.GetForeignKeys(this.catalog, this.schema, this.name).Rows)
        this.AddForeignKey(row, meta);
    }

    private void InitIndexes(IDataBaseSchema meta)
    {
      foreach (DataRow row in (InternalDataCollectionBase) meta.GetIndexInfo(this.catalog, this.schema, this.name).Rows)
        this.AddIndex(row, meta);
    }

    private void InitColumns(IDataBaseSchema meta)
    {
      foreach (DataRow row in (InternalDataCollectionBase) meta.GetColumns(this.catalog, this.schema, this.name, (string) null).Rows)
        this.AddColumn(row);
    }
  }
}
