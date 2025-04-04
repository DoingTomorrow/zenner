// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.PostgreSQLDataBaseMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;
using System.Data.Common;
using System.Globalization;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class PostgreSQLDataBaseMetadata(DbConnection connection) : AbstractDataBaseSchema(connection)
  {
    public override ITableMetadata GetTableMetadata(DataRow rs, bool extras)
    {
      return (ITableMetadata) new PostgreSQLTableMetadata(rs, (IDataBaseSchema) this, extras);
    }

    public override bool StoresMixedCaseQuotedIdentifiers => false;

    public override bool StoresLowerCaseIdentifiers => true;

    public override DataTable GetColumns(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string columnNamePattern)
    {
      DataTable columns = base.GetColumns(catalog, schemaPattern, tableNamePattern, columnNamePattern);
      columns.Locale = CultureInfo.InvariantCulture;
      return columns;
    }

    public override DataTable GetTables(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string[] types)
    {
      DataTable tables = base.GetTables(catalog, schemaPattern, tableNamePattern, types);
      tables.Locale = CultureInfo.InvariantCulture;
      return tables;
    }

    public override DataTable GetIndexColumns(
      string catalog,
      string schemaPattern,
      string tableName,
      string indexName)
    {
      DataTable indexColumns = base.GetIndexColumns(catalog, schemaPattern, tableName, indexName);
      indexColumns.Locale = CultureInfo.InvariantCulture;
      return indexColumns;
    }

    public override DataTable GetIndexInfo(string catalog, string schemaPattern, string tableName)
    {
      DataTable indexInfo = base.GetIndexInfo(catalog, schemaPattern, tableName);
      indexInfo.Locale = CultureInfo.InvariantCulture;
      return indexInfo;
    }

    public override DataTable GetForeignKeys(string catalog, string schema, string table)
    {
      DataTable foreignKeys = base.GetForeignKeys(catalog, schema, table);
      foreignKeys.Locale = CultureInfo.InvariantCulture;
      return foreignKeys;
    }
  }
}
