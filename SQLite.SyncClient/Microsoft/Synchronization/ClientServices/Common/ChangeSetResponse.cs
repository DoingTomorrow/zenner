// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.Common.ChangeSetResponse
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.Common
{
  public class ChangeSetResponse
  {
    private List<Conflict> _conflicts;
    private List<IOfflineEntity> _updatedItems;

    public byte[] ServerBlob { get; set; }

    public Exception Error { get; set; }

    public ReadOnlyCollection<Conflict> Conflicts
    {
      get => new ReadOnlyCollection<Conflict>((IList<Conflict>) this._conflicts);
    }

    public ReadOnlyCollection<IOfflineEntity> UpdatedItems
    {
      get => new ReadOnlyCollection<IOfflineEntity>((IList<IOfflineEntity>) this._updatedItems);
    }

    internal ChangeSetResponse()
    {
      this._conflicts = new List<Conflict>();
      this._updatedItems = new List<IOfflineEntity>();
    }

    internal void AddConflict(Conflict conflict) => this._conflicts.Add(conflict);

    internal void AddUpdatedItem(IOfflineEntity item) => this._updatedItems.Add(item);

    internal List<Conflict> ConflictsInternal => this._conflicts;
  }
}
