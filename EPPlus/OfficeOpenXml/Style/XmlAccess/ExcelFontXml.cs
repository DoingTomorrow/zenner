// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelFontXml
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public sealed class ExcelFontXml : StyleXmlHelper
  {
    private const string namePath = "d:name/@val";
    private const string sizePath = "d:sz/@val";
    private const string familyPath = "d:family/@val";
    private const string _colorPath = "d:color";
    private const string schemePath = "d:scheme/@val";
    private const string boldPath = "d:b";
    private const string italicPath = "d:i";
    private const string strikePath = "d:strike";
    private const string underLinedPath = "d:u";
    private const string verticalAlignPath = "d:vertAlign/@val";
    private string _name;
    private float _size;
    private int _family;
    private ExcelColorXml _color;
    private string _scheme = "";
    private bool _bold;
    private bool _italic;
    private bool _strike;
    private ExcelUnderLineType _underlineType;
    private string _verticalAlign;

    internal ExcelFontXml(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
      this._name = "";
      this._size = 0.0f;
      this._family = int.MinValue;
      this._scheme = "";
      this._color = this._color = new ExcelColorXml(this.NameSpaceManager);
      this._bold = false;
      this._italic = false;
      this._strike = false;
      this._underlineType = ExcelUnderLineType.None;
      this._verticalAlign = "";
    }

    internal ExcelFontXml(XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      this._name = this.GetXmlNodeString("d:name/@val");
      this._size = (float) this.GetXmlNodeDecimal("d:sz/@val");
      this._family = this.GetXmlNodeInt("d:family/@val");
      this._scheme = this.GetXmlNodeString("d:scheme/@val");
      this._color = new ExcelColorXml(nsm, topNode.SelectSingleNode("d:color", nsm));
      this._bold = topNode.SelectSingleNode("d:b", this.NameSpaceManager) != null;
      this._italic = topNode.SelectSingleNode("d:i", this.NameSpaceManager) != null;
      this._strike = topNode.SelectSingleNode("d:strike", this.NameSpaceManager) != null;
      this._verticalAlign = this.GetXmlNodeString("d:vertAlign/@val");
      if (topNode.SelectSingleNode("d:u", this.NameSpaceManager) != null)
      {
        string xmlNodeString = this.GetXmlNodeString("d:u/@val");
        if (xmlNodeString == "")
          this._underlineType = ExcelUnderLineType.Single;
        else
          this._underlineType = (ExcelUnderLineType) Enum.Parse(typeof (ExcelUnderLineType), xmlNodeString, true);
      }
      else
        this._underlineType = ExcelUnderLineType.None;
    }

    internal override string Id
    {
      get
      {
        return this.Name + "|" + (object) this.Size + "|" + (object) this.Family + "|" + this.Color.Id + "|" + this.Scheme + "|" + this.Bold.ToString() + "|" + this.Italic.ToString() + "|" + this.Strike.ToString() + "|" + this.VerticalAlign + "|" + this.UnderLineType.ToString();
      }
    }

    public string Name
    {
      get => this._name;
      set
      {
        this.Scheme = "";
        this._name = value;
      }
    }

    public float Size
    {
      get => this._size;
      set => this._size = value;
    }

    public int Family
    {
      get => this._family;
      set => this._family = value;
    }

    public ExcelColorXml Color
    {
      get => this._color;
      internal set => this._color = value;
    }

    public string Scheme
    {
      get => this._scheme;
      private set => this._scheme = value;
    }

    public bool Bold
    {
      get => this._bold;
      set => this._bold = value;
    }

    public bool Italic
    {
      get => this._italic;
      set => this._italic = value;
    }

    public bool Strike
    {
      get => this._strike;
      set => this._strike = value;
    }

    public bool UnderLine
    {
      get => this.UnderLineType != ExcelUnderLineType.None;
      set => this._underlineType = value ? ExcelUnderLineType.Single : ExcelUnderLineType.None;
    }

    public ExcelUnderLineType UnderLineType
    {
      get => this._underlineType;
      set => this._underlineType = value;
    }

    public string VerticalAlign
    {
      get => this._verticalAlign;
      set => this._verticalAlign = value;
    }

    public void SetFromFont(Font Font)
    {
      this.Name = Font.Name;
      this.Size = (float) (int) Font.Size;
      this.Strike = Font.Strikeout;
      this.Bold = Font.Bold;
      this.UnderLine = Font.Underline;
      this.Italic = Font.Italic;
    }

    internal ExcelFontXml Copy()
    {
      return new ExcelFontXml(this.NameSpaceManager)
      {
        Name = this.Name,
        Size = this.Size,
        Family = this.Family,
        Scheme = this.Scheme,
        Bold = this.Bold,
        Italic = this.Italic,
        UnderLineType = this.UnderLineType,
        Strike = this.Strike,
        VerticalAlign = this.VerticalAlign,
        Color = this.Color.Copy()
      };
    }

    internal override XmlNode CreateXmlNode(XmlNode topElement)
    {
      this.TopNode = topElement;
      if (this._bold)
        this.CreateNode("d:b");
      else
        this.DeleteAllNode("d:b");
      if (this._italic)
        this.CreateNode("d:i");
      else
        this.DeleteAllNode("d:i");
      if (this._strike)
        this.CreateNode("d:strike");
      else
        this.DeleteAllNode("d:strike");
      if (this._underlineType == ExcelUnderLineType.None)
        this.DeleteAllNode("d:u");
      else if (this._underlineType == ExcelUnderLineType.Single)
      {
        this.CreateNode("d:u");
      }
      else
      {
        string str = this._underlineType.ToString();
        this.SetXmlNodeString("d:u/@val", str.Substring(0, 1).ToLower() + str.Substring(1));
      }
      if (this._verticalAlign != "")
        this.SetXmlNodeString("d:vertAlign/@val", this._verticalAlign.ToString());
      if ((double) this._size > 0.0)
        this.SetXmlNodeString("d:sz/@val", this._size.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (this._color.Exists)
      {
        this.CreateNode("d:color");
        this.TopNode.AppendChild(this._color.CreateXmlNode(this.TopNode.SelectSingleNode("d:color", this.NameSpaceManager)));
      }
      if (!string.IsNullOrEmpty(this._name))
        this.SetXmlNodeString("d:name/@val", this._name);
      if (this._family > int.MinValue)
        this.SetXmlNodeString("d:family/@val", this._family.ToString());
      if (this._scheme != "")
        this.SetXmlNodeString("d:scheme/@val", this._scheme.ToString());
      return this.TopNode;
    }
  }
}
