// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.ExplicitConversionOperatorMapper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper.Mappers
{
  public class ExplicitConversionOperatorMapper : IObjectMapper
  {
    public object Map(ResolutionContext context, IMappingEngineRunner mapper)
    {
      return ExplicitConversionOperatorMapper.GetExplicitConversionOperator(context).Invoke((object) null, new object[1]
      {
        context.SourceValue
      });
    }

    public bool IsMatch(ResolutionContext context)
    {
      return (object) ExplicitConversionOperatorMapper.GetExplicitConversionOperator(context) != null;
    }

    private static MethodInfo GetExplicitConversionOperator(ResolutionContext context)
    {
      MethodInfo methodInfo1 = ((IEnumerable<MethodInfo>) context.SourceType.GetMethods(BindingFlags.Static | BindingFlags.Public)).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.Name == "op_Explicit")).Where<MethodInfo>((Func<MethodInfo, bool>) (mi => (object) mi.ReturnType == (object) context.DestinationType)).FirstOrDefault<MethodInfo>();
      MethodInfo method = context.DestinationType.GetMethod("op_Explicit", new Type[1]
      {
        context.SourceType
      });
      MethodInfo methodInfo2 = methodInfo1;
      return (object) methodInfo2 != null ? methodInfo2 : method;
    }
  }
}
