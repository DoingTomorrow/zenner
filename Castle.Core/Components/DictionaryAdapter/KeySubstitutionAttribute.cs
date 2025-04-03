// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.KeySubstitutionAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
  public class KeySubstitutionAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryKeyBuilder,
    IDictionaryBehavior
  {
    private readonly string oldValue;
    private readonly string newValue;

    public KeySubstitutionAttribute(string oldValue, string newValue)
    {
      this.oldValue = oldValue;
      this.newValue = newValue;
    }

    string IDictionaryKeyBuilder.GetKey(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      PropertyDescriptor property)
    {
      return key.Replace(this.oldValue, this.newValue);
    }
  }
}
