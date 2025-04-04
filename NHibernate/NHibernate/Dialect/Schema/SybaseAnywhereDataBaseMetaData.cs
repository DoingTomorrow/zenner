// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.SybaseAnywhereDataBaseMetaData
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class SybaseAnywhereDataBaseMetaData(DbConnection pObjConnection) : AbstractDataBaseSchema(pObjConnection)
  {
    public override ITableMetadata GetTableMetadata(DataRow rs, bool extras)
    {
      return (ITableMetadata) new SybaseAnywhereTableMetaData(rs, (IDataBaseSchema) this, extras);
    }

    public override ISet<string> GetReservedWords()
    {
      HashedSet<string> reservedWords = new HashedSet<string>();
      foreach (DataRow row in (InternalDataCollectionBase) this.Connection.GetSchema(DbMetaDataCollectionNames.ReservedWords).Rows)
        reservedWords.Add(row["reserved_word"].ToString());
      return (ISet<string>) reservedWords;
    }

    public override DataTable GetTables(
      string catalog,
      string schemaPattern,
      string tableNamePattern,
      string[] types)
    {
      return this.Connection.GetSchema("Tables", new string[3]
      {
        schemaPattern,
        tableNamePattern,
        null
      });
    }

    public override DataTable GetIndexInfo(string catalog, string schemaPattern, string tableName)
    {
      return this.Connection.GetSchema("Indexes", new string[3]
      {
        schemaPattern,
        tableName,
        null
      });
    }

    public override DataTable GetIndexColumns(
      string catalog,
      string schemaPattern,
      string tableName,
      string indexName)
    {
      return this.Connection.GetSchema("IndexColumns", new string[4]
      {
        schemaPattern,
        tableName,
        indexName,
        null
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
        schemaPattern,
        tableNamePattern,
        null
      });
    }

    public override DataTable GetForeignKeys(string catalog, string schema, string table)
    {
      return this.Connection.GetSchema("ForeignKeys", new string[3]
      {
        schema,
        table,
        null
      });
    }
  }
}
