// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.ODataJsonWriter
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
  internal class ODataJsonWriter(Uri serviceUri) : SyncWriter(serviceUri)
  {
    private XDocument doc;
    private XElement root;
    private XElement results;

    public override void StartFeed(bool isLastBatch, byte[] serverBlob)
    {
      base.StartFeed(isLastBatch, serverBlob);
      this.doc = new XDocument();
      XElement content1 = new XElement((XName) "root", (object) new XAttribute((XName) "type", (object) "object"));
      this.doc.Add((object) content1);
      this.root = new XElement((XName) "d", (object) new XAttribute((XName) "type", (object) "object"));
      content1.Add((object) this.root);
      XElement content2 = new XElement((XName) "__sync", (object) new XAttribute((XName) "type", (object) "object"));
      content2.Add((object) new XElement((XName) "moreChangesAvailable", new object[2]
      {
        (object) new XAttribute((XName) "type", (object) "boolean"),
        (object) !isLastBatch
      }));
      content2.Add((object) new XElement((XName) nameof (serverBlob), new object[2]
      {
        (object) new XAttribute((XName) "type", serverBlob != null ? (object) "string" : (object) "object"),
        serverBlob != null ? (object) Convert.ToBase64String(serverBlob) : (object) "null"
      }));
      this.root.Add((object) content2);
      this.results = new XElement((XName) "results", (object) new XAttribute((XName) "type", (object) "array"));
      this.root.Add((object) this.results);
    }

    public override void WriteFeed(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteNode(this.doc.CreateReader(), true);
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
      XElement content1 = this.WriteEntry(live, (XElement) null, liveTempId, emitMetadataOnly);
      if (conflicting != null)
      {
        XElement content2 = new XElement((XName) (isConflict ? "__syncConflict" : "__syncError"), (object) new XAttribute((XName) "type", (object) "object"));
        content2.Add((object) new XElement((XName) (isConflict ? "conflictResolution" : "errorDescription"), new object[2]
        {
          (object) new XAttribute((XName) "type", (object) "string"),
          (object) desc
        }));
        XElement xelement = new XElement((XName) (isConflict ? "conflictingChange" : "changeInError"), (object) new XAttribute((XName) "type", (object) "object"));
        this.WriteEntry(conflicting, xelement, conflictingTempId, false);
        content2.Add((object) xelement);
        content1.Add((object) content2);
      }
      this.results.Add((object) content1);
    }

    private XElement WriteEntry(
      IOfflineEntity live,
      XElement entryElement,
      string tempId,
      bool emitPartial)
    {
      string fullName = live.GetType().FullName;
      if (entryElement == null)
        entryElement = new XElement((XName) "item", (object) new XAttribute((XName) "type", (object) "object"));
      XElement content = new XElement((XName) "__metadata", (object) new XAttribute((XName) "type", (object) "object"));
      if (!string.IsNullOrEmpty(tempId))
        content.Add((object) new XElement((XName) nameof (tempId), new object[2]
        {
          (object) new XAttribute((XName) "type", (object) "string"),
          (object) tempId
        }));
      content.Add((object) new XElement((XName) "uri", new object[2]
      {
        (object) new XAttribute((XName) "type", (object) "string"),
        string.IsNullOrEmpty(live.GetServiceMetadata().Id) ? (object) string.Empty : (object) live.GetServiceMetadata().Id
      }));
      if (!string.IsNullOrEmpty(live.GetServiceMetadata().ETag))
        content.Add((object) new XElement((XName) "etag", new object[2]
        {
          (object) new XAttribute((XName) "type", (object) "string"),
          (object) live.GetServiceMetadata().ETag
        }));
      if (live.GetServiceMetadata().EditUri != (Uri) null)
        content.Add((object) new XElement((XName) "edituri", new object[2]
        {
          (object) new XAttribute((XName) "type", (object) "string"),
          (object) live.GetServiceMetadata().EditUri
        }));
      content.Add((object) new XElement((XName) "type", new object[2]
      {
        (object) new XAttribute((XName) "type", (object) "string"),
        (object) fullName
      }));
      if (live.GetServiceMetadata().IsTombstone)
        content.Add((object) new XElement((XName) "isDeleted", new object[2]
        {
          (object) new XAttribute((XName) "type", (object) "boolean"),
          (object) true
        }));
      else if (!emitPartial)
        this.WriteEntityContentsToElement(entryElement, live);
      entryElement.Add((object) content);
      return entryElement;
    }

    private void WriteEntityContentsToElement(XElement contentElement, IOfflineEntity entity)
    {
      foreach (PropertyInfo propertyInfo in ReflectionUtility.GetPropertyInfoMapping(entity.GetType()))
      {
        object objValue = propertyInfo.GetValue((object) entity, (object[]) null);
        if (objValue == null)
          contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
          {
            (object) new XAttribute((XName) "type", (object) "null"),
            null
          }));
        else if (propertyInfo.PropertyType == FormatterConstants.CharType || propertyInfo.PropertyType == FormatterConstants.StringType || propertyInfo.PropertyType == FormatterConstants.GuidType)
          contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
          {
            (object) new XAttribute((XName) "type", (object) "string"),
            objValue
          }));
        else if (propertyInfo.PropertyType == FormatterConstants.DateTimeType || propertyInfo.PropertyType == FormatterConstants.TimeSpanType || propertyInfo.PropertyType == FormatterConstants.DateTimeOffsetType)
          contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
          {
            (object) new XAttribute((XName) "type", (object) "string"),
            FormatterUtilities.ConvertDateTimeForType_Json(objValue, propertyInfo.PropertyType)
          }));
        else if (propertyInfo.PropertyType == FormatterConstants.BoolType)
          contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
          {
            (object) new XAttribute((XName) "type", (object) "boolean"),
            objValue
          }));
        else if (propertyInfo.PropertyType == FormatterConstants.ByteArrayType)
        {
          byte[] inArray = (byte[]) objValue;
          contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
          {
            (object) new XAttribute((XName) "type", (object) "string"),
            (object) Convert.ToBase64String(inArray)
          }));
        }
        else if (propertyInfo.PropertyType.IsGenericType() && propertyInfo.PropertyType.Name.Equals("Nullable`1", StringComparison.CurrentCultureIgnoreCase))
        {
          Type genericTypeArgument = propertyInfo.PropertyType.GenericTypeArguments[0];
          string str = "number";
          if (genericTypeArgument == FormatterConstants.BoolType)
          {
            str = "boolean";
          }
          else
          {
            if (genericTypeArgument == FormatterConstants.DateTimeType || genericTypeArgument == FormatterConstants.TimeSpanType || genericTypeArgument == FormatterConstants.DateTimeOffsetType)
            {
              contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
              {
                (object) new XAttribute((XName) "type", (object) "string"),
                FormatterUtilities.ConvertDateTimeForType_Json(objValue, genericTypeArgument)
              }));
              continue;
            }
            if (genericTypeArgument == FormatterConstants.CharType || genericTypeArgument == FormatterConstants.GuidType)
              str = "string";
          }
          contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
          {
            (object) new XAttribute((XName) "type", (object) str),
            objValue
          }));
        }
        else
          contentElement.Add((object) new XElement((XName) propertyInfo.Name, new object[2]
          {
            (object) new XAttribute((XName) "type", (object) "number"),
            objValue
          }));
      }
    }
  }
}
