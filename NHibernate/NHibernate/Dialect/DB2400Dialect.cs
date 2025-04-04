// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.DB2400Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;

#nullable disable
namespace NHibernate.Dialect
{
  public class DB2400Dialect : DB2Dialect
  {
    public DB2400Dialect()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.DB2400Driver";
    }

    public override bool SupportsSequences => false;

    public override string IdentitySelectString
    {
      get => "select identity_val_local() from sysibm.sysdummy1";
    }

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => false;

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      return new SqlStringBuilder(queryString).Add(" fetch first ").Add(limit).Add(" rows only ").ToSqlString();
    }

    public override bool UseMaxForLimit => true;

    public override bool SupportsVariableLimit => false;
  }
}
