// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelBinderProviderCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class ModelBinderProviderCollection : Collection<IModelBinderProvider>
  {
    private IResolver<IEnumerable<IModelBinderProvider>> _serviceResolver;

    public ModelBinderProviderCollection()
    {
      this._serviceResolver = (IResolver<IEnumerable<IModelBinderProvider>>) new MultiServiceResolver<IModelBinderProvider>((Func<IEnumerable<IModelBinderProvider>>) (() => (IEnumerable<IModelBinderProvider>) this.Items));
    }

    public ModelBinderProviderCollection(IList<IModelBinderProvider> list)
      : base(list)
    {
      this._serviceResolver = (IResolver<IEnumerable<IModelBinderProvider>>) new MultiServiceResolver<IModelBinderProvider>((Func<IEnumerable<IModelBinderProvider>>) (() => (IEnumerable<IModelBinderProvider>) this.Items));
    }

    internal ModelBinderProviderCollection(
      IResolver<IEnumerable<IModelBinderProvider>> resolver,
      params IModelBinderProvider[] providers)
      : base((IList<IModelBinderProvider>) providers)
    {
      this._serviceResolver = resolver ?? (IResolver<IEnumerable<IModelBinderProvider>>) new MultiServiceResolver<IModelBinderProvider>((Func<IEnumerable<IModelBinderProvider>>) (() => (IEnumerable<IModelBinderProvider>) this.Items));
    }

    private IEnumerable<IModelBinderProvider> CombinedItems => this._serviceResolver.Current;

    protected override void InsertItem(int index, IModelBinderProvider item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.InsertItem(index, item);
    }

    protected override void SetItem(int index, IModelBinderProvider item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.SetItem(index, item);
    }

    public IModelBinder GetBinder(Type modelType)
    {
      if (modelType == (Type) null)
        throw new ArgumentNullException(nameof (modelType));
      return this.CombinedItems.Select(providers => new
      {
        providers = providers,
        modelBinder = providers.GetBinder(modelType)
      }).Where(_param0 => _param0.modelBinder != null).Select(_param0 => _param0.modelBinder).FirstOrDefault<IModelBinder>();
    }
  }
}
