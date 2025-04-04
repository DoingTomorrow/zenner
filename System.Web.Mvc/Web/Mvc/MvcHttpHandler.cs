// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MvcHttpHandler
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Async;
using System.Web.Routing;
using System.Web.SessionState;

#nullable disable
namespace System.Web.Mvc
{
  public class MvcHttpHandler : 
    UrlRoutingHandler,
    IHttpAsyncHandler,
    IHttpHandler,
    IRequiresSessionState
  {
    private static readonly object _processRequestTag = new object();

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
      IHttpHandler httpHandler = MvcHttpHandler.GetHttpHandler(httpContext);
      IHttpAsyncHandler httpAsyncHandler = httpHandler as IHttpAsyncHandler;
      if (httpAsyncHandler != null)
      {
        BeginInvokeDelegate beginDelegate = (BeginInvokeDelegate) ((asyncCallback, asyncState) => httpAsyncHandler.BeginProcessRequest(HttpContext.Current, asyncCallback, asyncState));
        EndInvokeDelegate endDelegate = (EndInvokeDelegate) (asyncResult => httpAsyncHandler.EndProcessRequest(asyncResult));
        return AsyncResultWrapper.Begin(callback, state, beginDelegate, endDelegate, MvcHttpHandler._processRequestTag);
      }
      Action action = (Action) (() => httpHandler.ProcessRequest(HttpContext.Current));
      return AsyncResultWrapper.BeginSynchronous(callback, state, action, MvcHttpHandler._processRequestTag);
    }

    protected internal virtual void EndProcessRequest(IAsyncResult asyncResult)
    {
      AsyncResultWrapper.End(asyncResult, MvcHttpHandler._processRequestTag);
    }

    private static IHttpHandler GetHttpHandler(HttpContextBase httpContext)
    {
      MvcHttpHandler.DummyHttpHandler dummyHttpHandler = new MvcHttpHandler.DummyHttpHandler();
      dummyHttpHandler.PublicProcessRequest(httpContext);
      return dummyHttpHandler.HttpHandler;
    }

    protected override void VerifyAndProcessRequest(
      IHttpHandler httpHandler,
      HttpContextBase httpContext)
    {
      if (httpHandler == null)
        throw new ArgumentNullException(nameof (httpHandler));
      httpHandler.ProcessRequest(HttpContext.Current);
    }

    IAsyncResult IHttpAsyncHandler.BeginProcessRequest(
      HttpContext context,
      AsyncCallback cb,
      object extraData)
    {
      return this.BeginProcessRequest(context, cb, extraData);
    }

    void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result) => this.EndProcessRequest(result);

    private sealed class DummyHttpHandler : UrlRoutingHandler
    {
      public IHttpHandler HttpHandler;

      public void PublicProcessRequest(HttpContextBase httpContext)
      {
        this.ProcessRequest(httpContext);
      }

      protected override void VerifyAndProcessRequest(
        IHttpHandler httpHandler,
        HttpContextBase httpContext)
      {
        this.HttpHandler = httpHandler;
      }
    }
  }
}
