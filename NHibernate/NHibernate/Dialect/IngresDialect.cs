// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.IngresDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class IngresDialect : NHibernate.Dialect.Dialect
  {
    public IngresDialect()
    {
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
      this.RegisterColumnType(DbType.Decimal, 19, "decimal(18, $l)");
      this.RegisterColumnType(DbType.Double, "float8");
      this.RegisterColumnType(DbType.Int16, "int2");
      this.RegisterColumnType(DbType.Int32, "int4");
      this.RegisterColumnType(DbType.Int64, "int8");
      this.RegisterColumnType(DbType.Single, "float4");
      this.RegisterColumnType(DbType.StringFixedLength, "char(255)");
      this.RegisterColumnType(DbType.StringFixedLength, 4000, "char($l)");
      this.RegisterColumnType(DbType.String, "varchar(255)");
      this.RegisterColumnType(DbType.String, 4000, "varchar($l)");
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.IngresDriver";
    }
  }
}
