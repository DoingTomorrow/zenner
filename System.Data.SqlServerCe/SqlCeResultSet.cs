﻿// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeResultSet
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.ComponentModel;
using System.Data.SqlTypes;

#nullable disable
namespace System.Data.SqlServerCe
{
  public class SqlCeResultSet : SqlCeDataReader, IEnumerable, IListSource
  {
    private bool isInitialized;
    private ArrayList bookmarkArray;
    private SqlCeUpdatableRecord sqlUpdatableRecord;
    private ResultSetChangedEventHandler onResultSetChanged;

    protected SqlCeResultSet() => NativeMethods.LoadNativeBinaries();

    internal SqlCeResultSet(SqlCeConnection connection, SqlCeCommand command)
      : base(connection, command)
    {
      NativeMethods.LoadNativeBinaries();
    }

    internal event ResultSetChangedEventHandler ResultSetChanged
    {
      add => this.onResultSetChanged += value;
      remove => this.onResultSetChanged -= value;
    }

    private void OnResultSetChanged(object sender, ResultSetChangedEventArgs e)
    {
      if (this.onResultSetChanged == null)
        return;
      this.onResultSetChanged(sender, e);
    }

    public override object this[int index]
    {
      get => this.HasColumnChanged(index) ? this.sqlUpdatableRecord[index] : base[index];
    }

    public override object this[string name]
    {
      get
      {
        return this.HasColumnChanged(this.GetOrdinal(name)) ? this.sqlUpdatableRecord[name] : base[name];
      }
    }

    public bool Updatable
    {
      get
      {
        if (this.IsClosed)
          throw new InvalidOperationException(Res.GetString("SQL_SqlResultSetClosed", (object) nameof (Updatable)));
        return ResultSetOptions.None != (this.cursorCapabilities & ResultSetOptions.Updatable);
      }
    }

    public bool Scrollable
    {
      get
      {
        if (this.IsClosed)
          throw new InvalidOperationException(Res.GetString("SQL_SqlResultSetClosed", (object) nameof (Scrollable)));
        return ResultSetOptions.None != (this.cursorCapabilities & ResultSetOptions.Scrollable);
      }
    }

    public ResultSetSensitivity Sensitivity
    {
      get
      {
        ResultSetOptions cursorCapabilities = this.cursorCapabilities;
        if (this.IsClosed)
          throw new InvalidOperationException(Res.GetString("SQL_SqlResultSetClosed", (object) nameof (Sensitivity)));
        if ((cursorCapabilities & ResultSetOptions.Sensitive) != ResultSetOptions.None)
          return ResultSetSensitivity.Sensitive;
        return (cursorCapabilities & ResultSetOptions.Insensitive) != ResultSetOptions.None ? ResultSetSensitivity.Insensitive : ResultSetSensitivity.Asensitive;
      }
    }

    internal ArrayList BookmarkArray
    {
      get
      {
        if (!this.Scrollable)
          throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotScrollable"));
        if (this.bookmarkArray != null)
          return this.bookmarkArray;
        int hBookmark = 0;
        this.bookmarkArray = new ArrayList();
        if (!this.Move(DIRECTION.MOVE_FIRST))
          return this.bookmarkArray;
        bool flag = false;
        while (!flag)
        {
          int hr = NativeMethods.GetBookmark(this.pSeCursor, ref hBookmark, this.pError);
          if (hr == 0)
          {
            this.bookmarkArray.Add((object) hBookmark);
            hr = NativeMethods.Move(this.pSeCursor, DIRECTION.MOVE_NEXT, this.pError);
          }
          if (NativeMethods.Failed(hr))
          {
            int lMinor = 0;
            int minorError = NativeMethods.GetMinorError(this.pError, ref lMinor);
            if (25001 != lMinor)
            {
              if (minorError != 0)
                this.ProcessResults(minorError);
            }
            else
            {
              NativeMethods.ClearErrorInfo(this.pError);
              flag = true;
            }
          }
        }
        this.Move(DIRECTION.MOVE_FIRST);
        this.Move(DIRECTION.MOVE_PREVIOUS);
        return this.bookmarkArray;
      }
    }

    bool IListSource.ContainsListCollection => this.ContainsListCollection;

    protected bool ContainsListCollection => false;

    public ResultSetView ResultSetView => (ResultSetView) ((IListSource) this).GetList();

    IList IListSource.GetList() => this.GetList();

    protected IList GetList() => (IList) new ResultSetView(this);

    internal bool GotoRow(int hBookmark)
    {
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeResultSet));
      if (this.isOnRow)
        this.ReleaseCurrentRow();
      int hr = NativeMethods.GotoBookmark(this.pSeCursor, hBookmark, this.pError);
      if (NativeMethods.Failed(hr))
      {
        if (-2147217906 == hr)
          return false;
        throw this.connection.ProcessResults(hr, this.pError, (object) this);
      }
      this.cursorPosition = CursorPosition.OnRow;
      this.isOnRow = true;
      this.fetchDirection = FETCH.FORWARD;
      this.OnMove();
      return true;
    }

    private void InitializeMetaData()
    {
      if (this.metadata == null)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      if (this.fieldNameLookup == null)
        this.fieldNameLookup = new FieldNameLookup((IDataReader) this, -1);
      this.isInitialized = true;
    }

    internal void InternalUpdate(object sender)
    {
      int hBookmark = -1;
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeResultSet));
      if (!this.Updatable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotUpdatable"));
      if (!this.isOnRow)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      if (this.sqlUpdatableRecord == null)
        return;
      int hr1 = NativeMethods.Prepare(this.pSeCursor, SEPREPAREMODE.PREP_UPDATE, this.pError);
      if (hr1 != 0)
        this.ProcessResults(hr1);
      try
      {
        this.SetValues(SEPREPAREMODE.PREP_UPDATE, this.sqlUpdatableRecord);
        int hr2 = NativeMethods.UpdateRecord(this.pSeCursor, this.pError);
        if (hr2 != 0)
          this.ProcessResults(hr2);
        if (this.onResultSetChanged != null && this.bookmarkArray != null)
        {
          int bookmark = NativeMethods.GetBookmark(this.pSeCursor, ref hBookmark, this.pError);
          if (bookmark != 0)
            this.ProcessResults(bookmark);
          this.OnResultSetChanged(sender, new ResultSetChangedEventArgs(ChangeType.RowUpdated, new RowView(hBookmark)));
        }
        this.ReleaseCurrentRow();
        this.isOnRow = true;
        this.cursorPosition = CursorPosition.OnRow;
      }
      catch (Exception ex)
      {
        NativeMethods.Prepare(this.pSeCursor, SEPREPAREMODE.PREP_CANCEL, this.pError);
        throw ex;
      }
    }

    internal void RemoveBookmarkFromCache(int hBookmark)
    {
      if (this.bookmarkArray == null)
        return;
      int index = this.bookmarkArray.IndexOf((object) hBookmark);
      if (-1 == index)
        return;
      this.bookmarkArray.RemoveAt(index);
      if (this.onResultSetChanged == null)
        return;
      this.OnResultSetChanged((object) this, new ResultSetChangedEventArgs(ChangeType.RowDeleted, new RowView(hBookmark)));
    }

    internal int InternalInsert(bool fMoveTo, object sender, SqlCeUpdatableRecord record)
    {
      int hBookmark = 0;
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeResultSet));
      if (!this.Updatable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotUpdatable"));
      int hr1 = NativeMethods.Prepare(this.pSeCursor, SEPREPAREMODE.PREP_INSERT, this.pError);
      if (hr1 != 0)
        this.ProcessResults(hr1);
      try
      {
        this.SetValues(SEPREPAREMODE.PREP_INSERT, record);
        int hr2 = NativeMethods.InsertRecord(fMoveTo ? 1 : 0, this.pSeCursor, ref hBookmark, this.pError);
        if (hr2 != 0)
          this.ProcessResults(hr2);
        if (fMoveTo)
        {
          if (this.isOnRow)
            this.ReleaseCurrentRow();
          this.isOnRow = true;
          this.cursorPosition = CursorPosition.OnRow;
          this.OnMove();
        }
        if (this.bookmarkArray != null)
        {
          this.bookmarkArray.Add((object) hBookmark);
          if (this.onResultSetChanged != null)
            this.OnResultSetChanged(sender, new ResultSetChangedEventArgs(ChangeType.RowInserted, new RowView((ResultSetView) null, hBookmark, record)));
        }
        return hBookmark;
      }
      catch (Exception ex)
      {
        NativeMethods.Prepare(this.pSeCursor, SEPREPAREMODE.PREP_CANCEL, this.pError);
        throw ex;
      }
    }

    private unsafe void SetValues(SEPREPAREMODE mode, SqlCeUpdatableRecord record)
    {
      int fieldCount = record.FieldCount;
      ColumnUpdatedStatus[] columnUpdatedStatusArray = record != null ? record.ColumnsUpdatedStatus : throw new ArgumentException(record.GetType().ToString());
      if (columnUpdatedStatusArray == null)
        return;
      try
      {
        for (int ordinal = 0; ordinal < fieldCount; ++ordinal)
        {
          bool flag1 = false;
          bool flag2 = false;
          SqlCeType typeMap = this.metadata[ordinal].typeMap;
          if (columnUpdatedStatusArray[ordinal] == ColumnUpdatedStatus.None)
            flag1 = true;
          if (ColumnUpdatedStatus.ServerDefault == columnUpdatedStatusArray[ordinal])
            flag2 = true;
          if (!typeMap.isBLOB)
          {
            Accessor accessor = this.accessorArray[this.accessorIndex[ordinal]];
            accessor.CurrentIndex = this.bindingIndex[ordinal];
            if (flag1)
              accessor.StatusValue = DBStatus.S_IGNORE;
            else if (flag2)
              accessor.StatusValue = DBStatus.S_DEFAULT;
            else
              accessor.Value = record.values[ordinal];
          }
          else if (!flag1)
          {
            if (flag2)
            {
              int hr = NativeMethods.SetValue(this.pSeCursor, 2, (void*) null, ordinal, 0, this.pError);
              if (hr != 0)
                this.ProcessResults(hr);
            }
            else if (record.IsDBNull(ordinal))
            {
              int hr = NativeMethods.SetValue(this.pSeCursor, 0, (void*) null, ordinal, 0, this.pError);
              if (hr != 0)
                this.ProcessResults(hr);
            }
            else if (SqlDbType.NText == typeMap.sqlDbType)
            {
              object obj = record.values[ordinal];
              switch (obj)
              {
                case char[] _:
                  char[] chArray = (char[]) obj;
                  int size1 = chArray.Length * 2;
                  fixed (char* pBuffer = chArray)
                  {
                    int hr = NativeMethods.SetValue(this.pSeCursor, 0, (void*) pBuffer, ordinal, size1, this.pError);
                    if (hr != 0)
                    {
                      this.ProcessResults(hr);
                      continue;
                    }
                    continue;
                  }
                case string _:
                  string str = (string) obj;
                  int size2 = str.Length * 2;
                  fixed (char* pBuffer = str)
                  {
                    int hr = NativeMethods.SetValue(this.pSeCursor, 0, (void*) pBuffer, ordinal, size2, this.pError);
                    if (hr != 0)
                    {
                      this.ProcessResults(hr);
                      continue;
                    }
                    continue;
                  }
                default:
                  continue;
              }
            }
            else if (SqlDbType.Image == typeMap.sqlDbType)
            {
              byte[] numArray = (byte[]) record.values[ordinal];
              int length = numArray.Length;
              fixed (byte* pBuffer = numArray)
              {
                int hr = NativeMethods.SetValue(this.pSeCursor, 0, (void*) pBuffer, ordinal, length, this.pError);
                if (hr != 0)
                  this.ProcessResults(hr);
              }
            }
          }
        }
        if (this.accessorArray[0] == null)
          return;
        Accessor accessor1 = this.accessorArray[0];
        fixed (MEDBBINDING* prgBinding = accessor1.DbBinding)
        {
          int hr = NativeMethods.SetValues(this.connection.IQPServices, this.pSeCursor, (IntPtr) (void*) prgBinding, accessor1.Count, accessor1.DataHandle, this.pError);
          if (hr == 0)
            return;
          this.ProcessResults(hr);
        }
      }
      catch (SqlCeException ex)
      {
        NativeMethods.Prepare(this.pSeCursor, SEPREPAREMODE.PREP_CANCEL, this.pError);
        throw ex;
      }
    }

    protected override void OnMove()
    {
      if (this.sqlUpdatableRecord == null)
        return;
      foreach (ColumnUpdatedStatus columnUpdatedStatus in this.sqlUpdatableRecord.ColumnsUpdatedStatus)
        columnUpdatedStatus = ColumnUpdatedStatus.None;
    }

    internal SqlCeUpdatableRecord GetCurrentRecord()
    {
      if (!this.isInitialized)
        this.InitializeMetaData();
      SqlCeUpdatableRecord currentRecord = new SqlCeUpdatableRecord(this.metadata, (object[]) null, true, 0, this.fieldNameLookup);
      for (int ordinal = 0; ordinal < this.FieldCount; ++ordinal)
      {
        currentRecord[ordinal] = SETYPE.NUMERIC != this.metadata[ordinal].typeMap.seType ? this.GetValue(ordinal) : (object) this.GetSqlDecimal(ordinal);
        currentRecord.ColumnsUpdatedStatus[ordinal] = ColumnUpdatedStatus.None;
      }
      return currentRecord;
    }

    private void ValidateSet(string method)
    {
      if (!this.Updatable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotUpdatable"));
      if (!this.isInitialized)
        this.InitializeMetaData();
      if (this.sqlUpdatableRecord != null)
        return;
      if (!this.isInitialized)
        this.InitializeMetaData();
      this.sqlUpdatableRecord = new SqlCeUpdatableRecord(this.metadata, (object[]) null, true, 0, this.fieldNameLookup);
    }

    private bool HasColumnChanged(int ordinal)
    {
      if (this.sqlUpdatableRecord == null)
        return false;
      switch (this.sqlUpdatableRecord.ColumnsUpdatedStatus[ordinal])
      {
        case ColumnUpdatedStatus.None:
          return false;
        case ColumnUpdatedStatus.ServerDefault:
          throw new InvalidOperationException(Res.GetString("SQLCE_UnableToFetchDefaultValue"));
        default:
          return true;
      }
    }

    public SqlCeUpdatableRecord CreateRecord()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeResultSet));
      if (!this.Updatable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotUpdatable"));
      if (!this.isInitialized)
        this.InitializeMetaData();
      return new SqlCeUpdatableRecord(this.metadata, (object[]) null, this.Updatable, 0, this.fieldNameLookup);
    }

    public void Update() => this.InternalUpdate((object) this);

    public void Delete()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (SqlCeResultSet));
      if (!this.Updatable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotUpdatable"));
      if (!this.isOnRow)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      try
      {
        int hBookmark = -1;
        if (this.bookmarkArray != null)
        {
          int bookmark = NativeMethods.GetBookmark(this.pSeCursor, ref hBookmark, this.pError);
          if (bookmark != 0)
            this.ProcessResults(bookmark);
        }
        int hr = NativeMethods.DeleteRecord(this.pSeCursor, this.pError);
        if (hr != 0)
          this.ProcessResults(hr);
        this.RemoveBookmarkFromCache(hBookmark);
      }
      finally
      {
        this.ReleaseCurrentRow();
      }
    }

    public void Insert(SqlCeUpdatableRecord record)
    {
      this.InternalInsert(false, (object) this, record);
    }

    public void Insert(SqlCeUpdatableRecord record, DbInsertOptions options)
    {
      if ((DbInsertOptions.PositionOnInsertedRow & options) != (DbInsertOptions) 0 && (DbInsertOptions.KeepCurrentPosition & options) != (DbInsertOptions) 0)
        throw new ArgumentException(options.ToString());
      this.InternalInsert(DbInsertOptions.PositionOnInsertedRow == options, (object) this, record);
    }

    public bool ReadFirst()
    {
      if (!this.Scrollable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotScrollable"));
      if (IntPtr.Zero != this.pSeCursor)
      {
        bool flag = this.Move(DIRECTION.MOVE_FIRST);
        this.OnMove();
        return flag;
      }
      if (this.IsClosed)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (ReadFirst)));
      return false;
    }

    public bool ReadLast()
    {
      if (!this.Scrollable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotScrollable"));
      if (IntPtr.Zero != this.pSeCursor)
      {
        bool flag = this.Move(DIRECTION.MOVE_LAST);
        this.OnMove();
        return flag;
      }
      if (this.IsClosed)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (ReadLast)));
      return false;
    }

    public bool ReadPrevious()
    {
      if (!this.Scrollable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotScrollable"));
      if (IntPtr.Zero != this.pSeCursor)
      {
        if (CursorPosition.BeforeFirst == this.cursorPosition)
          return false;
        bool flag = this.Move(DIRECTION.MOVE_PREVIOUS);
        this.OnMove();
        return flag;
      }
      if (this.IsClosed)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (ReadPrevious)));
      return false;
    }

    public bool ReadAbsolute(int position)
    {
      if (!this.Scrollable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotScrollable"));
      ArrayList bookmarkArray = this.BookmarkArray;
      if (this.isOnRow)
        this.ReleaseCurrentRow();
      if (position >= 0 && position < bookmarkArray.Count)
        return this.GotoRow((int) bookmarkArray[position]);
      return position < 0 && Math.Abs(position) <= bookmarkArray.Count && this.GotoRow((int) bookmarkArray[bookmarkArray.Count + position]);
    }

    public bool ReadRelative(int position)
    {
      bool flag = true;
      if (!this.Scrollable)
        throw new InvalidOperationException(Res.GetString("SQLCE_CursorNotScrollable"));
      if (position == 0)
        return this.isOnRow;
      if (IntPtr.Zero != this.pSeCursor)
      {
        for (; flag && position > 0; flag = this.Move(DIRECTION.MOVE_NEXT))
          --position;
        for (; flag && position < 0; flag = this.Move(DIRECTION.MOVE_PREVIOUS))
          ++position;
      }
      else if (this.IsClosed)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (ReadRelative)));
      this.OnMove();
      return flag;
    }

    public void SetDefault(int ordinal)
    {
      this.ValidateSet(nameof (SetDefault));
      this.sqlUpdatableRecord.ColumnsUpdatedStatus[ordinal] = ColumnUpdatedStatus.ServerDefault;
    }

    public bool IsSetAsDefault(int ordinal)
    {
      this.ValidateSet(nameof (IsSetAsDefault));
      return ColumnUpdatedStatus.ServerDefault == this.sqlUpdatableRecord.ColumnsUpdatedStatus[ordinal];
    }

    public override IEnumerator GetEnumerator() => ((IEnumerable) this).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
      ResultSetEnumerator enumerator = new ResultSetEnumerator(this);
      enumerator.Reset();
      return (IEnumerator) enumerator;
    }

    public SqlMetaData GetSqlMetaData(int ordinal)
    {
      if (this.metadata == null)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      return this.metadata[ordinal].SqlMetaData;
    }

    public override bool IsDBNull(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.IsDBNull(ordinal) : base.IsDBNull(ordinal);
    }

    public override bool GetBoolean(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetBoolean(ordinal) : base.GetBoolean(ordinal);
    }

    public override Decimal GetDecimal(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetDecimal(ordinal) : base.GetDecimal(ordinal);
    }

    public override byte GetByte(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetByte(ordinal) : base.GetByte(ordinal);
    }

    public override long GetBytes(
      int ordinal,
      long dataIndex,
      byte[] buffer,
      int bufferIndex,
      int length)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetBytes(ordinal, dataIndex, buffer, bufferIndex, length) : base.GetBytes(ordinal, dataIndex, buffer, bufferIndex, length);
    }

    public override long GetChars(
      int ordinal,
      long dataIndex,
      char[] buffer,
      int bufferIndex,
      int length)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetChars(ordinal, dataIndex, buffer, bufferIndex, length) : base.GetChars(ordinal, dataIndex, buffer, bufferIndex, length);
    }

    public override DateTime GetDateTime(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetDateTime(ordinal) : base.GetDateTime(ordinal);
    }

    public override double GetDouble(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetDouble(ordinal) : base.GetDouble(ordinal);
    }

    public override float GetFloat(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetFloat(ordinal) : base.GetFloat(ordinal);
    }

    public override Guid GetGuid(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetGuid(ordinal) : base.GetGuid(ordinal);
    }

    public override short GetInt16(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetInt16(ordinal) : base.GetInt16(ordinal);
    }

    public override int GetInt32(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetInt32(ordinal) : base.GetInt32(ordinal);
    }

    public override long GetInt64(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetInt64(ordinal) : base.GetInt64(ordinal);
    }

    public override string GetString(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetString(ordinal) : base.GetString(ordinal);
    }

    public override object GetValue(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetValue(ordinal) : base.GetValue(ordinal);
    }

    public override int GetValues(object[] values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      int values1 = Math.Min(values.Length, this.FieldCount);
      for (int ordinal = 0; ordinal < values1; ++ordinal)
        values[ordinal] = this.GetValue(ordinal);
      return values1;
    }

    public void SetObjectRef(int ordinal, object buffer)
    {
      this.ValidateSet("SetValue");
      this.sqlUpdatableRecord.SetObjectRef(ordinal, buffer);
    }

    public int SetValues(object[] values)
    {
      this.ValidateSet(nameof (SetValues));
      return this.sqlUpdatableRecord.SetValues(values);
    }

    public void SetValue(int ordinal, object value)
    {
      this.ValidateSet(nameof (SetValue));
      this.sqlUpdatableRecord.SetValue(ordinal, value);
    }

    public void SetBoolean(int ordinal, bool value)
    {
      this.ValidateSet(nameof (SetBoolean));
      this.sqlUpdatableRecord.SetBoolean(ordinal, value);
    }

    public void SetByte(int ordinal, byte value)
    {
      this.ValidateSet(nameof (SetByte));
      this.sqlUpdatableRecord.SetByte(ordinal, value);
    }

    public void SetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
    {
      this.ValidateSet(nameof (SetBytes));
      SqlDbType sqlDbType = this.GetSqlMetaData(ordinal).SqlDbType;
      switch (sqlDbType)
      {
        case SqlDbType.Binary:
        case SqlDbType.Image:
        case SqlDbType.VarBinary:
          long num = 0;
          if (!this.IsDBNull(ordinal))
            num = this.GetBytes(ordinal, 0L, (byte[]) null, 0, 0);
          if (dataIndex > 0L || (long) length < num)
            this.sqlUpdatableRecord[ordinal] = this.GetValue(ordinal);
          this.sqlUpdatableRecord.SetBytes(ordinal, dataIndex, buffer, bufferIndex, length);
          break;
        default:
          throw new ArgumentException(Res.GetString("SQLCE_DataColumn_SetFailed", (object) buffer.ToString(), (object) this.GetName(ordinal), (object) sqlDbType.ToString()));
      }
    }

    public void SetChar(int ordinal, char c)
    {
      this.ValidateSet(nameof (SetChar));
      this.sqlUpdatableRecord.SetChar(ordinal, c);
    }

    public void SetChars(int ordinal, long dataIndex, char[] buffer, int bufferIndex, int length)
    {
      this.ValidateSet(nameof (SetChars));
      SqlDbType sqlDbType = this.GetSqlMetaData(ordinal).SqlDbType;
      switch (sqlDbType)
      {
        case SqlDbType.NChar:
        case SqlDbType.NText:
        case SqlDbType.NVarChar:
          long num = 0;
          if (!this.IsDBNull(ordinal))
            num = this.GetChars(ordinal, 0L, (char[]) null, 0, 0);
          if (dataIndex > 0L || (long) length < num)
            this.sqlUpdatableRecord[ordinal] = this.GetValue(ordinal);
          this.sqlUpdatableRecord.SetChars(ordinal, dataIndex, buffer, bufferIndex, length);
          break;
        default:
          throw new ArgumentException(Res.GetString("SQLCE_DataColumn_SetFailed", (object) buffer.ToString(), (object) this.GetName(ordinal), (object) sqlDbType.ToString()));
      }
    }

    public void SetDouble(int ordinal, double value)
    {
      this.ValidateSet(nameof (SetDouble));
      this.sqlUpdatableRecord.SetDouble(ordinal, value);
    }

    public void SetDecimal(int ordinal, Decimal value)
    {
      this.ValidateSet(nameof (SetDecimal));
      this.sqlUpdatableRecord.SetDecimal(ordinal, value);
    }

    public void SetDateTime(int ordinal, DateTime value)
    {
      this.ValidateSet(nameof (SetDateTime));
      this.sqlUpdatableRecord.SetDateTime(ordinal, value);
    }

    public void SetGuid(int ordinal, Guid value)
    {
      this.ValidateSet(nameof (SetGuid));
      this.sqlUpdatableRecord.SetGuid(ordinal, value);
    }

    public void SetInt16(int ordinal, short value)
    {
      this.ValidateSet(nameof (SetInt16));
      this.sqlUpdatableRecord.SetInt16(ordinal, value);
    }

    public void SetInt32(int ordinal, int value)
    {
      this.ValidateSet(nameof (SetInt32));
      this.sqlUpdatableRecord.SetInt32(ordinal, value);
    }

    public void SetInt64(int ordinal, long value)
    {
      this.ValidateSet(nameof (SetInt64));
      this.sqlUpdatableRecord.SetInt64(ordinal, value);
    }

    public void SetFloat(int ordinal, float value)
    {
      this.ValidateSet(nameof (SetFloat));
      this.sqlUpdatableRecord.SetFloat(ordinal, value);
    }

    public void SetString(int ordinal, string value)
    {
      this.ValidateSet(nameof (SetString));
      this.sqlUpdatableRecord.SetString(ordinal, value);
    }

    public override SqlBoolean GetSqlBoolean(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlBoolean(ordinal) : base.GetSqlBoolean(ordinal);
    }

    public override SqlMoney GetSqlMoney(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlMoney(ordinal) : base.GetSqlMoney(ordinal);
    }

    public override SqlDecimal GetSqlDecimal(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlDecimal(ordinal) : base.GetSqlDecimal(ordinal);
    }

    public override SqlByte GetSqlByte(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlByte(ordinal) : base.GetSqlByte(ordinal);
    }

    public override SqlBinary GetSqlBinary(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlBinary(ordinal) : base.GetSqlBinary(ordinal);
    }

    public override SqlDateTime GetSqlDateTime(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlDateTime(ordinal) : base.GetSqlDateTime(ordinal);
    }

    public override SqlDouble GetSqlDouble(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlDouble(ordinal) : base.GetSqlDouble(ordinal);
    }

    public override SqlSingle GetSqlSingle(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlSingle(ordinal) : base.GetSqlSingle(ordinal);
    }

    public override SqlGuid GetSqlGuid(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlGuid(ordinal) : base.GetSqlGuid(ordinal);
    }

    public override SqlInt16 GetSqlInt16(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlInt16(ordinal) : base.GetSqlInt16(ordinal);
    }

    public override SqlInt32 GetSqlInt32(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlInt32(ordinal) : base.GetSqlInt32(ordinal);
    }

    public override SqlInt64 GetSqlInt64(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlInt64(ordinal) : base.GetSqlInt64(ordinal);
    }

    public override SqlString GetSqlString(int ordinal)
    {
      return this.HasColumnChanged(ordinal) ? this.sqlUpdatableRecord.GetSqlString(ordinal) : base.GetSqlString(ordinal);
    }

    public void SetSqlBoolean(int ordinal, SqlBoolean value)
    {
      this.ValidateSet(nameof (SetSqlBoolean));
      this.sqlUpdatableRecord.SetSqlBoolean(ordinal, value);
    }

    public void SetSqlBinary(int ordinal, SqlBinary value)
    {
      this.ValidateSet(nameof (SetSqlBinary));
      this.sqlUpdatableRecord.SetSqlBinary(ordinal, value);
    }

    public void SetSqlByte(int ordinal, SqlByte value)
    {
      this.ValidateSet(nameof (SetSqlByte));
      this.sqlUpdatableRecord.SetSqlByte(ordinal, value);
    }

    public void SetSqlInt16(int ordinal, SqlInt16 value)
    {
      this.ValidateSet(nameof (SetSqlInt16));
      this.sqlUpdatableRecord.SetSqlInt16(ordinal, value);
    }

    public void SetSqlInt32(int ordinal, SqlInt32 value)
    {
      this.ValidateSet(nameof (SetSqlInt32));
      this.sqlUpdatableRecord.SetSqlInt32(ordinal, value);
    }

    public void SetSqlInt64(int ordinal, SqlInt64 value)
    {
      this.ValidateSet(nameof (SetSqlInt64));
      this.sqlUpdatableRecord.SetSqlInt64(ordinal, value);
    }

    public void SetSqlSingle(int ordinal, SqlSingle value)
    {
      this.ValidateSet(nameof (SetSqlSingle));
      this.sqlUpdatableRecord.SetSqlSingle(ordinal, value);
    }

    public void SetSqlDouble(int ordinal, SqlDouble value)
    {
      this.ValidateSet(nameof (SetSqlDouble));
      this.sqlUpdatableRecord.SetSqlDouble(ordinal, value);
    }

    public void SetSqlString(int ordinal, SqlString value)
    {
      this.ValidateSet(nameof (SetSqlString));
      this.sqlUpdatableRecord.SetSqlString(ordinal, value);
    }

    public void SetSqlMoney(int ordinal, SqlMoney value)
    {
      this.ValidateSet(nameof (SetSqlMoney));
      this.sqlUpdatableRecord.SetSqlMoney(ordinal, value);
    }

    public void SetSqlDecimal(int ordinal, SqlDecimal value)
    {
      this.ValidateSet(nameof (SetSqlDecimal));
      this.sqlUpdatableRecord.SetSqlDecimal(ordinal, value);
    }

    public void SetSqlDateTime(int ordinal, SqlDateTime value)
    {
      this.ValidateSet(nameof (SetSqlDateTime));
      this.sqlUpdatableRecord.SetSqlDateTime(ordinal, value);
    }

    public void SetSqlGuid(int ordinal, SqlGuid value)
    {
      this.ValidateSet(nameof (SetSqlGuid));
      this.sqlUpdatableRecord.SetSqlGuid(ordinal, value);
    }
  }
}
