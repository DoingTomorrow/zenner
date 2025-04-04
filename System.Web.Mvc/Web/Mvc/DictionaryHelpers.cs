// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DictionaryHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  internal static class DictionaryHelpers
  {
    public static IEnumerable<KeyValuePair<string, TValue>> FindKeysWithPrefix<TValue>(
      IDictionary<string, TValue> dictionary,
      string prefix)
    {
      TValue exactMatchValue;
      if (dictionary.TryGetValue(prefix, out exactMatchValue))
        yield return new KeyValuePair<string, TValue>(prefix, exactMatchValue);
      foreach (KeyValuePair<string, TValue> entry in (IEnumerable<KeyValuePair<string, TValue>>) dictionary)
      {
        string key = entry.Key;
        if (key.Length > prefix.Length && key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
          char charAfterPrefix = key[prefix.Length];
          switch (charAfterPrefix)
          {
            case '.':
            case '[':
              yield return entry;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public static bool DoesAnyKeyHavePrefix<TValue>(
      IDictionary<string, TValue> dictionary,
      string prefix)
    {
      return DictionaryHelpers.FindKeysWithPrefix<TValue>(dictionary, prefix).Any<KeyValuePair<string, TValue>>();
    }

    public static TValue GetOrDefault<TKey, TValue>(
      this IDictionary<TKey, TValue> dict,
      TKey key,
      TValue @default)
    {
      TValue obj;
      return dict.TryGetValue(key, out obj) ? obj : @default;
    }
  }
}
