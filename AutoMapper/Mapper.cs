// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using AutoMapper.Mappers;
using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper
{
  public static class Mapper
  {
    private static readonly Func<ConfigurationStore> _configurationInit = (Func<ConfigurationStore>) (() =>
    {
      PlatformAdapter.Resolve<IPlatformSpecificMapperRegistry>().Initialize();
      return new ConfigurationStore((ITypeMapFactory) new TypeMapFactory(), (IEnumerable<IObjectMapper>) MapperRegistry.Mappers);
    });
    private static ILazy<ConfigurationStore> _configuration = LazyFactory.Create<ConfigurationStore>(Mapper._configurationInit);
    private static readonly Func<IMappingEngine> _mappingEngineInit = (Func<IMappingEngine>) (() => (IMappingEngine) new MappingEngine((IConfigurationProvider) Mapper._configuration.Value));
    private static ILazy<IMappingEngine> _mappingEngine = LazyFactory.Create<IMappingEngine>(Mapper._mappingEngineInit);

    public static bool AllowNullDestinationValues
    {
      get => Mapper.Configuration.AllowNullDestinationValues;
      set => Mapper.Configuration.AllowNullDestinationValues = value;
    }

    public static TDestination Map<TDestination>(object source)
    {
      return Mapper.Engine.Map<TDestination>(source);
    }

    public static TDestination Map<TDestination>(
      object source,
      Action<IMappingOperationOptions> opts)
    {
      return Mapper.Engine.Map<TDestination>(source, opts);
    }

    public static TDestination Map<TSource, TDestination>(TSource source)
    {
      return Mapper.Engine.Map<TSource, TDestination>(source);
    }

    public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
      return Mapper.Engine.Map<TSource, TDestination>(source, destination);
    }

    public static TDestination Map<TSource, TDestination>(
      TSource source,
      TDestination destination,
      Action<IMappingOperationOptions> opts)
    {
      return Mapper.Engine.Map<TSource, TDestination>(source, destination, opts);
    }

    public static TDestination Map<TSource, TDestination>(
      TSource source,
      Action<IMappingOperationOptions> opts)
    {
      return Mapper.Engine.Map<TSource, TDestination>(source, opts);
    }

    public static object Map(object source, Type sourceType, Type destinationType)
    {
      return Mapper.Engine.Map(source, sourceType, destinationType);
    }

    public static object Map(
      object source,
      Type sourceType,
      Type destinationType,
      Action<IMappingOperationOptions> opts)
    {
      return Mapper.Engine.Map(source, sourceType, destinationType, opts);
    }

    public static object Map(
      object source,
      object destination,
      Type sourceType,
      Type destinationType)
    {
      return Mapper.Engine.Map(source, destination, sourceType, destinationType);
    }

    public static object Map(
      object source,
      object destination,
      Type sourceType,
      Type destinationType,
      Action<IMappingOperationOptions> opts)
    {
      return Mapper.Engine.Map(source, destination, sourceType, destinationType, opts);
    }

    public static TDestination DynamicMap<TSource, TDestination>(TSource source)
    {
      return Mapper.Engine.DynamicMap<TSource, TDestination>(source);
    }

    public static void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
    {
      Mapper.Engine.DynamicMap<TSource, TDestination>(source, destination);
    }

    public static TDestination DynamicMap<TDestination>(object source)
    {
      return Mapper.Engine.DynamicMap<TDestination>(source);
    }

    public static object DynamicMap(object source, Type sourceType, Type destinationType)
    {
      return Mapper.Engine.DynamicMap(source, sourceType, destinationType);
    }

    public static void DynamicMap(
      object source,
      object destination,
      Type sourceType,
      Type destinationType)
    {
      Mapper.Engine.DynamicMap(source, destination, sourceType, destinationType);
    }

    public static void Initialize(Action<IConfiguration> action)
    {
      Mapper.Reset();
      action(Mapper.Configuration);
      Mapper.Configuration.Seal();
    }

    [Obsolete("Formatters should not be used.")]
    public static IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      return Mapper.Configuration.AddFormatter<TValueFormatter>();
    }

    [Obsolete("Formatters should not be used.")]
    public static IFormatterCtorExpression AddFormatter(Type valueFormatterType)
    {
      return Mapper.Configuration.AddFormatter(valueFormatterType);
    }

    [Obsolete("Formatters should not be used.")]
    public static void AddFormatter(IValueFormatter formatter)
    {
      Mapper.Configuration.AddFormatter(formatter);
    }

    [Obsolete("Formatters should not be used.")]
    public static void AddFormatExpression(Func<ResolutionContext, string> formatExpression)
    {
      Mapper.Configuration.AddFormatExpression(formatExpression);
    }

    [Obsolete("Formatters should not be used.")]
    public static void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      Mapper.Configuration.SkipFormatter<TValueFormatter>();
    }

    public static IFormatterExpression ForSourceType<TSource>()
    {
      return Mapper.Configuration.ForSourceType<TSource>();
    }

    public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
      return Mapper.Configuration.CreateMap<TSource, TDestination>();
    }

    public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(
      MemberList memberList)
    {
      return Mapper.Configuration.CreateMap<TSource, TDestination>(memberList);
    }

    public static IMappingExpression CreateMap(Type sourceType, Type destinationType)
    {
      return Mapper.Configuration.CreateMap(sourceType, destinationType);
    }

    public static IMappingExpression CreateMap(
      Type sourceType,
      Type destinationType,
      MemberList memberList)
    {
      return Mapper.Configuration.CreateMap(sourceType, destinationType, memberList);
    }

    public static IProfileExpression CreateProfile(string profileName)
    {
      return Mapper.Configuration.CreateProfile(profileName);
    }

    public static void CreateProfile(
      string profileName,
      Action<IProfileExpression> profileConfiguration)
    {
      Mapper.Configuration.CreateProfile(profileName, profileConfiguration);
    }

    public static void AddProfile(Profile profile) => Mapper.Configuration.AddProfile(profile);

    public static void AddProfile<TProfile>() where TProfile : Profile, new()
    {
      Mapper.Configuration.AddProfile<TProfile>();
    }

    public static TypeMap FindTypeMapFor(Type sourceType, Type destinationType)
    {
      return Mapper.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);
    }

    public static TypeMap FindTypeMapFor<TSource, TDestination>()
    {
      return Mapper.ConfigurationProvider.FindTypeMapFor(typeof (TSource), typeof (TDestination));
    }

    public static TypeMap[] GetAllTypeMaps() => Mapper.ConfigurationProvider.GetAllTypeMaps();

    public static void AssertConfigurationIsValid()
    {
      Mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    public static void AssertConfigurationIsValid(TypeMap typeMap)
    {
      Mapper.ConfigurationProvider.AssertConfigurationIsValid(typeMap);
    }

    public static void AssertConfigurationIsValid(string profileName)
    {
      Mapper.ConfigurationProvider.AssertConfigurationIsValid(profileName);
    }

    public static void Reset()
    {
      MapperRegistry.Reset();
      Mapper._configuration = LazyFactory.Create<ConfigurationStore>(Mapper._configurationInit);
      Mapper._mappingEngine = LazyFactory.Create<IMappingEngine>(Mapper._mappingEngineInit);
    }

    public static IMappingEngine Engine => Mapper._mappingEngine.Value;

    public static IConfiguration Configuration => (IConfiguration) Mapper.ConfigurationProvider;

    private static IConfigurationProvider ConfigurationProvider
    {
      get => (IConfigurationProvider) Mapper._configuration.Value;
    }

    public static void AddGlobalIgnore(string startingwith)
    {
      Mapper.Configuration.AddGlobalIgnore(startingwith);
    }
  }
}
