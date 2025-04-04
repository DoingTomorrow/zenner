// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.UrlUtil
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Routing;

#nullable disable
namespace System.Web.WebPages
{
  internal static class UrlUtil
  {
    internal static string Url(string basePath, string path, params object[] pathParts)
    {
      if (basePath != null)
        path = VirtualPathUtility.Combine(basePath, path);
      path = VirtualPathUtility.ToAbsolute(path);
      return UrlUtil.BuildUrl(path, pathParts);
    }

    internal static string BuildUrl(string path, params object[] pathParts)
    {
      path = HttpUtility.UrlPathEncode(path);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object pathPart in pathParts)
      {
        if (UrlUtil.IsDisplayableType(pathPart.GetType()))
        {
          string str = Convert.ToString(pathPart, (IFormatProvider) CultureInfo.InvariantCulture);
          path = path + "/" + HttpUtility.UrlPathEncode(str);
        }
        else
        {
          foreach (KeyValuePair<string, object> keyValuePair in new RouteValueDictionary(pathPart))
          {
            if (stringBuilder.Length == 0)
              stringBuilder.Append('?');
            else
              stringBuilder.Append('&');
            string str = Convert.ToString(keyValuePair.Value, (IFormatProvider) CultureInfo.InvariantCulture);
            stringBuilder.Append(HttpUtility.UrlEncode(keyValuePair.Key)).Append('=').Append(HttpUtility.UrlEncode(str));
          }
        }
      }
      return path + (object) stringBuilder;
    }

    private static bool IsDisplayableType(Type t) => t.GetInterfaces().Length > 0;
  }
}
