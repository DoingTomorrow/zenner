// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.DelayedEnumerator`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  internal class DelayedEnumerator<T> : IEnumerable<T>, IEnumerable, IDelayedValue
  {
    private readonly DelayedEnumerator<T>.GetResult result;

    public Delegate ExecuteOnEval { get; set; }

    public DelayedEnumerator(DelayedEnumerator<T>.GetResult result) => this.result = result;

    public IEnumerable<T> Enumerable
    {
      get
      {
        IEnumerable<T> value = this.result();
        if ((object) this.ExecuteOnEval != null)
          value = (IEnumerable<T>) this.ExecuteOnEval.DynamicInvoke((object) value);
        foreach (T item in value)
          yield return item;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.Enumerable.GetEnumerator();

    public IEnumerator<T> GetEnumerator() => this.Enumerable.GetEnumerator();

    public delegate IEnumerable<T> GetResult();
  }
}
