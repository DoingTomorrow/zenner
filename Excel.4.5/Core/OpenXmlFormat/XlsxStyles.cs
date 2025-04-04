// Decompiled with JetBrains decompiler
// Type: Excel.Core.OpenXmlFormat.XlsxStyles
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Collections.Generic;

#nullable disable
namespace Excel.Core.OpenXmlFormat
{
  internal class XlsxStyles
  {
    private List<XlsxXf> _cellXfs;
    private List<XlsxNumFmt> _NumFmts;

    public XlsxStyles()
    {
      this._cellXfs = new List<XlsxXf>();
      this._NumFmts = new List<XlsxNumFmt>();
    }

    public List<XlsxXf> CellXfs
    {
      get => this._cellXfs;
      set => this._cellXfs = value;
    }

    public List<XlsxNumFmt> NumFmts
    {
      get => this._NumFmts;
      set => this._NumFmts = value;
    }
  }
}
