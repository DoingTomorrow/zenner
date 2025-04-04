// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.XmlAtomicTextNode
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class XmlAtomicTextNode(XmlBufferReader bufferReader) : XmlTextNode(XmlNodeType.Text, new StringHandle(bufferReader), new ValueHandle(bufferReader), XmlNode.XmlNodeFlags.HasValue | XmlNode.XmlNodeFlags.AtomicValue | XmlNode.XmlNodeFlags.SkipValue | XmlNode.XmlNodeFlags.HasContent, ReadState.Interactive, (XmlAttributeTextNode) null, 0)
  {
  }
}
