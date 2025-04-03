// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DefaultPropertyGetter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class DefaultPropertyGetter : IDictionaryPropertyGetter, IDictionaryBehavior
  {
    private readonly TypeConverter converter;

    public DefaultPropertyGetter(TypeConverter converter) => this.converter = converter;

    public int ExecutionOrder => int.MaxValue;

    public object GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor property,
      bool ifExists)
    {
      Type propertyType = property.PropertyType;
      return storedValue != null && !propertyType.IsInstanceOfType(storedValue) && this.converter != null && this.converter.CanConvertFrom(storedValue.GetType()) ? this.converter.ConvertFrom(storedValue) : storedValue;
    }
  }
}
