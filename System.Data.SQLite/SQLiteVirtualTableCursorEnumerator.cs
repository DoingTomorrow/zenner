// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteVirtualTableCursorEnumerator
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteVirtualTableCursorEnumerator : SQLiteVirtualTableCursor, IEnumerator
  {
    private IEnumerator enumerator;
    private bool endOfEnumerator;
    private bool disposed;

    public SQLiteVirtualTableCursorEnumerator(SQLiteVirtualTable table, IEnumerator enumerator)
      : base(table)
    {
      this.enumerator = enumerator;
      this.endOfEnumerator = true;
    }

    public virtual bool MoveNext()
    {
      this.CheckDisposed();
      this.CheckClosed();
      if (this.enumerator == null)
        return false;
      this.endOfEnumerator = !this.enumerator.MoveNext();
      if (!this.endOfEnumerator)
        this.NextRowIndex();
      return !this.endOfEnumerator;
    }

    public virtual object Current
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.enumerator == null ? (object) null : this.enumerator.Current;
      }
    }

    public virtual void Reset()
    {
      this.CheckDisposed();
      this.CheckClosed();
      if (this.enumerator == null)
        return;
      this.enumerator.Reset();
    }

    public virtual bool EndOfEnumerator
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.endOfEnumerator;
      }
    }

    public virtual bool IsOpen
    {
      get
      {
        this.CheckDisposed();
        return this.enumerator != null;
      }
    }

    public virtual void Close()
    {
      if (this.enumerator == null)
        return;
      this.enumerator = (IEnumerator) null;
    }

    public virtual void CheckClosed()
    {
      this.CheckDisposed();
      if (!this.IsOpen)
        throw new InvalidOperationException("virtual table cursor is closed");
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteVirtualTableCursorEnumerator).Name);
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
