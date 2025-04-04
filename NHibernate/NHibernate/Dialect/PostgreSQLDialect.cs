// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.PostgreSQLDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect
{
  public class PostgreSQLDialect : NHibernate.Dialect.Dialect
  {
    public PostgreSQLDialect()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.NpgsqlDriver";
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "char(255)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 8000, "char($l)");
      this.RegisterColumnType(DbType.AnsiString, "varchar(255)");
      this.RegisterColumnType(DbType.AnsiString, 8000, "varchar($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "text");
      this.RegisterColumnType(DbType.Binary, "bytea");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "bytea");
      this.RegisterColumnType(DbType.Boolean, "boolean");
      this.RegisterColumnType(DbType.Byte, "int2");
      this.RegisterColumnType(DbType.Currency, "decimal(16,4)");
      this.RegisterColumnType(DbType.Date, "date");
      this.RegisterColumnType(DbType.DateTime, "timestamp");
      this.RegisterColumnType(DbType.Decimal, "decimal(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "decimal($p, $s)");
      this.RegisterColumnType(DbType.Double, "float8");
      this.RegisterColumnType(DbType.Int16, "int2");
      this.RegisterColumnType(DbType.Int32, "int4");
      this.RegisterColumnType(DbType.Int64, "int8");
      this.RegisterColumnType(DbType.Single, "float4");
      this.RegisterColumnType(DbType.StringFixedLength, "char(255)");
      this.RegisterColumnType(DbType.StringFixedLength, 4000, "char($l)");
      this.RegisterColumnType(DbType.String, "varchar(255)");
      this.RegisterColumnType(DbType.String, 4000, "varchar($l)");
      this.RegisterColumnType(DbType.String, 1073741823, "text");
      this.RegisterColumnType(DbType.Time, "time");
      this.RegisterFunction("current_timestamp", (ISQLFunction) new NoArgSQLFunction("now", (IType) NHibernateUtil.DateTime, true));
      this.RegisterFunction("str", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "cast(?1 as varchar)"));
      this.RegisterFunction("locate", (ISQLFunction) new PositionSubstringFunction());
      this.RegisterFunction("iif", (ISQLFunction) new SQLFunctionTemplate((IType) null, "case when ?1 then ?2 else ?3 end"));
      this.RegisterFunction("replace", (ISQLFunction) new StandardSQLFunction("replace", (IType) NHibernateUtil.String));
      this.RegisterFunction("left", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "substr(?1,1,?2)"));
      this.RegisterFunction("mod", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "((?1) % (?2))"));
    }

    public override string AddColumnString => "add column";

    public override bool DropConstraints => false;

    public override string CascadeConstraintsString => " cascade";

    public override string GetSequenceNextValString(string sequenceName)
    {
      return "select " + this.GetSelectSequenceNextValString(sequenceName);
    }

    public override string GetSelectSequenceNextValString(string sequenceName)
    {
      return "nextval ('" + sequenceName + "')";
    }

    public override string GetCreateSequenceString(string sequenceName)
    {
      return "create sequence " + sequenceName;
    }

    public override string GetDropSequenceString(string sequenceName)
    {
      return "drop sequence " + sequenceName;
    }

    public override SqlString AddIdentifierOutParameterToInsert(
      SqlString insertString,
      string identifierColumnName,
      string parameterName)
    {
      return insertString.Append(" returning " + identifierColumnName);
    }

    public override InsertGeneratedIdentifierRetrievalMethod InsertGeneratedIdentifierRetrievalMethod
    {
      get => InsertGeneratedIdentifierRetrievalMethod.OutputParameter;
    }

    public override bool SupportsSequences => true;

    public override bool SupportsPooledSequences => true;

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => true;

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add(queryString);
      if (limit != null)
      {
        sqlStringBuilder.Add(" limit ");
        sqlStringBuilder.Add(limit);
      }
      if (offset != null)
      {
        sqlStringBuilder.Add(" offset ");
        sqlStringBuilder.Add(offset);
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override string GetForUpdateString(string aliases)
    {
      return this.ForUpdateString + " of " + aliases;
    }

    public override bool SupportsUnionAll => true;

    public override string GetSelectClauseNullString(SqlType sqlType)
    {
      return "null::" + this.GetTypeName(sqlType);
    }

    public override bool SupportsTemporaryTables => true;

    public override string CreateTemporaryTableString => "create temporary table";

    public override string CreateTemporaryTablePostfix => "on commit drop";

    public override string ToBooleanValueString(bool value) => !value ? "FALSE" : "TRUE";

    public override string SelectGUIDString => "select uuid_generate_v4()";

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new PostgreSQLDataBaseMetadata(connection);
    }

    public override long TimestampResolutionInTicks => 10;

    public override bool SupportsCurrentTimestampSelection => true;

    public override string CurrentTimestampSelectString => "SELECT CURRENT_TIMESTAMP";
  }
}
