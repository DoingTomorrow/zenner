// Decompiled with JetBrains decompiler
// Type: PDC_Handler.PDC_Database
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public static class PDC_Database
  {
    private static Logger logger = LogManager.GetLogger(nameof (PDC_Database));
    private const int METER_HARDWARE_ID_PDC = 102;

    public static List<Parameter> LoadParameter(DeviceVersion version)
    {
      return PDC_Database.LoadParameter(DbBasis.PrimaryDB, version);
    }

    public static List<Parameter> LoadParameter(DbBasis db, DeviceVersion version)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT DISTINCT MapID FROM HardwareType WHERE FirmwareVersion=@FirmwareVersion;";
          MeterDatabase.AddParameter(cmd, "@FirmwareVersion", (double) version.Version);
          object obj = cmd.ExecuteScalar();
          if (obj == null || obj == DBNull.Value)
            return (List<Parameter>) null;
          int int32 = Convert.ToInt32(obj);
          cmd.CommandText = "SELECT p.ParameterName, m.cValue, p.DefaultDivVif, p.VariableType, p.MaxValue FROM MapDef AS m, S3_Parameter AS p \r\n\t\t\t\t\t\t\t\t\t\tWHERE m.MapID=@MapID AND p.ParameterName=m.ConstName ORDER BY m.cValue ASC;";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@MapID", int32);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<Parameter>) null;
          List<Parameter> parameterList = new List<Parameter>();
          while (dataReader.Read())
          {
            string str = dataReader["ParameterName"].ToString();
            S3_VariableTypes s3VariableTypes = (S3_VariableTypes) Enum.ToObject(typeof (S3_VariableTypes), dataReader["VariableType"]);
            int num;
            switch (s3VariableTypes)
            {
              case S3_VariableTypes.INT8:
              case S3_VariableTypes.UINT8:
                num = 1;
                break;
              case S3_VariableTypes.UINT16:
              case S3_VariableTypes.INT16:
                num = 2;
                break;
              case S3_VariableTypes.UINT32:
              case S3_VariableTypes.INT32:
              case S3_VariableTypes.REAL32:
              case S3_VariableTypes.MeterTime1980:
                num = 4;
                break;
              case S3_VariableTypes.INT64:
              case S3_VariableTypes.UINT64:
              case S3_VariableTypes.REAL64:
                num = 8;
                break;
              case S3_VariableTypes.Address:
                num = 0;
                break;
              case S3_VariableTypes.ByteArray:
                num = (int) Convert.ToUInt16(dataReader["MaxValue"]);
                break;
              default:
                throw new NotImplementedException("Type is not implemented! Value: " + s3VariableTypes.ToString());
            }
            if (str == "cfg_key" && num != 16)
              throw new Exception("INTERNAL ERROR: The database is invalid! cfg_key should be 16 bytes!");
            parameterList.Add(new Parameter()
            {
              MapID = int32,
              Name = str,
              Address = Convert.ToUInt16(dataReader["cValue"]),
              Size = num,
              DifVif = dataReader["DefaultDivVif"].ToString(),
              Type = s3VariableTypes
            });
          }
          return parameterList;
        }
      }
      catch (InvalidMapFileException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<Parameter>) null;
      }
    }

    internal static ZR_ClassLibrary.HardwareType GetHardwareType(
      uint version,
      ushort hardwareVersion)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT HardwareTypeID, MapID, HardwareName, HardwareResource, Description FROM HardwareType WHERE FirmwareVersion=@FirmwareVersion AND HardwareVersion=@HardwareVersion;";
          MeterDatabase.AddParameter(cmd, "@FirmwareVersion", (double) version);
          MeterDatabase.AddParameter(cmd, "@HardwareVersion", (int) hardwareVersion);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (ZR_ClassLibrary.HardwareType) null;
          ZR_ClassLibrary.HardwareType hardwareType = new ZR_ClassLibrary.HardwareType();
          hardwareType.HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"]);
          hardwareType.MapID = Convert.ToInt32(dataReader["MapID"]);
          hardwareType.FirmwareVersion = version;
          hardwareType.HardwareName = dataReader["HardwareName"].ToString();
          hardwareType.HardwareVersion = (int) hardwareVersion;
          hardwareType.HardwareResource = dataReader["HardwareResource"].ToString();
          hardwareType.Description = dataReader["Description"].ToString();
          if (dataReader.Read())
            throw new Exception("INTERNAL ERROR: The hardware type is no unique! Version: " + version.ToString() + " HardwareVersion: " + hardwareVersion.ToString());
          return hardwareType;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (ZR_ClassLibrary.HardwareType) null;
      }
    }

    public static List<ZR_ClassLibrary.HardwareType> LoadHardwareTypeByDescription(
      string description)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT HardwareTypeID, MapID, FirmwareVersion, HardwareName, HardwareVersion, HardwareResource FROM HardwareType WHERE Description LIKE @Description;";
          MeterDatabase.AddParameter(cmd, "@Description", description);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<ZR_ClassLibrary.HardwareType>) null;
          List<ZR_ClassLibrary.HardwareType> hardwareTypeList = new List<ZR_ClassLibrary.HardwareType>();
          if (dataReader.Read())
            hardwareTypeList.Add(new ZR_ClassLibrary.HardwareType()
            {
              HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"]),
              MapID = Convert.ToInt32(dataReader["MapID"]),
              FirmwareVersion = Convert.ToUInt32(dataReader["FirmwareVersion"]),
              HardwareName = dataReader["HardwareName"].ToString(),
              HardwareVersion = Convert.ToInt32(dataReader["HardwareVersion"]),
              HardwareResource = dataReader["HardwareResource"].ToString(),
              Description = description
            });
          return hardwareTypeList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<ZR_ClassLibrary.HardwareType>) null;
      }
    }

    public static List<ZR_ClassLibrary.HardwareType> LoadHardwareType(string hardwareName)
    {
      if (string.IsNullOrEmpty(hardwareName))
        return (List<ZR_ClassLibrary.HardwareType>) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM HardwareType WHERE HardwareName=@HardwareName;";
          MeterDatabase.AddParameter(cmd, "@HardwareName", hardwareName);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<ZR_ClassLibrary.HardwareType>) null;
          List<ZR_ClassLibrary.HardwareType> hardwareTypeList = new List<ZR_ClassLibrary.HardwareType>();
          while (dataReader.Read())
            hardwareTypeList.Add(new ZR_ClassLibrary.HardwareType()
            {
              HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"]),
              MapID = Convert.ToInt32(dataReader["MapID"]),
              FirmwareVersion = Convert.ToUInt32(dataReader["FirmwareVersion"]),
              HardwareName = dataReader["HardwareName"].ToString(),
              HardwareVersion = Convert.ToInt32(dataReader["HardwareVersion"]),
              HardwareResource = dataReader["HardwareResource"].ToString(),
              Description = dataReader["Description"].ToString()
            });
          return hardwareTypeList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<ZR_ClassLibrary.HardwareType>) null;
      }
    }

    public static List<ZR_ClassLibrary.HardwareType> LoadHardwareType(DeviceVersion version)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM HardwareType WHERE FirmwareVersion=@FirmwareVersion;";
          MeterDatabase.AddParameter(cmd, "@FirmwareVersion", (double) version.Version);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<ZR_ClassLibrary.HardwareType>) null;
          List<ZR_ClassLibrary.HardwareType> hardwareTypeList = new List<ZR_ClassLibrary.HardwareType>();
          while (dataReader.Read())
            hardwareTypeList.Add(new ZR_ClassLibrary.HardwareType()
            {
              HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"]),
              MapID = Convert.ToInt32(dataReader["MapID"]),
              FirmwareVersion = Convert.ToUInt32(dataReader["FirmwareVersion"]),
              HardwareName = dataReader["HardwareName"].ToString(),
              HardwareVersion = Convert.ToInt32(dataReader["HardwareVersion"]),
              HardwareResource = dataReader["HardwareResource"].ToString(),
              Description = dataReader["Description"].ToString()
            });
          return hardwareTypeList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<ZR_ClassLibrary.HardwareType>) null;
      }
    }

    public static ZR_ClassLibrary.MeterInfo GetMeterInfo(uint meterInfoID)
    {
      return MeterDatabase.GetMeterInfo(meterInfoID);
    }

    public static List<ZR_ClassLibrary.MeterInfo> LoadMeterInfo(
      string sapMaterialNumber,
      PDC_DeviceIdentity hardware)
    {
      return MeterDatabase.LoadMeterInfo(sapMaterialNumber, hardware.ToString());
    }

    public static List<ZR_ClassLibrary.MeterInfo> LoadMeterInfo(int hardwareTypeID)
    {
      return MeterDatabase.LoadMeterInfoByHardwareTypeID(hardwareTypeID);
    }

    public static List<Meter> LoadMeter()
    {
      return PDC_Database.LoadMeter(new DateTime?(), new DateTime?());
    }

    public static List<Meter> LoadMeter(DateTime? start, DateTime? end)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Parameters.Clear();
          if (start.HasValue && end.HasValue)
          {
            cmd.CommandText = "SELECT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr FROM Meter as m, MeterInfo as mi WHERE mi.MeterHardwareID = 102 AND m.MeterInfoID=mi.MeterInfoID AND m.ProductionDate >= @start AND m.ProductionDate <= @end;";
            MeterDatabase.AddParameter(cmd, "@start", start);
            MeterDatabase.AddParameter(cmd, "@end", end);
          }
          else
            cmd.CommandText = "SELECT * FROM Meter;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<Meter>) null;
          List<Meter> meterList = new List<Meter>();
          while (dataReader.Read())
          {
            int int32_1 = Convert.ToInt32(dataReader["MeterID"]);
            if (int32_1 != 0)
            {
              int int32_2 = Convert.ToInt32(dataReader["MeterInfoID"]);
              string str = dataReader["SerialNr"].ToString();
              if (string.IsNullOrEmpty(str) || !str.StartsWith("6ZRI"))
              {
                List<ZR_ClassLibrary.MeterData> meterDataList = PDC_Database.LoadMeterData(int32_1, new int?(1));
                if (meterDataList != null && meterDataList.Count != 0 && PDC_Meter.IsValidZipBuffer(meterDataList[0].PValueBinary))
                {
                  DateTime? nullable1 = new DateTime?();
                  if (dataReader["ProductionDate"] != DBNull.Value)
                    nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ProductionDate"]));
                  DateTime? nullable2 = new DateTime?();
                  if (dataReader["ApprovalDate"] != DBNull.Value)
                    nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ApprovalDate"]));
                  meterList.Add(new Meter()
                  {
                    MeterID = int32_1,
                    MeterInfoID = int32_2,
                    SerialNr = str,
                    ProductionDate = nullable1,
                    ApprovalDate = nullable2,
                    OrderNr = dataReader["OrderNr"].ToString()
                  });
                }
              }
            }
          }
          return meterList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<Meter>) null;
      }
    }

    internal static ZR_ClassLibrary.MeterData LoadMeterData(int meterId, DateTime timePoint)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterData WHERE MeterID=@MeterID AND TimePoint=@TimePoint;";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterId);
          MeterDatabase.AddParameter(cmd, "@TimePoint", timePoint);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (ZR_ClassLibrary.MeterData) null;
          ZR_ClassLibrary.MeterData meterData = new ZR_ClassLibrary.MeterData()
          {
            MeterID = Convert.ToInt32(dataReader["MeterID"]),
            TimePoint = new DateTime?(Convert.ToDateTime(dataReader["TimePoint"])),
            PValue = dataReader["PValue"].ToString(),
            PValueBinary = dataReader["PValueBinary"] == DBNull.Value || dataReader["PValueBinary"] == null ? (byte[]) null : (byte[]) dataReader["PValueBinary"]
          };
          if (dataReader.Read())
            throw new Exception("INTERNAL ERROR: The meter data is no unique for this time point! MeterID: " + meterId.ToString() + ", Timepoint: " + timePoint.ToString());
          return meterData;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (ZR_ClassLibrary.MeterData) null;
      }
    }

    internal static List<ZR_ClassLibrary.MeterData> LoadMeterData(int meterID)
    {
      return PDC_Database.LoadMeterData(meterID, new int?());
    }

    internal static List<ZR_ClassLibrary.MeterData> LoadMeterData(int meterID, int? maxRows)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterData WHERE MeterID=@MeterID;";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<ZR_ClassLibrary.MeterData>) null;
          List<ZR_ClassLibrary.MeterData> meterDataList = new List<ZR_ClassLibrary.MeterData>();
          int? nullable1 = maxRows;
          while (dataReader.Read())
          {
            int? nullable2 = nullable1;
            int num = 0;
            if (nullable2.GetValueOrDefault() <= num & nullable2.HasValue)
              return meterDataList;
            nullable2 = nullable1;
            nullable1 = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() - 1) : new int?();
            int int32 = dataReader["PValue"] != DBNull.Value ? Convert.ToInt32(dataReader["PValue"]) : 0;
            byte[] numArray = dataReader["PValueBinary"] != DBNull.Value ? (byte[]) dataReader["PValueBinary"] : (byte[]) null;
            meterDataList.Add(new ZR_ClassLibrary.MeterData()
            {
              MeterID = Convert.ToInt32(dataReader["MeterID"]),
              TimePoint = new DateTime?(Convert.ToDateTime(dataReader["TimePoint"])),
              PValueID = int32,
              PValueBinary = numArray
            });
          }
          return meterDataList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<ZR_ClassLibrary.MeterData>) null;
      }
    }

    internal static object LoadSupportedFirmwareVersions()
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT FirmwareVersion FROM HardwareType WHERE HardwareName=@HardwareName1 OR HardwareName=@HardwareName2 GROUP BY FirmwareVersion;";
          MeterDatabase.AddParameter(cmd, "@HardwareName1", PDC_DeviceIdentity.PDC_WmBus.ToString());
          MeterDatabase.AddParameter(cmd, "@HardwareName2", PDC_DeviceIdentity.PDC_mBus.ToString());
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (object) null;
          List<string> stringList = new List<string>();
          while (dataReader.Read())
            stringList.Add(new DeviceVersion()
            {
              Version = Convert.ToUInt32(dataReader["FirmwareVersion"])
            }.VersionString);
          return (object) stringList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (object) null;
      }
    }

    internal static bool CreateType(
      string sapMaterialNumber,
      int hardwareTypeID,
      string typeDescription,
      PDC_Meter meter)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          if (!PDC_Database.CreateType(cmd, sapMaterialNumber, hardwareTypeID, typeDescription, meter))
            return false;
          cmd.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        if (cmd != null && cmd.Transaction != null)
          cmd.Transaction.Rollback();
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    internal static bool CreateType(
      IDbCommand cmd,
      string sapMaterialNumber,
      int hardwareTypeID,
      string typeDescription,
      PDC_Meter meter)
    {
      if (cmd == null || cmd.Connection == null || cmd.Connection.State != ConnectionState.Open)
        throw new ArgumentException(nameof (cmd));
      if (hardwareTypeID <= 0)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "HardwareTypeID is invalid!");
      if (meter == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Meter object is not available!");
      if (string.IsNullOrEmpty(sapMaterialNumber))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "SAP material number is not defined!");
      if (string.IsNullOrEmpty(typeDescription))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "The type description can not be empty!");
      if (sapMaterialNumber != "PDC_BASETYPE" && sapMaterialNumber != "NotDefined")
      {
        List<ZR_ClassLibrary.MeterInfo> meterInfoList = PDC_Database.LoadMeterInfo(sapMaterialNumber, meter.Version.Type);
        if (meterInfoList != null && meterInfoList.Count > 0)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, "More then one identical SAP number detected! SAP material number: " + sapMaterialNumber);
      }
      bool isBaseType = "PDC_BASETYPE" == sapMaterialNumber;
      int? nextUniqueId1;
      int? nextUniqueId2;
      if (isBaseType)
      {
        nextUniqueId1 = MeterDatabase.GetNextUniqueID(cmd, "MeterInfo_BASE", "MeterInfoID");
        nextUniqueId2 = MeterDatabase.GetNextUniqueID(cmd, "MeterType_BASE", "MeterTypeID");
      }
      else
      {
        nextUniqueId1 = MeterDatabase.GetNextUniqueID(cmd, "MeterInfo", "MeterInfoID");
        nextUniqueId2 = MeterDatabase.GetNextUniqueID(cmd, "MeterType", "MeterTypeID");
      }
      if (!nextUniqueId1.HasValue || !nextUniqueId2.HasValue)
        return false;
      ZR_ClassLibrary.MeterInfo meterInfo = MeterDatabase.AddMeterInfo(cmd, nextUniqueId1.Value, 102, nextUniqueId2.Value, sapMaterialNumber, "0", typeDescription, hardwareTypeID);
      if (meterInfo == null)
        return false;
      ZR_ClassLibrary.MeterType meterType = MeterDatabase.AddMeterType(cmd, nextUniqueId2.Value, "MTypeZelsius", meter.Version.Type.ToString(), DateTime.Now, typeDescription);
      if (meterType == null)
        return false;
      meter = PDC_Database.PrepareType(meterInfo, meter, isBaseType);
      byte[] EEPdata = meter.Zip();
      if (MeterDatabase.AddMeterTypeData(cmd, meterType.MTypeTableName, meterType.MeterTypeID, EEPdata, string.Empty) == null)
        return false;
      if (isBaseType)
      {
        if (!MeterDatabase.SetNextUniqueID(cmd, "MeterInfo_BASE", "MeterInfoID", nextUniqueId1.Value + 1) || !MeterDatabase.SetNextUniqueID(cmd, "MeterType_BASE", "MeterTypeID", nextUniqueId2.Value + 1))
          return false;
      }
      else if (!MeterDatabase.SetNextUniqueID(cmd, "MeterInfo", "MeterInfoID", nextUniqueId1.Value + 1) || !MeterDatabase.SetNextUniqueID(cmd, "MeterType", "MeterTypeID", nextUniqueId2.Value + 1))
        return false;
      meter.DBDeviceInfo.MeterInfo = meterInfo;
      return true;
    }

    private static PDC_Meter PrepareType(ZR_ClassLibrary.MeterInfo meterInfo, PDC_Meter meter, bool isBaseType)
    {
      if (meterInfo == null)
        throw new ArgumentNullException(nameof (meterInfo));
      if (meter == null)
        throw new ArgumentNullException(nameof (meter));
      if (!meter.SetSerialnumberFull((string) null))
        return (PDC_Meter) null;
      uint num = 0;
      DeviceIdentification deviceIdentification = meter.GetDeviceIdentification();
      if (deviceIdentification != null)
        num = deviceIdentification.BaseTypeID;
      DeviceIdentification ident = new DeviceIdentification();
      ident.MeterID = 0U;
      ident.SapProductionOrderNumber = 0U;
      ident.HardwareTypeID = !isBaseType ? (uint) meterInfo.HardwareTypeID : 0U;
      ident.MeterInfoID = (uint) meterInfo.MeterInfoID;
      ident.MeterTypeID = (uint) meterInfo.MeterTypeID;
      ident.BaseTypeID = num;
      if (meterInfo.PPSArtikelNr == "PDC_BASETYPE")
      {
        ident.SapMaterialNumber = 0U;
        ident.BaseTypeID = (uint) meterInfo.MeterInfoID;
      }
      else if (meterInfo.PPSArtikelNr == "NotDefined")
        ident.SapMaterialNumber = 0U;
      else
        ident.SapMaterialNumber = Util.IsNumeric((object) meterInfo.PPSArtikelNr) ? uint.Parse(meterInfo.PPSArtikelNr) : throw new ArgumentException("The value of 'meterInfo.PPSArtikelNr' is invalid!");
      if (!meter.SetDeviceIdentification(ident))
        return (PDC_Meter) null;
      meter.Version.HardwareTypeID = ident.HardwareTypeID;
      return meter;
    }

    internal static bool UpdateType(ZR_ClassLibrary.MeterInfo meterInfo, PDC_Meter meter)
    {
      if (meterInfo == null || meter == null)
        return false;
      meter = PDC_Database.PrepareType(meterInfo, meter, meterInfo.PPSArtikelNr == "PDC_BASETYPE");
      ZR_ClassLibrary.MeterType meterType = MeterDatabase.GetMeterType(meterInfo.MeterTypeID);
      if (meterType == null)
        return false;
      byte[] EEPdata = meter.Zip();
      if (!MeterDatabase.UpdateMeterTypeData(meterType.MTypeTableName, meterInfo.MeterTypeID, EEPdata, string.Empty) || !MeterDatabase.UpdateMeterInfo(meterInfo))
        return false;
      meter.DBDeviceInfo.MeterInfo = meterInfo;
      return true;
    }

    public static MeterTypeData LoadType(int meterInfoID)
    {
      ZR_ClassLibrary.MeterInfo meterInfo = MeterDatabase.LoadMeterInfo((uint) meterInfoID);
      if (meterInfo == null)
        return (MeterTypeData) null;
      ZR_ClassLibrary.MeterType meterType = MeterDatabase.GetMeterType(meterInfo.MeterTypeID);
      return meterType == null ? (MeterTypeData) null : MeterDatabase.GetMeterTypeData(meterType.MTypeTableName, meterInfo.MeterTypeID);
    }

    public static bool DeleteType(int meterInfoID)
    {
      ZR_ClassLibrary.MeterInfo meterInfo = PDC_Database.GetMeterInfo((uint) meterInfoID);
      if (meterInfo == null)
        return false;
      ZR_ClassLibrary.MeterType meterType = MeterDatabase.GetMeterType(meterInfo.MeterTypeID);
      if (meterType == null)
        return false;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          cmd.CommandText = "DELETE FROM MeterInfo WHERE MeterInfoID=@MeterInfoID;";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@MeterInfoID", meterInfoID);
          if (cmd.ExecuteNonQuery() != 1)
            return false;
          cmd.CommandText = "DELETE FROM MeterType WHERE MeterTypeID=@MeterTypeID;";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterType.MeterTypeID);
          if (cmd.ExecuteNonQuery() != 1)
            return false;
          cmd.CommandText = "DELETE FROM " + meterType.MTypeTableName + " WHERE MeterTypeID=@MeterTypeID;";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterInfo.MeterTypeID);
          if (cmd.ExecuteNonQuery() != 1)
            return false;
          try
          {
            cmd.CommandText = "DELETE FROM PPS_Cache WHERE PPS_MaterialNumber=@PPS_MaterialNumber;";
            cmd.Parameters.Clear();
            MeterDatabase.AddParameter(cmd, "@PPS_MaterialNumber", meterInfo.PPSArtikelNr);
            cmd.ExecuteNonQuery();
          }
          catch
          {
          }
          cmd.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        cmd.Transaction.Rollback();
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    internal static List<Firmware> LoadFirmware()
    {
      return PDC_Database.LoadFirmware(new PDC_DeviceIdentity?());
    }

    public static List<Firmware> LoadFirmware(PDC_DeviceIdentity? hardware)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          if (!hardware.HasValue)
          {
            cmd.CommandText = "SELECT FirmwareVersion, MapID FROM HardwareType WHERE HardwareName=@HardwareName1 OR HardwareName=@HardwareName2 GROUP BY FirmwareVersion, MapID ORDER BY MapID DESC;";
            MeterDatabase.AddParameter(cmd, "@HardwareName1", PDC_DeviceIdentity.PDC_WmBus.ToString());
            MeterDatabase.AddParameter(cmd, "@HardwareName2", PDC_DeviceIdentity.PDC_mBus.ToString());
          }
          else
          {
            cmd.CommandText = "SELECT FirmwareVersion, MapID FROM HardwareType WHERE HardwareName=@HardwareName GROUP BY FirmwareVersion, MapID ORDER BY MapID DESC;";
            MeterDatabase.AddParameter(cmd, "@HardwareName", hardware.Value.ToString());
          }
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<Firmware>) null;
          List<Firmware> firmwareList = new List<Firmware>();
          while (dataReader.Read())
          {
            ProgFiles firmware = MeterDatabase.GetFirmware(Convert.ToInt32(dataReader["MapID"]));
            if (firmware != null && !string.IsNullOrEmpty(firmware.HexText))
              firmwareList.Add(new Firmware()
              {
                Version = new DeviceVersion()
                {
                  Version = Convert.ToUInt32(dataReader["FirmwareVersion"])
                },
                FirmwareFile = firmware
              });
          }
          return firmwareList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        PDC_Database.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<Firmware>) null;
      }
    }

    public static PDC_Handler.Data GetData(string fullSerialNumber)
    {
      List<int> intList = !string.IsNullOrEmpty(fullSerialNumber) ? MeterDatabase.LoadMeter(fullSerialNumber) : throw new ArgumentNullException(nameof (fullSerialNumber), "Can not get data of the device!");
      if (intList == null)
        return (PDC_Handler.Data) null;
      int meterId = intList.Count <= 1 ? intList[0] : throw new Exception("Can not get data of the device! The serial number '" + fullSerialNumber + "' is not unique, it exist " + intList.Count.ToString() + " items. Check the 'Meter' table.");
      ZR_ClassLibrary.MeterData dataOfLastBackup = MeterDatabase.GetMeterDataOfLastBackup(meterId);
      if (dataOfLastBackup == null)
        return (PDC_Handler.Data) null;
      PDC_Meter pdcMeter = PDC_Meter.Unzip(dataOfLastBackup.PValueBinary);
      if (pdcMeter == null)
        throw new Exception("Can not get data of the device! Invalid data in table 'MeterData'. Serial number: " + fullSerialNumber + ", MeterID: " + meterId.ToString() + ", Timepoint: " + dataOfLastBackup.TimePoint.ToString());
      DeviceIdentification deviceIdentification = !(fullSerialNumber != pdcMeter.GetSerialnumberFull()) ? pdcMeter.GetDeviceIdentification() : throw new Exception("Can not get data of the device! Invalid data in table 'Meter' and 'MeterData'. The serial number in table 'Meter' is: " + fullSerialNumber + " The serial number in the table 'MeterData' is: " + pdcMeter.GetSerialnumberFull() + ". Check 'MeterData' table. MeterID: " + meterId.ToString() + ", Timepoint: " + dataOfLastBackup.TimePoint.ToString());
      return new PDC_Handler.Data()
      {
        MeterID = meterId,
        FullSerialNumber = fullSerialNumber,
        SapMaterialNumber = deviceIdentification != null ? deviceIdentification.SapMaterialNumber.ToString() : "",
        AesKey = pdcMeter.GetAESkey()
      };
    }
  }
}
