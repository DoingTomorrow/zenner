// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.AsyncActionDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc.Async
{
  public abstract class AsyncActionDescriptor : ActionDescriptor
  {
    public abstract IAsyncResult BeginExecute(
      ControllerContext controllerContext,
      IDictionary<string, object> parameters,
      AsyncCallback callback,
      object state);

    public abstract object EndExecute(IAsyncResult asyncResult);

    public override object Execute(
      ControllerContext controllerContext,
      IDictionary<string, object> parameters)
    {
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.AsyncActionDescriptor_CannotExecuteSynchronously, new object[1]
      {
        (object) this.ActionName
      }));
    }

    internal static AsyncManager GetAsyncManager(ControllerBase controller)
    {
      return controller is IAsyncManagerContainer managerContainer ? managerContainer.AsyncManager : throw Error.AsyncCommon_ControllerMustImplementIAsyncManagerContainer(controller.GetType());
    }
  }
}
