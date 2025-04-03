// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.XMLManager`1
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class XMLManager<T>
  {
    public void WriteToFile(
      string filename,
      List<string[]> nodeList,
      bool writeStartElement = true,
      bool writeEndElement = true)
    {
      StreamWriter streamWriter = new StreamWriter(filename);
      StringWriter w = new StringWriter();
      XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter) w);
      if (writeStartElement)
        xmlTextWriter.WriteStartElement("items");
      foreach (string[] node in nodeList)
      {
        xmlTextWriter.WriteStartElement("item");
        foreach (string str in node)
        {
          xmlTextWriter.WriteStartElement("prop");
          xmlTextWriter.WriteAttributeString("value", str);
          xmlTextWriter.WriteFullEndElement();
        }
        xmlTextWriter.WriteEndElement();
      }
      if (writeEndElement)
      {
        xmlTextWriter.WriteStartElement("items");
        xmlTextWriter.WriteFullEndElement();
      }
      xmlTextWriter.Flush();
      xmlTextWriter.Close();
      streamWriter.Write(w.ToString());
      streamWriter.Close();
    }

    public List<string[]> ReadStructureFromFile(string fileName)
    {
      FileStream inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load((Stream) inStream);
      return xmlDocument.GetElementsByTagName("item").Cast<XmlNode>().Select<XmlNode, string[]>((Func<XmlNode, string[]>) (node => node.ChildNodes.Cast<XmlElement>().Select<XmlElement, string>((Func<XmlElement, string>) (el => el.Attributes["value"].Value)).ToArray<string>())).ToList<string[]>();
    }

    public void WriteToFileSASModel() => throw new NotImplementedException();
  }
}
