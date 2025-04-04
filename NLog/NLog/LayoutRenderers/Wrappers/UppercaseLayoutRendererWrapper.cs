// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.UppercaseLayoutRendererWrapper
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
  [LayoutRenderer("uppercase")]
  [AmbientProperty("Uppercase")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class UppercaseLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    public UppercaseLayoutRendererWrapper()
    {
      this.Culture = CultureInfo.InvariantCulture;
      this.Uppercase = true;
    }

    [DefaultValue(true)]
    public bool Uppercase { get; set; }

    public CultureInfo Culture { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
      if (!this.Uppercase)
        return;
      CultureInfo culture = this.Culture;
      for (int index = 0; index < target.Length; ++index)
        target[index] = char.ToUpper(target[index], culture);
    }
  }
}
