// Decompiled with JetBrains decompiler
// Type: AutoMapper.ResolutionExpression`2
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace AutoMapper
{
  public class ResolutionExpression<TSource, TValueResolver> : 
    IResolverConfigurationExpression<TSource, TValueResolver>
    where TValueResolver : IValueResolver
  {
    private readonly PropertyMap _propertyMap;

    public ResolutionExpression(PropertyMap propertyMap) => this._propertyMap = propertyMap;

    public IResolverConfigurationExpression<TSource, TValueResolver> FromMember(
      Expression<Func<TSource, object>> sourceMember)
    {
      if (sourceMember.Body is MemberExpression)
        this._propertyMap.SourceMember = ((MemberExpression) sourceMember.Body).Member;
      this._propertyMap.ChainTypeMemberForResolver((IValueResolver) new DelegateBasedResolver<TSource>(sourceMember.Compile()));
      return (IResolverConfigurationExpression<TSource, TValueResolver>) this;
    }

    public IResolverConfigurationExpression<TSource, TValueResolver> FromMember(
      string sourcePropertyName)
    {
      this._propertyMap.SourceMember = typeof (TSource).GetMember(sourcePropertyName)[0];
      this._propertyMap.ChainTypeMemberForResolver((IValueResolver) new PropertyNameResolver(typeof (TSource), sourcePropertyName));
      return (IResolverConfigurationExpression<TSource, TValueResolver>) this;
    }

    public IResolverConfigurationExpression<TSource, TValueResolver> ConstructedBy(
      Func<TValueResolver> constructor)
    {
      this._propertyMap.ChainConstructorForResolver((IValueResolver) new DeferredInstantiatedResolver((Func<ResolutionContext, IValueResolver>) (ctxt => (IValueResolver) constructor())));
      return (IResolverConfigurationExpression<TSource, TValueResolver>) this;
    }
  }
}
