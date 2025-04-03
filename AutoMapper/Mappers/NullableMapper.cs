// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.NullableMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;

#nullable disable
namespace AutoMapper.Mappers
{
  public class NullableMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      return context.SourceValue;
    }

    public bool IsMatch(ResolutionContext context) => context.DestinationType.IsNullableType();
  }
}
