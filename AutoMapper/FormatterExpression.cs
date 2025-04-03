// Decompiled with JetBrains decompiler
// Type: AutoMapper.FormatterExpression
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace AutoMapper
{
  public class FormatterExpression : 
    IFormatterExpression,
    IFormatterConfiguration,
    IProfileConfiguration,
    IFormatterCtorConfigurator,
    IMappingOptions
  {
    private readonly Func<Type, IValueFormatter> _formatterCtor;
    private readonly IList<IValueFormatter> _formatters = (IList<IValueFormatter>) new List<IValueFormatter>();
    private readonly IDictionary<Type, IFormatterConfiguration> _typeSpecificFormatters = (IDictionary<Type, IFormatterConfiguration>) new Dictionary<Type, IFormatterConfiguration>();
    private readonly IList<Type> _formattersToSkip = (IList<Type>) new List<Type>();
    private readonly ISet<string> _prefixes = (ISet<string>) new HashSet<string>();
    private readonly ISet<string> _postfixes = (ISet<string>) new HashSet<string>();
    private readonly ISet<string> _destinationPrefixes = (ISet<string>) new HashSet<string>();
    private readonly ISet<string> _destinationPostfixes = (ISet<string>) new HashSet<string>();
    private readonly ISet<AliasedMember> _aliases = (ISet<AliasedMember>) new HashSet<AliasedMember>();
    private readonly List<MethodInfo> _sourceExtensionMethods = new List<MethodInfo>();

    public FormatterExpression(Func<Type, IValueFormatter> formatterCtor)
    {
      this._formatterCtor = formatterCtor;
      this.SourceMemberNamingConvention = (INamingConvention) new PascalCaseNamingConvention();
      this.DestinationMemberNamingConvention = (INamingConvention) new PascalCaseNamingConvention();
      this.RecognizePrefixes("Get");
      this.AllowNullDestinationValues = true;
      this.ConstructorMappingEnabled = true;
      this.IncludeSourceExtensionMethods(typeof (Enumerable).Assembly);
    }

    public bool AllowNullDestinationValues { get; set; }

    public bool AllowNullCollections { get; set; }

    public INamingConvention SourceMemberNamingConvention { get; set; }

    public INamingConvention DestinationMemberNamingConvention { get; set; }

    public IEnumerable<string> Prefixes => (IEnumerable<string>) this._prefixes;

    public IEnumerable<string> Postfixes => (IEnumerable<string>) this._postfixes;

    public IEnumerable<string> DestinationPrefixes
    {
      get => (IEnumerable<string>) this._destinationPrefixes;
    }

    public IEnumerable<string> DestinationPostfixes
    {
      get => (IEnumerable<string>) this._destinationPostfixes;
    }

    public IEnumerable<AliasedMember> Aliases => (IEnumerable<AliasedMember>) this._aliases;

    public bool ConstructorMappingEnabled { get; set; }

    public bool DataReaderMapperYieldReturnEnabled { get; set; }

    public IEnumerable<MethodInfo> SourceExtensionMethods
    {
      get => (IEnumerable<MethodInfo>) this._sourceExtensionMethods;
    }

    public IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      this.AddFormatter((IValueFormatter) new DeferredInstantiatedFormatter(this.BuildCtor(typeof (TValueFormatter))));
      return (IFormatterCtorExpression<TValueFormatter>) new FormatterCtorExpression<TValueFormatter>((IFormatterCtorConfigurator) this);
    }

    public IFormatterCtorExpression AddFormatter(Type valueFormatterType)
    {
      this.AddFormatter((IValueFormatter) new DeferredInstantiatedFormatter(this.BuildCtor(valueFormatterType)));
      return (IFormatterCtorExpression) new FormatterCtorExpression(valueFormatterType, (IFormatterCtorConfigurator) this);
    }

    public void AddFormatter(IValueFormatter valueFormatter)
    {
      this._formatters.Add(valueFormatter);
    }

    public void AddFormatExpression(Func<ResolutionContext, string> formatExpression)
    {
      this._formatters.Add((IValueFormatter) new ExpressionValueFormatter(formatExpression));
    }

    public void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      this._formattersToSkip.Add(typeof (TValueFormatter));
    }

    public IFormatterExpression ForSourceType<TSource>()
    {
      FormatterExpression formatterExpression = new FormatterExpression(this._formatterCtor);
      this._typeSpecificFormatters[typeof (TSource)] = (IFormatterConfiguration) formatterExpression;
      return (IFormatterExpression) formatterExpression;
    }

    public IValueFormatter[] GetFormatters() => this._formatters.ToArray<IValueFormatter>();

    public IDictionary<Type, IFormatterConfiguration> GetTypeSpecificFormatters()
    {
      return (IDictionary<Type, IFormatterConfiguration>) new Dictionary<Type, IFormatterConfiguration>(this._typeSpecificFormatters);
    }

    public Type[] GetFormatterTypesToSkip() => this._formattersToSkip.ToArray<Type>();

    public IEnumerable<IValueFormatter> GetFormattersToApply(ResolutionContext context)
    {
      return this.GetFormatters(context);
    }

    private IEnumerable<IValueFormatter> GetFormatters(ResolutionContext context)
    {
      Type valueType = context.SourceType;
      IFormatterConfiguration typeSpecificFormatterConfig;
      if (context.PropertyMap != null)
      {
        foreach (IValueFormatter formatter in context.PropertyMap.GetFormatters())
          yield return formatter;
        if (this.GetTypeSpecificFormatters().TryGetValue(valueType, out typeSpecificFormatterConfig) && !context.PropertyMap.FormattersToSkipContains(typeSpecificFormatterConfig.GetType()))
        {
          foreach (IValueFormatter typeSpecificFormatter in typeSpecificFormatterConfig.GetFormattersToApply(context))
            yield return typeSpecificFormatter;
        }
      }
      else if (this.GetTypeSpecificFormatters().TryGetValue(valueType, out typeSpecificFormatterConfig))
      {
        foreach (IValueFormatter typeSpecificFormatter in typeSpecificFormatterConfig.GetFormattersToApply(context))
          yield return typeSpecificFormatter;
      }
      foreach (IValueFormatter formatter in this.GetFormatters())
      {
        Type formatterType = FormatterExpression.GetFormatterType(formatter, context);
        if (FormatterExpression.CheckPropertyMapSkipList(context, formatterType) && FormatterExpression.CheckTypeSpecificSkipList(typeSpecificFormatterConfig, formatterType))
          yield return formatter;
      }
    }

    public void ConstructFormatterBy(Type formatterType, Func<IValueFormatter> instantiator)
    {
      this._formatters.RemoveAt(this._formatters.Count - 1);
      this._formatters.Add((IValueFormatter) new DeferredInstantiatedFormatter((Func<ResolutionContext, IValueFormatter>) (ctxt => instantiator())));
    }

    public bool MapNullSourceValuesAsNull => this.AllowNullDestinationValues;

    public bool MapNullSourceCollectionsAsNull => this.AllowNullCollections;

    public void IncludeSourceExtensionMethods(Assembly assembly)
    {
      this._sourceExtensionMethods.AddRange(((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => type.IsSealed && !type.IsGenericType && !type.IsNested)).SelectMany<Type, MethodInfo>((Func<Type, IEnumerable<MethodInfo>>) (type => (IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.IsDefined(typeof (ExtensionAttribute), false))).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.GetParameters().Length == 1)));
    }

    public void RecognizePrefixes(params string[] prefixes)
    {
      foreach (string prefix in prefixes)
        this._prefixes.Add(prefix);
    }

    public void RecognizePostfixes(params string[] postfixes)
    {
      foreach (string postfix in postfixes)
        this._postfixes.Add(postfix);
    }

    public void RecognizeAlias(string original, string alias)
    {
      this._aliases.Add(new AliasedMember(original, alias));
    }

    public void RecognizeDestinationPrefixes(params string[] prefixes)
    {
      foreach (string prefix in prefixes)
        this._destinationPrefixes.Add(prefix);
    }

    public void RecognizeDestinationPostfixes(params string[] postfixes)
    {
      foreach (string postfix in postfixes)
        this._destinationPostfixes.Add(postfix);
    }

    private static Type GetFormatterType(IValueFormatter formatter, ResolutionContext context)
    {
      return !(formatter is DeferredInstantiatedFormatter) ? formatter.GetType() : ((DeferredInstantiatedFormatter) formatter).GetFormatterType(context);
    }

    private static bool CheckTypeSpecificSkipList(
      IFormatterConfiguration valueFormatter,
      Type formatterType)
    {
      return valueFormatter == null || !((IEnumerable<Type>) valueFormatter.GetFormatterTypesToSkip()).Contains<Type>(formatterType);
    }

    private static bool CheckPropertyMapSkipList(ResolutionContext context, Type formatterType)
    {
      return context.PropertyMap == null || !context.PropertyMap.FormattersToSkipContains(formatterType);
    }

    private Func<ResolutionContext, IValueFormatter> BuildCtor(Type type)
    {
      return (Func<ResolutionContext, IValueFormatter>) (context =>
      {
        if (context.Options.ServiceCtor != null)
        {
          object obj = context.Options.ServiceCtor(type);
          if (obj != null)
            return (IValueFormatter) obj;
        }
        return this._formatterCtor(type);
      });
    }

    private static string DefaultPrefixTransformer(string src, string prefix)
    {
      return src == null || string.IsNullOrEmpty(prefix) || !src.StartsWith(prefix, StringComparison.Ordinal) ? src : src.Substring(prefix.Length);
    }

    private static string DefaultPostfixTransformer(string src, string postfix)
    {
      return src == null || string.IsNullOrEmpty(postfix) || !src.EndsWith(postfix, StringComparison.Ordinal) ? src : src.Remove(src.Length - postfix.Length);
    }

    private static string DefaultAliasTransformer(string src, string original, string alias)
    {
      return src == null || string.IsNullOrEmpty(original) || !string.Equals(src, original, StringComparison.Ordinal) ? src : alias;
    }

    private static string DefaultSourceMemberNameTransformer(string src)
    {
      return src == null || !src.StartsWith("Get", StringComparison.Ordinal) ? src : src.Substring(3);
    }
  }
}
