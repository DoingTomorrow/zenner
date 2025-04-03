// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.MemberGetter
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public abstract class MemberGetter : IMemberGetter, IMemberResolver, IValueResolver
  {
    protected static readonly IDelegateFactory DelegateFactory = PlatformAdapter.Resolve<IDelegateFactory>();

    public abstract MemberInfo MemberInfo { get; }

    public abstract string Name { get; }

    public abstract Type MemberType { get; }

    public abstract object GetValue(object source);

    public ResolutionResult Resolve(ResolutionResult source)
    {
      return source.Value != null ? source.New(this.GetValue(source.Value), this.MemberType) : source.New(source.Value, this.MemberType);
    }

    public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

    public abstract object[] GetCustomAttributes(bool inherit);

    public abstract bool IsDefined(Type attributeType, bool inherit);
  }
}
