// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.Rot13LayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("rot13")]
  [AppDomainFixedOutput]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class Rot13LayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    public Layout Text
    {
      get => this.Inner;
      set => this.Inner = value;
    }

    public static string DecodeRot13(string encodedValue)
    {
      StringBuilder encodedValue1 = new StringBuilder(encodedValue.Length);
      encodedValue1.Append(encodedValue);
      Rot13LayoutRendererWrapper.DecodeRot13(encodedValue1);
      return encodedValue1.ToString();
    }

    internal static void DecodeRot13(StringBuilder encodedValue)
    {
      if (encodedValue == null)
        return;
      for (int index = 0; index < encodedValue.Length; ++index)
        encodedValue[index] = Rot13LayoutRendererWrapper.DecodeRot13Char(encodedValue[index]);
    }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
      Rot13LayoutRendererWrapper.DecodeRot13(target);
    }

    private static char DecodeRot13Char(char c)
    {
      if (c >= 'A' && c <= 'M')
        return (char) (78 + ((int) c - 65));
      if (c >= 'a' && c <= 'm')
        return (char) (110 + ((int) c - 97));
      if (c >= 'N' && c <= 'Z')
        return (char) (65 + ((int) c - 78));
      return c >= 'n' && c <= 'z' ? (char) (97 + ((int) c - 110)) : c;
    }
  }
}
