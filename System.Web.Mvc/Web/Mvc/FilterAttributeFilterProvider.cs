// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FilterAttributeFilterProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class FilterAttributeFilterProvider : IFilterProvider
  {
    private readonly bool _cacheAttributeInstances;

    public FilterAttributeFilterProvider()
      : this(true)
    {
    }

    public FilterAttributeFilterProvider(bool cacheAttributeInstances)
    {
      this._cacheAttributeInstances = cacheAttributeInstances;
    }

    protected virtual IEnumerable<FilterAttribute> GetActionAttributes(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      return actionDescriptor.GetFilterAttributes(this._cacheAttributeInstances);
    }

    protected virtual IEnumerable<FilterAttribute> GetControllerAttributes(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      return actionDescriptor.ControllerDescriptor.GetFilterAttributes(this._cacheAttributeInstances);
    }

    public virtual IEnumerable<Filter> GetFilters(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      return controllerContext.Controller == null ? Enumerable.Empty<Filter>() : (IEnumerable<Filter>) this.GetControllerAttributes(controllerContext, actionDescriptor).Select<FilterAttribute, Filter>((Func<FilterAttribute, Filter>) (attr => new Filter((object) attr, FilterScope.Controller, new int?()))).Concat<Filter>(this.GetActionAttributes(controllerContext, actionDescriptor).Select<FilterAttribute, Filter>((Func<FilterAttribute, Filter>) (attr => new Filter((object) attr, FilterScope.Action, new int?())))).ToList<Filter>();
    }
  }
}
