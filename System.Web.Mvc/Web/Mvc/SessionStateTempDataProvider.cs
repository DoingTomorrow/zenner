// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.SessionStateTempDataProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class SessionStateTempDataProvider : ITempDataProvider
  {
    internal const string TempDataSessionStateKey = "__ControllerTempData";

    public virtual IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
    {
      HttpSessionStateBase session = controllerContext.HttpContext.Session;
      if (session == null || !(session["__ControllerTempData"] is Dictionary<string, object> dictionary))
        return (IDictionary<string, object>) new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      session.Remove("__ControllerTempData");
      return (IDictionary<string, object>) dictionary;
    }

    public virtual void SaveTempData(
      ControllerContext controllerContext,
      IDictionary<string, object> values)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      HttpSessionStateBase session = controllerContext.HttpContext.Session;
      bool flag = values != null && values.Count > 0;
      if (session == null)
      {
        if (flag)
          throw new InvalidOperationException(MvcResources.SessionStateTempDataProvider_SessionStateDisabled);
      }
      else if (flag)
      {
        session["__ControllerTempData"] = (object) values;
      }
      else
      {
        if (session["__ControllerTempData"] == null)
          return;
        session.Remove("__ControllerTempData");
      }
    }
  }
}
