// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.AddressTranslator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Utilities;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class AddressTranslator
  {
    private readonly ExcelDataProvider _excelDataProvider;

    public AddressTranslator(ExcelDataProvider excelDataProvider)
    {
      Require.That<ExcelDataProvider>(excelDataProvider).Named(nameof (excelDataProvider)).IsNotNull<ExcelDataProvider>();
      this._excelDataProvider = excelDataProvider;
    }

    public virtual void ToColAndRow(string address, out int col, out int row)
    {
      this.ToColAndRow(address, out col, out row, AddressTranslator.RangeCalculationBehaviour.FirstPart);
    }

    public virtual void ToColAndRow(
      string address,
      out int col,
      out int row,
      AddressTranslator.RangeCalculationBehaviour behaviour)
    {
      address = address.ToUpper();
      string alphaPart = this.GetAlphaPart(address);
      col = 0;
      int num1 = 26;
      for (int index = 0; index < alphaPart.Length; ++index)
      {
        int num2 = alphaPart.Length - index - 1;
        int numericAlphaValue = this.GetNumericAlphaValue(alphaPart[index]);
        col += num1 * num2 * numericAlphaValue;
        if (num2 == 0)
          col += numericAlphaValue;
      }
      row = this.GetIntPart(address) ?? this.GetRowIndexByBehaviour(behaviour);
    }

    private int GetRowIndexByBehaviour(
      AddressTranslator.RangeCalculationBehaviour behaviour)
    {
      return behaviour == AddressTranslator.RangeCalculationBehaviour.FirstPart ? 1 : this._excelDataProvider.ExcelMaxRows;
    }

    private int GetNumericAlphaValue(char c) => (int) c - 64;

    private string GetAlphaPart(string address) => Regex.Match(address, "[A-Z]+").Value;

    private int? GetIntPart(string address)
    {
      return Regex.IsMatch(address, "[0-9]+") ? new int?(int.Parse(Regex.Match(address, "[0-9]+").Value)) : new int?();
    }

    public enum RangeCalculationBehaviour
    {
      FirstPart,
      LastPart,
    }
  }
}
