// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.EventCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Event.Default
{
  public class EventCache : IDictionary, ICollection, IEnumerable
  {
    private IDictionary entityToCopyMap = IdentityMap.Instantiate(10);
    private IDictionary entityToOperatedOnFlagMap = IdentityMap.Instantiate(10);

    public int Count => this.entityToCopyMap.Count;

    public bool IsSynchronized => false;

    public object SyncRoot => (object) this;

    public void CopyTo(Array array, int index)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (index < 0)
        throw new ArgumentOutOfRangeException("arrayIndex is less than 0");
      if (this.entityToCopyMap.Count + index + 1 > array.Length)
        throw new ArgumentException("The number of elements in the source ICollection<T> is greater than the available space from arrayIndex to the end of the destination array.");
      this.entityToCopyMap.CopyTo(array, index);
    }

    IEnumerator IEnumerable.GetEnumerator() => this.entityToCopyMap.GetEnumerator();

    public object this[object key]
    {
      get => this.entityToCopyMap[key];
      set => this.Add(key, value);
    }

    public bool IsReadOnly => false;

    public bool IsFixedSize => false;

    public ICollection Keys => this.entityToCopyMap.Keys;

    public ICollection Values => this.entityToCopyMap.Values;

    public void Add(object key, object value)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.entityToCopyMap.Add(key, value);
      this.entityToOperatedOnFlagMap.Add(key, (object) false);
    }

    public bool Contains(object key) => this.entityToCopyMap.Contains(key);

    public void Remove(object key)
    {
      this.entityToCopyMap.Remove(key);
      this.entityToOperatedOnFlagMap.Remove(key);
    }

    public IDictionaryEnumerator GetEnumerator() => this.entityToCopyMap.GetEnumerator();

    public void Clear()
    {
      this.entityToCopyMap.Clear();
      this.entityToOperatedOnFlagMap.Clear();
    }

    public void Add(object entity, object copy, bool isOperatedOn)
    {
      if (entity == null)
        throw new ArgumentNullException("null entities are not supported", nameof (entity));
      if (copy == null)
        throw new ArgumentNullException("null entity copies are not supported", nameof (copy));
      this.entityToCopyMap.Add(entity, copy);
      this.entityToOperatedOnFlagMap.Add(entity, (object) isOperatedOn);
    }

    public IDictionary InvertMap() => IdentityMap.Invert(this.entityToCopyMap);

    public bool IsOperatedOn(object entity)
    {
      return entity != null ? (bool) this.entityToOperatedOnFlagMap[entity] : throw new ArgumentNullException("null entities are not supported", nameof (entity));
    }

    public void SetOperatedOn(object entity, bool isOperatedOn)
    {
      if (entity == null)
        throw new ArgumentNullException("null entities are not supported", nameof (entity));
      if (!this.entityToOperatedOnFlagMap.Contains(entity) || !this.entityToCopyMap.Contains(entity))
        throw new AssertionFailure("called EventCache.SetOperatedOn() for entity not found in EventCache");
      this.entityToOperatedOnFlagMap[entity] = (object) isOperatedOn;
    }
  }
}
