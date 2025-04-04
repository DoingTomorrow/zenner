// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeDataReader
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SqlServerCe
{
  public class SqlCeDataReader : DbDataReader
  {
    private object thisLock = new object();
    private int currAccessor;
    internal int[] bindingIndex;
    internal int[] accessorIndex;
    internal int recordsAffected;
    internal ResultSetOptions cursorCapabilities;
    internal CursorPosition cursorPosition;
    private int fieldCount;
    private int lastBlobColumn;
    private int indexColumnCount;
    private bool sequentialAccess;
    internal bool isDisposed;
    private bool isFinalized;
    private int sequentialOrdinal;
    private long sequentialUnitsRead;
    internal FETCH fetchDirection;
    internal bool isBaseTableCursor;
    internal bool isOnRow;
    private bool isClosed;
    private bool useIndex;
    private bool isHostControlled;
    internal SqlCeConnection connection;
    internal SqlCeTransaction transaction;
    internal Accessor[] accessorArray;
    internal IntPtr pSeCursor;
    internal IntPtr pError;
    private CommandBehavior behavior;
    private DataTable dbSchemaTable;
    internal MetaData[] metadata;
    internal FieldNameLookup fieldNameLookup;

    public override int Depth => 0;

    public override int FieldCount
    {
      get
      {
        if (this.isClosed)
          throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (FieldCount)));
        return this.fieldCount;
      }
    }

    public override bool IsClosed => this.isClosed;

    public override int RecordsAffected => this.recordsAffected;

    public override object this[int index] => this.GetValue(index);

    public override object this[string name] => this.GetValue(this.GetOrdinal(name));

    public override bool HasRows
    {
      get
      {
        if (this.isClosed)
          throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (HasRows)));
        if (!this.isBaseTableCursor && (this.cursorCapabilities & ResultSetOptions.Scrollable) == ResultSetOptions.None)
          throw new InvalidOperationException(Res.GetString("SQLCE_HasRowsScrollabilityRequirement"));
        if (IntPtr.Zero == this.pSeCursor)
          return false;
        bool hasRows = this.Move(DIRECTION.MOVE_FIRST);
        this.Move(DIRECTION.MOVE_PREVIOUS);
        return hasRows;
      }
    }

    public int HiddenFieldCount
    {
      get
      {
        if (this.isClosed)
          throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) "FieldCount"));
        return 0;
      }
    }

    internal SqlCeDataReader()
    {
      NativeMethods.LoadNativeBinaries();
      int errorInstance = NativeMethods.CreateErrorInstance(ref this.pError);
      if (errorInstance == 0)
        return;
      this.ProcessResults(errorInstance);
    }

    internal SqlCeDataReader(SqlCeConnection connection, SqlCeCommand command)
      : this()
    {
      this.InitializeReader(connection, command);
    }

    internal void InitializeReader(SqlCeConnection connection, SqlCeCommand command)
    {
      this.connection = connection;
      this.isHostControlled = false;
      this.currAccessor = -1;
      this.recordsAffected = -1;
      this.lastBlobColumn = -1;
      this.fieldCount = 0;
      this.useIndex = false;
      this.isOnRow = false;
      this.isBaseTableCursor = false;
      this.cursorPosition = CursorPosition.Undefined;
      this.isFinalized = false;
      if (command != null)
      {
        this.transaction = command.InternalTransaction;
        if (command.indexColOrdinals != null)
          this.indexColumnCount = command.indexColOrdinals.Length;
        this.cursorCapabilities = command.cursorCapabilities;
        this.fetchDirection = FETCH.UNKNOWN;
        this.behavior = command.behavior;
        if (command.IndexName != null && command.IndexName.Length > 0)
          this.useIndex = true;
      }
      else
        this.behavior = CommandBehavior.SingleRow;
      this.sequentialAccess = CommandBehavior.Default != (CommandBehavior.SequentialAccess & this.behavior);
    }

    public new void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~SqlCeDataReader() => this.Dispose(false);

    private void ReleaseNativeInterfaces()
    {
      if (IntPtr.Zero != this.pSeCursor)
        NativeMethods.SafeRelease(ref this.pSeCursor);
      if (!(IntPtr.Zero != this.pError))
        return;
      NativeMethods.SafeDelete(ref this.pError);
    }

    private new void Dispose(bool disposing)
    {
      lock (this.thisLock)
      {
        if (this.isFinalized)
          return;
        if (disposing)
        {
          if (this.metadata != null)
          {
            foreach (MetaData metaData in this.metadata)
              metaData.Dispose();
          }
          if (this.connection != null)
          {
            this.connection.RemoveWeakReference((object) this);
            this.isClosed = true;
          }
          if (this.accessorArray != null)
          {
            int length = this.accessorArray.Length;
            for (int index = 0; index < length; ++index)
            {
              if (this.accessorArray[index] != null)
              {
                this.accessorArray[index].Dispose();
                this.accessorArray[index] = (Accessor) null;
              }
            }
            this.accessorArray = (Accessor[]) null;
          }
          this.transaction = (SqlCeTransaction) null;
          this.isClosed = true;
          this.isOnRow = false;
          this.isDisposed = true;
          this.bindingIndex = (int[]) null;
          this.accessorIndex = (int[]) null;
          this.dbSchemaTable = (DataTable) null;
          this.accessorArray = (Accessor[]) null;
          this.metadata = (MetaData[]) null;
          this.fieldNameLookup = (FieldNameLookup) null;
          this.currAccessor = -1;
          this.sequentialOrdinal = 0;
          this.sequentialAccess = false;
          this.cursorPosition = CursorPosition.Undefined;
          this.fieldCount = -1;
        }
        if (!this.isHostControlled)
          this.ReleaseNativeInterfaces();
        if ((CommandBehavior.CloseConnection & this.behavior) != CommandBehavior.Default && this.connection != null)
          this.connection.Close();
        this.connection = (SqlCeConnection) null;
        if (disposing)
          return;
        this.isFinalized = true;
      }
    }

    public override void Close() => this.Dispose();

    public override DataTable GetSchemaTable()
    {
      if (this.dbSchemaTable == null)
      {
        if (0 < this.fieldCount)
        {
          this.BuildSchemaTable();
        }
        else
        {
          if (this.isClosed)
            throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (GetSchemaTable)));
          if (this.fieldCount < 0)
            return (DataTable) null;
        }
      }
      return this.dbSchemaTable;
    }

    public unsafe bool Seek(DbSeekOptions dbSeekOptions, params object[] index)
    {
      if (index == null)
        throw new ArgumentException(nameof (index));
      if (this.indexColumnCount == 0)
        throw new InvalidOperationException(Res.GetString("SQLCE_IndexNameNotSpecified"));
      if (index.Length > this.indexColumnCount)
        throw new InvalidOperationException(Res.GetString("SQLCE_TooManyParameters"));
      Accessor accessor = this.accessorArray[this.accessorArray.Length - 1];
      for (int index1 = 0; index1 < index.Length; ++index1)
      {
        accessor.CurrentIndex = index1;
        accessor.Value = index[index1];
      }
      fixed (MEDBBINDING* prgBinding = accessor.DbBinding)
      {
        int hr = NativeMethods.Seek(this.pSeCursor, this.connection.IQPServices, (IntPtr) (void*) prgBinding, accessor.Count, accessor.DataHandle, index.Length, (int) dbSeekOptions, this.pError);
        if (!NativeMethods.Failed(hr))
        {
          if (this.isOnRow)
            this.ReleaseCurrentRow();
          this.cursorPosition = CursorPosition.OnRow;
          return true;
        }
        if (-2147217895 != hr)
          this.ProcessResults(hr);
        return false;
      }
    }

    internal unsafe void SetRange(
      DbRangeOptions dbRangeOptions,
      object[] startData,
      object[] endData)
    {
      if (this.indexColumnCount == 0)
        throw new InvalidOperationException(Res.GetString("SQLCE_IndexNameNotSpecified"));
      if (startData != null && startData.Length > this.indexColumnCount || endData != null && endData.Length > this.indexColumnCount)
        throw new InvalidOperationException(Res.GetString("SQLCE_TooManyParameters"));
      IntPtr pStartData = IntPtr.Zero;
      IntPtr pEndData = IntPtr.Zero;
      int cStartKeyValues = 0;
      int cEndKeyValues = 0;
      Accessor accessor = this.accessorArray[this.accessorArray.Length - 1];
      if (startData != null)
      {
        cStartKeyValues = startData.Length;
        for (int index = 0; index < startData.Length; ++index)
        {
          accessor.CurrentIndex = index;
          accessor.Value = startData[index];
        }
      }
      if (endData != null)
      {
        cEndKeyValues = endData.Length;
        for (int index = 0; index < endData.Length; ++index)
        {
          accessor.CurrentIndex = index + accessor.Count;
          accessor.Value = endData[index];
        }
      }
      IntPtr dataHandle = accessor.DataHandle;
      if (startData != null)
        pStartData = dataHandle;
      if (endData != null)
        pEndData = ADP.IntPtrOffset(dataHandle, accessor.DataBufferSize);
      fixed (MEDBBINDING* prgBinding = accessor.DbBinding)
      {
        int hr = NativeMethods.SetRange(this.pSeCursor, this.connection.IQPServices, (IntPtr) (void*) prgBinding, accessor.Count, pStartData, cStartKeyValues, pEndData, cEndKeyValues, (int) dbRangeOptions, this.pError);
        if (hr != 0)
          this.ProcessResults(hr);
      }
    }

    internal void BuildSchemaTable()
    {
      if (this.metadata == null)
        return;
      DataTable dataTable = new DataTable("SchemaTable");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.MinimumCapacity = this.fieldCount;
      DataColumn column1 = new DataColumn("ColumnName", typeof (string));
      DataColumn column2 = new DataColumn("ColumnOrdinal", typeof (int));
      DataColumn column3 = new DataColumn("ColumnSize", typeof (int));
      DataColumn column4 = new DataColumn("NumericPrecision", typeof (short));
      DataColumn column5 = new DataColumn("NumericScale", typeof (short));
      DataColumn column6 = new DataColumn("IsUnique", typeof (bool));
      DataColumn column7 = new DataColumn("IsKey", typeof (bool));
      DataColumn column8 = new DataColumn("BaseColumnName", typeof (string));
      DataColumn column9 = new DataColumn("BaseTableName", typeof (string));
      DataColumn column10 = new DataColumn("DataType", typeof (Type));
      DataColumn column11 = new DataColumn("AllowDBNull", typeof (bool));
      DataColumn column12 = new DataColumn("ProviderType", typeof (SqlCeType));
      DataColumn column13 = new DataColumn("IsAutoIncrement", typeof (bool));
      DataColumn column14 = new DataColumn("IsRowVersion", typeof (bool));
      DataColumn column15 = new DataColumn("IsLong", typeof (bool));
      DataColumn column16 = new DataColumn("IsReadOnly", typeof (bool));
      DataColumn column17 = new DataColumn("IsAliased", typeof (bool));
      DataColumn column18 = new DataColumn("IsExpression", typeof (bool));
      DataColumn column19 = new DataColumn("IsIdentity", typeof (bool));
      column2.DefaultValue = (object) 0;
      column15.DefaultValue = (object) false;
      column4.DefaultValue = (object) null;
      column5.DefaultValue = (object) null;
      column6.DefaultValue = (object) null;
      column7.DefaultValue = (object) null;
      DataColumnCollection columns = dataTable.Columns;
      columns.Add(column1);
      columns.Add(column2);
      columns.Add(column3);
      columns.Add(column4);
      columns.Add(column5);
      columns.Add(column6);
      columns.Add(column7);
      columns.Add(column8);
      columns.Add(column9);
      columns.Add(column10);
      columns.Add(column11);
      columns.Add(column12);
      columns.Add(column17);
      columns.Add(column18);
      columns.Add(column19);
      columns.Add(column13);
      columns.Add(column14);
      columns.Add(column15);
      columns.Add(column16);
      bool flag = false;
      DataRow dataRow1 = (DataRow) null;
      DataRow dataRow2 = (DataRow) null;
      for (int index = 0; index < this.fieldCount; ++index)
      {
        MetaData info = this.metadata[index];
        DataRow row = dataTable.NewRow();
        row[column1] = (object) info.ColumnName;
        row[column2] = (object) info.ordinal;
        row[column3] = typeof (string) != info.typeMap.clrType ? (!info.typeMap.isBLOB ? (object) info.size : (object) SqlCeType.MAX_IMAGE_COLUMN_SIZE) : (!info.typeMap.isBLOB ? (object) (info.size / 2U) : (object) SqlCeType.MAX_NTEXT_COLUMN_SIZE);
        if (SETYPE.NUMERIC == info.typeMap.seType || SETYPE.MONEY == info.typeMap.seType)
        {
          row[column4] = (object) info.SqlMetaData.Precision;
          row[column5] = (object) info.SqlMetaData.Scale;
        }
        row[column10] = (object) info.typeMap.clrType;
        row[column15] = (object) info.typeMap.isBLOB;
        row[column16] = (object) info.isReadOnly;
        row[column14] = (object) info.isRowVersion;
        if ((this.behavior & CommandBehavior.KeyInfo) != CommandBehavior.Default)
        {
          if (info.isRowVersion && dataRow2 == null)
            dataRow2 = row;
          row[column6] = (object) info.isUnique;
          if (info.isUnique && dataRow1 == null)
            dataRow1 = row;
          row[column7] = (object) info.isKey;
          flag |= info.isKey;
        }
        row[column13] = (object) info.isIdentity;
        row[column12] = (object) info.typeMap;
        row[column17] = (object) SqlCeDataReader.IsAliased(info);
        row[column18] = (object) info.isExpression;
        row[column19] = (object) info.isIdentity;
        row[column11] = (object) info.isNullable;
        if (info.baseTableName != null)
          row[column9] = (object) info.baseTableName;
        if (info.baseColumnName != null)
          row[column8] = (object) info.baseColumnName;
        dataTable.Rows.Add(row);
        row.AcceptChanges();
      }
      if ((this.behavior & CommandBehavior.KeyInfo) != CommandBehavior.Default && !flag)
      {
        if (dataRow1 != null)
          dataRow1[column7] = (object) true;
        else if (dataRow2 != null)
          dataRow2[column7] = (object) true;
      }
      for (int index = 0; index < columns.Count; ++index)
        columns[index].ReadOnly = true;
      this.dbSchemaTable = dataTable;
    }

    private static bool IsAliased(MetaData info)
    {
      return info.ColumnName != null && info.baseColumnName != null && !info.ColumnName.Equals(info.baseColumnName);
    }

    protected bool IsCommandBehavior(CommandBehavior condition)
    {
      return condition == (condition & this.behavior);
    }

    internal unsafe void FillMetaData(SqlCeCommand command)
    {
      if (this.metadata != null)
        return;
      if ((this.behavior & CommandBehavior.SchemaOnly) == CommandBehavior.Default)
        this.FillMetaData(this.pSeCursor);
      else if (CommandType.TableDirect == command.CommandType)
      {
        this.FillMetaData(this.pSeCursor);
        NativeMethods.SafeRelease(ref this.pSeCursor);
      }
      else
      {
        this.FillMetaData(command.IQPPlan);
        if (this.metadata == null)
          return;
      }
      string str = (string) null;
      for (int index = 0; index < this.fieldCount; ++index)
      {
        MetaData metaData = this.metadata[index];
        if (metaData.baseTableName != null && 0 < metaData.baseTableName.Length)
        {
          if (str == null)
            str = metaData.baseTableName;
          else if (ADP.SrcCompare(str, metaData.baseTableName) != 0)
          {
            str = (string) null;
            break;
          }
        }
      }
      if (str == null)
        return;
      MEDBKEYINFO[] medbkeyinfoArray = new MEDBKEYINFO[this.fieldCount];
      try
      {
        for (int index = 0; index < this.fieldCount; ++index)
        {
          string baseColumnName = this.metadata[index].baseColumnName;
          medbkeyinfoArray[index].pwszBaseColumnName = baseColumnName.Length == 0 ? IntPtr.Zero : NativeMethods.MarshalStringToLPWSTR(baseColumnName);
        }
        fixed (MEDBKEYINFO* medbkeyinfoPtr = medbkeyinfoArray)
        {
          IntPtr prgDbKeyInfo = (IntPtr) (void*) medbkeyinfoPtr;
          int keyInfo = NativeMethods.GetKeyInfo(this.pSeCursor, command.ITransact, str, prgDbKeyInfo, this.fieldCount, this.pError);
          if (keyInfo != 0)
            this.ProcessResults(keyInfo);
          for (int index = 0; index < this.fieldCount; ++index)
          {
            if (this.metadata[index].baseColumnName.Length != 0)
            {
              this.metadata[index].isKey = medbkeyinfoArray[index].isKey != 0;
              this.metadata[index].isUnique = medbkeyinfoArray[index].isUnique != 0;
              this.metadata[index].isReadOnly = medbkeyinfoArray[index].isReadOnly != 0;
              this.metadata[index].isIdentity = medbkeyinfoArray[index].isIdentity != 0;
            }
          }
        }
      }
      finally
      {
        for (int index = 0; index < medbkeyinfoArray.Length; ++index)
          NativeMethods.CoTaskMemFree(medbkeyinfoArray[index].pwszBaseColumnName);
      }
    }

    private void FillMetaData(IntPtr pIUnknown)
    {
      int columnCount = 0;
      IntPtr zero = IntPtr.Zero;
      try
      {
        int columnInfo = NativeMethods.GetColumnInfo(pIUnknown, ref columnCount, ref zero, this.pError);
        if (NativeMethods.Failed(columnInfo))
        {
          int lMinor = 0;
          NativeMethods.GetMinorError(this.pError, ref lMinor);
          if (lMinor != 0)
          {
            if (28527 != lMinor)
              this.ProcessResults(columnInfo);
          }
          else if (-2147467259 != columnInfo)
            this.ProcessResults(columnInfo);
          NativeMethods.ClearErrorInfo(this.pError);
        }
        if (0 >= columnCount)
          return;
        this.PopulateMetaData(columnCount, zero);
      }
      finally
      {
        NativeMethods.DeleteArray(ref zero);
      }
    }

    private void PopulateMetaData(int columnCount, IntPtr prgColumnInfo)
    {
      int length = 0;
      int num1 = 1;
      MetaData[] metaDataArray1 = new MetaData[columnCount];
      SECOLUMNINFO structure = new SECOLUMNINFO();
      int num2 = Marshal.SizeOf(typeof (SECOLUMNINFO));
      int size = IntPtr.Size;
      int num3 = 0;
      int offset = 0;
      while (num3 < columnCount)
      {
        Marshal.PtrToStructure(ADP.IntPtrOffset(prgColumnInfo, offset), (object) structure);
        try
        {
          string name = Marshal.PtrToStringUni(structure.pwszColumn);
          string stringUni = Marshal.PtrToStringUni(structure.pwszBaseTable);
          if (name.StartsWith("#", StringComparison.InvariantCulture))
          {
            string str = name.Substring(1);
            try
            {
              Convert.ToInt32(str, (IFormatProvider) CultureInfo.CurrentCulture);
              name = "Column" + (object) num1++;
            }
            catch (FormatException ex)
            {
            }
            catch (OverflowException ex)
            {
            }
          }
          SqlCeType typeMap = SqlCeType.FromSeType(structure.type);
          MetaData metaData;
          if (metaDataArray1[length] != null)
          {
            metaData = metaDataArray1[length];
          }
          else
          {
            string databaseName = this.connection != null ? this.connection.Database : string.Empty;
            int maxLength = -1;
            if (!typeMap.isBLOB)
              maxLength = typeof (string) != typeMap.clrType ? (int) structure.ulSize : (int) structure.ulSize / 2;
            metaData = new MetaData(name, typeMap, (byte) structure.bPrecision, (byte) structure.bScale, (long) maxLength, databaseName, stringUni);
          }
          metaData.ordinal = (uint) num3;
          metaData.size = structure.ulSize;
          metaData.isReadOnly = structure.fIsWriteable != 1;
          metaData.isRowVersion = structure.fIsRowVersion == 1;
          metaData.isIdentity = structure.fIsIdentity == 1;
          metaData.isExpression = structure.fIsExpression == 1;
          metaData.isNullable = structure.fIsNullable == 1;
          metaData.hasDefault = structure.fHasDefault == 1;
          metaData.baseTableName = stringUni;
          metaData.baseColumnName = Marshal.PtrToStringUni(structure.pwszBaseColumn);
          metaData.value = (object) null;
          metaData.isUnique = false;
          metaData.isKey = false;
          metaData.typeMap = typeMap;
          metaDataArray1[length] = metaData;
          ++length;
        }
        finally
        {
          NativeMethods.SafeDelete(ref structure.pwszColumn);
          NativeMethods.SafeDelete(ref structure.pwszBaseTable);
          NativeMethods.SafeDelete(ref structure.pwszBaseColumn);
        }
        ++num3;
        offset += num2;
      }
      if (length < columnCount)
      {
        MetaData[] metaDataArray2 = new MetaData[length];
        for (int index = 0; index < length; ++index)
          metaDataArray2[index] = metaDataArray1[index];
        metaDataArray1 = metaDataArray2;
      }
      this.fieldCount = length;
      this.metadata = metaDataArray1;
    }

    private unsafe Accessor FetchValue(int index)
    {
      MetaData metaData1 = this.metadata[index];
      int index1 = this.accessorIndex[index];
      Accessor accessor = (Accessor) null;
      if (this.currAccessor != index1)
      {
        this.currAccessor = index1;
        if (-1 != this.lastBlobColumn)
        {
          MetaData metaData2 = this.metadata[this.lastBlobColumn];
          if (metaData2.value != null)
          {
            IntPtr ppUnknown = (IntPtr) metaData2.value;
            NativeMethods.SafeRelease(ref ppUnknown);
            int currentIndex = this.accessorArray[index1].CurrentIndex;
            this.accessorArray[index1].CurrentIndex = this.bindingIndex[this.lastBlobColumn];
            this.accessorArray[index1].CurrentIndex = currentIndex;
            metaData2.value = (object) null;
          }
        }
        accessor = this.accessorArray[index1];
        int values;
        fixed (MEDBBINDING* prgBinding = accessor.DbBinding)
          values = NativeMethods.GetValues(this.pSeCursor, 0, (IntPtr) (void*) prgBinding, accessor.Count, accessor.DataHandle, this.pError);
        if (values != 0)
          this.ProcessResults(values);
      }
      if (accessor == null)
        accessor = this.accessorArray[index1];
      accessor.CurrentIndex = this.bindingIndex[index];
      metaData1.value = accessor.Value;
      if (!metaData1.typeMap.isFixed)
        metaData1.size = (uint) accessor.SizeValue;
      if (metaData1.typeMap.isBLOB && !(metaData1.value is DBNull))
        this.lastBlobColumn = index;
      return accessor;
    }

    internal void ProcessResults(int hr)
    {
      Exception exception = (Exception) null;
      if (this.connection != null)
        exception = (Exception) this.connection.ProcessResults(hr, this.pError, (object) this);
      if (exception != null)
        throw exception;
    }

    public override bool NextResult() => false;

    protected virtual void OnMove()
    {
    }

    public override bool Read()
    {
      if (IntPtr.Zero != this.pSeCursor)
      {
        if (CursorPosition.AfterLast == this.cursorPosition)
          return false;
        bool flag = this.Move(DIRECTION.MOVE_NEXT);
        this.OnMove();
        if ((CommandBehavior.SingleRow & this.behavior) != CommandBehavior.Default)
          this.cursorPosition = CursorPosition.AfterLast;
        return flag;
      }
      if (this.isClosed)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderClosed", (object) nameof (Read)));
      return false;
    }

    internal bool Move(DIRECTION direction)
    {
      if (this.isOnRow)
        this.ReleaseCurrentRow();
      this.sequentialOrdinal = -1;
      this.sequentialUnitsRead = 0L;
      if (CursorPosition.AfterLast == this.cursorPosition && direction == DIRECTION.MOVE_NEXT || CursorPosition.BeforeFirst == this.cursorPosition && DIRECTION.MOVE_PREVIOUS == direction)
        return false;
      if (CursorPosition.OnRow != this.cursorPosition || DIRECTION.MOVE_LAST == direction || DIRECTION.MOVE_FIRST == direction)
      {
        if (this.isBaseTableCursor)
        {
          if (this.IsEndOfRowset(NativeMethods.Move(this.pSeCursor, direction, this.pError)))
            return false;
          this.cursorPosition = CursorPosition.OnRow;
          this.isOnRow = true;
          return true;
        }
        FETCH fetch = FETCH.UNKNOWN;
        switch (direction)
        {
          case DIRECTION.MOVE_NEXT:
          case DIRECTION.MOVE_FIRST:
          case DIRECTION.MOVE_LAST:
            fetch = FETCH.FORWARD;
            break;
          case DIRECTION.MOVE_PREVIOUS:
            fetch = FETCH.BACKWARD;
            break;
        }
        if (this.IsEndOfRowset(this.Move(this.pSeCursor, direction)))
        {
          if (FETCH.FORWARD == fetch)
          {
            this.fetchDirection = FETCH.BACKWARD;
            this.cursorPosition = CursorPosition.AfterLast;
          }
          else
          {
            this.fetchDirection = FETCH.FORWARD;
            this.cursorPosition = CursorPosition.BeforeFirst;
          }
          return false;
        }
        if (this.fetchDirection != fetch)
        {
          this.fetchDirection = fetch;
          if (this.IsEndOfRowset(this.Move(this.pSeCursor, direction)))
          {
            if (FETCH.FORWARD == fetch)
            {
              this.fetchDirection = FETCH.BACKWARD;
              this.cursorPosition = CursorPosition.AfterLast;
            }
            else
            {
              this.fetchDirection = FETCH.FORWARD;
              this.cursorPosition = CursorPosition.BeforeFirst;
            }
            return false;
          }
        }
      }
      this.cursorPosition = CursorPosition.OnRow;
      this.isOnRow = true;
      return true;
    }

    private int Move(IntPtr pSeCursor, DIRECTION direction)
    {
      int num = NativeMethods.Move(this.pSeCursor, direction, this.pError);
      if (-2147217906 == num || -2147217885 == num)
      {
        if (DIRECTION.MOVE_FIRST == direction)
          direction = DIRECTION.MOVE_NEXT;
        else if (DIRECTION.MOVE_LAST == direction)
          direction = DIRECTION.MOVE_PREVIOUS;
      }
      while (-2147217906 == num || -2147217885 == num)
        num = NativeMethods.Move(this.pSeCursor, direction, this.pError);
      return num;
    }

    private bool IsEndOfRowset(int hr)
    {
      if (NativeMethods.Failed(hr))
      {
        int lMinor = 0;
        NativeMethods.GetMinorError(this.pError, ref lMinor);
        if (25001 == lMinor)
        {
          NativeMethods.ClearErrorInfo(this.pError);
          return true;
        }
        this.ProcessResults(hr);
      }
      return false;
    }

    internal void ReleaseCurrentRow()
    {
      if (this.cursorPosition == CursorPosition.Undefined)
        return;
      this.currAccessor = -1;
      this.lastBlobColumn = -1;
      for (int index = 0; index < this.fieldCount; ++index)
      {
        MetaData metaData = this.metadata[index];
        if (metaData.value != null && metaData.value is IntPtr && metaData.typeMap.isBLOB)
        {
          IntPtr ppUnknown = (IntPtr) metaData.value;
          NativeMethods.SafeRelease(ref ppUnknown);
        }
        metaData.value = (object) null;
      }
      this.isOnRow = false;
      this.cursorPosition = CursorPosition.Undefined;
    }

    internal void CreateAccessors(
      SqlCeCommand command,
      int columnCount,
      bool allowMultipleAccessor)
    {
      this.CreateBindingsFromMetaData(command, columnCount, allowMultipleAccessor);
      for (int index = 0; index < this.accessorArray.Length; ++index)
      {
        if (this.accessorArray[index] != null)
          this.accessorArray[index].AllocData();
      }
    }

    private void CreateBindingsFromMetaData(
      SqlCeCommand command,
      int columnCount,
      bool allowMultipleAccessor)
    {
      this.accessorIndex = new int[columnCount];
      this.bindingIndex = new int[columnCount];
      int num1 = 0;
      int length = 1;
      int num2 = 0;
      if (allowMultipleAccessor)
      {
        for (int index = 0; index < columnCount; ++index)
        {
          if (this.metadata[index].typeMap.isBLOB)
          {
            this.accessorIndex[index] = length++;
            this.bindingIndex[index] = 0;
          }
          else
          {
            this.accessorIndex[index] = 0;
            this.bindingIndex[index] = num1++;
          }
        }
      }
      else
      {
        for (int index = 0; index < columnCount; ++index)
        {
          this.accessorIndex[index] = 0;
          this.bindingIndex[index] = num1++;
        }
        num2 = 1;
      }
      if (this.useIndex)
        ++length;
      this.accessorArray = new Accessor[length];
      for (int index1 = 0; index1 < columnCount; ++index1)
      {
        int index2 = this.accessorIndex[index1];
        Accessor accessor = this.accessorArray[index2];
        if (accessor == null)
        {
          int count = 1;
          if (index2 == 0 && num1 > 0)
            count = num1;
          this.accessorArray[index2] = accessor = new Accessor(count);
        }
        MetaData metaData = this.metadata[index1];
        int num3 = this.bindingIndex[index1];
        accessor.CurrentIndex = num3;
        accessor.Ordinal = (int) metaData.ordinal;
        if (metaData.typeMap.isBLOB)
        {
          tagDBOBJECT structure = new tagDBOBJECT();
          structure.dwFlags = 0;
          structure.iid = !this.sequentialAccess ? ADP.IID_ILockBytes : ADP.IID_ISequentialStream;
          IntPtr ptr = NativeMethods.CoTaskMemAlloc(ADP.SizeOf_tagDBOBJECT);
          if (IntPtr.Zero == ptr)
            throw new OutOfMemoryException();
          Marshal.StructureToPtr((object) structure, ptr, false);
          accessor.ObjectPtr = ptr;
        }
        int num4 = metaData.typeMap.fixlen;
        if (metaData.typeMap.isBLOB)
          num4 = IntPtr.Size;
        else if (-1 == num4)
          num4 = typeof (string) != metaData.typeMap.clrType ? (int) metaData.size : (int) metaData.size + 2;
        accessor.MaxLen = num4;
        accessor.SeType = metaData.typeMap.seType;
        accessor.Precision = metaData.SqlMetaData.Precision;
        accessor.Scale = metaData.SqlMetaData.Scale;
      }
      if (!this.useIndex)
        return;
      int index3 = length - 1;
      MetaData metaData1 = (MetaData) null;
      Accessor accessor1;
      this.accessorArray[index3] = accessor1 = new Accessor(this.indexColumnCount);
      for (int index4 = 0; index4 < this.indexColumnCount; ++index4)
      {
        for (int index5 = 0; index5 < this.metadata.Length; ++index5)
        {
          if ((int) this.metadata[index5].ordinal == (int) command.indexColOrdinals[index4])
          {
            metaData1 = this.metadata[index5];
            break;
          }
        }
        int num5 = metaData1.typeMap.fixlen;
        SETYPE seType = metaData1.typeMap.seType;
        if (-1 == num5)
          num5 = typeof (string) != metaData1.typeMap.clrType ? (int) metaData1.size : (int) metaData1.size + 2;
        accessor1.CurrentIndex = index4;
        accessor1.Ordinal = (int) metaData1.ordinal;
        accessor1.MaxLen = num5;
        accessor1.SeType = seType;
        accessor1.Precision = metaData1.SqlMetaData.Precision;
        accessor1.Scale = metaData1.SqlMetaData.Scale;
      }
      accessor1.NumValues = accessor1.Count * 2;
      accessor1.ActualDataBufferSize = accessor1.DataBufferSize * 2;
    }

    public override bool GetBoolean(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (bool) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override Decimal GetDecimal(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      if (metaData.value == null)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      return metaData.value is SqlDecimal ? (Decimal) (SqlDecimal) metaData.value : (Decimal) metaData.value;
    }

    public override byte GetByte(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (byte) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    private void DoSequentialCheck(int ordinal, long dataIndex, string method)
    {
      if (this.sequentialOrdinal != ordinal)
      {
        this.sequentialOrdinal = ordinal;
        this.sequentialUnitsRead = 0L;
        if (0L != dataIndex)
          throw new InvalidOperationException(Res.GetString("ADP_NonSeqByteAccess", (object) dataIndex.ToString((IFormatProvider) CultureInfo.CurrentCulture), (object) this.sequentialUnitsRead.ToString((IFormatProvider) CultureInfo.CurrentCulture), (object) method));
      }
      else if (this.sequentialUnitsRead != dataIndex)
        throw new InvalidOperationException(Res.GetString("ADP_NonSeqByteAccess", (object) dataIndex.ToString((IFormatProvider) CultureInfo.CurrentCulture), (object) this.sequentialUnitsRead.ToString((IFormatProvider) CultureInfo.CurrentCulture), (object) method));
    }

    public override long GetBytes(
      int ordinal,
      long dataIndex,
      byte[] buffer,
      int bufferIndex,
      int length)
    {
      MetaData info = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      if (IntPtr.Zero == this.pSeCursor)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      if (info.typeMap.clrType != typeof (byte[]))
        throw new InvalidCastException(Res.GetString("SQLCE_InvalidBaseColumnType", (object) nameof (GetBytes), (object) info.typeMap.SqlDbType.ToString()));
      if (buffer == null)
        return (long) info.size;
      if (this.sequentialAccess)
        this.DoSequentialCheck(ordinal, dataIndex, "SqlCeDataReader:GetBytes");
      if ((long) (int) dataIndex > (long) info.size || info.size == 0U)
        return 0;
      if (dataIndex < 0L)
        throw new ArgumentOutOfRangeException(Res.GetString("ADP_InvalidSourceBufferIndex", (object) buffer.Length, (object) dataIndex));
      if (bufferIndex < 0 || bufferIndex >= buffer.Length)
        throw new ArgumentOutOfRangeException(Res.GetString("ADP_InvalidDestinationBufferIndex", (object) buffer.Length, (object) bufferIndex));
      int num1 = buffer.Length - bufferIndex;
      int num2 = (int) ((long) info.size - dataIndex);
      if (length > num1 && num2 > num1)
        throw new ArgumentOutOfRangeException(Res.GetString("SQLCE_TooManyBytesAvailable", (object) num2, (object) num1));
      int bytes1 = Math.Min(buffer.Length - bufferIndex, length);
      if (bytes1 < 0)
        throw new IndexOutOfRangeException(Res.GetString("SQL_InvalidDataLength", (object) length.ToString((IFormatProvider) CultureInfo.CurrentCulture)));
      if (dataIndex > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(Res.GetString("ADP_InvalidSourceBufferIndex", (object) bytes1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) dataIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (!info.typeMap.isBLOB)
      {
        byte[] src = (byte[]) info.value;
        if ((long) src.Length - dataIndex < (long) bytes1)
          bytes1 = src.Length - (int) dataIndex;
        Buffer.BlockCopy((Array) src, (int) dataIndex, (Array) buffer, bufferIndex, bytes1);
        this.sequentialUnitsRead = dataIndex + (long) bytes1;
        return (long) bytes1;
      }
      int bytes2 = this.GetBytes(info, buffer, (int) dataIndex, bufferIndex, bytes1);
      if (bytes2 <= 0)
        return 0;
      this.sequentialUnitsRead = (long) (bufferIndex + bytes2);
      return (long) bytes2;
    }

    public override long GetChars(
      int ordinal,
      long dataIndex,
      char[] buffer,
      int bufferIndex,
      int length)
    {
      MetaData info = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      if (IntPtr.Zero == this.pSeCursor)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      if (info.typeMap.clrType != typeof (string))
        throw new InvalidCastException(Res.GetString("SQLCE_InvalidBaseColumnType", (object) nameof (GetChars), (object) info.typeMap.SqlDbType.ToString()));
      if (buffer == null)
        return (long) (info.size / 2U);
      if (this.sequentialAccess)
        this.DoSequentialCheck(ordinal, dataIndex, nameof (GetChars));
      if ((long) (int) dataIndex > (long) (info.size / 2U) || info.size == 0U)
        return 0;
      if (dataIndex < 0L)
        throw new ArgumentOutOfRangeException(Res.GetString("ADP_InvalidSourceBufferIndex", (object) buffer.Length, (object) dataIndex));
      if (bufferIndex < 0 || bufferIndex >= buffer.Length)
        throw new ArgumentOutOfRangeException(Res.GetString("ADP_InvalidDestinationBufferIndex", (object) buffer.Length, (object) bufferIndex));
      int num1 = buffer.Length - bufferIndex;
      int num2 = (int) ((long) (info.size / 2U) - dataIndex);
      if (length > num1 && num2 > num1)
        throw new ArgumentOutOfRangeException(Res.GetString("SQLCE_TooManyCharsAvailable", (object) num2, (object) num1));
      int chars1 = Math.Min(buffer.Length - bufferIndex, length);
      if (dataIndex > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(Res.GetString("ADP_InvalidSourceBufferIndex", (object) chars1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) dataIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (chars1 < 0)
        throw new IndexOutOfRangeException(Res.GetString("SQL_InvalidDataLength", (object) length.ToString((IFormatProvider) CultureInfo.CurrentCulture)));
      if (!info.typeMap.isBLOB)
      {
        string str = (string) info.value;
        if ((long) str.Length - dataIndex < (long) length)
          chars1 = str.Length - (int) dataIndex;
        str.CopyTo((int) dataIndex, buffer, bufferIndex, chars1);
        this.sequentialUnitsRead = dataIndex + (long) chars1;
        return (long) chars1;
      }
      int chars2 = this.GetChars(info, buffer, (int) dataIndex, bufferIndex, chars1);
      if (chars2 <= 0)
        return 0;
      this.sequentialUnitsRead = (long) (bufferIndex + chars2);
      return (long) chars2;
    }

    public override string GetDataTypeName(int index)
    {
      if (this.metadata != null)
        return this.metadata[index].typeMap.ToString();
      throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override DateTime GetDateTime(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (DateTime) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override double GetDouble(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (double) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override Type GetFieldType(int ordinal)
    {
      if (this.metadata != null)
        return this.metadata[ordinal].typeMap.clrType;
      throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override Type GetProviderSpecificFieldType(int ordinal)
    {
      if (this.metadata != null)
        return SqlCeType.SqlDbType2SqlType(this.metadata[ordinal].typeMap.sqlDbType);
      throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override float GetFloat(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (float) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override Guid GetGuid(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (Guid) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override short GetInt16(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (short) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override int GetInt32(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      if (metaData.value is int)
        return (int) metaData.value;
      if (metaData.value is uint)
        return Convert.ToInt32((object) (uint) metaData.value, (IFormatProvider) CultureInfo.CurrentCulture);
      if (metaData.value != null)
        throw new InvalidCastException();
      throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override long GetInt64(int ordinal)
    {
      MetaData metaData = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      return metaData.value != null ? (long) metaData.value : throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override string GetName(int index)
    {
      if (this.metadata != null)
        return this.metadata[index].ColumnName;
      throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
    }

    public override int GetOrdinal(string name)
    {
      if (this.fieldNameLookup == null)
      {
        if (this.metadata == null)
          throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
        this.fieldNameLookup = new FieldNameLookup((IDataReader) this, -1);
      }
      return this.fieldNameLookup.GetOrdinal(name);
    }

    public override string GetString(int ordinal)
    {
      MetaData info = !this.IsDBNull(ordinal) ? this.metadata[ordinal] : throw new SqlNullValueException(Res.GetString("SQLCE_ColumnContainsDbNullValue", (object) ordinal));
      if (!(IntPtr.Zero != this.pSeCursor))
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      return info.typeMap.isBLOB ? (string) this.GetLongValue(info, ordinal) : (string) info.value;
    }

    public override object GetValue(int ordinal)
    {
      if (this.IsDBNull(ordinal))
        return (object) DBNull.Value;
      MetaData info = this.metadata[ordinal];
      if (info.typeMap.isBLOB)
        return this.GetLongValue(info, ordinal);
      return info.value is SqlDecimal ? (object) (Decimal) (SqlDecimal) info.value : info.value;
    }

    public override int GetValues(object[] values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      if (!this.isOnRow || IntPtr.Zero == this.pSeCursor)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      int values1 = Math.Min(values.Length, this.FieldCount);
      if (!(IntPtr.Zero != this.pSeCursor))
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      for (int index = 0; index < values1; ++index)
      {
        MetaData info = this.metadata[index];
        if (info.value == null)
          this.FetchValue(index);
        values[index] = !info.typeMap.isBLOB ? (!(info.value is SqlDecimal) ? info.value : (object) (Decimal) (SqlDecimal) info.value) : this.GetLongValue(info, index);
      }
      return values1;
    }

    private object GetLongValue(MetaData info, int ordinal)
    {
      if (this.IsDBNull(ordinal))
        return (object) DBNull.Value;
      if (typeof (byte[]) == info.typeMap.clrType)
      {
        byte[] buffer = new byte[(IntPtr) info.size];
        this.GetBytes(info, buffer, 0, 0, (int) info.size);
        return (object) buffer;
      }
      int cch = (int) info.size / 2;
      char[] buffer1 = new char[cch];
      this.GetChars(info, buffer1, 0, 0, cch);
      return (object) new string(buffer1);
    }

    private unsafe int GetBytes(
      MetaData info,
      byte[] buffer,
      int dataIndex,
      int bufferIndex,
      int cb)
    {
      int bytesRead = 0;
      if (buffer.Length == 0)
        return 0;
      int hr;
      fixed (byte* pBuffer = buffer)
        hr = this.sequentialAccess ? NativeMethods.Read((IntPtr) info.value, (void*) pBuffer, bufferIndex, cb, out bytesRead, this.pError) : NativeMethods.ReadAt((IntPtr) info.value, dataIndex, (void*) pBuffer, bufferIndex, cb, out bytesRead, this.pError);
      if (hr != 0)
        this.ProcessResults(hr);
      return bytesRead;
    }

    private unsafe int GetChars(
      MetaData info,
      char[] buffer,
      int dataIndex,
      int bufferIndex,
      int cch)
    {
      int bytesRead = 0;
      if (buffer.Length == 0)
        return 0;
      int hr;
      fixed (char* pBuffer = buffer)
        hr = this.sequentialAccess ? NativeMethods.Read((IntPtr) info.value, (void*) pBuffer, bufferIndex * 2, cch * 2, out bytesRead, this.pError) : NativeMethods.ReadAt((IntPtr) info.value, dataIndex * 2, (void*) pBuffer, bufferIndex * 2, cch * 2, out bytesRead, this.pError);
      if (hr != 0)
        this.ProcessResults(hr);
      return bytesRead / 2;
    }

    public virtual SqlBoolean GetSqlBoolean(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlBoolean.Null : new SqlBoolean(this.GetBoolean(ordinal));
    }

    public virtual SqlMoney GetSqlMoney(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlMoney.Null : new SqlMoney(this.GetDecimal(ordinal));
    }

    public virtual SqlDecimal GetSqlDecimal(int ordinal)
    {
      if (this.IsDBNull(ordinal))
        return SqlDecimal.Null;
      MetaData metaData = this.metadata[ordinal];
      if (metaData.value == null)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      return metaData.value is SqlDecimal ? (SqlDecimal) metaData.value : new SqlDecimal((Decimal) metaData.value);
    }

    public virtual SqlByte GetSqlByte(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlByte.Null : new SqlByte(this.GetByte(ordinal));
    }

    public virtual SqlBinary GetSqlBinary(int ordinal)
    {
      if (this.IsDBNull(ordinal))
        return SqlBinary.Null;
      int bytes = (int) this.GetBytes(ordinal, 0L, (byte[]) null, 0, 0);
      byte[] buffer = new byte[bytes];
      if (bytes > 0)
        this.GetBytes(ordinal, 0L, buffer, 0, bytes);
      return new SqlBinary(buffer);
    }

    public virtual SqlDateTime GetSqlDateTime(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlDateTime.Null : new SqlDateTime(this.GetDateTime(ordinal));
    }

    public virtual SqlDouble GetSqlDouble(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlDouble.Null : new SqlDouble(this.GetDouble(ordinal));
    }

    public virtual SqlSingle GetSqlSingle(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlSingle.Null : new SqlSingle(this.GetFloat(ordinal));
    }

    public virtual SqlGuid GetSqlGuid(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlGuid.Null : new SqlGuid(this.GetGuid(ordinal));
    }

    public virtual SqlInt16 GetSqlInt16(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlInt16.Null : new SqlInt16(this.GetInt16(ordinal));
    }

    public virtual SqlInt32 GetSqlInt32(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlInt32.Null : new SqlInt32(this.GetInt32(ordinal));
    }

    public virtual SqlInt64 GetSqlInt64(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlInt64.Null : new SqlInt64(this.GetInt64(ordinal));
    }

    public virtual SqlString GetSqlString(int ordinal)
    {
      return this.IsDBNull(ordinal) ? SqlString.Null : new SqlString(this.GetString(ordinal));
    }

    public override bool IsDBNull(int ordinal)
    {
      if (!this.isOnRow || IntPtr.Zero == this.pSeCursor)
        throw new InvalidOperationException(Res.GetString("ADP_DataReaderNoData"));
      if (this.sequentialAccess && ordinal != this.sequentialOrdinal)
      {
        this.sequentialOrdinal = ordinal >= this.sequentialOrdinal ? ordinal : throw new InvalidOperationException(Res.GetString("ADP_NonSequentialColumnAccess", (object) ordinal.ToString((IFormatProvider) CultureInfo.CurrentCulture), (object) this.sequentialOrdinal.ToString((IFormatProvider) CultureInfo.CurrentCulture)));
        this.sequentialUnitsRead = 0L;
      }
      MetaData metaData = this.metadata[ordinal];
      if (metaData.value == null && IntPtr.Zero != this.pSeCursor)
        this.FetchValue(ordinal);
      return metaData.value is DBNull;
    }

    public override IEnumerator GetEnumerator()
    {
      return (IEnumerator) new DbEnumerator((IDataReader) this, this.IsCommandBehavior(CommandBehavior.CloseConnection));
    }

    public override char GetChar(int ordinal)
    {
      throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, nameof (GetChar)));
    }
  }
}
