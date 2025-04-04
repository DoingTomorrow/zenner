// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.BasicResultSetsCommand
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Driver
{
  public class BasicResultSetsCommand : IResultSetsCommand
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (BasicResultSetsCommand));
    private SqlString sqlString = SqlString.Empty;

    public BasicResultSetsCommand(ISessionImplementor session)
    {
      this.Commands = new List<ISqlCommand>();
      this.Session = session;
    }

    protected List<ISqlCommand> Commands { get; private set; }

    protected ISessionImplementor Session { get; private set; }

    public virtual void Append(ISqlCommand command)
    {
      this.Commands.Add(command);
      this.sqlString = this.sqlString.Append(command.Query).Append(";").Append(Environment.NewLine);
    }

    public bool HasQueries => this.Commands.Count > 0;

    public virtual SqlString Sql => this.sqlString;

    public virtual IDataReader GetReader(int? commandTimeout)
    {
      IBatcher batcher = this.Session.Batcher;
      SqlType[] array = this.Commands.SelectMany<ISqlCommand, SqlType>((System.Func<ISqlCommand, IEnumerable<SqlType>>) (c => (IEnumerable<SqlType>) c.ParameterTypes)).ToArray<SqlType>();
      this.ForEachSqlCommand((Action<ISqlCommand, int>) ((sqlLoaderCommand, offset) => sqlLoaderCommand.ResetParametersIndexesForTheCommand(offset)));
      IDbCommand command = batcher.PrepareQueryCommand(CommandType.Text, this.sqlString, array);
      if (commandTimeout.HasValue)
        command.CommandTimeout = commandTimeout.Value;
      BasicResultSetsCommand.log.Info((object) command.CommandText);
      this.BindParameters(command);
      return (IDataReader) new BatcherDataReaderWrapper(batcher, command);
    }

    protected virtual void BindParameters(IDbCommand command)
    {
      List<Parameter> wholeQueryParametersList = this.Sql.GetParameters().ToList<Parameter>();
      this.ForEachSqlCommand((Action<ISqlCommand, int>) ((sqlLoaderCommand, offset) => sqlLoaderCommand.Bind(command, (IList<Parameter>) wholeQueryParametersList, offset, this.Session)));
    }

    protected void ForEachSqlCommand(Action<ISqlCommand, int> actionToDo)
    {
      int num = 0;
      foreach (ISqlCommand command in this.Commands)
      {
        actionToDo(command, num);
        num += command.ParameterTypes.Length;
      }
    }
  }
}
