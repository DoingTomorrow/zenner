// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelComment
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing.Vml;
using OfficeOpenXml.Style;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelComment : ExcelVmlDrawingComment
  {
    private const string AUTHORS_PATH = "d:comments/d:authors";
    private const string AUTHOR_PATH = "d:comments/d:authors/d:author";
    internal XmlHelper _commentHelper;

    internal ExcelComment(XmlNamespaceManager ns, XmlNode commentTopNode, ExcelRangeBase cell)
      : base((XmlNode) null, cell, cell.Worksheet.VmlDrawingsComments.NameSpaceManager)
    {
      this._commentHelper = XmlHelperFactory.Create(ns, commentTopNode);
      XmlNode xmlNode = commentTopNode.SelectSingleNode("d:text", ns);
      if (xmlNode == null)
      {
        xmlNode = (XmlNode) commentTopNode.OwnerDocument.CreateElement("text", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        commentTopNode.AppendChild(xmlNode);
      }
      if (!cell.Worksheet._vmlDrawings.ContainsKey(ExcelCellBase.GetCellID(cell.Worksheet.SheetID, cell.Start.Row, cell.Start.Column)))
        cell.Worksheet._vmlDrawings.Add(cell);
      this.TopNode = cell.Worksheet.VmlDrawingsComments[ExcelCellBase.GetCellID(cell.Worksheet.SheetID, cell.Start.Row, cell.Start.Column)].TopNode;
      this.RichText = new ExcelRichTextCollection(ns, xmlNode);
    }

    public string Author
    {
      get
      {
        return this._commentHelper.TopNode.OwnerDocument.SelectSingleNode(string.Format("{0}[{1}]", (object) "d:comments/d:authors/d:author", (object) (this._commentHelper.GetXmlNodeInt("@authorId") + 1)), this._commentHelper.NameSpaceManager).InnerText;
      }
      set => this._commentHelper.SetXmlNodeString("@authorId", this.GetAuthor(value).ToString());
    }

    private int GetAuthor(string value)
    {
      int author = 0;
      bool flag = false;
      foreach (XmlNode selectNode in this._commentHelper.TopNode.OwnerDocument.SelectNodes("d:comments/d:authors/d:author", this._commentHelper.NameSpaceManager))
      {
        if (selectNode.InnerText == value)
        {
          flag = true;
          break;
        }
        ++author;
      }
      if (!flag)
      {
        XmlElement element = this._commentHelper.TopNode.OwnerDocument.CreateElement("d", "author", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        this._commentHelper.TopNode.OwnerDocument.SelectSingleNode("d:comments/d:authors", this._commentHelper.NameSpaceManager).AppendChild((XmlNode) element);
        element.InnerText = value;
      }
      return author;
    }

    public string Text
    {
      get => this.RichText.Text;
      set => this.RichText.Text = value;
    }

    public ExcelRichText Font => this.RichText.Count > 0 ? this.RichText[0] : (ExcelRichText) null;

    public ExcelRichTextCollection RichText { get; set; }
  }
}
