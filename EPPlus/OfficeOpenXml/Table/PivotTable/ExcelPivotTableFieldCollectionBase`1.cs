// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableFieldCollectionBase`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableFieldCollectionBase<T> : IEnumerable<T>, IEnumerable
  {
    protected ExcelPivotTable _table;
    internal List<T> _list = new List<T>();

    internal ExcelPivotTableFieldCollectionBase(ExcelPivotTable table) => this._table = table;

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

    public int Count => this._list.Count;

    internal void AddInternal(T field) => this._list.Add(field);

    internal void Clear() => this._list.Clear();

    public T this[int Index]
    {
      get
      {
        if (Index < 0 || Index >= this._list.Count)
          throw new ArgumentOutOfRangeException("Index out of range");
        return this._list[Index];
      }
    }
  }
}
