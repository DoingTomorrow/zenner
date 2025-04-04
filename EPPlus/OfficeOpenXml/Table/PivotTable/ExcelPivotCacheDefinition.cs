// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotCacheDefinition
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotCacheDefinition : XmlHelper
  {
    private const string _sourceWorksheetPath = "d:cacheSource/d:worksheetSource/@sheet";
    private const string _sourceAddressPath = "d:cacheSource/d:worksheetSource/@ref";
    internal ExcelRangeBase _sourceRange;

    internal ExcelPivotCacheDefinition(XmlNamespaceManager ns, ExcelPivotTable pivotTable)
      : base(ns, (XmlNode) null)
    {
      foreach (ZipPackageRelationship packageRelationship in pivotTable.Part.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheDefinition"))
        this.Relationship = packageRelationship;
      this.CacheDefinitionUri = UriHelper.ResolvePartUri(this.Relationship.SourceUri, this.Relationship.TargetUri);
      this.Part = pivotTable.WorkSheet._package.Package.GetPart(this.CacheDefinitionUri);
      this.CacheDefinitionXml = new XmlDocument();
      XmlHelper.LoadXmlSafe(this.CacheDefinitionXml, (Stream) this.Part.GetStream());
      this.TopNode = (XmlNode) this.CacheDefinitionXml.DocumentElement;
      this.PivotTable = pivotTable;
      if (this.CacheSource != eSourceType.Worksheet)
        return;
      string worksheetName = this.GetXmlNodeString("d:cacheSource/d:worksheetSource/@sheet");
      if (!pivotTable.WorkSheet.Workbook.Worksheets.Any<ExcelWorksheet>((Func<ExcelWorksheet, bool>) (t => t.Name == worksheetName)))
        return;
      this._sourceRange = (ExcelRangeBase) pivotTable.WorkSheet.Workbook.Worksheets[worksheetName].Cells[this.GetXmlNodeString("d:cacheSource/d:worksheetSource/@ref")];
    }

    internal ExcelPivotCacheDefinition(
      XmlNamespaceManager ns,
      ExcelPivotTable pivotTable,
      ExcelRangeBase sourceAddress,
      int tblId)
      : base(ns, (XmlNode) null)
    {
      this.PivotTable = pivotTable;
      ZipPackage package = pivotTable.WorkSheet._package.Package;
      this.CacheDefinitionXml = new XmlDocument();
      XmlHelper.LoadXmlSafe(this.CacheDefinitionXml, this.GetStartXml(sourceAddress), Encoding.UTF8);
      this.CacheDefinitionUri = XmlHelper.GetNewUri(package, "/xl/pivotCache/pivotCacheDefinition{0}.xml", tblId);
      this.Part = package.CreatePart(this.CacheDefinitionUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheDefinition+xml");
      this.TopNode = (XmlNode) this.CacheDefinitionXml.DocumentElement;
      this.CacheRecordUri = XmlHelper.GetNewUri(package, "/xl/pivotCache/pivotCacheRecords{0}.xml", tblId);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<pivotCacheRecords xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" count=\"0\" />");
      ZipPackagePart part = package.CreatePart(this.CacheRecordUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheRecords+xml");
      xmlDocument.Save((Stream) part.GetStream());
      this.RecordRelationship = this.Part.CreateRelationship(UriHelper.ResolvePartUri(this.CacheDefinitionUri, this.CacheRecordUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheRecords");
      this.RecordRelationshipID = this.RecordRelationship.Id;
      this.CacheDefinitionXml.Save((Stream) this.Part.GetStream());
    }

    internal ZipPackagePart Part { get; set; }

    public XmlDocument CacheDefinitionXml { get; private set; }

    public Uri CacheDefinitionUri { get; internal set; }

    internal Uri CacheRecordUri { get; set; }

    internal ZipPackageRelationship Relationship { get; set; }

    internal ZipPackageRelationship RecordRelationship { get; set; }

    internal string RecordRelationshipID
    {
      get => this.GetXmlNodeString("@r:id");
      set => this.SetXmlNodeString("@r:id", value);
    }

    public ExcelPivotTable PivotTable { get; private set; }

    public ExcelRangeBase SourceRange
    {
      get
      {
        if (this._sourceRange == null)
        {
          if (this.CacheSource != eSourceType.Worksheet)
            throw new ArgumentException("The cachesource is not a worksheet");
          ExcelWorksheet worksheet = this.PivotTable.WorkSheet.Workbook.Worksheets[this.GetXmlNodeString("d:cacheSource/d:worksheetSource/@sheet")];
          if (worksheet != null)
            this._sourceRange = (ExcelRangeBase) worksheet.Cells[this.GetXmlNodeString("d:cacheSource/d:worksheetSource/@ref")];
        }
        return this._sourceRange;
      }
      set
      {
        if (this.PivotTable.WorkSheet.Workbook != value.Worksheet.Workbook)
          throw new ArgumentException("Range must be in the same package as the pivottable");
        ExcelRangeBase sourceRange = this.SourceRange;
        if (value.End.Column - value.Start.Column != sourceRange.End.Column - sourceRange.Start.Column)
          throw new ArgumentException("Can not change the number of columns(fields) in the SourceRange");
        this.SetXmlNodeString("d:cacheSource/d:worksheetSource/@sheet", value.Worksheet.Name);
        this.SetXmlNodeString("d:cacheSource/d:worksheetSource/@ref", value.FirstAddress);
        this._sourceRange = value;
      }
    }

    public eSourceType CacheSource
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:cacheSource/@type");
        return xmlNodeString == "" ? eSourceType.Worksheet : (eSourceType) Enum.Parse(typeof (eSourceType), xmlNodeString, true);
      }
    }

    private string GetStartXml(ExcelRangeBase sourceAddress)
    {
      string str = "<pivotCacheDefinition xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" r:id=\"\" refreshOnLoad=\"1\" refreshedBy=\"SomeUser\" refreshedDate=\"40504.582403125001\" createdVersion=\"1\" refreshedVersion=\"3\" recordCount=\"5\" upgradeOnRefresh=\"1\">" + "<cacheSource type=\"worksheet\">" + string.Format("<worksheetSource ref=\"{0}\" sheet=\"{1}\" /> ", (object) sourceAddress.Address, (object) sourceAddress.WorkSheet) + "</cacheSource>" + string.Format("<cacheFields count=\"{0}\">", (object) (sourceAddress._toCol - sourceAddress._fromCol + 1));
      ExcelWorksheet worksheet = this.PivotTable.WorkSheet.Workbook.Worksheets[sourceAddress.WorkSheet];
      for (int fromCol = sourceAddress._fromCol; fromCol <= sourceAddress._toCol; ++fromCol)
        str = (worksheet == null || worksheet._values.GetValue(sourceAddress._fromRow, fromCol) == null || worksheet._values.GetValue(sourceAddress._fromRow, fromCol).ToString().Trim() == "" ? str + string.Format("<cacheField name=\"Column{0}\" numFmtId=\"0\">", (object) (fromCol - sourceAddress._fromCol + 1)) : str + string.Format("<cacheField name=\"{0}\" numFmtId=\"0\">", worksheet._values.GetValue(sourceAddress._fromRow, fromCol))) + "<sharedItems containsBlank=\"1\" /> " + "</cacheField>";
      return str + "</cacheFields>" + "</pivotCacheDefinition>";
    }
  }
}
