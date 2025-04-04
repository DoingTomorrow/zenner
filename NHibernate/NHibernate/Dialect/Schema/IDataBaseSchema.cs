// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.IDataBaseSchema
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public interface IDataBaseSchema
  {
    bool StoresMixedCaseQuotedIdentifiers { get; }

    bool StoresUpperCaseQuotedIdentifiers { get; }

    bool StoresUpperCaseIdentifiers { get; }

    bool StoresLowerCaseQuotedIdentifiers { get; }

    bool StoresLowerCaseIdentifiers { get; }

    DataTable GetTables(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string[] types);

    string ColumnNameForTableName { get; }

    ITableMetadata GetTableMetadata(DataRow rs, bool extras);

    DataTable GetColumns(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string columnNamePattern);

    DataTable GetIndexInfo(string catalog, string schemaPattern, string tableName);

    DataTable GetIndexColumns(
      string catalog,
      string schemaPattern,
      string tableName,
      string indexName);

    DataTable GetForeignKeys(string catalog, string schema, string table);

    ISet<string> GetReservedWords();
  }
}
