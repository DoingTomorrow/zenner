// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.SyncReader
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal abstract class SyncReader : IDisposable
  {
    protected EntryInfoWrapper currentEntryWrapper;
    protected bool currentNodeRead = false;
    protected ReaderItemType currentType;
    protected Stream inputStream;
    protected Type[] knownTypes;
    protected IOfflineEntity liveEntity;
    protected XmlReader reader;

    protected SyncReader(Stream stream, Type[] types)
    {
      this.inputStream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
      this.knownTypes = types;
    }

    public abstract ReaderItemType ItemType { get; }

    public void Dispose()
    {
      if (this.inputStream != null)
      {
        using (this.inputStream)
          this.inputStream.Dispose();
      }
      this.inputStream = (Stream) null;
      this.knownTypes = (Type[]) null;
    }

    public abstract void Start();

    public abstract IOfflineEntity GetItem();

    public abstract byte[] GetServerBlob();

    public abstract bool GetHasMoreChangesValue();

    public abstract bool Next();

    public virtual bool HasConflict()
    {
      return this.currentEntryWrapper != null && this.currentEntryWrapper.ConflictWrapper != null;
    }

    public virtual bool HasConflictTempId()
    {
      return this.currentEntryWrapper != null && this.currentEntryWrapper.ConflictWrapper != null && this.currentEntryWrapper.ConflictWrapper.TempId != null;
    }

    public virtual bool HasTempId()
    {
      return this.currentEntryWrapper != null && this.currentEntryWrapper.TempId != null;
    }

    public virtual string GetTempId()
    {
      return !this.HasTempId() ? (string) null : this.currentEntryWrapper.TempId;
    }

    public virtual string GetConflictTempId()
    {
      return !this.HasConflictTempId() ? (string) null : this.currentEntryWrapper.ConflictWrapper.TempId;
    }

    public virtual Conflict GetConflict()
    {
      if (!this.HasConflict())
        return (Conflict) null;
      Conflict conflict;
      if (this.currentEntryWrapper.IsConflict)
      {
        SyncConflict syncConflict = new SyncConflict();
        syncConflict.LiveEntity = this.liveEntity;
        syncConflict.LosingEntity = ReflectionUtility.GetObjectForType(this.currentEntryWrapper.ConflictWrapper, this.knownTypes);
        syncConflict.Resolution = (SyncConflictResolution) Enum.Parse(FormatterConstants.SyncConflictResolutionType, this.currentEntryWrapper.ConflictDesc, true);
        conflict = (Conflict) syncConflict;
      }
      else
      {
        SyncError syncError = new SyncError();
        syncError.LiveEntity = this.liveEntity;
        syncError.ErrorEntity = ReflectionUtility.GetObjectForType(this.currentEntryWrapper.ConflictWrapper, this.knownTypes);
        syncError.Description = this.currentEntryWrapper.ConflictDesc;
        conflict = (Conflict) syncError;
      }
      return conflict;
    }

    protected void CheckItemType(ReaderItemType type)
    {
      if (this.currentType != type)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} is not a valid {1} element.", (object) this.reader.Name, (object) type));
      this.currentNodeRead = true;
    }
  }
}
