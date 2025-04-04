// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.IDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public interface IDriver
  {
    void Configure(IDictionary<string, string> settings);

    IDbConnection CreateConnection();

    bool SupportsMultipleOpenReaders { get; }

    IDbCommand GenerateCommand(CommandType type, SqlString sqlString, SqlType[] parameterTypes);

    void PrepareCommand(IDbCommand command);

    IDbDataParameter GenerateParameter(IDbCommand command, string name, SqlType sqlType);

    void RemoveUnusedCommandParameters(IDbCommand cmd, SqlString sqlString);

    void ExpandQueryParameters(IDbCommand cmd, SqlString sqlString);

    IResultSetsCommand GetResultSetsCommand(ISessionImplementor session);

    bool SupportsMultipleQueries { get; }

    void AdjustCommand(IDbCommand command);
  }
}
