// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.Extensions.CollectionExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

#nullable disable
namespace RestSharp.Authenticators.OAuth.Extensions
{
  internal static class CollectionExtensions
  {
    public static IEnumerable<T> AsEnumerable<T>(this T item)
    {
      return (IEnumerable<T>) new T[1]{ item };
    }

    public static IEnumerable<T> And<T>(this T item, T other)
    {
      return (IEnumerable<T>) new T[2]{ item, other };
    }

    public static IEnumerable<T> And<T>(this IEnumerable<T> items, T item)
    {
      foreach (T i in items)
        yield return i;
      yield return item;
    }

    public static K TryWithKey<T, K>(this IDictionary<T, K> dictionary, T key)
    {
      return !dictionary.ContainsKey(key) ? default (K) : dictionary[key];
    }

    public static IEnumerable<T> ToEnumerable<T>(this object[] items) where T : class
    {
      foreach (object obj in items)
      {
        T record = obj as T;
        yield return record;
      }
    }

    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
      foreach (T obj in items)
        action(obj);
    }

    public static void AddRange(
      this IDictionary<string, string> collection,
      NameValueCollection range)
    {
      foreach (string allKey in range.AllKeys)
        collection.Add(allKey, range[allKey]);
    }

    public static string ToQueryString(this NameValueCollection collection)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (collection.Count > 0)
        stringBuilder.Append("?");
      int num = 0;
      foreach (string allKey in collection.AllKeys)
      {
        stringBuilder.AppendFormat("{0}={1}", (object) allKey, (object) collection[allKey].UrlEncode());
        ++num;
        if (num < collection.Count)
          stringBuilder.Append("&");
      }
      return stringBuilder.ToString();
    }

    public static string Concatenate(
      this WebParameterCollection collection,
      string separator,
      string spacer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int count = collection.Count;
      int num = 0;
      foreach (WebPair webPair in (WebPairCollection) collection)
      {
        stringBuilder.Append(webPair.Name);
        stringBuilder.Append(separator);
        stringBuilder.Append(webPair.Value);
        ++num;
        if (num < count)
          stringBuilder.Append(spacer);
      }
      return stringBuilder.ToString();
    }
  }
}
