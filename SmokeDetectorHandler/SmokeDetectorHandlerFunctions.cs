// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.SmokeDetectorHandlerFunctions
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using DeviceCollector;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class SmokeDetectorHandlerFunctions : HandlerFunctionsForProduction
  {
    private static Logger logger = LogManager.GetLogger(nameof (SmokeDetectorHandlerFunctions));
    internal IDeviceCollector MyDeviceCollector;
    private bool useWakeUpByEachAccess;
    private bool isRunning;
    private SmokeDetectorWindow MyWindow;

    public DeviceCommandsMBus DeviceCommands { get; private set; }

    public SpecialCommands Special { get; private set; }

    public CommonLoRaCommands LoRa { get; private set; }

    public CommonMBusCommands MBus { get; private set; }

    public CommonRadioCommands Radio { get; private set; }

    public Common32BitCommands Device { get; private set; }

    internal AsyncFunctionsEx Port { get; private set; }

    public MinoprotectIII WorkMeter { get; set; }

    internal MinoprotectIII ConnectedMeter { get; set; }

    internal MinoprotectIII BackupMeter { get; set; }

    public MinoprotectII ConnectedMeterMinoprotectII { get; private set; }

    [Obsolete]
    public SmokeDetectorHandlerFunctions()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      this.MyDeviceCollector = ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector] as IDeviceCollector;
      this.useWakeUpByEachAccess = false;
      this.isRunning = false;
      this.BasicHandlerFunctions(this.MyDeviceCollector);
    }

    public SmokeDetectorHandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.MyDeviceCollector = deviceCollector;
      this.useWakeUpByEachAccess = false;
      this.isRunning = false;
      this.BasicHandlerFunctions(deviceCollector);
    }

    private void BasicHandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.Port = new AsyncFunctionsEx(deviceCollector);
      this.DeviceCommands = new DeviceCommandsMBus((IPort) this.Port);
      this.Device = new Common32BitCommands(this.DeviceCommands);
      this.MBus = new CommonMBusCommands(this.Device);
      this.Radio = new CommonRadioCommands(this.Device);
      this.LoRa = new CommonLoRaCommands(this.Device);
      this.Special = new SpecialCommands(this.Device);
    }

    public void GMM_Dispose()
    {
    }

    public event ValueEventHandler<int> OnProgress;

    public event EventHandler<ValueIdentSet> ValueIdentSetReceived;

    public event EventHandlerEx<SmokeDetector.SmokeDensityAndSensitivity> OnSmokeDensityAndSensitivityValueReceived;

    public string ShowSmokeDetectorWindow()
    {
      using (this.MyWindow = new SmokeDetectorWindow(this))
      {
        int num = (int) this.MyWindow.ShowDialog();
        return this.MyWindow.NextComponentName;
      }
    }

    internal ToolStripItem[] GetComponentMenuItems()
    {
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem();
      toolStripMenuItem1.Name = "ComponentMenuItemGMM";
      toolStripMenuItem1.Size = new Size(173, 22);
      toolStripMenuItem1.Text = "GlobalMeterManager";
      toolStripMenuItem1.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem1.Tag = (object) GMM_Components.GMM.ToString();
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem();
      toolStripMenuItem2.Name = "ComponentMenuItemBack";
      toolStripMenuItem2.Size = new Size(173, 22);
      toolStripMenuItem2.Text = "Back";
      toolStripMenuItem2.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem2.Tag = (object) "";
      ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem();
      toolStripMenuItem3.Name = "ComponentMenuItemQuit";
      toolStripMenuItem3.Size = new Size(173, 22);
      toolStripMenuItem3.Text = "Quit";
      toolStripMenuItem3.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem3.Tag = (object) "Exit";
      ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
      ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem();
      toolStripMenuItem4.Name = "ComponentMenuItemConfigurator";
      toolStripMenuItem4.Size = new Size(173, 22);
      toolStripMenuItem4.Text = GMM_Components.Configurator.ToString();
      toolStripMenuItem4.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem4.Tag = (object) GMM_Components.Configurator.ToString();
      ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
      ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem();
      toolStripMenuItem5.Name = "ComponentMenuItemDeviceCollector";
      toolStripMenuItem5.Size = new Size(173, 22);
      toolStripMenuItem5.Text = GMM_Components.DeviceCollector.ToString();
      toolStripMenuItem5.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem5.Tag = (object) GMM_Components.DeviceCollector.ToString();
      ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem();
      toolStripMenuItem6.Name = "ComponentMenuItemAsyncCom";
      toolStripMenuItem6.Size = new Size(173, 22);
      toolStripMenuItem6.Text = GMM_Components.AsyncCom.ToString();
      toolStripMenuItem6.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem6.Tag = (object) GMM_Components.AsyncCom.ToString();
      return new ToolStripItem[8]
      {
        (ToolStripItem) toolStripMenuItem1,
        (ToolStripItem) toolStripMenuItem2,
        (ToolStripItem) toolStripMenuItem3,
        (ToolStripItem) toolStripSeparator1,
        (ToolStripItem) toolStripMenuItem4,
        (ToolStripItem) toolStripSeparator2,
        (ToolStripItem) toolStripMenuItem5,
        (ToolStripItem) toolStripMenuItem6
      };
    }

    private void MenuItemSelectComponent_Click(object sender, EventArgs e)
    {
      this.MyWindow.NextComponentName = (sender as ToolStripMenuItem).Tag.ToString();
      this.MyWindow.Close();
    }

    public void ClearAllData()
    {
      this.MyDeviceCollector.BreakRequest = false;
      this.ConnectedMeter = (MinoprotectIII) null;
      this.WorkMeter = (MinoprotectIII) null;
      this.BackupMeter = (MinoprotectIII) null;
      this.ConnectedMeterMinoprotectII = (MinoprotectII) null;
      SmokeDetectorHandlerFunctions.logger.Debug("Clear all values in handler.");
    }

    public bool ReadDevice(bool readLoggerToo)
    {
      return readLoggerToo ? this.ReadDevice(ReadPart.LoggerEvents) : this.ReadDevice();
    }

    public bool ReadDevice() => this.ReadDevice(ReadPart.LoRa);

    public bool ReadDevice(ReadPart parts)
    {
      this.ClearAllData();
      if (this.OnProgress != null)
        this.OnProgress((object) this, 20);
      byte[] buffer = this.ReadParameterBlock();
      if (buffer == null)
        return false;
      bool flag1 = buffer.Length == 29;
      bool flag2 = buffer.Length == 44;
      this.useWakeUpByEachAccess = flag1;
      if (this.OnProgress != null)
        this.OnProgress((object) this, 40);
      SmokeDetectorVersion smokeDetectorVersion = this.ReadVersion();
      if (smokeDetectorVersion == null)
        return false;
      if (flag2)
      {
        TestModeParameter testModeParameter = (TestModeParameter) null;
        if ((parts & ReadPart.TestModeParameter) == ReadPart.TestModeParameter)
        {
          if (this.OnProgress != null)
            this.OnProgress((object) this, 50);
          testModeParameter = this.ReadTestModeParameter();
          if (testModeParameter == null)
            return false;
        }
        ManufacturingParameter manufacturingParameter = (ManufacturingParameter) null;
        if ((parts & ReadPart.ManufacturingParameter) == ReadPart.ManufacturingParameter)
        {
          if (this.OnProgress != null)
            this.OnProgress((object) this, 52);
          manufacturingParameter = this.ReadManufacturingParameter();
          if (manufacturingParameter == null)
            return false;
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 53);
        LoRaParameter loRaParameter = (LoRaParameter) null;
        if ((parts & ReadPart.LoRa) == ReadPart.LoRa && smokeDetectorVersion.DeviceIdentity == (ushort) 28)
          loRaParameter = this.ReadLoRaParameter();
        List<MinoprotectIII_Events> minoprotectIiiEventsList = new List<MinoprotectIII_Events>();
        if ((parts & ReadPart.LoggerEvents) == ReadPart.LoggerEvents)
        {
          int e = 55;
          for (ushort address = 17; address <= (ushort) 25; ++address)
          {
            if (this.OnProgress != null)
              this.OnProgress((object) this, e);
            List<MinoprotectIII_Events> collection = this.ReadEventMemory(address);
            if (collection != null)
              minoprotectIiiEventsList.AddRange((IEnumerable<MinoprotectIII_Events>) collection);
            e += 5;
          }
        }
        MinoprotectIII minoprotectIii = new MinoprotectIII();
        minoprotectIii.Parameter = MinoprotectIII_Parameter.Parse(buffer);
        minoprotectIii.Version = smokeDetectorVersion;
        minoprotectIii.TestModeParameter = testModeParameter;
        minoprotectIii.ManufacturingParameter = manufacturingParameter;
        minoprotectIii.EventMemory = minoprotectIiiEventsList;
        minoprotectIii.LoRaParameter = loRaParameter;
        this.ConnectedMeter = minoprotectIii;
        this.WorkMeter = minoprotectIii.DeepCopy();
      }
      else
      {
        if (!flag1)
          return false;
        MinoprotectII_Events minoprotectIiEvents = (MinoprotectII_Events) null;
        if ((parts & ReadPart.LoggerEvents) == ReadPart.LoggerEvents)
        {
          if (this.OnProgress != null)
            this.OnProgress((object) this, 80);
          minoprotectIiEvents = this.ReadEventMemoryMinoprotectII();
          if (minoprotectIiEvents == null)
            return false;
        }
        this.ConnectedMeterMinoprotectII = new MinoprotectII()
        {
          Parameter = MinoprotectII_Parameter.Parse(buffer),
          Version = smokeDetectorVersion,
          Events = minoprotectIiEvents
        };
      }
      if (this.OnProgress != null)
        this.OnProgress((object) this, 100);
      ZR_ClassLibMessages.ClearErrors();
      if (this.ValueIdentSetReceived != null)
        this.ValueIdentSetReceived((object) this, new ValueIdentSet()
        {
          Manufacturer = "MINOL",
          Version = this.WorkMeter.Version.VersionString,
          SerialNumber = this.WorkMeter.Version.Serialnumber.ToString(),
          DeviceType = string.Format("{0} Minoprotect 3 v{1}", (object) this.WorkMeter.Version.Manufacturer, (object) this.WorkMeter.Version.VersionString),
          AvailableValues = this.GetValues((List<long>) null)
        });
      return true;
    }

    public bool WriteDevice()
    {
      this.MyDeviceCollector.BreakRequest = false;
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter", "Can not write data to device! Missing data.");
      if (this.ConnectedMeter == null)
        throw new ArgumentNullException("ConnectedMeter", "Can not write data to device! Missing data.");
      try
      {
        if (this.OnProgress != null)
          this.OnProgress((object) this, 40);
        byte[] writeBuffer1_1 = this.WorkMeter.Version.CreateWriteBuffer1();
        byte[] writeBuffer1_2 = this.ConnectedMeter.Version.CreateWriteBuffer1();
        if (!Util.ArraysEqual(writeBuffer1_1, writeBuffer1_2) && !this.MyDeviceCollector.SmokeDetectorHandler.WriteDevice(4, writeBuffer1_1))
          return false;
        if (this.OnProgress != null)
          this.OnProgress((object) this, 50);
        byte[] writeBuffer2_1 = this.WorkMeter.Version.CreateWriteBuffer2();
        byte[] writeBuffer2_2 = this.ConnectedMeter.Version.CreateWriteBuffer2();
        if (!Util.ArraysEqual(writeBuffer2_1, writeBuffer2_2) && !this.MyDeviceCollector.SmokeDetectorHandler.WriteDevice(0, writeBuffer2_1))
          return false;
        if (this.OnProgress != null)
          this.OnProgress((object) this, 60);
        if (this.WorkMeter.ManufacturingParameter != null && this.ConnectedMeter.ManufacturingParameter != null)
        {
          byte[] writeBuffer1 = this.WorkMeter.ManufacturingParameter.CreateWriteBuffer();
          byte[] writeBuffer2 = this.ConnectedMeter.ManufacturingParameter.CreateWriteBuffer();
          if (!Util.ArraysEqual(writeBuffer1, writeBuffer2) && !this.MyDeviceCollector.SmokeDetectorHandler.WriteDevice(3, writeBuffer1))
            return false;
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 80);
        byte[] writeBuffer3 = this.WorkMeter.Parameter.CreateWriteBuffer();
        byte[] writeBuffer4 = this.ConnectedMeter.Parameter.CreateWriteBuffer();
        if (!Util.ArraysEqual(writeBuffer3, writeBuffer4) && !this.MyDeviceCollector.SmokeDetectorHandler.WriteDevice(1, writeBuffer3))
          return false;
        if (this.OnProgress != null)
          this.OnProgress((object) this, 90);
        if (this.WorkMeter.TestModeParameter != null && this.ConnectedMeter.TestModeParameter != null)
        {
          byte[] writeBuffer5 = this.WorkMeter.TestModeParameter.CreateWriteBuffer();
          byte[] writeBuffer6 = this.ConnectedMeter.TestModeParameter.CreateWriteBuffer();
          if (!Util.ArraysEqual(writeBuffer5, writeBuffer6) && !this.MyDeviceCollector.SmokeDetectorHandler.WriteDevice(2, writeBuffer5))
            return false;
        }
        if (this.WorkMeter.LoRaParameter != null && this.ConnectedMeter.LoRaParameter != null && this.WorkMeter.LoRaParameter.Activation != this.ConnectedMeter.LoRaParameter.Activation)
        {
          try
          {
            AsyncHelpers.RunSync((Func<Task>) (async () =>
            {
              await this.LoRa.SetOTAA_ABPAsync(this.WorkMeter.LoRaParameter.Activation, (ProgressHandler) null, CancellationToken.None);
              await this.Device.BackupDeviceAsync((ProgressHandler) null, CancellationToken.None);
            }));
          }
          catch (Exception ex)
          {
            if (ex is AggregateException && ex.InnerException != null)
              throw ex.InnerException;
          }
        }
        if (this.WorkMeter.LoRaParameter != null && this.ConnectedMeter.LoRaParameter != null && (int) this.WorkMeter.LoRaParameter.TransmissionScenario != (int) this.ConnectedMeter.LoRaParameter.TransmissionScenario)
        {
          try
          {
            AsyncHelpers.RunSync((Func<Task>) (async () =>
            {
              await this.LoRa.SetTransmissionScenarioAsync(new byte[1]
              {
                this.WorkMeter.LoRaParameter.TransmissionScenario
              }, (ProgressHandler) null, CancellationToken.None);
              await this.Device.BackupDeviceAsync((ProgressHandler) null, CancellationToken.None);
            }));
          }
          catch (Exception ex)
          {
            if (ex is AggregateException && ex.InnerException != null)
              throw ex.InnerException;
          }
        }
        int? communicationScenario1;
        int num;
        if (this.WorkMeter.LoRaParameter != null && this.ConnectedMeter.LoRaParameter != null)
        {
          int? communicationScenario2 = this.WorkMeter.LoRaParameter.CommunicationScenario;
          if (communicationScenario2.HasValue)
          {
            communicationScenario2 = this.WorkMeter.LoRaParameter.CommunicationScenario;
            communicationScenario1 = this.ConnectedMeter.LoRaParameter.CommunicationScenario;
            num = !(communicationScenario2.GetValueOrDefault() == communicationScenario1.GetValueOrDefault() & communicationScenario2.HasValue == communicationScenario1.HasValue) ? 1 : 0;
            goto label_40;
          }
        }
        num = 0;
label_40:
        if (num != 0)
        {
          communicationScenario1 = this.WorkMeter.LoRaParameter.CommunicationScenario;
          int sc = communicationScenario1.Value;
          try
          {
            AsyncHelpers.RunSync((Func<Task>) (async () =>
            {
              byte[] scenario = BitConverter.GetBytes((ushort) sc);
              await this.Device.SetCommunicationScenarioAsync(scenario, (ProgressHandler) null, CancellationToken.None);
              scenario = (byte[]) null;
            }));
          }
          catch (Exception ex)
          {
            if (ex is AggregateException && ex.InnerException != null)
              throw ex.InnerException;
          }
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 100);
        return true;
      }
      finally
      {
        this.ConnectedMeter = (MinoprotectIII) null;
        this.WorkMeter = (MinoprotectIII) null;
      }
    }

    public bool OpenDevice(uint meterId, DateTime timePoint)
    {
      MeterData meterData = SmokeDetectorDatabase.LoadMeterData(meterId, timePoint);
      return meterData != null && this.OpenDevice(meterData);
    }

    public bool OpenDevice(MeterData meterData)
    {
      if (meterData == null)
        return false;
      this.BackupMeter = MinoprotectIII.Unzip(meterData.Buffer);
      this.WorkMeter = this.BackupMeter.DeepCopy();
      return true;
    }

    public override bool LoadLastBackup(int meterID)
    {
      BaseTables.MeterDataRow meterDataRow = GmmDbLib.MeterData.LoadLastBackup(DbBasis.PrimaryDB.BaseDbConnection, meterID);
      if (meterDataRow == null)
        return false;
      this.BackupMeter = MinoprotectIII.Unzip(meterDataRow.PValueBinary);
      this.WorkMeter = this.BackupMeter.DeepCopy();
      return true;
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(List<long> filter)
    {
      if (this.ConnectedMeterMinoprotectII != null)
        return this.ConnectedMeterMinoprotectII.GetValues(filter);
      return this.WorkMeter != null ? this.WorkMeter.GetValues(filter) : (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
    }

    internal static long GetValueIdentCurrentState()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.BitCompression, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    internal static long GetValueIdentMonthlyState()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.Month, ValueIdent.ValueIdPart_Creation.BitCompression, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    internal static long GetValueIdentDailyState()
    {
      return ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber, ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.Day, ValueIdent.ValueIdPart_Creation.BitCompression, (object) ValueIdent.ValueIdPart_Index.Any);
    }

    internal static long GetValueIdentManipulation()
    {
      return ValueIdent.GetValueIdentOfError(ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdentError.Manipulation);
    }

    internal static long GetValueIdentDeviceError()
    {
      return ValueIdent.GetValueIdentOfError(ValueIdent.ValueIdPart_MeterType.SmokeDetector, ValueIdent.ValueIdentError.DeviceError);
    }

    public bool SaveDevice(byte prefixSerialnumberFull)
    {
      return this.SaveDevice(prefixSerialnumberFull, new uint?());
    }

    public bool SaveDevice(byte prefixSerialnumberFull, uint? productionOrderNumber)
    {
      if (this.WorkMeter == null)
        throw new ArgumentNullException("meter", "Can not save the data in to database!");
      if (this.WorkMeter.Version == null)
        throw new ArgumentNullException("meter.Version", "Can not save the data in to database!");
      if (this.WorkMeter.ManufacturingParameter == null)
        throw new ArgumentNullException("meter.ManufacturingParameter", "Can not save the data in to database!");
      if (this.WorkMeter.ManufacturingParameter.MeterID == 0U || this.WorkMeter.ManufacturingParameter.MeterID == uint.MaxValue)
        throw new ArgumentException("The 'MeterID' is invalid! Value: " + this.WorkMeter.ManufacturingParameter.MeterID.ToString());
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      ZR_ClassLibrary.MeterInfo meterInfo = MeterDatabase.GetMeterInfo(this.WorkMeter.ManufacturingParameter.MeterInfoID);
      if (meterInfo != null && meterInfo.MeterHardwareID != 101)
        throw new ArgumentException("Can not update or add data in to table 'Meter'! The MeterInfoID is not the smoke detector hardware! DB: " + meterInfo?.ToString());
      string serialNr = string.Format("{0:00}{1:00000000}", (object) prefixSerialnumberFull.ToString("X2"), (object) this.WorkMeter.Version.Serialnumber);
      uint hardwareTypeId = (uint) this.WorkMeter.Version.HardwareTypeID;
      int meterInfoId = (int) this.WorkMeter.ManufacturingParameter.MeterInfoID;
      int meterId = (int) this.WorkMeter.ManufacturingParameter.MeterID;
      string orderNr = productionOrderNumber.HasValue ? productionOrderNumber.Value.ToString() : string.Empty;
      byte[] deviceMemory = this.WorkMeter.Zip();
      GmmDbLib.Device.Save(baseDbConnection, meterId, meterInfoId, hardwareTypeId, serialNr, orderNr, deviceMemory, true);
      this.BackupMeter = this.WorkMeter.DeepCopy();
      return true;
    }

    public bool SaveDevice(string printedSerialNumber, uint? productionOrderNumber)
    {
      if (this.WorkMeter == null)
        throw new ArgumentNullException("meter", "Can not save the data in to database!");
      if (this.WorkMeter.Version == null)
        throw new ArgumentNullException("meter.Version", "Can not save the data in to database!");
      if (this.WorkMeter.ManufacturingParameter == null)
        throw new ArgumentNullException("meter.ManufacturingParameter", "Can not save the data in to database!");
      if (this.WorkMeter.ManufacturingParameter.MeterID == 0U || this.WorkMeter.ManufacturingParameter.MeterID == uint.MaxValue)
        throw new ArgumentException("The 'MeterID' is invalid! Value: " + this.WorkMeter.ManufacturingParameter.MeterID.ToString());
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      ZR_ClassLibrary.MeterInfo meterInfo = MeterDatabase.GetMeterInfo(this.WorkMeter.ManufacturingParameter.MeterInfoID);
      if (meterInfo != null && meterInfo.MeterHardwareID != 101)
        throw new ArgumentException("Can not update or add data in to table 'Meter'! The MeterInfoID is not the smoke detector hardware! DB: " + meterInfo?.ToString());
      uint hardwareTypeId = (uint) this.WorkMeter.Version.HardwareTypeID;
      int meterInfoId = (int) this.WorkMeter.ManufacturingParameter.MeterInfoID;
      int meterId = (int) this.WorkMeter.ManufacturingParameter.MeterID;
      string orderNr = productionOrderNumber.HasValue ? productionOrderNumber.Value.ToString() : string.Empty;
      byte[] deviceMemory = this.WorkMeter.Zip();
      GmmDbLib.Device.Save(baseDbConnection, meterId, meterInfoId, hardwareTypeId, printedSerialNumber, orderNr, deviceMemory, true);
      this.BackupMeter = this.WorkMeter.DeepCopy();
      return true;
    }

    public SmokeDetectorVersion ReadVersion()
    {
      this.MyDeviceCollector.BreakRequest = false;
      if (this.useWakeUpByEachAccess)
        this.MyDeviceCollector.AsyncCom.ClearWakeup();
      uint serialnumber;
      string manufacturer;
      byte generation;
      byte medium;
      byte status;
      byte[] buffer;
      return !this.MyDeviceCollector.SmokeDetectorHandler.ReadVersion(out serialnumber, out manufacturer, out generation, out medium, out status, out buffer) ? (SmokeDetectorVersion) null : SmokeDetectorVersion.Parse(serialnumber, manufacturer, generation, medium, status, buffer);
    }

    public byte[] ReadParameterBlock()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.SmokeDetectorHandler.ReadParameter((ushort) 1) ?? (byte[]) null;
    }

    internal MinoprotectII_Events ReadEventMemoryMinoprotectII()
    {
      this.MyDeviceCollector.BreakRequest = false;
      this.MyDeviceCollector.AsyncCom.ClearWakeup();
      byte[] buffer = this.MyDeviceCollector.SmokeDetectorHandler.ReadEventMemory((ushort) 17);
      return buffer == null ? (MinoprotectII_Events) null : MinoprotectII_Events.Parse(buffer);
    }

    internal TestModeParameter ReadTestModeParameter()
    {
      this.MyDeviceCollector.BreakRequest = false;
      byte[] buffer = this.MyDeviceCollector.SmokeDetectorHandler.ReadParameter((ushort) 2);
      return buffer == null ? (TestModeParameter) null : TestModeParameter.Parse(buffer);
    }

    private LoRaParameter ReadLoRaParameter()
    {
      DeviceVersionMBus deviceVersionMbus = this.Device.DeviceCMD.ReadVersion((ProgressHandler) null, CancellationToken.None);
      byte[] appEui = this.LoRa.GetAppEUI((ProgressHandler) null, CancellationToken.None);
      byte[] devEui = this.LoRa.GetDevEUI((ProgressHandler) null, CancellationToken.None);
      byte[] appKey = this.LoRa.GetAppKey((ProgressHandler) null, CancellationToken.None);
      byte[] nwkSkey = this.LoRa.GetNwkSKey((ProgressHandler) null, CancellationToken.None);
      byte[] appSkey = this.LoRa.GetAppSKey((ProgressHandler) null, CancellationToken.None);
      OTAA_ABP otaaAbp = this.LoRa.GetOTAA_ABP((ProgressHandler) null, CancellationToken.None);
      byte transmissionScenario = this.LoRa.GetTransmissionScenario((ProgressHandler) null, CancellationToken.None);
      byte adr = this.LoRa.GetAdr((ProgressHandler) null, CancellationToken.None);
      LoRaWANVersion loRaWanVersion = this.LoRa.GetLoRaWAN_Version((ProgressHandler) null, CancellationToken.None);
      LoRaFcVersion loRaFcVersion = this.LoRa.GetLoRaFC_Version((ProgressHandler) null, CancellationToken.None);
      Version version1 = new Version(deviceVersionMbus.FirmwareVersionObj.VersionString);
      Version version2 = new Version("2.2.1");
      Version version3 = new Version("2.1.6");
      Version version4 = new Version("2.20.0");
      byte[] numArray = new byte[16];
      if (version1 >= version2)
        numArray = this.MBus.GetMBusKey((ProgressHandler) null, CancellationToken.None);
      ushort? nullable1 = new ushort?();
      if (version1 >= version4)
        nullable1 = this.Device.GetCommunicationScenario((ProgressHandler) null, CancellationToken.None).ScenarioOne;
      byte[] armUniqueId = DeviceIdentification.GetArmUniqueID(this.Device.ReadMemory((ProgressHandler) null, CancellationToken.None, 536346704U, 24U, (byte) 69));
      MBusChannelIdentification channelIdentification1 = new MBusChannelIdentification();
      MBusChannelIdentification channelIdentification2 = new MBusChannelIdentification();
      if (version1 >= version3)
      {
        channelIdentification1 = this.MBus.GetChannelIdentification((byte) 0, (ProgressHandler) null, CancellationToken.None);
        channelIdentification2 = this.MBus.GetChannelIdentification((byte) 85, (ProgressHandler) null, CancellationToken.None);
        uint result;
        if (uint.TryParse(channelIdentification2.SerialNumber.ToString(), out result))
        {
          uint uint32 = Utility.ConvertBcdUInt32ToUInt32(Utility.ReverseBytes(Utility.ConvertUnt32ToBcdUInt32(result)));
          channelIdentification2.SerialNumber = (long) uint32;
        }
      }
      bool radioOperation = this.Device.GetRadioOperation((ProgressHandler) null, CancellationToken.None);
      ushort? nullable2 = this.Read_Mbus_interval();
      SmokeDetectorHandlerFunctions.WeekDay? nullable3 = this.Read_Mbus_radio_suppression_days();
      byte? nullable4 = this.Read_Mbus_nighttime_start();
      byte? nullable5 = this.Read_Mbus_nighttime_stop();
      LoRaParameter loRaParameter = new LoRaParameter();
      loRaParameter.JoinEUI = appEui;
      loRaParameter.DevEUI = devEui;
      loRaParameter.DevKey = appKey;
      loRaParameter.NwkSKey = nwkSkey;
      loRaParameter.AppSKey = appSkey;
      loRaParameter.Activation = otaaAbp;
      loRaParameter.TransmissionScenario = transmissionScenario;
      loRaParameter.ADR = adr;
      loRaParameter.ArmUniqueID = armUniqueId;
      loRaParameter.MBusKey = numArray;
      loRaParameter.MBusIdent = channelIdentification1;
      loRaParameter.MBusIdentRadio3 = channelIdentification2;
      loRaParameter.RadioEnabled = radioOperation;
      loRaParameter.Mbus_interval = nullable2;
      loRaParameter.Mbus_radio_suppression_days = nullable3;
      loRaParameter.Mbus_nighttime_start = nullable4;
      loRaParameter.Mbus_nighttime_stop = nullable5;
      ushort? nullable6 = nullable1;
      loRaParameter.CommunicationScenario = nullable6.HasValue ? new int?((int) nullable6.GetValueOrDefault()) : new int?();
      loRaParameter.LoRaWanVersion = loRaWanVersion;
      loRaParameter.LoRaVersion = loRaFcVersion;
      return loRaParameter;
    }

    internal List<MinoprotectIII_Events> ReadEventMemory(ushort address)
    {
      this.MyDeviceCollector.BreakRequest = false;
      byte[] buffer = this.MyDeviceCollector.SmokeDetectorHandler.ReadEventMemory(address);
      return buffer == null ? (List<MinoprotectIII_Events>) null : MinoprotectIII_Events.Parse(buffer);
    }

    public bool StartRadioTest(RadioMode mode)
    {
      return this.ExecuteTestMode(new TestModeParameter()
      {
        FunctionTestMode = FunctionTestMode.NormalOperatingMode_no_Test,
        RadioMode = mode
      });
    }

    public bool StopRadioTest()
    {
      return this.ExecuteTestMode(new TestModeParameter()
      {
        FunctionTestMode = FunctionTestMode.NormalOperatingMode_no_Test,
        RadioMode = RadioMode.NormalOperatingMode_RX_TX
      });
    }

    public bool StartFunctionTest(FunctionTestMode mode)
    {
      return this.ExecuteTestMode(new TestModeParameter()
      {
        FunctionTestMode = mode,
        RadioMode = RadioMode.NormalOperatingMode_RX_TX
      });
    }

    public bool StopFunctionTest()
    {
      return this.ExecuteTestMode(new TestModeParameter()
      {
        FunctionTestMode = FunctionTestMode.NormalOperatingMode_no_Test,
        RadioMode = RadioMode.NormalOperatingMode_RX_TX
      });
    }

    private bool ExecuteTestMode(TestModeParameter testParameter)
    {
      byte[] buffer = testParameter != null ? testParameter.CreateWriteBuffer() : throw new ArgumentNullException(nameof (testParameter), "Can not execute the test mode!");
      if (buffer == null)
        return false;
      SmokeDetectorHandlerFunctions.logger.Debug("ExecuteTestMode(" + testParameter.ToString(20) + ")");
      return this.MyDeviceCollector.SmokeDetectorHandler.WriteDevice(2, buffer);
    }

    public MinoprotectIII_Parameter ReadParameter()
    {
      byte[] buffer = this.ReadParameterBlock();
      if (buffer == null)
        return (MinoprotectIII_Parameter) null;
      return buffer.Length == 44 ? MinoprotectIII_Parameter.Parse(buffer) : throw new Exception("The device is not Minoprotect III");
    }

    public ManufacturingParameter ReadManufacturingParameter()
    {
      this.MyDeviceCollector.BreakRequest = false;
      byte[] buffer = this.MyDeviceCollector.SmokeDetectorHandler.ReadParameter((ushort) 3);
      return buffer == null ? (ManufacturingParameter) null : ManufacturingParameter.Parse(buffer);
    }

    public bool TC_EnterTestMode()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_EnterTestMode();
    }

    public bool TC_ExitTestMode() => this.MyDeviceCollector.SmokeDetectorHandler.TC_ExitTestMode();

    public bool TC_TransmitterLedInSmokeChamberVoltageReferenceTL431()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_TransmitterLedInSmokeChamberVoltageReferenceTL431();
    }

    public bool TC_PiezoTestHighSoundPressure(byte duration = 0)
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_PiezoTestHighSoundPressure(duration);
    }

    public byte TC_PiezoAdjustValueHighSound()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_PiezoAdjustValueHighSound();
    }

    public bool TC_PiezoTestLowSoundPressure()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_PiezoTestLowSoundPressure();
    }

    public bool TC_SetDeliveryState()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_SetDeliveryState();
    }

    public SmokeDetector.HardwareState TC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo();
    }

    public bool TC_ButtonTest() => this.MyDeviceCollector.SmokeDetectorHandler.TC_ButtonTest();

    public SmokeDetector.EepromState? TC_EepromState()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_EepromState();
    }

    public SmokeDetector.TestData TC_TestData()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_TestData();
    }

    public SmokeDetector.ObstructionState TC_ObstructionCheck()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_ObstructionCheck();
    }

    public SmokeDetector.ObstructionState TC_ObstructionCalibrationRead()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_ObstructionCalibrationRead();
    }

    public bool TC_ObstructionCalibrationWrite(
      ushort near1,
      ushort near2,
      ushort near3,
      ushort near4,
      ushort near5,
      ushort near6)
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_ObstructionCalibrationWrite(new SmokeDetector.ObstructionState()
      {
        Near1 = near1,
        Near2 = near2,
        Near3 = near3,
        Near4 = near4,
        Near5 = near5,
        Near6 = near6
      });
    }

    public bool TC_SurroundingAreaMonitoringCheckTransmitter(SmokeDetector.Check led)
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_SurroundingAreaMonitoringCheckTransmitter(led);
    }

    public bool TC_SurroundingAreaMonitoringCheckReceiver()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_SurroundingAreaMonitoringCheckReceiver();
    }

    public byte? TC_SurroundingAreaMonitoringCheckReceiverTestResult()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_SurroundingAreaMonitoringCheckReceiverTestResult();
    }

    public DateTime? TC_ClearTestRecordT1()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_ClearTestRecordT1();
    }

    public DateTime? TC_EraseEEPROM()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_EraseEEPROM();
    }

    public bool? TC_CauseTestAlarm()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_CauseTestAlarm();
    }

    public bool TC_ButtonFunction()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_ButtonFunction();
    }

    public bool TC_ResetDevice() => this.MyDeviceCollector.SmokeDetectorHandler.TC_ResetDevice();

    public SmokeDetector.SmokeDensityAndSensitivity TC_ReadSmokeDensityAndSensitivity()
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_ReadSmokeDensityAndSensitivity();
    }

    public ushort? TC_WriteSmokeDensityThreshold_C_Value(byte value)
    {
      return this.MyDeviceCollector.SmokeDetectorHandler.TC_WriteSmokeDensityThreshold_C_Value(value);
    }

    public SmokeDetector.SmokeDensityAndSensitivity TC_ReadSmokeDensityAndSensitivity_once()
    {
      if (this.isRunning || !this.MyDeviceCollector.SmokeDetectorHandler.TC_ReadSmokeDensityAndSensitivity_90_times())
        return (SmokeDetector.SmokeDensityAndSensitivity) null;
      this.isRunning = true;
      try
      {
        if (!Util.Wait(10000L, "Wait 10 sec for value of SmokeDensityAndSensitivity", (ICancelable) this.MyDeviceCollector, SmokeDetectorHandlerFunctions.logger))
          return (SmokeDetector.SmokeDensityAndSensitivity) null;
        byte[] src1 = this.MyDeviceCollector.SmokeDetectorHandler.Read(4);
        if (src1 == null || src1.Length != 4 || src1[0] != (byte) 104 || src1[3] != (byte) 104 || (int) src1[1] != (int) src1[2])
          return (SmokeDetector.SmokeDensityAndSensitivity) null;
        byte num = src1[1];
        byte[] src2 = this.MyDeviceCollector.SmokeDetectorHandler.Read((int) num + 2);
        if (src2 == null)
          return (SmokeDetector.SmokeDensityAndSensitivity) null;
        byte[] numArray = new byte[(int) num + 6];
        Buffer.BlockCopy((Array) src1, 0, (Array) numArray, 0, 4);
        Buffer.BlockCopy((Array) src2, 0, (Array) numArray, 4, src2.Length);
        if (numArray != null)
          return SmokeDetector.SmokeDensityAndSensitivity.Parse(numArray);
      }
      finally
      {
        this.isRunning = false;
      }
      return (SmokeDetector.SmokeDensityAndSensitivity) null;
    }

    public bool TC_ReadSmokeDensityAndSensitivity_90_times()
    {
      if (this.isRunning || !this.MyDeviceCollector.SmokeDetectorHandler.TC_ReadSmokeDensityAndSensitivity_90_times())
        return false;
      this.isRunning = true;
      int num1 = 90;
      try
      {
        while (num1 > 0)
        {
          --num1;
          if (!Util.Wait(10000L, "Wait 10 sec for value of SmokeDensityAndSensitivity", (ICancelable) this.MyDeviceCollector, SmokeDetectorHandlerFunctions.logger))
            return false;
          if (this.OnSmokeDensityAndSensitivityValueReceived != null)
          {
            byte[] src1 = this.MyDeviceCollector.SmokeDetectorHandler.Read(4);
            if (src1 != null && src1.Length == 4 && src1[0] == (byte) 104 && src1[3] == (byte) 104 && (int) src1[1] == (int) src1[2])
            {
              byte num2 = src1[1];
              byte[] src2 = this.MyDeviceCollector.SmokeDetectorHandler.Read((int) num2 + 2);
              if (src2 != null)
              {
                byte[] numArray = new byte[(int) num2 + 6];
                Buffer.BlockCopy((Array) src1, 0, (Array) numArray, 0, 4);
                Buffer.BlockCopy((Array) src2, 0, (Array) numArray, 4, src2.Length);
                if (numArray != null)
                  this.OnSmokeDensityAndSensitivityValueReceived((object) this, SmokeDetector.SmokeDensityAndSensitivity.Parse(numArray));
              }
            }
          }
        }
      }
      finally
      {
        this.isRunning = false;
      }
      return true;
    }

    public SmokeDetector.SmokeDensityAndSensitivity TC_ReadSmokeDensityAndSensitivity_FirstResultFrom90()
    {
      if (!this.MyDeviceCollector.SmokeDetectorHandler.TC_ReadSmokeDensityAndSensitivity_90_times() || !Util.Wait(9000L, "Wait 9 sec for value of SmokeDensityAndSensitivity", (ICancelable) this.MyDeviceCollector, SmokeDetectorHandlerFunctions.logger))
        return (SmokeDetector.SmokeDensityAndSensitivity) null;
      byte[] src1 = this.MyDeviceCollector.SmokeDetectorHandler.Read(4);
      if (src1 == null || src1.Length != 4 || src1[0] != (byte) 104 || src1[3] != (byte) 104 || (int) src1[1] != (int) src1[2])
        return (SmokeDetector.SmokeDensityAndSensitivity) null;
      byte num = src1[1];
      byte[] src2 = this.MyDeviceCollector.SmokeDetectorHandler.Read((int) num + 2);
      if (src2 == null)
        return (SmokeDetector.SmokeDensityAndSensitivity) null;
      byte[] numArray = new byte[(int) num + 6];
      Buffer.BlockCopy((Array) src1, 0, (Array) numArray, 0, 4);
      Buffer.BlockCopy((Array) src2, 0, (Array) numArray, 4, src2.Length);
      return numArray == null ? (SmokeDetector.SmokeDensityAndSensitivity) null : SmokeDetector.SmokeDensityAndSensitivity.Parse(numArray);
    }

    public SmokeDetector.Smoke_A_B TC_Set_A_B()
    {
      byte[] buffer = this.MyDeviceCollector.SmokeDetectorHandler.TC_Set_A_B();
      return buffer == null ? (SmokeDetector.Smoke_A_B) null : SmokeDetector.Smoke_A_B.Parse(buffer);
    }

    public async Task TransmitModulatedCarrierAsync(
      ushort timeoutInSec,
      ProgressHandler progress,
      CancellationToken token)
    {
      DeviceVersionMBus deviceVersionMbus = await this.Device.DeviceCMD.ReadVersionAsync(progress, token);
      await this.Radio.TransmitModulatedCarrierAsync(timeoutInSec, progress, token);
    }

    public async Task TransmitUnmodulatedCarrierAsync(
      ushort timeoutInSec,
      ProgressHandler progress,
      CancellationToken token)
    {
      DeviceVersionMBus deviceVersionMbus = await this.Device.DeviceCMD.ReadVersionAsync(progress, token);
      await this.Radio.TransmitUnmodulatedCarrierAsync(timeoutInSec, progress, token);
    }

    public async Task StopRadioTests(ProgressHandler progress, CancellationToken token)
    {
      DeviceVersionMBus deviceVersionMbus = await this.Device.DeviceCMD.ReadVersionAsync(progress, token);
      await this.Radio.StopRadioTests(progress, token);
    }

    public async Task SendTestPacketAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort interval,
      ushort timeoutInSec,
      uint deviceID,
      byte[] arbitraryData,
      string syncWord = "91D3")
    {
      DeviceVersionMBus deviceVersionMbus = await this.Device.DeviceCMD.ReadVersionAsync(progress, token);
      await this.Radio.SendTestPacketAsync(interval, timeoutInSec, deviceID, arbitraryData, progress, token, syncWord);
    }

    public async Task<double?> ReceiveTestPacketAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte timeoutInSec,
      uint deviceID,
      string syncWord = "91D3")
    {
      double? testPacketAsync = await this.Radio.ReceiveTestPacketAsync(timeoutInSec, deviceID, progress, token, syncWord);
      return testPacketAsync;
    }

    public double? ReceiveTestPacket(
      ProgressHandler progress,
      CancellationToken token,
      byte timeoutInSec,
      uint deviceID,
      string syncWord = "91D3")
    {
      return this.Radio.ReceiveTestPacket(timeoutInSec, deviceID, progress, token, syncWord);
    }

    public async Task SetFrequencyIncrementAsync(
      ProgressHandler progress,
      CancellationToken token,
      int frequency_Hz)
    {
      DeviceVersionMBus deviceVersionMbus = await this.Device.DeviceCMD.ReadVersionAsync(progress, token);
      await this.Radio.SetFrequencyIncrementAsync(frequency_Hz, progress, token);
      await this.Device.ResetDeviceAsync(progress, token);
    }

    public async Task SetCenterFrequencyAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint frequency_Hz)
    {
      DeviceVersionMBus deviceVersionMbus = await this.Device.DeviceCMD.ReadVersionAsync(progress, token);
      await this.Radio.SetCenterFrequencyAsync(frequency_Hz, progress, token);
      await this.Device.ResetDeviceAsync(progress, token);
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int subDevice = 0)
    {
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      if (this.WorkMeter.Version == null)
        throw new ArgumentNullException("WorkMeter.Version");
      SortedList<OverrideID, ConfigurationParameter> r1 = new SortedList<OverrideID, ConfigurationParameter>();
      if (UserManager.IsNewLicenseModel())
      {
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.MeterID, (object) this.WorkMeter.ManufacturingParameter.MeterID);
        if (this.WorkMeter.LoRaParameter != null)
        {
          if (this.WorkMeter.LoRaParameter.TransmissionScenario == (byte) 3)
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.SerialNumber, (object) this.WorkMeter.LoRaParameter.MBusIdentRadio3.SerialNumber);
          else
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.SerialNumber, (object) this.WorkMeter.LoRaParameter.MBusIdent.SerialNumber);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.Manufacturer, (object) this.WorkMeter.LoRaParameter.MBusIdent.Manufacturer);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.MBusGeneration, (object) this.WorkMeter.LoRaParameter.MBusIdent.Generation);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.Medium, (object) this.WorkMeter.LoRaParameter.MBusIdent.Medium);
        }
        else
        {
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.SerialNumber, (object) this.WorkMeter.Version.Serialnumber);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.Manufacturer, (object) this.WorkMeter.Version.Manufacturer);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.MBusGeneration, (object) this.WorkMeter.Version.Generation);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.Medium, (object) this.WorkMeter.Version.Medium);
        }
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.FirmwareVersion, (object) this.WorkMeter.Version.VersionString);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.SapNumber, (object) this.WorkMeter.Version.SapNumber);
        SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.DeviceClock, (object) this.WorkMeter.Parameter.CurrentDateTime);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.DateOfFirstActivation, (object) this.WorkMeter.Parameter.DateOfFirstActivation);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.CurrentEvents, (object) this.WorkMeter.Parameter.CurrentStateOfEvents);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.PushButtonError, (object) this.WorkMeter.Parameter.PushButtonError);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.HornDriveLevel, (object) this.WorkMeter.Parameter.HornDriveLevel);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.RemovingDetection, (object) this.WorkMeter.Parameter.RemovingDetection);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.NumberSmokeAlarms, (object) this.WorkMeter.Parameter.NumberSmokeAlarms);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.NumberTestAlarms, (object) this.WorkMeter.Parameter.NumberTestAlarms);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.ObstructionDetection, (object) this.WorkMeter.Parameter.ObstructionDetection);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.SurroundingProximity, (object) this.WorkMeter.Parameter.SurroundingProximity);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.LedFailure, (object) this.WorkMeter.Parameter.LED_Failure);
        SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.StatusOfInterlinkedDevices, (object) this.WorkMeter.Parameter.StatusOfInterlinkedDevices);
        SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.SetToDelivery, (object) false, true, (string[]) null);
        if (this.WorkMeter.LoRaParameter != null)
        {
          byte transmissionScenario = this.WorkMeter.LoRaParameter.TransmissionScenario;
          int num1;
          switch (transmissionScenario)
          {
            case 1:
            case 2:
              num1 = 1;
              break;
            default:
              num1 = transmissionScenario == (byte) 3 ? 1 : 0;
              break;
          }
          bool flag1 = num1 != 0;
          bool flag2 = transmissionScenario == (byte) 10 || transmissionScenario == (byte) 11;
          if (flag1)
          {
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.JoinEUI, (object) BitConverter.ToUInt64(this.WorkMeter.LoRaParameter.JoinEUI, 0));
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.DevEUI, (object) BitConverter.ToUInt64(this.WorkMeter.LoRaParameter.DevEUI, 0));
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.AppKey, (object) this.WorkMeter.LoRaParameter.DevKey);
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.NwkSKey, (object) this.WorkMeter.LoRaParameter.NwkSKey);
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.AppSKey, (object) this.WorkMeter.LoRaParameter.AppSKey);
            SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.Activation, (object) this.WorkMeter.LoRaParameter.Activation.ToString(), false, new string[2]
            {
              "OTAA",
              "ABP"
            });
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.ADR, (object) this.WorkMeter.LoRaParameter.ADR);
            SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.SendJoinRequest, (object) false, true, (string[]) null);
            if (this.WorkMeter.LoRaParameter.LoRaWanVersion == null)
              this.WorkMeter.LoRaParameter.LoRaWanVersion = new LoRaWANVersion()
              {
                MainVersion = (byte) 1,
                MinorVersion = (byte) 0,
                ReleaseNr = (byte) 1
              };
            if (this.WorkMeter.LoRaParameter.LoRaVersion == null)
              this.WorkMeter.LoRaParameter.LoRaVersion = new LoRaFcVersion()
              {
                Version = BitConverter.ToUInt16(new byte[2]
                {
                  (byte) 16,
                  (byte) 0
                }, 0)
              };
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.LoRaWanVersion, (object) this.WorkMeter.LoRaParameter.LoRaWanVersion.ToString());
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.LoRaVersion, (object) this.WorkMeter.LoRaParameter.LoRaVersion.ToString());
          }
          if (flag2)
          {
            SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.AESKey, (object) AES.AesKeyToString(this.WorkMeter.LoRaParameter.MBusKey));
            ushort? mbusInterval = this.WorkMeter.LoRaParameter.Mbus_interval;
            if (mbusInterval.HasValue)
            {
              SortedList<OverrideID, ConfigurationParameter> r2 = r1;
              mbusInterval = this.WorkMeter.LoRaParameter.Mbus_interval;
              // ISSUE: variable of a boxed type
              __Boxed<ushort> local = (System.ValueType) mbusInterval.Value;
              SmokeDetectorHandlerFunctions.AddParam(true, r2, OverrideID.RadioSendInterval, (object) local);
            }
            SmokeDetectorHandlerFunctions.WeekDay? radioSuppressionDays = this.WorkMeter.LoRaParameter.Mbus_radio_suppression_days;
            if (radioSuppressionDays.HasValue)
            {
              radioSuppressionDays = this.WorkMeter.LoRaParameter.Mbus_radio_suppression_days;
              byte num2 = (byte) radioSuppressionDays.Value;
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveMonday, (object) !Convert.ToBoolean((int) num2 & 1));
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveTuesday, (object) !Convert.ToBoolean((int) num2 & 2));
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveWednesday, (object) !Convert.ToBoolean((int) num2 & 4));
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveThursday, (object) !Convert.ToBoolean((int) num2 & 8));
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveFriday, (object) !Convert.ToBoolean((int) num2 & 16));
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveSaturday, (object) !Convert.ToBoolean((int) num2 & 32));
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveSunday, (object) !Convert.ToBoolean((int) num2 & 64));
            }
            byte? nullable = this.WorkMeter.LoRaParameter.Mbus_nighttime_start;
            if (nullable.HasValue)
            {
              nullable = this.WorkMeter.LoRaParameter.Mbus_nighttime_start;
              byte num3 = nullable.Value;
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveStopTime, (object) num3);
            }
            nullable = this.WorkMeter.LoRaParameter.Mbus_nighttime_stop;
            if (nullable.HasValue)
            {
              nullable = this.WorkMeter.LoRaParameter.Mbus_nighttime_stop;
              byte num4 = nullable.Value;
              SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.RadioActiveStartTime, (object) num4);
            }
          }
          if (this.WorkMeter.LoRaParameter.CommunicationScenario.HasValue)
            SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.CommunicationScenario, (object) this.WorkMeter.LoRaParameter.CommunicationScenario);
          else
            SmokeDetectorHandlerFunctions.AddParam(true, r1, OverrideID.TransmissionScenario, (object) transmissionScenario);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.RadioEnabled, (object) this.WorkMeter.LoRaParameter.RadioEnabled);
          SmokeDetectorHandlerFunctions.AddParam(false, r1, OverrideID.RadioVersion, (object) "2.8");
        }
      }
      else
      {
        bool flag3 = false;
        if (UserRights.GlobalUserRights.PackageName == "ConfigurationManagerPro")
          flag3 = true;
        else if (UserRights.GlobalUserRights.PackageName == "ConfigurationManager" && UserRights.GlobalUserRights.OptionPackageName == "Professional")
          flag3 = true;
        bool flag4 = UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer);
        r1.Add(OverrideID.MeterID, new ConfigurationParameter(OverrideID.MeterID, (object) this.WorkMeter.ManufacturingParameter.MeterID));
        r1.Add(OverrideID.SerialNumber, new ConfigurationParameter(OverrideID.SerialNumber, (object) this.WorkMeter.Version.Serialnumber));
        r1.Add(OverrideID.Manufacturer, new ConfigurationParameter(OverrideID.Manufacturer, (object) this.WorkMeter.Version.Manufacturer));
        r1.Add(OverrideID.MBusGeneration, new ConfigurationParameter(OverrideID.MBusGeneration, (object) this.WorkMeter.Version.Generation));
        r1.Add(OverrideID.Medium, new ConfigurationParameter(OverrideID.Medium, (object) this.WorkMeter.Version.Medium));
        r1.Add(OverrideID.FirmwareVersion, new ConfigurationParameter(OverrideID.FirmwareVersion, (object) this.WorkMeter.Version.VersionString));
        r1.Add(OverrideID.SapNumber, new ConfigurationParameter(OverrideID.SapNumber, (object) this.WorkMeter.Version.SapNumber));
        r1.Add(OverrideID.DeviceClock, new ConfigurationParameter(OverrideID.DeviceClock, (object) this.WorkMeter.Parameter.CurrentDateTime));
        r1.Add(OverrideID.DateOfFirstActivation, new ConfigurationParameter(OverrideID.DateOfFirstActivation, (object) this.WorkMeter.Parameter.DateOfFirstActivation));
        r1.Add(OverrideID.CurrentEvents, new ConfigurationParameter(OverrideID.CurrentEvents, (object) this.WorkMeter.Parameter.CurrentStateOfEvents));
        r1.Add(OverrideID.PushButtonError, new ConfigurationParameter(OverrideID.PushButtonError, (object) this.WorkMeter.Parameter.PushButtonError));
        r1.Add(OverrideID.HornDriveLevel, new ConfigurationParameter(OverrideID.HornDriveLevel, (object) this.WorkMeter.Parameter.HornDriveLevel));
        r1.Add(OverrideID.RemovingDetection, new ConfigurationParameter(OverrideID.RemovingDetection, (object) this.WorkMeter.Parameter.RemovingDetection));
        r1.Add(OverrideID.NumberSmokeAlarms, new ConfigurationParameter(OverrideID.NumberSmokeAlarms, (object) this.WorkMeter.Parameter.NumberSmokeAlarms));
        r1.Add(OverrideID.NumberTestAlarms, new ConfigurationParameter(OverrideID.NumberTestAlarms, (object) this.WorkMeter.Parameter.NumberTestAlarms));
        r1.Add(OverrideID.ObstructionDetection, new ConfigurationParameter(OverrideID.ObstructionDetection, (object) this.WorkMeter.Parameter.ObstructionDetection));
        r1.Add(OverrideID.SurroundingProximity, new ConfigurationParameter(OverrideID.SurroundingProximity, (object) this.WorkMeter.Parameter.SurroundingProximity));
        r1.Add(OverrideID.LedFailure, new ConfigurationParameter(OverrideID.LedFailure, (object) this.WorkMeter.Parameter.LED_Failure));
        r1.Add(OverrideID.StatusOfInterlinkedDevices, new ConfigurationParameter(OverrideID.StatusOfInterlinkedDevices, (object) this.WorkMeter.Parameter.StatusOfInterlinkedDevices));
        r1.Add(OverrideID.SetToDelivery, new ConfigurationParameter(OverrideID.SetToDelivery, (object) false)
        {
          IsFunction = true,
          HasWritePermission = flag3 | flag4
        });
      }
      return r1;
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      SmokeDetectorHandlerFunctions.AddParam(canChanged, r, overrideID, obj, false, (string[]) null);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj,
      bool isFunction,
      string[] allowedValues)
    {
      if (!UserManager.IsConfigParamVisible(overrideID))
        return;
      bool flag = false;
      if (canChanged)
        flag = UserManager.IsConfigParamEditable(overrideID);
      r.Add(overrideID, new ConfigurationParameter(overrideID, obj)
      {
        HasWritePermission = flag,
        AllowedValues = allowedValues,
        IsFunction = isFunction
      });
    }

    public void ResetDevice()
    {
      this.Device.ResetDevice((ProgressHandler) null, CancellationToken.None);
    }

    public ushort? Read_Mbus_interval()
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_interval");
      return !address.HasValue ? new ushort?() : new ushort?((ushort) ((uint) BitConverter.ToUInt16(this.Device.ReadMemory((ProgressHandler) null, none, address.Value, 2U, (byte) 64), 0) * 2U));
    }

    public void Write_Mbus_interval(ushort mbus_interval)
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_interval");
      if (!address.HasValue)
        return;
      this.Device.WriteMemory((ProgressHandler) null, none, address.Value, BitConverter.GetBytes((int) mbus_interval / 2));
    }

    public SmokeDetectorHandlerFunctions.WeekDay? Read_Mbus_radio_suppression_days()
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_radio_suppression_days");
      return !address.HasValue ? new SmokeDetectorHandlerFunctions.WeekDay?() : new SmokeDetectorHandlerFunctions.WeekDay?((SmokeDetectorHandlerFunctions.WeekDay) this.Device.ReadMemory((ProgressHandler) null, none, address.Value, 1U, (byte) 64)[0]);
    }

    public void Write_Mbus_radio_suppression_days(byte mbus_radio_suppression_days)
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_radio_suppression_days");
      if (!address.HasValue)
        return;
      this.Device.WriteMemory((ProgressHandler) null, none, address.Value, new byte[1]
      {
        mbus_radio_suppression_days
      });
    }

    public byte? Read_Mbus_nighttime_start()
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_nighttime_start");
      return !address.HasValue ? new byte?() : new byte?(this.Device.ReadMemory((ProgressHandler) null, none, address.Value, 1U, (byte) 64)[0]);
    }

    public void Write_Mbus_nighttime_start(byte mbus_nighttime_start)
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_nighttime_start");
      if (!address.HasValue)
        return;
      this.Device.WriteMemory((ProgressHandler) null, none, address.Value, new byte[1]
      {
        mbus_nighttime_start
      });
    }

    public byte? Read_Mbus_nighttime_stop()
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_nighttime_stop");
      return !address.HasValue ? new byte?() : new byte?(this.Device.ReadMemory((ProgressHandler) null, none, address.Value, 1U, (byte) 64)[0]);
    }

    public void Write_Mbus_nighttime_stop(byte mbus_nighttime_stop)
    {
      CancellationToken none = CancellationToken.None;
      uint? address = this.GetAddress(this.DeviceCommands.ReadVersion((ProgressHandler) null, none).FirmwareVersion.Value, "Mbus_nighttime_stop");
      if (!address.HasValue)
        return;
      this.Device.WriteMemory((ProgressHandler) null, none, address.Value, new byte[1]
      {
        mbus_nighttime_stop
      });
    }

    private uint? GetAddress(uint version, string parameter)
    {
      switch (parameter)
      {
        case "Mbus_interval":
          if (version == 33665052U || version == 33669148U || version == 33673244U || version == 33685532U || version == 33689628U || version == 33693724U)
            return new uint?(536872746U);
          int num1;
          switch (version)
          {
            case 33706012:
              return new uint?(536872770U);
            case 33755164:
              num1 = 1;
              break;
            default:
              num1 = version == 33759260U ? 1 : 0;
              break;
          }
          if (num1 != 0)
            return new uint?(536872710U);
          if (version == 33763356U || version == 33767452U || version == 33771548U)
            return new uint?(536872718U);
          switch (version)
          {
            case 33775644:
              return new uint?(536872742U);
            case 34865180:
              return new uint?(536873174U);
            case 34869276:
              return new uint?(536874018U);
          }
          break;
        case "Mbus_radio_suppression_days":
          if (version == 33665052U || version == 33669148U || version == 33673244U || version == 33685532U || version == 33689628U || version == 33693724U)
            return new uint?(536872753U);
          int num2;
          switch (version)
          {
            case 33706012:
              return new uint?(536872777U);
            case 33755164:
              num2 = 1;
              break;
            default:
              num2 = version == 33759260U ? 1 : 0;
              break;
          }
          if (num2 != 0)
            return new uint?(536872717U);
          if (version == 33763356U || version == 33767452U || version == 33771548U)
            return new uint?(536872725U);
          switch (version)
          {
            case 33775644:
              return new uint?(536872749U);
            case 34865180:
              return new uint?(536873181U);
            case 34869276:
              return new uint?(536874025U);
          }
          break;
        case "Mbus_nighttime_start":
          if (version == 33665052U || version == 33669148U || version == 33673244U || version == 33685532U || version == 33689628U || version == 33693724U)
            return new uint?(536872751U);
          int num3;
          switch (version)
          {
            case 33706012:
              return new uint?(536872775U);
            case 33755164:
              num3 = 1;
              break;
            default:
              num3 = version == 33759260U ? 1 : 0;
              break;
          }
          if (num3 != 0)
            return new uint?(536872715U);
          if (version == 33763356U || version == 33767452U || version == 33771548U)
            return new uint?(536872723U);
          switch (version)
          {
            case 33775644:
              return new uint?(536872747U);
            case 34865180:
              return new uint?(536873179U);
            case 34869276:
              return new uint?(536874023U);
          }
          break;
        case "Mbus_nighttime_stop":
          if (version == 33665052U || version == 33669148U || version == 33673244U || version == 33685532U || version == 33689628U || version == 33693724U)
            return new uint?(536872752U);
          int num4;
          switch (version)
          {
            case 33706012:
              return new uint?(536872776U);
            case 33755164:
              num4 = 1;
              break;
            default:
              num4 = version == 33759260U ? 1 : 0;
              break;
          }
          if (num4 != 0)
            return new uint?(536872716U);
          if (version == 33763356U || version == 33767452U || version == 33771548U)
            return new uint?(536872724U);
          switch (version)
          {
            case 33775644:
              return new uint?(536872748U);
            case 34865180:
              return new uint?(536873180U);
            case 34869276:
              return new uint?(536874024U);
          }
          break;
      }
      return new uint?();
    }

    [Flags]
    public enum WeekDay : byte
    {
      MONDAY = 1,
      TUESDAY = 2,
      WEDNESDAY = 4,
      THURSDAY = 8,
      FRIDAY = 16, // 0x10
      SATURDAY = 32, // 0x20
      SUNDAY = 64, // 0x40
    }
  }
}
