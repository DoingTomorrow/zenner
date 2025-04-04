// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MvcHandler
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using Microsoft.Web.Infrastructure.DynamicValidationHelper;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Mvc.Async;
using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.Web.SessionState;

#nullable disable
namespace System.Web.Mvc
{
  public class MvcHandler : IHttpAsyncHandler, IHttpHandler, IRequiresSessionState
  {
    private static readonly object _processRequestTag = new object();
    internal static readonly string MvcVersion = MvcHandler.GetMvcVersionString();
    public static readonly string MvcVersionHeaderName = "X-AspNetMvc-Version";
    private ControllerBuilder _controllerBuilder;

    public MvcHandler(RequestContext requestContext)
    {
      this.RequestContext = requestContext != null ? requestContext : throw new ArgumentNullException(nameof (requestContext));
    }

    internal ControllerBuilder ControllerBuilder
    {
      get
      {
        if (this._controllerBuilder == null)
          this._controllerBuilder = ControllerBuilder.Current;
        return this._controllerBuilder;
      }
      set => this._controllerBuilder = value;
    }

    public static bool DisableMvcResponseHeader { get; set; }

    protected virtual bool IsReusable => false;

    public RequestContext RequestContext { get; private set; }

    protected internal virtual void AddVersionHeader(HttpContextBase httpContext)
    {
      if (MvcHandler.DisableMvcResponseHeader)
        return;
      httpContext.Response.AppendHeader(MvcHandler.MvcVersionHeaderName, MvcHandler.MvcVersion);
    }

    protected virtual IAsyncResult BeginProcessRequest(
      HttpContext httpContext,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessRequest((HttpContextBase) new HttpContextWrapper(httpContext), callback, state);
    }

    protected internal virtual IAsyncResult BeginProcessRequest(
      HttpContextBase httpContext,
      AsyncCallback callback,
      object state)
    {
      IController controller;
      IControllerFactory factory;
      this.ProcessRequestInit(httpContext, out controller, out factory);
      IAsyncController asyncController = controller as IAsyncController;
      if (asyncController != null)
      {
        BeginInvokeDelegate beginDelegate = (BeginInvokeDelegate) ((asyncCallback, asyncState) =>
        {
          try
          {
            return asyncController.BeginExecute(this.RequestContext, asyncCallback, asyncState);
          }
          catch
          {
            factory.ReleaseController((IController) asyncController);
            throw;
          }
        });
        EndInvokeDelegate endDelegate = (EndInvokeDelegate) (asyncResult =>
        {
          try
          {
            asyncController.EndExecute(asyncResult);
          }
          finally
          {
            factory.ReleaseController((IController) asyncController);
          }
        });
        SynchronizationContext synchronizationContext = SynchronizationContextUtil.GetSynchronizationContext();
        return AsyncResultWrapper.Begin(AsyncUtil.WrapCallbackForSynchronizedExecution(callback, synchronizationContext), state, beginDelegate, endDelegate, MvcHandler._processRequestTag);
      }
      Action action = (Action) (() =>
      {
        try
        {
          controller.Execute(this.RequestContext);
        }
        finally
        {
          factory.ReleaseController(controller);
        }
      });
      return AsyncResultWrapper.BeginSynchronous(callback, state, action, MvcHandler._processRequestTag);
    }

    protected internal virtual void EndProcessRequest(IAsyncResult asyncResult)
    {
      AsyncResultWrapper.End(asyncResult, MvcHandler._processRequestTag);
    }

    private static string GetMvcVersionString()
    {
      return new AssemblyName(typeof (MvcHandler).Assembly.FullName).Version.ToString(2);
    }

    protected virtual void ProcessRequest(HttpContext httpContext)
    {
      this.ProcessRequest((HttpContextBase) new HttpContextWrapper(httpContext));
    }

    protected internal virtual void ProcessRequest(HttpContextBase httpContext)
    {
      IController controller;
      IControllerFactory factory;
      this.ProcessRequestInit(httpContext, out controller, out factory);
      try
      {
        controller.Execute(this.RequestContext);
      }
      finally
      {
        factory.ReleaseController(controller);
      }
    }

    private void ProcessRequestInit(
      HttpContextBase httpContext,
      out IController controller,
      out IControllerFactory factory)
    {
      HttpContext current = HttpContext.Current;
      if (current != null)
      {
        bool? nullable = ValidationUtility.IsValidationEnabled(current);
        if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          ValidationUtility.EnableDynamicValidation(current);
      }
      this.AddVersionHeader(httpContext);
      this.RemoveOptionalRoutingParameters();
      string requiredString = this.RequestContext.RouteData.GetRequiredString(nameof (controller));
      factory = this.ControllerBuilder.GetControllerFactory();
      controller = factory.CreateController(this.RequestContext, requiredString);
      if (controller == null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ControllerBuilder_FactoryReturnedNull, new object[2]
        {
          (object) factory.GetType(),
          (object) requiredString
        }));
    }

    private void RemoveOptionalRoutingParameters()
    {
      RouteValueDictionary values = this.RequestContext.RouteData.Values;
      foreach (string key in values.Where<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>) (entry => entry.Value == UrlParameter.Optional)).Select<KeyValuePair<string, object>, string>((Func<KeyValuePair<string, object>, string>) (entry => entry.Key)).ToArray<string>())
        values.Remove(key);
    }

    bool IHttpHandler.IsReusable => this.IsReusable;

    void IHttpHandler.ProcessRequest(HttpContext httpContext) => this.ProcessRequest(httpContext);

    IAsyncResult IHttpAsyncHandler.BeginProcessRequest(
      HttpContext context,
      AsyncCallback cb,
      object extraData)
    {
      return this.BeginProcessRequest(context, cb, extraData);
    }

    void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result) => this.EndProcessRequest(result);
  }
}
