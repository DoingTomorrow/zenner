// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.JoinedEnumerable`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public class JoinedEnumerable<T> : IEnumerable<T>, IEnumerable
  {
    private readonly IEnumerable<T>[] enumerables;

    public JoinedEnumerable(IEnumerable<T>[] enumerables) => this.enumerables = enumerables;

    public JoinedEnumerable(List<IEnumerable<T>> enumerables)
      : this(enumerables.ToArray())
    {
    }

    public JoinedEnumerable(IEnumerable<T> first, IEnumerable<T> second)
      : this(new IEnumerable<T>[2]{ first, second })
    {
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return (IEnumerator<T>) new JoinedEnumerable<T>.JoinedEnumerator(this.enumerables);
    }

    public IEnumerator GetEnumerator() => (IEnumerator) ((IEnumerable<T>) this).GetEnumerator();

    private class JoinedEnumerator : IEnumerator<T>, IDisposable, IEnumerator
    {
      private readonly IEnumerator<T>[] enumerators;
      private int currentEnumIdx;
      private bool disposed;

      public JoinedEnumerator(IEnumerable<T>[] enumerables)
      {
        this.enumerators = new IEnumerator<T>[enumerables.Length];
        for (int index = 0; index < enumerables.Length; ++index)
          this.enumerators[index] = enumerables[index].GetEnumerator();
      }

      T IEnumerator<T>.Current => this.enumerators[this.currentEnumIdx].Current;

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      private void Dispose(bool disposing)
      {
        if (this.disposed)
          return;
        if (disposing)
        {
          for (; this.currentEnumIdx < this.enumerators.Length; ++this.currentEnumIdx)
            this.enumerators[this.currentEnumIdx].Dispose();
        }
        GC.SuppressFinalize((object) this);
        this.disposed = true;
      }

      ~JoinedEnumerator() => this.Dispose(false);

      public bool MoveNext()
      {
        for (; this.currentEnumIdx < this.enumerators.Length; ++this.currentEnumIdx)
        {
          if (this.enumerators[this.currentEnumIdx].MoveNext())
            return true;
          this.enumerators[this.currentEnumIdx].Dispose();
        }
        return false;
      }

      public void Reset()
      {
        foreach (IEnumerator enumerator in this.enumerators)
          enumerator.Reset();
        this.currentEnumIdx = 0;
      }

      public object Current => (object) ((IEnumerator<T>) this).Current;
    }
  }
}
