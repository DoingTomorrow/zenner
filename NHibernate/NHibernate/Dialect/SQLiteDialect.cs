// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.SQLiteDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Dialect
{
  public class SQLiteDialect : NHibernate.Dialect.Dialect
  {
    public SQLiteDialect()
    {
      this.RegisterColumnTypes();
      this.RegisterFunctions();
      this.RegisterKeywords();
      this.RegisterDefaultProperties();
    }

    protected virtual void RegisterColumnTypes()
    {
      this.RegisterColumnType(DbType.Binary, "BLOB");
      this.RegisterColumnType(DbType.Byte, "TINYINT");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INT");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.SByte, "INTEGER");
      this.RegisterColumnType(DbType.UInt16, "INTEGER");
      this.RegisterColumnType(DbType.UInt32, "INTEGER");
      this.RegisterColumnType(DbType.UInt64, "INTEGER");
      this.RegisterColumnType(DbType.Currency, "NUMERIC");
      this.RegisterColumnType(DbType.Decimal, "NUMERIC");
      this.RegisterColumnType(DbType.Double, "DOUBLE");
      this.RegisterColumnType(DbType.Single, "DOUBLE");
      this.RegisterColumnType(DbType.VarNumeric, "NUMERIC");
      this.RegisterColumnType(DbType.AnsiString, "TEXT");
      this.RegisterColumnType(DbType.String, "TEXT");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "TEXT");
      this.RegisterColumnType(DbType.StringFixedLength, "TEXT");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "DATETIME");
      this.RegisterColumnType(DbType.Time, "TIME");
      this.RegisterColumnType(DbType.Boolean, "BOOL");
      this.RegisterColumnType(DbType.Guid, "UNIQUEIDENTIFIER");
    }

    protected virtual void RegisterFunctions()
    {
      this.RegisterFunction("second", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "cast(strftime('%S', ?1) as int)"));
      this.RegisterFunction("minute", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "cast(strftime('%M', ?1) as int)"));
      this.RegisterFunction("hour", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "cast(strftime('%H', ?1) as int)"));
      this.RegisterFunction("day", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "cast(strftime('%d', ?1) as int)"));
      this.RegisterFunction("month", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "cast(strftime('%m', ?1) as int)"));
      this.RegisterFunction("year", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "cast(strftime('%Y', ?1) as int)"));
      this.RegisterFunction("current_timestamp", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.DateTime, "datetime(current_timestamp, 'localtime')"));
      this.RegisterFunction("date", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Date, "datetime(date(?1))"));
      this.RegisterFunction("substring", (ISQLFunction) new StandardSQLFunction("substr", (IType) NHibernateUtil.String));
      this.RegisterFunction("left", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "substr(?1,1,?2)"));
      this.RegisterFunction("trim", (ISQLFunction) new AnsiTrimEmulationFunction());
      this.RegisterFunction("replace", (ISQLFunction) new StandardSafeSQLFunction("replace", (IType) NHibernateUtil.String, 3));
      this.RegisterFunction("mod", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "((?1) % (?2))"));
      this.RegisterFunction("iif", (ISQLFunction) new SQLFunctionTemplate((IType) null, "case when ?1 then ?2 else ?3 end"));
      this.RegisterFunction("cast", (ISQLFunction) new SQLiteDialect.SQLiteCastFunction());
    }

    protected virtual void RegisterKeywords() => this.RegisterKeyword("int");

    protected virtual void RegisterDefaultProperties()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.SQLite20Driver";
      this.DefaultProperties["query.substitutions"] = "true 1, false 0, yes 'Y', no 'N'";
    }

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new SQLiteDataBaseMetaData(connection);
    }

    public override string AddColumnString => "add column";

    public override string IdentitySelectString => "select last_insert_rowid()";

    public override SqlString AppendIdentitySelectToInsert(SqlString insertSql)
    {
      return insertSql.Append("; " + this.IdentitySelectString);
    }

    public override bool SupportsInsertSelectIdentity => true;

    public override bool DropConstraints => false;

    public override string ForUpdateString => string.Empty;

    public override bool SupportsSubSelects => true;

    public override bool SupportsIfExistsBeforeTableName => true;

    public override bool HasDataTypeInIdentityColumn => false;

    public override bool SupportsIdentityColumns => true;

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => true;

    public override string IdentityColumnString => "integer primary key autoincrement";

    public override bool GenerateTablePrimaryKeyConstraintForIdentityColumn => false;

    public override string Qualify(string catalog, string schema, string table)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      if (!string.IsNullOrEmpty(catalog))
      {
        if (catalog.StartsWith(this.OpenQuote.ToString()))
        {
          catalog = catalog.Substring(1, catalog.Length - 1);
          flag = true;
        }
        if (catalog.EndsWith(this.CloseQuote.ToString()))
        {
          catalog = catalog.Substring(0, catalog.Length - 1);
          flag = true;
        }
        stringBuilder.Append(catalog).Append('_');
      }
      if (!string.IsNullOrEmpty(schema))
      {
        if (schema.StartsWith(this.OpenQuote.ToString()))
        {
          schema = schema.Substring(1, schema.Length - 1);
          flag = true;
        }
        if (schema.EndsWith(this.CloseQuote.ToString()))
        {
          schema = schema.Substring(0, schema.Length - 1);
          flag = true;
        }
        stringBuilder.Append(schema).Append('_');
      }
      if (table.StartsWith(this.OpenQuote.ToString()))
      {
        table = table.Substring(1, table.Length - 1);
        flag = true;
      }
      if (table.EndsWith(this.CloseQuote.ToString()))
      {
        table = table.Substring(0, table.Length - 1);
        flag = true;
      }
      string str = stringBuilder.Append(table).ToString();
      return flag ? this.OpenQuote.ToString() + str + (object) this.CloseQuote : str;
    }

    public override string NoColumnsInsertString => "DEFAULT VALUES";

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add(queryString);
      sqlStringBuilder.Add(" limit ");
      if (limit != null)
        sqlStringBuilder.Add(limit);
      else
        sqlStringBuilder.Add(int.MaxValue.ToString());
      if (offset != null)
      {
        sqlStringBuilder.Add(" offset ");
        sqlStringBuilder.Add(offset);
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override bool SupportsTemporaryTables => true;

    public override string CreateTemporaryTableString => "create temp table";

    public override bool DropTemporaryTableAfterUse() => true;

    public override string SelectGUIDString => "select randomblob(16)";

    public override string DisableForeignKeyConstraintsString => "PRAGMA foreign_keys = OFF";

    public override string EnableForeignKeyConstraintsString => "PRAGMA foreign_keys = ON";

    public override bool SupportsForeignKeyConstraintInAlterTable => false;

    [Serializable]
    protected class SQLiteCastFunction : CastFunction
    {
      protected override bool CastingIsRequired(string sqlType)
      {
        return !sqlType.ToLowerInvariant().Contains("date") && !sqlType.ToLowerInvariant().Contains("time");
      }
    }
  }
}
