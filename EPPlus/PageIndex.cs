// Decompiled with JetBrains decompiler
// Type: PageIndex
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
internal class PageIndex : IndexBase, IDisposable
{
  internal IndexBase _searchIx = new IndexBase();
  internal int Offset;
  internal int RowCount;

  public PageIndex()
  {
    this.Rows = new IndexItem[1024];
    this.RowCount = 0;
  }

  public PageIndex(IndexItem[] rows, int count)
  {
    this.Rows = rows;
    this.RowCount = count;
  }

  public PageIndex(PageIndex pageItem, int start, int size)
    : this(pageItem, start, size, pageItem.Index, pageItem.Offset)
  {
  }

  public PageIndex(PageIndex pageItem, int start, int size, short index, int offset)
  {
    this.Rows = new IndexItem[CellStore<int>.GetSize(size)];
    Array.Copy((Array) pageItem.Rows, start, (Array) this.Rows, 0, size);
    this.RowCount = size;
    this.Index = index;
    this.Offset = offset;
  }

  ~PageIndex() => this.Rows = (IndexItem[]) null;

  internal int IndexOffset => this.IndexExpanded + this.Offset;

  internal int IndexExpanded => (int) this.Index << 10;

  internal IndexItem[] Rows { get; set; }

  internal int GetPosition(int offset)
  {
    this._searchIx.Index = (short) offset;
    return Array.BinarySearch<IndexBase>((IndexBase[]) this.Rows, 0, this.RowCount, this._searchIx);
  }

  internal int GetNextRow(int row)
  {
    int position = this.GetPosition(row - this.IndexOffset);
    if (position >= 0)
      return position;
    int num = ~position;
    return num < this.RowCount ? num : -1;
  }

  public int MinIndex => this.Rows.Length > 0 ? this.IndexOffset + (int) this.Rows[0].Index : -1;

  public int MaxIndex
  {
    get => this.RowCount > 0 ? this.IndexOffset + (int) this.Rows[this.RowCount - 1].Index : -1;
  }

  public int GetIndex(int pos) => this.IndexOffset + (int) this.Rows[pos].Index;

  public void Dispose() => this.Rows = (IndexItem[]) null;
}
