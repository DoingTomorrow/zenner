// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlAttributeNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlAttributeNode : XmlNode
  {
    public XmlAttributeNode(XmlBufferReader bufferReader)
      : this(new StringHandle(bufferReader), new ValueHandle(bufferReader))
    {
    }

    private XmlAttributeNode(StringHandle localName, ValueHandle value)
      : base(XmlNodeType.Attribute, localName, value, XmlNode.XmlNodeFlags.CanGetAttribute | XmlNode.XmlNodeFlags.CanMoveToElement | XmlNode.XmlNodeFlags.HasValue | XmlNode.XmlNodeFlags.AtomicValue, ReadState.Interactive, new XmlAttributeTextNode(localName, value), 0)
    {
    }
  }
}
