// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelWorkbook
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using Ionic.Zip;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.Utils;
using OfficeOpenXml.VBA;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public sealed class ExcelWorkbook : XmlHelper, IDisposable
  {
    private const string codeModuleNamePath = "d:workbookPr/@codeName";
    private const string date1904Path = "d:workbookPr/@date1904";
    internal const double date1904Offset = 1462.0;
    internal ExcelPackage _package;
    private ExcelWorksheets _worksheets;
    private OfficeProperties _properties;
    private ExcelStyles _styles;
    internal Dictionary<string, ExcelWorkbook.SharedStringItem> _sharedStrings = new Dictionary<string, ExcelWorkbook.SharedStringItem>();
    internal List<ExcelWorkbook.SharedStringItem> _sharedStringsList = new List<ExcelWorkbook.SharedStringItem>();
    internal ExcelNamedRangeCollection _names;
    internal int _nextDrawingID;
    internal int _nextTableID = 1;
    internal int _nextPivotTableID = 1;
    internal XmlNamespaceManager _namespaceManager;
    internal FormulaParser _formulaParser;
    internal FormulaParserManager _parserManager;
    internal CellStore<List<OfficeOpenXml.FormulaParsing.LexicalAnalysis.Token>> _formulaTokens;
    private Decimal _standardFontWidth = Decimal.MinValue;
    private string _fontID = "";
    private ExcelProtection _protection;
    private ExcelWorkbookView _view;
    private ExcelVbaProject _vba;
    private XmlDocument _workbookXml;
    private XmlDocument _stylesXml;
    private string CALC_MODE_PATH = "d:calcPr/@calcMode";
    internal List<string> _externalReferences = new List<string>();
    internal bool _isCalculated;

    internal ExcelWorkbook(ExcelPackage package, XmlNamespaceManager namespaceManager)
      : base(namespaceManager)
    {
      this._package = package;
      this.WorkbookUri = new Uri("/xl/workbook.xml", UriKind.Relative);
      this.SharedStringsUri = new Uri("/xl/sharedStrings.xml", UriKind.Relative);
      this.StylesUri = new Uri("/xl/styles.xml", UriKind.Relative);
      this._names = new ExcelNamedRangeCollection(this);
      this._namespaceManager = namespaceManager;
      this.TopNode = (XmlNode) this.WorkbookXml.DocumentElement;
      this.SchemaNodeOrder = new string[18]
      {
        "fileVersion",
        "fileSharing",
        "workbookPr",
        "workbookProtection",
        "bookViews",
        "sheets",
        "functionGroups",
        "functionPrototypes",
        "externalReferences",
        "definedNames",
        "calcPr",
        "oleSize",
        "customWorkbookViews",
        "pivotCaches",
        "smartTagPr",
        "smartTagTypes",
        "webPublishing",
        "fileRecoveryPr"
      };
      this.GetSharedStrings();
    }

    private void GetSharedStrings()
    {
      if (!this._package.Package.PartExists(this.SharedStringsUri))
        return;
      XmlNodeList xmlNodeList = this._package.GetXmlFromUri(this.SharedStringsUri).SelectNodes("//d:sst/d:si", this.NameSpaceManager);
      this._sharedStringsList = new List<ExcelWorkbook.SharedStringItem>();
      if (xmlNodeList != null)
      {
        foreach (XmlNode xmlNode1 in xmlNodeList)
        {
          XmlNode xmlNode2 = xmlNode1.SelectSingleNode("d:t", this.NameSpaceManager);
          if (xmlNode2 != null)
            this._sharedStringsList.Add(new ExcelWorkbook.SharedStringItem()
            {
              Text = ExcelWorkbook.ExcelDecodeString(xmlNode2.InnerText)
            });
          else
            this._sharedStringsList.Add(new ExcelWorkbook.SharedStringItem()
            {
              Text = xmlNode1.InnerXml,
              isRichText = true
            });
        }
      }
      foreach (ZipPackageRelationship relationship in this.Part.GetRelationships())
      {
        if (relationship.TargetUri.OriginalString.ToLower().EndsWith("sharedstrings.xml"))
        {
          this.Part.DeleteRelationship(relationship.Id);
          break;
        }
      }
      this._package.Package.DeletePart(this.SharedStringsUri);
    }

    internal void GetDefinedNames()
    {
      XmlNodeList xmlNodeList = this.WorkbookXml.SelectNodes("//d:definedNames/d:definedName", this.NameSpaceManager);
      if (xmlNodeList == null)
        return;
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        string str = xmlElement.InnerText;
        int result1;
        ExcelWorksheet xlWorksheet;
        if (!int.TryParse(xmlElement.GetAttribute("localSheetId"), out result1))
        {
          result1 = -1;
          xlWorksheet = (ExcelWorksheet) null;
        }
        else
          xlWorksheet = this.Worksheets[result1 + 1];
        ExcelAddressBase.AddressType addressType = ExcelAddressBase.IsValid(str);
        if (str.IndexOf("[") == 0)
        {
          int num1 = str.IndexOf("[");
          int num2 = str.IndexOf("]", num1);
          int result2;
          if (num1 >= 0 && num2 >= 0 && int.TryParse(str.Substring(num1 + 1, num2 - num1 - 1), out result2) && result2 > 0 && result2 <= this._externalReferences.Count)
            str = str.Substring(0, num1) + "[" + this._externalReferences[result2 - 1] + "]" + str.Substring(num2 + 1);
        }
        ExcelNamedRange excelNamedRange;
        if (addressType == ExcelAddressBase.AddressType.Invalid || addressType == ExcelAddressBase.AddressType.InternalName || addressType == ExcelAddressBase.AddressType.ExternalName || addressType == ExcelAddressBase.AddressType.Formula || addressType == ExcelAddressBase.AddressType.ExternalAddress)
        {
          ExcelRangeBase Range = new ExcelRangeBase(this, xlWorksheet, xmlElement.GetAttribute("name"), true);
          excelNamedRange = xlWorksheet != null ? xlWorksheet.Names.Add(xmlElement.GetAttribute("name"), Range) : this._names.Add(xmlElement.GetAttribute("name"), Range);
          if (str.StartsWith("\""))
          {
            excelNamedRange.NameValue = (object) str.Substring(1, str.Length - 2);
          }
          else
          {
            double result3;
            if (double.TryParse(str, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result3))
              excelNamedRange.NameValue = (object) result3;
            else
              excelNamedRange.NameFormula = str;
          }
        }
        else
        {
          ExcelAddress excelAddress = new ExcelAddress(str, this._package, (ExcelAddressBase) null);
          if (result1 > -1)
          {
            excelNamedRange = !string.IsNullOrEmpty(excelAddress._ws) ? this.Worksheets[result1 + 1].Names.Add(xmlElement.GetAttribute("name"), new ExcelRangeBase(this, this.Worksheets[excelAddress._ws], str, false)) : this.Worksheets[result1 + 1].Names.Add(xmlElement.GetAttribute("name"), new ExcelRangeBase(this, this.Worksheets[result1 + 1], str, false));
          }
          else
          {
            ExcelWorksheet worksheet = this.Worksheets[excelAddress._ws];
            excelNamedRange = this._names.Add(xmlElement.GetAttribute("name"), new ExcelRangeBase(this, worksheet, str, false));
          }
        }
        if (xmlElement.GetAttribute("hidden") == "1" && excelNamedRange != null)
          excelNamedRange.IsNameHidden = true;
        if (!string.IsNullOrEmpty(xmlElement.GetAttribute("comment")))
          excelNamedRange.NameComment = xmlElement.GetAttribute("comment");
      }
    }

    public ExcelWorksheets Worksheets
    {
      get
      {
        if (this._worksheets == null)
          this._worksheets = new ExcelWorksheets(this._package, this._namespaceManager, this._workbookXml.DocumentElement.SelectSingleNode("d:sheets", this._namespaceManager) ?? this.CreateNode("d:sheets"));
        return this._worksheets;
      }
    }

    public ExcelNamedRangeCollection Names => this._names;

    internal FormulaParser FormulaParser
    {
      get
      {
        if (this._formulaParser == null)
          this._formulaParser = new FormulaParser((ExcelDataProvider) new EpplusExcelDataProvider(this._package));
        return this._formulaParser;
      }
    }

    public FormulaParserManager FormulaParserManager
    {
      get
      {
        if (this._parserManager == null)
          this._parserManager = new FormulaParserManager(this.FormulaParser);
        return this._parserManager;
      }
    }

    public Decimal MaxFontWidth
    {
      get
      {
        if (this._standardFontWidth == Decimal.MinValue || this._fontID != this.Styles.Fonts[0].Id)
        {
          ExcelFontXml font = this.Styles.Fonts[0];
          try
          {
            this._standardFontWidth = 0M;
            this._fontID = font.Id;
            Typeface typeface = new Typeface(new FontFamily(font.Name), font.Italic ? FontStyles.Normal : FontStyles.Italic, font.Bold ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal);
            for (int startIndex = 0; startIndex < 10; ++startIndex)
            {
              int num = (int) Math.Round(new FormattedText("0123456789".Substring(startIndex, 1), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, (double) font.Size * (4.0 / 3.0), (Brush) Brushes.Black).Width, 0);
              if ((Decimal) num > this._standardFontWidth)
                this._standardFontWidth = (Decimal) num;
            }
            if (this._standardFontWidth <= 0M)
              this._standardFontWidth = (Decimal) (int) ((double) font.Size * (2.0 / 3.0));
          }
          catch
          {
            this._standardFontWidth = (Decimal) (int) ((double) font.Size * (2.0 / 3.0));
          }
        }
        return this._standardFontWidth;
      }
      set => this._standardFontWidth = value;
    }

    public ExcelProtection Protection
    {
      get
      {
        if (this._protection == null)
        {
          this._protection = new ExcelProtection(this.NameSpaceManager, this.TopNode, this);
          this._protection.SchemaNodeOrder = this.SchemaNodeOrder;
        }
        return this._protection;
      }
    }

    public ExcelWorkbookView View
    {
      get
      {
        if (this._view == null)
          this._view = new ExcelWorkbookView(this.NameSpaceManager, this.TopNode, this);
        return this._view;
      }
    }

    public ExcelVbaProject VbaProject
    {
      get
      {
        if (this._vba == null && this._package.Package.PartExists(new Uri("/xl/vbaProject.bin", UriKind.Relative)))
          this._vba = new ExcelVbaProject(this);
        return this._vba;
      }
    }

    public void CreateVBAProject()
    {
      if (this._vba != null || this._package.Package.PartExists(new Uri("/xl/vbaProject.bin", UriKind.Relative)))
        throw new InvalidOperationException("VBA project already exists.");
      this._vba = new ExcelVbaProject(this);
      this._vba.Create();
    }

    internal Uri WorkbookUri { get; private set; }

    internal Uri StylesUri { get; private set; }

    internal Uri SharedStringsUri { get; private set; }

    internal ZipPackagePart Part => this._package.Package.GetPart(this.WorkbookUri);

    public XmlDocument WorkbookXml
    {
      get
      {
        if (this._workbookXml == null)
          this.CreateWorkbookXml(this._namespaceManager);
        return this._workbookXml;
      }
    }

    internal string CodeModuleName
    {
      get => this.GetXmlNodeString("d:workbookPr/@codeName");
      set => this.SetXmlNodeString("d:workbookPr/@codeName", value);
    }

    internal void CodeNameChange(string value) => this.CodeModuleName = value;

    public ExcelVBAModule CodeModule
    {
      get
      {
        return this.VbaProject != null ? this.VbaProject.Modules[this.CodeModuleName] : (ExcelVBAModule) null;
      }
    }

    public bool Date1904
    {
      get => this.GetXmlNodeBool("d:workbookPr/@date1904", false);
      set
      {
        if (this.Date1904 != value)
        {
          foreach (ExcelWorksheet worksheet in this.Worksheets)
            worksheet.UpdateCellsWithDate1904Setting();
        }
        this.SetXmlNodeBool("d:workbookPr/@date1904", value, false);
      }
    }

    private void CreateWorkbookXml(XmlNamespaceManager namespaceManager)
    {
      if (this._package.Package.PartExists(this.WorkbookUri))
      {
        this._workbookXml = this._package.GetXmlFromUri(this.WorkbookUri);
      }
      else
      {
        ZipPackagePart part = this._package.Package.CreatePart(this.WorkbookUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", this._package.Compression);
        this._workbookXml = new XmlDocument(namespaceManager.NameTable);
        this._workbookXml.PreserveWhitespace = false;
        XmlElement element1 = this._workbookXml.CreateElement("workbook", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.SetAttribute("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        this._workbookXml.AppendChild((XmlNode) element1);
        XmlElement element2 = this._workbookXml.CreateElement("bookViews", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.AppendChild((XmlNode) element2);
        XmlElement element3 = this._workbookXml.CreateElement("workbookView", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element2.AppendChild((XmlNode) element3);
        this._workbookXml.Save((TextWriter) new StreamWriter((Stream) part.GetStream(FileMode.Create, FileAccess.Write)));
        this._package.Package.Flush();
      }
    }

    public XmlDocument StylesXml
    {
      get
      {
        if (this._stylesXml == null)
        {
          if (this._package.Package.PartExists(this.StylesUri))
          {
            this._stylesXml = this._package.GetXmlFromUri(this.StylesUri);
          }
          else
          {
            ZipPackagePart part = this._package.Package.CreatePart(this.StylesUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml", this._package.Compression);
            StringBuilder stringBuilder = new StringBuilder("<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
            stringBuilder.Append("<numFmts />");
            stringBuilder.Append("<fonts count=\"1\"><font><sz val=\"11\" /><name val=\"Calibri\" /></font></fonts>");
            stringBuilder.Append("<fills><fill><patternFill patternType=\"none\" /></fill><fill><patternFill patternType=\"gray125\" /></fill></fills>");
            stringBuilder.Append("<borders><border><left /><right /><top /><bottom /><diagonal /></border></borders>");
            stringBuilder.Append("<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" /></cellStyleXfs>");
            stringBuilder.Append("<cellXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" xfId=\"0\" /></cellXfs>");
            stringBuilder.Append("<cellStyles><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\" /></cellStyles>");
            stringBuilder.Append("<dxfs count=\"0\" />");
            stringBuilder.Append("</styleSheet>");
            this._stylesXml = new XmlDocument();
            this._stylesXml.LoadXml(stringBuilder.ToString());
            this._stylesXml.Save((TextWriter) new StreamWriter((Stream) part.GetStream(FileMode.Create, FileAccess.Write)));
            this._package.Package.Flush();
            this._package.Workbook.Part.CreateRelationship(UriHelper.GetRelativeUri(this.WorkbookUri, this.StylesUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles");
            this._package.Package.Flush();
          }
        }
        return this._stylesXml;
      }
      set => this._stylesXml = value;
    }

    public ExcelStyles Styles
    {
      get
      {
        if (this._styles == null)
          this._styles = new ExcelStyles(this.NameSpaceManager, this.StylesXml, this);
        return this._styles;
      }
    }

    public OfficeProperties Properties
    {
      get
      {
        if (this._properties == null)
          this._properties = new OfficeProperties(this._package, this.NameSpaceManager);
        return this._properties;
      }
    }

    public ExcelCalcMode CalcMode
    {
      get
      {
        switch (this.GetXmlNodeString(this.CALC_MODE_PATH))
        {
          case "autoNoTable":
            return ExcelCalcMode.AutomaticNoTable;
          case "manual":
            return ExcelCalcMode.Manual;
          default:
            return ExcelCalcMode.Automatic;
        }
      }
      set
      {
        switch (value)
        {
          case ExcelCalcMode.AutomaticNoTable:
            this.SetXmlNodeString(this.CALC_MODE_PATH, "autoNoTable");
            break;
          case ExcelCalcMode.Manual:
            this.SetXmlNodeString(this.CALC_MODE_PATH, "manual");
            break;
          default:
            this.SetXmlNodeString(this.CALC_MODE_PATH, "auto");
            break;
        }
      }
    }

    internal void Save()
    {
      if (this.Worksheets.Count == 0)
        throw new InvalidOperationException("The workbook must contain at least one worksheet");
      this.DeleteCalcChain();
      if (this._vba == null && !this._package.Package.PartExists(new Uri("/xl/vbaProject.bin", UriKind.Relative)))
      {
        if (this.Part.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml")
          this.Part.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml";
      }
      else if (this.Part.ContentType != "application/vnd.ms-excel.sheet.macroEnabled.main+xml")
        this.Part.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.main+xml";
      this.UpdateDefinedNamesXml();
      if (this._workbookXml != null)
        this._package.SavePart(this.WorkbookUri, this._workbookXml);
      if (this._properties != null)
        this._properties.Save();
      this.Styles.UpdateXml();
      this._package.SavePart(this.StylesUri, this._stylesXml);
      bool flag = this.Protection.LockWindows || this.Protection.LockStructure;
      foreach (ExcelWorksheet worksheet in this.Worksheets)
      {
        if (flag && this.Protection.LockWindows)
          worksheet.View.WindowProtection = true;
        worksheet.Save();
        worksheet.Part.SaveHandler = new ZipPackagePart.SaveHandlerDelegate(worksheet.SaveHandler);
      }
      this._package.Package.CreatePart(this.SharedStringsUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml", this._package.Compression).SaveHandler = new ZipPackagePart.SaveHandlerDelegate(this.SaveSharedStringHandler);
      this.Part.CreateRelationship(UriHelper.GetRelativeUri(this.WorkbookUri, this.SharedStringsUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings");
      this.ValidateDataValidations();
      if (this._vba == null)
        return;
      this.VbaProject.Save();
    }

    private void DeleteCalcChain()
    {
      Uri uri1 = new Uri("/xl/calcChain.xml", UriKind.Relative);
      if (!this._package.Package.PartExists(uri1))
        return;
      Uri uri2 = new Uri("calcChain.xml", UriKind.Relative);
      foreach (ZipPackageRelationship relationship in this._package.Workbook.Part.GetRelationships())
      {
        if (relationship.TargetUri == uri2)
        {
          this._package.Workbook.Part.DeleteRelationship(relationship.Id);
          break;
        }
      }
      this._package.Package.DeletePart(uri1);
    }

    private void ValidateDataValidations()
    {
      foreach (ExcelWorksheet worksheet in this._package.Workbook.Worksheets)
      {
        if (!(worksheet is ExcelChartsheet))
          worksheet.DataValidations.ValidateAll();
      }
    }

    private void SaveSharedStringHandler(
      ZipOutputStream stream,
      Ionic.Zlib.CompressionLevel compressionLevel,
      string fileName)
    {
      stream.CompressionLevel = compressionLevel;
      stream.PutNextEntry(fileName);
      StringBuilder sb = new StringBuilder();
      StreamWriter streamWriter = new StreamWriter((Stream) stream);
      sb.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><sst xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" count=\"{0}\" uniqueCount=\"{0}\">", (object) this._sharedStrings.Count);
      foreach (string key in this._sharedStrings.Keys)
      {
        if (this._sharedStrings[key].isRichText)
        {
          sb.Append("<si>");
          ExcelWorkbook.ExcelEncodeString(sb, key);
          sb.Append("</si>");
        }
        else
        {
          if (key.Length > 0 && (key[0] == ' ' || key[key.Length - 1] == ' ' || key.Contains("  ") || key.Contains("\t")))
            sb.Append("<si><t xml:space=\"preserve\">");
          else
            sb.Append("<si><t>");
          ExcelWorkbook.ExcelEncodeString(sb, ExcelWorkbook.ExcelEscapeString(key));
          sb.Append("</t></si>");
        }
        if (sb.Length > 6291456)
        {
          streamWriter.Write(sb.ToString());
          sb = new StringBuilder();
        }
      }
      sb.Append("</sst>");
      streamWriter.Write(sb.ToString());
      streamWriter.Flush();
      this.Part.CreateRelationship(UriHelper.GetRelativeUri(this.WorkbookUri, this.SharedStringsUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings");
    }

    private static string ExcelEscapeString(string s)
    {
      return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }

    internal static void ExcelEncodeString(StreamWriter sw, string t)
    {
      if (Regex.IsMatch(t, "(_x[0-9A-F]{4,4}_)"))
      {
        Match match = Regex.Match(t, "(_x[0-9A-F]{4,4}_)");
        int num = 0;
        for (; match.Success; match = match.NextMatch())
        {
          t = t.Insert(match.Index + num, "_x005F");
          num += 6;
        }
      }
      for (int index = 0; index < t.Length; ++index)
      {
        if (t[index] <= '\u001F' && t[index] != '\t' && t[index] != '\n' && t[index] != '\r')
          sw.Write("_x00{0}_", (object) ((t[index] < '\n' ? "0" : "") + ((int) t[index]).ToString("X")));
        else
          sw.Write(t[index]);
      }
    }

    internal static void ExcelEncodeString(StringBuilder sb, string t)
    {
      if (Regex.IsMatch(t, "(_x[0-9A-F]{4,4}_)"))
      {
        Match match = Regex.Match(t, "(_x[0-9A-F]{4,4}_)");
        int num = 0;
        for (; match.Success; match = match.NextMatch())
        {
          t = t.Insert(match.Index + num, "_x005F");
          num += 6;
        }
      }
      for (int index = 0; index < t.Length; ++index)
      {
        if (t[index] <= '\u001F' && t[index] != '\t' && t[index] != '\n' && t[index] != '\r')
          sb.AppendFormat("_x00{0}_", (object) ((t[index] < '\n' ? "0" : "") + ((int) t[index]).ToString("X")));
        else
          sb.Append(t[index]);
      }
    }

    internal static string ExcelDecodeString(string t)
    {
      Match match = Regex.Match(t, "(_x005F|_x[0-9A-F]{4,4}_)");
      if (!match.Success)
        return t;
      bool flag = false;
      StringBuilder stringBuilder = new StringBuilder();
      int startIndex = 0;
      for (; match.Success; match = match.NextMatch())
      {
        if (startIndex < match.Index)
          stringBuilder.Append(t.Substring(startIndex, match.Index - startIndex));
        if (!flag && match.Value == "_x005F")
          flag = true;
        else if (flag)
        {
          stringBuilder.Append(match.Value);
          flag = false;
        }
        else
          stringBuilder.Append((char) int.Parse(match.Value.Substring(2, 4), NumberStyles.AllowHexSpecifier));
        startIndex = match.Index + match.Length;
      }
      stringBuilder.Append(t.Substring(startIndex, t.Length - startIndex));
      return stringBuilder.ToString();
    }

    private void UpdateDefinedNamesXml()
    {
      try
      {
        XmlNode oldChild = this.WorkbookXml.SelectSingleNode("//d:definedNames", this.NameSpaceManager);
        if (!this.ExistsNames())
        {
          if (oldChild == null)
            return;
          this.TopNode.RemoveChild(oldChild);
        }
        else
        {
          if (oldChild == null)
          {
            this.CreateNode("d:definedNames");
            oldChild = this.WorkbookXml.SelectSingleNode("//d:definedNames", this.NameSpaceManager);
          }
          else
            oldChild.RemoveAll();
          foreach (ExcelNamedRange name in this._names)
          {
            XmlElement element = this.WorkbookXml.CreateElement("definedName", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            oldChild.AppendChild((XmlNode) element);
            element.SetAttribute("name", name.Name);
            if (name.IsNameHidden)
              element.SetAttribute("hidden", "1");
            if (!string.IsNullOrEmpty(name.NameComment))
              element.SetAttribute("comment", name.NameComment);
            this.SetNameElement(name, element);
          }
          foreach (ExcelWorksheet worksheet in this._worksheets)
          {
            if (!(worksheet is ExcelChartsheet))
            {
              foreach (ExcelNamedRange name in worksheet.Names)
              {
                XmlElement element = this.WorkbookXml.CreateElement("definedName", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                oldChild.AppendChild((XmlNode) element);
                element.SetAttribute("name", name.Name);
                element.SetAttribute("localSheetId", name.LocalSheetId.ToString());
                if (name.IsNameHidden)
                  element.SetAttribute("hidden", "1");
                if (!string.IsNullOrEmpty(name.NameComment))
                  element.SetAttribute("comment", name.NameComment);
                this.SetNameElement(name, element);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Internal error updating named ranges ", ex);
      }
    }

    private void SetNameElement(ExcelNamedRange name, XmlElement elem)
    {
      if (name.IsName)
      {
        if (string.IsNullOrEmpty(name.NameFormula))
        {
          if (name.NameValue.GetType().IsPrimitive || name.NameValue is double || name.NameValue is Decimal)
            elem.InnerText = Convert.ToDouble(name.NameValue, (IFormatProvider) CultureInfo.InvariantCulture).ToString("R15", (IFormatProvider) CultureInfo.InvariantCulture);
          else if (name.NameValue is DateTime)
            elem.InnerText = ((DateTime) name.NameValue).ToOADate().ToString((IFormatProvider) CultureInfo.InvariantCulture);
          else
            elem.InnerText = "\"" + name.NameValue.ToString() + "\"";
        }
        else
          elem.InnerText = name.NameFormula;
      }
      else
        elem.InnerText = name.FullAddressAbsolute;
    }

    private bool ExistsNames()
    {
      if (this._names.Count != 0)
        return true;
      foreach (ExcelWorksheet worksheet in this.Worksheets)
      {
        if (!(worksheet is ExcelChartsheet) && worksheet.Names.Count > 0)
          return true;
      }
      return false;
    }

    internal bool ExistsTableName(string Name)
    {
      foreach (ExcelWorksheet worksheet in this.Worksheets)
      {
        if (worksheet.Tables._tableNames.ContainsKey(Name))
          return true;
      }
      return false;
    }

    internal bool ExistsPivotTableName(string Name)
    {
      foreach (ExcelWorksheet worksheet in this.Worksheets)
      {
        if (worksheet.PivotTables._pivotTableNames.ContainsKey(Name))
          return true;
      }
      return false;
    }

    internal void AddPivotTable(string cacheID, Uri defUri)
    {
      this.CreateNode("d:pivotCaches");
      XmlElement element = this.WorkbookXml.CreateElement("pivotCache", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      element.SetAttribute("cacheId", cacheID);
      ZipPackageRelationship relationship = this.Part.CreateRelationship(UriHelper.ResolvePartUri(this.WorkbookUri, defUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheDefinition");
      element.SetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", relationship.Id);
      this.WorkbookXml.SelectSingleNode("//d:pivotCaches", this.NameSpaceManager).AppendChild((XmlNode) element);
    }

    internal void GetExternalReferences()
    {
      XmlNodeList xmlNodeList = this.WorkbookXml.SelectNodes("//d:externalReferences/d:externalReference", this.NameSpaceManager);
      if (xmlNodeList == null)
        return;
      foreach (XmlElement xmlElement1 in xmlNodeList)
      {
        ZipPackageRelationship relationship1 = this.Part.GetRelationship(xmlElement1.GetAttribute("r:id"));
        ZipPackagePart part = this._package.Package.GetPart(UriHelper.ResolvePartUri(relationship1.SourceUri, relationship1.TargetUri));
        XmlDocument xmlDoc = new XmlDocument();
        XmlHelper.LoadXmlSafe(xmlDoc, (Stream) part.GetStream());
        if (xmlDoc.SelectSingleNode("//d:externalBook", this.NameSpaceManager) is XmlElement xmlElement2)
        {
          string attribute = xmlElement2.GetAttribute("r:id");
          ZipPackageRelationship relationship2 = part.GetRelationship(attribute);
          if (relationship2 != null)
            this._externalReferences.Add(relationship2.TargetUri.OriginalString);
        }
      }
    }

    public void Dispose()
    {
      this._sharedStrings.Clear();
      this._sharedStringsList.Clear();
      this._sharedStrings = (Dictionary<string, ExcelWorkbook.SharedStringItem>) null;
      this._sharedStringsList = (List<ExcelWorkbook.SharedStringItem>) null;
      this._vba = (ExcelVbaProject) null;
      this._worksheets.Dispose();
      this._package = (ExcelPackage) null;
      this._worksheets = (ExcelWorksheets) null;
      this._properties = (OfficeProperties) null;
      this._formulaParser = (FormulaParser) null;
    }

    internal class SharedStringItem
    {
      internal int pos;
      internal string Text;
      internal bool isRichText;
    }
  }
}
