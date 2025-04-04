// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Oracle8iDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect
{
  public class Oracle8iDialect : NHibernate.Dialect.Dialect
  {
    public override string CurrentTimestampSelectString => "select sysdate from dual";

    public override string CurrentTimestampSQLFunctionName => "sysdate";

    public override string AddColumnString => "add";

    public override string CascadeConstraintsString => " cascade constraints";

    public override string QuerySequencesString => "select sequence_name from user_sequences";

    public override string SelectGUIDString => "select rawtohex(sys_guid()) from dual";

    public override string CreateTemporaryTableString => "create global temporary table";

    public override string CreateTemporaryTablePostfix => "on commit delete rows";

    public override bool IsCurrentTimestampSelectStringCallable => false;

    public Oracle8iDialect()
    {
      this.RegisterCharacterTypeMappings();
      this.RegisterNumericTypeMappings();
      this.RegisterDateTimeTypeMappings();
      this.RegisterLargeObjectTypeMappings();
      this.RegisterGuidTypeMapping();
      this.RegisterReverseHibernateTypeMappings();
      this.RegisterFunctions();
      this.RegisterDefaultProperties();
    }

    protected virtual void RegisterGuidTypeMapping()
    {
      this.RegisterColumnType(DbType.Guid, "RAW(16)");
    }

    protected virtual void RegisterCharacterTypeMappings()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(255)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 2000, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR2(255)");
      this.RegisterColumnType(DbType.AnsiString, 4000, "VARCHAR2($l)");
      this.RegisterColumnType(DbType.StringFixedLength, "NCHAR(255)");
      this.RegisterColumnType(DbType.StringFixedLength, 2000, "NCHAR($l)");
      this.RegisterColumnType(DbType.String, "NVARCHAR2(255)");
      this.RegisterColumnType(DbType.String, 4000, "NVARCHAR2($l)");
    }

    protected virtual void RegisterNumericTypeMappings()
    {
      this.RegisterColumnType(DbType.Boolean, "NUMBER(1,0)");
      this.RegisterColumnType(DbType.Byte, "NUMBER(3,0)");
      this.RegisterColumnType(DbType.Int16, "NUMBER(5,0)");
      this.RegisterColumnType(DbType.Int32, "NUMBER(10,0)");
      this.RegisterColumnType(DbType.Int64, "NUMBER(20,0)");
      this.RegisterColumnType(DbType.UInt16, "NUMBER(5,0)");
      this.RegisterColumnType(DbType.UInt32, "NUMBER(10,0)");
      this.RegisterColumnType(DbType.UInt64, "NUMBER(20,0)");
      this.RegisterColumnType(DbType.Currency, "NUMBER(20,2)");
      this.RegisterColumnType(DbType.Single, "FLOAT(24)");
      this.RegisterColumnType(DbType.Double, "DOUBLE PRECISION");
      this.RegisterColumnType(DbType.Double, 19, "NUMBER($p,$s)");
      this.RegisterColumnType(DbType.Decimal, "NUMBER(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "NUMBER($p,$s)");
    }

    protected virtual void RegisterDateTimeTypeMappings()
    {
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "DATE");
      this.RegisterColumnType(DbType.Time, "DATE");
    }

    protected virtual void RegisterLargeObjectTypeMappings()
    {
      this.RegisterColumnType(DbType.Binary, "RAW(2000)");
      this.RegisterColumnType(DbType.Binary, 2000, "RAW($l)");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "BLOB");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "CLOB");
      this.RegisterColumnType(DbType.String, 1073741823, "NCLOB");
    }

    protected virtual void RegisterReverseHibernateTypeMappings()
    {
    }

    protected virtual void RegisterFunctions()
    {
      this.RegisterFunction("abs", (ISQLFunction) new StandardSQLFunction("abs"));
      this.RegisterFunction("sign", (ISQLFunction) new StandardSQLFunction("sign", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("acos", (ISQLFunction) new StandardSQLFunction("acos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("asin", (ISQLFunction) new StandardSQLFunction("asin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan", (ISQLFunction) new StandardSQLFunction("atan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cos", (ISQLFunction) new StandardSQLFunction("cos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cosh", (ISQLFunction) new StandardSQLFunction("cosh", (IType) NHibernateUtil.Double));
      this.RegisterFunction("exp", (ISQLFunction) new StandardSQLFunction("exp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("ln", (ISQLFunction) new StandardSQLFunction("ln", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sin", (ISQLFunction) new StandardSQLFunction("sin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sinh", (ISQLFunction) new StandardSQLFunction("sinh", (IType) NHibernateUtil.Double));
      this.RegisterFunction("stddev", (ISQLFunction) new StandardSQLFunction("stddev", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sqrt", (ISQLFunction) new StandardSQLFunction("sqrt", (IType) NHibernateUtil.Double));
      this.RegisterFunction("tan", (ISQLFunction) new StandardSQLFunction("tan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("tanh", (ISQLFunction) new StandardSQLFunction("tanh", (IType) NHibernateUtil.Double));
      this.RegisterFunction("variance", (ISQLFunction) new StandardSQLFunction("variance", (IType) NHibernateUtil.Double));
      this.RegisterFunction("round", (ISQLFunction) new StandardSQLFunction("round"));
      this.RegisterFunction("trunc", (ISQLFunction) new StandardSQLFunction("trunc"));
      this.RegisterFunction("ceil", (ISQLFunction) new StandardSQLFunction("ceil"));
      this.RegisterFunction("floor", (ISQLFunction) new StandardSQLFunction("floor"));
      this.RegisterFunction("chr", (ISQLFunction) new StandardSQLFunction("chr", (IType) NHibernateUtil.Character));
      this.RegisterFunction("initcap", (ISQLFunction) new StandardSQLFunction("initcap"));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSQLFunction("lower"));
      this.RegisterFunction("ltrim", (ISQLFunction) new StandardSQLFunction("ltrim"));
      this.RegisterFunction("rtrim", (ISQLFunction) new StandardSQLFunction("rtrim"));
      this.RegisterFunction("soundex", (ISQLFunction) new StandardSQLFunction("soundex"));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSQLFunction("upper"));
      this.RegisterFunction("ascii", (ISQLFunction) new StandardSQLFunction("ascii", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("length", (ISQLFunction) new StandardSQLFunction("length", (IType) NHibernateUtil.Int64));
      this.RegisterFunction("left", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "substr(?1, 1, ?2)"));
      this.RegisterFunction("right", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "substr(?1, -?2)"));
      this.RegisterFunction("to_char", (ISQLFunction) new StandardSQLFunction("to_char", (IType) NHibernateUtil.String));
      this.RegisterFunction("to_date", (ISQLFunction) new StandardSQLFunction("to_date", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("current_date", (ISQLFunction) new NoArgSQLFunction("current_date", (IType) NHibernateUtil.Date, false));
      this.RegisterFunction("current_time", (ISQLFunction) new NoArgSQLFunction("current_timestamp", (IType) NHibernateUtil.Time, false));
      this.RegisterFunction("current_timestamp", (ISQLFunction) new Oracle8iDialect.CurrentTimeStamp());
      this.RegisterFunction("second", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(second from cast(?1 as timestamp))"));
      this.RegisterFunction("minute", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(minute from cast(?1 as timestamp))"));
      this.RegisterFunction("hour", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "extract(hour from cast(?1 as timestamp))"));
      this.RegisterFunction("date", (ISQLFunction) new StandardSQLFunction("trunc", (IType) NHibernateUtil.Date));
      this.RegisterFunction("last_day", (ISQLFunction) new StandardSQLFunction("last_day", (IType) NHibernateUtil.Date));
      this.RegisterFunction("sysdate", (ISQLFunction) new NoArgSQLFunction("sysdate", (IType) NHibernateUtil.Date, false));
      this.RegisterFunction("systimestamp", (ISQLFunction) new NoArgSQLFunction("systimestamp", (IType) NHibernateUtil.Timestamp, false));
      this.RegisterFunction("uid", (ISQLFunction) new NoArgSQLFunction("uid", (IType) NHibernateUtil.Int32, false));
      this.RegisterFunction("user", (ISQLFunction) new NoArgSQLFunction("user", (IType) NHibernateUtil.String, false));
      this.RegisterFunction("rowid", (ISQLFunction) new NoArgSQLFunction("rowid", (IType) NHibernateUtil.Int64, false));
      this.RegisterFunction("rownum", (ISQLFunction) new NoArgSQLFunction("rownum", (IType) NHibernateUtil.Int64, false));
      this.RegisterFunction("instr", (ISQLFunction) new StandardSQLFunction("instr", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("instrb", (ISQLFunction) new StandardSQLFunction("instrb", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("lpad", (ISQLFunction) new StandardSQLFunction("lpad", (IType) NHibernateUtil.String));
      this.RegisterFunction("replace", (ISQLFunction) new StandardSQLFunction("replace", (IType) NHibernateUtil.String));
      this.RegisterFunction("rpad", (ISQLFunction) new StandardSQLFunction("rpad", (IType) NHibernateUtil.String));
      this.RegisterFunction("substr", (ISQLFunction) new StandardSQLFunction("substr", (IType) NHibernateUtil.String));
      this.RegisterFunction("substrb", (ISQLFunction) new StandardSQLFunction("substrb", (IType) NHibernateUtil.String));
      this.RegisterFunction("translate", (ISQLFunction) new StandardSQLFunction("translate", (IType) NHibernateUtil.String));
      this.RegisterFunction("locate", (ISQLFunction) new Oracle8iDialect.LocateFunction());
      this.RegisterFunction("substring", (ISQLFunction) new StandardSQLFunction("substr", (IType) NHibernateUtil.String));
      this.RegisterFunction("locate", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "instr(?2,?1)"));
      this.RegisterFunction("bit_length", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "vsize(?1)*8"));
      this.RegisterFunction("coalesce", (ISQLFunction) new NvlFunction());
      this.RegisterFunction("atan2", (ISQLFunction) new StandardSQLFunction("atan2", (IType) NHibernateUtil.Single));
      this.RegisterFunction("log", (ISQLFunction) new StandardSQLFunction("log", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("mod", (ISQLFunction) new StandardSQLFunction("mod", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("nvl", (ISQLFunction) new StandardSQLFunction("nvl"));
      this.RegisterFunction("nvl2", (ISQLFunction) new StandardSQLFunction("nvl2"));
      this.RegisterFunction("power", (ISQLFunction) new StandardSQLFunction("power", (IType) NHibernateUtil.Single));
      this.RegisterFunction("add_months", (ISQLFunction) new StandardSQLFunction("add_months", (IType) NHibernateUtil.Date));
      this.RegisterFunction("months_between", (ISQLFunction) new StandardSQLFunction("months_between", (IType) NHibernateUtil.Single));
      this.RegisterFunction("next_day", (ISQLFunction) new StandardSQLFunction("next_day", (IType) NHibernateUtil.Date));
      this.RegisterFunction("str", (ISQLFunction) new StandardSQLFunction("to_char", (IType) NHibernateUtil.String));
    }

    protected internal virtual void RegisterDefaultProperties()
    {
      this.DefaultProperties["jdbc.use_get_generated_keys"] = "false";
    }

    public override JoinFragment CreateOuterJoinFragment()
    {
      return (JoinFragment) new OracleJoinFragment();
    }

    public override CaseFragment CreateCaseFragment()
    {
      return (CaseFragment) new DecodeCaseFragment((NHibernate.Dialect.Dialect) this);
    }

    public override SqlString GetLimitString(SqlString sql, SqlString offset, SqlString limit)
    {
      sql = sql.Trim();
      bool flag = false;
      if (sql.EndsWithCaseInsensitive(" for update"))
      {
        sql = sql.Substring(0, sql.Length - 11);
        flag = true;
      }
      string columnOrAliasNames = this.ExtractColumnOrAliasNames(sql);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(sql.Parts.Count + 10);
      if (offset != null)
        sqlStringBuilder.Add("select " + columnOrAliasNames + " from ( select row_.*, rownum rownum_ from ( ");
      else
        sqlStringBuilder.Add("select " + columnOrAliasNames + " from ( ");
      sqlStringBuilder.Add(sql);
      if (offset != null && limit != null)
        sqlStringBuilder.Add(" ) row_ where rownum <=").Add(limit).Add(") where rownum_ >").Add(offset);
      else if (limit != null)
        sqlStringBuilder.Add(" ) where rownum <=").Add(limit);
      else
        sqlStringBuilder.Add(" ) row_ ) where rownum_ >").Add(offset);
      if (flag)
        sqlStringBuilder.Add(" for update");
      return sqlStringBuilder.ToSqlString();
    }

    private string ExtractColumnOrAliasNames(SqlString select)
    {
      List<SqlString> columnsOrAliases;
      NHibernate.Dialect.Dialect.ExtractColumnOrAliasNames(select, out columnsOrAliases, out Dictionary<SqlString, SqlString> _, out Dictionary<SqlString, SqlString> _);
      return StringHelper.Join(",", (IEnumerable) columnsOrAliases);
    }

    public virtual string GetBasicSelectClauseNullString(SqlType sqlType)
    {
      return base.GetSelectClauseNullString(sqlType);
    }

    public override string GetSelectClauseNullString(SqlType sqlType)
    {
      switch (sqlType.DbType)
      {
        case DbType.AnsiString:
        case DbType.String:
        case DbType.AnsiStringFixedLength:
        case DbType.StringFixedLength:
          return "to_char(null)";
        case DbType.Date:
        case DbType.DateTime:
        case DbType.Time:
          return "to_date(null)";
        default:
          return "to_number(null)";
      }
    }

    public override string GetSequenceNextValString(string sequenceName)
    {
      return "select " + this.GetSelectSequenceNextValString(sequenceName) + " from dual";
    }

    public override string GetSelectSequenceNextValString(string sequenceName)
    {
      return sequenceName + ".nextval";
    }

    public override SqlString AddIdentifierOutParameterToInsert(
      SqlString insertString,
      string identifierColumnName,
      string parameterName)
    {
      return insertString.Append(" returning " + identifierColumnName + " into :" + parameterName);
    }

    public override string GetCreateSequenceString(string sequenceName)
    {
      return "create sequence " + sequenceName;
    }

    public override string GetDropSequenceString(string sequenceName)
    {
      return "drop sequence " + sequenceName;
    }

    public override bool DropConstraints => false;

    public override string ForUpdateNowaitString => " for update nowait";

    public override bool SupportsSequences => true;

    public override bool SupportsPooledSequences => true;

    public override bool SupportsLimit => true;

    public override string GetForUpdateString(string aliases)
    {
      return this.ForUpdateString + " of " + aliases;
    }

    public override string GetForUpdateNowaitString(string aliases)
    {
      return this.ForUpdateString + " of " + aliases + " nowait";
    }

    public override bool UseMaxForLimit => true;

    public override bool ForUpdateOfColumns => true;

    public override bool SupportsUnionAll => true;

    public override bool SupportsCommentOn => true;

    public override bool SupportsTemporaryTables => true;

    public override string GenerateTemporaryTableName(string baseTableName)
    {
      string temporaryTableName = base.GenerateTemporaryTableName(baseTableName);
      return temporaryTableName.Length <= 30 ? temporaryTableName : temporaryTableName.Substring(1, 29);
    }

    public override bool DropTemporaryTableAfterUse() => false;

    public override bool SupportsCurrentTimestampSelection => true;

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new OracleDataBaseSchema(connection);
    }

    public override bool SupportsEmptyInList => false;

    public override bool SupportsExistsInSelect => false;

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

    [Serializable]
    private class LocateFunction : ISQLFunction
    {
      private static readonly ISQLFunction LocateWith2Params = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "instr(?2, ?1)");
      private static readonly ISQLFunction LocateWith3Params = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "instr(?2, ?1, ?3)");

      public IType ReturnType(IType columnType, IMapping mapping) => (IType) NHibernateUtil.Int32;

      public bool HasArguments => true;

      public bool HasParenthesesIfNoArguments => true;

      public SqlString Render(IList args, ISessionFactoryImplementor factory)
      {
        if (args.Count != 2 && args.Count != 3)
          throw new QueryException("'locate' function takes 2 or 3 arguments");
        return args.Count == 2 ? Oracle8iDialect.LocateFunction.LocateWith2Params.Render(args, factory) : Oracle8iDialect.LocateFunction.LocateWith3Params.Render(args, factory);
      }
    }
  }
}
