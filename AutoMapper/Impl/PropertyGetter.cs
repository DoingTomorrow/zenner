// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.PropertyGetter
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public class PropertyGetter : MemberGetter
  {
    private readonly PropertyInfo _propertyInfo;
    private readonly string _name;
    private readonly Type _memberType;
    private readonly LateBoundPropertyGet _lateBoundPropertyGet;

    public PropertyGetter(PropertyInfo propertyInfo)
    {
      this._propertyInfo = propertyInfo;
      this._name = this._propertyInfo.Name;
      this._memberType = this._propertyInfo.PropertyType;
      if ((object) this._propertyInfo.GetGetMethod(true) != null)
        this._lateBoundPropertyGet = MemberGetter.DelegateFactory.CreateGet(propertyInfo);
      else
        this._lateBoundPropertyGet = (LateBoundPropertyGet) (src => (object) null);
    }

    public override MemberInfo MemberInfo => (MemberInfo) this._propertyInfo;

    public override string Name => this._name;

    public override Type MemberType => this._memberType;

    public override object GetValue(object source) => this._lateBoundPropertyGet(source);

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return this._propertyInfo.GetCustomAttributes(attributeType, inherit);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return this._propertyInfo.GetCustomAttributes(inherit);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return this._propertyInfo.IsDefined(attributeType, inherit);
    }

    public bool Equals(PropertyGetter other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other._propertyInfo, (object) this._propertyInfo);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) obj.GetType() == (object) typeof (PropertyGetter) && this.Equals((PropertyGetter) obj);
    }

    public override int GetHashCode() => this._propertyInfo.GetHashCode();
  }
}
