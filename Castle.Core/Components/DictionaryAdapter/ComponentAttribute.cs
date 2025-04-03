// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.ComponentAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class ComponentAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryKeyBuilder,
    IDictionaryPropertyGetter,
    IDictionaryPropertySetter,
    IDictionaryBehavior
  {
    public bool NoPrefix
    {
      get => this.Prefix == "";
      set
      {
        if (!value)
          return;
        this.Prefix = "";
      }
    }

    public string Prefix { get; set; }

    string IDictionaryKeyBuilder.GetKey(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      PropertyDescriptor property)
    {
      return this.Prefix ?? key + "_";
    }

    object IDictionaryPropertyGetter.GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor property,
      bool ifExists)
    {
      if (storedValue != null)
        return storedValue;
      object propertyValue = dictionaryAdapter.This.ExtendedProperties[(object) property.PropertyName];
      if (propertyValue == null)
      {
        PropertyDescriptor descriptor = new PropertyDescriptor(property.Property, (object[]) null);
        descriptor.AddKeyBuilder((IDictionaryKeyBuilder) new KeyPrefixAttribute(key));
        propertyValue = dictionaryAdapter.This.Factory.GetAdapter(property.Property.PropertyType, dictionaryAdapter.This.Dictionary, descriptor);
        dictionaryAdapter.This.ExtendedProperties[(object) property.PropertyName] = propertyValue;
      }
      return propertyValue;
    }

    public bool SetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      ref object value,
      PropertyDescriptor property)
    {
      dictionaryAdapter.This.ExtendedProperties.Remove((object) property.PropertyName);
      return true;
    }
  }
}
