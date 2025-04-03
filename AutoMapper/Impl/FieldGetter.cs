// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.FieldGetter
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public class FieldGetter : MemberGetter
  {
    private readonly FieldInfo _fieldInfo;
    private readonly string _name;
    private readonly Type _memberType;
    private readonly LateBoundFieldGet _lateBoundFieldGet;

    public FieldGetter(FieldInfo fieldInfo)
    {
      this._fieldInfo = fieldInfo;
      this._name = fieldInfo.Name;
      this._memberType = fieldInfo.FieldType;
      this._lateBoundFieldGet = MemberGetter.DelegateFactory.CreateGet(fieldInfo);
    }

    public override MemberInfo MemberInfo => (MemberInfo) this._fieldInfo;

    public override string Name => this._name;

    public override Type MemberType => this._memberType;

    public override object GetValue(object source) => this._lateBoundFieldGet(source);

    public bool Equals(FieldGetter other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other._fieldInfo, (object) this._fieldInfo);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return (object) obj.GetType() == (object) typeof (FieldGetter) && this.Equals((FieldGetter) obj);
    }

    public override int GetHashCode() => this._fieldInfo.GetHashCode();

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return this._fieldInfo.GetCustomAttributes(attributeType, inherit);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return this._fieldInfo.GetCustomAttributes(inherit);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return this._fieldInfo.IsDefined(attributeType, inherit);
    }
  }
}
