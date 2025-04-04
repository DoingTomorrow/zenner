// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.IndexToAddressTranslator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Utilities;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class IndexToAddressTranslator
  {
    private readonly ExcelDataProvider _excelDataProvider;
    private readonly ExcelReferenceType _excelReferenceType;

    public IndexToAddressTranslator(ExcelDataProvider excelDataProvider)
      : this(excelDataProvider, ExcelReferenceType.AbsoluteRowAndColumn)
    {
    }

    public IndexToAddressTranslator(
      ExcelDataProvider excelDataProvider,
      ExcelReferenceType referenceType)
    {
      Require.That<ExcelDataProvider>(excelDataProvider).Named(nameof (excelDataProvider)).IsNotNull<ExcelDataProvider>();
      this._excelDataProvider = excelDataProvider;
      this._excelReferenceType = referenceType;
    }

    protected internal static string GetColumnLetter(int iColumnNumber, bool fixedCol)
    {
      if (iColumnNumber < 1)
        return "#REF!";
      string str = "";
      do
      {
        str = ((char) (65 + (iColumnNumber - 1) % 26)).ToString() + str;
        iColumnNumber = (iColumnNumber - (iColumnNumber - 1) % 26) / 26;
      }
      while (iColumnNumber > 0);
      return !fixedCol ? str : "$" + str;
    }

    public string ToAddress(int col, int row)
    {
      bool fixedCol = this._excelReferenceType == ExcelReferenceType.AbsoluteRowAndColumn || this._excelReferenceType == ExcelReferenceType.RelativeRowAbsolutColumn;
      return IndexToAddressTranslator.GetColumnLetter(col, fixedCol) + this.GetRowNumber(row);
    }

    private string GetRowNumber(int rowNo)
    {
      string rowNumber = rowNo < this._excelDataProvider.ExcelMaxRows ? rowNo.ToString() : string.Empty;
      if (string.IsNullOrEmpty(rowNumber))
        return rowNumber;
      switch (this._excelReferenceType)
      {
        case ExcelReferenceType.AbsoluteRowAndColumn:
        case ExcelReferenceType.AbsoluteRowRelativeColumn:
          return "$" + rowNumber;
        default:
          return rowNumber;
      }
    }
  }
}
