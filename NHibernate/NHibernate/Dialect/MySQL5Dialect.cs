// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MySQL5Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class MySQL5Dialect : MySQLDialect
  {
    public MySQL5Dialect()
    {
      this.RegisterColumnType(DbType.Decimal, "DECIMAL(19,5)");
      this.RegisterColumnType(DbType.Decimal, 19, "DECIMAL($p, $s)");
      this.RegisterColumnType(DbType.Guid, "BINARY(16)");
    }

    protected override void RegisterCastTypes()
    {
      base.RegisterCastTypes();
      this.RegisterCastType(DbType.Decimal, "DECIMAL(19,5)");
      this.RegisterCastType(DbType.Decimal, 19, "DECIMAL($p, $s)");
      this.RegisterCastType(DbType.Double, "DECIMAL(19,5)");
      this.RegisterCastType(DbType.Single, "DECIMAL(19,5)");
      this.RegisterCastType(DbType.Guid, "BINARY(16)");
    }

    public override bool SupportsSubSelects => true;

    public override string SelectGUIDString => "select uuid()";

    public override SqlString AppendIdentitySelectToInsert(SqlString insertString)
    {
      return insertString.Append(";" + this.IdentitySelectString);
    }

    public override bool SupportsInsertSelectIdentity => true;
  }
}
