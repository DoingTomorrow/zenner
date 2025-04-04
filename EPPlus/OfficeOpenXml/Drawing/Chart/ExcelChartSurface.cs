// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelChartSurface
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public class ExcelChartSurface : XmlHelper
  {
    private const string THICKNESS_PATH = "c:thickness/@val";
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;

    internal ExcelChartSurface(XmlNamespaceManager ns, XmlNode node)
      : base(ns, node)
    {
      this.SchemaNodeOrder = new string[3]
      {
        "thickness",
        "spPr",
        "pictureOptions"
      };
    }

    public int Thickness
    {
      get => this.GetXmlNodeInt("c:thickness/@val");
      set
      {
        if (value < 0 && value > 9)
          throw new ArgumentOutOfRangeException("Thickness out of range. (0-9)");
        this.SetXmlNodeString("c:thickness/@val", value.ToString());
      }
    }

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, "c:spPr");
        return this._fill;
      }
    }

    public ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.TopNode, "c:spPr/a:ln");
        return this._border;
      }
    }
  }
}
