// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteConnectionString
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

#nullable disable
namespace SQLite
{
  internal class SQLiteConnectionString
  {
    public string ConnectionString { get; private set; }

    public string DatabasePath { get; private set; }

    public bool StoreDateTimeAsTicks { get; private set; }

    public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks)
    {
      this.ConnectionString = databasePath;
      this.StoreDateTimeAsTicks = storeDateTimeAsTicks;
      this.DatabasePath = databasePath;
    }
  }
}
