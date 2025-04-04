// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.CachedLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System;
using System.ComponentModel;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("cached")]
  [AmbientProperty("Cached")]
  [AmbientProperty("ClearCache")]
  [ThreadAgnostic]
  public sealed class CachedLayoutRendererWrapper : WrapperLayoutRendererBase
  {
    private string _cachedValue;
    private string _renderedCacheKey;

    public CachedLayoutRendererWrapper()
    {
      this.Cached = true;
      this.ClearCache = CachedLayoutRendererWrapper.ClearCacheOption.OnInit | CachedLayoutRendererWrapper.ClearCacheOption.OnClose;
    }

    [DefaultValue(true)]
    public bool Cached { get; set; }

    public CachedLayoutRendererWrapper.ClearCacheOption ClearCache { get; set; }

    public Layout CacheKey { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      base.InitializeLayoutRenderer();
      if ((this.ClearCache & CachedLayoutRendererWrapper.ClearCacheOption.OnInit) != CachedLayoutRendererWrapper.ClearCacheOption.OnInit)
        return;
      this._cachedValue = (string) null;
    }

    protected override void CloseLayoutRenderer()
    {
      base.CloseLayoutRenderer();
      if ((this.ClearCache & CachedLayoutRendererWrapper.ClearCacheOption.OnClose) != CachedLayoutRendererWrapper.ClearCacheOption.OnClose)
        return;
      this._cachedValue = (string) null;
    }

    protected override string Transform(string text) => text;

    protected override string RenderInner(LogEventInfo logEvent)
    {
      if (!this.Cached)
        return base.RenderInner(logEvent);
      string str = this.CacheKey == null ? (string) null : this.CacheKey.Render(logEvent);
      if (this._cachedValue == null || this._renderedCacheKey != str)
      {
        this._cachedValue = base.RenderInner(logEvent);
        this._renderedCacheKey = str;
      }
      return this._cachedValue;
    }

    [Flags]
    public enum ClearCacheOption
    {
      None = 0,
      OnInit = 1,
      OnClose = 2,
    }
  }
}
