// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.DefaultCollectionTypeFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Type
{
  public class DefaultCollectionTypeFactory : ICollectionTypeFactory
  {
    public virtual CollectionType Array(
      string role,
      string propertyRef,
      bool embedded,
      System.Type elementClass)
    {
      return (CollectionType) new ArrayType(role, propertyRef, elementClass, embedded);
    }

    public virtual CollectionType Bag(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new BagType(role, propertyRef, embedded);
    }

    public virtual CollectionType Bag<T>(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new GenericBagType<T>(role, propertyRef);
    }

    public virtual CollectionType List(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new ListType(role, propertyRef, embedded);
    }

    public virtual CollectionType List<T>(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new GenericListType<T>(role, propertyRef);
    }

    public virtual CollectionType IdBag(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new IdentifierBagType(role, propertyRef, embedded);
    }

    public virtual CollectionType IdBag<T>(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new GenericIdentifierBagType<T>(role, propertyRef);
    }

    public virtual CollectionType Set(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new SetType(role, propertyRef, embedded);
    }

    public virtual CollectionType OrderedSet(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new OrderedSetType(role, propertyRef, embedded);
    }

    public virtual CollectionType SortedSet(
      string role,
      string propertyRef,
      bool embedded,
      IComparer comparer)
    {
      return (CollectionType) new SortedSetType(role, propertyRef, comparer, embedded);
    }

    public virtual CollectionType Set<T>(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new GenericSetType<T>(role, propertyRef);
    }

    public virtual CollectionType SortedSet<T>(
      string role,
      string propertyRef,
      bool embedded,
      IComparer<T> comparer)
    {
      return (CollectionType) new GenericSortedSetType<T>(role, propertyRef, comparer);
    }

    public virtual CollectionType OrderedSet<T>(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new GenericOrderedSetType<T>(role, propertyRef);
    }

    public virtual CollectionType Map(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new MapType(role, propertyRef, embedded);
    }

    public virtual CollectionType OrderedMap(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new OrderedMapType(role, propertyRef, embedded);
    }

    public virtual CollectionType SortedMap(
      string role,
      string propertyRef,
      bool embedded,
      IComparer comparer)
    {
      return (CollectionType) new SortedMapType(role, propertyRef, comparer, embedded);
    }

    public virtual CollectionType Map<TKey, TValue>(string role, string propertyRef, bool embedded)
    {
      return (CollectionType) new GenericMapType<TKey, TValue>(role, propertyRef);
    }

    public virtual CollectionType SortedDictionary<TKey, TValue>(
      string role,
      string propertyRef,
      bool embedded,
      IComparer<TKey> comparer)
    {
      return (CollectionType) new GenericSortedDictionaryType<TKey, TValue>(role, propertyRef, comparer);
    }

    public virtual CollectionType SortedList<TKey, TValue>(
      string role,
      string propertyRef,
      bool embedded,
      IComparer<TKey> comparer)
    {
      return (CollectionType) new GenericSortedListType<TKey, TValue>(role, propertyRef, comparer);
    }
  }
}
