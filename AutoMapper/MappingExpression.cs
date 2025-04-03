// Decompiled with JetBrains decompiler
// Type: AutoMapper.MappingExpression
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class MappingExpression : IMappingExpression, IMemberConfigurationExpression
  {
    private readonly TypeMap _typeMap;
    private readonly Func<Type, object> _typeConverterCtor;
    private PropertyMap _propertyMap;

    public MappingExpression(TypeMap typeMap, Func<Type, object> typeConverterCtor)
    {
      this._typeMap = typeMap;
      this._typeConverterCtor = typeConverterCtor;
    }

    public void ConvertUsing<TTypeConverter>() => this.ConvertUsing(typeof (TTypeConverter));

    public void ConvertUsing(Type typeConverterType)
    {
      Type type = typeof (ITypeConverter<,>).MakeGenericType(this._typeMap.SourceType, this._typeMap.DestinationType);
      this._typeMap.UseCustomMapper(new Func<ResolutionContext, object>(new DeferredInstantiatedConverter(type.IsAssignableFrom(typeConverterType) ? type : typeConverterType, this.BuildCtor<object>(typeConverterType)).Convert));
    }

    public void As(Type typeOverride) => this._typeMap.DestinationTypeOverride = typeOverride;

    public IMappingExpression WithProfile(string profileName)
    {
      this._typeMap.Profile = profileName;
      return (IMappingExpression) this;
    }

    public IMappingExpression ForMember(
      string name,
      Action<IMemberConfigurationExpression> memberOptions)
    {
      IMemberAccessor destinationProperty = (IMemberAccessor) null;
      PropertyInfo property = this._typeMap.DestinationType.GetProperty(name);
      if ((object) property != null)
        destinationProperty = (IMemberAccessor) new PropertyAccessor(property);
      if (destinationProperty == null)
        destinationProperty = (IMemberAccessor) new FieldAccessor(this._typeMap.DestinationType.GetField(name));
      this.ForDestinationMember(destinationProperty, memberOptions);
      return (IMappingExpression) new MappingExpression(this._typeMap, this._typeConverterCtor);
    }

    public IMappingExpression ForSourceMember(
      string sourceMemberName,
      Action<ISourceMemberConfigurationExpression> memberOptions)
    {
      MappingExpression.SourceMappingExpression mappingExpression = new MappingExpression.SourceMappingExpression(this._typeMap, ((IEnumerable<MemberInfo>) this._typeMap.SourceType.GetMember(sourceMemberName)).First<MemberInfo>());
      memberOptions((ISourceMemberConfigurationExpression) mappingExpression);
      return (IMappingExpression) new MappingExpression(this._typeMap, this._typeConverterCtor);
    }

    private void ForDestinationMember(
      IMemberAccessor destinationProperty,
      Action<IMemberConfigurationExpression> memberOptions)
    {
      this._propertyMap = this._typeMap.FindOrCreatePropertyMapFor(destinationProperty);
      memberOptions((IMemberConfigurationExpression) this);
    }

    public void MapFrom(string sourceMember)
    {
      MemberInfo[] member = this._typeMap.SourceType.GetMember(sourceMember);
      if (!((IEnumerable<MemberInfo>) member).Any<MemberInfo>())
        throw new AutoMapperConfigurationException(string.Format("Unable to find source member {0} on type {1}", new object[2]
        {
          (object) sourceMember,
          (object) this._typeMap.SourceType.FullName
        }));
      MemberInfo accessorCandidate = !((IEnumerable<MemberInfo>) member).Skip<MemberInfo>(1).Any<MemberInfo>() ? ((IEnumerable<MemberInfo>) member).Single<MemberInfo>() : throw new AutoMapperConfigurationException(string.Format("Source member {0} is ambiguous on type {1}", new object[2]
      {
        (object) sourceMember,
        (object) this._typeMap.SourceType.FullName
      }));
      this._propertyMap.SourceMember = accessorCandidate;
      this._propertyMap.AssignCustomValueResolver((IValueResolver) accessorCandidate.ToMemberGetter());
    }

    public IResolutionExpression ResolveUsing(IValueResolver valueResolver)
    {
      this._propertyMap.AssignCustomValueResolver(valueResolver);
      return (IResolutionExpression) new ResolutionExpression(this._typeMap.SourceType, this._propertyMap);
    }

    public IResolverConfigurationExpression ResolveUsing(Type valueResolverType)
    {
      this.ResolveUsing((IValueResolver) new DeferredInstantiatedResolver(this.BuildCtor<IValueResolver>(valueResolverType)));
      return (IResolverConfigurationExpression) new ResolutionExpression(this._typeMap.SourceType, this._propertyMap);
    }

    public IResolverConfigurationExpression ResolveUsing<TValueResolver>()
    {
      this.ResolveUsing((IValueResolver) new DeferredInstantiatedResolver(this.BuildCtor<IValueResolver>(typeof (TValueResolver))));
      return (IResolverConfigurationExpression) new ResolutionExpression(this._typeMap.SourceType, this._propertyMap);
    }

    public void Ignore() => this._propertyMap.Ignore();

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
        return (TServiceType) this._typeConverterCtor(type);
      });
    }

    private class SourceMappingExpression : ISourceMemberConfigurationExpression
    {
      private readonly SourceMemberConfig _sourcePropertyConfig;

      public SourceMappingExpression(TypeMap typeMap, MemberInfo sourceMember)
      {
        this._sourcePropertyConfig = typeMap.FindOrCreateSourceMemberConfigFor(sourceMember);
      }

      public void Ignore() => this._sourcePropertyConfig.Ignore();
    }
  }
}
