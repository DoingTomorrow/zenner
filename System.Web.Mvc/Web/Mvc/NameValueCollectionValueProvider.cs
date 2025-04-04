// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.NameValueCollectionValueProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;

#nullable disable
namespace System.Web.Mvc
{
  public class NameValueCollectionValueProvider : 
    IUnvalidatedValueProvider,
    IEnumerableValueProvider,
    IValueProvider
  {
    private readonly Lazy<PrefixContainer> _prefixContainer;
    private readonly Dictionary<string, NameValueCollectionValueProvider.ValueProviderResultPlaceholder> _values = new Dictionary<string, NameValueCollectionValueProvider.ValueProviderResultPlaceholder>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public NameValueCollectionValueProvider(NameValueCollection collection, CultureInfo culture)
      : this(collection, (NameValueCollection) null, culture)
    {
    }

    public NameValueCollectionValueProvider(
      NameValueCollection collection,
      NameValueCollection unvalidatedCollection,
      CultureInfo culture)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof (collection));
      unvalidatedCollection = unvalidatedCollection ?? collection;
      this._prefixContainer = new Lazy<PrefixContainer>((Func<PrefixContainer>) (() => new PrefixContainer((ICollection<string>) unvalidatedCollection.AllKeys)), true);
      foreach (string unvalidated in (NameObjectCollectionBase) unvalidatedCollection)
      {
        if (unvalidated != null)
          this._values[unvalidated] = new NameValueCollectionValueProvider.ValueProviderResultPlaceholder(unvalidated, collection, unvalidatedCollection, culture);
      }
    }

    public virtual bool ContainsPrefix(string prefix)
    {
      return this._prefixContainer.Value.ContainsPrefix(prefix);
    }

    public virtual ValueProviderResult GetValue(string key) => this.GetValue(key, false);

    public virtual ValueProviderResult GetValue(string key, bool skipValidation)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      NameValueCollectionValueProvider.ValueProviderResultPlaceholder resultPlaceholder;
      this._values.TryGetValue(key, out resultPlaceholder);
      if (resultPlaceholder == null)
        return (ValueProviderResult) null;
      return !skipValidation ? resultPlaceholder.ValidatedResult : resultPlaceholder.UnvalidatedResult;
    }

    public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
    {
      return this._prefixContainer.Value.GetKeysFromPrefix(prefix);
    }

    private sealed class ValueProviderResultPlaceholder
    {
      private readonly Lazy<ValueProviderResult> _validatedResultPlaceholder;
      private readonly Lazy<ValueProviderResult> _unvalidatedResultPlaceholder;

      public ValueProviderResultPlaceholder(
        string key,
        NameValueCollection validatedCollection,
        NameValueCollection unvalidatedCollection,
        CultureInfo culture)
      {
        this._validatedResultPlaceholder = new Lazy<ValueProviderResult>((Func<ValueProviderResult>) (() => NameValueCollectionValueProvider.ValueProviderResultPlaceholder.GetResultFromCollection(key, validatedCollection, culture)), LazyThreadSafetyMode.None);
        this._unvalidatedResultPlaceholder = new Lazy<ValueProviderResult>((Func<ValueProviderResult>) (() => NameValueCollectionValueProvider.ValueProviderResultPlaceholder.GetResultFromCollection(key, unvalidatedCollection, culture)), LazyThreadSafetyMode.None);
      }

      public ValueProviderResult ValidatedResult => this._validatedResultPlaceholder.Value;

      public ValueProviderResult UnvalidatedResult => this._unvalidatedResultPlaceholder.Value;

      private static ValueProviderResult GetResultFromCollection(
        string key,
        NameValueCollection collection,
        CultureInfo culture)
      {
        return new ValueProviderResult((object) collection.GetValues(key), collection[key], culture);
      }
    }
  }
}
