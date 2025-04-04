// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.XmlEncodeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("xml-encode")]
  [AmbientProperty("XmlEncode")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class XmlEncodeLayoutRendererWrapper : WrapperLayoutRendererBase
  {
    private static readonly char[] XmlEscapeChars = new char[5]
    {
      '<',
      '>',
      '&',
      '\'',
      '"'
    };

    public XmlEncodeLayoutRendererWrapper() => this.XmlEncode = true;

    [DefaultValue(true)]
    public bool XmlEncode { get; set; }

    protected override string Transform(string text)
    {
      return !this.XmlEncode ? text : XmlEncodeLayoutRendererWrapper.DoXmlEscape(text);
    }

    private static string DoXmlEscape(string text)
    {
      if (text.Length < 4096 && text.IndexOfAny(XmlEncodeLayoutRendererWrapper.XmlEscapeChars) < 0)
        return text;
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      for (int index = 0; index < text.Length; ++index)
      {
        switch (text[index])
        {
          case '"':
            stringBuilder.Append("&quot;");
            break;
          case '&':
            stringBuilder.Append("&amp;");
            break;
          case '\'':
            stringBuilder.Append("&apos;");
            break;
          case '<':
            stringBuilder.Append("&lt;");
            break;
          case '>':
            stringBuilder.Append("&gt;");
            break;
          default:
            stringBuilder.Append(text[index]);
            break;
        }
      }
      return stringBuilder.ToString();
    }
  }
}
