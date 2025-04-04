// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ControllerActionInvoker
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using Microsoft.Web.Infrastructure.DynamicValidationHelper;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ControllerActionInvoker : IActionInvoker
  {
    private static readonly ControllerDescriptorCache _staticDescriptorCache = new ControllerDescriptorCache();
    private ModelBinderDictionary _binders;
    private Func<ControllerContext, ActionDescriptor, IEnumerable<Filter>> _getFiltersThunk = new Func<ControllerContext, ActionDescriptor, IEnumerable<Filter>>(FilterProviders.Providers.GetFilters);
    private ControllerDescriptorCache _instanceDescriptorCache;

    public ControllerActionInvoker()
    {
    }

    internal ControllerActionInvoker(params object[] filters)
      : this()
    {
      if (filters == null)
        return;
      this._getFiltersThunk = (Func<ControllerContext, ActionDescriptor, IEnumerable<Filter>>) ((cc, ad) => ((IEnumerable<object>) filters).Select<object, Filter>((Func<object, Filter>) (f => new Filter(f, FilterScope.Action, new int?()))));
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

    internal ControllerDescriptorCache DescriptorCache
    {
      get
      {
        if (this._instanceDescriptorCache == null)
          this._instanceDescriptorCache = ControllerActionInvoker._staticDescriptorCache;
        return this._instanceDescriptorCache;
      }
      set => this._instanceDescriptorCache = value;
    }

    protected virtual ActionResult CreateActionResult(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor,
      object actionReturnValue)
    {
      if (actionReturnValue == null)
        return (ActionResult) new EmptyResult();
      if (!(actionReturnValue is ActionResult actionResult))
        actionResult = (ActionResult) new ContentResult()
        {
          Content = Convert.ToString(actionReturnValue, (IFormatProvider) CultureInfo.InvariantCulture)
        };
      return actionResult;
    }

    protected virtual ControllerDescriptor GetControllerDescriptor(
      ControllerContext controllerContext)
    {
      Type controllerType = controllerContext.Controller.GetType();
      return this.DescriptorCache.GetDescriptor(controllerType, (Func<ControllerDescriptor>) (() => (ControllerDescriptor) new ReflectedControllerDescriptor(controllerType)));
    }

    protected virtual ActionDescriptor FindAction(
      ControllerContext controllerContext,
      ControllerDescriptor controllerDescriptor,
      string actionName)
    {
      return controllerDescriptor.FindAction(controllerContext, actionName);
    }

    protected virtual FilterInfo GetFilters(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      return new FilterInfo(this._getFiltersThunk(controllerContext, actionDescriptor));
    }

    private IModelBinder GetModelBinder(ParameterDescriptor parameterDescriptor)
    {
      return parameterDescriptor.BindingInfo.Binder ?? this.Binders.GetBinder(parameterDescriptor.ParameterType);
    }

    protected virtual object GetParameterValue(
      ControllerContext controllerContext,
      ParameterDescriptor parameterDescriptor)
    {
      Type parameterType = parameterDescriptor.ParameterType;
      IModelBinder modelBinder = this.GetModelBinder(parameterDescriptor);
      IValueProvider valueProvider = controllerContext.Controller.ValueProvider;
      string str = parameterDescriptor.BindingInfo.Prefix ?? parameterDescriptor.ParameterName;
      Predicate<string> propertyFilter = ControllerActionInvoker.GetPropertyFilter(parameterDescriptor);
      ModelBindingContext bindingContext = new ModelBindingContext()
      {
        FallbackToEmptyPrefix = parameterDescriptor.BindingInfo.Prefix == null,
        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) null, parameterType),
        ModelName = str,
        ModelState = controllerContext.Controller.ViewData.ModelState,
        PropertyFilter = propertyFilter,
        ValueProvider = valueProvider
      };
      return modelBinder.BindModel(controllerContext, bindingContext) ?? parameterDescriptor.DefaultValue;
    }

    protected virtual IDictionary<string, object> GetParameterValues(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      Dictionary<string, object> parameterValues = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (ParameterDescriptor parameter in actionDescriptor.GetParameters())
        parameterValues[parameter.ParameterName] = this.GetParameterValue(controllerContext, parameter);
      return (IDictionary<string, object>) parameterValues;
    }

    private static Predicate<string> GetPropertyFilter(ParameterDescriptor parameterDescriptor)
    {
      ParameterBindingInfo bindingInfo = parameterDescriptor.BindingInfo;
      return (Predicate<string>) (propertyName => BindAttribute.IsPropertyAllowed(propertyName, bindingInfo.Include.ToArray<string>(), bindingInfo.Exclude.ToArray<string>()));
    }

    public virtual bool InvokeAction(ControllerContext controllerContext, string actionName)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(actionName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (actionName));
      ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(controllerContext);
      ActionDescriptor action = this.FindAction(controllerContext, controllerDescriptor, actionName);
      if (action == null)
        return false;
      FilterInfo filters = this.GetFilters(controllerContext, action);
      try
      {
        AuthorizationContext authorizationContext = this.InvokeAuthorizationFilters(controllerContext, filters.AuthorizationFilters, action);
        if (authorizationContext.Result != null)
        {
          this.InvokeActionResult(controllerContext, authorizationContext.Result);
        }
        else
        {
          if (controllerContext.Controller.ValidateRequest)
            ControllerActionInvoker.ValidateRequest(controllerContext);
          IDictionary<string, object> parameterValues = this.GetParameterValues(controllerContext, action);
          ActionExecutedContext actionExecutedContext = this.InvokeActionMethodWithFilters(controllerContext, filters.ActionFilters, action, parameterValues);
          this.InvokeActionResultWithFilters(controllerContext, filters.ResultFilters, actionExecutedContext.Result);
        }
      }
      catch (ThreadAbortException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        ExceptionContext exceptionContext = this.InvokeExceptionFilters(controllerContext, filters.ExceptionFilters, ex);
        if (!exceptionContext.ExceptionHandled)
          throw;
        else
          this.InvokeActionResult(controllerContext, exceptionContext.Result);
      }
      return true;
    }

    protected virtual ActionResult InvokeActionMethod(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor,
      IDictionary<string, object> parameters)
    {
      object actionReturnValue = actionDescriptor.Execute(controllerContext, parameters);
      return this.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
    }

    internal static ActionExecutedContext InvokeActionMethodFilter(
      IActionFilter filter,
      ActionExecutingContext preContext,
      Func<ActionExecutedContext> continuation)
    {
      filter.OnActionExecuting(preContext);
      if (preContext.Result != null)
        return new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, true, (Exception) null)
        {
          Result = preContext.Result
        };
      bool flag = false;
      ActionExecutedContext filterContext1;
      try
      {
        filterContext1 = continuation();
      }
      catch (ThreadAbortException ex)
      {
        ActionExecutedContext filterContext2 = new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, false, (Exception) null);
        filter.OnActionExecuted(filterContext2);
        throw;
      }
      catch (Exception ex)
      {
        flag = true;
        filterContext1 = new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, false, ex);
        filter.OnActionExecuted(filterContext1);
        if (!filterContext1.ExceptionHandled)
          throw;
      }
      if (!flag)
        filter.OnActionExecuted(filterContext1);
      return filterContext1;
    }

    protected virtual ActionExecutedContext InvokeActionMethodWithFilters(
      ControllerContext controllerContext,
      IList<IActionFilter> filters,
      ActionDescriptor actionDescriptor,
      IDictionary<string, object> parameters)
    {
      ActionExecutingContext preContext = new ActionExecutingContext(controllerContext, actionDescriptor, parameters);
      Func<ActionExecutedContext> seed = (Func<ActionExecutedContext>) (() => new ActionExecutedContext(controllerContext, actionDescriptor, false, (Exception) null)
      {
        Result = this.InvokeActionMethod(controllerContext, actionDescriptor, parameters)
      });
      return filters.Reverse<IActionFilter>().Aggregate<IActionFilter, Func<ActionExecutedContext>>(seed, (Func<Func<ActionExecutedContext>, IActionFilter, Func<ActionExecutedContext>>) ((next, filter) => (Func<ActionExecutedContext>) (() => ControllerActionInvoker.InvokeActionMethodFilter(filter, preContext, next))))();
    }

    protected virtual void InvokeActionResult(
      ControllerContext controllerContext,
      ActionResult actionResult)
    {
      actionResult.ExecuteResult(controllerContext);
    }

    internal static ResultExecutedContext InvokeActionResultFilter(
      IResultFilter filter,
      ResultExecutingContext preContext,
      Func<ResultExecutedContext> continuation)
    {
      filter.OnResultExecuting(preContext);
      if (preContext.Cancel)
        return new ResultExecutedContext((ControllerContext) preContext, preContext.Result, true, (Exception) null);
      bool flag = false;
      ResultExecutedContext filterContext1;
      try
      {
        filterContext1 = continuation();
      }
      catch (ThreadAbortException ex)
      {
        ResultExecutedContext filterContext2 = new ResultExecutedContext((ControllerContext) preContext, preContext.Result, false, (Exception) null);
        filter.OnResultExecuted(filterContext2);
        throw;
      }
      catch (Exception ex)
      {
        flag = true;
        filterContext1 = new ResultExecutedContext((ControllerContext) preContext, preContext.Result, false, ex);
        filter.OnResultExecuted(filterContext1);
        if (!filterContext1.ExceptionHandled)
          throw;
      }
      if (!flag)
        filter.OnResultExecuted(filterContext1);
      return filterContext1;
    }

    protected virtual ResultExecutedContext InvokeActionResultWithFilters(
      ControllerContext controllerContext,
      IList<IResultFilter> filters,
      ActionResult actionResult)
    {
      ResultExecutingContext preContext = new ResultExecutingContext(controllerContext, actionResult);
      Func<ResultExecutedContext> seed = (Func<ResultExecutedContext>) (() =>
      {
        this.InvokeActionResult(controllerContext, actionResult);
        return new ResultExecutedContext(controllerContext, actionResult, false, (Exception) null);
      });
      return filters.Reverse<IResultFilter>().Aggregate<IResultFilter, Func<ResultExecutedContext>>(seed, (Func<Func<ResultExecutedContext>, IResultFilter, Func<ResultExecutedContext>>) ((next, filter) => (Func<ResultExecutedContext>) (() => ControllerActionInvoker.InvokeActionResultFilter(filter, preContext, next))))();
    }

    protected virtual AuthorizationContext InvokeAuthorizationFilters(
      ControllerContext controllerContext,
      IList<IAuthorizationFilter> filters,
      ActionDescriptor actionDescriptor)
    {
      AuthorizationContext filterContext = new AuthorizationContext(controllerContext, actionDescriptor);
      foreach (IAuthorizationFilter filter in (IEnumerable<IAuthorizationFilter>) filters)
      {
        filter.OnAuthorization(filterContext);
        if (filterContext.Result != null)
          break;
      }
      return filterContext;
    }

    protected virtual ExceptionContext InvokeExceptionFilters(
      ControllerContext controllerContext,
      IList<IExceptionFilter> filters,
      Exception exception)
    {
      ExceptionContext filterContext = new ExceptionContext(controllerContext, exception);
      foreach (IExceptionFilter exceptionFilter in filters.Reverse<IExceptionFilter>())
        exceptionFilter.OnException(filterContext);
      return filterContext;
    }

    internal static void ValidateRequest(ControllerContext controllerContext)
    {
      if (controllerContext.IsChildAction)
        return;
      HttpContext current = HttpContext.Current;
      if (current != null)
        ValidationUtility.EnableDynamicValidation(current);
      controllerContext.HttpContext.Request.ValidateInput();
    }
  }
}
