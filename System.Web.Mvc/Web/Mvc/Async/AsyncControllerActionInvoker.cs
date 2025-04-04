// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.AsyncControllerActionInvoker
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  public class AsyncControllerActionInvoker : 
    ControllerActionInvoker,
    IAsyncActionInvoker,
    IActionInvoker
  {
    private static readonly object _invokeActionTag = new object();
    private static readonly object _invokeActionMethodTag = new object();
    private static readonly object _invokeActionMethodWithFiltersTag = new object();

    public virtual IAsyncResult BeginInvokeAction(
      ControllerContext controllerContext,
      string actionName,
      AsyncCallback callback,
      object state)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AsyncControllerActionInvoker.\u003C\u003Ec__DisplayClass23 cDisplayClass23_3 = new AsyncControllerActionInvoker.\u003C\u003Ec__DisplayClass23();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass23_3.controllerContext = controllerContext;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass23_3.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass23_3.controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(actionName))
        throw Error.ParameterCannotBeNullOrEmpty(nameof (actionName));
      // ISSUE: reference to a compiler-generated field
      ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(cDisplayClass23_3.controllerContext);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass23_3.actionDescriptor = this.FindAction(cDisplayClass23_3.controllerContext, controllerDescriptor, actionName);
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass23_3.actionDescriptor == null)
        return AsyncControllerActionInvoker.BeginInvokeAction_ActionNotFound(callback, state);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      FilterInfo filterInfo = this.GetFilters(cDisplayClass23_3.controllerContext, cDisplayClass23_3.actionDescriptor);
      Action continuation = (Action) null;
      BeginInvokeDelegate beginDelegate = (BeginInvokeDelegate) ((asyncCallback, asyncState) =>
      {
        try
        {
          // ISSUE: variable of a compiler-generated type
          AsyncControllerActionInvoker.\u003C\u003Ec__DisplayClass23 cDisplayClass23_1 = cDisplayClass23_3;
          AuthorizationContext authContext = this.InvokeAuthorizationFilters(controllerContext, filterInfo.AuthorizationFilters, actionDescriptor);
          if (authContext.Result != null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            continuation = (Action) (() => cDisplayClass23_1.\u003C\u003E4__this.InvokeActionResult(cDisplayClass23_1.controllerContext, authContext.Result));
          }
          else
          {
            // ISSUE: variable of a compiler-generated type
            AsyncControllerActionInvoker.\u003C\u003Ec__DisplayClass23 cDisplayClass23_2 = cDisplayClass23_3;
            if (controllerContext.Controller.ValidateRequest)
              ControllerActionInvoker.ValidateRequest(controllerContext);
            IDictionary<string, object> parameterValues = this.GetParameterValues(controllerContext, actionDescriptor);
            IAsyncResult asyncResult = this.BeginInvokeActionMethodWithFilters(controllerContext, filterInfo.ActionFilters, actionDescriptor, parameterValues, asyncCallback, asyncState);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            continuation = (Action) (() => cDisplayClass23_2.\u003C\u003E4__this.InvokeActionResultWithFilters(cDisplayClass23_2.controllerContext, filterInfo.ResultFilters, cDisplayClass23_2.\u003C\u003E4__this.EndInvokeActionMethodWithFilters(asyncResult).Result));
            return asyncResult;
          }
        }
        catch (ThreadAbortException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          // ISSUE: variable of a compiler-generated type
          AsyncControllerActionInvoker.\u003C\u003Ec__DisplayClass23 cDisplayClass23 = cDisplayClass23_3;
          ExceptionContext exceptionContext = this.InvokeExceptionFilters(controllerContext, filterInfo.ExceptionFilters, ex);
          if (!exceptionContext.ExceptionHandled)
          {
            throw;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            continuation = (Action) (() => cDisplayClass23.\u003C\u003E4__this.InvokeActionResult(cDisplayClass23.controllerContext, exceptionContext.Result));
          }
        }
        return AsyncControllerActionInvoker.BeginInvokeAction_MakeSynchronousAsyncResult(asyncCallback, asyncState);
      });
      EndInvokeDelegate<bool> endDelegate = (EndInvokeDelegate<bool>) (asyncResult =>
      {
        try
        {
          continuation();
        }
        catch (ThreadAbortException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          ExceptionContext exceptionContext = this.InvokeExceptionFilters(controllerContext, filterInfo.ExceptionFilters, ex);
          if (!exceptionContext.ExceptionHandled)
            throw;
          else
            this.InvokeActionResult(controllerContext, exceptionContext.Result);
        }
        return true;
      });
      return AsyncResultWrapper.Begin<bool>(callback, state, beginDelegate, endDelegate, AsyncControllerActionInvoker._invokeActionTag);
    }

    private static IAsyncResult BeginInvokeAction_ActionNotFound(
      AsyncCallback callback,
      object state)
    {
      BeginInvokeDelegate beginDelegate = new BeginInvokeDelegate(AsyncControllerActionInvoker.BeginInvokeAction_MakeSynchronousAsyncResult);
      EndInvokeDelegate<bool> endDelegate = (EndInvokeDelegate<bool>) (asyncResult => false);
      return AsyncResultWrapper.Begin<bool>(callback, state, beginDelegate, endDelegate, AsyncControllerActionInvoker._invokeActionTag);
    }

    private static IAsyncResult BeginInvokeAction_MakeSynchronousAsyncResult(
      AsyncCallback callback,
      object state)
    {
      SimpleAsyncResult simpleAsyncResult = new SimpleAsyncResult(state);
      simpleAsyncResult.MarkCompleted(true, callback);
      return (IAsyncResult) simpleAsyncResult;
    }

    protected internal virtual IAsyncResult BeginInvokeActionMethod(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor,
      IDictionary<string, object> parameters,
      AsyncCallback callback,
      object state)
    {
      return actionDescriptor is AsyncActionDescriptor actionDescriptor1 ? this.BeginInvokeAsynchronousActionMethod(controllerContext, actionDescriptor1, parameters, callback, state) : this.BeginInvokeSynchronousActionMethod(controllerContext, actionDescriptor, parameters, callback, state);
    }

    protected internal virtual IAsyncResult BeginInvokeActionMethodWithFilters(
      ControllerContext controllerContext,
      IList<IActionFilter> filters,
      ActionDescriptor actionDescriptor,
      IDictionary<string, object> parameters,
      AsyncCallback callback,
      object state)
    {
      Func<ActionExecutedContext> endContinuation = (Func<ActionExecutedContext>) null;
      BeginInvokeDelegate beginDelegate = (BeginInvokeDelegate) ((asyncCallback, asyncState) =>
      {
        ActionExecutingContext preContext = new ActionExecutingContext(controllerContext, actionDescriptor, parameters);
        IAsyncResult innerAsyncResult = (IAsyncResult) null;
        Func<Func<ActionExecutedContext>> seed = (Func<Func<ActionExecutedContext>>) (() =>
        {
          innerAsyncResult = this.BeginInvokeActionMethod(controllerContext, actionDescriptor, parameters, asyncCallback, asyncState);
          return (Func<ActionExecutedContext>) (() => new ActionExecutedContext(controllerContext, actionDescriptor, false, (Exception) null)
          {
            Result = this.EndInvokeActionMethod(innerAsyncResult)
          });
        });
        endContinuation = filters.Reverse<IActionFilter>().Aggregate<IActionFilter, Func<Func<ActionExecutedContext>>>(seed, (Func<Func<Func<ActionExecutedContext>>, IActionFilter, Func<Func<ActionExecutedContext>>>) ((next, filter) => (Func<Func<ActionExecutedContext>>) (() => AsyncControllerActionInvoker.InvokeActionMethodFilterAsynchronously(filter, preContext, next))))();
        if (innerAsyncResult != null)
          return innerAsyncResult;
        SimpleAsyncResult simpleAsyncResult = new SimpleAsyncResult(asyncState);
        simpleAsyncResult.MarkCompleted(true, asyncCallback);
        return (IAsyncResult) simpleAsyncResult;
      });
      EndInvokeDelegate<ActionExecutedContext> endDelegate = (EndInvokeDelegate<ActionExecutedContext>) (asyncResult => endContinuation());
      return AsyncResultWrapper.Begin<ActionExecutedContext>(callback, state, beginDelegate, endDelegate, AsyncControllerActionInvoker._invokeActionMethodWithFiltersTag);
    }

    private IAsyncResult BeginInvokeAsynchronousActionMethod(
      ControllerContext controllerContext,
      AsyncActionDescriptor actionDescriptor,
      IDictionary<string, object> parameters,
      AsyncCallback callback,
      object state)
    {
      BeginInvokeDelegate beginDelegate = (BeginInvokeDelegate) ((asyncCallback, asyncState) => actionDescriptor.BeginExecute(controllerContext, parameters, asyncCallback, asyncState));
      EndInvokeDelegate<ActionResult> endDelegate = (EndInvokeDelegate<ActionResult>) (asyncResult => this.CreateActionResult(controllerContext, (ActionDescriptor) actionDescriptor, actionDescriptor.EndExecute(asyncResult)));
      return AsyncResultWrapper.Begin<ActionResult>(callback, state, beginDelegate, endDelegate, AsyncControllerActionInvoker._invokeActionMethodTag);
    }

    private IAsyncResult BeginInvokeSynchronousActionMethod(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor,
      IDictionary<string, object> parameters,
      AsyncCallback callback,
      object state)
    {
      return AsyncResultWrapper.BeginSynchronous<ActionResult>(callback, state, (Func<ActionResult>) (() => this.InvokeSynchronousActionMethod(controllerContext, actionDescriptor, parameters)), AsyncControllerActionInvoker._invokeActionMethodTag);
    }

    public virtual bool EndInvokeAction(IAsyncResult asyncResult)
    {
      return AsyncResultWrapper.End<bool>(asyncResult, AsyncControllerActionInvoker._invokeActionTag);
    }

    protected internal virtual ActionResult EndInvokeActionMethod(IAsyncResult asyncResult)
    {
      return AsyncResultWrapper.End<ActionResult>(asyncResult, AsyncControllerActionInvoker._invokeActionMethodTag);
    }

    protected internal virtual ActionExecutedContext EndInvokeActionMethodWithFilters(
      IAsyncResult asyncResult)
    {
      return AsyncResultWrapper.End<ActionExecutedContext>(asyncResult, AsyncControllerActionInvoker._invokeActionMethodWithFiltersTag);
    }

    protected override ControllerDescriptor GetControllerDescriptor(
      ControllerContext controllerContext)
    {
      Type controllerType = controllerContext.Controller.GetType();
      return this.DescriptorCache.GetDescriptor(controllerType, (Func<ControllerDescriptor>) (() => (ControllerDescriptor) new ReflectedAsyncControllerDescriptor(controllerType)));
    }

    internal static Func<ActionExecutedContext> InvokeActionMethodFilterAsynchronously(
      IActionFilter filter,
      ActionExecutingContext preContext,
      Func<Func<ActionExecutedContext>> nextInChain)
    {
      filter.OnActionExecuting(preContext);
      if (preContext.Result != null)
      {
        ActionExecutedContext shortCircuitedPostContext = new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, true, (Exception) null)
        {
          Result = preContext.Result
        };
        return (Func<ActionExecutedContext>) (() => shortCircuitedPostContext);
      }
      try
      {
        Func<ActionExecutedContext> continuation = nextInChain();
        return (Func<ActionExecutedContext>) (() =>
        {
          bool flag = true;
          ActionExecutedContext filterContext;
          try
          {
            filterContext = continuation();
            flag = false;
          }
          catch (ThreadAbortException ex)
          {
            filter.OnActionExecuted(new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, false, (Exception) null));
            throw;
          }
          catch (Exception ex)
          {
            filterContext = new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, false, ex);
            filter.OnActionExecuted(filterContext);
            if (!filterContext.ExceptionHandled)
              throw;
          }
          if (!flag)
            filter.OnActionExecuted(filterContext);
          return filterContext;
        });
      }
      catch (ThreadAbortException ex)
      {
        ActionExecutedContext filterContext = new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, false, (Exception) null);
        filter.OnActionExecuted(filterContext);
        throw;
      }
      catch (Exception ex)
      {
        ActionExecutedContext postContext = new ActionExecutedContext((ControllerContext) preContext, preContext.ActionDescriptor, false, ex);
        filter.OnActionExecuted(postContext);
        if (postContext.ExceptionHandled)
          return (Func<ActionExecutedContext>) (() => postContext);
        throw;
      }
    }

    private ActionResult InvokeSynchronousActionMethod(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor,
      IDictionary<string, object> parameters)
    {
      return this.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
    }
  }
}
