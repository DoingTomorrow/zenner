// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBackup
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class SQLiteBackup : IDisposable
  {
    internal SQLiteBase _sql;
    internal SQLiteBackupHandle _sqlite_backup;
    internal IntPtr _destDb;
    internal byte[] _zDestName;
    internal IntPtr _sourceDb;
    internal byte[] _zSourceName;
    internal SQLiteErrorCode _stepResult;
    private bool disposed;

    internal SQLiteBackup(
      SQLiteBase sqlbase,
      SQLiteBackupHandle backup,
      IntPtr destDb,
      byte[] zDestName,
      IntPtr sourceDb,
      byte[] zSourceName)
    {
      this._sql = sqlbase;
      this._sqlite_backup = backup;
      this._destDb = destDb;
      this._zDestName = zDestName;
      this._sourceDb = sourceDb;
      this._zSourceName = zSourceName;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteBackup).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._sqlite_backup != null)
        {
          this._sqlite_backup.Dispose();
          this._sqlite_backup = (SQLiteBackupHandle) null;
        }
        this._zSourceName = (byte[]) null;
        this._sourceDb = IntPtr.Zero;
        this._zDestName = (byte[]) null;
        this._destDb = IntPtr.Zero;
        this._sql = (SQLiteBase) null;
      }
      this.disposed = true;
    }

    ~SQLiteBackup() => this.Dispose(false);
  }
}
