// Decompiled with JetBrains decompiler
// Type: SQLite.BaseTableQuery
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

#nullable disable
namespace SQLite
{
  public abstract class BaseTableQuery
  {
    protected class Ordering
    {
      public string ColumnName { get; set; }

      public bool Ascending { get; set; }
    }
  }
}
