// Decompiled with JetBrains decompiler
// Type: System.Web.PrefixContainer
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web
{
  internal class PrefixContainer
  {
    private readonly ICollection<string> _originalValues;
    private readonly string[] _sortedValues;

    internal PrefixContainer(ICollection<string> values)
    {
      this._originalValues = values != null ? values : throw new ArgumentNullException(nameof (values));
      this._sortedValues = values.Where<string>((Func<string, bool>) (val => val != null)).ToArray<string>();
      Array.Sort<string>(this._sortedValues, (IComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    internal bool ContainsPrefix(string prefix)
    {
      switch (prefix)
      {
        case null:
          throw new ArgumentNullException(nameof (prefix));
        case "":
          return this._sortedValues.Length > 0;
        default:
          return Array.BinarySearch<string>(this._sortedValues, prefix, (IComparer<string>) new PrefixContainer.PrefixComparer(prefix)) > -1;
      }
    }

    internal IDictionary<string, string> GetKeysFromPrefix(string prefix)
    {
      IDictionary<string, string> results = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (string originalValue in (IEnumerable<string>) this._originalValues)
      {
        if (originalValue != null && originalValue.Length != prefix.Length)
        {
          if (prefix.Length == 0)
            PrefixContainer.GetKeyFromEmptyPrefix(originalValue, results);
          else if (originalValue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            PrefixContainer.GetKeyFromNonEmptyPrefix(prefix, originalValue, results);
        }
      }
      return results;
    }

    private static void GetKeyFromEmptyPrefix(string entry, IDictionary<string, string> results)
    {
      int val1 = entry.IndexOf('.');
      int val2 = entry.IndexOf('[');
      int length = -1;
      if (val1 == -1)
      {
        if (val2 != -1)
          length = val2;
      }
      else
        length = val2 != -1 ? Math.Min(val1, val2) : val1;
      string key = length == -1 ? entry : entry.Substring(0, length);
      results[key] = key;
    }

    private static void GetKeyFromNonEmptyPrefix(
      string prefix,
      string entry,
      IDictionary<string, string> results)
    {
      int startIndex = prefix.Length + 1;
      string key;
      string str;
      switch (entry[prefix.Length])
      {
        case '.':
          int length = entry.IndexOf('.', startIndex);
          if (length == -1)
            length = entry.Length;
          key = entry.Substring(startIndex, length - startIndex);
          str = entry.Substring(0, length);
          break;
        case '[':
          int num = entry.IndexOf(']', startIndex);
          if (num == -1)
            return;
          key = entry.Substring(startIndex, num - startIndex);
          str = entry.Substring(0, num + 1);
          break;
        default:
          return;
      }
      if (results.ContainsKey(key))
        return;
      results.Add(key, str);
    }

    internal static bool IsPrefixMatch(string prefix, string testString)
    {
      if (testString == null)
        return false;
      if (prefix.Length == 0)
        return true;
      if (prefix.Length > testString.Length || !testString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        return false;
      if (testString.Length == prefix.Length)
        return true;
      switch (testString[prefix.Length])
      {
        case '.':
        case '[':
          return true;
        default:
          return false;
      }
    }

    private class PrefixComparer : IComparer<string>
    {
      private string _prefix;

      public PrefixComparer(string prefix) => this._prefix = prefix;

      public int Compare(string x, string y)
      {
        return PrefixContainer.IsPrefixMatch(this._prefix, object.ReferenceEquals((object) x, (object) this._prefix) ? y : x) ? 0 : StringComparer.OrdinalIgnoreCase.Compare(x, y);
      }
    }
  }
}
