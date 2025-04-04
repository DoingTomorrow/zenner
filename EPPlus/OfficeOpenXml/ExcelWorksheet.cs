// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelWorksheet
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Drawing.Vml;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using OfficeOpenXml.Utils;
using OfficeOpenXml.VBA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelWorksheet : XmlHelper, IDisposable
  {
    private const string outLineSummaryBelowPath = "d:sheetPr/d:outlinePr/@summaryBelow";
    private const string outLineSummaryRightPath = "d:sheetPr/d:outlinePr/@summaryRight";
    private const string outLineApplyStylePath = "d:sheetPr/d:outlinePr/@applyStyles";
    private const string tabColorPath = "d:sheetPr/d:tabColor/@rgb";
    private const string codeModuleNamePath = "d:sheetPr/@codeName";
    private const int BLOCKSIZE = 8192;
    internal CellStore<object> _values;
    internal CellStore<string> _types;
    internal CellStore<int> _styles;
    internal CellStore<object> _formulas;
    internal FlagCellStore _flags;
    internal CellStore<List<OfficeOpenXml.FormulaParsing.LexicalAnalysis.Token>> _formulaTokens;
    internal CellStore<Uri> _hyperLinks;
    internal CellStore<ExcelComment> _commentsStore;
    internal Dictionary<int, ExcelWorksheet.Formulas> _sharedFormulas = new Dictionary<int, ExcelWorksheet.Formulas>();
    internal int _minCol = 16384;
    internal int _maxCol;
    internal ExcelPackage _package;
    private Uri _worksheetUri;
    private string _name;
    private int _sheetID;
    private int _positionID;
    private string _relationshipID;
    private XmlDocument _worksheetXml;
    internal ExcelWorksheetView _sheetView;
    internal ExcelHeaderFooter _headerFooter;
    internal ExcelNamedRangeCollection _names;
    private double _defaultRowHeight = double.NaN;
    internal ExcelVmlDrawingCommentCollection _vmlDrawings;
    internal ExcelCommentCollection _comments;
    private ExcelWorksheet.MergeCellsCollection<string> _mergedCells = new ExcelWorksheet.MergeCellsCollection<string>();
    private ExcelSheetProtection _protection;
    private ExcelProtectedRangeCollection _protectedRanges;
    private ExcelDrawings _drawings;
    private ExcelTableCollection _tables;
    private ExcelPivotTableCollection _pivotTables;
    private ExcelConditionalFormattingCollection _conditionalFormatting;
    private ExcelDataValidationCollection _dataValidation;
    private ExcelBackgroundImage _backgroundImage;

    public ExcelWorksheet(
      XmlNamespaceManager ns,
      ExcelPackage excelPackage,
      string relID,
      Uri uriWorksheet,
      string sheetName,
      int sheetID,
      int positionID,
      eWorkSheetHidden hide)
      : base(ns, (XmlNode) null)
    {
      this.SchemaNodeOrder = new string[42]
      {
        "sheetPr",
        "tabColor",
        "outlinePr",
        "pageSetUpPr",
        "dimension",
        "sheetViews",
        "sheetFormatPr",
        "cols",
        "sheetData",
        "sheetProtection",
        "protectedRanges",
        "scenarios",
        "autoFilter",
        "sortState",
        "dataConsolidate",
        "customSheetViews",
        "customSheetViews",
        "mergeCells",
        "phoneticPr",
        "conditionalFormatting",
        "dataValidations",
        "hyperlinks",
        "printOptions",
        "pageMargins",
        "pageSetup",
        "headerFooter",
        "linePrint",
        "rowBreaks",
        "colBreaks",
        "customProperties",
        "cellWatches",
        "ignoredErrors",
        "smartTags",
        "drawing",
        "legacyDrawing",
        "legacyDrawingHF",
        "picture",
        "oleObjects",
        "activeXControls",
        "webPublishItems",
        "tableParts",
        "extLst"
      };
      this._package = excelPackage;
      this._relationshipID = relID;
      this._worksheetUri = uriWorksheet;
      this._name = sheetName;
      this._sheetID = sheetID;
      this._positionID = positionID;
      this.Hidden = hide;
      this._values = new CellStore<object>();
      this._types = new CellStore<string>();
      this._styles = new CellStore<int>();
      this._formulas = new CellStore<object>();
      this._flags = new FlagCellStore();
      this._commentsStore = new CellStore<ExcelComment>();
      this._hyperLinks = new CellStore<Uri>();
      this._names = new ExcelNamedRangeCollection(this.Workbook, this);
      this.CreateXml();
      this.TopNode = (XmlNode) this._worksheetXml.DocumentElement;
    }

    internal Uri WorksheetUri => this._worksheetUri;

    internal ZipPackagePart Part => this._package.Package.GetPart(this.WorksheetUri);

    internal string RelationshipID => this._relationshipID;

    internal int SheetID => this._sheetID;

    internal int PositionID
    {
      get => this._positionID;
      set => this._positionID = value;
    }

    public int Index => this._positionID;

    public ExcelAddressBase AutoFilterAddress
    {
      get
      {
        this.CheckSheetType();
        string xmlNodeString = this.GetXmlNodeString("d:autoFilter/@ref");
        return xmlNodeString == "" ? (ExcelAddressBase) null : new ExcelAddressBase(xmlNodeString);
      }
      internal set
      {
        this.CheckSheetType();
        this.SetXmlNodeString("d:autoFilter/@ref", value.Address);
      }
    }

    internal void CheckSheetType()
    {
      if (this is ExcelChartsheet)
        throw new NotSupportedException("This property or method is not supported for a Chartsheet");
    }

    public ExcelWorksheetView View
    {
      get
      {
        if (this._sheetView == null)
        {
          XmlNode node = this.TopNode.SelectSingleNode("d:sheetViews/d:sheetView", this.NameSpaceManager);
          if (node == null)
          {
            this.CreateNode("d:sheetViews/d:sheetView");
            node = this.TopNode.SelectSingleNode("d:sheetViews/d:sheetView", this.NameSpaceManager);
          }
          this._sheetView = new ExcelWorksheetView(this.NameSpaceManager, node, this);
        }
        return this._sheetView;
      }
    }

    public string Name
    {
      get => this._name;
      set
      {
        if (value == this._name)
          return;
        value = this._package.Workbook.Worksheets.ValidateFixSheetName(value);
        foreach (ExcelWorksheet worksheet in this.Workbook.Worksheets)
        {
          if (worksheet.PositionID != this.PositionID && worksheet.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException("Worksheet name must be unique");
        }
        this._package.Workbook.SetXmlNodeString(string.Format("d:sheets/d:sheet[@sheetId={0}]/@name", (object) this._sheetID), value);
        this.ChangeNames(value);
        this._name = value;
      }
    }

    private void ChangeNames(string value)
    {
      foreach (ExcelNamedRange name in this.Workbook.Names)
      {
        if (string.IsNullOrEmpty(name.NameFormula) && name.NameValue == null)
          name.ChangeWorksheet(this._name, value);
      }
      foreach (ExcelWorksheet worksheet in this.Workbook.Worksheets)
      {
        if (!(worksheet is ExcelChartsheet))
        {
          foreach (ExcelNamedRange name in worksheet.Names)
          {
            if (string.IsNullOrEmpty(name.NameFormula) && name.NameValue == null)
              name.ChangeWorksheet(this._name, value);
          }
        }
      }
    }

    public ExcelNamedRangeCollection Names
    {
      get
      {
        this.CheckSheetType();
        return this._names;
      }
    }

    public eWorkSheetHidden Hidden
    {
      get
      {
        switch (this._package.Workbook.GetXmlNodeString(string.Format("d:sheets/d:sheet[@sheetId={0}]/@state", (object) this._sheetID)))
        {
          case "hidden":
            return eWorkSheetHidden.Hidden;
          case "veryHidden":
            return eWorkSheetHidden.VeryHidden;
          default:
            return eWorkSheetHidden.Visible;
        }
      }
      set
      {
        if (value == eWorkSheetHidden.Visible)
        {
          this._package.Workbook.DeleteNode(string.Format("d:sheets/d:sheet[@sheetId={0}]/@state", (object) this._sheetID));
        }
        else
        {
          string str1 = value.ToString();
          string str2 = str1.Substring(0, 1).ToLower() + str1.Substring(1);
          this._package.Workbook.SetXmlNodeString(string.Format("d:sheets/d:sheet[@sheetId={0}]/@state", (object) this._sheetID), str2);
        }
      }
    }

    public double DefaultRowHeight
    {
      get
      {
        this.CheckSheetType();
        if (double.IsNaN(this._defaultRowHeight))
        {
          this._defaultRowHeight = this.GetXmlNodeDouble("d:sheetFormatPr/@defaultRowHeight");
          if (double.IsNaN(this._defaultRowHeight))
            this._defaultRowHeight = 15.0;
        }
        return this._defaultRowHeight;
      }
      set
      {
        this.CheckSheetType();
        this._defaultRowHeight = value;
        this.SetXmlNodeString("d:sheetFormatPr/@defaultRowHeight", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        this.SetXmlNodeBool("d:sheetFormatPr/@customHeight", value != 15.0);
        if (!double.IsNaN(this.GetXmlNodeDouble("d:sheetFormatPr/@defaultColWidth")))
          return;
        this.DefaultColWidth = 585.0 / 64.0;
      }
    }

    public double DefaultColWidth
    {
      get
      {
        this.CheckSheetType();
        double d = this.GetXmlNodeDouble("d:sheetFormatPr/@defaultColWidth");
        if (double.IsNaN(d))
          d = 585.0 / 64.0;
        return d;
      }
      set
      {
        this.CheckSheetType();
        this.SetXmlNodeString("d:sheetFormatPr/@defaultColWidth", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        if (!double.IsNaN(this.GetXmlNodeDouble("d:sheetFormatPr/@defaultRowHeight")))
          return;
        this.DefaultRowHeight = 15.0;
      }
    }

    public bool OutLineSummaryBelow
    {
      get
      {
        this.CheckSheetType();
        return this.GetXmlNodeBool("d:sheetPr/d:outlinePr/@summaryBelow");
      }
      set
      {
        this.CheckSheetType();
        this.SetXmlNodeString("d:sheetPr/d:outlinePr/@summaryBelow", value ? "1" : "0");
      }
    }

    public bool OutLineSummaryRight
    {
      get
      {
        this.CheckSheetType();
        return this.GetXmlNodeBool("d:sheetPr/d:outlinePr/@summaryRight");
      }
      set
      {
        this.CheckSheetType();
        this.SetXmlNodeString("d:sheetPr/d:outlinePr/@summaryRight", value ? "1" : "0");
      }
    }

    public bool OutLineApplyStyle
    {
      get
      {
        this.CheckSheetType();
        return this.GetXmlNodeBool("d:sheetPr/d:outlinePr/@applyStyles");
      }
      set
      {
        this.CheckSheetType();
        this.SetXmlNodeString("d:sheetPr/d:outlinePr/@applyStyles", value ? "1" : "0");
      }
    }

    public Color TabColor
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:sheetPr/d:tabColor/@rgb");
        return xmlNodeString == "" ? Color.Empty : Color.FromArgb(int.Parse(xmlNodeString, NumberStyles.AllowHexSpecifier));
      }
      set => this.SetXmlNodeString("d:sheetPr/d:tabColor/@rgb", value.ToArgb().ToString("X"));
    }

    internal string CodeModuleName
    {
      get => this.GetXmlNodeString("d:sheetPr/@codeName");
      set => this.SetXmlNodeString("d:sheetPr/@codeName", value);
    }

    internal void CodeNameChange(string value) => this.CodeModuleName = value;

    public ExcelVBAModule CodeModule
    {
      get
      {
        return this._package.Workbook.VbaProject != null ? this._package.Workbook.VbaProject.Modules[this.CodeModuleName] : (ExcelVBAModule) null;
      }
    }

    public XmlDocument WorksheetXml => this._worksheetXml;

    internal ExcelVmlDrawingCommentCollection VmlDrawingsComments
    {
      get
      {
        if (this._vmlDrawings == null)
          this.CreateVmlCollection();
        return this._vmlDrawings;
      }
    }

    public ExcelCommentCollection Comments
    {
      get
      {
        this.CheckSheetType();
        if (this._comments == null)
        {
          this.CreateVmlCollection();
          this._comments = new ExcelCommentCollection(this._package, this, this.NameSpaceManager);
        }
        return this._comments;
      }
    }

    private void CreateVmlCollection()
    {
      XmlNode xmlNode = this._worksheetXml.DocumentElement.SelectSingleNode("d:legacyDrawing/@r:id", this.NameSpaceManager);
      if (xmlNode == null)
      {
        this._vmlDrawings = new ExcelVmlDrawingCommentCollection(this._package, this, (Uri) null);
      }
      else
      {
        if (!this.Part.RelationshipExists(xmlNode.Value))
          return;
        ZipPackageRelationship relationship = this.Part.GetRelationship(xmlNode.Value);
        this._vmlDrawings = new ExcelVmlDrawingCommentCollection(this._package, this, UriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri));
        this._vmlDrawings.RelId = relationship.Id;
      }
    }

    private void CreateXml()
    {
      this._worksheetXml = new XmlDocument();
      this._worksheetXml.PreserveWhitespace = false;
      ZipPackagePart part = this._package.Package.GetPart(this.WorksheetUri);
      bool doAdjustDrawings = this._package.DoAdjustDrawings;
      this._package.DoAdjustDrawings = false;
      Stream stream = (Stream) part.GetStream();
      XmlTextReader xr = new XmlTextReader(stream);
      xr.ProhibitDtd = true;
      this.LoadColumns(xr);
      long position1 = stream.Position;
      this.LoadCells(xr);
      long position2 = stream.Position;
      this.LoadMergeCells(xr);
      this.LoadHyperLinks(xr);
      this.LoadRowPageBreakes(xr);
      this.LoadColPageBreakes(xr);
      stream.Seek(0L, SeekOrigin.Begin);
      Encoding encoding;
      string workSheetXml = this.GetWorkSheetXml(stream, position1, position2, out encoding);
      if (workSheetXml[0] != '<')
        XmlHelper.LoadXmlSafe(this._worksheetXml, workSheetXml.Substring(1, workSheetXml.Length - 1), encoding);
      else
        XmlHelper.LoadXmlSafe(this._worksheetXml, workSheetXml, encoding);
      this._package.DoAdjustDrawings = doAdjustDrawings;
      this.ClearNodes();
    }

    private void LoadRowPageBreakes(XmlTextReader xr)
    {
      if (!this.ReadUntil(xr, "rowBreaks", "colBreaks"))
        return;
      while (xr.Read() && xr.LocalName == "brk")
      {
        int result;
        if (xr.NodeType == XmlNodeType.Element && int.TryParse(xr.GetAttribute("id"), out result))
          this.Row(result).PageBreak = true;
      }
    }

    private void LoadColPageBreakes(XmlTextReader xr)
    {
      if (!this.ReadUntil(xr, "colBreaks"))
        return;
      while (xr.Read() && xr.LocalName == "brk")
      {
        int result;
        if (xr.NodeType == XmlNodeType.Element && int.TryParse(xr.GetAttribute("id"), out result))
          this.Column(result).PageBreak = true;
      }
    }

    private void ClearNodes()
    {
      if (this._worksheetXml.SelectSingleNode("//d:cols", this.NameSpaceManager) != null)
        this._worksheetXml.SelectSingleNode("//d:cols", this.NameSpaceManager).RemoveAll();
      if (this._worksheetXml.SelectSingleNode("//d:mergeCells", this.NameSpaceManager) != null)
        this._worksheetXml.SelectSingleNode("//d:mergeCells", this.NameSpaceManager).RemoveAll();
      if (this._worksheetXml.SelectSingleNode("//d:hyperlinks", this.NameSpaceManager) != null)
        this._worksheetXml.SelectSingleNode("//d:hyperlinks", this.NameSpaceManager).RemoveAll();
      if (this._worksheetXml.SelectSingleNode("//d:rowBreaks", this.NameSpaceManager) != null)
        this._worksheetXml.SelectSingleNode("//d:rowBreaks", this.NameSpaceManager).RemoveAll();
      if (this._worksheetXml.SelectSingleNode("//d:colBreaks", this.NameSpaceManager) == null)
        return;
      this._worksheetXml.SelectSingleNode("//d:colBreaks", this.NameSpaceManager).RemoveAll();
    }

    private string GetWorkSheetXml(Stream stream, long start, long end, out Encoding encoding)
    {
      StreamReader streamReader = new StreamReader(stream);
      int num = 0;
      StringBuilder stringBuilder1 = new StringBuilder();
      do
      {
        int count = stream.Length < 8192L ? (int) stream.Length : 8192;
        char[] buffer = new char[count];
        int charCount = streamReader.ReadBlock(buffer, 0, count);
        stringBuilder1.Append(buffer, 0, charCount);
        num += count;
      }
      while ((long) num < start + 20L && (long) num < end);
      Match match1 = Regex.Match(stringBuilder1.ToString(), string.Format("(<[^>]*{0}[^>]*>)", (object) "sheetData"));
      if (!match1.Success)
      {
        encoding = streamReader.CurrentEncoding;
        return stringBuilder1.ToString();
      }
      string input = stringBuilder1.ToString();
      string str = input.Substring(0, match1.Index);
      string workSheetXml;
      if (match1.Value.EndsWith("/>"))
      {
        workSheetXml = str + input.Substring(match1.Index, input.Length - match1.Index);
      }
      else
      {
        if (streamReader.Peek() != -1 && end - 8192L > 0L)
        {
          long offset = end - 8192L - 4096L < 0L ? 0L : end - 8192L - 4096L;
          stream.Seek(offset, SeekOrigin.Begin);
          int count = (int) (stream.Length - offset);
          char[] buffer = new char[count];
          streamReader = new StreamReader(stream);
          int charCount = streamReader.ReadBlock(buffer, 0, count);
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.Append(buffer, 0, charCount);
          input = stringBuilder2.ToString();
        }
        Match match2 = Regex.Match(input, string.Format("(</[^>]*{0}[^>]*>)", (object) "sheetData"));
        workSheetXml = str + "<sheetData/>" + input.Substring(match2.Index + match2.Length, input.Length - (match2.Index + match2.Length));
      }
      if (streamReader.Peek() > -1)
        workSheetXml += streamReader.ReadToEnd();
      encoding = streamReader.CurrentEncoding;
      return workSheetXml;
    }

    private void GetBlockPos(string xml, string tag, ref int start, ref int end)
    {
      Match match1 = Regex.Match(xml.Substring(start), string.Format("(<[^>]*{0}[^>]*>)", (object) tag));
      if (!match1.Success)
      {
        start = -1;
        end = -1;
      }
      else
      {
        int num = match1.Index + start;
        if (match1.Value.Substring(match1.Value.Length - 2, 1) == "/")
        {
          end = num + match1.Length;
        }
        else
        {
          Match match2 = Regex.Match(xml.Substring(start), string.Format("(</[^>]*{0}[^>]*>)", (object) tag));
          if (match2.Success)
            end = match2.Index + match2.Length + start;
        }
        start = num;
      }
    }

    private bool ReadUntil(XmlTextReader xr, params string[] tagName)
    {
      if (xr.EOF)
        return false;
      while (!Array.Exists<string>(tagName, (Predicate<string>) (tag => xr.LocalName.EndsWith(tag))))
      {
        xr.Read();
        if (xr.EOF)
          return false;
      }
      return xr.LocalName.EndsWith(tagName[0]);
    }

    private void LoadColumns(XmlTextReader xr)
    {
      List<IRangeID> rangeIdList = new List<IRangeID>();
      if (!this.ReadUntil(xr, "cols", "sheetData"))
        return;
      while (xr.Read() && !(xr.LocalName != "col"))
      {
        if (xr.NodeType == XmlNodeType.Element)
        {
          int num = int.Parse(xr.GetAttribute("min"));
          this._values.SetValue(0, num, (object) new ExcelColumn(this, num)
          {
            ColumnMax = int.Parse(xr.GetAttribute("max")),
            Width = (xr.GetAttribute("width") == null ? 0.0 : double.Parse(xr.GetAttribute("width"), (IFormatProvider) CultureInfo.InvariantCulture)),
            BestFit = (xr.GetAttribute("bestFit") != null && xr.GetAttribute("bestFit") == "1"),
            Collapsed = (xr.GetAttribute("collapsed") != null && xr.GetAttribute("collapsed") == "1"),
            Phonetic = (xr.GetAttribute("phonetic") != null && xr.GetAttribute("phonetic") == "1"),
            OutlineLevel = (xr.GetAttribute("outlineLevel") == null ? 0 : (int) (short) int.Parse(xr.GetAttribute("outlineLevel"), (IFormatProvider) CultureInfo.InvariantCulture)),
            Hidden = (xr.GetAttribute("hidden") != null && xr.GetAttribute("hidden") == "1")
          });
          int result;
          if (xr.GetAttribute("style") != null && int.TryParse(xr.GetAttribute("style"), out result))
            this._styles.SetValue(0, num, result);
        }
      }
    }

    private static bool ReadXmlReaderUntil(XmlTextReader xr, string nodeText, string altNode)
    {
      while (!(xr.LocalName == nodeText) && !(xr.LocalName == altNode))
      {
        if (!xr.Read())
        {
          xr.Close();
          return false;
        }
      }
      return true;
    }

    private void LoadHyperLinks(XmlTextReader xr)
    {
      if (!this.ReadUntil(xr, "hyperlinks", "rowBreaks", "colBreaks"))
        return;
      while (xr.Read() && xr.LocalName == "hyperlink")
      {
        int FromRow;
        int FromColumn;
        int ToRow;
        int ToColumn;
        ExcelCellBase.GetRowColFromAddress(xr.GetAttribute("ref"), out FromRow, out FromColumn, out ToRow, out ToColumn);
        ExcelHyperLink excelHyperLink = (ExcelHyperLink) null;
        if (xr.GetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships") != null)
        {
          string attribute = xr.GetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
          Uri targetUri = this.Part.GetRelationship(attribute).TargetUri;
          excelHyperLink = !targetUri.IsAbsoluteUri ? new ExcelHyperLink(targetUri.OriginalString, UriKind.Relative) : new ExcelHyperLink(targetUri.AbsoluteUri);
          excelHyperLink.RId = attribute;
          this.Part.DeleteRelationship(attribute);
        }
        else if (xr.GetAttribute("location") != null)
        {
          excelHyperLink = new ExcelHyperLink(xr.GetAttribute("location"), xr.GetAttribute("display"));
          excelHyperLink.RowSpann = ToRow - FromRow;
          excelHyperLink.ColSpann = ToColumn - FromColumn;
        }
        string attribute1 = xr.GetAttribute("tooltip");
        if (!string.IsNullOrEmpty(attribute1))
          excelHyperLink.ToolTip = attribute1;
        this._hyperLinks.SetValue(FromRow, FromColumn, (Uri) excelHyperLink);
      }
    }

    private void LoadCells(XmlTextReader xr)
    {
      this.ReadUntil(xr, "sheetData", "mergeCells", "hyperlinks", "rowBreaks", "colBreaks");
      ExcelAddressBase excelAddressBase = (ExcelAddressBase) null;
      string type = "";
      int styleID = 0;
      int num1 = 0;
      int num2 = 0;
      xr.Read();
      while (!xr.EOF)
      {
        while (xr.NodeType == XmlNodeType.EndElement)
          xr.Read();
        if (xr.LocalName == "row")
        {
          string attribute = xr.GetAttribute("r");
          if (attribute == null)
            ++num1;
          else
            num1 = Convert.ToInt32(attribute);
          if (this.DoAddRow(xr))
          {
            this._values.SetValue(num1, 0, (object) this.AddRow(xr, num1));
            if (xr.GetAttribute("s") != null)
              this._styles.SetValue(num1, 0, int.Parse(xr.GetAttribute("s"), (IFormatProvider) CultureInfo.InvariantCulture));
          }
          xr.Read();
        }
        else if (xr.LocalName == "c")
        {
          string attribute = xr.GetAttribute("r");
          if (attribute == null)
          {
            ++num2;
            excelAddressBase = new ExcelAddressBase(num1, num2, num1, num2);
          }
          else
          {
            excelAddressBase = new ExcelAddressBase(attribute);
            num2 = excelAddressBase._fromCol;
          }
          if (xr.GetAttribute("t") != null)
          {
            type = xr.GetAttribute("t");
            this._types.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, type);
          }
          else
            type = "";
          if (xr.GetAttribute("s") != null)
          {
            styleID = int.Parse(xr.GetAttribute("s"));
            this._styles.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, styleID);
            this._values.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) null);
          }
          else
            styleID = 0;
          xr.Read();
        }
        else if (xr.LocalName == "v")
        {
          this.SetValueFromXml(xr, type, styleID, excelAddressBase._fromRow, excelAddressBase._fromCol);
          xr.Read();
        }
        else if (xr.LocalName == "f")
        {
          switch (xr.GetAttribute("t"))
          {
            case null:
              this._formulas.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) xr.ReadElementContentAsString());
              this._values.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) null);
              continue;
            case "shared":
              string attribute1 = xr.GetAttribute("si");
              if (attribute1 != null)
              {
                int key = int.Parse(attribute1);
                this._formulas.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) key);
                this._values.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) null);
                string attribute2 = xr.GetAttribute("ref");
                string str = xr.ReadElementContentAsString();
                if (str != "")
                {
                  this._sharedFormulas.Add(key, new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default)
                  {
                    Index = key,
                    Formula = str,
                    Address = attribute2,
                    StartRow = excelAddressBase._fromRow,
                    StartCol = excelAddressBase._fromCol
                  });
                  continue;
                }
                continue;
              }
              xr.Read();
              continue;
            case "array":
              string attribute3 = xr.GetAttribute("ref");
              string str1 = xr.ReadElementContentAsString();
              int shareFunctionIndex = this.GetMaxShareFunctionIndex(true);
              this._formulas.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) shareFunctionIndex.ToString());
              this._values.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) null);
              this._sharedFormulas.Add(shareFunctionIndex, new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default)
              {
                Index = shareFunctionIndex,
                Formula = str1,
                Address = attribute3,
                StartRow = excelAddressBase._fromRow,
                StartCol = excelAddressBase._fromCol,
                IsArray = true
              });
              continue;
            default:
              xr.Read();
              continue;
          }
        }
        else
        {
          if (!(xr.LocalName == "is"))
            break;
          xr.Read();
          if (xr.LocalName == "t")
          {
            this._values.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) xr.ReadInnerXml());
          }
          else
          {
            this._values.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, (object) xr.ReadOuterXml());
            this._types.SetValue(excelAddressBase._fromRow, excelAddressBase._fromCol, "rt");
            this._flags.SetFlagValue(excelAddressBase._fromRow, excelAddressBase._fromCol, true, CellFlags.RichText);
          }
        }
      }
    }

    private bool DoAddRow(XmlTextReader xr)
    {
      int num = xr.GetAttribute("r") == null ? 0 : 1;
      if (xr.GetAttribute("spans") != null)
        ++num;
      return xr.AttributeCount > num;
    }

    private void LoadMergeCells(XmlTextReader xr)
    {
      if (!this.ReadUntil(xr, "mergeCells", "hyperlinks", "rowBreaks", "colBreaks") || xr.EOF)
        return;
      while (xr.Read() && !(xr.LocalName != "mergeCell"))
      {
        if (xr.NodeType == XmlNodeType.Element)
        {
          string attribute = xr.GetAttribute("ref");
          int FromRow;
          int FromColumn;
          int ToRow;
          int ToColumn;
          ExcelCellBase.GetRowColFromAddress(attribute, out FromRow, out FromColumn, out ToRow, out ToColumn);
          for (int Row = FromRow; Row <= ToRow; ++Row)
          {
            for (int Col = FromColumn; Col <= ToColumn; ++Col)
              this._flags.SetFlagValue(Row, Col, true, CellFlags.Merged);
          }
          this._mergedCells.List.Add(attribute);
        }
      }
    }

    private void UpdateMergedCells(StreamWriter sw)
    {
      sw.Write("<mergeCells>");
      foreach (string mergedCell in this._mergedCells)
        sw.Write("<mergeCell ref=\"{0}\" />", (object) mergedCell);
      sw.Write("</mergeCells>");
    }

    private RowInternal AddRow(XmlTextReader xr, int row)
    {
      return new RowInternal()
      {
        Collapsed = xr.GetAttribute("collapsed") != null && xr.GetAttribute("collapsed") == "1",
        OutlineLevel = xr.GetAttribute("outlineLevel") == null ? (short) 0 : short.Parse(xr.GetAttribute("outlineLevel"), (IFormatProvider) CultureInfo.InvariantCulture),
        Height = xr.GetAttribute("ht") == null ? -1.0 : double.Parse(xr.GetAttribute("ht"), (IFormatProvider) CultureInfo.InvariantCulture),
        Hidden = xr.GetAttribute("hidden") != null && xr.GetAttribute("hidden") == "1",
        Phonetic = xr.GetAttribute("ph") != null && xr.GetAttribute("ph") == "1",
        CustomHeight = xr.GetAttribute("customHeight") != null && xr.GetAttribute("customHeight") == "1"
      };
    }

    private void SetValueFromXml(XmlTextReader xr, string type, int styleID, int row, int col)
    {
      switch (type)
      {
        case "s":
          int index = xr.ReadElementContentAsInt();
          this._values.SetValue(row, col, (object) this._package.Workbook._sharedStringsList[index].Text);
          if (!this._package.Workbook._sharedStringsList[index].isRichText)
            break;
          this._flags.SetFlagValue(row, col, true, CellFlags.RichText);
          break;
        case "str":
          this._values.SetValue(row, col, (object) xr.ReadElementContentAsString());
          break;
        case "b":
          this._values.SetValue(row, col, (object) (xr.ReadElementContentAsString() != "0"));
          break;
        case "e":
          this._values.SetValue(row, col, this.GetErrorType(xr.ReadElementContentAsString()));
          break;
        default:
          string s = xr.ReadElementContentAsString();
          int numberFormatId = this.Workbook.Styles.CellXfs[styleID].NumberFormatId;
          if (numberFormatId >= 14 && numberFormatId <= 22 || numberFormatId >= 45 && numberFormatId <= 47)
          {
            double result;
            if (double.TryParse(s, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
            {
              if (this.Workbook.Date1904)
                result += 1462.0;
              this._values.SetValue(row, col, (object) DateTime.FromOADate(result));
              break;
            }
            this._values.SetValue(row, col, (object) "");
            break;
          }
          double result1;
          if (double.TryParse(s, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
          {
            this._values.SetValue(row, col, (object) result1);
            break;
          }
          this._values.SetValue(row, col, (object) double.NaN);
          break;
      }
    }

    private object GetErrorType(string v) => (object) ExcelErrorValue.Parse(v.ToUpper());

    public ExcelHeaderFooter HeaderFooter
    {
      get
      {
        if (this._headerFooter == null)
          this._headerFooter = new ExcelHeaderFooter(this.NameSpaceManager, this.TopNode.SelectSingleNode("d:headerFooter", this.NameSpaceManager) ?? this.CreateNode("d:headerFooter"), this);
        return this._headerFooter;
      }
    }

    public ExcelPrinterSettings PrinterSettings
    {
      get
      {
        ExcelPrinterSettings printerSettings = new ExcelPrinterSettings(this.NameSpaceManager, this.TopNode, this);
        printerSettings.SchemaNodeOrder = this.SchemaNodeOrder;
        return printerSettings;
      }
    }

    public ExcelRange Cells
    {
      get
      {
        this.CheckSheetType();
        return new ExcelRange(this, 1, 1, 1048576, 16384);
      }
    }

    public ExcelRange SelectedRange
    {
      get
      {
        this.CheckSheetType();
        return new ExcelRange(this, this.View.SelectedRange);
      }
    }

    public ExcelWorksheet.MergeCellsCollection<string> MergedCells
    {
      get
      {
        this.CheckSheetType();
        return this._mergedCells;
      }
    }

    public ExcelRow Row(int row)
    {
      this.CheckSheetType();
      return row >= 1 && row <= 1048576 ? new ExcelRow(this, row) : throw new ArgumentException("Row number out of bounds");
    }

    public ExcelColumn Column(int col)
    {
      this.CheckSheetType();
      if (col < 1 || col > 16384)
        throw new ArgumentException("Column number out of bounds");
      if (this._values.GetValue(0, col) is ExcelColumn c1)
      {
        if (c1.ColumnMin != c1.ColumnMax)
        {
          int columnMax = c1.ColumnMax;
          c1.ColumnMax = col;
          this.CopyColumn(c1, col + 1, columnMax);
        }
      }
      else
      {
        int row = 0;
        int col1 = col;
        if (this._values.PrevCell(ref row, ref col1))
        {
          ExcelColumn c = this._values.GetValue(0, col1) as ExcelColumn;
          int columnMax = c.ColumnMax;
          if (columnMax >= col)
          {
            c.ColumnMax = col - 1;
            if (columnMax > col)
              this.CopyColumn(c, col + 1, columnMax);
            return this.CopyColumn(c, col, col);
          }
        }
        c1 = new ExcelColumn(this, col);
        this._values.SetValue(0, col, (object) c1);
      }
      return c1;
    }

    public override string ToString() => this.Name;

    internal ExcelColumn CopyColumn(ExcelColumn c, int col, int maxCol)
    {
      ExcelColumn excelColumn = new ExcelColumn(this, col);
      excelColumn.ColumnMax = maxCol;
      if (c.StyleName != "")
        excelColumn.StyleName = c.StyleName;
      else
        excelColumn.StyleID = c.StyleID;
      excelColumn._hidden = c.Hidden;
      excelColumn.OutlineLevel = c.OutlineLevel;
      excelColumn.Phonetic = c.Phonetic;
      excelColumn.BestFit = c.BestFit;
      this._values.SetValue(0, col, (object) excelColumn);
      excelColumn.Width = c._width;
      return excelColumn;
    }

    public void Select() => this.View.TabSelected = true;

    public void Select(string Address) => this.Select(Address, true);

    public void Select(string Address, bool SelectSheet)
    {
      this.CheckSheetType();
      int FromRow;
      int FromColumn;
      ExcelCellBase.GetRowColFromAddress(Address, out FromRow, out FromColumn, out int _, out int _);
      if (SelectSheet)
        this.View.TabSelected = true;
      this.View.SelectedRange = Address;
      this.View.ActiveCell = ExcelCellBase.GetAddress(FromRow, FromColumn);
    }

    public void Select(ExcelAddress Address)
    {
      this.CheckSheetType();
      this.Select(Address, true);
    }

    public void Select(ExcelAddress Address, bool SelectSheet)
    {
      this.CheckSheetType();
      if (SelectSheet)
        this.View.TabSelected = true;
      string str = ExcelCellBase.GetAddress(Address.Start.Row, Address.Start.Column) + ":" + ExcelCellBase.GetAddress(Address.End.Row, Address.End.Column);
      if (Address.Addresses != null)
      {
        foreach (ExcelAddress address in Address.Addresses)
          str = str + " " + ExcelCellBase.GetAddress(address.Start.Row, address.Start.Column) + ":" + ExcelCellBase.GetAddress(address.End.Row, address.End.Column);
      }
      this.View.SelectedRange = str;
      this.View.ActiveCell = ExcelCellBase.GetAddress(Address.Start.Row, Address.Start.Column);
    }

    public void InsertRow(int rowFrom, int rows) => this.InsertRow(rowFrom, rows, 0);

    public void InsertRow(int rowFrom, int rows, int copyStylesFromRow)
    {
      this.CheckSheetType();
      ExcelAddressBase dimension = this.Dimension;
      if (dimension != null && dimension.End.Row > rowFrom && dimension.End.Row + rows > 1048576)
        throw new ArgumentOutOfRangeException("Can't insert. Rows will be shifted outside the boundries of the worksheet.");
      this._values.Insert(rowFrom, 0, rows, 0);
      this._formulas.Insert(rowFrom, 0, rows, 0);
      this._styles.Insert(rowFrom, 0, rows, 0);
      this._types.Insert(rowFrom, 0, rows, 0);
      this._commentsStore.Insert(rowFrom, 0, rows, 0);
      this._hyperLinks.Insert(rowFrom, 0, rows, 0);
      this._flags.Insert(rowFrom, 0, rows, 0);
      foreach (ExcelWorksheet.Formulas formulas in this._sharedFormulas.Values)
      {
        if (formulas.StartRow >= rowFrom)
          formulas.StartRow += rows;
        ExcelAddressBase excelAddressBase = new ExcelAddressBase(formulas.Address);
        if (excelAddressBase._fromRow >= rowFrom)
        {
          excelAddressBase._fromRow += rows;
          excelAddressBase._toRow += rows;
        }
        else if (excelAddressBase._toRow >= rowFrom)
          excelAddressBase._toRow += rows;
        formulas.Address = ExcelCellBase.GetAddress(excelAddressBase._fromRow, excelAddressBase._fromCol, excelAddressBase._toRow, excelAddressBase._toCol);
        formulas.Formula = ExcelCellBase.UpdateFormulaReferences(formulas.Formula, rows, 0, rowFrom, 0);
      }
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._formulas);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Value is string)
          cellsStoreEnumerator.Value = (object) ExcelCellBase.UpdateFormulaReferences(cellsStoreEnumerator.Value.ToString(), rows, 0, rowFrom, 0);
      }
      this.FixMergedCellsRow(rowFrom, rows, false);
      if (copyStylesFromRow <= 0)
        return;
      for (int index = 0; index < rows; ++index)
        this.Row(rowFrom + index).StyleID = this.Row(copyStylesFromRow).StyleID;
    }

    public void InsertColumn(int columnFrom, int columns)
    {
      this.InsertColumn(columnFrom, columns, 0);
    }

    public void InsertColumn(int columnFrom, int columns, int copyStylesFromColumn)
    {
      this.CheckSheetType();
      ExcelAddressBase dimension = this.Dimension;
      if (dimension != null && dimension.End.Column > columnFrom && dimension.End.Column + columns > 16384)
        throw new ArgumentOutOfRangeException("Can't insert. Columns will be shifted outside the boundries of the worksheet.");
      this._values.Insert(0, columnFrom, 0, columns);
      this._formulas.Insert(0, columnFrom, 0, columns);
      this._styles.Insert(0, columnFrom, 0, columns);
      this._types.Insert(0, columnFrom, 0, columns);
      this._commentsStore.Insert(0, columnFrom, 0, columns);
      this._hyperLinks.Insert(0, columnFrom, 0, columns);
      this._flags.Insert(0, columnFrom, 0, columns);
      foreach (ExcelWorksheet.Formulas formulas in this._sharedFormulas.Values)
      {
        if (formulas.StartCol >= columnFrom)
          formulas.StartCol += columns;
        ExcelAddressBase excelAddressBase = new ExcelAddressBase(formulas.Address);
        if (excelAddressBase._fromCol >= columnFrom)
        {
          excelAddressBase._fromCol += columns;
          excelAddressBase._toCol += columns;
        }
        else if (excelAddressBase._toCol >= columnFrom)
          excelAddressBase._toCol += columns;
        formulas.Address = ExcelCellBase.GetAddress(excelAddressBase._fromRow, excelAddressBase._fromCol, excelAddressBase._toRow, excelAddressBase._toCol);
        formulas.Formula = ExcelCellBase.UpdateFormulaReferences(formulas.Formula, 0, columns, 0, columnFrom);
      }
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._formulas);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Value is string)
          cellsStoreEnumerator.Value = (object) ExcelCellBase.UpdateFormulaReferences(cellsStoreEnumerator.Value.ToString(), 0, columns, 0, columnFrom);
      }
      this.FixMergedCellsColumn(columnFrom, columns, false);
      if (copyStylesFromColumn <= 0)
        return;
      for (int index = 0; index < columns; ++index)
        this.Column(columnFrom + index).StyleID = this.Column(copyStylesFromColumn).StyleID;
    }

    private void FixMergedCellsRow(int row, int rows, bool delete)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < this._mergedCells.Count; ++index)
      {
        ExcelAddressBase excelAddressBase1 = new ExcelAddressBase(this._mergedCells[index]);
        ExcelAddressBase excelAddressBase2;
        if (delete)
        {
          excelAddressBase2 = excelAddressBase1.DeleteRow(row, rows);
          if (excelAddressBase2 == null)
          {
            intList.Add(index);
            continue;
          }
        }
        else
          excelAddressBase2 = excelAddressBase1.AddRow(row, rows);
        if (excelAddressBase2._address != excelAddressBase1._address)
        {
          for (int fromRow = excelAddressBase2._fromRow; fromRow <= excelAddressBase2._toRow; ++fromRow)
          {
            for (int fromCol = excelAddressBase2._fromCol; fromCol <= excelAddressBase2._toCol; ++fromCol)
              this._flags.SetFlagValue(fromRow, fromCol, true, CellFlags.Merged);
          }
        }
        this._mergedCells.List[index] = excelAddressBase2._address;
      }
      for (int index = intList.Count - 1; index >= 0; --index)
        this._mergedCells.List.RemoveAt(intList[index]);
    }

    private void FixMergedCellsColumn(int column, int columns, bool delete)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < this._mergedCells.Count; ++index)
      {
        ExcelAddressBase excelAddressBase1 = new ExcelAddressBase(this._mergedCells[index]);
        ExcelAddressBase excelAddressBase2;
        if (delete)
        {
          excelAddressBase2 = excelAddressBase1.DeleteColumn(column, columns);
          if (excelAddressBase2 == null)
          {
            intList.Add(index);
            continue;
          }
        }
        else
          excelAddressBase2 = excelAddressBase1.AddColumn(column, columns);
        if (excelAddressBase2._address != excelAddressBase1._address)
        {
          for (int fromRow = excelAddressBase2._fromRow; fromRow <= excelAddressBase2._toRow; ++fromRow)
          {
            for (int fromCol = excelAddressBase2._fromCol; fromCol <= excelAddressBase2._toCol; ++fromCol)
              this._flags.SetFlagValue(fromRow, fromCol, true, CellFlags.Merged);
          }
        }
        this._mergedCells.List[index] = excelAddressBase2._address;
      }
      for (int index = intList.Count - 1; index >= 0; --index)
        this._mergedCells.List.RemoveAt(intList[index]);
    }

    private void FixSharedFormulasRows(int position, int rows)
    {
      List<ExcelWorksheet.Formulas> added = new List<ExcelWorksheet.Formulas>();
      List<ExcelWorksheet.Formulas> formulasList = new List<ExcelWorksheet.Formulas>();
      foreach (int key in this._sharedFormulas.Keys)
      {
        ExcelWorksheet.Formulas sharedFormula = this._sharedFormulas[key];
        int FromRow;
        int FromColumn;
        int ToRow;
        int ToColumn;
        ExcelCellBase.GetRowColFromAddress(sharedFormula.Address, out FromRow, out FromColumn, out ToRow, out ToColumn);
        if (position >= FromRow && position + Math.Abs(rows) <= ToRow)
        {
          if (rows > 0)
          {
            sharedFormula.Address = ExcelCellBase.GetAddress(FromRow, FromColumn) + ":" + ExcelCellBase.GetAddress(position - 1, ToColumn);
            if (ToRow != FromRow)
              added.Add(new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default)
              {
                StartCol = sharedFormula.StartCol,
                StartRow = position + rows,
                Address = ExcelCellBase.GetAddress(position + rows, FromColumn) + ":" + ExcelCellBase.GetAddress(ToRow + rows, ToColumn),
                Formula = ExcelCellBase.TranslateFromR1C1(ExcelCellBase.TranslateToR1C1(sharedFormula.Formula, sharedFormula.StartRow, sharedFormula.StartCol), position, sharedFormula.StartCol)
              });
          }
          else
            sharedFormula.Address = FromRow - rows >= ToRow ? ExcelCellBase.GetAddress(FromRow, FromColumn) + ":" + ExcelCellBase.GetAddress(ToRow + rows, ToColumn) : ExcelCellBase.GetAddress(FromRow, FromColumn, ToRow + rows, ToColumn);
        }
        else if (position <= ToRow)
        {
          if (rows > 0)
          {
            sharedFormula.StartRow += rows;
            sharedFormula.Address = ExcelCellBase.GetAddress(FromRow + rows, FromColumn) + ":" + ExcelCellBase.GetAddress(ToRow + rows, ToColumn);
          }
          else if (position <= FromRow && position + Math.Abs(rows) > ToRow)
          {
            formulasList.Add(sharedFormula);
          }
          else
          {
            ToRow = ToRow + rows < position - 1 ? position - 1 : ToRow + rows;
            if (position <= FromRow)
              FromRow = FromRow + rows < position ? position : FromRow + rows;
            sharedFormula.Address = ExcelCellBase.GetAddress(FromRow, FromColumn, ToRow, ToColumn);
            this.Cells[sharedFormula.Address].SetSharedFormulaID(sharedFormula.Index);
          }
        }
      }
      this.AddFormulas(added, position, rows);
      foreach (ExcelWorksheet.Formulas formulas in formulasList)
        this._sharedFormulas.Remove(formulas.Index);
      List<ExcelWorksheet.Formulas> newFormulas = new List<ExcelWorksheet.Formulas>();
      foreach (int key in this._sharedFormulas.Keys)
      {
        ExcelWorksheet.Formulas sharedFormula = this._sharedFormulas[key];
        this.UpdateSharedFormulaRow(ref sharedFormula, position, rows, ref newFormulas);
      }
      this.AddFormulas(newFormulas, position, rows);
    }

    private void AddFormulas(List<ExcelWorksheet.Formulas> added, int position, int rows)
    {
      foreach (ExcelWorksheet.Formulas formulas in added)
      {
        formulas.Index = this.GetMaxShareFunctionIndex(false);
        this._sharedFormulas.Add(formulas.Index, formulas);
        this.Cells[formulas.Address].SetSharedFormulaID(formulas.Index);
      }
    }

    private void UpdateSharedFormulaRow(
      ref ExcelWorksheet.Formulas formula,
      int startRow,
      int rows,
      ref List<ExcelWorksheet.Formulas> newFormulas)
    {
      int count = newFormulas.Count;
      int FromRow;
      int FromColumn;
      int ToRow;
      int ToColumn;
      ExcelCellBase.GetRowColFromAddress(formula.Address, out FromRow, out FromColumn, out ToRow, out ToColumn);
      string r1C1_1;
      if (rows > 0 || FromRow <= startRow)
      {
        r1C1_1 = ExcelCellBase.TranslateToR1C1(formula.Formula, formula.StartRow, formula.StartCol);
        formula.Formula = ExcelCellBase.TranslateFromR1C1(r1C1_1, FromRow, formula.StartCol);
      }
      else
      {
        r1C1_1 = ExcelCellBase.TranslateToR1C1(formula.Formula, formula.StartRow - rows, formula.StartCol);
        formula.Formula = ExcelCellBase.TranslateFromR1C1(r1C1_1, formula.StartRow, formula.StartCol);
      }
      string str1 = r1C1_1;
      for (int row = FromRow; row <= ToRow; ++row)
      {
        for (int index = FromColumn; index <= ToColumn; ++index)
        {
          string str2;
          string r1C1_2;
          if (rows > 0 || row < startRow)
          {
            str2 = ExcelCellBase.UpdateFormulaReferences(ExcelCellBase.TranslateFromR1C1(r1C1_1, row, index), rows, 0, startRow, 0);
            r1C1_2 = ExcelCellBase.TranslateToR1C1(str2, row, index);
          }
          else
          {
            str2 = ExcelCellBase.UpdateFormulaReferences(ExcelCellBase.TranslateFromR1C1(r1C1_1, row - rows, index), rows, 0, startRow, 0);
            r1C1_2 = ExcelCellBase.TranslateToR1C1(str2, row, index);
          }
          if (r1C1_2 != str1)
          {
            if (row == FromRow && index == FromColumn)
            {
              formula.Formula = str2;
            }
            else
            {
              if (newFormulas.Count == count)
                formula.Address = ExcelCellBase.GetAddress(formula.StartRow, formula.StartCol, row - 1, index);
              else
                newFormulas[newFormulas.Count - 1].Address = ExcelCellBase.GetAddress(newFormulas[newFormulas.Count - 1].StartRow, newFormulas[newFormulas.Count - 1].StartCol, row - 1, index);
              newFormulas.Add(new ExcelWorksheet.Formulas(SourceCodeTokenizer.Default)
              {
                Formula = str2,
                StartRow = row,
                StartCol = index
              });
              str1 = r1C1_2;
            }
          }
        }
      }
      if (rows < 0 && formula.StartRow > startRow)
      {
        if (formula.StartRow + rows < startRow)
          formula.StartRow = startRow;
        else
          formula.StartRow += rows;
      }
      if (newFormulas.Count <= count)
        return;
      newFormulas[newFormulas.Count - 1].Address = ExcelCellBase.GetAddress(newFormulas[newFormulas.Count - 1].StartRow, newFormulas[newFormulas.Count - 1].StartCol, ToRow, ToColumn);
    }

    public void DeleteRow(int row) => this.DeleteRow(row, 1);

    public void DeleteRow(int rowFrom, int rows)
    {
      this.CheckSheetType();
      this._values.Delete(rowFrom, 1, rows, 16384);
      this._types.Delete(rowFrom, 1, rows, 16384);
      this._formulas.Delete(rowFrom, 1, rows, 16384);
      this._styles.Delete(rowFrom, 1, rows, 16384);
      this._flags.Delete(rowFrom, 1, rows, 16384);
      this._commentsStore.Delete(rowFrom, 1, rows, 16384);
      this._hyperLinks.Delete(rowFrom, 1, rows, 16384);
      this.AdjustFormulasRow(rowFrom, rows);
      this.FixMergedCellsRow(rowFrom, rows, true);
    }

    public void DeleteColumn(int column) => this.DeleteColumn(column, 1);

    public void DeleteColumn(int columnFrom, int columns)
    {
      this._values.Delete(1, columnFrom, 1048576, columns);
      this._types.Delete(1, columnFrom, 1048576, columns);
      this._formulas.Delete(1, columnFrom, 1048576, columns);
      this._styles.Delete(1, columnFrom, 1048576, columns);
      this._flags.Delete(1, columnFrom, 1048576, columns);
      this._commentsStore.Delete(1, columnFrom, 1048576, columns);
      this._hyperLinks.Delete(1, columnFrom, 1048576, columns);
      this.AdjustFormulasColumn(columnFrom, columns);
      this.FixMergedCellsColumn(columnFrom, columns, true);
    }

    internal void AdjustFormulasRow(int rowFrom, int rows)
    {
      List<int> intList = new List<int>();
      foreach (ExcelWorksheet.Formulas formulas in this._sharedFormulas.Values)
      {
        ExcelAddressBase excelAddressBase = new ExcelAddress(formulas.Address).DeleteRow(rowFrom, rows);
        if (excelAddressBase == null)
        {
          intList.Add(formulas.Index);
        }
        else
        {
          formulas.Address = excelAddressBase.Address;
          formulas.Formula = ExcelCellBase.UpdateFormulaReferences(formulas.Formula, -rows, 0, rowFrom, 0);
          if (formulas.StartRow >= rowFrom)
            formulas.StartRow -= formulas.StartRow;
        }
      }
      foreach (int key in intList)
        this._sharedFormulas.Remove(key);
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._formulas, rowFrom, 1, 1048576, 16384);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Value is string)
          cellsStoreEnumerator.Value = (object) ExcelCellBase.UpdateFormulaReferences(cellsStoreEnumerator.Value.ToString(), -rows, 0, rowFrom, 0);
      }
    }

    internal void AdjustFormulasColumn(int columnFrom, int columns)
    {
      List<int> intList = new List<int>();
      foreach (ExcelWorksheet.Formulas formulas in this._sharedFormulas.Values)
      {
        ExcelAddressBase excelAddressBase = new ExcelAddress(formulas.Address).DeleteColumn(columnFrom, columns);
        if (excelAddressBase == null)
        {
          intList.Add(formulas.Index);
        }
        else
        {
          formulas.Address = excelAddressBase.Address;
          formulas.Formula = ExcelCellBase.UpdateFormulaReferences(formulas.Formula, 0, -columns, 0, columnFrom);
          if (formulas.StartCol >= columnFrom)
            formulas.StartCol -= formulas.StartCol;
        }
      }
      foreach (int key in intList)
        this._sharedFormulas.Remove(key);
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._formulas, 1, columnFrom, 1048576, 16384);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Value is string)
          cellsStoreEnumerator.Value = (object) ExcelCellBase.UpdateFormulaReferences(cellsStoreEnumerator.Value.ToString(), 0, -columns, 0, columnFrom);
      }
    }

    public void DeleteRow(int rowFrom, int rows, bool shiftOtherRowsUp)
    {
      this.DeleteRow(rowFrom, rows);
    }

    public object GetValue(int Row, int Column)
    {
      this.CheckSheetType();
      object obj = this._values.GetValue(Row, Column);
      if (obj == null)
        return (object) null;
      return this._flags.GetFlagValue(Row, Column, CellFlags.RichText) ? (object) this.Cells[Row, Column].RichText.Text : obj;
    }

    public T GetValue<T>(int Row, int Column)
    {
      this.CheckSheetType();
      object v = this._values.GetValue(Row, Column);
      if (v == null)
        return default (T);
      return this._flags.GetFlagValue(Row, Column, CellFlags.RichText) ? (T) this.Cells[Row, Column].RichText.Text : this.GetTypedValue<T>(v);
    }

    internal T GetTypedValue<T>(object v)
    {
      if (v == null)
        return default (T);
      Type type1 = v.GetType();
      Type type2 = typeof (T);
      if (type1 == type2)
        return (T) v;
      TypeConverter converter = TypeDescriptor.GetConverter(type1);
      if (type2 == typeof (DateTime))
      {
        if (type1 == typeof (TimeSpan))
          return (T) (ValueType) new DateTime(((TimeSpan) v).Ticks);
        DateTime result;
        return type1 == typeof (string) ? (DateTime.TryParse(v.ToString(), out result) ? (T) (ValueType) result : default (T)) : (converter.CanConvertTo(typeof (double)) ? (T) (ValueType) DateTime.FromOADate((double) converter.ConvertTo(v, typeof (double))) : default (T));
      }
      if (type2 == typeof (TimeSpan))
      {
        if (type1 == typeof (DateTime))
          return (T) (ValueType) new TimeSpan(((DateTime) v).Ticks);
        if (type1 == typeof (string))
        {
          TimeSpan result;
          return TimeSpan.TryParse(v.ToString(), out result) ? (T) (ValueType) result : default (T);
        }
        if (converter.CanConvertTo(typeof (double)))
          return (T) (ValueType) new TimeSpan(DateTime.FromOADate((double) converter.ConvertTo(v, typeof (double))).Ticks);
        try
        {
          return (T) Convert.ChangeType(v, typeof (T));
        }
        catch (Exception ex)
        {
          return default (T);
        }
      }
      else
      {
        if (converter.CanConvertTo(type2))
          return (T) converter.ConvertTo(v, typeof (T));
        if (type2.IsGenericType && type2.GetGenericTypeDefinition().Equals(typeof (Nullable<>)))
        {
          type2 = Nullable.GetUnderlyingType(type2);
          if (converter.CanConvertTo(type2))
            return (T) converter.ConvertTo(v, typeof (T));
        }
        if (type1 == typeof (double) && type2 == typeof (Decimal))
          return (T) (ValueType) Convert.ToDecimal(v);
        return type1 == typeof (Decimal) && type2 == typeof (double) ? (T) (ValueType) Convert.ToDouble(v) : default (T);
      }
    }

    public void SetValue(int Row, int Column, object Value)
    {
      this.CheckSheetType();
      if (Row < 1 || Column < 1 || Row > 1048576 && Column > 16384)
        throw new ArgumentOutOfRangeException("Row or Column out of range");
      this._values.SetValue(Row, Column, Value);
    }

    public void SetValue(string Address, object Value)
    {
      this.CheckSheetType();
      int row;
      int col;
      ExcelCellBase.GetRowCol(Address, out row, out col, true);
      if (row < 1 || col < 1 || row > 1048576 && col > 16384)
        throw new ArgumentOutOfRangeException("Address is invalid or out of range");
      this._values.SetValue(row, col, Value);
    }

    public int GetMergeCellId(int row, int column)
    {
      for (int Index = 0; Index < this._mergedCells.Count; ++Index)
      {
        ExcelRange cell = this.Cells[this._mergedCells[Index]];
        if (cell.Start.Row <= row && row <= cell.End.Row && cell.Start.Column <= column && column <= cell.End.Column)
          return Index + 1;
      }
      return 0;
    }

    internal void Save()
    {
      this.DeletePrinterSettings();
      if (this._worksheetXml != null && !(this is ExcelChartsheet))
      {
        if (this._headerFooter != null)
          this.HeaderFooter.Save();
        ExcelAddressBase dimension = this.Dimension;
        if (dimension == null)
          this.DeleteAllNode("d:dimension/@ref");
        else
          this.SetXmlNodeString("d:dimension/@ref", dimension.Address);
        int count = this.Drawings.Count;
        if (this._drawings.Count == 0)
          this.DeleteNode("d:drawing");
        this.SaveComments();
        this.HeaderFooter.SaveHeaderFooterImages();
        this.SaveTables();
        this.SavePivotTables();
      }
      if (!(this.Drawings.UriDrawing != (Uri) null))
        return;
      if (this.Drawings.Count == 0)
      {
        this.Part.DeleteRelationship(this.Drawings._drawingRelation.Id);
        this._package.Package.DeletePart(this.Drawings.UriDrawing);
      }
      else
      {
        this.Drawings.DrawingXml.Save((Stream) this.Drawings.Part.GetStream(FileMode.Create, FileAccess.Write));
        foreach (ExcelDrawing drawing in this.Drawings)
        {
          if (drawing is ExcelChart)
          {
            ExcelChart excelChart = (ExcelChart) drawing;
            excelChart.ChartXml.Save((Stream) excelChart.Part.GetStream(FileMode.Create, FileAccess.Write));
          }
        }
      }
    }

    internal void SaveHandler(
      ZipOutputStream stream,
      Ionic.Zlib.CompressionLevel compressionLevel,
      string fileName)
    {
      stream.CodecBufferSize = 8096;
      stream.CompressionLevel = compressionLevel;
      stream.PutNextEntry(fileName);
      this.SaveXml((Stream) stream);
    }

    private void DeletePrinterSettings()
    {
      XmlAttribute node = (XmlAttribute) this.WorksheetXml.SelectSingleNode("//d:pageSetup/@r:id", this.NameSpaceManager);
      if (node == null)
        return;
      string id = node.Value;
      node.OwnerElement.Attributes.Remove(node);
      if (!this.Part.RelationshipExists(id))
        return;
      ZipPackageRelationship relationship = this.Part.GetRelationship(id);
      Uri uri = UriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri);
      this.Part.DeleteRelationship(relationship.Id);
      if (!this._package.Package.PartExists(uri))
        return;
      this._package.Package.DeletePart(uri);
    }

    private void SaveComments()
    {
      if (this._comments != null)
      {
        if (this._comments.Count == 0)
        {
          if (this._comments.Uri != (Uri) null)
          {
            this.Part.DeleteRelationship(this._comments.RelId);
            this._package.Package.DeletePart(this._comments.Uri);
          }
          this.RemoveLegacyDrawingRel(this.VmlDrawingsComments.RelId);
        }
        else
        {
          if (this._comments.Uri == (Uri) null)
            this._comments.Uri = new Uri(string.Format("/xl/comments{0}.xml", (object) this.SheetID), UriKind.Relative);
          if (this._comments.Part == null)
          {
            this._comments.Part = this._package.Package.CreatePart(this._comments.Uri, "application/vnd.openxmlformats-officedocument.spreadsheetml.comments+xml", this._package.Compression);
            this.Part.CreateRelationship(UriHelper.GetRelativeUri(this.WorksheetUri, this._comments.Uri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments");
          }
          this._comments.CommentXml.Save((Stream) this._comments.Part.GetStream());
        }
      }
      if (this._vmlDrawings == null)
        return;
      if (this._vmlDrawings.Count == 0)
      {
        if (!(this._vmlDrawings.Uri != (Uri) null))
          return;
        this.Part.DeleteRelationship(this._vmlDrawings.RelId);
        this._package.Package.DeletePart(this._vmlDrawings.Uri);
      }
      else
      {
        if (this._vmlDrawings.Uri == (Uri) null)
          this._vmlDrawings.Uri = XmlHelper.GetNewUri(this._package.Package, "/xl/drawings/vmlDrawing{0}.vml");
        if (this._vmlDrawings.Part == null)
        {
          this._vmlDrawings.Part = this._package.Package.CreatePart(this._vmlDrawings.Uri, "application/vnd.openxmlformats-officedocument.vmlDrawing", this._package.Compression);
          ZipPackageRelationship relationship = this.Part.CreateRelationship(UriHelper.GetRelativeUri(this.WorksheetUri, this._vmlDrawings.Uri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing");
          this.SetXmlNodeString("d:legacyDrawing/@r:id", relationship.Id);
          this._vmlDrawings.RelId = relationship.Id;
        }
        this._vmlDrawings.VmlDrawingXml.Save((Stream) this._vmlDrawings.Part.GetStream());
      }
    }

    private void SaveTables()
    {
      foreach (ExcelTable table in this.Tables)
      {
        if (table.ShowHeader || table.ShowTotal)
        {
          int fromCol = table.Address._fromCol;
          HashSet<string> stringSet = new HashSet<string>();
          foreach (ExcelTableColumn column in (IEnumerable<ExcelTableColumn>) table.Columns)
          {
            string lower = column.Name.ToLower();
            if (stringSet.Contains(lower))
              throw new InvalidDataException(string.Format("Table {0} Column {1} does not have a unique name.", (object) table.Name, (object) column.Name));
            stringSet.Add(lower);
            if (table.ShowHeader)
              this._values.SetValue(table.Address._fromRow, fromCol, (object) column.Name);
            if (table.ShowTotal)
              this.SetTableTotalFunction(table, column, fromCol);
            if (!string.IsNullOrEmpty(column.CalculatedColumnFormula))
            {
              int num1 = table.ShowHeader ? table.Address._fromRow + 1 : table.Address._fromRow;
              int num2 = table.ShowTotal ? table.Address._toRow - 1 : table.Address._toRow;
              for (int row = num1; row <= num2; ++row)
                this.SetFormula(row, fromCol, (object) column.CalculatedColumnFormula);
            }
            ++fromCol;
          }
        }
        if (table.Part == null)
        {
          table.TableUri = XmlHelper.GetNewUri(this._package.Package, "/xl/tables/table{0}.xml", table.Id);
          table.Part = this._package.Package.CreatePart(table.TableUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.table+xml", this.Workbook._package.Compression);
          MemoryStream stream = table.Part.GetStream(FileMode.Create);
          table.TableXml.Save((Stream) stream);
          ZipPackageRelationship relationship = this.Part.CreateRelationship(UriHelper.GetRelativeUri(this.WorksheetUri, table.TableUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/table");
          table.RelationshipID = relationship.Id;
          this.CreateNode("d:tableParts");
          XmlNode xmlNode = this.TopNode.SelectSingleNode("d:tableParts", this.NameSpaceManager);
          XmlElement element = xmlNode.OwnerDocument.CreateElement("tablePart", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
          xmlNode.AppendChild((XmlNode) element);
          element.SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationship.Id);
        }
        else
        {
          MemoryStream stream = table.Part.GetStream(FileMode.Create);
          table.TableXml.Save((Stream) stream);
        }
      }
    }

    internal void SetTableTotalFunction(ExcelTable tbl, ExcelTableColumn col, int colNum = -1)
    {
      if (colNum == -1)
      {
        for (int Index = 0; Index < tbl.Columns.Count; ++Index)
        {
          if (tbl.Columns[Index].Name == col.Name)
            colNum = tbl.Address._fromCol + Index;
        }
      }
      if (col.TotalsRowFunction == RowFunctions.Custom)
        this.SetFormula(tbl.Address._toRow, colNum, (object) col.TotalsRowFormula);
      else if (col.TotalsRowFunction != RowFunctions.None)
      {
        switch (col.TotalsRowFunction)
        {
          case RowFunctions.Average:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "101"));
            break;
          case RowFunctions.Count:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "102"));
            break;
          case RowFunctions.CountNums:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "103"));
            break;
          case RowFunctions.Max:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "104"));
            break;
          case RowFunctions.Min:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "105"));
            break;
          case RowFunctions.StdDev:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "107"));
            break;
          case RowFunctions.Sum:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "109"));
            break;
          case RowFunctions.Var:
            this.SetFormula(tbl.Address._toRow, colNum, (object) ExcelWorksheet.GetTotalFunction(col, "110"));
            break;
          default:
            throw new Exception("Unknown RowFunction enum");
        }
      }
      else
        this._values.SetValue(tbl.Address._toRow, colNum, (object) col.TotalsRowLabel);
    }

    internal void SetFormula(int row, int col, object value)
    {
      this._formulas.SetValue(row, col, value);
      if (this._values.Exists(row, col))
        return;
      this._values.SetValue(row, col, (object) null);
    }

    internal void SetStyle(int row, int col, int value)
    {
      this._styles.SetValue(row, col, value);
      if (this._values.Exists(row, col))
        return;
      this._values.SetValue(row, col, (object) null);
    }

    private void SavePivotTables()
    {
      foreach (ExcelPivotTable pivotTable in this.PivotTables)
      {
        if (pivotTable.DataFields.Count > 1)
        {
          if (pivotTable.DataOnRows)
          {
            if (!(pivotTable.PivotTableXml.SelectSingleNode("//d:rowFields", pivotTable.NameSpaceManager) is XmlElement xmlElement2))
            {
              pivotTable.CreateNode("d:rowFields");
              xmlElement2 = pivotTable.PivotTableXml.SelectSingleNode("//d:rowFields", pivotTable.NameSpaceManager) as XmlElement;
            }
          }
          else if (!(pivotTable.PivotTableXml.SelectSingleNode("//d:colFields", pivotTable.NameSpaceManager) is XmlElement xmlElement2))
          {
            pivotTable.CreateNode("d:colFields");
            xmlElement2 = pivotTable.PivotTableXml.SelectSingleNode("//d:colFields", pivotTable.NameSpaceManager) as XmlElement;
          }
          if (xmlElement2.SelectSingleNode("d:field[@ x= \"-2\"]", pivotTable.NameSpaceManager) == null)
          {
            XmlElement element = pivotTable.PivotTableXml.CreateElement("field", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            element.SetAttribute("x", "-2");
            xmlElement2.AppendChild((XmlNode) element);
          }
        }
        pivotTable.PivotTableXml.Save((Stream) pivotTable.Part.GetStream(FileMode.Create));
        pivotTable.CacheDefinition.CacheDefinitionXml.Save((Stream) pivotTable.CacheDefinition.Part.GetStream(FileMode.Create));
      }
    }

    private static string GetTotalFunction(ExcelTableColumn col, string FunctionNum)
    {
      return string.Format("SUBTOTAL({0},{1}[{2}])", (object) FunctionNum, (object) col._tbl.Name, (object) col.Name);
    }

    private void SaveXml(Stream stream)
    {
      StreamWriter sw = new StreamWriter(stream, Encoding.UTF8, 65536);
      if (this is ExcelChartsheet)
      {
        sw.Write(this._worksheetXml.OuterXml);
      }
      else
      {
        this.CreateNode("d:cols");
        this.CreateNode("d:sheetData");
        this.CreateNode("d:mergeCells");
        this.CreateNode("d:hyperlinks");
        this.CreateNode("d:rowBreaks");
        this.CreateNode("d:colBreaks");
        string outerXml = this._worksheetXml.OuterXml;
        int start1 = 0;
        int end1 = 0;
        this.GetBlockPos(outerXml, "cols", ref start1, ref end1);
        sw.Write(outerXml.Substring(0, start1));
        List<int> intList1 = new List<int>();
        this.UpdateColumnData(sw);
        int start2 = end1;
        int end2 = end1;
        this.GetBlockPos(outerXml, "sheetData", ref start2, ref end2);
        sw.Write(outerXml.Substring(end1, start2 - end1));
        List<int> intList2 = new List<int>();
        this.UpdateRowCellData(sw);
        int start3 = end2;
        int end3 = end2;
        this.GetBlockPos(outerXml, "mergeCells", ref start3, ref end3);
        sw.Write(outerXml.Substring(end2, start3 - end2));
        if (this._mergedCells.Count > 0)
          this.UpdateMergedCells(sw);
        int start4 = end3;
        int end4 = end3;
        this.GetBlockPos(outerXml, "hyperlinks", ref start4, ref end4);
        sw.Write(outerXml.Substring(end3, start4 - end3));
        this.UpdateHyperLinks(sw);
        int start5 = end4;
        int end5 = end4;
        this.GetBlockPos(outerXml, "rowBreaks", ref start5, ref end5);
        sw.Write(outerXml.Substring(end4, start5 - end4));
        this.UpdateRowBreaks(sw);
        int start6 = end5;
        int end6 = end5;
        this.GetBlockPos(outerXml, "colBreaks", ref start6, ref end6);
        sw.Write(outerXml.Substring(end5, start6 - end5));
        this.UpdateColBreaks(sw);
        sw.Write(outerXml.Substring(end6, outerXml.Length - end6));
      }
      sw.Flush();
    }

    private void UpdateColBreaks(StreamWriter sw)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._values, 0, 0, 0, 16384);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Value is ExcelColumn excelColumn && excelColumn.PageBreak)
        {
          stringBuilder.AppendFormat("<brk id=\"{0}\" max=\"16383\" man=\"1\" />", (object) cellsStoreEnumerator.Column);
          ++num;
        }
      }
      if (num <= 0)
        return;
      sw.Write(string.Format("<colBreaks count=\"{0}\" manualBreakCount=\"{0}\">{1}</colBreaks>", (object) num, (object) stringBuilder.ToString()));
    }

    private void UpdateRowBreaks(StreamWriter sw)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._values, 0, 0, 1048576, 0);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Value is ExcelRow excelRow && excelRow.PageBreak)
        {
          stringBuilder.AppendFormat("<brk id=\"{0}\" max=\"1048575\" man=\"1\" />", (object) cellsStoreEnumerator.Row);
          ++num;
        }
      }
      if (num <= 0)
        return;
      sw.Write(string.Format("<rowBreaks count=\"{0}\" manualBreakCount=\"{0}\">{1}</rowBreaks>", (object) num, (object) stringBuilder.ToString()));
    }

    private void UpdateColumnData(StreamWriter sw)
    {
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._values, 0, 1, 0, 16384);
      bool flag = true;
      while (cellsStoreEnumerator.Next())
      {
        if (flag)
        {
          sw.Write("<cols>");
          flag = false;
        }
        ExcelColumn excelColumn = cellsStoreEnumerator.Value as ExcelColumn;
        ExcelStyleCollection<ExcelXfs> cellXfs = this._package.Workbook.Styles.CellXfs;
        sw.Write("<col min=\"{0}\" max=\"{1}\"", (object) excelColumn.ColumnMin, (object) excelColumn.ColumnMax);
        if (excelColumn.Hidden)
          sw.Write(" hidden=\"1\"");
        else if (excelColumn.BestFit)
          sw.Write(" bestFit=\"1\"");
        sw.Write(string.Format((IFormatProvider) CultureInfo.InvariantCulture, " width=\"{0}\" customWidth=\"1\"", (object) excelColumn.Width));
        if (excelColumn.OutlineLevel > 0)
        {
          sw.Write(" outlineLevel=\"{0}\" ", (object) excelColumn.OutlineLevel);
          if (excelColumn.Collapsed)
          {
            if (excelColumn.Hidden)
              sw.Write(" collapsed=\"1\"");
            else
              sw.Write(" collapsed=\"1\" hidden=\"1\"");
          }
        }
        if (excelColumn.Phonetic)
          sw.Write(" phonetic=\"1\"");
        int num = excelColumn.StyleID >= 0 ? cellXfs[excelColumn.StyleID].newID : excelColumn.StyleID;
        if (num > 0)
          sw.Write(" style=\"{0}\"", (object) num);
        sw.Write(" />");
      }
      if (flag)
        return;
      sw.Write("</cols>");
    }

    private void UpdateRowCellData(StreamWriter sw)
    {
      ExcelStyleCollection<ExcelXfs> cellXfs = this._package.Workbook.Styles.CellXfs;
      int prevRow = -1;
      StringBuilder stringBuilder = new StringBuilder();
      Dictionary<string, ExcelWorkbook.SharedStringItem> sharedStrings = this._package.Workbook._sharedStrings;
      ExcelStyles styles = this._package.Workbook.Styles;
      StringBuilder cache = new StringBuilder();
      cache.Append("<sheetData>");
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._values, 1, 0, 1048576, 16384);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Column > 0)
        {
          int newId = cellXfs[styles.GetStyleId(this, cellsStoreEnumerator.Row, cellsStoreEnumerator.Column)].newID;
          if (cellsStoreEnumerator.Row != prevRow)
          {
            this.WriteRow(cache, cellXfs, prevRow, cellsStoreEnumerator.Row);
            prevRow = cellsStoreEnumerator.Row;
          }
          object v = cellsStoreEnumerator.Value;
          object obj = this._formulas.GetValue(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column);
          if (obj is int key)
          {
            ExcelWorksheet.Formulas sharedFormula = this._sharedFormulas[key];
            if (sharedFormula.Address.IndexOf(':') > 0)
            {
              if (sharedFormula.StartCol == cellsStoreEnumerator.Column && sharedFormula.StartRow == cellsStoreEnumerator.Row)
              {
                if (sharedFormula.IsArray)
                  cache.AppendFormat("<c r=\"{0}\" s=\"{1}\"><f ref=\"{2}\" t=\"array\">{3}</f>{4}</c>", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId), (object) sharedFormula.Address, (object) SecurityElement.Escape(sharedFormula.Formula), this.GetFormulaValue(v));
                else
                  cache.AppendFormat("<c r=\"{0}\" s=\"{1}\"><f ref=\"{2}\" t=\"shared\"  si=\"{3}\">{4}</f>{5}</c>", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId), (object) sharedFormula.Address, (object) key, (object) SecurityElement.Escape(sharedFormula.Formula), this.GetFormulaValue(v));
              }
              else if (sharedFormula.IsArray)
                cache.AppendFormat("<c r=\"{0}\" s=\"{1}\" />", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId));
              else
                cache.AppendFormat("<c r=\"{0}\" s=\"{1}\"><f t=\"shared\" si=\"{2}\" />{3}</c>", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId), (object) key, this.GetFormulaValue(v));
            }
            else if (sharedFormula.IsArray)
            {
              cache.AppendFormat("<c r=\"{0}\" s=\"{1}\"><f ref=\"{2}\" t=\"array\">{3}</f>{4}</c>", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId), (object) string.Format("{0}:{1}", (object) sharedFormula.Address, (object) sharedFormula.Address), (object) SecurityElement.Escape(sharedFormula.Formula), this.GetFormulaValue(v));
            }
            else
            {
              cache.AppendFormat("<c r=\"{0}\" s=\"{1}\">", (object) sharedFormula.Address, (object) (newId < 0 ? 0 : newId));
              cache.AppendFormat("<f>{0}</f>{1}</c>", (object) SecurityElement.Escape(sharedFormula.Formula), this.GetFormulaValue(v));
            }
          }
          else if (obj != null && obj.ToString() != "")
          {
            cache.AppendFormat("<c r=\"{0}\" s=\"{1}\" {2}>", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId), (object) this.GetCellType(v));
            cache.AppendFormat("<f>{0}</f>{1}</c>", (object) SecurityElement.Escape(obj.ToString()), this.GetFormulaValue(v));
          }
          else if (v == null)
          {
            cache.AppendFormat("<c r=\"{0}\" s=\"{1}\" />", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId));
          }
          else
          {
            if (!v.GetType().IsPrimitive)
            {
              switch (v)
              {
                case double _:
                case Decimal _:
                case DateTime _:
                case TimeSpan _:
                  break;
                default:
                  int num;
                  if (!sharedStrings.ContainsKey(v.ToString()))
                  {
                    num = sharedStrings.Count;
                    sharedStrings.Add(v.ToString(), new ExcelWorkbook.SharedStringItem()
                    {
                      isRichText = this._flags.GetFlagValue(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column, CellFlags.RichText),
                      pos = num
                    });
                  }
                  else
                    num = sharedStrings[v.ToString()].pos;
                  cache.AppendFormat("<c r=\"{0}\" s=\"{1}\" t=\"s\">", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId));
                  cache.AppendFormat("<v>{0}</v></c>", (object) num);
                  goto label_28;
              }
            }
            string valueForXml = this.GetValueForXml(v);
            cache.AppendFormat("<c r=\"{0}\" s=\"{1}\" {2}>", (object) cellsStoreEnumerator.CellAddress, (object) (newId < 0 ? 0 : newId), (object) this.GetCellType(v));
            cache.AppendFormat("<v>{0}</v></c>", (object) valueForXml);
          }
        }
        else
        {
          this.WriteRow(cache, cellXfs, prevRow, cellsStoreEnumerator.Row);
          prevRow = cellsStoreEnumerator.Row;
        }
label_28:
        if (cache.Length > 6291456)
        {
          sw.Write(cache.ToString());
          cache = new StringBuilder();
        }
      }
      if (prevRow != -1)
        cache.Append("</row>");
      cache.Append("</sheetData>");
      sw.Write(cache.ToString());
      sw.Flush();
    }

    private object GetFormulaValue(object v)
    {
      return this._package.Workbook._isCalculated ? (object) ("<v>" + this.GetValueForXml(v) + "</v>") : (object) "";
    }

    private string GetCellType(object v)
    {
      switch (v)
      {
        case bool _:
          return " t=\"b\"";
        case double d when double.IsInfinity(d):
        case ExcelErrorValue _:
          return " t=\"e\"";
        default:
          return "";
      }
    }

    private string GetValueForXml(object v)
    {
      string valueForXml;
      try
      {
        if (v is DateTime dateTime)
        {
          double oaDate = dateTime.ToOADate();
          if (this.Workbook.Date1904)
            oaDate -= 1462.0;
          valueForXml = oaDate.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        else if (v is TimeSpan timeSpan)
        {
          valueForXml = new DateTime(timeSpan.Ticks).ToOADate().ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        else
        {
          if (!v.GetType().IsPrimitive)
          {
            switch (v)
            {
              case double _:
              case Decimal _:
                break;
              default:
                valueForXml = v.ToString();
                goto label_14;
            }
          }
          switch (v)
          {
            case double d1 when double.IsNaN(d1):
              valueForXml = "";
              break;
            case double d2 when double.IsInfinity(d2):
              valueForXml = "#NUM!";
              break;
            default:
              valueForXml = Convert.ToDouble(v, (IFormatProvider) CultureInfo.InvariantCulture).ToString("R15", (IFormatProvider) CultureInfo.InvariantCulture);
              break;
          }
        }
      }
      catch
      {
        valueForXml = "0";
      }
label_14:
      return valueForXml;
    }

    private void WriteRow(
      StringBuilder cache,
      ExcelStyleCollection<ExcelXfs> cellXfs,
      int prevRow,
      int row)
    {
      if (prevRow != -1)
        cache.Append("</row>");
      cache.AppendFormat("<row r=\"{0}\" ", (object) row);
      if (this._values.GetValue(row, 0) is RowInternal rowInternal)
      {
        if (rowInternal.Hidden)
          cache.Append("ht=\"0\" hidden=\"1\" ");
        else if (rowInternal.Height != this.DefaultRowHeight)
        {
          cache.AppendFormat(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "ht=\"{0}\" ", (object) rowInternal.Height));
          if (rowInternal.CustomHeight)
            cache.Append("customHeight=\"1\" ");
        }
        if (rowInternal.OutlineLevel > (short) 0)
        {
          cache.AppendFormat("outlineLevel =\"{0}\" ", (object) rowInternal.OutlineLevel);
          if (rowInternal.Collapsed)
          {
            if (rowInternal.Hidden)
              cache.Append(" collapsed=\"1\" ");
            else
              cache.Append(" collapsed=\"1\" hidden=\"1\" ");
          }
        }
        if (rowInternal.Phonetic)
          cache.Append("ph=\"1\" ");
      }
      int PositionID = this._styles.GetValue(row, 0);
      if (PositionID > 0)
        cache.AppendFormat("s=\"{0}\" customFormat=\"1\"", (object) cellXfs[PositionID].newID);
      cache.Append(">");
    }

    private void WriteRow(
      StreamWriter sw,
      ExcelStyleCollection<ExcelXfs> cellXfs,
      int prevRow,
      int row)
    {
      if (prevRow != -1)
        sw.Write("</row>");
      sw.Write("<row r=\"{0}\" ", (object) row);
      if (this._values.GetValue(row, 0) is RowInternal rowInternal)
      {
        if (rowInternal.Hidden)
          sw.Write("ht=\"0\" hidden=\"1\" ");
        else if (rowInternal.Height != this.DefaultRowHeight)
        {
          sw.Write(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "ht=\"{0}\" ", (object) rowInternal.Height));
          if (rowInternal.CustomHeight)
            sw.Write("customHeight=\"1\" ");
        }
        if (rowInternal.OutlineLevel > (short) 0)
        {
          sw.Write("outlineLevel =\"{0}\" ", (object) rowInternal.OutlineLevel);
          if (rowInternal.Collapsed)
          {
            if (rowInternal.Hidden)
              sw.Write(" collapsed=\"1\" ");
            else
              sw.Write(" collapsed=\"1\" hidden=\"1\" ");
          }
        }
        if (rowInternal.Phonetic)
          sw.Write("ph=\"1\" ");
      }
      int PositionID = this._styles.GetValue(row, 0);
      if (PositionID > 0)
        sw.Write("s=\"{0}\" customFormat=\"1\"", (object) cellXfs[PositionID].newID);
      sw.Write(">");
    }

    private void UpdateHyperLinks(StreamWriter sw)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      CellsStoreEnumerator<Uri> cellsStoreEnumerator = new CellsStoreEnumerator<Uri>(this._hyperLinks);
      bool flag = true;
      while (cellsStoreEnumerator.Next())
      {
        if (flag)
        {
          sw.Write("<hyperlinks>");
          flag = false;
        }
        Uri uri = this._hyperLinks.GetValue(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column);
        if (uri is ExcelHyperLink && !string.IsNullOrEmpty((uri as ExcelHyperLink).ReferenceAddress))
        {
          ExcelHyperLink excelHyperLink = uri as ExcelHyperLink;
          sw.Write("<hyperlink ref=\"{0}\" location=\"{1}\" {2}{3}/>", (object) this.Cells[cellsStoreEnumerator.Row, cellsStoreEnumerator.Column, cellsStoreEnumerator.Row + excelHyperLink.RowSpann, cellsStoreEnumerator.Column + excelHyperLink.ColSpann].Address, (object) ExcelCellBase.GetFullAddress(this.Name, excelHyperLink.ReferenceAddress), string.IsNullOrEmpty(excelHyperLink.Display) ? (object) "" : (object) ("display=\"" + SecurityElement.Escape(excelHyperLink.Display) + "\" "), string.IsNullOrEmpty(excelHyperLink.ToolTip) ? (object) "" : (object) ("tooltip=\"" + SecurityElement.Escape(excelHyperLink.ToolTip) + "\" "));
        }
        else if (uri != (Uri) null)
        {
          Uri targetUri = !(uri is ExcelHyperLink) ? uri : ((ExcelHyperLink) uri).OriginalUri;
          if (dictionary.ContainsKey(targetUri.OriginalString))
          {
            string str = dictionary[targetUri.OriginalString];
          }
          else
          {
            ZipPackageRelationship relationship = this.Part.CreateRelationship(targetUri, TargetMode.External, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink");
            if (uri is ExcelHyperLink)
            {
              ExcelHyperLink excelHyperLink = uri as ExcelHyperLink;
              sw.Write("<hyperlink ref=\"{0}\" {2}{3}r:id=\"{1}\" />", (object) ExcelCellBase.GetAddress(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column), (object) relationship.Id, string.IsNullOrEmpty(excelHyperLink.Display) ? (object) "" : (object) ("display=\"" + SecurityElement.Escape(excelHyperLink.Display) + "\" "), string.IsNullOrEmpty(excelHyperLink.ToolTip) ? (object) "" : (object) ("tooltip=\"" + SecurityElement.Escape(excelHyperLink.ToolTip) + "\" "));
            }
            else
              sw.Write("<hyperlink ref=\"{0}\" r:id=\"{1}\" />", (object) ExcelCellBase.GetAddress(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column), (object) relationship.Id);
            string id = relationship.Id;
          }
        }
      }
      if (flag)
        return;
      sw.Write("</hyperlinks>");
    }

    private XmlNode CreateHyperLinkCollection()
    {
      return this._worksheetXml.DocumentElement.InsertAfter((XmlNode) this._worksheetXml.CreateElement("hyperlinks", "http://schemas.openxmlformats.org/spreadsheetml/2006/main"), this._worksheetXml.SelectSingleNode("//d:conditionalFormatting", this.NameSpaceManager) ?? this._worksheetXml.SelectSingleNode("//d:mergeCells", this.NameSpaceManager) ?? this._worksheetXml.SelectSingleNode("//d:sheetData", this.NameSpaceManager));
    }

    public ExcelAddressBase Dimension
    {
      get
      {
        this.CheckSheetType();
        int fromRow;
        int fromCol;
        int toRow;
        int toCol;
        if (!this._values.GetDimension(out fromRow, out fromCol, out toRow, out toCol))
          return (ExcelAddressBase) null;
        return new ExcelAddressBase(fromRow, fromCol, toRow, toCol)
        {
          _ws = this.Name
        };
      }
    }

    public ExcelSheetProtection Protection
    {
      get
      {
        if (this._protection == null)
          this._protection = new ExcelSheetProtection(this.NameSpaceManager, this.TopNode, this);
        return this._protection;
      }
    }

    public ExcelProtectedRangeCollection ProtectedRanges
    {
      get
      {
        if (this._protectedRanges == null)
          this._protectedRanges = new ExcelProtectedRangeCollection(this.NameSpaceManager, this.TopNode, this);
        return this._protectedRanges;
      }
    }

    public ExcelDrawings Drawings
    {
      get
      {
        if (this._drawings == null)
          this._drawings = new ExcelDrawings(this._package, this);
        return this._drawings;
      }
    }

    public ExcelTableCollection Tables
    {
      get
      {
        this.CheckSheetType();
        if (this._tables == null)
          this._tables = new ExcelTableCollection(this);
        return this._tables;
      }
    }

    public ExcelPivotTableCollection PivotTables
    {
      get
      {
        this.CheckSheetType();
        if (this._pivotTables == null)
          this._pivotTables = new ExcelPivotTableCollection(this);
        return this._pivotTables;
      }
    }

    public ExcelConditionalFormattingCollection ConditionalFormatting
    {
      get
      {
        this.CheckSheetType();
        if (this._conditionalFormatting == null)
          this._conditionalFormatting = new ExcelConditionalFormattingCollection(this);
        return this._conditionalFormatting;
      }
    }

    public ExcelDataValidationCollection DataValidations
    {
      get
      {
        this.CheckSheetType();
        if (this._dataValidation == null)
          this._dataValidation = new ExcelDataValidationCollection(this);
        return this._dataValidation;
      }
    }

    public ExcelBackgroundImage BackgroundImage
    {
      get
      {
        if (this._backgroundImage == null)
          this._backgroundImage = new ExcelBackgroundImage(this.NameSpaceManager, this.TopNode, this);
        return this._backgroundImage;
      }
    }

    internal int GetStyleID(string StyleName)
    {
      ExcelNamedStyleXml excelNamedStyleXml = (ExcelNamedStyleXml) null;
      this.Workbook.Styles.NamedStyles.FindByID(StyleName, ref excelNamedStyleXml);
      if (excelNamedStyleXml.XfId == int.MinValue)
        excelNamedStyleXml.XfId = this.Workbook.Styles.CellXfs.FindIndexByID(excelNamedStyleXml.Style.Id);
      return excelNamedStyleXml.XfId;
    }

    public ExcelWorkbook Workbook => this._package.Workbook;

    internal int GetMaxShareFunctionIndex(bool isArray)
    {
      int key = this._sharedFormulas.Count + 1;
      if (isArray)
        key |= 1073741824;
      while (this._sharedFormulas.ContainsKey(key))
        ++key;
      return key;
    }

    internal void SetHFLegacyDrawingRel(string relID)
    {
      this.SetXmlNodeString("d:legacyDrawingHF/@r:id", relID);
    }

    internal void RemoveLegacyDrawingRel(string relID)
    {
      XmlNode oldChild = this.WorksheetXml.DocumentElement.SelectSingleNode(string.Format("d:legacyDrawing[@r:id=\"{0}\"]", (object) relID), this.NameSpaceManager);
      oldChild?.ParentNode.RemoveChild(oldChild);
    }

    internal void UpdateCellsWithDate1904Setting()
    {
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(this._values);
      double num = this.Workbook.Date1904 ? -1462.0 : 1462.0;
      while (cellsStoreEnumerator.MoveNext())
      {
        if (cellsStoreEnumerator.Value is DateTime)
        {
          try
          {
            double d = ((DateTime) cellsStoreEnumerator.Value).ToOADate() + num;
            cellsStoreEnumerator.Value = (object) DateTime.FromOADate(d);
          }
          catch
          {
          }
        }
      }
    }

    internal string GetFormula(int row, int col)
    {
      object obj = this._formulas.GetValue(row, col);
      if (obj is int key)
        return this._sharedFormulas[key].GetFormula(row, col);
      return obj != null ? obj.ToString() : "";
    }

    internal string GetFormulaR1C1(int row, int col)
    {
      object obj = this._formulas.GetValue(row, col);
      if (obj is int key)
      {
        ExcelWorksheet.Formulas sharedFormula = this._sharedFormulas[key];
        return ExcelCellBase.TranslateToR1C1(sharedFormula.Formula, sharedFormula.StartRow, sharedFormula.StartCol);
      }
      return obj != null ? ExcelCellBase.TranslateToR1C1(obj.ToString(), row, col) : "";
    }

    public void Dispose()
    {
      this._values.Dispose();
      this._formulas.Dispose();
      this._flags.Dispose();
      this._hyperLinks.Dispose();
      this._styles.Dispose();
      this._types.Dispose();
      this._commentsStore.Dispose();
      if (this._formulaTokens != null)
        this._formulaTokens.Dispose();
      this._values = (CellStore<object>) null;
      this._formulas = (CellStore<object>) null;
      this._flags = (FlagCellStore) null;
      this._hyperLinks = (CellStore<Uri>) null;
      this._styles = (CellStore<int>) null;
      this._types = (CellStore<string>) null;
      this._commentsStore = (CellStore<ExcelComment>) null;
      this._formulaTokens = (CellStore<List<OfficeOpenXml.FormulaParsing.LexicalAnalysis.Token>>) null;
      this._package = (ExcelPackage) null;
      this._pivotTables = (ExcelPivotTableCollection) null;
      this._protection = (ExcelSheetProtection) null;
      this._sharedFormulas.Clear();
      this._sharedFormulas = (Dictionary<int, ExcelWorksheet.Formulas>) null;
      this._sheetView = (ExcelWorksheetView) null;
      this._tables = (ExcelTableCollection) null;
      this._vmlDrawings = (ExcelVmlDrawingCommentCollection) null;
      this._conditionalFormatting = (ExcelConditionalFormattingCollection) null;
      this._dataValidation = (ExcelDataValidationCollection) null;
      this._drawings = (ExcelDrawings) null;
    }

    internal class Formulas
    {
      private ISourceCodeTokenizer _tokenizer;

      public Formulas(ISourceCodeTokenizer tokenizer) => this._tokenizer = tokenizer;

      internal int Index { get; set; }

      internal string Address { get; set; }

      internal bool IsArray { get; set; }

      public string Formula { get; set; }

      public int StartRow { get; set; }

      public int StartCol { get; set; }

      private IEnumerable<OfficeOpenXml.FormulaParsing.LexicalAnalysis.Token> Tokens { get; set; }

      internal string GetFormula(int row, int column)
      {
        if (this.StartRow == row && this.StartCol == column)
          return this.Formula;
        if (this.Tokens == null)
          this.Tokens = this._tokenizer.Tokenize(this.Formula);
        string formula = "";
        foreach (OfficeOpenXml.FormulaParsing.LexicalAnalysis.Token token in this.Tokens)
        {
          if (token.TokenType == OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenType.ExcelAddress)
          {
            ExcelFormulaAddress excelFormulaAddress = new ExcelFormulaAddress(token.Value);
            formula += excelFormulaAddress.GetOffset(row - this.StartRow, column - this.StartCol);
          }
          else
            formula += token.Value;
        }
        return formula;
      }
    }

    public class MergeCellsCollection<T> : IEnumerable<T>, IEnumerable
    {
      private List<T> _list = new List<T>();

      internal MergeCellsCollection()
      {
      }

      internal List<T> List => this._list;

      public T this[int Index] => this._list[Index];

      public int Count => this._list.Count;

      internal void Remove(T Item) => this._list.Remove(Item);

      public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
    }
  }
}
