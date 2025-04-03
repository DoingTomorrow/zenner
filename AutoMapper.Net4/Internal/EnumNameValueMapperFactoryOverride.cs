// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.EnumNameValueMapperFactoryOverride
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper.Internal
{
  public class EnumNameValueMapperFactoryOverride : IEnumNameValueMapperFactory
  {
    public IEnumNameValueMapper Create()
    {
      return (IEnumNameValueMapper) new EnumNameValueMapperFactoryOverride.EnumVameValueMapper();
    }

    private class EnumVameValueMapper : IEnumNameValueMapper
    {
      public bool IsMatch(Type enumDestinationType, string sourceValue)
      {
        return !((IEnumerable<string>) Enum.GetNames(enumDestinationType)).Contains<string>(sourceValue);
      }

      public object Convert(
        Type enumSourceType,
        Type enumDestinationType,
        ResolutionContext context)
      {
        Type underlyingType = Enum.GetUnderlyingType(enumSourceType);
        object obj = Convert.ChangeType(context.SourceValue, underlyingType);
        return Enum.ToObject(context.DestinationType, obj);
      }
    }
  }
}
