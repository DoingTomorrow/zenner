// Decompiled with JetBrains decompiler
// Type: AutoMapper.DeferredInstantiatedConverter
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class DeferredInstantiatedConverter : ITypeConverter<object, object>
  {
    private readonly Func<ResolutionContext, object> _instantiator;
    private readonly MethodInfo _converterMethod;

    public DeferredInstantiatedConverter(
      Type typeConverterType,
      Func<ResolutionContext, object> instantiator)
    {
      this._instantiator = instantiator;
      this._converterMethod = typeConverterType.GetMethod("Convert");
    }

    public object Convert(ResolutionContext context)
    {
      return this._converterMethod.Invoke(this._instantiator(context), (object[]) new ResolutionContext[1]
      {
        context
      });
    }
  }
}
