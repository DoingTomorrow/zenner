// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.FileSystemNormalizeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("filesystem-normalize")]
  [AmbientProperty("FSNormalize")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class FileSystemNormalizeLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    public FileSystemNormalizeLayoutRendererWrapper() => this.FSNormalize = true;

    [DefaultValue(true)]
    public bool FSNormalize { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder builder)
    {
      if (!this.FSNormalize)
        return;
      for (int index = 0; index < builder.Length; ++index)
      {
        if (!FileSystemNormalizeLayoutRendererWrapper.IsSafeCharacter(builder[index]))
          builder[index] = '_';
      }
    }

    private static bool IsSafeCharacter(char c)
    {
      return char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.' || c == ' ';
    }
  }
}
