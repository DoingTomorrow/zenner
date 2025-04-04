// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelTextFont
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style
{
  public class ExcelTextFont : XmlHelper
  {
    private string _path;
    private XmlNode _rootNode;
    private string _fontLatinPath = "a:latin/@typeface";
    private string _fontCsPath = "a:cs/@typeface";
    private string _boldPath = "@b";
    private string _underLinePath = "@u";
    private string _underLineColorPath = "a:uFill/a:solidFill/a:srgbClr/@val";
    private string _italicPath = "@i";
    private string _strikePath = "@strike";
    private string _sizePath = "@sz";
    private string _colorPath = "a:solidFill/a:srgbClr/@val";

    internal ExcelTextFont(
      XmlNamespaceManager namespaceManager,
      XmlNode rootNode,
      string path,
      string[] schemaNodeOrder)
      : base(namespaceManager, rootNode)
    {
      this.SchemaNodeOrder = schemaNodeOrder;
      this._rootNode = rootNode;
      if (path != "")
      {
        XmlNode xmlNode = rootNode.SelectSingleNode(path, namespaceManager);
        if (xmlNode != null)
          this.TopNode = xmlNode;
      }
      this._path = path;
    }

    public string LatinFont
    {
      get => this.GetXmlNodeString(this._fontLatinPath);
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._fontLatinPath, value);
      }
    }

    protected internal void CreateTopNode()
    {
      if (!(this._path != "") || this.TopNode != this._rootNode)
        return;
      this.CreateNode(this._path);
      this.TopNode = this._rootNode.SelectSingleNode(this._path, this.NameSpaceManager);
    }

    public string ComplexFont
    {
      get => this.GetXmlNodeString(this._fontCsPath);
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._fontCsPath, value);
      }
    }

    public bool Bold
    {
      get => this.GetXmlNodeBool(this._boldPath);
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._boldPath, value ? "1" : "0");
      }
    }

    public eUnderLineType UnderLine
    {
      get => this.TranslateUnderline(this.GetXmlNodeString(this._underLinePath));
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._underLinePath, this.TranslateUnderlineText(value));
      }
    }

    public Color UnderLineColor
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString(this._underLineColorPath);
        return xmlNodeString == "" ? Color.Empty : Color.FromArgb(int.Parse(xmlNodeString, NumberStyles.AllowHexSpecifier));
      }
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._underLineColorPath, value.ToArgb().ToString("X").Substring(2, 6));
      }
    }

    public bool Italic
    {
      get => this.GetXmlNodeBool(this._italicPath);
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._italicPath, value ? "1" : "0");
      }
    }

    public eStrikeType Strike
    {
      get => this.TranslateStrike(this.GetXmlNodeString(this._strikePath));
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._strikePath, this.TranslateStrikeText(value));
      }
    }

    public float Size
    {
      get => (float) (this.GetXmlNodeInt(this._sizePath) / 100);
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._sizePath, ((int) ((double) value * 100.0)).ToString());
      }
    }

    public Color Color
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString(this._colorPath);
        return xmlNodeString == "" ? Color.Empty : Color.FromArgb(int.Parse(xmlNodeString, NumberStyles.AllowHexSpecifier));
      }
      set
      {
        this.CreateTopNode();
        this.SetXmlNodeString(this._colorPath, value.ToArgb().ToString("X").Substring(2, 6));
      }
    }

    private eUnderLineType TranslateUnderline(string text)
    {
      switch (text)
      {
        case "sng":
          return eUnderLineType.Single;
        case "dbl":
          return eUnderLineType.Double;
        case "":
          return eUnderLineType.None;
        default:
          return (eUnderLineType) Enum.Parse(typeof (eUnderLineType), text);
      }
    }

    private string TranslateUnderlineText(eUnderLineType value)
    {
      switch (value)
      {
        case eUnderLineType.Double:
          return "dbl";
        case eUnderLineType.Single:
          return "sng";
        default:
          string str = value.ToString();
          return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
      }
    }

    private eStrikeType TranslateStrike(string text)
    {
      switch (text)
      {
        case "dblStrike":
          return eStrikeType.Double;
        case "sngStrike":
          return eStrikeType.Single;
        default:
          return eStrikeType.No;
      }
    }

    private string TranslateStrikeText(eStrikeType value)
    {
      switch (value)
      {
        case eStrikeType.Double:
          return "dblStrike";
        case eStrikeType.Single:
          return "sngStrike";
        default:
          return "noStrike";
      }
    }

    public void SetFromFont(Font Font)
    {
      this.LatinFont = Font.Name;
      this.ComplexFont = Font.Name;
      this.Size = Font.Size;
      if (Font.Bold)
        this.Bold = Font.Bold;
      if (Font.Italic)
        this.Italic = Font.Italic;
      if (Font.Underline)
        this.UnderLine = eUnderLineType.Single;
      if (!Font.Strikeout)
        return;
      this.Strike = eStrikeType.Single;
    }
  }
}
