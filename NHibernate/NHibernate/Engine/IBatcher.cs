// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.IBatcher
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Engine
{
  public interface IBatcher : IDisposable
  {
    IDbCommand PrepareQueryCommand(
      CommandType commandType,
      SqlString sql,
      SqlType[] parameterTypes);

    IDbCommand PrepareCommand(CommandType commandType, SqlString sql, SqlType[] parameterTypes);

    void CloseCommand(IDbCommand cmd, IDataReader reader);

    void CloseReader(IDataReader reader);

    IDbCommand PrepareBatchCommand(
      CommandType commandType,
      SqlString sql,
      SqlType[] parameterTypes);

    void AddToBatch(IExpectation expectation);

    void ExecuteBatch();

    void CloseCommands();

    IDataReader ExecuteReader(IDbCommand cmd);

    int ExecuteNonQuery(IDbCommand cmd);

    void AbortBatch(Exception e);

    void CancelLastQuery();

    bool HasOpenResources { get; }

    int BatchSize { get; set; }
  }
}
