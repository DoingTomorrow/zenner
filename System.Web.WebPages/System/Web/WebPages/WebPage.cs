// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPage
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.WebPages.Html;
using System.Web.WebPages.Scope;

#nullable disable
namespace System.Web.WebPages
{
  public abstract class WebPage : WebPageBase
  {
    private static readonly List<IWebPageRequestExecutor> _executors = new List<IWebPageRequestExecutor>();
    private HttpContextBase _context;
    private object _model;

    internal bool TopLevelPage { get; set; }

    public override HttpContextBase Context
    {
      get => this._context == null ? this.PageContext.HttpContext : this._context;
      set => this._context = value;
    }

    public HtmlHelper Html { get; private set; }

    public ValidationHelper Validation => this.PageContext.Validation;

    public object Model
    {
      get
      {
        if (WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
          WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (WebPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target = WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
        CallSite<Func<CallSite, object, bool>> pSite1 = WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
        if (WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
          WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (WebPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        object obj = WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) WebPage.\u003Cget_Model\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, this._model, (object) null);
        if (target((CallSite) pSite1, obj))
          this._model = ReflectionDynamicObject.WrapObjectIfInternal(this.PageContext.Model);
        return this._model;
      }
    }

    public ModelStateDictionary ModelState => this.PageContext.ModelState;

    public static void RegisterPageExecutor(IWebPageRequestExecutor executor)
    {
      WebPage._executors.Add(executor);
    }

    public override void ExecutePageHierarchy()
    {
      using (ScopeStorage.CreateTransientScope((IDictionary<object, object>) new ScopeStorageDictionary(ScopeStorage.CurrentScope, this.PageData)))
        this.ExecutePageHierarchy((IEnumerable<IWebPageRequestExecutor>) WebPage._executors);
    }

    internal void ExecutePageHierarchy(IEnumerable<IWebPageRequestExecutor> executors)
    {
      if (this.TopLevelPage && executors.Any<IWebPageRequestExecutor>((Func<IWebPageRequestExecutor, bool>) (executor => executor.Execute(this))))
        return;
      base.ExecutePageHierarchy();
    }

    public override HelperResult RenderPage(string path, params object[] data)
    {
      return base.RenderPage(path, data);
    }

    protected override void InitializePage()
    {
      base.InitializePage();
      this.Html = new HtmlHelper(this.ModelState, this.Validation);
    }
  }
}
