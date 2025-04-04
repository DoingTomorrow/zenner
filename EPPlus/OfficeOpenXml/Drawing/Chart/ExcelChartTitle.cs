// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartTitle
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartTitle : XmlHelper
  {
    private const string titlePath = "c:tx/c:rich/a:p/a:r/a:t";
    private const string TextVerticalPath = "xdr:sp/xdr:txBody/a:bodyPr/@vert";
    private ExcelDrawingBorder _border;
    private ExcelDrawingFill _fill;
    private string[] paragraphNodeOrder = new string[9]
    {
      "pPr",
      "defRPr",
      "solidFill",
      "uFill",
      "latin",
      "cs",
      "r",
      "rPr",
      "t"
    };
    private ExcelParagraphCollection _richText;

    internal ExcelChartTitle(XmlNamespaceManager nameSpaceManager, XmlNode node)
      : base(nameSpaceManager, node)
    {
      XmlNode newChild = node.SelectSingleNode("c:title", this.NameSpaceManager);
      if (newChild == null)
      {
        newChild = (XmlNode) node.OwnerDocument.CreateElement("c", "title", "http://schemas.openxmlformats.org/drawingml/2006/chart");
        node.InsertBefore(newChild, node.ChildNodes[0]);
        newChild.InnerXml = "<c:tx><c:rich><a:bodyPr /><a:lstStyle /><a:p><a:r><a:t /></a:r></a:p></c:rich></c:tx><c:layout /><c:overlay val=\"0\" />";
      }
      this.TopNode = newChild;
      this.SchemaNodeOrder = new string[5]
      {
        "tx",
        "bodyPr",
        "lstStyle",
        "layout",
        "overlay"
      };
    }

    public string Text
    {
      get => this.RichText.Text;
      set => this.RichText.Text = value;
    }

    public ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.TopNode, "c:spPr/a:ln");
        return this._border;
      }
    }

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, "c:spPr");
        return this._fill;
      }
    }

    public ExcelTextFont Font
    {
      get
      {
        if (this._richText == null || this._richText.Count == 0)
          this.RichText.Add("");
        return (ExcelTextFont) this._richText[0];
      }
    }

    public ExcelParagraphCollection RichText
    {
      get
      {
        if (this._richText == null)
          this._richText = new ExcelParagraphCollection(this.NameSpaceManager, this.TopNode, "c:tx/c:rich/a:p", this.paragraphNodeOrder);
        return this._richText;
      }
    }

    public bool Overlay
    {
      get => this.GetXmlNodeBool("c:overlay/@val");
      set => this.SetXmlNodeBool("c:overlay/@val", value);
    }

    public bool AnchorCtr
    {
      get => this.GetXmlNodeBool("c:tx/c:rich/a:bodyPr/@anchorCtr", false);
      set => this.SetXmlNodeBool("c:tx/c:rich/a:bodyPr/@anchorCtr", value, false);
    }

    public eTextAnchoringType Anchor
    {
      get
      {
        return ExcelDrawing.GetTextAchoringEnum(this.GetXmlNodeString("c:tx/c:rich/a:bodyPr/@anchor"));
      }
      set
      {
        this.SetXmlNodeString("c:tx/c:rich/a:bodyPr/@anchorCtr", ExcelDrawing.GetTextAchoringText(value));
      }
    }

    public eTextVerticalType TextVertical
    {
      get => ExcelDrawing.GetTextVerticalEnum(this.GetXmlNodeString("c:tx/c:rich/a:bodyPr/@vert"));
      set
      {
        this.SetXmlNodeString("c:tx/c:rich/a:bodyPr/@vert", ExcelDrawing.GetTextVerticalText(value));
      }
    }

    public double Rotation
    {
      get
      {
        int xmlNodeInt = this.GetXmlNodeInt("c:tx/c:rich/a:bodyPr/@rot");
        return xmlNodeInt < 0 ? (double) (360 - xmlNodeInt / 60000) : (double) (xmlNodeInt / 60000);
      }
      set
      {
        if (value < 0.0 || value > 360.0)
          throw new ArgumentOutOfRangeException("Rotation must be between 0 and 360");
        this.SetXmlNodeString("c:tx/c:rich/a:bodyPr/@rot", (value <= 180.0 ? (int) (value * 60000.0) : (int) ((value - 360.0) * 60000.0)).ToString());
      }
    }
  }
}
