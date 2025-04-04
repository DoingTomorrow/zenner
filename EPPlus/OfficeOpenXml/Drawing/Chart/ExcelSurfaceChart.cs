// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelSurfaceChart
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Chart
{
  public sealed class ExcelSurfaceChart : ExcelChart
  {
    private const string WIREFRAME_PATH = "c:wireframe/@val";
    private ExcelChartSurface _floor;
    private ExcelChartSurface _sideWall;
    private ExcelChartSurface _backWall;

    internal ExcelSurfaceChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
      this.Init();
    }

    internal ExcelSurfaceChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, uriChart, part, chartXml, chartNode)
    {
      this.Init();
    }

    internal ExcelSurfaceChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
      this.Init();
    }

    private void Init()
    {
      this._floor = new ExcelChartSurface(this.NameSpaceManager, this._chartXmlHelper.TopNode.SelectSingleNode("c:floor", this.NameSpaceManager));
      this._backWall = new ExcelChartSurface(this.NameSpaceManager, this._chartXmlHelper.TopNode.SelectSingleNode("c:sideWall", this.NameSpaceManager));
      this._sideWall = new ExcelChartSurface(this.NameSpaceManager, this._chartXmlHelper.TopNode.SelectSingleNode("c:backWall", this.NameSpaceManager));
      this.SetTypeProperties();
    }

    public new ExcelView3D View3D
    {
      get
      {
        if (this.IsType3D())
          return new ExcelView3D(this.NameSpaceManager, this.ChartXml.SelectSingleNode("//c:view3D", this.NameSpaceManager));
        throw new Exception("Charttype does not support 3D");
      }
    }

    public ExcelChartSurface Floor => this._floor;

    public ExcelChartSurface SideWall => this._sideWall;

    public ExcelChartSurface BackWall => this._backWall;

    public bool Wireframe
    {
      get => this._chartXmlHelper.GetXmlNodeBool("c:wireframe/@val");
      set => this._chartXmlHelper.SetXmlNodeBool("c:wireframe/@val", value);
    }

    internal void SetTypeProperties()
    {
      this.Wireframe = this.ChartType == eChartType.SurfaceWireframe || this.ChartType == eChartType.SurfaceTopViewWireframe;
      if (this.ChartType == eChartType.SurfaceTopView || this.ChartType == eChartType.SurfaceTopViewWireframe)
      {
        this.View3D.RotY = 0M;
        this.View3D.RotX = 90M;
      }
      else
      {
        this.View3D.RotY = 20M;
        this.View3D.RotX = 15M;
      }
      this.View3D.RightAngleAxes = false;
      this.View3D.Perspective = 0M;
      this.Axis[1].CrossBetween = eCrossBetween.MidCat;
    }

    internal override eChartType GetChartType(string name)
    {
      return this.Wireframe ? (name == "surfaceChart" ? eChartType.SurfaceTopViewWireframe : eChartType.SurfaceWireframe) : (name == "surfaceChart" ? eChartType.SurfaceTopView : eChartType.Surface);
    }
  }
}
