// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SyncProgressEvent
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class SyncProgressEvent
  {
    public TimeSpan Duration { get; private set; }

    public SyncStage SyncStage { get; private set; }

    public ICollection<IOfflineEntity> Changes { get; private set; }

    public ICollection<Conflict> Conflicts { get; private set; }

    public ICollection<IOfflineEntity> UpdatedItemsAfterInsertOnServer { get; private set; }

    public bool IsLastBatch { get; set; }

    public Type CreatedTable { get; set; }

    public SyncProgressEvent(
      SyncStage stage,
      TimeSpan duration,
      bool isLastBatch = true,
      ICollection<IOfflineEntity> changes = null,
      ICollection<Conflict> conflicts = null,
      ICollection<IOfflineEntity> updatedItems = null)
    {
      this.SyncStage = stage;
      this.Duration = duration;
      this.IsLastBatch = this.IsLastBatch;
      this.Changes = changes;
      this.Conflicts = conflicts;
      this.UpdatedItemsAfterInsertOnServer = updatedItems;
    }
  }
}
