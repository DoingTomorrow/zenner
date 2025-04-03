// Decompiled with JetBrains decompiler
// Type: AutoMapper.IProfileExpression
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public interface IProfileExpression : IFormatterExpression, IMappingOptions
  {
    IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>();

    IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>(MemberList memberList);

    IMappingExpression CreateMap(Type sourceType, Type destinationType);

    IMappingExpression CreateMap(Type sourceType, Type destinationType, MemberList memberList);

    void RecognizePrefixes(params string[] prefixes);

    void RecognizePostfixes(params string[] postfixes);

    void RecognizeAlias(string original, string alias);

    void RecognizeDestinationPrefixes(params string[] prefixes);

    void RecognizeDestinationPostfixes(params string[] postfixes);

    void AddGlobalIgnore(string propertyNameStartingWith);

    bool AllowNullDestinationValues { get; set; }

    bool AllowNullCollections { get; set; }

    void IncludeSourceExtensionMethods(Assembly assembly);
  }
}
