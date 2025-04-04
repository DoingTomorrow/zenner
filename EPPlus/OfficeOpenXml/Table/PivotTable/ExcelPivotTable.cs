// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTable
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTable : XmlHelper
  {
    private const string ID_PATH = "@id";
    private const string NAME_PATH = "@name";
    private const string DISPLAY_NAME_PATH = "@displayName";
    private const string FIRSTHEADERROW_PATH = "d:location/@firstHeaderRow";
    private const string FIRSTDATAROW_PATH = "d:location/@firstDataRow";
    private const string FIRSTDATACOL_PATH = "d:location/@firstDataCol";
    private const string STYLENAME_PATH = "d:pivotTableStyleInfo/@name";
    private ExcelPivotCacheDefinition _cacheDefinition;
    private ExcelPivotTableFieldCollection _fields;
    private ExcelPivotTableRowColumnFieldCollection _rowFields;
    private ExcelPivotTableRowColumnFieldCollection _columnFields;
    private ExcelPivotTableDataFieldCollection _dataFields;
    private ExcelPivotTableRowColumnFieldCollection _pageFields;
    private TableStyles _tableStyle = TableStyles.Medium6;

    internal ExcelPivotTable(ZipPackageRelationship rel, ExcelWorksheet sheet)
      : base(sheet.NameSpaceManager)
    {
      this.WorkSheet = sheet;
      this.PivotTableUri = UriHelper.ResolvePartUri(rel.SourceUri, rel.TargetUri);
      this.Relationship = rel;
      this.Part = sheet._package.Package.GetPart(this.PivotTableUri);
      this.PivotTableXml = new XmlDocument();
      XmlHelper.LoadXmlSafe(this.PivotTableXml, (Stream) this.Part.GetStream());
      this.init();
      this.TopNode = (XmlNode) this.PivotTableXml.DocumentElement;
      this.Address = new ExcelAddressBase(this.GetXmlNodeString("d:location/@ref"));
      this._cacheDefinition = new ExcelPivotCacheDefinition(sheet.NameSpaceManager, this);
      this.LoadFields();
      foreach (XmlElement selectNode in this.TopNode.SelectNodes("d:rowFields/d:field", this.NameSpaceManager))
      {
        int result;
        if (int.TryParse(selectNode.GetAttribute("x"), out result) && result >= 0)
          this.RowFields.AddInternal(this.Fields[result]);
        else
          selectNode.ParentNode.RemoveChild((XmlNode) selectNode);
      }
      foreach (XmlElement selectNode in this.TopNode.SelectNodes("d:colFields/d:field", this.NameSpaceManager))
      {
        int result;
        if (int.TryParse(selectNode.GetAttribute("x"), out result) && result >= 0)
          this.ColumnFields.AddInternal(this.Fields[result]);
        else
          selectNode.ParentNode.RemoveChild((XmlNode) selectNode);
      }
      foreach (XmlElement selectNode in this.TopNode.SelectNodes("d:pageFields/d:pageField", this.NameSpaceManager))
      {
        int result;
        if (int.TryParse(selectNode.GetAttribute("fld"), out result) && result >= 0)
        {
          ExcelPivotTableField field = this.Fields[result];
          field._pageFieldSettings = new ExcelPivotTablePageFieldSettings(this.NameSpaceManager, (XmlNode) selectNode, field, result);
          this.PageFields.AddInternal(field);
        }
      }
      foreach (XmlElement selectNode in this.TopNode.SelectNodes("d:dataFields/d:dataField", this.NameSpaceManager))
      {
        int result;
        if (int.TryParse(selectNode.GetAttribute("fld"), out result) && result >= 0)
        {
          ExcelPivotTableField field = this.Fields[result];
          this.DataFields.AddInternal(new ExcelPivotTableDataField(this.NameSpaceManager, (XmlNode) selectNode, field));
        }
      }
    }

    internal ExcelPivotTable(
      ExcelWorksheet sheet,
      ExcelAddressBase address,
      ExcelRangeBase sourceAddress,
      string name,
      int tblId)
      : base(sheet.NameSpaceManager)
    {
      this.WorkSheet = sheet;
      this.Address = address;
      ZipPackage package = sheet._package.Package;
      this.PivotTableXml = new XmlDocument();
      XmlHelper.LoadXmlSafe(this.PivotTableXml, this.GetStartXml(name, tblId, address, (ExcelAddressBase) sourceAddress), Encoding.UTF8);
      this.TopNode = (XmlNode) this.PivotTableXml.DocumentElement;
      this.PivotTableUri = XmlHelper.GetNewUri(package, "/xl/pivotTables/pivotTable{0}.xml", tblId);
      this.init();
      this.Part = package.CreatePart(this.PivotTableUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotTable+xml");
      this.PivotTableXml.Save((Stream) this.Part.GetStream());
      this.Relationship = sheet.Part.CreateRelationship(UriHelper.ResolvePartUri(sheet.WorksheetUri, this.PivotTableUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotTable");
      this._cacheDefinition = new ExcelPivotCacheDefinition(sheet.NameSpaceManager, this, sourceAddress, tblId);
      this._cacheDefinition.Relationship = this.Part.CreateRelationship(UriHelper.ResolvePartUri(this.PivotTableUri, this._cacheDefinition.CacheDefinitionUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheDefinition");
      sheet.Workbook.AddPivotTable(this.CacheID.ToString(), this._cacheDefinition.CacheDefinitionUri);
      this.LoadFields();
      using (ExcelRange cell = sheet.Cells[address.Address])
        cell.Clear();
    }

    private void init()
    {
      this.SchemaNodeOrder = new string[12]
      {
        "location",
        "pivotFields",
        "rowFields",
        "rowItems",
        "colFields",
        "colItems",
        "pageFields",
        "pageItems",
        "dataFields",
        "dataItems",
        "formats",
        "pivotTableStyleInfo"
      };
    }

    private void LoadFields()
    {
      int index = 0;
      foreach (XmlNode selectNode in this.TopNode.SelectNodes("d:pivotFields/d:pivotField", this.NameSpaceManager))
        this.Fields.AddInternal(new ExcelPivotTableField(this.NameSpaceManager, selectNode, this, index, index++));
      int num = 0;
      foreach (XmlElement selectNode in this._cacheDefinition.TopNode.SelectNodes("d:cacheFields/d:cacheField", this.NameSpaceManager))
        this.Fields[num++].SetCacheFieldNode((XmlNode) selectNode);
    }

    private string GetStartXml(
      string name,
      int id,
      ExcelAddressBase address,
      ExcelAddressBase sourceAddress)
    {
      string str = string.Format("<pivotTableDefinition xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" name=\"{0}\" cacheId=\"{1}\" dataOnRows=\"1\" applyNumberFormats=\"0\" applyBorderFormats=\"0\" applyFontFormats=\"0\" applyPatternFormats=\"0\" applyAlignmentFormats=\"0\" applyWidthHeightFormats=\"1\" dataCaption=\"Data\"  createdVersion=\"4\" showMemberPropertyTips=\"0\" useAutoFormatting=\"1\" itemPrintTitles=\"1\" indent=\"0\" compact=\"0\" compactData=\"0\" gridDropZones=\"1\">", (object) name, (object) id) + string.Format("<location ref=\"{0}\" firstHeaderRow=\"1\" firstDataRow=\"1\" firstDataCol=\"1\" /> ", (object) address.FirstAddress) + string.Format("<pivotFields count=\"{0}\">", (object) (sourceAddress._toCol - sourceAddress._fromCol + 1));
      for (int fromCol = sourceAddress._fromCol; fromCol <= sourceAddress._toCol; ++fromCol)
        str += "<pivotField showAll=\"0\" />";
      return str + "</pivotFields>" + "<pivotTableStyleInfo name=\"PivotStyleMedium9\" showRowHeaders=\"1\" showColHeaders=\"1\" showRowStripes=\"0\" showColStripes=\"0\" showLastColumn=\"1\" />" + "</pivotTableDefinition>";
    }

    internal ZipPackagePart Part { get; set; }

    public XmlDocument PivotTableXml { get; private set; }

    public Uri PivotTableUri { get; internal set; }

    internal ZipPackageRelationship Relationship { get; set; }

    internal int Id
    {
      get => this.GetXmlNodeInt("@id");
      set => this.SetXmlNodeString("@id", value.ToString());
    }

    public string Name
    {
      get => this.GetXmlNodeString("@name");
      set
      {
        if (this.WorkSheet.Workbook.ExistsTableName(value))
          throw new ArgumentException("PivotTable name is not unique");
        string name = this.Name;
        if (this.WorkSheet.Tables._tableNames.ContainsKey(name))
        {
          int tableName = this.WorkSheet.Tables._tableNames[name];
          this.WorkSheet.Tables._tableNames.Remove(name);
          this.WorkSheet.Tables._tableNames.Add(value, tableName);
        }
        this.SetXmlNodeString("@name", value);
        this.SetXmlNodeString("@displayName", this.cleanDisplayName(value));
      }
    }

    public ExcelPivotCacheDefinition CacheDefinition
    {
      get
      {
        if (this._cacheDefinition == null)
          this._cacheDefinition = new ExcelPivotCacheDefinition(this.NameSpaceManager, this, (ExcelRangeBase) null, 1);
        return this._cacheDefinition;
      }
    }

    private string cleanDisplayName(string name) => Regex.Replace(name, "[^\\w\\.-_]", "_");

    public ExcelWorksheet WorkSheet { get; set; }

    public ExcelAddressBase Address { get; internal set; }

    public bool DataOnRows
    {
      get => this.GetXmlNodeBool("@dataOnRows");
      set => this.SetXmlNodeBool("@dataOnRows", value);
    }

    public bool ApplyNumberFormats
    {
      get => this.GetXmlNodeBool("@applyNumberFormats");
      set => this.SetXmlNodeBool("@applyNumberFormats", value);
    }

    public bool ApplyBorderFormats
    {
      get => this.GetXmlNodeBool("@applyBorderFormats");
      set => this.SetXmlNodeBool("@applyBorderFormats", value);
    }

    public bool ApplyFontFormats
    {
      get => this.GetXmlNodeBool("@applyFontFormats");
      set => this.SetXmlNodeBool("@applyFontFormats", value);
    }

    public bool ApplyPatternFormats
    {
      get => this.GetXmlNodeBool("@applyPatternFormats");
      set => this.SetXmlNodeBool("@applyPatternFormats", value);
    }

    public bool ApplyWidthHeightFormats
    {
      get => this.GetXmlNodeBool("@applyWidthHeightFormats");
      set => this.SetXmlNodeBool("@applyWidthHeightFormats", value);
    }

    public bool ShowMemberPropertyTips
    {
      get => this.GetXmlNodeBool("@showMemberPropertyTips");
      set => this.SetXmlNodeBool("@showMemberPropertyTips", value);
    }

    public bool ShowCalcMember
    {
      get => this.GetXmlNodeBool("@showCalcMbrs");
      set => this.SetXmlNodeBool("@showCalcMbrs", value);
    }

    public bool EnableDrill
    {
      get => this.GetXmlNodeBool("@enableDrill", true);
      set => this.SetXmlNodeBool("@enableDrill", value);
    }

    public bool ShowDrill
    {
      get => this.GetXmlNodeBool("@showDrill", true);
      set => this.SetXmlNodeBool("@showDrill", value);
    }

    public bool ShowDataTips
    {
      get => this.GetXmlNodeBool("@showDataTips", true);
      set => this.SetXmlNodeBool("@showDataTips", value, true);
    }

    public bool FieldPrintTitles
    {
      get => this.GetXmlNodeBool("@fieldPrintTitles");
      set => this.SetXmlNodeBool("@fieldPrintTitles", value);
    }

    public bool ItemPrintTitles
    {
      get => this.GetXmlNodeBool("@itemPrintTitles");
      set => this.SetXmlNodeBool("@itemPrintTitles", value);
    }

    public bool ColumGrandTotals
    {
      get => this.GetXmlNodeBool("@colGrandTotals");
      set => this.SetXmlNodeBool("@colGrandTotals", value);
    }

    public bool RowGrandTotals
    {
      get => this.GetXmlNodeBool("@rowGrandTotals");
      set => this.SetXmlNodeBool("@rowGrandTotals", value);
    }

    public bool PrintDrill
    {
      get => this.GetXmlNodeBool("@printDrill");
      set => this.SetXmlNodeBool("@printDrill", value);
    }

    public bool ShowError
    {
      get => this.GetXmlNodeBool("@showError");
      set => this.SetXmlNodeBool("@showError", value);
    }

    public string ErrorCaption
    {
      get => this.GetXmlNodeString("@errorCaption");
      set => this.SetXmlNodeString("@errorCaption", value);
    }

    public string DataCaption
    {
      get => this.GetXmlNodeString("@dataCaption");
      set => this.SetXmlNodeString("@dataCaption", value);
    }

    public bool ShowHeaders
    {
      get => this.GetXmlNodeBool("@showHeaders");
      set => this.SetXmlNodeBool("@showHeaders", value);
    }

    public int PageWrap
    {
      get => this.GetXmlNodeInt("@pageWrap");
      set
      {
        if (value < 0)
          throw new Exception("Value can't be negative");
        this.SetXmlNodeString("@pageWrap", value.ToString());
      }
    }

    public bool UseAutoFormatting
    {
      get => this.GetXmlNodeBool("@useAutoFormatting");
      set => this.SetXmlNodeBool("@useAutoFormatting", value);
    }

    public bool GridDropZones
    {
      get => this.GetXmlNodeBool("@gridDropZones");
      set => this.SetXmlNodeBool("@gridDropZones", value);
    }

    public int Indent
    {
      get => this.GetXmlNodeInt("@indent");
      set => this.SetXmlNodeString("@indent", value.ToString());
    }

    public bool OutlineData
    {
      get => this.GetXmlNodeBool("@outlineData");
      set => this.SetXmlNodeBool("@outlineData", value);
    }

    public bool Outline
    {
      get => this.GetXmlNodeBool("@outline");
      set => this.SetXmlNodeBool("@outline", value);
    }

    public bool MultipleFieldFilters
    {
      get => this.GetXmlNodeBool("@multipleFieldFilters");
      set => this.SetXmlNodeBool("@multipleFieldFilters", value);
    }

    public bool Compact
    {
      get => this.GetXmlNodeBool("@compact");
      set => this.SetXmlNodeBool("@compact", value);
    }

    public bool CompactData
    {
      get => this.GetXmlNodeBool("@compactData");
      set => this.SetXmlNodeBool("@compactData", value);
    }

    public string GrandTotalCaption
    {
      get => this.GetXmlNodeString("@grandTotalCaption");
      set => this.SetXmlNodeString("@grandTotalCaption", value);
    }

    public string RowHeaderCaption
    {
      get => this.GetXmlNodeString("@rowHeaderCaption");
      set => this.SetXmlNodeString("@rowHeaderCaption", value);
    }

    public string MissingCaption
    {
      get => this.GetXmlNodeString("@missingCaption");
      set => this.SetXmlNodeString("@missingCaption", value);
    }

    public int FirstHeaderRow
    {
      get => this.GetXmlNodeInt("d:location/@firstHeaderRow");
      set => this.SetXmlNodeString("d:location/@firstHeaderRow", value.ToString());
    }

    public int FirstDataRow
    {
      get => this.GetXmlNodeInt("d:location/@firstDataRow");
      set => this.SetXmlNodeString("d:location/@firstDataRow", value.ToString());
    }

    public int FirstDataCol
    {
      get => this.GetXmlNodeInt("d:location/@firstDataCol");
      set => this.SetXmlNodeString("d:location/@firstDataCol", value.ToString());
    }

    public ExcelPivotTableFieldCollection Fields
    {
      get
      {
        if (this._fields == null)
          this._fields = new ExcelPivotTableFieldCollection(this, "");
        return this._fields;
      }
    }

    public ExcelPivotTableRowColumnFieldCollection RowFields
    {
      get
      {
        if (this._rowFields == null)
          this._rowFields = new ExcelPivotTableRowColumnFieldCollection(this, "rowFields");
        return this._rowFields;
      }
    }

    public ExcelPivotTableRowColumnFieldCollection ColumnFields
    {
      get
      {
        if (this._columnFields == null)
          this._columnFields = new ExcelPivotTableRowColumnFieldCollection(this, "colFields");
        return this._columnFields;
      }
    }

    public ExcelPivotTableDataFieldCollection DataFields
    {
      get
      {
        if (this._dataFields == null)
          this._dataFields = new ExcelPivotTableDataFieldCollection(this);
        return this._dataFields;
      }
    }

    public ExcelPivotTableRowColumnFieldCollection PageFields
    {
      get
      {
        if (this._pageFields == null)
          this._pageFields = new ExcelPivotTableRowColumnFieldCollection(this, "pageFields");
        return this._pageFields;
      }
    }

    public string StyleName
    {
      get => this.GetXmlNodeString("d:pivotTableStyleInfo/@name");
      set
      {
        if (value.StartsWith("PivotStyle"))
        {
          try
          {
            this._tableStyle = (TableStyles) Enum.Parse(typeof (TableStyles), value.Substring(10, value.Length - 10), true);
          }
          catch
          {
            this._tableStyle = TableStyles.Custom;
          }
        }
        else if (value == "None")
        {
          this._tableStyle = TableStyles.None;
          value = "";
        }
        else
          this._tableStyle = TableStyles.Custom;
        this.SetXmlNodeString("d:pivotTableStyleInfo/@name", value, true);
      }
    }

    public TableStyles TableStyle
    {
      get => this._tableStyle;
      set
      {
        this._tableStyle = value;
        if (value == TableStyles.Custom)
          return;
        this.SetXmlNodeString("d:pivotTableStyleInfo/@name", "PivotStyle" + value.ToString());
      }
    }

    internal int CacheID
    {
      get => this.GetXmlNodeInt("@cacheId");
      set => this.SetXmlNodeString("@cacheId", value.ToString());
    }
  }
}
