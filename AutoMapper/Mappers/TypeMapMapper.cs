// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.TypeMapMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper.Mappers
{
  public class TypeMapMapper : IObjectMapper
  {
    private readonly IEnumerable<ITypeMapObjectMapper> _mappers;

    public TypeMapMapper(IEnumerable<ITypeMapObjectMapper> mappers) => this._mappers = mappers;

    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      context.TypeMap.Seal();
      ITypeMapObjectMapper typeMapObjectMapper = this._mappers.First<ITypeMapObjectMapper>((Func<ITypeMapObjectMapper, bool>) (objectMapper => objectMapper.IsMatch(context, mapper)));
      return !context.TypeMap.ShouldAssignValue(context) ? (object) null : typeMapObjectMapper.Map(context, mapper);
    }

    public bool IsMatch(ResolutionContext context) => context.TypeMap != null;
  }
}
