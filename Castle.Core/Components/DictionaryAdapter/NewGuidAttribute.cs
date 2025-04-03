// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.NewGuidAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false)]
  public class NewGuidAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryPropertyGetter,
    IDictionaryBehavior
  {
    private static readonly Guid UnassignedGuid = new Guid();

    public object GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor property,
      bool ifExists)
    {
      if (storedValue == null || storedValue.Equals((object) NewGuidAttribute.UnassignedGuid))
      {
        storedValue = (object) Guid.NewGuid();
        property.SetPropertyValue(dictionaryAdapter, key, ref storedValue, dictionaryAdapter.This.Descriptor);
      }
      return storedValue;
    }
  }
}
