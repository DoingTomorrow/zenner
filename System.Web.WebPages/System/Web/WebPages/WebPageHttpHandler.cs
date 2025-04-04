// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageHttpHandler
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Web.Infrastructure.DynamicValidationHelper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Web.SessionState;

#nullable disable
namespace System.Web.WebPages
{
  public class WebPageHttpHandler : IHttpHandler, IRequiresSessionState
  {
    internal const string StartPageFileName = "_PageStart";
    public static readonly string WebPagesVersionHeaderName = "X-AspNetWebPages-Version";
    private static readonly List<string> _supportedExtensions = new List<string>();
    internal static readonly string WebPagesVersion = WebPageHttpHandler.GetVersionString();
    private readonly WebPage _webPage;
    private readonly Lazy<WebPageRenderingBase> _startPage;

    public WebPageHttpHandler(WebPage webPage)
      : this(webPage, new Lazy<WebPageRenderingBase>((Func<WebPageRenderingBase>) (() => System.Web.WebPages.StartPage.GetStartPage((WebPageRenderingBase) webPage, "_PageStart", (IEnumerable<string>) WebPageHttpHandler.GetRegisteredExtensions()))))
    {
    }

    internal WebPageHttpHandler(WebPage webPage, Lazy<WebPageRenderingBase> startPage)
    {
      this._webPage = webPage != null ? webPage : throw new ArgumentNullException(nameof (webPage));
      this._startPage = startPage;
    }

    public static bool DisableWebPagesResponseHeader { get; set; }

    public virtual bool IsReusable => false;

    internal WebPage RequestedPage => this._webPage;

    internal WebPageRenderingBase StartPage => this._startPage.Value;

    internal static void AddVersionHeader(HttpContextBase httpContext)
    {
      if (WebPageHttpHandler.DisableWebPagesResponseHeader)
        return;
      httpContext.Response.AppendHeader(WebPageHttpHandler.WebPagesVersionHeaderName, WebPageHttpHandler.WebPagesVersion);
    }

    public static IHttpHandler CreateFromVirtualPath(string virtualPath)
    {
      return WebPageHttpHandler.CreateFromVirtualPath(virtualPath, (IVirtualPathFactory) VirtualPathFactoryManager.Instance);
    }

    internal static IHttpHandler CreateFromVirtualPath(
      string virtualPath,
      IVirtualPathFactory virtualPathFactory)
    {
      WebPage instance = virtualPathFactory.CreateInstance<WebPage>(virtualPath);
      if (instance == null)
        return virtualPathFactory.CreateInstance<IHttpHandler>(virtualPath);
      instance.TopLevelPage = true;
      instance.VirtualPath = virtualPath;
      instance.VirtualPathFactory = virtualPathFactory;
      return (IHttpHandler) new WebPageHttpHandler(instance);
    }

    public static ReadOnlyCollection<string> GetRegisteredExtensions()
    {
      return new ReadOnlyCollection<string>((IList<string>) WebPageHttpHandler._supportedExtensions);
    }

    private static string GetVersionString()
    {
      return new AssemblyName(typeof (WebPageHttpHandler).Assembly.FullName).Version.ToString(2);
    }

    private static bool HandleError(Exception e)
    {
      if (e is SecurityException)
        return false;
      throw new HttpUnhandledException((string) null, e);
    }

    internal static void GenerateSourceFilesHeader(WebPageContext context)
    {
      if (!context.SourceFiles.Any<string>())
        return;
      string str = "=?UTF-8?B?" + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join("|", (IEnumerable<string>) context.SourceFiles))) + "?=";
      context.HttpContext.Response.AddHeader("X-SourceFiles", str);
    }

    public virtual void ProcessRequest(HttpContext context) => this.ProcessRequestInternal(context);

    internal void ProcessRequestInternal(HttpContext context)
    {
      ValidationUtility.EnableDynamicValidation(context);
      context.Request.ValidateInput();
      this.ProcessRequestInternal((HttpContextBase) new HttpContextWrapper(context));
    }

    internal void ProcessRequestInternal(HttpContextBase httpContext)
    {
      try
      {
        WebPageHttpHandler.AddVersionHeader(httpContext);
        this._webPage.ExecutePageHierarchy(new WebPageContext()
        {
          HttpContext = httpContext
        }, httpContext.Response.Output, this.StartPage);
        if (!WebPageHttpHandler.ShouldGenerateSourceHeader(httpContext))
          return;
        WebPageHttpHandler.GenerateSourceFilesHeader(this._webPage.PageContext);
      }
      catch (Exception ex)
      {
        if (WebPageHttpHandler.HandleError(ex))
          return;
        throw;
      }
    }

    public static void RegisterExtension(string extension)
    {
      WebPageHttpHandler._supportedExtensions.Add(extension);
    }

    internal static bool ShouldGenerateSourceHeader(HttpContextBase context)
    {
      return context.Request.IsLocal;
    }
  }
}
