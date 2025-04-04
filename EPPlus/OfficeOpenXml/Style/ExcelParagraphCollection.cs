// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelParagraphCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style
{
  public class ExcelParagraphCollection : XmlHelper, IEnumerable<ExcelParagraph>, IEnumerable
  {
    private List<ExcelParagraph> _list = new List<ExcelParagraph>();
    private string _path;

    internal ExcelParagraphCollection(
      XmlNamespaceManager ns,
      XmlNode topNode,
      string path,
      string[] schemaNodeOrder)
      : base(ns, topNode)
    {
      XmlNodeList xmlNodeList = topNode.SelectNodes(path + "/a:r", this.NameSpaceManager);
      this.SchemaNodeOrder = schemaNodeOrder;
      if (xmlNodeList != null)
      {
        foreach (XmlNode rootNode in xmlNodeList)
          this._list.Add(new ExcelParagraph(ns, rootNode, "", schemaNodeOrder));
      }
      this._path = path;
    }

    public ExcelParagraph this[int Index] => this._list[Index];

    public int Count => this._list.Count;

    public ExcelParagraph Add(string Text)
    {
      XmlDocument xmlDocument = !(this.TopNode is XmlDocument) ? this.TopNode.OwnerDocument : this.TopNode as XmlDocument;
      XmlNode xmlNode = this.TopNode.SelectSingleNode(this._path, this.NameSpaceManager);
      if (xmlNode == null)
      {
        this.CreateNode(this._path);
        xmlNode = this.TopNode.SelectSingleNode(this._path, this.NameSpaceManager);
      }
      XmlElement element1 = xmlDocument.CreateElement("a", "r", "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlNode.AppendChild((XmlNode) element1);
      XmlElement element2 = xmlDocument.CreateElement("a", "rPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
      element1.AppendChild((XmlNode) element2);
      ExcelParagraph excelParagraph = new ExcelParagraph(this.NameSpaceManager, (XmlNode) element1, "", this.SchemaNodeOrder);
      excelParagraph.ComplexFont = "Calibri";
      excelParagraph.LatinFont = "Calibri";
      excelParagraph.Size = 11f;
      excelParagraph.Text = Text;
      this._list.Add(excelParagraph);
      return excelParagraph;
    }

    public void Clear()
    {
      this._list.Clear();
      this.TopNode.RemoveAll();
    }

    public void RemoveAt(int Index)
    {
      XmlNode oldChild = this._list[Index].TopNode;
      while (oldChild != null && oldChild.Name != "a:r")
        oldChild = oldChild.ParentNode;
      oldChild.ParentNode.RemoveChild(oldChild);
      this._list.RemoveAt(Index);
    }

    public void Remove(ExcelRichText Item) => this.TopNode.RemoveChild(Item.TopNode);

    public string Text
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (ExcelParagraph excelParagraph in this._list)
          stringBuilder.Append(excelParagraph.Text);
        return stringBuilder.ToString();
      }
      set
      {
        if (this.Count == 0)
        {
          this.Add(value);
        }
        else
        {
          this[0].Text = value;
          int count = this.Count;
          for (int Index = this.Count - 1; Index > 0; --Index)
            this.RemoveAt(Index);
        }
      }
    }

    IEnumerator<ExcelParagraph> IEnumerable<ExcelParagraph>.GetEnumerator()
    {
      return (IEnumerator<ExcelParagraph>) this._list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
  }
}
