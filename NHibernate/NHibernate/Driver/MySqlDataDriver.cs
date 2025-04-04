// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.MySqlDataDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;

#nullable disable
namespace NHibernate.Driver
{
  public class MySqlDataDriver : ReflectionBasedDriver
  {
    public MySqlDataDriver()
      : base("MySql.Data.MySqlClient", "MySql.Data", "MySql.Data.MySqlClient.MySqlConnection", "MySql.Data.MySqlClient.MySqlCommand")
    {
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => "?";

    public override bool SupportsMultipleOpenReaders => false;

    protected override bool SupportsPreparingCommands => false;

    public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
    {
      return (IResultSetsCommand) new BasicResultSetsCommand(session);
    }

    public override bool SupportsMultipleQueries => true;
  }
}
