// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.DotConnectMySqlDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Driver
{
  public class DotConnectMySqlDriver : ReflectionBasedDriver
  {
    public DotConnectMySqlDriver()
      : base("Devart.Data.MySql", "Devart.Data.MySql.NHibernate.NHibernateMySqlConnection", "Devart.Data.MySql.NHibernate.NHibernateMySqlCommand")
    {
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => "@";
  }
}
