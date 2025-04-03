// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.StringListWrapper`1
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  internal class StringListWrapper<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
  {
    private readonly string key;
    private readonly char separator;
    private readonly IDictionary dictionary;
    private readonly List<T> inner;

    public StringListWrapper(string key, string list, char separator, IDictionary dictionary)
    {
      this.key = key;
      this.separator = separator;
      this.dictionary = dictionary;
      this.inner = new List<T>();
      this.ParseList(list);
    }

    public int IndexOf(T item) => this.inner.IndexOf(item);

    public void Insert(int index, T item)
    {
      this.inner.Insert(index, item);
      this.SynchronizeDictionary();
    }

    public void RemoveAt(int index)
    {
      this.inner.RemoveAt(index);
      this.SynchronizeDictionary();
    }

    public T this[int index]
    {
      get => this.inner[index];
      set
      {
        this.inner[index] = value;
        this.SynchronizeDictionary();
      }
    }

    public void Add(T item)
    {
      this.inner.Add(item);
      this.SynchronizeDictionary();
    }

    public void Clear()
    {
      this.inner.Clear();
      this.SynchronizeDictionary();
    }

    public bool Contains(T item) => this.inner.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this.inner.CopyTo(array, arrayIndex);

    public int Count => this.inner.Count;

    public bool IsReadOnly => false;

    public bool Remove(T item)
    {
      if (!this.inner.Remove(item))
        return false;
      this.SynchronizeDictionary();
      return true;
    }

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.inner.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.inner.GetEnumerator();

    private void ParseList(string list)
    {
      if (list == null)
        return;
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
      string str1 = list;
      char[] chArray = new char[1]{ this.separator };
      foreach (string str2 in str1.Split(chArray))
        this.inner.Add((T) converter.ConvertFrom((object) str2));
    }

    private void SynchronizeDictionary()
    {
      this.dictionary[(object) this.key] = (object) StringListAttribute.BuildString((IEnumerable) this.inner, this.separator);
    }
  }
}
