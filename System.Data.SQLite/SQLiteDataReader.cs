// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDataReader
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteDataReader : DbDataReader
  {
    private SQLiteCommand _command;
    private SQLiteConnectionFlags _flags;
    private int _activeStatementIndex;
    private SQLiteStatement _activeStatement;
    private int _readingState;
    private int _rowsAffected;
    private int _fieldCount;
    private int _stepCount;
    private Dictionary<string, int> _fieldIndexes;
    private SQLiteType[] _fieldTypeArray;
    private CommandBehavior _commandBehavior;
    internal bool _disposeCommand;
    internal bool _throwOnDisposed;
    private SQLiteKeyReader _keyInfo;
    internal int _version;
    private string _baseSchemaName;
    private bool disposed;

    internal SQLiteDataReader(SQLiteCommand cmd, CommandBehavior behave)
    {
      this._throwOnDisposed = true;
      this._command = cmd;
      this._version = this._command.Connection._version;
      this._baseSchemaName = this._command.Connection._baseSchemaName;
      this._commandBehavior = behave;
      this._activeStatementIndex = -1;
      this._rowsAffected = -1;
      this.RefreshFlags();
      SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.NewDataReader, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) this._command, (IDataReader) this, (CriticalHandle) null, (string) null, (object) new object[1]
      {
        (object) behave
      }));
      if (this._command == null)
        return;
      this.NextResult();
    }

    private void CheckDisposed()
    {
      if (this.disposed && this._throwOnDisposed)
        throw new ObjectDisposedException(typeof (SQLiteDataReader).Name);
    }

    protected override void Dispose(bool disposing)
    {
      SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.DisposingDataReader, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) this._command, (IDataReader) this, (CriticalHandle) null, (string) null, (object) new object[9]
      {
        (object) disposing,
        (object) this.disposed,
        (object) this._commandBehavior,
        (object) this._readingState,
        (object) this._rowsAffected,
        (object) this._stepCount,
        (object) this._fieldCount,
        (object) this._disposeCommand,
        (object) this._throwOnDisposed
      }));
      try
      {
        if (this.disposed)
          return;
        this._throwOnDisposed = false;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    internal void Cancel() => this._version = 0;

    public override void Close()
    {
      this.CheckDisposed();
      SQLiteConnection.OnChanged(SQLiteDataReader.GetConnection(this), new ConnectionEventArgs(SQLiteConnectionEventType.ClosingDataReader, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) this._command, (IDataReader) this, (CriticalHandle) null, (string) null, (object) new object[7]
      {
        (object) this._commandBehavior,
        (object) this._readingState,
        (object) this._rowsAffected,
        (object) this._stepCount,
        (object) this._fieldCount,
        (object) this._disposeCommand,
        (object) this._throwOnDisposed
      }));
      try
      {
        if (this._command != null)
        {
          try
          {
            try
            {
              if (this._version != 0)
              {
                try
                {
                  do
                    ;
                  while (this.NextResult());
                }
                catch (SQLiteException ex)
                {
                }
              }
              this._command.ResetDataReader();
            }
            finally
            {
              if ((this._commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.Default && this._command.Connection != null)
                this._command.Connection.Close();
            }
          }
          finally
          {
            if (this._disposeCommand)
              this._command.Dispose();
          }
        }
        this._command = (SQLiteCommand) null;
        this._activeStatement = (SQLiteStatement) null;
        this._fieldIndexes = (Dictionary<string, int>) null;
        this._fieldTypeArray = (SQLiteType[]) null;
      }
      finally
      {
        if (this._keyInfo != null)
        {
          this._keyInfo.Dispose();
          this._keyInfo = (SQLiteKeyReader) null;
        }
      }
    }

    private void CheckClosed()
    {
      if (!this._throwOnDisposed)
        return;
      if (this._command == null)
        throw new InvalidOperationException("DataReader has been closed");
      if (this._version == 0)
        throw new SQLiteException("Execution was aborted by the user");
      SQLiteConnection connection = this._command.Connection;
      if (connection._version != this._version || connection.State != ConnectionState.Open)
        throw new InvalidOperationException("Connection was closed, statement was terminated");
    }

    private void CheckValidRow()
    {
      if (this._readingState != 0)
        throw new InvalidOperationException("No current row");
    }

    public override IEnumerator GetEnumerator()
    {
      this.CheckDisposed();
      return (IEnumerator) new DbEnumerator((IDataReader) this, (this._commandBehavior & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection);
    }

    public override int Depth
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return 0;
      }
    }

    public override int FieldCount
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this._keyInfo == null ? this._fieldCount : this._fieldCount + this._keyInfo.Count;
      }
    }

    public void RefreshFlags()
    {
      this.CheckDisposed();
      this._flags = SQLiteCommand.GetFlags(this._command);
    }

    public int StepCount
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this._stepCount;
      }
    }

    private int PrivateVisibleFieldCount => this._fieldCount;

    public override int VisibleFieldCount
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.PrivateVisibleFieldCount;
      }
    }

    private void VerifyForGet()
    {
      this.CheckClosed();
      this.CheckValidRow();
    }

    private TypeAffinity VerifyType(int i, DbType typ)
    {
      if ((this._flags & SQLiteConnectionFlags.NoVerifyTypeAffinity) == SQLiteConnectionFlags.NoVerifyTypeAffinity)
        return TypeAffinity.None;
      TypeAffinity affinity = this.GetSQLiteType(this._flags, i).Affinity;
      switch (affinity)
      {
        case TypeAffinity.Int64:
          if (typ == DbType.Int64 || typ == DbType.Int32 || typ == DbType.Int16 || typ == DbType.Byte || typ == DbType.SByte || typ == DbType.Boolean || typ == DbType.DateTime || typ == DbType.Double || typ == DbType.Single || typ == DbType.Decimal)
            return affinity;
          break;
        case TypeAffinity.Double:
          if (typ == DbType.Double || typ == DbType.Single || typ == DbType.Decimal || typ == DbType.DateTime)
            return affinity;
          break;
        case TypeAffinity.Text:
          if (typ == DbType.String || typ == DbType.Guid || typ == DbType.DateTime || typ == DbType.Decimal)
            return affinity;
          break;
        case TypeAffinity.Blob:
          if (typ == DbType.Guid || typ == DbType.Binary || typ == DbType.String)
            return affinity;
          break;
      }
      throw new InvalidCastException();
    }

    private void InvokeReadValueCallback(
      int index,
      SQLiteReadEventArgs eventArgs,
      out bool complete)
    {
      complete = false;
      SQLiteConnectionFlags flags = this._flags;
      this._flags &= ~SQLiteConnectionFlags.UseConnectionReadValueCallbacks;
      try
      {
        string dataTypeName = this.GetDataTypeName(index);
        if (dataTypeName == null)
          return;
        SQLiteConnection connection = SQLiteDataReader.GetConnection(this);
        SQLiteTypeCallbacks callbacks;
        if (connection == null || !connection.TryGetTypeCallbacks(dataTypeName, out callbacks) || callbacks == null)
          return;
        SQLiteReadValueCallback readValueCallback = callbacks.ReadValueCallback;
        if (readValueCallback == null)
          return;
        object readValueUserData = callbacks.ReadValueUserData;
        readValueCallback((SQLiteConvert) this._activeStatement._sql, this, flags, eventArgs, dataTypeName, index, readValueUserData, out complete);
      }
      finally
      {
        this._flags |= SQLiteConnectionFlags.UseConnectionReadValueCallbacks;
      }
    }

    internal long? GetRowId(int i)
    {
      this.VerifyForGet();
      if (this._keyInfo == null)
        return new long?();
      int rowIdIndex = this._keyInfo.GetRowIdIndex(this.GetDatabaseName(i), this.GetTableName(i));
      return rowIdIndex == -1 ? new long?() : new long?(this.GetInt64(rowIdIndex));
    }

    public SQLiteBlob GetBlob(int i, bool readOnly)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetBlob), (SQLiteReadEventArgs) new SQLiteReadBlobEventArgs(readOnly), liteDataReaderValue), out complete);
        if (complete)
          return liteDataReaderValue.BlobValue;
      }
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetBlob(i - this.PrivateVisibleFieldCount, readOnly) : SQLiteBlob.Create(this, i, readOnly);
    }

    public override bool GetBoolean(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetBoolean), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.BooleanValue.HasValue)
            throw new SQLiteException("missing boolean return value");
          return liteDataReaderValue.BooleanValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetBoolean(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Boolean);
      return Convert.ToBoolean(this.GetValue(i), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public override byte GetByte(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetByte), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          byte? byteValue = liteDataReaderValue.ByteValue;
          if (!(byteValue.HasValue ? new int?((int) byteValue.GetValueOrDefault()) : new int?()).HasValue)
            throw new SQLiteException("missing byte return value");
          return liteDataReaderValue.ByteValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetByte(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Byte);
      return Convert.ToByte(this._activeStatement._sql.GetInt32(this._activeStatement, i));
    }

    public override long GetBytes(
      int i,
      long fieldOffset,
      byte[] buffer,
      int bufferoffset,
      int length)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteReadArrayEventArgs extraEventArgs = new SQLiteReadArrayEventArgs(fieldOffset, buffer, bufferoffset, length);
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetBytes), (SQLiteReadEventArgs) extraEventArgs, liteDataReaderValue), out complete);
        if (complete)
        {
          byte[] bytesValue = liteDataReaderValue.BytesValue;
          if (bytesValue == null)
            return -1;
          Array.Copy((Array) bytesValue, extraEventArgs.DataOffset, (Array) extraEventArgs.ByteBuffer, (long) extraEventArgs.BufferOffset, (long) extraEventArgs.Length);
          return (long) extraEventArgs.Length;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetBytes(i - this.PrivateVisibleFieldCount, fieldOffset, buffer, bufferoffset, length);
      int num = (int) this.VerifyType(i, DbType.Binary);
      return this._activeStatement._sql.GetBytes(this._activeStatement, i, (int) fieldOffset, buffer, bufferoffset, length);
    }

    public override char GetChar(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetChar), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          char? charValue = liteDataReaderValue.CharValue;
          if (!(charValue.HasValue ? new int?((int) charValue.GetValueOrDefault()) : new int?()).HasValue)
            throw new SQLiteException("missing character return value");
          return liteDataReaderValue.CharValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetChar(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.SByte);
      return Convert.ToChar(this._activeStatement._sql.GetInt32(this._activeStatement, i));
    }

    public override long GetChars(
      int i,
      long fieldoffset,
      char[] buffer,
      int bufferoffset,
      int length)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteReadArrayEventArgs extraEventArgs = new SQLiteReadArrayEventArgs(fieldoffset, buffer, bufferoffset, length);
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetChars), (SQLiteReadEventArgs) extraEventArgs, liteDataReaderValue), out complete);
        if (complete)
        {
          char[] charsValue = liteDataReaderValue.CharsValue;
          if (charsValue == null)
            return -1;
          Array.Copy((Array) charsValue, extraEventArgs.DataOffset, (Array) extraEventArgs.CharBuffer, (long) extraEventArgs.BufferOffset, (long) extraEventArgs.Length);
          return (long) extraEventArgs.Length;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetChars(i - this.PrivateVisibleFieldCount, fieldoffset, buffer, bufferoffset, length);
      if ((this._flags & SQLiteConnectionFlags.NoVerifyTextAffinity) != SQLiteConnectionFlags.NoVerifyTextAffinity)
      {
        int num = (int) this.VerifyType(i, DbType.String);
      }
      return this._activeStatement._sql.GetChars(this._activeStatement, i, (int) fieldoffset, buffer, bufferoffset, length);
    }

    public override string GetDataTypeName(int i)
    {
      this.CheckDisposed();
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDataTypeName(i - this.PrivateVisibleFieldCount);
      TypeAffinity nAffinity = TypeAffinity.Uninitialized;
      return this._activeStatement._sql.ColumnType(this._activeStatement, i, ref nAffinity);
    }

    public override DateTime GetDateTime(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetDateTime), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.DateTimeValue.HasValue)
            throw new SQLiteException("missing date/time return value");
          return liteDataReaderValue.DateTimeValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDateTime(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.DateTime);
      return this._activeStatement._sql.GetDateTime(this._activeStatement, i);
    }

    public override Decimal GetDecimal(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetDecimal), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.DecimalValue.HasValue)
            throw new SQLiteException("missing decimal return value");
          return liteDataReaderValue.DecimalValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDecimal(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Decimal);
      return Decimal.Parse(this._activeStatement._sql.GetText(this._activeStatement, i), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public override double GetDouble(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetDouble), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.DoubleValue.HasValue)
            throw new SQLiteException("missing double return value");
          return liteDataReaderValue.DoubleValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetDouble(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Double);
      return this._activeStatement._sql.GetDouble(this._activeStatement, i);
    }

    public override Type GetFieldType(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetFieldType(i - this.PrivateVisibleFieldCount) : SQLiteConvert.SQLiteTypeToType(this.GetSQLiteType(this._flags, i));
    }

    public override float GetFloat(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetFloat), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.FloatValue.HasValue)
            throw new SQLiteException("missing float return value");
          return liteDataReaderValue.FloatValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetFloat(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Single);
      return Convert.ToSingle(this._activeStatement._sql.GetDouble(this._activeStatement, i));
    }

    public override Guid GetGuid(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetGuid), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.GuidValue.HasValue)
            throw new SQLiteException("missing guid return value");
          return liteDataReaderValue.GuidValue.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetGuid(i - this.PrivateVisibleFieldCount);
      if (this.VerifyType(i, DbType.Guid) != TypeAffinity.Blob)
        return new Guid(this._activeStatement._sql.GetText(this._activeStatement, i));
      byte[] numArray = new byte[16];
      this._activeStatement._sql.GetBytes(this._activeStatement, i, 0, numArray, 0, 16);
      return new Guid(numArray);
    }

    public override short GetInt16(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetInt16), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          short? int16Value = liteDataReaderValue.Int16Value;
          if (!(int16Value.HasValue ? new int?((int) int16Value.GetValueOrDefault()) : new int?()).HasValue)
            throw new SQLiteException("missing int16 return value");
          return liteDataReaderValue.Int16Value.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetInt16(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Int16);
      return Convert.ToInt16(this._activeStatement._sql.GetInt32(this._activeStatement, i));
    }

    public override int GetInt32(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetInt32), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.Int32Value.HasValue)
            throw new SQLiteException("missing int32 return value");
          return liteDataReaderValue.Int32Value.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetInt32(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Int32);
      return this._activeStatement._sql.GetInt32(this._activeStatement, i);
    }

    public override long GetInt64(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetInt64), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
        {
          if (!liteDataReaderValue.Int64Value.HasValue)
            throw new SQLiteException("missing int64 return value");
          return liteDataReaderValue.Int64Value.Value;
        }
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetInt64(i - this.PrivateVisibleFieldCount);
      int num = (int) this.VerifyType(i, DbType.Int64);
      return this._activeStatement._sql.GetInt64(this._activeStatement, i);
    }

    public override string GetName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnName(this._activeStatement, i);
    }

    public string GetDatabaseName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnDatabaseName(this._activeStatement, i);
    }

    public string GetTableName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnTableName(this._activeStatement, i);
    }

    public string GetOriginalName(int i)
    {
      this.CheckDisposed();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.GetName(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.ColumnOriginalName(this._activeStatement, i);
    }

    public override int GetOrdinal(string name)
    {
      this.CheckDisposed();
      int num = this._throwOnDisposed ? 1 : 0;
      if (this._fieldIndexes == null)
        this._fieldIndexes = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      int ordinal;
      if (!this._fieldIndexes.TryGetValue(name, out ordinal))
      {
        ordinal = this._activeStatement._sql.ColumnIndex(this._activeStatement, name);
        if (ordinal == -1 && this._keyInfo != null)
        {
          ordinal = this._keyInfo.GetOrdinal(name);
          if (ordinal > -1)
            ordinal += this.PrivateVisibleFieldCount;
        }
        this._fieldIndexes.Add(name, ordinal);
      }
      return ordinal;
    }

    public override DataTable GetSchemaTable()
    {
      this.CheckDisposed();
      return this.GetSchemaTable(true, false);
    }

    private static void GetStatementColumnParents(
      SQLiteBase sql,
      SQLiteStatement stmt,
      int fieldCount,
      ref Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns,
      ref Dictionary<int, SQLiteDataReader.ColumnParent> columnToParent)
    {
      if (parentToColumns == null)
        parentToColumns = new Dictionary<SQLiteDataReader.ColumnParent, List<int>>((IEqualityComparer<SQLiteDataReader.ColumnParent>) new SQLiteDataReader.ColumnParent());
      if (columnToParent == null)
        columnToParent = new Dictionary<int, SQLiteDataReader.ColumnParent>();
      for (int index = 0; index < fieldCount; ++index)
      {
        string databaseName = sql.ColumnDatabaseName(stmt, index);
        string tableName = sql.ColumnTableName(stmt, index);
        string columnName = sql.ColumnOriginalName(stmt, index);
        SQLiteDataReader.ColumnParent key = new SQLiteDataReader.ColumnParent(databaseName, tableName, (string) null);
        SQLiteDataReader.ColumnParent columnParent = new SQLiteDataReader.ColumnParent(databaseName, tableName, columnName);
        List<int> intList;
        if (!parentToColumns.TryGetValue(key, out intList))
          parentToColumns.Add(key, new List<int>((IEnumerable<int>) new int[1]
          {
            index
          }));
        else if (intList != null)
          intList.Add(index);
        else
          parentToColumns[key] = new List<int>((IEnumerable<int>) new int[1]
          {
            index
          });
        columnToParent.Add(index, columnParent);
      }
    }

    private static int CountParents(
      Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns)
    {
      int num = 0;
      if (parentToColumns != null)
      {
        foreach (SQLiteDataReader.ColumnParent key in parentToColumns.Keys)
        {
          if (key != null && !string.IsNullOrEmpty(key.TableName))
            ++num;
        }
      }
      return num;
    }

    internal DataTable GetSchemaTable(bool wantUniqueInfo, bool wantDefaultValue)
    {
      this.CheckClosed();
      int num = this._throwOnDisposed ? 1 : 0;
      Dictionary<SQLiteDataReader.ColumnParent, List<int>> parentToColumns = (Dictionary<SQLiteDataReader.ColumnParent, List<int>>) null;
      Dictionary<int, SQLiteDataReader.ColumnParent> columnToParent = (Dictionary<int, SQLiteDataReader.ColumnParent>) null;
      SQLiteDataReader.GetStatementColumnParents(this._command.Connection._sql, this._activeStatement, this._fieldCount, ref parentToColumns, ref columnToParent);
      DataTable tbl = new DataTable("SchemaTable");
      DataTable dataTable = (DataTable) null;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      tbl.Locale = CultureInfo.InvariantCulture;
      tbl.Columns.Add(SchemaTableColumn.ColumnName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.ColumnSize, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.NumericPrecision, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.NumericScale, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.IsUnique, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.IsKey, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.BaseServerName, typeof (string));
      tbl.Columns.Add(SchemaTableOptionalColumn.BaseCatalogName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.BaseColumnName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.BaseSchemaName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.BaseTableName, typeof (string));
      tbl.Columns.Add(SchemaTableColumn.DataType, typeof (Type));
      tbl.Columns.Add(SchemaTableColumn.AllowDBNull, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.ProviderType, typeof (int));
      tbl.Columns.Add(SchemaTableColumn.IsAliased, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.IsExpression, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsAutoIncrement, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsRowVersion, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsHidden, typeof (bool));
      tbl.Columns.Add(SchemaTableColumn.IsLong, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.IsReadOnly, typeof (bool));
      tbl.Columns.Add(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof (Type));
      tbl.Columns.Add(SchemaTableOptionalColumn.DefaultValue, typeof (object));
      tbl.Columns.Add("DataTypeName", typeof (string));
      tbl.Columns.Add("CollationType", typeof (string));
      tbl.BeginLoadData();
      for (int index = 0; index < this._fieldCount; ++index)
      {
        SQLiteType sqLiteType = this.GetSQLiteType(this._flags, index);
        DataRow row1 = tbl.NewRow();
        DbType type = sqLiteType.Type;
        row1[SchemaTableColumn.ColumnName] = (object) this.GetName(index);
        row1[SchemaTableColumn.ColumnOrdinal] = (object) index;
        row1[SchemaTableColumn.ColumnSize] = (object) SQLiteConvert.DbTypeToColumnSize(type);
        row1[SchemaTableColumn.NumericPrecision] = SQLiteConvert.DbTypeToNumericPrecision(type);
        row1[SchemaTableColumn.NumericScale] = SQLiteConvert.DbTypeToNumericScale(type);
        row1[SchemaTableColumn.ProviderType] = (object) sqLiteType.Type;
        row1[SchemaTableColumn.IsLong] = (object) false;
        row1[SchemaTableColumn.AllowDBNull] = (object) true;
        row1[SchemaTableOptionalColumn.IsReadOnly] = (object) false;
        row1[SchemaTableOptionalColumn.IsRowVersion] = (object) false;
        row1[SchemaTableColumn.IsUnique] = (object) false;
        row1[SchemaTableColumn.IsKey] = (object) false;
        row1[SchemaTableOptionalColumn.IsAutoIncrement] = (object) false;
        row1[SchemaTableColumn.DataType] = (object) this.GetFieldType(index);
        row1[SchemaTableOptionalColumn.IsHidden] = (object) false;
        row1[SchemaTableColumn.BaseSchemaName] = (object) this._baseSchemaName;
        string columnName = columnToParent[index].ColumnName;
        if (!string.IsNullOrEmpty(columnName))
          row1[SchemaTableColumn.BaseColumnName] = (object) columnName;
        row1[SchemaTableColumn.IsExpression] = (object) string.IsNullOrEmpty(columnName);
        row1[SchemaTableColumn.IsAliased] = (object) (string.Compare(this.GetName(index), columnName, StringComparison.OrdinalIgnoreCase) != 0);
        string tableName = columnToParent[index].TableName;
        if (!string.IsNullOrEmpty(tableName))
          row1[SchemaTableColumn.BaseTableName] = (object) tableName;
        string databaseName = columnToParent[index].DatabaseName;
        if (!string.IsNullOrEmpty(databaseName))
          row1[SchemaTableOptionalColumn.BaseCatalogName] = (object) databaseName;
        string dataType = (string) null;
        if (!string.IsNullOrEmpty(columnName))
        {
          string collateSequence = (string) null;
          bool notNull = false;
          bool primaryKey = false;
          bool autoIncrement = false;
          this._command.Connection._sql.ColumnMetaData((string) row1[SchemaTableOptionalColumn.BaseCatalogName], (string) row1[SchemaTableColumn.BaseTableName], columnName, ref dataType, ref collateSequence, ref notNull, ref primaryKey, ref autoIncrement);
          if (notNull || primaryKey)
            row1[SchemaTableColumn.AllowDBNull] = (object) false;
          row1[SchemaTableColumn.IsKey] = (object) (bool) (!primaryKey ? 0 : (SQLiteDataReader.CountParents(parentToColumns) <= 1 ? 1 : 0));
          row1[SchemaTableOptionalColumn.IsAutoIncrement] = (object) autoIncrement;
          row1["CollationType"] = (object) collateSequence;
          string[] strArray1 = dataType.Split('(');
          if (strArray1.Length > 1)
          {
            dataType = strArray1[0];
            string[] strArray2 = strArray1[1].Split(')');
            if (strArray2.Length > 1)
            {
              string[] strArray3 = strArray2[0].Split(',', '.');
              if (sqLiteType.Type == DbType.Binary || SQLiteConvert.IsStringDbType(sqLiteType.Type))
              {
                row1[SchemaTableColumn.ColumnSize] = (object) Convert.ToInt32(strArray3[0], (IFormatProvider) CultureInfo.InvariantCulture);
              }
              else
              {
                row1[SchemaTableColumn.NumericPrecision] = (object) Convert.ToInt32(strArray3[0], (IFormatProvider) CultureInfo.InvariantCulture);
                if (strArray3.Length > 1)
                  row1[SchemaTableColumn.NumericScale] = (object) Convert.ToInt32(strArray3[1], (IFormatProvider) CultureInfo.InvariantCulture);
              }
            }
          }
          if (wantDefaultValue)
          {
            using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].TABLE_INFO([{1}])", row1[SchemaTableOptionalColumn.BaseCatalogName], row1[SchemaTableColumn.BaseTableName]), this._command.Connection))
            {
              using (DbDataReader dbDataReader = (DbDataReader) sqLiteCommand.ExecuteReader())
              {
                while (dbDataReader.Read())
                {
                  if (string.Compare((string) row1[SchemaTableColumn.BaseColumnName], dbDataReader.GetString(1), StringComparison.OrdinalIgnoreCase) == 0)
                  {
                    if (!dbDataReader.IsDBNull(4))
                    {
                      row1[SchemaTableOptionalColumn.DefaultValue] = dbDataReader[4];
                      break;
                    }
                    break;
                  }
                }
              }
            }
          }
          if (wantUniqueInfo)
          {
            if ((string) row1[SchemaTableOptionalColumn.BaseCatalogName] != empty1 || (string) row1[SchemaTableColumn.BaseTableName] != empty2)
            {
              empty1 = (string) row1[SchemaTableOptionalColumn.BaseCatalogName];
              empty2 = (string) row1[SchemaTableColumn.BaseTableName];
              dataTable = this._command.Connection.GetSchema("Indexes", new string[4]
              {
                (string) row1[SchemaTableOptionalColumn.BaseCatalogName],
                null,
                (string) row1[SchemaTableColumn.BaseTableName],
                null
              });
            }
            foreach (DataRow row2 in (InternalDataCollectionBase) dataTable.Rows)
            {
              DataTable schema = this._command.Connection.GetSchema("IndexColumns", new string[5]
              {
                (string) row1[SchemaTableOptionalColumn.BaseCatalogName],
                null,
                (string) row1[SchemaTableColumn.BaseTableName],
                (string) row2["INDEX_NAME"],
                null
              });
              foreach (DataRow row3 in (InternalDataCollectionBase) schema.Rows)
              {
                if (string.Compare(SQLiteConvert.GetStringOrNull(row3["COLUMN_NAME"]), columnName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                  if (parentToColumns.Count == 1 && schema.Rows.Count == 1 && !(bool) row1[SchemaTableColumn.AllowDBNull])
                    row1[SchemaTableColumn.IsUnique] = row2["UNIQUE"];
                  if (schema.Rows.Count == 1)
                  {
                    if ((bool) row2["PRIMARY_KEY"])
                    {
                      if (!string.IsNullOrEmpty(dataType))
                      {
                        if (string.Compare(dataType, "integer", StringComparison.OrdinalIgnoreCase) != 0)
                          break;
                        break;
                      }
                      break;
                    }
                    break;
                  }
                  break;
                }
              }
            }
          }
          if (string.IsNullOrEmpty(dataType))
          {
            TypeAffinity nAffinity = TypeAffinity.Uninitialized;
            dataType = this._activeStatement._sql.ColumnType(this._activeStatement, index, ref nAffinity);
          }
          if (!string.IsNullOrEmpty(dataType))
            row1["DataTypeName"] = (object) dataType;
        }
        tbl.Rows.Add(row1);
      }
      if (this._keyInfo != null)
        this._keyInfo.AppendSchemaTable(tbl);
      tbl.AcceptChanges();
      tbl.EndLoadData();
      return tbl;
    }

    public override string GetString(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetString), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
          return liteDataReaderValue.StringValue;
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetString(i - this.PrivateVisibleFieldCount);
      if ((this._flags & SQLiteConnectionFlags.NoVerifyTextAffinity) != SQLiteConnectionFlags.NoVerifyTextAffinity)
      {
        int num = (int) this.VerifyType(i, DbType.String);
      }
      return this._activeStatement._sql.GetText(this._activeStatement, i);
    }

    public override object GetValue(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      if ((this._flags & SQLiteConnectionFlags.UseConnectionReadValueCallbacks) == SQLiteConnectionFlags.UseConnectionReadValueCallbacks)
      {
        SQLiteDataReaderValue liteDataReaderValue = new SQLiteDataReaderValue();
        bool complete;
        this.InvokeReadValueCallback(i, (SQLiteReadEventArgs) new SQLiteReadValueEventArgs(nameof (GetValue), (SQLiteReadEventArgs) null, liteDataReaderValue), out complete);
        if (complete)
          return liteDataReaderValue.Value;
      }
      if (i >= this.PrivateVisibleFieldCount && this._keyInfo != null)
        return this._keyInfo.GetValue(i - this.PrivateVisibleFieldCount);
      SQLiteType sqLiteType = this.GetSQLiteType(this._flags, i);
      if ((this._flags & SQLiteConnectionFlags.DetectTextAffinity) == SQLiteConnectionFlags.DetectTextAffinity && (sqLiteType == null || sqLiteType.Affinity == TypeAffinity.Text))
        sqLiteType = this.GetSQLiteType(sqLiteType, this._activeStatement._sql.GetText(this._activeStatement, i));
      else if ((this._flags & SQLiteConnectionFlags.DetectStringType) == SQLiteConnectionFlags.DetectStringType && (sqLiteType == null || SQLiteConvert.IsStringDbType(sqLiteType.Type)))
        sqLiteType = this.GetSQLiteType(sqLiteType, this._activeStatement._sql.GetText(this._activeStatement, i));
      return this._activeStatement._sql.GetValue(this._activeStatement, this._flags, i, sqLiteType);
    }

    public override int GetValues(object[] values)
    {
      this.CheckDisposed();
      int values1 = this.FieldCount;
      if (values.Length < values1)
        values1 = values.Length;
      for (int ordinal = 0; ordinal < values1; ++ordinal)
        values[ordinal] = this.GetValue(ordinal);
      return values1;
    }

    public NameValueCollection GetValues()
    {
      this.CheckDisposed();
      if (this._activeStatement == null || this._activeStatement._sql == null)
        throw new InvalidOperationException();
      int visibleFieldCount = this.PrivateVisibleFieldCount;
      NameValueCollection values = new NameValueCollection(visibleFieldCount);
      for (int index = 0; index < visibleFieldCount; ++index)
      {
        string name = this._activeStatement._sql.ColumnName(this._activeStatement, index);
        string text = this._activeStatement._sql.GetText(this._activeStatement, index);
        values.Add(name, text);
      }
      return values;
    }

    public override bool HasRows
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        if ((this._flags & SQLiteConnectionFlags.StickyHasRows) != SQLiteConnectionFlags.StickyHasRows)
          return this._readingState != 1;
        return this._readingState != 1 || this._stepCount > 0;
      }
    }

    public override bool IsClosed
    {
      get
      {
        this.CheckDisposed();
        return this._command == null;
      }
    }

    public override bool IsDBNull(int i)
    {
      this.CheckDisposed();
      this.VerifyForGet();
      return i >= this.PrivateVisibleFieldCount && this._keyInfo != null ? this._keyInfo.IsDBNull(i - this.PrivateVisibleFieldCount) : this._activeStatement._sql.IsNull(this._activeStatement, i);
    }

    public override bool NextResult()
    {
      this.CheckDisposed();
      this.CheckClosed();
      int num1 = this._throwOnDisposed ? 1 : 0;
      SQLiteStatement stmt = (SQLiteStatement) null;
      bool flag = (this._commandBehavior & CommandBehavior.SchemaOnly) != CommandBehavior.Default;
      int num2;
      while (true)
      {
        do
        {
          if (stmt == null && this._activeStatement != null && this._activeStatement._sql != null && this._activeStatement._sql.IsOpen())
          {
            if (!flag)
            {
              int num3 = (int) this._activeStatement._sql.Reset(this._activeStatement);
            }
            if ((this._commandBehavior & CommandBehavior.SingleResult) != CommandBehavior.Default)
            {
              while (true)
              {
                SQLiteStatement statement;
                do
                {
                  statement = this._command.GetStatement(this._activeStatementIndex + 1);
                  if (statement != null)
                  {
                    ++this._activeStatementIndex;
                    if (!flag && statement._sql.Step(statement))
                      ++this._stepCount;
                    if (statement._sql.ColumnCount(statement) == 0)
                    {
                      int changes = 0;
                      bool readOnly = false;
                      if (!statement.TryGetChanges(ref changes, ref readOnly))
                        return false;
                      if (!readOnly)
                      {
                        if (this._rowsAffected == -1)
                          this._rowsAffected = 0;
                        this._rowsAffected += changes;
                      }
                    }
                  }
                  else
                    goto label_17;
                }
                while (flag);
                int num4 = (int) statement._sql.Reset(statement);
              }
label_17:
              return false;
            }
          }
          stmt = this._command.GetStatement(this._activeStatementIndex + 1);
          if (stmt == null)
            return false;
          if (this._readingState < 1)
            this._readingState = 1;
          ++this._activeStatementIndex;
          num2 = stmt._sql.ColumnCount(stmt);
          if (!flag || num2 == 0)
          {
            if (!flag && stmt._sql.Step(stmt))
            {
              ++this._stepCount;
              this._readingState = -1;
              goto label_35;
            }
            else if (num2 == 0)
            {
              int changes = 0;
              bool readOnly = false;
              if (!stmt.TryGetChanges(ref changes, ref readOnly))
                return false;
              if (!readOnly)
              {
                if (this._rowsAffected == -1)
                  this._rowsAffected = 0;
                this._rowsAffected += changes;
              }
            }
            else
              goto label_34;
          }
          else
            goto label_35;
        }
        while (flag);
        int num5 = (int) stmt._sql.Reset(stmt);
      }
label_34:
      this._readingState = 1;
label_35:
      this._activeStatement = stmt;
      this._fieldCount = num2;
      this._fieldIndexes = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._fieldTypeArray = new SQLiteType[this.PrivateVisibleFieldCount];
      if ((this._commandBehavior & CommandBehavior.KeyInfo) != CommandBehavior.Default)
        this.LoadKeyInfo();
      return true;
    }

    internal static SQLiteConnection GetConnection(SQLiteDataReader dataReader)
    {
      try
      {
        if (dataReader != null)
        {
          SQLiteCommand command = dataReader._command;
          if (command != null)
          {
            SQLiteConnection connection = command.Connection;
            if (connection != null)
              return connection;
          }
        }
      }
      catch (ObjectDisposedException ex)
      {
      }
      return (SQLiteConnection) null;
    }

    private SQLiteType GetSQLiteType(SQLiteType oldType, string text)
    {
      if (SQLiteConvert.LooksLikeNull(text))
        return new SQLiteType(TypeAffinity.Null, DbType.Object);
      if (SQLiteConvert.LooksLikeInt64(text))
        return new SQLiteType(TypeAffinity.Int64, DbType.Int64);
      if (SQLiteConvert.LooksLikeDouble(text))
        return new SQLiteType(TypeAffinity.Double, DbType.Double);
      return this._activeStatement != null && SQLiteConvert.LooksLikeDateTime((SQLiteConvert) this._activeStatement._sql, text) ? new SQLiteType(TypeAffinity.DateTime, DbType.DateTime) : oldType;
    }

    private SQLiteType GetSQLiteType(SQLiteConnectionFlags flags, int i)
    {
      SQLiteType sqLiteType = this._fieldTypeArray[i] ?? (this._fieldTypeArray[i] = new SQLiteType());
      if (sqLiteType.Affinity == TypeAffinity.Uninitialized)
        sqLiteType.Type = SQLiteConvert.TypeNameToDbType(SQLiteDataReader.GetConnection(this), this._activeStatement._sql.ColumnType(this._activeStatement, i, ref sqLiteType.Affinity), flags);
      else
        sqLiteType.Affinity = this._activeStatement._sql.ColumnAffinity(this._activeStatement, i);
      return sqLiteType;
    }

    public override bool Read()
    {
      this.CheckDisposed();
      this.CheckClosed();
      int num = this._throwOnDisposed ? 1 : 0;
      if ((this._commandBehavior & CommandBehavior.SchemaOnly) != CommandBehavior.Default)
        return false;
      if (this._readingState == -1)
      {
        this._readingState = 0;
        return true;
      }
      if (this._readingState == 0)
      {
        if ((this._commandBehavior & CommandBehavior.SingleRow) == CommandBehavior.Default && this._activeStatement._sql.Step(this._activeStatement))
        {
          ++this._stepCount;
          if (this._keyInfo != null)
            this._keyInfo.Reset();
          return true;
        }
        this._readingState = 1;
      }
      return false;
    }

    public override int RecordsAffected
    {
      get
      {
        this.CheckDisposed();
        return this._rowsAffected;
      }
    }

    public override object this[string name]
    {
      get
      {
        this.CheckDisposed();
        return this.GetValue(this.GetOrdinal(name));
      }
    }

    public override object this[int i]
    {
      get
      {
        this.CheckDisposed();
        return this.GetValue(i);
      }
    }

    private void LoadKeyInfo()
    {
      if (this._keyInfo != null)
      {
        this._keyInfo.Dispose();
        this._keyInfo = (SQLiteKeyReader) null;
      }
      this._keyInfo = new SQLiteKeyReader(this._command.Connection, this, this._activeStatement);
    }

    private sealed class ColumnParent : IEqualityComparer<SQLiteDataReader.ColumnParent>
    {
      public string DatabaseName;
      public string TableName;
      public string ColumnName;

      public ColumnParent()
      {
      }

      public ColumnParent(string databaseName, string tableName, string columnName)
        : this()
      {
        this.DatabaseName = databaseName;
        this.TableName = tableName;
        this.ColumnName = columnName;
      }

      public bool Equals(SQLiteDataReader.ColumnParent x, SQLiteDataReader.ColumnParent y)
      {
        return x == null && y == null || x != null && y != null && string.Equals(x.DatabaseName, y.DatabaseName, StringComparison.OrdinalIgnoreCase) && string.Equals(x.TableName, y.TableName, StringComparison.OrdinalIgnoreCase) && string.Equals(x.ColumnName, y.ColumnName, StringComparison.OrdinalIgnoreCase);
      }

      public int GetHashCode(SQLiteDataReader.ColumnParent obj)
      {
        int hashCode = 0;
        if (obj != null && obj.DatabaseName != null)
          hashCode ^= obj.DatabaseName.GetHashCode();
        if (obj != null && obj.TableName != null)
          hashCode ^= obj.TableName.GetHashCode();
        if (obj != null && obj.ColumnName != null)
          hashCode ^= obj.ColumnName.GetHashCode();
        return hashCode;
      }
    }
  }
}
