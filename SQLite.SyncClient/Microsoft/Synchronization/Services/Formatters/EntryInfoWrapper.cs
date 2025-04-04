// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.EntryInfoWrapper
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal abstract class EntryInfoWrapper
  {
    public string TypeName;
    public Dictionary<string, string> PropertyBag = new Dictionary<string, string>();
    public bool IsTombstone;
    public string ConflictDesc;
    public EntryInfoWrapper ConflictWrapper;
    public bool IsConflict;
    public string ETag;
    public string TempId;
    public Uri EditUri;
    public string Id;

    protected abstract void LoadConflictEntry(XElement entry);

    protected abstract void LoadEntryProperties(XElement entry);

    protected abstract void LoadTypeName(XElement entry);

    protected EntryInfoWrapper(XElement reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      this.PropertyBag = new Dictionary<string, string>();
      this.LoadTypeName(reader);
      this.LoadEntryProperties(reader);
      this.LoadConflictEntry(reader);
    }
  }
}
