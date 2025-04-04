// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.InformixDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Dialect
{
  public class InformixDialect : NHibernate.Dialect.Dialect
  {
    public InformixDialect()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, (int) byte.MaxValue, "VARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, 32739, "LVARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "TEXT");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR(255)");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "BYTE");
      this.RegisterColumnType(DbType.Binary, "BYTE");
      this.RegisterColumnType(DbType.Boolean, "BOOLEAN");
      this.RegisterColumnType(DbType.Currency, "DECIMAL(16,4)");
      this.RegisterColumnType(DbType.Byte, "SMALLINT");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "datetime year to fraction(5)");
      this.RegisterColumnType(DbType.Decimal, "DECIMAL(19, 5)");
      this.RegisterColumnType(DbType.Decimal, 19, "DECIMAL($p, $s)");
      this.RegisterColumnType(DbType.Double, "DOUBLE");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INTEGER");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "SmallFloat");
      this.RegisterColumnType(DbType.Time, "datetime hour to second");
      this.RegisterColumnType(DbType.StringFixedLength, "CHAR($l)");
      this.RegisterColumnType(DbType.String, (int) byte.MaxValue, "VARCHAR($l)");
      this.RegisterColumnType(DbType.String, 32739, "LVARCHAR($l)");
      this.RegisterColumnType(DbType.String, int.MaxValue, "TEXT");
      this.RegisterColumnType(DbType.String, "VARCHAR(255)");
      this.RegisterFunction("substr", (ISQLFunction) new StandardSQLFunction("substr"));
      this.RegisterFunction("coalesce", (ISQLFunction) new NvlFunction());
      this.RegisterFunction("current_timestamp", (ISQLFunction) new NoArgSQLFunction("current", (IType) NHibernateUtil.DateTime, false));
      this.RegisterFunction("sysdate", (ISQLFunction) new NoArgSQLFunction("today", (IType) NHibernateUtil.DateTime, false));
      this.RegisterFunction("current", (ISQLFunction) new NoArgSQLFunction("current", (IType) NHibernateUtil.DateTime, false));
      this.RegisterFunction("today", (ISQLFunction) new NoArgSQLFunction("today", (IType) NHibernateUtil.DateTime, false));
      this.RegisterFunction("day", (ISQLFunction) new StandardSQLFunction("day", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("month", (ISQLFunction) new StandardSQLFunction("month", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("year", (ISQLFunction) new StandardSQLFunction("year", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("date", (ISQLFunction) new StandardSQLFunction("date", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("mdy", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.DateTime, "mdy(?1, ?2, ?3)"));
      this.RegisterFunction("to_char", (ISQLFunction) new StandardSQLFunction("to_char", (IType) NHibernateUtil.String));
      this.RegisterFunction("to_date", (ISQLFunction) new StandardSQLFunction("to_date", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("instr", (ISQLFunction) new StandardSQLFunction("instr", (IType) NHibernateUtil.String));
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.OdbcDriver";
      this.DefaultProperties["prepare_sql"] = "true";
    }

    public override string IdentityInsertString => "0";

    public override string CreateTemporaryTableString => "create temp table";

    public override string CreateTemporaryTablePostfix => "with no log";

    public override bool IsCurrentTimestampSelectStringCallable => true;

    public override string CurrentTimestampSelectString
    {
      get => "select current from systables where tabid=1";
    }

    public override string CurrentTimestampSQLFunctionName => "current";

    public override IViolatedConstraintNameExtracter ViolatedConstraintNameExtracter
    {
      get => (IViolatedConstraintNameExtracter) new IfxViolatedConstraintExtracter();
    }

    public override string AddColumnString => "add";

    public override bool ForUpdateOfColumns => true;

    public override bool SupportsOuterJoinForUpdate => false;

    public override string GetForUpdateString(string aliases)
    {
      return this.ForUpdateString + " of " + aliases;
    }

    public override bool SupportsTemporaryTables => true;

    public override bool? PerformTemporaryTableDDLInIsolation() => new bool?(false);

    public override int RegisterResultSetOutParameter(DbCommand statement, int position)
    {
      return position;
    }

    public override DbDataReader GetResultSet(DbCommand statement)
    {
      return statement.ExecuteReader(CommandBehavior.SingleResult);
    }

    public override bool SupportsCurrentTimestampSelection => true;

    public override long TimestampResolutionInTicks => 100;

    public override bool SupportsIdentityColumns => true;

    public override bool HasDataTypeInIdentityColumn => false;

    public override string GetIdentitySelectString(
      string identityColumn,
      string tableName,
      DbType type)
    {
      return type != DbType.Int64 ? "select dbinfo('sqlca.sqlerrd1') from systables where tabid=1" : "select dbinfo('serial8') from systables where tabid=1";
    }

    public override string IdentitySelectString
    {
      get => "select dbinfo('sqlca.sqlerrd1') from systables where tabid=1";
    }

    public override string GetIdentityColumnString(DbType type)
    {
      return type != DbType.Int64 ? "serial not null" : "serial8 not null";
    }

    public override string IdentityColumnString => "serial not null";

    public override bool SupportsSequences => false;

    public override JoinFragment CreateOuterJoinFragment()
    {
      return (JoinFragment) new InformixJoinFragment();
    }

    public override string ToBooleanValueString(bool value) => !value ? "f" : "t";

    public override bool SupportsLimit => false;

    public override bool SupportsLimitOffset => false;

    public override bool SupportsVariableLimit => false;

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      int selectInsertPoint = InformixDialect.GetAfterSelectInsertPoint(queryString);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      if (offset != null)
      {
        sqlStringBuilder.Add(" skip ");
        sqlStringBuilder.Add(offset);
      }
      if (limit != null)
      {
        sqlStringBuilder.Add(" first ");
        sqlStringBuilder.Add(limit);
      }
      return queryString.Insert(selectInsertPoint, sqlStringBuilder.ToSqlString());
    }

    public override bool SupportsUnionAll => true;

    public override bool SupportsEmptyInList => false;

    public override bool SupportsResultSetPositionQueryMethodsOnForwardOnlyCursor => false;

    public override bool DoesRepeatableReadCauseReadersToBlockWriters => true;

    public override ISQLExceptionConverter BuildSQLExceptionConverter()
    {
      return (ISQLExceptionConverter) new SQLStateConverter(this.ViolatedConstraintNameExtracter);
    }

    private static int GetAfterSelectInsertPoint(SqlString text)
    {
      return text.StartsWithCaseInsensitive("select") ? 6 : -1;
    }

    public override string GetAddForeignKeyConstraintString(
      string constraintName,
      string[] foreignKey,
      string referencedTable,
      string[] primaryKey,
      bool referencesPrimaryKey)
    {
      StringBuilder stringBuilder = new StringBuilder(200);
      stringBuilder.Append(" add constraint foreign key (").Append(StringHelper.Join(", ", (IEnumerable) foreignKey)).Append(") references ").Append(referencedTable);
      if (!referencesPrimaryKey)
        stringBuilder.Append(" (").Append(StringHelper.Join(", ", (IEnumerable) primaryKey)).Append(')');
      stringBuilder.Append(" constraint ").Append(constraintName);
      return stringBuilder.ToString();
    }
  }
}
