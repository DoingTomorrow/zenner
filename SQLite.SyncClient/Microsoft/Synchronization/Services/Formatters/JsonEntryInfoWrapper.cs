// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.JsonEntryInfoWrapper
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Xml.Linq;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal class JsonEntryInfoWrapper(XElement reader) : EntryInfoWrapper(reader)
  {
    protected override void LoadConflictEntry(XElement entry)
    {
      XElement xelement1 = entry.Element((XName) "__syncConflict");
      if (xelement1 != null)
      {
        this.IsConflict = true;
        this.ConflictDesc = (xelement1.Element((XName) "conflictResolution") ?? throw new InvalidOperationException("Conflict resolution not specified for Json object " + this.TypeName)).Value;
        this.ConflictWrapper = (EntryInfoWrapper) new JsonEntryInfoWrapper(xelement1.Element((XName) "conflictingChange") ?? throw new InvalidOperationException("conflictingChange not specified for Json syncConflict object " + this.TypeName));
      }
      else
      {
        XElement xelement2 = entry.Element((XName) "__syncError");
        if (xelement2 == null)
          return;
        this.IsConflict = false;
        XElement xelement3 = xelement2.Element((XName) "errorDescription");
        if (xelement3 != null)
          this.ConflictDesc = xelement3.Value;
        this.ConflictWrapper = (EntryInfoWrapper) new JsonEntryInfoWrapper(xelement2.Element((XName) "changeInError") ?? throw new InvalidOperationException("errorInChange not specified for Json syncError object " + this.TypeName));
      }
    }

    protected override void LoadEntryProperties(XElement entry)
    {
      foreach (XElement element in entry.Elements())
      {
        if (!element.Name.LocalName.Equals("__metadata", StringComparison.CurrentCultureIgnoreCase) && !element.Name.LocalName.Equals("isDeleted", StringComparison.CurrentCultureIgnoreCase))
        {
          XAttribute xattribute = element.Attribute((XName) "type");
          if (xattribute != null && xattribute.Value.Equals("null", StringComparison.OrdinalIgnoreCase))
            this.PropertyBag[element.Name.LocalName] = (string) null;
          else
            this.PropertyBag[element.Name.LocalName] = element.Value;
        }
      }
    }

    protected override void LoadTypeName(XElement entry)
    {
      foreach (XElement element in entry.Elements((XName) "__metadata"))
      {
        this.TypeName = element.Element((XName) "type").Value;
        if (element.Element((XName) "uri") != null)
        {
          this.Id = element.Element((XName) "uri").Value;
          this.EditUri = new Uri(this.Id, UriKind.RelativeOrAbsolute);
        }
        if (element.Element((XName) "tempId") != null)
          this.TempId = element.Element((XName) "tempId").Value;
        if (string.IsNullOrEmpty(this.Id) && this.TempId == null)
          throw new InvalidOperationException("A uri or a tempId key must be present in the __metadata object. Entity in error: " + entry.ToString(SaveOptions.None));
        if (element.Element((XName) "etag") != null)
          this.ETag = element.Element((XName) "etag").Value;
        if (element.Element((XName) "edituri") != null)
          this.EditUri = new Uri(element.Element((XName) "edituri").Value, UriKind.RelativeOrAbsolute);
        if (element.Element((XName) "isDeleted") != null)
          this.IsTombstone = bool.Parse(element.Element((XName) "isDeleted").Value);
      }
      if (string.IsNullOrEmpty(this.TypeName))
        throw new InvalidOperationException("Json object does not have a _metadata tag containing the type information.");
    }
  }
}
