// Decompiled with JetBrains decompiler
// Type: AutoMapper.ConstructorMap
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class ConstructorMap
  {
    private static readonly IDelegateFactory DelegateFactory = PlatformAdapter.Resolve<IDelegateFactory>();
    private readonly LateBoundParamsCtor _runtimeCtor;

    public ConstructorInfo Ctor { get; private set; }

    public IEnumerable<ConstructorParameterMap> CtorParams { get; private set; }

    public ConstructorMap(ConstructorInfo ctor, IEnumerable<ConstructorParameterMap> ctorParams)
    {
      this.Ctor = ctor;
      this.CtorParams = ctorParams;
      this._runtimeCtor = ConstructorMap.DelegateFactory.CreateCtor(ctor, this.CtorParams);
    }

    public object ResolveValue(ResolutionContext context, IMappingEngineRunner mappingEngine)
    {
      List<object> objectList = new List<object>();
      foreach (ConstructorParameterMap ctorParam in this.CtorParams)
      {
        ResolutionResult resolutionResult = ctorParam.ResolveValue(context);
        Type type = resolutionResult.Type;
        Type parameterType = ctorParam.Parameter.ParameterType;
        TypeMap typeMapFor = mappingEngine.ConfigurationProvider.FindTypeMapFor(resolutionResult, parameterType);
        Type sourceType = typeMapFor != null ? typeMapFor.SourceType : type;
        ResolutionContext typeContext = context.CreateTypeContext(typeMapFor, resolutionResult.Value, (object) null, sourceType, parameterType);
        object obj = mappingEngine.Map(typeContext);
        objectList.Add(obj);
      }
      return this._runtimeCtor(objectList.ToArray());
    }
  }
}
