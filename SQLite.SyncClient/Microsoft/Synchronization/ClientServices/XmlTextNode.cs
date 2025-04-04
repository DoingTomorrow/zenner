// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlTextNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlTextNode : XmlNode
  {
    protected XmlTextNode(
      XmlNodeType nodeType,
      StringHandle localName,
      ValueHandle value,
      XmlNode.XmlNodeFlags nodeFlags,
      ReadState readState,
      XmlAttributeTextNode attributeTextNode,
      int depthDelta)
      : base(nodeType, localName, value, nodeFlags, readState, attributeTextNode, depthDelta)
    {
    }
  }
}
