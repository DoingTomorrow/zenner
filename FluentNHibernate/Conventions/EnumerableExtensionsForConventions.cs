// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.EnumerableExtensionsForConventions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public static class EnumerableExtensionsForConventions
  {
    public static bool Contains<T>(this IEnumerable<T> collection, string expected) where T : class, IInspector
    {
      return collection.Contains<T>((Func<T, bool>) (x => x.StringIdentifierForModel == expected));
    }

    public static bool Contains<T>(this IEnumerable<T> collection, Func<T, bool> prediate) where T : class, IInspector
    {
      return (object) collection.FirstOrDefault<T>(prediate) != null;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> collection) => collection.Count<T>() == 0;

    public static bool IsNotEmpty<T>(this IEnumerable<T> collection) => !collection.IsEmpty<T>();
  }
}
