// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelCell
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class ExcelCell
  {
    public ExcelCell(object val, string formula, int colIndex, int rowIndex)
    {
      this.Value = val;
      this.Formula = formula;
      this.ColIndex = colIndex;
      this.RowIndex = rowIndex;
    }

    public int ColIndex { get; private set; }

    public int RowIndex { get; private set; }

    public object Value { get; private set; }

    public string Formula { get; private set; }
  }
}
