﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.CompositeDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class CompositeDisposable : 
    ICollection<IDisposable>,
    IEnumerable<IDisposable>,
    IEnumerable,
    ICancelable,
    IDisposable
  {
    private readonly object _gate = new object();
    private bool _disposed;
    private List<IDisposable> _disposables;
    private int _count;
    private const int SHRINK_THRESHOLD = 64;

    public CompositeDisposable() => this._disposables = new List<IDisposable>();

    public CompositeDisposable(int capacity)
    {
      this._disposables = capacity >= 0 ? new List<IDisposable>(capacity) : throw new ArgumentOutOfRangeException(nameof (capacity));
    }

    public CompositeDisposable(params IDisposable[] disposables)
      : this((IEnumerable<IDisposable>) disposables)
    {
    }

    public CompositeDisposable(IEnumerable<IDisposable> disposables)
    {
      this._disposables = disposables != null ? new List<IDisposable>(disposables) : throw new ArgumentNullException(nameof (disposables));
      this._count = !this._disposables.Contains((IDisposable) null) ? this._disposables.Count : throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof (disposables));
    }

    public int Count => this._count;

    public void Add(IDisposable item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      bool flag = false;
      lock (this._gate)
      {
        flag = this._disposed;
        if (!this._disposed)
        {
          this._disposables.Add(item);
          ++this._count;
        }
      }
      if (!flag)
        return;
      item.Dispose();
    }

    public bool Remove(IDisposable item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      bool flag = false;
      lock (this._gate)
      {
        if (!this._disposed)
        {
          int index = this._disposables.IndexOf(item);
          if (index >= 0)
          {
            flag = true;
            this._disposables[index] = (IDisposable) null;
            --this._count;
            if (this._disposables.Capacity > 64)
            {
              if (this._count < this._disposables.Capacity / 2)
              {
                List<IDisposable> disposables = this._disposables;
                this._disposables = new List<IDisposable>(this._disposables.Capacity / 2);
                foreach (IDisposable disposable in disposables)
                {
                  if (disposable != null)
                    this._disposables.Add(disposable);
                }
              }
            }
          }
        }
      }
      if (flag)
        item.Dispose();
      return flag;
    }

    public void Dispose()
    {
      IDisposable[] disposableArray = (IDisposable[]) null;
      lock (this._gate)
      {
        if (!this._disposed)
        {
          this._disposed = true;
          disposableArray = this._disposables.ToArray();
          this._disposables.Clear();
          this._count = 0;
        }
      }
      if (disposableArray == null)
        return;
      foreach (IDisposable disposable in disposableArray)
        disposable?.Dispose();
    }

    public void Clear()
    {
      IDisposable[] disposableArray = (IDisposable[]) null;
      lock (this._gate)
      {
        disposableArray = this._disposables.ToArray();
        this._disposables.Clear();
        this._count = 0;
      }
      foreach (IDisposable disposable in disposableArray)
        disposable?.Dispose();
    }

    public bool Contains(IDisposable item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      lock (this._gate)
        return this._disposables.Contains(item);
    }

    public void CopyTo(IDisposable[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0 || arrayIndex >= array.Length)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex));
      lock (this._gate)
        Array.Copy((Array) this._disposables.Where<IDisposable>((Func<IDisposable, bool>) (d => d != null)).ToArray<IDisposable>(), 0, (Array) array, arrayIndex, array.Length - arrayIndex);
    }

    public bool IsReadOnly => false;

    public IEnumerator<IDisposable> GetEnumerator()
    {
      IEnumerable<IDisposable> disposables = (IEnumerable<IDisposable>) null;
      lock (this._gate)
        disposables = (IEnumerable<IDisposable>) this._disposables.Where<IDisposable>((Func<IDisposable, bool>) (d => d != null)).ToList<IDisposable>();
      return disposables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool IsDisposed => this._disposed;
  }
}
