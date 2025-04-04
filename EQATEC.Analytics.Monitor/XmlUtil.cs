// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.XmlUtil
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal static class XmlUtil
  {
    internal static XmlWriter CreateXmlWriter(Stream ms)
    {
      return XmlWriter.Create(ms, new XmlWriterSettings()
      {
        Encoding = (Encoding) new UTF8Encoding(false)
      });
    }

    internal static string GetInnerText(XmlReader xr)
    {
      if (xr.NodeType != XmlNodeType.Element || xr.IsEmptyElement)
        return (string) null;
      XmlReader xmlReader = xr.ReadSubtree();
      StringBuilder stringBuilder = new StringBuilder();
      if (xmlReader.Read())
      {
        while (xmlReader.Read() && (xmlReader.NodeType == XmlNodeType.Text || xmlReader.NodeType == XmlNodeType.CDATA || xmlReader.NodeType == XmlNodeType.Whitespace || xmlReader.NodeType == XmlNodeType.SignificantWhitespace))
          stringBuilder.Append(xmlReader.Value);
      }
      return stringBuilder.ToString();
    }

    internal static void WriteStartElement(
      XmlWriter xtw,
      string elementName,
      params string[] attrs)
    {
      xtw.WriteStartElement(elementName);
      for (int index = 1; index < attrs.Length; index += 2)
      {
        if (attrs[index - 1] != null && attrs[index] != null)
          xtw.WriteAttributeString(attrs[index - 1], XmlUtil.SanitizeAttributeValue(attrs[index]));
      }
      if (attrs.Length % 2 != 1 || attrs[attrs.Length - 1] == null)
        return;
      xtw.WriteString(attrs[attrs.Length - 1]);
    }

    internal static void WriteFullElement(XmlWriter xtw, string elementName, params string[] attrs)
    {
      XmlUtil.WriteStartElement(xtw, elementName, attrs);
      xtw.WriteEndElement();
    }

    internal static string SanitizeAttributeValue(string attrValue)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < attrValue.Length; ++index)
      {
        char ch = attrValue[index];
        if (ch < char.MinValue || ch >= ' ' || ch == '\t' || ch == '\r' || ch == '\n')
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }
  }
}
