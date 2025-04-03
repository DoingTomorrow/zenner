// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.KeyPrefixAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
  public class KeyPrefixAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryKeyBuilder,
    IDictionaryBehavior
  {
    private string keyPrefix;

    public KeyPrefixAttribute()
    {
    }

    public KeyPrefixAttribute(string keyPrefix) => this.keyPrefix = keyPrefix;

    public string KeyPrefix
    {
      get => this.keyPrefix;
      set => this.keyPrefix = value;
    }

    string IDictionaryKeyBuilder.GetKey(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      PropertyDescriptor property)
    {
      return this.keyPrefix + key;
    }
  }
}
