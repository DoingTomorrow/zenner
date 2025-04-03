// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Generic.SQLiteModuleEnumerable`1
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace System.Data.SQLite.Generic
{
  public class SQLiteModuleEnumerable<T> : SQLiteModuleEnumerable
  {
    private IEnumerable<T> enumerable;
    private bool disposed;

    public SQLiteModuleEnumerable(string name, IEnumerable<T> enumerable)
      : base(name, (IEnumerable) enumerable)
    {
      this.enumerable = enumerable;
    }

    public override SQLiteErrorCode Open(
      SQLiteVirtualTable table,
      ref SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      cursor = (SQLiteVirtualTableCursor) new SQLiteVirtualTableCursorEnumerator<T>(table, this.enumerable.GetEnumerator());
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Column(
      SQLiteVirtualTableCursor cursor,
      SQLiteContext context,
      int index)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator<T> cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      T current = ((IEnumerator<T>) cursorEnumerator).Current;
      if ((object) current != null)
        context.SetString(this.GetStringFromObject(cursor, (object) current));
      else
        context.SetNull();
      return SQLiteErrorCode.Ok;
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleEnumerable<T>).Name);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        int num = this.disposed ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
