// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.LowercaseLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("lowercase")]
  [AmbientProperty("Lowercase")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class LowercaseLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    public LowercaseLayoutRendererWrapper()
    {
      this.Culture = CultureInfo.InvariantCulture;
      this.Lowercase = true;
    }

    [DefaultValue(true)]
    public bool Lowercase { get; set; }

    public CultureInfo Culture { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
      if (!this.Lowercase)
        return;
      CultureInfo culture = this.Culture;
      for (int index = 0; index < target.Length; ++index)
        target[index] = char.ToLower(target[index], culture);
    }
  }
}
