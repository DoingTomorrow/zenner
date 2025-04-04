// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.JsonEncodeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Targets;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("json-encode")]
  [AmbientProperty("JsonEncode")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class JsonEncodeLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    public JsonEncodeLayoutRendererWrapper()
    {
      this.JsonEncode = true;
      this.EscapeUnicode = true;
    }

    [DefaultValue(true)]
    public bool JsonEncode { get; set; }

    [DefaultValue(true)]
    public bool EscapeUnicode { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
      if (!this.JsonEncode || !this.RequiresJsonEncode(target))
        return;
      string str = DefaultJsonSerializer.EscapeString(target.ToString(), this.EscapeUnicode);
      target.Length = 0;
      target.Append(str);
    }

    private bool RequiresJsonEncode(StringBuilder target)
    {
      for (int index = 0; index < target.Length; ++index)
      {
        if (DefaultJsonSerializer.RequiresJsonEscape(target[index], this.EscapeUnicode))
          return true;
      }
      return false;
    }
  }
}
