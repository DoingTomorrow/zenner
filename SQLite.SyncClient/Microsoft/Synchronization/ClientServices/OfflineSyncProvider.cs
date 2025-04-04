// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.OfflineSyncProvider
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Threading.Tasks;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public abstract class OfflineSyncProvider
  {
    public abstract Task BeginSession();

    public abstract Task<ChangeSet> GetChangeSet(Guid state);

    public abstract Task OnChangeSetUploaded(Guid state, ChangeSetResponse response);

    public abstract byte[] GetServerBlob();

    public abstract Task SaveChangeSet(ChangeSet changeSet);

    public abstract void EndSession();
  }
}
