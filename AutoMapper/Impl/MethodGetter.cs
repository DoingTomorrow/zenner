// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.MethodGetter
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public class MethodGetter : MemberGetter
  {
    private readonly MethodInfo _methodInfo;
    private readonly string _name;
    private readonly Type _memberType;
    private readonly LateBoundMethod _lateBoundMethod;

    public MethodGetter(MethodInfo methodInfo)
    {
      this._methodInfo = methodInfo;
      this._name = this._methodInfo.Name;
      this._memberType = this._methodInfo.ReturnType;
      this._lateBoundMethod = MemberGetter.DelegateFactory.CreateGet(methodInfo);
    }

    public override MemberInfo MemberInfo => (MemberInfo) this._methodInfo;

    public override string Name => this._name;

    public override Type MemberType => this._memberType;

    public override object GetValue(object source)
    {
      return (object) this._memberType != null ? this._lateBoundMethod(source, new object[0]) : (object) null;
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return this._methodInfo.GetCustomAttributes(attributeType, inherit);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return this._methodInfo.GetCustomAttributes(inherit);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return this._methodInfo.IsDefined(attributeType, inherit);
    }

    public bool Equals(MethodGetter other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other._methodInfo, (object) this._methodInfo);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) obj.GetType() == (object) typeof (MethodGetter) && this.Equals((MethodGetter) obj);
    }

    public override int GetHashCode() => this._methodInfo.GetHashCode();
  }
}
