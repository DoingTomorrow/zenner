// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.FlagsEnumMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper.Mappers
{
  public class FlagsEnumMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      Type enumerationType = TypeHelper.GetEnumerationType(context.DestinationType);
      return context.SourceValue == null ? mapper.CreateObject(context) : Enum.Parse(enumerationType, context.SourceValue.ToString(), true);
    }

    public bool IsMatch(ResolutionContext context)
    {
      Type enumerationType1 = TypeHelper.GetEnumerationType(context.SourceType);
      Type enumerationType2 = TypeHelper.GetEnumerationType(context.DestinationType);
      return (object) enumerationType1 != null && (object) enumerationType2 != null && ((IEnumerable<object>) enumerationType1.GetCustomAttributes(typeof (FlagsAttribute), false)).Any<object>() && ((IEnumerable<object>) enumerationType2.GetCustomAttributes(typeof (FlagsAttribute), false)).Any<object>();
    }
  }
}
