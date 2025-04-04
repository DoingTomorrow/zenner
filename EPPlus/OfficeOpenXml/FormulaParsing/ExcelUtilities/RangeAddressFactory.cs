// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.RangeAddressFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Utilities;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class RangeAddressFactory
  {
    private readonly ExcelDataProvider _excelDataProvider;
    private readonly AddressTranslator _addressTranslator;
    private readonly IndexToAddressTranslator _indexToAddressTranslator;

    public RangeAddressFactory(ExcelDataProvider excelDataProvider)
      : this(excelDataProvider, new AddressTranslator(excelDataProvider), new IndexToAddressTranslator(excelDataProvider, ExcelReferenceType.RelativeRowAndColumn))
    {
    }

    public RangeAddressFactory(
      ExcelDataProvider excelDataProvider,
      AddressTranslator addressTranslator,
      IndexToAddressTranslator indexToAddressTranslator)
    {
      Require.That<ExcelDataProvider>(excelDataProvider).Named(nameof (excelDataProvider)).IsNotNull<ExcelDataProvider>();
      Require.That<AddressTranslator>(addressTranslator).Named(nameof (addressTranslator)).IsNotNull<AddressTranslator>();
      Require.That<IndexToAddressTranslator>(indexToAddressTranslator).Named(nameof (indexToAddressTranslator)).IsNotNull<IndexToAddressTranslator>();
      this._excelDataProvider = excelDataProvider;
      this._addressTranslator = addressTranslator;
      this._indexToAddressTranslator = indexToAddressTranslator;
    }

    public RangeAddress Create(int col, int row) => this.Create(string.Empty, col, row);

    public RangeAddress Create(string worksheetName, int col, int row)
    {
      return new RangeAddress()
      {
        Address = this._indexToAddressTranslator.ToAddress(col, row),
        Worksheet = worksheetName,
        FromCol = col,
        ToCol = col,
        FromRow = row,
        ToRow = row
      };
    }

    public RangeAddress Create(string worksheetName, string address)
    {
      Require.That<string>(address).Named("range").IsNotNullOrEmpty();
      ExcelAddressBase excelAddressBase = new ExcelAddressBase(address);
      string str = string.IsNullOrEmpty(excelAddressBase.WorkSheet) ? worksheetName : excelAddressBase.WorkSheet;
      return new RangeAddress()
      {
        Address = excelAddressBase.Address,
        Worksheet = str,
        FromRow = excelAddressBase._fromRow,
        FromCol = excelAddressBase._fromCol,
        ToRow = excelAddressBase._toRow,
        ToCol = excelAddressBase._toCol
      };
    }

    public RangeAddress Create(string range)
    {
      Require.That<string>(range).Named(nameof (range)).IsNotNullOrEmpty();
      ExcelAddressBase excelAddressBase = new ExcelAddressBase(range);
      return new RangeAddress()
      {
        Address = excelAddressBase.Address,
        Worksheet = excelAddressBase.WorkSheet ?? "",
        FromRow = excelAddressBase._fromRow,
        FromCol = excelAddressBase._fromCol,
        ToRow = excelAddressBase._toRow,
        ToCol = excelAddressBase._toCol
      };
    }

    private void HandleSingleCellAddress(RangeAddress rangeAddress, ExcelAddressInfo addressInfo)
    {
      int col;
      int row;
      this._addressTranslator.ToColAndRow(addressInfo.StartCell, out col, out row);
      rangeAddress.FromCol = col;
      rangeAddress.ToCol = col;
      rangeAddress.FromRow = row;
      rangeAddress.ToRow = row;
    }

    private void HandleMultipleCellAddress(RangeAddress rangeAddress, ExcelAddressInfo addressInfo)
    {
      int col1;
      int row1;
      this._addressTranslator.ToColAndRow(addressInfo.StartCell, out col1, out row1);
      int col2;
      int row2;
      this._addressTranslator.ToColAndRow(addressInfo.EndCell, out col2, out row2, AddressTranslator.RangeCalculationBehaviour.LastPart);
      rangeAddress.FromCol = col1;
      rangeAddress.ToCol = col2;
      rangeAddress.FromRow = row1;
      rangeAddress.ToRow = row2;
    }
  }
}
