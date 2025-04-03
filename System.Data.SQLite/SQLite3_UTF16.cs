// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLite3_UTF16
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class SQLite3_UTF16 : SQLite3
  {
    private bool disposed;

    internal SQLite3_UTF16(
      SQLiteDateFormats fmt,
      DateTimeKind kind,
      string fmtString,
      IntPtr db,
      string fileName,
      bool ownHandle)
      : base(fmt, kind, fmtString, db, fileName, ownHandle)
    {
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLite3_UTF16).Name);
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

    public override string ToString(IntPtr b, int nbytelen)
    {
      this.CheckDisposed();
      return SQLite3_UTF16.UTF16ToString(b, nbytelen);
    }

    public static string UTF16ToString(IntPtr b, int nbytelen)
    {
      if (nbytelen == 0 || b == IntPtr.Zero)
        return string.Empty;
      return nbytelen == -1 ? Marshal.PtrToStringUni(b) : Marshal.PtrToStringUni(b, nbytelen / 2);
    }

    internal override void Open(
      string strFilename,
      string vfsName,
      SQLiteConnectionFlags connectionFlags,
      SQLiteOpenFlagsEnum openFlags,
      int maxPoolSize,
      bool usePool)
    {
      if (this._sql != null)
        this.Close(true);
      if (this._sql != null)
        throw new SQLiteException("connection handle is still active");
      this._usePool = usePool;
      this._fileName = strFilename;
      this._flags = connectionFlags;
      if (usePool)
      {
        this._sql = SQLiteConnectionPool.Remove(strFilename, maxPoolSize, out this._poolVersion);
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.OpenedFromPool, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, strFilename, (object) new object[8]
        {
          (object) typeof (SQLite3_UTF16),
          (object) strFilename,
          (object) vfsName,
          (object) connectionFlags,
          (object) openFlags,
          (object) maxPoolSize,
          (object) usePool,
          (object) this._poolVersion
        }));
      }
      if (this._sql == null)
      {
        try
        {
        }
        finally
        {
          IntPtr zero = IntPtr.Zero;
          int extFuncs = (connectionFlags & SQLiteConnectionFlags.NoExtensionFunctions) != SQLiteConnectionFlags.NoExtensionFunctions ? 1 : 0;
          SQLiteErrorCode errorCode;
          if (vfsName != null || extFuncs != 0)
          {
            errorCode = UnsafeNativeMethods.sqlite3_open16_interop(SQLiteConvert.ToUTF8(strFilename), SQLiteConvert.ToUTF8(vfsName), openFlags, extFuncs, ref zero);
          }
          else
          {
            if ((openFlags & SQLiteOpenFlagsEnum.Create) != SQLiteOpenFlagsEnum.Create && !File.Exists(strFilename))
              throw new SQLiteException(SQLiteErrorCode.CantOpen, strFilename);
            if (vfsName != null)
              throw new SQLiteException(SQLiteErrorCode.CantOpen, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "cannot open using UTF-16 and VFS \"{0}\": need interop assembly", (object) vfsName));
            errorCode = UnsafeNativeMethods.sqlite3_open16(strFilename, ref zero);
          }
          if (errorCode != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode, (string) null);
          this._sql = new SQLiteConnectionHandle(zero, true);
        }
        lock (this._sql)
          ;
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) this._sql, strFilename, (object) new object[7]
        {
          (object) typeof (SQLite3_UTF16),
          (object) strFilename,
          (object) vfsName,
          (object) connectionFlags,
          (object) openFlags,
          (object) maxPoolSize,
          (object) usePool
        }));
      }
      if ((connectionFlags & SQLiteConnectionFlags.NoBindFunctions) != SQLiteConnectionFlags.NoBindFunctions)
      {
        if (this._functions == null)
          this._functions = new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
        foreach (KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction> bindFunction in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction>>) SQLiteFunction.BindFunctions((SQLiteBase) this, connectionFlags))
          this._functions[bindFunction.Key] = bindFunction.Value;
      }
      this.SetTimeout(0);
      GC.KeepAlive((object) this._sql);
    }

    internal override void Bind_DateTime(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      DateTime dt)
    {
      switch (this._datetimeFormat)
      {
        case SQLiteDateFormats.Ticks:
        case SQLiteDateFormats.JulianDay:
        case SQLiteDateFormats.UnixEpoch:
          base.Bind_DateTime(stmt, flags, index, dt);
          break;
        default:
          if ((flags & SQLiteConnectionFlags.LogBind) == SQLiteConnectionFlags.LogBind)
            SQLite3.LogBind(stmt?._sqlite_stmt, index, dt);
          this.Bind_Text(stmt, flags, index, this.ToString(dt));
          break;
      }
    }

    internal override void Bind_Text(
      SQLiteStatement stmt,
      SQLiteConnectionFlags flags,
      int index,
      string value)
    {
      SQLiteStatementHandle sqliteStmt = stmt._sqlite_stmt;
      if ((flags & SQLiteConnectionFlags.LogBind) == SQLiteConnectionFlags.LogBind)
        SQLite3.LogBind(sqliteStmt, index, value);
      SQLiteErrorCode errorCode = UnsafeNativeMethods.sqlite3_bind_text16((IntPtr) sqliteStmt, index, value, value.Length * 2, (IntPtr) -1);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this.GetLastError());
    }

    internal override DateTime GetDateTime(SQLiteStatement stmt, int index)
    {
      if (this._datetimeFormat == SQLiteDateFormats.Ticks)
        return SQLiteConvert.TicksToDateTime(this.GetInt64(stmt, index), this._datetimeKind);
      if (this._datetimeFormat == SQLiteDateFormats.JulianDay)
        return SQLiteConvert.ToDateTime(this.GetDouble(stmt, index), this._datetimeKind);
      return this._datetimeFormat == SQLiteDateFormats.UnixEpoch ? SQLiteConvert.UnixEpochToDateTime(this.GetInt64(stmt, index), this._datetimeKind) : this.ToDateTime(this.GetText(stmt, index));
    }

    internal override string ColumnName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      IntPtr b = UnsafeNativeMethods.sqlite3_column_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len);
      return !(b == IntPtr.Zero) ? SQLite3_UTF16.UTF16ToString(b, len) : throw new SQLiteException(SQLiteErrorCode.NoMem, this.GetLastError());
    }

    internal override string GetText(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_text16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnOriginalName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_origin_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnDatabaseName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_database_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string ColumnTableName(SQLiteStatement stmt, int index)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_column_table_name16_interop((IntPtr) stmt._sqlite_stmt, index, ref len), len);
    }

    internal override string GetParamValueText(IntPtr ptr)
    {
      int len = 0;
      return SQLite3_UTF16.UTF16ToString(UnsafeNativeMethods.sqlite3_value_text16_interop(ptr, ref len), len);
    }

    internal override void ReturnError(IntPtr context, string value)
    {
      UnsafeNativeMethods.sqlite3_result_error16(context, value, value.Length * 2);
    }

    internal override void ReturnText(IntPtr context, string value)
    {
      UnsafeNativeMethods.sqlite3_result_text16(context, value, value.Length * 2, (IntPtr) -1);
    }
  }
}
