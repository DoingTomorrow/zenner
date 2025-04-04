// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WrapLineLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("wrapline")]
  [AmbientProperty("WrapLine")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class WrapLineLayoutRendererWrapper : WrapperLayoutRendererBase
  {
    public WrapLineLayoutRendererWrapper() => this.WrapLine = 80;

    [DefaultValue(80)]
    public int WrapLine { get; set; }

    protected override string Transform(string text)
    {
      if (this.WrapLine <= 0)
        return text;
      int length = this.WrapLine;
      if (text.Length <= length)
        return text;
      StringBuilder stringBuilder = new StringBuilder(text.Length + text.Length / length * Environment.NewLine.Length);
      for (int startIndex = 0; startIndex < text.Length; startIndex += length)
      {
        if (length + startIndex > text.Length)
          length = text.Length - startIndex;
        stringBuilder.Append(text.Substring(startIndex, length));
        if (length + startIndex < text.Length)
          stringBuilder.Append(Environment.NewLine);
      }
      return stringBuilder.ToString();
    }
  }
}
