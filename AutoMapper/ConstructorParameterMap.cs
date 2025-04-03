// Decompiled with JetBrains decompiler
// Type: AutoMapper.ConstructorParameterMap
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public class ConstructorParameterMap
  {
    public ConstructorParameterMap(ParameterInfo parameter, IMemberGetter[] sourceResolvers)
    {
      this.Parameter = parameter;
      this.SourceResolvers = sourceResolvers;
    }

    public ParameterInfo Parameter { get; private set; }

    public IMemberGetter[] SourceResolvers { get; private set; }

    public ResolutionResult ResolveValue(ResolutionContext context)
    {
      return ((IEnumerable<IMemberGetter>) this.SourceResolvers).Aggregate<IMemberGetter, ResolutionResult>(new ResolutionResult(context), (Func<ResolutionResult, IMemberGetter, ResolutionResult>) ((current, resolver) => resolver.Resolve(current)));
    }
  }
}
