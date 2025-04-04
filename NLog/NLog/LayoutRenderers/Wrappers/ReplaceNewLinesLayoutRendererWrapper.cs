// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.ReplaceNewLinesLayoutRendererWrapper
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
  [LayoutRenderer("replace-newlines")]
  [AmbientProperty("ReplaceNewLines")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class ReplaceNewLinesLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    public ReplaceNewLinesLayoutRendererWrapper() => this.Replacement = " ";

    [DefaultValue(" ")]
    public string Replacement { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
      target.Replace(Environment.NewLine, this.Replacement);
    }
  }
}
