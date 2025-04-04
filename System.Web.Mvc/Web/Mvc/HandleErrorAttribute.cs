// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HandleErrorAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
  public class HandleErrorAttribute : FilterAttribute, IExceptionFilter
  {
    private const string DefaultView = "Error";
    private readonly object _typeId = new object();
    private Type _exceptionType = typeof (Exception);
    private string _master;
    private string _view;

    public Type ExceptionType
    {
      get => this._exceptionType;
      set
      {
        if (value == (Type) null)
          throw new ArgumentNullException(nameof (value));
        this._exceptionType = typeof (Exception).IsAssignableFrom(value) ? value : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ExceptionViewAttribute_NonExceptionType, new object[1]
        {
          (object) value.FullName
        }));
      }
    }

    public string Master
    {
      get => this._master ?? string.Empty;
      set => this._master = value;
    }

    public override object TypeId => this._typeId;

    public string View
    {
      get => string.IsNullOrEmpty(this._view) ? "Error" : this._view;
      set => this._view = value;
    }

    public virtual void OnException(ExceptionContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (filterContext.IsChildAction || filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
        return;
      Exception exception = filterContext.Exception;
      if (new HttpException((string) null, exception).GetHttpCode() != 500 || !this.ExceptionType.IsInstanceOfType((object) exception))
        return;
      string controllerName = (string) filterContext.RouteData.Values["controller"];
      string actionName = (string) filterContext.RouteData.Values["action"];
      HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
      ExceptionContext exceptionContext = filterContext;
      ViewResult viewResult1 = new ViewResult();
      viewResult1.ViewName = this.View;
      viewResult1.MasterName = this.Master;
      viewResult1.ViewData = (ViewDataDictionary) new ViewDataDictionary<HandleErrorInfo>(model);
      viewResult1.TempData = filterContext.Controller.TempData;
      ViewResult viewResult2 = viewResult1;
      exceptionContext.Result = (ActionResult) viewResult2;
      filterContext.ExceptionHandled = true;
      filterContext.HttpContext.Response.Clear();
      filterContext.HttpContext.Response.StatusCode = 500;
      filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
    }
  }
}
