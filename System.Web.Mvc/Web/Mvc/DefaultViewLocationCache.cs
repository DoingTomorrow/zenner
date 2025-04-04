// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DefaultViewLocationCache
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Caching;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class DefaultViewLocationCache : IViewLocationCache
  {
    private static readonly TimeSpan _defaultTimeSpan = new TimeSpan(0, 15, 0);
    public static readonly IViewLocationCache Null = (IViewLocationCache) new NullViewLocationCache();

    public DefaultViewLocationCache()
      : this(DefaultViewLocationCache._defaultTimeSpan)
    {
    }

    public DefaultViewLocationCache(TimeSpan timeSpan)
    {
      this.TimeSpan = timeSpan.Ticks >= 0L ? timeSpan : throw new InvalidOperationException(MvcResources.DefaultViewLocationCache_NegativeTimeSpan);
    }

    public TimeSpan TimeSpan { get; private set; }

    public string GetViewLocation(HttpContextBase httpContext, string key)
    {
      if (httpContext == null)
        throw new ArgumentNullException(nameof (httpContext));
      return (string) httpContext.Cache[key];
    }

    public void InsertViewLocation(HttpContextBase httpContext, string key, string virtualPath)
    {
      if (httpContext == null)
        throw new ArgumentNullException(nameof (httpContext));
      httpContext.Cache.Insert(key, (object) virtualPath, (CacheDependency) null, Cache.NoAbsoluteExpiration, this.TimeSpan);
    }
  }
}
