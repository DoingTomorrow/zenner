// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.JsonAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.LayoutRenderers.Wrappers;

#nullable disable
namespace NLog.Layouts
{
  [NLogConfigurationItem]
  [ThreadAgnostic]
  [ThreadSafe]
  public class JsonAttribute
  {
    internal readonly JsonEncodeLayoutRendererWrapper LayoutWrapper = new JsonEncodeLayoutRendererWrapper();

    public JsonAttribute()
      : this((string) null, (Layout) null, true)
    {
    }

    public JsonAttribute(string name, Layout layout)
      : this(name, layout, true)
    {
    }

    public JsonAttribute(string name, Layout layout, bool encode)
    {
      this.Name = name;
      this.Layout = layout;
      this.Encode = encode;
      this.IncludeEmptyValue = false;
    }

    [RequiredParameter]
    public string Name { get; set; }

    [RequiredParameter]
    public Layout Layout
    {
      get => this.LayoutWrapper.Inner;
      set => this.LayoutWrapper.Inner = value;
    }

    public bool Encode
    {
      get => this.LayoutWrapper.JsonEncode;
      set => this.LayoutWrapper.JsonEncode = value;
    }

    public bool EscapeUnicode
    {
      get => this.LayoutWrapper.EscapeUnicode;
      set => this.LayoutWrapper.EscapeUnicode = value;
    }

    public bool IncludeEmptyValue { get; set; }
  }
}
