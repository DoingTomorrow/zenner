// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.ICollectionTypeFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Bytecode
{
  public interface ICollectionTypeFactory
  {
    CollectionType Array(string role, string propertyRef, bool embedded, System.Type elementClass);

    CollectionType Bag(string role, string propertyRef, bool embedded);

    CollectionType Bag<T>(string role, string propertyRef, bool embedded);

    CollectionType List(string role, string propertyRef, bool embedded);

    CollectionType List<T>(string role, string propertyRef, bool embedded);

    CollectionType IdBag(string role, string propertyRef, bool embedded);

    CollectionType IdBag<T>(string role, string propertyRef, bool embedded);

    CollectionType Set(string role, string propertyRef, bool embedded);

    CollectionType OrderedSet(string role, string propertyRef, bool embedded);

    CollectionType SortedSet(string role, string propertyRef, bool embedded, IComparer comparer);

    CollectionType Set<T>(string role, string propertyRef, bool embedded);

    CollectionType SortedSet<T>(
      string role,
      string propertyRef,
      bool embedded,
      IComparer<T> comparer);

    CollectionType OrderedSet<T>(string role, string propertyRef, bool embedded);

    CollectionType Map(string role, string propertyRef, bool embedded);

    CollectionType OrderedMap(string role, string propertyRef, bool embedded);

    CollectionType SortedMap(string role, string propertyRef, bool embedded, IComparer comparer);

    CollectionType Map<TKey, TValue>(string role, string propertyRef, bool embedded);

    CollectionType SortedDictionary<TKey, TValue>(
      string role,
      string propertyRef,
      bool embedded,
      IComparer<TKey> comparer);

    CollectionType SortedList<TKey, TValue>(
      string role,
      string propertyRef,
      bool embedded,
      IComparer<TKey> comparer);
  }
}
