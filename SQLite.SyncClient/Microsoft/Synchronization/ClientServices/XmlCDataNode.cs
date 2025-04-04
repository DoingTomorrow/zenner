// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlCDataNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlCDataNode(XmlBufferReader bufferReader) : XmlTextNode(XmlNodeType.CDATA, new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNode.XmlNodeFlags.HasValue | XmlNode.XmlNodeFlags.HasContent, ReadState.Interactive, (XmlAttributeTextNode) null, 0)
  {
  }
}
