// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.BuildManagerExceptionUtil
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Web.Infrastructure;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  internal static class BuildManagerExceptionUtil
  {
    internal static bool IsUnsupportedExtensionError(HttpException e)
    {
      for (Exception exception = (Exception) e; exception != null; exception = exception.InnerException)
      {
        MethodBase targetSite = exception.TargetSite;
        if (targetSite != (MethodBase) null && targetSite.Name == "GetBuildProviderTypeFromExtension" && targetSite.DeclaringType != (Type) null && targetSite.DeclaringType.Name == "CompilationUtil")
          return true;
      }
      return false;
    }

    internal static void ThrowIfUnsupportedExtension(string virtualPath, HttpException e)
    {
      if (BuildManagerExceptionUtil.IsUnsupportedExtensionError(e))
        throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_FileNotSupported, new object[2]
        {
          (object) Path.GetExtension(virtualPath),
          (object) virtualPath
        }));
    }

    internal static void ThrowIfCodeDomDefinedExtension(string virtualPath, HttpException e)
    {
      if (!(e is HttpCompileException))
        return;
      string extension = Path.GetExtension(virtualPath);
      if (InfrastructureHelper.IsCodeDomDefinedExtension(extension))
        throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.WebPage_FileNotSupported, new object[2]
        {
          (object) extension,
          (object) virtualPath
        }));
    }
  }
}
