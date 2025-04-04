// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewEngineCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ViewEngineCollection : Collection<IViewEngine>
  {
    private IResolver<IEnumerable<IViewEngine>> _serviceResolver;

    public ViewEngineCollection()
    {
      this._serviceResolver = (IResolver<IEnumerable<IViewEngine>>) new MultiServiceResolver<IViewEngine>((Func<IEnumerable<IViewEngine>>) (() => (IEnumerable<IViewEngine>) this.Items));
    }

    public ViewEngineCollection(IList<IViewEngine> list)
      : base(list)
    {
      this._serviceResolver = (IResolver<IEnumerable<IViewEngine>>) new MultiServiceResolver<IViewEngine>((Func<IEnumerable<IViewEngine>>) (() => (IEnumerable<IViewEngine>) this.Items));
    }

    internal ViewEngineCollection(
      IResolver<IEnumerable<IViewEngine>> serviceResolver,
      params IViewEngine[] engines)
      : base((IList<IViewEngine>) engines)
    {
      this._serviceResolver = serviceResolver ?? (IResolver<IEnumerable<IViewEngine>>) new MultiServiceResolver<IViewEngine>((Func<IEnumerable<IViewEngine>>) (() => (IEnumerable<IViewEngine>) this.Items));
    }

    private IEnumerable<IViewEngine> CombinedItems => this._serviceResolver.Current;

    protected override void InsertItem(int index, IViewEngine item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.InsertItem(index, item);
    }

    protected override void SetItem(int index, IViewEngine item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.SetItem(index, item);
    }

    private ViewEngineResult Find(
      Func<IViewEngine, ViewEngineResult> cacheLocator,
      Func<IViewEngine, ViewEngineResult> locator)
    {
      return this.Find(cacheLocator, false) ?? this.Find(locator, true);
    }

    private ViewEngineResult Find(
      Func<IViewEngine, ViewEngineResult> lookup,
      bool trackSearchedPaths)
    {
      List<string> source = (List<string>) null;
      if (trackSearchedPaths)
        source = new List<string>();
      foreach (IViewEngine combinedItem in this.CombinedItems)
      {
        if (combinedItem != null)
        {
          ViewEngineResult viewEngineResult = lookup(combinedItem);
          if (viewEngineResult.View != null)
            return viewEngineResult;
          if (trackSearchedPaths)
            source.AddRange(viewEngineResult.SearchedLocations);
        }
      }
      return trackSearchedPaths ? new ViewEngineResult((IEnumerable<string>) source.Distinct<string>().ToList<string>()) : (ViewEngineResult) null;
    }

    public virtual ViewEngineResult FindPartialView(
      ControllerContext controllerContext,
      string partialViewName)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(partialViewName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (partialViewName));
      return this.Find((Func<IViewEngine, ViewEngineResult>) (e => e.FindPartialView(controllerContext, partialViewName, true)), (Func<IViewEngine, ViewEngineResult>) (e => e.FindPartialView(controllerContext, partialViewName, false)));
    }

    public virtual ViewEngineResult FindView(
      ControllerContext controllerContext,
      string viewName,
      string masterName)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(viewName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (viewName));
      return this.Find((Func<IViewEngine, ViewEngineResult>) (e => e.FindView(controllerContext, viewName, masterName, true)), (Func<IViewEngine, ViewEngineResult>) (e => e.FindView(controllerContext, viewName, masterName, false)));
    }
  }
}
