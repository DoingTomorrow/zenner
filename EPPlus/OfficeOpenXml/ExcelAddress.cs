// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelAddress
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelAddress : ExcelAddressBase
  {
    private string fullAddress;
    private ExcelPackage _package;

    internal ExcelAddress()
    {
    }

    public ExcelAddress(int fromRow, int fromCol, int toRow, int toColumn)
      : base(fromRow, fromCol, toRow, toColumn)
    {
      this._ws = "";
    }

    public ExcelAddress(string address)
      : base(address)
    {
    }

    internal ExcelAddress(string ws, string address)
      : base(address)
    {
      if (!string.IsNullOrEmpty(this._ws))
        return;
      this._ws = ws;
    }

    internal ExcelAddress(string ws, string address, bool isName)
      : base(address, isName)
    {
      if (!string.IsNullOrEmpty(this._ws))
        return;
      this._ws = ws;
    }

    public ExcelAddress(string Address, ExcelPackage package, ExcelAddressBase referenceAddress)
      : base(Address, package, referenceAddress)
    {
    }

    public new string Address
    {
      get
      {
        if (string.IsNullOrEmpty(this._address) && this._fromRow > 0)
          this._address = ExcelCellBase.GetAddress(this._fromRow, this._fromCol, this._toRow, this._toCol);
        return this._address;
      }
      set
      {
        this.SetAddress(value);
        this.ChangeAddress();
      }
    }
  }
}
