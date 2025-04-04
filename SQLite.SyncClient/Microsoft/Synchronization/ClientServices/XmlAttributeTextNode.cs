// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlAttributeTextNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlAttributeTextNode(StringHandle localName, ValueHandle value) : XmlTextNode(XmlNodeType.Text, localName, value, XmlNode.XmlNodeFlags.CanGetAttribute | XmlNode.XmlNodeFlags.CanMoveToElement | XmlNode.XmlNodeFlags.HasValue | XmlNode.XmlNodeFlags.AtomicValue | XmlNode.XmlNodeFlags.HasContent, ReadState.Interactive, (XmlAttributeTextNode) null, 1)
  {
  }
}
