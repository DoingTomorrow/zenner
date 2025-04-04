// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.NameValueCollectionExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace System.Web.Mvc
{
  public static class NameValueCollectionExtensions
  {
    public static void CopyTo(
      this NameValueCollection collection,
      IDictionary<string, object> destination)
    {
      collection.CopyTo(destination, false);
    }

    public static void CopyTo(
      this NameValueCollection collection,
      IDictionary<string, object> destination,
      bool replaceEntries)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof (collection));
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      foreach (string key in collection.Keys)
      {
        if (replaceEntries || !destination.ContainsKey(key))
          destination[key] = (object) collection[key];
      }
    }
  }
}
