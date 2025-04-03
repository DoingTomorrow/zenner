// Decompiled with JetBrains decompiler
// Type: AutoMapper.ValueFormatter`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;

#nullable disable
namespace AutoMapper
{
  public abstract class ValueFormatter<T> : IValueFormatter
  {
    public string FormatValue(ResolutionContext context)
    {
      if (context.SourceValue == null)
        return (string) null;
      return !(context.SourceValue is T) ? context.SourceValue.ToNullSafeString() : this.FormatValueCore((T) context.SourceValue);
    }

    protected abstract string FormatValueCore(T value);
  }
}
