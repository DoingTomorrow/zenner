// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.FieldAccessor
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public class FieldAccessor : 
    FieldGetter,
    IMemberAccessor,
    IMemberGetter,
    IMemberResolver,
    IValueResolver
  {
    private readonly LateBoundFieldSet _lateBoundFieldSet;

    public FieldAccessor(FieldInfo fieldInfo)
      : base(fieldInfo)
    {
      this._lateBoundFieldSet = MemberGetter.DelegateFactory.CreateSet(fieldInfo);
    }

    public void SetValue(object destination, object value)
    {
      this._lateBoundFieldSet(destination, value);
    }
  }
}
