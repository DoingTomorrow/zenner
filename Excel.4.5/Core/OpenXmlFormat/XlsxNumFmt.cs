// Decompiled with JetBrains decompiler
// Type: Excel.Core.OpenXmlFormat.XlsxNumFmt
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.OpenXmlFormat
{
  internal class XlsxNumFmt
  {
    public const string N_numFmt = "numFmt";
    public const string A_numFmtId = "numFmtId";
    public const string A_formatCode = "formatCode";
    private int _Id;
    private string _FormatCode;

    public int Id
    {
      get => this._Id;
      set => this._Id = value;
    }

    public string FormatCode
    {
      get => this._FormatCode;
      set => this._FormatCode = value;
    }

    public XlsxNumFmt(int id, string formatCode)
    {
      this._Id = id;
      this._FormatCode = formatCode;
    }
  }
}
