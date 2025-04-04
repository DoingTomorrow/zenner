// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelWorksheets
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Drawing.Vml;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using OfficeOpenXml.Utils;
using OfficeOpenXml.VBA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelWorksheets : XmlHelper, IEnumerable<ExcelWorksheet>, IEnumerable, IDisposable
  {
    private const string ERR_DUP_WORKSHEET = "A worksheet with this name already exists in the workbook";
    internal const string WORKSHEET_CONTENTTYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
    internal const string CHARTSHEET_CONTENTTYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.chartsheet+xml";
    private ExcelPackage _pck;
    private Dictionary<int, ExcelWorksheet> _worksheets;
    private XmlNamespaceManager _namespaceManager;

    internal ExcelWorksheets(ExcelPackage pck, XmlNamespaceManager nsm, XmlNode topNode)
      : base(nsm, topNode)
    {
      this._pck = pck;
      this._namespaceManager = nsm;
      this._worksheets = new Dictionary<int, ExcelWorksheet>();
      int num = 1;
      foreach (XmlNode childNode in topNode.ChildNodes)
      {
        string sheetName = childNode.Attributes["name"].Value;
        string str = childNode.Attributes["r:id"].Value;
        int int32 = Convert.ToInt32(childNode.Attributes["sheetId"].Value);
        eWorkSheetHidden eWorkSheetHidden = eWorkSheetHidden.Visible;
        XmlNode attribute = (XmlNode) childNode.Attributes["state"];
        if (attribute != null)
          eWorkSheetHidden = this.TranslateHidden(attribute.Value);
        ZipPackageRelationship relationship = pck.Workbook.Part.GetRelationship(str);
        Uri uriWorksheet = UriHelper.ResolvePartUri(pck.Workbook.WorkbookUri, relationship.TargetUri);
        if (relationship.RelationshipType.EndsWith("chartsheet"))
          this._worksheets.Add(num, (ExcelWorksheet) new ExcelChartsheet(this._namespaceManager, this._pck, str, uriWorksheet, sheetName, int32, num, eWorkSheetHidden));
        else
          this._worksheets.Add(num, new ExcelWorksheet(this._namespaceManager, this._pck, str, uriWorksheet, sheetName, int32, num, eWorkSheetHidden));
        ++num;
      }
    }

    private eWorkSheetHidden TranslateHidden(string value)
    {
      switch (value)
      {
        case "hidden":
          return eWorkSheetHidden.Hidden;
        case "veryHidden":
          return eWorkSheetHidden.VeryHidden;
        default:
          return eWorkSheetHidden.Visible;
      }
    }

    public int Count => this._worksheets.Count;

    public IEnumerator<ExcelWorksheet> GetEnumerator()
    {
      return (IEnumerator<ExcelWorksheet>) this._worksheets.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._worksheets.Values.GetEnumerator();
    }

    public ExcelWorksheet Add(string Name) => this.AddSheet(Name, false, new eChartType?());

    private ExcelWorksheet AddSheet(string Name, bool isChart, eChartType? chartType)
    {
      if (this.GetByName(Name) != null)
        throw new InvalidOperationException("A worksheet with this name already exists in the workbook");
      int sheetID;
      Uri uriWorksheet;
      this.GetSheetURI(ref Name, out sheetID, out uriWorksheet, isChart);
      StreamWriter writer = new StreamWriter((Stream) this._pck.Package.CreatePart(uriWorksheet, isChart ? "application/vnd.openxmlformats-officedocument.spreadsheetml.chartsheet+xml" : "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write));
      this.CreateNewWorksheet(isChart).Save((TextWriter) writer);
      this._pck.Package.Flush();
      string workbookRel = this.CreateWorkbookRel(Name, sheetID, uriWorksheet, isChart);
      int num = this._worksheets.Count + 1;
      ExcelWorksheet sheet = !isChart ? new ExcelWorksheet(this._namespaceManager, this._pck, workbookRel, uriWorksheet, Name, sheetID, num, eWorkSheetHidden.Visible) : (ExcelWorksheet) new ExcelChartsheet(this._namespaceManager, this._pck, workbookRel, uriWorksheet, Name, sheetID, num, eWorkSheetHidden.Visible, chartType.Value);
      this._worksheets.Add(num, sheet);
      if (this._pck.Workbook.VbaProject != null)
      {
        string nameFromWorksheet = this._pck.Workbook.VbaProject.GetModuleNameFromWorksheet(sheet);
        this._pck.Workbook.VbaProject.Modules.Add(new ExcelVBAModule(new ModuleNameChange(sheet.CodeNameChange))
        {
          Name = nameFromWorksheet,
          Code = "",
          Attributes = this._pck.Workbook.VbaProject.GetDocumentAttributes(Name, "0{00020820-0000-0000-C000-000000000046}"),
          Type = eModuleType.Document,
          HelpContext = 0
        });
        sheet.CodeModuleName = Name;
      }
      return sheet;
    }

    public ExcelWorksheet Add(string Name, ExcelWorksheet Copy)
    {
      if (Copy is ExcelChartsheet)
        throw new ArgumentException("Can not copy a chartsheet");
      if (this.GetByName(Name) != null)
        throw new InvalidOperationException("A worksheet with this name already exists in the workbook");
      int sheetID;
      Uri uriWorksheet;
      this.GetSheetURI(ref Name, out sheetID, out uriWorksheet, false);
      StreamWriter writer = new StreamWriter((Stream) this._pck.Package.CreatePart(uriWorksheet, "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write));
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(Copy.WorksheetXml.OuterXml);
      xmlDocument.Save((TextWriter) writer);
      this._pck.Package.Flush();
      ExcelWorksheet excelWorksheet = new ExcelWorksheet(this._namespaceManager, this._pck, this.CreateWorkbookRel(Name, sheetID, uriWorksheet, false), uriWorksheet, Name, sheetID, this._worksheets.Count + 1, eWorkSheetHidden.Visible);
      if (Copy.Comments.Count > 0)
        this.CopyComment(Copy, excelWorksheet);
      else if (Copy.VmlDrawingsComments.Count > 0)
        this.CopyVmlDrawing(Copy, excelWorksheet);
      this.CopyHeaderFooterPictures(Copy, excelWorksheet);
      if (Copy.Drawings.Count > 0)
        this.CopyDrawing(Copy, excelWorksheet);
      if (Copy.Tables.Count > 0)
        this.CopyTable(Copy, excelWorksheet);
      if (Copy.PivotTables.Count > 0)
        this.CopyPivotTable(Copy, excelWorksheet);
      if (Copy.Names.Count > 0)
        this.CopySheetNames(Copy, excelWorksheet);
      this.CloneCells(Copy, excelWorksheet);
      this._worksheets.Add(this._worksheets.Count + 1, excelWorksheet);
      XmlNode xmlNode = excelWorksheet.WorksheetXml.SelectSingleNode("//d:pageSetup", this._namespaceManager);
      if (xmlNode != null)
      {
        XmlAttribute namedItem = (XmlAttribute) xmlNode.Attributes.GetNamedItem("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        if (namedItem != null)
        {
          string str = namedItem.Value;
          xmlNode.Attributes.Remove(namedItem);
        }
      }
      return excelWorksheet;
    }

    public ExcelChartsheet AddChart(string Name, eChartType chartType)
    {
      return (ExcelChartsheet) this.AddSheet(Name, true, new eChartType?(chartType));
    }

    private void CopySheetNames(ExcelWorksheet Copy, ExcelWorksheet added)
    {
      foreach (ExcelNamedRange name in Copy.Names)
        (name.IsName ? (string.IsNullOrEmpty(name.NameFormula) ? added.Names.AddValue(name.Name, name.Value) : added.Names.AddFormula(name.Name, name.Formula)) : (!(name.WorkSheet == Copy.Name) ? added.Names.Add(name.Name, (ExcelRangeBase) added.Workbook.Worksheets[name.WorkSheet].Cells[name.FirstAddress]) : added.Names.Add(name.Name, (ExcelRangeBase) added.Cells[name.FirstAddress]))).NameComment = name.NameComment;
    }

    private void CopyTable(ExcelWorksheet Copy, ExcelWorksheet added)
    {
      string str = "";
      foreach (ExcelTable table in Copy.Tables)
      {
        string outerXml1 = table.TableXml.OuterXml;
        int num1 = this._pck.Workbook._nextTableID++;
        string Name;
        if (str == "")
        {
          Name = Copy.Tables.GetNewTableName();
        }
        else
        {
          int num2 = int.Parse(str.Substring(5)) + 1;
          Name = string.Format("Table{0}", (object) num2);
          while (this._pck.Workbook.ExistsPivotTableName(Name))
            Name = string.Format("Table{0}", (object) ++num2);
        }
        str = Name;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(outerXml1);
        xmlDocument.SelectSingleNode("//d:table/@id", table.NameSpaceManager).Value = num1.ToString();
        xmlDocument.SelectSingleNode("//d:table/@name", table.NameSpaceManager).Value = Name;
        xmlDocument.SelectSingleNode("//d:table/@displayName", table.NameSpaceManager).Value = Name;
        string outerXml2 = xmlDocument.OuterXml;
        Uri uri = new Uri(string.Format("/xl/tables/table{0}.xml", (object) num1), UriKind.Relative);
        StreamWriter streamWriter = new StreamWriter((Stream) this._pck.Package.CreatePart(uri, "application/vnd.openxmlformats-officedocument.spreadsheetml.table+xml", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write));
        streamWriter.Write(outerXml2);
        streamWriter.Flush();
        ZipPackageRelationship relationship = added.Part.CreateRelationship(UriHelper.GetRelativeUri(added.WorksheetUri, uri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/table");
        if (table.RelationshipID == null)
        {
          XmlNode xmlNode = added.WorksheetXml.SelectSingleNode("//d:tableParts", table.NameSpaceManager);
          if (xmlNode == null)
          {
            added.CreateNode("d:tableParts");
            xmlNode = added.WorksheetXml.SelectSingleNode("//d:tableParts", table.NameSpaceManager);
          }
          XmlElement element = added.WorksheetXml.CreateElement("tablePart", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
          xmlNode.AppendChild((XmlNode) element);
          element.SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationship.Id);
        }
        else
          (added.WorksheetXml.SelectSingleNode(string.Format("//d:tableParts/d:tablePart/@r:id[.='{0}']", (object) table.RelationshipID), table.NameSpaceManager) as XmlAttribute).Value = relationship.Id;
      }
    }

    private void CopyPivotTable(ExcelWorksheet Copy, ExcelWorksheet added)
    {
      string str1 = "";
      foreach (ExcelPivotTable pivotTable in Copy.PivotTables)
      {
        string outerXml1 = pivotTable.PivotTableXml.OuterXml;
        int num1 = this._pck.Workbook._nextPivotTableID++;
        string Name;
        if (str1 == "")
        {
          Name = Copy.PivotTables.GetNewTableName();
        }
        else
        {
          int num2 = int.Parse(str1.Substring(10)) + 1;
          Name = string.Format("PivotTable{0}", (object) num2);
          while (this._pck.Workbook.ExistsPivotTableName(Name))
            Name = string.Format("PivotTable{0}", (object) ++num2);
        }
        str1 = Name;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(outerXml1);
        xmlDocument.SelectSingleNode("//d:pivotTableDefinition/@name", pivotTable.NameSpaceManager).Value = Name;
        string outerXml2 = xmlDocument.OuterXml;
        Uri uri1 = new Uri(string.Format("/xl/pivotTables/pivotTable{0}.xml", (object) num1), UriKind.Relative);
        ZipPackagePart part1 = this._pck.Package.CreatePart(uri1, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotTable+xml", this._pck.Compression);
        StreamWriter streamWriter1 = new StreamWriter((Stream) part1.GetStream(FileMode.Create, FileAccess.Write));
        streamWriter1.Write(outerXml2);
        streamWriter1.Flush();
        string outerXml3 = pivotTable.CacheDefinition.CacheDefinitionXml.OuterXml;
        Uri uri2 = new Uri(string.Format("/xl/pivotCache/pivotcachedefinition{0}.xml", (object) num1), UriKind.Relative);
        while (this._pck.Package.PartExists(uri2))
          uri2 = new Uri(string.Format("/xl/pivotCache/pivotcachedefinition{0}.xml", (object) ++num1), UriKind.Relative);
        ZipPackagePart part2 = this._pck.Package.CreatePart(uri2, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheDefinition+xml", this._pck.Compression);
        StreamWriter streamWriter2 = new StreamWriter((Stream) part2.GetStream(FileMode.Create, FileAccess.Write));
        streamWriter2.Write(outerXml3);
        streamWriter2.Flush();
        string str2 = "<pivotCacheRecords xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" count=\"0\" />";
        Uri uri3 = new Uri(string.Format("/xl/pivotCache/pivotrecords{0}.xml", (object) num1), UriKind.Relative);
        while (this._pck.Package.PartExists(uri3))
          uri3 = new Uri(string.Format("/xl/pivotCache/pivotrecords{0}.xml", (object) ++num1), UriKind.Relative);
        StreamWriter streamWriter3 = new StreamWriter((Stream) this._pck.Package.CreatePart(uri3, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheRecords+xml", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write));
        streamWriter3.Write(str2);
        streamWriter3.Flush();
        added.Part.CreateRelationship(UriHelper.ResolvePartUri(added.WorksheetUri, uri1), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotTable");
        part1.CreateRelationship(UriHelper.ResolvePartUri(pivotTable.Relationship.SourceUri, uri2), pivotTable.CacheDefinition.Relationship.TargetMode, pivotTable.CacheDefinition.Relationship.RelationshipType);
        part2.CreateRelationship(UriHelper.ResolvePartUri(uri2, uri3), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheRecords");
      }
    }

    private void CopyHeaderFooterPictures(ExcelWorksheet Copy, ExcelWorksheet added)
    {
      if (Copy._headerFooter == null)
        return;
      this.CopyText(Copy.HeaderFooter._oddHeader, added.HeaderFooter.OddHeader);
      this.CopyText(Copy.HeaderFooter._oddFooter, added.HeaderFooter.OddFooter);
      this.CopyText(Copy.HeaderFooter._evenHeader, added.HeaderFooter.EvenHeader);
      this.CopyText(Copy.HeaderFooter._evenFooter, added.HeaderFooter.EvenFooter);
      this.CopyText(Copy.HeaderFooter._firstHeader, added.HeaderFooter.FirstHeader);
      this.CopyText(Copy.HeaderFooter._firstFooter, added.HeaderFooter.FirstFooter);
      if (Copy.HeaderFooter.Pictures.Count <= 0)
        return;
      Uri uri = Copy.HeaderFooter.Pictures.Uri;
      XmlHelper.GetNewUri(this._pck.Package, "/xl/drawings/vmlDrawing{0}.vml");
      foreach (ExcelVmlDrawingPicture picture in (IEnumerable) Copy.HeaderFooter.Pictures)
      {
        ExcelVmlDrawingPicture vmlDrawingPicture = added.HeaderFooter.Pictures.Add(picture.Id, picture.ImageUri, picture.Title, picture.Width, picture.Height);
        foreach (XmlAttribute attribute in (XmlNamedNodeMap) picture.TopNode.Attributes)
          (vmlDrawingPicture.TopNode as XmlElement).SetAttribute(attribute.Name, attribute.Value);
        vmlDrawingPicture.TopNode.InnerXml = picture.TopNode.InnerXml;
      }
    }

    private void CopyText(ExcelHeaderFooterText from, ExcelHeaderFooterText to)
    {
      if (from == null)
        return;
      to.LeftAlignedText = from.LeftAlignedText;
      to.CenteredText = from.CenteredText;
      to.RightAlignedText = from.RightAlignedText;
    }

    private void CloneCells(ExcelWorksheet Copy, ExcelWorksheet added)
    {
      bool flag = Copy.Workbook == this._pck.Workbook;
      bool doAdjustDrawings = this._pck.DoAdjustDrawings;
      this._pck.DoAdjustDrawings = false;
      added.MergedCells.List.AddRange((IEnumerable<string>) Copy.MergedCells.List);
      foreach (int key in Copy._sharedFormulas.Keys)
        added._sharedFormulas.Add(key, Copy._sharedFormulas[key]);
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(Copy._values);
      while (cellsStoreEnumerator.Next())
      {
        int row = cellsStoreEnumerator.Row;
        int column = cellsStoreEnumerator.Column;
        int num1 = 0;
        if (row == 0)
        {
          if (Copy._values.GetValue(row, column) is ExcelColumn excelColumn)
          {
            added._values.SetValue(row, column, (object) excelColumn.Clone(added, excelColumn.ColumnMin));
            num1 = excelColumn.StyleID;
          }
        }
        else if (column == 0)
        {
          ExcelRow excelRow = Copy.Row(row);
          if (excelRow != null)
          {
            excelRow.Clone(added);
            num1 = excelRow.StyleID;
          }
        }
        else
          num1 = this.CopyValues(Copy, added, row, column);
        if (!flag)
        {
          if (dictionary.ContainsKey(num1))
          {
            added._styles.SetValue(row, column, dictionary[num1]);
          }
          else
          {
            int num2 = added.Workbook.Styles.CloneStyle(Copy.Workbook.Styles, num1);
            dictionary.Add(num1, num2);
            added._styles.SetValue(row, column, num2);
            if (Copy.Workbook.Styles.CellXfs[num1].XfId > 0)
            {
              string name = Copy.Workbook.Styles.NamedStyles[Copy.Workbook.Styles.CellXfs[num1].XfId].Name;
              if (!Copy.Workbook.Styles.NamedStyles.ExistsKey(name))
                Copy.Workbook.Styles.CreateNamedStyle(name).StyleXfId = num2;
            }
          }
        }
      }
      added._package.DoAdjustDrawings = doAdjustDrawings;
    }

    private int CopyValues(ExcelWorksheet Copy, ExcelWorksheet added, int row, int col)
    {
      added._values.SetValue(row, col, Copy._values.GetValue(row, col));
      string str = Copy._types.GetValue(row, col);
      if (str != null)
        added._types.SetValue(row, col, str);
      object obj1 = Copy._formulas.GetValue(row, col);
      if (obj1 != null)
        added.SetFormula(row, col, obj1);
      int num = Copy._styles.GetValue(row, col);
      if (num != 0)
        added._styles.SetValue(row, col, num);
      object obj2 = Copy._formulas.GetValue(row, col);
      if (obj2 != null)
        added._formulas.SetValue(row, col, obj2);
      return num;
    }

    private void CopyComment(ExcelWorksheet Copy, ExcelWorksheet workSheet)
    {
      string innerXml1 = Copy.Comments.CommentXml.InnerXml;
      Uri uri1 = new Uri(string.Format("/xl/comments{0}.xml", (object) workSheet.SheetID), UriKind.Relative);
      if (this._pck.Package.PartExists(uri1))
        uri1 = XmlHelper.GetNewUri(this._pck.Package, "/xl/drawings/vmldrawing{0}.vml");
      StreamWriter streamWriter1 = new StreamWriter((Stream) this._pck.Package.CreatePart(uri1, "application/vnd.openxmlformats-officedocument.spreadsheetml.comments+xml", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write));
      streamWriter1.Write(innerXml1);
      streamWriter1.Flush();
      workSheet.Part.CreateRelationship(UriHelper.GetRelativeUri(workSheet.WorksheetUri, uri1), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments");
      string innerXml2 = Copy.VmlDrawingsComments.VmlDrawingXml.InnerXml;
      Uri uri2 = new Uri(string.Format("/xl/drawings/vmldrawing{0}.vml", (object) workSheet.SheetID), UriKind.Relative);
      if (this._pck.Package.PartExists(uri2))
        uri2 = XmlHelper.GetNewUri(this._pck.Package, "/xl/drawings/vmldrawing{0}.vml");
      StreamWriter streamWriter2 = new StreamWriter((Stream) this._pck.Package.CreatePart(uri2, "application/vnd.openxmlformats-officedocument.vmlDrawing", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write));
      streamWriter2.Write(innerXml2);
      streamWriter2.Flush();
      ZipPackageRelationship relationship = workSheet.Part.CreateRelationship(UriHelper.GetRelativeUri(workSheet.WorksheetUri, uri2), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing");
      if (!(workSheet.WorksheetXml.SelectSingleNode("//d:legacyDrawing", this._namespaceManager) is XmlElement xmlElement))
      {
        workSheet.CreateNode("d:legacyDrawing");
        xmlElement = workSheet.WorksheetXml.SelectSingleNode("//d:legacyDrawing", this._namespaceManager) as XmlElement;
      }
      xmlElement.SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationship.Id);
    }

    private void CopyDrawing(ExcelWorksheet Copy, ExcelWorksheet workSheet)
    {
      string outerXml = Copy.Drawings.DrawingXml.OuterXml;
      Uri uri = new Uri(string.Format("/xl/drawings/drawing{0}.xml", (object) workSheet.SheetID), UriKind.Relative);
      ZipPackagePart part1 = this._pck.Package.CreatePart(uri, "application/vnd.openxmlformats-officedocument.drawing+xml", this._pck.Compression);
      StreamWriter streamWriter1 = new StreamWriter((Stream) part1.GetStream(FileMode.Create, FileAccess.Write));
      streamWriter1.Write(outerXml);
      streamWriter1.Flush();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(outerXml);
      ZipPackageRelationship relationship1 = workSheet.Part.CreateRelationship(UriHelper.GetRelativeUri(workSheet.WorksheetUri, uri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing");
      (workSheet.WorksheetXml.SelectSingleNode("//d:drawing", this._namespaceManager) as XmlElement).SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationship1.Id);
      foreach (ExcelDrawing drawing in Copy.Drawings)
      {
        if (drawing is ExcelChart)
        {
          string innerXml = (drawing as ExcelChart).ChartXml.InnerXml;
          Uri newUri = XmlHelper.GetNewUri(this._pck.Package, "/xl/charts/chart{0}.xml");
          StreamWriter streamWriter2 = new StreamWriter((Stream) this._pck.Package.CreatePart(newUri, "application/vnd.openxmlformats-officedocument.drawingml.chart+xml", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write));
          streamWriter2.Write(innerXml);
          streamWriter2.Flush();
          string str = drawing.TopNode.SelectSingleNode("xdr:graphicFrame/a:graphic/a:graphicData/c:chart/@r:id", Copy.Drawings.NameSpaceManager).Value;
          ZipPackageRelationship relationship2 = part1.CreateRelationship(UriHelper.GetRelativeUri(uri, newUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart");
          (xmlDocument.SelectSingleNode(string.Format("//c:chart/@r:id[.='{0}']", (object) str), Copy.Drawings.NameSpaceManager) as XmlAttribute).Value = relationship2.Id;
        }
        else if (drawing is ExcelPicture)
        {
          ExcelPicture excelPicture = drawing as ExcelPicture;
          Uri uriPic = excelPicture.UriPic;
          if (!workSheet.Workbook._package.Package.PartExists(uriPic))
          {
            ZipPackagePart part2 = workSheet.Workbook._package.Package.CreatePart(uriPic, excelPicture.ContentType, CompressionLevel.Level0);
            excelPicture.Image.Save((Stream) part2.GetStream(FileMode.Create, FileAccess.Write), excelPicture.ImageFormat);
          }
          string str = drawing.TopNode.SelectSingleNode("xdr:pic/xdr:blipFill/a:blip/@r:embed", Copy.Drawings.NameSpaceManager).Value;
          ZipPackageRelationship relationship3 = part1.CreateRelationship(UriHelper.GetRelativeUri(workSheet.WorksheetUri, uriPic), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
          (xmlDocument.SelectSingleNode(string.Format("//xdr:pic/xdr:blipFill/a:blip/@r:embed[.='{0}']", (object) str), Copy.Drawings.NameSpaceManager) as XmlAttribute).Value = relationship3.Id;
        }
      }
      StreamWriter streamWriter3 = new StreamWriter((Stream) part1.GetStream(FileMode.Create, FileAccess.Write));
      streamWriter3.Write(xmlDocument.OuterXml);
      streamWriter3.Flush();
    }

    private void CopyVmlDrawing(ExcelWorksheet origSheet, ExcelWorksheet newSheet)
    {
      string outerXml = origSheet.VmlDrawingsComments.VmlDrawingXml.OuterXml;
      Uri uri = new Uri(string.Format("/xl/drawings/vmlDrawing{0}.vml", (object) newSheet.SheetID), UriKind.Relative);
      using (StreamWriter streamWriter = new StreamWriter((Stream) this._pck.Package.CreatePart(uri, "application/vnd.openxmlformats-officedocument.vmlDrawing", this._pck.Compression).GetStream(FileMode.Create, FileAccess.Write)))
      {
        streamWriter.Write(outerXml);
        streamWriter.Flush();
      }
      ZipPackageRelationship relationship = newSheet.Part.CreateRelationship(UriHelper.GetRelativeUri(newSheet.WorksheetUri, uri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing");
      if (!(newSheet.WorksheetXml.SelectSingleNode("//d:legacyDrawing", this._namespaceManager) is XmlElement xmlElement))
        xmlElement = newSheet.WorksheetXml.CreateNode(XmlNodeType.Entity, "//d:legacyDrawing", this._namespaceManager.LookupNamespace("d")) as XmlElement;
      xmlElement?.SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationship.Id);
    }

    private string CreateWorkbookRel(string Name, int sheetID, Uri uriWorksheet, bool isChart)
    {
      ZipPackageRelationship relationship = this._pck.Workbook.Part.CreateRelationship(UriHelper.GetRelativeUri(this._pck.Workbook.WorkbookUri, uriWorksheet), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/" + (isChart ? "chartsheet" : "worksheet"));
      this._pck.Package.Flush();
      XmlElement element = this._pck.Workbook.WorkbookXml.CreateElement("sheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      element.SetAttribute("name", Name);
      element.SetAttribute("sheetId", sheetID.ToString());
      element.SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationship.Id);
      this.TopNode.AppendChild((XmlNode) element);
      return relationship.Id;
    }

    private void GetSheetURI(ref string Name, out int sheetID, out Uri uriWorksheet, bool isChart)
    {
      Name = this.ValidateFixSheetName(Name);
      sheetID = 0;
      foreach (ExcelWorksheet excelWorksheet in this)
      {
        if (excelWorksheet.SheetID > sheetID)
          sheetID = excelWorksheet.SheetID;
      }
      ++sheetID;
      if (isChart)
        uriWorksheet = new Uri("/xl/chartsheets/chartsheet" + sheetID.ToString() + ".xml", UriKind.Relative);
      else
        uriWorksheet = new Uri("/xl/worksheets/sheet" + sheetID.ToString() + ".xml", UriKind.Relative);
    }

    internal string ValidateFixSheetName(string Name)
    {
      if (this.ValidateName(Name))
      {
        if (Name.IndexOf(':') > -1)
          Name = Name.Replace(":", " ");
        if (Name.IndexOf('/') > -1)
          Name = Name.Replace("/", " ");
        if (Name.IndexOf('\\') > -1)
          Name = Name.Replace("\\", " ");
        if (Name.IndexOf('?') > -1)
          Name = Name.Replace("?", " ");
        if (Name.IndexOf('[') > -1)
          Name = Name.Replace("[", " ");
        if (Name.IndexOf(']') > -1)
          Name = Name.Replace("]", " ");
      }
      if (Name.Trim() == "")
        throw new ArgumentException("The worksheet can not have an empty name");
      if (Name.Length > 31)
        Name = Name.Substring(0, 31);
      return Name;
    }

    private bool ValidateName(string Name) => Regex.IsMatch(Name, ":|\\?|/|\\\\|\\[|\\]");

    internal XmlDocument CreateNewWorksheet(bool isChart)
    {
      XmlDocument newWorksheet = new XmlDocument();
      XmlElement element1 = newWorksheet.CreateElement(isChart ? "chartsheet" : "worksheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      element1.SetAttribute("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
      newWorksheet.AppendChild((XmlNode) element1);
      if (isChart)
      {
        XmlElement element2 = newWorksheet.CreateElement("sheetPr", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.AppendChild((XmlNode) element2);
        XmlElement element3 = newWorksheet.CreateElement("sheetViews", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.AppendChild((XmlNode) element3);
        XmlElement element4 = newWorksheet.CreateElement("sheetView", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element4.SetAttribute("workbookViewId", "0");
        element4.SetAttribute("zoomToFit", "1");
        element3.AppendChild((XmlNode) element4);
      }
      else
      {
        XmlElement element5 = newWorksheet.CreateElement("sheetViews", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.AppendChild((XmlNode) element5);
        XmlElement element6 = newWorksheet.CreateElement("sheetView", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element6.SetAttribute("workbookViewId", "0");
        element5.AppendChild((XmlNode) element6);
        XmlElement element7 = newWorksheet.CreateElement("sheetFormatPr", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element7.SetAttribute("defaultRowHeight", "15");
        element1.AppendChild((XmlNode) element7);
        XmlElement element8 = newWorksheet.CreateElement("sheetData", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.AppendChild((XmlNode) element8);
      }
      return newWorksheet;
    }

    public void Delete(int Index)
    {
      ExcelWorksheet worksheet = this._worksheets[Index];
      if (worksheet.Drawings.Count > 0)
        worksheet.Drawings.ClearDrawings();
      Uri uri = new Uri(string.Format("/xl/drawings/drawing{0}.xml", (object) worksheet.SheetID), UriKind.Relative);
      if (this._pck.Package.PartExists(uri))
        this._pck.Package.DeletePart(uri);
      this._pck.Package.DeletePart(worksheet.WorksheetUri);
      this._pck.Workbook.Part.DeleteRelationship(worksheet.RelationshipID);
      XmlNode xmlNode = this._pck.Workbook.WorkbookXml.SelectSingleNode("//d:workbook/d:sheets", this._namespaceManager);
      if (xmlNode != null)
      {
        XmlNode oldChild = xmlNode.SelectSingleNode(string.Format("./d:sheet[@sheetId={0}]", (object) worksheet.SheetID), this._namespaceManager);
        if (oldChild != null)
          xmlNode.RemoveChild(oldChild);
      }
      this._worksheets.Remove(Index);
      if (this._pck.Workbook.VbaProject != null)
        this._pck.Workbook.VbaProject.Modules.Remove(worksheet.CodeModule);
      this.ReindexWorksheetDictionary();
      if (this._pck.Workbook.View.ActiveTab >= this._pck.Workbook.Worksheets.Count)
        --this._pck.Workbook.View.ActiveTab;
      if (this._pck.Workbook.View.ActiveTab == worksheet.SheetID)
        this._pck.Workbook.Worksheets[0].View.TabSelected = true;
    }

    public void Delete(string name)
    {
      this.Delete((this[name] ?? throw new ArgumentException(string.Format("Could not find worksheet to delete '{0}'", (object) name))).PositionID);
    }

    public void Delete(ExcelWorksheet Worksheet)
    {
      if (Worksheet.PositionID > this._worksheets.Count || Worksheet != this._worksheets[Worksheet.PositionID])
        throw new ArgumentException("Worksheet is not in the collection.");
      this.Delete(Worksheet.PositionID);
    }

    private void ReindexWorksheetDictionary()
    {
      int num = 1;
      Dictionary<int, ExcelWorksheet> dictionary = new Dictionary<int, ExcelWorksheet>();
      foreach (KeyValuePair<int, ExcelWorksheet> worksheet in this._worksheets)
      {
        worksheet.Value.PositionID = num;
        dictionary.Add(num++, worksheet.Value);
      }
      this._worksheets = dictionary;
    }

    public ExcelWorksheet this[int PositionID] => this._worksheets[PositionID];

    public ExcelWorksheet this[string Name] => this.GetByName(Name);

    public ExcelWorksheet Copy(string Name, string NewName)
    {
      return this.Add(NewName, this[Name] ?? throw new ArgumentException(string.Format("Copy worksheet error: Could not find worksheet to copy '{0}'", (object) Name)));
    }

    internal ExcelWorksheet GetBySheetID(int localSheetID)
    {
      foreach (ExcelWorksheet bySheetId in this)
      {
        if (bySheetId.SheetID == localSheetID)
          return bySheetId;
      }
      return (ExcelWorksheet) null;
    }

    private ExcelWorksheet GetByName(string Name)
    {
      if (string.IsNullOrEmpty(Name))
        return (ExcelWorksheet) null;
      ExcelWorksheet byName = (ExcelWorksheet) null;
      foreach (ExcelWorksheet excelWorksheet in this._worksheets.Values)
      {
        if (excelWorksheet.Name.ToLower() == Name.ToLower())
          byName = excelWorksheet;
      }
      return byName;
    }

    public void MoveBefore(string sourceName, string targetName)
    {
      this.Move(sourceName, targetName, false);
    }

    public void MoveBefore(int sourcePositionId, int targetPositionId)
    {
      this.Move(sourcePositionId, targetPositionId, false);
    }

    public void MoveAfter(string sourceName, string targetName)
    {
      this.Move(sourceName, targetName, true);
    }

    public void MoveAfter(int sourcePositionId, int targetPositionId)
    {
      this.Move(sourcePositionId, targetPositionId, true);
    }

    public void MoveToStart(string sourceName)
    {
      this.Move((this[sourceName] ?? throw new Exception(string.Format("Move worksheet error: Could not find worksheet to move '{0}'", (object) sourceName))).PositionID, 1, false);
    }

    public void MoveToStart(int sourcePositionId) => this.Move(sourcePositionId, 1, false);

    public void MoveToEnd(string sourceName)
    {
      this.Move((this[sourceName] ?? throw new Exception(string.Format("Move worksheet error: Could not find worksheet to move '{0}'", (object) sourceName))).PositionID, this._worksheets.Count, true);
    }

    public void MoveToEnd(int sourcePositionId)
    {
      this.Move(sourcePositionId, this._worksheets.Count, true);
    }

    private void Move(string sourceName, string targetName, bool placeAfter)
    {
      this.Move((this[sourceName] ?? throw new Exception(string.Format("Move worksheet error: Could not find worksheet to move '{0}'", (object) sourceName))).PositionID, (this[targetName] ?? throw new Exception(string.Format("Move worksheet error: Could not find worksheet to move '{0}'", (object) targetName))).PositionID, placeAfter);
    }

    private void Move(int sourcePositionId, int targetPositionId, bool placeAfter)
    {
      ExcelWorksheet sourceSheet = this[sourcePositionId];
      if (sourceSheet == null)
        throw new Exception(string.Format("Move worksheet error: Could not find worksheet at position '{0}'", (object) sourcePositionId));
      ExcelWorksheet targetSheet = this[targetPositionId];
      if (targetSheet == null)
        throw new Exception(string.Format("Move worksheet error: Could not find worksheet at position '{0}'", (object) targetPositionId));
      if (this._worksheets.Count < 2)
        return;
      int num = 1;
      Dictionary<int, ExcelWorksheet> dictionary = new Dictionary<int, ExcelWorksheet>();
      foreach (KeyValuePair<int, ExcelWorksheet> worksheet in this._worksheets)
      {
        if (worksheet.Key == targetPositionId)
        {
          if (!placeAfter)
          {
            sourceSheet.PositionID = num;
            dictionary.Add(num++, sourceSheet);
          }
          worksheet.Value.PositionID = num;
          dictionary.Add(num++, worksheet.Value);
          if (placeAfter)
          {
            sourceSheet.PositionID = num;
            dictionary.Add(num++, sourceSheet);
          }
        }
        else if (worksheet.Key != sourcePositionId)
        {
          worksheet.Value.PositionID = num;
          dictionary.Add(num++, worksheet.Value);
        }
      }
      this._worksheets = dictionary;
      this.MoveSheetXmlNode(sourceSheet, targetSheet, placeAfter);
    }

    private void MoveSheetXmlNode(
      ExcelWorksheet sourceSheet,
      ExcelWorksheet targetSheet,
      bool placeAfter)
    {
      XmlNode newChild = this.TopNode.SelectSingleNode(string.Format("d:sheet[@sheetId = '{0}']", (object) sourceSheet.SheetID), this._namespaceManager);
      XmlNode refChild = this.TopNode.SelectSingleNode(string.Format("d:sheet[@sheetId = '{0}']", (object) targetSheet.SheetID), this._namespaceManager);
      if (newChild == null || refChild == null)
        throw new Exception("Source SheetId and Target SheetId must be valid");
      if (placeAfter)
        this.TopNode.InsertAfter(newChild, refChild);
      else
        this.TopNode.InsertBefore(newChild, refChild);
    }

    public void Dispose()
    {
      foreach (IDisposable disposable in this._worksheets.Values)
        disposable.Dispose();
      this._worksheets = (Dictionary<int, ExcelWorksheet>) null;
      this._pck = (ExcelPackage) null;
    }
  }
}
