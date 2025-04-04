// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ColoredConsoleTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets
{
  [Target("ColoredConsole")]
  public sealed class ColoredConsoleTarget : TargetWithLayoutHeaderAndFooter
  {
    private bool _pauseLogging;
    private static readonly IList<ConsoleRowHighlightingRule> DefaultConsoleRowHighlightingRules = (IList<ConsoleRowHighlightingRule>) new List<ConsoleRowHighlightingRule>()
    {
      new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Fatal", ConsoleOutputColor.Red, ConsoleOutputColor.NoChange),
      new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Error", ConsoleOutputColor.Yellow, ConsoleOutputColor.NoChange),
      new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Warn", ConsoleOutputColor.Magenta, ConsoleOutputColor.NoChange),
      new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Info", ConsoleOutputColor.White, ConsoleOutputColor.NoChange),
      new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Debug", ConsoleOutputColor.Gray, ConsoleOutputColor.NoChange),
      new ConsoleRowHighlightingRule((ConditionExpression) "level == LogLevel.Trace", ConsoleOutputColor.DarkGray, ConsoleOutputColor.NoChange)
    };
    private Encoding _encoding;

    public ColoredConsoleTarget()
    {
      this.WordHighlightingRules = (IList<ConsoleWordHighlightingRule>) new List<ConsoleWordHighlightingRule>();
      this.RowHighlightingRules = (IList<ConsoleRowHighlightingRule>) new List<ConsoleRowHighlightingRule>();
      this.UseDefaultRowHighlightingRules = true;
      this._pauseLogging = false;
      this.DetectConsoleAvailable = false;
      this.OptimizeBufferReuse = true;
    }

    public ColoredConsoleTarget(string name)
      : this()
    {
      this.Name = name;
    }

    [DefaultValue(false)]
    public bool ErrorStream { get; set; }

    [DefaultValue(true)]
    public bool UseDefaultRowHighlightingRules { get; set; }

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

    [ArrayParameter(typeof (ConsoleRowHighlightingRule), "highlight-row")]
    public IList<ConsoleRowHighlightingRule> RowHighlightingRules { get; private set; }

    [ArrayParameter(typeof (ConsoleWordHighlightingRule), "highlight-word")]
    public IList<ConsoleWordHighlightingRule> WordHighlightingRules { get; private set; }

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
      LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
      this.WriteToOutput(nullEvent, this.RenderLogEvent(this.Header, nullEvent));
    }

    protected override void CloseTarget()
    {
      if (this.Footer != null)
      {
        LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
        this.WriteToOutput(nullEvent, this.RenderLogEvent(this.Footer, nullEvent));
      }
      base.CloseTarget();
    }

    protected override void Write(LogEventInfo logEvent)
    {
      if (this._pauseLogging)
        return;
      this.WriteToOutput(logEvent, this.RenderLogEvent(this.Layout, logEvent));
    }

    private void WriteToOutput(LogEventInfo logEvent, string message)
    {
      ConsoleColor foregroundColor = Console.ForegroundColor;
      ConsoleColor backgroundColor = Console.BackgroundColor;
      bool flag1 = false;
      bool flag2 = false;
      try
      {
        ConsoleRowHighlightingRule highlightingRule1 = this.GetMatchingRowHighlightingRule(logEvent);
        flag1 = ColoredConsoleTarget.IsColorChange(highlightingRule1.ForegroundColor, foregroundColor);
        if (flag1)
          Console.ForegroundColor = (ConsoleColor) highlightingRule1.ForegroundColor;
        flag2 = ColoredConsoleTarget.IsColorChange(highlightingRule1.BackgroundColor, backgroundColor);
        if (flag2)
          Console.BackgroundColor = (ConsoleColor) highlightingRule1.BackgroundColor;
        try
        {
          TextWriter output = this.ErrorStream ? Console.Error : Console.Out;
          if (this.WordHighlightingRules.Count == 0)
          {
            output.WriteLine(message);
          }
          else
          {
            message = message.Replace("\a", "\a\a");
            foreach (ConsoleWordHighlightingRule highlightingRule2 in (IEnumerable<ConsoleWordHighlightingRule>) this.WordHighlightingRules)
              message = highlightingRule2.ReplaceWithEscapeSequences(message);
            ColoredConsoleTarget.ColorizeEscapeSequences(output, message, new ColoredConsoleTarget.ColorPair(Console.ForegroundColor, Console.BackgroundColor), new ColoredConsoleTarget.ColorPair(foregroundColor, backgroundColor));
            output.WriteLine();
            flag1 = flag2 = true;
          }
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
      finally
      {
        if (flag1)
          Console.ForegroundColor = foregroundColor;
        if (flag2)
          Console.BackgroundColor = backgroundColor;
      }
    }

    private ConsoleRowHighlightingRule GetMatchingRowHighlightingRule(LogEventInfo logEvent)
    {
      foreach (ConsoleRowHighlightingRule highlightingRule in (IEnumerable<ConsoleRowHighlightingRule>) this.RowHighlightingRules)
      {
        if (highlightingRule.CheckCondition(logEvent))
          return highlightingRule;
      }
      if (this.UseDefaultRowHighlightingRules)
      {
        foreach (ConsoleRowHighlightingRule highlightingRule in (IEnumerable<ConsoleRowHighlightingRule>) ColoredConsoleTarget.DefaultConsoleRowHighlightingRules)
        {
          if (highlightingRule.CheckCondition(logEvent))
            return highlightingRule;
        }
      }
      return ConsoleRowHighlightingRule.Default;
    }

    private static bool IsColorChange(ConsoleOutputColor targetColor, ConsoleColor oldColor)
    {
      return targetColor != ConsoleOutputColor.NoChange && targetColor != (ConsoleOutputColor) oldColor;
    }

    private static void ColorizeEscapeSequences(
      TextWriter output,
      string message,
      ColoredConsoleTarget.ColorPair startingColor,
      ColoredConsoleTarget.ColorPair defaultColor)
    {
      Stack<ColoredConsoleTarget.ColorPair> colorPairStack = new Stack<ColoredConsoleTarget.ColorPair>();
      colorPairStack.Push(startingColor);
      int startIndex = 0;
      while (startIndex < message.Length)
      {
        int index = startIndex;
        while (index < message.Length && message[index] >= ' ')
          ++index;
        if (index != startIndex)
          output.Write(message.Substring(startIndex, index - startIndex));
        if (index >= message.Length)
        {
          startIndex = index;
          break;
        }
        char ch = message[index];
        char minValue = char.MinValue;
        if (index + 1 < message.Length)
          minValue = message[index + 1];
        ColoredConsoleTarget.ColorPair colorPair;
        if (ch == '\a' && minValue == '\a')
        {
          output.Write('\a');
          startIndex = index + 2;
        }
        else if (ch == '\r' || ch == '\n')
        {
          Console.ForegroundColor = defaultColor.ForegroundColor;
          Console.BackgroundColor = defaultColor.BackgroundColor;
          output.Write(ch);
          colorPair = colorPairStack.Peek();
          Console.ForegroundColor = colorPair.ForegroundColor;
          colorPair = colorPairStack.Peek();
          Console.BackgroundColor = colorPair.BackgroundColor;
          startIndex = index + 1;
        }
        else if (ch == '\a')
        {
          if (minValue == 'X')
          {
            colorPairStack.Pop();
            colorPair = colorPairStack.Peek();
            Console.ForegroundColor = colorPair.ForegroundColor;
            colorPair = colorPairStack.Peek();
            Console.BackgroundColor = colorPair.BackgroundColor;
            startIndex = index + 2;
          }
          else
          {
            ConsoleOutputColor consoleOutputColor1 = (ConsoleOutputColor) ((int) minValue - 65);
            ConsoleOutputColor consoleOutputColor2 = (ConsoleOutputColor) ((int) message[index + 2] - 65);
            if (consoleOutputColor1 != ConsoleOutputColor.NoChange)
              Console.ForegroundColor = (ConsoleColor) consoleOutputColor1;
            if (consoleOutputColor2 != ConsoleOutputColor.NoChange)
              Console.BackgroundColor = (ConsoleColor) consoleOutputColor2;
            colorPairStack.Push(new ColoredConsoleTarget.ColorPair(Console.ForegroundColor, Console.BackgroundColor));
            startIndex = index + 3;
          }
        }
        else
        {
          output.Write(ch);
          startIndex = index + 1;
        }
      }
      if (startIndex >= message.Length)
        return;
      output.Write(message.Substring(startIndex));
    }

    internal struct ColorPair
    {
      private readonly ConsoleColor _foregroundColor;
      private readonly ConsoleColor _backgroundColor;

      internal ColorPair(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
      {
        this._foregroundColor = foregroundColor;
        this._backgroundColor = backgroundColor;
      }

      internal ConsoleColor BackgroundColor => this._backgroundColor;

      internal ConsoleColor ForegroundColor => this._foregroundColor;
    }
  }
}
