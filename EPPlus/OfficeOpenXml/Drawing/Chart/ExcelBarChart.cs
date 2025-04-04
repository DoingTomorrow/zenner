// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Chart.ExcelBarChart
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
  public sealed class ExcelBarChart : ExcelChart
  {
    private string _directionPath = "c:barDir/@val";
    private string _shapePath = "c:shape/@val";
    private ExcelChartDataLabel _DataLabel;

    internal ExcelBarChart(
      ExcelDrawings drawings,
      XmlNode node,
      eChartType type,
      ExcelChart topChart,
      ExcelPivotTable PivotTableSource)
      : base(drawings, node, type, topChart, PivotTableSource)
    {
      this.SetChartNodeText("");
      this.SetTypeProperties(drawings, type);
    }

    internal ExcelBarChart(
      ExcelDrawings drawings,
      XmlNode node,
      Uri uriChart,
      ZipPackagePart part,
      XmlDocument chartXml,
      XmlNode chartNode)
      : base(drawings, node, uriChart, part, chartXml, chartNode)
    {
      this.SetChartNodeText(chartNode.Name);
    }

    internal ExcelBarChart(ExcelChart topChart, XmlNode chartNode)
      : base(topChart, chartNode)
    {
      this.SetChartNodeText(chartNode.Name);
    }

    private void SetChartNodeText(string chartNodeText)
    {
      if (!string.IsNullOrEmpty(chartNodeText))
        return;
      chartNodeText = this.GetChartNodeText();
    }

    private void SetTypeProperties(ExcelDrawings drawings, eChartType type)
    {
      if (type == eChartType.BarClustered || type == eChartType.BarStacked || type == eChartType.BarStacked100 || type == eChartType.BarClustered3D || type == eChartType.BarStacked3D || type == eChartType.BarStacked1003D || type == eChartType.ConeBarClustered || type == eChartType.ConeBarStacked || type == eChartType.ConeBarStacked100 || type == eChartType.CylinderBarClustered || type == eChartType.CylinderBarStacked || type == eChartType.CylinderBarStacked100 || type == eChartType.PyramidBarClustered || type == eChartType.PyramidBarStacked || type == eChartType.PyramidBarStacked100)
        this.Direction = eDirection.Bar;
      else if (type == eChartType.ColumnClustered || type == eChartType.ColumnStacked || type == eChartType.ColumnStacked100 || type == eChartType.Column3D || type == eChartType.ColumnClustered3D || type == eChartType.ColumnStacked3D || type == eChartType.ColumnStacked1003D || type == eChartType.ConeCol || type == eChartType.ConeColClustered || type == eChartType.ConeColStacked || type == eChartType.ConeColStacked100 || type == eChartType.CylinderCol || type == eChartType.CylinderColClustered || type == eChartType.CylinderColStacked || type == eChartType.CylinderColStacked100 || type == eChartType.PyramidCol || type == eChartType.PyramidColClustered || type == eChartType.PyramidColStacked || type == eChartType.PyramidColStacked100)
        this.Direction = eDirection.Column;
      if (type == eChartType.Column3D || type == eChartType.ColumnClustered3D || type == eChartType.ColumnStacked3D || type == eChartType.ColumnStacked1003D || type == eChartType.BarClustered3D || type == eChartType.BarStacked3D || type == eChartType.BarStacked1003D)
        this.Shape = eShape.Box;
      else if (type == eChartType.CylinderBarClustered || type == eChartType.CylinderBarStacked || type == eChartType.CylinderBarStacked100 || type == eChartType.CylinderCol || type == eChartType.CylinderColClustered || type == eChartType.CylinderColStacked || type == eChartType.CylinderColStacked100)
        this.Shape = eShape.Cylinder;
      else if (type == eChartType.ConeBarClustered || type == eChartType.ConeBarStacked || type == eChartType.ConeBarStacked100 || type == eChartType.ConeCol || type == eChartType.ConeColClustered || type == eChartType.ConeColStacked || type == eChartType.ConeColStacked100)
      {
        this.Shape = eShape.Cone;
      }
      else
      {
        if (type != eChartType.PyramidBarClustered && type != eChartType.PyramidBarStacked && type != eChartType.PyramidBarStacked100 && type != eChartType.PyramidCol && type != eChartType.PyramidColClustered && type != eChartType.PyramidColStacked && type != eChartType.PyramidColStacked100)
          return;
        this.Shape = eShape.Pyramid;
      }
    }

    public eDirection Direction
    {
      get => this.GetDirectionEnum(this._chartXmlHelper.GetXmlNodeString(this._directionPath));
      internal set
      {
        this._chartXmlHelper.SetXmlNodeString(this._directionPath, this.GetDirectionText(value));
      }
    }

    public eShape Shape
    {
      get => this.GetShapeEnum(this._chartXmlHelper.GetXmlNodeString(this._shapePath));
      internal set
      {
        this._chartXmlHelper.SetXmlNodeString(this._shapePath, this.GetShapeText(value));
      }
    }

    public ExcelChartDataLabel DataLabel
    {
      get
      {
        if (this._DataLabel == null)
          this._DataLabel = new ExcelChartDataLabel(this.NameSpaceManager, this.ChartNode);
        return this._DataLabel;
      }
    }

    private string GetDirectionText(eDirection direction)
    {
      return direction == eDirection.Bar ? "bar" : "col";
    }

    private eDirection GetDirectionEnum(string direction)
    {
      switch (direction)
      {
        case "bar":
          return eDirection.Bar;
        default:
          return eDirection.Column;
      }
    }

    private string GetShapeText(eShape Shape)
    {
      switch (Shape)
      {
        case eShape.Box:
          return "box";
        case eShape.Cone:
          return "cone";
        case eShape.ConeToMax:
          return "coneToMax";
        case eShape.Cylinder:
          return "cylinder";
        case eShape.Pyramid:
          return "pyramid";
        case eShape.PyramidToMax:
          return "pyramidToMax";
        default:
          return "box";
      }
    }

    private eShape GetShapeEnum(string text)
    {
      switch (text)
      {
        case "box":
          return eShape.Box;
        case "cone":
          return eShape.Cone;
        case "coneToMax":
          return eShape.ConeToMax;
        case "cylinder":
          return eShape.Cylinder;
        case "pyramid":
          return eShape.Pyramid;
        case "pyramidToMax":
          return eShape.PyramidToMax;
        default:
          return eShape.Box;
      }
    }

    internal override eChartType GetChartType(string name)
    {
      switch (name)
      {
        case "barChart":
          if (this.Direction == eDirection.Bar)
          {
            if (this.Grouping == eGrouping.Stacked)
              return eChartType.BarStacked;
            return this.Grouping == eGrouping.PercentStacked ? eChartType.BarStacked100 : eChartType.BarClustered;
          }
          if (this.Grouping == eGrouping.Stacked)
            return eChartType.ColumnStacked;
          return this.Grouping == eGrouping.PercentStacked ? eChartType.ColumnStacked100 : eChartType.ColumnClustered;
        case "bar3DChart":
          if (this.Shape == eShape.Box)
          {
            if (this.Direction == eDirection.Bar)
            {
              if (this.Grouping == eGrouping.Stacked)
                return eChartType.BarStacked3D;
              return this.Grouping == eGrouping.PercentStacked ? eChartType.BarStacked1003D : eChartType.BarClustered3D;
            }
            if (this.Grouping == eGrouping.Stacked)
              return eChartType.ColumnStacked3D;
            return this.Grouping == eGrouping.PercentStacked ? eChartType.ColumnStacked1003D : eChartType.ColumnClustered3D;
          }
          if (this.Shape == eShape.Cone || this.Shape == eShape.ConeToMax)
          {
            if (this.Direction == eDirection.Bar)
            {
              if (this.Grouping == eGrouping.Stacked)
                return eChartType.ConeBarStacked;
              if (this.Grouping == eGrouping.PercentStacked)
                return eChartType.ConeBarStacked100;
              if (this.Grouping == eGrouping.Clustered)
                return eChartType.ConeBarClustered;
            }
            else
            {
              if (this.Grouping == eGrouping.Stacked)
                return eChartType.ConeColStacked;
              if (this.Grouping == eGrouping.PercentStacked)
                return eChartType.ConeColStacked100;
              return this.Grouping == eGrouping.Clustered ? eChartType.ConeColClustered : eChartType.ConeCol;
            }
          }
          if (this.Shape == eShape.Cylinder)
          {
            if (this.Direction == eDirection.Bar)
            {
              if (this.Grouping == eGrouping.Stacked)
                return eChartType.CylinderBarStacked;
              if (this.Grouping == eGrouping.PercentStacked)
                return eChartType.CylinderBarStacked100;
              if (this.Grouping == eGrouping.Clustered)
                return eChartType.CylinderBarClustered;
            }
            else
            {
              if (this.Grouping == eGrouping.Stacked)
                return eChartType.CylinderColStacked;
              if (this.Grouping == eGrouping.PercentStacked)
                return eChartType.CylinderColStacked100;
              return this.Grouping == eGrouping.Clustered ? eChartType.CylinderColClustered : eChartType.CylinderCol;
            }
          }
          if (this.Shape == eShape.Pyramid || this.Shape == eShape.PyramidToMax)
          {
            if (this.Direction == eDirection.Bar)
            {
              if (this.Grouping == eGrouping.Stacked)
                return eChartType.PyramidBarStacked;
              if (this.Grouping == eGrouping.PercentStacked)
                return eChartType.PyramidBarStacked100;
              if (this.Grouping == eGrouping.Clustered)
                return eChartType.PyramidBarClustered;
              break;
            }
            if (this.Grouping == eGrouping.Stacked)
              return eChartType.PyramidColStacked;
            if (this.Grouping == eGrouping.PercentStacked)
              return eChartType.PyramidColStacked100;
            return this.Grouping == eGrouping.Clustered ? eChartType.PyramidColClustered : eChartType.PyramidCol;
          }
          break;
      }
      return base.GetChartType(name);
    }
  }
}
