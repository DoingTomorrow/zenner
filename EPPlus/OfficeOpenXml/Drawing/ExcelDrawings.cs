// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.ExcelDrawings
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Table.PivotTable;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing
{
  public class ExcelDrawings : IEnumerable<ExcelDrawing>, IEnumerable, IDisposable
  {
    private XmlDocument _drawingsXml = new XmlDocument();
    private Dictionary<string, int> _drawingNames;
    private List<ExcelDrawing> _drawings;
    internal Dictionary<string, string> _hashes = new Dictionary<string, string>();
    internal ExcelPackage _package;
    internal ZipPackageRelationship _drawingRelation;
    private XmlNamespaceManager _nsManager;
    private ZipPackagePart _part;
    private Uri _uriDrawing;

    internal ExcelDrawings(ExcelPackage xlPackage, ExcelWorksheet sheet)
    {
      this._drawingsXml = new XmlDocument();
      this._drawingsXml.PreserveWhitespace = false;
      this._drawings = new List<ExcelDrawing>();
      this._drawingNames = new Dictionary<string, int>();
      this._package = xlPackage;
      this.Worksheet = sheet;
      XmlNode xmlNode = sheet.WorksheetXml.SelectSingleNode("//d:drawing", sheet.NameSpaceManager);
      this.CreateNSM();
      if (xmlNode == null)
        return;
      this._drawingRelation = sheet.Part.GetRelationship(xmlNode.Attributes["r:id"].Value);
      this._uriDrawing = UriHelper.ResolvePartUri(sheet.WorksheetUri, this._drawingRelation.TargetUri);
      this._part = xlPackage.Package.GetPart(this._uriDrawing);
      XmlHelper.LoadXmlSafe(this._drawingsXml, (Stream) this._part.GetStream());
      this.AddDrawings();
    }

    internal ExcelWorksheet Worksheet { get; set; }

    public XmlDocument DrawingXml => this._drawingsXml;

    private void AddDrawings()
    {
      foreach (XmlNode childNode in this._drawingsXml.DocumentElement.ChildNodes)
      {
        ExcelDrawing excelDrawing;
        switch (childNode.LocalName)
        {
          case "oneCellAnchor":
            excelDrawing = new ExcelDrawing(this, childNode, "xdr:sp/xdr:nvSpPr/xdr:cNvPr/@name");
            break;
          case "twoCellAnchor":
            excelDrawing = ExcelDrawing.GetDrawing(this, childNode);
            break;
          case "absoluteAnchor":
            excelDrawing = ExcelDrawing.GetDrawing(this, childNode);
            break;
          default:
            excelDrawing = (ExcelDrawing) null;
            break;
        }
        if (excelDrawing != null)
        {
          this._drawings.Add(excelDrawing);
          if (!this._drawingNames.ContainsKey(excelDrawing.Name.ToLower()))
            this._drawingNames.Add(excelDrawing.Name.ToLower(), this._drawings.Count - 1);
        }
      }
    }

    private void CreateNSM()
    {
      this._nsManager = new XmlNamespaceManager((XmlNameTable) new NameTable());
      this._nsManager.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
      this._nsManager.AddNamespace("xdr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      this._nsManager.AddNamespace("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      this._nsManager.AddNamespace("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    }

    public XmlNamespaceManager NameSpaceManager => this._nsManager;

    public IEnumerator GetEnumerator() => (IEnumerator) this._drawings.GetEnumerator();

    IEnumerator<ExcelDrawing> IEnumerable<ExcelDrawing>.GetEnumerator()
    {
      return (IEnumerator<ExcelDrawing>) this._drawings.GetEnumerator();
    }

    public ExcelDrawing this[int PositionID] => this._drawings[PositionID];

    public ExcelDrawing this[string Name]
    {
      get
      {
        return this._drawingNames.ContainsKey(Name.ToLower()) ? this._drawings[this._drawingNames[Name.ToLower()]] : (ExcelDrawing) null;
      }
    }

    public int Count => this._drawings == null ? 0 : this._drawings.Count;

    internal ZipPackagePart Part => this._part;

    public Uri UriDrawing => this._uriDrawing;

    public ExcelChart AddChart(string Name, eChartType ChartType, ExcelPivotTable PivotTableSource)
    {
      if (this._drawingNames.ContainsKey(Name.ToLower()))
        throw new Exception("Name already exists in the drawings collection");
      if (ChartType == eChartType.StockHLC || ChartType == eChartType.StockOHLC || ChartType == eChartType.StockVOHLC)
        throw new NotImplementedException("Chart type is not supported in the current version");
      if (this.Worksheet is ExcelChartsheet && this._drawings.Count > 0)
        throw new InvalidOperationException("Chart Worksheets can't have more than one chart");
      ExcelChart newChart = ExcelChart.GetNewChart(this, (XmlNode) this.CreateDrawingXml(), ChartType, (ExcelChart) null, PivotTableSource);
      newChart.Name = Name;
      this._drawings.Add((ExcelDrawing) newChart);
      this._drawingNames.Add(Name.ToLower(), this._drawings.Count - 1);
      return newChart;
    }

    public ExcelChart AddChart(string Name, eChartType ChartType)
    {
      return this.AddChart(Name, ChartType, (ExcelPivotTable) null);
    }

    public ExcelPicture AddPicture(string Name, Image image)
    {
      return this.AddPicture(Name, image, (Uri) null);
    }

    public ExcelPicture AddPicture(string Name, Image image, Uri Hyperlink)
    {
      if (image == null)
        throw new Exception("AddPicture: Image can't be null");
      if (this._drawingNames.ContainsKey(Name.ToLower()))
        throw new Exception("Name already exists in the drawings collection");
      XmlElement drawingXml = this.CreateDrawingXml();
      drawingXml.SetAttribute("editAs", "oneCell");
      ExcelPicture excelPicture = new ExcelPicture(this, (XmlNode) drawingXml, image, Hyperlink);
      excelPicture.Name = Name;
      this._drawings.Add((ExcelDrawing) excelPicture);
      this._drawingNames.Add(Name.ToLower(), this._drawings.Count - 1);
      return excelPicture;
    }

    public ExcelPicture AddPicture(string Name, FileInfo ImageFile)
    {
      return this.AddPicture(Name, ImageFile, (Uri) null);
    }

    public ExcelPicture AddPicture(string Name, FileInfo ImageFile, Uri Hyperlink)
    {
      if (this.Worksheet is ExcelChartsheet && this._drawings.Count > 0)
        throw new InvalidOperationException("Chart worksheets can't have more than one drawing");
      if (ImageFile == null)
        throw new Exception("AddPicture: ImageFile can't be null");
      if (this._drawingNames.ContainsKey(Name.ToLower()))
        throw new Exception("Name already exists in the drawings collection");
      XmlElement drawingXml = this.CreateDrawingXml();
      drawingXml.SetAttribute("editAs", "oneCell");
      ExcelPicture excelPicture = new ExcelPicture(this, (XmlNode) drawingXml, ImageFile, Hyperlink);
      excelPicture.Name = Name;
      this._drawings.Add((ExcelDrawing) excelPicture);
      this._drawingNames.Add(Name.ToLower(), this._drawings.Count - 1);
      return excelPicture;
    }

    public ExcelShape AddShape(string Name, eShapeStyle Style)
    {
      if (this.Worksheet is ExcelChartsheet && this._drawings.Count > 0)
        throw new InvalidOperationException("Chart worksheets can't have more than one drawing");
      if (this._drawingNames.ContainsKey(Name.ToLower()))
        throw new Exception("Name already exists in the drawings collection");
      ExcelShape excelShape = new ExcelShape(this, (XmlNode) this.CreateDrawingXml(), Style);
      excelShape.Name = Name;
      excelShape.Style = Style;
      this._drawings.Add((ExcelDrawing) excelShape);
      this._drawingNames.Add(Name.ToLower(), this._drawings.Count - 1);
      return excelShape;
    }

    private XmlElement CreateDrawingXml()
    {
      if (this.DrawingXml.OuterXml == "")
      {
        this.DrawingXml.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><xdr:wsDr xmlns:xdr=\"{0}\" xmlns:a=\"{1}\" />", (object) "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", (object) "http://schemas.openxmlformats.org/drawingml/2006/main"));
        this._uriDrawing = new Uri(string.Format("/xl/drawings/drawing{0}.xml", (object) this.Worksheet.SheetID), UriKind.Relative);
        ZipPackage package = this.Worksheet._package.Package;
        this._part = package.CreatePart(this._uriDrawing, "application/vnd.openxmlformats-officedocument.drawing+xml", this._package.Compression);
        StreamWriter writer = new StreamWriter((Stream) this._part.GetStream(FileMode.Create, FileAccess.Write));
        this.DrawingXml.Save((TextWriter) writer);
        writer.Close();
        package.Flush();
        this._drawingRelation = this.Worksheet.Part.CreateRelationship(UriHelper.GetRelativeUri(this.Worksheet.WorksheetUri, this._uriDrawing), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing");
        XmlElement element = this.Worksheet.WorksheetXml.CreateElement("drawing", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element.SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", this._drawingRelation.Id);
        this.Worksheet.WorksheetXml.DocumentElement.AppendChild((XmlNode) element);
        package.Flush();
      }
      XmlNode xmlNode = this._drawingsXml.SelectSingleNode("//xdr:wsDr", this.NameSpaceManager);
      XmlElement element1;
      if (this.Worksheet is ExcelChartsheet)
      {
        element1 = this._drawingsXml.CreateElement("xdr", "absoluteAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        XmlElement element2 = this._drawingsXml.CreateElement("xdr", "pos", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        element2.SetAttribute("y", "0");
        element2.SetAttribute("x", "0");
        element1.AppendChild((XmlNode) element2);
        XmlElement element3 = this._drawingsXml.CreateElement("xdr", "ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        element3.SetAttribute("cy", "6072876");
        element3.SetAttribute("cx", "9299263");
        element1.AppendChild((XmlNode) element3);
        xmlNode.AppendChild((XmlNode) element1);
      }
      else
      {
        element1 = this._drawingsXml.CreateElement("xdr", "twoCellAnchor", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        xmlNode.AppendChild((XmlNode) element1);
        XmlElement element4 = this._drawingsXml.CreateElement("xdr", "from", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        element1.AppendChild((XmlNode) element4);
        element4.InnerXml = "<xdr:col>0</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>0</xdr:row><xdr:rowOff>0</xdr:rowOff>";
        XmlElement element5 = this._drawingsXml.CreateElement("xdr", "to", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
        element1.AppendChild((XmlNode) element5);
        element5.InnerXml = "<xdr:col>10</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>10</xdr:row><xdr:rowOff>0</xdr:rowOff>";
      }
      return element1;
    }

    public void Remove(int Index)
    {
      if (this.Worksheet is ExcelChartsheet && this._drawings.Count > 0)
        throw new InvalidOperationException("Can' remove charts from chart worksheets");
      this.RemoveDrawing(Index);
    }

    internal void RemoveDrawing(int Index)
    {
      ExcelDrawing drawing = this._drawings[Index];
      drawing.DeleteMe();
      for (int index = Index + 1; index < this._drawings.Count; ++index)
      {
        Dictionary<string, int> drawingNames;
        string lower;
        (drawingNames = this._drawingNames)[lower = this._drawings[index].Name.ToLower()] = drawingNames[lower] - 1;
      }
      this._drawingNames.Remove(drawing.Name.ToLower());
      this._drawings.Remove(drawing);
    }

    public void Remove(ExcelDrawing Drawing)
    {
      this.Remove(this._drawingNames[Drawing.Name.ToLower()]);
    }

    public void Remove(string Name) => this.Remove(this._drawingNames[Name.ToLower()]);

    public void Clear()
    {
      if (this.Worksheet is ExcelChartsheet && this._drawings.Count > 0)
        throw new InvalidOperationException("Can' remove charts from chart worksheets");
      this.ClearDrawings();
    }

    internal void ClearDrawings()
    {
      while (this.Count > 0)
        this.RemoveDrawing(0);
    }

    internal void AdjustWidth(int[,] pos)
    {
      int index = 0;
      foreach (ExcelDrawing excelDrawing in this)
      {
        if (excelDrawing.EditAs != eEditAs.TwoCell)
        {
          if (excelDrawing.EditAs == eEditAs.Absolute)
            excelDrawing.SetPixelLeft(pos[index, 0]);
          excelDrawing.SetPixelWidth(pos[index, 1]);
        }
        ++index;
      }
    }

    internal void AdjustHeight(int[,] pos)
    {
      int index = 0;
      foreach (ExcelDrawing excelDrawing in this)
      {
        if (excelDrawing.EditAs != eEditAs.TwoCell)
        {
          if (excelDrawing.EditAs == eEditAs.Absolute)
            excelDrawing.SetPixelTop(pos[index, 0]);
          excelDrawing.SetPixelHeight(pos[index, 1]);
        }
        ++index;
      }
    }

    internal int[,] GetDrawingWidths()
    {
      int[,] drawingWidths = new int[this.Count, 2];
      int index = 0;
      foreach (ExcelDrawing excelDrawing in this)
      {
        drawingWidths[index, 0] = excelDrawing.GetPixelLeft();
        drawingWidths[index++, 1] = excelDrawing.GetPixelWidth();
      }
      return drawingWidths;
    }

    internal int[,] GetDrawingHeight()
    {
      int[,] drawingHeight = new int[this.Count, 2];
      int index = 0;
      foreach (ExcelDrawing excelDrawing in this)
      {
        drawingHeight[index, 0] = excelDrawing.GetPixelTop();
        drawingHeight[index++, 1] = excelDrawing.GetPixelHeight();
      }
      return drawingHeight;
    }

    public void Dispose()
    {
      this._drawingsXml = (XmlDocument) null;
      this._hashes.Clear();
      this._hashes = (Dictionary<string, string>) null;
      this._part = (ZipPackagePart) null;
      this._drawingNames.Clear();
      this._drawingNames = (Dictionary<string, int>) null;
      this._drawingRelation = (ZipPackageRelationship) null;
      foreach (ExcelDrawing drawing in this._drawings)
        drawing.Dispose();
      this._drawings.Clear();
      this._drawings = (List<ExcelDrawing>) null;
    }

    internal class ImageCompare
    {
      internal byte[] image { get; set; }

      internal string relID { get; set; }

      internal bool Comparer(byte[] compareImg)
      {
        if (compareImg.Length != this.image.Length)
          return false;
        for (int index = 0; index < this.image.Length; ++index)
        {
          if ((int) this.image[index] != (int) compareImg[index])
            return false;
        }
        return true;
      }
    }
  }
}
