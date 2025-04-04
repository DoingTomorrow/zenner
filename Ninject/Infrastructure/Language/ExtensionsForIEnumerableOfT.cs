// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Language.ExtensionsForIEnumerableOfT
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Infrastructure.Language
{
  public static class ExtensionsForIEnumerableOfT
  {
    public static void Map<T>(this IEnumerable<T> series, Action<T> action)
    {
      foreach (T obj in series)
        action(obj);
    }

    public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> series)
    {
      return series.Select<T, T>((Func<T, T>) (x => x));
    }
  }
}
