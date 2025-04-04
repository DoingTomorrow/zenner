// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationStartPage
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Web.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.Caching;
using System.Web.Hosting;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class ApplicationStartPage : WebPageExecutingBase
  {
    private static readonly Action<Action> _safeExecuteStartPageThunk = ApplicationStartPage.GetSafeExecuteStartPageThunk();
    public static readonly string StartPageVirtualPath = "~/_appstart.";
    public static readonly string CacheKeyPrefix = "__AppStartPage__";

    public HttpApplication Application { get; internal set; }

    public override HttpContextBase Context
    {
      get => (HttpContextBase) new HttpContextWrapper(this.Application.Context);
    }

    public static HtmlString Markup { get; private set; }

    internal static Exception Exception { get; private set; }

    public TextWriter Output { get; internal set; }

    public override string VirtualPath
    {
      get => ApplicationStartPage.StartPageVirtualPath;
      set => throw new NotSupportedException();
    }

    internal void ExecuteInternal()
    {
      ApplicationStartPage._safeExecuteStartPageThunk((Action) (() =>
      {
        this.Output = (TextWriter) new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
        this.Execute();
        ApplicationStartPage.Markup = new HtmlString(this.Output.ToString());
      }));
    }

    internal static void ExecuteStartPage(HttpApplication application)
    {
      ApplicationStartPage.ExecuteStartPage(application, (Action<string>) (vpath => ApplicationStartPage.MonitorFile(vpath)), (IVirtualPathFactory) VirtualPathFactoryManager.Instance, (IEnumerable<string>) WebPageHttpHandler.GetRegisteredExtensions());
    }

    internal static void ExecuteStartPage(
      HttpApplication application,
      Action<string> monitorFile,
      IVirtualPathFactory virtualPathFactory,
      IEnumerable<string> supportedExtensions)
    {
      try
      {
        ApplicationStartPage.ExecuteStartPageInternal(application, monitorFile, virtualPathFactory, supportedExtensions);
      }
      catch (Exception ex)
      {
        ApplicationStartPage.Exception = ex;
        throw new HttpException((string) null, ex);
      }
    }

    internal static void ExecuteStartPageInternal(
      HttpApplication application,
      Action<string> monitorFile,
      IVirtualPathFactory virtualPathFactory,
      IEnumerable<string> supportedExtensions)
    {
      ApplicationStartPage applicationStartPage = (ApplicationStartPage) null;
      foreach (string supportedExtension in supportedExtensions)
      {
        string virtualPath = ApplicationStartPage.StartPageVirtualPath + supportedExtension;
        monitorFile(virtualPath);
        if (virtualPathFactory.Exists(virtualPath) && applicationStartPage == null)
        {
          applicationStartPage = virtualPathFactory.CreateInstance<ApplicationStartPage>(virtualPath);
          applicationStartPage.Application = application;
          applicationStartPage.VirtualPathFactory = virtualPathFactory;
          applicationStartPage.ExecuteInternal();
        }
      }
    }

    private static Action<Action> GetSafeExecuteStartPageThunk()
    {
      return typeof (HttpResponse).GetProperty("DisableCustomHttpEncoder", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic) != (PropertyInfo) null ? new Action<Action>(HttpContextHelper.ExecuteInNullContext) : (Action<Action>) (action => action());
    }

    private static void InitiateShutdown(string key, object value, CacheItemRemovedReason reason)
    {
      if (reason != CacheItemRemovedReason.DependencyChanged)
        return;
      ThreadPool.QueueUserWorkItem(new WaitCallback(ApplicationStartPage.ShutdownCallBack));
    }

    private static void MonitorFile(string virtualPath)
    {
      CacheDependency cacheDependency = HostingEnvironment.VirtualPathProvider.GetCacheDependency(virtualPath, (IEnumerable) new List<string>()
      {
        virtualPath
      }, DateTime.UtcNow);
      HttpRuntime.Cache.Insert(ApplicationStartPage.CacheKeyPrefix + virtualPath, (object) virtualPath, cacheDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(ApplicationStartPage.InitiateShutdown));
    }

    private static void ShutdownCallBack(object state) => InfrastructureHelper.UnloadAppDomain();

    public override void Write(HelperResult result) => result?.WriteTo(this.Output);

    public override void WriteLiteral(object value) => this.Output.Write(value);

    public override void Write(object value) => this.Output.Write(HttpUtility.HtmlEncode(value));

    protected internal override TextWriter GetOutputWriter() => this.Output;
  }
}
