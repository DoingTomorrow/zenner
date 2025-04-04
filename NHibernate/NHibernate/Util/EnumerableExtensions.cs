// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.EnumerableExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Util
{
  public static class EnumerableExtensions
  {
    public static bool Any(this IEnumerable source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      using (EnumerableExtensions.DisposableEnumerator disposableEnumerator = source.GetDisposableEnumerator())
      {
        if (disposableEnumerator.MoveNext())
          return true;
      }
      return false;
    }

    public static object First(this IEnumerable source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (source is IList list)
      {
        if (list.Count > 0)
          return list[0];
      }
      else
      {
        using (EnumerableExtensions.DisposableEnumerator disposableEnumerator = source.GetDisposableEnumerator())
        {
          if (disposableEnumerator.MoveNext())
            return disposableEnumerator.Current;
        }
      }
      throw new InvalidOperationException("Sequence contains no elements");
    }

    public static object FirstOrNull(this IEnumerable source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (source is IList list)
      {
        if (list.Count > 0)
          return list[0];
      }
      else
      {
        using (EnumerableExtensions.DisposableEnumerator disposableEnumerator = source.GetDisposableEnumerator())
        {
          if (disposableEnumerator.MoveNext())
            return disposableEnumerator.Current;
        }
      }
      return (object) null;
    }

    private static EnumerableExtensions.DisposableEnumerator GetDisposableEnumerator(
      this IEnumerable source)
    {
      return new EnumerableExtensions.DisposableEnumerator(source);
    }

    internal class DisposableEnumerator : IDisposable, IEnumerator
    {
      private readonly IEnumerator wrapped;

      public DisposableEnumerator(IEnumerable source) => this.wrapped = source.GetEnumerator();

      public void Dispose()
      {
        if (!(this.wrapped is IDisposable wrapped))
          return;
        wrapped.Dispose();
      }

      public bool MoveNext() => this.wrapped.MoveNext();

      public void Reset() => this.wrapped.Reset();

      public object Current => this.wrapped.Current;
    }
  }
}
