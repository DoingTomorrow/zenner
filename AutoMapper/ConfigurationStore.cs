// Decompiled with JetBrains decompiler
// Type: AutoMapper.ConfigurationStore
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Impl;
using AutoMapper.Internal;
using AutoMapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class ConfigurationStore : 
    IConfigurationProvider,
    IProfileConfiguration,
    IConfiguration,
    IProfileExpression,
    IFormatterExpression,
    IMappingOptions
  {
    internal const string DefaultProfileName = "";
    private static readonly IDictionaryFactory DictionaryFactory = PlatformAdapter.Resolve<IDictionaryFactory>();
    private readonly ITypeMapFactory _typeMapFactory;
    private readonly IEnumerable<IObjectMapper> _mappers;
    private readonly ThreadSafeList<TypeMap> _typeMaps = new ThreadSafeList<TypeMap>();
    private readonly AutoMapper.Internal.IDictionary<TypePair, TypeMap> _typeMapCache = ConfigurationStore.DictionaryFactory.CreateDictionary<TypePair, TypeMap>();
    private readonly AutoMapper.Internal.IDictionary<string, FormatterExpression> _formatterProfiles = ConfigurationStore.DictionaryFactory.CreateDictionary<string, FormatterExpression>();
    private Func<Type, object> _serviceCtor = new Func<Type, object>(ObjectCreator.CreateObject);
    private readonly List<string> _globalIgnore;

    public ConfigurationStore(ITypeMapFactory typeMapFactory, IEnumerable<IObjectMapper> mappers)
    {
      this._typeMapFactory = typeMapFactory;
      this._mappers = mappers;
      this._globalIgnore = new List<string>();
    }

    public event EventHandler<TypeMapCreatedEventArgs> TypeMapCreated;

    public Func<Type, object> ServiceCtor => this._serviceCtor;

    public bool AllowNullDestinationValues
    {
      get => this.GetProfile("").AllowNullDestinationValues;
      set => this.GetProfile("").AllowNullDestinationValues = value;
    }

    public bool AllowNullCollections
    {
      get => this.GetProfile("").AllowNullCollections;
      set => this.GetProfile("").AllowNullCollections = value;
    }

    public void IncludeSourceExtensionMethods(Assembly assembly)
    {
      this.GetProfile("").IncludeSourceExtensionMethods(assembly);
    }

    public INamingConvention SourceMemberNamingConvention
    {
      get => this.GetProfile("").SourceMemberNamingConvention;
      set => this.GetProfile("").SourceMemberNamingConvention = value;
    }

    public INamingConvention DestinationMemberNamingConvention
    {
      get => this.GetProfile("").DestinationMemberNamingConvention;
      set => this.GetProfile("").DestinationMemberNamingConvention = value;
    }

    public IEnumerable<string> Prefixes => this.GetProfile("").Prefixes;

    public IEnumerable<string> Postfixes => this.GetProfile("").Postfixes;

    public IEnumerable<string> DestinationPrefixes => this.GetProfile("").DestinationPrefixes;

    public IEnumerable<string> DestinationPostfixes => this.GetProfile("").DestinationPostfixes;

    public IEnumerable<AliasedMember> Aliases => this.GetProfile("").Aliases;

    public bool ConstructorMappingEnabled => this.GetProfile("").ConstructorMappingEnabled;

    public bool DataReaderMapperYieldReturnEnabled
    {
      get => this.GetProfile("").DataReaderMapperYieldReturnEnabled;
    }

    public IEnumerable<MethodInfo> SourceExtensionMethods
    {
      get => this.GetProfile("").SourceExtensionMethods;
    }

    bool IProfileConfiguration.MapNullSourceValuesAsNull => this.AllowNullDestinationValues;

    bool IProfileConfiguration.MapNullSourceCollectionsAsNull => this.AllowNullCollections;

    public IProfileExpression CreateProfile(string profileName)
    {
      Profile profile = new Profile(profileName);
      profile.Initialize(this);
      return (IProfileExpression) profile;
    }

    public void CreateProfile(string profileName, Action<IProfileExpression> profileConfiguration)
    {
      Profile profile = new Profile(profileName);
      profile.Initialize(this);
      profileConfiguration((IProfileExpression) profile);
    }

    public void AddProfile(Profile profile)
    {
      profile.Initialize(this);
      profile.Configure();
    }

    public void AddProfile<TProfile>() where TProfile : Profile, new()
    {
      this.AddProfile((Profile) new TProfile());
    }

    public void ConstructServicesUsing(Func<Type, object> constructor)
    {
      this._serviceCtor = constructor;
    }

    public void DisableConstructorMapping()
    {
      this.GetProfile("").ConstructorMappingEnabled = false;
    }

    public void EnableYieldReturnForDataReaderMapper()
    {
      this.GetProfile("").DataReaderMapperYieldReturnEnabled = true;
    }

    public void Seal()
    {
      this._typeMaps.Each<TypeMap>((Action<TypeMap>) (typeMap => typeMap.Seal()));
    }

    public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
      return this.CreateMap<TSource, TDestination>("");
    }

    public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(
      MemberList memberList)
    {
      return this.CreateMap<TSource, TDestination>("", memberList);
    }

    public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(
      string profileName)
    {
      return this.CreateMap<TSource, TDestination>(profileName, MemberList.Destination);
    }

    public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(
      string profileName,
      MemberList memberList)
    {
      return this.CreateMappingExpression<TSource, TDestination>(this.CreateTypeMap(typeof (TSource), typeof (TDestination), profileName, memberList));
    }

    public IMappingExpression CreateMap(Type sourceType, Type destinationType)
    {
      return this.CreateMap(sourceType, destinationType, MemberList.Destination);
    }

    public IMappingExpression CreateMap(
      Type sourceType,
      Type destinationType,
      MemberList memberList)
    {
      return this.CreateMap(sourceType, destinationType, memberList, "");
    }

    public IMappingExpression CreateMap(
      Type sourceType,
      Type destinationType,
      MemberList memberList,
      string profileName)
    {
      return this.CreateMappingExpression(this.CreateTypeMap(sourceType, destinationType, profileName, memberList), destinationType);
    }

    public void RecognizePrefixes(params string[] prefixes)
    {
      this.GetProfile("").RecognizePrefixes(prefixes);
    }

    public void RecognizePostfixes(params string[] postfixes)
    {
      this.GetProfile("").RecognizePostfixes(postfixes);
    }

    public void RecognizeAlias(string original, string alias)
    {
      this.GetProfile("").RecognizeAlias(original, alias);
    }

    public void RecognizeDestinationPrefixes(params string[] prefixes)
    {
      this.GetProfile("").RecognizeDestinationPrefixes(prefixes);
    }

    public void RecognizeDestinationPostfixes(params string[] postfixes)
    {
      this.GetProfile("").RecognizeDestinationPostfixes(postfixes);
    }

    public TypeMap CreateTypeMap(Type source, Type destination)
    {
      return this.CreateTypeMap(source, destination, "", MemberList.Destination);
    }

    public TypeMap CreateTypeMap(
      Type source,
      Type destination,
      string profileName,
      MemberList memberList)
    {
      TypeMap typeMap = this.FindExplicitlyDefinedTypeMap(source, destination);
      if (typeMap == null)
      {
        FormatterExpression profile = this.GetProfile(profileName);
        typeMap = this._typeMapFactory.CreateTypeMap(source, destination, (IMappingOptions) profile, memberList);
        typeMap.Profile = profileName;
        typeMap.IgnorePropertiesStartingWith = this._globalIgnore;
        this.IncludeBaseMappings(source, destination, typeMap);
        this._typeMaps.Add(typeMap);
        this._typeMapCache.AddOrUpdate(new TypePair(source, destination), typeMap, (Func<TypePair, TypeMap, TypeMap>) ((tp, tm) => typeMap));
        this.OnTypeMapCreated(typeMap);
      }
      return typeMap;
    }

    private void IncludeBaseMappings(Type source, Type destination, TypeMap typeMap)
    {
      foreach (TypeMap typeMap1 in this._typeMaps.Where<TypeMap>((Func<TypeMap, bool>) (t => t.TypeHasBeenIncluded(source, destination))))
      {
        foreach (PropertyMap propertyMap1 in typeMap1.GetPropertyMaps().Where<PropertyMap>((Func<PropertyMap, bool>) (m => m.IsMapped())))
        {
          PropertyMap inheritedMappedProperty = propertyMap1;
          PropertyMap propertyMap2 = typeMap.GetPropertyMaps().SingleOrDefault<PropertyMap>((Func<PropertyMap, bool>) (m => m.DestinationProperty.Name == inheritedMappedProperty.DestinationProperty.Name));
          if (propertyMap2 != null && inheritedMappedProperty.HasCustomValueResolver)
            propertyMap2.AssignCustomValueResolver(inheritedMappedProperty.GetSourceValueResolvers().First<IValueResolver>());
          else if (propertyMap2 == null)
          {
            PropertyMap mappedProperty = new PropertyMap(inheritedMappedProperty);
            typeMap.AddInheritedPropertyMap(mappedProperty);
          }
        }
        if (typeMap1.BeforeMap != null)
          typeMap.AddBeforeMapAction(typeMap1.BeforeMap);
        if (typeMap1.AfterMap != null)
          typeMap.AddAfterMapAction(typeMap1.AfterMap);
      }
    }

    public IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      return this.GetProfile("").AddFormatter<TValueFormatter>();
    }

    public IFormatterCtorExpression AddFormatter(Type valueFormatterType)
    {
      return this.GetProfile("").AddFormatter(valueFormatterType);
    }

    public void AddFormatter(IValueFormatter formatter)
    {
      this.GetProfile("").AddFormatter(formatter);
    }

    public void AddFormatExpression(Func<ResolutionContext, string> formatExpression)
    {
      this.GetProfile("").AddFormatExpression(formatExpression);
    }

    public void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      this.GetProfile("").SkipFormatter<TValueFormatter>();
    }

    public IFormatterExpression ForSourceType<TSource>()
    {
      return this.GetProfile("").ForSourceType<TSource>();
    }

    public TypeMap[] GetAllTypeMaps() => this._typeMaps.ToArray<TypeMap>();

    public TypeMap FindTypeMapFor(Type sourceType, Type destinationType)
    {
      return this.FindTypeMapFor((object) null, (object) null, sourceType, destinationType);
    }

    public TypeMap FindTypeMapFor(
      object source,
      object destination,
      Type sourceType,
      Type destinationType)
    {
      TypePair key = new TypePair(sourceType, destinationType);
      TypeMap typeMapFor;
      if (!this._typeMapCache.TryGetValue(key, out typeMapFor))
      {
        typeMapFor = this.FindTypeMap(source, destination, sourceType, destinationType, "");
        if (source == null || (object) source.GetType() == (object) sourceType)
          this._typeMapCache[key] = typeMapFor;
      }
      else if (source != null && typeMapFor != null && !typeMapFor.SourceType.IsAssignableFrom(source.GetType()))
        typeMapFor = this.FindTypeMapFor(source, destination, source.GetType(), destinationType);
      if (typeMapFor == null && destination != null && (object) destination.GetType() != (object) destinationType)
        typeMapFor = this.FindTypeMapFor(source, destination, sourceType, destination.GetType());
      if (typeMapFor != null && (object) typeMapFor.DestinationTypeOverride != null)
        return this.FindTypeMapFor(source, destination, sourceType, typeMapFor.DestinationTypeOverride);
      if (typeMapFor != null && typeMapFor.HasDerivedTypesToInclude() && source != null && (object) source.GetType() != (object) sourceType)
      {
        Type potentialSourceType = source.GetType();
        IEnumerable<TypeMap> source1 = this._typeMaps.Where<TypeMap>((Func<TypeMap, bool>) (t =>
        {
          if (!destinationType.IsAssignableFrom(t.DestinationType) || !t.SourceType.IsAssignableFrom(source.GetType()))
            return false;
          return destinationType.IsAssignableFrom(t.DestinationType) || (object) t.GetDerivedTypeFor(potentialSourceType) != null;
        }));
        TypeMap potentialDestTypeMap = source1.OrderByDescending<TypeMap, int>((Func<TypeMap, int>) (t => ConfigurationStore.GetInheritanceDepth(t.DestinationType))).FirstOrDefault<TypeMap>();
        List<TypeMap> list = source1.Where<TypeMap>((Func<TypeMap, bool>) (t => (object) t.DestinationType == (object) potentialDestTypeMap.DestinationType)).ToList<TypeMap>();
        if (list.Count > 1)
          potentialDestTypeMap = list.OrderByDescending<TypeMap, int>((Func<TypeMap, int>) (t => ConfigurationStore.GetInheritanceDepth(t.SourceType))).FirstOrDefault<TypeMap>();
        if (potentialDestTypeMap == typeMapFor)
          return typeMapFor;
        Type destinationType1 = potentialDestTypeMap.DestinationType;
        TypeMap explicitlyDefinedTypeMap = this.FindExplicitlyDefinedTypeMap(potentialSourceType, destinationType1);
        if (explicitlyDefinedTypeMap == null)
        {
          Type sourceType1 = (object) destinationType1 != (object) destinationType ? potentialSourceType : typeMapFor.SourceType;
          typeMapFor = this.FindTypeMap(source, destination, sourceType1, destinationType1, "");
        }
        else
          typeMapFor = explicitlyDefinedTypeMap;
      }
      return typeMapFor;
    }

    private static int GetInheritanceDepth(Type type)
    {
      return (object) type != null ? ConfigurationStore.InheritanceTree(type).Count<Type>() : throw new ArgumentNullException(nameof (type));
    }

    private static IEnumerable<Type> InheritanceTree(Type type)
    {
      for (; (object) type != null; type = type.BaseType)
        yield return type;
    }

    public TypeMap FindTypeMapFor(ResolutionResult resolutionResult, Type destinationType)
    {
      return this.FindTypeMapFor(resolutionResult.Value, (object) null, resolutionResult.Type, destinationType) ?? this.FindTypeMapFor(resolutionResult.Value, (object) null, resolutionResult.MemberType, destinationType);
    }

    public IFormatterConfiguration GetProfileConfiguration(string profileName)
    {
      return (IFormatterConfiguration) this.GetProfile(profileName);
    }

    public void AssertConfigurationIsValid(TypeMap typeMap)
    {
      this.AssertConfigurationIsValid(Enumerable.Repeat<TypeMap>(typeMap, 1));
    }

    public void AssertConfigurationIsValid(string profileName)
    {
      this.AssertConfigurationIsValid(this._typeMaps.Where<TypeMap>((Func<TypeMap, bool>) (typeMap => typeMap.Profile == profileName)));
    }

    public void AssertConfigurationIsValid()
    {
      this.AssertConfigurationIsValid((IEnumerable<TypeMap>) this._typeMaps);
    }

    public IObjectMapper[] GetMappers() => this._mappers.ToArray<IObjectMapper>();

    private IMappingExpression<TSource, TDestination> CreateMappingExpression<TSource, TDestination>(
      TypeMap typeMap)
    {
      IMappingExpression<TSource, TDestination> mappingExpression = (IMappingExpression<TSource, TDestination>) new MappingExpression<TSource, TDestination>(typeMap, this._serviceCtor, (IProfileExpression) this);
      foreach (MemberInfo publicWriteAccessor in new TypeInfo(typeof (TDestination)).GetPublicWriteAccessors())
      {
        if (((IEnumerable<object>) publicWriteAccessor.GetCustomAttributes(true)).Any<object>((Func<object, bool>) (x => x is IgnoreMapAttribute)))
          mappingExpression = mappingExpression.ForMember(publicWriteAccessor.Name, (Action<IMemberConfigurationExpression<TSource>>) (y => y.Ignore()));
      }
      return mappingExpression;
    }

    private IMappingExpression CreateMappingExpression(TypeMap typeMap, Type destinationType)
    {
      IMappingExpression mappingExpression = (IMappingExpression) new MappingExpression(typeMap, this._serviceCtor);
      foreach (MemberInfo publicWriteAccessor in new TypeInfo(destinationType).GetPublicWriteAccessors())
      {
        if (((IEnumerable<object>) publicWriteAccessor.GetCustomAttributes(true)).Any<object>((Func<object, bool>) (x => x is IgnoreMapAttribute)))
          mappingExpression = mappingExpression.ForMember(publicWriteAccessor.Name, (Action<IMemberConfigurationExpression>) (y => y.Ignore()));
      }
      return mappingExpression;
    }

    private void AssertConfigurationIsValid(IEnumerable<TypeMap> typeMaps)
    {
      AutoMapperConfigurationException.TypeMapConfigErrors[] array = typeMaps.Where<TypeMap>((Func<TypeMap, bool>) (typeMap => ConfigurationStore.ShouldCheckMap(typeMap))).Select(typeMap => new
      {
        typeMap = typeMap,
        unmappedPropertyNames = typeMap.GetUnmappedPropertyNames()
      }).Where(_param0 => _param0.unmappedPropertyNames.Length > 0).Select(_param0 => new AutoMapperConfigurationException.TypeMapConfigErrors(_param0.typeMap, _param0.unmappedPropertyNames)).ToArray<AutoMapperConfigurationException.TypeMapConfigErrors>();
      if (((IEnumerable<AutoMapperConfigurationException.TypeMapConfigErrors>) array).Any<AutoMapperConfigurationException.TypeMapConfigErrors>())
        throw new AutoMapperConfigurationException(array);
      List<TypeMap> typeMapsChecked = new List<TypeMap>();
      foreach (TypeMap typeMap in (IEnumerable<TypeMap>) this._typeMaps)
        this.DryRunTypeMap((ICollection<TypeMap>) typeMapsChecked, new ResolutionContext(typeMap, (object) null, typeMap.SourceType, typeMap.DestinationType, new MappingOperationOptions()));
    }

    private static bool ShouldCheckMap(TypeMap typeMap)
    {
      return typeMap.CustomMapper == null && !FeatureDetector.IsIDataRecordType(typeMap.SourceType);
    }

    private TypeMap FindTypeMap(
      object source,
      object destination,
      Type sourceType,
      Type destinationType,
      string profileName)
    {
      TypeMap typeMap = this.FindExplicitlyDefinedTypeMap(sourceType, destinationType);
      if (typeMap == null && destinationType.IsNullableType())
        typeMap = this.FindExplicitlyDefinedTypeMap(sourceType, destinationType.GetTypeOfNullable());
      if (typeMap == null)
      {
        typeMap = this._typeMaps.FirstOrDefault<TypeMap>((Func<TypeMap, bool>) (x => (object) x.SourceType == (object) sourceType && (object) x.GetDerivedTypeFor(sourceType) == (object) destinationType));
        if (typeMap == null)
        {
          foreach (Type sourceType1 in sourceType.GetInterfaces())
          {
            typeMap = this.FindTypeMapFor(source, destination, sourceType1, destinationType);
            if (typeMap != null)
            {
              Type derivedTypeFor = typeMap.GetDerivedTypeFor(sourceType);
              if ((object) derivedTypeFor != (object) destinationType)
              {
                typeMap = this.CreateTypeMap(sourceType, derivedTypeFor, profileName, typeMap.ConfiguredMemberList);
                break;
              }
              break;
            }
          }
          if ((object) sourceType.BaseType != null && typeMap == null)
            typeMap = this.FindTypeMapFor(source, destination, sourceType.BaseType, destinationType);
        }
      }
      return typeMap;
    }

    private TypeMap FindExplicitlyDefinedTypeMap(Type sourceType, Type destinationType)
    {
      return this._typeMaps.FirstOrDefault<TypeMap>((Func<TypeMap, bool>) (x => (object) x.DestinationType == (object) destinationType && (object) x.SourceType == (object) sourceType));
    }

    private void DryRunTypeMap(ICollection<TypeMap> typeMapsChecked, ResolutionContext context)
    {
      if (context.TypeMap != null)
        typeMapsChecked.Add(context.TypeMap);
      IObjectMapper objectMapper = ((IEnumerable<IObjectMapper>) this.GetMappers()).FirstOrDefault<IObjectMapper>((Func<IObjectMapper, bool>) (mapper => mapper.IsMatch(context)));
      if (objectMapper == null && context.SourceType.IsNullableType())
      {
        ResolutionContext nullableContext = context.CreateValueContext((object) null, Nullable.GetUnderlyingType(context.SourceType));
        objectMapper = ((IEnumerable<IObjectMapper>) this.GetMappers()).FirstOrDefault<IObjectMapper>((Func<IObjectMapper, bool>) (mapper => mapper.IsMatch(nullableContext)));
      }
      switch (objectMapper)
      {
        case null:
          throw new AutoMapperConfigurationException(context);
        case TypeMapMapper _:
          using (IEnumerator<PropertyMap> enumerator = context.TypeMap.GetPropertyMaps().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              PropertyMap current = enumerator.Current;
              if (!current.IsIgnored())
              {
                IMemberResolver memberResolver = current.GetSourceValueResolvers().OfType<IMemberResolver>().LastOrDefault<IMemberResolver>();
                if (memberResolver != null)
                {
                  Type memberType1 = memberResolver.MemberType;
                  Type memberType2 = current.DestinationProperty.MemberType;
                  TypeMap memberTypeMap = this.FindTypeMapFor(memberType1, memberType2);
                  if (!typeMapsChecked.Any<TypeMap>((Func<TypeMap, bool>) (typeMap => object.Equals((object) typeMap, (object) memberTypeMap))))
                  {
                    ResolutionContext memberContext = context.CreateMemberContext(memberTypeMap, (object) null, (object) null, memberType1, current);
                    this.DryRunTypeMap(typeMapsChecked, memberContext);
                  }
                }
              }
            }
            break;
          }
        case ArrayMapper _:
        case EnumerableMapper _:
        case CollectionMapper _:
          Type elementType1 = AutoMapper.Mappers.TypeHelper.GetElementType(context.SourceType);
          Type elementType2 = AutoMapper.Mappers.TypeHelper.GetElementType(context.DestinationType);
          TypeMap itemTypeMap = this.FindTypeMapFor(elementType1, elementType2);
          if (typeMapsChecked.Any<TypeMap>((Func<TypeMap, bool>) (typeMap => object.Equals((object) typeMap, (object) itemTypeMap))))
            break;
          ResolutionContext elementContext = context.CreateElementContext(itemTypeMap, (object) null, elementType1, elementType2, 0);
          this.DryRunTypeMap(typeMapsChecked, elementContext);
          break;
      }
    }

    protected void OnTypeMapCreated(TypeMap typeMap)
    {
      EventHandler<TypeMapCreatedEventArgs> typeMapCreated = this.TypeMapCreated;
      if (typeMapCreated == null)
        return;
      typeMapCreated((object) this, new TypeMapCreatedEventArgs(typeMap));
    }

    internal FormatterExpression GetProfile(string profileName)
    {
      return this._formatterProfiles.GetOrAdd(profileName, (Func<string, FormatterExpression>) (name => new FormatterExpression((Func<Type, IValueFormatter>) (t => (IValueFormatter) this._serviceCtor(t)))));
    }

    public void AddGlobalIgnore(string startingwith) => this._globalIgnore.Add(startingwith);
  }
}
