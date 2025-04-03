// Decompiled with JetBrains decompiler
// Type: GMM_Handler.DataBaseAccess
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using DeviceCollector;
using GmmDbLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace GMM_Handler
{
  public class DataBaseAccess
  {
    internal ZR_HandlerFunctions MyHandler;
    internal MeterDBAccess MyDB;
    private DbBasis MyTypedDB;
    private IDbConnection MyDbConnection;
    private int ret;
    private string ErrorMsg;
    private StringBuilder SQL = new StringBuilder(2000);
    private SortedList<int, DataBaseAccess.LoggerEntryData> LoggerEntriesList;
    private SortedList<string, DataBaseAccess.PValueDescription> PValDescriptionCache;
    private string ManufacturerString = string.Empty;
    private string MeterGenerationString = string.Empty;
    private Meter LastMeter = (Meter) null;
    private Schema.DatabaseIdentificationDataTable DatabaseIdentTable;
    private Schema.DatabaseIdentificationDataTable AutoWriteIdentTable;
    internal Schema.MeterInfoDataTable MeterInfoTable;
    internal Schema.MeterInfoPropertiesDataTable MeterInfoPropertiesTable;
    private SortedList<int, SortedList<DataBaseAccess.MeterInfoProperties, string>> AllMeterInfoProperties;
    internal Schema.MeterDataTable MeterTable;
    internal Schema.MTypeZelsiusDataTable MTypeZelsiusTable;

    public DataBaseAccess(ZR_HandlerFunctions MyHandlerIn)
    {
      this.MyHandler = MyHandlerIn;
      this.MyDB = Datenbankverbindung.MainDBAccess;
      this.MyTypedDB = DbBasis.PrimaryDB;
      this.MyDbConnection = this.MyTypedDB.GetDbConnection();
    }

    private int getDataTableForSQLCommand(string SQLCommand, DataTable table)
    {
      this.MyTypedDB.ZRDataAdapter(SQLCommand, this.MyDbConnection).Fill(table);
      return table.Rows.Count;
    }

    internal bool GetHardwareInfo(ZR_MeterIdent theIdent)
    {
      int num1 = theIdent.MeterInfoBaseID;
      if (num1 == 0)
        num1 = theIdent.MeterInfoID;
      if (num1 == 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal MeterInfoID");
        return false;
      }
      this.SQL.Length = 0;
      this.SQL.Append("SELECT ");
      this.SQL.Append(" MeterInfo.MeterInfoID");
      this.SQL.Append(",MeterInfo.HardwareTypeID");
      this.SQL.Append(",MeterInfo.MeterTypeID");
      this.SQL.Append(",MeterInfo.DefaultFunctionNr");
      this.SQL.Append(",HardwareType.MapID");
      this.SQL.Append(",HardwareType.LinkerTableId");
      this.SQL.Append(",HardwareType.HardwareName");
      this.SQL.Append(",HardwareType.Hardwareversion");
      this.SQL.Append(",HardwareType.Firmwareversion");
      this.SQL.Append(",HardwareType.HardwareResource");
      this.SQL.Append(",HardwareType.extEEPSize");
      this.SQL.Append(",HardwareType.HardwareOptions");
      this.SQL.Append(" FROM MeterInfo, HardwareType");
      this.SQL.Append(" WHERE MeterInfoID = " + num1.ToString());
      this.SQL.Append(" AND MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID");
      try
      {
        DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable table1 = new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable();
        if (this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table1) == 1)
        {
          if (theIdent.lFirmwareVersion > 0L && theIdent.lFirmwareVersion != (long) table1[0].FirmwareVersion)
          {
            if (!table1[0].IsHardwareOptionsNull())
            {
              string[] strArray1 = table1[0].HardwareOptions.Split(';');
              Schema.HardwareTypeDataTable table2 = new Schema.HardwareTypeDataTable();
              do
              {
                int num2 = -1;
                for (int index = 0; index < strArray1.Length; ++index)
                {
                  string[] strArray2 = strArray1[index].Split('=');
                  if (strArray2.Length == 2 && !(strArray2[0] != "CompatibleHwTypeId"))
                  {
                    num2 = int.Parse(strArray2[1]);
                    break;
                  }
                }
                if (num2 >= 0)
                {
                  table2.Clear();
                  if (this.getDataTableForSQLCommand("SELECT * FROM HardwareType WHERE HardwareTypeId = " + num2.ToString(), (DataTable) table2) == 1)
                  {
                    if (theIdent.lFirmwareVersion != (long) table2[0].FirmwareVersion)
                    {
                      if (!table2[0].IsHardwareOptionsNull())
                        strArray1 = table2[0].HardwareOptions.Split(';');
                      else
                        goto label_26;
                    }
                    else
                      strArray1 = (string[]) null;
                  }
                  else
                    goto label_26;
                }
                else
                  goto label_26;
              }
              while (strArray1 != null);
              theIdent.MapId = table2[0].MapID;
              theIdent.LinkerTableID = table2[0].LinkerTableID;
              theIdent.HardwareTypeID = table2[0].HardwareTypeID;
              theIdent.HardwareName = table2[0].HardwareName;
              theIdent.HardwareVersion = table2[0].HardwareVersion.ToString();
              theIdent.HardwareResource = table2[0].HardwareResource;
              theIdent.extEEPSize = table2[0].extEEPSize;
            }
            else
              goto label_26;
          }
          else
          {
            theIdent.lFirmwareVersion = (long) table1[0].FirmwareVersion;
            theIdent.MapId = table1[0].MapID;
            theIdent.LinkerTableID = table1[0].LinkerTableID;
            theIdent.HardwareTypeID = table1[0].HardwareTypeID;
            theIdent.HardwareName = table1[0].HardwareName;
            theIdent.HardwareVersion = table1[0].HardwareVersion.ToString();
            theIdent.HardwareResource = table1[0].HardwareResource;
            theIdent.extEEPSize = table1[0].extEEPSize;
          }
          theIdent.MeterTypeID = table1[0].MeterTypeID;
          theIdent.DefaultFunctionNr = ushort.Parse(table1[0].DefaultFunctionNr);
          theIdent.sFirmwareVersion = ParameterService.GetVersionString(theIdent.lFirmwareVersion, 7);
        }
        else
          goto label_26;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on table HardwareType");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      return true;
label_26:
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Device hardware data not found!");
      return false;
    }

    internal ZR_MeterIdent IsFirmwareCompatible(ZR_MeterIdent DataIdent, long DeviceFirmware)
    {
      Schema.HardwareTypeDataTable table = new Schema.HardwareTypeDataTable();
      if (this.getDataTableForSQLCommand("SELECT * FROM HardwareType WHERE HardwareTypeId = " + DataIdent.HardwareTypeID.ToString(), (DataTable) table) != 1)
        return (ZR_MeterIdent) null;
      while (true)
      {
        if (!table[0].IsHardwareOptionsNull())
        {
          string[] strArray1 = table[0].HardwareOptions.Split(';');
          int num = -1;
          for (int index = 0; index < strArray1.Length; ++index)
          {
            string[] strArray2 = strArray1[index].Split('=');
            if (strArray2.Length == 2 && !(strArray2[0] != "CompatibleHwTypeId"))
            {
              num = int.Parse(strArray2[1]);
              break;
            }
          }
          if (num >= 0)
          {
            table.Clear();
            if (this.getDataTableForSQLCommand("SELECT * FROM HardwareType WHERE HardwareTypeId = " + num.ToString(), (DataTable) table) == 1)
            {
              if (DeviceFirmware != (long) table[0].FirmwareVersion)
              {
                if (!table[0].IsHardwareOptionsNull())
                  table[0].HardwareOptions.Split(';');
                else
                  goto label_14;
              }
              else
                goto label_16;
            }
            else
              goto label_11;
          }
          else
            goto label_9;
        }
        else
          break;
      }
      return (ZR_MeterIdent) null;
label_9:
      return (ZR_MeterIdent) null;
label_11:
      return (ZR_MeterIdent) null;
label_14:
      return (ZR_MeterIdent) null;
label_16:
      ZR_MeterIdent zrMeterIdent = DataIdent.Clone();
      zrMeterIdent.lFirmwareVersion = (long) table[0].FirmwareVersion;
      zrMeterIdent.MapId = table[0].MapID;
      zrMeterIdent.LinkerTableID = table[0].LinkerTableID;
      zrMeterIdent.HardwareTypeID = table[0].HardwareTypeID;
      zrMeterIdent.HardwareName = table[0].HardwareName;
      zrMeterIdent.HardwareVersion = table[0].HardwareVersion.ToString();
      zrMeterIdent.HardwareResource = table[0].HardwareResource;
      zrMeterIdent.extEEPSize = table[0].extEEPSize;
      return zrMeterIdent;
    }

    internal bool GetDeviceKeys(
      int DeviceMeterID,
      out long Key,
      out MeterDBAccess.ValueTypes ValueType)
    {
      Key = 0L;
      ValueType = MeterDBAccess.ValueTypes.GovernmentRandomNr;
      Schema.MeterDataDataTable table = new Schema.MeterDataDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM MeterData");
      this.SQL.Append(" WHERE MeterID = " + DeviceMeterID.ToString());
      this.SQL.Append(" AND ( PValueId = " + 96.ToString());
      this.SQL.Append(" OR PValueId = " + 217.ToString());
      this.SQL.Append(") ORDER BY TimePoint ASC");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on table MeterData");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      bool deviceKeys = false;
      if (table.Rows.Count > 0)
      {
        try
        {
          for (int index = 0; index < table.Rows.Count; ++index)
          {
            if (table[index].PValueID == 96)
            {
              Key = long.Parse(table[index].PValue);
              ValueType = MeterDBAccess.ValueTypes.GovernmentRandomNr;
              deviceKeys = true;
            }
            else
            {
              Key = long.Parse(table[index].PValue);
              ValueType = MeterDBAccess.ValueTypes.MeterKey;
              deviceKeys = true;
            }
          }
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal eprom data");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
          return false;
        }
      }
      return deviceKeys;
    }

    internal bool SetDeviceKey(int DeviceMeterID, long Key, MeterDBAccess.ValueTypes ValueType)
    {
      Schema.MeterDataDataTable MyDataTable = new Schema.MeterDataDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM MeterData");
      this.SQL.Append(" WHERE MeterID = " + DeviceMeterID.ToString());
      this.SQL.Append(" AND ( PValueId = " + 96.ToString());
      this.SQL.Append(" OR PValueId = " + 217.ToString());
      this.SQL.Append(")");
      ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
      try
      {
        zrDataAdapter.Fill((DataTable) MyDataTable);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on table MeterData");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      MyDataTable.Clear();
      Schema.MeterDataRow row = (Schema.MeterDataRow) MyDataTable.NewRow();
      row.MeterID = DeviceMeterID;
      row.PValueID = (int) ValueType;
      row.PValue = Key.ToString();
      row.TimePoint = DateTime.Now;
      MyDataTable.Rows.Add((DataRow) row);
      try
      {
        zrDataAdapter.Update((DataTable) MyDataTable);
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

    internal bool GetTypeBaseData(
      int MeterInfoID,
      out ZR_MeterIdent TheIdent,
      out byte[] EpromData)
    {
      TheIdent = (ZR_MeterIdent) null;
      EpromData = (byte[]) null;
      DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable table = new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT");
      this.SQL.Append(" MeterInfo.HardwareTypeID");
      this.SQL.Append(",MeterInfo.MeterTypeID");
      this.SQL.Append(",MeterInfo.MeterHardwareID");
      this.SQL.Append(",MeterInfo.PPSArtikelNr");
      this.SQL.Append(",MeterInfo.DefaultFunctionNr");
      this.SQL.Append(",MeterInfo.Description");
      this.SQL.Append(",HardwareType.HardwareVersion");
      this.SQL.Append(",HardwareType.FirmwareVersion");
      this.SQL.Append(",HardwareType.HardwareName");
      this.SQL.Append(",HardwareType.MapID");
      this.SQL.Append(",HardwareType.LinkerTableID");
      this.SQL.Append(",HardwareType.extEEPSize");
      this.SQL.Append(",MTypeZelsius.EEPData");
      this.SQL.Append(",MTypeZelsius.TypeOverrideString");
      this.SQL.Append(" FROM MeterInfo, HardwareType, MeterType, MTypeZelsius");
      this.SQL.Append(" WHERE MeterInfo.MeterInfoID = " + MeterInfoID.ToString());
      this.SQL.Append(" AND   MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID");
      this.SQL.Append(" AND   MeterInfo.MeterTypeID = MeterType.MeterTypeID");
      this.SQL.Append(" AND   MeterInfo.MeterTypeID = MTypeZelsius.MeterTypeID");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on MeterInfo");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      if (table.Rows.Count != 1)
      {
        this.AddErrorText("Fehler : Daten nicht gefunden");
        return false;
      }
      TheIdent = new ZR_MeterIdent(MeterBasis.Type);
      TheIdent.MeterInfoID = MeterInfoID;
      DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow zelsiusJoinedRow = table[0];
      try
      {
        TheIdent.PPSArtikelNr = zelsiusJoinedRow.PPSArtikelNr;
        TheIdent.MeterInfoDescription = zelsiusJoinedRow.Description;
        TheIdent.HardwareTypeID = zelsiusJoinedRow.HardwareTypeID;
        TheIdent.MeterTypeID = zelsiusJoinedRow.MeterTypeID;
        TheIdent.MeterHardwareID = zelsiusJoinedRow.MeterHardwareID;
        int DefaultFunctionNr = int.Parse(zelsiusJoinedRow.DefaultFunctionNr);
        if (!this.GetBaseTypeId(TheIdent, DefaultFunctionNr))
          return false;
        TheIdent.HardwareVersion = zelsiusJoinedRow.HardwareVersion.ToString();
        TheIdent.lFirmwareVersion = (long) zelsiusJoinedRow.FirmwareVersion;
        TheIdent.HardwareName = zelsiusJoinedRow.HardwareName;
        TheIdent.MapId = zelsiusJoinedRow.MapID;
        TheIdent.LinkerTableID = zelsiusJoinedRow.LinkerTableID;
        TheIdent.extEEPSize = zelsiusJoinedRow.extEEPSize;
        byte[] eePdata = zelsiusJoinedRow.EEPdata;
        TheIdent.TypeOverrideString = zelsiusJoinedRow.TypeOverrideString;
        int length = TheIdent.extEEPSize;
        if (eePdata.Length < length)
          length = eePdata.Length;
        if (eePdata.Length > TheIdent.extEEPSize)
        {
          for (int extEepSize = TheIdent.extEEPSize; extEepSize < eePdata.Length; ++extEepSize)
          {
            if (eePdata[extEepSize] > (byte) 0)
            {
              if (this.MyHandler.MyDataBaseAccess.IsMeterInfoProperty(TheIdent.MeterInfoID, DataBaseAccess.MeterInfoProperties.ExtendedTypePromSize, (string) null))
              {
                ZR_ClassLibMessages.AddWarning("EEProm data size out of database > eeprom space");
                length = eePdata.Length;
                break;
              }
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Database data set size > eeprom space");
              return false;
            }
          }
        }
        EpromData = new byte[length];
        for (int index = 0; index < length; ++index)
          EpromData[index] = eePdata[index];
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on MeterInfo");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      return true;
    }

    internal bool GetLastMeterInfoID(ZR_MeterIdent TheIdent)
    {
      Schema.MeterDataTable table = new Schema.MeterDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM Meter");
      this.SQL.Append(" WHERE MeterID = " + TheIdent.MeterID.ToString());
      try
      {
        TheIdent.MeterInfoID = this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table) != 1 ? TheIdent.MeterInfoBaseID : table[0].MeterInfoID;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on read meter ident data.");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      return true;
    }

    private bool GetBaseTypeId(ZR_MeterIdent TheIdent, int DefaultFunctionNr)
    {
      if (TheIdent.PPSArtikelNr == "BASETYPE")
      {
        TheIdent.MeterInfoBaseID = TheIdent.MeterInfoID;
      }
      else
      {
        Schema.MeterInfoDataTable table = new Schema.MeterInfoDataTable();
        this.SQL.Length = 0;
        this.SQL.Append("SELECT * FROM MeterInfo");
        this.SQL.Append(" WHERE MeterTypeID = " + TheIdent.MeterTypeID.ToString());
        this.SQL.Append(" AND   HardwareTypeID = " + TheIdent.HardwareTypeID.ToString());
        this.SQL.Append(" AND   MeterHardwareID = " + TheIdent.MeterHardwareID.ToString());
        this.SQL.Append(" AND   DefaultFunctionNr = '" + DefaultFunctionNr.ToString() + "'");
        this.SQL.Append(" AND   PPSArtikelNr = 'BASETYPE'");
        try
        {
          if (this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table) != 1)
          {
            if (table.Rows.Count == 0)
            {
              TheIdent.MeterInfoBaseID = TheIdent.MeterInfoID;
              return true;
            }
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "More then one Basetype");
            return false;
          }
          TheIdent.MeterInfoBaseID = table[0].MeterInfoID;
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "MeterInfo access error");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
          return false;
        }
      }
      return true;
    }

    internal bool GetMeterEpromData(
      ZR_MeterIdent TheMeterIdent,
      ref DateTime StorageTime,
      out byte[] EpromData)
    {
      EpromData = (byte[]) null;
      Schema.MeterDataDataTable meterDataDataTable = new Schema.MeterDataDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM MeterData");
      this.SQL.Append(" WHERE MeterID = " + TheMeterIdent.MeterID.ToString());
      this.SQL.Append(" AND PValueId = " + 1.ToString());
      if (StorageTime != DateTime.MaxValue)
        this.SQL.Append(" AND TimePoint = @TimePoint");
      this.SQL.Append(" ORDER BY TimePoint DESC");
      try
      {
        ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
        DbParameter parameter = zrDataAdapter.SelectCommand.CreateParameter();
        parameter.DbType = DbType.DateTime;
        parameter.ParameterName = "@TimePoint";
        parameter.Value = (object) StorageTime;
        zrDataAdapter.SelectCommand.Parameters.Add((object) parameter);
        zrDataAdapter.Fill((DataTable) meterDataDataTable);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf MeterData");
        this.AddErrorText(ex.ToString());
        return false;
      }
      if (meterDataDataTable.Rows.Count < 1)
      {
        this.AddErrorText("Meter data not found");
        return false;
      }
      if (StorageTime == DateTime.MaxValue)
        StorageTime = meterDataDataTable[0].TimePoint;
      try
      {
        EpromData = meterDataDataTable[0].PValueBinary;
        if (!meterDataDataTable[0].IsPValueNull())
        {
          try
          {
            long num = long.Parse(meterDataDataTable[0].PValue);
            if (num > 0L)
              TheMeterIdent.lFirmwareVersion = num;
          }
          catch
          {
          }
        }
      }
      catch (Exception ex)
      {
        this.AddErrorText("Illegal eprom data at database!");
        this.AddErrorText(ex.ToString());
        return false;
      }
      return true;
    }

    internal bool GetOverrides(int MeterInfoID, out ArrayList Overrides)
    {
      Overrides = (ArrayList) null;
      Schema.TypeOverwriteParametersDataTable table = new Schema.TypeOverwriteParametersDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM TypeOverwriteParameters");
      this.SQL.Append(" WHERE MeterInfoID = " + MeterInfoID.ToString());
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf  TypeOverwriteParameters");
        this.AddErrorText(ex.ToString());
        return false;
      }
      Overrides = new ArrayList();
      try
      {
        IEnumerator enumerator = table.Rows.GetEnumerator();
        try
        {
          while (true)
          {
            if (enumerator.MoveNext())
            {
              Schema.TypeOverwriteParametersRow current = (Schema.TypeOverwriteParametersRow) enumerator.Current;
              OverrideID poid = (OverrideID) current.POID;
              string str1 = !current.IsParameterValueNull() ? current.ParameterValue : string.Empty;
              switch (poid)
              {
                case OverrideID.EnergyResolution:
                  string energyUnitString = MeterMath.GetTrueEnergyUnitString(str1);
                  if (!(str1 == ""))
                  {
                    str1 = energyUnitString;
                    break;
                  }
                  goto label_22;
                case OverrideID.VolumeResolution:
                  string volumeUnitString = MeterMath.GetTrueVolumeUnitString(str1);
                  if (!(str1 == ""))
                  {
                    str1 = volumeUnitString;
                    break;
                  }
                  goto label_22;
                case OverrideID.Input1Unit:
                case OverrideID.Input2Unit:
                  string str2 = MeterMath.GetTrueVolumeUnitString(str1);
                  if (str1 == "")
                    str2 = MeterMath.GetTrueEnergyUnitString(str1);
                  if (!(str1 == ""))
                  {
                    str1 = str2;
                    break;
                  }
                  goto label_22;
              }
              OverrideParameter overrideParameter = new OverrideParameter(poid, str1, true);
              Overrides.Add((object) overrideParameter);
            }
            else
              break;
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on read overrides from database.");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      return true;
label_22:
      throw new ArgumentException("Illegal unit string");
    }

    internal bool GetHardwareTables(
      ZR_MeterIdent theIdent,
      out ArrayList Map,
      out SortedList Includes,
      out ArrayList BlockLinkOrder)
    {
      Map = (ArrayList) null;
      Includes = (SortedList) null;
      BlockLinkOrder = (ArrayList) null;
      Schema.MapDefDataTable table1 = new Schema.MapDefDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM MapDef WHERE MapID = " + theIdent.MapId.ToString());
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table1);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle MapDef");
        this.AddErrorText(ex.ToString());
        return false;
      }
      if (table1.Rows.Count == 0)
        return false;
      Map = new ArrayList();
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        DataBaseAccess.MapEntry mapEntry = new DataBaseAccess.MapEntry(table1[index].ConstName, table1[index].cValue, table1[index].ByteSize);
        Map.Add((object) mapEntry);
      }
      Schema.IncludeDefDataTable table2 = new Schema.IncludeDefDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM IncludeDef WHERE MapID = " + theIdent.MapId.ToString());
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table2);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle IncludeDef");
        this.AddErrorText(ex.ToString());
        return false;
      }
      if (table2.Rows.Count == 0)
        return false;
      Includes = new SortedList();
      for (int index = 0; index < table2.Rows.Count; ++index)
        Includes.Add((object) table2[index].ConstName, (object) table2[index].cValue);
      Schema.LinkerTableDataTable table3 = new Schema.LinkerTableDataTable();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM LinkerTable WHERE LinkerTableID = " + theIdent.LinkerTableID.ToString());
      this.SQL.Append(" ORDER BY BlockPosition");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table3);
        if (table3.Rows.Count == 0)
          return false;
        BlockLinkOrder = new ArrayList();
        BlockLinkOrder.Add((object) new BlockLinkDefines()
        {
          BlockType = LinkBlockTypes.EpromHeader
        });
        for (int index = 0; index < table3.Rows.Count; ++index)
        {
          BlockLinkDefines blockLinkDefines = new BlockLinkDefines();
          blockLinkDefines.BlockType = (LinkBlockTypes) table3[index].BlockId;
          if (table3[index].AddressRangeCPU.ToString()[0] == '1')
            blockLinkDefines.AddressRangeCPU = true;
          blockLinkDefines.AddressRangePosition = table3[index].AddressRangePosition;
          blockLinkDefines.AddressRangeVariable = !table3[index].IsAddressRangeVariableNull() ? table3[index].AddressRangeVariable : string.Empty;
          BlockLinkOrder.Add((object) blockLinkDefines);
        }
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle LinkerTable");
        this.AddErrorText(ex.ToString());
        return false;
      }
      return true;
    }

    internal bool GetFunctionNumbersList(ZR_MeterIdent TheIdent, out ArrayList NiededFunctions)
    {
      long num = 1;
      if (TheIdent.lFirmwareVersion >= 33554432L)
        num = TheIdent.lFirmwareVersion & 4095L;
      NiededFunctions = new ArrayList();
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM ZRFunction");
      this.SQL.Append(" WHERE FirmwareVersionMin <= " + TheIdent.lFirmwareVersion.ToString());
      this.SQL.Append(" AND FirmwareVersionMax >= " + TheIdent.lFirmwareVersion.ToString());
      this.SQL.Append(" AND HardwareResource like '%" + TheIdent.HardwareName + "%'");
      this.SQL.Append(" ORDER BY FunctionGroup,FunctionName,FunctionVersion DESC");
      Schema.ZRFunctionDataTable table = new Schema.ZRFunctionDataTable();
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          if (((long) table[index].FirmwareVersionMax & num) == 0L)
          {
            ushort functionNumber = (ushort) table[index].FunctionNumber;
            NiededFunctions.Add((object) functionNumber);
          }
        }
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle ZRFunction");
        this.AddErrorText(ex.ToString());
        return false;
      }
      return true;
    }

    internal bool GetFunctionHeaders(ArrayList FunctionList, Schema.ZRFunctionDataTable TheTable)
    {
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM ZRFunction WHERE FunctionNumber IN (");
      if (FunctionList.Count == 0)
        return false;
      foreach (ushort function in FunctionList)
        this.SQL.Append(function.ToString() + ",");
      --this.SQL.Length;
      this.SQL.Append(")");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) TheTable);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle ZRFunction");
        this.AddErrorText(ex.ToString());
        return false;
      }
      return TheTable.Rows.Count == FunctionList.Count;
    }

    internal bool GetFunctionParameters(
      ArrayList FunctionList,
      Schema.ZRParameterDataTable TheTable)
    {
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM ZRParameter WHERE FunctionNumber IN (");
      if (FunctionList.Count == 0)
        return false;
      foreach (ushort function in FunctionList)
        this.SQL.Append(function.ToString() + ",");
      --this.SQL.Length;
      this.SQL.Append(") ORDER BY FunctionNumber,StructureNr,StructureIndex");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) TheTable);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle ZRParameter");
        this.AddErrorText(ex.ToString());
        return false;
      }
      return true;
    }

    internal bool GetRuntimeCode(
      ArrayList FunctionList,
      DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable TheTable)
    {
      if (FunctionList.Count == 0)
        return false;
      this.SQL.Length = 0;
      this.SQL.Append("SELECT");
      this.SQL.Append(" RuntimeCode.FunctionNumber");
      this.SQL.Append(",RuntimeCode.CodeID");
      this.SQL.Append(",RuntimeCode.CodeSequenceType");
      this.SQL.Append(",RuntimeCode.CodeSequenceInfo");
      this.SQL.Append(",Code.LineNr");
      this.SQL.Append(",Code.CodeType");
      this.SQL.Append(",Code.CodeValue");
      this.SQL.Append(",Code.LineInfo");
      this.SQL.Append(",RuntimeCode.CodeSequenceName");
      this.SQL.Append(" FROM RuntimeCode, Code WHERE FunctionNumber IN (");
      foreach (ushort function in FunctionList)
        this.SQL.Append(function.ToString() + ",");
      --this.SQL.Length;
      this.SQL.Append(") AND RuntimeCode.CodeID = Code.CodeID");
      this.SQL.Append(" ORDER BY FunctionNumber, RuntimeCode.CodeID, LineNr");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) TheTable);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle RuntimeCode");
        this.AddErrorText(ex.ToString());
        return false;
      }
      return true;
    }

    internal DataBaseAccess.LoggerEntryData GetLoggerEntriesInfos(int FunctionNumber)
    {
      if (this.LoggerEntriesList == null)
      {
        Schema.DataloggerDataTable table = new Schema.DataloggerDataTable();
        try
        {
          this.getDataTableForSQLCommand("SELECT * FROM Datalogger", (DataTable) table);
        }
        catch (Exception ex)
        {
          throw new Exception("Table Datalogger wurde nicht geöffnet", ex);
        }
        this.LoggerEntriesList = new SortedList<int, DataBaseAccess.LoggerEntryData>();
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          Schema.DataloggerRow dataloggerRow = table[index];
          int functionNumber = dataloggerRow.FunctionNumber;
          DataBaseAccess.LoggerEntryData loggerEntryData = new DataBaseAccess.LoggerEntryData();
          int loggerType = (int) dataloggerRow.LoggerType;
          loggerEntryData.Entries = (int) dataloggerRow.MaxLoggerEntries;
          loggerEntryData.EntrySize = loggerType != 3 ? 0 : (int) dataloggerRow.EntrySize;
          loggerEntryData.Interval = dataloggerRow.LoggerInterval;
          this.LoggerEntriesList.Add(functionNumber, loggerEntryData);
        }
      }
      int index1 = this.LoggerEntriesList.IndexOfKey(FunctionNumber);
      return index1 >= 0 ? this.LoggerEntriesList.Values[index1] : new DataBaseAccess.LoggerEntryData();
    }

    internal bool GetMenus(ArrayList FunctionList, Schema.MenuDataTable TheTable)
    {
      this.SQL.Length = 0;
      this.SQL.Append("SELECT * FROM Menu WHERE FunctionNumber IN (");
      if (FunctionList.Count == 0)
        return false;
      foreach (ushort function in FunctionList)
        this.SQL.Append(function.ToString() + ",");
      --this.SQL.Length;
      this.SQL.Append(") ORDER BY FunctionNumber,XPos,YPos");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) TheTable);
      }
      catch
      {
        return this.MyHandler.AddErrorPointMessage("Fehler : Datenbankzugriff auf die Tabelle Menu");
      }
      return true;
    }

    internal bool GetDisplayCodes(
      string CodeList,
      DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable TheTable)
    {
      this.SQL.Length = 0;
      this.SQL.Append("SELECT");
      this.SQL.Append(" DisplayCode.InterpreterCode");
      this.SQL.Append(",DisplayCode.SequenceNr");
      this.SQL.Append(",DisplayCode.CodeSequenceType");
      this.SQL.Append(",DisplayCode.CodeSequenceInfo");
      this.SQL.Append(",DisplayCode.CodeID");
      this.SQL.Append(",Code.LineNr");
      this.SQL.Append(",Code.CodeType");
      this.SQL.Append(",Code.CodeValue");
      this.SQL.Append(",Code.LineInfo");
      this.SQL.Append("  FROM DisplayCode, Code WHERE InterpreterCode IN (");
      if (CodeList.Length == 0)
        return false;
      this.SQL.Append(CodeList);
      this.SQL.Append(") AND DisplayCode.CodeID = Code.CodeID");
      this.SQL.Append(" ORDER BY InterpreterCode,SequenceNr,LineNr");
      try
      {
        this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) TheTable);
      }
      catch (Exception ex)
      {
        this.AddErrorText("Fehler : Datenbankzugriff auf die Tabelle DisplayCode");
        this.AddErrorText(ex.ToString());
        return false;
      }
      return true;
    }

    private void AddErrorText(string TheText)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, TheText);
    }

    internal DataBaseAccess.PValueDescription GetValueDescriptions(
      Meter TheMeter,
      string ZDF_ParameterID)
    {
      if (this.LastMeter == null || this.LastMeter != TheMeter)
      {
        string manufacturer = MBusDevice.GetManufacturer((short) ((Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusManufacturer"]).ValueEprom);
        string str = ((Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusMeterType"]).ValueEprom.ToString();
        if (this.ManufacturerString != manufacturer || this.MeterGenerationString != str)
        {
          this.PValDescriptionCache = (SortedList<string, DataBaseAccess.PValueDescription>) null;
          this.ManufacturerString = manufacturer;
          this.MeterGenerationString = str;
        }
        this.LastMeter = TheMeter;
      }
      if (this.PValDescriptionCache == null)
      {
        try
        {
          ZRDataAdapter zrDataAdapter1 = this.MyTypedDB.ZRDataAdapter("SELECT * FROM PValueIdent WHERE PValueID > 999 ORDER BY PValueID", this.MyDbConnection);
          Schema.PValueIdentDataTable pvalueIdentDataTable = new Schema.PValueIdentDataTable();
          zrDataAdapter1.Fill((DataTable) pvalueIdentDataTable);
          ZRDataAdapter zrDataAdapter2 = this.MyTypedDB.ZRDataAdapter("SELECT * FROM MBusParameterTranslation WHERE Manufacturer = '" + this.ManufacturerString + "' AND PValueID > 999 AND VersionMin <= " + this.MeterGenerationString + " AND VersionMax >= " + this.MeterGenerationString + " ORDER BY PValueID", this.MyDbConnection);
          Schema.MBusParameterTranslationDataTable translationDataTable = new Schema.MBusParameterTranslationDataTable();
          zrDataAdapter2.Fill((DataTable) translationDataTable);
          this.PValDescriptionCache = new SortedList<string, DataBaseAccess.PValueDescription>();
          foreach (Schema.MBusParameterTranslationRow parameterTranslationRow in (TypedTableBase<Schema.MBusParameterTranslationRow>) translationDataTable)
          {
            Schema.PValueIdentRow[] pvalueIdentRowArray = (Schema.PValueIdentRow[]) pvalueIdentDataTable.Select("PValueID = " + parameterTranslationRow.PValueID.ToString(), "PValueID");
            if (pvalueIdentRowArray.Length >= 1 && this.PValDescriptionCache.IndexOfKey(parameterTranslationRow.MBusZDF) < 0)
              this.PValDescriptionCache.Add(parameterTranslationRow.MBusZDF, new DataBaseAccess.PValueDescription()
              {
                PValueID = parameterTranslationRow.PValueID,
                ValueName = pvalueIdentRowArray[0].PValueName,
                Unit = pvalueIdentRowArray[0].Unit,
                ValueDescription = pvalueIdentRowArray[0].Description
              });
          }
        }
        catch
        {
          return (DataBaseAccess.PValueDescription) null;
        }
      }
      int index = this.PValDescriptionCache.IndexOfKey(ZDF_ParameterID);
      return index >= 0 ? this.PValDescriptionCache.Values[index] : (DataBaseAccess.PValueDescription) null;
    }

    internal bool IsDatabaseSwitchTrue(string Switch)
    {
      if (this.DatabaseIdentTable == null)
      {
        ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter("SELECT * FROM DatabaseIdentification WHERE InfoName = '" + Switch + "'", this.MyDbConnection);
        this.DatabaseIdentTable = new Schema.DatabaseIdentificationDataTable();
        zrDataAdapter.Fill((DataTable) this.DatabaseIdentTable);
      }
      Schema.DatabaseIdentificationRow[] identificationRowArray = (Schema.DatabaseIdentificationRow[]) this.DatabaseIdentTable.Select("InfoName = '" + Switch + "'");
      return identificationRowArray.Length == 1 && identificationRowArray[0].InfoData == "true";
    }

    internal bool WriteDailyMeterData(Meter ReadMeter)
    {
      try
      {
        if (!ReadMeter.MyHandler.BackupForEachReadInternal)
        {
          if (this.AutoWriteIdentTable == null)
          {
            this.AutoWriteIdentTable = new Schema.DatabaseIdentificationDataTable();
            this.MyTypedDB.ZRDataAdapter("SELECT * FROM DatabaseIdentification WHERE InfoName = 'DatabaseSaveOption'", this.MyDbConnection).Fill((DataTable) this.AutoWriteIdentTable);
          }
          if (this.AutoWriteIdentTable.Rows.Count == 1 && this.AutoWriteIdentTable[0].InfoData == "DailyAutosaveOff")
            return true;
          Schema.MeterDataDataTable table = new Schema.MeterDataDataTable();
          this.SQL.Length = 0;
          this.SQL.Append("SELECT * FROM MeterData WHERE MeterID = ");
          this.SQL.Append(ReadMeter.MyIdent.MeterID.ToString());
          this.SQL.Append(" AND PValueID = 1");
          this.getDataTableForSQLCommand(this.SQL.ToString(), (DataTable) table);
          foreach (Schema.MeterDataRow row in (InternalDataCollectionBase) table.Rows)
          {
            if (row.TimePoint.Date == DateTime.Now.Date)
              return true;
          }
        }
        ReadMeter.DatabaseTime = DateTime.Now;
        if (ReadMeter.MyHandler.LoggerRestoreState == 0)
          return this.WriteMeterData(ReadMeter.Eprom, ReadMeter.MyLoggerStore.BlockStartAddress, ReadMeter.MyIdent, ReadMeter.DatabaseTime);
        ReadMeter.ReadConnectedMeterLoggerData();
        return this.WriteMeterData(ReadMeter.Eprom, ReadMeter.UsedEpromSize, ReadMeter.MyIdent, ReadMeter.DatabaseTime);
      }
      catch (Exception ex)
      {
        return this.MyHandler.AddErrorPointMessage("Database read error" + ZR_Constants.SystemNewLine + ex.ToString());
      }
    }

    internal bool WriteMeterData(
      byte[] EEProm,
      int EpromSize,
      ZR_MeterIdent TheMeterIdent,
      DateTime TimePoint)
    {
      TimePoint = new DateTime(TimePoint.Year, TimePoint.Month, TimePoint.Day, TimePoint.Hour, TimePoint.Minute, TimePoint.Second);
      byte[] numArray = new byte[EpromSize];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = EEProm[index];
      this.MyDbConnection.Open();
      IDbTransaction Transaction = this.MyDbConnection.BeginTransaction();
      try
      {
        Schema.MeterDataTable MyDataTable1 = new Schema.MeterDataTable();
        this.SQL.Length = 0;
        this.SQL.Append("SELECT * FROM Meter WHERE MeterID = ");
        this.SQL.Append(TheMeterIdent.MeterID.ToString());
        ZRDataAdapter zrDataAdapter1 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        if (MyDataTable1.Rows.Count == 0)
        {
          Schema.MeterRow row = (Schema.MeterRow) MyDataTable1.NewRow();
          row.MeterID = TheMeterIdent.MeterID;
          row.MeterInfoID = TheMeterIdent.MeterInfoID;
          row.SerialNr = TheMeterIdent.SerialNr;
          row.ProductionDate = TimePoint;
          row.OrderNr = "??";
          MyDataTable1.Rows.Add((DataRow) row);
          zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction);
        }
        else
        {
          MyDataTable1[0].MeterInfoID = TheMeterIdent.MeterInfoID;
          MyDataTable1[0].SerialNr = TheMeterIdent.SerialNr;
          zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction);
        }
        Schema.MeterDataDataTable MyDataTable2 = new Schema.MeterDataDataTable();
        this.SQL.Length = 0;
        this.SQL.Append("SELECT * FROM Meterdata WHERE MeterID = -1");
        ZRDataAdapter zrDataAdapter2 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
        zrDataAdapter2.Fill((DataTable) MyDataTable2, Transaction);
        Schema.MeterDataRow row1 = (Schema.MeterDataRow) MyDataTable2.NewRow();
        row1.MeterID = TheMeterIdent.MeterID;
        row1.TimePoint = TimePoint;
        row1.PValueID = 1;
        row1.PValue = TheMeterIdent.lFirmwareVersion.ToString();
        row1.PValueBinary = numArray;
        MyDataTable2.Rows.Add((DataRow) row1);
        zrDataAdapter2.Update((DataTable) MyDataTable2, Transaction);
      }
      catch (Exception ex)
      {
        this.MyHandler.AddErrorPointMessage("Database write Error" + ZR_Constants.SystemNewLine + ex.ToString());
        goto label_10;
      }
      Transaction.Commit();
      this.MyDbConnection.Close();
      return true;
label_10:
      Transaction.Rollback();
      this.MyDbConnection.Close();
      return false;
    }

    internal bool SaveType(Meter TheMeter, ZR_MeterIdent TheIdent)
    {
      string empty = string.Empty;
      IDbTransaction Transaction = (IDbTransaction) null;
      try
      {
        this.MyDbConnection.Open();
        Transaction = this.MyDbConnection.BeginTransaction();
        Schema.MeterInfoDataTable MyDataTable1 = new Schema.MeterInfoDataTable();
        this.SQL.Length = 0;
        this.SQL.Append("SELECT * FROM MeterInfo WHERE MeterInfoID = ");
        this.SQL.Append(TheIdent.MeterInfoID.ToString());
        ZRDataAdapter zrDataAdapter1 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        if (MyDataTable1.Rows.Count == 1)
        {
          MyDataTable1[0].PPSArtikelNr = TheIdent.PPSArtikelNr;
          MyDataTable1[0].Description = TheIdent.MeterInfoDescription;
          if (zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction) == 1)
          {
            Schema.MeterTypeDataTable MyDataTable2 = new Schema.MeterTypeDataTable();
            this.SQL.Length = 0;
            this.SQL.Append("SELECT * FROM MeterType WHERE MeterTypeID = ");
            this.SQL.Append(TheIdent.MeterTypeID.ToString());
            ZRDataAdapter zrDataAdapter2 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
            zrDataAdapter2.Fill((DataTable) MyDataTable2, Transaction);
            if (MyDataTable2.Rows.Count == 1)
            {
              MyDataTable2[0].GenerateDate = DateTime.Now;
              if (zrDataAdapter2.Update((DataTable) MyDataTable2, Transaction) == 1)
              {
                Schema.MTypeZelsiusDataTable MyDataTable3 = new Schema.MTypeZelsiusDataTable();
                this.SQL.Length = 0;
                this.SQL.Append("SELECT * FROM MTypeZelsius WHERE MeterTypeID = ");
                this.SQL.Append(TheIdent.MeterTypeID.ToString());
                ZRDataAdapter zrDataAdapter3 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
                zrDataAdapter3.Fill((DataTable) MyDataTable3, Transaction);
                if (MyDataTable3.Rows.Count == 1)
                {
                  byte[] numArray = new byte[TheMeter.MyLoggerStore.BlockStartAddress];
                  for (int index = 0; index < numArray.Length; ++index)
                    numArray[index] = TheMeter.Eprom[index];
                  MyDataTable3[0].EEPdata = numArray;
                  MyDataTable3[0].TypeOverrideString = TheIdent.TypeOverrideString;
                  if (zrDataAdapter3.Update((DataTable) MyDataTable3, Transaction) != 1)
                    goto label_12;
                }
                else
                  goto label_12;
              }
              else
                goto label_12;
            }
            else
              goto label_12;
          }
          else
            goto label_12;
        }
        else
          goto label_12;
      }
      catch (Exception ex)
      {
        empty = ex.ToString();
        goto label_12;
      }
      Transaction.Commit();
      this.MyDbConnection.Close();
      return true;
label_12:
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on write type to database");
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, empty);
      Transaction.Rollback();
      this.MyDbConnection.Close();
      return false;
    }

    internal bool SaveAsNewType(Meter TheMeter, ZR_MeterIdent NewTypeIdent)
    {
      string ErrMsg = string.Empty;
      IDbTransaction Transaction = (IDbTransaction) null;
      try
      {
        this.MyDbConnection.Open();
        Transaction = this.MyDbConnection.BeginTransaction();
        long newID1;
        if (NewTypeIdent.MeterInfoID > 0)
        {
          newID1 = (long) NewTypeIdent.MeterInfoID;
        }
        else
        {
          this.ret = this.MyDB.getNewID("MeterInfo", "MeterInfoID", out newID1, out ErrMsg);
          if (this.ret != 0)
            goto label_18;
        }
        long newID2;
        if (NewTypeIdent.MeterInfoID > 0 && NewTypeIdent.MeterTypeID > 0)
        {
          newID2 = (long) NewTypeIdent.MeterTypeID;
        }
        else
        {
          this.ret = this.MyDB.getNewID("MeterType", "MeterTypeID", out newID2, out ErrMsg);
          if (this.ret != 0)
            goto label_18;
        }
        string str1 = ((Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterInfoID"]).ValueEprom.ToString();
        Schema.MeterInfoDataTable MyDataTable1 = new Schema.MeterInfoDataTable();
        this.SQL.Length = 0;
        this.SQL.Append("SELECT * FROM MeterInfo WHERE MeterInfoID = ");
        this.SQL.Append(str1);
        ZRDataAdapter zrDataAdapter1 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        if (MyDataTable1.Rows.Count == 1)
        {
          string str2 = MyDataTable1[0].MeterTypeID.ToString();
          Schema.MeterTypeDataTable MyDataTable2 = new Schema.MeterTypeDataTable();
          this.SQL.Length = 0;
          this.SQL.Append("SELECT * FROM MeterType WHERE MeterTypeID = ");
          this.SQL.Append(str2);
          ZRDataAdapter zrDataAdapter2 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
          zrDataAdapter2.Fill((DataTable) MyDataTable2, Transaction);
          if (MyDataTable2.Rows.Count == 1)
          {
            string mtypeTableName = MyDataTable2[0].MTypeTableName;
            Schema.MTypeZelsiusDataTable MyDataTable3 = new Schema.MTypeZelsiusDataTable();
            this.SQL.Length = 0;
            this.SQL.Append("SELECT * FROM " + mtypeTableName + " WHERE MeterTypeID = ");
            this.SQL.Append(str2);
            ZRDataAdapter zrDataAdapter3 = this.MyTypedDB.ZRDataAdapter(this.SQL.ToString(), this.MyDbConnection);
            zrDataAdapter3.Fill((DataTable) MyDataTable3, Transaction);
            if (MyDataTable3.Rows.Count == 1)
            {
              Schema.MeterInfoRow row1 = MyDataTable1.NewMeterInfoRow();
              row1.MeterInfoID = (int) newID1;
              row1.MeterTypeID = (int) newID2;
              row1.PPSArtikelNr = NewTypeIdent.PPSArtikelNr;
              row1.Description = NewTypeIdent.MeterInfoDescription;
              row1.DefaultFunctionNr = MyDataTable1[0].DefaultFunctionNr;
              row1.HardwareTypeID = MyDataTable1[0].HardwareTypeID;
              row1.MeterHardwareID = MyDataTable1[0].MeterHardwareID;
              MyDataTable1.AddMeterInfoRow(row1);
              if (zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction) == 1)
              {
                Schema.MeterTypeRow row2 = MyDataTable2.NewMeterTypeRow();
                row2.MeterTypeID = (int) newID2;
                row2.GenerateDate = DateTime.Now;
                row2.Description = "Auto generated";
                row2.MTypeTableName = MyDataTable2[0].MTypeTableName;
                MyDataTable2.Rows.Add((DataRow) row2);
                if (zrDataAdapter2.Update((DataTable) MyDataTable2, Transaction) == 1)
                {
                  byte[] numArray = new byte[TheMeter.MyLoggerStore.BlockStartAddress];
                  for (int index = 0; index < numArray.Length; ++index)
                    numArray[index] = TheMeter.Eprom[index];
                  Schema.MTypeZelsiusRow row3 = MyDataTable3.NewMTypeZelsiusRow();
                  row3.MeterTypeID = (int) newID2;
                  row3.EEPdata = numArray;
                  row3.TypeOverrideString = NewTypeIdent.TypeOverrideString;
                  MyDataTable3.Rows.Add((DataRow) row3);
                  if (zrDataAdapter3.Update((DataTable) MyDataTable3, Transaction) != 1)
                    goto label_18;
                }
                else
                  goto label_18;
              }
              else
                goto label_18;
            }
            else
              goto label_18;
          }
          else
            goto label_18;
        }
        else
          goto label_18;
      }
      catch (Exception ex)
      {
        ErrMsg = ex.ToString();
        goto label_18;
      }
      Transaction.Commit();
      this.MyDbConnection.Close();
      return true;
label_18:
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on write type to database");
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ErrMsg);
      Transaction.Rollback();
      this.MyDbConnection.Close();
      return false;
    }

    internal bool LoadTypeList()
    {
      ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter("SELECT MeterInfo.MeterInfoID, MeterInfo.MeterTypeID, MeterInfo.MeterHardwareID, MeterInfo.HardwareTypeID,MeterInfo.PPSArtikelNr,MeterInfo.DefaultFunctionNr,MeterInfo.Description FROM MeterInfo,MeterType WHERE MeterInfo.MeterTypeID = MeterType.MeterTypeID AND MeterType.MTypeTableName = 'MTypeZelsius'", this.MyDbConnection);
      this.MeterInfoTable = new Schema.MeterInfoDataTable();
      zrDataAdapter.Fill((DataTable) this.MeterInfoTable);
      return true;
    }

    internal bool IsMeterInfoProperty(
      int MeterInfoId,
      DataBaseAccess.MeterInfoProperties TheProperty,
      string PropertyValue)
    {
      if (this.MeterInfoPropertiesTable == null)
      {
        ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter("SELECT MeterInfoProperties.MeterInfoID, MeterInfoProperties.PropertyField1 FROM MeterInfo,MeterInfoProperties WHERE MeterInfo.MeterInfoID = MeterInfoProperties.MeterInfoID", this.MyDbConnection);
        this.MeterInfoPropertiesTable = new Schema.MeterInfoPropertiesDataTable();
        zrDataAdapter.Fill((DataTable) this.MeterInfoPropertiesTable);
        this.AllMeterInfoProperties = new SortedList<int, SortedList<DataBaseAccess.MeterInfoProperties, string>>();
        string[] names = Enum.GetNames(typeof (DataBaseAccess.MeterInfoProperties));
        for (int index = 0; index < this.MeterInfoPropertiesTable.Count; ++index)
        {
          Schema.MeterInfoPropertiesRow infoPropertiesRow = this.MeterInfoPropertiesTable[index];
          SortedList<DataBaseAccess.MeterInfoProperties, string> sortedList = new SortedList<DataBaseAccess.MeterInfoProperties, string>();
          string propertyField1 = this.MeterInfoPropertiesTable[index].PropertyField1;
          char[] chArray = new char[1]{ '|' };
          foreach (string str1 in propertyField1.Split(chArray))
          {
            string str2 = str1.Trim();
            if (str2.Length != 0)
            {
              for (int key = 0; key < names.Length; ++key)
              {
                if (str2.StartsWith(names[key]))
                {
                  if (str2.Length == names[key].Length)
                  {
                    sortedList.Add((DataBaseAccess.MeterInfoProperties) key, (string) null);
                    break;
                  }
                  if (str2.Length > names[key].Length + 1 && str2[names[key].Length] == '=')
                  {
                    sortedList.Add((DataBaseAccess.MeterInfoProperties) key, str2.Substring(names[key].Length + 1));
                    break;
                  }
                }
              }
            }
          }
          this.AllMeterInfoProperties.Add(this.MeterInfoPropertiesTable[index].MeterInfoId, sortedList);
        }
      }
      int index1 = this.AllMeterInfoProperties.IndexOfKey(MeterInfoId);
      if (index1 < 0)
        return false;
      int index2 = this.AllMeterInfoProperties.Values[index1].IndexOfKey(TheProperty);
      if (index2 < 0)
        return false;
      if (PropertyValue == null)
      {
        if (this.AllMeterInfoProperties.Values[index1].Values[index2] == null)
          return true;
      }
      else if (this.AllMeterInfoProperties.Values[index1].Values[index2] != null && this.AllMeterInfoProperties.Values[index1].Values[index2] == PropertyValue)
        return true;
      return false;
    }

    internal bool LoadMeterList()
    {
      ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter("SELECT Meter.MeterID,Meter.MeterInfoID,Meter.SerialNr,Meter.ProductionDate,Meter.ApprovalDate,Meter.OrderNr FROM Meter,MeterInfo,MeterType WHERE Meter.MeterInfoID = MeterInfo.MeterInfoID AND MeterInfo.MeterTypeID = MeterType.MeterTypeID AND MeterType.MTypeTableName = 'MTypeZelsius' ORDER BY Meter.ProductionDate", this.MyDbConnection);
      this.MeterTable = new Schema.MeterDataTable();
      zrDataAdapter.Fill((DataTable) this.MeterTable);
      return true;
    }

    internal bool LoadMeterListFromTypeID(
      DataSetAllErr8002Meters.Err8002MeterDataTable TheDataTable,
      int MeterTypeID)
    {
      try
      {
        this.MyTypedDB.ZRDataAdapter("SELECT Meter.MeterID,Meter.MeterInfoID,Meter.SerialNr,Meter.ProductionDate,Meter.ApprovalDate,MeterInfo.PPSArtikelNr AS SAP_Number,MeterInfo.Description AS Kurztext FROM Meter,MeterInfo WHERE Meter.MeterInfoID = MeterInfo.MeterInfoID AND MeterInfo.MeterTypeID = " + MeterTypeID.ToString(), this.MyDbConnection).Fill((DataTable) TheDataTable);
      }
      catch (Exception ex)
      {
        this.ErrorMsg = ex.ToString();
        return false;
      }
      return true;
    }

    internal DateTime GetLastProgDate(int MeterID)
    {
      DateTime minValue = DateTime.MinValue;
      try
      {
        ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter("SELECT TimePoint, MeterID FROM MeterData WHERE MeterId = " + MeterID.ToString() + " AND PValueID = 1 ORDER BY TimePoint DESC", this.MyDbConnection);
        DataTable dataTable = new DataTable();
        zrDataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count > 0)
          return (DateTime) dataTable.Rows[0]["TimePoint"];
      }
      catch
      {
      }
      return minValue;
    }

    internal bool IsSerie2Type(int MeterTypeID)
    {
      if (this.MTypeZelsiusTable == null)
      {
        ZRDataAdapter zrDataAdapter = this.MyTypedDB.ZRDataAdapter("SELECT * FROM MTypeZelsius", this.MyDbConnection);
        this.MTypeZelsiusTable = new Schema.MTypeZelsiusDataTable();
        zrDataAdapter.Fill((DataTable) this.MTypeZelsiusTable);
      }
      return ((Schema.MTypeZelsiusRow[]) this.MTypeZelsiusTable.Select("MeterTypeId = " + MeterTypeID.ToString())).Length == 1;
    }

    internal class MapEntry
    {
      internal string Name;
      internal int Address;
      internal short ByteSize;

      internal MapEntry(string TheName, int TheAddress, short TheByteSize)
      {
        this.Name = TheName;
        this.Address = TheAddress;
        this.ByteSize = TheByteSize;
      }
    }

    internal class LoggerEntryData
    {
      internal int Entries;
      internal int EntrySize;
      internal int Interval;
    }

    public class PValueDescription
    {
      public int PValueID;
      public string ValueName;
      public string Unit;
      public string ValueDescription;
    }

    public enum MeterInfoProperties
    {
      ExtendedTypePromSize,
      RuntimeVarsPointerToEEProm,
      DatasetError,
      OverridesChanged,
    }
  }
}
