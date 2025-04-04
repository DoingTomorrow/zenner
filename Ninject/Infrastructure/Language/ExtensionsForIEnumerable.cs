// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Language.ExtensionsForIEnumerable
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Collections;
using System.Linq;

#nullable disable
namespace Ninject.Infrastructure.Language
{
  internal static class ExtensionsForIEnumerable
  {
    public static IEnumerable CastSlow(this IEnumerable series, Type elementType)
    {
      return typeof (Enumerable).GetMethod("Cast").MakeGenericMethod(elementType).Invoke((object) null, (object[]) new IEnumerable[1]
      {
        series
      }) as IEnumerable;
    }

    public static Array ToArraySlow(this IEnumerable series, Type elementType)
    {
      return typeof (Enumerable).GetMethod("ToArray").MakeGenericMethod(elementType).Invoke((object) null, (object[]) new IEnumerable[1]
      {
        series
      }) as Array;
    }

    public static IList ToListSlow(this IEnumerable series, Type elementType)
    {
      return typeof (Enumerable).GetMethod("ToList").MakeGenericMethod(elementType).Invoke((object) null, (object[]) new IEnumerable[1]
      {
        series
      }) as IList;
    }
  }
}
