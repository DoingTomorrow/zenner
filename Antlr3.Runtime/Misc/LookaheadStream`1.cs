// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Misc.LookaheadStream`1
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;

#nullable disable
namespace Antlr.Runtime.Misc
{
  public abstract class LookaheadStream<T> : FastQueue<T> where T : class
  {
    private int _currentElementIndex;
    private T _previousElement;
    private T _eof = default (T);
    private int _lastMarker;
    private int _markDepth;

    public T EndOfFile
    {
      get => this._eof;
      protected set => this._eof = value;
    }

    public T PreviousElement => this._previousElement;

    public override void Clear()
    {
      base.Clear();
      this._currentElementIndex = 0;
      this._p = 0;
      this._previousElement = default (T);
    }

    public abstract T NextElement();

    public abstract bool IsEndOfFile(T o);

    public override T Dequeue()
    {
      T obj = this[0];
      ++this._p;
      if (this._p == this._data.Count && this._markDepth == 0)
        this.Clear();
      return obj;
    }

    public virtual void Consume()
    {
      this.SyncAhead(1);
      this._previousElement = this.Dequeue();
      ++this._currentElementIndex;
    }

    protected virtual void SyncAhead(int need)
    {
      int n = this._p + need - 1 - this._data.Count + 1;
      if (n <= 0)
        return;
      this.Fill(n);
    }

    public virtual void Fill(int n)
    {
      for (int index = 0; index < n; ++index)
      {
        T o = this.NextElement();
        if (this.IsEndOfFile(o))
          this._eof = o;
        this._data.Add(o);
      }
    }

    public override int Count => throw new NotSupportedException("streams are of unknown size");

    public virtual T LT(int k)
    {
      if (k == 0)
        return default (T);
      if (k < 0)
        return this.LB(-k);
      this.SyncAhead(k);
      return this._p + k - 1 > this._data.Count ? this._eof : this[k - 1];
    }

    public virtual int Index => this._currentElementIndex;

    public virtual int Mark()
    {
      ++this._markDepth;
      this._lastMarker = this._p;
      return this._lastMarker;
    }

    public virtual void Release(int marker)
    {
      if (this._markDepth == 0)
        throw new InvalidOperationException();
      --this._markDepth;
    }

    public virtual void Rewind(int marker)
    {
      this.Seek(marker);
      this.Release(marker);
    }

    public virtual void Rewind() => this.Rewind(this._lastMarker);

    public virtual void Seek(int index) => this._p = index;

    protected virtual T LB(int k)
    {
      if (k == 1)
        return this._previousElement;
      throw new ArgumentException("can't look backwards more than one token in this stream");
    }
  }
}
