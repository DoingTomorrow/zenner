// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.ValueTypePropertyAccessor
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public class ValueTypePropertyAccessor : 
    PropertyGetter,
    IMemberAccessor,
    IMemberGetter,
    IMemberResolver,
    IValueResolver
  {
    private readonly MethodInfo _lateBoundPropertySet;
    private readonly bool _hasSetter;

    public ValueTypePropertyAccessor(PropertyInfo propertyInfo)
      : base(propertyInfo)
    {
      MethodInfo setMethod = propertyInfo.GetSetMethod(true);
      this._hasSetter = (object) setMethod != null;
      if (!this._hasSetter)
        return;
      this._lateBoundPropertySet = setMethod;
    }

    public bool HasSetter => this._hasSetter;

    public void SetValue(object destination, object value)
    {
      this._lateBoundPropertySet.Invoke(destination, new object[1]
      {
        value
      });
    }
  }
}
