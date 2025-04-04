// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlEndOfFileNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlEndOfFileNode(XmlBufferReader bufferReader) : XmlNode(XmlNodeType.None, new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNode.XmlNodeFlags.None, ReadState.EndOfFile, (XmlAttributeTextNode) null, 0)
  {
  }
}
