// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ValueProviderCollection
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class ValueProviderCollection : 
    Collection<IValueProvider>,
    IUnvalidatedValueProvider,
    IEnumerableValueProvider,
    IValueProvider
  {
    public ValueProviderCollection()
    {
    }

    public ValueProviderCollection(IList<IValueProvider> list)
      : base(list)
    {
    }

    public virtual bool ContainsPrefix(string prefix)
    {
      return this.Any<IValueProvider>((Func<IValueProvider, bool>) (vp => vp.ContainsPrefix(prefix)));
    }

    public virtual ValueProviderResult GetValue(string key) => this.GetValue(key, false);

    public virtual ValueProviderResult GetValue(string key, bool skipValidation)
    {
      return this.Select(provider => new
      {
        provider = provider,
        result = ValueProviderCollection.GetValueFromProvider(provider, key, skipValidation)
      }).Where(_param0 => _param0.result != null).Select(_param0 => _param0.result).FirstOrDefault<ValueProviderResult>();
    }

    public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
    {
      return this.Select(provider => new
      {
        provider = provider,
        result = ValueProviderCollection.GetKeysFromPrefixFromProvider(provider, prefix)
      }).Where(_param0 => _param0.result != null && _param0.result.Any<KeyValuePair<string, string>>()).Select(_param0 => _param0.result).FirstOrDefault<IDictionary<string, string>>() ?? (IDictionary<string, string>) new Dictionary<string, string>();
    }

    internal static ValueProviderResult GetValueFromProvider(
      IValueProvider provider,
      string key,
      bool skipValidation)
    {
      return !(provider is IUnvalidatedValueProvider unvalidatedValueProvider) ? provider.GetValue(key) : unvalidatedValueProvider.GetValue(key, skipValidation);
    }

    internal static IDictionary<string, string> GetKeysFromPrefixFromProvider(
      IValueProvider provider,
      string prefix)
    {
      return !(provider is IEnumerableValueProvider enumerableValueProvider) ? (IDictionary<string, string>) null : enumerableValueProvider.GetKeysFromPrefix(prefix);
    }

    protected override void InsertItem(int index, IValueProvider item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.InsertItem(index, item);
    }

    protected override void SetItem(int index, IValueProvider item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      base.SetItem(index, item);
    }
  }
}
