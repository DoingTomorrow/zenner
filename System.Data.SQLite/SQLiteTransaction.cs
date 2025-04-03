// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTransaction
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Data.Common;
using System.Threading;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteTransaction : DbTransaction
  {
    internal SQLiteConnection _cnn;
    internal int _version;
    private IsolationLevel _level;
    private bool disposed;

    internal SQLiteTransaction(SQLiteConnection connection, bool deferredLock)
    {
      this._cnn = connection;
      this._version = this._cnn._version;
      this._level = deferredLock ? IsolationLevel.ReadCommitted : IsolationLevel.Serializable;
      if (this._cnn._transactionLevel++ != 0)
        return;
      try
      {
        using (SQLiteCommand command = this._cnn.CreateCommand())
        {
          if (!deferredLock)
            command.CommandText = "BEGIN IMMEDIATE";
          else
            command.CommandText = "BEGIN";
          command.ExecuteNonQuery();
        }
      }
      catch (SQLiteException ex)
      {
        --this._cnn._transactionLevel;
        this._cnn = (SQLiteConnection) null;
        throw;
      }
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteTransaction).Name);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed || !disposing || !this.IsValid(false))
          return;
        this.IssueRollback(false);
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    public override void Commit()
    {
      this.CheckDisposed();
      this.IsValid(true);
      if (this._cnn._transactionLevel - 1 == 0)
      {
        using (SQLiteCommand command = this._cnn.CreateCommand())
        {
          command.CommandText = "COMMIT";
          command.ExecuteNonQuery();
        }
      }
      --this._cnn._transactionLevel;
      this._cnn = (SQLiteConnection) null;
    }

    public SQLiteConnection Connection
    {
      get
      {
        this.CheckDisposed();
        return this._cnn;
      }
    }

    protected override DbConnection DbConnection => (DbConnection) this.Connection;

    public override IsolationLevel IsolationLevel
    {
      get
      {
        this.CheckDisposed();
        return this._level;
      }
    }

    public override void Rollback()
    {
      this.CheckDisposed();
      this.IsValid(true);
      this.IssueRollback(true);
    }

    internal void IssueRollback(bool throwError)
    {
      SQLiteConnection sqLiteConnection = Interlocked.Exchange<SQLiteConnection>(ref this._cnn, (SQLiteConnection) null);
      if (sqLiteConnection == null)
        return;
      try
      {
        using (SQLiteCommand command = sqLiteConnection.CreateCommand())
        {
          command.CommandText = "ROLLBACK";
          command.ExecuteNonQuery();
        }
      }
      catch
      {
        if (throwError)
          throw;
      }
      sqLiteConnection._transactionLevel = 0;
    }

    internal bool IsValid(bool throwError)
    {
      if (this._cnn == null)
      {
        if (throwError)
          throw new ArgumentNullException("No connection associated with this transaction");
        return false;
      }
      if (this._cnn._version != this._version)
      {
        if (throwError)
          throw new SQLiteException("The connection was closed and re-opened, changes were already rolled back");
        return false;
      }
      if (this._cnn.State != ConnectionState.Open)
      {
        if (throwError)
          throw new SQLiteException("Connection was closed");
        return false;
      }
      if (this._cnn._transactionLevel != 0 && !this._cnn._sql.AutoCommit)
        return true;
      this._cnn._transactionLevel = 0;
      if (throwError)
        throw new SQLiteException("No transaction is active on this connection");
      return false;
    }
  }
}
