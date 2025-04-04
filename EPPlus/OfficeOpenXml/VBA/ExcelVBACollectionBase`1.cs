// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVBACollectionBase`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVBACollectionBase<T> : IEnumerable<T>, IEnumerable
  {
    protected internal List<T> _list = new List<T>();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

    public T this[string Name]
    {
      get
      {
        return this._list.Find((Predicate<T>) (f => f.GetType().GetProperty(nameof (Name)).GetValue((object) f, (object[]) null).ToString().ToLower() == Name.ToLower()));
      }
    }

    public T this[int Index] => this._list[Index];

    public int Count => this._list.Count;

    public bool Exists(string Name)
    {
      return this._list.Exists((Predicate<T>) (f => f.GetType().GetProperty(nameof (Name)).GetValue((object) f, (object[]) null).ToString().ToLower() == Name.ToLower()));
    }

    public void Remove(T Item) => this._list.Remove(Item);

    public void RemoveAt(int index) => this._list.RemoveAt(index);

    internal void Clear() => this._list.Clear();
  }
}
