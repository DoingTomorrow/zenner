// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DatabaseAccess
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  internal class S3_DatabaseAccess
  {
    private static Logger DatabaseAccessLogger = LogManager.GetLogger(nameof (S3_DatabaseAccess));
    private HardwareTypeSupport _HardwareAndFirmwareInfos;
    private List<Function> cachedFunctionList;
    internal S3_HandlerFunctions MyFunctions;
    internal BaseDbConnection BaseDb;
    internal DbConnection DbConn;
    internal bool DailyAutosave = true;
    private StringBuilder SQL = new StringBuilder(2000);
    private Schema.DatabaseIdentificationDataTable DatabaseIdentTable;
    private static SortedList<uint, SortedList<uint, List<string>>> CompatibilityLists;

    internal HardwareTypeSupport HardwareAndFirmwareInfos
    {
      get
      {
        if (this._HardwareAndFirmwareInfos == null)
          this._HardwareAndFirmwareInfos = new HardwareTypeSupport(new string[2]
          {
            "zelsius C5",
            "WR4"
          });
        return this._HardwareAndFirmwareInfos;
      }
    }

    internal List<Function> CachedFunctionList
    {
      get
      {
        if (this.cachedFunctionList == null)
          this.cachedFunctionList = this.LoadPrecompiledFunctions();
        return this.cachedFunctionList;
      }
    }

    public S3_DatabaseAccess(S3_HandlerFunctions MyFunctions, DbBasis usedDatabase)
    {
      this.MyFunctions = MyFunctions;
      this.BaseDb = usedDatabase.BaseDbConnection;
      this.DbConn = this.BaseDb.GetNewConnection();
      string databaseOption = this.GetDatabaseOption("DatabaseSaveOption");
      if (databaseOption == null || !(databaseOption == "DailyAutosaveOff"))
        return;
      this.DailyAutosave = false;
    }

    internal DbDataAdapter GetDataAdapter(string selectSql)
    {
      return this.BaseDb.GetDataAdapter(selectSql, this.DbConn);
    }

    internal string GetDatabaseOption(string option)
    {
      if (this.DatabaseIdentTable == null)
      {
        DbDataAdapter dataAdapter = this.BaseDb.GetDataAdapter("SELECT * FROM DatabaseIdentification", this.DbConn);
        this.DatabaseIdentTable = new Schema.DatabaseIdentificationDataTable();
        dataAdapter.Fill((DataTable) this.DatabaseIdentTable);
      }
      Schema.DatabaseIdentificationRow[] identificationRowArray = (Schema.DatabaseIdentificationRow[]) this.DatabaseIdentTable.Select("InfoName = '" + option + "'");
      return identificationRowArray.Length == 1 ? identificationRowArray[0].InfoData : (string) null;
    }

    private byte[] DifVifValuesFromString(string ValueString)
    {
      byte[] numArray = (byte[]) null;
      try
      {
        string[] strArray = Util.RemoveEmptyEntries(ValueString.Split(' '));
        if (strArray.Length != 0)
        {
          numArray = new byte[strArray.Length];
          for (int index = 0; index < numArray.Length; ++index)
            numArray[index] = byte.Parse(strArray[index], NumberStyles.HexNumber);
        }
      }
      catch
      {
        ZR_ClassLibMessages.AddWarning("Illegal DifVif string: '" + ValueString + "'");
        numArray = (byte[]) null;
      }
      return numArray;
    }

    internal byte GetApprovalRevision(int hardwareTypeId)
    {
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo = this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo(hardwareTypeId);
      if (hardwareAndFirmwareInfo == null)
        return 0;
      byte result1 = 0;
      if (!hardwareAndFirmwareInfo.IsHardwareOptionsNull())
        byte.TryParse(ParameterService.GetParameter(hardwareAndFirmwareInfo.HardwareOptions, "ApprovalRevison", '=', ';'), out result1);
      byte result2 = 0;
      if (!hardwareAndFirmwareInfo.IsOptionsNull())
        byte.TryParse(ParameterService.GetParameter(hardwareAndFirmwareInfo.Options, "ApprovalRevison", '=', ';'), out result2);
      return (int) result1 > (int) result2 ? result1 : result2;
    }

    internal bool GetHardwareTypeIdFromVersion(S3_DeviceId theDeviceId)
    {
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo = this.HardwareAndFirmwareInfos.GetExactOrNewestMapCompatibleHardwareAndFirmwareInfo((int) theDeviceId.HardwareMask, (int) theDeviceId.FirmwareVersion);
      if (hardwareAndFirmwareInfo != null)
      {
        theDeviceId.HardwareTypeId = (uint) hardwareAndFirmwareInfo.HardwareTypeID;
        theDeviceId.MapId = (uint) hardwareAndFirmwareInfo.MapID;
        this.AddHardwareInfosFromHardwareTypeId(theDeviceId);
        return true;
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Device firmware version " + theDeviceId.FirmwareVersionString + " not supported.");
      return false;
    }

    internal bool IsDeviceHardwareCompatible(S3_DeviceId newDeviceId, S3_DeviceId oldDeviceId)
    {
      if (newDeviceId.FirmwareVersion < 83886080U)
        return true;
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo1 = this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo((int) newDeviceId.HardwareMask, (int) newDeviceId.FirmwareVersion);
      if (hardwareAndFirmwareInfo1 == null)
        return false;
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo2 = this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo((int) oldDeviceId.HardwareMask, (int) oldDeviceId.FirmwareVersion);
      if (hardwareAndFirmwareInfo2 == null)
        return false;
      if (hardwareAndFirmwareInfo1.HardwareVersion == hardwareAndFirmwareInfo2.HardwareVersion)
        return true;
      bool flag = true;
      if ((hardwareAndFirmwareInfo1.HardwareVersion & 64) != (hardwareAndFirmwareInfo2.HardwareVersion & 64))
        flag = false;
      if (flag && (hardwareAndFirmwareInfo1.HardwareVersion & 15) != (hardwareAndFirmwareInfo2.HardwareVersion & 15) && ((hardwareAndFirmwareInfo1.HardwareVersion & 15) != 1 || (hardwareAndFirmwareInfo2.HardwareVersion & 15) != 4))
        flag = false;
      if (flag)
        return true;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Device hardware is not compatible!");
      stringBuilder.AppendLine(" Base hardware: ...... " + ParameterService.GetHardwareString(oldDeviceId.HardwareMask));
      stringBuilder.AppendLine(" Destination hardware: " + ParameterService.GetHardwareString(newDeviceId.HardwareMask));
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, stringBuilder.ToString());
      return false;
    }

    internal bool IsDeviceHardwareAndFirmwareMapCompatible(
      S3_DeviceId newDeviceId,
      S3_DeviceId oldDeviceId)
    {
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo1 = this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo((int) newDeviceId.HardwareMask, (int) newDeviceId.FirmwareVersion);
      if (hardwareAndFirmwareInfo1 == null)
        return false;
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo2 = this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo((int) oldDeviceId.HardwareMask, (int) oldDeviceId.FirmwareVersion);
      return hardwareAndFirmwareInfo2 != null && this.HardwareAndFirmwareInfos.IsHardwareCompatible(hardwareAndFirmwareInfo2, hardwareAndFirmwareInfo1);
    }

    internal string GetFirmwareStringFromHardwareTypeId(int HardwareTypeId)
    {
      try
      {
        return ParameterService.GetVersionString((long) this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo(HardwareTypeId).FirmwareVersion, 8);
      }
      catch
      {
        return "Firware version for HardwareTypeId " + HardwareTypeId.ToString() + " not in data base";
      }
    }

    internal void AddHardwareInfosFromHardwareTypeId(S3_DeviceId theDeviceId)
    {
      if (theDeviceId.HardwareTypeId == 0U)
        return;
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo = this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo((int) theDeviceId.HardwareTypeId);
      if (hardwareAndFirmwareInfo == null)
        return;
      if (!hardwareAndFirmwareInfo.IsHardwareOptionsNull())
        theDeviceId.HardwareOptions = hardwareAndFirmwareInfo.HardwareOptions;
      if (!hardwareAndFirmwareInfo.IsHardwareResourceNull())
      {
        string[] strArray = Util.RemoveEmptyEntries(hardwareAndFirmwareInfo.HardwareResource.Split(';'));
        if (strArray.Length != 0)
        {
          theDeviceId.HardwareResources = new SortedList<string, int>();
          for (int index = 0; index < strArray.Length; ++index)
            theDeviceId.HardwareResources.Add(strArray[index], 0);
        }
      }
    }

    internal bool ExistsDalyBackup(uint MeterId)
    {
      try
      {
        DbDataAdapter dataAdapter = this.BaseDb.GetDataAdapter("SELECT * FROM MeterData WHERE MeterID=@MeterID AND TimePoint > @TimePoint", this.DbConn);
        DbCommand selectCommand = dataAdapter.SelectCommand;
        DbParameter parameter1 = selectCommand.CreateParameter();
        parameter1.DbType = DbType.Int32;
        parameter1.ParameterName = "@MeterID";
        parameter1.Value = (object) MeterId;
        selectCommand.Parameters.Add((object) parameter1);
        DbParameter parameter2 = selectCommand.CreateParameter();
        parameter2.DbType = DbType.DateTime;
        parameter2.ParameterName = "@TimePoint";
        DbParameter dbParameter = parameter2;
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.ToUniversalTime();
        // ISSUE: variable of a boxed type
        __Boxed<DateTime> date = (System.ValueType) dateTime.Date;
        dbParameter.Value = (object) date;
        selectCommand.Parameters.Add((object) parameter2);
        Schema.MeterDataDataTable meterDataDataTable = new Schema.MeterDataDataTable();
        dataAdapter.Fill((DataTable) meterDataDataTable);
        return meterDataDataTable.Count > 0;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on read data from table HardwareType");
        throw ex;
      }
    }

    internal DbDataAdapter GetDataTableBySQLCommand(string SQLCommand, DataTable Table)
    {
      DbDataAdapter dataAdapter = this.BaseDb.GetDataAdapter(SQLCommand, this.DbConn);
      dataAdapter.Fill(Table);
      return dataAdapter;
    }

    internal DbDataAdapter GetDataTableBySQLCommand(
      string SQLCommand,
      DataTable Table,
      DbTransaction TheTransaction)
    {
      DbDataAdapter dataAdapter = this.BaseDb.GetDataAdapter(SQLCommand, this.DbConn, TheTransaction);
      dataAdapter.Fill(Table);
      return dataAdapter;
    }

    internal bool GetNewestHardwareTypeIdFromHardwareIdentification(
      DeviceHardwareIdentification hardwareIdentification,
      ref S3_DeviceIdentification theDeviceId)
    {
      uint num1;
      if (hardwareIdentification.DeviceType == DHI_DeviceType.WR4)
      {
        num1 = 513U;
        switch (hardwareIdentification.TempSensorType)
        {
          case DHI_TempSensorType.PT100:
            num1 |= 16U;
            break;
          case DHI_TempSensorType.PT500:
            num1 |= 32U;
            break;
          case DHI_TempSensorType.PT1000:
            num1 |= 64U;
            break;
        }
      }
      else
      {
        uint num2 = 2048;
        uint num3;
        if (hardwareIdentification.VolumeMeterType == DHI_VolumeMeterType.IUF)
        {
          if (hardwareIdentification.VolumeMeterBounding == DHI_VolumeMeterBounding.Combi)
            num3 = num2 | 64U;
          else
            goto label_17;
        }
        else if (hardwareIdentification.VolumeMeterBounding == DHI_VolumeMeterBounding.Combi)
          num3 = num2 | 32U;
        else if (hardwareIdentification.VolumeMeterBounding == DHI_VolumeMeterBounding.Compact)
          num3 = num2 | 16U;
        else
          goto label_17;
        num1 = !hardwareIdentification.CommunicationInterfaces.Contains(DHI_CommunicationInterface.MBus) ? (!hardwareIdentification.CommunicationInterfaces.Contains(DHI_CommunicationInterface.radio3_WMBus_868) ? (!hardwareIdentification.CommunicationInterfaces.Contains(DHI_CommunicationInterface.LoRa) ? num3 | 4U : num3 | 8U) : num3 | 2U) : num3 | 1U;
        goto label_14;
label_17:
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal hardware identification");
        return false;
      }
label_14:
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo1 = this.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo((int) num1, (int) theDeviceId.FirmwareVersion);
      if (hardwareAndFirmwareInfo1 == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Hardware type for base firmware not available.");
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo2 = this.HardwareAndFirmwareInfos.GetNewestFullCompatibleHardwareAndFirmwareInfo(hardwareAndFirmwareInfo1, (int) num1);
      theDeviceId.HardwareMask = num1;
      theDeviceId.MapId = (uint) hardwareAndFirmwareInfo2.MapID;
      theDeviceId.HardwareTypeId = (uint) hardwareAndFirmwareInfo2.HardwareTypeID;
      this.AddHardwareInfosFromHardwareTypeId((S3_DeviceId) theDeviceId);
      return true;
    }

    public List<Function> LoadPrecompiledFunctions()
    {
      List<Function> source = new List<Function>();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (DbConnection newConnection = this.BaseDb.GetNewConnection())
        {
          newConnection.Open();
          cmd = (IDbCommand) newConnection.CreateCommand();
          cmd.CommandText = "SELECT * FROM ZRFunction WHERE HardwareResource LIKE @HardwareResource;";
          MeterDatabase.AddParameter(cmd, "@HardwareResource", "%;Serie3;%");
          using (IDataReader dataReader = cmd.ExecuteReader())
          {
            if (dataReader == null)
              return source;
            while (dataReader.Read())
              source.Add(new Function()
              {
                FunctionNumber = Convert.ToInt32(dataReader["FunctionNumber"]),
                FunctionName = Util.ToString(dataReader["FunctionName"]),
                FunctionGroup = Util.ToString(dataReader["FunctionGroup"]),
                FullName = Util.ToString(dataReader["FullName"]),
                MeterUnit = Util.ToString(dataReader["MeterUnit"]),
                FirmwareVersionMin = Util.ToInteger(dataReader["FirmwareVersionMin"]),
                FirmwareVersionMax = Util.ToInteger(dataReader["FirmwareVersionMax"]),
                FunctionType = Util.ToInteger(dataReader["FunctionType"]),
                FunctionShortInfo = Util.ToString(dataReader["FunctionShortInfo"]),
                FunctionDescription = Util.ToString(dataReader["FunctionDescription"]),
                FunctionVersion = Util.ToInteger(dataReader["FunctionVersion"]),
                AccessRight = Util.ToInteger(dataReader["AccessRight"]),
                UserGroup = Util.ToInteger(dataReader["UserGroup"]),
                Symbolname = Util.ToString(dataReader["Symbolname"]),
                LoggerType = Util.ToInteger(dataReader["LoggerType"]),
                HardwareResource = Util.ToString(dataReader["HardwareResource"]),
                AccessRights = Util.ToString(dataReader["AccessRights"]),
                SoftwareResource = Util.ToString(dataReader["SoftwareResource"])
              });
          }
          List<FunctionPrecompiled> functionPrecompiledList = MeterDatabase.LoadFunctionPrecompiled(cmd, source.Select<Function, int>((System.Func<Function, int>) (x => x.FunctionNumber)).ToList<int>());
          foreach (Function function in source)
          {
            Function f = function;
            f.Precompiled = functionPrecompiledList.FindAll((Predicate<FunctionPrecompiled>) (x => x.FunctionNumber == f.FunctionNumber));
          }
          return source;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        S3_DatabaseAccess.DatabaseAccessLogger.Error(ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return source;
      }
    }

    internal Function GetFunction(ushort functionNumber)
    {
      Function function = this.CachedFunctionList.Find((Predicate<Function>) (e => e.FunctionNumber == (int) functionNumber));
      if (function == null)
        S3_DatabaseAccess.DatabaseAccessLogger.Fatal("INTERNAL ERROR!!! C5 device has a function, but database does not have it! Function number: " + functionNumber.ToString());
      return function;
    }

    internal bool GetDeviceKeys(
      int DeviceMeterID,
      out uint Key,
      out MeterDBAccess.ValueTypes ValueType)
    {
      Key = 0U;
      ValueType = MeterDBAccess.ValueTypes.GovernmentRandomNr;
      Schema.MeterDataDataTable meterDataDataTable = new Schema.MeterDataDataTable();
      try
      {
        this.SQL.Length = 0;
        this.SQL.Append("SELECT * FROM MeterData");
        this.SQL.Append(" WHERE MeterID = " + DeviceMeterID.ToString());
        this.SQL.Append(" AND ( PValueId = " + 96.ToString());
        this.SQL.Append(" OR PValueId = " + 217.ToString());
        this.SQL.Append(") ORDER BY TimePoint ASC");
        this.GetDataAdapter(this.SQL.ToString()).Fill((DataTable) meterDataDataTable);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on table MeterData");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      bool deviceKeys = false;
      if (meterDataDataTable.Rows.Count > 0)
      {
        try
        {
          int index = meterDataDataTable.Rows.Count - 1;
          MeterDBAccess.ValueTypes pvalueId = (MeterDBAccess.ValueTypes) meterDataDataTable[index].PValueID;
          Key = uint.Parse(meterDataDataTable[index].PValue);
          ValueType = pvalueId != MeterDBAccess.ValueTypes.GovernmentRandomNr ? MeterDBAccess.ValueTypes.MeterKey : MeterDBAccess.ValueTypes.GovernmentRandomNr;
          deviceKeys = true;
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal eprom data");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
          return false;
        }
      }
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Key not found");
      return deviceKeys;
    }

    internal bool SetDeviceKey(uint DeviceMeterID, uint Key, MeterDBAccess.ValueTypes ValueType)
    {
      Schema.MeterDataDataTable meterDataDataTable = new Schema.MeterDataDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM MeterData");
      this.SQL.Append(" WHERE MeterID = " + DeviceMeterID.ToString());
      this.SQL.Append(" AND ( PValueId = " + 96.ToString());
      this.SQL.Append(" OR PValueId = " + 217.ToString());
      this.SQL.Append(")");
      DbDataAdapter dataAdapter = this.BaseDb.GetDataAdapter(this.SQL.ToString(), this.DbConn, out DbCommandBuilder _);
      try
      {
        dataAdapter.Fill((DataTable) meterDataDataTable);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on table MeterData");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      try
      {
        meterDataDataTable.Clear();
        Schema.MeterDataRow row = (Schema.MeterDataRow) meterDataDataTable.NewRow();
        row.MeterID = (int) DeviceMeterID;
        row.PValueID = (int) ValueType;
        row.PValue = Key.ToString();
        row.TimePoint = DateTime.Now.ToUniversalTime();
        meterDataDataTable.Rows.Add((DataRow) row);
        dataAdapter.Update((DataTable) meterDataDataTable);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Write key error");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      return true;
    }

    internal bool IsMapCompatible(
      uint destinationMapId,
      uint sourceMapId,
      string requiredCompatibility)
    {
      if ((int) destinationMapId == (int) sourceMapId)
        return true;
      if (S3_DatabaseAccess.CompatibilityLists == null)
      {
        try
        {
          Schema.ProgFilesDataTable progFilesDataTable = new Schema.ProgFilesDataTable();
          this.GetDataAdapter("SELECT MapID,Options,SourceInfo FROM ProgFiles").Fill((DataTable) progFilesDataTable);
          SortedList<uint, SortedList<uint, List<string>>> sortedList1 = new SortedList<uint, SortedList<uint, List<string>>>();
          foreach (Schema.ProgFilesRow progFilesRow in (TypedTableBase<Schema.ProgFilesRow>) progFilesDataTable)
          {
            uint mapId = (uint) progFilesRow.MapID;
            if (!progFilesRow.IsOptionsNull())
            {
              string[] strArray1 = progFilesRow.Options.Split(';');
              for (int index1 = 0; index1 < strArray1.Length; ++index1)
              {
                if (strArray1[index1].StartsWith("CompatibleMapId"))
                {
                  string[] strArray2 = strArray1[index1].Split('=')[1].Split(',');
                  List<string> stringList = new List<string>();
                  for (int index2 = 1; index2 < strArray2.Length; ++index2)
                    stringList.Add(strArray2[index2]);
                  uint key = (uint) int.Parse(strArray2[0]);
                  int index3 = sortedList1.IndexOfKey(mapId);
                  SortedList<uint, List<string>> sortedList2;
                  if (index3 >= 0)
                  {
                    sortedList2 = sortedList1.Values[index3];
                  }
                  else
                  {
                    sortedList2 = new SortedList<uint, List<string>>();
                    sortedList1.Add(mapId, sortedList2);
                  }
                  sortedList2.Add(key, stringList);
                  if (mapId == 62U && key == 59U)
                    sortedList2.Add(57U, new List<string>()
                    {
                      "US",
                      "TS"
                    });
                }
              }
            }
          }
          S3_DatabaseAccess.CompatibilityLists = sortedList1;
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on table ProgFiles by creation compatiblity informations");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
          return false;
        }
      }
      int index4 = S3_DatabaseAccess.CompatibilityLists.IndexOfKey(destinationMapId);
      if (index4 < 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "DestinationMapId not supported", S3_DatabaseAccess.DatabaseAccessLogger);
        return false;
      }
      int index5 = S3_DatabaseAccess.CompatibilityLists.Values[index4].IndexOfKey(sourceMapId);
      if (index5 < 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "The firmware versions are not compatible.", S3_DatabaseAccess.DatabaseAccessLogger);
        return false;
      }
      List<string> stringList1 = S3_DatabaseAccess.CompatibilityLists.Values[index4].Values[index5];
      if (stringList1.Contains("full") || stringList1.Contains(requiredCompatibility))
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "The firmware versions is not compatible for: " + requiredCompatibility, S3_DatabaseAccess.DatabaseAccessLogger);
      return false;
    }

    internal class IdsFromVersion
    {
      internal uint Version;
      internal uint HardwareMask;
      internal uint MapId;
      internal uint HardwareTypeId;

      internal IdsFromVersion(uint Version, uint HardwareMask, uint MapId, uint HardwareTypeId)
      {
        this.Version = Version;
        this.HardwareMask = HardwareMask;
        this.MapId = MapId;
        this.HardwareTypeId = HardwareTypeId;
      }

      public override string ToString()
      {
        return new FirmwareVersion(this.Version).ToString() + "; HwTypeId:" + this.HardwareTypeId.ToString() + "; MapID:" + this.MapId.ToString();
      }
    }
  }
}
