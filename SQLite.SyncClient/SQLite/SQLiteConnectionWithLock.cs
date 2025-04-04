// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteConnectionWithLock
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Threading;

#nullable disable
namespace SQLite
{
  internal class SQLiteConnectionWithLock(
    SQLiteConnectionString connectionString,
    SQLiteOpenFlags openFlags) : SQLiteConnection(connectionString.DatabasePath, openFlags, connectionString.StoreDateTimeAsTicks)
  {
    private readonly object _lockPoint = new object();

    public IDisposable Lock()
    {
      return (IDisposable) new SQLiteConnectionWithLock.LockWrapper(this._lockPoint);
    }

    private class LockWrapper : IDisposable
    {
      private object _lockPoint;

      public LockWrapper(object lockPoint)
      {
        this._lockPoint = lockPoint;
        Monitor.Enter(this._lockPoint);
      }

      public void Dispose() => Monitor.Exit(this._lockPoint);
    }
  }
}
