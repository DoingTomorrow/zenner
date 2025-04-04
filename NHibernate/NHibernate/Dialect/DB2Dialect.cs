// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.DB2Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Data;
using System.Text;

#nullable disable
namespace NHibernate.Dialect
{
  public class DB2Dialect : NHibernate.Dialect.Dialect
  {
    public DB2Dialect()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(254)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 254, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR(254)");
      this.RegisterColumnType(DbType.AnsiString, 8000, "VARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "CLOB");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "BLOB");
      this.RegisterColumnType(DbType.Boolean, "SMALLINT");
      this.RegisterColumnType(DbType.Byte, "SMALLINT");
      this.RegisterColumnType(DbType.Currency, "DECIMAL(16,4)");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "TIMESTAMP");
      this.RegisterColumnType(DbType.Decimal, "DECIMAL(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "DECIMAL(19, $l)");
      this.RegisterColumnType(DbType.Double, "DOUBLE");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INTEGER");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "REAL");
      this.RegisterColumnType(DbType.StringFixedLength, "CHAR(254)");
      this.RegisterColumnType(DbType.StringFixedLength, 254, "CHAR($l)");
      this.RegisterColumnType(DbType.String, "VARCHAR(254)");
      this.RegisterColumnType(DbType.String, 8000, "VARCHAR($l)");
      this.RegisterColumnType(DbType.String, int.MaxValue, "CLOB");
      this.RegisterColumnType(DbType.Time, "TIME");
      this.RegisterFunction("abs", (ISQLFunction) new StandardSQLFunction("abs"));
      this.RegisterFunction("absval", (ISQLFunction) new StandardSQLFunction("absval"));
      this.RegisterFunction("sign", (ISQLFunction) new StandardSQLFunction("sign", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("ceiling", (ISQLFunction) new StandardSQLFunction("ceiling"));
      this.RegisterFunction("ceil", (ISQLFunction) new StandardSQLFunction("ceil"));
      this.RegisterFunction("floor", (ISQLFunction) new StandardSQLFunction("floor"));
      this.RegisterFunction("round", (ISQLFunction) new StandardSQLFunction("round"));
      this.RegisterFunction("acos", (ISQLFunction) new StandardSQLFunction("acos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("asin", (ISQLFunction) new StandardSQLFunction("asin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan", (ISQLFunction) new StandardSQLFunction("atan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cos", (ISQLFunction) new StandardSQLFunction("cos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cot", (ISQLFunction) new StandardSQLFunction("cot", (IType) NHibernateUtil.Double));
      this.RegisterFunction("degrees", (ISQLFunction) new StandardSQLFunction("degrees", (IType) NHibernateUtil.Double));
      this.RegisterFunction("exp", (ISQLFunction) new StandardSQLFunction("exp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("float", (ISQLFunction) new StandardSQLFunction("float", (IType) NHibernateUtil.Double));
      this.RegisterFunction("hex", (ISQLFunction) new StandardSQLFunction("hex", (IType) NHibernateUtil.String));
      this.RegisterFunction("ln", (ISQLFunction) new StandardSQLFunction("ln", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log", (ISQLFunction) new StandardSQLFunction("log", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log10", (ISQLFunction) new StandardSQLFunction("log10", (IType) NHibernateUtil.Double));
      this.RegisterFunction("radians", (ISQLFunction) new StandardSQLFunction("radians", (IType) NHibernateUtil.Double));
      this.RegisterFunction("rand", (ISQLFunction) new NoArgSQLFunction("rand", (IType) NHibernateUtil.Double));
      this.RegisterFunction("sin", (ISQLFunction) new StandardSQLFunction("sin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("soundex", (ISQLFunction) new StandardSQLFunction("soundex", (IType) NHibernateUtil.String));
      this.RegisterFunction("sqrt", (ISQLFunction) new StandardSQLFunction("sqrt", (IType) NHibernateUtil.Double));
      this.RegisterFunction("stddev", (ISQLFunction) new StandardSQLFunction("stddev", (IType) NHibernateUtil.Double));
      this.RegisterFunction("tan", (ISQLFunction) new StandardSQLFunction("tan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("variance", (ISQLFunction) new StandardSQLFunction("variance", (IType) NHibernateUtil.Double));
      this.RegisterFunction("julian_day", (ISQLFunction) new StandardSQLFunction("julian_day", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("microsecond", (ISQLFunction) new StandardSQLFunction("microsecond", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("midnight_seconds", (ISQLFunction) new StandardSQLFunction("midnight_seconds", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("minute", (ISQLFunction) new StandardSQLFunction("minute", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("month", (ISQLFunction) new StandardSQLFunction("month", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("monthname", (ISQLFunction) new StandardSQLFunction("monthname", (IType) NHibernateUtil.String));
      this.RegisterFunction("quarter", (ISQLFunction) new StandardSQLFunction("quarter", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("hour", (ISQLFunction) new StandardSQLFunction("hour", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("second", (ISQLFunction) new StandardSQLFunction("second", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("date", (ISQLFunction) new StandardSQLFunction("date", (IType) NHibernateUtil.Date));
      this.RegisterFunction("day", (ISQLFunction) new StandardSQLFunction("day", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("dayname", (ISQLFunction) new StandardSQLFunction("dayname", (IType) NHibernateUtil.String));
      this.RegisterFunction("dayofweek", (ISQLFunction) new StandardSQLFunction("dayofweek", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("dayofweek_iso", (ISQLFunction) new StandardSQLFunction("dayofweek_iso", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("dayofyear", (ISQLFunction) new StandardSQLFunction("dayofyear", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("days", (ISQLFunction) new StandardSQLFunction("days", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("time", (ISQLFunction) new StandardSQLFunction("time", (IType) NHibernateUtil.Time));
      this.RegisterFunction("timestamp", (ISQLFunction) new StandardSQLFunction("timestamp", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("timestamp_iso", (ISQLFunction) new StandardSQLFunction("timestamp_iso", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("week", (ISQLFunction) new StandardSQLFunction("week", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("week_iso", (ISQLFunction) new StandardSQLFunction("week_iso", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("year", (ISQLFunction) new StandardSQLFunction("year", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("double", (ISQLFunction) new StandardSQLFunction("double", (IType) NHibernateUtil.Double));
      this.RegisterFunction("varchar", (ISQLFunction) new StandardSQLFunction("varchar", (IType) NHibernateUtil.String));
      this.RegisterFunction("real", (ISQLFunction) new StandardSQLFunction("real", (IType) NHibernateUtil.Single));
      this.RegisterFunction("bigint", (ISQLFunction) new StandardSQLFunction("bigint", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("char", (ISQLFunction) new StandardSQLFunction("char", (IType) NHibernateUtil.Character));
      this.RegisterFunction("integer", (ISQLFunction) new StandardSQLFunction("integer", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("smallint", (ISQLFunction) new StandardSQLFunction("smallint", (IType) NHibernateUtil.Int16));
      this.RegisterFunction("digits", (ISQLFunction) new StandardSQLFunction("digits", (IType) NHibernateUtil.String));
      this.RegisterFunction("chr", (ISQLFunction) new StandardSQLFunction("chr", (IType) NHibernateUtil.Character));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSQLFunction("upper"));
      this.RegisterFunction("ucase", (ISQLFunction) new StandardSQLFunction("ucase"));
      this.RegisterFunction("lcase", (ISQLFunction) new StandardSQLFunction("lcase"));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSQLFunction("lower"));
      this.RegisterFunction("length", (ISQLFunction) new StandardSQLFunction("length", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("ltrim", (ISQLFunction) new StandardSQLFunction("ltrim"));
      this.RegisterFunction("mod", (ISQLFunction) new StandardSQLFunction("mod", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("substring", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "substring(?1, ?2, ?3)"));
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.DB2Driver";
    }

    public override string AddColumnString => "add column";

    public override bool DropConstraints => false;

    public override bool SupportsIdentityColumns => true;

    public override string IdentitySelectString
    {
      get => "select identity_val_local() from sysibm.sysdummy1";
    }

    public override string IdentityColumnString => "not null generated by default as identity";

    public override string IdentityInsertString => "default";

    public override string GetSelectSequenceNextValString(string sequenceName)
    {
      return "nextval for " + sequenceName;
    }

    public override string GetSequenceNextValString(string sequenceName)
    {
      return "values nextval for " + sequenceName;
    }

    public override string GetCreateSequenceString(string sequenceName)
    {
      return "create sequence " + sequenceName;
    }

    public override string GetDropSequenceString(string sequenceName)
    {
      return "drop sequence " + sequenceName + " restrict";
    }

    public override bool SupportsSequences => true;

    public override bool SupportsLimit => true;

    public override bool UseMaxForLimit => true;

    public override SqlString GetLimitString(
      SqlString querySqlString,
      SqlString offset,
      SqlString limit)
    {
      string rowNumber = DB2Dialect.GetRowNumber(querySqlString);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add("select * from (select ").Add(rowNumber).Add(querySqlString.Substring(7)).Add(") as tempresult where rownum ");
      if (offset != null && limit != null)
        sqlStringBuilder.Add("between ").Add(offset).Add("+1 and ").Add(limit);
      else if (limit != null)
        sqlStringBuilder.Add("<= ").Add(limit);
      else
        sqlStringBuilder.Add("> ").Add(offset);
      return sqlStringBuilder.ToSqlString();
    }

    private static string GetRowNumber(SqlString sql)
    {
      return new StringBuilder().Append("rownumber() over(").Append((object) sql.SubstringStartingWithLast("order by")).Append(") as rownum, ").ToString();
    }

    public override string ForUpdateString => " for read only with rs";
  }
}
