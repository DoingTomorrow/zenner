// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MsSqlCeDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Dialect
{
  public class MsSqlCeDialect : NHibernate.Dialect.Dialect
  {
    public MsSqlCeDialect()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "NCHAR(255)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 4000, "NCHAR");
      this.RegisterColumnType(DbType.AnsiString, "NVARCHAR(255)");
      this.RegisterColumnType(DbType.AnsiString, 4000, "NVARCHAR");
      this.RegisterColumnType(DbType.AnsiString, 1073741823, "NTEXT");
      this.RegisterColumnType(DbType.Binary, "VARBINARY(8000)");
      this.RegisterColumnType(DbType.Binary, 8000, "VARBINARY($l)");
      this.RegisterColumnType(DbType.Binary, 1073741823, "IMAGE");
      this.RegisterColumnType(DbType.Boolean, "BIT");
      this.RegisterColumnType(DbType.Byte, "TINYINT");
      this.RegisterColumnType(DbType.Currency, "MONEY");
      this.RegisterColumnType(DbType.DateTime, "DATETIME");
      this.RegisterColumnType(DbType.Decimal, "NUMERIC(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "NUMERIC($p, $s)");
      this.RegisterColumnType(DbType.Double, "FLOAT");
      this.RegisterColumnType(DbType.Guid, "UNIQUEIDENTIFIER");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INT");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "REAL");
      this.RegisterColumnType(DbType.StringFixedLength, "NCHAR(255)");
      this.RegisterColumnType(DbType.StringFixedLength, 4000, "NCHAR($l)");
      this.RegisterColumnType(DbType.String, "NVARCHAR(255)");
      this.RegisterColumnType(DbType.String, 4000, "NVARCHAR($l)");
      this.RegisterColumnType(DbType.String, 1073741823, "NTEXT");
      this.RegisterColumnType(DbType.Time, "DATETIME");
      this.RegisterFunction("substring", (ISQLFunction) new EmulatedLengthSubstringFunction());
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.SqlServerCeDriver";
      this.DefaultProperties["prepare_sql"] = "false";
    }

    public override string AddColumnString => "add";

    public override string NullColumnString => " null";

    public override bool QualifyIndexName => false;

    public override string ForUpdateString => string.Empty;

    public override bool SupportsIdentityColumns => true;

    public override string IdentitySelectString => "select @@IDENTITY";

    public override string IdentityColumnString => "IDENTITY NOT NULL";

    public override bool SupportsLimit => false;

    public override bool SupportsLimitOffset => false;

    public override bool SupportsVariableLimit => false;

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new MsSqlCeDataBaseSchema(connection);
    }

    public override string Qualify(string catalog, string schema, string table)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      bool flag = false;
      if (!string.IsNullOrEmpty(catalog))
        stringBuilder1.Append(catalog).Append('.');
      StringBuilder stringBuilder2 = new StringBuilder();
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
        stringBuilder2.Append(schema).Append('_');
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
      string str = stringBuilder2.Append(table).ToString();
      if (flag)
        str = this.OpenQuote.ToString() + str + (object) this.CloseQuote;
      return stringBuilder1.Append(str).ToString();
    }
  }
}
