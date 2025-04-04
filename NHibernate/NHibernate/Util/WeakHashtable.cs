// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.WeakHashtable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Util
{
  [Serializable]
  public class WeakHashtable : IDictionary, ICollection, IEnumerable
  {
    private Hashtable innerHashtable = new Hashtable();

    public void Scavenge()
    {
      ArrayList arrayList = new ArrayList();
      foreach (DictionaryEntry dictionaryEntry in this.innerHashtable)
      {
        WeakRefWrapper key = (WeakRefWrapper) dictionaryEntry.Key;
        WeakRefWrapper weakRefWrapper = (WeakRefWrapper) dictionaryEntry.Value;
        if (!key.IsAlive || !weakRefWrapper.IsAlive)
          arrayList.Add((object) key);
      }
      foreach (object key in arrayList)
        this.innerHashtable.Remove(key);
    }

    public bool Contains(object key)
    {
      return this.innerHashtable.Contains((object) WeakRefWrapper.Wrap(key));
    }

    public void Add(object key, object value)
    {
      this.Scavenge();
      this.innerHashtable.Add((object) WeakRefWrapper.Wrap(key), (object) WeakRefWrapper.Wrap(value));
    }

    public void Clear() => this.innerHashtable.Clear();

    public IDictionaryEnumerator GetEnumerator()
    {
      return (IDictionaryEnumerator) new WeakEnumerator(this.innerHashtable.GetEnumerator());
    }

    public void Remove(object key) => this.innerHashtable.Remove((object) WeakRefWrapper.Wrap(key));

    public object this[object key]
    {
      get => WeakRefWrapper.Unwrap(this.innerHashtable[(object) WeakRefWrapper.Wrap(key)]);
      set
      {
        this.Scavenge();
        this.innerHashtable[(object) WeakRefWrapper.Wrap(key)] = (object) WeakRefWrapper.Wrap(value);
      }
    }

    public ICollection Keys => throw new NotImplementedException();

    public ICollection Values => throw new NotImplementedException();

    public bool IsReadOnly => this.innerHashtable.IsReadOnly;

    public bool IsFixedSize => this.innerHashtable.IsFixedSize;

    public void CopyTo(Array array, int index) => throw new NotImplementedException();

    public int Count => this.innerHashtable.Count;

    public object SyncRoot => this.innerHashtable.SyncRoot;

    public bool IsSynchronized => this.innerHashtable.IsSynchronized;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
