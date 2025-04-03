// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.TypeConverterMapper
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using AutoMapper.Internal;
using System;
using System.ComponentModel;

#nullable disable
namespace AutoMapper.Mappers
{
  public class TypeConverterMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      if (context.SourceValue == null)
        return mapper.CreateObject(context);
      Func<object> converter = TypeConverterMapper.GetConverter(context);
      return converter == null ? (object) null : converter();
    }

    private static Func<object> GetConverter(ResolutionContext context)
    {
      TypeConverter typeConverter = TypeConverterMapper.GetTypeConverter(context.SourceType);
      if (typeConverter.CanConvertTo(context.DestinationType))
        return (Func<object>) (() => typeConverter.ConvertTo(context.SourceValue, context.DestinationType));
      if (context.DestinationType.IsNullableType() && typeConverter.CanConvertTo(Nullable.GetUnderlyingType(context.DestinationType)))
        return (Func<object>) (() => typeConverter.ConvertTo(context.SourceValue, Nullable.GetUnderlyingType(context.DestinationType)));
      typeConverter = TypeConverterMapper.GetTypeConverter(context.DestinationType);
      return typeConverter.CanConvertFrom(context.SourceType) ? (Func<object>) (() => typeConverter.ConvertFrom(context.SourceValue)) : (Func<object>) null;
    }

    public bool IsMatch(ResolutionContext context)
    {
      return TypeConverterMapper.GetConverter(context) != null;
    }

    private static TypeConverter GetTypeConverter(Type type) => TypeDescriptor.GetConverter(type);
  }
}
