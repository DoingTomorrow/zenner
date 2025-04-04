// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.ExcelTable
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table
{
  public class ExcelTable : XmlHelper
  {
    private const string ID_PATH = "@id";
    private const string NAME_PATH = "@name";
    private const string DISPLAY_NAME_PATH = "@displayName";
    private const string HEADERROWCOUNT_PATH = "@headerRowCount";
    private const string AUTOFILTER_PATH = "d:autoFilter/@ref";
    private const string TOTALSROWCOUNT_PATH = "@totalsRowCount";
    private const string TOTALSROWSHOWN_PATH = "@totalsRowShown";
    private const string STYLENAME_PATH = "d:tableStyleInfo/@name";
    private const string SHOWFIRSTCOLUMN_PATH = "d:tableStyleInfo/@showFirstColumn";
    private const string SHOWLASTCOLUMN_PATH = "d:tableStyleInfo/@showLastColumn";
    private const string SHOWROWSTRIPES_PATH = "d:tableStyleInfo/@showRowStripes";
    private const string SHOWCOLUMNSTRIPES_PATH = "d:tableStyleInfo/@showColumnStripes";
    private const string TOTALSROWCELLSTYLE_PATH = "@totalsRowCellStyle";
    private const string DATACELLSTYLE_PATH = "@dataCellStyle";
    private const string HEADERROWCELLSTYLE_PATH = "@headerRowCellStyle";
    private ExcelTableColumnCollection _cols;
    private TableStyles _tableStyle = TableStyles.Medium6;

    internal ExcelTable(ZipPackageRelationship rel, ExcelWorksheet sheet)
      : base(sheet.NameSpaceManager)
    {
      this.WorkSheet = sheet;
      this.TableUri = UriHelper.ResolvePartUri(rel.SourceUri, rel.TargetUri);
      this.RelationshipID = rel.Id;
      this.Part = sheet._package.Package.GetPart(this.TableUri);
      this.TableXml = new XmlDocument();
      XmlHelper.LoadXmlSafe(this.TableXml, (Stream) this.Part.GetStream());
      this.init();
      this.Address = new ExcelAddressBase(this.GetXmlNodeString("@ref"));
    }

    internal ExcelTable(ExcelWorksheet sheet, ExcelAddressBase address, string name, int tblId)
      : base(sheet.NameSpaceManager)
    {
      this.WorkSheet = sheet;
      this.Address = address;
      this.TableXml = new XmlDocument();
      XmlHelper.LoadXmlSafe(this.TableXml, this.GetStartXml(name, tblId), Encoding.UTF8);
      this.TopNode = (XmlNode) this.TableXml.DocumentElement;
      this.init();
      if (address._fromRow != address._toRow)
        return;
      this.ShowHeader = false;
    }

    private void init()
    {
      this.TopNode = (XmlNode) this.TableXml.DocumentElement;
      this.SchemaNodeOrder = new string[3]
      {
        "autoFilter",
        "tableColumns",
        "tableStyleInfo"
      };
    }

    private string GetStartXml(string name, int tblId)
    {
      string str1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>" + string.Format("<table xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" id=\"{0}\" name=\"{1}\" displayName=\"{2}\" ref=\"{3}\" headerRowCount=\"1\">", (object) tblId, (object) name, (object) this.cleanDisplayName(name), (object) this.Address.Address) + string.Format("<autoFilter ref=\"{0}\" />", (object) this.Address.Address);
      int num1 = this.Address._toCol - this.Address._fromCol + 1;
      string str2 = str1 + string.Format("<tableColumns count=\"{0}\">", (object) num1);
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      for (int index = 1; index <= num1; ++index)
      {
        ExcelRange cell = this.WorkSheet.Cells[this.Address._fromRow, this.Address._fromCol + index - 1];
        string key;
        if (cell.Value == null || dictionary.ContainsKey(cell.Value.ToString()))
        {
          int num2 = index;
          do
          {
            key = string.Format("Column{0}", (object) num2++);
          }
          while (dictionary.ContainsKey(key));
        }
        else
          key = SecurityElement.Escape(cell.Value.ToString());
        dictionary.Add(key, key);
        str2 += string.Format("<tableColumn id=\"{0}\" name=\"{1}\" />", (object) index, (object) key);
      }
      return str2 + "</tableColumns>" + "<tableStyleInfo name=\"TableStyleMedium9\" showFirstColumn=\"0\" showLastColumn=\"0\" showRowStripes=\"1\" showColumnStripes=\"0\" /> " + "</table>";
    }

    private string cleanDisplayName(string name) => Regex.Replace(name, "[^\\w\\.-_]", "_");

    internal ZipPackagePart Part { get; set; }

    public XmlDocument TableXml { get; set; }

    public Uri TableUri { get; internal set; }

    internal string RelationshipID { get; set; }

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
          throw new ArgumentException("Tablename is not unique");
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

    public ExcelWorksheet WorkSheet { get; set; }

    public ExcelAddressBase Address { get; internal set; }

    public ExcelTableColumnCollection Columns
    {
      get
      {
        if (this._cols == null)
          this._cols = new ExcelTableColumnCollection(this);
        return this._cols;
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
        this.SetXmlNodeString("d:tableStyleInfo/@name", nameof (TableStyle) + value.ToString());
      }
    }

    public bool ShowHeader
    {
      get => this.GetXmlNodeInt("@headerRowCount") != 0;
      set
      {
        if (this.Address._toRow - this.Address._fromRow < 0 && value || this.Address._toRow - this.Address._fromRow == 1 && value && this.ShowTotal)
          throw new Exception("Cant set ShowHeader-property. Table has too few rows");
        if (value)
        {
          this.DeleteNode("@headerRowCount");
          this.WriteAutoFilter(this.ShowTotal);
        }
        else
        {
          this.SetXmlNodeString("@headerRowCount", "0");
          this.DeleteAllNode("d:autoFilter/@ref");
        }
      }
    }

    internal ExcelAddressBase AutoFilterAddress
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:autoFilter/@ref");
        return xmlNodeString == "" ? (ExcelAddressBase) null : new ExcelAddressBase(xmlNodeString);
      }
    }

    private void WriteAutoFilter(bool showTotal)
    {
      if (!this.ShowHeader)
        return;
      this.SetXmlNodeString("d:autoFilter/@ref", !showTotal ? this.Address.Address : ExcelCellBase.GetAddress(this.Address._fromRow, this.Address._fromCol, this.Address._toRow - 1, this.Address._toCol));
    }

    public bool ShowFilter
    {
      get => this.ShowHeader && this.AutoFilterAddress != null;
      set
      {
        if (this.ShowHeader)
        {
          if (value)
            this.WriteAutoFilter(this.ShowTotal);
          else
            this.DeleteAllNode("d:autoFilter/@ref");
        }
        else if (value)
          throw new InvalidOperationException("Filter can only be applied when ShowHeader is set to true");
      }
    }

    public bool ShowTotal
    {
      get => this.GetXmlNodeInt("@totalsRowCount") == 1;
      set
      {
        if (value == this.ShowTotal)
          return;
        this.Address = !value ? (ExcelAddressBase) new ExcelAddress(this.WorkSheet.Name, ExcelCellBase.GetAddress(this.Address.Start.Row, this.Address.Start.Column, this.Address.End.Row - 1, this.Address.End.Column)) : (ExcelAddressBase) new ExcelAddress(this.WorkSheet.Name, ExcelCellBase.GetAddress(this.Address.Start.Row, this.Address.Start.Column, this.Address.End.Row + 1, this.Address.End.Column));
        this.SetXmlNodeString("@ref", this.Address.Address);
        if (value)
          this.SetXmlNodeString("@totalsRowCount", "1");
        else
          this.DeleteNode("@totalsRowCount");
        this.WriteAutoFilter(value);
      }
    }

    public string StyleName
    {
      get => this.GetXmlNodeString("d:tableStyleInfo/@name");
      set
      {
        if (value.StartsWith("TableStyle"))
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
        this.SetXmlNodeString("d:tableStyleInfo/@name", value, true);
      }
    }

    public bool ShowFirstColumn
    {
      get => this.GetXmlNodeBool("d:tableStyleInfo/@showFirstColumn");
      set => this.SetXmlNodeBool("d:tableStyleInfo/@showFirstColumn", value, false);
    }

    public bool ShowLastColumn
    {
      get => this.GetXmlNodeBool("d:tableStyleInfo/@showLastColumn");
      set => this.SetXmlNodeBool("d:tableStyleInfo/@showLastColumn", value, false);
    }

    public bool ShowRowStripes
    {
      get => this.GetXmlNodeBool("d:tableStyleInfo/@showRowStripes");
      set => this.SetXmlNodeBool("d:tableStyleInfo/@showRowStripes", value, false);
    }

    public bool ShowColumnStripes
    {
      get => this.GetXmlNodeBool("d:tableStyleInfo/@showColumnStripes");
      set => this.SetXmlNodeBool("d:tableStyleInfo/@showColumnStripes", value, false);
    }

    public string TotalsRowCellStyle
    {
      get => this.GetXmlNodeString("@totalsRowCellStyle");
      set
      {
        if (this.WorkSheet.Workbook.Styles.NamedStyles.FindIndexByID(value) < 0)
          throw new Exception(string.Format("Named style {0} does not exist.", (object) value));
        this.SetXmlNodeString(this.TopNode, "@totalsRowCellStyle", value, true);
        if (!this.ShowTotal)
          return;
        this.WorkSheet.Cells[this.Address._toRow, this.Address._fromCol, this.Address._toRow, this.Address._toCol].StyleName = value;
      }
    }

    public string DataCellStyleName
    {
      get => this.GetXmlNodeString("@dataCellStyle");
      set
      {
        if (this.WorkSheet.Workbook.Styles.NamedStyles.FindIndexByID(value) < 0)
          throw new Exception(string.Format("Named style {0} does not exist.", (object) value));
        this.SetXmlNodeString(this.TopNode, "@dataCellStyle", value, true);
        int FromRow = this.Address._fromRow + (this.ShowHeader ? 1 : 0);
        int ToRow = this.Address._toRow - (this.ShowTotal ? 1 : 0);
        if (FromRow >= ToRow)
          return;
        this.WorkSheet.Cells[FromRow, this.Address._fromCol, ToRow, this.Address._toCol].StyleName = value;
      }
    }

    public string HeaderRowCellStyle
    {
      get => this.GetXmlNodeString("@headerRowCellStyle");
      set
      {
        if (this.WorkSheet.Workbook.Styles.NamedStyles.FindIndexByID(value) < 0)
          throw new Exception(string.Format("Named style {0} does not exist.", (object) value));
        this.SetXmlNodeString(this.TopNode, "@headerRowCellStyle", value, true);
        if (!this.ShowHeader)
          return;
        this.WorkSheet.Cells[this.Address._fromRow, this.Address._fromCol, this.Address._fromRow, this.Address._toCol].StyleName = value;
      }
    }
  }
}
