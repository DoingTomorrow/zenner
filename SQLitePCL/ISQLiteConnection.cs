// Decompiled with JetBrains decompiler
// Type: SQLitePCL.ISQLiteConnection
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

using System;

#nullable disable
namespace SQLitePCL
{
  public interface ISQLiteConnection : IDisposable
  {
    ISQLiteStatement Prepare(string sql);

    void CreateFunction(string name, int numberOfArguments, Function function, bool deterministic);

    void CreateAggregate(
      string name,
      int numberOfArguments,
      AggregateStep step,
      AggregateFinal final);

    long LastInsertRowId();

    string ErrorMessage();
  }
}
