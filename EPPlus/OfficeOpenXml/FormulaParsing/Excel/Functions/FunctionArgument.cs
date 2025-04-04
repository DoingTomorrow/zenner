// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.FunctionArgument
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class FunctionArgument
  {
    private ExcelCellState _excelCellState;

    public FunctionArgument(object val) => this.Value = val;

    public void SetExcelStateFlag(ExcelCellState state) => this._excelCellState |= state;

    public bool ExcelStateFlagIsSet(ExcelCellState state)
    {
      return (this._excelCellState & state) != (ExcelCellState) 0;
    }

    public object Value { get; private set; }

    public Type Type => this.Value == null ? (Type) null : this.Value.GetType();

    public bool IsExcelRange => this.Value != null && this.Value is ExcelDataProvider.IRangeInfo;

    public bool ValueIsExcelError => ExcelErrorValue.Values.IsErrorValue(this.Value);

    public ExcelErrorValue ValueAsExcelErrorValue => ExcelErrorValue.Parse(this.Value.ToString());

    public ExcelDataProvider.IRangeInfo ValueAsRangeInfo
    {
      get => this.Value as ExcelDataProvider.IRangeInfo;
    }

    public object ValueFirst
    {
      get
      {
        return !(this.Value is ExcelDataProvider.IRangeInfo rangeInfo) ? this.Value : rangeInfo.GetValue(rangeInfo.Address._fromRow, rangeInfo.Address._fromCol);
      }
    }
  }
}
