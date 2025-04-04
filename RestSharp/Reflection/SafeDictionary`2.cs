// Decompiled with JetBrains decompiler
// Type: RestSharp.Reflection.SafeDictionary`2
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.Collections.Generic;

#nullable disable
namespace RestSharp.Reflection
{
  internal class SafeDictionary<TKey, TValue>
  {
    private readonly object _padlock = new object();
    private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    public bool TryGetValue(TKey key, out TValue value)
    {
      return this._dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key] => this._dictionary[key];

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<TKey, TValue>>) this._dictionary).GetEnumerator();
    }

    public void Add(TKey key, TValue value)
    {
      lock (this._padlock)
      {
        if (this._dictionary.ContainsKey(key))
          return;
        this._dictionary.Add(key, value);
      }
    }
  }
}
