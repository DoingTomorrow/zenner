// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.StringsProcessor
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public static class StringsProcessor
  {
    internal static ReplaceProcessedString CreateReplaceProcessedString(
      string stringToProcess,
      string replacePattern)
    {
      string str = stringToProcess;
      int num = 0;
      int startIndex = str.IndexOf('(');
      ReplaceProcessedString replaceProcessedString = new ReplaceProcessedString()
      {
        OriginalString = stringToProcess
      };
      for (; startIndex > 0; startIndex = str.IndexOf('('))
      {
        int endingBrace = StringsProcessor.FindEndingBrace(str.Substring(startIndex + 1, str.Length - startIndex - 1));
        if (endingBrace > 0)
        {
          string oldValue = str.Substring(startIndex, endingBrace + 2);
          str = str.Replace(oldValue, string.Format(replacePattern, (object) num));
          replaceProcessedString.ReplacementsDictionary.Add(string.Format(replacePattern, (object) num), oldValue);
          ++num;
        }
      }
      replaceProcessedString.ProcessedString = str;
      return replaceProcessedString;
    }

    internal static int FindStartingBrace(string target)
    {
      int length1 = target.LastIndexOf('(');
      int length2 = target.LastIndexOf(')');
      int num = 0;
      for (; length2 > length1 && length1 != -1; length1 = target.Substring(0, length1).LastIndexOf('('))
      {
        ++num;
        length2 = target.Substring(0, length2).LastIndexOf(')');
      }
      for (int index = 0; index < num; ++index)
        length1 = target.Substring(0, length1).LastIndexOf('(');
      return length1;
    }

    internal static int FindEndingBrace(string target)
    {
      int num1 = target.IndexOf('(');
      int endingBrace = target.IndexOf(')');
      int num2 = 0;
      for (; endingBrace > num1 && num1 != -1; num1 = target.IndexOf('(', num1 + 1))
      {
        ++num2;
        endingBrace = target.IndexOf(')', endingBrace + 1);
      }
      return endingBrace;
    }

    internal static IEnumerable<string> ExtractDistinctWhereClauses(ICriterion whereClause)
    {
      string input = string.Empty;
      string source = whereClause.ToString();
      int num1 = -1;
      for (int index = 0; index < source.Length; ++index)
      {
        char ch = source.ElementAt<char>(index);
        int num2;
        if (!ch.Equals('('))
        {
          ch = source.ElementAt<char>(index);
          num2 = !ch.Equals(')') ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 == 0)
        {
          input += source.Substring(num1 + 1, index - num1 - 1);
          num1 = index;
        }
      }
      if (num1 <= 0)
        input = source;
      string[] strArray = Regex.Split(input, "( and )|( or )", RegexOptions.IgnoreCase);
      List<string> distinctWhereClauses = new List<string>();
      foreach (string str in strArray)
        distinctWhereClauses.Add(str.Trim());
      return (IEnumerable<string>) distinctWhereClauses;
    }

    internal static bool ValueAppearsAsString(string operation, string value)
    {
      return !double.TryParse(value, out double _) && !operation.Equals("in", StringComparison.CurrentCultureIgnoreCase);
    }

    internal static string AdjustWhereClauseOperands(string clause)
    {
      bool flag = false;
      if (clause.StartsWith("not", StringComparison.InvariantCultureIgnoreCase))
      {
        clause = clause.Substring(4, clause.Length - 4).Trim();
        flag = true;
      }
      int length = clause.IndexOf(' ');
      int num = clause.IndexOf(' ', length + 1);
      string str1 = clause.Substring(0, length);
      string operation = clause.Substring(length + 1, num - length - 1);
      string str2 = clause.Substring(num + 1, clause.Length - (num + 1));
      return StringsProcessor.ValueAppearsAsString(operation, str2) ? string.Format("{0}{1} {2} '{3}'", flag ? (object) "not " : (object) "", (object) str1, (object) operation, (object) str2) : string.Format("{0}{1} {2} {3}", flag ? (object) "not " : (object) "", (object) str1, (object) operation, (object) str2);
    }

    internal static int SearchWordPositionOuterLevel(string word, string target)
    {
      for (int index = target.IndexOf(word, StringComparison.InvariantCultureIgnoreCase); index > 0; index = target.IndexOf(word, index + 1, StringComparison.InvariantCultureIgnoreCase))
      {
        if (StringsProcessor.CountStringOccurrences(target.Substring(0, index - 1), "(") == StringsProcessor.CountStringOccurrences(target.Substring(0, index - 1), ")"))
          return index;
      }
      return -1;
    }

    internal static int CountStringOccurrences(string text, string pattern)
    {
      int num1 = 0;
      int startIndex = 0;
      int num2;
      while ((num2 = text.IndexOf(pattern, startIndex)) != -1)
      {
        startIndex = num2 + pattern.Length;
        ++num1;
      }
      return num1;
    }
  }
}
