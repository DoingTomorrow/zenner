// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.DictionaryValueProvider`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace System.Web.Mvc
{
  public class DictionaryValueProvider<TValue> : IEnumerableValueProvider, IValueProvider
  {
    private readonly Lazy<PrefixContainer> _prefixContainer;
    private readonly Dictionary<string, ValueProviderResult> _values = new Dictionary<string, ValueProviderResult>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public DictionaryValueProvider(IDictionary<string, TValue> dictionary, CultureInfo culture)
    {
      if (dictionary == null)
        throw new ArgumentNullException(nameof (dictionary));
      foreach (KeyValuePair<string, TValue> keyValuePair in (IEnumerable<KeyValuePair<string, TValue>>) dictionary)
      {
        object rawValue = (object) keyValuePair.Value;
        string attemptedValue = Convert.ToString(rawValue, (IFormatProvider) culture);
        this._values[keyValuePair.Key] = new ValueProviderResult(rawValue, attemptedValue, culture);
      }
      this._prefixContainer = new Lazy<PrefixContainer>((Func<PrefixContainer>) (() => new PrefixContainer((ICollection<string>) this._values.Keys)), true);
    }

    public virtual bool ContainsPrefix(string prefix)
    {
      return this._prefixContainer.Value.ContainsPrefix(prefix);
    }

    public virtual ValueProviderResult GetValue(string key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      ValueProviderResult valueProviderResult;
      this._values.TryGetValue(key, out valueProviderResult);
      return valueProviderResult;
    }

    public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
    {
      return this._prefixContainer.Value.GetKeysFromPrefix(prefix);
    }
  }
}
