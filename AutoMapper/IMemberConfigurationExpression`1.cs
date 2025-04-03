// Decompiled with JetBrains decompiler
// Type: AutoMapper.IMemberConfigurationExpression`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace AutoMapper
{
  public interface IMemberConfigurationExpression<TSource>
  {
    [Obsolete("Formatters should not be used.")]
    void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter;

    [Obsolete("Formatters should not be used.")]
    IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter;

    [Obsolete("Formatters should not be used.")]
    IFormatterCtorExpression AddFormatter(Type valueFormatterType);

    [Obsolete("Formatters should not be used.")]
    void AddFormatter(IValueFormatter formatter);

    void NullSubstitute(object nullSubstitute);

    IResolverConfigurationExpression<TSource, TValueResolver> ResolveUsing<TValueResolver>() where TValueResolver : IValueResolver;

    IResolverConfigurationExpression<TSource> ResolveUsing(Type valueResolverType);

    IResolutionExpression<TSource> ResolveUsing(IValueResolver valueResolver);

    void ResolveUsing(Func<TSource, object> resolver);

    void MapFrom<TMember>(Expression<Func<TSource, TMember>> sourceMember);

    void Ignore();

    void SetMappingOrder(int mappingOrder);

    void UseDestinationValue();

    void DoNotUseDestinationValue();

    void UseValue<TValue>(TValue value);

    void UseValue(object value);

    void Condition(Func<TSource, bool> condition);

    void Condition(Func<ResolutionContext, bool> condition);
  }
}
