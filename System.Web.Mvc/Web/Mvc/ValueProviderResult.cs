// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValueProviderResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [Serializable]
  public class ValueProviderResult
  {
    private static readonly CultureInfo _staticCulture = CultureInfo.InvariantCulture;
    private CultureInfo _instanceCulture;

    protected ValueProviderResult()
    {
    }

    public ValueProviderResult(object rawValue, string attemptedValue, CultureInfo culture)
    {
      this.RawValue = rawValue;
      this.AttemptedValue = attemptedValue;
      this.Culture = culture;
    }

    public string AttemptedValue { get; protected set; }

    public CultureInfo Culture
    {
      get
      {
        if (this._instanceCulture == null)
          this._instanceCulture = ValueProviderResult._staticCulture;
        return this._instanceCulture;
      }
      protected set => this._instanceCulture = value;
    }

    public object RawValue { get; protected set; }

    private static object ConvertSimpleType(
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      if (value == null || destinationType.IsInstanceOfType(value))
        return value;
      switch (value)
      {
        case string str when str.Trim().Length == 0:
          return (object) null;
        case IConvertible convertible:
          try
          {
            return convertible.ToType(destinationType, (IFormatProvider) culture);
          }
          catch
          {
            break;
          }
      }
      TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
      bool flag = converter.CanConvertFrom(value.GetType());
      if (!flag)
        converter = TypeDescriptor.GetConverter(value.GetType());
      if (!flag)
      {
        if (!converter.CanConvertTo(destinationType))
        {
          if (destinationType.IsEnum && value is int num)
            return Enum.ToObject(destinationType, num);
          Type underlyingType = Nullable.GetUnderlyingType(destinationType);
          if (underlyingType != (Type) null)
            return ValueProviderResult.ConvertSimpleType(culture, value, underlyingType);
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ValueProviderResult_NoConverterExists, new object[2]
          {
            (object) value.GetType().FullName,
            (object) destinationType.FullName
          }));
        }
      }
      try
      {
        return flag ? converter.ConvertFrom((ITypeDescriptorContext) null, culture, value) : converter.ConvertTo((ITypeDescriptorContext) null, culture, value, destinationType);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ValueProviderResult_ConversionThrew, new object[2]
        {
          (object) value.GetType().FullName,
          (object) destinationType.FullName
        }), ex);
      }
    }

    public object ConvertTo(Type type) => this.ConvertTo(type, (CultureInfo) null);

    public virtual object ConvertTo(Type type, CultureInfo culture)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      return ValueProviderResult.UnwrapPossibleArrayType(culture ?? this.Culture, this.RawValue, type);
    }

    private static object UnwrapPossibleArrayType(
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      if (value == null || destinationType.IsInstanceOfType(value))
        return value;
      Array array = value as Array;
      if (destinationType.IsArray)
      {
        Type elementType = destinationType.GetElementType();
        if (array != null)
        {
          IList instance = (IList) Array.CreateInstance(elementType, array.Length);
          for (int index = 0; index < array.Length; ++index)
            instance[index] = ValueProviderResult.ConvertSimpleType(culture, array.GetValue(index), elementType);
          return (object) instance;
        }
        object obj = ValueProviderResult.ConvertSimpleType(culture, value, elementType);
        IList instance1 = (IList) Array.CreateInstance(elementType, 1);
        instance1[0] = obj;
        return (object) instance1;
      }
      if (array == null)
        return ValueProviderResult.ConvertSimpleType(culture, value, destinationType);
      if (array.Length <= 0)
        return (object) null;
      value = array.GetValue(0);
      return ValueProviderResult.ConvertSimpleType(culture, value, destinationType);
    }
  }
}
