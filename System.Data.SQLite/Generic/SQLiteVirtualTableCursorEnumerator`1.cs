// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Generic.SQLiteVirtualTableCursorEnumerator`1
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace System.Data.SQLite.Generic
{
  public class SQLiteVirtualTableCursorEnumerator<T> : 
    SQLiteVirtualTableCursorEnumerator,
    IEnumerator<T>,
    IDisposable,
    IEnumerator
  {
    private IEnumerator<T> enumerator;
    private bool disposed;

    public SQLiteVirtualTableCursorEnumerator(SQLiteVirtualTable table, IEnumerator<T> enumerator)
      : base(table, (IEnumerator) enumerator)
    {
      this.enumerator = enumerator;
    }

    T IEnumerator<T>.Current
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.enumerator == null ? default (T) : this.enumerator.Current;
      }
    }

    public override void Close()
    {
      if (this.enumerator != null)
        this.enumerator = (IEnumerator<T>) null;
      base.Close();
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteVirtualTableCursorEnumerator<T>).Name);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed)
          return;
        this.Close();
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
