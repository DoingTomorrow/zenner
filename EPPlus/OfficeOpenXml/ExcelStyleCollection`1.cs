// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelStyleCollection`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelStyleCollection<T> : IEnumerable<T>, IEnumerable
  {
    private bool _setNextIdManual;
    internal List<T> _list = new List<T>();
    private Dictionary<string, int> _dic = new Dictionary<string, int>();
    internal int NextId;

    public ExcelStyleCollection() => this._setNextIdManual = false;

    public ExcelStyleCollection(bool SetNextIdManual) => this._setNextIdManual = SetNextIdManual;

    public XmlNode TopNode { get; set; }

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

    public T this[int PositionID] => this._list[PositionID];

    public int Count => this._list.Count;

    internal int Add(string key, T item)
    {
      this._list.Add(item);
      if (!this._dic.ContainsKey(key.ToLower()))
        this._dic.Add(key.ToLower(), this._list.Count - 1);
      if (this._setNextIdManual)
        ++this.NextId;
      return this._list.Count - 1;
    }

    internal bool FindByID(string key, ref T obj)
    {
      if (!this._dic.ContainsKey(key.ToLower()))
        return false;
      obj = this._list[this._dic[key.ToLower()]];
      return true;
    }

    internal int FindIndexByID(string key)
    {
      return this._dic.ContainsKey(key.ToLower()) ? this._dic[key.ToLower()] : int.MinValue;
    }

    internal bool ExistsKey(string key) => this._dic.ContainsKey(key.ToLower());

    internal void Sort(Comparison<T> c) => this._list.Sort(c);
  }
}
