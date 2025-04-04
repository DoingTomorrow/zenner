// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.SybaseASE15Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Dialect
{
  public class SybaseASE15Dialect : NHibernate.Dialect.Dialect
  {
    public SybaseASE15Dialect()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.SybaseAseClientDriver";
      this.RegisterColumnType(DbType.Boolean, "tinyint");
      this.RegisterColumnType(DbType.Int16, "smallint");
      this.RegisterColumnType(DbType.Int16, (int) byte.MaxValue, "tinyint");
      this.RegisterColumnType(DbType.Int32, "int");
      this.RegisterColumnType(DbType.Int64, "bigint");
      this.RegisterColumnType(DbType.Decimal, "numeric(18,0)");
      this.RegisterColumnType(DbType.Single, "real");
      this.RegisterColumnType(DbType.Double, "float");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "char(1)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, (int) byte.MaxValue, "char($l)");
      this.RegisterColumnType(DbType.StringFixedLength, "nchar(1)");
      this.RegisterColumnType(DbType.StringFixedLength, (int) byte.MaxValue, "nchar($l)");
      this.RegisterColumnType(DbType.AnsiString, "varchar(255)");
      this.RegisterColumnType(DbType.AnsiString, 16384, "varchar($l)");
      this.RegisterColumnType(DbType.String, "nvarchar(255)");
      this.RegisterColumnType(DbType.String, 16384, "nvarchar($l)");
      this.RegisterColumnType(DbType.String, int.MaxValue, "text");
      this.RegisterColumnType(DbType.DateTime, "datetime");
      this.RegisterColumnType(DbType.Time, "time");
      this.RegisterColumnType(DbType.Date, "date");
      this.RegisterColumnType(DbType.Binary, 8000, "varbinary($l)");
      this.RegisterColumnType(DbType.Binary, "varbinary");
      this.RegisterFunction("abs", (ISQLFunction) new StandardSQLFunction("abs"));
      this.RegisterFunction("acos", (ISQLFunction) new StandardSQLFunction("acos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("ascii", (ISQLFunction) new StandardSQLFunction("ascii", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("asin", (ISQLFunction) new StandardSQLFunction("asin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("atan", (ISQLFunction) new StandardSQLFunction("atan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("bit_length", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datalength(?1) * 8"));
      this.RegisterFunction("ceiling", (ISQLFunction) new StandardSQLFunction("ceiling"));
      this.RegisterFunction("char", (ISQLFunction) new StandardSQLFunction("char", (IType) NHibernateUtil.String));
      this.RegisterFunction("concat", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "(", "+", ")"));
      this.RegisterFunction("cos", (ISQLFunction) new StandardSQLFunction("cos", (IType) NHibernateUtil.Double));
      this.RegisterFunction("cot", (ISQLFunction) new StandardSQLFunction("cot", (IType) NHibernateUtil.Double));
      this.RegisterFunction("current_date", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.Date));
      this.RegisterFunction("current_time", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.Time));
      this.RegisterFunction("current_timestamp", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("datename", (ISQLFunction) new StandardSQLFunction("datename", (IType) NHibernateUtil.String));
      this.RegisterFunction("day", (ISQLFunction) new StandardSQLFunction("day", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("degrees", (ISQLFunction) new StandardSQLFunction("degrees", (IType) NHibernateUtil.Double));
      this.RegisterFunction("exp", (ISQLFunction) new StandardSQLFunction("exp", (IType) NHibernateUtil.Double));
      this.RegisterFunction("extract", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(?1, ?3)"));
      this.RegisterFunction("floor", (ISQLFunction) new StandardSQLFunction("floor"));
      this.RegisterFunction("getdate", (ISQLFunction) new NoArgSQLFunction("getdate", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("getutcdate", (ISQLFunction) new NoArgSQLFunction("getutcdate", (IType) NHibernateUtil.Timestamp));
      this.RegisterFunction("hour", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(hour, ?1)"));
      this.RegisterFunction("isnull", (ISQLFunction) new StandardSQLFunction("isnull"));
      this.RegisterFunction("len", (ISQLFunction) new StandardSQLFunction("len", (IType) NHibernateUtil.Int64));
      this.RegisterFunction("length", (ISQLFunction) new StandardSQLFunction("len", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("locate", (ISQLFunction) new CharIndexFunction());
      this.RegisterFunction("log", (ISQLFunction) new StandardSQLFunction("log", (IType) NHibernateUtil.Double));
      this.RegisterFunction("log10", (ISQLFunction) new StandardSQLFunction("log10", (IType) NHibernateUtil.Double));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSQLFunction("lower"));
      this.RegisterFunction("ltrim", (ISQLFunction) new StandardSQLFunction("ltrim"));
      this.RegisterFunction("minute", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(minute, ?1)"));
      this.RegisterFunction("mod", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "?1 % ?2"));
      this.RegisterFunction("month", (ISQLFunction) new StandardSQLFunction("month", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("pi", (ISQLFunction) new NoArgSQLFunction("pi", (IType) NHibernateUtil.Double));
      this.RegisterFunction("radians", (ISQLFunction) new StandardSQLFunction("radians", (IType) NHibernateUtil.Double));
      this.RegisterFunction("rand", (ISQLFunction) new StandardSQLFunction("rand", (IType) NHibernateUtil.Double));
      this.RegisterFunction("reverse", (ISQLFunction) new StandardSQLFunction("reverse"));
      this.RegisterFunction("round", (ISQLFunction) new StandardSQLFunction("round"));
      this.RegisterFunction("rtrim", (ISQLFunction) new StandardSQLFunction("rtrim"));
      this.RegisterFunction("second", (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.Int32, "datepart(second, ?1)"));
      this.RegisterFunction("sign", (ISQLFunction) new StandardSQLFunction("sign", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("sin", (ISQLFunction) new StandardSQLFunction("sin", (IType) NHibernateUtil.Double));
      this.RegisterFunction("space", (ISQLFunction) new StandardSQLFunction("space", (IType) NHibernateUtil.String));
      this.RegisterFunction("sqrt", (ISQLFunction) new StandardSQLFunction("sqrt", (IType) NHibernateUtil.Double));
      this.RegisterFunction("square", (ISQLFunction) new StandardSQLFunction("square"));
      this.RegisterFunction("str", (ISQLFunction) new StandardSQLFunction("str", (IType) NHibernateUtil.String));
      this.RegisterFunction("tan", (ISQLFunction) new StandardSQLFunction("tan", (IType) NHibernateUtil.Double));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSQLFunction("upper"));
      this.RegisterFunction("user", (ISQLFunction) new NoArgSQLFunction("user", (IType) NHibernateUtil.String));
      this.RegisterFunction("year", (ISQLFunction) new StandardSQLFunction("year", (IType) NHibernateUtil.Int32));
      this.RegisterFunction("substring", (ISQLFunction) new EmulatedLengthSubstringFunction());
    }

    public override string AddColumnString => "add";

    public override string NullColumnString => " null";

    public override bool QualifyIndexName => false;

    public override bool SupportsIdentityColumns => true;

    public override string IdentitySelectString => "select @@identity";

    public override string IdentityColumnString => "identity not null";

    public override bool SupportsInsertSelectIdentity => true;

    public override bool SupportsCurrentTimestampSelection => true;

    public override bool IsCurrentTimestampSelectStringCallable => false;

    public override string CurrentTimestampSelectString => "select getdate()";

    public override bool SupportsTemporaryTables => false;

    public override string SelectGUIDString => "select newid()";

    public override bool SupportsEmptyInList => false;

    public override bool SupportsUnionAll => true;

    public override bool SupportsExistsInSelect => false;

    public override bool DoesReadCommittedCauseWritersToBlockReaders => true;

    public override bool DoesRepeatableReadCauseReadersToBlockWriters => true;

    public override bool SupportsCascadeDelete => false;

    public override int MaxAliasLength => 30;

    public override bool AreStringComparisonsCaseInsensitive => false;

    public override string CurrentTimestampSQLFunctionName => "getdate()";

    public override bool SupportsExpectedLobUsagePattern => false;

    public override char OpenQuote => '[';

    public override char CloseQuote => ']';

    public override string ForUpdateString => string.Empty;

    public override string GenerateTemporaryTableName(string baseTableName) => "#" + baseTableName;

    public override bool DropTemporaryTableAfterUse() => true;

    public override SqlString AppendIdentitySelectToInsert(SqlString insertString)
    {
      return insertString.Append("\nselect @@identity");
    }

    public override string AppendLockHint(LockMode lockMode, string tableName)
    {
      return lockMode.GreaterThan(LockMode.Read) ? tableName + " holdlock" : tableName;
    }

    public override SqlString ApplyLocksToSql(
      SqlString sql,
      IDictionary<string, LockMode> aliasedLockModes,
      IDictionary<string, string[]> keyColumnNames)
    {
      StringBuilder stringBuilder = new StringBuilder(sql.ToString());
      int num1 = 0;
      foreach (KeyValuePair<string, LockMode> aliasedLockMode in (IEnumerable<KeyValuePair<string, LockMode>>) aliasedLockModes)
      {
        LockMode lockMode = aliasedLockMode.Value;
        if (lockMode.GreaterThan(LockMode.Read))
        {
          string key = aliasedLockMode.Key;
          int num2 = -1;
          int num3 = -1;
          if (sql.EndsWith(" " + key))
          {
            num2 = sql.Length - key.Length + num1;
            num3 = num2 + key.Length;
          }
          else
          {
            int num4 = sql.IndexOfCaseInsensitive(" " + key + " ");
            if (num4 <= -1)
              num4 = sql.IndexOfCaseInsensitive(" " + key + ",");
            if (num4 > -1)
            {
              num2 = num4 + num1 + 1;
              num3 = num2 + key.Length;
            }
          }
          if (num2 > -1)
          {
            string str = this.AppendLockHint(lockMode, key);
            stringBuilder.Remove(num2, num3 - num2 + 1);
            stringBuilder.Insert(num2, str);
            num1 += str.Length - key.Length;
          }
        }
      }
      return new SqlString(stringBuilder.ToString());
    }

    public override int RegisterResultSetOutParameter(DbCommand statement, int position)
    {
      return position;
    }

    public override DbDataReader GetResultSet(DbCommand statement) => statement.ExecuteReader();
  }
}
