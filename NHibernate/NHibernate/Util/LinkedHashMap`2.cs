// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.LinkedHashMap`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.DebugHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace NHibernate.Util
{
  [DebuggerTypeProxy(typeof (CollectionProxy<>))]
  [Serializable]
  public class LinkedHashMap<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    private readonly LinkedHashMap<TKey, TValue>.Entry header;
    private readonly Dictionary<TKey, LinkedHashMap<TKey, TValue>.Entry> entries;
    private long version;

    public LinkedHashMap()
      : this(0, (IEqualityComparer<TKey>) null)
    {
    }

    public LinkedHashMap(int capacity)
      : this(capacity, (IEqualityComparer<TKey>) null)
    {
    }

    public LinkedHashMap(IEqualityComparer<TKey> equalityComparer)
      : this(0, equalityComparer)
    {
    }

    public LinkedHashMap(int capacity, IEqualityComparer<TKey> equalityComparer)
    {
      this.header = LinkedHashMap<TKey, TValue>.CreateSentinel();
      this.entries = new Dictionary<TKey, LinkedHashMap<TKey, TValue>.Entry>(capacity, equalityComparer);
    }

    public virtual bool ContainsKey(TKey key) => this.entries.ContainsKey(key);

    public virtual void Add(TKey key, TValue value)
    {
      LinkedHashMap<TKey, TValue>.Entry entry = new LinkedHashMap<TKey, TValue>.Entry(key, value);
      this.entries.Add(key, entry);
      ++this.version;
      this.InsertEntry(entry);
    }

    public virtual bool Remove(TKey key) => this.RemoveImpl(key);

    public bool TryGetValue(TKey key, out TValue value)
    {
      LinkedHashMap<TKey, TValue>.Entry entry;
      bool flag = this.entries.TryGetValue(key, out entry);
      value = !flag ? default (TValue) : entry.Value;
      return flag;
    }

    public TValue this[TKey key]
    {
      get => this.entries[key].Value;
      set
      {
        LinkedHashMap<TKey, TValue>.Entry e;
        if (this.entries.TryGetValue(key, out e))
          this.OverrideEntry(e, value);
        else
          this.Add(key, value);
      }
    }

    private void OverrideEntry(LinkedHashMap<TKey, TValue>.Entry e, TValue value)
    {
      ++this.version;
      LinkedHashMap<TKey, TValue>.RemoveEntry(e);
      e.Value = value;
      this.InsertEntry(e);
    }

    public virtual ICollection<TKey> Keys
    {
      get => (ICollection<TKey>) new LinkedHashMap<TKey, TValue>.KeyCollection(this);
    }

    public virtual ICollection<TValue> Values
    {
      get => (ICollection<TValue>) new LinkedHashMap<TKey, TValue>.ValuesCollection(this);
    }

    public void Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

    public virtual void Clear()
    {
      ++this.version;
      this.entries.Clear();
      this.header.Next = this.header;
      this.header.Prev = this.header;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => this.Contains(item.Key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
        array.SetValue((object) keyValuePair, arrayIndex++);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) => this.Remove(item.Key);

    public virtual int Count => this.entries.Count;

    public virtual bool IsReadOnly => false;

    public virtual IEnumerator GetEnumerator()
    {
      return (IEnumerator) new LinkedHashMap<TKey, TValue>.Enumerator(this);
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<TKey, TValue>>) new LinkedHashMap<TKey, TValue>.Enumerator(this);
    }

    private bool IsEmpty => this.header.Next == this.header;

    public virtual bool IsFixedSize => false;

    public virtual TKey FirstKey => this.First != null ? this.First.Key : default (TKey);

    public virtual TValue FirstValue => this.First != null ? this.First.Value : default (TValue);

    public virtual TKey LastKey => this.Last != null ? this.Last.Key : default (TKey);

    public virtual TValue LastValue => this.Last != null ? this.Last.Value : default (TValue);

    public virtual bool Contains(TKey key) => this.ContainsKey(key);

    public virtual bool ContainsValue(TValue value)
    {
      if ((object) value == null)
      {
        for (LinkedHashMap<TKey, TValue>.Entry next = this.header.Next; next != this.header; next = next.Next)
        {
          if ((object) next.Value == null)
            return true;
        }
      }
      else
      {
        for (LinkedHashMap<TKey, TValue>.Entry next = this.header.Next; next != this.header; next = next.Next)
        {
          if (value.Equals((object) next.Value))
            return true;
        }
      }
      return false;
    }

    private static LinkedHashMap<TKey, TValue>.Entry CreateSentinel()
    {
      LinkedHashMap<TKey, TValue>.Entry sentinel = new LinkedHashMap<TKey, TValue>.Entry(default (TKey), default (TValue));
      sentinel.Prev = sentinel;
      sentinel.Next = sentinel;
      return sentinel;
    }

    private static void RemoveEntry(LinkedHashMap<TKey, TValue>.Entry entry)
    {
      entry.Next.Prev = entry.Prev;
      entry.Prev.Next = entry.Next;
    }

    private void InsertEntry(LinkedHashMap<TKey, TValue>.Entry entry)
    {
      entry.Next = this.header;
      entry.Prev = this.header.Prev;
      this.header.Prev.Next = entry;
      this.header.Prev = entry;
    }

    private LinkedHashMap<TKey, TValue>.Entry First
    {
      get => !this.IsEmpty ? this.header.Next : (LinkedHashMap<TKey, TValue>.Entry) null;
    }

    private LinkedHashMap<TKey, TValue>.Entry Last
    {
      get => !this.IsEmpty ? this.header.Prev : (LinkedHashMap<TKey, TValue>.Entry) null;
    }

    private bool RemoveImpl(TKey key)
    {
      bool flag = false;
      LinkedHashMap<TKey, TValue>.Entry entry;
      if (this.entries.TryGetValue(key, out entry))
      {
        flag = this.entries.Remove(key);
        ++this.version;
        LinkedHashMap<TKey, TValue>.RemoveEntry(entry);
      }
      return flag;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      for (LinkedHashMap<TKey, TValue>.Entry next = this.header.Next; next != this.header; next = next.Next)
      {
        stringBuilder.Append((object) next.Key);
        stringBuilder.Append('=');
        stringBuilder.Append((object) next.Value);
        if (next.Next != this.header)
          stringBuilder.Append(',');
      }
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    [Serializable]
    protected class Entry
    {
      private readonly TKey key;
      private TValue evalue;
      private LinkedHashMap<TKey, TValue>.Entry next;
      private LinkedHashMap<TKey, TValue>.Entry prev;

      public Entry(TKey key, TValue value)
      {
        this.key = key;
        this.evalue = value;
      }

      public TKey Key => this.key;

      public TValue Value
      {
        get => this.evalue;
        set => this.evalue = value;
      }

      public LinkedHashMap<TKey, TValue>.Entry Next
      {
        get => this.next;
        set => this.next = value;
      }

      public LinkedHashMap<TKey, TValue>.Entry Prev
      {
        get => this.prev;
        set => this.prev = value;
      }

      public override int GetHashCode()
      {
        return ((object) this.key == null ? 0 : this.key.GetHashCode()) ^ ((object) this.evalue == null ? 0 : this.evalue.GetHashCode());
      }

      public override bool Equals(object obj)
      {
        if (!(obj is LinkedHashMap<TKey, TValue>.Entry entry))
          return false;
        if (entry == this)
          return true;
        if (((object) this.key == null ? ((object) entry.Key == null ? 1 : 0) : (this.key.Equals((object) entry.Key) ? 1 : 0)) == 0)
          return false;
        return (object) this.evalue != null ? this.evalue.Equals((object) entry.Value) : (object) entry.Value == null;
      }

      public override string ToString()
      {
        return "[" + (object) this.key + "=" + (object) this.evalue + "]";
      }
    }

    private class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
    {
      private readonly LinkedHashMap<TKey, TValue> dictionary;

      public KeyCollection(LinkedHashMap<TKey, TValue> dictionary) => this.dictionary = dictionary;

      void ICollection<TKey>.Add(TKey item)
      {
        throw new NotSupportedException("LinkedHashMap KeyCollection is readonly.");
      }

      void ICollection<TKey>.Clear()
      {
        throw new NotSupportedException("LinkedHashMap KeyCollection is readonly.");
      }

      bool ICollection<TKey>.Contains(TKey item)
      {
        foreach (TKey key in (IEnumerable<TKey>) this)
        {
          if (key.Equals((object) item))
            return true;
        }
        return false;
      }

      public void CopyTo(TKey[] array, int arrayIndex)
      {
        foreach (TKey key in (IEnumerable<TKey>) this)
          array.SetValue((object) key, arrayIndex++);
      }

      bool ICollection<TKey>.Remove(TKey item)
      {
        throw new NotSupportedException("LinkedHashMap KeyCollection is readonly.");
      }

      public int Count => this.dictionary.Count;

      bool ICollection<TKey>.IsReadOnly => true;

      IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
      {
        return (IEnumerator<TKey>) new LinkedHashMap<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) ((IEnumerable<TKey>) this).GetEnumerator();
      }

      private class Enumerator(LinkedHashMap<TKey, TValue> dictionary) : 
        LinkedHashMap<TKey, TValue>.ForwardEnumerator<TKey>(dictionary)
      {
        public override TKey Current
        {
          get
          {
            if (this.dictionary.version != this.version)
              throw new InvalidOperationException("Enumerator was modified");
            return this.current.Key;
          }
        }
      }
    }

    private abstract class ForwardEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
      protected readonly LinkedHashMap<TKey, TValue> dictionary;
      protected LinkedHashMap<TKey, TValue>.Entry current;
      protected readonly long version;

      public ForwardEnumerator(LinkedHashMap<TKey, TValue> dictionary)
      {
        this.dictionary = dictionary;
        this.version = dictionary.version;
        this.current = dictionary.header;
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        if (this.dictionary.version != this.version)
          throw new InvalidOperationException("Enumerator was modified");
        if (this.current.Next == this.dictionary.header)
          return false;
        this.current = this.current.Next;
        return true;
      }

      public void Reset() => this.current = this.dictionary.header;

      object IEnumerator.Current => (object) this.Current;

      public abstract T Current { get; }
    }

    private class ValuesCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
    {
      private readonly LinkedHashMap<TKey, TValue> dictionary;

      public ValuesCollection(LinkedHashMap<TKey, TValue> dictionary)
      {
        this.dictionary = dictionary;
      }

      void ICollection<TValue>.Add(TValue item)
      {
        throw new NotSupportedException("LinkedHashMap ValuesCollection is readonly.");
      }

      void ICollection<TValue>.Clear()
      {
        throw new NotSupportedException("LinkedHashMap ValuesCollection is readonly.");
      }

      bool ICollection<TValue>.Contains(TValue item)
      {
        foreach (TValue obj in (IEnumerable<TValue>) this)
        {
          if (obj.Equals((object) item))
            return true;
        }
        return false;
      }

      public void CopyTo(TValue[] array, int arrayIndex)
      {
        foreach (TValue obj in (IEnumerable<TValue>) this)
          array.SetValue((object) obj, arrayIndex++);
      }

      bool ICollection<TValue>.Remove(TValue item)
      {
        throw new NotSupportedException("LinkedHashMap ValuesCollection is readonly.");
      }

      public int Count => this.dictionary.Count;

      bool ICollection<TValue>.IsReadOnly => true;

      IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
      {
        return (IEnumerator<TValue>) new LinkedHashMap<TKey, TValue>.ValuesCollection.Enumerator(this.dictionary);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) ((IEnumerable<TValue>) this).GetEnumerator();
      }

      private class Enumerator(LinkedHashMap<TKey, TValue> dictionary) : 
        LinkedHashMap<TKey, TValue>.ForwardEnumerator<TValue>(dictionary)
      {
        public override TValue Current
        {
          get
          {
            if (this.dictionary.version != this.version)
              throw new InvalidOperationException("Enumerator was modified");
            return this.current.Value;
          }
        }
      }
    }

    private class Enumerator(LinkedHashMap<TKey, TValue> dictionary) : 
      LinkedHashMap<TKey, TValue>.ForwardEnumerator<KeyValuePair<TKey, TValue>>(dictionary)
    {
      public override KeyValuePair<TKey, TValue> Current
      {
        get
        {
          if (this.dictionary.version != this.version)
            throw new InvalidOperationException("Enumerator was modified");
          return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
        }
      }
    }

    protected abstract class BackwardEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
      protected readonly LinkedHashMap<TKey, TValue> dictionary;
      private LinkedHashMap<TKey, TValue>.Entry current;
      protected readonly long version;

      public BackwardEnumerator(LinkedHashMap<TKey, TValue> dictionary)
      {
        this.dictionary = dictionary;
        this.version = dictionary.version;
        this.current = dictionary.header;
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        if (this.dictionary.version != this.version)
          throw new InvalidOperationException("Enumerator was modified");
        if (this.current.Prev == this.dictionary.header)
          return false;
        this.current = this.current.Prev;
        return true;
      }

      public void Reset() => this.current = this.dictionary.header;

      object IEnumerator.Current => (object) this.Current;

      public abstract T Current { get; }
    }
  }
}
