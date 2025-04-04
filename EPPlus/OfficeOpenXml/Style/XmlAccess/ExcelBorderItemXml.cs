// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelBorderItemXml
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public sealed class ExcelBorderItemXml : StyleXmlHelper
  {
    private const string _colorPath = "d:color";
    private ExcelBorderStyle _borderStyle;
    private ExcelColorXml _color;

    internal ExcelBorderItemXml(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
      this._borderStyle = ExcelBorderStyle.None;
      this._color = new ExcelColorXml(this.NameSpaceManager);
    }

    internal ExcelBorderItemXml(XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      if (topNode != null)
      {
        this._borderStyle = this.GetBorderStyle(this.GetXmlNodeString("@style"));
        this._color = new ExcelColorXml(nsm, topNode.SelectSingleNode("d:color", nsm));
        this.Exists = true;
      }
      else
        this.Exists = false;
    }

    private ExcelBorderStyle GetBorderStyle(string style)
    {
      if (style == "")
        return ExcelBorderStyle.None;
      string str = style.Substring(0, 1).ToUpper() + style.Substring(1, style.Length - 1);
      try
      {
        return (ExcelBorderStyle) Enum.Parse(typeof (ExcelBorderStyle), str);
      }
      catch
      {
        return ExcelBorderStyle.None;
      }
    }

    public ExcelBorderStyle Style
    {
      get => this._borderStyle;
      set
      {
        this._borderStyle = value;
        this.Exists = true;
      }
    }

    public ExcelColorXml Color
    {
      get => this._color;
      internal set => this._color = value;
    }

    internal override string Id => this.Exists ? this.Style.ToString() + this.Color.Id : "None";

    internal ExcelBorderItemXml Copy()
    {
      return new ExcelBorderItemXml(this.NameSpaceManager)
      {
        Style = this._borderStyle,
        Color = this._color.Copy()
      };
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode)
    {
      this.TopNode = topNode;
      if (this.Style != ExcelBorderStyle.None)
      {
        this.SetXmlNodeString("@style", this.SetBorderString(this.Style));
        if (this.Color.Exists)
        {
          this.CreateNode("d:color");
          topNode.AppendChild(this.Color.CreateXmlNode(this.TopNode.SelectSingleNode("d:color", this.NameSpaceManager)));
        }
      }
      return this.TopNode;
    }

    private string SetBorderString(ExcelBorderStyle Style)
    {
      string name = Enum.GetName(typeof (ExcelBorderStyle), (object) Style);
      return name.Substring(0, 1).ToLower() + name.Substring(1, name.Length - 1);
    }

    public bool Exists { get; private set; }
  }
}
