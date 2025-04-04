// Decompiled with JetBrains decompiler
// Type: CellStore`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;

#nullable disable
internal class CellStore<T> : IDisposable
{
  internal const int pageBits = 10;
  internal const int PageSize = 1024;
  internal const int PageSizeMin = 1024;
  internal const int PageSizeMax = 2048;
  internal const int ColSizeMin = 32;
  internal const int PagesPerColumnMin = 32;
  private List<T> _values = new List<T>();
  internal ColumnIndex[] _columnIndex;
  internal IndexBase _searchIx = new IndexBase();
  internal int ColumnCount;
  private int _colPos = -1;
  private int _row;
  private int _col;

  public CellStore() => this._columnIndex = new ColumnIndex[32];

  ~CellStore()
  {
    if (this._values != null)
    {
      this._values.Clear();
      this._values = (List<T>) null;
    }
    this._columnIndex = (ColumnIndex[]) null;
  }

  internal int GetPosition(int Column)
  {
    this._searchIx.Index = (short) Column;
    return Array.BinarySearch<IndexBase>((IndexBase[]) this._columnIndex, 0, this.ColumnCount, this._searchIx);
  }

  internal CellStore<T> Clone()
  {
    CellStore<T> cellStore = new CellStore<T>();
    for (int index1 = 0; index1 < this.ColumnCount; ++index1)
    {
      int index2 = (int) this._columnIndex[index1].Index;
      for (int index3 = 0; index3 < this._columnIndex[index1].PageCount; ++index3)
      {
        for (int index4 = 0; index4 < this._columnIndex[index1]._pages[index3].RowCount; ++index4)
        {
          int Row = this._columnIndex[index1]._pages[index3].IndexOffset + (int) this._columnIndex[index1]._pages[index3].Rows[index4].Index;
          cellStore.SetValue(Row, index2, this._values[this._columnIndex[index1]._pages[index3].Rows[index4].IndexPointer]);
        }
      }
    }
    return cellStore;
  }

  internal int Count
  {
    get
    {
      int count = 0;
      for (int index1 = 0; index1 < this.ColumnCount; ++index1)
      {
        for (int index2 = 0; index2 < this._columnIndex[index1].PageCount; ++index2)
          count += this._columnIndex[index1]._pages[index2].RowCount;
      }
      return count;
    }
  }

  internal bool GetDimension(out int fromRow, out int fromCol, out int toRow, out int toCol)
  {
    if (this.ColumnCount == 0)
    {
      fromRow = fromCol = toRow = toCol = 0;
      return false;
    }
    fromCol = (int) this._columnIndex[0].Index;
    if (fromCol <= 0 && this.ColumnCount > 1)
      fromCol = (int) this._columnIndex[1].Index;
    else if (this.ColumnCount == 1 && fromCol <= 0)
    {
      fromRow = fromCol = toRow = toCol = 0;
      return false;
    }
    toCol = (int) this._columnIndex[this.ColumnCount - 1].Index;
    fromRow = toRow = 0;
    for (int index1 = 0; index1 < this.ColumnCount; ++index1)
    {
      int num1 = this._columnIndex[index1].PageCount <= 0 || this._columnIndex[index1]._pages[0].RowCount <= 0 || this._columnIndex[index1]._pages[0].Rows[0].Index <= (short) 0 ? (this._columnIndex[index1]._pages[0].RowCount <= 1 ? (this._columnIndex[index1].PageCount <= 1 ? 0 : this._columnIndex[index1]._pages[0].IndexOffset + (int) this._columnIndex[index1]._pages[1].Rows[0].Index) : this._columnIndex[index1]._pages[0].IndexOffset + (int) this._columnIndex[index1]._pages[0].Rows[1].Index) : this._columnIndex[index1]._pages[0].IndexOffset + (int) this._columnIndex[index1]._pages[0].Rows[0].Index;
      int index2 = this._columnIndex[index1].PageCount - 1;
      while (this._columnIndex[index1]._pages[index2].RowCount == 0 && index2 != 0)
        --index2;
      PageIndex page = this._columnIndex[index1]._pages[index2];
      int num2 = page.RowCount <= 0 ? num1 : page.IndexOffset + (int) page.Rows[page.RowCount - 1].Index;
      if (num1 > 0 && (num1 < fromRow || fromRow == 0))
        fromRow = num1;
      if (num1 > 0 && (num2 > toRow || toRow == 0))
        toRow = num2;
    }
    if (fromRow > 0 && toRow > 0)
      return true;
    fromRow = fromCol = toRow = toCol = 0;
    return false;
  }

  internal int FindNext(int Column)
  {
    int position = this.GetPosition(Column);
    return position < 0 ? ~position : position;
  }

  internal T GetValue(int Row, int Column)
  {
    int pointer = this.GetPointer(Row, Column);
    return pointer >= 0 ? this._values[pointer] : default (T);
  }

  private int GetPointer(int Row, int Column)
  {
    int position1 = this.GetPosition(Column);
    if (position1 < 0)
      return -1;
    int position2 = this._columnIndex[position1].GetPosition(Row);
    if (position2 < 0 || position2 >= this._columnIndex[position1].PageCount)
      return -1;
    PageIndex page = this._columnIndex[position1]._pages[position2];
    if (page.MinIndex > Row)
    {
      int index = position2 - 1;
      if (index < 0)
        return -1;
      page = this._columnIndex[position1]._pages[index];
    }
    this._searchIx.Index = (short) (Row - page.IndexOffset);
    int index1 = Array.BinarySearch<IndexBase>((IndexBase[]) page.Rows, 0, page.RowCount, this._searchIx);
    return index1 >= 0 ? page.Rows[index1].IndexPointer : -1;
  }

  internal bool Exists(int Row, int Column) => this.GetPointer(Row, Column) >= 0;

  internal bool Exists(int Row, int Column, ref T value)
  {
    int pointer = this.GetPointer(Row, Column);
    if (pointer < 0)
      return false;
    value = this._values[pointer];
    return true;
  }

  internal void SetValue(int Row, int Column, T Value)
  {
    lock (this._columnIndex)
    {
      int index1 = Array.BinarySearch<IndexBase>((IndexBase[]) this._columnIndex, 0, this.ColumnCount, new IndexBase()
      {
        Index = (short) Column
      });
      short index2 = (short) (Row >> 10);
      if (index1 >= 0)
      {
        int index3 = this._columnIndex[index1].GetPosition(Row);
        if (index3 < 0)
        {
          index3 = ~index3;
          if (index3 - 1 < 0 || this._columnIndex[index1]._pages[index3 - 1].IndexOffset + 1024 - 1 < Row)
            this.AddPage(this._columnIndex[index1], index3, index2);
          else
            --index3;
        }
        if (index3 >= this._columnIndex[index1].PageCount)
          this.AddPage(this._columnIndex[index1], index3, index2);
        PageIndex page = this._columnIndex[index1]._pages[index3];
        if (page.IndexOffset > Row)
        {
          --index3;
          short num = (short) ((int) index2 - 1);
          if (index3 < 0)
            throw new Exception("Unexpected error when setting value");
          page = this._columnIndex[index1]._pages[index3];
        }
        short ix = (short) (Row - (((int) page.Index << 10) + page.Offset));
        this._searchIx.Index = ix;
        int index4 = Array.BinarySearch<IndexBase>((IndexBase[]) page.Rows, 0, page.RowCount, this._searchIx);
        if (index4 < 0)
        {
          int pos = ~index4;
          this.AddCell(this._columnIndex[index1], index3, pos, ix, Value);
        }
        else
          this._values[page.Rows[index4].IndexPointer] = Value;
      }
      else
      {
        int pos = ~index1;
        this.AddColumn(pos, Column);
        this.AddPage(this._columnIndex[pos], 0, index2);
        short ix = (short) (Row - ((int) index2 << 10));
        this.AddCell(this._columnIndex[pos], 0, 0, ix, Value);
      }
    }
  }

  internal void Insert(int fromRow, int fromCol, int rows, int columns)
  {
    lock (this._columnIndex)
    {
      if (columns > 0)
      {
        int num = this.GetPosition(fromCol);
        if (num < 0)
          num = ~num;
        for (int index = num; index < this.ColumnCount; ++index)
          this._columnIndex[index].Index += (short) (int) (short) columns;
      }
      else
      {
        int num = fromRow >> 10;
        for (int index = 0; index < this.ColumnCount; ++index)
        {
          ColumnIndex column = this._columnIndex[index];
          int position1 = column.GetPosition(fromRow);
          if (position1 >= 0)
          {
            if (fromRow >= column._pages[position1].MinIndex && fromRow <= column._pages[position1].MaxIndex)
            {
              int offset = fromRow - column._pages[position1].IndexOffset;
              int rowPos = column._pages[position1].GetPosition(offset);
              if (rowPos < 0)
                rowPos = ~rowPos;
              this.UpdateIndexOffset(column, position1, rowPos, fromRow, rows);
            }
            else if (column._pages[position1].MinIndex > fromRow - 1 && position1 > 0)
            {
              int offset = fromRow - (num - 1 << 10);
              int position2 = column._pages[position1 - 1].GetPosition(offset);
              if (position2 > 0 && position1 > 0)
                this.UpdateIndexOffset(column, position1 - 1, position2, fromRow, rows);
            }
            else if (column.PageCount >= position1 + 1)
            {
              int offset = fromRow - column._pages[position1].IndexOffset;
              int rowPos = column._pages[position1].GetPosition(offset);
              if (rowPos < 0)
                rowPos = ~rowPos;
              if (column._pages[position1].RowCount > rowPos)
                this.UpdateIndexOffset(column, position1, rowPos, fromRow, rows);
              else
                this.UpdateIndexOffset(column, position1 + 1, 0, fromRow, rows);
            }
          }
          else
            this.UpdateIndexOffset(column, ~position1, 0, fromRow, rows);
        }
      }
    }
  }

  internal void Clear(int fromRow, int fromCol, int rows, int columns)
  {
    this.Delete(fromRow, fromCol, rows, columns, false);
  }

  internal void Delete(int fromRow, int fromCol, int rows, int columns)
  {
    this.Delete(fromRow, fromCol, rows, columns, true);
  }

  internal void Delete(int fromRow, int fromCol, int rows, int columns, bool shift)
  {
    lock (this._columnIndex)
    {
      if (columns > 0 && fromRow == 1 && rows >= 1048576)
      {
        this.DeleteColumns(fromCol, columns, shift);
      }
      else
      {
        int num1 = fromCol + columns - 1;
        int num2 = fromRow >> 10;
        for (int index = 0; index < this.ColumnCount; ++index)
        {
          ColumnIndex column = this._columnIndex[index];
          if ((int) column.Index >= fromCol)
          {
            if ((int) column.Index > num1)
              break;
            int pagePos1 = column.GetPosition(fromRow);
            if (pagePos1 < 0)
              pagePos1 = ~pagePos1;
            if (pagePos1 < column.PageCount)
            {
              PageIndex page = column._pages[pagePos1];
              if (page.RowCount > 0 && page.MinIndex > fromRow && page.MaxIndex <= fromRow + rows)
              {
                rows -= page.MinIndex - fromRow;
                fromRow = page.MinIndex;
              }
              if (page.RowCount > 0 && page.MinIndex <= fromRow && page.MaxIndex >= fromRow)
              {
                int toRow = fromRow + rows;
                int num3 = this.DeleteCells(column._pages[pagePos1], fromRow, toRow);
                if (shift && num3 != fromRow)
                  this.UpdatePageOffset(column, pagePos1, num3 - fromRow);
                if (toRow > num3 && pagePos1 < column.PageCount && column._pages[pagePos1].MinIndex < toRow)
                {
                  int pagePos2 = num3 == fromRow ? pagePos1 : pagePos1 + 1;
                  int rows1 = this.DeletePage(fromRow, toRow - num3, column, pagePos2);
                  if (rows1 > 0)
                  {
                    int position = column.GetPosition(fromRow);
                    this.DeleteCells(column._pages[position], fromRow, fromRow + rows1);
                    if (shift)
                      this.UpdatePageOffset(column, position, rows1);
                  }
                }
              }
              else if (pagePos1 > 0 && column._pages[pagePos1].IndexOffset > fromRow)
              {
                int offset = fromRow + rows - 1 - (num2 - 1 << 10);
                int position = column._pages[pagePos1 - 1].GetPosition(offset);
                if (position > 0 && pagePos1 > 0 && shift)
                  this.UpdateIndexOffset(column, pagePos1 - 1, position, fromRow + rows - 1, -rows);
              }
              else if (shift && pagePos1 + 1 < column.PageCount)
                this.UpdateIndexOffset(column, pagePos1 + 1, 0, column._pages[pagePos1 + 1].MinIndex, -rows);
            }
          }
        }
      }
    }
  }

  private void UpdatePageOffset(ColumnIndex column, int pagePos, int rows)
  {
    if (++pagePos >= column.PageCount)
      return;
    for (int index = pagePos; index < column.PageCount; ++index)
    {
      if (column._pages[index].Offset - rows <= -1024)
      {
        --column._pages[index].Index;
        column._pages[index].Offset -= rows - 1024;
      }
      else
        column._pages[index].Offset -= rows;
    }
    if (Math.Abs(column._pages[pagePos].Offset) <= 1024 && Math.Abs(column._pages[pagePos].Rows[column._pages[pagePos].RowCount - 1].Index) <= (short) 2048)
      return;
    rows = this.ResetPageOffset(column, pagePos, rows);
  }

  private int ResetPageOffset(ColumnIndex column, int pagePos, int rows)
  {
    PageIndex page1 = column._pages[pagePos];
    if (page1.Offset < -1024)
    {
      PageIndex page2 = column._pages[pagePos - 1];
      short num = -1;
      if ((int) page1.Index - 1 == (int) page2.Index)
      {
        if (page1.IndexOffset + (int) page1.Rows[page1.RowCount - 1].Index - page2.IndexOffset + (int) page2.Rows[0].Index <= 2048)
          this.MergePage(column, pagePos - 1);
      }
      else
      {
        page1.Index -= (short) (int) num;
        page1.Offset += 1024;
      }
    }
    else if (page1.Offset > 1024)
    {
      PageIndex page3 = column._pages[pagePos + 1];
      short num = 1;
      if ((int) page1.Index + 1 != (int) page3.Index)
      {
        page1.Index += (short) (int) num;
        page1.Offset += 1024;
      }
    }
    return rows;
  }

  private int DeletePage(int fromRow, int rows, ColumnIndex column, int pagePos)
  {
    for (PageIndex page = column._pages[pagePos]; page != null && page.MinIndex >= fromRow && page.MaxIndex < fromRow + rows; page = column._pages[pagePos])
    {
      int num = page.MaxIndex - page.MinIndex + 1;
      rows -= num;
      int offset = page.Offset;
      Array.Copy((Array) column._pages, pagePos + 1, (Array) column._pages, pagePos, column.PageCount - pagePos + 1);
      --column.PageCount;
      if (column.PageCount == 0)
        return 0;
      for (int index = pagePos; index < column.PageCount; ++index)
      {
        column._pages[index].Offset -= num;
        if (column._pages[index].Offset <= -1024)
        {
          --column._pages[index].Index;
          column._pages[index].Offset += 1024;
        }
      }
      if (column.PageCount <= pagePos)
        return 0;
    }
    return rows;
  }

  private int DeleteCells(PageIndex page, int fromRow, int toRow)
  {
    int num1 = page.GetPosition(fromRow - page.IndexOffset);
    if (num1 < 0)
      num1 = ~num1;
    int maxIndex1 = page.MaxIndex;
    int offset = toRow - page.IndexOffset;
    if (offset > 2048)
      offset = 2048;
    int sourceIndex = page.GetPosition(offset);
    if (sourceIndex < 0)
      sourceIndex = ~sourceIndex;
    if (num1 <= sourceIndex && num1 < page.RowCount && page.GetIndex(num1) < toRow)
    {
      if (toRow > page.MaxIndex)
      {
        if (fromRow == page.MinIndex)
          return fromRow;
        int maxIndex2 = page.MaxIndex;
        int num2 = page.MaxIndex - page.GetIndex(num1) + 1;
        page.RowCount -= num2;
        return maxIndex2 + 1;
      }
      int num3 = sourceIndex - num1;
      for (int index = sourceIndex; index < page.RowCount; ++index)
        page.Rows[index].Index -= (short) (int) (short) num3;
      Array.Copy((Array) page.Rows, sourceIndex, (Array) page.Rows, num1, page.RowCount - sourceIndex);
      page.RowCount -= num3;
      return toRow;
    }
    return toRow >= maxIndex1 ? maxIndex1 : toRow;
  }

  private void DeleteColumns(int fromCol, int columns, bool shift)
  {
    int sourceIndex = this.GetPosition(fromCol);
    if (sourceIndex < 0)
      sourceIndex = ~sourceIndex;
    int destinationIndex = sourceIndex;
    for (int index = sourceIndex; index < this.ColumnCount && (int) this._columnIndex[index].Index >= fromCol + columns; ++index)
      destinationIndex = index;
    if (this.Count <= sourceIndex)
      return;
    if ((int) this._columnIndex[sourceIndex].Index >= fromCol && (int) this._columnIndex[sourceIndex].Index <= fromCol + columns)
    {
      if ((int) this._columnIndex[sourceIndex].Index > this.ColumnCount)
        Array.Copy((Array) this._columnIndex, sourceIndex, (Array) this._columnIndex, destinationIndex, destinationIndex - sourceIndex);
      this.ColumnCount -= columns;
    }
    if (!shift)
      return;
    for (int index = destinationIndex + 1; index < this.ColumnCount; ++index)
      this._columnIndex[index].Index -= (short) (int) (short) columns;
  }

  private void UpdateIndexOffset(ColumnIndex column, int pagePos, int rowPos, int row, int rows)
  {
    if (pagePos >= column.PageCount)
      return;
    PageIndex page1 = column._pages[pagePos];
    if (rows > 1024)
    {
      short addPages = (short) (rows >> 10);
      int num = rows - 1024 * (int) addPages;
      for (int index = pagePos + 1; index < column.PageCount; ++index)
      {
        if (column._pages[index].Offset + num > 1024)
        {
          column._pages[index].Index += (short) (int) (short) ((int) addPages + 1);
          column._pages[index].Offset += num - 1024;
        }
        else
        {
          column._pages[index].Index += (short) (int) addPages;
          column._pages[index].Offset += num;
        }
      }
      int size = page1.RowCount - rowPos;
      if (page1.RowCount <= rowPos)
        return;
      if (column.PageCount - 1 == pagePos)
      {
        PageIndex page2 = this.CopyNew(page1, rowPos, size);
        page2.Index = (short) (row + rows >> 10);
        page2.Offset = row + rows - (int) page2.Index * 1024 - (int) page2.Rows[0].Index;
        if (page2.Offset > 1024)
        {
          ++page2.Index;
          page2.Offset -= 1024;
        }
        this.AddPage(column, pagePos + 1, page2);
        page1.RowCount = rowPos;
      }
      else if (column._pages[pagePos + 1].RowCount + size > 2048)
        this.SplitPageInsert(column, pagePos, rowPos, rows, size, (int) addPages);
      else
        this.CopyMergePage(page1, rowPos, rows, size, column._pages[pagePos + 1]);
    }
    else
    {
      for (int index = rowPos; index < page1.RowCount; ++index)
        page1.Rows[index].Index += (short) (int) (short) rows;
      if (page1.Offset + (int) page1.Rows[page1.RowCount - 1].Index >= 2048)
      {
        this.AdjustIndex(column, pagePos);
        if (page1.Offset + (int) page1.Rows[page1.RowCount - 1].Index >= 2048)
          pagePos = this.SplitPage(column, pagePos);
      }
      for (int index = pagePos + 1; index < column.PageCount; ++index)
      {
        if (column._pages[index].Offset + rows < 1024)
        {
          column._pages[index].Offset += rows;
        }
        else
        {
          ++column._pages[index].Index;
          column._pages[index].Offset = (column._pages[index].Offset + rows) % 1024;
        }
      }
    }
  }

  private void SplitPageInsert(
    ColumnIndex column,
    int pagePos,
    int rowPos,
    int rows,
    int size,
    int addPages)
  {
    CellStore<T>.GetSize(size);
    PageIndex page1 = column._pages[pagePos];
    int rowPos1 = -1;
    for (int index = rowPos; index < page1.RowCount; ++index)
    {
      if (page1.IndexExpanded - ((int) page1.Rows[index].Index + rows) > 1024)
      {
        rowPos1 = index;
        break;
      }
      page1.Rows[index].Index += (short) (int) (short) rows;
    }
    int size1 = page1.RowCount - rowPos1;
    page1.RowCount = rowPos1;
    if (size1 <= 0)
      return;
    int indexOffset = page1.IndexOffset;
    PageIndex page2 = this.CopyNew(page1, rowPos1, size1);
    short num1 = (short) ((int) page1.Index + addPages);
    int num2 = page1.IndexOffset + rows - (int) num1 * 1024;
    if (num2 > 1024)
    {
      num1 += (short) (num2 / 1024);
      num2 %= 1024;
    }
    page2.Index = num1;
    page2.Offset = num2;
    this.AddPage(column, pagePos + 1, page2);
  }

  private void CopyMergePage(PageIndex page, int rowPos, int rows, int size, PageIndex ToPage)
  {
    int indexOffset = page.IndexOffset;
    int index1 = (int) page.Rows[rowPos].Index;
    IndexItem[] destinationArray = new IndexItem[CellStore<T>.GetSize(ToPage.RowCount + size)];
    page.RowCount -= size;
    Array.Copy((Array) page.Rows, rowPos, (Array) destinationArray, 0, size);
    for (int index2 = 0; index2 < size; ++index2)
      destinationArray[index2].Index += (short) (int) (short) (page.IndexOffset + rows - ToPage.IndexOffset);
    Array.Copy((Array) ToPage.Rows, 0, (Array) destinationArray, size, ToPage.RowCount);
    ToPage.Rows = destinationArray;
    ToPage.RowCount += size;
  }

  private void MergePage(ColumnIndex column, int pagePos)
  {
    PageIndex page1 = column._pages[pagePos];
    PageIndex page2 = column._pages[pagePos + 1];
    PageIndex pageIndex = new PageIndex(page1, 0, page1.RowCount + page2.RowCount);
    pageIndex.RowCount = page1.RowCount + page2.RowCount;
    Array.Copy((Array) page1.Rows, 0, (Array) pageIndex.Rows, 0, page1.RowCount);
    Array.Copy((Array) page2.Rows, 0, (Array) pageIndex.Rows, page1.RowCount, page2.RowCount);
    for (int rowCount = page1.RowCount; rowCount < pageIndex.RowCount; ++rowCount)
      pageIndex.Rows[rowCount].Index += (short) (int) (short) (page2.IndexOffset - page1.IndexOffset);
    column._pages[pagePos] = pageIndex;
    --column.PageCount;
    if (column.PageCount <= pagePos + 1)
      return;
    Array.Copy((Array) column._pages, pagePos + 2, (Array) column._pages, pagePos + 1, column.PageCount - (pagePos + 1));
    for (int index = pagePos + 1; index < column.PageCount; ++index)
    {
      --column._pages[index].Index;
      column._pages[index].Offset += 1024;
    }
  }

  private PageIndex CopyNew(PageIndex pageFrom, int rowPos, int size)
  {
    IndexItem[] indexItemArray = new IndexItem[CellStore<T>.GetSize(size)];
    Array.Copy((Array) pageFrom.Rows, rowPos, (Array) indexItemArray, 0, size);
    return new PageIndex(indexItemArray, size);
  }

  internal static int GetSize(int size)
  {
    int size1 = 256;
    while (size1 < size)
      size1 <<= 1;
    return size1;
  }

  private void AddCell(ColumnIndex columnIndex, int pagePos, int pos, short ix, T value)
  {
    PageIndex page = columnIndex._pages[pagePos];
    if (page.RowCount == page.Rows.Length)
    {
      if (page.RowCount == 2048)
      {
        pagePos = this.SplitPage(columnIndex, pagePos);
        if (columnIndex._pages[pagePos - 1].RowCount > pos)
          --pagePos;
        else
          pos -= columnIndex._pages[pagePos - 1].RowCount;
        page = columnIndex._pages[pagePos];
      }
      else
      {
        IndexItem[] destinationArray = new IndexItem[page.Rows.Length << 1];
        Array.Copy((Array) page.Rows, 0, (Array) destinationArray, 0, page.RowCount);
        page.Rows = destinationArray;
      }
    }
    if (pos < page.RowCount)
      Array.Copy((Array) page.Rows, pos, (Array) page.Rows, pos + 1, page.RowCount - pos);
    IndexItem[] rows = page.Rows;
    int index = pos;
    IndexItem indexItem1 = new IndexItem();
    indexItem1.Index = ix;
    indexItem1.IndexPointer = this._values.Count;
    IndexItem indexItem2 = indexItem1;
    rows[index] = indexItem2;
    this._values.Add(value);
    ++page.RowCount;
  }

  private int SplitPage(ColumnIndex columnIndex, int pagePos)
  {
    PageIndex page = columnIndex._pages[pagePos];
    if (page.Offset != 0)
    {
      int offset = page.Offset;
      page.Offset = 0;
      for (int index = 0; index < page.RowCount; ++index)
        page.Rows[index].Index -= (short) (int) (short) offset;
    }
    int num = 0;
    for (int index = 0; index < page.RowCount; ++index)
    {
      if (page.Rows[index].Index > (short) 1024)
      {
        num = index;
        break;
      }
    }
    PageIndex pageIndex1 = new PageIndex(page, 0, num);
    PageIndex pageIndex2 = new PageIndex(page, num, page.RowCount - num, (short) ((int) page.Index + 1), page.Offset);
    for (int index = 0; index < pageIndex2.RowCount; ++index)
      pageIndex2.Rows[index].Index -= (short) 1024;
    columnIndex._pages[pagePos] = pageIndex1;
    if (columnIndex.PageCount + 1 > columnIndex._pages.Length)
    {
      PageIndex[] destinationArray = new PageIndex[columnIndex._pages.Length << 1];
      Array.Copy((Array) columnIndex._pages, 0, (Array) destinationArray, 0, columnIndex.PageCount);
      columnIndex._pages = destinationArray;
    }
    Array.Copy((Array) columnIndex._pages, pagePos + 1, (Array) columnIndex._pages, pagePos + 2, columnIndex.PageCount - pagePos - 1);
    columnIndex._pages[pagePos + 1] = pageIndex2;
    ++columnIndex.PageCount;
    return pagePos + 1;
  }

  private PageIndex AdjustIndex(ColumnIndex columnIndex, int pagePos)
  {
    PageIndex page = columnIndex._pages[pagePos];
    if (page.Offset + (int) page.Rows[0].Index >= 1024 || page.Offset >= 1024 || page.Rows[0].Index >= (short) 1024)
    {
      ++page.Index;
      page.Offset -= 1024;
    }
    else if (page.Offset + (int) page.Rows[0].Index <= -1024 || page.Offset <= -1024 || page.Rows[0].Index <= (short) -1024)
    {
      --page.Index;
      page.Offset += 1024;
    }
    return page;
  }

  private void AddPageRowOffset(PageIndex page, short offset)
  {
    for (int index = 0; index < page.RowCount; ++index)
      page.Rows[index].Index += (short) (int) offset;
  }

  private void AddPage(ColumnIndex column, int pos, short index)
  {
    this.AddPage(column, pos);
    PageIndex[] pages = column._pages;
    int index1 = pos;
    PageIndex pageIndex1 = new PageIndex();
    pageIndex1.Index = index;
    PageIndex pageIndex2 = pageIndex1;
    pages[index1] = pageIndex2;
    if (pos <= 0)
      return;
    PageIndex page = column._pages[pos - 1];
    if (page.RowCount <= 0 || page.Rows[page.RowCount - 1].Index <= (short) 1024)
      return;
    column._pages[pos].Offset = (int) page.Rows[page.RowCount - 1].Index - 1024;
  }

  private void AddPage(ColumnIndex column, int pos, PageIndex page)
  {
    this.AddPage(column, pos);
    column._pages[pos] = page;
  }

  private void AddPage(ColumnIndex column, int pos)
  {
    if (column.PageCount == column._pages.Length)
    {
      PageIndex[] destinationArray = new PageIndex[column._pages.Length * 2];
      Array.Copy((Array) column._pages, 0, (Array) destinationArray, 0, column.PageCount);
      column._pages = destinationArray;
    }
    if (pos < column.PageCount)
      Array.Copy((Array) column._pages, pos, (Array) column._pages, pos + 1, column.PageCount - pos);
    ++column.PageCount;
  }

  private void AddColumn(int pos, int Column)
  {
    if (this.ColumnCount == this._columnIndex.Length)
    {
      ColumnIndex[] destinationArray = new ColumnIndex[this._columnIndex.Length * 2];
      Array.Copy((Array) this._columnIndex, 0, (Array) destinationArray, 0, this.ColumnCount);
      this._columnIndex = destinationArray;
    }
    if (pos < this.ColumnCount)
      Array.Copy((Array) this._columnIndex, pos, (Array) this._columnIndex, pos + 1, this.ColumnCount - pos);
    ColumnIndex[] columnIndex1 = this._columnIndex;
    int index = pos;
    ColumnIndex columnIndex2 = new ColumnIndex();
    columnIndex2.Index = (short) Column;
    ColumnIndex columnIndex3 = columnIndex2;
    columnIndex1[index] = columnIndex3;
    ++this.ColumnCount;
  }

  public ulong Current
  {
    get => (ulong) this._row << 32 | (ulong) (uint) this._columnIndex[this._colPos].Index;
  }

  public void Dispose()
  {
    if (this._values != null)
      this._values.Clear();
    for (int index = 0; index < this.ColumnCount; ++index)
    {
      if (this._columnIndex[index] != null)
        this._columnIndex[index].Dispose();
    }
    this._values = (List<T>) null;
    this._columnIndex = (ColumnIndex[]) null;
  }

  public bool MoveNext() => this.GetNextCell(ref this._row, ref this._colPos, 0, 1048576, 16384);

  internal bool NextCell(ref int row, ref int col)
  {
    return this.NextCell(ref row, ref col, 0, 0, 1048576, 16384);
  }

  internal bool NextCell(
    ref int row,
    ref int col,
    int minRow,
    int minColPos,
    int maxRow,
    int maxColPos)
  {
    if (minColPos >= this.ColumnCount)
      return false;
    if (maxColPos >= this.ColumnCount)
      maxColPos = this.ColumnCount - 1;
    int position = this.GetPosition(col);
    if (position >= 0)
    {
      if (position > maxColPos)
      {
        if (col <= minColPos)
          return false;
        col = minColPos;
        return this.NextCell(ref row, ref col);
      }
      bool nextCell = this.GetNextCell(ref row, ref position, minColPos, maxRow, maxColPos);
      col = (int) this._columnIndex[position].Index;
      return nextCell;
    }
    int row1 = ~position;
    if (row1 > (int) this._columnIndex[row1].Index)
    {
      if (col <= minColPos)
        return false;
      col = minColPos;
      return this.NextCell(ref row, ref col, minRow, minColPos, maxRow, maxColPos);
    }
    bool nextCell1 = this.GetNextCell(ref row1, ref row, minColPos, maxRow, maxColPos);
    col = (int) this._columnIndex[row1].Index;
    return nextCell1;
  }

  internal bool GetNextCell(
    ref int row,
    ref int colPos,
    int startColPos,
    int endRow,
    int endColPos)
  {
    if (this.ColumnCount == 0)
      return false;
    if (++colPos < this.ColumnCount && colPos <= endColPos)
    {
      int nextRow1 = this._columnIndex[colPos].GetNextRow(row);
      if (nextRow1 == row)
        return true;
      int num1;
      int num2;
      if (nextRow1 > row)
      {
        num1 = nextRow1;
        num2 = colPos;
      }
      else
      {
        num1 = int.MaxValue;
        num2 = 0;
      }
      for (int index = colPos + 1; index < this.ColumnCount && index <= endColPos; ++index)
      {
        int nextRow2 = this._columnIndex[index].GetNextRow(row);
        if (nextRow2 == row)
        {
          colPos = index;
          return true;
        }
        if (nextRow2 > row && nextRow2 < num1)
        {
          num1 = nextRow2;
          num2 = index;
        }
      }
      int index1 = startColPos;
      if (row < endRow)
      {
        ++row;
        for (; index1 < colPos; ++index1)
        {
          int nextRow3 = this._columnIndex[index1].GetNextRow(row);
          if (nextRow3 == row)
          {
            colPos = index1;
            return true;
          }
          if (nextRow3 > row && (nextRow3 < num1 || nextRow3 == num1 && index1 < num2) && nextRow3 <= endRow)
          {
            num1 = nextRow3;
            num2 = index1;
          }
        }
      }
      if (num1 == int.MaxValue || num1 > endRow)
        return false;
      row = num1;
      colPos = num2;
      return true;
    }
    if (colPos <= startColPos || row >= endRow)
      return false;
    colPos = startColPos - 1;
    ++row;
    return this.GetNextCell(ref row, ref colPos, startColPos, endRow, endColPos);
  }

  internal bool GetNextCell(
    ref int row,
    ref int colPos,
    int startColPos,
    int endRow,
    int endColPos,
    ref int[] pagePos,
    ref int[] cellPos)
  {
    if (colPos == endColPos)
    {
      colPos = startColPos;
      ++row;
    }
    else
      ++colPos;
    if (pagePos[colPos] < 0)
    {
      if (pagePos[colPos] == -1)
        pagePos[colPos] = this._columnIndex[colPos].GetPosition(row);
    }
    else if (this._columnIndex[colPos]._pages[pagePos[colPos]].RowCount <= row)
    {
      if (this._columnIndex[colPos].PageCount > pagePos[colPos])
        ++pagePos[colPos];
      else
        pagePos[colPos] = -2;
    }
    int num = this._columnIndex[colPos]._pages[pagePos[colPos]].IndexOffset + (int) this._columnIndex[colPos]._pages[pagePos[colPos]].Rows[cellPos[colPos]].Index;
    if (num == row)
      row = num;
    return true;
  }

  internal bool PrevCell(ref int row, ref int col)
  {
    return this.PrevCell(ref row, ref col, 0, 0, 1048576, 16384);
  }

  internal bool PrevCell(
    ref int row,
    ref int col,
    int minRow,
    int minColPos,
    int maxRow,
    int maxColPos)
  {
    if (minColPos >= this.ColumnCount)
      return false;
    if (maxColPos >= this.ColumnCount)
      maxColPos = this.ColumnCount - 1;
    int position = this.GetPosition(col);
    if (position >= 0)
    {
      if (position == 0)
      {
        if (col >= maxColPos || row == minRow)
          return false;
        --row;
        col = maxColPos;
        return this.PrevCell(ref row, ref col, minRow, minColPos, maxRow, maxColPos);
      }
      bool prevCell = this.GetPrevCell(ref row, ref position, minRow, minColPos, maxColPos);
      if (prevCell)
        col = (int) this._columnIndex[position].Index;
      return prevCell;
    }
    int colPos = ~position;
    if (colPos == 0)
    {
      if (col >= maxColPos)
        return false;
      col = maxColPos;
      return this.PrevCell(ref row, ref col, minRow, minColPos, maxRow, maxColPos);
    }
    bool prevCell1 = this.GetPrevCell(ref row, ref colPos, minRow, minColPos, maxColPos);
    if (prevCell1)
      col = (int) this._columnIndex[colPos].Index;
    return prevCell1;
  }

  internal bool GetPrevCell(
    ref int row,
    ref int colPos,
    int startRow,
    int startColPos,
    int endColPos)
  {
    if (this.ColumnCount == 0)
      return false;
    if (--colPos >= startColPos)
    {
      int nextRow1 = this._columnIndex[colPos].GetNextRow(row);
      if (nextRow1 == row)
        return true;
      int num1;
      int num2;
      if (nextRow1 > row && nextRow1 >= startRow)
      {
        num1 = nextRow1;
        num2 = colPos;
      }
      else
      {
        num1 = int.MaxValue;
        num2 = 0;
      }
      int index1 = colPos + 1;
      if (index1 <= endColPos)
      {
        for (; index1 >= 0; --index1)
        {
          int nextRow2 = this._columnIndex[index1].GetNextRow(row);
          if (nextRow2 == row)
          {
            colPos = index1;
            return true;
          }
          if (nextRow2 > row && nextRow2 < num1 && nextRow2 >= startRow)
          {
            num1 = nextRow2;
            num2 = index1;
          }
        }
      }
      if (row > startRow)
      {
        int index2 = endColPos;
        --row;
        for (; index2 > colPos; --index2)
        {
          int nextRow3 = this._columnIndex[index2].GetNextRow(row);
          if (nextRow3 == row)
          {
            colPos = index2;
            return true;
          }
          if (nextRow3 > row && nextRow3 < num1 && nextRow3 >= startRow)
          {
            num1 = nextRow3;
            num2 = index2;
          }
        }
      }
      if (num1 == int.MaxValue || startRow < num1)
        return false;
      row = num1;
      colPos = num2;
      return true;
    }
    colPos = this.ColumnCount;
    --row;
    if (row >= startRow)
      return this.GetPrevCell(ref colPos, ref row, startRow, startColPos, endColPos);
    this.Reset();
    return false;
  }

  public void Reset()
  {
    this._colPos = -1;
    this._row = 0;
  }
}
