// Decompiled with JetBrains decompiler
// Type: AutoMapper.IFormatterConfiguration
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper
{
  [Obsolete("Formatters should not be used")]
  public interface IFormatterConfiguration : IProfileConfiguration
  {
    [Obsolete("Formatters should not be used")]
    IValueFormatter[] GetFormatters();

    [Obsolete("Formatters should not be used")]
    IDictionary<Type, IFormatterConfiguration> GetTypeSpecificFormatters();

    [Obsolete("Formatters should not be used")]
    Type[] GetFormatterTypesToSkip();

    [Obsolete("Formatters should not be used")]
    IEnumerable<IValueFormatter> GetFormattersToApply(ResolutionContext context);
  }
}
