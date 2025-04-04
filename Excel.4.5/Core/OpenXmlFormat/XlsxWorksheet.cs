// Decompiled with JetBrains decompiler
// Type: Excel.Core.OpenXmlFormat.XlsxWorksheet
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.OpenXmlFormat
{
  internal class XlsxWorksheet
  {
    public const string N_dimension = "dimension";
    public const string N_worksheet = "worksheet";
    public const string N_row = "row";
    public const string N_col = "col";
    public const string N_c = "c";
    public const string N_v = "v";
    public const string N_t = "t";
    public const string A_ref = "ref";
    public const string A_r = "r";
    public const string A_t = "t";
    public const string A_s = "s";
    public const string N_sheetData = "sheetData";
    public const string N_inlineStr = "inlineStr";
    private XlsxDimension _dimension;
    private string _Name;
    private int _id;
    private string _rid;
    private string _path;

    public bool IsEmpty { get; set; }

    public XlsxDimension Dimension
    {
      get => this._dimension;
      set => this._dimension = value;
    }

    public int ColumnsCount
    {
      get
      {
        if (this.IsEmpty)
          return 0;
        return this._dimension != null ? this._dimension.LastCol : -1;
      }
    }

    public int RowsCount
    {
      get => this._dimension != null ? this._dimension.LastRow - this._dimension.FirstRow + 1 : -1;
    }

    public string Name => this._Name;

    public int Id => this._id;

    public string RID
    {
      get => this._rid;
      set => this._rid = value;
    }

    public string Path
    {
      get => this._path;
      set => this._path = value;
    }

    public XlsxWorksheet(string name, int id, string rid)
    {
      this._Name = name;
      this._id = id;
      this._rid = rid;
    }
  }
}
