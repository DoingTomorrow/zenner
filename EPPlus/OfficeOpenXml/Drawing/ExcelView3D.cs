// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.ExcelView3D
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing
{
  public sealed class ExcelView3D : XmlHelper
  {
    private const string perspectivePath = "c:perspective/@val";
    private const string rotXPath = "c:rotX/@val";
    private const string rotYPath = "c:rotY/@val";
    private const string rAngAxPath = "c:rAngAx/@val";
    private const string depthPercentPath = "c:depthPercent/@val";
    private const string heightPercentPath = "c:hPercent/@val";

    internal ExcelView3D(XmlNamespaceManager ns, XmlNode node)
      : base(ns, node)
    {
      this.SchemaNodeOrder = new string[6]
      {
        "rotX",
        "hPercent",
        "rotY",
        "depthPercent",
        "rAngAx",
        "perspective"
      };
    }

    public Decimal Perspective
    {
      get => (Decimal) this.GetXmlNodeInt("c:perspective/@val");
      set
      {
        this.SetXmlNodeString("c:perspective/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal RotX
    {
      get => this.GetXmlNodeDecimal("c:rotX/@val");
      set
      {
        this.CreateNode("c:rotX/@val");
        this.SetXmlNodeString("c:rotX/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public Decimal RotY
    {
      get => this.GetXmlNodeDecimal("c:rotY/@val");
      set
      {
        this.CreateNode("c:rotY/@val");
        this.SetXmlNodeString("c:rotY/@val", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }

    public bool RightAngleAxes
    {
      get => this.GetXmlNodeBool("c:rAngAx/@val");
      set => this.SetXmlNodeBool("c:rAngAx/@val", value);
    }

    public int DepthPercent
    {
      get => this.GetXmlNodeInt("c:depthPercent/@val");
      set
      {
        if (value < 0 || value > 2000)
          throw new ArgumentOutOfRangeException("Value must be between 0 and 2000");
        this.SetXmlNodeString("c:depthPercent/@val", value.ToString());
      }
    }

    public int HeightPercent
    {
      get => this.GetXmlNodeInt("c:hPercent/@val");
      set
      {
        if (value < 5 || value > 500)
          throw new ArgumentOutOfRangeException("Value must be between 5 and 500");
        this.SetXmlNodeString("c:hPercent/@val", value.ToString());
      }
    }
  }
}
