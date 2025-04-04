// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.SequencedHashMap
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.DebugHelpers;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace NHibernate.Util
{
  [DebuggerTypeProxy(typeof (CollectionProxy<>))]
  [Serializable]
  public class SequencedHashMap : IDictionary, ICollection, IEnumerable, IDeserializationCallback
  {
    private SequencedHashMap.Entry _sentinel;
    private Hashtable _entries;
    private long _modCount;

    private static SequencedHashMap.Entry CreateSentinel()
    {
      SequencedHashMap.Entry sentinel = new SequencedHashMap.Entry((object) null, (object) null);
      sentinel.Prev = sentinel;
      sentinel.Next = sentinel;
      return sentinel;
    }

    public SequencedHashMap()
      : this(0, 1f, (IEqualityComparer) null)
    {
    }

    public SequencedHashMap(int capacity)
      : this(capacity, 1f, (IEqualityComparer) null)
    {
    }

    public SequencedHashMap(int capacity, float loadFactor)
      : this(capacity, loadFactor, (IEqualityComparer) null)
    {
    }

    public SequencedHashMap(int capacity, IEqualityComparer equalityComparer)
      : this(capacity, 1f, equalityComparer)
    {
    }

    public SequencedHashMap(IEqualityComparer equalityComparer)
      : this(0, 1f, equalityComparer)
    {
    }

    public SequencedHashMap(int capacity, float loadFactor, IEqualityComparer equalityComparer)
    {
      this._sentinel = SequencedHashMap.CreateSentinel();
      this._entries = new Hashtable(capacity, loadFactor, equalityComparer);
    }

    private void RemoveEntry(SequencedHashMap.Entry entry)
    {
      entry.Next.Prev = entry.Prev;
      entry.Prev.Next = entry.Next;
    }

    private void InsertEntry(SequencedHashMap.Entry entry)
    {
      entry.Next = this._sentinel;
      entry.Prev = this._sentinel.Prev;
      this._sentinel.Prev.Next = entry;
      this._sentinel.Prev = entry;
    }

    public virtual bool IsFixedSize => false;

    public virtual bool IsReadOnly => false;

    public virtual object this[object o]
    {
      get => ((SequencedHashMap.Entry) this._entries[o])?.Value;
      set
      {
        ++this._modCount;
        SequencedHashMap.Entry entry = (SequencedHashMap.Entry) this._entries[o];
        if (entry != null)
        {
          this.RemoveEntry(entry);
          entry.Value = value;
        }
        else
        {
          entry = new SequencedHashMap.Entry(o, value);
          this._entries[o] = (object) entry;
        }
        this.InsertEntry(entry);
      }
    }

    public virtual ICollection Keys => (ICollection) new SequencedHashMap.KeyCollection(this);

    public virtual ICollection Values => (ICollection) new SequencedHashMap.ValuesCollection(this);

    public virtual void Add(object key, object value) => this[key] = value;

    public virtual void Clear()
    {
      ++this._modCount;
      this._entries.Clear();
      this._sentinel.Next = this._sentinel;
      this._sentinel.Prev = this._sentinel;
    }

    public virtual bool Contains(object key) => this.ContainsKey(key);

    public virtual IDictionaryEnumerator GetEnumerator()
    {
      return (IDictionaryEnumerator) new SequencedHashMap.OrderedEnumerator(this, SequencedHashMap.ReturnType.ReturnEntry);
    }

    public virtual void Remove(object key) => this.RemoveImpl(key);

    public virtual int Count => this._entries.Count;

    public virtual bool IsSynchronized => false;

    public virtual object SyncRoot => (object) this;

    public virtual void CopyTo(Array array, int index)
    {
      foreach (DictionaryEntry dictionaryEntry in this)
        array.SetValue((object) dictionaryEntry, index++);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new SequencedHashMap.OrderedEnumerator(this, SequencedHashMap.ReturnType.ReturnEntry);
    }

    private bool IsEmpty => this._sentinel.Next == this._sentinel;

    public virtual bool ContainsKey(object key) => this._entries.ContainsKey(key);

    public virtual bool ContainsValue(object value)
    {
      if (value == null)
      {
        for (SequencedHashMap.Entry next = this._sentinel.Next; next != this._sentinel; next = next.Next)
        {
          if (next.Value == null)
            return true;
        }
      }
      else
      {
        for (SequencedHashMap.Entry next = this._sentinel.Next; next != this._sentinel; next = next.Next)
        {
          if (value.Equals(next.Value))
            return true;
        }
      }
      return false;
    }

    private SequencedHashMap.Entry First
    {
      get => !this.IsEmpty ? this._sentinel.Next : (SequencedHashMap.Entry) null;
    }

    public virtual object FirstKey => this.First != null ? this.First.Key : (object) null;

    public virtual object FirstValue => this.First != null ? this.First.Value : (object) null;

    private SequencedHashMap.Entry Last
    {
      get => !this.IsEmpty ? this._sentinel.Prev : (SequencedHashMap.Entry) null;
    }

    public virtual object LastKey => this.Last != null ? this.Last.Key : (object) null;

    public virtual object LastValue => this.Last != null ? this.Last.Value : (object) null;

    public void OnDeserialization(object sender) => this._entries.OnDeserialization(sender);

    private void RemoveImpl(object key)
    {
      SequencedHashMap.Entry entry = (SequencedHashMap.Entry) this._entries[key];
      if (entry == null)
        return;
      this._entries.Remove(key);
      ++this._modCount;
      this.RemoveEntry(entry);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      for (SequencedHashMap.Entry next = this._sentinel.Next; next != this._sentinel; next = next.Next)
      {
        stringBuilder.Append(next.Key);
        stringBuilder.Append('=');
        stringBuilder.Append(next.Value);
        if (next.Next != this._sentinel)
          stringBuilder.Append(',');
      }
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    [Serializable]
    private class Entry
    {
      private object _key;
      private object _value;
      private SequencedHashMap.Entry _next;
      private SequencedHashMap.Entry _prev;

      public Entry(object key, object value)
      {
        this._key = key;
        this._value = value;
      }

      public object Key => this._key;

      public object Value
      {
        get => this._value;
        set => this._value = value;
      }

      public SequencedHashMap.Entry Next
      {
        get => this._next;
        set => this._next = value;
      }

      public SequencedHashMap.Entry Prev
      {
        get => this._prev;
        set => this._prev = value;
      }

      public override int GetHashCode()
      {
        return (this._key == null ? 0 : this._key.GetHashCode()) ^ (this._value == null ? 0 : this._value.GetHashCode());
      }

      public override bool Equals(object obj)
      {
        if (!(obj is SequencedHashMap.Entry entry))
          return false;
        if (entry == this)
          return true;
        if ((this._key == null ? (entry.Key == null ? 1 : 0) : (this._key.Equals(entry.Key) ? 1 : 0)) == 0)
          return false;
        return this._value != null ? this._value.Equals(entry.Value) : entry.Value == null;
      }

      public override string ToString() => "[" + this._key + "=" + this._value + "]";
    }

    private class KeyCollection : ICollection, IEnumerable
    {
      private SequencedHashMap _parent;

      public KeyCollection(SequencedHashMap parent) => this._parent = parent;

      public int Count => this._parent.Count;

      public bool IsSynchronized => false;

      public object SyncRoot => (object) this;

      public void CopyTo(Array array, int index)
      {
        foreach (object obj in this)
          array.SetValue(obj, index++);
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new SequencedHashMap.OrderedEnumerator(this._parent, SequencedHashMap.ReturnType.ReturnKey);
      }

      public bool Contains(object o) => this._parent.ContainsKey(o);
    }

    private class ValuesCollection : ICollection, IEnumerable
    {
      private SequencedHashMap _parent;

      public ValuesCollection(SequencedHashMap parent) => this._parent = parent;

      public int Count => this._parent.Count;

      public bool IsSynchronized => false;

      public object SyncRoot => (object) this;

      public void CopyTo(Array array, int index)
      {
        foreach (object obj in this)
          array.SetValue(obj, index++);
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new SequencedHashMap.OrderedEnumerator(this._parent, SequencedHashMap.ReturnType.ReturnValue);
      }

      public bool Contains(object o) => this._parent.ContainsValue(o);
    }

    private enum ReturnType
    {
      ReturnKey,
      ReturnValue,
      ReturnEntry,
    }

    private class OrderedEnumerator : IDictionaryEnumerator, IEnumerator
    {
      private SequencedHashMap _parent;
      private SequencedHashMap.ReturnType _returnType;
      private SequencedHashMap.Entry _pos;
      private long _expectedModCount;

      public OrderedEnumerator(SequencedHashMap parent, SequencedHashMap.ReturnType returnType)
      {
        this._parent = parent;
        this._returnType = returnType;
        this._pos = this._parent._sentinel;
        this._expectedModCount = this._parent._modCount;
      }

      public object Current
      {
        get
        {
          if (this._parent._modCount != this._expectedModCount)
            throw new InvalidOperationException("Enumerator was modified");
          switch (this._returnType)
          {
            case SequencedHashMap.ReturnType.ReturnKey:
              return this._pos.Key;
            case SequencedHashMap.ReturnType.ReturnValue:
              return this._pos.Value;
            case SequencedHashMap.ReturnType.ReturnEntry:
              return (object) new DictionaryEntry(this._pos.Key, this._pos.Value);
            default:
              return (object) null;
          }
        }
      }

      public bool MoveNext()
      {
        if (this._parent._modCount != this._expectedModCount)
          throw new InvalidOperationException("Enumerator was modified");
        if (this._pos.Next == this._parent._sentinel)
          return false;
        this._pos = this._pos.Next;
        return true;
      }

      public void Reset() => this._pos = this._parent._sentinel;

      public DictionaryEntry Entry => new DictionaryEntry(this._pos.Key, this._pos.Value);

      public object Key => this._pos.Key;

      public object Value => this._pos.Value;
    }
  }
}
