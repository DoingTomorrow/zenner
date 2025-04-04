// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.OracleDataBaseSchema
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class OracleDataBaseSchema(DbConnection connection) : AbstractDataBaseSchema(connection)
  {
    public override ITableMetadata GetTableMetadata(DataRow rs, bool extras)
    {
      return (ITableMetadata) new OracleTableMetadata(rs, (IDataBaseSchema) this, extras);
    }

    public override bool StoresUpperCaseIdentifiers => true;

    public override DataTable GetTables(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string[] types)
    {
      return this.Connection.GetSchema("Tables", new string[2]
      {
        string.IsNullOrEmpty(schemaPattern) ? (string) null : schemaPattern,
        tableNamePattern
      });
    }

    public override DataTable GetColumns(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string columnNamePattern)
    {
      return this.Connection.GetSchema("Columns", new string[3]
      {
        string.IsNullOrEmpty(schemaPattern) ? (string) null : schemaPattern,
        tableNamePattern,
        columnNamePattern
      });
    }

    public override DataTable GetIndexColumns(
      string catalog,
      string schemaPattern,
      string tableName,
      string indexName)
    {
      return this.Connection.GetSchema("IndexColumns", new string[5]
      {
        string.IsNullOrEmpty(schemaPattern) ? (string) null : schemaPattern,
        indexName,
        null,
        tableName,
        null
      });
    }

    public override DataTable GetIndexInfo(string catalog, string schemaPattern, string tableName)
    {
      return this.Connection.GetSchema("Indexes", new string[4]
      {
        string.IsNullOrEmpty(schemaPattern) ? (string) null : schemaPattern,
        null,
        null,
        tableName
      });
    }

    public override DataTable GetForeignKeys(string catalog, string schema, string table)
    {
      return this.Connection.GetSchema("ForeignKeys", new string[3]
      {
        string.IsNullOrEmpty(schema) ? (string) null : schema,
        table,
        null
      });
    }
  }
}
