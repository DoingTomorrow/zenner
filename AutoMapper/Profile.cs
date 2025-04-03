// Decompiled with JetBrains decompiler
// Type: AutoMapper.Profile
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class Profile : IProfileExpression, IFormatterExpression, IMappingOptions
  {
    private ConfigurationStore _configurator;

    internal Profile(string profileName) => this.ProfileName = profileName;

    protected Profile() => this.ProfileName = this.GetType().FullName;

    public virtual string ProfileName { get; private set; }

    public void DisableConstructorMapping() => this.GetProfile().ConstructorMappingEnabled = false;

    public bool AllowNullDestinationValues
    {
      get => this.GetProfile().AllowNullDestinationValues;
      set => this.GetProfile().AllowNullDestinationValues = value;
    }

    public bool AllowNullCollections
    {
      get => this.GetProfile().AllowNullCollections;
      set => this.GetProfile().AllowNullCollections = value;
    }

    public void IncludeSourceExtensionMethods(Assembly assembly)
    {
      this.GetProfile().IncludeSourceExtensionMethods(assembly);
    }

    public INamingConvention SourceMemberNamingConvention
    {
      get => this.GetProfile().SourceMemberNamingConvention;
      set => this.GetProfile().SourceMemberNamingConvention = value;
    }

    public INamingConvention DestinationMemberNamingConvention
    {
      get => this.GetProfile().DestinationMemberNamingConvention;
      set => this.GetProfile().DestinationMemberNamingConvention = value;
    }

    public IEnumerable<string> Prefixes => this.GetProfile().Prefixes;

    public IEnumerable<string> Postfixes => this.GetProfile().Postfixes;

    public IEnumerable<string> DestinationPrefixes => this.GetProfile().DestinationPrefixes;

    public IEnumerable<string> DestinationPostfixes => this.GetProfile().DestinationPostfixes;

    public IEnumerable<AliasedMember> Aliases => throw new NotImplementedException();

    public bool ConstructorMappingEnabled => this._configurator.ConstructorMappingEnabled;

    public bool DataReaderMapperYieldReturnEnabled
    {
      get => this._configurator.DataReaderMapperYieldReturnEnabled;
    }

    public IEnumerable<MethodInfo> SourceExtensionMethods
    {
      get => this.GetProfile().SourceExtensionMethods;
    }

    public IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      return this.GetProfile().AddFormatter<TValueFormatter>();
    }

    public IFormatterCtorExpression AddFormatter(Type valueFormatterType)
    {
      return this.GetProfile().AddFormatter(valueFormatterType);
    }

    public void AddFormatter(IValueFormatter formatter)
    {
      this.GetProfile().AddFormatter(formatter);
    }

    public void AddFormatExpression(Func<ResolutionContext, string> formatExpression)
    {
      this.GetProfile().AddFormatExpression(formatExpression);
    }

    public void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      this.GetProfile().SkipFormatter<TValueFormatter>();
    }

    public IFormatterExpression ForSourceType<TSource>()
    {
      return this.GetProfile().ForSourceType<TSource>();
    }

    public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
      return this.CreateMap<TSource, TDestination>(MemberList.Destination);
    }

    public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(
      MemberList memberList)
    {
      return this._configurator.CreateMap<TSource, TDestination>(this.ProfileName, memberList);
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
      return this._configurator.CreateMap(sourceType, destinationType, memberList, this.ProfileName);
    }

    public void RecognizeAlias(string original, string alias)
    {
      this.GetProfile().RecognizeAlias(original, alias);
    }

    public void RecognizePrefixes(params string[] prefixes)
    {
      this.GetProfile().RecognizePrefixes(prefixes);
    }

    public void RecognizePostfixes(params string[] postfixes)
    {
      this.GetProfile().RecognizePostfixes(postfixes);
    }

    public void RecognizeDestinationPrefixes(params string[] prefixes)
    {
      this.GetProfile().RecognizeDestinationPrefixes(prefixes);
    }

    public void RecognizeDestinationPostfixes(params string[] postfixes)
    {
      this.GetProfile().RecognizeDestinationPostfixes(postfixes);
    }

    public void AddGlobalIgnore(string propertyNameStartingWith)
    {
      this._configurator.AddGlobalIgnore(propertyNameStartingWith);
    }

    protected internal virtual void Configure()
    {
    }

    public void Initialize(ConfigurationStore configurator) => this._configurator = configurator;

    private FormatterExpression GetProfile() => this._configurator.GetProfile(this.ProfileName);
  }
}
