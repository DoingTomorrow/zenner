// Decompiled with JetBrains decompiler
// Type: System.Reactive.Helpers
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive
{
  internal static class Helpers
  {
    public static int? GetLength<T>(IEnumerable<T> source)
    {
      switch (source)
      {
        case T[] objArray:
          return new int?(objArray.Length);
        case IList<T> objList:
          return new int?(objList.Count);
        default:
          return new int?();
      }
    }

    public static IObservable<T> Unpack<T>(IObservable<T> source)
    {
      bool flag;
      do
      {
        flag = false;
        if (source is IEvaluatableObservable<T> evaluatableObservable)
        {
          source = evaluatableObservable.Eval();
          flag = true;
        }
      }
      while (flag);
      return source;
    }
  }
}
