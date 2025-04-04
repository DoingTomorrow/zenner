// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.Map.MapProxy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Proxy.Map
{
  [Serializable]
  public class MapProxy : INHibernateProxy, IDictionary, ICollection, IEnumerable
  {
    private readonly MapLazyInitializer li;

    internal MapProxy(MapLazyInitializer li) => this.li = li;

    public ILazyInitializer HibernateLazyInitializer => (ILazyInitializer) this.li;

    public bool Contains(object key) => this.li.Map.Contains(key);

    public void Add(object key, object value) => this.li.Map.Add(key, value);

    public void Clear() => this.li.Map.Clear();

    IDictionaryEnumerator IDictionary.GetEnumerator() => this.li.Map.GetEnumerator();

    public void Remove(object key) => this.li.Map.Remove(key);

    public object this[object key]
    {
      get => this.li.Map[key];
      set => this.li.Map[key] = value;
    }

    public ICollection Keys => this.li.Map.Keys;

    public ICollection Values => this.li.Map.Values;

    public bool IsReadOnly => this.li.Map.IsReadOnly;

    public bool IsFixedSize => this.li.Map.IsFixedSize;

    public void CopyTo(Array array, int index)
    {
      object[] objArray1 = new object[this.Count];
      object[] objArray2 = new object[this.Count];
      if (this.Keys != null)
        this.Keys.CopyTo((Array) objArray1, index);
      if (this.Values != null)
        this.Values.CopyTo((Array) objArray2, index);
      for (int index1 = index; index1 < this.Count; ++index1)
      {
        if (objArray1[index1] != null || objArray2[index1] != null)
          array.SetValue((object) new DictionaryEntry(objArray1[index1], objArray2[index1]), index1);
      }
    }

    public int Count => this.li.Map.Count;

    public object SyncRoot => this.li.Map.SyncRoot;

    public bool IsSynchronized => this.li.Map.IsSynchronized;

    public IEnumerator GetEnumerator() => (IEnumerator) this.li.Map.GetEnumerator();
  }
}
