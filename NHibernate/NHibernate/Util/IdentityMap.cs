// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.IdentityMap
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Util
{
  [Serializable]
  public sealed class IdentityMap : IDictionary, ICollection, IEnumerable, IDeserializationCallback
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (IdentityMap));
    private IDictionary map;

    public static IDictionary Instantiate(int size)
    {
      return (IDictionary) new IdentityMap((IDictionary) new Hashtable(size, (IEqualityComparer) new IdentityEqualityComparer()));
    }

    public static IDictionary InstantiateSequenced(int size)
    {
      return (IDictionary) new IdentityMap((IDictionary) new SequencedHashMap(size, (IEqualityComparer) new IdentityEqualityComparer()));
    }

    public static ICollection ConcurrentEntries(IDictionary map)
    {
      return (ICollection) ((IdentityMap) map).EntryList;
    }

    public static ICollection Entries(IDictionary map)
    {
      return (ICollection) ((IdentityMap) map).EntryList;
    }

    private IdentityMap(IDictionary underlyingMap) => this.map = underlyingMap;

    public int Count => this.map.Count;

    public bool IsSynchronized => this.map.IsSynchronized;

    public object SyncRoot => this.map.SyncRoot;

    public void Add(object key, object val) => this.map.Add(this.VerifyValidKey(key), val);

    public void Clear() => this.map.Clear();

    public bool Contains(object key) => key != null && this.map.Contains(this.VerifyValidKey(key));

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.map.GetEnumerator();

    public IDictionaryEnumerator GetEnumerator() => this.map.GetEnumerator();

    public bool IsFixedSize => this.map.IsFixedSize;

    public bool IsReadOnly => this.map.IsReadOnly;

    public ICollection Keys => this.map.Keys;

    public void Remove(object key)
    {
      if (key == null)
        return;
      this.map.Remove(this.VerifyValidKey(key));
    }

    public object this[object key]
    {
      get => key == null ? (object) null : this.map[key];
      set => this.map[this.VerifyValidKey(key)] = value;
    }

    public ICollection Values => this.map.Values;

    public void CopyTo(Array array, int i) => this.map.CopyTo(array, i);

    public IList EntryList
    {
      get
      {
        IList entryList = (IList) new ArrayList(this.map.Count);
        foreach (DictionaryEntry dictionaryEntry1 in this.map)
        {
          DictionaryEntry dictionaryEntry2 = new DictionaryEntry(dictionaryEntry1.Key, dictionaryEntry1.Value);
          entryList.Add((object) dictionaryEntry2);
        }
        return entryList;
      }
    }

    private object VerifyValidKey(object obj)
    {
      return !(obj is ValueType) ? obj : throw new ArgumentException("There is a problem with your mappings.  You are probably trying to map a System.ValueType to a <class> which NHibernate does not allow or you are incorrectly using the IDictionary that is mapped to a <set>.  \n\nA ValueType (" + (object) obj.GetType() + ") can not be used with IdentityKey.  The thread at google has a good description about what happens with boxing and unboxing ValueTypes and why they can not be used as an IdentityKey: http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&oe=UTF-8&threadm=bds2rm%24ruc%241%40charly.heeg.de&rnum=1&prev=/groups%3Fhl%3Den%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26q%3DSystem.Runtime.CompilerServices.RuntimeHelpers.GetHashCode%26sa%3DN%26tab%3Dwg", "key");
    }

    public static IDictionary Invert(IDictionary map)
    {
      IDictionary dictionary = IdentityMap.Instantiate(map.Count);
      foreach (DictionaryEntry dictionaryEntry in dictionary)
        dictionary[dictionaryEntry.Value] = dictionaryEntry.Key;
      return dictionary;
    }

    public void OnDeserialization(object sender)
    {
      ((IDeserializationCallback) this.map).OnDeserialization(sender);
    }
  }
}
