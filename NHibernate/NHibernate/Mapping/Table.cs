// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Table
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Table : IRelationalModel
  {
    private static int tableCounter;
    private readonly List<string> checkConstraints = new List<string>();
    private readonly LinkedHashMap<string, Column> columns = new LinkedHashMap<string, Column>();
    private readonly Dictionary<Table.ForeignKeyKey, ForeignKey> foreignKeys = new Dictionary<Table.ForeignKeyKey, ForeignKey>();
    private readonly Dictionary<string, Index> indexes = new Dictionary<string, Index>();
    private readonly int uniqueInteger;
    private readonly Dictionary<string, UniqueKey> uniqueKeys = new Dictionary<string, UniqueKey>();
    private string catalog;
    private string comment;
    private bool hasDenormalizedTables;
    private IKeyValue idValue;
    private bool isAbstract;
    private bool isSchemaQuoted;
    private string name;
    private bool quoted;
    private string schema;
    private SchemaAction schemaActions = SchemaAction.All;
    private string subselect;

    public Table() => this.uniqueInteger = Table.tableCounter++;

    public Table(string name)
      : this()
    {
      this.Name = name;
    }

    public string Name
    {
      get => this.name;
      set
      {
        if (value[0] == '`')
        {
          this.quoted = true;
          this.name = value.Substring(1, value.Length - 2);
        }
        else
          this.name = value;
      }
    }

    public int ColumnSpan => this.columns.Count;

    public virtual IEnumerable<Column> ColumnIterator => (IEnumerable<Column>) this.columns.Values;

    public virtual IEnumerable<Index> IndexIterator => (IEnumerable<Index>) this.indexes.Values;

    public IEnumerable<ForeignKey> ForeignKeyIterator
    {
      get => (IEnumerable<ForeignKey>) this.foreignKeys.Values;
    }

    public virtual IEnumerable<UniqueKey> UniqueKeyIterator
    {
      get => (IEnumerable<UniqueKey>) this.uniqueKeys.Values;
    }

    public virtual PrimaryKey PrimaryKey { get; set; }

    public string Schema
    {
      get => this.schema;
      set
      {
        if (value != null && value[0] == '`')
        {
          this.isSchemaQuoted = true;
          this.schema = value.Substring(1, value.Length - 2);
        }
        else
          this.schema = value;
      }
    }

    public int UniqueInteger => this.uniqueInteger;

    public bool IsQuoted
    {
      get => this.quoted;
      set => this.quoted = value;
    }

    public IEnumerable<string> CheckConstraintsIterator
    {
      get => (IEnumerable<string>) this.checkConstraints;
    }

    public bool IsAbstractUnionTable => this.HasDenormalizedTables && this.isAbstract;

    public bool HasDenormalizedTables => this.hasDenormalizedTables;

    public bool IsAbstract
    {
      get => this.isAbstract;
      set => this.isAbstract = value;
    }

    internal IDictionary<string, UniqueKey> UniqueKeys
    {
      get
      {
        if (this.uniqueKeys.Count <= 1)
          return (IDictionary<string, UniqueKey>) this.uniqueKeys;
        Dictionary<string, UniqueKey> uniqueKeys = new Dictionary<string, UniqueKey>(this.uniqueKeys.Count);
        foreach (KeyValuePair<string, UniqueKey> uniqueKey1 in this.uniqueKeys)
        {
          UniqueKey uniqueKey2 = uniqueKey1.Value;
          IList<Column> columns = uniqueKey2.Columns;
          bool flag = false;
          foreach (KeyValuePair<string, UniqueKey> keyValuePair in new Dictionary<string, UniqueKey>((IDictionary<string, UniqueKey>) uniqueKeys))
          {
            if (Table.AreSameColumns((ICollection<Column>) keyValuePair.Value.Columns, (ICollection<Column>) columns))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            uniqueKeys[uniqueKey1.Key] = uniqueKey2;
        }
        return (IDictionary<string, UniqueKey>) uniqueKeys;
      }
    }

    public bool HasPrimaryKey => this.PrimaryKey != null;

    public string Catalog
    {
      get => this.catalog;
      set => this.catalog = value;
    }

    public string Comment
    {
      get => this.comment;
      set => this.comment = value;
    }

    public string Subselect
    {
      get => this.subselect;
      set => this.subselect = value;
    }

    public IKeyValue IdentifierValue
    {
      get => this.idValue;
      set => this.idValue = value;
    }

    public bool IsSubselect => !string.IsNullOrEmpty(this.subselect);

    public bool IsPhysicalTable => !this.IsSubselect && !this.IsAbstractUnionTable;

    public SchemaAction SchemaActions
    {
      get => this.schemaActions;
      set => this.schemaActions = value;
    }

    public string RowId { get; set; }

    public bool IsSchemaQuoted => this.isSchemaQuoted;

    public string SqlCreateString(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      string defaultCatalog,
      string defaultSchema)
    {
      StringBuilder stringBuilder = new StringBuilder(this.HasPrimaryKey ? dialect.CreateTableString : dialect.CreateMultisetTableString).Append(' ').Append(this.GetQualifiedName(dialect, defaultCatalog, defaultSchema)).Append(" (");
      bool flag1 = this.idValue != null && this.idValue.IsIdentityColumn(dialect);
      string str = (string) null;
      if (this.HasPrimaryKey && flag1)
      {
        foreach (Column column in this.PrimaryKey.ColumnIterator)
          str = column.GetQuotedName(dialect);
      }
      bool flag2 = false;
      foreach (Column column in this.ColumnIterator)
      {
        if (flag2)
          stringBuilder.Append(", ");
        flag2 = true;
        stringBuilder.Append(column.GetQuotedName(dialect)).Append(' ');
        if (flag1 && column.GetQuotedName(dialect).Equals(str))
        {
          if (dialect.HasDataTypeInIdentityColumn)
            stringBuilder.Append(column.GetSqlType(dialect, p));
          stringBuilder.Append(' ').Append(dialect.GetIdentityColumnString(column.GetSqlTypeCode(p).DbType));
        }
        else
        {
          stringBuilder.Append(column.GetSqlType(dialect, p));
          if (!string.IsNullOrEmpty(column.DefaultValue))
            stringBuilder.Append(" default ").Append(column.DefaultValue).Append(" ");
          if (column.IsNullable)
            stringBuilder.Append(dialect.NullColumnString);
          else
            stringBuilder.Append(" not null");
        }
        if (column.IsUnique)
        {
          if (dialect.SupportsUnique)
            stringBuilder.Append(" unique");
          else
            this.GetUniqueKey(column.GetQuotedName(dialect) + "_").AddColumn(column);
        }
        if (column.HasCheckConstraint && dialect.SupportsColumnCheck)
          stringBuilder.Append(" check( ").Append(column.CheckConstraint).Append(") ");
        if (!string.IsNullOrEmpty(column.Comment))
          stringBuilder.Append(dialect.GetColumnComment(column.Comment));
      }
      if (this.HasPrimaryKey && (dialect.GenerateTablePrimaryKeyConstraintForIdentityColumn || !flag1))
        stringBuilder.Append(", ").Append(this.PrimaryKey.SqlConstraintString(dialect, defaultSchema));
      foreach (UniqueKey uniqueKey in this.UniqueKeyIterator)
        stringBuilder.Append(',').Append(uniqueKey.SqlConstraintString(dialect));
      if (dialect.SupportsTableCheck)
      {
        foreach (string checkConstraint in this.checkConstraints)
          stringBuilder.Append(", check (").Append(checkConstraint).Append(") ");
      }
      if (!dialect.SupportsForeignKeyConstraintInAlterTable)
      {
        foreach (ForeignKey foreignKey in this.ForeignKeyIterator)
        {
          if (foreignKey.HasPhysicalConstraint)
            stringBuilder.Append(",").Append(foreignKey.SqlConstraintString(dialect, foreignKey.Name, defaultCatalog, defaultSchema));
        }
      }
      stringBuilder.Append(")");
      if (!string.IsNullOrEmpty(this.comment))
        stringBuilder.Append(dialect.GetTableComment(this.comment));
      stringBuilder.Append(dialect.TableTypeString);
      return stringBuilder.ToString();
    }

    public string SqlDropString(NHibernate.Dialect.Dialect dialect, string defaultCatalog, string defaultSchema)
    {
      return dialect.GetDropTableString(this.GetQualifiedName(dialect, defaultCatalog, defaultSchema));
    }

    public string GetQualifiedName(NHibernate.Dialect.Dialect dialect)
    {
      return this.GetQualifiedName(dialect, (string) null, (string) null);
    }

    public virtual string GetQualifiedName(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema)
    {
      if (!string.IsNullOrEmpty(this.subselect))
        return "( " + this.subselect + " )";
      string quotedName = this.GetQuotedName(dialect);
      string schema = this.schema == null ? defaultSchema : this.GetQuotedSchema(dialect);
      string catalog = this.catalog ?? defaultCatalog;
      return dialect.Qualify(catalog, schema, quotedName);
    }

    public string GetQuotedName() => !this.quoted ? this.name : "`" + this.name + "`";

    public string GetQuotedName(NHibernate.Dialect.Dialect dialect)
    {
      return !this.IsQuoted ? this.name : dialect.QuoteForTableName(this.name);
    }

    public string GetQuotedSchema() => !this.IsSchemaQuoted ? this.schema : "`" + this.schema + "`";

    public string GetQuotedSchema(NHibernate.Dialect.Dialect dialect)
    {
      return !this.IsSchemaQuoted ? this.schema : dialect.OpenQuote.ToString() + this.schema + (object) dialect.CloseQuote;
    }

    public string GetQuotedSchemaName(NHibernate.Dialect.Dialect dialect)
    {
      if (this.schema == null)
        return (string) null;
      return this.schema.StartsWith("`") ? dialect.QuoteForSchemaName(this.schema.Substring(1, this.schema.Length - 2)) : this.schema;
    }

    public Column GetColumn(int n)
    {
      IEnumerator<Column> enumerator = this.columns.Values.GetEnumerator();
      for (int index = 0; index <= n; ++index)
        enumerator.MoveNext();
      return enumerator.Current;
    }

    public void AddColumn(Column column)
    {
      Column column1 = this.GetColumn(column);
      if (column1 == null)
      {
        this.columns[column.CanonicalName] = column;
        column.uniqueInteger = this.columns.Count;
      }
      else
        column.uniqueInteger = column1.uniqueInteger;
    }

    public string[] SqlAlterStrings(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      ITableMetadata tableInfo,
      string defaultCatalog,
      string defaultSchema)
    {
      StringBuilder stringBuilder1 = new StringBuilder("alter table ").Append(this.GetQualifiedName(dialect, defaultCatalog, defaultSchema)).Append(' ').Append(dialect.AddColumnString);
      List<string> stringList = new List<string>(this.ColumnSpan);
      foreach (Column column in this.ColumnIterator)
      {
        if (tableInfo.GetColumnMetadata(column.Name) == null)
        {
          StringBuilder stringBuilder2 = new StringBuilder(stringBuilder1.ToString()).Append(' ').Append(column.GetQuotedName(dialect)).Append(' ').Append(column.GetSqlType(dialect, p));
          string defaultValue = column.DefaultValue;
          if (!string.IsNullOrEmpty(defaultValue))
          {
            stringBuilder2.Append(" default ").Append(defaultValue);
            if (column.IsNullable)
              stringBuilder2.Append(dialect.NullColumnString);
            else
              stringBuilder2.Append(" not null");
          }
          if (column.Unique && dialect.SupportsUnique && (!column.IsNullable || dialect.SupportsNotNullUnique))
            stringBuilder2.Append(" unique");
          if (column.HasCheckConstraint && dialect.SupportsColumnCheck)
            stringBuilder2.Append(" check(").Append(column.CheckConstraint).Append(") ");
          string comment = column.Comment;
          if (comment != null)
            stringBuilder2.Append(dialect.GetColumnComment(comment));
          stringList.Add(stringBuilder2.ToString());
        }
      }
      return stringList.ToArray();
    }

    public Index GetIndex(string indexName)
    {
      Index index;
      this.indexes.TryGetValue(indexName, out index);
      return index;
    }

    public Index AddIndex(Index index)
    {
      this.indexes[index.Name] = this.GetIndex(index.Name) == null ? index : throw new MappingException("Index " + index.Name + " already exists!");
      return index;
    }

    public Index GetOrCreateIndex(string indexName)
    {
      Index index = this.GetIndex(indexName);
      if (index == null)
      {
        index = new Index();
        index.Name = indexName;
        index.Table = this;
        this.indexes[indexName] = index;
      }
      return index;
    }

    public UniqueKey GetUniqueKey(string keyName)
    {
      UniqueKey uniqueKey;
      this.uniqueKeys.TryGetValue(keyName, out uniqueKey);
      return uniqueKey;
    }

    public UniqueKey AddUniqueKey(UniqueKey uniqueKey)
    {
      this.uniqueKeys[uniqueKey.Name] = this.GetUniqueKey(uniqueKey.Name) == null ? uniqueKey : throw new MappingException("UniqueKey " + uniqueKey.Name + " already exists!");
      return uniqueKey;
    }

    public UniqueKey GetOrCreateUniqueKey(string keyName)
    {
      UniqueKey uniqueKey = this.GetUniqueKey(keyName);
      if (uniqueKey == null)
      {
        uniqueKey = new UniqueKey();
        uniqueKey.Name = keyName;
        uniqueKey.Table = this;
        this.uniqueKeys[keyName] = uniqueKey;
      }
      return uniqueKey;
    }

    public virtual void CreateForeignKeys()
    {
    }

    public virtual ForeignKey CreateForeignKey(
      string keyName,
      IEnumerable<Column> keyColumns,
      string referencedEntityName)
    {
      return this.CreateForeignKey(keyName, keyColumns, referencedEntityName, (IEnumerable<Column>) null);
    }

    public virtual ForeignKey CreateForeignKey(
      string keyName,
      IEnumerable<Column> keyColumns,
      string referencedEntityName,
      IEnumerable<Column> referencedColumns)
    {
      IEnumerable<Column> columns1 = keyColumns;
      IEnumerable<Column> columns2 = referencedColumns;
      Table.ForeignKeyKey key = new Table.ForeignKeyKey(columns1, referencedEntityName, columns2);
      ForeignKey foreignKey;
      this.foreignKeys.TryGetValue(key, out foreignKey);
      if (foreignKey == null)
      {
        foreignKey = new ForeignKey();
        if (!string.IsNullOrEmpty(keyName))
          foreignKey.Name = keyName;
        else
          foreignKey.Name = "FK" + this.UniqueColumnString((IEnumerable) columns1, referencedEntityName);
        foreignKey.Table = this;
        this.foreignKeys.Add(key, foreignKey);
        foreignKey.ReferencedEntityName = referencedEntityName;
        foreignKey.AddColumns(columns1);
        if (referencedColumns != null)
          foreignKey.AddReferencedColumns(columns2);
      }
      if (!string.IsNullOrEmpty(keyName))
        foreignKey.Name = keyName;
      return foreignKey;
    }

    public virtual UniqueKey CreateUniqueKey(IList<Column> keyColumns)
    {
      UniqueKey uniqueKey = this.GetOrCreateUniqueKey("UK" + this.UniqueColumnString((IEnumerable) keyColumns));
      uniqueKey.AddColumns((IEnumerable<Column>) keyColumns);
      return uniqueKey;
    }

    public string UniqueColumnString(IEnumerable uniqueColumns)
    {
      return this.UniqueColumnString(uniqueColumns, (string) null);
    }

    public string UniqueColumnString(IEnumerable iterator, string referencedEntityName)
    {
      int num = 37;
      if (referencedEntityName != null)
        num ^= referencedEntityName.GetHashCode();
      foreach (object obj in iterator)
        num ^= obj.GetHashCode();
      return this.name.GetHashCode().ToString("X") + num.GetHashCode().ToString("X");
    }

    public void SetIdentifierValue(SimpleValue identifierValue)
    {
      this.idValue = (IKeyValue) identifierValue;
    }

    public void AddCheckConstraint(string constraint)
    {
      if (string.IsNullOrEmpty(constraint))
        return;
      this.checkConstraints.Add(constraint);
    }

    internal void SetHasDenormalizedTables() => this.hasDenormalizedTables = true;

    public virtual bool ContainsColumn(Column column) => this.columns.ContainsValue(column);

    public virtual Column GetColumn(Column column)
    {
      if (column == null)
        return (Column) null;
      Column column1;
      this.columns.TryGetValue(column.CanonicalName, out column1);
      return !column.Equals(column1) ? (Column) null : column1;
    }

    private static bool AreSameColumns(ICollection<Column> col1, ICollection<Column> col2)
    {
      if (col1.Count != col2.Count)
        return false;
      foreach (Column column in (IEnumerable<Column>) col1)
      {
        if (!col2.Contains(column))
          return false;
      }
      foreach (Column column in (IEnumerable<Column>) col2)
      {
        if (!col1.Contains(column))
          return false;
      }
      return true;
    }

    public virtual string[] SqlCommentStrings(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema)
    {
      List<string> stringList = new List<string>();
      if (dialect.SupportsCommentOn)
      {
        string qualifiedName = this.GetQualifiedName(dialect, defaultCatalog, defaultSchema);
        if (!string.IsNullOrEmpty(this.comment))
        {
          StringBuilder stringBuilder = new StringBuilder().Append("comment on table ").Append(qualifiedName).Append(" is '").Append(this.comment).Append("'");
          stringList.Add(stringBuilder.ToString());
        }
        foreach (Column column in this.ColumnIterator)
        {
          string comment = column.Comment;
          if (comment != null)
          {
            StringBuilder stringBuilder = new StringBuilder().Append("comment on column ").Append(qualifiedName).Append('.').Append(column.GetQuotedName(dialect)).Append(" is '").Append(comment).Append("'");
            stringList.Add(stringBuilder.ToString());
          }
        }
      }
      return stringList.ToArray();
    }

    public virtual string SqlTemporaryTableCreateString(NHibernate.Dialect.Dialect dialect, IMapping mapping)
    {
      StringBuilder stringBuilder = new StringBuilder(dialect.CreateTemporaryTableString).Append(' ').Append(this.name).Append(" (");
      bool flag = false;
      foreach (Column column in this.ColumnIterator)
      {
        stringBuilder.Append(column.GetQuotedName(dialect)).Append(' ');
        stringBuilder.Append(column.GetSqlType(dialect, mapping));
        if (flag)
          stringBuilder.Append(", ");
        flag = true;
        if (column.IsNullable)
          stringBuilder.Append(dialect.NullColumnString);
        else
          stringBuilder.Append(" not null");
      }
      stringBuilder.Append(") ");
      stringBuilder.Append(dialect.CreateTemporaryTablePostfix);
      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder().Append(this.GetType().FullName).Append('(');
      if (this.Catalog != null)
        stringBuilder.Append(this.Catalog + ".");
      if (this.Schema != null)
        stringBuilder.Append(this.Schema + ".");
      stringBuilder.Append(this.Name).Append(')');
      return stringBuilder.ToString();
    }

    public void ValidateColumns(NHibernate.Dialect.Dialect dialect, IMapping mapping, ITableMetadata tableInfo)
    {
      foreach (Column column in this.ColumnIterator)
      {
        IColumnMetadata columnMetadata = tableInfo.GetColumnMetadata(column.Name);
        if (columnMetadata == null)
          throw new HibernateException(string.Format("Missing column: {0} in {1}", (object) column.Name, (object) dialect.Qualify(tableInfo.Catalog, tableInfo.Schema, tableInfo.Name)));
        if (!column.GetSqlType(dialect, mapping).StartsWith(columnMetadata.TypeName, StringComparison.OrdinalIgnoreCase))
          throw new HibernateException(string.Format("Wrong column type in {0} for column {1}. Found: {2}, Expected {3}", (object) dialect.Qualify(tableInfo.Catalog, tableInfo.Schema, tableInfo.Name), (object) column.Name, (object) columnMetadata.TypeName.ToLowerInvariant(), (object) column.GetSqlType(dialect, mapping)));
      }
    }

    [Serializable]
    internal class ForeignKeyKey : IEqualityComparer<Table.ForeignKeyKey>
    {
      internal List<Column> columns;
      internal string referencedClassName;
      internal List<Column> referencedColumns;

      internal ForeignKeyKey(
        IEnumerable<Column> columns,
        string referencedClassName,
        IEnumerable<Column> referencedColumns)
      {
        this.referencedClassName = referencedClassName;
        this.columns = new List<Column>(columns);
        if (referencedColumns != null)
          this.referencedColumns = new List<Column>(referencedColumns);
        else
          this.referencedColumns = new List<Column>();
      }

      public bool Equals(Table.ForeignKeyKey x, Table.ForeignKeyKey y)
      {
        return CollectionHelper.CollectionEquals<Column>((ICollection<Column>) y.columns, (ICollection<Column>) x.columns) && CollectionHelper.CollectionEquals<Column>((ICollection<Column>) y.referencedColumns, (ICollection<Column>) x.referencedColumns);
      }

      public int GetHashCode(Table.ForeignKeyKey obj)
      {
        return CollectionHelper.GetHashCode<Column>((IEnumerable<Column>) obj.columns) ^ CollectionHelper.GetHashCode<Column>((IEnumerable<Column>) obj.referencedColumns);
      }

      public override int GetHashCode() => this.GetHashCode(this);

      public override bool Equals(object other)
      {
        return other is Table.ForeignKeyKey y && this.Equals(this, y);
      }
    }
  }
}
