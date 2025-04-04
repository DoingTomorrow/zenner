// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.GlobalFilterCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public sealed class GlobalFilterCollection : IEnumerable<Filter>, IEnumerable, IFilterProvider
  {
    private List<Filter> _filters = new List<Filter>();

    public int Count => this._filters.Count;

    public void Add(object filter) => this.AddInternal(filter, new int?());

    public void Add(object filter, int order) => this.AddInternal(filter, new int?(order));

    private void AddInternal(object filter, int? order)
    {
      GlobalFilterCollection.ValidateFilterInstance(filter);
      this._filters.Add(new Filter(filter, FilterScope.Global, order));
    }

    public void Clear() => this._filters.Clear();

    public bool Contains(object filter)
    {
      return this._filters.Any<Filter>((Func<Filter, bool>) (f => f.Instance == filter));
    }

    public IEnumerator<Filter> GetEnumerator()
    {
      return (IEnumerator<Filter>) this._filters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._filters.GetEnumerator();

    IEnumerable<Filter> IFilterProvider.GetFilters(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      return (IEnumerable<Filter>) this;
    }

    public void Remove(object filter)
    {
      this._filters.RemoveAll((Predicate<Filter>) (f => f.Instance == filter));
    }

    private static void ValidateFilterInstance(object instance)
    {
      switch (instance)
      {
        case null:
          break;
        case IActionFilter _:
          break;
        case IAuthorizationFilter _:
          break;
        case IExceptionFilter _:
          break;
        case IResultFilter _:
          break;
        default:
          throw new InvalidOperationException(MvcResources.GlobalFilterCollection_UnsupportedFilterInstance);
      }
    }
  }
}
