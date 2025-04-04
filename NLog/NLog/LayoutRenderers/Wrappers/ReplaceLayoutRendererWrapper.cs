// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.ReplaceLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("replace")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class ReplaceLayoutRendererWrapper : WrapperLayoutRendererBase
  {
    private System.Text.RegularExpressions.Regex _regex;

    public string SearchFor { get; set; }

    public bool Regex { get; set; }

    public string ReplaceWith { get; set; }

    public string ReplaceGroupName { get; set; }

    public bool IgnoreCase { get; set; }

    public bool WholeWords { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      base.InitializeLayoutRenderer();
      string str = this.SearchFor;
      if (!this.Regex)
        str = System.Text.RegularExpressions.Regex.Escape(str);
      RegexOptions options = RegexOptions.Compiled;
      if (this.IgnoreCase)
        options |= RegexOptions.IgnoreCase;
      if (this.WholeWords)
        str = "\\b" + str + "\\b";
      this._regex = new System.Text.RegularExpressions.Regex(str, options);
    }

    protected override string Transform(string text)
    {
      ReplaceLayoutRendererWrapper.Replacer replacer = new ReplaceLayoutRendererWrapper.Replacer(text, this.ReplaceGroupName, this.ReplaceWith);
      return !string.IsNullOrEmpty(this.ReplaceGroupName) ? this._regex.Replace(text, new MatchEvaluator(replacer.EvaluateMatch)) : this._regex.Replace(text, this.ReplaceWith);
    }

    public static string ReplaceNamedGroup(
      string input,
      string groupName,
      string replacement,
      Match match)
    {
      StringBuilder stringBuilder = new StringBuilder(input);
      int index = match.Index;
      int length = match.Length;
      foreach (Capture capture in (IEnumerable<Capture>) match.Groups[groupName].Captures.OfType<Capture>().OrderByDescending<Capture, int>((Func<Capture, int>) (c => c.Index)))
      {
        if (capture != null)
        {
          length += replacement.Length - capture.Length;
          stringBuilder.Remove(capture.Index, capture.Length);
          stringBuilder.Insert(capture.Index, replacement);
        }
      }
      int startIndex = index + length;
      stringBuilder.Remove(startIndex, stringBuilder.Length - startIndex);
      stringBuilder.Remove(0, index);
      return stringBuilder.ToString();
    }

    [ThreadAgnostic]
    public class Replacer
    {
      private readonly string _text;
      private readonly string _replaceGroupName;
      private readonly string _replaceWith;

      internal Replacer(string text, string replaceGroupName, string replaceWith)
      {
        this._text = text;
        this._replaceGroupName = replaceGroupName;
        this._replaceWith = replaceWith;
      }

      internal string EvaluateMatch(Match match)
      {
        return ReplaceLayoutRendererWrapper.ReplaceNamedGroup(this._text, this._replaceGroupName, this._replaceWith, match);
      }
    }
  }
}
