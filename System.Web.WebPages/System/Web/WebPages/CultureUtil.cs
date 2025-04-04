// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.CultureUtil
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

#nullable disable
namespace System.Web.WebPages
{
  internal static class CultureUtil
  {
    internal static void SetCulture(Thread thread, HttpContextBase context, string cultureName)
    {
      CultureInfo culture = CultureUtil.GetCulture(context, cultureName);
      if (culture == null)
        return;
      thread.CurrentCulture = culture;
    }

    internal static void SetUICulture(Thread thread, HttpContextBase context, string cultureName)
    {
      CultureInfo culture = CultureUtil.GetCulture(context, cultureName);
      if (culture == null)
        return;
      thread.CurrentUICulture = culture;
    }

    private static CultureInfo GetCulture(HttpContextBase context, string cultureName)
    {
      return cultureName.Equals("auto", StringComparison.OrdinalIgnoreCase) ? CultureUtil.DetermineAutoCulture(context) : CultureInfo.GetCultureInfo(cultureName);
    }

    private static CultureInfo DetermineAutoCulture(HttpContextBase context)
    {
      HttpRequestBase request = context.Request;
      CultureInfo autoCulture = (CultureInfo) null;
      if (request.UserLanguages != null)
      {
        string name = ((IEnumerable<string>) request.UserLanguages).FirstOrDefault<string>();
        if (!string.IsNullOrWhiteSpace(name))
        {
          int length = name.IndexOf(';');
          if (length != -1)
            name = name.Substring(0, length);
          try
          {
            autoCulture = new CultureInfo(name);
          }
          catch (CultureNotFoundException ex)
          {
          }
        }
      }
      return autoCulture;
    }
  }
}
