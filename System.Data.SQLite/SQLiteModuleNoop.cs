// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModuleNoop
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteModuleNoop : SQLiteModule
  {
    private Dictionary<string, SQLiteErrorCode> resultCodes;
    private bool disposed;

    public SQLiteModuleNoop(string name)
      : base(name)
    {
      this.resultCodes = new Dictionary<string, SQLiteErrorCode>();
    }

    protected virtual SQLiteErrorCode GetDefaultResultCode() => SQLiteErrorCode.Ok;

    protected virtual bool ResultCodeToEofResult(SQLiteErrorCode resultCode)
    {
      return resultCode != SQLiteErrorCode.Ok;
    }

    protected virtual bool ResultCodeToFindFunctionResult(SQLiteErrorCode resultCode)
    {
      return resultCode == SQLiteErrorCode.Ok;
    }

    protected virtual SQLiteErrorCode GetMethodResultCode(string methodName)
    {
      SQLiteErrorCode sqLiteErrorCode;
      return methodName == null || this.resultCodes == null || this.resultCodes == null || !this.resultCodes.TryGetValue(methodName, out sqLiteErrorCode) ? this.GetDefaultResultCode() : sqLiteErrorCode;
    }

    protected virtual bool SetMethodResultCode(string methodName, SQLiteErrorCode resultCode)
    {
      if (methodName == null || this.resultCodes == null)
        return false;
      this.resultCodes[methodName] = resultCode;
      return true;
    }

    public override SQLiteErrorCode Create(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Create));
    }

    public override SQLiteErrorCode Connect(
      SQLiteConnection connection,
      IntPtr pClientData,
      string[] arguments,
      ref SQLiteVirtualTable table,
      ref string error)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Connect));
    }

    public override SQLiteErrorCode BestIndex(SQLiteVirtualTable table, SQLiteIndex index)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (BestIndex));
    }

    public override SQLiteErrorCode Disconnect(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Disconnect));
    }

    public override SQLiteErrorCode Destroy(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Destroy));
    }

    public override SQLiteErrorCode Open(
      SQLiteVirtualTable table,
      ref SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Open));
    }

    public override SQLiteErrorCode Close(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Close));
    }

    public override SQLiteErrorCode Filter(
      SQLiteVirtualTableCursor cursor,
      int indexNumber,
      string indexString,
      SQLiteValue[] values)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Filter));
    }

    public override SQLiteErrorCode Next(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Next));
    }

    public override bool Eof(SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      return this.ResultCodeToEofResult(this.GetMethodResultCode(nameof (Eof)));
    }

    public override SQLiteErrorCode Column(
      SQLiteVirtualTableCursor cursor,
      SQLiteContext context,
      int index)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Column));
    }

    public override SQLiteErrorCode RowId(SQLiteVirtualTableCursor cursor, ref long rowId)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (RowId));
    }

    public override SQLiteErrorCode Update(
      SQLiteVirtualTable table,
      SQLiteValue[] values,
      ref long rowId)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Update));
    }

    public override SQLiteErrorCode Begin(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Begin));
    }

    public override SQLiteErrorCode Sync(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Sync));
    }

    public override SQLiteErrorCode Commit(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Commit));
    }

    public override SQLiteErrorCode Rollback(SQLiteVirtualTable table)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Rollback));
    }

    public override bool FindFunction(
      SQLiteVirtualTable table,
      int argumentCount,
      string name,
      ref SQLiteFunction function,
      ref IntPtr pClientData)
    {
      this.CheckDisposed();
      return this.ResultCodeToFindFunctionResult(this.GetMethodResultCode(nameof (FindFunction)));
    }

    public override SQLiteErrorCode Rename(SQLiteVirtualTable table, string newName)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Rename));
    }

    public override SQLiteErrorCode Savepoint(SQLiteVirtualTable table, int savepoint)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Savepoint));
    }

    public override SQLiteErrorCode Release(SQLiteVirtualTable table, int savepoint)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (Release));
    }

    public override SQLiteErrorCode RollbackTo(SQLiteVirtualTable table, int savepoint)
    {
      this.CheckDisposed();
      return this.GetMethodResultCode(nameof (RollbackTo));
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleNoop).Name);
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
