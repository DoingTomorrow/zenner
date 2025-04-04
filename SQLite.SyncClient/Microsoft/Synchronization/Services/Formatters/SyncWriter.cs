// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.SyncWriter
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal abstract class SyncWriter
  {
    private Uri _baseUri;

    protected Uri BaseUri => this._baseUri;

    protected SyncWriter(Uri serviceUri)
    {
      this._baseUri = !(serviceUri == (Uri) null) ? serviceUri : throw new ArgumentNullException(nameof (serviceUri));
    }

    public virtual void StartFeed(bool isLastBatch, byte[] serverBlob)
    {
    }

    public virtual void AddItem(IOfflineEntity entry, string tempId)
    {
      this.AddItem(entry, tempId, false);
    }

    public virtual void AddItem(IOfflineEntity entry, string tempId, bool emitMetadataOnly)
    {
      if (entry == null)
        throw new ArgumentNullException(nameof (entry));
      if (string.IsNullOrEmpty(entry.GetServiceMetadata().Id) && entry.GetServiceMetadata().IsTombstone)
        return;
      this.WriteItemInternal(entry, tempId, (IOfflineEntity) null, (string) null, (string) null, false, emitMetadataOnly);
    }

    public abstract void WriteItemInternal(
      IOfflineEntity live,
      string liveTempId,
      IOfflineEntity conflicting,
      string conflictingTempId,
      string desc,
      bool isConflict,
      bool emitMetadataOnly);

    public abstract void WriteFeed(XmlWriter writer);
  }
}
