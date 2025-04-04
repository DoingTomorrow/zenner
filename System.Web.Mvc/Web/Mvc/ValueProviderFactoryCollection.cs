// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValueProviderFactoryCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class ValueProviderFactoryCollection : Collection<ValueProviderFactory>
  {
    private IResolver<IEnumerable<ValueProviderFactory>> _serviceResolver;

    public ValueProviderFactoryCollection()
    {
      this._serviceResolver = (IResolver<IEnumerable<ValueProviderFactory>>) new MultiServiceResolver<ValueProviderFactory>((Func<IEnumerable<ValueProviderFactory>>) (() => (IEnumerable<ValueProviderFactory>) this.Items));
    }

    public ValueProviderFactoryCollection(IList<ValueProviderFactory> list)
      : base(list)
    {
      this._serviceResolver = (IResolver<IEnumerable<ValueProviderFactory>>) new MultiServiceResolver<ValueProviderFactory>((Func<IEnumerable<ValueProviderFactory>>) (() => (IEnumerable<ValueProviderFactory>) this.Items));
    }

    internal ValueProviderFactoryCollection(
      IResolver<IEnumerable<ValueProviderFactory>> serviceResolver,
      params ValueProviderFactory[] valueProviderFactories)
      : base((IList<ValueProviderFactory>) valueProviderFactories)
    {
      this._serviceResolver = serviceResolver ?? (IResolver<IEnumerable<ValueProviderFactory>>) new MultiServiceResolver<ValueProviderFactory>((Func<IEnumerable<ValueProviderFactory>>) (() => (IEnumerable<ValueProviderFactory>) this.Items));
    }

    public IValueProvider GetValueProvider(ControllerContext controllerContext)
    {
      return (IValueProvider) new ValueProviderCollection((IList<IValueProvider>) this._serviceResolver.Current.Select(factory => new
      {
        factory = factory,
        valueProvider = factory.GetValueProvider(controllerContext)
      }).Where(_param0 => _param0.valueProvider != null).Select(_param0 => _param0.valueProvider).ToList<IValueProvider>());
    }

    protected override void InsertItem(int index, ValueProviderFactory item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.InsertItem(index, item);
    }

    protected override void SetItem(int index, ValueProviderFactory item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.SetItem(index, item);
    }
  }
}
