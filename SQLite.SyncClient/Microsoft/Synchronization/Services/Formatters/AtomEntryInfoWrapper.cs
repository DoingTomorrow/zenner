﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.AtomEntryInfoWrapper
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Globalization;
using System.Xml.Linq;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal class AtomEntryInfoWrapper(XElement reader) : EntryInfoWrapper(reader)
  {
    protected override void LoadConflictEntry(XElement entry)
    {
      XElement xelement1 = entry.Element(FormatterConstants.SyncNamespace + "syncConflict");
      if (xelement1 != null)
      {
        this.IsConflict = true;
        this.ConflictDesc = (xelement1.Element(FormatterConstants.SyncNamespace + "conflictResolution") ?? throw new InvalidOperationException("Conflict resolution not specified for entry element " + this.TypeName)).Value;
        this.ConflictWrapper = (EntryInfoWrapper) new AtomEntryInfoWrapper(this.GetSubElement(xelement1.Element(FormatterConstants.SyncNamespace + "conflictingChange") ?? throw new InvalidOperationException("conflictingChange not specified for syncConflict element " + this.TypeName)));
      }
      else
      {
        XElement xelement2 = entry.Element(FormatterConstants.SyncNamespace + "syncError");
        if (xelement2 == null)
          return;
        this.IsConflict = false;
        XElement xelement3 = xelement2.Element(FormatterConstants.SyncNamespace + "errorDescription");
        if (xelement3 != null)
          this.ConflictDesc = xelement3.Value;
        this.ConflictWrapper = (EntryInfoWrapper) new AtomEntryInfoWrapper(this.GetSubElement(xelement2.Element(FormatterConstants.SyncNamespace + "changeInError") ?? throw new InvalidOperationException("errorInChange not specified for syncError element " + this.TypeName)));
      }
    }

    private XElement GetSubElement(XElement entryElement)
    {
      return entryElement.Element(FormatterConstants.AtomNamespaceUri + "entry") ?? entryElement.Element(FormatterConstants.AtomDeletedEntryNamespace + "deleted-entry");
    }

    protected override void LoadEntryProperties(XElement entry)
    {
      if (entry.Name.Namespace.Equals((object) FormatterConstants.AtomDeletedEntryNamespace))
      {
        this.IsTombstone = true;
        XElement xelement = entry.Element(FormatterConstants.AtomXmlNamespace + "ref");
        if (xelement != null)
          this.Id = xelement.Value;
        if (string.IsNullOrEmpty(this.Id))
          throw new InvalidOperationException("A atom:ref element must be present for a tombstone entry. Entity in error: " + entry.ToString(SaveOptions.None));
      }
      else
      {
        XElement xelement1 = (XElement) null;
        XAttribute xattribute1 = entry.Attribute(FormatterConstants.ODataMetadataNamespace + "etag");
        if (xattribute1 != null)
          this.ETag = xattribute1.Value;
        XElement xelement2 = entry.Element(FormatterConstants.SyncNamespace + "tempId");
        if (xelement2 != null)
          this.TempId = xelement2.Value;
        XElement xelement3 = entry.Element(FormatterConstants.AtomXmlNamespace + "id");
        if (xelement3 != null)
          this.Id = xelement3.Value;
        if (string.IsNullOrEmpty(this.Id) && this.TempId == null)
          throw new InvalidOperationException("A atom:id or a sync:tempId element must be present. Entity in error: " + entry.ToString(SaveOptions.None));
        foreach (XElement element in entry.Elements(FormatterConstants.AtomXmlNamespace + "link"))
        {
          foreach (XAttribute attribute in element.Attributes((XName) "rel"))
          {
            if (attribute.Value.Equals("edit", StringComparison.CurrentCultureIgnoreCase))
            {
              if (this.EditUri != (Uri) null)
                throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Multiple Edit Url's found for atom with {0}: '{1}'", this.Id == null ? (object) "TempId" : (object) "Id", this.Id == null ? (object) this.TempId : (object) this.Id));
              this.EditUri = new Uri((element.Attribute((XName) "href") ?? throw new InvalidOperationException(string.Format("No href attribute found in the edit link for atom with  {0}: '{1}'", this.Id == null ? (object) "TempId" : (object) "Id", (object) (this.Id ?? this.TempId)))).Value, UriKind.RelativeOrAbsolute);
            }
          }
        }
        XElement xelement4 = entry.Element(FormatterConstants.AtomNamespaceUri + "content");
        if (xelement4 != null)
          xelement1 = xelement4.Element(FormatterConstants.ODataMetadataNamespace + "properties");
        if (xelement1 != null)
        {
          foreach (XElement element in xelement1.Elements())
          {
            if (element.Name.Namespace == FormatterConstants.ODataDataNamespace)
            {
              XAttribute xattribute2 = element.Attribute(FormatterConstants.ODataMetadataNamespace + "null");
              if (xattribute2 != null && xattribute2.Value.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
                this.PropertyBag[element.Name.LocalName] = (string) null;
              else
                this.PropertyBag[element.Name.LocalName] = element.Value;
            }
          }
        }
      }
    }

    protected override void LoadTypeName(XElement entry)
    {
      bool flag = entry.Name.Namespace == FormatterConstants.AtomDeletedEntryNamespace;
      foreach (XElement xelement in flag ? entry.Elements(FormatterConstants.SyncNamespace + "category") : entry.Elements(FormatterConstants.AtomNamespaceUri + "category"))
      {
        if (flag)
          this.TypeName = xelement.Value;
        else
          this.TypeName = xelement.Attribute((XName) "term").Value;
      }
      if (string.IsNullOrEmpty(this.TypeName))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Category element not found in {0} element.", flag ? (object) "deleted-entry" : (object) nameof (entry)));
    }
  }
}
