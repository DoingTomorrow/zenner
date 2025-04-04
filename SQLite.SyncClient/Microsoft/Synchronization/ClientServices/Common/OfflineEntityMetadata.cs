// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.Common.OfflineEntityMetadata
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.Common
{
  public class OfflineEntityMetadata : INotifyPropertyChanged
  {
    private Uri _editUri;
    private string _etag;
    private string _id;
    private bool _isTombstone;

    public OfflineEntityMetadata()
    {
      this._isTombstone = false;
      this._id = (string) null;
      this._etag = (string) null;
      this._editUri = (Uri) null;
    }

    public OfflineEntityMetadata(bool isTombstone, string id, string etag, Uri editUri)
    {
      this._isTombstone = isTombstone;
      this._id = id;
      this._etag = etag;
      this._editUri = editUri;
    }

    public bool IsTombstone
    {
      get => this._isTombstone;
      set
      {
        if (value == this._isTombstone)
          return;
        this._isTombstone = value;
        this.RaisePropertyChanged(nameof (IsTombstone));
      }
    }

    public string Id
    {
      get => this._id;
      set
      {
        if (!(value != this._id))
          return;
        this._id = value;
        this.RaisePropertyChanged(nameof (Id));
      }
    }

    public string ETag
    {
      get => this._etag;
      set
      {
        if (!(value != this._etag))
          return;
        this._etag = value;
        this.RaisePropertyChanged(nameof (ETag));
      }
    }

    public Uri EditUri
    {
      get => this._editUri;
      set
      {
        if (!(value != this._editUri))
          return;
        this._editUri = value;
        this.RaisePropertyChanged(nameof (EditUri));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    internal OfflineEntityMetadata Clone()
    {
      return new OfflineEntityMetadata(this._isTombstone, this._id, this._etag, this._editUri);
    }
  }
}
