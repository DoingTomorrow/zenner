// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.RangeCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml
{
  internal class RangeCollection : IEnumerator<IRangeID>, IEnumerator, IEnumerable, IDisposable
  {
    private RangeCollection.IndexItem[] _cellIndex;
    private List<IRangeID> _cells;
    private static readonly RangeCollection.Compare _comparer = new RangeCollection.Compare();
    private int _ix = -1;

    internal RangeCollection(List<IRangeID> cells)
    {
      this._cells = cells;
      this.InitSize(this._cells);
      for (int index = 0; index < this._cells.Count; ++index)
        this._cellIndex[index] = new RangeCollection.IndexItem(cells[index].RangeID, index);
    }

    ~RangeCollection()
    {
      this._cells = (List<IRangeID>) null;
      this._cellIndex = (RangeCollection.IndexItem[]) null;
    }

    internal IRangeID this[ulong RangeID]
    {
      get => this._cells[this._cellIndex[this.IndexOf(RangeID)].ListPointer];
    }

    internal IRangeID this[int Index] => this._cells[this._cellIndex[Index].ListPointer];

    internal int Count => this._cells.Count;

    internal void Add(IRangeID cell)
    {
      int num = this.IndexOf(cell.RangeID);
      if (num >= 0)
        throw new Exception("Item already exists");
      this.Insert(~num, cell);
    }

    internal void Delete(ulong key)
    {
      int destinationIndex = this.IndexOf(key);
      if (destinationIndex < 0)
        throw new Exception("Key does not exists");
      int listPointer = this._cellIndex[destinationIndex].ListPointer;
      Array.Copy((Array) this._cellIndex, destinationIndex + 1, (Array) this._cellIndex, destinationIndex, this._cells.Count - destinationIndex - 1);
      this._cells.RemoveAt(listPointer);
      for (int index = 0; index < this._cells.Count; ++index)
      {
        if (this._cellIndex[index].ListPointer >= listPointer)
          --this._cellIndex[index].ListPointer;
      }
    }

    internal int IndexOf(ulong key)
    {
      return Array.BinarySearch<RangeCollection.IndexItem>(this._cellIndex, 0, this._cells.Count, new RangeCollection.IndexItem(key), (IComparer<RangeCollection.IndexItem>) RangeCollection._comparer);
    }

    internal bool ContainsKey(ulong key) => this.IndexOf(key) >= 0;

    private int _size { get; set; }

    internal int InsertRowsUpdateIndex(ulong rowID, int rows)
    {
      int num1 = this.IndexOf(rowID);
      if (num1 < 0)
        num1 = ~num1;
      ulong num2 = (ulong) rows << 29;
      for (int index = num1; index < this._cells.Count; ++index)
        this._cellIndex[index].RangeID += num2;
      return num1;
    }

    internal int InsertRows(ulong rowID, int rows)
    {
      int num1 = this.IndexOf(rowID);
      if (num1 < 0)
        num1 = ~num1;
      ulong num2 = (ulong) rows << 29;
      for (int index = num1; index < this._cells.Count; ++index)
      {
        this._cellIndex[index].RangeID += num2;
        this._cells[this._cellIndex[index].ListPointer].RangeID += num2;
      }
      return num1;
    }

    internal int DeleteRows(ulong rowID, int rows, bool updateCells)
    {
      ulong num1 = (ulong) rows << 29;
      int index1 = this.IndexOf(rowID);
      if (index1 < 0)
        index1 = ~index1;
      if (index1 >= this._cells.Count || this._cellIndex[index1] == null)
        return -1;
      while (index1 < this._cells.Count && this._cellIndex[index1].RangeID < rowID + num1)
        this.Delete(this._cellIndex[index1].RangeID);
      int num2 = this.IndexOf(rowID + num1);
      if (num2 < 0)
        num2 = ~num2;
      for (int index2 = num2; index2 < this._cells.Count; ++index2)
      {
        this._cellIndex[index2].RangeID -= num1;
        if (updateCells)
          this._cells[this._cellIndex[index2].ListPointer].RangeID -= num1;
      }
      return index1;
    }

    internal void InsertColumn(ulong ColumnID, int columns)
    {
      throw new Exception("Working on it...");
    }

    internal void DeleteColumn(ulong ColumnID, int columns)
    {
      throw new Exception("Working on it...");
    }

    private void InitSize(List<IRangeID> _cells)
    {
      this._size = 128;
      while (_cells.Count > this._size)
        this._size <<= 1;
      this._cellIndex = new RangeCollection.IndexItem[this._size];
    }

    private void CheckSize()
    {
      if (this._cells.Count < this._size)
        return;
      this._size <<= 1;
      Array.Resize<RangeCollection.IndexItem>(ref this._cellIndex, this._size);
    }

    private void Insert(int ix, IRangeID cell)
    {
      this.CheckSize();
      Array.Copy((Array) this._cellIndex, ix, (Array) this._cellIndex, ix + 1, this._cells.Count - ix);
      this._cellIndex[ix] = new RangeCollection.IndexItem(cell.RangeID, this._cells.Count);
      this._cells.Add(cell);
    }

    IRangeID IEnumerator<IRangeID>.Current => throw new NotImplementedException();

    void IDisposable.Dispose() => this._ix = -1;

    object IEnumerator.Current => (object) this._cells[this._cellIndex[this._ix].ListPointer];

    bool IEnumerator.MoveNext()
    {
      ++this._ix;
      return this._ix < this._cells.Count;
    }

    void IEnumerator.Reset() => this._ix = -1;

    IEnumerator IEnumerable.GetEnumerator() => this.MemberwiseClone() as IEnumerator;

    private class IndexItem
    {
      internal ulong RangeID;
      internal int ListPointer;

      internal IndexItem(ulong cellId) => this.RangeID = cellId;

      internal IndexItem(ulong cellId, int listPointer)
      {
        this.RangeID = cellId;
        this.ListPointer = listPointer;
      }
    }

    internal class Compare : IComparer<RangeCollection.IndexItem>
    {
      int IComparer<RangeCollection.IndexItem>.Compare(
        RangeCollection.IndexItem x,
        RangeCollection.IndexItem y)
      {
        if (x.RangeID < y.RangeID)
          return -1;
        return x.RangeID <= y.RangeID ? 0 : 1;
      }
    }
  }
}
