// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleWordHighlightingRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace NLog.Targets
{
  [NLogConfigurationItem]
  public class ConsoleWordHighlightingRule
  {
    private System.Text.RegularExpressions.Regex _compiledRegex;

    public ConsoleWordHighlightingRule()
    {
      this.BackgroundColor = ConsoleOutputColor.NoChange;
      this.ForegroundColor = ConsoleOutputColor.NoChange;
    }

    public ConsoleWordHighlightingRule(
      string text,
      ConsoleOutputColor foregroundColor,
      ConsoleOutputColor backgroundColor)
    {
      this.Text = text;
      this.ForegroundColor = foregroundColor;
      this.BackgroundColor = backgroundColor;
    }

    public string Regex { get; set; }

    [DefaultValue(false)]
    public bool CompileRegex { get; set; }

    public string Text { get; set; }

    [DefaultValue(false)]
    public bool WholeWords { get; set; }

    [DefaultValue(false)]
    public bool IgnoreCase { get; set; }

    [DefaultValue("NoChange")]
    public ConsoleOutputColor ForegroundColor { get; set; }

    [DefaultValue("NoChange")]
    public ConsoleOutputColor BackgroundColor { get; set; }

    public System.Text.RegularExpressions.Regex CompiledRegex
    {
      get
      {
        if (this._compiledRegex == null)
        {
          string regexExpression = this.GetRegexExpression();
          if (regexExpression == null)
            return (System.Text.RegularExpressions.Regex) null;
          RegexOptions regexOptions = this.GetRegexOptions(RegexOptions.Compiled);
          this._compiledRegex = new System.Text.RegularExpressions.Regex(regexExpression, regexOptions);
        }
        return this._compiledRegex;
      }
    }

    private RegexOptions GetRegexOptions(RegexOptions regexOptions)
    {
      if (this.IgnoreCase)
        regexOptions |= RegexOptions.IgnoreCase;
      return regexOptions;
    }

    private string GetRegexExpression()
    {
      string regexExpression = this.Regex;
      if (regexExpression == null && this.Text != null)
      {
        regexExpression = System.Text.RegularExpressions.Regex.Escape(this.Text);
        if (this.WholeWords)
          regexExpression = "\\b" + regexExpression + "\\b";
      }
      return regexExpression;
    }

    private string MatchEvaluator(Match m)
    {
      StringBuilder stringBuilder = new StringBuilder(m.Value.Length + 5);
      stringBuilder.Append('\a');
      stringBuilder.Append((char) (this.ForegroundColor + 65));
      stringBuilder.Append((char) (this.BackgroundColor + 65));
      stringBuilder.Append(m.Value);
      stringBuilder.Append('\a');
      stringBuilder.Append('X');
      return stringBuilder.ToString();
    }

    internal string ReplaceWithEscapeSequences(string message)
    {
      if (this.CompileRegex)
      {
        System.Text.RegularExpressions.Regex compiledRegex = this.CompiledRegex;
        return compiledRegex == null ? message : compiledRegex.Replace(message, new System.Text.RegularExpressions.MatchEvaluator(this.MatchEvaluator));
      }
      string regexExpression = this.GetRegexExpression();
      if (regexExpression == null)
        return message;
      RegexOptions regexOptions = this.GetRegexOptions(RegexOptions.None);
      return System.Text.RegularExpressions.Regex.Replace(message, regexExpression, new System.Text.RegularExpressions.MatchEvaluator(this.MatchEvaluator), regexOptions);
    }
  }
}
