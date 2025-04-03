// Decompiled with JetBrains decompiler
// Type: DeviceCollector.DeviceCollectorFunctions
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class DeviceCollectorFunctions : 
    IDeviceCollector,
    I_ZR_Component,
    ICancelable,
    IReadoutConfig,
    IWindow
  {
    internal static Logger logger = LogManager.GetLogger(nameof (DeviceCollectorFunctions));
    public const string MyComponentName = "DeviceCollector";
    public const string SerialBusDateFormat = "dd.MM.yyyy";
    public const string SerialBusDateTimeFormat = "dd.MM.yyyy HH:mm:ss";
    internal static ResourceManager SerialBusMessage = new ResourceManager("DeviceCollector.BusRes", typeof (DeviceCollectorFunctions).Assembly);
    private static ByteField EmptyByteField = new ByteField(0);
    private static long JobCounter = 0;
    private bool _breakRequest = false;
    internal bool DeviceIsModified = false;
    internal bool MinoHeadCombiConnected = false;
    internal DeviceCollectorWindow BusWindow;
    internal WavePortConnector MyWavePort;
    internal Thread BusThread;
    private int connectionProfileID = -1;
    public DeviceList MyDeviceList;
    public IAsyncFunctions MyCom;
    internal Receiver MyReceiver;
    internal DeviceCollectorFunctions.Functions RunningFunction;
    internal BusStatusClass BusState;
    internal int WorkCounter;
    internal DeviceInfo TempDeviceInfo = new DeviceInfo();
    internal BusInfo MyBusInfo;
    private const int MaxDataBytes = 250;
    private RadioReader radioReader;
    private EDC edcHandler;
    private PDC pdcHandler;
    private SmokeDetector smokeDetectorHandler;
    public byte[] DataBytes = new byte[250];
    internal bool IsPluginObject = false;
    private bool readoutConfigByBusFile = false;
    private bool autosave = false;
    private string MessageBaseInfo = string.Empty;
    private ConfigList ConfigList;

    public event EventHandler<BusDevice> OnDeviceListChanged;

    public event EventHandlerEx<int> OnProgress;

    public event EventHandlerEx<string> OnProgressMessage;

    public bool BreakRequest
    {
      get => this._breakRequest;
      set
      {
        this._breakRequest = value;
        if (this.MyCom == null)
          return;
        this.MyCom.BreakRequest = this._breakRequest;
      }
    }

    public int ConnectionProfileID => this.connectionProfileID;

    public bool ReadoutConfigByBusFile
    {
      get => this.readoutConfigByBusFile;
      set
      {
        this.readoutConfigByBusFile = value;
        if (this.readoutConfigByBusFile)
          this.DisableConfigList();
        (this.MyCom as AsyncFunctions).ReadoutConfigByBusFile = this.readoutConfigByBusFile;
      }
    }

    public bool Autosave
    {
      get => this.autosave;
      set
      {
        this.autosave = value;
        if (this.autosave)
          this.ReadoutConfigByBusFile = true;
        if (!this.IsPluginObject)
          return;
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("DeviceCollector", nameof (Autosave), this.autosave.ToString());
      }
    }

    public bool DisableBusWriteOnDispose
    {
      get => !this.autosave;
      set => this.Autosave = !value;
    }

    public string MinomatV4_Challenge { get; set; }

    public string MinomatV4_DurationDay { get; set; }

    public string MinomatV4_DurationDueDate { get; set; }

    public string MinomatV4_DurationMonth { get; set; }

    public string MinomatV4_DurationQuarterHour { get; set; }

    public string MinomatV4_GSM_ID { get; set; }

    public string MinomatV4_MinolID { get; set; }

    public string MinomatV4_SessionKey { get; set; }

    public string MinomatV4_SourceAddress { get; set; }

    public bool SendFirstApplicationReset { get; set; }

    public bool UseREQ_UD2_5B { get; set; }

    public bool SendFirstSND_NKE { get; set; }

    public bool IsMultiTelegrammEnabled { get; set; }

    public IAsyncFunctions AsyncCom => this.MyCom;

    internal ZR_ClassLibrary.BusMode MyBusMode { get; set; }

    public PointToPointDevices? SelectedDeviceMBusType
    {
      get
      {
        ZR_ClassLibrary.BusMode baseMode = this.GetBaseMode();
        if (baseMode != ZR_ClassLibrary.BusMode.MBus && baseMode != 0)
          return new PointToPointDevices?();
        BusDevice selectedDevice = this.GetSelectedDevice();
        if (selectedDevice == null)
          return new PointToPointDevices?();
        if (selectedDevice.DeviceType == DeviceTypes.TemperatureSensor)
          return new PointToPointDevices?(PointToPointDevices.TemperatureSensor);
        if (selectedDevice.DeviceType == DeviceTypes.HumiditySensor)
          return new PointToPointDevices?(PointToPointDevices.HumiditySensor);
        switch (selectedDevice)
        {
          case EDC _:
            return new PointToPointDevices?(PointToPointDevices.EDC);
          case PDC _:
            return new PointToPointDevices?(PointToPointDevices.PDC);
          case Serie3MBus _:
            return new PointToPointDevices?(PointToPointDevices.ZR_Serie3);
          case Serie2MBus _:
            return new PointToPointDevices?(PointToPointDevices.ZR_Serie2);
          case Serie1MBus _:
            return new PointToPointDevices?(PointToPointDevices.ZR_Serie1);
          case MBusDevice _:
            return new PointToPointDevices?(PointToPointDevices.MBus);
          default:
            throw new NotImplementedException();
        }
      }
      set
      {
        if (!value.HasValue)
          throw new NotSupportedException();
        switch (value.Value)
        {
          case PointToPointDevices.MBus:
            if (this.IsSelectedDevice(DeviceTypes.MBus) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.MBus, 0))
              break;
            break;
          case PointToPointDevices.ZR_Serie1:
            if (this.IsSelectedDevice(DeviceTypes.ZR_Serie1) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.ZR_Serie1, 0))
              break;
            break;
          case PointToPointDevices.ZR_Serie2:
            if (this.IsSelectedDevice(DeviceTypes.ZR_Serie2) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.ZR_Serie2, 0))
              break;
            break;
          case PointToPointDevices.ZR_Serie3:
            if (this.IsSelectedDevice(DeviceTypes.ZR_Serie3) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.ZR_Serie3, 0))
              break;
            break;
          case PointToPointDevices.EDC:
            if (this.IsSelectedDevice(DeviceTypes.EDC) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.EDC, 0))
              break;
            break;
          case PointToPointDevices.PDC:
            if (this.IsSelectedDevice(DeviceTypes.PDC) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.PDC, 0))
              break;
            break;
          case PointToPointDevices.TemperatureSensor:
            if (this.IsSelectedDevice(DeviceTypes.TemperatureSensor) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.TemperatureSensor, 0))
              break;
            break;
          case PointToPointDevices.HumiditySensor:
            if (this.IsSelectedDevice(DeviceTypes.HumiditySensor) || this.GetSelectedDevice() != null && !this.DeleteSelectedDevice() || this.AddDevice(DeviceTypes.HumiditySensor, 0))
              break;
            break;
          default:
            throw new NotImplementedException();
        }
      }
    }

    internal string DaKonId { get; set; }

    internal DateTime ReadFromTime { get; set; }

    internal DateTime ReadToTime { get; set; }

    internal string Password { get; set; }

    public int MaxRequestRepeat { get; set; }

    internal int ScanStartAddress { get; set; }

    internal string ScanStartSerialnumber { get; set; }

    internal int OrganizeStartAddress { get; set; }

    internal int CycleTime { get; set; }

    internal bool OnlySecondaryAddressing { get; set; }

    internal bool FastSecondaryAddressing { get; set; }

    internal bool KeepExistingDestinationAddress { get; set; }

    internal bool ChangeInterfaceBaudrateToo { get; set; }

    internal bool UseExternalKeyForReading { get; set; }

    internal bool BeepSignalForReadResult { get; set; }

    internal bool LogToFileEnabled { get; set; }

    internal string LogFilePath { get; set; }

    public RadioReader RadioReader
    {
      get
      {
        this.GetDeviceCollectorSettingsList();
        if (this.radioReader == null)
          this.radioReader = new RadioReader(this);
        return this.radioReader;
      }
    }

    public SmokeDetector SmokeDetectorHandler
    {
      get
      {
        if (this.smokeDetectorHandler == null)
          this.smokeDetectorHandler = new SmokeDetector(this);
        return this.smokeDetectorHandler;
      }
    }

    public EDC EDCHandler
    {
      get
      {
        if (this.edcHandler == null)
          this.edcHandler = new EDC(this);
        return this.edcHandler;
      }
    }

    public PDC PDCHandler
    {
      get
      {
        if (this.pdcHandler == null)
          this.pdcHandler = new PDC(this);
        return this.pdcHandler;
      }
    }

    internal DeviceCollectorFunctions()
    {
      this.IsPluginObject = true;
      this.BaseConstructor((IAsyncFunctions) null);
      string str = PlugInLoader.GmmConfiguration.GetValue("DeviceCollector", nameof (Autosave));
      if (!string.IsNullOrEmpty(str))
        this.Autosave = bool.Parse(str);
      this.MyBusInfo = new BusInfo(this, true, this.autosave);
    }

    public DeviceCollectorFunctions(IAsyncFunctions SpecialAsyncCom)
    {
      this.BaseConstructor(SpecialAsyncCom);
      this.MyBusInfo = new BusInfo(this, true, false);
    }

    public DeviceCollectorFunctions(IAsyncFunctions asyncCom, bool useFileBusSettings)
    {
      this.BaseConstructor(asyncCom);
      this.ReadoutConfigByBusFile = useFileBusSettings;
      this.MyBusInfo = new BusInfo(this, true, this.ReadoutConfigByBusFile);
      if (this.MyBusInfo == null)
        return;
      this.MyCom.SetCommParameter(this.MyBusInfo.CommParam);
      this.SetDeviceCollectorSettings(this.MyBusInfo.ReadoutSettingsList);
    }

    public DeviceCollectorFunctions(
      IAsyncFunctions SpecialAsyncCom,
      DeviceCollectorFunctions.Initialise init)
    {
      this.BaseConstructor(SpecialAsyncCom);
      this.ReadoutConfigByBusFile = true;
      this.MyBusInfo = init != DeviceCollectorFunctions.Initialise.BusInfo && init != DeviceCollectorFunctions.Initialise.Both ? new BusInfo(this) : new BusInfo(this, false, true);
      if (this.MyBusInfo != null)
        this.SetDeviceCollectorSettings(this.MyBusInfo.ReadoutSettingsList);
      if (init != DeviceCollectorFunctions.Initialise.AsyncCom && init != DeviceCollectorFunctions.Initialise.Both)
        return;
      this.MyCom.SetCommParameter(this.MyBusInfo.CommParam);
    }

    private void BaseConstructor(IAsyncFunctions SpecialAsyncCom)
    {
      this.SetDefaultSettings();
      this.RunningFunction = DeviceCollectorFunctions.Functions.NoFunction;
      this.MyReceiver = (Receiver) null;
      this.BusState = new BusStatusClass(this);
      this.BreakRequest = false;
      this.IsMultiTelegrammEnabled = true;
      this.SendFirstSND_NKE = false;
      this.UseREQ_UD2_5B = true;
      this.SendFirstApplicationReset = false;
      if (SpecialAsyncCom == null)
      {
        if (!this.IsPluginObject)
          throw new Exception("AsyncCom object is not available");
        this.MyCom = (IAsyncFunctions) PlugInLoader.GetPlugIn("AsyncCom").GetPluginInfo().Interface;
      }
      else
        this.MyCom = SpecialAsyncCom;
      this.MyDeviceList = this.GetDeviceListForBusMode();
      this.MyCom.OnAsyncComMessage += new EventHandler<GMM_EventArgs>(this.AsyncComMessage);
    }

    public void Dispose()
    {
      this.pdcHandler = (PDC) null;
      this.edcHandler = (EDC) null;
      if (this.radioReader != null)
        this.radioReader.Close();
      this.smokeDetectorHandler = (SmokeDetector) null;
      this.radioReader = (RadioReader) null;
    }

    public void RaiseEventOnDeviceListChanged(DeviceList sender, BusDevice e)
    {
      if (this.OnDeviceListChanged == null)
        return;
      this.OnDeviceListChanged((object) sender, e);
    }

    public void SetDefaultSettings()
    {
      this.MyBusMode = ZR_ClassLibrary.BusMode.MBusPointToPoint;
      this.ReadFromTime = new DateTime(1900, 1, 1);
      this.ReadToTime = SystemValues.DateTimeNow.AddYears(1);
      this.DaKonId = string.Empty;
      this.Password = string.Empty;
      this.LogFilePath = string.Empty;
      this.ScanStartSerialnumber = "fffffff0";
      this.OrganizeStartAddress = 1;
      this.MaxRequestRepeat = 3;
      this.ScanStartAddress = 0;
      this.CycleTime = 5;
      this.OnlySecondaryAddressing = false;
      this.FastSecondaryAddressing = false;
      this.ChangeInterfaceBaudrateToo = false;
      this.UseExternalKeyForReading = false;
      this.BeepSignalForReadResult = false;
      this.KeepExistingDestinationAddress = false;
      this.LogToFileEnabled = false;
      this.IsMultiTelegrammEnabled = true;
      this.UseREQ_UD2_5B = true;
      this.SendFirstApplicationReset = false;
      this.SendFirstSND_NKE = false;
    }

    public bool SetDeviceCollectorSettings(
      SortedList<DeviceCollectorSettings, object> settings)
    {
      return this.ChangeDeviceCollectorSettings(settings);
    }

    public bool SetDeviceCollectorSettings(SortedList<string, string> settings)
    {
      return this.ChangeDeviceCollectorSettings(settings);
    }

    public bool SetDeviceCollectorSettings(string deviceCollectorSettings)
    {
      if (string.IsNullOrEmpty(deviceCollectorSettings))
        return false;
      string[] strArray = deviceCollectorSettings.Split(';');
      if (strArray.Length == 0)
        return false;
      SortedList<DeviceCollectorSettings, object> settings = new SortedList<DeviceCollectorSettings, object>();
      List<string> stringList = new List<string>((IEnumerable<string>) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (DeviceCollectorSettings)));
      for (int index = 0; index + 1 < strArray.Length; index += 2)
      {
        if (stringList.Contains(strArray[index]))
        {
          DeviceCollectorSettings key = (DeviceCollectorSettings) Enum.Parse(typeof (DeviceCollectorSettings), strArray[index], true);
          string str = strArray[index + 1];
          if (!settings.ContainsKey(key))
            settings.Add(key, (object) str);
          else
            DeviceCollectorFunctions.logger.Error("Multiple DeviceCollectorSettings detected: {0}={1} and {2}={3}", new object[4]
            {
              (object) key,
              (object) strArray[index + 1],
              (object) key,
              settings[key]
            });
        }
      }
      return this.SetDeviceCollectorSettings(settings);
    }

    public bool ChangeDeviceCollectorSettings(SortedList<string, string> settings)
    {
      if (settings == null)
        throw new ArgumentNullException(nameof (settings));
      List<string> stringList = new List<string>((IEnumerable<string>) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (DeviceCollectorSettings)));
      SortedList<DeviceCollectorSettings, object> settings1 = new SortedList<DeviceCollectorSettings, object>();
      int index = settings.IndexOfKey("ConnectionProfileID");
      if (index >= 0)
        this.connectionProfileID = int.Parse(settings.Values[index]);
      foreach (KeyValuePair<string, string> setting in settings)
      {
        if (stringList.Contains(setting.Key))
        {
          DeviceCollectorSettings key = (DeviceCollectorSettings) Enum.Parse(typeof (DeviceCollectorSettings), setting.Key, true);
          string str = setting.Value;
          settings1.Add(key, (object) str);
        }
      }
      return this.ChangeDeviceCollectorSettings(settings1);
    }

    public bool ChangeDeviceCollectorSettings(
      SortedList<DeviceCollectorSettings, object> settings)
    {
      if (settings == null)
        return true;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<DeviceCollectorSettings, object> setting in settings)
      {
        stringBuilder.Append((object) setting.Key);
        stringBuilder.Append("=");
        stringBuilder.Append(setting.Value);
        stringBuilder.Append(ZR_Constants.SystemNewLine);
      }
      DeviceCollectorFunctions.logger.Info("Set DeviceCollectorSettings: " + stringBuilder.ToString());
      try
      {
        if (settings.ContainsKey(DeviceCollectorSettings.BusMode))
          this.SetBaseMode((ZR_ClassLibrary.BusMode) Enum.Parse(typeof (ZR_ClassLibrary.BusMode), settings[DeviceCollectorSettings.BusMode].ToString(), true));
        if (settings.ContainsKey(DeviceCollectorSettings.FromTime))
          this.ReadFromTime = ZR_ClassLibrary.Util.ToDateTime(settings[DeviceCollectorSettings.FromTime]);
        if (settings.ContainsKey(DeviceCollectorSettings.ToTime))
          this.ReadToTime = ZR_ClassLibrary.Util.ToDateTime(settings[DeviceCollectorSettings.ToTime]);
        if (settings.ContainsKey(DeviceCollectorSettings.DaKonId))
          this.DaKonId = ZR_ClassLibrary.Util.ToString(settings[DeviceCollectorSettings.DaKonId]).Trim();
        if (settings.ContainsKey(DeviceCollectorSettings.Password))
          this.Password = ZR_ClassLibrary.Util.ToString(settings[DeviceCollectorSettings.Password]);
        if (settings.ContainsKey(DeviceCollectorSettings.LogFilePath))
          this.LogFilePath = ZR_ClassLibrary.Util.ToString(settings[DeviceCollectorSettings.LogFilePath]);
        if (settings.ContainsKey(DeviceCollectorSettings.ScanStartSerialnumber))
          this.ScanStartSerialnumber = ZR_ClassLibrary.Util.ToString(settings[DeviceCollectorSettings.ScanStartSerialnumber]);
        if (settings.ContainsKey(DeviceCollectorSettings.OrganizeStartAddress))
          this.OrganizeStartAddress = ZR_ClassLibrary.Util.ToInteger(settings[DeviceCollectorSettings.OrganizeStartAddress]);
        if (settings.ContainsKey(DeviceCollectorSettings.MaxRequestRepeat))
          this.MaxRequestRepeat = ZR_ClassLibrary.Util.ToInteger(settings[DeviceCollectorSettings.MaxRequestRepeat]);
        if (settings.ContainsKey(DeviceCollectorSettings.ScanStartAddress))
          this.ScanStartAddress = ZR_ClassLibrary.Util.ToInteger(settings[DeviceCollectorSettings.ScanStartAddress]);
        if (settings.ContainsKey(DeviceCollectorSettings.CycleTime))
          this.CycleTime = ZR_ClassLibrary.Util.ToInteger(settings[DeviceCollectorSettings.CycleTime]);
        if (settings.ContainsKey(DeviceCollectorSettings.OnlySecondaryAddressing))
          this.OnlySecondaryAddressing = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.OnlySecondaryAddressing]);
        if (settings.ContainsKey(DeviceCollectorSettings.FastSecondaryAddressing))
          this.FastSecondaryAddressing = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.FastSecondaryAddressing]);
        if (settings.ContainsKey(DeviceCollectorSettings.ChangeInterfaceBaudrateToo))
          this.ChangeInterfaceBaudrateToo = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.ChangeInterfaceBaudrateToo]);
        if (settings.ContainsKey(DeviceCollectorSettings.UseExternalKeyForReading))
          this.UseExternalKeyForReading = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.UseExternalKeyForReading]);
        if (settings.ContainsKey(DeviceCollectorSettings.BeepSignalOnReadResult))
          this.BeepSignalForReadResult = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.BeepSignalOnReadResult]);
        if (settings.ContainsKey(DeviceCollectorSettings.KeepExistingDestinationAddress))
          this.KeepExistingDestinationAddress = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.KeepExistingDestinationAddress]);
        if (settings.ContainsKey(DeviceCollectorSettings.LogToFileEnabled))
          this.LogToFileEnabled = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.LogToFileEnabled]);
        if (settings.ContainsKey(DeviceCollectorSettings.IsMultiTelegrammEnabled))
          this.IsMultiTelegrammEnabled = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.IsMultiTelegrammEnabled]);
        if (settings.ContainsKey(DeviceCollectorSettings.UseREQ_UD2_5B))
          this.UseREQ_UD2_5B = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.UseREQ_UD2_5B]);
        if (settings.ContainsKey(DeviceCollectorSettings.SendFirstApplicationReset))
          this.SendFirstApplicationReset = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.SendFirstApplicationReset]);
        if (settings.ContainsKey(DeviceCollectorSettings.SendFirstSND_NKE))
          this.SendFirstSND_NKE = ZR_ClassLibrary.Util.ToBoolean(settings[DeviceCollectorSettings.SendFirstSND_NKE]);
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_Challenge) && settings[DeviceCollectorSettings.MinomatV4_Challenge] != null)
          this.MinomatV4_Challenge = settings[DeviceCollectorSettings.MinomatV4_Challenge].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationDay) && settings[DeviceCollectorSettings.MinomatV4_DurationDay] != null)
          this.MinomatV4_DurationDay = settings[DeviceCollectorSettings.MinomatV4_DurationDay].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationDueDate) && settings[DeviceCollectorSettings.MinomatV4_DurationDueDate] != null)
          this.MinomatV4_DurationDueDate = settings[DeviceCollectorSettings.MinomatV4_DurationDueDate].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationMonth) && settings[DeviceCollectorSettings.MinomatV4_DurationMonth] != null)
          this.MinomatV4_DurationMonth = settings[DeviceCollectorSettings.MinomatV4_DurationMonth].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationQuarterHour) && settings[DeviceCollectorSettings.MinomatV4_DurationQuarterHour] != null)
          this.MinomatV4_DurationQuarterHour = settings[DeviceCollectorSettings.MinomatV4_DurationQuarterHour].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_GSM_ID) && settings[DeviceCollectorSettings.MinomatV4_GSM_ID] != null)
          this.MinomatV4_GSM_ID = settings[DeviceCollectorSettings.MinomatV4_GSM_ID].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_MinolID) && settings[DeviceCollectorSettings.MinomatV4_MinolID] != null)
          this.MinomatV4_MinolID = settings[DeviceCollectorSettings.MinomatV4_MinolID].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_SessionKey) && settings[DeviceCollectorSettings.MinomatV4_SessionKey] != null)
          this.MinomatV4_SessionKey = settings[DeviceCollectorSettings.MinomatV4_SessionKey].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.MinomatV4_SourceAddress) && settings[DeviceCollectorSettings.MinomatV4_SourceAddress] != null)
          this.MinomatV4_SourceAddress = settings[DeviceCollectorSettings.MinomatV4_SourceAddress].ToString();
        if (settings.ContainsKey(DeviceCollectorSettings.SelectedDeviceMBusType))
        {
          ZR_ClassLibrary.BusMode myBusMode = this.MyBusMode;
          if (myBusMode == ZR_ClassLibrary.BusMode.MBus || myBusMode == ZR_ClassLibrary.BusMode.MBusPointToPoint)
          {
            string str = settings[DeviceCollectorSettings.SelectedDeviceMBusType].ToString();
            if (!string.IsNullOrEmpty(str) && Enum.IsDefined(typeof (PointToPointDevices), (object) str))
              this.SelectedDeviceMBusType = new PointToPointDevices?((PointToPointDevices) Enum.Parse(typeof (PointToPointDevices), str, true));
          }
        }
        if (settings.ContainsKey(DeviceCollectorSettings.PrimaryAddress) && this.GetBaseMode() == ZR_ClassLibrary.BusMode.MBus)
        {
          BusDevice selectedDevice = this.GetSelectedDevice();
          if (selectedDevice != null && selectedDevice is MBusDevice)
            selectedDevice.Info.A_Field = (byte) ZR_ClassLibrary.Util.ToInteger(settings[DeviceCollectorSettings.PrimaryAddress]);
        }
        if (settings.ContainsKey(DeviceCollectorSettings.SecondaryAddress))
        {
          if (this.GetBaseMode() == ZR_ClassLibrary.BusMode.MBus)
          {
            BusDevice selectedDevice = this.GetSelectedDevice();
            if (selectedDevice != null && selectedDevice is MBusDevice)
            {
              selectedDevice.Info.MeterNumberOriginal = ZR_ClassLibrary.Util.ConvertUnt32ToBcdUInt32((uint) ZR_ClassLibrary.Util.ToInteger(settings[DeviceCollectorSettings.SecondaryAddress]));
              selectedDevice.Info.MeterNumber = selectedDevice.Info.MeterNumberOriginal.ToString("x08");
            }
          }
        }
      }
      catch (Exception ex)
      {
        string str = "Can not set all readout settings! Error: " + ex.Message;
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
        DeviceCollectorFunctions.logger.Error(ex, str);
      }
      return true;
    }

    public SortedList<DeviceCollectorSettings, object> GetDeviceCollectorSettings()
    {
      return new SortedList<DeviceCollectorSettings, object>()
      {
        {
          DeviceCollectorSettings.BusMode,
          (object) this.MyBusMode
        },
        {
          DeviceCollectorSettings.FromTime,
          (object) this.ReadFromTime
        },
        {
          DeviceCollectorSettings.ToTime,
          (object) this.ReadToTime
        },
        {
          DeviceCollectorSettings.DaKonId,
          (object) this.DaKonId
        },
        {
          DeviceCollectorSettings.Password,
          (object) this.Password
        },
        {
          DeviceCollectorSettings.LogFilePath,
          (object) this.LogFilePath
        },
        {
          DeviceCollectorSettings.ScanStartSerialnumber,
          (object) this.ScanStartSerialnumber
        },
        {
          DeviceCollectorSettings.OrganizeStartAddress,
          (object) this.OrganizeStartAddress
        },
        {
          DeviceCollectorSettings.MaxRequestRepeat,
          (object) this.MaxRequestRepeat
        },
        {
          DeviceCollectorSettings.ScanStartAddress,
          (object) this.ScanStartAddress
        },
        {
          DeviceCollectorSettings.CycleTime,
          (object) this.CycleTime
        },
        {
          DeviceCollectorSettings.OnlySecondaryAddressing,
          (object) this.OnlySecondaryAddressing
        },
        {
          DeviceCollectorSettings.FastSecondaryAddressing,
          (object) this.FastSecondaryAddressing
        },
        {
          DeviceCollectorSettings.ChangeInterfaceBaudrateToo,
          (object) this.ChangeInterfaceBaudrateToo
        },
        {
          DeviceCollectorSettings.UseExternalKeyForReading,
          (object) this.UseExternalKeyForReading
        },
        {
          DeviceCollectorSettings.BeepSignalOnReadResult,
          (object) this.BeepSignalForReadResult
        },
        {
          DeviceCollectorSettings.KeepExistingDestinationAddress,
          (object) this.KeepExistingDestinationAddress
        },
        {
          DeviceCollectorSettings.LogToFileEnabled,
          (object) this.LogToFileEnabled
        },
        {
          DeviceCollectorSettings.IsMultiTelegrammEnabled,
          (object) this.IsMultiTelegrammEnabled
        },
        {
          DeviceCollectorSettings.UseREQ_UD2_5B,
          (object) this.UseREQ_UD2_5B
        },
        {
          DeviceCollectorSettings.SendFirstApplicationReset,
          (object) this.SendFirstApplicationReset
        },
        {
          DeviceCollectorSettings.SendFirstSND_NKE,
          (object) this.SendFirstSND_NKE
        },
        {
          DeviceCollectorSettings.SelectedDeviceMBusType,
          (object) this.SelectedDeviceMBusType
        },
        {
          DeviceCollectorSettings.MinomatV4_Challenge,
          (object) this.MinomatV4_Challenge
        },
        {
          DeviceCollectorSettings.MinomatV4_DurationDay,
          (object) this.MinomatV4_DurationDay
        },
        {
          DeviceCollectorSettings.MinomatV4_DurationDueDate,
          (object) this.MinomatV4_DurationDueDate
        },
        {
          DeviceCollectorSettings.MinomatV4_DurationMonth,
          (object) this.MinomatV4_DurationMonth
        },
        {
          DeviceCollectorSettings.MinomatV4_DurationQuarterHour,
          (object) this.MinomatV4_DurationQuarterHour
        },
        {
          DeviceCollectorSettings.MinomatV4_GSM_ID,
          (object) this.MinomatV4_GSM_ID
        },
        {
          DeviceCollectorSettings.MinomatV4_MinolID,
          (object) this.MinomatV4_MinolID
        },
        {
          DeviceCollectorSettings.MinomatV4_SessionKey,
          (object) this.MinomatV4_SessionKey
        },
        {
          DeviceCollectorSettings.MinomatV4_SourceAddress,
          (object) this.MinomatV4_SourceAddress
        }
      };
    }

    public string GetDeviceCollectorSettingsAsString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<DeviceCollectorSettings, object> collectorSetting in this.GetDeviceCollectorSettings())
      {
        if (collectorSetting.Value != null)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(";");
          stringBuilder.Append(collectorSetting.Key.ToString());
          stringBuilder.Append(";");
          if (collectorSetting.Value is DateTime)
            stringBuilder.Append(((DateTime) collectorSetting.Value).ToString((IFormatProvider) FixedFormates.TheFormates));
          else
            stringBuilder.Append(collectorSetting.Value.ToString());
        }
      }
      return stringBuilder.ToString();
    }

    public SortedList<string, string> GetDeviceCollectorSettingsList()
    {
      SortedList<string, string> collectorSettingsList = new SortedList<string, string>();
      foreach (KeyValuePair<DeviceCollectorSettings, object> collectorSetting in this.GetDeviceCollectorSettings())
      {
        if (collectorSetting.Value != null)
        {
          string key = collectorSetting.Key.ToString();
          string str = !(collectorSetting.Value is DateTime) ? collectorSetting.Value.ToString() : ((DateTime) collectorSetting.Value).ToString((IFormatProvider) CultureInfo.InvariantCulture);
          collectorSettingsList.Add(key, str);
        }
      }
      return collectorSettingsList;
    }

    private void AsyncComMessage(object sender, GMM_EventArgs MessageObj)
    {
      if (MessageObj.TheMessageType == GMM_EventArgs.MessageType.KeyReceived && this.UseExternalKeyForReading || this.OnMessage == null)
        return;
      this.OnMessage(sender, MessageObj);
    }

    public void RaiseProgressEvent(int progressPercentage)
    {
      this.RaiseProgressEvent(progressPercentage, string.Empty);
    }

    public void RaiseProgressEvent(int progressPercentage, string status)
    {
      if (this.OnMessage == null)
        return;
      this.OnMessage((object) this, new GMM_EventArgs(status)
      {
        TheMessageType = GMM_EventArgs.MessageType.MessageAndProgressPercentage,
        ProgressPercentage = progressPercentage
      });
    }

    public bool GetDeviceCollectorInfo(out object InfoObject)
    {
      InfoObject = (object) null;
      return this.MyDeviceList != null && this.MyDeviceList.GetDeviceCollectorInfo(out InfoObject);
    }

    public void ShowBusWindow() => this.ShowBusWindow("");

    public string ShowBusWindow(string ComponentList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.DeviceCollector))
        return "";
      if (this.MyCom != null && this.MyCom.IsLocked && (this.BusWindow == null || this.BusWindow != null && this.MyCom.Owner != this.BusWindow.Name))
      {
        int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", string.Format(DeviceCollectorFunctions.SerialBusMessage.GetString("AsyncComIsLocked"), (object) this.MyCom.Owner));
        return "GMM";
      }
      if (this.BusWindow == null)
        this.BusWindow = new DeviceCollectorWindow(this);
      this.BusWindow.InitStartMenu(ComponentList);
      int num1 = (int) this.BusWindow.ShowDialog();
      return this.BusWindow.StartComponentName;
    }

    public void GMM_Dispose()
    {
      if (this.MyDeviceList is MinomatList && ((MinomatList) this.MyDeviceList).IsConnected)
        ((MinomatList) this.MyDeviceList).DisconnectFromMinomat();
      this.ComClose();
      if (!this.ReadoutConfigByBusFile || !this.autosave)
        return;
      this.MyBusInfo.WriteBusInfoToFile();
      this.MyBusInfo.SaveBusInfoFileNameForPlugIn();
    }

    public bool ComOpen()
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.WaveFlowRadio)
      {
        this.MyCom.Close();
        return this.OpenWaveFlowRadio();
      }
      if (this.MyCom.IsOpen || this.MyCom.Transceiver != ZR_ClassLibrary.TransceiverDevice.MinoHead || this.MyBusMode != ZR_ClassLibrary.BusMode.MinomatV4 && this.MyBusMode != ZR_ClassLibrary.BusMode.MinomatV3)
        return this.MyCom.Open();
      return this.MyCom.Open() && this.MyCom.CallTransceiverFunction(TransceiverDeviceFunction.TransparentModeV3On);
    }

    public bool ComClose()
    {
      try
      {
        return this.MyBusMode == ZR_ClassLibrary.BusMode.WaveFlowRadio ? this.CloseWaveFlowRadio() : this.MyCom.Close();
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        DeviceCollectorFunctions.logger.Error(ex, message);
        return false;
      }
    }

    public bool SetBreak() => this.MyCom.SetBreak();

    public bool ClearBreak() => this.MyCom.ClearBreak();

    public ZR_ClassLibrary.BusMode GetBaseMode() => this.MyBusMode;

    public bool SetBaseMode(ZR_ClassLibrary.BusMode NewBusMode)
    {
      if ((NewBusMode == ZR_ClassLibrary.BusMode.MinomatV2 || NewBusMode == ZR_ClassLibrary.BusMode.MinomatV3 || NewBusMode == ZR_ClassLibrary.BusMode.MinomatV4) && this.MyCom.Transceiver == ZR_ClassLibrary.TransceiverDevice.MinoHead)
        this.MyCom.Close();
      if (NewBusMode == ZR_ClassLibrary.BusMode.WaveFlowRadio)
      {
        this.CloseWaveFlowRadio();
        this.MyCom.Close();
      }
      if (this.MyBusMode == NewBusMode)
        return true;
      this.MyBusMode = NewBusMode;
      if (this.MyCom.IsOpen)
        this.MyCom.ChangeDriverSettings();
      this.MyDeviceList = this.GetDeviceListForBusMode();
      if (this.BusWindow != null)
      {
        this.BusWindow.ClearTables();
        this.BusWindow.RefreshBusInfo();
      }
      return true;
    }

    internal DeviceList GetDeviceListForBusMode()
    {
      DeviceList deviceListForBusMode;
      switch (this.MyBusMode)
      {
        case ZR_ClassLibrary.BusMode.MBusPointToPoint:
        case ZR_ClassLibrary.BusMode.MBus:
        case ZR_ClassLibrary.BusMode.MinomatV3:
        case ZR_ClassLibrary.BusMode.MinomatV4:
        case ZR_ClassLibrary.BusMode.Minol_Device:
        case ZR_ClassLibrary.BusMode.SmokeDetector:
        case ZR_ClassLibrary.BusMode.NFC:
          deviceListForBusMode = (DeviceList) new MBusList(this);
          break;
        case ZR_ClassLibrary.BusMode.WaveFlowRadio:
          deviceListForBusMode = (DeviceList) new WaveFlowList(this);
          break;
        case ZR_ClassLibrary.BusMode.MinomatV2:
          deviceListForBusMode = (DeviceList) new MinomatList(this);
          break;
        case ZR_ClassLibrary.BusMode.MinomatRadioTest:
        case ZR_ClassLibrary.BusMode.Radio2:
        case ZR_ClassLibrary.BusMode.Radio3:
        case ZR_ClassLibrary.BusMode.Radio4:
        case ZR_ClassLibrary.BusMode.wMBusS1:
        case ZR_ClassLibrary.BusMode.wMBusS1M:
        case ZR_ClassLibrary.BusMode.wMBusS2:
        case ZR_ClassLibrary.BusMode.wMBusT1:
        case ZR_ClassLibrary.BusMode.wMBusT2_meter:
        case ZR_ClassLibrary.BusMode.wMBusT2_other:
        case ZR_ClassLibrary.BusMode.wMBusC1A:
        case ZR_ClassLibrary.BusMode.wMBusC1B:
        case ZR_ClassLibrary.BusMode.Radio3_868_95_RUSSIA:
        case ZR_ClassLibrary.BusMode.RadioMS:
          deviceListForBusMode = this.MyDeviceList == null || !(this.MyDeviceList is RadioList) ? (DeviceList) new RadioList(this) : this.MyDeviceList;
          break;
        case ZR_ClassLibrary.BusMode.RelayDevice:
          deviceListForBusMode = this.MyDeviceList == null || !(this.MyDeviceList is RelayList) ? (DeviceList) new RelayList(this) : this.MyDeviceList;
          break;
        default:
          throw new NotImplementedException();
      }
      return deviceListForBusMode;
    }

    public void SetAsyncComSettings(SortedList<string, string> settings)
    {
      this.MyBusInfo.SetAsyncComSettings(settings);
    }

    public void SetAsyncComSettings(SortedList<AsyncComSettings, object> settings)
    {
      this.MyBusInfo.SetAsyncComSettings(settings);
    }

    public SortedList<string, string> GetAsyncComSettings() => this.MyBusInfo.GetBusSettings();

    public string GetAsyncComSettings(AsyncComSettings key) => this.MyBusInfo.GetBusSettings(key);

    public int SetMaxRepeat(int MaxRepeat)
    {
      int maxRequestRepeat = this.MaxRequestRepeat;
      this.MaxRequestRepeat = MaxRepeat;
      return maxRequestRepeat;
    }

    internal void SendProgress(object sender, int progress)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, progress);
    }

    internal void SendProgressMessage(object sender, string progressMessage)
    {
      if (this.OnProgressMessage == null)
        return;
      this.OnProgressMessage(sender, progressMessage);
    }

    public event EventHandler<GMM_EventArgs> OnMessage;

    internal void SendMessage(int MessageInt, GMM_EventArgs.MessageType MessageType)
    {
      if (this.OnMessage == null)
        return;
      GMM_EventArgs e = new GMM_EventArgs(this.MessageBaseInfo, MessageInt, MessageType);
      this.OnMessage((object) this, e);
      if (e.Cancel)
        this.BreakRequest = true;
    }

    internal void SendMessage(
      string MessageString,
      int MessageInt,
      GMM_EventArgs.MessageType MessageType)
    {
      if (this.OnMessage == null)
        return;
      GMM_EventArgs e = new GMM_EventArgs(this.MessageBaseInfo + MessageString, MessageInt, MessageType);
      this.OnMessage((object) this, e);
      if (e.Cancel)
        this.BreakRequest = true;
    }

    internal void SendMessage(GMM_EventArgs e)
    {
      if (!string.IsNullOrEmpty(e.EventMessage))
        DeviceCollectorFunctions.logger.Info(e.EventMessage);
      if (this.OnMessage == null)
        return;
      this.OnMessage((object) this, e);
      Application.DoEvents();
      if (e.Cancel)
        this.BreakRequest = true;
    }

    public void SetMessageInfo(string info) => this.MessageBaseInfo = info;

    public void BreakAllFunctions() => this.BreakRequest = true;

    private bool SetRunningFunction(DeviceCollectorFunctions.Functions NewFunction)
    {
      this.RunningFunction = NewFunction;
      this.WorkCounter = 0;
      ++DeviceCollectorFunctions.JobCounter;
      return true;
    }

    private bool EndRunningFunction()
    {
      this.RunningFunction = DeviceCollectorFunctions.Functions.NoFunction;
      return true;
    }

    public bool WriteBusInfo()
    {
      this.ReadoutConfigByBusFile = true;
      if (!this.MyBusInfo.WriteBusInfoToFile())
        return false;
      this.MyBusInfo.SaveBusInfoFileNameForPlugIn();
      return true;
    }

    public bool WriteBusInfo(string BusInfoName) => this.WriteBusInfo(BusInfoName, false);

    public bool WriteBusInfo(string BusInfoName, bool SetNameToDefault)
    {
      this.ReadoutConfigByBusFile = true;
      this.MyBusInfo.SetBusinfoFilename(BusInfoName);
      this.MyBusInfo.WriteBusInfoToFile();
      if (SetNameToDefault)
        this.MyBusInfo.SetBusinfoFilenameToDefault();
      this.MyBusInfo.SaveBusInfoFileNameForPlugIn();
      return true;
    }

    public bool WriteLastUsedBusInfo(string BusInfoName)
    {
      this.ReadoutConfigByBusFile = true;
      this.MyBusInfo.SetBusinfoFilename(BusInfoName);
      if (!this.MyBusInfo.WriteBusInfoToFile())
        return false;
      this.MyBusInfo.SaveBusInfoFileNameForPlugIn();
      return PlugInLoader.GmmConfiguration.WriteConfigFile();
    }

    public bool ReadBusInfo(string BusInfoName)
    {
      this.ReadoutConfigByBusFile = true;
      this.ComClose();
      return this.ReadBusInfo(BusInfoName, false);
    }

    public bool ReadBusInfo(string BusInfoName, bool SetNameToDefault)
    {
      this.ReadoutConfigByBusFile = true;
      ZR_ClassLibMessages.ClearErrors();
      this.MyDeviceList = this.GetDeviceListForBusMode();
      this.MyBusInfo = new BusInfo(this, BusInfoName, false);
      if (ZR_ClassLibMessages.GetLastError() != 0 || !this.MyCom.SetCommParameter(this.MyBusInfo.CommParam) || !this.SetDeviceCollectorSettings(this.MyBusInfo.ReadoutSettingsList))
        return false;
      if (SetNameToDefault || Path.GetExtension(this.MyBusInfo.BusInfoFilename) == ".defbus")
      {
        this.MyBusInfo.SetBusinfoFilenameToDefault();
        this.MyBusInfo.SaveBusInfoFileNameForPlugIn();
      }
      return true;
    }

    public string GetBusFilename() => this.MyBusInfo.BusInfoFilename;

    public void DeleteBusInfo()
    {
      if (this.MyDeviceList == null)
        return;
      this.MyDeviceList.DeleteBusList();
    }

    public BusDevice GetSelectedDevice()
    {
      return this.MyDeviceList == null ? (BusDevice) null : this.MyDeviceList.SelectedDevice;
    }

    public int GetIndexOfSelectedDevice()
    {
      return this.MyDeviceList == null ? -1 : this.MyDeviceList.GetIndexOfSelectedDevice();
    }

    public bool IsSelectedDevice(DeviceTypes TestType)
    {
      if (this.MyDeviceList == null || this.MyDeviceList.SelectedDevice == null)
        return false;
      switch (TestType)
      {
        case DeviceTypes.MBus:
          if (this.MyDeviceList.SelectedDevice is MBusDevice)
            return true;
          break;
        case DeviceTypes.ZR_Serie1:
          if (this.MyDeviceList.SelectedDevice is Serie1MBus)
            return true;
          break;
        case DeviceTypes.ZR_Serie2:
          return !(this.MyDeviceList.SelectedDevice is Serie3MBus) && this.MyDeviceList.SelectedDevice is Serie2MBus;
        case DeviceTypes.ZR_EHCA:
          if (this.MyDeviceList.SelectedDevice is EHCA_MBus)
            return true;
          break;
        case DeviceTypes.Minol_Device:
          if (this.MyDeviceList.SelectedDevice is MinolDevice)
            return true;
          break;
        case DeviceTypes.ZR_Serie3:
          if (this.MyDeviceList.SelectedDevice is Serie3MBus)
            return true;
          break;
        case DeviceTypes.EDC:
          if (this.MyDeviceList.SelectedDevice is EDC)
            return true;
          break;
        case DeviceTypes.PDC:
          if (this.MyDeviceList.SelectedDevice is PDC)
            return true;
          break;
      }
      return false;
    }

    public bool AddDevice(DeviceTypes NewType, int PrimaryAddress)
    {
      if (this.GetBaseMode() == ZR_ClassLibrary.BusMode.MBusPointToPoint)
        this.MyDeviceList.DeleteBusList();
      if (!this.MyDeviceList.AddDevice(NewType, true))
        return false;
      if (PrimaryAddress > 0 && PrimaryAddress < 256)
      {
        ((MBusDevice) this.MyDeviceList.SelectedDevice).PrimaryDeviceAddress = (byte) PrimaryAddress;
        ((MBusDevice) this.MyDeviceList.SelectedDevice).PrimaryAddressKnown = true;
        ((MBusDevice) this.MyDeviceList.SelectedDevice).PrimaryAddressOk = true;
      }
      this.MyDeviceList.WorkBusAddresses();
      return true;
    }

    public bool AddDevice(DeviceTypes NewType, int PrimaryAddress, long SerialNumber)
    {
      return this.AddDevice(NewType, PrimaryAddress, SerialNumber.ToString());
    }

    public bool AddDevice(DeviceTypes NewType, int PrimaryAddress, string SerialNumber)
    {
      if (this.MyDeviceList is RadioList)
      {
        if (string.IsNullOrEmpty(SerialNumber))
          return false;
        long funkId;
        try
        {
          funkId = ZR_ClassLibrary.Util.ToLong((object) SerialNumber);
        }
        catch
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Invalid number! Value: " + SerialNumber);
          return false;
        }
        return (this.MyDeviceList as RadioList).AddExpectedDevice(funkId);
      }
      if (!this.MyDeviceList.AddDevice(NewType, true))
        return false;
      if (this.MyDeviceList.MyBus.MyBusMode == ZR_ClassLibrary.BusMode.WaveFlowRadio)
      {
        this.SetSerialNumberByWaveFlowDevice(SerialNumber);
        return true;
      }
      if (this.MyDeviceList.SelectedDevice is MinomatDevice)
      {
        this.MyDeviceList.SelectedDevice.Info.MeterNumber = SerialNumber;
        return true;
      }
      if (PrimaryAddress > 0 && PrimaryAddress < 256)
      {
        ((MBusDevice) this.MyDeviceList.SelectedDevice).PrimaryDeviceAddress = (byte) PrimaryAddress;
        ((MBusDevice) this.MyDeviceList.SelectedDevice).PrimaryAddressKnown = true;
        ((MBusDevice) this.MyDeviceList.SelectedDevice).PrimaryAddressOk = true;
      }
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
      {
        this.MyDeviceList.SelectedDevice.Info.MeterNumber = SerialNumber;
        this.MyDeviceList.WorkBusAddresses();
      }
      return true;
    }

    private void SetSerialNumberByWaveFlowDevice(string SerialNumber)
    {
      this.MyDeviceList.SelectedDevice.Info.MeterNumber = SerialNumber;
    }

    public bool ScanFromSerialNumber(string StartSerialnumber)
    {
      this.BreakRequest = false;
      return this.MyDeviceList.ScanFromSerialNumber(StartSerialnumber);
    }

    public bool ScanFromAddress(int ScanAddress)
    {
      this.BreakRequest = false;
      return this.MyDeviceList.ScanFromAddress(ScanAddress);
    }

    public bool SearchSingleDeviceBySerialNumber(string SerialNumber)
    {
      this.BreakRequest = false;
      return this.MyDeviceList.SearchSingleDeviceBySerialNumber(SerialNumber);
    }

    public bool SearchSingleDeviceByPrimaryAddress(int Address)
    {
      this.BreakRequest = false;
      return this.MyDeviceList.SearchSingleDeviceByPrimaryAddress(Address);
    }

    public bool SetSelectedDeviceByPrimaryAddress(int Address)
    {
      return this.MyDeviceList.SelectDeviceByPrimaryAddress(Address);
    }

    public bool SetSelectedDeviceBySerialNumber(string SerialNumber)
    {
      return this.MyDeviceList.SelectDeviceBySerialNumber(SerialNumber);
    }

    public bool SetPhysicalDeviceBySerialNumber(string SerialNumber)
    {
      return this.MyDeviceList.SetPhysicalDeviceBySerialNumber(SerialNumber);
    }

    public bool SetSelectedDeviceByIndex(int index) => this.MyDeviceList.SelectDeviceByIndex(index);

    public bool SetBaudrate(int Baudrate)
    {
      return this.SetBaudrate(Baudrate, this.ChangeInterfaceBaudrateToo);
    }

    public bool SetBaudrate(int Baudrate, bool ChangeAsyncComBaudrate)
    {
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
      {
        this.DeviceIsModified = true;
        this.MyDeviceList.SelectedDevice.UseMaxBaudrate = false;
        if (!((MBusDevice) this.MyDeviceList.SelectedDevice).SetBaudrate(Baudrate))
          return false;
        if (ChangeAsyncComBaudrate)
          this.MyCom.SingleParameter(CommParameter.Baudrate, Baudrate.ToString());
        return true;
      }
      int num = (int) MessageBox.Show("The selected device does not support this SetBaudrate command");
      return false;
    }

    public bool SetPrimaryAddress(int Address)
    {
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
      {
        this.DeviceIsModified = true;
        return Address >= 0 && Address <= 250 && this.MyDeviceList.SetPrimaryAddressOnBus(Address);
      }
      int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("7"));
      return false;
    }

    public bool SetPrimaryAddressWithoutShift(int Address)
    {
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
      {
        this.DeviceIsModified = true;
        return Address >= 0 && Address <= 250 && this.MyDeviceList.SetPrimaryAddressOnBusWithoutShift(Address);
      }
      int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString("7"));
      return false;
    }

    public bool ReadParameter(out string ZDF_Data)
    {
      ZDF_Data = "";
      DeviceInfo Info;
      if (!this.ReadParameter(out Info))
        return false;
      ZDF_Data = Info.GetZDFParameterString();
      return true;
    }

    public bool ReadParameter(out DeviceInfo Info)
    {
      DateTime now = ParameterService.GetNow();
      return this.ReadParameter(out Info, now);
    }

    public bool ReadParameter(out DeviceInfo Info, DateTime timePoint)
    {
      Info = (DeviceInfo) null;
      if (this.MyDeviceList == null)
        return false;
      this.BreakRequest = false;
      if (this.MyDeviceList.SelectedDevice == null && this.MyDeviceList.MyBus.MyBusMode == ZR_ClassLibrary.BusMode.MBusPointToPoint)
        this.MyDeviceList.AddDevice(DeviceTypes.MBus, true);
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
      {
        MBusDevice selectedDevice = this.MyDeviceList.SelectedDevice as MBusDevice;
        int num = 0;
        if (this.OnProgress != null)
          this.OnProgress((object) this, 30);
        if (this.SendFirstApplicationReset)
          selectedDevice.SendMeterApplicationResetAsBroadcast();
        if (this.OnProgress != null)
          this.OnProgress((object) this, 40);
        if (this.SendFirstSND_NKE)
          selectedDevice.SND_NKE_Broadcast();
        int e1 = 40;
        while (!this.BreakRequest)
        {
          ++num;
          if (this.OnProgress != null)
            this.OnProgress((object) this, e1);
          e1 += 10;
          if (e1 > 100)
            e1 = 90;
          if (!selectedDevice.REQ_UD2(timePoint))
          {
            this.SendMessage(new GMM_EventArgs("Failed to receive M-Bus response frame #" + num.ToString(), GMM_EventArgs.MessageType.SimpleMessage));
            return false;
          }
          this.SendMessage(new GMM_EventArgs("Read frame #" + num.ToString(), GMM_EventArgs.MessageType.SimpleMessage));
          if (Info != null)
          {
            DeviceInfo nextInfo = new DeviceInfo(selectedDevice.Info);
            if ((int) Info.ManufacturerCode == (int) nextInfo.ManufacturerCode && (int) Info.Medium == (int) nextInfo.Medium && Info.MeterNumber == nextInfo.MeterNumber)
            {
              foreach (DeviceInfo.MBusParamStruct parameter in nextInfo.ParameterList)
              {
                DeviceInfo.MBusParamStruct p = parameter;
                if (!Info.ParameterList.Exists((Predicate<DeviceInfo.MBusParamStruct>) (e => e.DefineString == p.DefineString && e.ValueString == p.ValueString)))
                  Info.ParameterList.Add(p);
              }
            }
            else
            {
              DeviceInfo deviceInfo = Info.SubDevices.Find((Predicate<DeviceInfo>) (e => (int) e.ManufacturerCode == (int) nextInfo.ManufacturerCode && (int) e.Medium == (int) nextInfo.Medium && e.MeterNumber == nextInfo.MeterNumber));
              if (deviceInfo != null)
              {
                foreach (DeviceInfo.MBusParamStruct parameter in nextInfo.ParameterList)
                {
                  DeviceInfo.MBusParamStruct p = parameter;
                  if (!deviceInfo.ParameterList.Exists((Predicate<DeviceInfo.MBusParamStruct>) (e => e.DefineString == p.DefineString && e.ValueString == p.ValueString)))
                    deviceInfo.ParameterList.Add(p);
                }
              }
              else
                Info.SubDevices.Add(nextInfo);
            }
          }
          else
            Info = new DeviceInfo(selectedDevice.Info);
          if (selectedDevice.followingTelegrammAnnounced && this.IsMultiTelegrammEnabled)
          {
            selectedDevice.followingTelegrammAnnounced = false;
            selectedDevice.followingTelegrammTransmit_FCB_Odd = !selectedDevice.followingTelegrammTransmit_FCB_Odd;
          }
          else
            break;
        }
        this.MyDeviceList.SelectedDevice.Info = Info;
        if (this.OnProgress != null)
          this.OnProgress((object) this, 99);
        return true;
      }
      if (this.MyDeviceList.MyBus.MyBusMode == ZR_ClassLibrary.BusMode.WaveFlowRadio)
        return this.ReadWaveFlowParameter(out Info);
      if (this.MyDeviceList.SelectedDevice is MinomatDevice)
      {
        bool flag = ((MinomatDevice) this.MyDeviceList.SelectedDevice).ReadParameters();
        Info = new DeviceInfo(this.MyDeviceList.SelectedDevice.Info);
        return flag;
      }
      if (this.MyDeviceList.SelectedDevice == null)
        return false;
      Info = this.MyDeviceList.SelectedDevice.Info;
      return true;
    }

    public byte[] GetTransmitBuffer()
    {
      return this.MyDeviceList == null || this.MyDeviceList.SelectedDevice == null || this.MyDeviceList.SelectedDevice.TransmitBuffer == null ? (byte[]) null : this.MyDeviceList.SelectedDevice.TransmitBuffer.Data;
    }

    public byte[] GetReceiveBuffer()
    {
      return this.MyDeviceList == null || this.MyDeviceList.SelectedDevice == null || this.MyDeviceList.SelectedDevice.TotalReceiveBuffer == null ? (byte[]) null : this.MyDeviceList.SelectedDevice.TotalReceiveBuffer.ToArray();
    }

    public UniqueIdentification GetUniqueIdentificationOfSelectedDevice()
    {
      if (this.MyDeviceList == null || this.MyDeviceList.SelectedDevice == null || this.MyDeviceList.SelectedDevice.Info == null)
        return (UniqueIdentification) null;
      return new UniqueIdentification()
      {
        Manufacturer = this.MyDeviceList.SelectedDevice.Info.Manufacturer,
        Version = this.MyDeviceList.SelectedDevice.Info.Version.ToString(),
        Medium = this.MyDeviceList.SelectedDevice.Info.MediumString,
        ParameterList = this.MyDeviceList.SelectedDevice.Info.ParameterListWithoutValues
      };
    }

    public bool GetParameter(out DeviceInfo Info)
    {
      Info = (DeviceInfo) null;
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
      {
        if (!this.MyDeviceList.SelectedDevice.Info.ParameterOk)
          return false;
        Info = new DeviceInfo(this.MyDeviceList.SelectedDevice.Info);
      }
      else if (this.MyDeviceList.MyBus.MyBusMode == ZR_ClassLibrary.BusMode.WaveFlowRadio)
      {
        if (!this.TryGetInfoFromSelectedWaveFlowDevice(out Info))
          return false;
      }
      else
      {
        if (!(this.MyDeviceList.SelectedDevice is MinomatDevice) || !this.MyDeviceList.SelectedDevice.Info.ParameterOk)
          return false;
        Info = new DeviceInfo(this.MyDeviceList.SelectedDevice.Info);
      }
      return true;
    }

    private bool TryGetInfoFromSelectedWaveFlowDevice(out DeviceInfo Info)
    {
      Info = (DeviceInfo) null;
      if (!this.MyDeviceList.SelectedDevice.Info.ParameterOk)
        return false;
      Info = new DeviceInfo(this.MyDeviceList.SelectedDevice.Info);
      return true;
    }

    public bool GetParameter(out DeviceInfo Info, int DeviceListIndex)
    {
      Info = (DeviceInfo) null;
      if (DeviceListIndex >= this.MyDeviceList.bus.Count)
        return false;
      Info = new DeviceInfo(((BusDevice) this.MyDeviceList.bus[DeviceListIndex]).Info);
      return true;
    }

    public List<DeviceInfo> GetParameters()
    {
      return this.MyBusMode == ZR_ClassLibrary.BusMode.RelayDevice && this.MyDeviceList is RelayList deviceList ? deviceList.GetAllParametersOfSelectedDevice() : (List<DeviceInfo>) null;
    }

    public int GetNumberOfDevices() => this.MyDeviceList.bus.Count;

    public bool GetDeviceConfiguration(
      out SortedList<OverrideID, ConfigurationParameter> ConfigParamList)
    {
      ConfigParamList = (SortedList<OverrideID, ConfigurationParameter>) null;
      if (!(this.MyDeviceList.SelectedDevice is MinomatDevice))
        return false;
      ((MinomatDevice) this.MyDeviceList.SelectedDevice).GetDeviceConfiguration(out ConfigParamList);
      return true;
    }

    public bool SerchDeviceAcrossBaudrates()
    {
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
        return ((MBusDevice) this.MyDeviceList.SelectedDevice).SerchBaudrate();
      int num = (int) MessageBox.Show("The selected device does not support the SerchDeviceAcrossBaudrates command");
      return false;
    }

    public bool ResetDevice()
    {
      DeviceCollectorFunctions.logger.Trace(nameof (ResetDevice));
      return this.CheckSelectedDevice() && this.MyDeviceList.SelectedDevice.ResetDevice();
    }

    public bool ResetDevice(bool loadBackup)
    {
      DeviceCollectorFunctions.logger.Trace("ResetDevice load backup");
      return this.CheckSelectedDevice() && this.MyDeviceList.SelectedDevice.ResetDevice(loadBackup);
    }

    public bool ResetDevice(int AfterResetBaudrate)
    {
      if (DeviceCollectorFunctions.logger.IsTraceEnabled)
        DeviceCollectorFunctions.logger.Trace("ResetDevice. AfterResetBaudrate: " + AfterResetBaudrate.ToString());
      return this.CheckSelectedDevice() && this.MyDeviceList.SelectedDevice.ResetDevice(AfterResetBaudrate);
    }

    private bool CheckSelectedDevice()
    {
      if (this.MyDeviceList.SelectedDevice != null)
        return true;
      string str = "Access to not available selected device";
      DeviceCollectorFunctions.logger.Error(str);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, str);
      return false;
    }

    public bool DeviceProtectionGet()
    {
      return this.CheckSelectedDevice() && this.MyDeviceList.SelectedDevice.DeviceProtectionGet();
    }

    public bool DeviceProtectionSet()
    {
      return this.CheckSelectedDevice() && this.MyDeviceList.SelectedDevice.DeviceProtectionSet();
    }

    public bool DeviceProtectionReset(uint meterKey)
    {
      return this.CheckSelectedDevice() && this.MyDeviceList.SelectedDevice.DeviceProtectionReset(meterKey);
    }

    public bool DeviceProtectionSetKey(uint meterKey)
    {
      return this.CheckSelectedDevice() && this.MyDeviceList.SelectedDevice.DeviceProtectionSetKey(meterKey);
    }

    public bool SetEmergencyMode()
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).SetEmergencyMode();
      int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString(DeviceCollectorFunctions.SerialBusMessage.GetString(DeviceCollectorFunctions.SerialBusMessage.GetString("10"))));
      return false;
    }

    public bool RunBackup()
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).RunBackup();
      int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString(DeviceCollectorFunctions.SerialBusMessage.GetString(DeviceCollectorFunctions.SerialBusMessage.GetString("11"))));
      return false;
    }

    public bool DeleteMeterKey(int MeterKey)
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).DeleteMeterKey(MeterKey);
      if (this.MyDeviceList.SelectedDevice is RDM_Bus)
        return ((RDM_Bus) this.MyDeviceList.SelectedDevice).DeleteMeterKey(MeterKey);
      int num = (int) GMM_MessageBox.ShowMessage("DeviceCollector", DeviceCollectorFunctions.SerialBusMessage.GetString(DeviceCollectorFunctions.SerialBusMessage.GetString(DeviceCollectorFunctions.SerialBusMessage.GetString("12"))));
      return false;
    }

    public bool SetNewPin(int NewPin)
    {
      if (this.MyDeviceList.SelectedDevice is RDM_Bus)
        return ((RDM_Bus) this.MyDeviceList.SelectedDevice).SetNewPin(NewPin);
      int num = (int) MessageBox.Show("The selected device does not support the SetNewPin command");
      return false;
    }

    public bool ReadVersion(out ReadVersionData versionData)
    {
      return this.MyDeviceList.SelectedDevice.ReadVersion(out versionData);
    }

    public bool ReadVersion(
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr)
    {
      return this.ReadVersion((int[]) null, out Manufacturer, out Medium, out MBusMeterType, out Version, out MBusSerialNr);
    }

    public bool ReadVersion(
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr,
      out int ConfigAdr,
      out int HardwareMask)
    {
      return this.ReadVersion((int[]) null, out Manufacturer, out Medium, out MBusMeterType, out Version, out MBusSerialNr, out ConfigAdr, out HardwareMask);
    }

    public bool ReadVersion(
      int[] Bautrates,
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr)
    {
      return this.ReadVersion(Bautrates, out Manufacturer, out Medium, out MBusMeterType, out Version, out MBusSerialNr, out int _, out int _);
    }

    public bool ReadVersion(
      int[] Bautrates,
      out short Manufacturer,
      out byte Medium,
      out byte MBusMeterType,
      out long Version,
      out int MBusSerialNr,
      out int HardwareTypeId,
      out int HardwareMask)
    {
      this.BreakRequest = false;
      Manufacturer = (short) 0;
      Medium = (byte) 0;
      MBusMeterType = (byte) 0;
      Version = 0L;
      MBusSerialNr = 0;
      HardwareTypeId = 0;
      HardwareMask = 0;
      if (this.MyDeviceList.SelectedDevice is Serie3MBus)
      {
        ReadVersionData versionData;
        if (!this.MyDeviceList.SelectedDevice.ReadVersion(out versionData))
          return false;
        Manufacturer = versionData.MBusManufacturer;
        Medium = versionData.MBusMedium;
        MBusMeterType = versionData.MBusGeneration;
        MBusSerialNr = (int) versionData.MBusSerialNr;
        Version = (long) versionData.Version;
        HardwareTypeId = (int) versionData.BuildRevision;
        HardwareMask = (int) versionData.HardwareIdentification;
        return true;
      }
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
      {
        if (!((Serie2MBus) this.MyDeviceList.SelectedDevice).ReadVersion(Bautrates))
          return false;
        Manufacturer = this.MyDeviceList.SelectedDevice.Info.ManufacturerCode;
        Medium = this.MyDeviceList.SelectedDevice.Info.Medium;
        MBusMeterType = this.MyDeviceList.SelectedDevice.Info.Version;
        MBusSerialNr = int.Parse(this.MyDeviceList.SelectedDevice.Info.MeterNumber);
        Version = ((Serie2MBus) this.MyDeviceList.SelectedDevice).DeviceVersion;
        HardwareTypeId = ((Serie2MBus) this.MyDeviceList.SelectedDevice).HardwareTypeId;
        HardwareMask = ((Serie2MBus) this.MyDeviceList.SelectedDevice).HardwareMask;
        return true;
      }
      if (this.MyDeviceList.SelectedDevice is RDM_Bus)
        return ((RDM_Bus) this.MyDeviceList.SelectedDevice).ReadVersion(ref Version);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, DeviceCollectorFunctions.SerialBusMessage.GetString("NoVersionCommandAvailable"));
      return false;
    }

    public bool IsDeviceModified()
    {
      if (!this.DeviceIsModified)
        return false;
      this.DeviceIsModified = false;
      return true;
    }

    public bool UseMaxBaudrate()
    {
      if (this.MyDeviceList.SelectedDevice == null)
        return false;
      this.MyDeviceList.SelectedDevice.UseMaxBaudrate = true;
      return true;
    }

    public bool DigitalInputsAndOutputs(
      uint NewOutputMask,
      uint NewOutputState,
      ref uint OldOutputState,
      ref uint OldInputState)
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).DigitalInputsAndOutputs(NewOutputMask, NewOutputState, ref OldOutputState, ref OldInputState);
      int num = (int) MessageBox.Show("The selected device does not support the DigitalInputsAndOutputs command");
      return false;
    }

    public byte[] RunIoTest(IoTestFunctions theFunction)
    {
      return this.MyDeviceList.SelectedDevice == null ? (byte[]) null : this.MyDeviceList.SelectedDevice.RunIoTest(theFunction);
    }

    public bool SetOutput(int OutputNumber, bool State)
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
      {
        if (OutputNumber < 1 || OutputNumber > 2)
          return false;
        uint OldOutputState = 0;
        uint OldInputState = 0;
        uint NewOutputMask = (uint) (1 << OutputNumber - 1);
        uint NewOutputState = 0;
        if (State)
          NewOutputState = NewOutputMask;
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).DigitalInputsAndOutputs(NewOutputMask, NewOutputState, ref OldOutputState, ref OldInputState);
      }
      if (this.MyDeviceList.SelectedDevice is Serie1MBus)
        return OutputNumber >= 1 && OutputNumber <= 2 && ((Serie1MBus) this.MyDeviceList.SelectedDevice).SetOutput(OutputNumber, State);
      int num = (int) MessageBox.Show("The selected device does not support the SetOutput command");
      return false;
    }

    public bool GetInput(int InputNumber, out bool InputState)
    {
      InputState = false;
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
      {
        if (InputNumber < 1 || InputNumber > 2)
          return false;
        uint OldOutputState = 0;
        uint OldInputState = 0;
        if (!((Serie2MBus) this.MyDeviceList.SelectedDevice).DigitalInputsAndOutputs(0U, 0U, ref OldOutputState, ref OldInputState))
          return false;
        uint num = (uint) (1 << InputNumber - 1);
        if ((OldInputState & num) > 0U)
          InputState = true;
        return true;
      }
      if (this.MyDeviceList.SelectedDevice is Serie1MBus)
        return InputNumber >= 1 && InputNumber <= 2 && ((Serie1MBus) this.MyDeviceList.SelectedDevice).GetInput(InputNumber, out InputState);
      int num1 = (int) MessageBox.Show("The selected device does not support the GetInput command");
      return false;
    }

    public ImpulseInputCounters ReadInputCounters()
    {
      return ((Serie3MBus) this.MyDeviceList.SelectedDevice).ReadInputCounters();
    }

    public bool TransmitRadioFrame()
    {
      if (this.MyDeviceList.SelectedDevice is RDM_Bus)
        return ((RDM_Bus) this.MyDeviceList.SelectedDevice).TransmitRadioFrame();
      int num = (int) MessageBox.Show("The selected device does not support the TransmitRadioFrame command");
      return false;
    }

    public bool SetOptoTimeoutSeconds(int Seconds)
    {
      return this.MyDeviceList.SelectedDevice.SetOptoTimeoutSeconds(Seconds);
    }

    public bool FlyingTestActivate() => this.MyDeviceList.SelectedDevice.FlyingTestActivate();

    public bool FlyingTestStart() => this.MyDeviceList.SelectedDevice.FlyingTestStart();

    public bool FlyingTestStop() => this.MyDeviceList.SelectedDevice.FlyingTestStop();

    public bool FlyingTestReadVolume(out float volume, out MBusDeviceState state)
    {
      return this.MyDeviceList.SelectedDevice.FlyingTestReadVolume(out volume, out state);
    }

    public bool AdcTestActivate() => this.MyDeviceList.SelectedDevice.AdcTestActivate();

    public bool CapacityOfTestActivate()
    {
      return this.MyDeviceList.SelectedDevice.CapacityOfTestActivate();
    }

    public bool AdcTestCycleWithSimulatedVolume(float simulationVolume)
    {
      return this.MyDeviceList.SelectedDevice.AdcTestCycleWithSimulatedVolume(simulationVolume);
    }

    public bool RadioTestActivate(RadioTestMode testMode)
    {
      return this.MyDeviceList.SelectedDevice.RadioTest(testMode);
    }

    public bool Start512HzRtcCalibration()
    {
      return this.MyDeviceList.SelectedDevice.Start512HzRtcCalibration();
    }

    public bool TestDone(long dispValueId)
    {
      return this.MyDeviceList.SelectedDevice.TestDone(dispValueId);
    }

    public bool ReadMemory(long FullStartAddress, int NumberOfBytes, out ByteField MemoryData)
    {
      MemoryData = DeviceCollectorFunctions.EmptyByteField;
      if (this.MyDeviceList.SelectedDevice is Serie1MBus)
      {
        MemoryData = new ByteField(NumberOfBytes);
        return ((Serie1MBus) this.MyDeviceList.SelectedDevice).ReadMemory(FullStartAddress, NumberOfBytes, ref MemoryData);
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "The selected device does not support this ReadMemory command");
      return false;
    }

    public bool ReadMemory(
      MemoryLocation Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData)
    {
      this.BreakRequest = false;
      if (this.MyBusMode != ZR_ClassLibrary.BusMode.Minol_Device)
        return this.ReadMemory((int) Location, StartAddress, NumberOfBytes, out MemoryData);
      if (this.MyDeviceList.SelectedDevice == null)
        this.MyDeviceList.SelectedDevice = (BusDevice) new MinolDevice(this);
      return ((MinolDevice) this.MyDeviceList.SelectedDevice).ReadMemory(Location, StartAddress, NumberOfBytes, out MemoryData);
    }

    public bool ReadMemory(
      MemoryLocation Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData,
      bool useOnlyLongWakeUpSequence)
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.Minol_Device)
      {
        if (this.MyDeviceList.SelectedDevice == null)
          this.MyDeviceList.SelectedDevice = (BusDevice) new MinolDevice(this);
        if (this.MyDeviceList.SelectedDevice is MinolDevice)
          ((MinolDevice) this.MyDeviceList.SelectedDevice).UseOnlyLongWakeUpSequence = useOnlyLongWakeUpSequence;
      }
      return this.ReadMemory(Location, StartAddress, NumberOfBytes, out MemoryData);
    }

    public bool ReadMemory(
      int Location,
      int StartAddress,
      int NumberOfBytes,
      out ByteField MemoryData)
    {
      this.BreakRequest = false;
      MemoryData = DeviceCollectorFunctions.EmptyByteField;
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
      {
        if (NumberOfBytes >= 65536 || NumberOfBytes < 0)
        {
          int num = (int) MessageBox.Show("ReadMemory: Number off bytes out of range", "DeviceCollector");
          return false;
        }
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).Location = Location;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).StartAddress = StartAddress;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).NumberOfBytes = NumberOfBytes;
        if (!((Serie2MBus) this.MyDeviceList.SelectedDevice).ReadMemory())
          return false;
        MemoryData = ((Serie2MBus) this.MyDeviceList.SelectedDevice).DataBuffer;
        return true;
      }
      if (this.MyDeviceList.SelectedDevice is EHCA_MBus)
      {
        ((EHCA_MBus) this.MyDeviceList.SelectedDevice).StartAddress = StartAddress;
        ((EHCA_MBus) this.MyDeviceList.SelectedDevice).NumberOfBytes = NumberOfBytes;
        if (!((EHCA_MBus) this.MyDeviceList.SelectedDevice).ReadMemory())
          return false;
        MemoryData = ((EHCA_MBus) this.MyDeviceList.SelectedDevice).DataBuffer;
        return true;
      }
      if (this.MyDeviceList.SelectedDevice is RDM_Bus)
        return ((RDM_Bus) this.MyDeviceList.SelectedDevice).ReadMemory(StartAddress, NumberOfBytes, out MemoryData);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "The selected device does not support this ReadMemory command");
      return false;
    }

    public bool ReadParameterGroup(ParameterGroups TheParameterGroup, out object ParameterData)
    {
      this.BreakRequest = false;
      ParameterData = (object) null;
      return this.MyDeviceList.SelectedDevice != null && this.MyDeviceList.SelectedDevice.ReadParameterGroup(TheParameterGroup, out ParameterData);
    }

    public bool ReadAnswerString(string RequestString, out string AnswerString)
    {
      AnswerString = string.Empty;
      return this.MyDeviceList.SelectedDevice != null && this.MyDeviceList.SelectedDevice.ReadAnswerString(RequestString, out AnswerString);
    }

    public bool WriteParameterGroup(ParameterGroups TheParameterGroup, object ParameterData)
    {
      return this.MyDeviceList.SelectedDevice != null && this.MyDeviceList.SelectedDevice.WriteParameterGroup(TheParameterGroup, ParameterData);
    }

    public bool ResetParameterGroup(ParameterGroups TheParameterGroup)
    {
      return this.MyDeviceList.SelectedDevice != null && this.MyDeviceList.SelectedDevice.ResetParameterGroup(TheParameterGroup);
    }

    private bool OpenWaveFlowRadio()
    {
      if (this.MyWavePort == null)
        this.MyWavePort = new WavePortConnector(this);
      return this.MyWavePort.OpenPort();
    }

    private bool CloseWaveFlowRadio()
    {
      if (this.MyWavePort != null)
        this.MyWavePort.ClosePort();
      return true;
    }

    private bool ReadWaveFlowParameter(out DeviceInfo Info)
    {
      bool flag = ((WaveFlowDevice) this.MyDeviceList.SelectedDevice).ReadParameters();
      Info = new DeviceInfo(this.MyDeviceList.SelectedDevice.Info);
      return flag;
    }

    public string GetWafeFlowSerialNumber()
    {
      try
      {
        return ((WaveFlowDevice) this.MyDeviceList.SelectedDevice).GetSerialNumber();
      }
      catch
      {
        return string.Empty;
      }
    }

    public bool ReadWaveFlowParameterGroup(
      ParameterGroups TheParameterGroup,
      out SortedList ParameterList,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      ParameterList = new SortedList();
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        object ParameterData = new object();
        if (!this.ReadParameterGroup(TheParameterGroup, out ParameterData))
        {
          Fehlerstring = ((WaveFlowDevice) this.MyDeviceList.SelectedDevice).GetLastErrorString();
          return false;
        }
        ParameterList = (SortedList) ParameterData;
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool ReadWaveFlowAnswer(
      string RequestString,
      out string XMLString,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      XMLString = string.Empty;
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        if (this.ReadAnswerString(RequestString, out XMLString))
          return true;
        Fehlerstring = ((WaveFlowDevice) this.MyDeviceList.SelectedDevice).GetLastErrorString();
        return false;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool ReadWavePortFirmware(out string TheFirmwareString, out string Fehlerstring)
    {
      TheFirmwareString = string.Empty;
      Fehlerstring = "Nicht implementiert";
      return false;
    }

    public bool WriteWaveFlowParameterGroup(
      ParameterGroups TheParameterGroup,
      SortedList ParameterList,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        if (this.WriteParameterGroup(TheParameterGroup, (object) ParameterList))
          return true;
        Fehlerstring = ((WaveFlowDevice) this.MyDeviceList.SelectedDevice).GetLastErrorString();
        return false;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool ResetWaveFlowParameterGroup(
      ParameterGroups TheParameterGroup,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        if (this.ResetParameterGroup(TheParameterGroup))
          return true;
        Fehlerstring = ((WaveFlowDevice) this.MyDeviceList.SelectedDevice).GetLastErrorString();
        return false;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool SetWafeFlowRepeaters(string[] SerialNumbers, out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      try
      {
        if (this.MyDeviceList.SelectedDevice != null)
          return this.MyDeviceList.SelectedDevice.SetRepeaters(SerialNumbers, out Fehlerstring);
        Fehlerstring = "No device in bus list";
        return false;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool ActivateWafeFlowRepeaters(out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        this.MyDeviceList.SelectedDevice.ActivateRepeaters();
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool DeactivateWafeFlowRepeaters(out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        this.MyDeviceList.SelectedDevice.DeactivateRepeaters();
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool GetWafeFlowRepeaters(out string[] Repeaters, out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      Repeaters = new string[0];
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        Repeaters = this.MyDeviceList.SelectedDevice.GetRepeaters();
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool GetWafeFlowRepeatersAreActivated(out bool Activated, out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      Activated = false;
      try
      {
        if (this.MyDeviceList.SelectedDevice == null)
        {
          Fehlerstring = "No device in bus list";
          return false;
        }
        Activated = this.MyDeviceList.SelectedDevice.GetRepeatersAreActivated();
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public bool WriteMemory(MemoryLocation Location, int StartAddress, ByteField data)
    {
      if (this.MyBusMode != ZR_ClassLibrary.BusMode.Minol_Device)
        return this.WriteMemory((int) Location, StartAddress, data);
      if (this.MyDeviceList.SelectedDevice == null || !(this.MyDeviceList.SelectedDevice is MinolDevice))
        this.MyDeviceList.SelectedDevice = (BusDevice) new MinolDevice(this);
      return ((MinolDevice) this.MyDeviceList.SelectedDevice).WriteMemory(Location, StartAddress, data);
    }

    public bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      return this.MyDeviceList.SelectedDevice.EraseFlash(StartAddress, NumberOfBytes);
    }

    public bool UpdateMemory(
      MemoryLocation Location,
      int StartAddress,
      ByteField OldData,
      ByteField NewData)
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
      {
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).Location = (int) Location;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).StartAddress = StartAddress;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).OldDataBuffer = OldData;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).DataBuffer = NewData;
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).UpdateMemory();
      }
      int num = (int) MessageBox.Show("The selected device does not support this UpdateMemory command");
      return false;
    }

    public bool WriteMemory(int Location, int StartAddress, ByteField data)
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
      {
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).Location = Location;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).StartAddress = StartAddress;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).DataBuffer = data;
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).WriteMemory();
      }
      if (this.MyDeviceList.SelectedDevice is EHCA_MBus)
      {
        ((EHCA_MBus) this.MyDeviceList.SelectedDevice).StartAddress = StartAddress;
        ((EHCA_MBus) this.MyDeviceList.SelectedDevice).DataBuffer = data;
        return ((EHCA_MBus) this.MyDeviceList.SelectedDevice).WriteMemory();
      }
      if (this.MyDeviceList.SelectedDevice is RDM_Bus)
        return ((RDM_Bus) this.MyDeviceList.SelectedDevice).WriteMemory(StartAddress, ref data);
      int num = (int) MessageBox.Show("The selected device does not support this WriteMemory command");
      return false;
    }

    public bool WriteBitfield(int Address, byte AndMask, byte OrMask)
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
      {
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).StartAddress = Address;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).AndMask = AndMask;
        ((Serie2MBus) this.MyDeviceList.SelectedDevice).OrMask = OrMask;
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).WriteBitfield();
      }
      if (this.MyDeviceList.SelectedDevice is EHCA_MBus)
      {
        ((EHCA_MBus) this.MyDeviceList.SelectedDevice).StartAddress = Address;
        return ((EHCA_MBus) this.MyDeviceList.SelectedDevice).WriteBitfield((uint) AndMask, (uint) OrMask);
      }
      int num = (int) MessageBox.Show("The selected device does not support this WriteBitfield command");
      return false;
    }

    public bool WriteBitfield(int Address, uint AndMask, uint OrMask)
    {
      if (this.MyDeviceList.SelectedDevice is EHCA_MBus)
      {
        ((EHCA_MBus) this.MyDeviceList.SelectedDevice).StartAddress = Address;
        return ((EHCA_MBus) this.MyDeviceList.SelectedDevice).WriteBitfield(AndMask, OrMask);
      }
      int num = (int) MessageBox.Show("The selected device does not support this WriteBitfield command");
      return false;
    }

    public bool WriteBit(long FullAddress, bool BitData)
    {
      if (this.MyDeviceList.SelectedDevice is Serie1MBus)
        return ((Serie1MBus) this.MyDeviceList.SelectedDevice).WriteBit(FullAddress, BitData);
      int num = (int) MessageBox.Show("The selected device does not support this WriteBit command");
      return false;
    }

    public bool WriteNibble(long FullAddress, byte NibbleData)
    {
      if (this.MyDeviceList.SelectedDevice is Serie1MBus)
        return ((Serie1MBus) this.MyDeviceList.SelectedDevice).WriteNibble(FullAddress, NibbleData);
      int num = (int) MessageBox.Show("The selected device does not support this WriteNibble command");
      return false;
    }

    public bool WriteByte(long FullAddress, byte ByteData)
    {
      if (this.MyDeviceList.SelectedDevice is Serie1MBus)
        return ((Serie1MBus) this.MyDeviceList.SelectedDevice).WriteByte(FullAddress, ByteData);
      int num = (int) MessageBox.Show("The selected device does not support this WriteByte command");
      return false;
    }

    public bool SetParameterListDefault()
    {
      if (this.MyDeviceList.SelectedDevice is MBusDevice)
      {
        this.DeviceIsModified = true;
        return ((MBusDevice) this.MyDeviceList.SelectedDevice).MeterApplicationReset();
      }
      int num = (int) MessageBox.Show("The selected device does not support this SetParameterListDefault command");
      return false;
    }

    public bool SetParameterListDefault(int BusListIndex)
    {
      if (this.MyDeviceList.bus[BusListIndex] is Serie1MBus)
      {
        this.DeviceIsModified = true;
        return ((MBusDevice) this.MyDeviceList.bus[BusListIndex]).MeterApplicationReset();
      }
      if (!(this.MyDeviceList.bus[BusListIndex] is Serie2MBus))
        return true;
      this.DeviceIsModified = true;
      return ((MBusDevice) this.MyDeviceList.bus[BusListIndex]).MeterApplicationReset();
    }

    public bool SetParameterListAll()
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
        return ((MBusDevice) this.MyDeviceList.SelectedDevice).SelectAllParameter();
      return this.MyDeviceList.SelectedDevice is Serie1MBus && ((MBusDevice) this.MyDeviceList.SelectedDevice).SelectAllParameter();
    }

    public bool SetParameterListAllParameters(int BusListIndex)
    {
      if (this.MyDeviceList.bus[BusListIndex] is Serie1MBus)
        return ((MBusDevice) this.MyDeviceList.bus[BusListIndex]).SelectAllParameter();
      return !(this.MyDeviceList.bus[BusListIndex] is Serie2MBus) || ((MBusDevice) this.MyDeviceList.bus[BusListIndex]).SelectAllParameter();
    }

    public bool WriteDueDateMonth(ushort month)
    {
      return this.MyDeviceList.SelectedDevice.WriteDueDateMonth(month);
    }

    public bool SelectParameterList(int ListNumber, int function)
    {
      return this.MyDeviceList.SelectedDevice.SelectParameterList(ListNumber, function);
    }

    public bool SelectParameterList(int BusListIndex, int ParameterListNumber, int function)
    {
      return ((BusDevice) this.MyDeviceList.bus[BusListIndex]).SelectParameterList(ParameterListNumber, function);
    }

    public ParameterListInfo ReadParameterList()
    {
      return this.MyDeviceList.SelectedDevice.ReadParameterList();
    }

    public void ClearCounters()
    {
    }

    public int GetJobCounter() => this.BusState.TotalJobCounter;

    public int GetErrorCounter() => this.BusState.TotalErrorCounter;

    public int GetTransmitBlockCounter() => this.BusState.TotalTransmitBlockCounter;

    public int GetReceiveBlockCounter() => this.BusState.TotalReceiveBlockCounter;

    internal void MemoryWriteWatch(int address, ref byte[] data)
    {
      string text = "";
      for (int index = 0; index < data.Length; ++index)
      {
        if ((index & 15) == 0)
          text = text + Environment.NewLine + (address + index).ToString("x04") + ":";
        text = text + " " + data[index].ToString("x02");
      }
      int num = (int) MessageBox.Show(text, "DeviceCollector memory watch");
    }

    public bool StartMeterMonitor(int SampleTime)
    {
      if (this.MyDeviceList.SelectedDevice is Serie2MBus)
        return ((Serie2MBus) this.MyDeviceList.SelectedDevice).StartMeterMonitor(SampleTime);
      int num = (int) MessageBox.Show("The selected device does not support this StartMeterMonitor command");
      return false;
    }

    public bool GetMeterMonitorData(out ByteField MonitorData)
    {
      MonitorData = (ByteField) null;
      return this.MyDeviceList.SelectedDevice != null && this.MyDeviceList.SelectedDevice.GetMeterMonitorData(out MonitorData);
    }

    public void StartTestloopReadEEProm()
    {
      if (!(this.MyDeviceList.SelectedDevice is Serie2MBus))
        return;
      this.BreakRequest = false;
      if (!this.SetRunningFunction(DeviceCollectorFunctions.Functions.TestloopReadEEProm))
        return;
      this.BusState.StartGlobalFunctionTask(BusStatusClass.GlobalFunctionTasks.TestloopReadEEProm);
      this.BusThread = ThreadStarter.CreateThread(new RuntimeThread.Start(new RuntimeThread().TestloopReadEEProm), this);
      this.BusThread.Name = "TestloopReadEEProm";
      this.BusThread.Start();
    }

    public void StartTestloopWriteReadEEProm()
    {
      if (!(this.MyDeviceList.SelectedDevice is Serie2MBus))
        return;
      this.BreakRequest = false;
      if (!this.SetRunningFunction(DeviceCollectorFunctions.Functions.TestloopWriteReadEEProm))
        return;
      this.BusState.StartGlobalFunctionTask(BusStatusClass.GlobalFunctionTasks.TestloopWriteReadEEProm);
      this.BusThread = ThreadStarter.CreateThread(new RuntimeThread.Start(new RuntimeThread().TestloopWriteReadEEProm), this);
      this.BusThread.Name = "TestloopWriteReadEEProm";
      this.BusThread.Start();
    }

    public void StopTestLoop()
    {
      if (!(this.MyDeviceList.SelectedDevice is Serie2MBus))
        return;
      this.BreakRequest = true;
      while (!this.BusThread.Join(100))
      {
        Thread.Sleep(20);
        Application.DoEvents();
      }
      this.EndRunningFunction();
      this.BreakRequest = false;
    }

    public bool StartReceiver() => this.MyReceiver.StartReceiver();

    public bool StopReceiver() => this.MyReceiver.StopReceiver();

    public bool SystemInit()
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).SystemInit();
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public bool StartHKVEReceptionWindow()
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).StartHKVEReceptionWindow();
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public bool SetMinomatV2Configuration(MinomatV2.Configuration configuration)
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).SetConfiguration(configuration);
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public MinomatV2.Configuration GetMinomatV2Configuration()
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).GetConfiguration();
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public MinomatV2.SystemStatus GetMinomatV2SystemStatus()
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).GetSystemStatus();
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public List<string> GetRegisteredHKVE()
    {
      if (this.MyBusMode != ZR_ClassLibrary.BusMode.MinomatV2)
        throw new NotImplementedException("Only valid for MinomatV2");
      List<MinomatDevice> devices = new List<MinomatDevice>();
      List<string> registeredHkve = new List<string>();
      if (((MinomatList) this.MyDeviceList).GetAllRegisteredDevices(out devices, (byte) 0, (byte) 100))
      {
        foreach (MinomatDevice minomatDevice in devices)
          registeredHkve.Add(minomatDevice.Info.MeterNumber);
      }
      return registeredHkve;
    }

    public List<string> GetUnregisteredHKVE()
    {
      if (this.MyBusMode != ZR_ClassLibrary.BusMode.MinomatV2)
        throw new NotImplementedException("Only valid for MinomatV2");
      List<MinomatDevice> devices = new List<MinomatDevice>();
      List<string> unregisteredHkve = new List<string>();
      if (((MinomatList) this.MyDeviceList).GetAllRegisteredDevices(out devices, (byte) 100, (byte) 200))
      {
        foreach (MinomatDevice minomatDevice in devices)
          unregisteredHkve.Add(minomatDevice.Info.MeterNumber);
      }
      return unregisteredHkve;
    }

    public bool RegisterHKVE(List<string> devices)
    {
      if (this.MyBusMode != ZR_ClassLibrary.BusMode.MinomatV2)
        throw new NotImplementedException("Only valid for MinomatV2");
      if (devices == null)
        return false;
      List<MinomatDevice> deviceList = new List<MinomatDevice>();
      foreach (string device in devices)
      {
        MinomatDevice minomatDevice = new MinomatDevice(this);
        minomatDevice.Info.MeterNumber = device;
        deviceList.Add(minomatDevice);
      }
      return this.RegisterHKVE(deviceList);
    }

    public bool RegisterHKVE(MinomatDevice device)
    {
      if (this.MyBusMode != ZR_ClassLibrary.BusMode.MinomatV2)
        throw new NotImplementedException("Only valid for MinomatV2");
      return this.RegisterHKVE(new List<MinomatDevice>()
      {
        device
      });
    }

    public bool RegisterHKVE(List<MinomatDevice> deviceList)
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).RegisterHKVE(deviceList);
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public bool DeregisterHKVE(List<string> devices)
    {
      if (this.MyBusMode != ZR_ClassLibrary.BusMode.MinomatV2)
        throw new NotImplementedException("Only valid for MinomatV2");
      if (devices == null)
        return false;
      List<MinomatDevice> deviceList = new List<MinomatDevice>();
      foreach (string device in devices)
      {
        MinomatDevice minomatDevice = new MinomatDevice(this);
        minomatDevice.Info.MeterNumber = device;
        deviceList.Add(minomatDevice);
      }
      return this.DeRegisterHKVE(deviceList);
    }

    public bool DeRegisterHKVE(List<MinomatDevice> deviceList)
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).DeRegisterHKVE(deviceList);
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public bool PingSelectedDevice()
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).ConnectToMinomat();
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public bool SetTime(DateTime dateTime)
    {
      if (this.MyBusMode == ZR_ClassLibrary.BusMode.MinomatV2)
        return ((MinomatList) this.MyDeviceList).SetMinomatRTC(dateTime);
      throw new NotImplementedException("Only valid for MinomatV2");
    }

    public bool ReadDeviceList()
    {
      this.MyDeviceList = this.GetDeviceListForBusMode();
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.GetDeviceCollectorSettings();
      switch (this.MyBusMode)
      {
        case ZR_ClassLibrary.BusMode.MBusPointToPoint:
        case ZR_ClassLibrary.BusMode.MBus:
        case ZR_ClassLibrary.BusMode.WaveFlowRadio:
        case ZR_ClassLibrary.BusMode.MinomatRadioTest:
        case ZR_ClassLibrary.BusMode.Minol_Device:
        case ZR_ClassLibrary.BusMode.RelayDevice:
        case ZR_ClassLibrary.BusMode.Radio2:
        case ZR_ClassLibrary.BusMode.Radio3:
        case ZR_ClassLibrary.BusMode.Radio4:
        case ZR_ClassLibrary.BusMode.wMBusS1:
        case ZR_ClassLibrary.BusMode.wMBusS1M:
        case ZR_ClassLibrary.BusMode.wMBusS2:
        case ZR_ClassLibrary.BusMode.wMBusT1:
        case ZR_ClassLibrary.BusMode.wMBusT2_meter:
        case ZR_ClassLibrary.BusMode.wMBusT2_other:
        case ZR_ClassLibrary.BusMode.wMBusC1A:
        case ZR_ClassLibrary.BusMode.wMBusC1B:
        case ZR_ClassLibrary.BusMode.Radio3_868_95_RUSSIA:
        case ZR_ClassLibrary.BusMode.RadioMS:
          return false;
        case ZR_ClassLibrary.BusMode.MinomatV2:
          this.MyDeviceList.DeleteBusList();
          MinomatDevice NewDevice1 = new MinomatDevice(this);
          NewDevice1.Info.MeterNumber = collectorSettings[DeviceCollectorSettings.DaKonId].ToString();
          NewDevice1.DeviceType = DeviceTypes.MinomatDevice;
          NewDevice1.Info.DeviceType = NewDevice1.DeviceType;
          NewDevice1.Info.A_Field = byte.MaxValue;
          NewDevice1.PrimaryAddressOk = false;
          this.MyDeviceList.AddDevice((object) NewDevice1, true);
          this.SetSelectedDeviceBySerialNumber(NewDevice1.Info.MeterNumber);
          List<MinomatDevice> devices = new List<MinomatDevice>();
          if (!((MinomatList) this.MyDeviceList).GetAllRegisteredDevices(out devices, (byte) 0, (byte) 200) || devices == null)
            return false;
          foreach (object NewDevice2 in devices)
            this.MyDeviceList.AddDevice(NewDevice2, false);
          return true;
        default:
          throw new NotImplementedException(this.MyBusMode.ToString());
      }
    }

    public bool DeleteSelectedDevice() => this.MyDeviceList.DeleteSelectedDevice();

    public Dictionary<string, string> LoadAvailableCOMservers()
    {
      return this.MyCom.LoadAvailableCOMservers();
    }

    public object ShowWindow(object parameters)
    {
      if (parameters != null)
      {
        if (parameters is string)
          this.SetDeviceCollectorSettings(parameters.ToString());
        else if (parameters is SortedList<DeviceCollectorSettings, object>)
          this.SetDeviceCollectorSettings(parameters as SortedList<DeviceCollectorSettings, object>);
      }
      this.ShowBusWindow();
      return (object) this.GetDeviceCollectorSettings();
    }

    public void SetReadoutConfiguration(ConfigList configList)
    {
      if (this.ReadoutConfigByBusFile)
      {
        this.DisableConfigList();
      }
      else
      {
        if (configList == null)
          throw new ArgumentNullException(nameof (configList));
        if (this.ConfigList == null)
        {
          this.ConfigList = configList;
          this.ConfigList.PropertyChanged += new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
          this.ConfigList.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ConfigList_CollectionChanged);
        }
        else if (this.ConfigList != configList)
          throw new ArgumentException("this.configList != configList");
        this.SetDeviceCollectorSettings(configList.GetSortedList());
      }
    }

    private void DisableConfigList()
    {
      if (this.ConfigList == null)
        return;
      this.ConfigList.PropertyChanged -= new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
      this.ConfigList.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ConfigList_CollectionChanged);
      this.ConfigList = (ConfigList) null;
    }

    private void ConfigList_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.configValuesChanged();
    }

    private void ConfigList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.configValuesChanged();
    }

    private void configValuesChanged()
    {
      SortedList<string, string> collectorSettingsList = this.GetDeviceCollectorSettingsList();
      SortedList<string, string> settings = new SortedList<string, string>();
      foreach (KeyValuePair<string, string> sorted in this.ConfigList.GetSortedList())
      {
        if (!collectorSettingsList.ContainsKey(sorted.Key) || !(collectorSettingsList[sorted.Key] == sorted.Value))
          settings.Add(sorted.Key, sorted.Value);
      }
      if (settings.Count <= 0)
        return;
      this.SetDeviceCollectorSettings(settings);
    }

    public ConfigList GetReadoutConfiguration() => this.ConfigList;

    public enum Initialise
    {
      BusInfo,
      AsyncCom,
      Both,
    }

    internal enum Functions
    {
      NoFunction,
      ReqUD2,
      ReadVersion,
      ReadMemory,
      WriteMemory,
      ReadParameter,
      DeviceReset,
      TestloopReadEEProm,
      TestloopWriteReadEEProm,
      DeleteMeterKey,
      WriteBitfield,
      RunBackup,
      SetBaudrate,
      SetPrimaryAddress,
      SearchSingleDeviceByPrimaryAddress,
      SearchSingleDeviceBySerialNumber,
      ConnectingToMinomat,
    }
  }
}
