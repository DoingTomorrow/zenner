// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MultiServiceResolver`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  internal class MultiServiceResolver<TService> : IResolver<IEnumerable<TService>> where TService : class
  {
    private Lazy<IEnumerable<TService>> _itemsFromService;
    private Func<IEnumerable<TService>> _itemsThunk;
    private Func<IDependencyResolver> _resolverThunk;

    public MultiServiceResolver(Func<IEnumerable<TService>> itemsThunk)
    {
      this._itemsThunk = itemsThunk != null ? itemsThunk : throw new ArgumentNullException(nameof (itemsThunk));
      this._resolverThunk = (Func<IDependencyResolver>) (() => DependencyResolver.Current);
      this._itemsFromService = new Lazy<IEnumerable<TService>>((Func<IEnumerable<TService>>) (() => this._resolverThunk().GetServices<TService>()));
    }

    internal MultiServiceResolver(
      Func<IEnumerable<TService>> itemsThunk,
      IDependencyResolver resolver)
      : this(itemsThunk)
    {
      if (resolver == null)
        return;
      this._resolverThunk = (Func<IDependencyResolver>) (() => resolver);
    }

    public IEnumerable<TService> Current
    {
      get => this._itemsFromService.Value.Concat<TService>(this._itemsThunk());
    }
  }
}
