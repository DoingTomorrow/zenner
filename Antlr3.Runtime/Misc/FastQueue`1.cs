// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Misc.FastQueue`1
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Misc
{
  public class FastQueue<T>
  {
    internal List<T> _data = new List<T>();
    internal int _p;

    public virtual int Count => this._data.Count - this._p;

    public virtual int Range { get; protected set; }

    public virtual T this[int i]
    {
      get
      {
        int index = this._p + i;
        if (index >= this._data.Count)
          throw new ArgumentException(string.Format("queue index {0} > last index {1}", (object) index, (object) (this._data.Count - 1)));
        if (index < 0)
          throw new ArgumentException(string.Format("queue index {0} < 0", (object) index));
        if (index > this.Range)
          this.Range = index;
        return this._data[index];
      }
    }

    public virtual T Dequeue()
    {
      if (this.Count == 0)
        throw new InvalidOperationException();
      T obj = this[0];
      ++this._p;
      if (this._p == this._data.Count)
        this.Clear();
      return obj;
    }

    public virtual void Enqueue(T o) => this._data.Add(o);

    public virtual T Peek() => this[0];

    public virtual void Clear()
    {
      this._p = 0;
      this._data.Clear();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      int count = this.Count;
      for (int i = 0; i < count; ++i)
      {
        stringBuilder.Append((object) this[i]);
        if (i + 1 < count)
          stringBuilder.Append(" ");
      }
      return stringBuilder.ToString();
    }
  }
}
