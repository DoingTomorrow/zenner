// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.ChangeSet
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class ChangeSet
  {
    public byte[] ServerBlob { get; set; }

    public ICollection<IOfflineEntity> Data { get; set; }

    public bool IsLastBatch { get; set; }

    public ChangeSet()
    {
      this.ServerBlob = (byte[]) null;
      this.Data = (ICollection<IOfflineEntity>) new List<IOfflineEntity>();
      this.IsLastBatch = true;
    }

    internal void AddItem(IOfflineEntity iOfflineEntity)
    {
      if (this.Data == null)
        this.Data = (ICollection<IOfflineEntity>) new List<IOfflineEntity>();
      this.Data.Add(iOfflineEntity);
    }
  }
}
