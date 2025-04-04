// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets
{
  [Target("Console")]
  public sealed class ConsoleTarget : TargetWithLayoutHeaderAndFooter
  {
    private bool _pauseLogging;
    private Encoding _encoding;

    [DefaultValue(false)]
    public bool Error { get; set; }

    public Encoding Encoding
    {
      get
      {
        return ConsoleTargetHelper.GetConsoleOutputEncoding(this._encoding, this.IsInitialized, this._pauseLogging);
      }
      set
      {
        if (!ConsoleTargetHelper.SetConsoleOutputEncoding(value, this.IsInitialized, this._pauseLogging))
          return;
        this._encoding = value;
      }
    }

    [DefaultValue(false)]
    public bool DetectConsoleAvailable { get; set; }

    public ConsoleTarget()
    {
      this._pauseLogging = false;
      this.DetectConsoleAvailable = false;
      this.OptimizeBufferReuse = true;
    }

    public ConsoleTarget(string name)
      : this()
    {
      this.Name = name;
    }

    protected override void InitializeTarget()
    {
      this._pauseLogging = false;
      if (this.DetectConsoleAvailable)
      {
        string reason;
        this._pauseLogging = !ConsoleTargetHelper.IsConsoleAvailable(out reason);
        if (this._pauseLogging)
          InternalLogger.Info<string>("Console has been detected as turned off. Disable DetectConsoleAvailable to skip detection. Reason: {0}", reason);
      }
      if (this._encoding != null && !this._pauseLogging)
        Console.OutputEncoding = this._encoding;
      base.InitializeTarget();
      if (this.Header == null)
        return;
      this.WriteToOutput(this.RenderLogEvent(this.Header, LogEventInfo.CreateNullEvent()));
    }

    protected override void CloseTarget()
    {
      if (this.Footer != null)
        this.WriteToOutput(this.RenderLogEvent(this.Footer, LogEventInfo.CreateNullEvent()));
      base.CloseTarget();
    }

    protected override void Write(LogEventInfo logEvent)
    {
      if (this._pauseLogging)
        return;
      this.WriteToOutput(this.RenderLogEvent(this.Layout, logEvent));
    }

    private void WriteToOutput(string textLine)
    {
      if (this._pauseLogging)
        return;
      TextWriter output = this.GetOutput();
      try
      {
        output.WriteLine(textLine);
      }
      catch (IndexOutOfRangeException ex)
      {
        this._pauseLogging = true;
        InternalLogger.Warn((Exception) ex, "An IndexOutOfRangeException has been thrown and this is probably due to a race condition.Logging to the console will be paused. Enable by reloading the config or re-initialize the targets");
      }
      catch (ArgumentOutOfRangeException ex)
      {
        this._pauseLogging = true;
        InternalLogger.Warn((Exception) ex, "An ArgumentOutOfRangeException has been thrown and this is probably due to a race condition.Logging to the console will be paused. Enable by reloading the config or re-initialize the targets");
      }
    }

    private TextWriter GetOutput() => !this.Error ? Console.Out : Console.Error;
  }
}
