// Decompiled with JetBrains decompiler
// Type: AutoMapper.IMappingEngine
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public interface IMappingEngine : IDisposable
  {
    IConfigurationProvider ConfigurationProvider { get; }

    TDestination Map<TDestination>(object source);

    TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts);

    TDestination Map<TSource, TDestination>(TSource source);

    TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions> opts);

    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    TDestination Map<TSource, TDestination>(
      TSource source,
      TDestination destination,
      Action<IMappingOperationOptions> opts);

    object Map(object source, Type sourceType, Type destinationType);

    object Map(
      object source,
      Type sourceType,
      Type destinationType,
      Action<IMappingOperationOptions> opts);

    object Map(object source, object destination, Type sourceType, Type destinationType);

    object Map(
      object source,
      object destination,
      Type sourceType,
      Type destinationType,
      Action<IMappingOperationOptions> opts);

    TDestination DynamicMap<TSource, TDestination>(TSource source);

    TDestination DynamicMap<TDestination>(object source);

    object DynamicMap(object source, Type sourceType, Type destinationType);

    void DynamicMap<TSource, TDestination>(TSource source, TDestination destination);

    void DynamicMap(object source, object destination, Type sourceType, Type destinationType);
  }
}
