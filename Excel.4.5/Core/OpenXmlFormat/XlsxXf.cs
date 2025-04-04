// Decompiled with JetBrains decompiler
// Type: Excel.Core.OpenXmlFormat.XlsxXf
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.OpenXmlFormat
{
  internal class XlsxXf
  {
    public const string N_xf = "xf";
    public const string A_numFmtId = "numFmtId";
    public const string A_xfId = "xfId";
    public const string A_applyNumberFormat = "applyNumberFormat";
    private int _Id;
    private int _numFmtId;
    private bool _applyNumberFormat;

    public int Id
    {
      get => this._Id;
      set => this._Id = value;
    }

    public int NumFmtId
    {
      get => this._numFmtId;
      set => this._numFmtId = value;
    }

    public bool ApplyNumberFormat
    {
      get => this._applyNumberFormat;
      set => this._applyNumberFormat = value;
    }

    public XlsxXf(int id, int numFmtId, string applyNumberFormat)
    {
      this._Id = id;
      this._numFmtId = numFmtId;
      this._applyNumberFormat = applyNumberFormat != null && applyNumberFormat == "1";
    }
  }
}
