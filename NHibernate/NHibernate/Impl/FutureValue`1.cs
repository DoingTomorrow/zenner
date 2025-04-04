// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.FutureValue`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  internal class FutureValue<T> : IFutureValue<T>, IDelayedValue
  {
    private readonly FutureValue<T>.GetResult getResult;

    public FutureValue(FutureValue<T>.GetResult result) => this.getResult = result;

    public T Value
    {
      get
      {
        IEnumerator<T> enumerator = this.getResult().GetEnumerator();
        if (!enumerator.MoveNext())
        {
          T obj = default (T);
          if ((object) this.ExecuteOnEval != null)
            obj = (T) this.ExecuteOnEval.DynamicInvoke((object) obj);
          return obj;
        }
        T obj1 = enumerator.Current;
        if ((object) this.ExecuteOnEval != null)
          obj1 = (T) this.ExecuteOnEval.DynamicInvoke((object) obj1);
        return obj1;
      }
    }

    public Delegate ExecuteOnEval { get; set; }

    public delegate IEnumerable<T> GetResult();
  }
}
