// Decompiled with JetBrains decompiler
// Type: AutoMapper.MappingEngine
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Impl;
using AutoMapper.Internal;
using AutoMapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper
{
  public class MappingEngine : IMappingEngine, IDisposable, IMappingEngineRunner
  {
    private static readonly IDictionaryFactory DictionaryFactory = PlatformAdapter.Resolve<IDictionaryFactory>();
    private static readonly IProxyGeneratorFactory ProxyGeneratorFactory = PlatformAdapter.Resolve<IProxyGeneratorFactory>();
    private bool _disposed;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IObjectMapper[] _mappers;
    private readonly AutoMapper.Internal.IDictionary<TypePair, IObjectMapper> _objectMapperCache = MappingEngine.DictionaryFactory.CreateDictionary<TypePair, IObjectMapper>();
    private readonly Func<Type, object> _serviceCtor;

    public MappingEngine(IConfigurationProvider configurationProvider)
      : this(configurationProvider, MappingEngine.DictionaryFactory.CreateDictionary<TypePair, IObjectMapper>(), configurationProvider.ServiceCtor)
    {
    }

    public MappingEngine(
      IConfigurationProvider configurationProvider,
      AutoMapper.Internal.IDictionary<TypePair, IObjectMapper> objectMapperCache,
      Func<Type, object> serviceCtor)
    {
      this._configurationProvider = configurationProvider;
      this._objectMapperCache = objectMapperCache;
      this._serviceCtor = serviceCtor;
      this._mappers = configurationProvider.GetMappers();
      this._configurationProvider.TypeMapCreated += new EventHandler<TypeMapCreatedEventArgs>(this.ClearTypeMap);
    }

    public IConfigurationProvider ConfigurationProvider => this._configurationProvider;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && this._configurationProvider != null)
        this._configurationProvider.TypeMapCreated -= new EventHandler<TypeMapCreatedEventArgs>(this.ClearTypeMap);
      this._disposed = true;
    }

    public TDestination Map<TDestination>(object source)
    {
      return this.Map<TDestination>(source, new Action<IMappingOperationOptions>(this.DefaultMappingOptions));
    }

    public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts)
    {
      TDestination destination = default (TDestination);
      if (source != null)
      {
        Type type = source.GetType();
        Type destinationType = typeof (TDestination);
        destination = (TDestination) this.Map(source, type, destinationType, opts);
      }
      return destination;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
      Type sourceType = typeof (TSource);
      Type destinationType = typeof (TDestination);
      return (TDestination) this.Map((object) source, sourceType, destinationType, new Action<IMappingOperationOptions>(this.DefaultMappingOptions));
    }

    public TDestination Map<TSource, TDestination>(
      TSource source,
      Action<IMappingOperationOptions> opts)
    {
      Type sourceType = typeof (TSource);
      Type destinationType = typeof (TDestination);
      return (TDestination) this.Map((object) source, sourceType, destinationType, opts);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
      return this.Map<TSource, TDestination>(source, destination, new Action<IMappingOperationOptions>(this.DefaultMappingOptions));
    }

    public TDestination Map<TSource, TDestination>(
      TSource source,
      TDestination destination,
      Action<IMappingOperationOptions> opts)
    {
      Type sourceType = typeof (TSource);
      Type destinationType = typeof (TDestination);
      return (TDestination) this.Map((object) source, (object) destination, sourceType, destinationType, opts);
    }

    public object Map(object source, Type sourceType, Type destinationType)
    {
      return this.Map(source, sourceType, destinationType, new Action<IMappingOperationOptions>(this.DefaultMappingOptions));
    }

    public object Map(
      object source,
      Type sourceType,
      Type destinationType,
      Action<IMappingOperationOptions> opts)
    {
      TypeMap typeMapFor = this.ConfigurationProvider.FindTypeMapFor(source, (object) null, sourceType, destinationType);
      MappingOperationOptions options = new MappingOperationOptions();
      opts((IMappingOperationOptions) options);
      return ((IMappingEngineRunner) this).Map(new ResolutionContext(typeMapFor, source, sourceType, destinationType, options));
    }

    public object Map(object source, object destination, Type sourceType, Type destinationType)
    {
      return this.Map(source, destination, sourceType, destinationType, new Action<IMappingOperationOptions>(this.DefaultMappingOptions));
    }

    public object Map(
      object source,
      object destination,
      Type sourceType,
      Type destinationType,
      Action<IMappingOperationOptions> opts)
    {
      TypeMap typeMapFor = this.ConfigurationProvider.FindTypeMapFor(source, destination, sourceType, destinationType);
      MappingOperationOptions options = new MappingOperationOptions();
      opts((IMappingOperationOptions) options);
      return ((IMappingEngineRunner) this).Map(new ResolutionContext(typeMapFor, source, destination, sourceType, destinationType, options));
    }

    public TDestination DynamicMap<TSource, TDestination>(TSource source)
    {
      Type sourceType = typeof (TSource);
      Type destinationType = typeof (TDestination);
      return (TDestination) this.DynamicMap((object) source, sourceType, destinationType);
    }

    public void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
    {
      Type sourceType = typeof (TSource);
      Type destinationType = typeof (TDestination);
      this.DynamicMap((object) source, (object) destination, sourceType, destinationType);
    }

    public TDestination DynamicMap<TDestination>(object source)
    {
      Type sourceType = source == null ? typeof (object) : source.GetType();
      Type destinationType = typeof (TDestination);
      return (TDestination) this.DynamicMap(source, sourceType, destinationType);
    }

    public object DynamicMap(object source, Type sourceType, Type destinationType)
    {
      return ((IMappingEngineRunner) this).Map(new ResolutionContext(this.ConfigurationProvider.FindTypeMapFor(source, (object) null, sourceType, destinationType) ?? this.ConfigurationProvider.CreateTypeMap(sourceType, destinationType), source, sourceType, destinationType, new MappingOperationOptions()
      {
        CreateMissingTypeMaps = true
      }));
    }

    public void DynamicMap(
      object source,
      object destination,
      Type sourceType,
      Type destinationType)
    {
      ((IMappingEngineRunner) this).Map(new ResolutionContext(this.ConfigurationProvider.FindTypeMapFor(source, destination, sourceType, destinationType) ?? this.ConfigurationProvider.CreateTypeMap(sourceType, destinationType), source, destination, sourceType, destinationType, new MappingOperationOptions()
      {
        CreateMissingTypeMaps = true
      }));
    }

    public TDestination Map<TSource, TDestination>(ResolutionContext parentContext, TSource source)
    {
      Type destinationType = typeof (TDestination);
      Type sourceType = typeof (TSource);
      TypeMap typeMapFor = this.ConfigurationProvider.FindTypeMapFor((object) source, (object) null, sourceType, destinationType);
      return (TDestination) ((IMappingEngineRunner) this).Map(parentContext.CreateTypeContext(typeMapFor, (object) source, (object) null, sourceType, destinationType));
    }

    object IMappingEngineRunner.Map(ResolutionContext context)
    {
      try
      {
        TypePair key = new TypePair(context.SourceType, context.DestinationType);
        Func<TypePair, IObjectMapper> valueFactory = (Func<TypePair, IObjectMapper>) (tp => ((IEnumerable<IObjectMapper>) this._mappers).FirstOrDefault<IObjectMapper>((Func<IObjectMapper, bool>) (mapper => mapper.IsMatch(context))));
        IObjectMapper orAdd = this._objectMapperCache.GetOrAdd(key, valueFactory);
        if (orAdd == null)
        {
          if (context.Options.CreateMissingTypeMaps)
          {
            TypeMap typeMap = this.ConfigurationProvider.CreateTypeMap(context.SourceType, context.DestinationType);
            context = context.CreateTypeContext(typeMap, context.SourceValue, context.DestinationValue, context.SourceType, context.DestinationType);
            orAdd = this._objectMapperCache.GetOrAdd(key, valueFactory);
          }
          else
            return context.SourceValue == null ? ObjectCreator.CreateDefaultValue(context.DestinationType) : throw new AutoMapperMappingException(context, "Missing type map configuration or unsupported mapping.");
        }
        return orAdd.Map(context, (IMappingEngineRunner) this);
      }
      catch (AutoMapperMappingException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new AutoMapperMappingException(context, ex);
      }
    }

    string IMappingEngineRunner.FormatValue(ResolutionContext context)
    {
      TypeMap contextTypeMap = context.GetContextTypeMap();
      IFormatterConfiguration formatterConfiguration = contextTypeMap != null ? this.ConfigurationProvider.GetProfileConfiguration(contextTypeMap.Profile) : this.ConfigurationProvider.GetProfileConfiguration("");
      object sourceValue = context.SourceValue;
      string str = context.SourceValue.ToNullSafeString();
      foreach (IValueFormatter valueFormatter in formatterConfiguration.GetFormattersToApply(context))
      {
        str = valueFormatter.FormatValue(context.CreateValueContext(sourceValue));
        sourceValue = (object) str;
      }
      return str == null && !((IMappingEngineRunner) this).ShouldMapSourceValueAsNull(context) ? string.Empty : str;
    }

    object IMappingEngineRunner.CreateObject(ResolutionContext context)
    {
      TypeMap typeMap = context.TypeMap;
      Type type = context.DestinationType;
      if (typeMap != null)
      {
        if (typeMap.DestinationCtor != null)
          return typeMap.DestinationCtor(context);
        if (typeMap.ConstructDestinationUsingServiceLocator)
          return context.Options.ServiceCtor(type);
        if (typeMap.ConstructorMap != null)
          return typeMap.ConstructorMap.ResolveValue(context, (IMappingEngineRunner) this);
      }
      if (context.DestinationValue != null)
        return context.DestinationValue;
      if (type.IsInterface)
        type = MappingEngine.ProxyGeneratorFactory.Create().GetProxyType(type);
      return ObjectCreator.CreateObject(type);
    }

    bool IMappingEngineRunner.ShouldMapSourceValueAsNull(ResolutionContext context)
    {
      if (context.DestinationType.IsValueType && !context.DestinationType.IsNullableType())
        return false;
      TypeMap contextTypeMap = context.GetContextTypeMap();
      return contextTypeMap != null ? this.ConfigurationProvider.GetProfileConfiguration(contextTypeMap.Profile).MapNullSourceValuesAsNull : this.ConfigurationProvider.MapNullSourceValuesAsNull;
    }

    bool IMappingEngineRunner.ShouldMapSourceCollectionAsNull(ResolutionContext context)
    {
      TypeMap contextTypeMap = context.GetContextTypeMap();
      return contextTypeMap != null ? this.ConfigurationProvider.GetProfileConfiguration(contextTypeMap.Profile).MapNullSourceCollectionsAsNull : this.ConfigurationProvider.MapNullSourceCollectionsAsNull;
    }

    private void ClearTypeMap(object sender, TypeMapCreatedEventArgs e)
    {
      this._objectMapperCache.TryRemove(new TypePair(e.TypeMap.SourceType, e.TypeMap.DestinationType), out IObjectMapper _);
    }

    private void DefaultMappingOptions(IMappingOperationOptions opts)
    {
      opts.ConstructServicesUsing(this._serviceCtor);
    }
  }
}
