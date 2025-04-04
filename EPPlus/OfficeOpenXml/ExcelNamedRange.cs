// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelNamedRange
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml
{
  public sealed class ExcelNamedRange : ExcelRangeBase
  {
    private ExcelWorksheet _sheet;

    public ExcelNamedRange(
      string name,
      ExcelWorksheet nameSheet,
      ExcelWorksheet sheet,
      string address,
      int index)
      : base(sheet, address)
    {
      this.Name = name;
      this._sheet = nameSheet;
      this.Index = index;
    }

    internal ExcelNamedRange(string name, ExcelWorkbook wb, ExcelWorksheet nameSheet, int index)
      : base(wb, nameSheet, name, true)
    {
      this.Name = name;
      this._sheet = nameSheet;
      this.Index = index;
    }

    public string Name { get; internal set; }

    public int LocalSheetId => this._sheet == null ? -1 : this._sheet.PositionID - 1;

    internal int Index { get; set; }

    public bool IsNameHidden { get; set; }

    public string NameComment { get; set; }

    internal object NameValue { get; set; }

    internal string NameFormula { get; set; }

    public override string ToString() => this.Name;
  }
}
