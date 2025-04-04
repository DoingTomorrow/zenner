// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HttpRequestExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public static class HttpRequestExtensions
  {
    internal const string XHttpMethodOverrideKey = "X-HTTP-Method-Override";

    public static string GetHttpMethodOverride(this HttpRequestBase request)
    {
      string a1 = request != null ? request.HttpMethod : throw new ArgumentNullException(nameof (request));
      if (!string.Equals(a1, "POST", StringComparison.OrdinalIgnoreCase))
        return a1;
      string a2 = (string) null;
      string header = request.Headers["X-HTTP-Method-Override"];
      if (!string.IsNullOrEmpty(header))
      {
        a2 = header;
      }
      else
      {
        string str1 = request.Form["X-HTTP-Method-Override"];
        if (!string.IsNullOrEmpty(str1))
        {
          a2 = str1;
        }
        else
        {
          string str2 = request.QueryString["X-HTTP-Method-Override"];
          if (!string.IsNullOrEmpty(str2))
            a2 = str2;
        }
      }
      if (a2 != null && !string.Equals(a2, "GET", StringComparison.OrdinalIgnoreCase) && !string.Equals(a2, "POST", StringComparison.OrdinalIgnoreCase))
        a1 = a2;
      return a1;
    }
  }
}
