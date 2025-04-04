// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.FilterKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cache
{
  [Serializable]
  public class FilterKey
  {
    private readonly string filterName;
    private readonly Dictionary<string, TypedValue> filterParameters = new Dictionary<string, TypedValue>();

    public FilterKey(
      string name,
      IEnumerable<KeyValuePair<string, object>> @params,
      IDictionary<string, IType> types,
      EntityMode entityMode)
    {
      this.filterName = name;
      foreach (KeyValuePair<string, object> keyValuePair in @params)
      {
        IType type = types[keyValuePair.Key];
        this.filterParameters[keyValuePair.Key] = new TypedValue(type, keyValuePair.Value, entityMode);
      }
    }

    public override int GetHashCode()
    {
      return 37 * (37 * 13 + this.filterName.GetHashCode()) + CollectionHelper.GetHashCode<KeyValuePair<string, TypedValue>>((IEnumerable<KeyValuePair<string, TypedValue>>) this.filterParameters);
    }

    public override bool Equals(object other)
    {
      return other is FilterKey filterKey && filterKey.filterName.Equals(this.filterName) && CollectionHelper.DictionaryEquals<string, TypedValue>((IDictionary<string, TypedValue>) filterKey.filterParameters, (IDictionary<string, TypedValue>) this.filterParameters);
    }

    public override string ToString()
    {
      return string.Format("FilterKey[{0}{1}]", (object) this.filterName, (object) CollectionPrinter.ToString((IDictionary) this.filterParameters));
    }

    public static ISet CreateFilterKeys(
      IDictionary<string, IFilter> enabledFilters,
      EntityMode entityMode)
    {
      if (enabledFilters.Count == 0)
        return (ISet) null;
      Set filterKeys = (Set) new HashedSet();
      foreach (FilterImpl filterImpl in (IEnumerable<IFilter>) enabledFilters.Values)
      {
        FilterKey o = new FilterKey(filterImpl.Name, (IEnumerable<KeyValuePair<string, object>>) filterImpl.Parameters, filterImpl.FilterDefinition.ParameterTypes, entityMode);
        filterKeys.Add((object) o);
      }
      return (ISet) filterKeys;
    }
  }
}
