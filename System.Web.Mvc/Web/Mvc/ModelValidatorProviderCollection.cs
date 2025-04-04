// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelValidatorProviderCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class ModelValidatorProviderCollection : Collection<ModelValidatorProvider>
  {
    private IResolver<IEnumerable<ModelValidatorProvider>> _serviceResolver;

    public ModelValidatorProviderCollection()
    {
      this._serviceResolver = (IResolver<IEnumerable<ModelValidatorProvider>>) new MultiServiceResolver<ModelValidatorProvider>((Func<IEnumerable<ModelValidatorProvider>>) (() => (IEnumerable<ModelValidatorProvider>) this.Items));
    }

    public ModelValidatorProviderCollection(IList<ModelValidatorProvider> list)
      : base(list)
    {
      this._serviceResolver = (IResolver<IEnumerable<ModelValidatorProvider>>) new MultiServiceResolver<ModelValidatorProvider>((Func<IEnumerable<ModelValidatorProvider>>) (() => (IEnumerable<ModelValidatorProvider>) this.Items));
    }

    internal ModelValidatorProviderCollection(
      IResolver<IEnumerable<ModelValidatorProvider>> serviceResolver,
      params ModelValidatorProvider[] validatorProvidors)
      : base((IList<ModelValidatorProvider>) validatorProvidors)
    {
      this._serviceResolver = serviceResolver ?? (IResolver<IEnumerable<ModelValidatorProvider>>) new MultiServiceResolver<ModelValidatorProvider>((Func<IEnumerable<ModelValidatorProvider>>) (() => (IEnumerable<ModelValidatorProvider>) this.Items));
    }

    private IEnumerable<ModelValidatorProvider> CombinedItems => this._serviceResolver.Current;

    protected override void InsertItem(int index, ModelValidatorProvider item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.InsertItem(index, item);
    }

    protected override void SetItem(int index, ModelValidatorProvider item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.SetItem(index, item);
    }

    public IEnumerable<ModelValidator> GetValidators(
      ModelMetadata metadata,
      ControllerContext context)
    {
      return this.CombinedItems.SelectMany<ModelValidatorProvider, ModelValidator>((Func<ModelValidatorProvider, IEnumerable<ModelValidator>>) (provider => provider.GetValidators(metadata, context)));
    }
  }
}
