// Decompiled with JetBrains decompiler
// Type: AutoMapper.IMappingExpression`2
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace AutoMapper
{
  public interface IMappingExpression<TSource, TDestination>
  {
    IMappingExpression<TSource, TDestination> ForMember(
      Expression<Func<TDestination, object>> destinationMember,
      Action<IMemberConfigurationExpression<TSource>> memberOptions);

    IMappingExpression<TSource, TDestination> ForMember(
      string name,
      Action<IMemberConfigurationExpression<TSource>> memberOptions);

    void ForAllMembers(
      Action<IMemberConfigurationExpression<TSource>> memberOptions);

    IMappingExpression<TSource, TDestination> Include<TOtherSource, TOtherDestination>()
      where TOtherSource : TSource
      where TOtherDestination : TDestination;

    IMappingExpression<TSource, TDestination> WithProfile(string profileName);

    void ConvertUsing(Func<TSource, TDestination> mappingFunction);

    void ConvertUsing(ITypeConverter<TSource, TDestination> converter);

    void ConvertUsing<TTypeConverter>() where TTypeConverter : ITypeConverter<TSource, TDestination>;

    IMappingExpression<TSource, TDestination> BeforeMap(Action<TSource, TDestination> beforeFunction);

    IMappingExpression<TSource, TDestination> BeforeMap<TMappingAction>() where TMappingAction : IMappingAction<TSource, TDestination>;

    IMappingExpression<TSource, TDestination> AfterMap(Action<TSource, TDestination> afterFunction);

    IMappingExpression<TSource, TDestination> AfterMap<TMappingAction>() where TMappingAction : IMappingAction<TSource, TDestination>;

    IMappingExpression<TSource, TDestination> ConstructUsing(Func<TSource, TDestination> ctor);

    IMappingExpression<TSource, TDestination> ConstructUsing(
      Func<ResolutionContext, TDestination> ctor);

    void As<T>();

    IMappingExpression<TSource, TDestination> MaxDepth(int depth);

    IMappingExpression<TSource, TDestination> ConstructUsingServiceLocator();

    IMappingExpression<TDestination, TSource> ReverseMap();

    IMappingExpression<TSource, TDestination> ForSourceMember(
      Expression<Func<TSource, object>> sourceMember,
      Action<ISourceMemberConfigurationExpression<TSource>> memberOptions);

    IMappingExpression<TSource, TDestination> ForSourceMember(
      string sourceMemberName,
      Action<ISourceMemberConfigurationExpression<TSource>> memberOptions);
  }
}
