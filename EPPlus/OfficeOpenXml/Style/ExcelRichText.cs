// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.ExcelRichText
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
  public class ExcelRichText : XmlHelper
  {
    private const string TEXT_PATH = "d:t";
    private const string BOLD_PATH = "d:rPr/d:b";
    private const string ITALIC_PATH = "d:rPr/d:i";
    private const string STRIKE_PATH = "d:rPr/d:strike";
    private const string UNDERLINE_PATH = "d:rPr/d:u";
    private const string VERT_ALIGN_PATH = "d:rPr/d:vertAlign/@val";
    private const string SIZE_PATH = "d:rPr/d:sz/@val";
    private const string FONT_PATH = "d:rPr/d:rFont/@val";
    private const string COLOR_PATH = "d:rPr/d:color/@rgb";
    private ExcelRichText.CallbackDelegate _callback;
    private bool _preserveSpace;

    internal ExcelRichText(XmlNamespaceManager ns, XmlNode topNode)
      : base(ns, topNode)
    {
      this.SchemaNodeOrder = new string[13]
      {
        "rPr",
        "t",
        "b",
        "i",
        "strike",
        "u",
        "vertAlign",
        "sz",
        "color",
        "rFont",
        "family",
        "scheme",
        "charset"
      };
      this.PreserveSpace = false;
    }

    internal void SetCallback(ExcelRichText.CallbackDelegate callback) => this._callback = callback;

    public string Text
    {
      get => this.GetXmlNodeString("d:t");
      set
      {
        this.SetXmlNodeString("d:t", value, false);
        if (this.PreserveSpace)
          (this.TopNode.SelectSingleNode("d:t", this.NameSpaceManager) as XmlElement).SetAttribute("xml:space", "preserve");
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    public bool PreserveSpace
    {
      get
      {
        return this.TopNode.SelectSingleNode("d:t", this.NameSpaceManager) is XmlElement xmlElement ? xmlElement.GetAttribute("xml:space") == "preserve" : this._preserveSpace;
      }
      set
      {
        if (this.TopNode.SelectSingleNode("d:t", this.NameSpaceManager) is XmlElement xmlElement)
        {
          if (value)
            xmlElement.SetAttribute("xml:space", "preserve");
          else
            xmlElement.RemoveAttribute("xml:space");
        }
        this._preserveSpace = false;
      }
    }

    public bool Bold
    {
      get => this.ExistNode("d:rPr/d:b");
      set
      {
        if (value)
          this.CreateNode("d:rPr/d:b");
        else
          this.DeleteNode("d:rPr/d:b");
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    public bool Italic
    {
      get => this.ExistNode("d:rPr/d:i");
      set
      {
        if (value)
          this.CreateNode("d:rPr/d:i");
        else
          this.DeleteNode("d:rPr/d:i");
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    public bool Strike
    {
      get => this.ExistNode("d:rPr/d:strike");
      set
      {
        if (value)
          this.CreateNode("d:rPr/d:strike");
        else
          this.DeleteNode("d:rPr/d:strike");
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    public bool UnderLine
    {
      get => this.ExistNode("d:rPr/d:u");
      set
      {
        if (value)
          this.CreateNode("d:rPr/d:u");
        else
          this.DeleteNode("d:rPr/d:u");
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    public ExcelVerticalAlignmentFont VerticalAlign
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:rPr/d:vertAlign/@val");
        if (xmlNodeString == "")
          return ExcelVerticalAlignmentFont.None;
        try
        {
          return (ExcelVerticalAlignmentFont) Enum.Parse(typeof (ExcelVerticalAlignmentFont), xmlNodeString, true);
        }
        catch
        {
          return ExcelVerticalAlignmentFont.None;
        }
      }
      set
      {
        if (value == ExcelVerticalAlignmentFont.None)
          this.DeleteNode("d:rPr/d:vertAlign/@val");
        else
          this.SetXmlNodeString("d:rPr/d:vertAlign/@val", value.ToString().ToLowerInvariant());
      }
    }

    public float Size
    {
      get => Convert.ToSingle(this.GetXmlNodeDecimal("d:rPr/d:sz/@val"));
      set
      {
        this.SetXmlNodeString("d:rPr/d:sz/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    public string FontName
    {
      get => this.GetXmlNodeString("d:rPr/d:rFont/@val");
      set
      {
        this.SetXmlNodeString("d:rPr/d:rFont/@val", value);
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    public Color Color
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:rPr/d:color/@rgb");
        return xmlNodeString == "" ? Color.Empty : Color.FromArgb(int.Parse(xmlNodeString, NumberStyles.AllowHexSpecifier));
      }
      set
      {
        this.SetXmlNodeString("d:rPr/d:color/@rgb", value.ToArgb().ToString("X"));
        if (this._callback == null)
          return;
        this._callback();
      }
    }

    internal delegate void CallbackDelegate();
  }
}
