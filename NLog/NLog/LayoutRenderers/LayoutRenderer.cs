// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [NLogConfigurationItem]
  public abstract class LayoutRenderer : ISupportsInitialize, IRenderable, IDisposable
  {
    private const int MaxInitialRenderBufferLength = 16384;
    private int _maxRenderedLength;
    private bool _isInitialized;

    protected LoggingConfiguration LoggingConfiguration { get; private set; }

    public override string ToString()
    {
      LayoutRendererAttribute customAttribute = this.GetType().GetCustomAttribute<LayoutRendererAttribute>();
      return customAttribute != null ? string.Format("Layout Renderer: ${{{0}}}", (object) customAttribute.Name) : this.GetType().Name;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public string Render(LogEventInfo logEvent)
    {
      int capacity = this._maxRenderedLength;
      if (capacity > 16384)
        capacity = 16384;
      StringBuilder builder = new StringBuilder(capacity);
      this.RenderAppendBuilder(logEvent, builder);
      if (builder.Length > this._maxRenderedLength)
        this._maxRenderedLength = builder.Length;
      return builder.ToString();
    }

    void ISupportsInitialize.Initialize(LoggingConfiguration configuration)
    {
      this.Initialize(configuration);
    }

    void ISupportsInitialize.Close() => this.Close();

    internal void Initialize(LoggingConfiguration configuration)
    {
      if (this.LoggingConfiguration == null)
        this.LoggingConfiguration = configuration;
      if (this._isInitialized)
        return;
      this._isInitialized = true;
      this.InitializeLayoutRenderer();
    }

    internal void Close()
    {
      if (!this._isInitialized)
        return;
      this.LoggingConfiguration = (LoggingConfiguration) null;
      this._isInitialized = false;
      this.CloseLayoutRenderer();
    }

    internal void RenderAppendBuilder(LogEventInfo logEvent, StringBuilder builder)
    {
      if (!this._isInitialized)
      {
        this._isInitialized = true;
        this.InitializeLayoutRenderer();
      }
      try
      {
        this.Append(builder, logEvent);
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "Exception in layout renderer.");
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }

    protected abstract void Append(StringBuilder builder, LogEventInfo logEvent);

    protected virtual void InitializeLayoutRenderer()
    {
    }

    protected virtual void CloseLayoutRenderer()
    {
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.Close();
    }

    protected IFormatProvider GetFormatProvider(
      LogEventInfo logEvent,
      IFormatProvider layoutCulture = null)
    {
      IFormatProvider formatProvider = logEvent.FormatProvider ?? layoutCulture;
      if (formatProvider == null && this.LoggingConfiguration != null)
        formatProvider = (IFormatProvider) this.LoggingConfiguration.DefaultCultureInfo;
      return formatProvider;
    }

    protected CultureInfo GetCulture(LogEventInfo logEvent, CultureInfo layoutCulture = null)
    {
      if (!(logEvent.FormatProvider is CultureInfo cultureInfo))
        cultureInfo = layoutCulture;
      CultureInfo culture = cultureInfo;
      if (culture == null && this.LoggingConfiguration != null)
        culture = this.LoggingConfiguration.DefaultCultureInfo;
      return culture;
    }

    public static void Register<T>(string name) where T : LayoutRenderer
    {
      Type layoutRendererType = typeof (T);
      LayoutRenderer.Register(name, layoutRendererType);
    }

    public static void Register(string name, Type layoutRendererType)
    {
      ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition(name, layoutRendererType);
    }

    public static void Register(string name, Func<LogEventInfo, object> func)
    {
      LayoutRenderer.Register(name, (Func<LogEventInfo, LoggingConfiguration, object>) ((info, configuration) => func(info)));
    }

    public static void Register(
      string name,
      Func<LogEventInfo, LoggingConfiguration, object> func)
    {
      FuncLayoutRenderer renderer = new FuncLayoutRenderer(name, func);
      ConfigurationItemFactory.Default.GetLayoutRenderers().RegisterFuncLayout(name, renderer);
    }
  }
}
