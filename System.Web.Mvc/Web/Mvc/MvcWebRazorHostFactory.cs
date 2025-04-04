// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MvcWebRazorHostFactory
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Razor;
using System.Web.WebPages.Razor;

#nullable disable
namespace System.Web.Mvc
{
  public class MvcWebRazorHostFactory : WebRazorHostFactory
  {
    public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
    {
      WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);
      return !host.IsSpecialPage ? (WebPageRazorHost) new MvcWebPageRazorHost(virtualPath, physicalPath) : host;
    }
  }
}
