// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.ExcelLookupNavigator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class ExcelLookupNavigator : LookupNavigator
  {
    private int _currentRow;
    private int _currentCol;
    private object _currentValue;
    private RangeAddress _rangeAddress;
    private int _index;

    public ExcelLookupNavigator(
      LookupDirection direction,
      LookupArguments arguments,
      ParsingContext parsingContext)
      : base(direction, arguments, parsingContext)
    {
      this.Initialize();
    }

    private void Initialize()
    {
      this._index = 0;
      RangeAddressFactory rangeAddressFactory = new RangeAddressFactory(this.ParsingContext.ExcelDataProvider);
      this._rangeAddress = this.Arguments.RangeInfo != null ? rangeAddressFactory.Create(this.Arguments.RangeInfo.Address.WorkSheet, this.Arguments.RangeInfo.Address.Address) : rangeAddressFactory.Create(this.ParsingContext.Scopes.Current.Address.Worksheet, this.Arguments.RangeAddress);
      this._currentCol = this._rangeAddress.FromCol;
      this._currentRow = this._rangeAddress.FromRow;
      this.SetCurrentValue();
    }

    private void SetCurrentValue()
    {
      this._currentValue = this.ParsingContext.ExcelDataProvider.GetCellValue(this._rangeAddress.Worksheet, this._currentRow, this._currentCol);
    }

    private bool HasNext()
    {
      return this.Direction == LookupDirection.Vertical ? this._currentRow < this._rangeAddress.ToRow : this._currentCol < this._rangeAddress.ToCol;
    }

    public override int Index => this._index;

    public override bool MoveNext()
    {
      if (!this.HasNext())
        return false;
      if (this.Direction == LookupDirection.Vertical)
        ++this._currentRow;
      else
        ++this._currentCol;
      ++this._index;
      this.SetCurrentValue();
      return true;
    }

    public override object CurrentValue => this._currentValue;

    public override object GetLookupValue()
    {
      int currentRow = this._currentRow;
      int currentCol = this._currentCol;
      int col;
      int row;
      if (this.Direction == LookupDirection.Vertical)
      {
        col = currentCol + (this.Arguments.LookupIndex - 1);
        row = currentRow + this.Arguments.LookupOffset;
      }
      else
      {
        row = currentRow + (this.Arguments.LookupIndex - 1);
        col = currentCol + this.Arguments.LookupOffset;
      }
      return this.ParsingContext.ExcelDataProvider.GetCellValue(this._rangeAddress.Worksheet, row, col);
    }
  }
}
