// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.CacheRefreshStatistics
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class CacheRefreshStatistics
  {
    public Exception Error { get; set; }

    public bool Cancelled { get; set; }

    public DateTime StartTime { get; internal set; }

    public DateTime EndTime { get; internal set; }

    public uint TotalChangeSetsDownloaded { get; internal set; }

    public uint TotalChangeSetsUploaded { get; internal set; }

    public uint TotalUploads { get; internal set; }

    public uint TotalDownloads { get; internal set; }

    public uint TotalSyncConflicts { get; internal set; }

    public uint TotalSyncErrors { get; internal set; }
  }
}
