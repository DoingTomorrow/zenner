// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HandleErrorInfo
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class HandleErrorInfo
  {
    public HandleErrorInfo(Exception exception, string controllerName, string actionName)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      if (string.IsNullOrEmpty(controllerName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (controllerName));
      if (string.IsNullOrEmpty(actionName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (actionName));
      this.Exception = exception;
      this.ControllerName = controllerName;
      this.ActionName = actionName;
    }

    public string ActionName { get; private set; }

    public string ControllerName { get; private set; }

    public Exception Exception { get; private set; }
  }
}
