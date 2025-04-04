// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.SQLite20Driver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Driver
{
  public class SQLite20Driver : ReflectionBasedDriver
  {
    public SQLite20Driver()
      : base("System.Data.SQLite", "System.Data.SQLite", "System.Data.SQLite.SQLiteConnection", "System.Data.SQLite.SQLiteCommand")
    {
    }

    public override IDbConnection CreateConnection()
    {
      DbConnection connection = (DbConnection) base.CreateConnection();
      connection.StateChange += new StateChangeEventHandler(SQLite20Driver.Connection_StateChange);
      return (IDbConnection) connection;
    }

    private static void Connection_StateChange(object sender, StateChangeEventArgs e)
    {
      if (e.OriginalState != ConnectionState.Broken && e.OriginalState != ConnectionState.Closed && e.OriginalState != ConnectionState.Connecting || e.CurrentState != ConnectionState.Open)
        return;
      using (DbCommand command = ((DbConnection) sender).CreateCommand())
      {
        command.CommandText = "PRAGMA foreign_keys = ON";
        command.ExecuteNonQuery();
      }
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => "@";

    public override bool SupportsMultipleOpenReaders => false;

    public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
    {
      return (IResultSetsCommand) new BasicResultSetsCommand(session);
    }

    public override bool SupportsMultipleQueries => true;
  }
}
