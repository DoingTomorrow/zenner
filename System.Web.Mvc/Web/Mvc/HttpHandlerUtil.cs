// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HttpHandlerUtil
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  internal static class HttpHandlerUtil
  {
    public static IHttpHandler WrapForServerExecute(IHttpHandler httpHandler)
    {
      return !(httpHandler is IHttpAsyncHandler httpHandler1) ? (IHttpHandler) new HttpHandlerUtil.ServerExecuteHttpHandlerWrapper(httpHandler) : (IHttpHandler) new HttpHandlerUtil.ServerExecuteHttpHandlerAsyncWrapper(httpHandler1);
    }

    internal class ServerExecuteHttpHandlerWrapper : Page
    {
      private readonly IHttpHandler _httpHandler;

      public ServerExecuteHttpHandlerWrapper(IHttpHandler httpHandler)
      {
        this._httpHandler = httpHandler;
      }

      internal IHttpHandler InnerHandler => this._httpHandler;

      public override void ProcessRequest(HttpContext context)
      {
        HttpHandlerUtil.ServerExecuteHttpHandlerWrapper.Wrap((Action) (() => this._httpHandler.ProcessRequest(context)));
      }

      protected static void Wrap(Action action)
      {
        HttpHandlerUtil.ServerExecuteHttpHandlerWrapper.Wrap<object>((Func<object>) (() =>
        {
          action();
          return (object) null;
        }));
      }

      protected static TResult Wrap<TResult>(Func<TResult> func)
      {
        try
        {
          return func();
        }
        catch (HttpException ex)
        {
          if (ex.GetHttpCode() != 500)
            throw new HttpException(500, MvcResources.ViewPageHttpHandlerWrapper_ExceptionOccurred, (Exception) ex);
          throw;
        }
      }
    }

    private sealed class ServerExecuteHttpHandlerAsyncWrapper : 
      HttpHandlerUtil.ServerExecuteHttpHandlerWrapper,
      IHttpAsyncHandler,
      IHttpHandler
    {
      private readonly IHttpAsyncHandler _httpHandler;

      public ServerExecuteHttpHandlerAsyncWrapper(IHttpAsyncHandler httpHandler)
        : base((IHttpHandler) httpHandler)
      {
        this._httpHandler = httpHandler;
      }

      public IAsyncResult BeginProcessRequest(
        HttpContext context,
        AsyncCallback cb,
        object extraData)
      {
        return HttpHandlerUtil.ServerExecuteHttpHandlerWrapper.Wrap<IAsyncResult>((Func<IAsyncResult>) (() => this._httpHandler.BeginProcessRequest(context, cb, extraData)));
      }

      public void EndProcessRequest(IAsyncResult result)
      {
        HttpHandlerUtil.ServerExecuteHttpHandlerWrapper.Wrap((Action) (() => this._httpHandler.EndProcessRequest(result)));
      }
    }
  }
}
