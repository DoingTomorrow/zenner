// Decompiled with JetBrains decompiler
// Type: MinolHandler.DatabaseAccess
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace MinolHandler
{
  internal class DatabaseAccess
  {
    private static Logger logger = LogManager.GetLogger(nameof (DatabaseAccess));
    private MinolHandlerFunctions MyFunctions;
    private SortedList<string, DeviceDB_Data> TelegramParameterCacheBySignature = new SortedList<string, DeviceDB_Data>();
    private SortedList<int, DeviceDB_Data> TelegramParameterCacheByMapID = new SortedList<int, DeviceDB_Data>();
    private IDbConnection myPrimaryConnection;

    internal DatabaseAccess(MinolHandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.myPrimaryConnection = DbBasis.PrimaryDB.GetDbConnection();
    }

    internal bool GetTelegramParameters(
      int MapID,
      out string DeviceName,
      out int HardwareTypeID,
      out SortedList<string, List<TelegramParameter>> AllTelegramParameters)
    {
      AllTelegramParameters = (SortedList<string, List<TelegramParameter>>) null;
      DeviceName = string.Empty;
      HardwareTypeID = -1;
      int index1 = this.TelegramParameterCacheByMapID.IndexOfKey(MapID);
      if (index1 >= 0)
      {
        AllTelegramParameters = this.TelegramParameterCacheByMapID.Values[index1].Parameter;
        DeviceName = this.TelegramParameterCacheByMapID.Values[index1].Name;
        HardwareTypeID = this.TelegramParameterCacheByMapID.Values[index1].HardwareTypeID;
        return true;
      }
      string empty = string.Empty;
      AllTelegramParameters = new SortedList<string, List<TelegramParameter>>();
      try
      {
        DatabaseAccess.logger.Trace("Load hardware type");
        string str1 = "SELECT HardwareResource, HardwareTypeID FROM HardwareType WHERE MapID = " + MapID.ToString();
        IDbCommand dbCommand = DbBasis.PrimaryDB.DbCommand(this.myPrimaryConnection);
        if (this.myPrimaryConnection.State == ConnectionState.Closed)
          dbCommand.Connection.Open();
        dbCommand.CommandText = str1;
        IDataReader dataReader1 = dbCommand.ExecuteReader();
        if (!dataReader1.Read())
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Unknown hardware type!");
          return false;
        }
        DeviceName = Convert.ToString(dataReader1["HardwareResource"]);
        HardwareTypeID = Convert.ToInt32(dataReader1[nameof (HardwareTypeID)]);
        dataReader1.Close();
        if (DatabaseAccess.logger.IsTraceEnabled)
          DatabaseAccess.logger.Trace<string, int>("Done! DeviceName: {0}, HardwareTypeID: {1}", DeviceName, HardwareTypeID);
        DatabaseAccess.logger.Trace("Load memory map");
        string str2 = string.Format("SELECT RangeName, Name, Type, Parent, RD_Type, Address, BitMask, \r\n                                                Description, Length, RD_Data, RD_Factor, RD_Divisor, ParamType, UseK, \r\n                                                UseMulDiv, ValueIdent, OverrideID\r\n                                         FROM   MinolDeviceData \r\n                                         WHERE  MapID={0} \r\n                                         ORDER BY [Index];", (object) MapID);
        dbCommand.CommandText = str2;
        IDataReader dataReader2 = dbCommand.ExecuteReader();
        int num = 0;
        while (dataReader2.Read())
        {
          ++num;
          empty = dataReader2["Name"].ToString();
          string key = dataReader2["RangeName"].ToString();
          string str3 = dataReader2["Type"].ToString();
          if (!key.StartsWith("EEPROM") && !(str3 == "SYS") && !(str3 == "READING"))
          {
            TelegramParameter telegramParameter = new TelegramParameter();
            telegramParameter.Name = empty;
            telegramParameter.Type = str3;
            if (dataReader2["Parent"] != DBNull.Value)
              telegramParameter.Parent = Convert.ToString(dataReader2["Parent"]);
            if (dataReader2["RD_Data"] != DBNull.Value)
              telegramParameter.RD_Data = Convert.ToString(dataReader2["RD_Data"]);
            if (dataReader2["RD_Type"] != DBNull.Value)
              telegramParameter.RD_Type = Convert.ToString(dataReader2["RD_Type"]);
            if (dataReader2["Address"] != DBNull.Value)
              telegramParameter.Address = Convert.ToInt32(dataReader2["Address"]);
            if (dataReader2["Description"] != DBNull.Value)
              telegramParameter.Description = Convert.ToString(dataReader2["Description"]);
            telegramParameter.BitMask = dataReader2["BitMask"] == DBNull.Value ? 1 : Convert.ToInt32(dataReader2["BitMask"]);
            if (dataReader2["Length"] != DBNull.Value)
              telegramParameter.ByteLength = Convert.ToInt32(dataReader2["Length"]);
            telegramParameter.RD_Factor = dataReader2["RD_Factor"] == DBNull.Value ? 1 : Convert.ToInt32(dataReader2["RD_Factor"]);
            telegramParameter.RD_Divisor = dataReader2["RD_Divisor"] == DBNull.Value ? 1 : Convert.ToInt32(dataReader2["RD_Divisor"]);
            telegramParameter.UseK = dataReader2["UseK"] != DBNull.Value && Convert.ToBoolean(dataReader2["UseK"]);
            telegramParameter.UseMulDiv = dataReader2["UseMulDiv"] != DBNull.Value && Convert.ToBoolean(dataReader2["UseMulDiv"]);
            telegramParameter.ValueIdent = dataReader2["ValueIdent"] == DBNull.Value ? -1L : Convert.ToInt64(dataReader2["ValueIdent"]);
            telegramParameter.OverrideID = dataReader2["OverrideID"] == DBNull.Value ? -1 : Convert.ToInt32(dataReader2["OverrideID"]);
            if (dataReader2["ParamType"] == DBNull.Value)
            {
              telegramParameter.ParameterType = (Type) null;
            }
            else
            {
              switch (Convert.ToString(dataReader2["ParamType"]))
              {
                case "long":
                  telegramParameter.ParameterType = typeof (long);
                  break;
                case "double":
                  telegramParameter.ParameterType = typeof (double);
                  break;
                case "decimal":
                  telegramParameter.ParameterType = typeof (Decimal);
                  break;
                case "DateTime":
                  telegramParameter.ParameterType = typeof (DateTime);
                  break;
                default:
                  telegramParameter.ParameterType = (Type) null;
                  break;
              }
            }
            int index2 = AllTelegramParameters.IndexOfKey(key);
            if (index2 < 0)
            {
              List<TelegramParameter> telegramParameterList = new List<TelegramParameter>();
              AllTelegramParameters.Add(key, telegramParameterList);
              index2 = AllTelegramParameters.IndexOfKey(key);
            }
            AllTelegramParameters.Values[index2].Add(telegramParameter);
          }
        }
        dataReader2.Close();
        if (num <= 0)
          return false;
        if (DatabaseAccess.logger.IsTraceEnabled)
          DatabaseAccess.logger.Trace("Done! Memory map has {0} rows", num);
        this.TelegramParameterCacheByMapID.Add(MapID, new DeviceDB_Data(HardwareTypeID, MapID, DeviceName, AllTelegramParameters));
        return true;
      }
      catch (Exception ex)
      {
        string str = "Error on load TelegramParameter '" + empty + "' from database " + ex.Message;
        DatabaseAccess.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, str);
        return false;
      }
      finally
      {
        if (this.myPrimaryConnection.State == ConnectionState.Open)
          this.myPrimaryConnection.Close();
      }
    }

    internal bool GetTelegramParameters(
      int Signature_RD_Data,
      int Version,
      int Length,
      out string DeviceName,
      out int HardwareTypeID,
      out int MapID,
      out SortedList<string, List<TelegramParameter>> AllTelegramParameters)
    {
      AllTelegramParameters = (SortedList<string, List<TelegramParameter>>) null;
      DeviceName = string.Empty;
      HardwareTypeID = -1;
      MapID = -1;
      if (Signature_RD_Data == 29380 || Signature_RD_Data == 34142 || Signature_RD_Data == 42209 || Signature_RD_Data == 47047)
        Signature_RD_Data = 58218;
      if (Signature_RD_Data == 64788 || Signature_RD_Data == 20679 || Signature_RD_Data == 27966)
        Signature_RD_Data = 10759;
      if (Signature_RD_Data == 59478)
        Signature_RD_Data = 36398;
      if (Signature_RD_Data == 7781 && Version == 1)
      {
        Signature_RD_Data = 39058;
        Version = 2;
      }
      if (Signature_RD_Data == 38265 && Version == 6)
        Signature_RD_Data = 35849;
      if (Signature_RD_Data == 63717 && Version == 6)
        Signature_RD_Data = 35849;
      if (Signature_RD_Data == 20359 && Version == 6)
        Signature_RD_Data = 35849;
      if (Signature_RD_Data == 361 && Version == 6)
        Signature_RD_Data = 36398;
      if (Signature_RD_Data == 13556 && Version == 6)
        Signature_RD_Data = 36398;
      if (Signature_RD_Data == 40274 && Version == 6)
        Signature_RD_Data = 58218;
      if (Signature_RD_Data == 53957 && Version == 6)
        Signature_RD_Data = 58218;
      string key = Signature_RD_Data.ToString() + "_" + Version.ToString() + "_" + Length.ToString();
      int index = this.TelegramParameterCacheBySignature.IndexOfKey(key);
      if (index >= 0)
      {
        if (DatabaseAccess.logger.IsTraceEnabled)
          DatabaseAccess.logger.Trace<int, int, int>("Get chached memory map. Signatur 0x{0:X4}, version {1}, length {2}", Signature_RD_Data, Version, Length);
        AllTelegramParameters = this.TelegramParameterCacheBySignature.Values[index].Parameter;
        DeviceName = this.TelegramParameterCacheBySignature.Values[index].Name;
        HardwareTypeID = this.TelegramParameterCacheBySignature.Values[index].HardwareTypeID;
        MapID = this.TelegramParameterCacheBySignature.Values[index].MapID;
        return true;
      }
      string empty = string.Empty;
      try
      {
        if (DatabaseAccess.logger.IsTraceEnabled)
          DatabaseAccess.logger.Trace<int, int, int>("Try get MapID from database. Signatur 0x{0:X4}, version {1}, length {2}", Signature_RD_Data, Version, Length);
        string str1;
        if (Signature_RD_Data > 0)
        {
          str1 = "SELECT MapID FROM MinolDeviceData WHERE Signature=" + Signature_RD_Data.ToString();
        }
        else
        {
          switch (Length)
          {
            case 155:
              str1 = "SELECT MapID FROM MinolDeviceData WHERE MapID=27";
              break;
            case 175:
              str1 = "SELECT MapID FROM MinolDeviceData WHERE MapID=28";
              break;
            default:
              str1 = "SELECT MapID FROM MinolDeviceData WHERE RangeName = 'RAM' AND NAME = 'L1' AND RD_Data = '" + Length.ToString() + "'";
              break;
          }
        }
        IDbCommand dbCommand = DbBasis.PrimaryDB.DbCommand(this.myPrimaryConnection);
        if (this.myPrimaryConnection.State == ConnectionState.Closed)
          dbCommand.Connection.Open();
        dbCommand.CommandText = str1;
        object obj = dbCommand.ExecuteScalar();
        if (obj == null || obj == DBNull.Value)
        {
          string str2 = "SELECT MapID FROM MinolDeviceData WHERE RangeName = 'RAM' AND NAME = 'L1' AND RD_Data = '" + Length.ToString() + "'";
          dbCommand.CommandText = str2;
          obj = dbCommand.ExecuteScalar();
          if (obj == null || obj == DBNull.Value)
            return false;
        }
        MapID = Convert.ToInt32(obj);
        DatabaseAccess.logger.Trace("Found MapID={0}", MapID);
        if (!this.GetTelegramParameters(MapID, out DeviceName, out HardwareTypeID, out AllTelegramParameters))
          return false;
        this.TelegramParameterCacheBySignature.Add(key, new DeviceDB_Data(HardwareTypeID, MapID, DeviceName, AllTelegramParameters));
      }
      catch (Exception ex)
      {
        string str = "Error on load TelegramParameter '" + empty + "' from database. " + ex.Message;
        DatabaseAccess.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, str);
        return false;
      }
      finally
      {
        if (this.myPrimaryConnection.State == ConnectionState.Open)
          this.myPrimaryConnection.Close();
      }
      return true;
    }

    internal MinolDevice Load(int MeterId, DateTime TimePoint)
    {
      string empty1 = string.Empty;
      int? nullable1 = new int?();
      int? nullable2 = new int?();
      string empty2 = string.Empty;
      try
      {
        string SqlCommand1 = "SELECT * FROM Meter WHERE MeterID = " + MeterId.ToString();
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand1, this.myPrimaryConnection);
        Schema.MeterDataTable meterDataTable = new Schema.MeterDataTable();
        zrDataAdapter1.Fill((DataTable) meterDataTable);
        if (meterDataTable.Count != 1)
          return (MinolDevice) null;
        string SqlCommand2 = "SELECT * FROM MeterInfo WHERE MeterInfoID = " + new int?(meterDataTable[0].MeterInfoID).ToString();
        ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand2, this.myPrimaryConnection);
        Schema.MeterInfoDataTable meterInfoDataTable = new Schema.MeterInfoDataTable();
        zrDataAdapter2.Fill((DataTable) meterInfoDataTable);
        if (meterInfoDataTable.Count != 1)
          return (MinolDevice) null;
        int? nullable3 = new int?(meterInfoDataTable[0].HardwareTypeID);
        int? nullable4 = nullable3;
        int num = 0;
        if (nullable4.GetValueOrDefault() == num & nullable4.HasValue)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load Meter failed! HardwareTypeID is 0!");
          return (MinolDevice) null;
        }
        string SqlCommand3 = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + nullable3.ToString();
        ZRDataAdapter zrDataAdapter3 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand3, this.myPrimaryConnection);
        Schema.HardwareTypeDataTable hardwareTypeDataTable = new Schema.HardwareTypeDataTable();
        zrDataAdapter3.Fill((DataTable) hardwareTypeDataTable);
        if (hardwareTypeDataTable.Count != 1)
          return (MinolDevice) null;
        string hardwareResource = hardwareTypeDataTable[0].HardwareResource;
        if (string.IsNullOrEmpty(hardwareResource))
          return (MinolDevice) null;
        int mapId = hardwareTypeDataTable[0].MapID;
        if (mapId == 0)
          return (MinolDevice) null;
        IDbCommand DbCommand = DbBasis.PrimaryDB.DbCommand(DbBasis.PrimaryDB.GetDbConnection());
        DbCommand.CommandText = "SELECT * FROM MeterData WHERE MeterID=@MeterID AND TimePoint = @TimePoint;";
        IDbDataParameter parameter1 = DbCommand.CreateParameter();
        parameter1.DbType = DbType.Int32;
        parameter1.ParameterName = "@MeterID";
        parameter1.Value = (object) MeterId;
        DbCommand.Parameters.Add((object) parameter1);
        IDbDataParameter parameter2 = DbCommand.CreateParameter();
        parameter2.DbType = DbType.DateTime;
        parameter2.ParameterName = "@TimePoint";
        parameter2.Value = (object) TimePoint;
        DbCommand.Parameters.Add((object) parameter2);
        ZRDataAdapter zrDataAdapter4 = DbBasis.PrimaryDB.ZRDataAdapter(DbCommand);
        Schema.MeterDataDataTable meterDataDataTable = new Schema.MeterDataDataTable();
        zrDataAdapter4.Fill((DataTable) meterDataDataTable);
        byte[] pvalueBinary = meterDataDataTable[0].PValueBinary;
        MinolDevice minolDevice = this.MyFunctions.MyDevices.CreateMinolDevice(hardwareResource, nullable3.Value, mapId);
        if (minolDevice == null)
          return (MinolDevice) null;
        minolDevice.SetMap(Util.ConvertByteArrayToInt16Array(pvalueBinary));
        minolDevice.MeterID = new int?(MeterId);
        return minolDevice;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load MeterData failed! " + ex.Message);
        return (MinolDevice) null;
      }
      finally
      {
        this.myPrimaryConnection.Close();
      }
    }

    internal List<MinolDevice> Load(int hardwareTypeID) => throw new NotImplementedException();

    internal Dictionary<DateTime, MinolDevice> Load(int hardwareTypeID, string serialNumber)
    {
      string empty1 = string.Empty;
      int? nullable1 = new int?();
      long? nullable2 = new long?();
      string empty2 = string.Empty;
      try
      {
        string SqlCommand1 = string.Format("SELECT * FROM MeterInfo WHERE HardwareTypeID = {0} AND MeterTypeID={1}", (object) hardwareTypeID, (object) 0);
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand1, this.myPrimaryConnection);
        Schema.MeterInfoDataTable meterInfoDataTable = new Schema.MeterInfoDataTable();
        zrDataAdapter1.Fill((DataTable) meterInfoDataTable);
        if (meterInfoDataTable.Count != 1)
          return (Dictionary<DateTime, MinolDevice>) null;
        string SqlCommand2 = string.Format("SELECT * FROM Meter WHERE MeterInfoID={0} AND SerialNr='{1}'", (object) new int?(meterInfoDataTable[0].MeterInfoID), (object) serialNumber);
        ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand2, this.myPrimaryConnection);
        Schema.MeterDataTable meterDataTable = new Schema.MeterDataTable();
        zrDataAdapter2.Fill((DataTable) meterDataTable);
        if (meterDataTable.Count != 1)
          return (Dictionary<DateTime, MinolDevice>) null;
        long? nullable3 = new long?((long) meterDataTable[0].MeterID);
        string SqlCommand3 = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + hardwareTypeID.ToString();
        ZRDataAdapter zrDataAdapter3 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand3, this.myPrimaryConnection);
        Schema.HardwareTypeDataTable hardwareTypeDataTable = new Schema.HardwareTypeDataTable();
        zrDataAdapter3.Fill((DataTable) hardwareTypeDataTable);
        if (hardwareTypeDataTable.Count != 1)
          return (Dictionary<DateTime, MinolDevice>) null;
        string hardwareResource = hardwareTypeDataTable[0].HardwareResource;
        if (string.IsNullOrEmpty(hardwareResource))
          return (Dictionary<DateTime, MinolDevice>) null;
        int mapId = hardwareTypeDataTable[0].MapID;
        if (mapId == 0)
          return (Dictionary<DateTime, MinolDevice>) null;
        string SqlCommand4 = "SELECT * FROM MeterData WHERE MeterID = " + nullable3.ToString();
        ZRDataAdapter zrDataAdapter4 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand4, this.myPrimaryConnection);
        Schema.MeterDataDataTable meterDataDataTable = new Schema.MeterDataDataTable();
        zrDataAdapter4.Fill((DataTable) meterDataDataTable);
        if (meterDataDataTable.Rows.Count == 0)
          return (Dictionary<DateTime, MinolDevice>) null;
        Dictionary<DateTime, MinolDevice> dictionary = new Dictionary<DateTime, MinolDevice>();
        foreach (DataRow row in (InternalDataCollectionBase) meterDataDataTable.Rows)
        {
          DateTime key = (DateTime) row[meterDataDataTable.TimePointColumn];
          byte[] buffer = (byte[]) row[meterDataDataTable.PValueBinaryColumn];
          MinolDevice minolDevice = this.MyFunctions.MyDevices.CreateMinolDevice(hardwareResource, hardwareTypeID, mapId);
          if (minolDevice == null)
            return (Dictionary<DateTime, MinolDevice>) null;
          minolDevice.SetMap(Util.ConvertByteArrayToInt16Array(buffer));
          dictionary.Add(key, minolDevice);
        }
        return dictionary;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load MeterData failed! " + ex.Message);
        return (Dictionary<DateTime, MinolDevice>) null;
      }
      finally
      {
        this.myPrimaryConnection.Close();
      }
    }

    internal MinolDevice LoadType(int meterInfoID)
    {
      if (meterInfoID == 0)
        return (MinolDevice) null;
      string empty1 = string.Empty;
      int? nullable1 = new int?();
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      byte[] buffer = (byte[]) null;
      try
      {
        string SqlCommand1 = "SELECT * FROM MeterInfo WHERE MeterInfoID = " + meterInfoID.ToString();
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand1, this.myPrimaryConnection);
        Schema.MeterInfoDataTable meterInfoDataTable = new Schema.MeterInfoDataTable();
        zrDataAdapter1.Fill((DataTable) meterInfoDataTable);
        if (meterInfoDataTable.Count != 1)
          return (MinolDevice) null;
        int? nullable2 = new int?(meterInfoDataTable[0].HardwareTypeID);
        int? nullable3 = nullable2;
        int num = 0;
        if (nullable3.GetValueOrDefault() == num & nullable3.HasValue)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load MeterType failed! HardwareTypeID is 0!");
          return (MinolDevice) null;
        }
        int meterTypeId = meterInfoDataTable[0].MeterTypeID;
        if (meterTypeId == 0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load MeterType failed! MeterTypeID is 0!");
          return (MinolDevice) null;
        }
        string SqlCommand2 = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + nullable2.ToString();
        ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand2, this.myPrimaryConnection);
        Schema.HardwareTypeDataTable hardwareTypeDataTable = new Schema.HardwareTypeDataTable();
        zrDataAdapter2.Fill((DataTable) hardwareTypeDataTable);
        if (hardwareTypeDataTable.Count != 1)
          return (MinolDevice) null;
        string hardwareResource = hardwareTypeDataTable[0].HardwareResource;
        if (string.IsNullOrEmpty(hardwareResource))
          return (MinolDevice) null;
        int mapId = hardwareTypeDataTable[0].MapID;
        if (mapId == 0)
          return (MinolDevice) null;
        string SqlCommand3 = "SELECT * FROM MeterType WHERE MeterTypeID=" + meterTypeId.ToString();
        ZRDataAdapter zrDataAdapter3 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand3, this.myPrimaryConnection);
        Schema.MeterTypeDataTable meterTypeDataTable = new Schema.MeterTypeDataTable();
        zrDataAdapter3.Fill((DataTable) meterTypeDataTable);
        if (meterTypeDataTable.Count != 1)
          return (MinolDevice) null;
        string mtypeTableName = meterTypeDataTable[0].MTypeTableName;
        if (string.IsNullOrEmpty(mtypeTableName))
          return (MinolDevice) null;
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          IDbCommand dbCommand = DbBasis.PrimaryDB.DbCommand(dbConnection);
          dbCommand.CommandText = "SELECT DeviceMemory FROM " + mtypeTableName + " WHERE MeterTypeId = " + meterTypeId.ToString();
          dbConnection.Open();
          IDataReader dataReader = dbCommand.ExecuteReader();
          if (dataReader.Read() && dataReader["DeviceMemory"] != null && dataReader["DeviceMemory"] != DBNull.Value)
            buffer = (byte[]) dataReader["DeviceMemory"];
        }
        if (buffer == null)
          return (MinolDevice) null;
        MinolDevice minolDevice = this.MyFunctions.MyDevices.CreateMinolDevice(hardwareResource, nullable2.Value, mapId);
        if (minolDevice == null)
          return (MinolDevice) null;
        minolDevice.SetMap(Util.ConvertByteArrayToInt16Array(buffer));
        return minolDevice;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load MeterType failed! " + ex.Message);
        return (MinolDevice) null;
      }
      finally
      {
        this.myPrimaryConnection.Close();
      }
    }

    internal bool Save(MinolDevice device, out DateTime backupTimePoint)
    {
      return this.Save(device, out backupTimePoint, new int?(), string.Empty, string.Empty);
    }

    internal bool Save(
      MinolDevice device,
      out DateTime backupTimePoint,
      int? meterInfoID,
      string OrderNumber,
      string TheSerialNumber)
    {
      string empty = string.Empty;
      backupTimePoint = SystemValues.DateTimeNow;
      backupTimePoint = backupTimePoint.AddMilliseconds((double) (backupTimePoint.Millisecond * -1));
      if (device == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "MinolDevice is null!");
        return false;
      }
      if (device.HardwareTypeID < 1)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "HardwareTypeID is invalid! Value: " + device.HardwareTypeID.ToString());
        return false;
      }
      bool flag = device.HardwareTypeID == 63;
      byte[] byteArray = Util.ConvertInt16ArrayToByteArray(device.Map);
      this.myPrimaryConnection.Open();
      IDbTransaction Transaction = this.myPrimaryConnection.BeginTransaction();
      try
      {
        if (!meterInfoID.HasValue)
        {
          string SqlCommand = "SELECT * FROM MeterInfo WHERE HardwareTypeID = " + device.HardwareTypeID.ToString() + " AND MeterTypeID = 0;";
          ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand, this.myPrimaryConnection);
          Schema.MeterInfoDataTable MyDataTable = new Schema.MeterInfoDataTable();
          zrDataAdapter.Fill((DataTable) MyDataTable, Transaction);
          if (MyDataTable.Count != 1)
            return false;
          meterInfoID = new int?(MyDataTable[0].MeterInfoID);
        }
        if (!device.MeterID.HasValue)
        {
          string SqlCommand = string.Format("SELECT * FROM Meter WHERE MeterInfoID={0} AND SerialNr='{1}'", (object) meterInfoID, (object) device.SerialNumber);
          ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand, this.myPrimaryConnection);
          Schema.MeterDataTable MyDataTable = new Schema.MeterDataTable();
          zrDataAdapter.Fill((DataTable) MyDataTable, Transaction);
          if (MyDataTable.Count == 1 && !flag)
          {
            device.MeterID = new int?(MyDataTable[0].MeterID);
          }
          else
          {
            if (MyDataTable.Count != 0 && !flag)
              return false;
            IDbCommand command = this.myPrimaryConnection.CreateCommand();
            command.Transaction = Transaction;
            device.MeterID = MeterDatabase.GetNextUniqueID(command, "Meter", "MeterID");
            if (!device.MeterID.HasValue)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Generate new MeterID failed!");
              return false;
            }
            Schema.MeterRow row = MyDataTable.NewMeterRow();
            row.MeterID = device.MeterID.Value;
            row.MeterInfoID = meterInfoID.Value;
            row.SerialNr = !(TheSerialNumber != string.Empty) ? TheSerialNumber : device.SerialNumber.ToString();
            row.ProductionDate = backupTimePoint;
            if (OrderNumber.Trim() != string.Empty)
              row.OrderNr = OrderNumber;
            MyDataTable.AddMeterRow(row);
            if (zrDataAdapter.Update((DataTable) MyDataTable, Transaction) != 1 || !MeterDatabase.SetNextUniqueID(command, "Meter", "MeterID", device.MeterID.Value + 1))
              return false;
          }
        }
        else
        {
          string SqlCommand = string.Format("SELECT * FROM Meter WHERE MeterID={0}", (object) device.MeterID.Value);
          ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand, this.myPrimaryConnection);
          Schema.MeterDataTable MyDataTable = new Schema.MeterDataTable();
          zrDataAdapter.Fill((DataTable) MyDataTable, Transaction);
          if (MyDataTable.Count == 0)
          {
            Schema.MeterRow row = MyDataTable.NewMeterRow();
            row.MeterID = device.MeterID.Value;
            row.MeterInfoID = meterInfoID.Value;
            row.SerialNr = !(TheSerialNumber != string.Empty) ? device.SerialNumber.ToString() : TheSerialNumber;
            row.ProductionDate = backupTimePoint;
            if (OrderNumber.Trim() != string.Empty)
              row.OrderNr = OrderNumber;
            MyDataTable.AddMeterRow(row);
            if (zrDataAdapter.Update((DataTable) MyDataTable, Transaction) != 1)
              return false;
          }
          else
          {
            MyDataTable[0].MeterInfoID = meterInfoID.Value;
            MyDataTable[0].SerialNr = !(TheSerialNumber != string.Empty) ? device.SerialNumber.ToString() : TheSerialNumber;
            if (OrderNumber.Trim() != string.Empty)
              MyDataTable[0].OrderNr = OrderNumber;
            if (zrDataAdapter.Update((DataTable) MyDataTable, Transaction) != 1)
              return false;
          }
        }
        string SqlCommand1 = "SELECT * FROM MeterData WHERE MeterID = -1";
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand1, this.myPrimaryConnection);
        Schema.MeterDataDataTable MyDataTable1 = new Schema.MeterDataDataTable();
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        Schema.MeterDataRow row1 = MyDataTable1.NewMeterDataRow();
        row1.MeterID = device.MeterID.Value;
        row1.TimePoint = backupTimePoint;
        row1.PValueID = 1;
        row1.PValueBinary = byteArray;
        MyDataTable1.AddMeterDataRow(row1);
        if (zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction) != 1)
        {
          Transaction.Rollback();
          return false;
        }
        Transaction.Commit();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Save MeterData failed! " + ex.Message);
        return false;
      }
      finally
      {
        this.myPrimaryConnection.Close();
      }
      return true;
    }

    internal bool SaveType(MinolDevice device, int meterTypeID)
    {
      if (device == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "SaveType failed! The MinolDevice is null!");
        return false;
      }
      if (meterTypeID == 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "SaveType failed! The MeterTypeID is invalid! Value: 0");
        return false;
      }
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      try
      {
        string SqlCommand = "SELECT * FROM MeterType WHERE MeterTypeID=" + meterTypeID.ToString();
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand, this.myPrimaryConnection);
        Schema.MeterTypeDataTable meterTypeDataTable = new Schema.MeterTypeDataTable();
        zrDataAdapter.Fill((DataTable) meterTypeDataTable);
        if (meterTypeDataTable.Count != 1)
          return false;
        string mtypeTableName = meterTypeDataTable[0].MTypeTableName;
        if (string.IsNullOrEmpty(mtypeTableName))
          return false;
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          IDbCommand dbCommand = DbBasis.PrimaryDB.DbCommand(dbConnection);
          dbCommand.CommandText = "DELETE FROM " + mtypeTableName + " WHERE MeterTypeId = " + meterTypeID.ToString();
          dbConnection.Open();
          dbCommand.Transaction = dbConnection.BeginTransaction();
          dbCommand.ExecuteNonQuery();
          dbCommand.CommandText = "INSERT INTO " + mtypeTableName + " (MeterTypeId, DeviceMemory) VALUES (@MeterTypeId, @DeviceMemory);";
          IDataParameter parameter1 = (IDataParameter) dbCommand.CreateParameter();
          parameter1.ParameterName = "@MeterTypeId";
          parameter1.DbType = DbType.Int32;
          parameter1.Value = (object) meterTypeID;
          dbCommand.Parameters.Add((object) parameter1);
          IDataParameter parameter2 = (IDataParameter) dbCommand.CreateParameter();
          parameter2.ParameterName = "@DeviceMemory";
          parameter2.DbType = DbType.Binary;
          parameter2.Value = (object) Util.ConvertInt16ArrayToByteArray(device.Map);
          dbCommand.Parameters.Add((object) parameter2);
          if (dbCommand.ExecuteNonQuery() != 1)
            return false;
          dbCommand.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Save MeterType failed! " + ex.Message);
        return false;
      }
      finally
      {
        this.myPrimaryConnection.Close();
      }
    }

    internal void CreateStaticClassOfMinolDeviceDataTable(string path)
    {
      List<int> intList = MinolDeviceData.LoadMapIDOfMinolDeviceData(DbBasis.PrimaryDB.BaseDbConnection);
      if (intList == null)
        throw new NullReferenceException("The list of MapID's con be null");
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.AppendLine("internal static DeviceDB_Data GetMapData(string IdentKey)");
      stringBuilder2.AppendLine("{");
      stringBuilder2.AppendLine("    switch (IdentKey)");
      stringBuilder2.AppendLine("    {");
      foreach (int MapID in intList)
      {
        string DeviceName;
        int HardwareTypeID;
        SortedList<string, List<TelegramParameter>> AllTelegramParameters;
        if (!this.GetTelegramParameters(MapID, out DeviceName, out HardwareTypeID, out AllTelegramParameters))
          throw new Exception("Can not load MAP. MapID: " + MapID.ToString());
        string empty = string.Empty;
        string key;
        switch (MapID)
        {
          case 27:
            key = "0_3_155";
            break;
          case 28:
            key = "0_1_175";
            break;
          default:
            string rdData1 = AllTelegramParameters["RAM"].Find((Predicate<TelegramParameter>) (e => e.Name == "Signature")).RD_Data;
            int result1 = 0;
            Util.TryParse(rdData1, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
            int result2;
            Util.TryParse(AllTelegramParameters["RAM"].Find((Predicate<TelegramParameter>) (e => e.Name == "Version")).RD_Data, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result2);
            string rdData2 = AllTelegramParameters["RAM"].Find((Predicate<TelegramParameter>) (e => e.Name == "L1")).RD_Data;
            int result3;
            if (!Util.TryParse(rdData2, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result3))
              throw new Exception("Invalid Length: " + rdData2);
            key = result1.ToString() + "_" + result2.ToString() + "_" + result3.ToString();
            break;
        }
        if (!this.TelegramParameterCacheBySignature.ContainsKey(key))
          this.TelegramParameterCacheBySignature.Add(key, new DeviceDB_Data(HardwareTypeID, MapID, DeviceName, AllTelegramParameters));
        stringBuilder2.AppendFormat("        case \"{0}\":   return GetMapData{0}();", (object) key);
        stringBuilder2.AppendLine();
        stringBuilder1.AppendFormat("private static DeviceDB_Data GetMapData{0}()", (object) key);
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine("{");
        stringBuilder1.AppendFormat("    var prms{0} = new SortedList<string, List<TelegramParameter>>();", (object) key);
        stringBuilder1.AppendLine();
        foreach (KeyValuePair<string, List<TelegramParameter>> keyValuePair in AllTelegramParameters)
        {
          stringBuilder1.AppendFormat("    var items{0}{1} = new List<TelegramParameter>() ", (object) key, (object) keyValuePair.Key);
          stringBuilder1.AppendLine();
          stringBuilder1.Append("    {");
          stringBuilder1.AppendLine();
          foreach (TelegramParameter telegramParameter in keyValuePair.Value)
          {
            stringBuilder1.Append("        new TelegramParameter() {");
            stringBuilder1.AppendFormat("Address={0},BitMask={1},ByteLength={2},Name=\"{3}\",OverrideID={4},ParameterType={5},Parent=\"{6}\",RD_Data=\"{7}\",RD_Divisor={8},RD_Factor={9},RD_Type=\"{10}\",Type=\"{11}\",UseK={12},UseMulDiv={13},ValueIdent={14}", (object) telegramParameter.Address, (object) telegramParameter.BitMask, (object) telegramParameter.ByteLength, (object) telegramParameter.Name, (object) telegramParameter.OverrideID, telegramParameter.ParameterType == (Type) null ? (object) "null" : (object) ("typeof(" + telegramParameter.ParameterType?.ToString() + ")"), (object) telegramParameter.Parent, (object) telegramParameter.RD_Data, (object) telegramParameter.RD_Divisor, (object) telegramParameter.RD_Factor, (object) telegramParameter.RD_Type, (object) telegramParameter.Type, (object) telegramParameter.UseK.ToString().ToLower(), (object) telegramParameter.UseMulDiv.ToString().ToLower(), (object) telegramParameter.ValueIdent);
            stringBuilder1.AppendLine("},");
          }
          stringBuilder1.Append("    };");
          stringBuilder1.AppendLine();
          stringBuilder1.AppendFormat("    prms{0}.Add(\"{1}\", items{0}{1});", (object) key, (object) keyValuePair.Key);
          stringBuilder1.AppendLine();
        }
        stringBuilder1.AppendFormat("    return new DeviceDB_Data({1}, {2}, \"{3}\", prms{0});", (object) key, (object) HardwareTypeID, (object) MapID, (object) DeviceName);
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine("}");
        stringBuilder1.AppendLine();
      }
      stringBuilder2.AppendLine("        default:");
      stringBuilder2.AppendLine("            throw new NotSupportedException(IdentKey);");
      stringBuilder2.AppendLine("    }");
      stringBuilder2.AppendLine("}");
      stringBuilder2.AppendLine();
      stringBuilder1.Insert(0, stringBuilder2.ToString());
      File.WriteAllText(path, stringBuilder1.ToString());
    }
  }
}
