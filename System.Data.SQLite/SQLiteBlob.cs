// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBlob
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteBlob : IDisposable
  {
    internal SQLiteBase _sql;
    internal SQLiteBlobHandle _sqlite_blob;
    private bool disposed;

    private SQLiteBlob(SQLiteBase sqlbase, SQLiteBlobHandle blob)
    {
      this._sql = sqlbase;
      this._sqlite_blob = blob;
    }

    public static SQLiteBlob Create(SQLiteDataReader dataReader, int i, bool readOnly)
    {
      SQLiteConnection connection = SQLiteDataReader.GetConnection(dataReader);
      if (connection == null)
        throw new InvalidOperationException("Connection not available");
      if (!(connection._sql is SQLite3 sql1))
        throw new InvalidOperationException("Connection has no wrapper");
      SQLiteConnectionHandle sql2 = sql1._sql;
      if (sql2 == null)
        throw new InvalidOperationException("Connection has an invalid handle.");
      long? rowId = dataReader.GetRowId(i);
      if (!rowId.HasValue)
        throw new InvalidOperationException("No RowId is available");
      SQLiteBlobHandle blob = (SQLiteBlobHandle) null;
      try
      {
      }
      finally
      {
        IntPtr zero = IntPtr.Zero;
        SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_open((IntPtr) sql1._sql, SQLiteConvert.ToUTF8(dataReader.GetDatabaseName(i)), SQLiteConvert.ToUTF8(dataReader.GetTableName(i)), SQLiteConvert.ToUTF8(dataReader.GetName(i)), rowId.Value, readOnly ? 0 : 1, ref zero);
        if (errorCode != SQLiteErrorCode.Ok)
          throw new SQLiteException(errorCode, (string) null);
        blob = new SQLiteBlobHandle(sql2, zero);
      }
      SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) dataReader, (CriticalHandle) blob, (string) null, (object) new object[4]
      {
        (object) typeof (SQLiteBlob),
        (object) dataReader,
        (object) i,
        (object) readOnly
      }));
      return new SQLiteBlob((SQLiteBase) sql1, blob);
    }

    private void CheckOpen()
    {
      if ((IntPtr) this._sqlite_blob == IntPtr.Zero)
        throw new InvalidOperationException("Blob is not open");
    }

    private void VerifyParameters(byte[] buffer, int count, int offset)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0)
        throw new ArgumentException("Negative offset not allowed.");
      if (count < 0)
        throw new ArgumentException("Negative count not allowed.");
      if (count > buffer.Length)
        throw new ArgumentException("Buffer is too small.");
    }

    public void Reopen(long rowId)
    {
      this.CheckDisposed();
      this.CheckOpen();
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_reopen((IntPtr) this._sqlite_blob, rowId);
      if (errorCode != SQLiteErrorCode.Ok)
      {
        this.Dispose();
        throw new SQLiteException(errorCode, (string) null);
      }
    }

    public int GetCount()
    {
      this.CheckDisposed();
      this.CheckOpen();
      return UnsafeNativeMethods.sqlite3_blob_bytes((IntPtr) this._sqlite_blob);
    }

    public void Read(byte[] buffer, int count, int offset)
    {
      this.CheckDisposed();
      this.CheckOpen();
      this.VerifyParameters(buffer, count, offset);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_read((IntPtr) this._sqlite_blob, buffer, count, offset);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, (string) null);
    }

    public void Write(byte[] buffer, int count, int offset)
    {
      this.CheckDisposed();
      this.CheckOpen();
      this.VerifyParameters(buffer, count, offset);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_blob_write((IntPtr) this._sqlite_blob, buffer, count, offset);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, (string) null);
    }

    public void Close() => this.Dispose();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteBlob).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._sqlite_blob != null)
        {
          this._sqlite_blob.Dispose();
          this._sqlite_blob = (SQLiteBlobHandle) null;
        }
        this._sql = (SQLiteBase) null;
      }
      this.disposed = true;
    }

    ~SQLiteBlob() => this.Dispose(false);
  }
}
