// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelFillXml
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public class ExcelFillXml : StyleXmlHelper
  {
    private const string fillPatternTypePath = "d:patternFill/@patternType";
    private const string _patternColorPath = "d:patternFill/d:bgColor";
    private const string _backgroundColorPath = "d:patternFill/d:fgColor";
    protected ExcelFillStyle _fillPatternType;
    protected ExcelColorXml _patternColor;
    protected ExcelColorXml _backgroundColor;

    internal ExcelFillXml(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
      this._fillPatternType = ExcelFillStyle.None;
      this._backgroundColor = new ExcelColorXml(this.NameSpaceManager);
      this._patternColor = new ExcelColorXml(this.NameSpaceManager);
    }

    internal ExcelFillXml(XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      this.PatternType = this.GetPatternType(this.GetXmlNodeString("d:patternFill/@patternType"));
      this._backgroundColor = new ExcelColorXml(nsm, topNode.SelectSingleNode("d:patternFill/d:fgColor", nsm));
      this._patternColor = new ExcelColorXml(nsm, topNode.SelectSingleNode("d:patternFill/d:bgColor", nsm));
    }

    private ExcelFillStyle GetPatternType(string patternType)
    {
      if (patternType == "")
        return ExcelFillStyle.None;
      patternType = patternType.Substring(0, 1).ToUpper() + patternType.Substring(1, patternType.Length - 1);
      try
      {
        return (ExcelFillStyle) Enum.Parse(typeof (ExcelFillStyle), patternType);
      }
      catch
      {
        return ExcelFillStyle.None;
      }
    }

    internal override string Id
    {
      get => this.PatternType.ToString() + this.PatternColor.Id + this.BackgroundColor.Id;
    }

    public ExcelFillStyle PatternType
    {
      get => this._fillPatternType;
      set => this._fillPatternType = value;
    }

    public ExcelColorXml PatternColor
    {
      get => this._patternColor;
      internal set => this._patternColor = value;
    }

    public ExcelColorXml BackgroundColor
    {
      get => this._backgroundColor;
      internal set => this._backgroundColor = value;
    }

    internal virtual ExcelFillXml Copy()
    {
      return new ExcelFillXml(this.NameSpaceManager)
      {
        PatternType = this._fillPatternType,
        BackgroundColor = this._backgroundColor.Copy(),
        PatternColor = this._patternColor.Copy()
      };
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode)
    {
      this.TopNode = topNode;
      this.SetXmlNodeString("d:patternFill/@patternType", this.SetPatternString(this._fillPatternType));
      if (this.PatternType != ExcelFillStyle.None)
      {
        topNode.SelectSingleNode("d:patternFill/@patternType", this.NameSpaceManager);
        if (this.BackgroundColor.Exists)
        {
          this.CreateNode("d:patternFill/d:fgColor");
          this.BackgroundColor.CreateXmlNode(topNode.SelectSingleNode("d:patternFill/d:fgColor", this.NameSpaceManager));
          if (this.PatternColor.Exists)
          {
            this.CreateNode("d:patternFill/d:bgColor");
            this.PatternColor.CreateXmlNode(topNode.SelectSingleNode("d:patternFill/d:bgColor", this.NameSpaceManager));
          }
        }
      }
      return topNode;
    }

    private string SetPatternString(ExcelFillStyle pattern)
    {
      string name = Enum.GetName(typeof (ExcelFillStyle), (object) pattern);
      return name.Substring(0, 1).ToLower() + name.Substring(1, name.Length - 1);
    }
  }
}
