// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FilterInfo
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class FilterInfo
  {
    private List<IActionFilter> _actionFilters = new List<IActionFilter>();
    private List<IAuthorizationFilter> _authorizationFilters = new List<IAuthorizationFilter>();
    private List<IExceptionFilter> _exceptionFilters = new List<IExceptionFilter>();
    private List<IResultFilter> _resultFilters = new List<IResultFilter>();

    public FilterInfo()
    {
    }

    public FilterInfo(IEnumerable<Filter> filters)
    {
      List<object> list = filters.Select<Filter, object>((Func<Filter, object>) (f => f.Instance)).ToList<object>();
      this._actionFilters.AddRange(list.OfType<IActionFilter>());
      this._authorizationFilters.AddRange(list.OfType<IAuthorizationFilter>());
      this._exceptionFilters.AddRange(list.OfType<IExceptionFilter>());
      this._resultFilters.AddRange(list.OfType<IResultFilter>());
    }

    public IList<IActionFilter> ActionFilters => (IList<IActionFilter>) this._actionFilters;

    public IList<IAuthorizationFilter> AuthorizationFilters
    {
      get => (IList<IAuthorizationFilter>) this._authorizationFilters;
    }

    public IList<IExceptionFilter> ExceptionFilters
    {
      get => (IList<IExceptionFilter>) this._exceptionFilters;
    }

    public IList<IResultFilter> ResultFilters => (IList<IResultFilter>) this._resultFilters;
  }
}
