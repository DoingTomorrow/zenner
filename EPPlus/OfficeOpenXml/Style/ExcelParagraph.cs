// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelParagraph
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style
{
  public sealed class ExcelParagraph(
    XmlNamespaceManager ns,
    XmlNode rootNode,
    string path,
    string[] schemaNodeOrder) : ExcelTextFont(ns, rootNode, path + "a:rPr", schemaNodeOrder)
  {
    private const string TextPath = "../a:t";

    public string Text
    {
      get => this.GetXmlNodeString("../a:t");
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString("../a:t", value);
      }
    }
  }
}
