// Decompiled with JetBrains decompiler
// Type: AutoMapper.IFormatterCtorExpression`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  [Obsolete("Formatters should not be used.")]
  public interface IFormatterCtorExpression<TValueFormatter> where TValueFormatter : IValueFormatter
  {
    void ConstructedBy(Func<TValueFormatter> constructor);
  }
}
