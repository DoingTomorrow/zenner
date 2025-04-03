// Decompiled with JetBrains decompiler
// Type: AutoMapper.PropertyNameResolver
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class PropertyNameResolver : IValueResolver
  {
    private readonly Type _sourceType;
    private readonly PropertyInfo _propertyInfo;

    public PropertyNameResolver(Type sourceType, string propertyName)
    {
      this._sourceType = sourceType;
      this._propertyInfo = sourceType.GetProperty(propertyName);
    }

    public ResolutionResult Resolve(ResolutionResult source)
    {
      if (source.Value == null)
        return source;
      Type type = source.Value.GetType();
      if (!this._sourceType.IsAssignableFrom(type))
        throw new ArgumentException("Expected obj to be of type " + (object) this._sourceType + " but was " + (object) type);
      object obj = this._propertyInfo.GetValue(source.Value, (object[]) null);
      return source.New(obj);
    }
  }
}
