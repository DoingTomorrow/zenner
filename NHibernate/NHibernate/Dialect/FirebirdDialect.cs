// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.FirebirdDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect
{
  public class FirebirdDialect : NHibernate.Dialect.Dialect
  {
    public FirebirdDialect()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(255)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 8000, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR(255)");
      this.RegisterColumnType(DbType.AnsiString, 8000, "VARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "BLOB SUB_TYPE 1");
      this.RegisterColumnType(DbType.Binary, "BLOB SUB_TYPE 0");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "BLOB SUB_TYPE 0");
      this.RegisterColumnType(DbType.Boolean, "SMALLINT");
      this.RegisterColumnType(DbType.Byte, "SMALLINT");
      this.RegisterColumnType(DbType.Currency, "DECIMAL(18,4)");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "TIMESTAMP");
      this.RegisterColumnType(DbType.Decimal, "DECIMAL(18,5)");
      this.RegisterColumnType(DbType.Decimal, 18, "DECIMAL($p, $s)");
      this.RegisterColumnType(DbType.Double, "DOUBLE PRECISION");
      this.RegisterColumnType(DbType.Guid, "CHAR(16) CHARACTER SET OCTETS");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INTEGER");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "FLOAT");
      this.RegisterColumnType(DbType.StringFixedLength, "CHAR(255)");
      this.RegisterColumnType(DbType.StringFixedLength, 4000, "CHAR($l)");
      this.RegisterColumnType(DbType.String, "VARCHAR(255)");
      this.RegisterColumnType(DbType.String, 4000, "VARCHAR($l)");
      this.RegisterColumnType(DbType.String, 1073741823, "BLOB SUB_TYPE 1");
      this.RegisterColumnType(DbType.Time, "TIME");
      this.RegisterFunction("current_timestamp", (ISQLFunction) new FirebirdDialect.CurrentTimeStamp());
      this.RegisterFunction("length", (ISQLFunction) new StandardSafeSQLFunction("char_length", (IType) NHibernateUtil.Int64, 1));
      this.RegisterFunction("nullif", (ISQLFunction) new StandardSafeSQLFunction("nullif", 2));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSafeSQLFunction("lower", (IType) NHibernateUtil.String, 1));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSafeSQLFunction("upper", (IType) NHibernateUtil.String, 1));
      this.RegisterFunction("mod", (ISQLFunction) new StandardSafeSQLFunction("mod", (IType) NHibernateUtil.Double, 2));
      this.RegisterFunction("str", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "cast(?1 as VARCHAR(255))"));
      this.RegisterFunction("sysdate", (ISQLFunction) new FirebirdDialect.CastedFunction("today", (IType) NHibernateUtil.Date));
      this.RegisterFunction("today", (ISQLFunction) new FirebirdDialect.CastedFunction("today", (IType) NHibernateUtil.Date));
      this.RegisterFunction("yesterday", (ISQLFunction) new FirebirdDialect.CastedFunction("yesterday", (IType) NHibernateUtil.Date));
      this.RegisterFunction("tomorrow", (ISQLFunction) new FirebirdDialect.CastedFunction("tomorrow", (IType) NHibernateUtil.Date));
      this.RegisterFunction("now", (ISQLFunction) new FirebirdDialect.CastedFunction("now", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("iif", (ISQLFunction) new StandardSafeSQLFunction("iif", 3));
      this.RegisterFunction("char_length", (ISQLFunction) new StandardSafeSQLFunction("char_length", (IType) NHibernateUtil.Int64, 1));
      this.RegisterFunction("bit_length", (ISQLFunction) new StandardSafeSQLFunction("bit_length", (IType) NHibernateUtil.Int64, 1));
      this.RegisterFunction("octet_length", (ISQLFunction) new StandardSafeSQLFunction("octet_length", (IType) NHibernateUtil.Int64, 1));
      this.RegisterFunction("abs", (ISQLFunction) new StandardSQLFunction("abs", (IType) NHibernateUtil.Double));
      this.RegisterFunction("bin_and", (ISQLFunction) new StandardSQLFunction("bin_and", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("bin_or", (ISQLFunction) new StandardSQLFunction("bin_or", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("bin_xor", (ISQLFunction) new StandardSQLFunction("bin_xor", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("ceiling", (ISQLFunction) new StandardSQLFunction("ceiling", (IType) NHibernateUtil.Double));
      this.RegisterFunction("div", (ISQLFunction) new StandardSQLFunction("div", (IType) NHibernateUtil.Double));
      this.RegisterFunction("dpower", (ISQLFunction) new StandardSQLFunction("dpower", (IType) NHibernateUtil.Double));
      this.RegisterFunction("ln", (ISQLFunction) new StandardSQLFunction("ln", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log", (ISQLFunction) new StandardSQLFunction("log", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log10", (ISQLFunction) new StandardSQLFunction("log10", (IType) NHibernateUtil.Double));
      this.RegisterFunction("pi", (ISQLFunction) new NoArgSQLFunction("pi", (IType) NHibernateUtil.Double));
      this.RegisterFunction("rand", (ISQLFunction) new NoArgSQLFunction("rand", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sign", (ISQLFunction) new StandardSQLFunction("sign", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("sqtr", (ISQLFunction) new StandardSQLFunction("sqtr", (IType) NHibernateUtil.Double));
      this.RegisterFunction("truncate", (ISQLFunction) new StandardSQLFunction("truncate"));
      this.RegisterFunction("floor", (ISQLFunction) new StandardSafeSQLFunction("floor", (IType) NHibernateUtil.Double, 1));
      this.RegisterFunction("round", (ISQLFunction) new StandardSQLFunction("round"));
      this.RegisterFunction("dow", (ISQLFunction) new StandardSQLFunction("dow", (IType) NHibernateUtil.String));
      this.RegisterFunction("sdow", (ISQLFunction) new StandardSQLFunction("sdow", (IType) NHibernateUtil.String));
      this.RegisterFunction("addday", (ISQLFunction) new StandardSQLFunction("addday", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("addhour", (ISQLFunction) new StandardSQLFunction("addhour", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("addmillisecond", (ISQLFunction) new StandardSQLFunction("addmillisecond", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("addminute", (ISQLFunction) new StandardSQLFunction("addminute", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("addmonth", (ISQLFunction) new StandardSQLFunction("addmonth", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("addsecond", (ISQLFunction) new StandardSQLFunction("addsecond", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("addweek", (ISQLFunction) new StandardSQLFunction("addweek", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("addyear", (ISQLFunction) new StandardSQLFunction("addyear", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("getexacttimestamp", (ISQLFunction) new NoArgSQLFunction("getexacttimestamp", (IType) NHibernateUtil.DateTime));
      this.RegisterFunction("ascii_char", (ISQLFunction) new StandardSQLFunction("ascii_char"));
      this.RegisterFunction("ascii_val", (ISQLFunction) new StandardSQLFunction("ascii_val", (IType) NHibernateUtil.Int16));
      this.RegisterFunction("lpad", (ISQLFunction) new StandardSQLFunction("lpad"));
      this.RegisterFunction("ltrim", (ISQLFunction) new StandardSQLFunction("ltrim"));
      this.RegisterFunction("sright", (ISQLFunction) new StandardSQLFunction("sright"));
      this.RegisterFunction("rpad", (ISQLFunction) new StandardSQLFunction("rpad"));
      this.RegisterFunction("rtrim", (ISQLFunction) new StandardSQLFunction("rtrim"));
      this.RegisterFunction("strlen", (ISQLFunction) new StandardSQLFunction("strlen", (IType) NHibernateUtil.Int16));
      this.RegisterFunction("substr", (ISQLFunction) new StandardSQLFunction("substr"));
      this.RegisterFunction("substrlen", (ISQLFunction) new StandardSQLFunction("substrlen", (IType) NHibernateUtil.Int16));
      this.RegisterFunction("locate", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "position(?1, ?2, cast(?3 as int))"));
      this.RegisterFunction("replace", (ISQLFunction) new StandardSafeSQLFunction("replace", (IType) NHibernateUtil.String, 3));
      this.RegisterFunction("string2blob", (ISQLFunction) new StandardSQLFunction("string2blob"));
      this.RegisterFunction("acos", (ISQLFunction) new StandardSQLFunction("acos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("asin", (ISQLFunction) new StandardSQLFunction("asin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan", (ISQLFunction) new StandardSQLFunction("atan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan2", (ISQLFunction) new StandardSQLFunction("atan2", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cos", (ISQLFunction) new StandardSQLFunction("cos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cosh", (ISQLFunction) new StandardSQLFunction("cosh", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cot", (ISQLFunction) new StandardSQLFunction("cot", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sin", (ISQLFunction) new StandardSQLFunction("sin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sinh", (ISQLFunction) new StandardSQLFunction("sinh", (IType) NHibernateUtil.Double));
      this.RegisterFunction("tan", (ISQLFunction) new StandardSQLFunction("tan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("tanh", (ISQLFunction) new StandardSQLFunction("tanh", (IType) NHibernateUtil.Double));
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.FirebirdClientDriver";
    }

    public override string AddColumnString => "add";

    public override string GetSelectSequenceNextValString(string sequenceName)
    {
      return string.Format("gen_id({0}, 1 )", (object) sequenceName);
    }

    public override string GetSequenceNextValString(string sequenceName)
    {
      return string.Format("select gen_id({0}, 1 ) from RDB$DATABASE", (object) sequenceName);
    }

    public override string GetCreateSequenceString(string sequenceName)
    {
      return string.Format("create generator {0}", (object) sequenceName);
    }

    public override string GetDropSequenceString(string sequenceName)
    {
      return string.Format("drop generator {0}", (object) sequenceName);
    }

    public override bool SupportsSequences => true;

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => true;

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      int selectInsertPoint = FirebirdDialect.GetAfterSelectInsertPoint(queryString);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      if (limit != null)
      {
        sqlStringBuilder.Add(" first ");
        sqlStringBuilder.Add(limit);
      }
      if (offset != null)
      {
        sqlStringBuilder.Add(" skip ");
        sqlStringBuilder.Add(offset);
      }
      return queryString.Insert(selectInsertPoint, sqlStringBuilder.ToSqlString());
    }

    private static int GetAfterSelectInsertPoint(SqlString text)
    {
      return text.StartsWithCaseInsensitive("select") ? 6 : -1;
    }

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new FirebirdDataBaseSchema(connection);
    }

    public override string QuerySequencesString
    {
      get
      {
        return "select RDB$GENERATOR_NAME from RDB$GENERATORS where (RDB$SYSTEM_FLAG is NULL) or (RDB$SYSTEM_FLAG <> 1)";
      }
    }

    public override SqlString AddIdentifierOutParameterToInsert(
      SqlString insertString,
      string identifierColumnName,
      string parameterName)
    {
      return insertString.Append(" returning " + identifierColumnName);
    }

    public override long TimestampResolutionInTicks => 1000;

    public override bool SupportsCurrentTimestampSelection => true;

    public override string CurrentTimestampSelectString
    {
      get => "select CURRENT_TIMESTAMP from RDB$DATABASE";
    }

    public override string SelectGUIDString => "select GEN_UUID() from RDB$DATABASE";

    [Serializable]
    private class CastedFunction(string name, IType returnType) : NoArgSQLFunction(name, returnType, false)
    {
      public override SqlString Render(IList args, ISessionFactoryImplementor factory)
      {
        return new SqlStringBuilder().Add("cast('").Add(this.Name).Add("' as ").Add(this.FunctionReturnType.SqlTypes((IMapping) factory)[0].ToString()).Add(")").ToSqlString();
      }
    }

    [Serializable]
    private class CurrentTimeStamp : NoArgSQLFunction
    {
      public CurrentTimeStamp()
        : base("current_timestamp", (IType) NHibernateUtil.DateTime, true)
      {
      }

      public override SqlString Render(IList args, ISessionFactoryImplementor factory)
      {
        return new SqlString(this.Name);
      }
    }
  }
}
