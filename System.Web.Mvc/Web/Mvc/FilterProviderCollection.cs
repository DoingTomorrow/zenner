// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FilterProviderCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class FilterProviderCollection : Collection<IFilterProvider>
  {
    private static FilterProviderCollection.FilterComparer _filterComparer = new FilterProviderCollection.FilterComparer();
    private IResolver<IEnumerable<IFilterProvider>> _serviceResolver;

    public FilterProviderCollection()
    {
      this._serviceResolver = (IResolver<IEnumerable<IFilterProvider>>) new MultiServiceResolver<IFilterProvider>((Func<IEnumerable<IFilterProvider>>) (() => (IEnumerable<IFilterProvider>) this.Items));
    }

    public FilterProviderCollection(IList<IFilterProvider> providers)
      : base(providers)
    {
      this._serviceResolver = (IResolver<IEnumerable<IFilterProvider>>) new MultiServiceResolver<IFilterProvider>((Func<IEnumerable<IFilterProvider>>) (() => (IEnumerable<IFilterProvider>) this.Items));
    }

    internal FilterProviderCollection(
      IResolver<IEnumerable<IFilterProvider>> serviceResolver,
      params IFilterProvider[] providers)
      : base((IList<IFilterProvider>) providers)
    {
      this._serviceResolver = serviceResolver ?? (IResolver<IEnumerable<IFilterProvider>>) new MultiServiceResolver<IFilterProvider>((Func<IEnumerable<IFilterProvider>>) (() => (IEnumerable<IFilterProvider>) this.Items));
    }

    private IEnumerable<IFilterProvider> CombinedItems => this._serviceResolver.Current;

    private static bool AllowMultiple(object filterInstance)
    {
      return !(filterInstance is IMvcFilter mvcFilter) || mvcFilter.AllowMultiple;
    }

    public IEnumerable<Filter> GetFilters(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (actionDescriptor == null)
        throw new ArgumentNullException(nameof (actionDescriptor));
      return this.RemoveDuplicates(this.CombinedItems.SelectMany<IFilterProvider, Filter>((Func<IFilterProvider, IEnumerable<Filter>>) (fp => fp.GetFilters(controllerContext, actionDescriptor))).OrderBy<Filter, Filter>((Func<Filter, Filter>) (filter => filter), (IComparer<Filter>) FilterProviderCollection._filterComparer).Reverse<Filter>()).Reverse<Filter>();
    }

    private IEnumerable<Filter> RemoveDuplicates(IEnumerable<Filter> filters)
    {
      HashSet<Type> visitedTypes = new HashSet<Type>();
      foreach (Filter filter in filters)
      {
        object filterInstance = filter.Instance;
        Type filterInstanceType = filterInstance.GetType();
        if (!visitedTypes.Contains(filterInstanceType) || FilterProviderCollection.AllowMultiple(filterInstance))
        {
          yield return filter;
          visitedTypes.Add(filterInstanceType);
        }
      }
    }

    private class FilterComparer : IComparer<Filter>
    {
      public int Compare(Filter x, Filter y)
      {
        if (x == null && y == null)
          return 0;
        if (x == null)
          return -1;
        if (y == null)
          return 1;
        if (x.Order < y.Order)
          return -1;
        if (x.Order > y.Order)
          return 1;
        if (x.Scope < y.Scope)
          return -1;
        return x.Scope > y.Scope ? 1 : 0;
      }
    }
  }
}
