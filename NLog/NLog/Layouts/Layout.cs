// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.Layout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

#nullable disable
namespace NLog.Layouts
{
  [NLogConfigurationItem]
  public abstract class Layout : ISupportsInitialize, IRenderable
  {
    private bool _isInitialized;
    private bool _scannedForObjects;
    private const int MaxInitialRenderBufferLength = 16384;
    private int _maxRenderedLength;

    internal bool ThreadAgnostic { get; set; }

    internal bool ThreadSafe { get; set; }

    internal StackTraceUsage StackTraceUsage { get; private set; }

    protected LoggingConfiguration LoggingConfiguration { get; private set; }

    public static implicit operator Layout([Localizable(false)] string text)
    {
      return Layout.FromString(text);
    }

    public static Layout FromString(string layoutText)
    {
      return Layout.FromString(layoutText, ConfigurationItemFactory.Default);
    }

    public static Layout FromString(
      string layoutText,
      ConfigurationItemFactory configurationItemFactory)
    {
      return (Layout) new SimpleLayout(layoutText, configurationItemFactory);
    }

    public virtual void Precalculate(LogEventInfo logEvent)
    {
      if (this.ThreadAgnostic)
        return;
      this.Render(logEvent);
    }

    public string Render(LogEventInfo logEvent)
    {
      if (!this._isInitialized)
        this.Initialize(this.LoggingConfiguration);
      object obj;
      if (!this.ThreadAgnostic && logEvent.TryGetCachedLayoutValue(this, out obj))
        return obj?.ToString() ?? string.Empty;
      string str = this.GetFormattedMessage(logEvent) ?? string.Empty;
      if (!this.ThreadAgnostic)
        logEvent.AddCachedLayoutValue(this, (object) str);
      return str;
    }

    internal virtual void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
    {
      this.Precalculate(logEvent);
    }

    internal void RenderAppendBuilder(
      LogEventInfo logEvent,
      StringBuilder target,
      bool cacheLayoutResult = false)
    {
      if (!this._isInitialized)
        this.Initialize(this.LoggingConfiguration);
      object obj;
      if (!this.ThreadAgnostic && logEvent.TryGetCachedLayoutValue(this, out obj))
      {
        target.Append(obj?.ToString() ?? string.Empty);
      }
      else
      {
        cacheLayoutResult = cacheLayoutResult && !this.ThreadAgnostic;
        using (AppendBuilderCreator appendBuilderCreator = new AppendBuilderCreator(target, cacheLayoutResult))
        {
          this.RenderFormattedMessage(logEvent, appendBuilderCreator.Builder);
          if (!cacheLayoutResult)
            return;
          logEvent.AddCachedLayoutValue(this, (object) appendBuilderCreator.Builder.ToString());
        }
      }
    }

    internal string RenderAllocateBuilder(LogEventInfo logEvent, StringBuilder reusableBuilder = null)
    {
      int capacity = this._maxRenderedLength;
      if (capacity > 16384)
        capacity = 16384;
      StringBuilder target = reusableBuilder ?? new StringBuilder(capacity);
      this.RenderFormattedMessage(logEvent, target);
      if (target.Length > this._maxRenderedLength)
        this._maxRenderedLength = target.Length;
      return target.ToString();
    }

    protected virtual void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      target.Append(this.GetFormattedMessage(logEvent) ?? string.Empty);
    }

    void ISupportsInitialize.Initialize(LoggingConfiguration configuration)
    {
      this.Initialize(configuration);
    }

    void ISupportsInitialize.Close() => this.Close();

    internal void Initialize(LoggingConfiguration configuration)
    {
      if (this._isInitialized)
        return;
      this.LoggingConfiguration = configuration;
      this._isInitialized = true;
      this._scannedForObjects = false;
      this.InitializeLayout();
      if (this._scannedForObjects)
        return;
      InternalLogger.Debug("Initialized Layout done but not scanned for objects");
      this.PerformObjectScanning();
    }

    internal void PerformObjectScanning()
    {
      List<object> reachableObjects = ObjectGraphScanner.FindReachableObjects<object>(true, (object) this);
      this.ThreadAgnostic = reachableObjects.All<object>((Func<object, bool>) (item => item.GetType().IsDefined(typeof (ThreadAgnosticAttribute), true)));
      this.ThreadSafe = reachableObjects.All<object>((Func<object, bool>) (item => item.GetType().IsDefined(typeof (ThreadSafeAttribute), true)));
      this.StackTraceUsage = StackTraceUsage.None;
      this.StackTraceUsage = reachableObjects.OfType<IUsesStackTrace>().DefaultIfEmpty<IUsesStackTrace>().Max<IUsesStackTrace, StackTraceUsage>((Func<IUsesStackTrace, StackTraceUsage>) (item => item != null ? item.StackTraceUsage : StackTraceUsage.None));
      this._scannedForObjects = true;
    }

    internal void Close()
    {
      if (!this._isInitialized)
        return;
      this.LoggingConfiguration = (LoggingConfiguration) null;
      this._isInitialized = false;
      this.CloseLayout();
    }

    protected virtual void InitializeLayout() => this.PerformObjectScanning();

    protected virtual void CloseLayout()
    {
    }

    protected abstract string GetFormattedMessage(LogEventInfo logEvent);

    public static void Register<T>(string name) where T : Layout
    {
      Type layoutType = typeof (T);
      Layout.Register(name, layoutType);
    }

    public static void Register(string name, Type layoutType)
    {
      ConfigurationItemFactory.Default.Layouts.RegisterDefinition(name, layoutType);
    }

    internal string ToStringWithNestedItems<T>(
      IList<T> nestedItems,
      Func<T, string> nextItemToString)
    {
      if (nestedItems == null || nestedItems.Count <= 0)
        return this.ToString();
      string[] array = nestedItems.Select<T, string>((Func<T, string>) (c => nextItemToString(c))).ToArray<string>();
      return this.GetType().Name + "=" + string.Join("|", array);
    }
  }
}
