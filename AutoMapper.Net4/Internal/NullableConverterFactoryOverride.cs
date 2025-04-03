// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.NullableConverterFactoryOverride
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System;
using System.ComponentModel;

#nullable disable
namespace AutoMapper.Internal
{
  public class NullableConverterFactoryOverride : INullableConverterFactory
  {
    public INullableConverter Create(Type nullableType)
    {
      return (INullableConverter) new NullableConverterFactoryOverride.NullableConverterImpl(new NullableConverter(nullableType));
    }

    private class NullableConverterImpl : INullableConverter
    {
      private readonly NullableConverter _nullableConverter;

      public NullableConverterImpl(NullableConverter nullableConverter)
      {
        this._nullableConverter = nullableConverter;
      }

      public object ConvertFrom(object value) => this._nullableConverter.ConvertFrom(value);

      public Type UnderlyingType => this._nullableConverter.UnderlyingType;
    }
  }
}
