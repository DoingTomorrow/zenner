// Decompiled with JetBrains decompiler
// Type: AutoMapper.DelegateBasedResolver`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public class DelegateBasedResolver<TSource> : IValueResolver
  {
    private readonly Func<TSource, object> _method;

    public DelegateBasedResolver(Func<TSource, object> method) => this._method = method;

    public ResolutionResult Resolve(ResolutionResult source)
    {
      if (source.Value != null && !(source.Value is TSource))
        throw new ArgumentException("Expected obj to be of type " + (object) typeof (TSource) + " but was " + (object) source.Value.GetType());
      object obj = this._method((TSource) source.Value);
      return source.New(obj);
    }
  }
}
