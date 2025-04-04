// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageHttpModule
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  internal class WebPageHttpModule : IHttpModule
  {
    internal static EventHandler Initialize;
    internal static EventHandler ApplicationStart;
    internal static EventHandler BeginRequest;
    internal static EventHandler EndRequest;
    private static bool _appStartExecuted = false;
    private static readonly object _appStartExecutedLock = new object();
    private static readonly object _hasBeenRegisteredKey = new object();

    internal static bool AppStartExecuteCompleted { get; set; }

    public void Dispose()
    {
    }

    public void Init(HttpApplication application)
    {
      if (application.Context.Items[WebPageHttpModule._hasBeenRegisteredKey] != null)
        return;
      application.Context.Items[WebPageHttpModule._hasBeenRegisteredKey] = (object) true;
      WebPageHttpModule.InitApplication(application);
    }

    internal static void InitApplication(HttpApplication application)
    {
      WebPageHttpModule.StartApplication(application);
      WebPageHttpModule.InitializeApplication(application);
    }

    internal static void InitializeApplication(HttpApplication application)
    {
      WebPageHttpModule.InitializeApplication(application, new EventHandler(WebPageHttpModule.OnApplicationPostResolveRequestCache), WebPageHttpModule.Initialize);
    }

    internal static void InitializeApplication(
      HttpApplication application,
      EventHandler onApplicationPostResolveRequestCache,
      EventHandler initialize)
    {
      if (initialize != null)
        initialize((object) application, EventArgs.Empty);
      application.PostResolveRequestCache += onApplicationPostResolveRequestCache;
      if (ApplicationStartPage.Exception != null || WebPageHttpModule.BeginRequest != null)
        application.BeginRequest += new EventHandler(WebPageHttpModule.OnBeginRequest);
      application.EndRequest += new EventHandler(WebPageHttpModule.OnEndRequest);
    }

    internal static void StartApplication(HttpApplication application)
    {
      WebPageHttpModule.StartApplication(application, new Action<HttpApplication>(ApplicationStartPage.ExecuteStartPage), WebPageHttpModule.ApplicationStart);
    }

    internal static void StartApplication(
      HttpApplication application,
      Action<HttpApplication> executeStartPage,
      EventHandler applicationStart)
    {
      lock (WebPageHttpModule._appStartExecutedLock)
      {
        if (WebPageHttpModule._appStartExecuted)
          return;
        WebPageHttpModule._appStartExecuted = true;
        executeStartPage(application);
        WebPageHttpModule.AppStartExecuteCompleted = true;
        if (applicationStart == null)
          return;
        applicationStart((object) application, EventArgs.Empty);
      }
    }

    internal static void OnApplicationPostResolveRequestCache(object sender, EventArgs e)
    {
      new WebPageRoute().DoPostResolveRequestCache((HttpContextBase) new HttpContextWrapper(((HttpApplication) sender).Context));
    }

    internal static void OnBeginRequest(object sender, EventArgs e)
    {
      if (ApplicationStartPage.Exception != null)
        throw new HttpException((string) null, ApplicationStartPage.Exception);
      if (WebPageHttpModule.BeginRequest == null)
        return;
      WebPageHttpModule.BeginRequest(sender, e);
    }

    internal static void OnEndRequest(object sender, EventArgs e)
    {
      if (WebPageHttpModule.EndRequest != null)
        WebPageHttpModule.EndRequest(sender, e);
      RequestResourceTracker.DisposeResources((HttpContextBase) new HttpContextWrapper(((HttpApplication) sender).Context));
    }
  }
}
