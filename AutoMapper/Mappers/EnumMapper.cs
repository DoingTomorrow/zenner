// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.EnumMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;

#nullable disable
namespace AutoMapper.Mappers
{
  public class EnumMapper : IObjectMapper
  {
    private static readonly INullableConverterFactory NullableConverterFactory = PlatformAdapter.Resolve<INullableConverterFactory>();
    private static readonly IEnumNameValueMapperFactory EnumNameValueMapperFactory = PlatformAdapter.Resolve<IEnumNameValueMapperFactory>(false);

    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      bool toEnum = false;
      Type enumerationType1 = TypeHelper.GetEnumerationType(context.SourceType);
      Type enumerationType2 = TypeHelper.GetEnumerationType(context.DestinationType);
      if (EnumMapper.EnumToStringMapping(context, ref toEnum))
      {
        if (context.SourceValue == null)
          return mapper.CreateObject(context);
        if (!toEnum)
          return (object) Enum.GetName(enumerationType1, context.SourceValue);
        string str = context.SourceValue.ToString();
        return string.IsNullOrEmpty(str) ? mapper.CreateObject(context) : Enum.Parse(enumerationType2, str, true);
      }
      if (EnumMapper.EnumToEnumMapping(context))
      {
        if (context.SourceValue == null)
          return mapper.ShouldMapSourceValueAsNull(context) && context.DestinationType.IsNullableType() ? (object) null : mapper.CreateObject(context);
        if (!Enum.IsDefined(enumerationType1, context.SourceValue))
          return Enum.ToObject(enumerationType2, context.SourceValue);
        if (FeatureDetector.IsEnumGetNamesSupported)
        {
          IEnumNameValueMapper enumNameValueMapper = EnumMapper.EnumNameValueMapperFactory.Create();
          if (enumNameValueMapper.IsMatch(enumerationType2, context.SourceValue.ToString()))
            return enumNameValueMapper.Convert(enumerationType1, enumerationType2, context);
        }
        return Enum.Parse(enumerationType2, Enum.GetName(enumerationType1, context.SourceValue), true);
      }
      if (!EnumMapper.EnumToUnderlyingTypeMapping(context, ref toEnum))
        return (object) null;
      if (toEnum)
        return Enum.Parse(enumerationType2, context.SourceValue.ToString(), true);
      return EnumMapper.EnumToNullableTypeMapping(context) ? EnumMapper.ConvertEnumToNullableType(context) : Convert.ChangeType(context.SourceValue, context.DestinationType, (IFormatProvider) null);
    }

    public bool IsMatch(ResolutionContext context)
    {
      bool toEnum = false;
      return EnumMapper.EnumToStringMapping(context, ref toEnum) || EnumMapper.EnumToEnumMapping(context) || EnumMapper.EnumToUnderlyingTypeMapping(context, ref toEnum);
    }

    private static bool EnumToEnumMapping(ResolutionContext context)
    {
      Type enumerationType1 = TypeHelper.GetEnumerationType(context.SourceType);
      Type enumerationType2 = TypeHelper.GetEnumerationType(context.DestinationType);
      return (object) enumerationType1 != null && (object) enumerationType2 != null;
    }

    private static bool EnumToUnderlyingTypeMapping(ResolutionContext context, ref bool toEnum)
    {
      Type enumerationType1 = TypeHelper.GetEnumerationType(context.SourceType);
      Type enumerationType2 = TypeHelper.GetEnumerationType(context.DestinationType);
      if ((object) enumerationType1 != null)
        return context.DestinationType.IsAssignableFrom(Enum.GetUnderlyingType(enumerationType1));
      if ((object) enumerationType2 == null)
        return false;
      toEnum = true;
      return context.SourceType.IsAssignableFrom(Enum.GetUnderlyingType(enumerationType2));
    }

    private static bool EnumToStringMapping(ResolutionContext context, ref bool toEnum)
    {
      Type enumerationType1 = TypeHelper.GetEnumerationType(context.SourceType);
      Type enumerationType2 = TypeHelper.GetEnumerationType(context.DestinationType);
      if ((object) enumerationType1 != null)
        return context.DestinationType.IsAssignableFrom(typeof (string));
      if ((object) enumerationType2 == null)
        return false;
      toEnum = true;
      return context.SourceType.IsAssignableFrom(typeof (string));
    }

    private static bool EnumToNullableTypeMapping(ResolutionContext context)
    {
      return context.DestinationType.IsGenericType && context.DestinationType.GetGenericTypeDefinition().Equals(typeof (Nullable<>));
    }

    private static object ConvertEnumToNullableType(ResolutionContext context)
    {
      INullableConverter nullableConverter = EnumMapper.NullableConverterFactory.Create(context.DestinationType);
      if (context.IsSourceValueNull)
        return nullableConverter.ConvertFrom(context.SourceValue);
      Type underlyingType = nullableConverter.UnderlyingType;
      return Convert.ChangeType(context.SourceValue, underlyingType, (IFormatProvider) null);
    }
  }
}
