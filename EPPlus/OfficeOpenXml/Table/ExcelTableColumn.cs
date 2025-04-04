// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.ExcelTableColumn
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table
{
  public class ExcelTableColumn : XmlHelper
  {
    private const string TOTALSROWFORMULA_PATH = "d:totalsRowFormula";
    private const string DATACELLSTYLE_PATH = "@dataCellStyle";
    private const string CALCULATEDCOLUMNFORMULA_PATH = "d:calculatedColumnFormula";
    internal ExcelTable _tbl;

    internal ExcelTableColumn(XmlNamespaceManager ns, XmlNode topNode, ExcelTable tbl, int pos)
      : base(ns, topNode)
    {
      this._tbl = tbl;
      this.Position = pos;
    }

    public int Id
    {
      get => this.GetXmlNodeInt("@id");
      set => this.SetXmlNodeString("@id", value.ToString());
    }

    public int Position { get; private set; }

    public string Name
    {
      get
      {
        string name = this.GetXmlNodeString("@name");
        if (string.IsNullOrEmpty(name))
          name = !this._tbl.ShowHeader ? "Column" + (this.Position + 1).ToString() : this._tbl.WorkSheet.GetValue<string>(this._tbl.Address._fromRow, this._tbl.Address._fromCol + this.Position);
        return name;
      }
      set
      {
        this.SetXmlNodeString("@name", value);
        this._tbl.WorkSheet.SetTableTotalFunction(this._tbl, this);
      }
    }

    public string TotalsRowLabel
    {
      get => this.GetXmlNodeString("@totalsRowLabel");
      set => this.SetXmlNodeString("@totalsRowLabel", value);
    }

    public RowFunctions TotalsRowFunction
    {
      get
      {
        return this.GetXmlNodeString("@totalsRowFunction") == "" ? RowFunctions.None : (RowFunctions) Enum.Parse(typeof (RowFunctions), this.GetXmlNodeString("@totalsRowFunction"), true);
      }
      set
      {
        string str = value != RowFunctions.Custom ? value.ToString() : throw new Exception("Use the TotalsRowFormula-property to set a custom table formula");
        this.SetXmlNodeString("@totalsRowFunction", str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1));
        this._tbl.WorkSheet.SetTableTotalFunction(this._tbl, this);
      }
    }

    public string TotalsRowFormula
    {
      get => this.GetXmlNodeString("d:totalsRowFormula");
      set
      {
        if (value.StartsWith("="))
          value = value.Substring(1, value.Length - 1);
        this.SetXmlNodeString("@totalsRowFunction", "custom");
        this.SetXmlNodeString("d:totalsRowFormula", value);
        this._tbl.WorkSheet.SetTableTotalFunction(this._tbl, this);
      }
    }

    public string DataCellStyleName
    {
      get => this.GetXmlNodeString("@dataCellStyle");
      set
      {
        if (this._tbl.WorkSheet.Workbook.Styles.NamedStyles.FindIndexByID(value) < 0)
          throw new Exception(string.Format("Named style {0} does not exist.", (object) value));
        this.SetXmlNodeString(this.TopNode, "@dataCellStyle", value, true);
        int FromRow = this._tbl.Address._fromRow + (this._tbl.ShowHeader ? 1 : 0);
        int ToRow = this._tbl.Address._toRow - (this._tbl.ShowTotal ? 1 : 0);
        int num = this._tbl.Address._fromCol + this.Position;
        if (FromRow >= ToRow)
          return;
        this._tbl.WorkSheet.Cells[FromRow, num, ToRow, num].StyleName = value;
      }
    }

    public string CalculatedColumnFormula
    {
      get => this.GetXmlNodeString("d:calculatedColumnFormula");
      set
      {
        if (value.StartsWith("="))
          value = value.Substring(1, value.Length - 1);
        this.SetXmlNodeString("d:calculatedColumnFormula", value);
      }
    }
  }
}
