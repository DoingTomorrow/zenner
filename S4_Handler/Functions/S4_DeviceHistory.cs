// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_DeviceHistory
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_DeviceHistory
  {
    private StringBuilder TheHistory;
    private S4_HandlerFunctions MyFunctions;

    internal S4_DeviceHistory(S4_HandlerFunctions myFunctions)
    {
      this.MyFunctions = myFunctions;
      this.TheHistory = new StringBuilder();
      this.AddHeader("IUW device history");
      StringBuilder theHistory = this.TheHistory;
      DateTime now = DateTime.Now;
      string longDateString = now.ToLongDateString();
      now = DateTime.Now;
      string longTimeString = now.ToLongTimeString();
      string str = "History creation time: " + longDateString + " " + longTimeString;
      theHistory.AppendLine(str);
      this.TheHistory.AppendLine();
    }

    internal void AddHeader(string headerText)
    {
      if (this.TheHistory.Length > Environment.NewLine.Length)
      {
        string str = this.TheHistory.ToString();
        if (!str.EndsWith(Environment.NewLine + Environment.NewLine))
        {
          if (!str.EndsWith(Environment.NewLine))
            this.TheHistory.AppendLine();
          this.TheHistory.AppendLine();
        }
      }
      this.TheHistory.AppendLine("***************************************************");
      this.TheHistory.AppendLine("*** " + headerText);
      this.TheHistory.AppendLine("***************************************************");
    }

    internal async Task PrepareHistory(
      ProgressHandler progress,
      CancellationToken cancellationToken)
    {
      List<Exception> allExceptions = new List<Exception>();
      try
      {
        S4_Meter connectedMeter = this.MyFunctions.myMeters.ConnectedMeter;
        S4_DeviceIdentification ident = connectedMeter.deviceIdentification;
        this.TheHistory.AppendLine("Serial number: " + ident.PrintedSerialNumberAsString);
        this.TheHistory.AppendLine("MeterID: ..... " + ident.MeterID.Value.ToString());
        this.TheHistory.AppendLine();
        S4_CurrentData currentData = await this.MyFunctions.ReadCurrentDataAsync(progress, cancellationToken);
        this.AddHeader("Current device data");
        this.TheHistory.AppendLine(currentData.ToTextBlock());
        this.AddHeader("Enhanced device identification ***");
        this.TheHistory.AppendLine(ident.ToString());
        this.TheHistory.AppendLine();
        S4_FunctionalState functionalState = await this.MyFunctions.ReadAlliveAndStateAsync(progress, cancellationToken);
        this.AddHeader("Functional state");
        this.TheHistory.AppendLine(functionalState.ToTextBlock());
        S4_SystemState deviceState = await this.MyFunctions.GetDeviceState(progress, cancellationToken);
        this.AddHeader("Diagnostic state");
        this.TheHistory.AppendLine(deviceState.ToTextBlock());
        DeviceStateCounter stateCounter = await this.MyFunctions.ReadStateCounterAsync(progress, cancellationToken);
        this.AddHeader("Current state counters");
        this.TheHistory.AppendLine(stateCounter.ToTextBlock());
        ObservableCollection<LoggerListItem> loggerList = await this.MyFunctions.ReadLoggerListAsync(progress, cancellationToken);
        this.AddHeader("Logger overview");
        foreach (LoggerListItem logger in (Collection<LoggerListItem>) loggerList)
        {
          this.TheHistory.Append(logger.LoggerName);
          StringBuilder theHistory1 = this.TheHistory;
          ushort num = logger.Entries;
          string str1 = "; Stored entries:" + num.ToString();
          theHistory1.Append(str1);
          StringBuilder theHistory2 = this.TheHistory;
          num = logger.MaxEntries;
          string str2 = "; Maximal entries:" + num.ToString();
          theHistory2.Append(str2);
          this.TheHistory.AppendLine();
        }
        this.AddHeader("Key date logger");
        await this.AddLogger("KeyDate", progress, cancellationToken);
        this.AddHeader("Month logger");
        await this.AddLogger("Month", progress, cancellationToken);
        this.AddHeader("Near past logger");
        await this.AddLogger("NearPast", progress, cancellationToken);
        this.AddHeader("Event logger");
        await this.AddLogger("Event", progress, cancellationToken);
        this.TheHistory.AppendLine();
        this.AddHeader("Device configuration");
        SortedList<OverrideID, ConfigurationParameter> connectedConfig = connectedMeter.GetConfigurationParameters("");
        foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in connectedConfig)
        {
          KeyValuePair<OverrideID, ConfigurationParameter> keyValue = keyValuePair;
          if (!keyValue.Value.IsFunction)
          {
            string winValue = keyValue.Value.GetStringValueWin();
            if (!(winValue == "NaN"))
            {
              OverrideID overrideId = keyValue.Key;
              string key = overrideId.ToString();
              string value = keyValue.Value.GetStringValueWin();
              this.TheHistory.Append(key + ": ");
              string str3 = key;
              overrideId = OverrideID.SmartFunctions;
              string str4 = overrideId.ToString();
              if (str3 != str4)
              {
                this.TheHistory.AppendLine(value);
              }
              else
              {
                this.TheHistory.AppendLine();
                string[] smartFunctions = value.Split(new char[1]
                {
                  ';'
                }, StringSplitOptions.RemoveEmptyEntries);
                string[] strArray = smartFunctions;
                for (int index = 0; index < strArray.Length; ++index)
                {
                  string smartFunction = strArray[index];
                  this.TheHistory.AppendLine("    " + smartFunction);
                  smartFunction = (string) null;
                }
                strArray = (string[]) null;
                smartFunctions = (string[]) null;
              }
              winValue = (string) null;
              key = (string) null;
              value = (string) null;
              keyValue = new KeyValuePair<OverrideID, ConfigurationParameter>();
            }
          }
        }
        S4_Meter backupMeter = (S4_Meter) null;
        try
        {
          this.AddHeader("Last database backup compare");
          if (!this.MyFunctions.LoadLastBackup((int) ident.MeterID.Value))
          {
            this.TheHistory.AppendLine("Last database backup not available");
          }
          else
          {
            backupMeter = this.MyFunctions.myMeters.BackupMeter;
            StringBuilder theHistory3 = this.TheHistory;
            DateTime dateTime = backupMeter.BackupTime.Value;
            string shortDateString = dateTime.ToShortDateString();
            dateTime = backupMeter.BackupTime.Value;
            string shortTimeString = dateTime.ToShortTimeString();
            string str5 = "Database backup time: " + shortDateString + " " + shortTimeString;
            theHistory3.AppendLine(str5);
            this.TheHistory.AppendLine();
            this.TheHistory.AppendLine("Real time clock calibration:");
            this.TheHistory.AppendLine("Last meter backup: " + backupMeter.meterMemory.GetParameterValue<short>(S4_Params.rtcCalibrationValue).ToString());
            this.TheHistory.AppendLine("Connected meter:   " + connectedMeter.meterMemory.GetParameterValue<short>(S4_Params.rtcCalibrationValue).ToString());
            if (backupMeter.meterMemory.IsParameterAvailable(S4_Params.cfg_radio_frequency_inc_dec) && connectedMeter.meterMemory.IsParameterAvailable(S4_Params.cfg_radio_frequency_inc_dec))
            {
              this.TheHistory.AppendLine();
              this.TheHistory.AppendLine("Radio frequency calibration:");
              StringBuilder theHistory4 = this.TheHistory;
              short parameterValue = backupMeter.meterMemory.GetParameterValue<short>(S4_Params.cfg_radio_frequency_inc_dec);
              string str6 = "Last meter backup: " + parameterValue.ToString();
              theHistory4.AppendLine(str6);
              StringBuilder theHistory5 = this.TheHistory;
              parameterValue = connectedMeter.meterMemory.GetParameterValue<short>(S4_Params.cfg_radio_frequency_inc_dec);
              string str7 = "Connected meter:   " + parameterValue.ToString();
              theHistory5.AppendLine(str7);
            }
            this.TheHistory.AppendLine();
            this.TheHistory.AppendLine("Connected meter changes in compare to backup meter:");
            SortedList<OverrideID, ConfigurationParameter> backupConfig = backupMeter.GetConfigurationParameters("");
            foreach (OverrideID theOverride in (IEnumerable<OverrideID>) backupConfig.Keys)
            {
              if (connectedConfig.ContainsKey(theOverride))
              {
                ConfigurationParameter connectedParameter = connectedConfig[theOverride];
                ConfigurationParameter backupParameter = backupConfig[theOverride];
                if (!connectedParameter.IsFunction)
                {
                  string connectedParameterValue = connectedParameter.GetStringValueWin();
                  string backupParameterValue = backupParameter.GetStringValueWin();
                  if (backupParameterValue != connectedParameterValue)
                    this.TheHistory.AppendLine(theOverride.ToString() + ": " + backupParameterValue + " -> " + connectedParameterValue);
                  connectedParameter = (ConfigurationParameter) null;
                  backupParameter = (ConfigurationParameter) null;
                  connectedParameterValue = (string) null;
                  backupParameterValue = (string) null;
                }
              }
            }
            backupConfig = (SortedList<OverrideID, ConfigurationParameter>) null;
          }
        }
        catch (Exception ex)
        {
          this.TheHistory.AppendLine();
          this.TheHistory.AppendLine("Database backup exception");
          this.TheHistory.AppendLine();
          allExceptions.Add(ex);
        }
        try
        {
          this.AddHeader("Type compare");
          uint? nullable = ident.SAP_MaterialNumber;
          uint SAP_Type = nullable.Value;
          S4_HandlerFunctions functions = this.MyFunctions;
          nullable = ident.MeterInfoID;
          int meterInfoID = (int) nullable.Value;
          functions.OpenType(meterInfoID);
          S4_Meter typeMeter = this.MyFunctions.myMeters.TypeMeter;
          this.TheHistory.AppendLine("Type creation: " + typeMeter.DatabaseTypeCreationString);
          this.TheHistory.AppendLine();
          this.TheHistory.AppendLine("Relative ultrasonic calibrations from type [%]:");
          S4_TDC_Calibration typeCalibration = new S4_TDC_Calibration();
          typeCalibration.LoadCalibrationFromMemory(typeMeter.meterMemory);
          List<float> connectedCalibrations = typeCalibration.GetSmallCalibrationChanges(connectedMeter.meterMemory);
          S4_TDC_Calibration currentCalibration = new S4_TDC_Calibration();
          currentCalibration.LoadCalibrationFromMemory(typeMeter.meterMemory);
          List<float> backupCalibrations = currentCalibration.GetSmallCalibrationChanges(backupMeter.meterMemory);
          this.TheHistory.AppendLine("to backup | to connected");
          for (int i = 0; i < connectedCalibrations.Count; ++i)
          {
            StringBuilder theHistory = this.TheHistory;
            float num = backupCalibrations[i];
            string str8 = num.ToString("f2").PadLeft(9);
            num = connectedCalibrations[i];
            string str9 = num.ToString("f2").PadLeft(12);
            string str10 = str8 + " | " + str9;
            theHistory.AppendLine(str10);
          }
          typeMeter = (S4_Meter) null;
          typeCalibration = (S4_TDC_Calibration) null;
          connectedCalibrations = (List<float>) null;
          currentCalibration = (S4_TDC_Calibration) null;
          backupCalibrations = (List<float>) null;
        }
        catch (Exception ex)
        {
          this.TheHistory.AppendLine();
          this.TheHistory.AppendLine("Type exception");
          this.TheHistory.AppendLine();
          allExceptions.Add(ex);
        }
        try
        {
          this.AddHeader("Meter changes during production");
          using (DbConnection myDbConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
          {
            StringBuilder sql = new StringBuilder();
            StringBuilder stringBuilder1 = sql;
            uint? meterId = ident.MeterID;
            uint num = meterId.Value;
            string str11 = "SELECT * FROM Meter WHERE MeterID = " + num.ToString();
            stringBuilder1.Append(str11);
            DbDataAdapter meterDataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(sql.ToString(), myDbConnection);
            BaseTables.MeterDataTable meterTable = new BaseTables.MeterDataTable();
            meterDataAdapter.Fill((DataTable) meterTable);
            DateTime lastApprovalDate = DateTime.MinValue;
            if (meterTable.Rows.Count != 1)
            {
              this.TheHistory.AppendLine("Meter row not found");
            }
            else
            {
              this.TheHistory.Append("Approval date: ");
              if (meterTable[0].IsApprovalDateNull())
              {
                this.TheHistory.AppendLine("Not defined");
              }
              else
              {
                lastApprovalDate = meterTable[0].ApprovalDate;
                this.TheHistory.AppendLine(lastApprovalDate.ToShortDateString());
              }
            }
            sql.Clear();
            StringBuilder stringBuilder2 = sql;
            meterId = ident.MeterID;
            num = meterId.Value;
            string str12 = "SELECT * FROM MeterChanges WHERE MeterID = " + num.ToString() + " ORDER BY ChangeDate";
            stringBuilder2.Append(str12);
            DbDataAdapter meterChangesDataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(sql.ToString(), myDbConnection);
            BaseTables.MeterChangesDataTable meterChangesTable = new BaseTables.MeterChangesDataTable();
            meterChangesDataAdapter.Fill((DataTable) meterChangesTable);
            foreach (BaseTables.MeterChangesRow meterChangesRow in (TypedTableBase<BaseTables.MeterChangesRow>) meterChangesTable)
            {
              BaseTables.MeterChangesRow theRow = meterChangesRow;
              if (!theRow.IsApprovalDateNull() && !(theRow.ApprovalDate == new DateTime(1900, 1, 1)))
              {
                if (lastApprovalDate == DateTime.MinValue)
                  lastApprovalDate = theRow.ApprovalDate;
                else if (!(lastApprovalDate == theRow.ApprovalDate))
                  lastApprovalDate = theRow.ApprovalDate;
                else
                  continue;
                this.TheHistory.AppendLine("Changed approval date: " + lastApprovalDate.ToShortDateString());
                theRow = (BaseTables.MeterChangesRow) null;
              }
            }
            sql = (StringBuilder) null;
            meterDataAdapter = (DbDataAdapter) null;
            meterTable = (BaseTables.MeterDataTable) null;
            meterChangesDataAdapter = (DbDataAdapter) null;
            meterChangesTable = (BaseTables.MeterChangesDataTable) null;
          }
        }
        catch (Exception ex)
        {
          this.TheHistory.AppendLine();
          this.TheHistory.AppendLine("Database exception");
          this.TheHistory.AppendLine();
          allExceptions.Add(ex);
        }
        connectedMeter = (S4_Meter) null;
        ident = (S4_DeviceIdentification) null;
        currentData = (S4_CurrentData) null;
        functionalState = (S4_FunctionalState) null;
        deviceState = (S4_SystemState) null;
        stateCounter = (DeviceStateCounter) null;
        loggerList = (ObservableCollection<LoggerListItem>) null;
        connectedConfig = (SortedList<OverrideID, ConfigurationParameter>) null;
        backupMeter = (S4_Meter) null;
      }
      catch (Exception ex)
      {
        this.TheHistory.AppendLine();
        this.TheHistory.AppendLine("Exception");
        this.TheHistory.AppendLine();
        allExceptions.Add(ex);
      }
      if (allExceptions.Count <= 0)
      {
        allExceptions = (List<Exception>) null;
      }
      else
      {
        if (allExceptions.Count == 1)
        {
          ExceptionViewer.Show(allExceptions[0]);
        }
        else
        {
          AggregateException ae = new AggregateException((IEnumerable<Exception>) allExceptions);
          ExceptionViewer.Show((Exception) ae);
          ae = (AggregateException) null;
        }
        allExceptions = (List<Exception>) null;
      }
    }

    private async Task AddLogger(
      string loggerName,
      ProgressHandler progress,
      CancellationToken cancellationToken)
    {
      List<KeyValuePair<string, string>> loggerData = await this.MyFunctions.ReadLoggerDataAsListAsync(loggerName, progress, cancellationToken);
      foreach (KeyValuePair<string, string> keyValuePair in loggerData)
      {
        KeyValuePair<string, string> keyValue = keyValuePair;
        this.TheHistory.Append(keyValue.Key + " -> ");
        this.TheHistory.AppendLine(keyValue.Value);
        keyValue = new KeyValuePair<string, string>();
      }
      loggerData = (List<KeyValuePair<string, string>>) null;
    }

    public override string ToString() => this.TheHistory.ToString();
  }
}
