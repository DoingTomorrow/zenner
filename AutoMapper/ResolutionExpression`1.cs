// Decompiled with JetBrains decompiler
// Type: AutoMapper.ResolutionExpression`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace AutoMapper
{
  public class ResolutionExpression<TSource> : 
    IResolverConfigurationExpression<TSource>,
    IResolutionExpression<TSource>,
    IResolverConfigurationExpression,
    IResolutionExpression
  {
    private readonly Type _sourceType;
    private readonly PropertyMap _propertyMap;

    public ResolutionExpression(PropertyMap propertyMap)
      : this(typeof (TSource), propertyMap)
    {
    }

    public ResolutionExpression(Type sourceType, PropertyMap propertyMap)
    {
      this._sourceType = sourceType;
      this._propertyMap = propertyMap;
    }

    public void FromMember(Expression<Func<TSource, object>> sourceMember)
    {
      if (sourceMember.Body is MemberExpression)
        this._propertyMap.SourceMember = (sourceMember.Body as MemberExpression).Member;
      this._propertyMap.ChainTypeMemberForResolver((IValueResolver) new DelegateBasedResolver<TSource>(sourceMember.Compile()));
    }

    public void FromMember(string sourcePropertyName)
    {
      this._propertyMap.SourceMember = this._sourceType.GetMember(sourcePropertyName)[0];
      this._propertyMap.ChainTypeMemberForResolver((IValueResolver) new PropertyNameResolver(this._sourceType, sourcePropertyName));
    }

    IResolutionExpression IResolverConfigurationExpression.ConstructedBy(
      Func<IValueResolver> constructor)
    {
      return (IResolutionExpression) this.ConstructedBy(constructor);
    }

    public IResolutionExpression<TSource> ConstructedBy(Func<IValueResolver> constructor)
    {
      this._propertyMap.ChainConstructorForResolver((IValueResolver) new DeferredInstantiatedResolver((Func<ResolutionContext, IValueResolver>) (ctxt => constructor())));
      return (IResolutionExpression<TSource>) this;
    }
  }
}
