// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Controller
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc.Async;
using System.Web.Mvc.Properties;
using System.Web.Profile;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class Controller : 
    ControllerBase,
    IActionFilter,
    IAuthorizationFilter,
    IDisposable,
    IExceptionFilter,
    IResultFilter,
    IAsyncController,
    IController,
    IAsyncManagerContainer
  {
    private static readonly object _executeTag = new object();
    private static readonly object _executeCoreTag = new object();
    private readonly AsyncManager _asyncManager = new AsyncManager();
    private IActionInvoker _actionInvoker;
    private ModelBinderDictionary _binders;
    private RouteCollection _routeCollection;
    private ITempDataProvider _tempDataProvider;
    private ViewEngineCollection _viewEngineCollection;
    private IDependencyResolver _resolver;

    internal IDependencyResolver Resolver
    {
      get => this._resolver ?? DependencyResolver.CurrentCache;
      set => this._resolver = value;
    }

    public AsyncManager AsyncManager => this._asyncManager;

    protected virtual bool DisableAsyncSupport => false;

    public IActionInvoker ActionInvoker
    {
      get
      {
        if (this._actionInvoker == null)
          this._actionInvoker = this.CreateActionInvoker();
        return this._actionInvoker;
      }
      set => this._actionInvoker = value;
    }

    protected internal ModelBinderDictionary Binders
    {
      get
      {
        if (this._binders == null)
          this._binders = ModelBinders.Binders;
        return this._binders;
      }
      set => this._binders = value;
    }

    public HttpContextBase HttpContext
    {
      get
      {
        return this.ControllerContext != null ? this.ControllerContext.HttpContext : (HttpContextBase) null;
      }
    }

    public ModelStateDictionary ModelState => this.ViewData.ModelState;

    public ProfileBase Profile
    {
      get => this.HttpContext != null ? this.HttpContext.Profile : (ProfileBase) null;
    }

    public HttpRequestBase Request
    {
      get => this.HttpContext != null ? this.HttpContext.Request : (HttpRequestBase) null;
    }

    public HttpResponseBase Response
    {
      get => this.HttpContext != null ? this.HttpContext.Response : (HttpResponseBase) null;
    }

    internal RouteCollection RouteCollection
    {
      get
      {
        if (this._routeCollection == null)
          this._routeCollection = RouteTable.Routes;
        return this._routeCollection;
      }
      set => this._routeCollection = value;
    }

    public RouteData RouteData
    {
      get => this.ControllerContext != null ? this.ControllerContext.RouteData : (RouteData) null;
    }

    public HttpServerUtilityBase Server
    {
      get => this.HttpContext != null ? this.HttpContext.Server : (HttpServerUtilityBase) null;
    }

    public HttpSessionStateBase Session
    {
      get => this.HttpContext != null ? this.HttpContext.Session : (HttpSessionStateBase) null;
    }

    public ITempDataProvider TempDataProvider
    {
      get
      {
        if (this._tempDataProvider == null)
          this._tempDataProvider = this.CreateTempDataProvider();
        return this._tempDataProvider;
      }
      set => this._tempDataProvider = value;
    }

    public UrlHelper Url { get; set; }

    public IPrincipal User => this.HttpContext != null ? this.HttpContext.User : (IPrincipal) null;

    public ViewEngineCollection ViewEngineCollection
    {
      get => this._viewEngineCollection ?? ViewEngines.Engines;
      set => this._viewEngineCollection = value;
    }

    protected internal ContentResult Content(string content)
    {
      return this.Content(content, (string) null);
    }

    protected internal ContentResult Content(string content, string contentType)
    {
      return this.Content(content, contentType, (Encoding) null);
    }

    protected internal virtual ContentResult Content(
      string content,
      string contentType,
      Encoding contentEncoding)
    {
      return new ContentResult()
      {
        Content = content,
        ContentType = contentType,
        ContentEncoding = contentEncoding
      };
    }

    protected virtual IActionInvoker CreateActionInvoker()
    {
      return (IActionInvoker) this.Resolver.GetService<IAsyncActionInvoker>() ?? this.Resolver.GetService<IActionInvoker>() ?? (IActionInvoker) new AsyncControllerActionInvoker();
    }

    protected virtual ITempDataProvider CreateTempDataProvider()
    {
      return this.Resolver.GetService<ITempDataProvider>() ?? (ITempDataProvider) new SessionStateTempDataProvider();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    protected override void ExecuteCore()
    {
      this.PossiblyLoadTempData();
      try
      {
        string requiredString = this.RouteData.GetRequiredString("action");
        if (this.ActionInvoker.InvokeAction(this.ControllerContext, requiredString))
          return;
        this.HandleUnknownAction(requiredString);
      }
      finally
      {
        this.PossiblySaveTempData();
      }
    }

    protected internal FileContentResult File(byte[] fileContents, string contentType)
    {
      return this.File(fileContents, contentType, (string) null);
    }

    protected internal virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      string fileDownloadName)
    {
      FileContentResult fileContentResult = new FileContentResult(fileContents, contentType);
      fileContentResult.FileDownloadName = fileDownloadName;
      return fileContentResult;
    }

    protected internal FileStreamResult File(Stream fileStream, string contentType)
    {
      return this.File(fileStream, contentType, (string) null);
    }

    protected internal virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      string fileDownloadName)
    {
      FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType);
      fileStreamResult.FileDownloadName = fileDownloadName;
      return fileStreamResult;
    }

    protected internal FilePathResult File(string fileName, string contentType)
    {
      return this.File(fileName, contentType, (string) null);
    }

    protected internal virtual FilePathResult File(
      string fileName,
      string contentType,
      string fileDownloadName)
    {
      FilePathResult filePathResult = new FilePathResult(fileName, contentType);
      filePathResult.FileDownloadName = fileDownloadName;
      return filePathResult;
    }

    protected virtual void HandleUnknownAction(string actionName)
    {
      throw new HttpException(404, string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Controller_UnknownAction, new object[2]
      {
        (object) actionName,
        (object) this.GetType().FullName
      }));
    }

    protected internal HttpNotFoundResult HttpNotFound() => this.HttpNotFound((string) null);

    protected internal virtual HttpNotFoundResult HttpNotFound(string statusDescription)
    {
      return new HttpNotFoundResult(statusDescription);
    }

    protected internal virtual JavaScriptResult JavaScript(string script)
    {
      return new JavaScriptResult() { Script = script };
    }

    protected internal JsonResult Json(object data)
    {
      return this.Json(data, (string) null, (Encoding) null, JsonRequestBehavior.DenyGet);
    }

    protected internal JsonResult Json(object data, string contentType)
    {
      return this.Json(data, contentType, (Encoding) null, JsonRequestBehavior.DenyGet);
    }

    protected internal virtual JsonResult Json(
      object data,
      string contentType,
      Encoding contentEncoding)
    {
      return this.Json(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
    }

    protected internal JsonResult Json(object data, JsonRequestBehavior behavior)
    {
      return this.Json(data, (string) null, (Encoding) null, behavior);
    }

    protected internal JsonResult Json(
      object data,
      string contentType,
      JsonRequestBehavior behavior)
    {
      return this.Json(data, contentType, (Encoding) null, behavior);
    }

    protected internal virtual JsonResult Json(
      object data,
      string contentType,
      Encoding contentEncoding,
      JsonRequestBehavior behavior)
    {
      return new JsonResult()
      {
        Data = data,
        ContentType = contentType,
        ContentEncoding = contentEncoding,
        JsonRequestBehavior = behavior
      };
    }

    protected override void Initialize(RequestContext requestContext)
    {
      base.Initialize(requestContext);
      this.Url = new UrlHelper(requestContext);
    }

    protected virtual void OnActionExecuting(ActionExecutingContext filterContext)
    {
    }

    protected virtual void OnActionExecuted(ActionExecutedContext filterContext)
    {
    }

    protected virtual void OnAuthorization(AuthorizationContext filterContext)
    {
    }

    protected virtual void OnException(ExceptionContext filterContext)
    {
    }

    protected virtual void OnResultExecuted(ResultExecutedContext filterContext)
    {
    }

    protected virtual void OnResultExecuting(ResultExecutingContext filterContext)
    {
    }

    protected internal PartialViewResult PartialView()
    {
      return this.PartialView((string) null, (object) null);
    }

    protected internal PartialViewResult PartialView(object model)
    {
      return this.PartialView((string) null, model);
    }

    protected internal PartialViewResult PartialView(string viewName)
    {
      return this.PartialView(viewName, (object) null);
    }

    protected internal virtual PartialViewResult PartialView(string viewName, object model)
    {
      if (model != null)
        this.ViewData.Model = model;
      PartialViewResult partialViewResult = new PartialViewResult();
      partialViewResult.ViewName = viewName;
      partialViewResult.ViewData = this.ViewData;
      partialViewResult.TempData = this.TempData;
      partialViewResult.ViewEngineCollection = this.ViewEngineCollection;
      return partialViewResult;
    }

    internal void PossiblyLoadTempData()
    {
      if (this.ControllerContext.IsChildAction)
        return;
      this.TempData.Load(this.ControllerContext, this.TempDataProvider);
    }

    internal void PossiblySaveTempData()
    {
      if (this.ControllerContext.IsChildAction)
        return;
      this.TempData.Save(this.ControllerContext, this.TempDataProvider);
    }

    protected internal virtual RedirectResult Redirect(string url)
    {
      return !string.IsNullOrEmpty(url) ? new RedirectResult(url) : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (url));
    }

    protected internal virtual RedirectResult RedirectPermanent(string url)
    {
      return !string.IsNullOrEmpty(url) ? new RedirectResult(url, true) : throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (url));
    }

    protected internal RedirectToRouteResult RedirectToAction(string actionName)
    {
      return this.RedirectToAction(actionName, (RouteValueDictionary) null);
    }

    protected internal RedirectToRouteResult RedirectToAction(string actionName, object routeValues)
    {
      return this.RedirectToAction(actionName, new RouteValueDictionary(routeValues));
    }

    protected internal RedirectToRouteResult RedirectToAction(
      string actionName,
      RouteValueDictionary routeValues)
    {
      return this.RedirectToAction(actionName, (string) null, routeValues);
    }

    protected internal RedirectToRouteResult RedirectToAction(
      string actionName,
      string controllerName)
    {
      return this.RedirectToAction(actionName, controllerName, (RouteValueDictionary) null);
    }

    protected internal RedirectToRouteResult RedirectToAction(
      string actionName,
      string controllerName,
      object routeValues)
    {
      return this.RedirectToAction(actionName, controllerName, new RouteValueDictionary(routeValues));
    }

    protected internal virtual RedirectToRouteResult RedirectToAction(
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues)
    {
      return new RedirectToRouteResult(this.RouteData != null ? RouteValuesHelpers.MergeRouteValues(actionName, controllerName, this.RouteData.Values, routeValues, true) : RouteValuesHelpers.MergeRouteValues(actionName, controllerName, (RouteValueDictionary) null, routeValues, true));
    }

    protected internal RedirectToRouteResult RedirectToActionPermanent(string actionName)
    {
      return this.RedirectToActionPermanent(actionName, (RouteValueDictionary) null);
    }

    protected internal RedirectToRouteResult RedirectToActionPermanent(
      string actionName,
      object routeValues)
    {
      return this.RedirectToActionPermanent(actionName, new RouteValueDictionary(routeValues));
    }

    protected internal RedirectToRouteResult RedirectToActionPermanent(
      string actionName,
      RouteValueDictionary routeValues)
    {
      return this.RedirectToActionPermanent(actionName, (string) null, routeValues);
    }

    protected internal RedirectToRouteResult RedirectToActionPermanent(
      string actionName,
      string controllerName)
    {
      return this.RedirectToActionPermanent(actionName, controllerName, (RouteValueDictionary) null);
    }

    protected internal RedirectToRouteResult RedirectToActionPermanent(
      string actionName,
      string controllerName,
      object routeValues)
    {
      return this.RedirectToActionPermanent(actionName, controllerName, new RouteValueDictionary(routeValues));
    }

    protected internal virtual RedirectToRouteResult RedirectToActionPermanent(
      string actionName,
      string controllerName,
      RouteValueDictionary routeValues)
    {
      RouteValueDictionary values = this.RouteData != null ? this.RouteData.Values : (RouteValueDictionary) null;
      return new RedirectToRouteResult((string) null, RouteValuesHelpers.MergeRouteValues(actionName, controllerName, values, routeValues, true), true);
    }

    protected internal RedirectToRouteResult RedirectToRoute(object routeValues)
    {
      return this.RedirectToRoute(new RouteValueDictionary(routeValues));
    }

    protected internal RedirectToRouteResult RedirectToRoute(RouteValueDictionary routeValues)
    {
      return this.RedirectToRoute((string) null, routeValues);
    }

    protected internal RedirectToRouteResult RedirectToRoute(string routeName)
    {
      return this.RedirectToRoute(routeName, (RouteValueDictionary) null);
    }

    protected internal RedirectToRouteResult RedirectToRoute(string routeName, object routeValues)
    {
      return this.RedirectToRoute(routeName, new RouteValueDictionary(routeValues));
    }

    protected internal virtual RedirectToRouteResult RedirectToRoute(
      string routeName,
      RouteValueDictionary routeValues)
    {
      return new RedirectToRouteResult(routeName, RouteValuesHelpers.GetRouteValues(routeValues));
    }

    protected internal RedirectToRouteResult RedirectToRoutePermanent(object routeValues)
    {
      return this.RedirectToRoutePermanent(new RouteValueDictionary(routeValues));
    }

    protected internal RedirectToRouteResult RedirectToRoutePermanent(
      RouteValueDictionary routeValues)
    {
      return this.RedirectToRoutePermanent((string) null, routeValues);
    }

    protected internal RedirectToRouteResult RedirectToRoutePermanent(string routeName)
    {
      return this.RedirectToRoutePermanent(routeName, (RouteValueDictionary) null);
    }

    protected internal RedirectToRouteResult RedirectToRoutePermanent(
      string routeName,
      object routeValues)
    {
      return this.RedirectToRoutePermanent(routeName, new RouteValueDictionary(routeValues));
    }

    protected internal virtual RedirectToRouteResult RedirectToRoutePermanent(
      string routeName,
      RouteValueDictionary routeValues)
    {
      return new RedirectToRouteResult(routeName, RouteValuesHelpers.GetRouteValues(routeValues), true);
    }

    protected internal bool TryUpdateModel<TModel>(TModel model) where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, (string) null, (string[]) null, (string[]) null, this.ValueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(TModel model, string prefix) where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, prefix, (string[]) null, (string[]) null, this.ValueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(TModel model, string[] includeProperties) where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, (string) null, includeProperties, (string[]) null, this.ValueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties)
      where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, prefix, includeProperties, (string[]) null, this.ValueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties,
      string[] excludeProperties)
      where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, prefix, includeProperties, excludeProperties, this.ValueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(TModel model, IValueProvider valueProvider) where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, (string) null, (string[]) null, (string[]) null, valueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(
      TModel model,
      string prefix,
      IValueProvider valueProvider)
      where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, prefix, (string[]) null, (string[]) null, valueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(
      TModel model,
      string[] includeProperties,
      IValueProvider valueProvider)
      where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, (string) null, includeProperties, (string[]) null, valueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties,
      IValueProvider valueProvider)
      where TModel : class
    {
      return this.TryUpdateModel<TModel>(model, prefix, includeProperties, (string[]) null, valueProvider);
    }

    protected internal bool TryUpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties,
      string[] excludeProperties,
      IValueProvider valueProvider)
      where TModel : class
    {
      if ((object) model == null)
        throw new ArgumentNullException(nameof (model));
      if (valueProvider == null)
        throw new ArgumentNullException(nameof (valueProvider));
      Predicate<string> predicate = (Predicate<string>) (propertyName => BindAttribute.IsPropertyAllowed(propertyName, includeProperties, excludeProperties));
      this.Binders.GetBinder(typeof (TModel)).BindModel(this.ControllerContext, new ModelBindingContext()
      {
        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) (() => (object) (TModel) model), typeof (TModel)),
        ModelName = prefix,
        ModelState = this.ModelState,
        PropertyFilter = predicate,
        ValueProvider = valueProvider
      });
      return this.ModelState.IsValid;
    }

    protected internal bool TryValidateModel(object model)
    {
      return this.TryValidateModel(model, (string) null);
    }

    protected internal bool TryValidateModel(object model, string prefix)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      foreach (ModelValidationResult validationResult in ModelValidator.GetModelValidator(ModelMetadataProviders.Current.GetMetadataForType((Func<object>) (() => model), model.GetType()), this.ControllerContext).Validate((object) null))
        this.ModelState.AddModelError(DefaultModelBinder.CreateSubPropertyName(prefix, validationResult.MemberName), validationResult.Message);
      return this.ModelState.IsValid;
    }

    protected internal void UpdateModel<TModel>(TModel model) where TModel : class
    {
      this.UpdateModel<TModel>(model, (string) null, (string[]) null, (string[]) null, this.ValueProvider);
    }

    protected internal void UpdateModel<TModel>(TModel model, string prefix) where TModel : class
    {
      this.UpdateModel<TModel>(model, prefix, (string[]) null, (string[]) null, this.ValueProvider);
    }

    protected internal void UpdateModel<TModel>(TModel model, string[] includeProperties) where TModel : class
    {
      this.UpdateModel<TModel>(model, (string) null, includeProperties, (string[]) null, this.ValueProvider);
    }

    protected internal void UpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties)
      where TModel : class
    {
      this.UpdateModel<TModel>(model, prefix, includeProperties, (string[]) null, this.ValueProvider);
    }

    protected internal void UpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties,
      string[] excludeProperties)
      where TModel : class
    {
      this.UpdateModel<TModel>(model, prefix, includeProperties, excludeProperties, this.ValueProvider);
    }

    protected internal void UpdateModel<TModel>(TModel model, IValueProvider valueProvider) where TModel : class
    {
      this.UpdateModel<TModel>(model, (string) null, (string[]) null, (string[]) null, valueProvider);
    }

    protected internal void UpdateModel<TModel>(
      TModel model,
      string prefix,
      IValueProvider valueProvider)
      where TModel : class
    {
      this.UpdateModel<TModel>(model, prefix, (string[]) null, (string[]) null, valueProvider);
    }

    protected internal void UpdateModel<TModel>(
      TModel model,
      string[] includeProperties,
      IValueProvider valueProvider)
      where TModel : class
    {
      this.UpdateModel<TModel>(model, (string) null, includeProperties, (string[]) null, valueProvider);
    }

    protected internal void UpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties,
      IValueProvider valueProvider)
      where TModel : class
    {
      this.UpdateModel<TModel>(model, prefix, includeProperties, (string[]) null, valueProvider);
    }

    protected internal void UpdateModel<TModel>(
      TModel model,
      string prefix,
      string[] includeProperties,
      string[] excludeProperties,
      IValueProvider valueProvider)
      where TModel : class
    {
      if (!this.TryUpdateModel<TModel>(model, prefix, includeProperties, excludeProperties, valueProvider))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Controller_UpdateModel_UpdateUnsuccessful, new object[1]
        {
          (object) typeof (TModel).FullName
        }));
    }

    protected internal void ValidateModel(object model) => this.ValidateModel(model, (string) null);

    protected internal void ValidateModel(object model, string prefix)
    {
      if (!this.TryValidateModel(model, prefix))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Controller_Validate_ValidationFailed, new object[1]
        {
          (object) model.GetType().FullName
        }));
    }

    protected internal ViewResult View() => this.View((string) null, (string) null, (object) null);

    protected internal ViewResult View(object model)
    {
      return this.View((string) null, (string) null, model);
    }

    protected internal ViewResult View(string viewName)
    {
      return this.View(viewName, (string) null, (object) null);
    }

    protected internal ViewResult View(string viewName, string masterName)
    {
      return this.View(viewName, masterName, (object) null);
    }

    protected internal ViewResult View(string viewName, object model)
    {
      return this.View(viewName, (string) null, model);
    }

    protected internal virtual ViewResult View(string viewName, string masterName, object model)
    {
      if (model != null)
        this.ViewData.Model = model;
      ViewResult viewResult = new ViewResult();
      viewResult.ViewName = viewName;
      viewResult.MasterName = masterName;
      viewResult.ViewData = this.ViewData;
      viewResult.TempData = this.TempData;
      viewResult.ViewEngineCollection = this.ViewEngineCollection;
      return viewResult;
    }

    protected internal ViewResult View(IView view) => this.View(view, (object) null);

    protected internal virtual ViewResult View(IView view, object model)
    {
      if (model != null)
        this.ViewData.Model = model;
      ViewResult viewResult = new ViewResult();
      viewResult.View = view;
      viewResult.ViewData = this.ViewData;
      viewResult.TempData = this.TempData;
      return viewResult;
    }

    IAsyncResult IAsyncController.BeginExecute(
      RequestContext requestContext,
      AsyncCallback callback,
      object state)
    {
      return this.BeginExecute(requestContext, callback, state);
    }

    void IAsyncController.EndExecute(IAsyncResult asyncResult) => this.EndExecute(asyncResult);

    protected virtual IAsyncResult BeginExecute(
      RequestContext requestContext,
      AsyncCallback callback,
      object state)
    {
      if (this.DisableAsyncSupport)
      {
        Action action = (Action) (() => this.Execute(requestContext));
        return AsyncResultWrapper.BeginSynchronous(callback, state, action, Controller._executeTag);
      }
      if (requestContext == null)
        throw new ArgumentNullException(nameof (requestContext));
      this.VerifyExecuteCalledOnce();
      this.Initialize(requestContext);
      return AsyncResultWrapper.Begin(callback, state, new BeginInvokeDelegate(this.BeginExecuteCore), new EndInvokeDelegate(this.EndExecuteCore), Controller._executeTag);
    }

    protected virtual IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
    {
      this.PossiblyLoadTempData();
      try
      {
        string actionName = this.RouteData.GetRequiredString("action");
        IActionInvoker invoker = this.ActionInvoker;
        IAsyncActionInvoker asyncInvoker = invoker as IAsyncActionInvoker;
        if (asyncInvoker != null)
        {
          BeginInvokeDelegate beginDelegate = (BeginInvokeDelegate) ((asyncCallback, asyncState) => asyncInvoker.BeginInvokeAction(this.ControllerContext, actionName, asyncCallback, asyncState));
          EndInvokeDelegate endDelegate = (EndInvokeDelegate) (asyncResult =>
          {
            if (asyncInvoker.EndInvokeAction(asyncResult))
              return;
            this.HandleUnknownAction(actionName);
          });
          return AsyncResultWrapper.Begin(callback, state, beginDelegate, endDelegate, Controller._executeCoreTag);
        }
        Action action = (Action) (() =>
        {
          if (invoker.InvokeAction(this.ControllerContext, actionName))
            return;
          this.HandleUnknownAction(actionName);
        });
        return AsyncResultWrapper.BeginSynchronous(callback, state, action, Controller._executeCoreTag);
      }
      catch
      {
        this.PossiblySaveTempData();
        throw;
      }
    }

    protected virtual void EndExecute(IAsyncResult asyncResult)
    {
      AsyncResultWrapper.End(asyncResult, Controller._executeTag);
    }

    protected virtual void EndExecuteCore(IAsyncResult asyncResult)
    {
      try
      {
        AsyncResultWrapper.End(asyncResult, Controller._executeCoreTag);
      }
      finally
      {
        this.PossiblySaveTempData();
      }
    }

    void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
    {
      this.OnActionExecuting(filterContext);
    }

    void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
    {
      this.OnActionExecuted(filterContext);
    }

    void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
    {
      this.OnAuthorization(filterContext);
    }

    void IExceptionFilter.OnException(ExceptionContext filterContext)
    {
      this.OnException(filterContext);
    }

    void IResultFilter.OnResultExecuting(ResultExecutingContext filterContext)
    {
      this.OnResultExecuting(filterContext);
    }

    void IResultFilter.OnResultExecuted(ResultExecutedContext filterContext)
    {
      this.OnResultExecuted(filterContext);
    }
  }
}
