// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ControllerBase
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.Async;
using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.Web.WebPages.Scope;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class ControllerBase : IController
  {
    private readonly SingleEntryGate _executeWasCalledGate = new SingleEntryGate();
    private DynamicViewDataDictionary _dynamicViewDataDictionary;
    private TempDataDictionary _tempDataDictionary;
    private bool _validateRequest = true;
    private IValueProvider _valueProvider;
    private ViewDataDictionary _viewDataDictionary;

    public ControllerContext ControllerContext { get; set; }

    public TempDataDictionary TempData
    {
      get
      {
        if (this.ControllerContext != null && this.ControllerContext.IsChildAction)
          return this.ControllerContext.ParentActionViewContext.TempData;
        if (this._tempDataDictionary == null)
          this._tempDataDictionary = new TempDataDictionary();
        return this._tempDataDictionary;
      }
      set => this._tempDataDictionary = value;
    }

    public bool ValidateRequest
    {
      get => this._validateRequest;
      set => this._validateRequest = value;
    }

    public IValueProvider ValueProvider
    {
      get
      {
        if (this._valueProvider == null)
          this._valueProvider = ValueProviderFactories.Factories.GetValueProvider(this.ControllerContext);
        return this._valueProvider;
      }
      set => this._valueProvider = value;
    }

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewDataDictionary == null)
          this._dynamicViewDataDictionary = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewDataDictionary;
      }
    }

    public ViewDataDictionary ViewData
    {
      get
      {
        if (this._viewDataDictionary == null)
          this._viewDataDictionary = new ViewDataDictionary();
        return this._viewDataDictionary;
      }
      set => this._viewDataDictionary = value;
    }

    protected virtual void Execute(RequestContext requestContext)
    {
      if (requestContext == null)
        throw new ArgumentNullException(nameof (requestContext));
      if (requestContext.HttpContext == null)
        throw new ArgumentException(MvcResources.ControllerBase_CannotExecuteWithNullHttpContext, nameof (requestContext));
      this.VerifyExecuteCalledOnce();
      this.Initialize(requestContext);
      using (ScopeStorage.CreateTransientScope())
        this.ExecuteCore();
    }

    protected abstract void ExecuteCore();

    protected virtual void Initialize(RequestContext requestContext)
    {
      this.ControllerContext = new ControllerContext(requestContext, this);
    }

    internal void VerifyExecuteCalledOnce()
    {
      if (!this._executeWasCalledGate.TryEnter())
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ControllerBase_CannotHandleMultipleRequests, new object[1]
        {
          (object) this.GetType()
        }));
    }

    void IController.Execute(RequestContext requestContext) => this.Execute(requestContext);
  }
}
