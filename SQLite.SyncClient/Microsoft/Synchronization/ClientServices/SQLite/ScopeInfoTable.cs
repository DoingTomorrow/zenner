// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.ScopeInfoTable
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  internal class ScopeInfoTable
  {
    [PrimaryKey]
    public string ScopeName { get; set; }

    [MaxLength(250)]
    public string ServiceUri { get; set; }

    public DateTime LastSyncDate { get; set; }

    public byte[] AnchorBlob { get; set; }

    [MaxLength(8000)]
    public string Configuration { get; set; }
  }
}
