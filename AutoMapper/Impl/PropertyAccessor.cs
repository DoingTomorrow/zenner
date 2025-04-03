// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.PropertyAccessor
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public class PropertyAccessor : 
    PropertyGetter,
    IMemberAccessor,
    IMemberGetter,
    IMemberResolver,
    IValueResolver
  {
    private readonly LateBoundPropertySet _lateBoundPropertySet;
    private readonly bool _hasSetter;

    public PropertyAccessor(PropertyInfo propertyInfo)
      : base(propertyInfo)
    {
      this._hasSetter = (object) propertyInfo.GetSetMethod(true) != null;
      if (!this._hasSetter)
        return;
      this._lateBoundPropertySet = MemberGetter.DelegateFactory.CreateSet(propertyInfo);
    }

    public bool HasSetter => this._hasSetter;

    public virtual void SetValue(object destination, object value)
    {
      this._lateBoundPropertySet(destination, value);
    }
  }
}
