// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_HandlerFunctions
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommunicationPort.Functions;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using HandlerLib.NFC;
using NLog;
using StartupLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  public class S4_HandlerFunctions : 
    HandlerFunctionsForProduction,
    IHandler,
    IReadoutConfig,
    ILoggerManager
  {
    internal const string FullTimeFormat = "dd.MM.yyyy HH:mm:ss";
    private const ushort DefaultSyncWord = 37331;
    internal static string Right_AllHandlersEnabled = "Right\\AllHandlersEnabled";
    internal static Logger Base_S4_HandlerFunctionsLogger = LogManager.GetLogger(nameof (S4_HandlerFunctions));
    public ChannelLogger S4_HandlerFunctionsLogger;
    internal ConfigList configList;
    public CommunicationPortFunctions myPort;
    internal BaseDbConnection myDb;
    internal S4_DeviceCommandsNFC myDeviceCommands;
    internal S4_AllMeters myMeters;
    internal S4_ProtocolConfiguration myProtocolConfiguration;
    internal S4_LoggerManager myLoggerManager;
    internal S4_Modules myModules;
    private HardwareTypeSupport _hardwareTypeSupport;
    private ReadPartsSelection ReadSelection = ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode);
    private S4_RadioTests radioTests;

    internal NfcDeviceCommands nfcCmd => this.myDeviceCommands.CommonNfcCommands;

    internal HardwareTypeSupport myHardwareTypeSupport
    {
      get
      {
        if (this._hardwareTypeSupport == null)
          this._hardwareTypeSupport = new HardwareTypeSupport(new string[1]
          {
            "IUW"
          });
        return this._hardwareTypeSupport;
      }
    }

    internal bool ProtocolMode
    {
      get
      {
        return (this.ReadSelection & ReadPartsSelection.ProtocolOnlyMode) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode);
      }
    }

    internal bool NFC_BlockMode
    {
      get => this.myDeviceCommands != null && this.myDeviceCommands.CommonNfcCommands.NFC_BlockMode;
      set => this.myDeviceCommands.CommonNfcCommands.NFC_BlockMode = value;
    }

    public override bool MapCheckDisabled { get; set; }

    internal CommunicationPortFunctions checkedPort
    {
      get
      {
        return this.myPort != null ? this.myPort : throw new ArgumentNullException("CommunicationPort object not available.");
      }
    }

    internal S4_DeviceCommandsNFC checkedCommands
    {
      get
      {
        if (this.myPort == null)
          throw new ArgumentNullException("CommunicationPort object not available.");
        return this.myDeviceCommands != null ? this.myDeviceCommands : throw new Exception("CommunicationPort not defined");
      }
    }

    internal NfcDeviceCommands checkedNfcCommands
    {
      get
      {
        if (this.myPort == null)
          throw new ArgumentNullException("CommunicationPort object not available.");
        return this.myDeviceCommands != null ? this.myDeviceCommands.CommonNfcCommands : throw new Exception("CommunicationPort not defined");
      }
    }

    public S4_HandlerFunctions()
    {
      this.MapCheckDisabled = false;
      this.myMeters = new S4_AllMeters(this);
      this.myLoggerManager = new S4_LoggerManager();
    }

    public S4_HandlerFunctions(CommunicationPortFunctions myPort)
      : this()
    {
      this.myPort = myPort;
      this.configList = myPort.GetReadoutConfiguration();
      this.configList.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ConfigList_CollectionChanged);
      this.configList.PropertyChanged += new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
      this.SetCommandDeviceObject();
      this.S4_HandlerFunctionsLogger = new ChannelLogger(S4_HandlerFunctions.Base_S4_HandlerFunctionsLogger, this.configList);
    }

    public S4_HandlerFunctions(BaseDbConnection myDb)
      : this()
    {
      this.myDb = myDb;
      this.S4_HandlerFunctionsLogger = new ChannelLogger(S4_HandlerFunctions.Base_S4_HandlerFunctionsLogger, (string) null);
    }

    public S4_HandlerFunctions(CommunicationPortFunctions myPort, BaseDbConnection myDb)
      : this(myPort)
    {
      this.myDb = myDb;
    }

    public S4_HandlerFunctions(
      CommunicationPortFunctions myPort,
      BaseDbConnection myDb,
      Common32BitCommands IrDaCommands)
      : this(myPort)
    {
      this.myDb = myDb;
      this.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaCommands = IrDaCommands;
    }

    public void Dispose()
    {
      if (this.configList != null)
      {
        this.configList.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ConfigList_CollectionChanged);
        this.configList.PropertyChanged -= new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
        this.configList = (ConfigList) null;
      }
      if (this.myPort != null)
      {
        this.myPort.Dispose();
        this.myPort = (CommunicationPortFunctions) null;
      }
      this.myDeviceCommands = (S4_DeviceCommandsNFC) null;
      S4_SmartFunctionManager.Dispose();
    }

    public override void Clear()
    {
      this.ReadSelection = ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode);
      this.myProtocolConfiguration = (S4_ProtocolConfiguration) null;
      this.myLoggerManager.Clear();
      this.myMeters = new S4_AllMeters(this);
      S4_AllMeters.TypeCacheClear();
    }

    public override void SetReadoutConfiguration(ConfigList configList)
    {
      if (configList == null)
        throw new ArgumentNullException(nameof (configList));
      if (this.configList == null)
      {
        this.configList = configList;
        this.configList.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ConfigList_CollectionChanged);
        this.configList.PropertyChanged += new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
      }
      else if (this.configList != configList)
        throw new ArgumentException("this.configList != configList");
      if (this.myPort == null)
        this.myPort = new CommunicationPortFunctions();
      this.myPort.SetReadoutConfiguration(configList);
      this.SetCommandDeviceObject();
    }

    private void ConfigList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.SetCommandDeviceObject();
    }

    private void ConfigList_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "BusMode"))
        return;
      this.SetCommandDeviceObject();
    }

    public override ConfigList GetReadoutConfiguration() => this.configList;

    private void SetCommandDeviceObject()
    {
      if (this.myPort == null)
        throw new ArgumentNullException("myPort");
      if (this.configList == null)
        return;
      string busMode1 = this.configList.BusMode;
      ZENNER.CommonLibrary.BusMode busMode2 = ZENNER.CommonLibrary.BusMode.NFC;
      string str1 = busMode2.ToString();
      if (busMode1 == str1)
      {
        if (this.myDeviceCommands == null)
          this.myDeviceCommands = new S4_DeviceCommandsNFC(this.myPort);
      }
      else
      {
        string busMode3 = this.configList.BusMode;
        busMode2 = ZENNER.CommonLibrary.BusMode.MBus;
        string str2 = busMode2.ToString();
        int num;
        if (!(busMode3 == str2))
        {
          string busMode4 = this.configList.BusMode;
          busMode2 = ZENNER.CommonLibrary.BusMode.MBusPointToPoint;
          string str3 = busMode2.ToString();
          num = busMode4 == str3 ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0 && this.myDeviceCommands == null)
        {
          this.myDeviceCommands = new S4_DeviceCommandsNFC(this.myPort);
          this.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaCommands = new Common32BitCommands(new DeviceCommandsMBus((IPort) this.myPort));
        }
      }
      if (this.S4_HandlerFunctionsLogger != null)
        return;
      this.S4_HandlerFunctionsLogger = new ChannelLogger(S4_HandlerFunctions.Base_S4_HandlerFunctionsLogger, this.configList);
    }

    public override void SetCommunicationPort(CommunicationPortFunctions myPort)
    {
      this.myPort = myPort;
      if (this.myDeviceCommands != null)
        return;
      this.SetCommandDeviceObject();
    }

    public async Task CheckCommunicationFirmwareVersionsForConnectedDevice(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await NFC_Versions.CheckVersions(this.myMeters.checkedConnectedMeter.deviceIdentification.FirmwareVersionObj, this.myPort, this.checkedNfcCommands, progress, cancelToken);
    }

    public async Task<DeviceIdentification> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace(nameof (ReadVersionAsync));
      DeviceIdentification deviceIdentification = await this.checkedNfcCommands.ReadVersionAsync(progress, cancelToken);
      return deviceIdentification;
    }

    public override void Open() => this.checkedPort.Open();

    public override void Close() => this.checkedPort.Close();

    public override async Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ReadPartsSelection readPartsSelection)
    {
      this.ReadSelection = readPartsSelection;
      this.S4_HandlerFunctionsLogger.Trace(nameof (ReadDeviceAsync));
      if (this.ProtocolMode)
      {
        this.myProtocolConfiguration = new S4_ProtocolConfiguration(this);
        await this.myProtocolConfiguration.ReadConfigurationParametersAsync(progress, cancelToken, readPartsSelection);
      }
      else
      {
        this.myProtocolConfiguration = (S4_ProtocolConfiguration) null;
        this.myLoggerManager.Clear();
        await this.myMeters.ReadDeviceAsync(progress, cancelToken, readPartsSelection);
      }
      return this.myDeviceCommands.CommonNfcCommands.myNfcRepeater.TotalDeviceRepeats;
    }

    public override async Task WriteDeviceAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace(nameof (WriteDeviceAsync));
      if (this.ProtocolMode)
      {
        await this.myProtocolConfiguration.WriteConfigurationParametersAsync(progress, cancelToken);
        this.myProtocolConfiguration = (S4_ProtocolConfiguration) null;
        this.myMeters.ConnectedMeter = (S4_Meter) null;
      }
      else
        await this.myMeters.WriteDeviceAsync(progress, cancelToken);
    }

    public override async Task ReInitMeasurementAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace("ReInitMeasurement");
      await this.ReInitMeasurementAsync(S4_HandlerFunctions.MeasurementParts.All, progress, cancelToken);
    }

    public override SortedList<long, SortedList<DateTime, double>> GetValues(int subDevice = 0)
    {
      if (this.myLoggerManager == null && this.myLoggerManager.LoggerList == null)
        return (SortedList<long, SortedList<DateTime, double>>) null;
      List<VolumeLoggerData> dataListFromMemory = S4_LoggerManager.GetMonthLoggerDataListFromMemory(this.myMeters.checkedWorkMeter.meterMemory);
      if (dataListFromMemory == null)
        return (SortedList<long, SortedList<DateTime, double>>) null;
      SortedList<DateTime, double> sortedList1 = new SortedList<DateTime, double>();
      SortedList<DateTime, double> sortedList2 = new SortedList<DateTime, double>();
      SortedList<DateTime, double> sortedList3 = new SortedList<DateTime, double>();
      foreach (VolumeLoggerData volumeLoggerData in dataListFromMemory)
      {
        DateTime loggerTime = volumeLoggerData.LoggerTime;
        if (loggerTime.Hour == 0 && loggerTime.Minute == 0 && loggerTime.Second == 0 && loggerTime.Day == 1)
        {
          if (!sortedList1.ContainsKey(loggerTime))
            sortedList1.Add(loggerTime, Math.Round(volumeLoggerData.Volume, 3));
        }
        else if (loggerTime.Hour == 0 && loggerTime.Minute == 0 && loggerTime.Second == 0 && loggerTime.Day == 16)
        {
          if (!sortedList3.ContainsKey(loggerTime))
            sortedList3.Add(loggerTime, Math.Round(volumeLoggerData.Volume, 3));
        }
        else if (!sortedList2.ContainsKey(loggerTime))
          sortedList2.Add(loggerTime, Math.Round(volumeLoggerData.Volume, 3));
      }
      return new SortedList<long, SortedList<DateTime, double>>()
      {
        {
          289476673L,
          sortedList1
        },
        {
          293670977L,
          sortedList3
        },
        {
          302059585L,
          sortedList2
        }
      };
    }

    public override IEnumerable GetEvents()
    {
      List<EventLoggerData> dataListFromMemory = S4_LoggerManager.GetEventLoggerDataListFromMemory(this.myMeters.checkedWorkMeter.meterMemory);
      if (dataListFromMemory == null)
        return (IEnumerable) null;
      return UserManager.CheckPermission(UserManager.Role_Developer) ? (IEnumerable) dataListFromMemory : (IEnumerable) dataListFromMemory.FindAll((Predicate<EventLoggerData>) (x => x.Event == LoggerEventTypes.SmartFunctionEvent));
    }

    public bool checkWorkMeterNotNull() => this.myMeters.WorkMeter != null;

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int subDevice = 0)
    {
      if (subDevice != 0)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      if (this.ProtocolMode)
        return this.myProtocolConfiguration != null ? this.myProtocolConfiguration.GetConfigList() : throw new Exception("Read device before GetConfigurationParameters");
      return this.myMeters.WorkMeter == null ? new SortedList<OverrideID, ConfigurationParameter>() : this.myMeters.checkedWorkMeter.GetConfigurationParameters("Prepare parameters for handler interface.");
    }

    public SortedList<OverrideID, ConfigurationParameter> GetConfigurationParametersFromBackup(
      int subDevice = 0)
    {
      if (subDevice != 0)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      return this.myMeters.BackupMeter == null ? new SortedList<OverrideID, ConfigurationParameter>() : this.myMeters.BackupMeter.GetConfigurationParameters("Prepare parameters for handler interface.");
    }

    public override void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int subDevice = 0)
    {
      if (subDevice != 0)
        return;
      if (this.ProtocolMode)
      {
        if (this.myProtocolConfiguration == null)
          throw new Exception("Read device before SetConfigurationParameters");
        this.myProtocolConfiguration.WorkWriteParameterChanges(parameterList);
      }
      else
      {
        this.myLoggerManager.Clear();
        try
        {
          this.myMeters.CloneWorkMeterForUndo();
          this.myMeters.checkedWorkMeter.SetConfigurationParameters(parameterList);
        }
        catch (Exception ex)
        {
          this.myMeters.Undo();
          throw new Exception("ConfigurationParameters error. Change not done.", ex);
        }
      }
    }

    public override BatteryEnergyManagement GetBatteryCalculations()
    {
      return (BatteryEnergyManagement) this.myMeters.checkedWorkMeter.BatteryManager;
    }

    public override DeviceIdentification GetDeviceIdentification()
    {
      return this.ProtocolMode ? (DeviceIdentification) this.myProtocolConfiguration.ReadDeviceIdentification : (DeviceIdentification) this.myMeters.checkedWorkMeter.deviceIdentification;
    }

    public override void OpenType(int meterInfoID) => this.myMeters.OpenType(meterInfoID);

    public override void OpenType(string typeCreationString)
    {
      S4_AllMeters.TypeCacheClear();
      this.myMeters.OpenType(typeCreationString);
    }

    public override void OpenCompareType(int meterInfoID)
    {
      this.myMeters.OpenCompareType(meterInfoID);
    }

    public override int SaveType(
      OpenTransaction openTransaction,
      string sapNumber,
      string description)
    {
      return this.myMeters.SaveType(openTransaction, sapNumber, description);
    }

    public override int SaveType(
      OpenTransaction openTransaction,
      string sapNumber,
      string description,
      int meterhardwareid)
    {
      return this.myMeters.SaveType(openTransaction, sapNumber, description, new int?(meterhardwareid));
    }

    public override DateTime? SaveMeter()
    {
      this.S4_HandlerFunctionsLogger.Trace("SaveMeter backup");
      if (this.myMeters == null)
        throw new ArgumentNullException("myMeters");
      if (this.myMeters.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      DeviceIdentification deviceIdentification = this.GetDeviceIdentification();
      if (deviceIdentification == null || !deviceIdentification.MeterID.HasValue)
        throw new ArgumentException("Invalid MeterID!");
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      int meterID = (int) deviceIdentification.MeterID.Value;
      uint? nullable1 = deviceIdentification.MeterInfoID;
      int meterInfoID = (int) nullable1.Value;
      nullable1 = deviceIdentification.FirmwareVersion;
      uint hardwareTypeID_OR_firmwareVersion = nullable1.Value;
      string serialNumberAsString = deviceIdentification.PrintedSerialNumberAsString;
      string productionOrderNumber = deviceIdentification.SAP_ProductionOrderNumber;
      byte[] compressedData = this.myMeters.WorkMeter.meterMemory.GetCompressedData();
      DateTime? nullable2 = Device.Save(baseDbConnection, meterID, meterInfoID, hardwareTypeID_OR_firmwareVersion, serialNumberAsString, productionOrderNumber, compressedData, false);
      this.myMeters.SetBackupMeter(compressedData);
      return nullable2;
    }

    public override bool LoadLastBackup(int meterID)
    {
      if (this.S4_HandlerFunctionsLogger == null)
        this.S4_HandlerFunctionsLogger = new ChannelLogger(S4_HandlerFunctions.Base_S4_HandlerFunctionsLogger, (string) null);
      this.S4_HandlerFunctionsLogger.Trace("LoadLastBackup. MeterID=" + meterID.ToString());
      BaseTables.MeterDataRow backupRow = GmmDbLib.MeterData.LoadLastBackup(DbBasis.PrimaryDB.BaseDbConnection, meterID);
      if (backupRow == null)
        return false;
      this.myMeters.SetBackupMeter(backupRow);
      return true;
    }

    public override void OverwriteFromType(CommonOverwriteGroups[] overwriteGroups)
    {
      this.myMeters.GuarnteeTypeReadyForOverwrite();
      this.myMeters.OverwriteSrcToDest(HandlerMeterObjects.TypeMeter, HandlerMeterObjects.WorkMeter, overwriteGroups);
    }

    public override void OverwriteSrcToDest(
      HandlerMeterObjects sourceObject,
      HandlerMeterObjects destinationObject,
      CommonOverwriteGroups[] overwriteGroups)
    {
      if (sourceObject == HandlerMeterObjects.TypeMeter)
        this.myMeters.GuarnteeTypeReadyForOverwrite();
      this.myMeters.OverwriteSrcToDest(sourceObject, destinationObject, overwriteGroups);
    }

    public override CommonOverwriteGroups[] GetAllOverwriteGroups()
    {
      return HandlerFunctionsForProduction.GetImplementedOverwriteGroups(Enum.GetNames(typeof (S4_HandlerFunctions.S4_OverwriteGroups)));
    }

    public override string GetOverwriteGroupInfo(CommonOverwriteGroups overwriteGroupe)
    {
      return S4_DeviceMemory.GetOverwriteGroupInfo(overwriteGroupe);
    }

    public override void SaveMeterObject(HandlerMeterObjects meterObject)
    {
      this.myMeters.SaveMeterObject(meterObject);
    }

    public override async Task Calibrate_RTC(
      ProgressHandler progress,
      CancellationToken token,
      double calibrationValue)
    {
      this.S4_HandlerFunctionsLogger.Trace("Set RTC_Calibration_Value");
      ushort cal_value = 0;
      short value = (short) (calibrationValue * 1048576.0 / (1000000.0 - calibrationValue));
      if (value < (short) 0)
      {
        value += (short) 512;
        cal_value = (ushort) value;
        cal_value |= (ushort) 32768;
      }
      else
        cal_value = (ushort) value;
      await this.checkedNfcCommands.SetRTCCalibrationValue(cal_value, progress, token);
    }

    public async Task<byte[]> NFC_GetTagIdentAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace("GetNFC_TagIdentAsync");
      byte[] tagIdentAsync = await this.checkedNfcCommands.mySubunitCommands.NFC_GetTagIdentAsync(progress, cancelToken);
      return tagIdentAsync;
    }

    public async Task<NfcCouplerCurrents> GetNFC_CouplerCurrentAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace(nameof (GetNFC_CouplerCurrentAsync));
      NfcCouplerCurrents nfcCouplerCurrent = await this.checkedCommands.myMiConConnector.GetNFC_CouplerCurrent(progress, cancelToken);
      return nfcCouplerCurrent;
    }

    public async Task<string> GetNFC_CouplerEchoAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      string echoAsync = await this.checkedNfcCommands.mySubunitCommands.GetEchoAsync(progress, cancelToken);
      return echoAsync;
    }

    public async Task<string> GetNFC_CouplerIdentAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      string couplerIdentAsync = await this.checkedNfcCommands.mySubunitCommands.ReadCouplerIdentificationAsync(progress, cancelToken);
      return couplerIdentAsync;
    }

    public async Task<MiConConnectorVersion> GetNFC_ConnectorVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      MiConConnectorVersion connectorVersionAsync = await this.checkedNfcCommands.mySubunitCommands.ReadMiConIdentificationAsync(progress, cancelToken);
      return connectorVersionAsync;
    }

    public async Task Set_MiConConnector_RfOn_Async(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] numArray = await this.checkedNfcCommands.mySubunitCommands.SetRfOnAsync(progress, cancelToken);
    }

    public async Task Set_MiConConnector_RfOff_Async(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] numArray = await this.checkedNfcCommands.mySubunitCommands.SetRfOffAsync(progress, cancelToken);
    }

    public async Task<S4_SystemState> GetDeviceState(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_SystemState deviceStatesAsync = await this.checkedCommands.GetDeviceStatesAsync(progress, cancelToken);
      return deviceStatesAsync;
    }

    public async Task<bool> IsTestActive(
      HandlerFunctionsForProduction.CommonDeviceModes testMode,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_SystemState iuwState = await this.checkedCommands.GetDeviceStatesAsync(progress, cancelToken);
      bool flag = iuwState.IsCommonDeviceModeActive(testMode);
      iuwState = (S4_SystemState) null;
      return flag;
    }

    public async Task DoMiConConnectorReset(ProgressHandler progress, CancellationToken cancelToken)
    {
      await this.checkedNfcCommands.mySubunitCommands.MiConConnector_Reset(progress, cancelToken);
    }

    public async Task SetMiConDeviceIdentificationAsync(
      byte?[] devIdent,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.checkedNfcCommands.mySubunitCommands.WriteMiConIdentificationAsync(devIdent, progress, cancelToken);
    }

    public async Task<string> CheckNfcCommunication(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_NfcFunctions nfcFunction = new S4_NfcFunctions(this);
      string str = await nfcFunction.CheckNfcCommunication(progress, cancelToken);
      nfcFunction = (S4_NfcFunctions) null;
      return str;
    }

    public async Task<NdcMiConModuleHardwareIds> GetNdcMiConModuleHardwareIds(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      NdcMiConModuleHardwareIds moduleHardwareIds = await this.checkedNfcCommands.mySubunitCommands.ReadNdcMiConModuleHardwareIds(progress, cancelToken);
      return moduleHardwareIds;
    }

    public override async Task<NfcDeviceIdentification> GetMiConDeviceIdentification(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      MiConConnectorVersion deviceIdentification = await this.checkedNfcCommands.mySubunitCommands.ReadMiConIdentificationAsync(progress, cancelToken);
      return (NfcDeviceIdentification) deviceIdentification;
    }

    public async Task<bool> GetUnlockPinState(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] readData = await this.checkedNfcCommands.myNfcMemoryTransceiver.ReadMemoryAsync(1342181392U, 1U, progress, cancelToken);
      bool unlockPinState = ((int) readData[0] & 32) == 0;
      readData = (byte[]) null;
      return unlockPinState;
    }

    public override async Task SetLcdTestStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte lcdTestState)
    {
      if (lcdTestState > (byte) 0)
      {
        await this.nfcCmd.SetModeAsync(S4_DeviceModes.TestModePrepared, progress, cancelToken);
        List<byte> modeParameter = new List<byte>();
        modeParameter.Add(lcdTestState);
        await this.nfcCmd.SetModeAsync(S4_DeviceModes.LcdTest, progress, cancelToken, modeParameter.ToArray());
        modeParameter = (List<byte>) null;
      }
      else
        await this.nfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
    }

    public async Task StopAllTestAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      await this.myDeviceCommands.StopAllTestAsync(progress, cancelToken);
    }

    public async Task<DateTimeOffset> GetSystemDateTime(
      ProgressHandler progress,
      CancellationToken cancellationToken)
    {
      DateTimeOffset systemDateTime = await this.checkedNfcCommands.GetSystemDateTime(progress, cancellationToken);
      return systemDateTime;
    }

    public async Task<DateTime> ReadHighResulutionDeviceTime(
      ProgressHandler progress,
      CancellationToken cancellationToken)
    {
      AddressRange sysDateTimeRange = this.myMeters.checkedConnectedMeter.meterMemory.GetAddressRange(S4_Params.sysDateTime.ToString());
      byte[] sysDateTimeBytes = await this.checkedNfcCommands.ReadMemoryAsync(progress, cancellationToken, sysDateTimeRange.StartAddress, sysDateTimeRange.ByteSize);
      DateTime dateTimeFromBcd = FirmwareDateTimeSupport.ToDateTimeFromBCD(sysDateTimeBytes);
      sysDateTimeRange = (AddressRange) null;
      sysDateTimeBytes = (byte[]) null;
      return dateTimeFromBcd;
    }

    public async Task<DateTimeOffset> SetDeviceTime(
      DateTime dateTime,
      Decimal timeZone,
      ProgressHandler progress,
      CancellationToken cancellationToken)
    {
      DateTimeOffset dateTimeOffset1 = new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, new TimeSpan(0, (int) (timeZone * 60M), 0));
      DateTimeOffset dateTimeOffset2 = await this.checkedCommands.CommonNfcCommands.SetSystemDateTime(dateTimeOffset1, progress, cancellationToken);
      DateTimeOffset setTime = dateTimeOffset2;
      dateTimeOffset2 = new DateTimeOffset();
      if (this.myMeters.WorkMeter != null)
        FirmwareDateTimeSupport.SetTimeOffsetToMemoryBCD(setTime, (DeviceMemory) this.myMeters.WorkMeter.meterMemory);
      return setTime;
    }

    public async Task<DateTimeOffset> SetTimeForTimeZoneFromPcTime(
      Decimal timeZone,
      ProgressHandler progress,
      CancellationToken cancellationToken)
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.ToUniversalTime();
      DateTime dateTimeAtDeviceTimeZone = dateTime.AddHours((double) timeZone);
      DateTimeOffset dateTimeOffset1 = new DateTimeOffset(dateTimeAtDeviceTimeZone.Year, dateTimeAtDeviceTimeZone.Month, dateTimeAtDeviceTimeZone.Day, dateTimeAtDeviceTimeZone.Hour, dateTimeAtDeviceTimeZone.Minute, dateTimeAtDeviceTimeZone.Second, new TimeSpan(0, (int) (timeZone * 60M), 0));
      DateTimeOffset dateTimeOffset2 = await this.checkedCommands.CommonNfcCommands.SetSystemDateTime(dateTimeOffset1, progress, cancellationToken);
      DateTimeOffset setTime = dateTimeOffset2;
      dateTimeOffset2 = new DateTimeOffset();
      return setTime;
    }

    public override async Task SetModeAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      Enum mode)
    {
      await this.SetModeAsync(progress, cancelToken, mode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
    }

    public async Task SetModeAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      Enum mode,
      byte lcdTestState = 0,
      ushort timeoutSeconds = 10,
      ushort interval = 0,
      uint deviceID = 305419896,
      ushort syncWord = 37331,
      byte[] arbitraryData = null)
    {
      if (new FirmwareVersion(this.myMeters.ConnectedMeter.deviceIdentification.FirmwareVersion.Value) < (object) "1.1.0 IUW")
        throw new Exception("Firmware version not supported");
      switch (mode)
      {
        case HandlerFunctionsForProduction.CommonDeviceModes commonDeviceModes1:
          List<byte> modeParameter = new List<byte>();
          HandlerFunctionsForProduction.CommonDeviceModes commonDeviceModes = commonDeviceModes1;
          switch (commonDeviceModes)
          {
            case HandlerFunctionsForProduction.CommonDeviceModes.OperationMode:
              await this.checkedCommands.StopAllTestAsync(progress, cancelToken);
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.DeliveryMode:
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.DeliveryMode, progress, cancelToken, arbitraryData);
              if (this.myMeters.ConnectedMeter != null)
              {
                try
                {
                  await this.GenerateAllChecksums(progress, cancelToken);
                }
                catch (Exception ex)
                {
                  if (!ex.Message.ToUpper().Contains("TIMEOUT"))
                    throw ex;
                }
                await Task.Delay(2000);
                break;
              }
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.StandbyCurrentMode:
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.CurrentTest, progress, cancelToken);
              byte[] numArray = await this.checkedNfcCommands.mySubunitCommands.SetRfOffAsync(progress, cancelToken);
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.RTC_CalibrationMode:
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.RtcCalibrationTestStart, progress, cancelToken);
              await this.checkedNfcCommands.SetRTCCalibrationValue((ushort) 0, progress, cancelToken);
              await this.Set_MiConConnector_RfOff_Async(progress, cancelToken);
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.RTC_CalibrationVerifyMode:
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.RtcCalibrationTestStart, progress, cancelToken);
              await this.Set_MiConConnector_RfOff_Async(progress, cancelToken);
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.LcdTest:
              modeParameter.Add(lcdTestState);
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.LcdTest, progress, cancelToken, modeParameter.ToArray());
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitUnmodulatedCarrier:
              modeParameter.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeoutSeconds));
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.RadioTestTransmitUnmodulatedCarrier, progress, cancelToken, modeParameter.ToArray());
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitModulatedCarrier:
              modeParameter.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeoutSeconds));
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.RadioTestTransmitModulatedCarrier, progress, cancelToken, modeParameter.ToArray());
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.RadioTestReceiveTestPacket:
              if (timeoutSeconds > (ushort) byte.MaxValue)
                throw new ArgumentException("Timeout > 255 not allowed.");
              modeParameter.Add((byte) arbitraryData.Length);
              modeParameter.Add((byte) ((uint) syncWord >> 8));
              modeParameter.Add((byte) syncWord);
              modeParameter.Add((byte) timeoutSeconds);
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.RadioTestReceiveTestPacket, progress, cancelToken, modeParameter.ToArray());
              break;
            case HandlerFunctionsForProduction.CommonDeviceModes.RadioTestSendTestPacket:
              if (arbitraryData == null)
                throw new ArgumentException("ArbitraryData not defined");
              modeParameter.AddRange((IEnumerable<byte>) BitConverter.GetBytes(interval));
              modeParameter.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeoutSeconds));
              modeParameter.AddRange((IEnumerable<byte>) arbitraryData);
              await this.nfcCmd.SetModeAsync(S4_DeviceModes.RadioTestSendTestPacket, progress, cancelToken, modeParameter.ToArray());
              break;
            default:
              throw new NotImplementedException("CommonDeviceModes not supported: " + ((int) mode).ToString());
          }
          modeParameter = (List<byte>) null;
          break;
        case S4_DeviceModes _:
          await this.nfcCmd.SetModeAsync((S4_DeviceModes) mode, progress, cancelToken);
          break;
        default:
          throw new ArgumentException("mode: Illegal enum type");
      }
    }

    public async Task ReInitMeasurementAsync(
      S4_HandlerFunctions.MeasurementParts measurementParts,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (new FirmwareVersion(this.myMeters.checkedWorkMeter.deviceIdentification.FirmwareVersion.Value) >= (object) "1.4.5 IUW")
      {
        byte[] parts = BitConverter.GetBytes((ushort) measurementParts);
        byte[] resultAsync = await this.checkedNfcCommands.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.ReInitMeasurement, parts);
        parts = (byte[]) null;
      }
      else
      {
        await Task.Delay(2000);
        progress.Report("Wait meter stable");
        await this.SetModeAsync(progress, cancelToken, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.DeliveryMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
        progress.Report("Wait delivery mode stable");
        await Task.Delay(3000);
        await this.SetModeAsync(progress, cancelToken, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.OperationMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
        progress.Report("Wait operation mode stable");
        await Task.Delay(9000);
      }
    }

    public async Task PrepareForFlyingTestAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace(nameof (PrepareForFlyingTestAsync));
      await this.checkedCommands.PrepareForFlyingTestAsync(progress, cancelToken);
    }

    public async Task<FlyingTestData> ReadFlyingTestResultsAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace(nameof (ReadFlyingTestResultsAsync));
      uint FtDataStartAddress = this.myMeters.ConnectedMeter.meterMemory.GetParameterAddress(S4_Params.flyingTestData);
      AddressRange tdcFlyingTestRange = new AddressRange(FtDataStartAddress, 36U);
      S4_DeviceMemory connectedMeterMemory = this.myMeters.ConnectedMeter.meterMemory;
      FlyingTestData flyingTestData = await this.checkedCommands.ReadFlyingTestResultsAsync(tdcFlyingTestRange, (DeviceMemory) connectedMeterMemory, progress, cancelToken);
      tdcFlyingTestRange = (AddressRange) null;
      connectedMeterMemory = (S4_DeviceMemory) null;
      return flyingTestData;
    }

    public async Task LogTdcInternals(
      int cycleTime,
      string fileExtension,
      EventHandler<string> tdcEvent,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_TDC_Internals tdcInternals = new S4_TDC_Internals(this.myMeters.ConnectedMeter, this.checkedCommands);
      await tdcInternals.LogTdcInternals(cycleTime, fileExtension, tdcEvent, progress, cancelToken);
      tdcInternals = (S4_TDC_Internals) null;
    }

    public void FlowCalibration(List<FlowCalibrationPoint> calibrationPoints)
    {
      SortedList<double, double> cf = new SortedList<double, double>();
      foreach (FlowCalibrationPoint calibrationPoint in calibrationPoints)
        cf.Add(calibrationPoint.Flow_Qm, 100.0 / (100.0 + calibrationPoint.Error_Percent));
      S4_DeviceMemory meterMemory = this.myMeters.checkedWorkMeter.meterMemory;
      S4_TDC_Calibration s4TdcCalibration = new S4_TDC_Calibration();
      s4TdcCalibration.LoadCalibrationFromMemory(meterMemory);
      s4TdcCalibration.AdjustCalibration(cf);
      s4TdcCalibration.CopyCalibrationToMemory(meterMemory);
    }

    internal async Task SetZeroOffsetParameters(
      EventHandler<string> strEvent,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (this.myMeters == null)
        throw new ArgumentNullException("myMeter");
      if (this.myMeters.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      uint i = 0;
      S4_SystemState deviceState;
      do
      {
        await Task.Delay(500);
        deviceState = await this.GetDeviceState(progress, cancelToken);
        if (i > 20U)
          throw new TimeoutException("Timeout: " + Test_State.TEST_STATE_STOPPED.ToString());
        ++i;
      }
      while (deviceState.TestState != Test_State.TEST_STATE_STOPPED);
      S4_DeviceMemory meterMemory = this.myMeters.WorkMeter.meterMemory;
      uint addr = meterMemory.GetParameterAddress(S4_Params.zeroOffsetMeasUnit1);
      AddressRange tdcOffsetMeas_0 = new AddressRange(addr, 4U);
      addr = meterMemory.GetParameterAddress(S4_Params.zeroOffsetMeasUnit2);
      AddressRange tdcOffsetMeas_1 = new AddressRange(addr, 4U);
      addr = meterMemory.GetParameterAddress(S4_Params.zeroOffsetUnit1);
      AddressRange tdcOffset_0 = new AddressRange(addr, 4U);
      addr = meterMemory.GetParameterAddress(S4_Params.zeroOffsetUnit2);
      AddressRange tdcOffset_1 = new AddressRange(addr, 4U);
      meterMemory.GarantMemoryAvailable(tdcOffsetMeas_0);
      await this.checkedNfcCommands.ReadMemoryAsync(tdcOffsetMeas_0, (DeviceMemory) meterMemory, progress, cancelToken);
      meterMemory.GarantMemoryAvailable(tdcOffsetMeas_1);
      await this.checkedNfcCommands.ReadMemoryAsync(tdcOffsetMeas_1, (DeviceMemory) meterMemory, progress, cancelToken);
      meterMemory.GarantMemoryAvailable(tdcOffset_0);
      await this.checkedNfcCommands.ReadMemoryAsync(tdcOffset_0, (DeviceMemory) meterMemory, progress, cancelToken);
      meterMemory.GarantMemoryAvailable(tdcOffset_1);
      await this.checkedNfcCommands.ReadMemoryAsync(tdcOffset_1, (DeviceMemory) meterMemory, progress, cancelToken);
      S4_TDC_Calibration tdcCalib = new S4_TDC_Calibration();
      tdcCalib.CopyZeroOffsetParameters(meterMemory, strEvent);
      if (strEvent != null)
        strEvent((object) this, "write to device...\r\n");
      meterMemory.GarantMemoryAvailable(tdcOffset_0);
      await this.checkedNfcCommands.WriteMemoryAsync(tdcOffset_0, (DeviceMemory) meterMemory, progress, cancelToken);
      meterMemory.GarantMemoryAvailable(tdcOffset_1);
      await this.checkedNfcCommands.WriteMemoryAsync(tdcOffset_1, (DeviceMemory) meterMemory, progress, cancelToken);
      deviceState = (S4_SystemState) null;
      meterMemory = (S4_DeviceMemory) null;
      tdcOffsetMeas_0 = (AddressRange) null;
      tdcOffsetMeas_1 = (AddressRange) null;
      tdcOffset_0 = (AddressRange) null;
      tdcOffset_1 = (AddressRange) null;
      tdcCalib = (S4_TDC_Calibration) null;
    }

    public async Task CalibrateZeroOffset(
      int calibrationTime,
      EventHandler<string> strEvent,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (strEvent != null)
        strEvent((object) this, "check IUW TestState...\r\n");
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
      if (strEvent != null)
        strEvent((object) this, "enable TestMode...\r\n");
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.TestModePrepared, progress, cancelToken);
      if (strEvent != null)
        strEvent((object) this, "start ZeroOffset_Test for " + calibrationTime.ToString() + " seconds...\r\n");
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.ZeroOffsetTestStart, progress, cancelToken);
      await Task.Delay(calibrationTime * 1000, cancelToken);
      if (strEvent != null)
        strEvent((object) this, "stop ZeroOffset_Test...\r\n");
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.ZeroOffsetTestOrderStop, progress, cancelToken);
      if (strEvent != null)
        strEvent((object) this, "set new parameter...\r\n");
      await this.SetZeroOffsetParameters(strEvent, progress, cancelToken);
      if (strEvent != null)
        strEvent((object) this, "disable TestMode...\r\n");
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
    }

    public async Task<ZeroFlowCheckResults> UltrasonicZeroOffset(
      int analysingTime,
      bool calibrate,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_DeviceMemory mm = this.myMeters.WorkMeter.meterMemory;
      progress.SplitScaled(new double[12]
      {
        0.2,
        1.0,
        0.2,
        0.2,
        0.2,
        (double) analysingTime,
        0.2,
        0.2,
        0.2,
        0.2,
        0.2,
        0.2
      });
      progress.BaseMessage = "Prepare test:  ";
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
      S4_TDC_Internals tdcInternals = new S4_TDC_Internals(this.myMeters.WorkMeter, this.checkedCommands);
      UltrasonicState ultrasonicStateAsync = await tdcInternals.ReadAndGetUltrasonicStateAsync(progress, cancelToken);
      if (!tdcInternals.IsUltrasonicOk())
        throw new Exception("Ultrasonic system is not perfect. Zero flow calibration needs perfect system!");
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.TestModePrepared, progress, cancelToken);
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.ZeroOffsetTestStart, progress, cancelToken);
      byte[] numArray = await this.checkedNfcCommands.mySubunitCommands.SetRfOffAsync(progress, cancelToken);
      progress.BaseMessage = "Run ZeroOffset detection for " + analysingTime.ToString() + " seconds: ";
      progress.Split(analysingTime);
      for (int i = analysingTime; i > 0; --i)
      {
        await Task.Delay(1000, cancelToken);
        progress.Report(i.ToString());
      }
      progress.BaseMessage = "Finish test:  ";
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.ZeroOffsetTestOrderStop, progress, cancelToken);
      int waitLoops = 0;
      while (true)
      {
        S4_SystemState deviceState = await this.GetDeviceState(progress, cancelToken);
        if (deviceState.TestState != Test_State.TEST_STATE_STOPPED)
        {
          if (waitLoops <= 20)
          {
            await Task.Delay(500);
            deviceState = (S4_SystemState) null;
            ++waitLoops;
          }
          else
            break;
        }
        else
          goto label_16;
      }
      throw new TimeoutException("Timeout: " + Test_State.TEST_STATE_STOPPED.ToString());
label_16:
      AddressRange TDC_zeroOffsetMeasRange = mm.GetRangeOfParameters(new S4_Params[2]
      {
        S4_Params.zeroOffsetMeasUnit1,
        S4_Params.zeroOffsetMeasUnit2
      });
      AddressRange TDC_zeroOffsetSetRange = mm.GetRangeOfParameters(new S4_Params[2]
      {
        S4_Params.zeroOffsetUnit1,
        S4_Params.zeroOffsetUnit2
      });
      await this.checkedNfcCommands.ReadMemoryAsync(TDC_zeroOffsetMeasRange, (DeviceMemory) mm, progress, cancelToken);
      await this.checkedNfcCommands.ReadMemoryAsync(TDC_zeroOffsetSetRange, (DeviceMemory) mm, progress, cancelToken);
      ZeroFlowCheckResults checkResults = new ZeroFlowCheckResults()
      {
        TransducerPair1CalibrationOffset = mm.GetParameterValue<int>(S4_Params.zeroOffsetUnit1),
        TransducerPair2CalibrationOffset = mm.GetParameterValue<int>(S4_Params.zeroOffsetUnit2),
        TransducerPair1Offset = mm.GetParameterValue<int>(S4_Params.zeroOffsetMeasUnit1),
        TransducerPair2Offset = mm.GetParameterValue<int>(S4_Params.zeroOffsetMeasUnit2)
      };
      checkResults.TransducerPair1OffsetCalibrationError = checkResults.TransducerPair1Offset - checkResults.TransducerPair1CalibrationOffset;
      checkResults.TransducerPair2OffsetCalibrationError = checkResults.TransducerPair2Offset - checkResults.TransducerPair2CalibrationOffset;
      if (calibrate)
      {
        progress.BaseMessage = "Calibrate device:  ";
        mm.SetParameterValue<int>(S4_Params.zeroOffsetUnit1, checkResults.TransducerPair1Offset);
        mm.SetParameterValue<int>(S4_Params.zeroOffsetUnit2, checkResults.TransducerPair2Offset);
        progress.Split(2);
        mm.GarantMemoryAvailable(TDC_zeroOffsetSetRange);
        await this.checkedNfcCommands.WriteMemoryAsync(TDC_zeroOffsetSetRange, (DeviceMemory) mm, progress, cancelToken);
      }
      await this.nfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
      ZeroFlowCheckResults flowCheckResults = checkResults;
      mm = (S4_DeviceMemory) null;
      tdcInternals = (S4_TDC_Internals) null;
      TDC_zeroOffsetMeasRange = (AddressRange) null;
      TDC_zeroOffsetSetRange = (AddressRange) null;
      checkResults = (ZeroFlowCheckResults) null;
      return flowCheckResults;
    }

    public async Task<UltrasonicTestResults> RunUltrasonicTest(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (new FirmwareVersion(this.myMeters.checkedWorkMeter.deviceIdentification.FirmwareVersion.Value) < (object) "0.2.1 IUW")
        throw new Exception("This firmware dont support the UltrasonicTest");
      S4_TDC_Internals tdcInternals = new S4_TDC_Internals(this.myMeters.checkedWorkMeter, this.checkedCommands);
      UltrasonicTestResults ultrasonicTestResults = await tdcInternals.RunUltrasonicTestAsync(progress, cancelToken);
      tdcInternals = (S4_TDC_Internals) null;
      return ultrasonicTestResults;
    }

    public async Task<UltrasonicState> ReadUltrasonicState(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_TDC_Internals tdcInternals = new S4_TDC_Internals(this.myMeters.checkedWorkMeter, this.checkedCommands);
      UltrasonicState ultrasonicStateAsync = await tdcInternals.ReadAndGetUltrasonicStateAsync(progress, cancelToken);
      tdcInternals = (S4_TDC_Internals) null;
      return ultrasonicStateAsync;
    }

    private ushort[] Sliding_Average(
      ushort[] sourceData,
      uint startOffset,
      uint data_size,
      uint average_window_size)
    {
      StringBuilder stringBuilder = new StringBuilder();
      uint length = data_size - average_window_size + data_size % average_window_size;
      ushort[] numArray = new ushort[(int) length];
      for (int index1 = 0; (long) index1 < (long) length; ++index1)
      {
        for (int index2 = 0; (long) index2 < (long) average_window_size; ++index2)
          numArray[index1] += sourceData[(long) startOffset + (long) index1 + (long) index2];
        numArray[index1] = (ushort) ((uint) numArray[index1] / average_window_size);
        stringBuilder.Append(" " + numArray[index1].ToString("x04"));
      }
      this.S4_HandlerFunctionsLogger.Trace("Curve average data: " + stringBuilder.ToString());
      return numArray;
    }

    public async Task SetTransducerTestVoltage(
      byte Uslt_Offset_mV,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_DeviceMemory meterMemory = this.myMeters.checkedConnectedMeter.meterMemory;
      meterMemory.SetParameterValue<byte>(S4_Params.Uslt_Offset, Uslt_Offset_mV);
      await this.checkedNfcCommands.WriteMemoryAsync(meterMemory.GetParameterAddressRange(S4_Params.Uslt_Offset), (DeviceMemory) meterMemory, progress, cancelToken);
      meterMemory = (S4_DeviceMemory) null;
    }

    public async Task<double> ReadAndGetTemperature(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (new FirmwareVersion(this.myMeters.checkedWorkMeter.deviceIdentification.FirmwareVersion.Value) < (object) "0.2.1 IUW")
        throw new Exception("This firmware dont support the Temperature");
      double temperature = await this.myMeters.checkedWorkMeter.ReadAndGetTemperature(this.checkedNfcCommands, progress, cancelToken);
      return temperature;
    }

    public async Task ResetSimulationMode(ProgressHandler progress, CancellationToken cancelToken)
    {
      float simulatedVolume = 0.0f;
      S4_DeviceMemory mem = this.myMeters.checkedWorkMeter.meterMemory;
      if (!mem.IsParameterAvailable(S4_Params.devFlowSimulation))
        throw new Exception("Volume simulation not supported in this firmware");
      mem.SetParameterValue<float>(S4_Params.devFlowSimulation, simulatedVolume);
      await this.checkedNfcCommands.WriteMemoryAsync(mem.GetParameterAddressRange(S4_Params.devFlowSimulation), (DeviceMemory) mem, progress, cancelToken);
      mem = (S4_DeviceMemory) null;
    }

    public async Task DeleteAllScenariosInDeviceAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.checkedNfcCommands.DeleteAllModuleConfigurations(progress, cancelToken);
    }

    public async Task DeleteScenarioRangeAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ScenarioRanges scenarioRange)
    {
      S4_ScenarioManager scenarioManager = new S4_ScenarioManager(this.myDeviceCommands);
      ushort rangeBase = (ushort) scenarioRange;
      ushort[] scenarios = await scenarioManager.ReadAvailableModuleConfigurations(progress, cancelToken);
      ushort[] numArray = scenarios;
      for (int index = 0; index < numArray.Length; ++index)
      {
        ushort scenario = numArray[index];
        if ((int) scenario / 100 * 100 == (int) rangeBase)
          await scenarioManager.DeleteScenario(progress, cancelToken, scenario);
      }
      numArray = (ushort[]) null;
      scenarioManager = (S4_ScenarioManager) null;
      scenarios = (ushort[]) null;
    }

    public async Task<DeviceHistoryData> PrepareDeviceHistory(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int num = await this.ReadDeviceAsync(progress, cancelToken, ReadPartsSelection.AllWithoutLogger);
      S4_DeviceHistory history = new S4_DeviceHistory(this);
      DeviceHistoryData historyData = new DeviceHistoryData();
      try
      {
        await history.PrepareHistory(progress, cancelToken);
      }
      catch (Exception ex)
      {
        historyData.HistoryPrepareExceptions = ex;
      }
      historyData.HistoryText = history.ToString();
      DeviceHistoryData deviceHistoryData = historyData;
      history = (S4_DeviceHistory) null;
      historyData = (DeviceHistoryData) null;
      return deviceHistoryData;
    }

    public async Task ProtectionSetByDb(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace(nameof (ProtectionSetByDb));
      BaseDbConnection db = this.myDb;
      uint? nullable = this.myMeters.ConnectedMeter.deviceIdentification.MeterID;
      int MeterID = (int) nullable.Value;
      uint mk = MeterKeyManager.GetOrCreateMeterKey(db, (uint) MeterID);
      mk ^= 2572510890U;
      await this.checkedNfcCommands.LockDevice(mk, progress, cancelToken);
      nullable = this.myMeters.ConnectedMeter.deviceIdentification.FirmwareVersion;
      if (!(new FirmwareVersion(nullable.Value) >= (object) "1.5.1 IUW"))
        return;
      byte[] resultAsync = await this.myDeviceCommands.CommonNfcCommands.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.UpdateNdef);
    }

    public async Task ProtectionSetAgainAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace("ProtectionSetAgain");
      await this.checkedNfcCommands.LockDevice(0U, progress, cancelToken);
      if (!(new FirmwareVersion(this.myMeters.ConnectedMeter.deviceIdentification.FirmwareVersion.Value) >= (object) "1.5.1 IUW"))
        return;
      byte[] resultAsync = await this.myDeviceCommands.CommonNfcCommands.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.UpdateNdef);
    }

    public override async Task ProtectionResetByDb(
      ProgressHandler progress,
      CancellationToken cancelToken,
      BaseDbConnection dbCon = null)
    {
      BaseDbConnection localDB = dbCon != null ? dbCon : this.myDb;
      this.S4_HandlerFunctionsLogger.Trace(nameof (ProtectionResetByDb));
      uint mk = MeterKeyManager.GetMeterKey(localDB, this.myMeters.ConnectedMeter.deviceIdentification.MeterID.Value);
      mk ^= 2572510890U;
      await this.checkedNfcCommands.UnlockDevice(mk, progress, cancelToken);
      localDB = (BaseDbConnection) null;
    }

    public override bool IsProtected()
    {
      bool isProtected = this.myMeters.checkedWorkMeter.IsProtected;
      if (this.myMeters.WorkMeter == null)
        isProtected = this.myMeters.checkedConnectedMeter.IsProtected;
      return isProtected;
    }

    public override async Task<DeviceHistoryData> GetDeviceHistory(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      DeviceHistoryData deviceHistory = await this.PrepareDeviceHistory(progress, cancelToken);
      return deviceHistory;
    }

    public async Task BackupDeviceAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.S4_HandlerFunctionsLogger.Trace(nameof (BackupDeviceAsync));
      await this.checkedNfcCommands.BackupDeviceAsync(progress, cancelToken);
    }

    public override bool SetBackup(byte[] zippedBuffer)
    {
      return !string.IsNullOrEmpty(this.myMeters.SetBackupMeter(zippedBuffer));
    }

    public uint? GetFirmwareVersionFromBackupMeter()
    {
      return this.myMeters.BackupMeter != null && this.myMeters.BackupMeter.meterMemory != null ? this.myMeters.BackupMeter.deviceIdentification.FirmwareVersion : new uint?();
    }

    public override bool SetConfigurationParameterFromBackup() => true;

    public override async Task ResetDeviceAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.ResetDeviceAsync(progress, cancelToken, false);
    }

    public async Task ResetDeviceAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      bool loadBackup)
    {
      int defaultRepeats = this.configList.MaxRequestRepeat;
      try
      {
        this.S4_HandlerFunctionsLogger.Trace("ResetDeviceAsync. LoadBackup=" + loadBackup.ToString());
        this.configList.MaxRequestRepeat = 1;
        await this.checkedNfcCommands.ResetDeviceAsync(progress, cancelToken, loadBackup);
        this.configList.MaxRequestRepeat = 10;
        DeviceIdentification deviceIdentification = await this.ReadVersionAsync(progress, cancelToken);
        deviceIdentification = (DeviceIdentification) null;
      }
      finally
      {
        this.configList.MaxRequestRepeat = defaultRepeats;
      }
    }

    public async Task<S4_ChecksumCheckResults> CheckAllChecksums(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_ChecksumManager cm = new S4_ChecksumManager(this.checkedNfcCommands);
      S4_ChecksumCheckResults checksumCheckResults = await cm.CheckAllChecksums(progress, cancelToken);
      cm = (S4_ChecksumManager) null;
      return checksumCheckResults;
    }

    public async Task GenerateAllChecksums(ProgressHandler progress, CancellationToken cancelToken)
    {
      S4_ChecksumManager cm = new S4_ChecksumManager(this.checkedNfcCommands);
      await cm.GenerateAllChecksums(progress, cancelToken);
      cm = (S4_ChecksumManager) null;
    }

    public async Task GenerateFirmwareChecksums(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_ChecksumManager cm = new S4_ChecksumManager(this.checkedNfcCommands);
      await cm.GenerateFirmwareChecksums(progress, cancelToken);
      cm = (S4_ChecksumManager) null;
    }

    public async Task GenerateParameterChecksum(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_ChecksumManager cm = new S4_ChecksumManager(this.checkedNfcCommands);
      await cm.GenerateParameterChecksum(progress, cancelToken);
      cm = (S4_ChecksumManager) null;
    }

    public async Task<S4_CurrentData> ReadCurrentDataAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_CurrentData currentData = await this.checkedCommands.ReadCurrentDataAsync(progress, cancelToken);
      S4_CurrentData s4CurrentData = currentData;
      currentData = (S4_CurrentData) null;
      return s4CurrentData;
    }

    public async Task<S4_FunctionalState> ReadAlliveAndStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_FunctionalState currentData = await this.checkedCommands.ReadAlliveAndStateAsync(progress, cancelToken);
      S4_FunctionalState s4FunctionalState = currentData;
      currentData = (S4_FunctionalState) null;
      return s4FunctionalState;
    }

    public async Task<DeviceStateCounter> ReadStateCounterAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      DeviceStateCounter stateCounters = await this.checkedCommands.ReadStateCounterAsync(progress, cancelToken);
      DeviceStateCounter deviceStateCounter = stateCounters;
      stateCounters = (DeviceStateCounter) null;
      return deviceStateCounter;
    }

    public async Task<ObservableCollection<LoggerListItem>> ReadLoggerListAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      ObservableCollection<LoggerListItem> observableCollection = await this.myLoggerManager.ReadLoggerListAsync(this.checkedNfcCommands, progress, cancelToken);
      return observableCollection;
    }

    public async Task<List<KeyValuePair<string, string>>> ReadLoggerDataAsListAsync(
      string LoggerName,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<KeyValuePair<string, string>> keyValuePairList = await this.myLoggerManager.ReadLoggerDataAsListAsync(LoggerName, this.checkedCommands, progress, cancelToken);
      return keyValuePairList;
    }

    public async Task<IEnumerable> ReadLoggerDataAsync(
      string LoggerName,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      IEnumerable enumerable = await this.myLoggerManager.ReadLoggerDataAsync(LoggerName, this.checkedCommands, progress, cancelToken);
      return enumerable;
    }

    public async Task<S4_SmartFunctionInfo> ReadSmartFunctionInfoAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_SmartFunctionInfo smartFunctionInfo = await this.checkedCommands.ReadSmartFunctionInfoAsync(progress, cancelToken);
      S4_SmartFunctionInfo smartFunctionInfo1 = smartFunctionInfo;
      smartFunctionInfo = (S4_SmartFunctionInfo) null;
      return smartFunctionInfo1;
    }

    public async Task DeleteAllSmartFunctionsAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.checkedCommands.DeleteAllSmartFunctionsAsync(progress, cancelToken);
    }

    public async Task ClearEventLoggerAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      S4_DeviceMemory memory = this.myMeters.ConnectedMeter.meterMemory;
      S4_LoggerManager.ClearEventLoggerMemory(memory);
      memory = (S4_DeviceMemory) null;
    }

    public async Task ClearMonthLoggerAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      S4_DeviceMemory memory = this.myMeters.ConnectedMeter.meterMemory;
      S4_LoggerManager.ClearMonthLoggerInMemory(memory);
      memory = (S4_DeviceMemory) null;
    }

    public async Task ResetNDCModuleStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.checkedCommands.ResetNDCModuleStateAsync(progress, cancelToken);
    }

    public async Task ClearSysState(ProgressHandler progress, CancellationToken cancelToken)
    {
      string paramName = S4_Params.sysState.ToString();
      uint sysStateAddress = this.myMeters.ConnectedMeter.meterMemory.GetParameterAddress(paramName);
      byte[] sysStateData = this.myMeters.ConnectedMeter.meterMemory.GetDataForParameter(paramName);
      await this.checkedNfcCommands.ClearSysStateAsync(progress, cancelToken, sysStateAddress, sysStateData);
      paramName = (string) null;
      sysStateData = (byte[]) null;
    }

    public S4_RadioTests RadioTests
    {
      get
      {
        if (this.radioTests == null)
          this.radioTests = new S4_RadioTests(this);
        return this.radioTests;
      }
    }

    public S4_CurrentMeasure GetCurrentMeasureObject() => new S4_CurrentMeasure(this);

    public void SetSAP_Type(
      int typeMeterInfoID,
      CommonOverwriteGroups[] groupsToOverwrite,
      SortedList<OverrideID, ConfigurationParameter> SAPConfigParameterList)
    {
      S4_Meter checkedWorkMeter = this.myMeters.checkedWorkMeter;
      if (checkedWorkMeter.meterMemory.SmartFunctionFlashRange != null)
      {
        if (checkedWorkMeter.MySmartFunctionManager == null)
          checkedWorkMeter.MySmartFunctionManager = new S4_SmartFunctionManager(checkedWorkMeter.meterMemory);
        checkedWorkMeter.MySmartFunctionManager.FunctionReloadRequired = true;
      }
      checkedWorkMeter.SetTimeForTimeZoneFromPcTime = true;
      checkedWorkMeter.InitEventsOnWrite = true;
      checkedWorkMeter.ScenariosRequired = true;
      this.OpenType(typeMeterInfoID);
      this.OverwriteFromType(groupsToOverwrite);
      this.SetConfigurationParameters(SAPConfigParameterList, 0);
    }

    public enum S4_OverwriteGroups
    {
      IdentData = 0,
      UltrasonicCalibration = 2,
      UltrasonicHydraulicTestSetup = 3,
      UltrasonicLimits = 4,
      UltrasonicTempSensorCurve = 5,
      MenuDefinition = 6,
      RTC_Calibration = 12, // 0x0000000C
      ConfigurationParameters = 13, // 0x0000000D
      CarrierFrequencyCalibration = 14, // 0x0000000E
      ZeroFlowCalibration = 15, // 0x0000000F
    }

    public enum MeasurementParts : ushort
    {
      All,
    }
  }
}
