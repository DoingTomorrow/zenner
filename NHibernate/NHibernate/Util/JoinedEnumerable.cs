// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.JoinedEnumerable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Util
{
  public class JoinedEnumerable : IEnumerable, IEnumerator, IDisposable
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (JoinedEnumerable));
    private readonly IEnumerator[] _enumerators;
    private int _current;
    private bool _isAlreadyDisposed;

    public JoinedEnumerable(IEnumerable[] enumerables)
    {
      this._enumerators = new IEnumerator[enumerables.Length];
      for (int index = 0; index < enumerables.Length; ++index)
        this._enumerators[index] = enumerables[index].GetEnumerator();
      this._current = 0;
    }

    public JoinedEnumerable(List<IEnumerable> enumerables)
      : this(enumerables.ToArray())
    {
    }

    public JoinedEnumerable(IEnumerable first, IEnumerable second)
      : this(new IEnumerable[2]{ first, second })
    {
    }

    public bool MoveNext()
    {
      for (; this._current < this._enumerators.Length; ++this._current)
      {
        if (this._enumerators[this._current].MoveNext())
          return true;
        if (this._enumerators[this._current] is IDisposable enumerator)
          enumerator.Dispose();
      }
      return false;
    }

    public void Reset()
    {
      for (int index = 0; index < this._enumerators.Length; ++index)
        this._enumerators[index].Reset();
      this._current = 0;
    }

    public object Current => this._enumerators[this._current].Current;

    public IEnumerator GetEnumerator()
    {
      this.Reset();
      return (IEnumerator) this;
    }

    ~JoinedEnumerable() => this.Dispose(false);

    public void Dispose()
    {
      JoinedEnumerable.log.Debug((object) "running JoinedEnumerable.Dispose()");
      this.Dispose(true);
    }

    protected virtual void Dispose(bool isDisposing)
    {
      if (this._isAlreadyDisposed)
        return;
      if (isDisposing)
      {
        for (; this._current < this._enumerators.Length; ++this._current)
        {
          if (this._enumerators[this._current] is IDisposable enumerator)
            enumerator.Dispose();
        }
      }
      this._isAlreadyDisposed = true;
      GC.SuppressFinalize((object) this);
    }
  }
}
