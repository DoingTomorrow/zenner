// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MySQLDialect
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
using System.Text;

#nullable disable
namespace NHibernate.Dialect
{
  public class MySQLDialect : NHibernate.Dialect.Dialect
  {
    private readonly TypeNames castTypeNames = new TypeNames();

    public MySQLDialect()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(255)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, (int) byte.MaxValue, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, (int) ushort.MaxValue, "TEXT");
      this.RegisterColumnType(DbType.AnsiStringFixedLength, 16777215, "MEDIUMTEXT");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR(255)");
      this.RegisterColumnType(DbType.AnsiString, (int) byte.MaxValue, "VARCHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, (int) ushort.MaxValue, "TEXT");
      this.RegisterColumnType(DbType.AnsiString, 16777215, "MEDIUMTEXT");
      this.RegisterColumnType(DbType.StringFixedLength, "CHAR(255)");
      this.RegisterColumnType(DbType.StringFixedLength, (int) byte.MaxValue, "CHAR($l)");
      this.RegisterColumnType(DbType.StringFixedLength, (int) ushort.MaxValue, "TEXT");
      this.RegisterColumnType(DbType.StringFixedLength, 16777215, "MEDIUMTEXT");
      this.RegisterColumnType(DbType.String, "VARCHAR(255)");
      this.RegisterColumnType(DbType.String, (int) byte.MaxValue, "VARCHAR($l)");
      this.RegisterColumnType(DbType.String, (int) ushort.MaxValue, "TEXT");
      this.RegisterColumnType(DbType.String, 16777215, "MEDIUMTEXT");
      this.RegisterColumnType(DbType.Binary, "LONGBLOB");
      this.RegisterColumnType(DbType.Binary, (int) sbyte.MaxValue, "TINYBLOB");
      this.RegisterColumnType(DbType.Binary, (int) ushort.MaxValue, "BLOB");
      this.RegisterColumnType(DbType.Binary, 16777215, "MEDIUMBLOB");
      this.RegisterColumnType(DbType.Boolean, "TINYINT(1)");
      this.RegisterColumnType(DbType.Byte, "TINYINT UNSIGNED");
      this.RegisterColumnType(DbType.Currency, "NUMERIC(18,4)");
      this.RegisterColumnType(DbType.Decimal, "NUMERIC(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "NUMERIC($p, $s)");
      this.RegisterColumnType(DbType.Double, "DOUBLE");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INTEGER");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "FLOAT");
      this.RegisterColumnType(DbType.Byte, 1, "BIT");
      this.RegisterColumnType(DbType.SByte, "TINYINT");
      this.RegisterColumnType(DbType.UInt16, "SMALLINT UNSIGNED");
      this.RegisterColumnType(DbType.UInt32, "INTEGER UNSIGNED");
      this.RegisterColumnType(DbType.UInt64, "BIGINT UNSIGNED");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "DATETIME");
      this.RegisterColumnType(DbType.Time, "TIME");
      this.RegisterColumnType(DbType.Guid, "VARCHAR(40)");
      this.RegisterCastTypes();
      this.RegisterFunction("concat", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "concat(", ",", ")"));
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.MySqlDataDriver";
    }

    public override string AddColumnString => "add column";

    public override bool QualifyIndexName => false;

    public override bool SupportsIdentityColumns => true;

    public override string IdentitySelectString => "SELECT LAST_INSERT_ID()";

    public override string IdentityColumnString => "NOT NULL AUTO_INCREMENT";

    public override char CloseQuote => '`';

    public override char OpenQuote => '`';

    public override bool SupportsIfExistsBeforeTableName => true;

    public override bool SupportsLimit => true;

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new MySQLDataBaseSchema(connection);
    }

    public override bool SupportsSubSelects => false;

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(queryString);
      sqlStringBuilder.Add(" limit ");
      if (offset != null)
      {
        sqlStringBuilder.Add(offset);
        sqlStringBuilder.Add(", ");
      }
      if (limit != null)
        sqlStringBuilder.Add(limit);
      else
        sqlStringBuilder.Add(int.MaxValue.ToString());
      return sqlStringBuilder.ToSqlString();
    }

    public override string GetAddForeignKeyConstraintString(
      string constraintName,
      string[] foreignKey,
      string referencedTable,
      string[] primaryKey,
      bool referencesPrimaryKey)
    {
      string str = string.Join(", ", foreignKey);
      return new StringBuilder(30).Append(" add index (").Append(str).Append("), add constraint ").Append(constraintName).Append(" foreign key (").Append(str).Append(") references ").Append(referencedTable).Append(" (").Append(string.Join(", ", primaryKey)).Append(')').ToString();
    }

    public override string GetDropForeignKeyConstraintString(string constraintName)
    {
      return " drop foreign key " + constraintName;
    }

    public override string GetDropPrimaryKeyConstraintString(string constraintName)
    {
      return " drop primary key " + constraintName;
    }

    public override string GetDropIndexConstraintString(string constraintName)
    {
      return " drop index " + constraintName;
    }

    public override bool SupportsTemporaryTables => true;

    public override string CreateTemporaryTableString => "create temporary table if not exists";

    protected virtual void RegisterCastTypes()
    {
      this.RegisterCastType(DbType.AnsiString, "CHAR");
      this.RegisterCastType(DbType.AnsiStringFixedLength, "CHAR");
      this.RegisterCastType(DbType.String, "CHAR");
      this.RegisterCastType(DbType.StringFixedLength, "CHAR");
      this.RegisterCastType(DbType.Binary, "BINARY");
      this.RegisterCastType(DbType.Int16, "SIGNED");
      this.RegisterCastType(DbType.Int32, "SIGNED");
      this.RegisterCastType(DbType.Int64, "SIGNED");
      this.RegisterCastType(DbType.UInt16, "UNSIGNED");
      this.RegisterCastType(DbType.UInt32, "UNSIGNED");
      this.RegisterCastType(DbType.UInt64, "UNSIGNED");
      this.RegisterCastType(DbType.Guid, "CHAR(40)");
      this.RegisterCastType(DbType.Time, "TIME");
      this.RegisterCastType(DbType.Date, "DATE");
      this.RegisterCastType(DbType.DateTime, "DATETIME");
    }

    protected void RegisterCastType(DbType code, string name) => this.castTypeNames.Put(code, name);

    protected void RegisterCastType(DbType code, int capacity, string name)
    {
      this.castTypeNames.Put(code, capacity, name);
    }

    public override string GetCastTypeName(SqlType sqlType)
    {
      return this.castTypeNames.Get(sqlType.DbType, (int) byte.MaxValue, 19, 2) ?? throw new HibernateException(string.Format("No CAST() type mapping for SqlType {0}", (object) sqlType));
    }
  }
}
