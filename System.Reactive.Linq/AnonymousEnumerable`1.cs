// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousEnumerable`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace System.Reactive
{
  internal sealed class AnonymousEnumerable<T> : IEnumerable<T>, IEnumerable
  {
    private readonly Func<IEnumerator<T>> getEnumerator;

    public AnonymousEnumerable(Func<IEnumerator<T>> getEnumerator)
    {
      this.getEnumerator = getEnumerator;
    }

    public IEnumerator<T> GetEnumerator() => this.getEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
