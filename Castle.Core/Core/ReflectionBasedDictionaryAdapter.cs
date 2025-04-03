// Decompiled with JetBrains decompiler
// Type: Castle.Core.ReflectionBasedDictionaryAdapter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.Core
{
  public sealed class ReflectionBasedDictionaryAdapter : IDictionary, ICollection, IEnumerable
  {
    private readonly Dictionary<string, object> properties = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public ReflectionBasedDictionaryAdapter(object target)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      foreach (PropertyInfo property in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (property.CanRead && property.GetIndexParameters().Length <= 0)
        {
          object propertyValue = this.GetPropertyValue(target, property);
          this.properties[property.Name] = propertyValue;
        }
      }
    }

    public bool Contains(object key) => this.properties.ContainsKey(key.ToString());

    public object this[object key]
    {
      get
      {
        object obj;
        this.properties.TryGetValue(key.ToString(), out obj);
        return obj;
      }
      set => throw new NotImplementedException();
    }

    public void Add(object key, object value) => throw new NotImplementedException();

    public void Clear() => throw new NotImplementedException();

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
      return (IDictionaryEnumerator) new ReflectionBasedDictionaryAdapter.DictionaryEntryEnumeratorAdapter((IDictionaryEnumerator) this.properties.GetEnumerator());
    }

    public void Remove(object key)
    {
    }

    public ICollection Keys => (ICollection) this.properties.Keys;

    public ICollection Values => (ICollection) this.properties.Values;

    public bool IsReadOnly => true;

    bool IDictionary.IsFixedSize => throw new NotImplementedException();

    void ICollection.CopyTo(Array array, int index) => throw new NotImplementedException();

    public int Count => this.properties.Count;

    public object SyncRoot => (object) this.properties;

    public bool IsSynchronized => false;

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new ReflectionBasedDictionaryAdapter.DictionaryEntryEnumeratorAdapter((IDictionaryEnumerator) this.properties.GetEnumerator());
    }

    private object GetPropertyValue(object target, PropertyInfo property)
    {
      try
      {
        return property.GetValue(target, (object[]) null);
      }
      catch (MethodAccessException ex)
      {
        throw;
      }
    }

    private class DictionaryEntryEnumeratorAdapter : IDictionaryEnumerator, IEnumerator
    {
      private readonly IDictionaryEnumerator enumerator;
      private KeyValuePair<string, object> current;

      public DictionaryEntryEnumeratorAdapter(IDictionaryEnumerator enumerator)
      {
        this.enumerator = enumerator;
      }

      public object Key => (object) this.current.Key;

      public object Value => this.current.Value;

      public DictionaryEntry Entry => new DictionaryEntry(this.Key, this.Value);

      public bool MoveNext()
      {
        bool flag = this.enumerator.MoveNext();
        if (flag)
          this.current = (KeyValuePair<string, object>) this.enumerator.Current;
        return flag;
      }

      public void Reset() => this.enumerator.Reset();

      public object Current => (object) new DictionaryEntry(this.Key, this.Value);
    }
  }
}
