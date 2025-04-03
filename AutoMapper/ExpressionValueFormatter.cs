// Decompiled with JetBrains decompiler
// Type: AutoMapper.ExpressionValueFormatter
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public class ExpressionValueFormatter : IValueFormatter
  {
    private readonly Func<ResolutionContext, string> _valueFormatterExpression;

    public ExpressionValueFormatter(
      Func<ResolutionContext, string> valueFormatterExpression)
    {
      this._valueFormatterExpression = valueFormatterExpression;
    }

    public string FormatValue(ResolutionContext context) => this._valueFormatterExpression(context);
  }
}
