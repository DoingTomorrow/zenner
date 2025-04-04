// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValueProviderUtil
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc
{
  internal static class ValueProviderUtil
  {
    public static bool CollectionContainsPrefix(IEnumerable<string> collection, string prefix)
    {
      foreach (string str in collection)
      {
        if (str != null)
        {
          if (prefix.Length == 0)
            return true;
          if (str.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
          {
            if (str.Length == prefix.Length)
              return true;
            switch (str[prefix.Length])
            {
              case '.':
              case '[':
                return true;
              default:
                continue;
            }
          }
        }
      }
      return false;
    }
  }
}
