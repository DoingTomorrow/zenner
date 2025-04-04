// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.ADP
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class ADP
  {
    internal const int MaxConnectionStringCacheSize = 250;
    internal const string BeginTransaction = "BeginTransaction";
    internal const string CommandSetEnlistedConnection = "set_Connection";
    internal const string GetDatabaseInfo = "GetDatabaseInfo";
    internal const string ChangeDatabase = "ChangeDatabase";
    internal const string Cancel = "Cancel";
    internal const string Clone = "Clone";
    internal const string CommitTransaction = "CommitTransaction";
    internal const string ConnectionString = "ConnectionString";
    internal const string DataSetColumn = "DataSetColumn";
    internal const string DataSetTable = "DataSetTable";
    internal const string Delete = "Delete";
    internal const string DeleteCommand = "DeleteCommand";
    internal const string DeriveParameters = "DeriveParameters";
    internal const string ExecuteReader = "ExecuteReader";
    internal const string ExecuteNonQuery = "ExecuteNonQuery";
    internal const string ExecuteTableDirect = "ExecuteTableDirect";
    internal const string ExecuteScalar = "ExecuteScalar";
    internal const string ExecuteXmlReader = "ExecuteXmlReader";
    internal const string ExecuteResultSet = "ExecuteResultSet";
    internal const string Fill = "Fill";
    internal const string FillSchema = "FillSchema";
    internal const string GetBytes = "GetBytes";
    internal const string GetChars = "GetChars";
    internal const string Insert = "Insert";
    internal const string Parameter = "Parameter";
    internal const string ParameterName = "ParameterName";
    internal const string ParameterIndex = "ParameterIndex";
    internal const string Prepare = "Prepare";
    internal const string Remove = "Remove";
    internal const string RollbackTransaction = "RollbackTransaction";
    internal const string SaveTransaction = "SaveTransaction";
    internal const string Select = "Select";
    internal const string SelectCommand = "SelectCommand";
    internal const string SourceColumn = "SourceColumn";
    internal const string SourceVersion = "SourceVersion";
    internal const string SourceTable = "SourceTable";
    internal const string Update = "Update";
    internal const string UpdateCommand = "UpdateCommand";
    internal const string sysTableTombstone = "__sysOCSDeletedRows";
    internal const string sysContextColumn = "__sysTrackingContext";
    internal const string sysTableCommitSequence = "__sysTxCommitSequence";
    internal const string sysTableTrackedObjects = "__sysOCSTrackedObjects";
    internal const int DecimalScaleOfMoney = 4;
    internal const string sysTableSyncArticles = "__sysSyncArticles";
    internal const string sysTableSyncSubscriptions = "__sysSyncSubscriptions";
    internal const int DB_E_BADBOOKMARK = -2147217906;
    internal const int DB_E_DELETEDROW = -2147217885;
    internal const int DB_E_NOTFOUND = -2147217895;
    internal const int SSCE_M_LOCKTIMEOUT = 25090;
    internal const int SSCE_M_PENDINGUPDATE = 25126;
    internal const int SSCE_M_DBUPGRADENEEDED = 25138;
    internal const int SSCE_M_NOCURRENTRECORD = 25001;
    internal const int SSCE_M_COLUMNORDINALNOTFOUND = 28527;
    internal const int SSCE_M_MANAGEDEXCEPTION = 27772;
    internal const int SSCE_M_TRANSACTIONABORTED = 27776;
    internal const int SSCE_M_TABLENOTTRACKED = 28543;
    internal const int SQLCE_WrongTrackingVersion = 30001;
    internal const int SQLCE_WrongCleanupSequence = 30002;
    internal const int MAX_PARAMETER_NAME_LENGTH = 128;
    internal const int ExecutedQuery = 1;
    internal const int ExecutedNonQuery = 2;
    internal const int SchemaOnly = 3;
    internal const short VARIANT_TRUE = -1;
    internal const short VARIANT_FALSE = 0;
    internal const int TRUE = 1;
    internal const int FALSE = 0;
    internal const int DBACCESSOR_ROWDATA = 2;
    internal const int DBACCESSOR_PARAMETERDATA = 4;
    internal const int S_OK = 0;
    internal const int SESETCOLUMN_DEFAULT = 2;
    internal const int E_FAIL = -2147467259;
    internal const CompareOptions compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
    private static readonly Type StackOverflowType = typeof (StackOverflowException);
    private static readonly Type OutOfMemoryType = typeof (OutOfMemoryException);
    private static readonly Type ThreadAbortType = typeof (ThreadAbortException);
    private static readonly Type NullReferenceType = typeof (NullReferenceException);
    internal static readonly object EventRowUpdated = new object();
    internal static readonly object EventRowUpdating = new object();
    internal static readonly object EventInfoMessage = new object();
    internal static readonly object EventStateChange = new object();
    internal static readonly object EventFlushFailure = new object();
    internal static readonly int SizeOf_tagDBOBJECT = Marshal.SizeOf(typeof (tagDBOBJECT));
    internal static readonly Guid IID_ISequentialStream = new Guid(208878128, (short) 10780, (short) 4558, (byte) 173, (byte) 229, (byte) 0, (byte) 170, (byte) 0, (byte) 68, (byte) 119, (byte) 61);
    internal static readonly Guid IID_ILockBytes = new Guid(10, (short) 0, (short) 0, (byte) 192, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 70);

    private ADP()
    {
    }

    internal static IntPtr IntPtrOffset(IntPtr pbase, int offset)
    {
      return 4 == IntPtr.Size ? (IntPtr) (pbase.ToInt32() + offset) : (IntPtr) (pbase.ToInt64() + (long) offset);
    }

    internal static int SrcCompare(string strA, string strB)
    {
      return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
    }

    internal static bool IsEmpty(string str) => str == null || 0 == str.Length;

    internal static bool IsAlive(WeakReference value)
    {
      try
      {
        if (value != null)
        {
          if (value.IsAlive)
            return true;
        }
      }
      catch (Exception ex)
      {
        if (!ADP.IsCatchableExceptionType(ex))
          throw ex;
      }
      return false;
    }

    internal static bool IsCatchableExceptionType(Exception e)
    {
      Type type = e.GetType();
      return type != ADP.StackOverflowType && type != ADP.OutOfMemoryType && type != ADP.ThreadAbortType && type != ADP.NullReferenceType;
    }

    internal static List<string> GetTablePrimaryKey(string tableName, SqlCeConnection Connection)
    {
      if (Connection == null)
        throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
      List<string> tablePrimaryKey = new List<string>();
      SqlCeCommand command = Connection.CreateCommand();
      command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.INDEXES  WHERE PRIMARY_KEY=1 AND TABLE_NAME = @tablename ORDER BY ORDINAL_POSITION ASC";
      command.Parameters.AddWithValue("@tablename", (object) tableName);
      SqlCeDataReader sqlCeDataReader = command.ExecuteReader();
      while (sqlCeDataReader.Read())
        tablePrimaryKey.Add(sqlCeDataReader.GetString(0));
      sqlCeDataReader.Close();
      sqlCeDataReader.Dispose();
      command.Parameters.Clear();
      command.Dispose();
      return tablePrimaryKey;
    }

    internal static string GetTableRowGuidColumn(string tableName, SqlCeConnection Connection)
    {
      if (Connection == null)
        throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
      string tableRowGuidColumn = (string) null;
      SqlCeCommand command = Connection.CreateCommand();
      command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = @tablename and (COLUMN_FLAGS & 0x100) <> 0";
      command.Parameters.AddWithValue("@tablename", (object) tableName);
      SqlCeDataReader sqlCeDataReader = command.ExecuteReader();
      try
      {
        if (sqlCeDataReader.Read())
          tableRowGuidColumn = sqlCeDataReader.GetString(0);
      }
      catch (Exception ex)
      {
        tableRowGuidColumn = (string) null;
      }
      finally
      {
        sqlCeDataReader.Close();
        sqlCeDataReader.Dispose();
        command.Parameters.Clear();
        command.Dispose();
      }
      return tableRowGuidColumn;
    }

    internal static SqlCeTableColumns GetTableColumns(string tableName, SqlCeConnection Connection)
    {
      if (Connection == null)
        throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
      SqlCeTableColumns tableColumns = new SqlCeTableColumns(tableName);
      SqlCeDataReader sqlCeDataReader = (SqlCeDataReader) null;
      SqlCeCommand command = Connection.CreateCommand();
      try
      {
        command.CommandText = " SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH,  COLUMN_DEFAULT, NUMERIC_PRECISION, NUMERIC_SCALE, ORDINAL_POSITION  FROM INFORMATION_SCHEMA.COLUMNS  WHERE COLUMN_NAME NOT LIKE '__sys%' AND TABLE_NAME = @tablename ORDER BY ORDINAL_POSITION ASC";
        command.Parameters.AddWithValue("@tablename", (object) tableName);
        sqlCeDataReader = command.ExecuteReader();
        object[] values = new object[sqlCeDataReader.FieldCount];
        while (sqlCeDataReader.Read())
        {
          sqlCeDataReader.GetValues(values);
          SqlCeInfoSchemaColumn column = new SqlCeInfoSchemaColumn()
          {
            ColumnName = (string) values[0]
          };
          column.ParamName = "@_" + column.ColumnName;
          column.SqlCeType = SqlCeType.FromDataType((string) values[1]);
          if (values[2] != null && DBNull.Value != values[2])
            column.MaxLength = (int) values[2];
          column.DefaultValue = values[3];
          if (values[4] != null && DBNull.Value != values[4])
            column.Precision = Convert.ToByte(values[4], (IFormatProvider) CultureInfo.InvariantCulture);
          if (values[5] != null && DBNull.Value != values[5])
            column.Scale = Convert.ToByte(values[5], (IFormatProvider) CultureInfo.InvariantCulture);
          if (values[6] != null && DBNull.Value != values[6])
            column.Ordinal = (int) values[6];
          tableColumns.Add(column);
        }
      }
      finally
      {
        if (sqlCeDataReader != null)
        {
          sqlCeDataReader.Close();
          sqlCeDataReader.Dispose();
        }
        command.Parameters.Clear();
        command.Dispose();
      }
      return tableColumns;
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    internal static object BitConverterGetObject(
      SqlCeType ceType,
      byte[] fullBytes,
      int offset,
      int length)
    {
      object obj = (object) DBNull.Value;
      switch (ceType.SeType)
      {
        case SETYPE.TINYINT:
          obj = (object) fullBytes[offset];
          break;
        case SETYPE.SMALLINT:
          obj = (object) BitConverter.ToInt16(fullBytes, offset);
          break;
        case SETYPE.INTEGER:
          obj = (object) BitConverter.ToInt32(fullBytes, offset);
          break;
        case SETYPE.BIGINT:
          obj = (object) BitConverter.ToInt64(fullBytes, offset);
          break;
        case SETYPE.NCHAR:
        case SETYPE.NVARCHAR:
          if (length == 0)
          {
            obj = (object) string.Empty;
            break;
          }
          int bytesUsed = 0;
          int charsUsed = 0;
          bool completed = false;
          char[] chars = new char[Encoding.Unicode.GetMaxCharCount(length)];
          Encoding.Unicode.GetDecoder().Convert(fullBytes, offset, length, chars, 0, chars.Length, true, out bytesUsed, out charsUsed, out completed);
          obj = (object) new string(chars, 0, charsUsed);
          break;
        case SETYPE.BINARY:
        case SETYPE.VARBINARY:
        case SETYPE.ROWVERSION:
          byte[] dst = new byte[length];
          if (length > 0)
            Buffer.BlockCopy((Array) fullBytes, offset, (Array) dst, 0, length);
          obj = (object) dst;
          break;
        case SETYPE.DATETIME:
          DBTIMESTAMP pDbTimestamp = new DBTIMESTAMP();
          uint uint32 = BitConverter.ToUInt32(fullBytes, offset);
          int int32_1 = BitConverter.ToInt32(fullBytes, offset + 4);
          NativeMethods.uwutil_ConvertToDBTIMESTAMP(ref pDbTimestamp, uint32, int32_1);
          obj = (object) new DateTime((int) pDbTimestamp.year, (int) pDbTimestamp.month, (int) pDbTimestamp.day, (int) pDbTimestamp.hour, (int) pDbTimestamp.minute, (int) pDbTimestamp.second, (int) (pDbTimestamp.fraction / 1000000U));
          break;
        case SETYPE.UNIQUEIDENTIFIER:
          byte[] numArray = new byte[length];
          Buffer.BlockCopy((Array) fullBytes, offset, (Array) numArray, 0, length);
          obj = (object) new Guid(numArray);
          break;
        case SETYPE.BIT:
          obj = (object) BitConverter.ToBoolean(fullBytes, offset);
          break;
        case SETYPE.REAL:
          obj = (object) BitConverter.ToSingle(fullBytes, offset);
          break;
        case SETYPE.FLOAT:
          obj = (object) BitConverter.ToDouble(fullBytes, offset);
          break;
        case SETYPE.MONEY:
          int int32_2 = BitConverter.ToInt32(fullBytes, offset);
          int int32_3 = BitConverter.ToInt32(fullBytes, offset + 4);
          obj = (object) new Decimal(int32_2, int32_3, 0, int32_3 < 0, (byte) 4);
          break;
        case SETYPE.NUMERIC:
          byte fullByte = fullBytes[offset + 1];
          bool boolean = Convert.ToBoolean(fullBytes[offset + 2]);
          obj = (object) new Decimal(BitConverter.ToInt32(fullBytes, offset + 3), BitConverter.ToInt32(fullBytes, offset + 7), BitConverter.ToInt32(fullBytes, offset + 11), !boolean, fullByte);
          break;
      }
      return obj;
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    internal static byte[] BitConverterGetBytes(SqlCeType ceType, object val, ref int length)
    {
      switch (ceType.SeType)
      {
        case SETYPE.TINYINT:
          length = 1;
          return new byte[1]{ (byte) val };
        case SETYPE.SMALLINT:
          length = 2;
          return BitConverter.GetBytes((short) val);
        case SETYPE.INTEGER:
          length = 4;
          return BitConverter.GetBytes((int) val);
        case SETYPE.BIGINT:
          length = 8;
          return BitConverter.GetBytes((long) val);
        case SETYPE.NCHAR:
        case SETYPE.NVARCHAR:
          int bytesUsed = 0;
          int charsUsed = 0;
          bool completed = false;
          char[] charArray = ((string) ValueMapper.GetMappedValue(SqlDbType.NChar, val)).ToCharArray();
          byte[] numArray = new byte[Encoding.Unicode.GetMaxByteCount(charArray.Length)];
          Encoding.Unicode.GetEncoder().Convert(charArray, 0, charArray.Length, numArray, 0, numArray.Length, true, out charsUsed, out bytesUsed, out completed);
          length = bytesUsed;
          byte[] destinationArray = new byte[length];
          Array.Copy((Array) numArray, (Array) destinationArray, length);
          return destinationArray;
        case SETYPE.BINARY:
        case SETYPE.VARBINARY:
        case SETYPE.ROWVERSION:
          byte[] src = (byte[]) val;
          byte[] dst = new byte[src.Length];
          length = dst.Length;
          Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, dst.Length);
          return dst;
        case SETYPE.DATETIME:
          DateTime dateTime = (DateTime) val;
          DBTIMESTAMP pDbTimestamp = new DBTIMESTAMP();
          pDbTimestamp.year = (short) dateTime.Year;
          pDbTimestamp.month = (ushort) dateTime.Month;
          pDbTimestamp.day = (ushort) dateTime.Day;
          pDbTimestamp.hour = (ushort) dateTime.Hour;
          pDbTimestamp.minute = (ushort) dateTime.Minute;
          pDbTimestamp.second = (ushort) dateTime.Second;
          pDbTimestamp.fraction = (uint) (dateTime.Millisecond * 1000000);
          length = 8;
          byte[] bytes1 = new byte[8];
          uint dtTime = 0;
          int dtDay = 0;
          NativeMethods.uwutil_ConvertFromDBTIMESTAMP(pDbTimestamp, ref dtTime, ref dtDay);
          BitConverter.GetBytes(dtTime).CopyTo((Array) bytes1, 0);
          BitConverter.GetBytes(dtDay).CopyTo((Array) bytes1, 4);
          return bytes1;
        case SETYPE.UNIQUEIDENTIFIER:
          length = 16;
          return ((Guid) val).ToByteArray();
        case SETYPE.BIT:
          length = 2;
          return new byte[2]
          {
            (bool) val ? (byte) 1 : (byte) 0,
            (byte) 0
          };
        case SETYPE.REAL:
          length = 4;
          return BitConverter.GetBytes((float) val);
        case SETYPE.FLOAT:
          length = 8;
          return BitConverter.GetBytes((double) val);
        case SETYPE.MONEY:
          int[] bits1 = Decimal.GetBits((Decimal) val);
          long num1 = (long) ((bits1[0] + bits1[1]) * 10000);
          byte[] bytes2 = new byte[8];
          BitConverter.GetBytes(num1).CopyTo((Array) bytes2, 0);
          length = 8;
          return bytes2;
        case SETYPE.NUMERIC:
          int[] bits2 = Decimal.GetBits((Decimal) val);
          int num2 = bits2[0];
          int num3 = bits2[1];
          int num4 = bits2[2];
          bool flag = bits2[3] >= 0;
          byte num5 = (byte) (bits2[3] >> 16 & (int) byte.MaxValue);
          length = 19;
          byte[] bytes3 = new byte[19];
          bytes3[0] = (byte) 0;
          bytes3[1] = num5;
          bytes3[2] = Convert.ToByte(flag);
          BitConverter.GetBytes(num2).CopyTo((Array) bytes3, 3);
          BitConverter.GetBytes(num3).CopyTo((Array) bytes3, 7);
          BitConverter.GetBytes(num4).CopyTo((Array) bytes3, 11);
          return bytes3;
        default:
          length = -1;
          return (byte[]) null;
      }
    }

    internal class DstComparer : IEqualityComparer<string>
    {
      public bool Equals(string strA, string strB)
      {
        return 0 == CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
      }

      public int GetHashCode(string str) => str.ToLower(CultureInfo.CurrentCulture).GetHashCode();
    }
  }
}
