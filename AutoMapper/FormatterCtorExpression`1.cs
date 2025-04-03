// Decompiled with JetBrains decompiler
// Type: AutoMapper.FormatterCtorExpression`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public class FormatterCtorExpression<TValueFormatter> : IFormatterCtorExpression<TValueFormatter> where TValueFormatter : IValueFormatter
  {
    private readonly IFormatterCtorConfigurator _formatterCtorConfigurator;

    public FormatterCtorExpression(
      IFormatterCtorConfigurator formatterCtorConfigurator)
    {
      this._formatterCtorConfigurator = formatterCtorConfigurator;
    }

    public void ConstructedBy(Func<TValueFormatter> constructor)
    {
      this._formatterCtorConfigurator.ConstructFormatterBy(typeof (TValueFormatter), (Func<IValueFormatter>) (() => (IValueFormatter) constructor()));
    }
  }
}
