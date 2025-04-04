// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.CollectionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public sealed class CollectionHelper
  {
    public static readonly IEnumerable EmptyEnumerable = (IEnumerable) new CollectionHelper.EmptyEnumerableClass();
    public static readonly IDictionary EmptyMap = (IDictionary) new CollectionHelper.EmptyMapClass();
    public static readonly ICollection EmptyCollection = (ICollection) CollectionHelper.EmptyMap;
    public static readonly IList EmptyList = (IList) new CollectionHelper.EmptyListClass();

    public static bool CollectionEquals(ICollection c1, ICollection c2)
    {
      if (c1 == c2)
        return true;
      if (c1 == null || c2 == null || c1.Count != c2.Count)
        return false;
      IEnumerator enumerator1 = c1.GetEnumerator();
      IEnumerator enumerator2 = c2.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        enumerator2.MoveNext();
        if (!object.Equals(enumerator1.Current, enumerator2.Current))
          return false;
      }
      return true;
    }

    public static bool DictionaryEquals(IDictionary a, IDictionary b)
    {
      if (object.Equals((object) a, (object) b))
        return true;
      if (a == null || b == null || a.Count != b.Count)
        return false;
      foreach (object key in (IEnumerable) a.Keys)
      {
        if (!object.Equals(a[key], b[key]))
          return false;
      }
      return true;
    }

    public static bool DictionaryEquals<K, V>(IDictionary<K, V> a, IDictionary<K, V> b)
    {
      if (object.Equals((object) a, (object) b))
        return true;
      if (a == null || b == null || a.Count != b.Count)
        return false;
      foreach (K key in (IEnumerable<K>) a.Keys)
      {
        if (!object.Equals((object) a[key], (object) b[key]))
          return false;
      }
      return true;
    }

    public static bool SetEquals(ISet a, ISet b)
    {
      if (object.Equals((object) a, (object) b))
        return true;
      if (a == null || b == null || a.Count != b.Count)
        return false;
      foreach (object o in (IEnumerable) a)
      {
        if (!b.Contains(o))
          return false;
      }
      return true;
    }

    public static int GetHashCode(IEnumerable coll)
    {
      int hashCode = 0;
      foreach (object obj in coll)
      {
        if (obj != null)
          hashCode += obj.GetHashCode();
      }
      return hashCode;
    }

    public static IDictionary<string, T> CreateCaseInsensitiveHashtable<T>()
    {
      return (IDictionary<string, T>) new Dictionary<string, T>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    }

    public static IDictionary<string, T> CreateCaseInsensitiveHashtable<T>(
      IDictionary<string, T> dictionary)
    {
      return (IDictionary<string, T>) new Dictionary<string, T>(dictionary, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    }

    private CollectionHelper()
    {
    }

    public static int GetHashCode<T>(IEnumerable<T> coll)
    {
      int hashCode = 0;
      foreach (T obj in coll)
      {
        if (!obj.Equals((object) default (T)))
          hashCode += obj.GetHashCode();
      }
      return hashCode;
    }

    public static bool SetEquals<T>(ISet<T> a, ISet<T> b)
    {
      if (object.Equals((object) a, (object) b))
        return true;
      if (a == null || b == null || a.Count != b.Count)
        return false;
      foreach (T obj in (IEnumerable<T>) a)
      {
        if (!b.Contains(obj))
          return false;
      }
      return true;
    }

    public static bool CollectionEquals<T>(ICollection<T> c1, ICollection<T> c2)
    {
      if (c1 == c2)
        return true;
      if (c1 == null || c2 == null || c1.Count != c2.Count)
        return false;
      IEnumerator enumerator1 = (IEnumerator) c1.GetEnumerator();
      IEnumerator enumerator2 = (IEnumerator) c2.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        enumerator2.MoveNext();
        if (!object.Equals(enumerator1.Current, enumerator2.Current))
          return false;
      }
      return true;
    }

    [Serializable]
    private class EmptyEnumerator : IDictionaryEnumerator, IEnumerator
    {
      public object Key => throw new InvalidOperationException("EmptyEnumerator.get_Key");

      public object Value => throw new InvalidOperationException("EmptyEnumerator.get_Value");

      public DictionaryEntry Entry
      {
        get => throw new InvalidOperationException("EmptyEnumerator.get_Entry");
      }

      public void Reset()
      {
      }

      public object Current => throw new InvalidOperationException("EmptyEnumerator.get_Current");

      public bool MoveNext() => false;
    }

    private class EmptyEnumerableClass : IEnumerable
    {
      public IEnumerator GetEnumerator() => (IEnumerator) new CollectionHelper.EmptyEnumerator();
    }

    [Serializable]
    private class EmptyMapClass : IDictionary, ICollection, IEnumerable
    {
      private static readonly CollectionHelper.EmptyEnumerator emptyEnumerator = new CollectionHelper.EmptyEnumerator();

      public bool Contains(object key) => false;

      public void Add(object key, object value) => throw new NotSupportedException("EmptyMap.Add");

      public void Clear() => throw new NotSupportedException("EmptyMap.Clear");

      IDictionaryEnumerator IDictionary.GetEnumerator()
      {
        return (IDictionaryEnumerator) CollectionHelper.EmptyMapClass.emptyEnumerator;
      }

      public void Remove(object key) => throw new NotSupportedException("EmptyMap.Remove");

      public object this[object key]
      {
        get => (object) null;
        set => throw new NotSupportedException("EmptyMap.set_Item");
      }

      public ICollection Keys => (ICollection) this;

      public ICollection Values => (ICollection) this;

      public bool IsReadOnly => true;

      public bool IsFixedSize => true;

      public void CopyTo(Array array, int index)
      {
      }

      public int Count => 0;

      public object SyncRoot => (object) this;

      public bool IsSynchronized => false;

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) CollectionHelper.EmptyMapClass.emptyEnumerator;
      }
    }

    [Serializable]
    private class EmptyListClass : IList, ICollection, IEnumerable
    {
      public int Add(object value) => throw new NotImplementedException();

      public bool Contains(object value) => false;

      public void Clear() => throw new NotImplementedException();

      public int IndexOf(object value) => -1;

      public void Insert(int index, object value) => throw new NotImplementedException();

      public void Remove(object value) => throw new NotImplementedException();

      public void RemoveAt(int index) => throw new NotImplementedException();

      public object this[int index]
      {
        get => throw new IndexOutOfRangeException();
        set => throw new IndexOutOfRangeException();
      }

      public bool IsReadOnly => true;

      public bool IsFixedSize => true;

      public void CopyTo(Array array, int index)
      {
      }

      public int Count => 0;

      public object SyncRoot => (object) this;

      public bool IsSynchronized => false;

      public IEnumerator GetEnumerator() => (IEnumerator) new CollectionHelper.EmptyEnumerator();
    }

    [Serializable]
    private class EmptyEnumerator<TKey, TValue> : 
      IEnumerator<KeyValuePair<TKey, TValue>>,
      IDisposable,
      IEnumerator
    {
      KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current
      {
        get
        {
          throw new InvalidOperationException(string.Format("EmptyEnumerator<{0},{1}>.KeyValuePair", (object) typeof (TKey), (object) typeof (TValue)));
        }
      }

      public void Dispose()
      {
      }

      public bool MoveNext() => false;

      public void Reset()
      {
      }

      public object Current
      {
        get
        {
          throw new InvalidOperationException(string.Format("EmptyEnumerator<{0},{1}>.Current", (object) typeof (TKey), (object) typeof (TValue)));
        }
      }
    }

    [Serializable]
    public class EmptyEnumerableClass<T> : IEnumerable<T>, IEnumerable
    {
      IEnumerator<T> IEnumerable<T>.GetEnumerator()
      {
        return (IEnumerator<T>) new CollectionHelper.EmptyEnumerator<T>();
      }

      public IEnumerator GetEnumerator() => (IEnumerator) ((IEnumerable<T>) this).GetEnumerator();
    }

    [Serializable]
    private class EmptyEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
      T IEnumerator<T>.Current
      {
        get => throw new InvalidOperationException("EmptyEnumerator.get_Current");
      }

      public void Dispose()
      {
      }

      public bool MoveNext() => false;

      public void Reset()
      {
      }

      public object Current => throw new InvalidOperationException("EmptyEnumerator.get_Current");
    }

    [Serializable]
    public class EmptyMapClass<TKey, TValue> : 
      IDictionary<TKey, TValue>,
      ICollection<KeyValuePair<TKey, TValue>>,
      IEnumerable<KeyValuePair<TKey, TValue>>,
      IEnumerable
    {
      private static readonly CollectionHelper.EmptyEnumerator<TKey, TValue> emptyEnumerator = new CollectionHelper.EmptyEnumerator<TKey, TValue>();

      public bool ContainsKey(TKey key) => false;

      public void Add(TKey key, TValue value)
      {
        throw new NotSupportedException(string.Format("EmptyMapClass<{0},{1}>.Add", (object) typeof (TKey), (object) typeof (TValue)));
      }

      public bool Remove(TKey key)
      {
        throw new NotSupportedException(string.Format("EmptyMapClass<{0},{1}>.Remove", (object) typeof (TKey), (object) typeof (TValue)));
      }

      public bool TryGetValue(TKey key, out TValue value)
      {
        value = default (TValue);
        return false;
      }

      public TValue this[TKey key]
      {
        get => default (TValue);
        set
        {
          throw new NotSupportedException(string.Format("EmptyMapClass<{0},{1}>.set_Item", (object) typeof (TKey), (object) typeof (TValue)));
        }
      }

      public ICollection<TKey> Keys => (ICollection<TKey>) new List<TKey>();

      public ICollection<TValue> Values => (ICollection<TValue>) new List<TValue>();

      public void Add(KeyValuePair<TKey, TValue> item)
      {
        throw new NotSupportedException(string.Format("EmptyMapClass<{0},{1}>.Add", (object) typeof (TKey), (object) typeof (TValue)));
      }

      public void Clear()
      {
        throw new NotSupportedException(string.Format("EmptyMapClass<{0},{1}>.Clear", (object) typeof (TKey), (object) typeof (TValue)));
      }

      public bool Contains(KeyValuePair<TKey, TValue> item) => false;

      public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
      {
      }

      public bool Remove(KeyValuePair<TKey, TValue> item)
      {
        throw new NotSupportedException(string.Format("EmptyMapClass<{0},{1}>.Remove", (object) typeof (TKey), (object) typeof (TValue)));
      }

      public int Count => 0;

      public bool IsReadOnly => true;

      IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
      {
        return (IEnumerator<KeyValuePair<TKey, TValue>>) CollectionHelper.EmptyMapClass<TKey, TValue>.emptyEnumerator;
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) ((IEnumerable<KeyValuePair<TKey, TValue>>) this).GetEnumerator();
      }
    }
  }
}
