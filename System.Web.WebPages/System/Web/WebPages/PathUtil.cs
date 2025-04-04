// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.PathUtil
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.IO;

#nullable disable
namespace System.Web.WebPages
{
  internal static class PathUtil
  {
    internal static string GetExtension(string path)
    {
      if (string.IsNullOrEmpty(path))
        return path;
      int length = path.Length;
      while (--length >= 0)
      {
        char ch = path[length];
        if (ch == '.')
        {
          if (length != path.Length - 1)
            return path.Substring(length);
          break;
        }
        if ((int) ch == (int) Path.DirectorySeparatorChar || (int) ch == (int) Path.AltDirectorySeparatorChar)
          break;
      }
      return string.Empty;
    }

    internal static bool IsWithinAppRoot(string appDomainAppVirtualPath, string virtualPath)
    {
      if (appDomainAppVirtualPath == null)
        return true;
      string virtualPath1 = virtualPath;
      if (!VirtualPathUtility.IsAbsolute(virtualPath1))
        virtualPath1 = VirtualPathUtility.ToAbsolute(virtualPath1);
      return VirtualPathUtility.ToAppRelative(virtualPath1, appDomainAppVirtualPath) != null;
    }

    internal static bool IsSimpleName(string path)
    {
      return !VirtualPathUtility.IsAbsolute(path) && !VirtualPathUtility.IsAppRelative(path) && !path.StartsWith(".", StringComparison.OrdinalIgnoreCase);
    }
  }
}
