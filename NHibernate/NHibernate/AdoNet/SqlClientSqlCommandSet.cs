// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.SqlClientSqlCommandSet
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data.SqlClient;
using System.Reflection;

#nullable disable
namespace NHibernate.AdoNet
{
  public class SqlClientSqlCommandSet : IDisposable
  {
    private static Type sqlCmdSetType = Assembly.Load("System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089").GetType("System.Data.SqlClient.SqlCommandSet");
    private object instance;
    private SqlClientSqlCommandSet.PropSetter<SqlConnection> connectionSetter;
    private SqlClientSqlCommandSet.PropSetter<SqlTransaction> transactionSetter;
    private SqlClientSqlCommandSet.PropSetter<int> commandTimeoutSetter;
    private SqlClientSqlCommandSet.PropGetter<SqlConnection> connectionGetter;
    private SqlClientSqlCommandSet.PropGetter<SqlCommand> commandGetter;
    private SqlClientSqlCommandSet.AppendCommand doAppend;
    private SqlClientSqlCommandSet.ExecuteNonQueryCommand doExecuteNonQuery;
    private SqlClientSqlCommandSet.DisposeCommand doDispose;
    private int countOfCommands;

    public SqlClientSqlCommandSet()
    {
      this.instance = Activator.CreateInstance(SqlClientSqlCommandSet.sqlCmdSetType, true);
      this.connectionSetter = (SqlClientSqlCommandSet.PropSetter<SqlConnection>) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.PropSetter<SqlConnection>), this.instance, "set_Connection");
      this.transactionSetter = (SqlClientSqlCommandSet.PropSetter<SqlTransaction>) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.PropSetter<SqlTransaction>), this.instance, "set_Transaction");
      this.commandTimeoutSetter = (SqlClientSqlCommandSet.PropSetter<int>) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.PropSetter<int>), this.instance, "set_CommandTimeout");
      this.connectionGetter = (SqlClientSqlCommandSet.PropGetter<SqlConnection>) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.PropGetter<SqlConnection>), this.instance, "get_Connection");
      this.commandGetter = (SqlClientSqlCommandSet.PropGetter<SqlCommand>) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.PropGetter<SqlCommand>), this.instance, "get_BatchCommand");
      this.doAppend = (SqlClientSqlCommandSet.AppendCommand) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.AppendCommand), this.instance, "Append");
      this.doExecuteNonQuery = (SqlClientSqlCommandSet.ExecuteNonQueryCommand) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.ExecuteNonQueryCommand), this.instance, "ExecuteNonQuery");
      this.doDispose = (SqlClientSqlCommandSet.DisposeCommand) Delegate.CreateDelegate(typeof (SqlClientSqlCommandSet.DisposeCommand), this.instance, "Dispose");
    }

    public void Append(SqlCommand command)
    {
      SqlClientSqlCommandSet.AssertHasParameters(command);
      this.doAppend(command);
      ++this.countOfCommands;
    }

    private static void AssertHasParameters(SqlCommand command)
    {
      if (command.Parameters.Count == 0)
        throw new ArgumentException("A command in SqlCommandSet must have parameters. You can't pass hardcoded sql strings.");
    }

    public SqlCommand BatchCommand => this.commandGetter();

    public int CountOfCommands => this.countOfCommands;

    public int ExecuteNonQuery()
    {
      if (this.Connection == null)
        throw new ArgumentNullException("Connection was not set! You must set the connection property before calling ExecuteNonQuery()");
      return this.CountOfCommands == 0 ? 0 : this.doExecuteNonQuery();
    }

    public SqlConnection Connection
    {
      get => this.connectionGetter();
      set => this.connectionSetter(value);
    }

    public SqlTransaction Transaction
    {
      set => this.transactionSetter(value);
    }

    public int CommandTimeout
    {
      set => this.commandTimeoutSetter(value);
    }

    public void Dispose() => this.doDispose();

    private delegate void PropSetter<T>(T item);

    private delegate T PropGetter<T>();

    private delegate void AppendCommand(SqlCommand command);

    private delegate int ExecuteNonQueryCommand();

    private delegate void DisposeCommand();
  }
}
