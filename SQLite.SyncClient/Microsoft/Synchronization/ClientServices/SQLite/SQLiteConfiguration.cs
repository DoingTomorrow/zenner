// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.SQLiteConfiguration
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  public class SQLiteConfiguration
  {
    public string ScopeName { get; set; }

    public Uri ServiceUri { get; set; }

    public List<string> Types { get; set; }

    public byte[] AnchorBlob { get; set; }

    public DateTime LastSyncDate { get; set; }
  }
}
