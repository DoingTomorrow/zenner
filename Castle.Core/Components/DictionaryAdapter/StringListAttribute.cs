// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.StringListAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class StringListAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryPropertyGetter,
    IDictionaryPropertySetter,
    IDictionaryBehavior
  {
    private char separator = ',';

    public char Separator
    {
      get => this.separator;
      set => this.separator = value;
    }

    object IDictionaryPropertyGetter.GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor property,
      bool ifExists)
    {
      Type propertyType = property.PropertyType;
      if ((storedValue == null || !storedValue.GetType().IsInstanceOfType((object) propertyType)) && propertyType.IsGenericType)
      {
        Type genericTypeDefinition = propertyType.GetGenericTypeDefinition();
        if (genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (ICollection<>) || genericTypeDefinition == typeof (List<>) || genericTypeDefinition == typeof (IEnumerable<>))
        {
          Type genericArgument = propertyType.GetGenericArguments()[0];
          TypeConverter converter = TypeDescriptor.GetConverter(genericArgument);
          if (converter != null && converter.CanConvertFrom(typeof (string)))
            return Activator.CreateInstance(typeof (StringListWrapper<>).MakeGenericType(genericArgument), (object) key, storedValue, (object) this.separator, (object) dictionaryAdapter.This.Dictionary);
        }
      }
      return storedValue;
    }

    bool IDictionaryPropertySetter.SetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      ref object value,
      PropertyDescriptor property)
    {
      if (value is IEnumerable enumerable)
        value = (object) StringListAttribute.BuildString(enumerable, this.separator);
      return true;
    }

    internal static string BuildString(IEnumerable enumerable, char separator)
    {
      bool flag = true;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in enumerable)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(separator);
        stringBuilder.Append(obj.ToString());
      }
      return stringBuilder.ToString();
    }
  }
}
