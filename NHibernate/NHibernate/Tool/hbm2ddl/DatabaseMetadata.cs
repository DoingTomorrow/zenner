// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.DatabaseMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Dialect.Schema;
using NHibernate.Exceptions;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class DatabaseMetadata
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DatabaseMetadata));
    private readonly IDictionary<string, ITableMetadata> tables = (IDictionary<string, ITableMetadata>) new Dictionary<string, ITableMetadata>();
    private readonly ISet<string> sequences = (ISet<string>) new HashedSet<string>();
    private readonly bool extras;
    private readonly NHibernate.Dialect.Dialect dialect;
    private readonly IDataBaseSchema meta;
    private readonly ISQLExceptionConverter sqlExceptionConverter;
    private static readonly string[] Types = new string[2]
    {
      "TABLE",
      "VIEW"
    };

    public DatabaseMetadata(DbConnection connection, NHibernate.Dialect.Dialect dialect)
      : this(connection, dialect, true)
    {
    }

    public DatabaseMetadata(DbConnection connection, NHibernate.Dialect.Dialect dialect, bool extras)
    {
      this.meta = dialect.GetDataBaseSchema(connection);
      this.dialect = dialect;
      this.extras = extras;
      this.InitSequences(connection, dialect);
      this.sqlExceptionConverter = dialect.BuildSQLExceptionConverter();
    }

    public ITableMetadata GetTableMetadata(
      string name,
      string schema,
      string catalog,
      bool isQuoted)
    {
      string key = this.Identifier(catalog, schema, name);
      ITableMetadata tableMetadata1;
      this.tables.TryGetValue(key, out tableMetadata1);
      if (tableMetadata1 != null)
        return tableMetadata1;
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) (!isQuoted || !this.meta.StoresMixedCaseQuotedIdentifiers ? (isQuoted && this.meta.StoresUpperCaseQuotedIdentifiers || !isQuoted && this.meta.StoresUpperCaseIdentifiers ? this.meta.GetTables(StringHelper.ToUpperCase(catalog), StringHelper.ToUpperCase(schema), StringHelper.ToUpperCase(name), DatabaseMetadata.Types) : (isQuoted && this.meta.StoresLowerCaseQuotedIdentifiers || !isQuoted && this.meta.StoresLowerCaseIdentifiers ? this.meta.GetTables(StringHelper.ToLowerCase(catalog), StringHelper.ToLowerCase(schema), StringHelper.ToLowerCase(name), DatabaseMetadata.Types) : this.meta.GetTables(catalog, schema, name, DatabaseMetadata.Types))) : this.meta.GetTables(catalog, schema, name, DatabaseMetadata.Types)).Rows)
        {
          string str = Convert.ToString(row[this.meta.ColumnNameForTableName]);
          if (name.Equals(str, StringComparison.InvariantCultureIgnoreCase))
          {
            ITableMetadata tableMetadata2 = this.meta.GetTableMetadata(row, this.extras);
            this.tables[key] = tableMetadata2;
            return tableMetadata2;
          }
        }
        DatabaseMetadata.log.Info((object) ("table not found: " + name));
        return (ITableMetadata) null;
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.sqlExceptionConverter, (Exception) ex, "could not get table metadata: " + name);
      }
    }

    private string Identifier(string catalog, string schema, string name)
    {
      return this.dialect.Qualify(catalog, schema, name);
    }

    private void InitSequences(DbConnection connection, NHibernate.Dialect.Dialect dialect)
    {
      if (!dialect.SupportsSequences)
        return;
      string querySequencesString = dialect.QuerySequencesString;
      if (querySequencesString == null)
        return;
      using (IDbCommand command = (IDbCommand) connection.CreateCommand())
      {
        command.CommandText = querySequencesString;
        using (IDataReader dataReader = command.ExecuteReader())
        {
          while (dataReader.Read())
            this.sequences.Add(((string) dataReader[0]).ToLower().Trim());
        }
      }
    }

    public bool IsSequence(object key)
    {
      if (!(key is string str))
        return false;
      char[] chArray = new char[1]{ '.' };
      string[] strArray = str.Split(chArray);
      return this.sequences.Contains(strArray[strArray.Length - 1].ToLower());
    }

    public bool IsTable(object key)
    {
      if (key is string name)
      {
        Table table1 = new Table(name);
        if (this.GetTableMetadata(table1.Name, table1.Schema, table1.Catalog, table1.IsQuoted) != null)
          return true;
        string[] strArray = name.Split('.');
        if (strArray.Length == 3)
        {
          Table table2 = new Table(strArray[2]);
          table2.Catalog = strArray[0];
          table2.Schema = strArray[1];
          return this.GetTableMetadata(table2.Name, table2.Schema, table2.Catalog, table2.IsQuoted) != null;
        }
        if (strArray.Length == 2)
        {
          Table table3 = new Table(strArray[1]);
          table3.Schema = strArray[0];
          return this.GetTableMetadata(table3.Name, table3.Schema, table3.Catalog, table3.IsQuoted) != null;
        }
      }
      return false;
    }

    public override string ToString()
    {
      return nameof (DatabaseMetadata) + StringHelper.CollectionToString((ICollection) this.tables.Keys) + " " + StringHelper.CollectionToString((ICollection) this.sequences);
    }
  }
}
