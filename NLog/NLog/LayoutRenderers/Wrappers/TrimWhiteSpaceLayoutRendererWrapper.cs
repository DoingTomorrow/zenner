// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.TrimWhiteSpaceLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("trim-whitespace")]
  [AmbientProperty("TrimWhiteSpace")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class TrimWhiteSpaceLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    public TrimWhiteSpaceLayoutRendererWrapper() => this.TrimWhiteSpace = true;

    [DefaultValue(true)]
    public bool TrimWhiteSpace { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
      if (target == null || target.Length == 0 || !this.TrimWhiteSpace)
        return;
      this.TrimRight(target);
      if (target.Length <= 0)
        return;
      this.TrimLeft(target);
    }

    private void TrimRight(StringBuilder sb)
    {
      int index = sb.Length - 1;
      while (index >= 0 && char.IsWhiteSpace(sb[index]))
        --index;
      if (index >= sb.Length - 1)
        return;
      sb.Length = index + 1;
    }

    private void TrimLeft(StringBuilder sb)
    {
      int num = 0;
      while (num < sb.Length && char.IsWhiteSpace(sb[num]))
        ++num;
      if (num <= 0)
        return;
      sb.Remove(0, num);
    }
  }
}
