// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.AtomHelper
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal static class AtomHelper
  {
    internal static bool IsAtomElement(XmlReader reader, string name)
    {
      return reader.NodeType == XmlNodeType.Element && reader.LocalName == name && (XNamespace) reader.NamespaceURI == FormatterConstants.AtomNamespaceUri;
    }

    internal static bool IsAtomTombstone(XmlReader reader, string name)
    {
      return reader.NodeType == XmlNodeType.Element && reader.LocalName == name && (XNamespace) reader.NamespaceURI == FormatterConstants.AtomDeletedEntryNamespace;
    }

    internal static bool IsODataNamespace(XmlReader reader, XNamespace ns)
    {
      return reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == ns.NamespaceName;
    }

    internal static bool IsSyncElement(XmlReader reader, string name)
    {
      return reader.NodeType == XmlNodeType.Element && reader.LocalName == name && reader.NamespaceURI == FormatterConstants.SyncNamespace.NamespaceName;
    }
  }
}
