// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.CollectionExtensions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Utils
{
  public static class CollectionExtensions
  {
    [DebuggerStepThrough]
    public static void Each<T>(this IEnumerable<T> enumerable, Action<T> each)
    {
      foreach (T obj in enumerable)
        each(obj);
    }

    [DebuggerStepThrough]
    public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, params T[] singles)
    {
      return enumerable.Except<T>((IEnumerable<T>) singles);
    }
  }
}
