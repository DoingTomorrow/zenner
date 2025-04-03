// Decompiled with JetBrains decompiler
// Type: AutoMapper.EnumerableExtensions
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace AutoMapper
{
  public static class EnumerableExtensions
  {
    public static void Each<T>(this IEnumerable<T> items, Action<T> action)
    {
      foreach (T obj in items)
        action(obj);
    }
  }
}
