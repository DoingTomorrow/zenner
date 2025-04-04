// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelEncryption
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelEncryption
  {
    private bool _isEncrypted;
    private string _password;
    private EncryptionVersion _version = EncryptionVersion.Agile;

    internal ExcelEncryption() => this.Algorithm = EncryptionAlgorithm.AES256;

    internal ExcelEncryption(EncryptionAlgorithm encryptionAlgorithm)
    {
      this.Algorithm = encryptionAlgorithm;
    }

    public bool IsEncrypted
    {
      get => this._isEncrypted;
      set
      {
        this._isEncrypted = value;
        if (this._isEncrypted)
        {
          if (this._password != null)
            return;
          this._password = "";
        }
        else
          this._password = (string) null;
      }
    }

    public string Password
    {
      get => this._password;
      set
      {
        this._password = value;
        this._isEncrypted = value != null;
      }
    }

    public EncryptionAlgorithm Algorithm { get; set; }

    public EncryptionVersion Version
    {
      get => this._version;
      set
      {
        if (value == this.Version)
          return;
        this.Algorithm = value != EncryptionVersion.Agile ? EncryptionAlgorithm.AES128 : EncryptionAlgorithm.AES256;
        this._version = value;
      }
    }
  }
}
