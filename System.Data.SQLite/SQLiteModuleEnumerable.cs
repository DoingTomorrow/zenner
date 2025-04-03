// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModuleEnumerable
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;
using System.Globalization;

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteModuleEnumerable : SQLiteModuleCommon
  {
    private IEnumerable enumerable;
    private bool objectIdentity;
    private bool disposed;

    public SQLiteModuleEnumerable(string name, IEnumerable enumerable)
      : this(name, enumerable, false)
    {
    }

    public SQLiteModuleEnumerable(string name, IEnumerable enumerable, bool objectIdentity)
      : base(name)
    {
      this.enumerable = enumerable != null ? enumerable : throw new ArgumentNullException(nameof (enumerable));
      this.objectIdentity = objectIdentity;
    }

    protected virtual SQLiteErrorCode CursorEndOfEnumeratorError(SQLiteVirtualTableCursor cursor)
    {
      this.SetCursorError(cursor, "already hit end of enumerator");
      return SQLiteErrorCode.Error;
    }

    public override SQLiteErrorCode Create(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error)
    {
      this.CheckDisposed();
      if (this.DeclareTable(connection, this.GetSqlForDeclareTable(), ref error) != SQLiteErrorCode.Ok)
        return SQLiteErrorCode.Error;
      table = new SQLiteVirtualTable(arguments);
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Connect(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error)
    {
      this.CheckDisposed();
      if (this.DeclareTable(connection, this.GetSqlForDeclareTable(), ref error) != SQLiteErrorCode.Ok)
        return SQLiteErrorCode.Error;
      table = new SQLiteVirtualTable(arguments);
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index)
    {
      this.CheckDisposed();
      if (table.BestIndex(index))
        return SQLiteErrorCode.Ok;
      this.SetTableError(table, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "failed to select best index for virtual table \"{0}\"", (object) table.TableName));
      return SQLiteErrorCode.Error;
    }

    public override SQLiteErrorCode Disconnect(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      table.Dispose();
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Destroy(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      table.Dispose();
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Open(
      SQLiteVirtualTable table,
      ref SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      cursor = (SQLiteVirtualTableCursor) new SQLiteVirtualTableCursorEnumerator(table, this.enumerable.GetEnumerator());
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      cursorEnumerator.Close();
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Filter(
      SQLiteVirtualTableCursor cursor,
      int indexNumber,
      string indexString,
      SQLiteValue[] values)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      cursorEnumerator.Filter(indexNumber, indexString, values);
      cursorEnumerator.Reset();
      cursorEnumerator.MoveNext();
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      cursorEnumerator.MoveNext();
      return SQLiteErrorCode.Ok;
    }

    public override bool Eof(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      return !(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator) ? this.ResultCodeToEofResult(this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator))) : cursorEnumerator.EndOfEnumerator;
    }

    public override SQLiteErrorCode Column(
      SQLiteVirtualTableCursor cursor,
      SQLiteContext context,
      int index)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      object current = cursorEnumerator.Current;
      if (current != null)
        context.SetString(this.GetStringFromObject(cursor, current));
      else
        context.SetNull();
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      object current = cursorEnumerator.Current;
      rowId = this.GetRowIdFromObject(cursor, current);
      return SQLiteErrorCode.Ok;
    }

    public override SQLiteErrorCode Update(
      SQLiteVirtualTable table,
      SQLiteValue[] values,
      ref long rowId)
    {
      this.CheckDisposed();
      this.SetTableError(table, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "virtual table \"{0}\" is read-only", (object) table.TableName));
      return SQLiteErrorCode.Error;
    }

    public override SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName)
    {
      this.CheckDisposed();
      if (table.Rename(newName))
        return SQLiteErrorCode.Ok;
      this.SetTableError(table, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "failed to rename virtual table from \"{0}\" to \"{1}\"", (object) table.TableName, (object) newName));
      return SQLiteErrorCode.Error;
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleEnumerable).Name);
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
