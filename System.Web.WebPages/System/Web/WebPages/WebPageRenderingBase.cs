// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageRenderingBase
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Web.Caching;
using System.Web.Profile;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class WebPageRenderingBase : WebPageExecutingBase, ITemplateFile
  {
    private IPrincipal _user;
    private UrlDataList _urlData;
    private TemplateFileInfo _templateFileInfo;
    private DisplayModeProvider _displayModeProvider;

    public virtual Cache Cache => this.Context != null ? this.Context.Cache : (Cache) null;

    internal DisplayModeProvider DisplayModeProvider
    {
      get => this._displayModeProvider ?? DisplayModeProvider.Instance;
      set => this._displayModeProvider = value;
    }

    protected internal IDisplayMode DisplayMode => DisplayModeProvider.GetDisplayMode(this.Context);

    public abstract string Layout { get; set; }

    public abstract IDictionary<object, object> PageData { get; }

    public abstract object Page { get; }

    public WebPageContext PageContext { get; internal set; }

    public ProfileBase Profile => this.Context != null ? this.Context.Profile : (ProfileBase) null;

    public virtual HttpRequestBase Request
    {
      get => this.Context != null ? this.Context.Request : (HttpRequestBase) null;
    }

    public virtual HttpResponseBase Response
    {
      get => this.Context != null ? this.Context.Response : (HttpResponseBase) null;
    }

    public virtual HttpServerUtilityBase Server
    {
      get => this.Context != null ? this.Context.Server : (HttpServerUtilityBase) null;
    }

    public virtual HttpSessionStateBase Session
    {
      get => this.Context != null ? this.Context.Session : (HttpSessionStateBase) null;
    }

    public virtual IList<string> UrlData
    {
      get
      {
        if (this._urlData == null)
        {
          WebPageMatch webPageMatch = WebPageRoute.GetWebPageMatch(this.Context);
          this._urlData = webPageMatch == null ? new UrlDataList((string) null) : new UrlDataList(webPageMatch.PathInfo);
        }
        return (IList<string>) this._urlData;
      }
    }

    public virtual IPrincipal User
    {
      get => this._user == null ? this.Context.User : this._user;
      internal set => this._user = value;
    }

    public virtual TemplateFileInfo TemplateInfo
    {
      get
      {
        if (this._templateFileInfo == null)
          this._templateFileInfo = new TemplateFileInfo(this.VirtualPath);
        return this._templateFileInfo;
      }
    }

    public virtual bool IsPost => this.Request.HttpMethod == "POST";

    public virtual bool IsAjax
    {
      get
      {
        HttpRequestBase request = this.Request;
        if (request == null)
          return false;
        if (request["X-Requested-With"] == "XMLHttpRequest")
          return true;
        return request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
      }
    }

    public string Culture
    {
      get => Thread.CurrentThread.CurrentCulture.Name;
      set
      {
        if (string.IsNullOrEmpty(value))
          throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (value));
        CultureUtil.SetCulture(Thread.CurrentThread, this.Context, value);
      }
    }

    public string UICulture
    {
      get => Thread.CurrentThread.CurrentUICulture.Name;
      set
      {
        if (string.IsNullOrEmpty(value))
          throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (value));
        CultureUtil.SetUICulture(Thread.CurrentThread, this.Context, value);
      }
    }

    public abstract void ExecutePageHierarchy();

    public abstract HelperResult RenderPage(string path, params object[] data);
  }
}
