// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Collections.HashList
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Collections
{
  internal sealed class HashList : ICollection, IDictionary, IEnumerable
  {
    private Hashtable _dictionary = new Hashtable();
    private ArrayList _insertionOrderList = new ArrayList();
    private int _version;

    public HashList()
      : this(-1)
    {
    }

    public HashList(int capacity)
    {
      if (capacity < 0)
      {
        this._dictionary = new Hashtable();
        this._insertionOrderList = new ArrayList();
      }
      else
      {
        this._dictionary = new Hashtable(capacity);
        this._insertionOrderList = new ArrayList(capacity);
      }
      this._version = 0;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new HashList.HashListEnumerator(this, HashList.HashListEnumerator.EnumerationMode.Entry);
    }

    public bool IsReadOnly => this._dictionary.IsReadOnly;

    public IDictionaryEnumerator GetEnumerator()
    {
      return (IDictionaryEnumerator) new HashList.HashListEnumerator(this, HashList.HashListEnumerator.EnumerationMode.Entry);
    }

    public object this[object key]
    {
      get => this._dictionary[key];
      set
      {
        bool flag = !this._dictionary.Contains(key);
        this._dictionary[key] = value;
        if (flag)
          this._insertionOrderList.Add(key);
        ++this._version;
      }
    }

    public void Remove(object key)
    {
      this._dictionary.Remove(key);
      this._insertionOrderList.Remove(key);
      ++this._version;
    }

    public bool Contains(object key) => this._dictionary.Contains(key);

    public void Clear()
    {
      this._dictionary.Clear();
      this._insertionOrderList.Clear();
      ++this._version;
    }

    public ICollection Values => (ICollection) new HashList.ValueCollection(this);

    public void Add(object key, object value)
    {
      this._dictionary.Add(key, value);
      this._insertionOrderList.Add(key);
      ++this._version;
    }

    public ICollection Keys => (ICollection) new HashList.KeyCollection(this);

    public bool IsFixedSize => this._dictionary.IsFixedSize;

    public bool IsSynchronized => this._dictionary.IsSynchronized;

    public int Count => this._dictionary.Count;

    public void CopyTo(Array array, int index)
    {
      int count = this._insertionOrderList.Count;
      for (int index1 = 0; index1 < count; ++index1)
      {
        DictionaryEntry dictionaryEntry = new DictionaryEntry(this._insertionOrderList[index1], this._dictionary[this._insertionOrderList[index1]]);
        array.SetValue((object) dictionaryEntry, index++);
      }
    }

    public object SyncRoot => this._dictionary.SyncRoot;

    private void CopyKeysTo(Array array, int index)
    {
      int count = this._insertionOrderList.Count;
      for (int index1 = 0; index1 < count; ++index1)
        array.SetValue(this._insertionOrderList[index1], index++);
    }

    private void CopyValuesTo(Array array, int index)
    {
      int count = this._insertionOrderList.Count;
      for (int index1 = 0; index1 < count; ++index1)
        array.SetValue(this._dictionary[this._insertionOrderList[index1]], index++);
    }

    private sealed class HashListEnumerator : IDictionaryEnumerator, IEnumerator
    {
      private HashList _hashList;
      private ArrayList _orderList;
      private HashList.HashListEnumerator.EnumerationMode _mode;
      private int _index;
      private int _version;
      private object _key;
      private object _value;

      internal HashListEnumerator()
      {
        this._index = 0;
        this._key = (object) null;
        this._value = (object) null;
      }

      internal HashListEnumerator(
        HashList hashList,
        HashList.HashListEnumerator.EnumerationMode mode)
      {
        this._hashList = hashList;
        this._mode = mode;
        this._version = hashList._version;
        this._orderList = hashList._insertionOrderList;
        this._index = 0;
        this._key = (object) null;
        this._value = (object) null;
      }

      public object Key
      {
        get
        {
          return this._key != null ? this._key : throw new InvalidOperationException("Enumeration has either not started or has already finished.");
        }
      }

      public object Value
      {
        get
        {
          if (this._key == null)
            throw new InvalidOperationException("Enumeration has either not started or has already finished.");
          return this._value;
        }
      }

      public DictionaryEntry Entry
      {
        get
        {
          return this._key != null ? new DictionaryEntry(this._key, this._value) : throw new InvalidOperationException("Enumeration has either not started or has already finished.");
        }
      }

      public void Reset()
      {
        if (this._version != this._hashList._version)
          throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
        this._index = 0;
        this._key = (object) null;
        this._value = (object) null;
      }

      public object Current
      {
        get
        {
          if (this._key == null)
            throw new InvalidOperationException("Enumeration has either not started or has already finished.");
          if (this._mode == HashList.HashListEnumerator.EnumerationMode.Key)
            return this._key;
          return this._mode == HashList.HashListEnumerator.EnumerationMode.Value ? this._value : (object) new DictionaryEntry(this._key, this._value);
        }
      }

      public bool MoveNext()
      {
        if (this._version != this._hashList._version)
          throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
        if (this._index < this._orderList.Count)
        {
          this._key = this._orderList[this._index];
          this._value = this._hashList[this._key];
          ++this._index;
          return true;
        }
        this._key = (object) null;
        return false;
      }

      internal enum EnumerationMode
      {
        Key,
        Value,
        Entry,
      }
    }

    private sealed class KeyCollection : ICollection, IEnumerable
    {
      private HashList _hashList;

      internal KeyCollection()
      {
      }

      internal KeyCollection(HashList hashList) => this._hashList = hashList;

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        ArrayList insertionOrderList = this._hashList._insertionOrderList;
        for (int index = 0; index < insertionOrderList.Count; ++index)
        {
          if (index > 0)
            stringBuilder.Append(", ");
          stringBuilder.Append(insertionOrderList[index]);
        }
        stringBuilder.Append("]");
        return stringBuilder.ToString();
      }

      public override bool Equals(object o)
      {
        if (o is HashList.KeyCollection)
        {
          HashList.KeyCollection keyCollection = (HashList.KeyCollection) o;
          if (this.Count == 0 && keyCollection.Count == 0)
            return true;
          if (this.Count == keyCollection.Count)
          {
            for (int index = 0; index < this.Count; ++index)
            {
              if (this._hashList._insertionOrderList[index] == keyCollection._hashList._insertionOrderList[index] || this._hashList._insertionOrderList[index].Equals(keyCollection._hashList._insertionOrderList[index]))
                return true;
            }
          }
        }
        return false;
      }

      public override int GetHashCode() => this._hashList._insertionOrderList.GetHashCode();

      public bool IsSynchronized => this._hashList.IsSynchronized;

      public int Count => this._hashList.Count;

      public void CopyTo(Array array, int index) => this._hashList.CopyKeysTo(array, index);

      public object SyncRoot => this._hashList.SyncRoot;

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new HashList.HashListEnumerator(this._hashList, HashList.HashListEnumerator.EnumerationMode.Key);
      }
    }

    private sealed class ValueCollection : ICollection, IEnumerable
    {
      private HashList _hashList;

      internal ValueCollection()
      {
      }

      internal ValueCollection(HashList hashList) => this._hashList = hashList;

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        IEnumerator enumerator = (IEnumerator) new HashList.HashListEnumerator(this._hashList, HashList.HashListEnumerator.EnumerationMode.Value);
        if (enumerator.MoveNext())
        {
          stringBuilder.Append(enumerator.Current != null ? enumerator.Current : (object) "null");
          while (enumerator.MoveNext())
          {
            stringBuilder.Append(", ");
            stringBuilder.Append(enumerator.Current != null ? enumerator.Current : (object) "null");
          }
        }
        stringBuilder.Append("]");
        return stringBuilder.ToString();
      }

      public bool IsSynchronized => this._hashList.IsSynchronized;

      public int Count => this._hashList.Count;

      public void CopyTo(Array array, int index) => this._hashList.CopyValuesTo(array, index);

      public object SyncRoot => this._hashList.SyncRoot;

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new HashList.HashListEnumerator(this._hashList, HashList.HashListEnumerator.EnumerationMode.Value);
      }
    }
  }
}
