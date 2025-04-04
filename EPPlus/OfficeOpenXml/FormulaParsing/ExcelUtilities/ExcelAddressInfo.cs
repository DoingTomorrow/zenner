// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.ExcelAddressInfo
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Utilities;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class ExcelAddressInfo
  {
    private ExcelAddressInfo(string address)
    {
      string str = address;
      this.Worksheet = string.Empty;
      if (address.Contains("!"))
      {
        string[] strArray = address.Split('!');
        this.Worksheet = strArray[0];
        str = strArray[1];
      }
      if (str.Contains(":"))
      {
        string[] strArray = str.Split(':');
        this.StartCell = strArray[0];
        this.EndCell = strArray[1];
      }
      else
        this.StartCell = str;
      this.AddressOnSheet = str;
    }

    public static ExcelAddressInfo Parse(string address)
    {
      Require.That<string>(address).Named(nameof (address)).IsNotNullOrEmpty();
      return new ExcelAddressInfo(address);
    }

    public string Worksheet { get; private set; }

    public bool WorksheetIsSpecified => !string.IsNullOrEmpty(this.Worksheet);

    public bool IsMultipleCells => !string.IsNullOrEmpty(this.EndCell);

    public string StartCell { get; private set; }

    public string EndCell { get; private set; }

    public string AddressOnSheet { get; private set; }
  }
}
