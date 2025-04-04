// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.ODataAtomWriter
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal class ODataAtomWriter(Uri serviceUri) : SyncWriter(serviceUri)
  {
    private XDocument document;
    private XElement root;

    public override void StartFeed(bool isLastBatch, byte[] serverBlob)
    {
      base.StartFeed(isLastBatch, serverBlob);
      this.document = new XDocument();
      XNamespace xnamespace = (XNamespace) this.BaseUri.ToString();
      XNamespace atomXmlNamespace = FormatterConstants.AtomXmlNamespace;
      this.root = new XElement(FormatterConstants.AtomXmlNamespace + "feed", new object[6]
      {
        (object) new XAttribute(XNamespace.Xmlns + "base", (object) xnamespace),
        (object) new XAttribute((XName) "xmlns", (object) FormatterConstants.AtomXmlNamespace),
        (object) new XAttribute(XNamespace.Xmlns + "d", (object) FormatterConstants.ODataDataNamespace),
        (object) new XAttribute(XNamespace.Xmlns + "m", (object) FormatterConstants.ODataMetadataNamespace),
        (object) new XAttribute(XNamespace.Xmlns + "at", (object) FormatterConstants.AtomDeletedEntryNamespace),
        (object) new XAttribute(XNamespace.Xmlns + "sync", (object) FormatterConstants.SyncNamespace)
      });
      this.root.Add((object) new XElement(atomXmlNamespace + "title", (object) string.Empty));
      this.root.Add((object) new XElement(atomXmlNamespace + "id", (object) Guid.NewGuid().ToString("B")));
      this.root.Add((object) new XElement(atomXmlNamespace + "updated", (object) XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Unspecified)));
      this.root.Add((object) new XElement(atomXmlNamespace + "link", new object[2]
      {
        (object) new XAttribute((XName) "rel", (object) "self"),
        (object) new XAttribute((XName) "href", (object) string.Empty)
      }));
      this.root.Add((object) new XElement(FormatterConstants.SyncNamespace + "moreChangesAvailable", (object) !isLastBatch));
      this.root.Add((object) new XElement(FormatterConstants.SyncNamespace + nameof (serverBlob), serverBlob != null ? (object) Convert.ToBase64String(serverBlob) : (object) "null"));
    }

    public override void WriteFeed(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      this.document.Add((object) this.root);
      writer.WriteNode(this.document.CreateReader(), true);
      writer.Flush();
    }

    public override void WriteItemInternal(
      IOfflineEntity live,
      string liveTempId,
      IOfflineEntity conflicting,
      string conflictingTempId,
      string desc,
      bool isConflict,
      bool emitMetadataOnly)
    {
      XElement content1 = this.WriteEntry(live, liveTempId, emitMetadataOnly);
      if (conflicting != null)
      {
        XElement content2 = new XElement(FormatterConstants.SyncNamespace + (isConflict ? "syncConflict" : "syncError"));
        content2.Add((object) new XElement(FormatterConstants.SyncNamespace + (isConflict ? "conflictResolution" : "errorDescription"), (object) desc));
        XElement content3 = new XElement(FormatterConstants.SyncNamespace + (isConflict ? "conflictingChange" : "changeInError"));
        content3.Add((object) this.WriteEntry(conflicting, conflictingTempId, false));
        content2.Add((object) content3);
        content1.Add((object) content2);
      }
      this.root.Add((object) content1);
    }

    private XElement WriteEntry(IOfflineEntity live, string tempId, bool emitPartial)
    {
      string fullName = live.GetType().FullName;
      if (!live.GetServiceMetadata().IsTombstone)
      {
        XElement xelement = new XElement(FormatterConstants.AtomXmlNamespace + "entry");
        if (!string.IsNullOrEmpty(live.GetServiceMetadata().ETag))
          xelement.Add((object) new XAttribute(FormatterConstants.ODataMetadataNamespace + "etag", (object) live.GetServiceMetadata().ETag));
        if (!string.IsNullOrEmpty(tempId))
          xelement.Add((object) new XElement(FormatterConstants.SyncNamespace + nameof (tempId), (object) tempId));
        xelement.Add((object) new XElement(FormatterConstants.AtomXmlNamespace + "id", string.IsNullOrEmpty(live.GetServiceMetadata().Id) ? (object) string.Empty : (object) live.GetServiceMetadata().Id));
        xelement.Add((object) new XElement(FormatterConstants.AtomXmlNamespace + "title", (object) new XAttribute((XName) "type", (object) "text")));
        xelement.Add((object) new XElement(FormatterConstants.AtomXmlNamespace + "updated", (object) XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Unspecified)));
        xelement.Add((object) new XElement(FormatterConstants.AtomXmlNamespace + "author", (object) new XElement(FormatterConstants.AtomXmlNamespace + "name")));
        xelement.Add((object) new XElement(FormatterConstants.AtomXmlNamespace + "link", new object[3]
        {
          (object) new XAttribute((XName) "rel", (object) "edit"),
          (object) new XAttribute((XName) "title", (object) fullName),
          (object) new XAttribute((XName) "href", live.GetServiceMetadata().EditUri != (Uri) null ? (object) live.GetServiceMetadata().EditUri.ToString() : (object) string.Empty)
        }));
        xelement.Add((object) new XElement(FormatterConstants.AtomXmlNamespace + "category", new object[2]
        {
          (object) new XAttribute((XName) "term", (object) live.GetType().FullName),
          (object) new XAttribute((XName) "schema", (object) FormatterConstants.ODataSchemaNamespace)
        }));
        XElement content = new XElement(FormatterConstants.AtomXmlNamespace + "content");
        if (!emitPartial)
          content.Add((object) this.WriteEntityContents(live));
        xelement.Add((object) content);
        return xelement;
      }
      XElement xelement1 = new XElement(FormatterConstants.AtomDeletedEntryNamespace + "deleted-entry");
      xelement1.Add((object) new XElement(FormatterConstants.AtomNamespaceUri + "ref", (object) live.GetServiceMetadata().Id));
      xelement1.Add((object) new XElement(FormatterConstants.SyncNamespace + "category", (object) fullName));
      return xelement1;
    }

    private XElement WriteEntityContents(IOfflineEntity entity)
    {
      XElement xelement = new XElement(FormatterConstants.ODataMetadataNamespace + "properties");
      foreach (PropertyInfo propertyInfo in ReflectionUtility.GetPropertyInfoMapping(entity.GetType()))
      {
        string edmType = FormatterUtilities.GetEdmType(propertyInfo.PropertyType);
        object objValue = propertyInfo.GetValue((object) entity, (object[]) null);
        Type type = propertyInfo.PropertyType;
        if (propertyInfo.PropertyType.IsGenericType() && propertyInfo.PropertyType.Name.Equals("Nullable`1", StringComparison.CurrentCultureIgnoreCase))
          type = propertyInfo.PropertyType.GetGenericArguments()[0];
        if (objValue == null)
          xelement.Add((object) new XElement(FormatterConstants.ODataDataNamespace + propertyInfo.Name, new object[2]
          {
            (object) new XAttribute(FormatterConstants.ODataMetadataNamespace + "type", (object) edmType),
            (object) new XAttribute(FormatterConstants.ODataMetadataNamespace + "null", (object) true)
          }));
        else if (type == FormatterConstants.DateTimeType || type == FormatterConstants.TimeSpanType || type == FormatterConstants.DateTimeOffsetType)
          xelement.Add((object) new XElement(FormatterConstants.ODataDataNamespace + propertyInfo.Name, new object[2]
          {
            (object) new XAttribute(FormatterConstants.ODataMetadataNamespace + "type", (object) edmType),
            FormatterUtilities.ConvertDateTimeForType_Atom(objValue, type)
          }));
        else if (type != FormatterConstants.ByteArrayType)
        {
          xelement.Add((object) new XElement(FormatterConstants.ODataDataNamespace + propertyInfo.Name, new object[2]
          {
            (object) new XAttribute(FormatterConstants.ODataMetadataNamespace + "type", (object) edmType),
            objValue
          }));
        }
        else
        {
          byte[] inArray = (byte[]) objValue;
          xelement.Add((object) new XElement(FormatterConstants.ODataDataNamespace + propertyInfo.Name, new object[2]
          {
            (object) new XAttribute(FormatterConstants.ODataMetadataNamespace + "type", (object) edmType),
            (object) Convert.ToBase64String(inArray)
          }));
        }
      }
      return xelement;
    }
  }
}
