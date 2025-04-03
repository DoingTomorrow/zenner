// Decompiled with JetBrains decompiler
// Type: AutoMapper.NullReferenceExceptionSwallowingResolver
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public class NullReferenceExceptionSwallowingResolver : IMemberResolver, IValueResolver
  {
    private readonly IMemberResolver _inner;

    public NullReferenceExceptionSwallowingResolver(IMemberResolver inner) => this._inner = inner;

    public ResolutionResult Resolve(ResolutionResult source)
    {
      try
      {
        return this._inner.Resolve(source);
      }
      catch (NullReferenceException ex)
      {
        return source.New((object) null, this.MemberType);
      }
    }

    public Type MemberType => this._inner.MemberType;
  }
}
