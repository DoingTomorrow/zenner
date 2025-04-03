// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.StringMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

#nullable disable
namespace AutoMapper.Mappers
{
  public class StringMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      return context.SourceValue == null ? (object) mapper.FormatValue(context.CreateValueContext((object) null)) : (object) mapper.FormatValue(context);
    }

    public bool IsMatch(ResolutionContext context)
    {
      return context.DestinationType.Equals(typeof (string));
    }
  }
}
