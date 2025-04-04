// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.AbstractDataBaseSchema
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public abstract class AbstractDataBaseSchema : IDataBaseSchema
  {
    private readonly DbConnection connection;

    protected AbstractDataBaseSchema(DbConnection connection) => this.connection = connection;

    protected DbConnection Connection => this.connection;

    public virtual bool StoresMixedCaseQuotedIdentifiers => true;

    public virtual bool StoresUpperCaseQuotedIdentifiers => false;

    public virtual bool StoresUpperCaseIdentifiers => false;

    public virtual bool StoresLowerCaseQuotedIdentifiers => false;

    public virtual bool StoresLowerCaseIdentifiers => false;

    public virtual DataTable GetTables(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string[] types)
    {
      return this.connection.GetSchema("Tables", new string[3]
      {
        catalog,
        schemaPattern,
        tableNamePattern
      });
    }

    public virtual string ColumnNameForTableName => "TABLE_NAME";

    public abstract ITableMetadata GetTableMetadata(DataRow rs, bool extras);

    public virtual DataTable GetColumns(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string columnNamePattern)
    {
      return this.connection.GetSchema("Columns", new string[4]
      {
        catalog,
        schemaPattern,
        tableNamePattern,
        columnNamePattern
      });
    }

    public virtual DataTable GetIndexInfo(string catalog, string schemaPattern, string tableName)
    {
      return this.connection.GetSchema("Indexes", new string[4]
      {
        catalog,
        schemaPattern,
        tableName,
        null
      });
    }

    public virtual DataTable GetIndexColumns(
      string catalog,
      string schemaPattern,
      string tableName,
      string indexName)
    {
      return this.connection.GetSchema("IndexColumns", new string[5]
      {
        catalog,
        schemaPattern,
        tableName,
        indexName,
        null
      });
    }

    public virtual DataTable GetForeignKeys(string catalog, string schema, string table)
    {
      return this.connection.GetSchema(this.ForeignKeysSchemaName, new string[4]
      {
        catalog,
        schema,
        table,
        null
      });
    }

    public virtual ISet<string> GetReservedWords()
    {
      HashedSet<string> reservedWords = new HashedSet<string>();
      foreach (DataRow row in (InternalDataCollectionBase) this.connection.GetSchema(DbMetaDataCollectionNames.ReservedWords).Rows)
        reservedWords.Add(row["ReservedWord"].ToString());
      return (ISet<string>) reservedWords;
    }

    protected virtual string ForeignKeysSchemaName => "ForeignKeys";
  }
}
