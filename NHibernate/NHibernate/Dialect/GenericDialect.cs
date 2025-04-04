// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.GenericDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class GenericDialect : NHibernate.Dialect.Dialect
  {
    public GenericDialect()
    {
      this.RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR($l)");
      this.RegisterColumnType(DbType.AnsiString, "VARCHAR($l)");
      this.RegisterColumnType(DbType.Binary, "VARBINARY($l)");
      this.RegisterColumnType(DbType.Boolean, "BIT");
      this.RegisterColumnType(DbType.Byte, "TINYINT");
      this.RegisterColumnType(DbType.Currency, "MONEY");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "DATETIME");
      this.RegisterColumnType(DbType.Decimal, "DECIMAL(19, $l)");
      this.RegisterColumnType(DbType.Double, "DOUBLE PRECISION");
      this.RegisterColumnType(DbType.Guid, "UNIQUEIDENTIFIER");
      this.RegisterColumnType(DbType.Int16, "SMALLINT");
      this.RegisterColumnType(DbType.Int32, "INT");
      this.RegisterColumnType(DbType.Int64, "BIGINT");
      this.RegisterColumnType(DbType.Single, "REAL");
      this.RegisterColumnType(DbType.StringFixedLength, "NCHAR($l)");
      this.RegisterColumnType(DbType.String, "NVARCHAR($l)");
      this.RegisterColumnType(DbType.Time, "TIME");
    }

    public override string AddColumnString => "add column";
  }
}
