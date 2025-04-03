// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.StringValuesAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
  public class StringValuesAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryPropertySetter,
    IDictionaryBehavior
  {
    private string format;

    public string Format
    {
      get => this.format;
      set => this.format = value;
    }

    bool IDictionaryPropertySetter.SetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      ref object value,
      PropertyDescriptor property)
    {
      if (value != null)
        value = (object) this.GetPropertyAsString(property, value);
      return true;
    }

    private string GetPropertyAsString(PropertyDescriptor property, object value)
    {
      if (!string.IsNullOrEmpty(this.format))
        return string.Format(this.format, value);
      TypeConverter typeConverter = property.TypeConverter;
      return typeConverter != null && typeConverter.CanConvertTo(typeof (string)) ? (string) typeConverter.ConvertTo(value, typeof (string)) : value.ToString();
    }
  }
}
