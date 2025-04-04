// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.CacheRequestResult
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class CacheRequestResult
  {
    public Guid Id { get; set; }

    public ChangeSet ChangeSet { get; set; }

    public ChangeSetResponse ChangeSetResponse { get; set; }

    public Exception Error { get; set; }

    public object State { get; set; }

    public HttpState HttpStep { get; set; }

    public uint BatchUploadCount { get; set; }

    public CacheRequestResult(
      Guid id,
      ChangeSetResponse response,
      int uploadCount,
      Exception error,
      HttpState step,
      object state)
    {
      this.ChangeSetResponse = response;
      this.Error = error;
      this.State = state;
      this.HttpStep = step;
      this.Id = id;
      this.BatchUploadCount = (uint) uploadCount;
      if (this.Error == null)
        return;
      if (this.ChangeSetResponse == null)
        this.ChangeSetResponse = new ChangeSetResponse();
      this.ChangeSetResponse.Error = this.Error;
    }

    public CacheRequestResult(
      Guid id,
      ChangeSet changeSet,
      Exception error,
      HttpState step,
      object state)
    {
      this.ChangeSet = changeSet;
      this.Error = error;
      this.State = state;
      this.Id = id;
      this.HttpStep = step;
    }
  }
}
