// Decompiled with JetBrains decompiler
// Type: AutoMapper.IFormatterExpression
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  [Obsolete("Formatters should not be used.")]
  public interface IFormatterExpression
  {
    [Obsolete("Formatters should not be used.")]
    IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter;

    [Obsolete("Formatters should not be used.")]
    IFormatterCtorExpression AddFormatter(Type valueFormatterType);

    [Obsolete("Formatters should not be used.")]
    void AddFormatter(IValueFormatter formatter);

    [Obsolete("Formatters should not be used.")]
    void AddFormatExpression(Func<ResolutionContext, string> formatExpression);

    [Obsolete("Formatters should not be used.")]
    void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter;

    [Obsolete("Formatters should not be used.")]
    IFormatterExpression ForSourceType<TSource>();
  }
}
