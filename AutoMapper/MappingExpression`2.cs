// Decompiled with JetBrains decompiler
// Type: AutoMapper.MappingExpression`2
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class MappingExpression<TSource, TDestination> : 
    IMappingExpression<TSource, TDestination>,
    IMemberConfigurationExpression<TSource>,
    IFormatterCtorConfigurator
  {
    private readonly TypeMap _typeMap;
    private readonly Func<Type, object> _serviceCtor;
    private readonly IProfileExpression _configurationContainer;
    private PropertyMap _propertyMap;

    public MappingExpression(
      TypeMap typeMap,
      Func<Type, object> serviceCtor,
      IProfileExpression configurationContainer)
    {
      this._typeMap = typeMap;
      this._serviceCtor = serviceCtor;
      this._configurationContainer = configurationContainer;
    }

    public IMappingExpression<TSource, TDestination> ForMember(
      Expression<Func<TDestination, object>> destinationMember,
      Action<IMemberConfigurationExpression<TSource>> memberOptions)
    {
      this.ForDestinationMember(ReflectionHelper.FindProperty((LambdaExpression) destinationMember).ToMemberAccessor(), memberOptions);
      return (IMappingExpression<TSource, TDestination>) new MappingExpression<TSource, TDestination>(this._typeMap, this._serviceCtor, this._configurationContainer);
    }

    public IMappingExpression<TSource, TDestination> ForMember(
      string name,
      Action<IMemberConfigurationExpression<TSource>> memberOptions)
    {
      IMemberAccessor destinationProperty = (IMemberAccessor) null;
      PropertyInfo property = this._typeMap.DestinationType.GetProperty(name);
      if ((object) property != null)
        destinationProperty = (IMemberAccessor) new PropertyAccessor(property);
      if (destinationProperty == null)
        destinationProperty = (IMemberAccessor) new FieldAccessor(this._typeMap.DestinationType.GetField(name));
      this.ForDestinationMember(destinationProperty, memberOptions);
      return (IMappingExpression<TSource, TDestination>) new MappingExpression<TSource, TDestination>(this._typeMap, this._serviceCtor, this._configurationContainer);
    }

    public void ForAllMembers(
      Action<IMemberConfigurationExpression<TSource>> memberOptions)
    {
      new TypeInfo(this._typeMap.DestinationType).GetPublicWriteAccessors().Each<MemberInfo>((Action<MemberInfo>) (acc => this.ForDestinationMember(acc.ToMemberAccessor(), memberOptions)));
    }

    public IMappingExpression<TSource, TDestination> Include<TOtherSource, TOtherDestination>()
      where TOtherSource : TSource
      where TOtherDestination : TDestination
    {
      this._typeMap.IncludeDerivedTypes(typeof (TOtherSource), typeof (TOtherDestination));
      return (IMappingExpression<TSource, TDestination>) this;
    }

    public IMappingExpression<TSource, TDestination> WithProfile(string profileName)
    {
      this._typeMap.Profile = profileName;
      return (IMappingExpression<TSource, TDestination>) this;
    }

    public void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      this._propertyMap.AddFormatterToSkip<TValueFormatter>();
    }

    public IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
    {
      this.AddFormatter((IValueFormatter) new DeferredInstantiatedFormatter(this.BuildCtor<IValueFormatter>(typeof (TValueFormatter))));
      return (IFormatterCtorExpression<TValueFormatter>) new FormatterCtorExpression<TValueFormatter>((IFormatterCtorConfigurator) this);
    }

    public IFormatterCtorExpression AddFormatter(Type valueFormatterType)
    {
      this.AddFormatter((IValueFormatter) new DeferredInstantiatedFormatter(this.BuildCtor<IValueFormatter>(valueFormatterType)));
      return (IFormatterCtorExpression) new FormatterCtorExpression(valueFormatterType, (IFormatterCtorConfigurator) this);
    }

    public void AddFormatter(IValueFormatter formatter)
    {
      this._propertyMap.AddFormatter(formatter);
    }

    public void NullSubstitute(object nullSubstitute)
    {
      this._propertyMap.SetNullSubstitute(nullSubstitute);
    }

    public IResolverConfigurationExpression<TSource, TValueResolver> ResolveUsing<TValueResolver>() where TValueResolver : IValueResolver
    {
      this.ResolveUsing((IValueResolver) new DeferredInstantiatedResolver(this.BuildCtor<IValueResolver>(typeof (TValueResolver))));
      return (IResolverConfigurationExpression<TSource, TValueResolver>) new ResolutionExpression<TSource, TValueResolver>(this._propertyMap);
    }

    public IResolverConfigurationExpression<TSource> ResolveUsing(Type valueResolverType)
    {
      this.ResolveUsing((IValueResolver) new DeferredInstantiatedResolver(this.BuildCtor<IValueResolver>(valueResolverType)));
      return (IResolverConfigurationExpression<TSource>) new ResolutionExpression<TSource>(this._propertyMap);
    }

    public IResolutionExpression<TSource> ResolveUsing(IValueResolver valueResolver)
    {
      this._propertyMap.AssignCustomValueResolver(valueResolver);
      return (IResolutionExpression<TSource>) new ResolutionExpression<TSource>(this._propertyMap);
    }

    public void ResolveUsing(Func<TSource, object> resolver)
    {
      this._propertyMap.AssignCustomValueResolver((IValueResolver) new DelegateBasedResolver<TSource>(resolver));
    }

    public void MapFrom<TMember>(Expression<Func<TSource, TMember>> sourceMember)
    {
      this._propertyMap.SetCustomValueResolverExpression<TSource, TMember>(sourceMember);
    }

    public void UseValue<TValue>(TValue value)
    {
      this.MapFrom<TValue>((Expression<Func<TSource, TValue>>) (src => value));
    }

    public void UseValue(object value)
    {
      this._propertyMap.AssignCustomValueResolver((IValueResolver) new DelegateBasedResolver<TSource>((Func<TSource, object>) (src => value)));
    }

    public void Condition(Func<TSource, bool> condition)
    {
      this.Condition((Func<ResolutionContext, bool>) (context => condition((TSource) context.Parent.SourceValue)));
    }

    public IMappingExpression<TSource, TDestination> MaxDepth(int depth)
    {
      this._typeMap.SetCondition((Func<ResolutionContext, bool>) (o => MappingExpression<TSource, TDestination>.PassesDepthCheck(o, depth)));
      return (IMappingExpression<TSource, TDestination>) this;
    }

    public IMappingExpression<TSource, TDestination> ConstructUsingServiceLocator()
    {
      this._typeMap.ConstructDestinationUsingServiceLocator = true;
      return (IMappingExpression<TSource, TDestination>) this;
    }

    public IMappingExpression<TDestination, TSource> ReverseMap()
    {
      return this._configurationContainer.CreateMap<TDestination, TSource>(MemberList.Source);
    }

    public IMappingExpression<TSource, TDestination> ForSourceMember(
      Expression<Func<TSource, object>> sourceMember,
      Action<ISourceMemberConfigurationExpression<TSource>> memberOptions)
    {
      MappingExpression<TSource, TDestination>.SourceMappingExpression mappingExpression = new MappingExpression<TSource, TDestination>.SourceMappingExpression(this._typeMap, ReflectionHelper.FindProperty((LambdaExpression) sourceMember));
      memberOptions((ISourceMemberConfigurationExpression<TSource>) mappingExpression);
      return (IMappingExpression<TSource, TDestination>) this;
    }

    public IMappingExpression<TSource, TDestination> ForSourceMember(
      string sourceMemberName,
      Action<ISourceMemberConfigurationExpression<TSource>> memberOptions)
    {
      MappingExpression<TSource, TDestination>.SourceMappingExpression mappingExpression = new MappingExpression<TSource, TDestination>.SourceMappingExpression(this._typeMap, ((IEnumerable<MemberInfo>) this._typeMap.SourceType.GetMember(sourceMemberName)).First<MemberInfo>());
      memberOptions((ISourceMemberConfigurationExpression<TSource>) mappingExpression);
      return (IMappingExpression<TSource, TDestination>) this;
    }

    private static bool PassesDepthCheck(ResolutionContext context, int maxDepth)
    {
      if (context.InstanceCache.ContainsKey(context))
        return true;
      ResolutionContext resolutionContext = context;
      int num = 1;
      for (; resolutionContext.Parent != null; resolutionContext = resolutionContext.Parent)
      {
        if ((object) resolutionContext.SourceType == (object) context.TypeMap.SourceType && (object) resolutionContext.DestinationType == (object) context.TypeMap.DestinationType)
          ++num;
      }
      return num <= maxDepth;
    }

    public void Condition(Func<ResolutionContext, bool> condition)
    {
      this._propertyMap.ApplyCondition(condition);
    }

    public void Ignore() => this._propertyMap.Ignore();

    public void UseDestinationValue() => this._propertyMap.UseDestinationValue = true;

    public void DoNotUseDestinationValue() => this._propertyMap.UseDestinationValue = false;

    public void SetMappingOrder(int mappingOrder)
    {
      this._propertyMap.SetMappingOrder(mappingOrder);
    }

    public void ConstructFormatterBy(Type formatterType, Func<IValueFormatter> instantiator)
    {
      this._propertyMap.RemoveLastFormatter();
      this._propertyMap.AddFormatter((IValueFormatter) new DeferredInstantiatedFormatter((Func<ResolutionContext, IValueFormatter>) (ctxt => instantiator())));
    }

    public void ConvertUsing(Func<TSource, TDestination> mappingFunction)
    {
      this._typeMap.UseCustomMapper((Func<ResolutionContext, object>) (source => (object) mappingFunction((TSource) source.SourceValue)));
    }

    public void ConvertUsing(
      Func<ResolutionContext, TDestination> mappingFunction)
    {
      this._typeMap.UseCustomMapper((Func<ResolutionContext, object>) (context => (object) mappingFunction(context)));
    }

    public void ConvertUsing(
      Func<ResolutionContext, TSource, TDestination> mappingFunction)
    {
      this._typeMap.UseCustomMapper((Func<ResolutionContext, object>) (source => (object) mappingFunction(source, (TSource) source.SourceValue)));
    }

    public void ConvertUsing(ITypeConverter<TSource, TDestination> converter)
    {
      this.ConvertUsing(new Func<ResolutionContext, TDestination>(converter.Convert));
    }

    public void ConvertUsing<TTypeConverter>() where TTypeConverter : ITypeConverter<TSource, TDestination>
    {
      this.ConvertUsing(new Func<ResolutionContext, TDestination>(new DeferredInstantiatedConverter<TSource, TDestination>(this.BuildCtor<ITypeConverter<TSource, TDestination>>(typeof (TTypeConverter))).Convert));
    }

    public IMappingExpression<TSource, TDestination> BeforeMap(
      Action<TSource, TDestination> beforeFunction)
    {
      this._typeMap.AddBeforeMapAction((Action<object, object>) ((src, dest) => beforeFunction((TSource) src, (TDestination) dest)));
      return (IMappingExpression<TSource, TDestination>) this;
    }

    public IMappingExpression<TSource, TDestination> BeforeMap<TMappingAction>() where TMappingAction : IMappingAction<TSource, TDestination>
    {
      return this.BeforeMap((Action<TSource, TDestination>) ((src, dest) => ((TMappingAction) this._serviceCtor(typeof (TMappingAction))).Process(src, dest)));
    }

    public IMappingExpression<TSource, TDestination> AfterMap(
      Action<TSource, TDestination> afterFunction)
    {
      this._typeMap.AddAfterMapAction((Action<object, object>) ((src, dest) => afterFunction((TSource) src, (TDestination) dest)));
      return (IMappingExpression<TSource, TDestination>) this;
    }

    public IMappingExpression<TSource, TDestination> AfterMap<TMappingAction>() where TMappingAction : IMappingAction<TSource, TDestination>
    {
      return this.AfterMap((Action<TSource, TDestination>) ((src, dest) => ((TMappingAction) this._serviceCtor(typeof (TMappingAction))).Process(src, dest)));
    }

    public IMappingExpression<TSource, TDestination> ConstructUsing(Func<TSource, TDestination> ctor)
    {
      return this.ConstructUsing((Func<ResolutionContext, TDestination>) (ctxt => ctor((TSource) ctxt.SourceValue)));
    }

    public IMappingExpression<TSource, TDestination> ConstructUsing(
      Func<ResolutionContext, TDestination> ctor)
    {
      this._typeMap.DestinationCtor = (Func<ResolutionContext, object>) (ctxt => (object) ctor(ctxt));
      return (IMappingExpression<TSource, TDestination>) this;
    }

    private void ForDestinationMember(
      IMemberAccessor destinationProperty,
      Action<IMemberConfigurationExpression<TSource>> memberOptions)
    {
      this._propertyMap = this._typeMap.FindOrCreatePropertyMapFor(destinationProperty);
      memberOptions((IMemberConfigurationExpression<TSource>) this);
    }

    public void As<T>() => this._typeMap.DestinationTypeOverride = typeof (T);

    private Func<ResolutionContext, TServiceType> BuildCtor<TServiceType>(Type type)
    {
      return (Func<ResolutionContext, TServiceType>) (context =>
      {
        if (context.Options.ServiceCtor != null)
        {
          object obj = context.Options.ServiceCtor(type);
          if (obj != null)
            return (TServiceType) obj;
        }
        return (TServiceType) this._serviceCtor(type);
      });
    }

    private class SourceMappingExpression : 
      ISourceMemberConfigurationExpression<TSource>,
      ISourceMemberConfigurationExpression
    {
      private readonly SourceMemberConfig _sourcePropertyConfig;

      public SourceMappingExpression(TypeMap typeMap, MemberInfo memberInfo)
      {
        this._sourcePropertyConfig = typeMap.FindOrCreateSourceMemberConfigFor(memberInfo);
      }

      public void Ignore() => this._sourcePropertyConfig.Ignore();
    }
  }
}
