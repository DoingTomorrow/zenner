// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.SimpleLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

#nullable disable
namespace NLog.Layouts
{
  [Layout("SimpleLayout")]
  [NLog.Config.ThreadAgnostic]
  [NLog.Config.ThreadSafe]
  [AppDomainFixedOutput]
  public class SimpleLayout : Layout, IUsesStackTrace
  {
    private string _fixedText;
    private string _layoutText;
    private ConfigurationItemFactory _configurationItemFactory;

    public SimpleLayout()
      : this(string.Empty)
    {
    }

    public SimpleLayout(string txt)
      : this(txt, ConfigurationItemFactory.Default)
    {
    }

    public SimpleLayout(string txt, ConfigurationItemFactory configurationItemFactory)
    {
      this._configurationItemFactory = configurationItemFactory;
      this.Text = txt;
    }

    internal SimpleLayout(
      LayoutRenderer[] renderers,
      string text,
      ConfigurationItemFactory configurationItemFactory)
    {
      this._configurationItemFactory = configurationItemFactory;
      this.SetRenderers(renderers, text);
    }

    public string OriginalText { get; private set; }

    public string Text
    {
      get => this._layoutText;
      set
      {
        this.OriginalText = value;
        LayoutRenderer[] renderers;
        string text;
        if (value == null)
        {
          renderers = ArrayHelper.Empty<LayoutRenderer>();
          text = string.Empty;
        }
        else
          renderers = LayoutParser.CompileLayout(this._configurationItemFactory, new SimpleStringReader(value), false, out text);
        this.SetRenderers(renderers, text);
      }
    }

    public bool IsFixedText => this._fixedText != null;

    public string FixedText => this._fixedText;

    public ReadOnlyCollection<LayoutRenderer> Renderers { get; private set; }

    public new StackTraceUsage StackTraceUsage => base.StackTraceUsage;

    public static implicit operator SimpleLayout(string text)
    {
      return text == null ? (SimpleLayout) null : new SimpleLayout(text);
    }

    public static string Escape(string text) => text.Replace("${", "${literal:text=${}");

    public static string Evaluate(string text, LogEventInfo logEvent)
    {
      return new SimpleLayout(text).Render(logEvent);
    }

    public static string Evaluate(string text)
    {
      return SimpleLayout.Evaluate(text, LogEventInfo.CreateNullEvent());
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.Text))
      {
        ReadOnlyCollection<LayoutRenderer> renderers = this.Renderers;
        // ISSUE: explicit non-virtual call
        if ((renderers != null ? (__nonvirtual (renderers.Count) > 0 ? 1 : 0) : 0) != 0)
          return this.ToStringWithNestedItems<LayoutRenderer>((IList<LayoutRenderer>) this.Renderers, (Func<LayoutRenderer, string>) (r => r.ToString()));
      }
      return "'" + this.Text + "'";
    }

    internal void SetRenderers(LayoutRenderer[] renderers, string text)
    {
      this.Renderers = new ReadOnlyCollection<LayoutRenderer>((IList<LayoutRenderer>) renderers);
      this._fixedText = this.Renderers.Count != 1 || !(this.Renderers[0] is LiteralLayoutRenderer) ? (string) null : ((LiteralLayoutRenderer) this.Renderers[0]).Text;
      this._layoutText = text;
      if (this.LoggingConfiguration == null)
        return;
      this.PerformObjectScanning();
    }

    protected override void InitializeLayout()
    {
      for (int index = 0; index < this.Renderers.Count; ++index)
      {
        LayoutRenderer renderer = this.Renderers[index];
        try
        {
          renderer.Initialize(this.LoggingConfiguration);
        }
        catch (Exception ex)
        {
          if (InternalLogger.IsWarnEnabled || InternalLogger.IsErrorEnabled)
            InternalLogger.Warn(ex, "Exception in '{0}.InitializeLayout()'", (object) renderer.GetType().FullName);
          if (ex.MustBeRethrown())
            throw;
        }
      }
      base.InitializeLayout();
    }

    internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
    {
      if (this.ThreadAgnostic)
        return;
      this.RenderAppendBuilder(logEvent, target, true);
    }

    protected override string GetFormattedMessage(LogEventInfo logEvent)
    {
      return this.IsFixedText ? this._fixedText : this.RenderAllocateBuilder(logEvent);
    }

    private void RenderAllRenderers(LogEventInfo logEvent, StringBuilder target)
    {
      for (int index = 0; index < this.Renderers.Count; ++index)
      {
        LayoutRenderer renderer = this.Renderers[index];
        try
        {
          renderer.RenderAppendBuilder(logEvent, target);
        }
        catch (Exception ex)
        {
          if (InternalLogger.IsWarnEnabled || InternalLogger.IsErrorEnabled)
            InternalLogger.Warn(ex, "Exception in '{0}.Append()'", (object) renderer.GetType().FullName);
          if (ex.MustBeRethrown())
            throw;
        }
      }
    }

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      if (this.IsFixedText)
        target.Append(this._fixedText);
      else
        this.RenderAllRenderers(logEvent, target);
    }
  }
}
