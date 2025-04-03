// Decompiled with JetBrains decompiler
// Type: AutoMapper.IMappingEngineRunner
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

#nullable disable
namespace AutoMapper
{
  public interface IMappingEngineRunner
  {
    object Map(ResolutionContext context);

    object CreateObject(ResolutionContext context);

    string FormatValue(ResolutionContext context);

    IConfigurationProvider ConfigurationProvider { get; }

    bool ShouldMapSourceValueAsNull(ResolutionContext context);

    bool ShouldMapSourceCollectionAsNull(ResolutionContext context);
  }
}
