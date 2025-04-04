// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.WebRazorHostFactory
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.WebPages.Razor.Configuration;
using System.Web.WebPages.Razor.Resources;

#nullable disable
namespace System.Web.WebPages.Razor
{
  public class WebRazorHostFactory
  {
    private static ConcurrentDictionary<string, Func<WebRazorHostFactory>> _factories = new ConcurrentDictionary<string, Func<WebRazorHostFactory>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    internal static Func<string, Type> TypeFactory = new Func<string, Type>(WebRazorHostFactory.DefaultTypeFactory);

    public static WebPageRazorHost CreateDefaultHost(string virtualPath)
    {
      return WebRazorHostFactory.CreateDefaultHost(virtualPath, (string) null);
    }

    public static WebPageRazorHost CreateDefaultHost(string virtualPath, string physicalPath)
    {
      return WebRazorHostFactory.CreateHostFromConfigCore((RazorWebSectionGroup) null, virtualPath, physicalPath);
    }

    public static WebPageRazorHost CreateHostFromConfig(string virtualPath)
    {
      return WebRazorHostFactory.CreateHostFromConfig(virtualPath, (string) null);
    }

    public static WebPageRazorHost CreateHostFromConfig(string virtualPath, string physicalPath)
    {
      return !string.IsNullOrEmpty(virtualPath) ? WebRazorHostFactory.CreateHostFromConfigCore(WebRazorHostFactory.GetRazorSection(virtualPath), virtualPath, physicalPath) : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, new object[1]
      {
        (object) nameof (virtualPath)
      }), nameof (virtualPath));
    }

    public static WebPageRazorHost CreateHostFromConfig(
      RazorWebSectionGroup config,
      string virtualPath)
    {
      return WebRazorHostFactory.CreateHostFromConfig(config, virtualPath, (string) null);
    }

    public static WebPageRazorHost CreateHostFromConfig(
      RazorWebSectionGroup config,
      string virtualPath,
      string physicalPath)
    {
      if (config == null)
        throw new ArgumentNullException(nameof (config));
      if (string.IsNullOrEmpty(virtualPath))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, new object[1]
        {
          (object) nameof (virtualPath)
        }), nameof (virtualPath));
      return WebRazorHostFactory.CreateHostFromConfigCore(config, virtualPath, physicalPath);
    }

    internal static WebPageRazorHost CreateHostFromConfigCore(
      RazorWebSectionGroup config,
      string virtualPath,
      string physicalPath)
    {
      virtualPath = WebRazorHostFactory.EnsureAppRelative(virtualPath);
      WebPageRazorHost host;
      if (virtualPath.StartsWith("~/App_Code", StringComparison.OrdinalIgnoreCase))
      {
        host = (WebPageRazorHost) new WebCodeRazorHost(virtualPath, physicalPath);
      }
      else
      {
        WebRazorHostFactory razorHostFactory = (WebRazorHostFactory) null;
        if (config != null && config.Host != null && !string.IsNullOrEmpty(config.Host.FactoryType))
          razorHostFactory = WebRazorHostFactory._factories.GetOrAdd(config.Host.FactoryType, new Func<string, Func<WebRazorHostFactory>>(WebRazorHostFactory.CreateFactory))();
        host = (razorHostFactory ?? new WebRazorHostFactory()).CreateHost(virtualPath, physicalPath);
        if (config != null && config.Pages != null)
          WebRazorHostFactory.ApplyConfigurationToHost(config.Pages, host);
      }
      return host;
    }

    private static Func<WebRazorHostFactory> CreateFactory(string typeName)
    {
      Type type = WebRazorHostFactory.TypeFactory(typeName);
      return !(type == (Type) null) ? ((Expression<Func<WebRazorHostFactory>>) (() => Expression.New(type))).Compile() : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, RazorWebResources.Could_Not_Locate_FactoryType, new object[1]
      {
        (object) typeName
      }));
    }

    public static void ApplyConfigurationToHost(RazorPagesSection config, WebPageRazorHost host)
    {
      host.DefaultPageBaseClass = config.PageBaseType;
      foreach (string str in config.Namespaces.OfType<NamespaceInfo>().Select<NamespaceInfo, string>((Func<NamespaceInfo, string>) (ns => ns.Namespace)))
        host.NamespaceImports.Add(str);
    }

    public virtual WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
    {
      return new WebPageRazorHost(virtualPath, physicalPath);
    }

    internal static RazorWebSectionGroup GetRazorSection(string virtualPath)
    {
      return new RazorWebSectionGroup()
      {
        Host = (HostSection) WebConfigurationManager.GetSection(HostSection.SectionName, virtualPath),
        Pages = (RazorPagesSection) WebConfigurationManager.GetSection(RazorPagesSection.SectionName, virtualPath)
      };
    }

    private static string EnsureAppRelative(string virtualPath)
    {
      if (HostingEnvironment.IsHosted)
        virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
      else if (virtualPath.StartsWith("/", StringComparison.Ordinal))
        virtualPath = "~" + virtualPath;
      else if (!virtualPath.StartsWith("~/", StringComparison.Ordinal))
        virtualPath = "~/" + virtualPath;
      return virtualPath;
    }

    private static Type DefaultTypeFactory(string typeName)
    {
      return BuildManager.GetType(typeName, false, false);
    }
  }
}
