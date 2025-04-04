// Decompiled with JetBrains decompiler
// Type: ColumnIndex
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
internal class ColumnIndex : IndexBase, IDisposable
{
  internal IndexBase _searchIx = new IndexBase();
  internal PageIndex[] _pages;
  internal int PageCount;

  public ColumnIndex()
  {
    this._pages = new PageIndex[32];
    this.PageCount = 0;
  }

  ~ColumnIndex() => this._pages = (PageIndex[]) null;

  internal int GetPosition(int Row)
  {
    this._searchIx.Index = (short) (Row >> 10);
    int res1 = Array.BinarySearch<IndexBase>((IndexBase[]) this._pages, 0, this.PageCount, this._searchIx);
    if (res1 >= 0)
    {
      this.GetPage(Row, ref res1);
      return res1;
    }
    int res2 = ~res1;
    return this.GetPage(Row, ref res2) ? res2 : res1;
  }

  private bool GetPage(int Row, ref int res)
  {
    if (res < this.PageCount && this._pages[res].MinIndex <= Row && this._pages[res].MaxIndex >= Row)
      return true;
    if (res + 1 < this.PageCount && this._pages[res + 1].MinIndex <= Row)
    {
      do
      {
        ++res;
      }
      while (res + 1 < this.PageCount && this._pages[res + 1].MinIndex <= Row);
      return true;
    }
    if (res - 1 < 0 || this._pages[res - 1].MaxIndex < Row)
      return false;
    do
    {
      --res;
    }
    while (res - 1 > 0 && this._pages[res - 1].MaxIndex >= Row);
    return true;
  }

  internal int GetNextRow(int row)
  {
    int position = this.GetPosition(row);
    if (position < 0)
    {
      int index = ~position;
      if (index >= this.PageCount)
        return -1;
      if (this._pages[index].IndexOffset + (int) this._pages[index].Rows[0].Index >= row)
        return this._pages[index].IndexOffset + (int) this._pages[index].Rows[0].Index;
      return index + 1 >= this.PageCount ? -1 : this._pages[index + 1].IndexOffset + (int) this._pages[index].Rows[0].Index;
    }
    if (position >= this.PageCount)
      return -1;
    int nextRow = this._pages[position].GetNextRow(row);
    if (nextRow >= 0)
      return this._pages[position].IndexOffset + (int) this._pages[position].Rows[nextRow].Index;
    int index1;
    return (index1 = position + 1) < this.PageCount ? this._pages[index1].IndexOffset + (int) this._pages[index1].Rows[0].Index : -1;
  }

  internal int FindNext(int Page)
  {
    int position = this.GetPosition(Page);
    return position < 0 ? ~position : position;
  }

  public void Dispose()
  {
    for (int index = 0; index < this.PageCount; ++index)
      this._pages[index].Dispose();
    this._pages = (PageIndex[]) null;
  }
}
