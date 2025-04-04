// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.RequestExtensions
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  public static class RequestExtensions
  {
    public static bool IsUrlLocalToHost(this HttpRequestBase request, string url)
    {
      if (url.IsEmpty())
        return false;
      if (url[0] == '/' && (url.Length == 1 || url[1] != '/' && url[1] != '\\'))
        return true;
      return url.Length > 1 && url[0] == '~' && url[1] == '/';
    }
  }
}
