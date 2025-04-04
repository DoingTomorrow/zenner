// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.PathHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  internal static class PathHelpers
  {
    private static UrlRewriterHelper _urlRewriterHelper = new UrlRewriterHelper();

    public static string GenerateClientUrl(HttpContextBase httpContext, string contentPath)
    {
      if (string.IsNullOrEmpty(contentPath))
        return contentPath;
      string query;
      contentPath = PathHelpers.StripQuery(contentPath, out query);
      return PathHelpers.GenerateClientUrlInternal(httpContext, contentPath) + query;
    }

    private static string GenerateClientUrlInternal(HttpContextBase httpContext, string contentPath)
    {
      if (string.IsNullOrEmpty(contentPath))
        return contentPath;
      if (contentPath[0] == '~')
      {
        string absolute = VirtualPathUtility.ToAbsolute(contentPath, httpContext.Request.ApplicationPath);
        string contentPath1 = httpContext.Response.ApplyAppPathModifier(absolute);
        return PathHelpers.GenerateClientUrlInternal(httpContext, contentPath1);
      }
      if (!PathHelpers._urlRewriterHelper.WasRequestRewritten(httpContext))
        return contentPath;
      string relativePath = PathHelpers.MakeRelative(httpContext.Request.Path, contentPath);
      return PathHelpers.MakeAbsolute(httpContext.Request.RawUrl, relativePath);
    }

    public static string MakeAbsolute(string basePath, string relativePath)
    {
      basePath = PathHelpers.StripQuery(basePath, out string _);
      return VirtualPathUtility.Combine(basePath, relativePath);
    }

    public static string MakeRelative(string fromPath, string toPath)
    {
      string str = VirtualPathUtility.MakeRelative(fromPath, toPath);
      if (string.IsNullOrEmpty(str) || str[0] == '?')
        str = "./" + str;
      return str;
    }

    private static string StripQuery(string path, out string query)
    {
      int num = path.IndexOf('?');
      if (num >= 0)
      {
        query = path.Substring(num);
        return path.Substring(0, num);
      }
      query = (string) null;
      return path;
    }

    internal static void ResetUrlRewriterHelper()
    {
      PathHelpers._urlRewriterHelper = new UrlRewriterHelper();
    }
  }
}
