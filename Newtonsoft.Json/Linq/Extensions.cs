﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.Extensions
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public static class Extensions
  {
    public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.Ancestors())).AsJEnumerable();
    }

    public static IJEnumerable<JToken> AncestorsAndSelf<T>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.AncestorsAndSelf())).AsJEnumerable();
    }

    public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.Descendants())).AsJEnumerable();
    }

    public static IJEnumerable<JToken> DescendantsAndSelf<T>(this IEnumerable<T> source) where T : JContainer
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.DescendantsAndSelf())).AsJEnumerable();
    }

    public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<JObject, JProperty>((Func<JObject, IEnumerable<JProperty>>) (d => d.Properties())).AsJEnumerable<JProperty>();
    }

    public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
    {
      return source.Values<JToken, JToken>(key).AsJEnumerable();
    }

    public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
    {
      return source.Values((object) null);
    }

    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
    {
      return source.Values<JToken, U>(key);
    }

    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
    {
      return source.Values<JToken, U>((object) null);
    }

    public static U Value<U>(this IEnumerable<JToken> value) => value.Value<JToken, U>();

    public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      return value is JToken token ? token.Convert<JToken, U>() : throw new ArgumentException("Source value must be a JToken.");
    }

    internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      foreach (T obj in source)
      {
        JToken token = (JToken) obj;
        if (key == null)
        {
          if (token is JValue)
          {
            yield return ((JValue) token).Convert<JValue, U>();
          }
          else
          {
            foreach (JToken child in token.Children())
              yield return child.Convert<JToken, U>();
          }
        }
        else
        {
          JToken token1 = token[key];
          if (token1 != null)
            yield return token1.Convert<JToken, U>();
        }
        token = (JToken) null;
      }
    }

    public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
    {
      return source.Children<T, JToken>().AsJEnumerable();
    }

    public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (c => (IEnumerable<JToken>) c.Children())).Convert<JToken, U>();
    }

    internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      foreach (T token in source)
        yield return token.Convert<JToken, U>();
    }

    internal static U Convert<T, U>(this T token) where T : JToken
    {
      if ((object) token == null)
        return default (U);
      if ((object) token is U && typeof (U) != typeof (IComparable) && typeof (U) != typeof (IFormattable))
        return (U) (object) token;
      if (!(token is JValue jvalue))
        throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) token.GetType(), (object) typeof (T)));
      if (jvalue.Value is U)
        return (U) jvalue.Value;
      Type type = typeof (U);
      if (ReflectionUtils.IsNullableType(type))
      {
        if (jvalue.Value == null)
          return default (U);
        type = Nullable.GetUnderlyingType(type);
      }
      return (U) System.Convert.ChangeType(jvalue.Value, type, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
    {
      return source.AsJEnumerable<JToken>();
    }

    public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
    {
      if (source == null)
        return (IJEnumerable<T>) null;
      return source is IJEnumerable<T> ? (IJEnumerable<T>) source : (IJEnumerable<T>) new JEnumerable<T>(source);
    }
  }
}
