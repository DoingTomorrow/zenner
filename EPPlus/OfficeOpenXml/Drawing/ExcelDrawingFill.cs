// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.ExcelDrawingFill
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing
{
  public sealed class ExcelDrawingFill : XmlHelper
  {
    private const string ColorPath = "/a:solidFill/a:srgbClr/@val";
    private const string alphaPath = "/a:solidFill/a:srgbClr/a:alpha/@val";
    private string _fillPath;
    private XmlNode _fillNode;
    private eFillStyle _style;
    private XmlNode _fillTypeNode;

    internal ExcelDrawingFill(
      XmlNamespaceManager nameSpaceManager,
      XmlNode topNode,
      string fillPath)
      : base(nameSpaceManager, topNode)
    {
      this._fillPath = fillPath;
      this._fillNode = topNode.SelectSingleNode(this._fillPath, this.NameSpaceManager);
      this.SchemaNodeOrder = new string[16]
      {
        "tickLblPos",
        "spPr",
        "txPr",
        "dLblPos",
        "crossAx",
        "printSettings",
        "showVal",
        "prstGeom",
        "noFill",
        "solidFill",
        "blipFill",
        "gradFill",
        "noFill",
        "pattFill",
        "ln",
        "prstDash"
      };
      if (this._fillNode == null)
        return;
      this._fillTypeNode = topNode.SelectSingleNode("solidFill");
      if (this._fillTypeNode == null)
        this._fillTypeNode = topNode.SelectSingleNode("noFill");
      if (this._fillTypeNode == null)
        this._fillTypeNode = topNode.SelectSingleNode("blipFill");
      if (this._fillTypeNode == null)
        this._fillTypeNode = topNode.SelectSingleNode("gradFill");
      if (this._fillTypeNode != null)
        return;
      this._fillTypeNode = topNode.SelectSingleNode("pattFill");
    }

    public eFillStyle Style
    {
      get
      {
        if (this._fillTypeNode == null)
          return eFillStyle.SolidFill;
        this._style = this.GetStyleEnum(this._fillTypeNode.Name);
        return this._style;
      }
      set
      {
        this._style = value == eFillStyle.NoFill || value == eFillStyle.SolidFill ? value : throw new NotImplementedException("Fillstyle not implemented");
        this.CreateFillTopNode(value);
      }
    }

    private void CreateFillTopNode(eFillStyle value)
    {
      if (this._fillTypeNode != null)
        this.TopNode.RemoveChild(this._fillTypeNode);
      this.CreateNode(this._fillPath + "/a:" + this.GetStyleText(value), false);
      this._fillNode = this.TopNode.SelectSingleNode(this._fillPath + "/a:" + this.GetStyleText(value), this.NameSpaceManager);
    }

    private eFillStyle GetStyleEnum(string name)
    {
      switch (name)
      {
        case "noFill":
          return eFillStyle.NoFill;
        case "blipFill":
          return eFillStyle.BlipFill;
        case "gradFill":
          return eFillStyle.GradientFill;
        case "grpFill":
          return eFillStyle.GroupFill;
        case "pattFill":
          return eFillStyle.PatternFill;
        default:
          return eFillStyle.SolidFill;
      }
    }

    private string GetStyleText(eFillStyle style)
    {
      switch (style)
      {
        case eFillStyle.NoFill:
          return "noFill";
        case eFillStyle.GradientFill:
          return "gradFill";
        case eFillStyle.PatternFill:
          return "pattFill";
        case eFillStyle.BlipFill:
          return "blipFill";
        case eFillStyle.GroupFill:
          return "grpFill";
        default:
          return "solidFill";
      }
    }

    public Color Color
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString(this._fillPath + "/a:solidFill/a:srgbClr/@val");
        return xmlNodeString == "" ? Color.FromArgb(79, 129, 189) : Color.FromArgb(int.Parse(xmlNodeString, NumberStyles.AllowHexSpecifier));
      }
      set
      {
        if (this._fillTypeNode == null)
          this._style = eFillStyle.SolidFill;
        else if (this._style != eFillStyle.SolidFill)
          throw new Exception("FillStyle must be set to SolidFill");
        this.CreateNode(this._fillPath, false);
        this.SetXmlNodeString(this._fillPath + "/a:solidFill/a:srgbClr/@val", value.ToArgb().ToString("X").Substring(2, 6));
      }
    }

    public int Transparancy
    {
      get
      {
        return 100 - this.GetXmlNodeInt(this._fillPath + "/a:solidFill/a:srgbClr/a:alpha/@val") / 1000;
      }
      set
      {
        if (this._fillTypeNode == null)
        {
          this._style = eFillStyle.SolidFill;
          this.Color = Color.FromArgb(79, 129, 189);
        }
        else if (this._style != eFillStyle.SolidFill)
          throw new Exception("FillStyle must be set to SolidFill");
        this.SetXmlNodeString(this._fillPath + "/a:solidFill/a:srgbClr/a:alpha/@val", ((100 - value) * 1000).ToString());
      }
    }
  }
}
