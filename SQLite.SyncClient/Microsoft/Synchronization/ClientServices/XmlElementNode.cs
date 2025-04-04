// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlElementNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlElementNode : XmlNode
  {
    private XmlEndElementNode endElementNode;
    private int bufferOffset;
    public int NameOffset;
    public int NameLength;

    public XmlEndElementNode EndElement => this.endElementNode;

    public int BufferOffset
    {
      get => this.bufferOffset;
      set => this.bufferOffset = value;
    }

    public XmlElementNode(XmlBufferReader bufferReader)
      : this(new StringHandle(bufferReader), new ValueHandle(bufferReader))
    {
    }

    private XmlElementNode(StringHandle localName, ValueHandle value)
      : base(XmlNodeType.Element, localName, value, XmlNode.XmlNodeFlags.CanGetAttribute | XmlNode.XmlNodeFlags.HasContent, ReadState.Interactive, (XmlAttributeTextNode) null, -1)
    {
      this.endElementNode = new XmlEndElementNode(localName, value);
    }
  }
}
