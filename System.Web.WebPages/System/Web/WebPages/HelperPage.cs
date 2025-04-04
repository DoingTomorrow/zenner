// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.HelperPage
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.WebPages.Html;
using System.Web.WebPages.Instrumentation;

#nullable disable
namespace System.Web.WebPages
{
  public class HelperPage
  {
    private static WebPageContext _pageContext;
    private static InstrumentationService _instrumentationService = (InstrumentationService) null;

    private static InstrumentationService InstrumentationService
    {
      get
      {
        if (HelperPage._instrumentationService == null)
          HelperPage._instrumentationService = new InstrumentationService();
        return HelperPage._instrumentationService;
      }
    }

    public static HttpContextBase Context
    {
      get => (HttpContextBase) new HttpContextWrapper(HttpContext.Current);
    }

    public static WebPageRenderingBase CurrentPage => HelperPage.PageContext.Page;

    public static object Page => HelperPage.CurrentPage.Page;

    public static object Model
    {
      get => !(HelperPage.CurrentPage is WebPage currentPage) ? (object) null : currentPage.Model;
    }

    public static ModelStateDictionary ModelState
    {
      get
      {
        return !(HelperPage.CurrentPage is WebPage currentPage) ? (ModelStateDictionary) null : currentPage.ModelState;
      }
    }

    public static HtmlHelper Html
    {
      get
      {
        return !(HelperPage.CurrentPage is WebPage currentPage) ? (HtmlHelper) null : currentPage.Html;
      }
    }

    public static WebPageContext PageContext
    {
      get => HelperPage._pageContext ?? WebPageContext.Current;
      set => HelperPage._pageContext = value;
    }

    public static HttpApplicationStateBase AppState
    {
      get
      {
        return HelperPage.Context != null ? HelperPage.Context.Application : (HttpApplicationStateBase) null;
      }
    }

    public static object App => HelperPage.CurrentPage.App;

    public static string VirtualPath => HelperPage.PageContext.Page.VirtualPath;

    public static Cache Cache
    {
      get => HelperPage.Context != null ? HelperPage.Context.Cache : (Cache) null;
    }

    public static HttpRequestBase Request
    {
      get => HelperPage.Context != null ? HelperPage.Context.Request : (HttpRequestBase) null;
    }

    public static HttpResponseBase Response
    {
      get => HelperPage.Context != null ? HelperPage.Context.Response : (HttpResponseBase) null;
    }

    public static HttpServerUtilityBase Server
    {
      get => HelperPage.Context != null ? HelperPage.Context.Server : (HttpServerUtilityBase) null;
    }

    public static HttpSessionStateBase Session
    {
      get => HelperPage.Context != null ? HelperPage.Context.Session : (HttpSessionStateBase) null;
    }

    public static IList<string> UrlData => HelperPage.CurrentPage.UrlData;

    public static IPrincipal User => HelperPage.CurrentPage.User;

    public static bool IsPost => HelperPage.CurrentPage.IsPost;

    public static bool IsAjax => HelperPage.CurrentPage.IsAjax;

    public static IDictionary<object, object> PageData => HelperPage.PageContext.PageData;

    protected static string HelperVirtualPath { get; set; }

    public static string Href(string path, params object[] pathParts)
    {
      return HelperPage.CurrentPage.Href(path, pathParts);
    }

    public static void WriteTo(TextWriter writer, object value)
    {
      WebPageExecutingBase.WriteTo(writer, value);
    }

    public static void WriteLiteralTo(TextWriter writer, object value)
    {
      WebPageExecutingBase.WriteLiteralTo(writer, value);
    }

    public static void WriteTo(TextWriter writer, HelperResult value)
    {
      WebPageExecutingBase.WriteTo(writer, value);
    }

    public static void WriteLiteralTo(TextWriter writer, HelperResult value)
    {
      WebPageExecutingBase.WriteLiteralTo(writer, (object) value);
    }

    public static void WriteAttributeTo(
      TextWriter writer,
      string name,
      PositionTagged<string> prefix,
      PositionTagged<string> suffix,
      params AttributeValue[] values)
    {
      HelperPage.CurrentPage.WriteAttributeTo(HelperPage.VirtualPath, writer, name, prefix, suffix, values);
    }

    public static void BeginContext(
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      HelperPage.BeginContext(HelperPage.PageContext.Page.GetOutputWriter(), virtualPath, startPosition, length, isLiteral);
    }

    public static void BeginContext(
      TextWriter writer,
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      if (!HelperPage.InstrumentationService.IsAvailable)
        return;
      HelperPage.InstrumentationService.BeginContext(HelperPage.Context, virtualPath, writer, startPosition, length, isLiteral);
    }

    public static void EndContext(
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      HelperPage.EndContext(HelperPage.PageContext.Page.GetOutputWriter(), virtualPath, startPosition, length, isLiteral);
    }

    public static void EndContext(
      TextWriter writer,
      string virtualPath,
      int startPosition,
      int length,
      bool isLiteral)
    {
      if (!HelperPage.InstrumentationService.IsAvailable)
        return;
      HelperPage.InstrumentationService.EndContext(HelperPage.Context, virtualPath, writer, startPosition, length, isLiteral);
    }
  }
}
