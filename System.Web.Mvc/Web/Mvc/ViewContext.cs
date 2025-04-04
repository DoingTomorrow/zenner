// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewContext
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.WebPages.Scope;

#nullable disable
namespace System.Web.Mvc
{
  public class ViewContext : ControllerContext
  {
    private const string ClientValidationScript = "<script type=\"text/javascript\">\r\n//<![CDATA[\r\nif (!window.mvcClientValidationMetadata) {{ window.mvcClientValidationMetadata = []; }}\r\nwindow.mvcClientValidationMetadata.push({0});\r\n//]]>\r\n</script>";
    internal static readonly string ClientValidationKeyName = nameof (ClientValidationEnabled);
    internal static readonly string UnobtrusiveJavaScriptKeyName = nameof (UnobtrusiveJavaScriptEnabled);
    private static readonly object _formContextKey = new object();
    private static readonly object _lastFormNumKey = new object();
    private Func<IDictionary<object, object>> _scopeThunk;
    private IDictionary<object, object> _transientScope;
    private DynamicViewDataDictionary _dynamicViewDataDictionary;
    private Func<string> _formIdGenerator;
    private FormContext _defaultFormContext = new FormContext();

    public ViewContext()
    {
    }

    public ViewContext(
      ControllerContext controllerContext,
      IView view,
      ViewDataDictionary viewData,
      TempDataDictionary tempData,
      TextWriter writer)
      : base(controllerContext)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (view == null)
        throw new ArgumentNullException(nameof (view));
      if (viewData == null)
        throw new ArgumentNullException(nameof (viewData));
      if (tempData == null)
        throw new ArgumentNullException(nameof (tempData));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      this.View = view;
      this.ViewData = viewData;
      this.Writer = writer;
      this.TempData = tempData;
    }

    public virtual bool ClientValidationEnabled
    {
      get => ViewContext.GetClientValidationEnabled(this.Scope, this.HttpContext);
      set => ViewContext.SetClientValidationEnabled(value, this.Scope, this.HttpContext);
    }

    public virtual FormContext FormContext
    {
      get
      {
        return this.HttpContext.Items[ViewContext._formContextKey] is FormContext formContext ? formContext : this._defaultFormContext;
      }
      set => this.HttpContext.Items[ViewContext._formContextKey] = (object) value;
    }

    internal Func<string> FormIdGenerator
    {
      get
      {
        if (this._formIdGenerator == null)
          this._formIdGenerator = new Func<string>(this.DefaultFormIdGenerator);
        return this._formIdGenerator;
      }
      set => this._formIdGenerator = value;
    }

    internal static Func<IDictionary<object, object>> GlobalScopeThunk { get; set; }

    private IDictionary<object, object> Scope
    {
      get
      {
        if (this.ScopeThunk != null)
          return this.ScopeThunk();
        if (this._transientScope == null)
          this._transientScope = (IDictionary<object, object>) new Dictionary<object, object>();
        return this._transientScope;
      }
    }

    internal Func<IDictionary<object, object>> ScopeThunk
    {
      get => this._scopeThunk ?? ViewContext.GlobalScopeThunk;
      set => this._scopeThunk = value;
    }

    public virtual TempDataDictionary TempData { get; set; }

    public virtual bool UnobtrusiveJavaScriptEnabled
    {
      get => ViewContext.GetUnobtrusiveJavaScriptEnabled(this.Scope, this.HttpContext);
      set => ViewContext.SetUnobtrusiveJavaScriptEnabled(value, this.Scope, this.HttpContext);
    }

    public virtual IView View { get; set; }

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewDataDictionary == null)
          this._dynamicViewDataDictionary = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewDataDictionary;
      }
    }

    public virtual ViewDataDictionary ViewData { get; set; }

    public virtual TextWriter Writer { get; set; }

    private string DefaultFormIdGenerator()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "form{0}", new object[1]
      {
        (object) ViewContext.IncrementFormCount(this.HttpContext.Items)
      });
    }

    internal static bool GetClientValidationEnabled(
      IDictionary<object, object> scope = null,
      HttpContextBase httpContext = null)
    {
      return ViewContext.ScopeCache.Get(scope, httpContext).ClientValidationEnabled;
    }

    internal FormContext GetFormContextForClientValidation()
    {
      return !this.ClientValidationEnabled ? (FormContext) null : this.FormContext;
    }

    internal static bool GetUnobtrusiveJavaScriptEnabled(
      IDictionary<object, object> scope = null,
      HttpContextBase httpContext = null)
    {
      return ViewContext.ScopeCache.Get(scope, httpContext).UnobtrusiveJavaScriptEnabled;
    }

    private static int IncrementFormCount(IDictionary items)
    {
      object obj = items[ViewContext._lastFormNumKey];
      int num = obj != null ? (int) obj + 1 : 0;
      items[ViewContext._lastFormNumKey] = (object) num;
      return num;
    }

    public void OutputClientValidation()
    {
      FormContext clientValidation = this.GetFormContextForClientValidation();
      if (clientValidation == null || this.UnobtrusiveJavaScriptEnabled)
        return;
      this.Writer.Write(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "<script type=\"text/javascript\">\r\n//<![CDATA[\r\nif (!window.mvcClientValidationMetadata) {{ window.mvcClientValidationMetadata = []; }}\r\nwindow.mvcClientValidationMetadata.push({0});\r\n//]]>\r\n</script>".Replace("\r\n", Environment.NewLine), new object[1]
      {
        (object) clientValidation.GetJsonValidationMetadata()
      }));
    }

    internal static void SetClientValidationEnabled(
      bool enabled,
      IDictionary<object, object> scope = null,
      HttpContextBase httpContext = null)
    {
      ViewContext.ScopeCache.Get(scope, httpContext).ClientValidationEnabled = enabled;
    }

    internal static void SetUnobtrusiveJavaScriptEnabled(
      bool enabled,
      IDictionary<object, object> scope = null,
      HttpContextBase httpContext = null)
    {
      ViewContext.ScopeCache.Get(scope, httpContext).UnobtrusiveJavaScriptEnabled = enabled;
    }

    private static TValue ScopeGet<TValue>(
      IDictionary<object, object> scope,
      string name,
      TValue defaultValue = null)
    {
      object obj;
      return scope.TryGetValue((object) name, out obj) ? (TValue) Convert.ChangeType(obj, typeof (TValue), (IFormatProvider) CultureInfo.InvariantCulture) : defaultValue;
    }

    private sealed class ScopeCache
    {
      private static readonly object _cacheKey = new object();
      private bool _clientValidationEnabled;
      private IDictionary<object, object> _scope;
      private bool _unobtrusiveJavaScriptEnabled;

      private ScopeCache(IDictionary<object, object> scope)
      {
        this._scope = scope;
        this._clientValidationEnabled = ViewContext.ScopeGet<bool>(scope, ViewContext.ClientValidationKeyName);
        this._unobtrusiveJavaScriptEnabled = ViewContext.ScopeGet<bool>(scope, ViewContext.UnobtrusiveJavaScriptKeyName);
      }

      public bool ClientValidationEnabled
      {
        get => this._clientValidationEnabled;
        set
        {
          this._clientValidationEnabled = value;
          this._scope[(object) ViewContext.ClientValidationKeyName] = (object) value;
        }
      }

      public bool UnobtrusiveJavaScriptEnabled
      {
        get => this._unobtrusiveJavaScriptEnabled;
        set
        {
          this._unobtrusiveJavaScriptEnabled = value;
          this._scope[(object) ViewContext.UnobtrusiveJavaScriptKeyName] = (object) value;
        }
      }

      public static ViewContext.ScopeCache Get(
        IDictionary<object, object> scope,
        HttpContextBase httpContext)
      {
        if (httpContext == null && System.Web.HttpContext.Current != null)
          httpContext = (HttpContextBase) new HttpContextWrapper(System.Web.HttpContext.Current);
        ViewContext.ScopeCache scopeCache = (ViewContext.ScopeCache) null;
        scope = scope ?? ScopeStorage.CurrentScope;
        if (httpContext != null)
          scopeCache = httpContext.Items[ViewContext.ScopeCache._cacheKey] as ViewContext.ScopeCache;
        if (scopeCache == null || scopeCache._scope != scope)
        {
          scopeCache = new ViewContext.ScopeCache(scope);
          if (httpContext != null)
            httpContext.Items[ViewContext.ScopeCache._cacheKey] = (object) scopeCache;
        }
        return scopeCache;
      }
    }
  }
}
