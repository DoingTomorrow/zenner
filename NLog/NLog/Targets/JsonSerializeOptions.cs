// Decompiled with JetBrains decompiler
// Type: NLog.Targets.JsonSerializeOptions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets
{
  public class JsonSerializeOptions
  {
    [DefaultValue(true)]
    public bool QuoteKeys { get; set; }

    public IFormatProvider FormatProvider { get; set; }

    public string Format { get; set; }

    [DefaultValue(false)]
    public bool EscapeUnicode { get; set; }

    [DefaultValue(false)]
    public bool EnumAsInteger { get; set; }

    [DefaultValue(false)]
    public bool SanitizeDictionaryKeys { get; set; }

    [DefaultValue(10)]
    public int MaxRecursionLimit { get; set; }

    public JsonSerializeOptions()
    {
      this.QuoteKeys = true;
      this.MaxRecursionLimit = 10;
    }
  }
}
