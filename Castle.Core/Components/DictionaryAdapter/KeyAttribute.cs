// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.KeyAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class KeyAttribute : DictionaryBehaviorAttribute, IDictionaryKeyBuilder, IDictionaryBehavior
  {
    private readonly string key;

    public KeyAttribute(string key) => this.key = key;

    public KeyAttribute(string[] keys) => this.key = string.Join(",", keys);

    public string Key { get; private set; }

    string IDictionaryKeyBuilder.GetKey(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      PropertyDescriptor property)
    {
      return this.key;
    }
  }
}
