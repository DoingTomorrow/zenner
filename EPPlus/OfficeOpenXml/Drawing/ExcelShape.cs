// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.ExcelShape
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Style;
using System;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing
{
  public sealed class ExcelShape : ExcelDrawing
  {
    private const string ShapeStylePath = "xdr:sp/xdr:spPr/a:prstGeom/@prst";
    private const string PARAGRAPH_PATH = "xdr:sp/xdr:txBody/a:p";
    private const string TextPath = "xdr:sp/xdr:txBody/a:p/a:r/a:t";
    private const string TextAnchoringPath = "xdr:sp/xdr:txBody/a:bodyPr/@anchor";
    private const string TextAnchoringCtlPath = "xdr:sp/xdr:txBody/a:bodyPr/@anchorCtr";
    private const string TEXT_ALIGN_PATH = "xdr:sp/xdr:txBody/a:p/a:pPr/@algn";
    private const string INDENT_ALIGN_PATH = "xdr:sp/xdr:txBody/a:p/a:pPr/@lvl";
    private const string TextVerticalPath = "xdr:sp/xdr:txBody/a:bodyPr/@vert";
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;
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
    private ExcelTextFont _font;
    private string lockTextPath = "xdr:sp/@fLocksText";
    private ExcelParagraphCollection _richText;

    internal ExcelShape(ExcelDrawings drawings, XmlNode node)
      : base(drawings, node, "xdr:sp/xdr:nvSpPr/xdr:cNvPr/@name")
    {
      this.init();
    }

    internal ExcelShape(ExcelDrawings drawings, XmlNode node, eShapeStyle style)
      : base(drawings, node, "xdr:sp/xdr:nvSpPr/xdr:cNvPr/@name")
    {
      this.init();
      XmlElement element = node.OwnerDocument.CreateElement("xdr", "sp", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      element.SetAttribute("macro", "");
      element.SetAttribute("textlink", "");
      node.AppendChild((XmlNode) element);
      element.InnerXml = this.ShapeStartXml();
      node.AppendChild((XmlNode) element.OwnerDocument.CreateElement("xdr", "clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing"));
    }

    private void init()
    {
      this.SchemaNodeOrder = new string[11]
      {
        "prstGeom",
        "ln",
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
    }

    public eShapeStyle Style
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("xdr:sp/xdr:spPr/a:prstGeom/@prst");
        try
        {
          return (eShapeStyle) Enum.Parse(typeof (eShapeStyle), xmlNodeString, true);
        }
        catch
        {
          throw new Exception(string.Format("Invalid shapetype {0}", (object) xmlNodeString));
        }
      }
      set
      {
        string str = value.ToString();
        this.SetXmlNodeString("xdr:sp/xdr:spPr/a:prstGeom/@prst", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
      }
    }

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, "xdr:sp/xdr:spPr");
        return this._fill;
      }
    }

    public ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.TopNode, "xdr:sp/xdr:spPr/a:ln");
        return this._border;
      }
    }

    public ExcelTextFont Font
    {
      get
      {
        if (this._font == null)
        {
          if (this.TopNode.SelectSingleNode("xdr:sp/xdr:txBody/a:p", this.NameSpaceManager) == null)
          {
            this.Text = "";
            this.TopNode.SelectSingleNode("xdr:sp/xdr:txBody/a:p", this.NameSpaceManager);
          }
          this._font = new ExcelTextFont(this.NameSpaceManager, this.TopNode, "xdr:sp/xdr:txBody/a:p/a:pPr/a:defRPr", this.paragraphNodeOrder);
        }
        return this._font;
      }
    }

    public string Text
    {
      get => this.GetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:r/a:t");
      set => this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:r/a:t", value);
    }

    public bool LockText
    {
      get => this.GetXmlNodeBool(this.lockTextPath, true);
      set => this.SetXmlNodeBool(this.lockTextPath, value);
    }

    public ExcelParagraphCollection RichText
    {
      get
      {
        if (this._richText == null)
          this._richText = new ExcelParagraphCollection(this.NameSpaceManager, this.TopNode, "xdr:sp/xdr:txBody/a:p", this.paragraphNodeOrder);
        return this._richText;
      }
    }

    public eTextAnchoringType TextAnchoring
    {
      get
      {
        return ExcelDrawing.GetTextAchoringEnum(this.GetXmlNodeString("xdr:sp/xdr:txBody/a:bodyPr/@anchor"));
      }
      set
      {
        this.SetXmlNodeString("xdr:sp/xdr:txBody/a:bodyPr/@anchor", ExcelDrawing.GetTextAchoringText(value));
      }
    }

    public bool TextAnchoringControl
    {
      get => this.GetXmlNodeBool("xdr:sp/xdr:txBody/a:bodyPr/@anchorCtr");
      set
      {
        if (value)
          this.SetXmlNodeString("xdr:sp/xdr:txBody/a:bodyPr/@anchorCtr", "1");
        else
          this.SetXmlNodeString("xdr:sp/xdr:txBody/a:bodyPr/@anchorCtr", "0");
      }
    }

    public eTextAlignment TextAlignment
    {
      get
      {
        switch (this.GetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@algn"))
        {
          case "ctr":
            return eTextAlignment.Center;
          case "r":
            return eTextAlignment.Right;
          case "dist":
            return eTextAlignment.Distributed;
          case "just":
            return eTextAlignment.Justified;
          case "justLow":
            return eTextAlignment.JustifiedLow;
          case "thaiDist":
            return eTextAlignment.ThaiDistributed;
          default:
            return eTextAlignment.Left;
        }
      }
      set
      {
        switch (value)
        {
          case eTextAlignment.Center:
            this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@algn", "ctr");
            break;
          case eTextAlignment.Right:
            this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@algn", "r");
            break;
          case eTextAlignment.Distributed:
            this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@algn", "dist");
            break;
          case eTextAlignment.Justified:
            this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@algn", "just");
            break;
          case eTextAlignment.JustifiedLow:
            this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@algn", "justLow");
            break;
          case eTextAlignment.ThaiDistributed:
            this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@algn", "thaiDist");
            break;
          default:
            this.DeleteNode("xdr:sp/xdr:txBody/a:p/a:pPr/@algn");
            break;
        }
      }
    }

    public int Indent
    {
      get => this.GetXmlNodeInt("xdr:sp/xdr:txBody/a:p/a:pPr/@lvl");
      set
      {
        if (value < 0 || value > 8)
          throw new ArgumentOutOfRangeException("Indent level must be between 0 and 8");
        this.SetXmlNodeString("xdr:sp/xdr:txBody/a:p/a:pPr/@lvl", value.ToString());
      }
    }

    public eTextVerticalType TextVertical
    {
      get
      {
        return ExcelDrawing.GetTextVerticalEnum(this.GetXmlNodeString("xdr:sp/xdr:txBody/a:bodyPr/@vert"));
      }
      set
      {
        this.SetXmlNodeString("xdr:sp/xdr:txBody/a:bodyPr/@vert", ExcelDrawing.GetTextVerticalText(value));
      }
    }

    private string ShapeStartXml()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("<xdr:nvSpPr><xdr:cNvPr id=\"{0}\" name=\"{1}\" /><xdr:cNvSpPr /></xdr:nvSpPr><xdr:spPr><a:prstGeom prst=\"rect\"><a:avLst /></a:prstGeom></xdr:spPr><xdr:style><a:lnRef idx=\"2\"><a:schemeClr val=\"accent1\"><a:shade val=\"50000\" /></a:schemeClr></a:lnRef><a:fillRef idx=\"1\"><a:schemeClr val=\"accent1\" /></a:fillRef><a:effectRef idx=\"0\"><a:schemeClr val=\"accent1\" /></a:effectRef><a:fontRef idx=\"minor\"><a:schemeClr val=\"lt1\" /></a:fontRef></xdr:style><xdr:txBody><a:bodyPr vertOverflow=\"clip\" rtlCol=\"0\" anchor=\"ctr\" /><a:lstStyle /><a:p></a:p></xdr:txBody>", (object) this._id, (object) this.Name);
      return stringBuilder.ToString();
    }

    internal new string Id => this.Name + this.Text;
  }
}
