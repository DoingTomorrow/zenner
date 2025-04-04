// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.UrlEncodeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("url-encode")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class UrlEncodeLayoutRendererWrapper : WrapperLayoutRendererBase
  {
    public UrlEncodeLayoutRendererWrapper() => this.SpaceAsPlus = true;

    public bool SpaceAsPlus { get; set; }

    public bool EscapeDataRfc3986 { get; set; }

    public bool EscapeDataNLogLegacy { get; set; }

    protected override string Transform(string text)
    {
      if (string.IsNullOrEmpty(text))
        return string.Empty;
      UrlHelper.EscapeEncodingFlag stringEncodingFlags = UrlHelper.GetUriStringEncodingFlags(this.EscapeDataNLogLegacy, this.SpaceAsPlus, this.EscapeDataRfc3986);
      StringBuilder target = new StringBuilder(text.Length + 20);
      UrlHelper.EscapeDataEncode(text, target, stringEncodingFlags);
      return target.ToString();
    }
  }
}
