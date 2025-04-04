// Decompiled with JetBrains decompiler
// Type: CellsStoreEnumerator`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
internal class CellsStoreEnumerator<T> : 
  IEnumerable<T>,
  IEnumerable,
  IEnumerator<T>,
  IDisposable,
  IEnumerator
{
  private CellStore<T> _cellStore;
  private int row;
  private int colPos;
  private int[] pagePos;
  private int[] cellPos;
  private int _startRow;
  private int _startCol;
  private int _endRow;
  private int _endCol;
  private int minRow;
  private int minColPos;
  private int maxRow;
  private int maxColPos;

  public CellsStoreEnumerator(CellStore<T> cellStore)
    : this(cellStore, 0, 0, 1048576, 16384)
  {
  }

  public CellsStoreEnumerator(
    CellStore<T> cellStore,
    int StartRow,
    int StartCol,
    int EndRow,
    int EndCol)
  {
    this._cellStore = cellStore;
    this._startRow = StartRow;
    this._startCol = StartCol;
    this._endRow = EndRow;
    this._endCol = EndCol;
    this.Init();
  }

  internal void Init()
  {
    this.minRow = this._startRow;
    this.maxRow = this._endRow;
    this.minColPos = this._cellStore.GetPosition(this._startCol);
    if (this.minColPos < 0)
      this.minColPos = ~this.minColPos;
    this.maxColPos = this._cellStore.GetPosition(this._endCol);
    if (this.maxColPos < 0)
      this.maxColPos = ~this.maxColPos - 1;
    this.row = this.minRow;
    this.colPos = this.minColPos - 1;
    int length = this.maxColPos - this.minColPos + 1;
    this.pagePos = new int[length];
    this.cellPos = new int[length];
    for (int index = 0; index < length; ++index)
    {
      this.pagePos[index] = -1;
      this.cellPos[index] = -1;
    }
  }

  internal int Row => this.row;

  internal int Column
  {
    get
    {
      if (this.colPos == -1)
        this.MoveNext();
      return this.colPos == -1 ? 0 : (int) this._cellStore._columnIndex[this.colPos].Index;
    }
  }

  internal T Value
  {
    get
    {
      lock (this._cellStore)
        return this._cellStore.GetValue(this.row, this.Column);
    }
    set
    {
      lock (this._cellStore)
        this._cellStore.SetValue(this.row, this.Column, value);
    }
  }

  internal bool Next()
  {
    return this._cellStore.GetNextCell(ref this.row, ref this.colPos, this.minColPos, this.maxRow, this.maxColPos);
  }

  internal bool Previous()
  {
    lock (this._cellStore)
      return this._cellStore.GetPrevCell(ref this.row, ref this.colPos, this.minRow, this.minColPos, this.maxColPos);
  }

  public string CellAddress => ExcelCellBase.GetAddress(this.Row, this.Column);

  public IEnumerator<T> GetEnumerator()
  {
    this.Reset();
    return (IEnumerator<T>) this;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    this.Reset();
    return (IEnumerator) this;
  }

  public T Current => this.Value;

  public void Dispose()
  {
  }

  object IEnumerator.Current
  {
    get
    {
      this.Reset();
      return (object) this;
    }
  }

  public bool MoveNext() => this.Next();

  public void Reset() => this.Init();
}
