// Decompiled with JetBrains decompiler
// Type: Microsoft.Internal.Web.Utils.VirtualPathUtilityWrapper
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Web;

#nullable disable
namespace Microsoft.Internal.Web.Utils
{
  internal sealed class VirtualPathUtilityWrapper : IVirtualPathUtility
  {
    public string Combine(string basePath, string relativePath)
    {
      return VirtualPathUtility.Combine(basePath, relativePath);
    }

    public string ToAbsolute(string virtualPath) => VirtualPathUtility.ToAbsolute(virtualPath);
  }
}
