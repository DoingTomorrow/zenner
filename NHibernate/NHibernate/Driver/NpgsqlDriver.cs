// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.NpgsqlDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public class NpgsqlDriver : ReflectionBasedDriver
  {
    public NpgsqlDriver()
      : base("Npgsql", "Npgsql", "Npgsql.NpgsqlConnection", "Npgsql.NpgsqlCommand")
    {
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => ":";

    public override bool SupportsMultipleOpenReaders => false;

    protected override bool SupportsPreparingCommands => true;

    public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
    {
      return (IResultSetsCommand) new BasicResultSetsCommand(session);
    }

    public override bool SupportsMultipleQueries => true;

    protected override void InitializeParameter(
      IDbDataParameter dbParam,
      string name,
      SqlType sqlType)
    {
      base.InitializeParameter(dbParam, name, sqlType);
      if (sqlType.DbType != DbType.Currency)
        return;
      dbParam.DbType = DbType.Decimal;
    }
  }
}
