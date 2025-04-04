// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteException
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace SQLite
{
  public class SQLiteException : Exception
  {
    public SQLite3.Result Result { get; private set; }

    protected SQLiteException(SQLite3.Result r, string message)
      : base(message)
    {
      this.Result = r;
    }

    public static SQLiteException New(SQLite3.Result r, string message)
    {
      return new SQLiteException(r, message);
    }
  }
}
