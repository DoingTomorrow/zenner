// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.StringFormatAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  public class StringFormatAttribute : 
    DictionaryBehaviorAttribute,
    IDictionaryPropertyGetter,
    IDictionaryBehavior
  {
    private static readonly char[] PropertyDelimeters = new char[2]
    {
      ',',
      ' '
    };

    public StringFormatAttribute(string format, string properties)
    {
      this.Format = format != null ? format : throw new ArgumentNullException(nameof (format));
      this.Properties = properties;
    }

    public string Format { get; private set; }

    public string Properties { get; private set; }

    object IDictionaryPropertyGetter.GetPropertyValue(
      IDictionaryAdapter dictionaryAdapter,
      string key,
      object storedValue,
      PropertyDescriptor property,
      bool ifExists)
    {
      return (object) string.Format(this.Format, this.GetFormatArguments(dictionaryAdapter, property.Property.Name)).Trim();
    }

    private object[] GetFormatArguments(
      IDictionaryAdapter dictionaryAdapter,
      string formattedPropertyName)
    {
      string[] strArray = this.Properties.Split(StringFormatAttribute.PropertyDelimeters, StringSplitOptions.RemoveEmptyEntries);
      object[] formatArguments = new object[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
      {
        string propertyName = strArray[index];
        formatArguments[index] = !(propertyName != formattedPropertyName) ? (object) "(recursive)" : dictionaryAdapter.GetProperty(propertyName, false);
      }
      return formatArguments;
    }
  }
}
