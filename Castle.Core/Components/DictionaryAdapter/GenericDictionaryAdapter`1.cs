// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.GenericDictionaryAdapter`1
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class GenericDictionaryAdapter<TValue> : AbstractDictionaryAdapter
  {
    private readonly IDictionary<string, TValue> dictionary;

    public GenericDictionaryAdapter(IDictionary<string, TValue> dictionary)
    {
      this.dictionary = dictionary;
    }

    public override bool IsReadOnly => this.dictionary.IsReadOnly;

    public override bool Contains(object key)
    {
      return this.dictionary.Keys.Contains(GenericDictionaryAdapter<TValue>.GetKey(key));
    }

    public override object this[object key]
    {
      get => (object) this.dictionary[GenericDictionaryAdapter<TValue>.GetKey(key)];
      set => this.dictionary[GenericDictionaryAdapter<TValue>.GetKey(key)] = (TValue) value;
    }

    private static string GetKey(object key)
    {
      return key != null ? key.ToString() : throw new ArgumentNullException(nameof (key));
    }
  }
}
