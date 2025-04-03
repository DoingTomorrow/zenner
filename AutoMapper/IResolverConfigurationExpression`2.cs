// Decompiled with JetBrains decompiler
// Type: AutoMapper.IResolverConfigurationExpression`2
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace AutoMapper
{
  public interface IResolverConfigurationExpression<TSource, TValueResolver> where TValueResolver : IValueResolver
  {
    IResolverConfigurationExpression<TSource, TValueResolver> FromMember(
      Expression<Func<TSource, object>> sourceMember);

    IResolverConfigurationExpression<TSource, TValueResolver> FromMember(string sourcePropertyName);

    IResolverConfigurationExpression<TSource, TValueResolver> ConstructedBy(
      Func<TValueResolver> constructor);
  }
}
