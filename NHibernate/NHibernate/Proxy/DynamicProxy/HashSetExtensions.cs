// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.HashSetExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  public static class HashSetExtensions
  {
    public static HashSet<T> Merge<T>(this HashSet<T> source, IEnumerable<T> toMerge)
    {
      foreach (T obj in toMerge)
        source.Add(obj);
      return source;
    }
  }
}
