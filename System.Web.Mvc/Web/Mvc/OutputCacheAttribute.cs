// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.OutputCacheAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc.Properties;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public class OutputCacheAttribute : ActionFilterAttribute, IExceptionFilter
  {
    private const string CacheKeyPrefix = "_MvcChildActionCache_";
    private static ObjectCache _childActionCache;
    private static object _childActionFilterFinishCallbackKey = new object();
    private OutputCacheParameters _cacheSettings = new OutputCacheParameters()
    {
      VaryByParam = "*"
    };
    private Func<ObjectCache> _childActionCacheThunk = (Func<ObjectCache>) (() => OutputCacheAttribute.ChildActionCache);
    private bool _locationWasSet;
    private bool _noStoreWasSet;

    public OutputCacheAttribute()
    {
    }

    internal OutputCacheAttribute(ObjectCache childActionCache)
    {
      this._childActionCacheThunk = (Func<ObjectCache>) (() => childActionCache);
    }

    public string CacheProfile
    {
      get => this._cacheSettings.CacheProfile ?? string.Empty;
      set => this._cacheSettings.CacheProfile = value;
    }

    internal OutputCacheParameters CacheSettings => this._cacheSettings;

    public static ObjectCache ChildActionCache
    {
      get => OutputCacheAttribute._childActionCache ?? (ObjectCache) MemoryCache.Default;
      set => OutputCacheAttribute._childActionCache = value;
    }

    private ObjectCache ChildActionCacheInternal => this._childActionCacheThunk();

    public int Duration
    {
      get => this._cacheSettings.Duration;
      set => this._cacheSettings.Duration = value;
    }

    public OutputCacheLocation Location
    {
      get => this._cacheSettings.Location;
      set
      {
        this._cacheSettings.Location = value;
        this._locationWasSet = true;
      }
    }

    public bool NoStore
    {
      get => this._cacheSettings.NoStore;
      set
      {
        this._cacheSettings.NoStore = value;
        this._noStoreWasSet = true;
      }
    }

    public string SqlDependency
    {
      get => this._cacheSettings.SqlDependency ?? string.Empty;
      set => this._cacheSettings.SqlDependency = value;
    }

    public string VaryByContentEncoding
    {
      get => this._cacheSettings.VaryByContentEncoding ?? string.Empty;
      set => this._cacheSettings.VaryByContentEncoding = value;
    }

    public string VaryByCustom
    {
      get => this._cacheSettings.VaryByCustom ?? string.Empty;
      set => this._cacheSettings.VaryByCustom = value;
    }

    public string VaryByHeader
    {
      get => this._cacheSettings.VaryByHeader ?? string.Empty;
      set => this._cacheSettings.VaryByHeader = value;
    }

    public string VaryByParam
    {
      get => this._cacheSettings.VaryByParam ?? string.Empty;
      set => this._cacheSettings.VaryByParam = value;
    }

    private static void ClearChildActionFilterFinishCallback(ControllerContext controllerContext)
    {
      controllerContext.HttpContext.Items.Remove(OutputCacheAttribute._childActionFilterFinishCallbackKey);
    }

    private static void CompleteChildAction(ControllerContext filterContext, bool wasException)
    {
      Action<bool> filterFinishCallback = OutputCacheAttribute.GetChildActionFilterFinishCallback(filterContext);
      if (filterFinishCallback == null)
        return;
      OutputCacheAttribute.ClearChildActionFilterFinishCallback(filterContext);
      filterFinishCallback(wasException);
    }

    private static Action<bool> GetChildActionFilterFinishCallback(
      ControllerContext controllerContext)
    {
      return controllerContext.HttpContext.Items[OutputCacheAttribute._childActionFilterFinishCallbackKey] as Action<bool>;
    }

    internal string GetChildActionUniqueId(ActionExecutingContext filterContext)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("_MvcChildActionCache_");
      stringBuilder.Append(filterContext.ActionDescriptor.UniqueId);
      stringBuilder.Append(DescriptorUtil.CreateUniqueId((object) this.VaryByCustom));
      if (!string.IsNullOrEmpty(this.VaryByCustom))
      {
        string varyByCustomString = filterContext.HttpContext.ApplicationInstance.GetVaryByCustomString(HttpContext.Current, this.VaryByCustom);
        stringBuilder.Append(varyByCustomString);
      }
      stringBuilder.Append(OutputCacheAttribute.GetUniqueIdFromActionParameters(filterContext, OutputCacheAttribute.SplitVaryByParam(this.VaryByParam)));
      using (SHA256 shA256 = SHA256.Create())
        return Convert.ToBase64String(shA256.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
    }

    private static string GetUniqueIdFromActionParameters(
      ActionExecutingContext filterContext,
      IEnumerable<string> keys)
    {
      Dictionary<string, object> keyValues = new Dictionary<string, object>(filterContext.ActionParameters, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      keys = (IEnumerable<string>) (keys ?? (IEnumerable<string>) keyValues.Keys).Select<string, string>((Func<string, string>) (key => key.ToUpperInvariant())).OrderBy<string, string>((Func<string, string>) (key => key), (IComparer<string>) StringComparer.Ordinal);
      return DescriptorUtil.CreateUniqueId(((IEnumerable<object>) keys).Concat<object>(keys.Select<string, object>((Func<string, object>) (key => !keyValues.ContainsKey(key) ? (object) null : keyValues[key]))));
    }

    public static bool IsChildActionCacheActive(ControllerContext controllerContext)
    {
      return OutputCacheAttribute.GetChildActionFilterFinishCallback(controllerContext) != null;
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (!filterContext.IsChildAction || filterContext.Exception == null)
        return;
      OutputCacheAttribute.CompleteChildAction((ControllerContext) filterContext, true);
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (!filterContext.IsChildAction)
        return;
      this.ValidateChildActionConfiguration();
      string uniqueId = OutputCacheAttribute.GetChildActionFilterFinishCallback((ControllerContext) filterContext) == null ? this.GetChildActionUniqueId(filterContext) : throw new InvalidOperationException(MvcResources.OutputCacheAttribute_CannotNestChildCache);
      if (this.ChildActionCacheInternal.Get(uniqueId) is string str)
      {
        filterContext.Result = (ActionResult) new ContentResult()
        {
          Content = str
        };
      }
      else
      {
        StringWriter cachingWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
        TextWriter originalWriter = filterContext.HttpContext.Response.Output;
        filterContext.HttpContext.Response.Output = (TextWriter) cachingWriter;
        OutputCacheAttribute.SetChildActionFilterFinishCallback((ControllerContext) filterContext, (Action<bool>) (wasException =>
        {
          filterContext.HttpContext.Response.Output = originalWriter;
          string s = cachingWriter.ToString();
          filterContext.HttpContext.Response.Write(s);
          if (wasException)
            return;
          this.ChildActionCacheInternal.Add(uniqueId, (object) s, DateTimeOffset.UtcNow.AddSeconds((double) this.Duration));
        }));
      }
    }

    public void OnException(ExceptionContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (!filterContext.IsChildAction)
        return;
      OutputCacheAttribute.CompleteChildAction((ControllerContext) filterContext, true);
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (filterContext.IsChildAction)
        return;
      using (OutputCacheAttribute.OutputCachedPage outputCachedPage = new OutputCacheAttribute.OutputCachedPage(this._cacheSettings))
        outputCachedPage.ProcessRequest(HttpContext.Current);
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (!filterContext.IsChildAction)
        return;
      OutputCacheAttribute.CompleteChildAction((ControllerContext) filterContext, filterContext.Exception != null);
    }

    private static void SetChildActionFilterFinishCallback(
      ControllerContext controllerContext,
      Action<bool> callback)
    {
      controllerContext.HttpContext.Items[OutputCacheAttribute._childActionFilterFinishCallbackKey] = (object) callback;
    }

    private static IEnumerable<string> SplitVaryByParam(string varyByParam)
    {
      if (string.Equals(varyByParam, "none", StringComparison.OrdinalIgnoreCase))
        return Enumerable.Empty<string>();
      if (string.Equals(varyByParam, "*", StringComparison.OrdinalIgnoreCase))
        return (IEnumerable<string>) null;
      return ((IEnumerable<string>) varyByParam.Split(';')).Select(part => new
      {
        part = part,
        trimmed = part.Trim()
      }).Where(_param0 => !string.IsNullOrEmpty(_param0.trimmed)).Select(_param0 => _param0.trimmed);
    }

    private void ValidateChildActionConfiguration()
    {
      if (this.Duration <= 0)
        throw new InvalidOperationException(MvcResources.OutputCacheAttribute_InvalidDuration);
      if (string.IsNullOrWhiteSpace(this.VaryByParam))
        throw new InvalidOperationException(MvcResources.OutputCacheAttribute_InvalidVaryByParam);
      if (!string.IsNullOrWhiteSpace(this.CacheProfile) || !string.IsNullOrWhiteSpace(this.SqlDependency) || !string.IsNullOrWhiteSpace(this.VaryByContentEncoding) || !string.IsNullOrWhiteSpace(this.VaryByHeader) || this._locationWasSet || this._noStoreWasSet)
        throw new InvalidOperationException(MvcResources.OutputCacheAttribute_ChildAction_UnsupportedSetting);
    }

    private sealed class OutputCachedPage : Page
    {
      private OutputCacheParameters _cacheSettings;

      public OutputCachedPage(OutputCacheParameters cacheSettings)
      {
        this.ID = Guid.NewGuid().ToString();
        this._cacheSettings = cacheSettings;
      }

      protected override void FrameworkInitialize()
      {
        base.FrameworkInitialize();
        this.InitOutputCache(this._cacheSettings);
      }
    }
  }
}
