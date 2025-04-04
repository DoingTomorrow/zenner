// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Vml.ExcelVmlDrawingComment
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Vml
{
  public class ExcelVmlDrawingComment : ExcelVmlDrawingBase, IRangeID
  {
    private const string VERTICAL_ALIGNMENT_PATH = "x:ClientData/x:TextVAlign";
    private const string HORIZONTAL_ALIGNMENT_PATH = "x:ClientData/x:TextHAlign";
    private const string VISIBLE_PATH = "x:ClientData/x:Visible";
    private const string BACKGROUNDCOLOR_PATH = "@fillcolor";
    private const string BACKGROUNDCOLOR2_PATH = "v:fill/@color2";
    private const string LINESTYLE_PATH = "v:stroke/@dashstyle";
    private const string ENDCAP_PATH = "v:stroke/@endcap";
    private const string LINECOLOR_PATH = "@strokecolor";
    private const string LINEWIDTH_PATH = "@strokeweight";
    private const string TEXTBOX_STYLE_PATH = "v:textbox/@style";
    private const string LOCKED_PATH = "x:ClientData/x:Locked";
    private const string LOCK_TEXT_PATH = "x:ClientData/x:LockText";
    private const string STYLE_PATH = "@style";
    private ExcelVmlDrawingPosition _from;
    private ExcelVmlDrawingPosition _to;

    internal ExcelVmlDrawingComment(XmlNode topNode, ExcelRangeBase range, XmlNamespaceManager ns)
      : base(topNode, ns)
    {
      this.Range = range;
      this.SchemaNodeOrder = new string[17]
      {
        "fill",
        "stroke",
        "shadow",
        "path",
        "textbox",
        "ClientData",
        "MoveWithCells",
        "SizeWithCells",
        "Anchor",
        nameof (Locked),
        "AutoFill",
        nameof (LockText),
        "TextHAlign",
        "TextVAlign",
        "Row",
        "Column",
        nameof (Visible)
      };
    }

    internal ExcelRangeBase Range { get; set; }

    public eTextAlignVerticalVml VerticalAlignment
    {
      get
      {
        switch (this.GetXmlNodeString("x:ClientData/x:TextVAlign"))
        {
          case "Center":
            return eTextAlignVerticalVml.Center;
          case "Bottom":
            return eTextAlignVerticalVml.Bottom;
          default:
            return eTextAlignVerticalVml.Top;
        }
      }
      set
      {
        switch (value)
        {
          case eTextAlignVerticalVml.Center:
            this.SetXmlNodeString("x:ClientData/x:TextVAlign", "Center");
            break;
          case eTextAlignVerticalVml.Bottom:
            this.SetXmlNodeString("x:ClientData/x:TextVAlign", "Bottom");
            break;
          default:
            this.DeleteNode("x:ClientData/x:TextVAlign");
            break;
        }
      }
    }

    public eTextAlignHorizontalVml HorizontalAlignment
    {
      get
      {
        switch (this.GetXmlNodeString("x:ClientData/x:TextHAlign"))
        {
          case "Center":
            return eTextAlignHorizontalVml.Center;
          case "Right":
            return eTextAlignHorizontalVml.Right;
          default:
            return eTextAlignHorizontalVml.Left;
        }
      }
      set
      {
        switch (value)
        {
          case eTextAlignHorizontalVml.Center:
            this.SetXmlNodeString("x:ClientData/x:TextHAlign", "Center");
            break;
          case eTextAlignHorizontalVml.Right:
            this.SetXmlNodeString("x:ClientData/x:TextHAlign", "Right");
            break;
          default:
            this.DeleteNode("x:ClientData/x:TextHAlign");
            break;
        }
      }
    }

    public bool Visible
    {
      get => this.TopNode.SelectSingleNode("x:ClientData/x:Visible", this.NameSpaceManager) != null;
      set
      {
        if (value)
        {
          this.CreateNode("x:ClientData/x:Visible");
          this.Style = this.SetStyle(this.Style, "visibility", "visible");
        }
        else
        {
          this.DeleteNode("x:ClientData/x:Visible");
          this.Style = this.SetStyle(this.Style, "visibility", "hidden");
        }
      }
    }

    public Color BackgroundColor
    {
      get
      {
        string s = this.GetXmlNodeString("@fillcolor");
        if (s == "")
          return Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 225);
        if (s.StartsWith("#"))
          s = s.Substring(1, s.Length - 1);
        int result;
        return int.TryParse(s, NumberStyles.AllowHexSpecifier, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? Color.FromArgb(result) : Color.Empty;
      }
      set
      {
        this.SetXmlNodeString("@fillcolor", "#" + value.ToArgb().ToString("X").Substring(2, 6));
      }
    }

    public eLineStyleVml LineStyle
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("v:stroke/@dashstyle");
        switch (xmlNodeString)
        {
          case "":
            return eLineStyleVml.Solid;
          case "1 1":
            return (eLineStyleVml) Enum.Parse(typeof (eLineStyleVml), this.GetXmlNodeString("v:stroke/@endcap"), true);
          default:
            return (eLineStyleVml) Enum.Parse(typeof (eLineStyleVml), xmlNodeString, true);
        }
      }
      set
      {
        if (value == eLineStyleVml.Round || value == eLineStyleVml.Square)
        {
          this.SetXmlNodeString("v:stroke/@dashstyle", "1 1");
          if (value == eLineStyleVml.Round)
            this.SetXmlNodeString("v:stroke/@endcap", "round");
          else
            this.DeleteNode("v:stroke/@endcap");
        }
        else
        {
          string str = value.ToString();
          this.SetXmlNodeString("v:stroke/@dashstyle", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
          this.DeleteNode("v:stroke/@endcap");
        }
      }
    }

    public Color LineColor
    {
      get
      {
        string s = this.GetXmlNodeString("@strokecolor");
        if (s == "")
          return Color.Black;
        if (s.StartsWith("#"))
          s = s.Substring(1, s.Length - 1);
        int result;
        return int.TryParse(s, NumberStyles.AllowHexSpecifier, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? Color.FromArgb(result) : Color.Empty;
      }
      set
      {
        this.SetXmlNodeString("@strokecolor", "#" + value.ToArgb().ToString("X").Substring(2, 6));
      }
    }

    public float LineWidth
    {
      get
      {
        string s = this.GetXmlNodeString("@strokeweight");
        if (s == "")
          return 0.75f;
        if (s.EndsWith("pt"))
          s = s.Substring(0, s.Length - 2);
        float result;
        return float.TryParse(s, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result : 0.0f;
      }
      set
      {
        this.SetXmlNodeString("@strokeweight", value.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "pt");
      }
    }

    public bool AutoFit
    {
      get
      {
        string str;
        this.GetStyle(this.GetXmlNodeString("v:textbox/@style"), "mso-fit-shape-to-text", out str);
        return str == "t";
      }
      set
      {
        this.SetXmlNodeString("v:textbox/@style", this.SetStyle(this.GetXmlNodeString("v:textbox/@style"), "mso-fit-shape-to-text", value ? "t" : ""));
      }
    }

    public bool Locked
    {
      get => this.GetXmlNodeBool("x:ClientData/x:Locked", false);
      set => this.SetXmlNodeBool("x:ClientData/x:Locked", value, false);
    }

    public bool LockText
    {
      get => this.GetXmlNodeBool("x:ClientData/x:LockText", false);
      set => this.SetXmlNodeBool("x:ClientData/x:LockText", value, false);
    }

    public ExcelVmlDrawingPosition From
    {
      get
      {
        if (this._from == null)
          this._from = new ExcelVmlDrawingPosition(this.NameSpaceManager, this.TopNode.SelectSingleNode("x:ClientData", this.NameSpaceManager), 0);
        return this._from;
      }
    }

    public ExcelVmlDrawingPosition To
    {
      get
      {
        if (this._to == null)
          this._to = new ExcelVmlDrawingPosition(this.NameSpaceManager, this.TopNode.SelectSingleNode("x:ClientData", this.NameSpaceManager), 4);
        return this._to;
      }
    }

    internal string Style
    {
      get => this.GetXmlNodeString("@style");
      set => this.SetXmlNodeString("@style", value);
    }

    ulong IRangeID.RangeID
    {
      get
      {
        return ExcelCellBase.GetCellID(this.Range.Worksheet.SheetID, this.Range.Start.Row, this.Range.Start.Column);
      }
      set
      {
      }
    }
  }
}
