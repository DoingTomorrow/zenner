// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.NullableConverterFactory
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper.Internal
{
  public class NullableConverterFactory : INullableConverterFactory
  {
    public INullableConverter Create(Type nullableType)
    {
      return (INullableConverter) new NullableConverterFactory.NullableConverterImpl(nullableType);
    }

    private class NullableConverterImpl : INullableConverter
    {
      private readonly Type _nullableType;
      private readonly Type _underlyingType;

      public NullableConverterImpl(Type nullableType)
      {
        this._nullableType = nullableType;
        this._underlyingType = Nullable.GetUnderlyingType(this._nullableType);
      }

      public object ConvertFrom(object value)
      {
        if (value == null)
          return Activator.CreateInstance(this._nullableType);
        return (object) value.GetType() == (object) this.UnderlyingType ? Activator.CreateInstance(this._nullableType, value) : Activator.CreateInstance(this._nullableType, Convert.ChangeType(value, this.UnderlyingType, (IFormatProvider) null));
      }

      public Type UnderlyingType => this._underlyingType;
    }
  }
}
