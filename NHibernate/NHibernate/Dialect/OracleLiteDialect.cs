// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.OracleLiteDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Type;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class OracleLiteDialect : Oracle9iDialect
  {
    public OracleLiteDialect()
    {
      this.DefaultProperties["prepare_sql"] = "false";
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.OracleLiteDataClientDriver";
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(255)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 2000, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR2(255)");
      this.RegisterColumnType(DbType.AnsiString, 2000, "VARCHAR2($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "CLOB");
      this.RegisterColumnType(DbType.Binary, "RAW(2000)");
      this.RegisterColumnType(DbType.Binary, 2000, "RAW($l)");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "BLOB");
      this.RegisterColumnType(DbType.Boolean, "NUMBER(1,0)");
      this.RegisterColumnType(DbType.Byte, "NUMBER(3,0)");
      this.RegisterColumnType(DbType.Currency, "NUMBER(19,1)");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "TIMESTAMP(4)");
      this.RegisterColumnType(DbType.Decimal, "NUMBER(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "NUMBER(19, $l)");
      this.RegisterColumnType(DbType.Double, "DOUBLE PRECISION");
      this.RegisterColumnType(DbType.Int16, "NUMBER(5,0)");
      this.RegisterColumnType(DbType.Int32, "NUMBER(10,0)");
      this.RegisterColumnType(DbType.Int64, "NUMBER(20,0)");
      this.RegisterColumnType(DbType.UInt16, "NUMBER(5,0)");
      this.RegisterColumnType(DbType.UInt32, "NUMBER(10,0)");
      this.RegisterColumnType(DbType.UInt64, "NUMBER(20,0)");
      this.RegisterColumnType(DbType.Single, "FLOAT(24)");
      this.RegisterColumnType(DbType.StringFixedLength, "NCHAR(255)");
      this.RegisterColumnType(DbType.StringFixedLength, 2000, "NCHAR($l)");
      this.RegisterColumnType(DbType.String, "VARCHAR2(255)");
      this.RegisterColumnType(DbType.String, 2000, "VARCHAR2($l)");
      this.RegisterColumnType(DbType.String, 1073741823, "CLOB");
      this.RegisterColumnType(DbType.Time, "DATE");
      this.RegisterFunction("stddev", (ISQLFunction) new StandardSQLFunction("stddev", (IType) NHibernateUtil.Double));
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
      this.RegisterFunction("upper", (ISQLFunction) new StandardSQLFunction("upper"));
      this.RegisterFunction("ascii", (ISQLFunction) new StandardSQLFunction("ascii", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("length", (ISQLFunction) new StandardSQLFunction("length", (IType) NHibernateUtil.Int64));
      this.RegisterFunction("to_char", (ISQLFunction) new StandardSQLFunction("to_char", (IType) NHibernateUtil.String));
      this.RegisterFunction("to_date", (ISQLFunction) new StandardSQLFunction("to_date", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("last_day", (ISQLFunction) new StandardSQLFunction("last_day", (IType) NHibernateUtil.Date));
      this.RegisterFunction("sysdate", (ISQLFunction) new NoArgSQLFunction("sysdate", (IType) NHibernateUtil.Date, false));
      this.RegisterFunction("user", (ISQLFunction) new NoArgSQLFunction("user", (IType) NHibernateUtil.String, false));
      this.RegisterFunction("concat", (ISQLFunction) new StandardSQLFunction("concat", (IType) NHibernateUtil.String));
      this.RegisterFunction("instr", (ISQLFunction) new StandardSQLFunction("instr", (IType) NHibernateUtil.String));
      this.RegisterFunction("instrb", (ISQLFunction) new StandardSQLFunction("instrb", (IType) NHibernateUtil.String));
      this.RegisterFunction("lpad", (ISQLFunction) new StandardSQLFunction("lpad", (IType) NHibernateUtil.String));
      this.RegisterFunction("replace", (ISQLFunction) new StandardSQLFunction("replace", (IType) NHibernateUtil.String));
      this.RegisterFunction("rpad", (ISQLFunction) new StandardSQLFunction("rpad", (IType) NHibernateUtil.String));
      this.RegisterFunction("substr", (ISQLFunction) new StandardSQLFunction("substr", (IType) NHibernateUtil.String));
      this.RegisterFunction("substrb", (ISQLFunction) new StandardSQLFunction("substrb", (IType) NHibernateUtil.String));
      this.RegisterFunction("translate", (ISQLFunction) new StandardSQLFunction("translate", (IType) NHibernateUtil.String));
      this.RegisterFunction("mod", (ISQLFunction) new StandardSQLFunction("mod", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("nvl", (ISQLFunction) new StandardSQLFunction("nvl"));
      this.RegisterFunction("add_months", (ISQLFunction) new StandardSQLFunction("add_months", (IType) NHibernateUtil.Date));
      this.RegisterFunction("months_between", (ISQLFunction) new StandardSQLFunction("months_between", (IType) NHibernateUtil.Single));
      this.RegisterFunction("next_day", (ISQLFunction) new StandardSQLFunction("next_day", (IType) NHibernateUtil.Date));
    }

    public override string GetCreateSequenceString(string sequenceName)
    {
      return "create sequence " + sequenceName;
    }

    protected override string GetCreateSequenceString(
      string sequenceName,
      int initialValue,
      int incrementSize)
    {
      return this.GetCreateSequenceString(sequenceName) + " increment by " + (object) incrementSize + " start with " + (object) initialValue;
    }
  }
}
