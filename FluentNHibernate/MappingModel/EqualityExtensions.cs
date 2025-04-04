// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.EqualityExtensions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  public static class EqualityExtensions
  {
    public static bool ContentEquals<TKey, TValue>(
      this IDictionary<TKey, TValue> left,
      IDictionary<TKey, TValue> right)
    {
      if (left.Count<KeyValuePair<TKey, TValue>>() != right.Count<KeyValuePair<TKey, TValue>>())
        return false;
      int index = 0;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) left)
      {
        if (!keyValuePair.Equals((object) right.ElementAt<KeyValuePair<TKey, TValue>>(index)))
          return false;
        ++index;
      }
      return true;
    }

    public static bool ContentEquals<T>(this IEnumerable<T> left, IEnumerable<T> right)
    {
      if (left.Count<T>() != right.Count<T>())
        return false;
      int index = 0;
      foreach (T obj in left)
      {
        if (!obj.Equals((object) right.ElementAt<T>(index)))
          return false;
        ++index;
      }
      return true;
    }
  }
}
