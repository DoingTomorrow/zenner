// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ConnectionEventArgs
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  public class ConnectionEventArgs : System.EventArgs
  {
    public readonly SQLiteConnectionEventType EventType;
    public readonly StateChangeEventArgs EventArgs;
    public readonly IDbTransaction Transaction;
    public readonly IDbCommand Command;
    public readonly IDataReader DataReader;
    public readonly CriticalHandle CriticalHandle;
    public readonly string Text;
    public readonly object Data;

    internal ConnectionEventArgs(
      SQLiteConnectionEventType eventType,
      StateChangeEventArgs eventArgs,
      IDbTransaction transaction,
      IDbCommand command,
      IDataReader dataReader,
      CriticalHandle criticalHandle,
      string text,
      object data)
    {
      this.EventType = eventType;
      this.EventArgs = eventArgs;
      this.Transaction = transaction;
      this.Command = command;
      this.DataReader = dataReader;
      this.CriticalHandle = criticalHandle;
      this.Text = text;
      this.Data = data;
    }
  }
}
