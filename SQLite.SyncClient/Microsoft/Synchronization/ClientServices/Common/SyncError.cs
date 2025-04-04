// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.Common.SyncError
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

#nullable disable
namespace Microsoft.Synchronization.ClientServices.Common
{
  public class SyncError : Conflict
  {
    public IOfflineEntity ErrorEntity { get; set; }

    public string Description { get; internal set; }
  }
}
