// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.SybaseASA9Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect
{
  public class SybaseASA9Dialect : NHibernate.Dialect.Dialect
  {
    public SybaseASA9Dialect()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.SybaseAsaClientDriver";
      this.DefaultProperties["prepare_sql"] = "false";
      this.RegisterColumnType(DbType.AnsiStringFixedLength, (int) byte.MaxValue, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR(255)");
      this.RegisterColumnType(DbType.AnsiString, (int) byte.MaxValue, "VARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "LONG VARCHAR");
      this.RegisterColumnType(DbType.Binary, "BINARY(255)");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "LONG BINARY");
      this.RegisterColumnType(DbType.Boolean, "BIT");
      this.RegisterColumnType(DbType.Byte, "SMALLINT");
      this.RegisterColumnType(DbType.Currency, "DECIMAL(18,4)");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "TIMESTAMP");
      this.RegisterColumnType(DbType.Decimal, "DECIMAL(18,5)");
      this.RegisterColumnType(DbType.Decimal, 18, "DECIMAL(18,$l)");
      this.RegisterColumnType(DbType.Double, "DOUBLE");
      this.RegisterColumnType(DbType.Guid, "CHAR(16)");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INTEGER");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "FLOAT");
      this.RegisterColumnType(DbType.StringFixedLength, (int) byte.MaxValue, "CHAR($l)");
      this.RegisterColumnType(DbType.String, 1073741823, "LONG VARCHAR");
      this.RegisterColumnType(DbType.String, (int) byte.MaxValue, "VARCHAR($l)");
      this.RegisterColumnType(DbType.String, "LONG VARCHAR");
      this.RegisterColumnType(DbType.Time, "TIME");
      this.RegisterColumnType(DbType.SByte, "SMALLINT");
      this.RegisterColumnType(DbType.UInt16, "UNSIGNED SMALLINT");
      this.RegisterColumnType(DbType.UInt32, "UNSIGNED INT");
      this.RegisterColumnType(DbType.UInt64, "UNSIGNED BIGINT");
      this.RegisterColumnType(DbType.VarNumeric, "NUMERIC($l)");
      this.RegisterFunction("current_timestamp", (ISQLFunction) new StandardSQLFunction("current_timestamp"));
      this.RegisterFunction("length", (ISQLFunction) new StandardSafeSQLFunction("length", (IType) NHibernateUtil.String, 1));
      this.RegisterFunction("nullif", (ISQLFunction) new StandardSafeSQLFunction("nullif", 2));
      this.RegisterFunction("lower", (ISQLFunction) new StandardSafeSQLFunction("lower", (IType) NHibernateUtil.String, 1));
      this.RegisterFunction("upper", (ISQLFunction) new StandardSafeSQLFunction("upper", (IType) NHibernateUtil.String, 1));
      this.RegisterFunction("now", (ISQLFunction) new StandardSQLFunction("now"));
      this.RegisterKeyword("top");
    }

    public override bool SupportsLimit => true;

    public override bool SupportsVariableLimit => false;

    public override bool OffsetStartsAtOne => true;

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      int selectInsertPoint = SybaseASA9Dialect.GetAfterSelectInsertPoint(queryString);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add(" top ");
      if (limit != null)
        sqlStringBuilder.Add(limit);
      else
        sqlStringBuilder.Add(int.MaxValue.ToString());
      if (offset != null)
      {
        sqlStringBuilder.Add(" start at ");
        sqlStringBuilder.Add(offset);
      }
      return queryString.Insert(selectInsertPoint, sqlStringBuilder.ToSqlString());
    }

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new SybaseAnywhereDataBaseMetaData(connection);
    }

    public override string AddColumnString => "add";

    public override string NullColumnString => " null";

    public override bool QualifyIndexName => false;

    public override string ForUpdateString => string.Empty;

    public override bool SupportsIdentityColumns => true;

    public override string IdentitySelectString => "select @@identity";

    public override string IdentityColumnString => "identity not null";

    public override string NoColumnsInsertString => "default values";

    public override bool DropConstraints => false;

    private static int GetAfterSelectInsertPoint(SqlString sql)
    {
      string[] strArray = new string[3]
      {
        "select distinct",
        "select all",
        "select"
      };
      for (int index = 0; index != strArray.Length; ++index)
      {
        string str = strArray[index];
        if (sql.StartsWithCaseInsensitive(str))
          return str.Length;
      }
      return 0;
    }
  }
}
