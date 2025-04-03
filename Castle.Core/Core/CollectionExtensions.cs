// Decompiled with JetBrains decompiler
// Type: Castle.Core.CollectionExtensions
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Castle.Core
{
  public static class CollectionExtensions
  {
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
      if (items == null)
        return;
      foreach (T obj in items)
        action(obj);
    }
  }
}
