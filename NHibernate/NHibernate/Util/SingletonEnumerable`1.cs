// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.SingletonEnumerable`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public sealed class SingletonEnumerable<T> : IEnumerable<T>, IEnumerable
  {
    private readonly T value;

    public SingletonEnumerable(T value) => this.value = value;

    public IEnumerator<T> GetEnumerator()
    {
      return (IEnumerator<T>) new SingletonEnumerable<T>.SingletonEnumerator(this.value);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private class SingletonEnumerator : IEnumerator<T>, IDisposable, IEnumerator
    {
      private readonly T current;
      private bool hasNext = true;

      public SingletonEnumerator(T value) => this.current = value;

      public T Current => this.current;

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        bool hasNext = this.hasNext;
        this.hasNext = false;
        return hasNext;
      }

      public void Reset()
      {
      }

      object IEnumerator.Current => (object) this.Current;
    }
  }
}
