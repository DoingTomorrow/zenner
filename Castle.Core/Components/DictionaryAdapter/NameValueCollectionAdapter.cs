// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.NameValueCollectionAdapter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Specialized;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class NameValueCollectionAdapter : AbstractDictionaryAdapter
  {
    private readonly NameValueCollection nameValues;

    public NameValueCollectionAdapter(NameValueCollection nameValues)
    {
      this.nameValues = nameValues;
    }

    public override bool IsReadOnly => false;

    public override bool Contains(object key)
    {
      return Array.IndexOf<object>((object[]) this.nameValues.AllKeys, key) >= 0;
    }

    public override object this[object key]
    {
      get => (object) this.nameValues[key.ToString()];
      set
      {
        string str = value?.ToString();
        this.nameValues[key.ToString()] = str;
      }
    }

    public static NameValueCollectionAdapter Adapt(NameValueCollection nameValues)
    {
      return new NameValueCollectionAdapter(nameValues);
    }
  }
}
