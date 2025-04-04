// Decompiled with JetBrains decompiler
// Type: System.Reactive.Lookup`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Reactive
{
  internal class Lookup<K, E> : ILookup<K, E>, IEnumerable<IGrouping<K, E>>, IEnumerable
  {
    private readonly Dictionary<K, List<E>> d;

    public Lookup(IEqualityComparer<K> comparer) => this.d = new Dictionary<K, List<E>>(comparer);

    public void Add(K key, E element)
    {
      List<E> eList = (List<E>) null;
      if (!this.d.TryGetValue(key, out eList))
        this.d[key] = eList = new List<E>();
      eList.Add(element);
    }

    public bool Contains(K key) => this.d.ContainsKey(key);

    public int Count => this.d.Count;

    public IEnumerable<E> this[K key]
    {
      get
      {
        List<E> elements = (List<E>) null;
        return !this.d.TryGetValue(key, out elements) ? Enumerable.Empty<E>() : this.Hide(elements);
      }
    }

    private IEnumerable<E> Hide(List<E> elements)
    {
      foreach (E element in elements)
        yield return element;
    }

    public IEnumerator<IGrouping<K, E>> GetEnumerator()
    {
      foreach (KeyValuePair<K, List<E>> kv in this.d)
        yield return (IGrouping<K, E>) new Lookup<K, E>.Grouping(kv);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private class Grouping : IGrouping<K, E>, IEnumerable<E>, IEnumerable
    {
      private KeyValuePair<K, List<E>> kv;

      public Grouping(KeyValuePair<K, List<E>> kv) => this.kv = kv;

      public K Key => this.kv.Key;

      public IEnumerator<E> GetEnumerator() => (IEnumerator<E>) this.kv.Value.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
