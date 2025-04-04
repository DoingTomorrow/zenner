// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Scope.AspNetRequestScopeStorageProvider
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages.Scope
{
  public class AspNetRequestScopeStorageProvider : IScopeStorageProvider
  {
    private static readonly object _pageScopeKey = new object();
    private static readonly object _requestScopeKey = new object();
    private readonly HttpContextBase _httpContext;
    private readonly Func<bool> _appStartExecuted;

    public AspNetRequestScopeStorageProvider()
      : this((HttpContextBase) null, (Func<bool>) (() => WebPageHttpModule.AppStartExecuteCompleted))
    {
    }

    internal AspNetRequestScopeStorageProvider(
      HttpContextBase httpContext,
      Func<bool> appStartExecuted)
    {
      this._httpContext = httpContext;
      this._appStartExecuted = appStartExecuted;
      this.ApplicationScope = (IDictionary<object, object>) new ApplicationScopeStorageDictionary();
    }

    public IDictionary<object, object> CurrentScope
    {
      get => this.PageScope ?? this.RequestScopeInternal ?? this.ApplicationScope;
      set
      {
        if (!this._appStartExecuted())
          throw new InvalidOperationException(WebPageResources.StateStorage_StorageScopesCannotBeCreated);
        this.PageScope = value;
      }
    }

    public IDictionary<object, object> GlobalScope => this.ApplicationScope;

    public IDictionary<object, object> ApplicationScope { get; private set; }

    public IDictionary<object, object> RequestScope
    {
      get
      {
        return this.RequestScopeInternal ?? throw new InvalidOperationException(WebPageResources.StateStorage_RequestScopeNotAvailable);
      }
    }

    private HttpContextBase HttpContext
    {
      get
      {
        System.Web.HttpContext current = System.Web.HttpContext.Current;
        HttpContextBase httpContext = this._httpContext;
        if (httpContext != null)
          return httpContext;
        return current != null ? (HttpContextBase) new HttpContextWrapper(current) : (HttpContextBase) null;
      }
    }

    private IDictionary<object, object> RequestScopeInternal
    {
      get
      {
        if (!this._appStartExecuted())
          return (IDictionary<object, object>) null;
        IDictionary<object, object> requestScopeInternal = (IDictionary<object, object>) this.HttpContext.Items[AspNetRequestScopeStorageProvider._requestScopeKey];
        if (requestScopeInternal == null)
          this.HttpContext.Items[AspNetRequestScopeStorageProvider._requestScopeKey] = (object) (ScopeStorageDictionary) (requestScopeInternal = (IDictionary<object, object>) new ScopeStorageDictionary(this.ApplicationScope));
        return requestScopeInternal;
      }
    }

    private IDictionary<object, object> PageScope
    {
      get
      {
        return this.HttpContext == null ? (IDictionary<object, object>) null : (IDictionary<object, object>) this.HttpContext.Items[AspNetRequestScopeStorageProvider._pageScopeKey];
      }
      set
      {
        this.HttpContext.Items[AspNetRequestScopeStorageProvider._pageScopeKey] = (object) value;
      }
    }
  }
}
