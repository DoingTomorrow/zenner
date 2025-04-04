// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelColorXml
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
  public sealed class ExcelColorXml : StyleXmlHelper
  {
    private bool _auto;
    private string _theme;
    private Decimal _tint;
    private string _rgb;
    private int _indexed;
    private bool _exists;

    internal ExcelColorXml(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
      this._auto = false;
      this._theme = "";
      this._tint = 0M;
      this._rgb = "";
      this._indexed = int.MinValue;
    }

    internal ExcelColorXml(XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      if (topNode == null)
      {
        this._exists = false;
      }
      else
      {
        this._exists = true;
        this._auto = this.GetXmlNodeBool("@auto");
        this._theme = this.GetXmlNodeString("@theme");
        this._tint = this.GetXmlNodeDecimal("@tint");
        this._rgb = this.GetXmlNodeString("@rgb");
        this._indexed = this.GetXmlNodeInt("@indexed");
      }
    }

    internal override string Id
    {
      get
      {
        return this._auto.ToString() + "|" + this._theme + "|" + (object) this._tint + "|" + this._rgb + "|" + (object) this._indexed;
      }
    }

    public bool Auto
    {
      get => this._auto;
      set
      {
        int num = value ? 1 : 0;
        this._auto = value;
        this._exists = true;
        this.Clear();
      }
    }

    public string Theme => this._theme;

    public Decimal Tint
    {
      get => this._tint;
      set
      {
        this._tint = value;
        this._exists = true;
      }
    }

    public string Rgb
    {
      get => this._rgb;
      set
      {
        this._rgb = value;
        this._exists = true;
        this._indexed = int.MinValue;
        this._auto = false;
      }
    }

    public int Indexed
    {
      get => this._indexed;
      set
      {
        if (value < 0 || value > 65)
          throw new ArgumentOutOfRangeException("Index out of range");
        this.Clear();
        this._indexed = value;
        this._exists = true;
      }
    }

    internal void Clear()
    {
      this._theme = "";
      this._tint = Decimal.MaxValue;
      this._indexed = int.MinValue;
      this._rgb = "";
      this._auto = false;
    }

    public void SetColor(Color color)
    {
      this.Clear();
      this._rgb = color.ToArgb().ToString("X");
    }

    internal ExcelColorXml Copy()
    {
      return new ExcelColorXml(this.NameSpaceManager)
      {
        _indexed = this.Indexed,
        _tint = this.Tint,
        _rgb = this.Rgb,
        _theme = this.Theme,
        _auto = this.Auto,
        _exists = this.Exists
      };
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode)
    {
      this.TopNode = topNode;
      if (this._rgb != "")
        this.SetXmlNodeString("@rgb", this._rgb);
      else if (this._indexed >= 0)
        this.SetXmlNodeString("@indexed", this._indexed.ToString());
      else if (this._auto)
        this.SetXmlNodeBool("@auto", this._auto);
      else
        this.SetXmlNodeString("@theme", this._theme.ToString());
      if (this._tint != Decimal.MaxValue)
        this.SetXmlNodeString("@tint", this._tint.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return this.TopNode;
    }

    internal bool Exists => this._exists;
  }
}
