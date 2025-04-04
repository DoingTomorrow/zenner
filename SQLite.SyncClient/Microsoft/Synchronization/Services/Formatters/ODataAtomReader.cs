// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.ODataAtomReader
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal class ODataAtomReader : SyncReader
  {
    public ODataAtomReader(Stream stream)
      : this(stream, (Type[]) null)
    {
    }

    public ODataAtomReader(Stream stream, Type[] knownTypes)
      : base(stream, knownTypes)
    {
      this.reader = XmlReader.Create(stream);
    }

    public override void Start()
    {
      int content = (int) this.reader.MoveToContent();
      if (!AtomHelper.IsAtomElement(this.reader, "feed"))
        throw new InvalidOperationException("Not a valid ATOM feed.");
    }

    public override ReaderItemType ItemType => this.currentType;

    public override IOfflineEntity GetItem()
    {
      this.CheckItemType(ReaderItemType.Entry);
      this.currentEntryWrapper = (EntryInfoWrapper) new AtomEntryInfoWrapper((XElement) XNode.ReadFrom(this.reader));
      this.liveEntity = ReflectionUtility.GetObjectForType(this.currentEntryWrapper, this.knownTypes);
      return this.liveEntity;
    }

    public override bool GetHasMoreChangesValue()
    {
      this.CheckItemType(ReaderItemType.HasMoreChanges);
      return (bool) this.reader.ReadElementContentAs(FormatterConstants.BoolType, (IXmlNamespaceResolver) null);
    }

    public override byte[] GetServerBlob()
    {
      this.CheckItemType(ReaderItemType.SyncBlob);
      return Convert.FromBase64String((string) this.reader.ReadElementContentAs(FormatterConstants.StringType, (IXmlNamespaceResolver) null));
    }

    public override bool Next()
    {
      if (this.currentType != ReaderItemType.BOF && !this.currentNodeRead)
        this.reader.Skip();
      do
      {
        this.currentEntryWrapper = (EntryInfoWrapper) null;
        this.liveEntity = (IOfflineEntity) null;
        if (AtomHelper.IsAtomElement(this.reader, "entry") || AtomHelper.IsAtomTombstone(this.reader, "deleted-entry"))
        {
          this.currentType = ReaderItemType.Entry;
          this.currentNodeRead = false;
          return true;
        }
        if (AtomHelper.IsSyncElement(this.reader, "serverBlob"))
        {
          this.currentType = ReaderItemType.SyncBlob;
          this.currentNodeRead = false;
          return true;
        }
        if (AtomHelper.IsSyncElement(this.reader, "moreChangesAvailable"))
        {
          this.currentType = ReaderItemType.HasMoreChanges;
          this.currentNodeRead = false;
          return true;
        }
      }
      while (this.reader.Read());
      this.currentType = ReaderItemType.EOF;
      return false;
    }
  }
}
