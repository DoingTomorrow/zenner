// Decompiled with JetBrains decompiler
// Type: PDC_Handler.PDC_HandlerFunctions
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using DeviceCollector;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public sealed class PDC_HandlerFunctions : ICreateMeter
  {
    private static Logger logger = LogManager.GetLogger(nameof (PDC_HandlerFunctions));
    public static readonly DateTime DateTimeNull = new DateTime(2000, 1, 1);
    internal IDeviceCollector MyDeviceCollector;
    internal PDC_Meter ConnectedMeter;
    internal PDC_Meter WorkMeter;
    internal PDC_Meter TypeMeter;
    internal PDC_Meter BackupMeter;
    private PDC_HandlerWindow MyWindow;
    private AsyncOperation asyncOperation = (AsyncOperation) null;

    public PDC_HandlerFunctions()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      if (!ZR_Component.CommonGmmInterface.LoadedComponentsList.ContainsKey(GMM_Components.DeviceCollector))
        return;
      this.MyDeviceCollector = ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector] as IDeviceCollector;
    }

    public PDC_HandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.MyDeviceCollector = deviceCollector;
    }

    public PDC_Meter Meter => this.WorkMeter;

    public event ValueEventHandler<int> OnProgress;

    public string ShowHandlerWindow()
    {
      this.MyWindow = new PDC_HandlerWindow(this);
      this.MyWindow.BringToFront();
      int num = (int) this.MyWindow.ShowDialog();
      string nextComponentName = this.MyWindow.NextComponentName;
      this.MyWindow.Dispose();
      return nextComponentName;
    }

    public void GMM_Dispose()
    {
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
      ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
      ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem();
      toolStripMenuItem4.Name = "ComponentMenuItemDeviceCollector";
      toolStripMenuItem4.Size = new Size(173, 22);
      toolStripMenuItem4.Text = GMM_Components.DeviceCollector.ToString();
      toolStripMenuItem4.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem4.Tag = (object) GMM_Components.DeviceCollector.ToString();
      ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem();
      toolStripMenuItem5.Name = "ComponentMenuItemAsyncCom";
      toolStripMenuItem5.Size = new Size(173, 22);
      toolStripMenuItem5.Text = GMM_Components.AsyncCom.ToString();
      toolStripMenuItem5.Click += new System.EventHandler(this.MenuItemSelectComponent_Click);
      toolStripMenuItem5.Tag = (object) GMM_Components.AsyncCom.ToString();
      return new ToolStripItem[6]
      {
        (ToolStripItem) toolStripMenuItem1,
        (ToolStripItem) toolStripMenuItem2,
        (ToolStripItem) toolStripMenuItem3,
        (ToolStripItem) toolStripSeparator,
        (ToolStripItem) toolStripMenuItem4,
        (ToolStripItem) toolStripMenuItem5
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
      PDC_HandlerFunctions.logger.Debug("Clear all values in handler.");
      this.ConnectedMeter = (PDC_Meter) null;
      this.WorkMeter = (PDC_Meter) null;
      this.BackupMeter = (PDC_Meter) null;
      this.TypeMeter = (PDC_Meter) null;
    }

    public bool UpgradeFirmware(byte[] firmware)
    {
      if (firmware == null)
        throw new ArgumentNullException("Firmware can not be null!");
      if (this.OnProgress != null)
        this.OnProgress((object) this, 1);
      if (this.ManageIrDaWakeUpAndReadVersion() != null)
      {
        this.RadioDisable();
        this.PulseDisable();
      }
      if (!this.UpdateModeEnter())
        return false;
      for (uint address = 38400; address < (uint) ushort.MaxValue; address += 512U)
      {
        if (!this.UpdateFirmwareBlock(firmware, address))
        {
          Thread.Sleep(100);
          PDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 1 of 5");
          if (!this.UpdateFirmwareBlock(firmware, address))
          {
            Thread.Sleep(100);
            PDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 2 of 5");
            if (!this.UpdateFirmwareBlock(firmware, address))
            {
              Thread.Sleep(100);
              PDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 3 of 5");
              if (!this.UpdateFirmwareBlock(firmware, address))
              {
                Thread.Sleep(100);
                PDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 4 of 5");
                if (!this.UpdateFirmwareBlock(firmware, address))
                {
                  PDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 5 of 5");
                  return false;
                }
              }
            }
          }
        }
      }
      if (this.OnProgress != null)
        this.OnProgress((object) this, 100);
      return this.UpdateModeExit() && this.ReadVersion() != null;
    }

    private bool UpdateFirmwareBlock(byte[] firmware, uint address)
    {
      if (!this.UpdateModeEraseFlash(address))
      {
        Thread.Sleep(100);
        PDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 1 of 4");
        if (!this.UpdateModeEraseFlash(address))
        {
          Thread.Sleep(100);
          PDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 2 of 4");
          if (!this.UpdateModeEraseFlash(address))
          {
            Thread.Sleep(100);
            PDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 3 of 4");
            if (!this.UpdateModeEraseFlash(address))
            {
              PDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 4 of 4");
              return false;
            }
          }
        }
      }
      for (uint index1 = address; index1 < address + 512U; index1 += 128U)
      {
        byte[] numArray1 = new byte[128];
        Buffer.BlockCopy((Array) firmware, (int) index1, (Array) numArray1, 0, numArray1.Length);
        if (this.OnProgress != null)
          this.OnProgress((object) this, ((int) index1 - 38400) * 100 / 27135);
        if (!this.UpdateModeWriteFlash(index1, numArray1))
        {
          Thread.Sleep(100);
          PDC_HandlerFunctions.logger.Warn("Failed to write 128 bytes! Attempt 1 of 2");
          if (!this.UpdateModeWriteFlash(index1, numArray1))
          {
            PDC_HandlerFunctions.logger.Warn("Failed to write 128 bytes! Attempt 2 of 2");
            return false;
          }
        }
        byte[] numArray2 = this.UpdateModeReadFlash(index1, (byte) 128);
        if (numArray2 == null)
        {
          Thread.Sleep(100);
          PDC_HandlerFunctions.logger.Warn("Failed to read 128 bytes! Attempt 1 of 3");
          numArray2 = this.UpdateModeReadFlash(index1, (byte) 128);
          if (numArray2 == null)
          {
            Thread.Sleep(100);
            PDC_HandlerFunctions.logger.Warn("Failed to read 128 bytes! Attempt 2 of 3");
            numArray2 = this.UpdateModeReadFlash(index1, (byte) 128);
            if (numArray2 == null)
            {
              PDC_HandlerFunctions.logger.Warn("Can not read 128 bytes. Attempt 3 of 3");
              return false;
            }
          }
        }
        for (int index2 = 0; index2 < numArray2.Length; ++index2)
        {
          if ((int) numArray1[index2] != (int) numArray2[index2])
          {
            PDC_HandlerFunctions.logger.Warn("Corrupt block detected! Write error.");
            return false;
          }
        }
      }
      return true;
    }

    public bool ReadDevice() => this.ReadDevice(false);

    public bool ReadDevice(bool readLoggerToo)
    {
      this.MyDeviceCollector.BreakRequest = false;
      DeviceVersion version = this.ManageIrDaWakeUpAndReadVersion();
      if (version == null)
        return false;
      RangeSet<ushort> blocksToRead = version.Type == PDC_DeviceIdentity.PDC_WmBus ? PDC_MemoryMap.GetMemoryBlocksToRead(version) : throw new Exception(Ot.Gtt(Tg.Common, "NotPDC1", "Connected device is not PDC"));
      if (blocksToRead == null)
        throw new NotSupportedException("This firmware version is not supported! Version: " + version.VersionString);
      PDC_MemoryMap map = new PDC_MemoryMap();
      if (!this.ReadMemory(map, blocksToRead))
        return false;
      if (map.IsEmpty())
        throw new Exception("The MAP is empty! Version:" + version.VersionString);
      if (readLoggerToo)
      {
        RangeSet<ushort> loggerBlocksToRead = PDC_HandlerFunctions.GetLoggerBlocksToRead(version, map);
        if (loggerBlocksToRead != null && loggerBlocksToRead.Count > 0 && !this.ReadMemory(map, loggerBlocksToRead))
          return false;
      }
      this.ConnectedMeter = new PDC_Meter(map);
      this.ConnectedMeter.Version = version;
      this.TryLoadType(version);
      this.WorkMeter = this.ConnectedMeter.DeepCopy();
      return true;
    }

    private void TryLoadType(DeviceVersion version)
    {
      DeviceIdentification deviceIdentification = this.ConnectedMeter.GetDeviceIdentification();
      if (deviceIdentification == null || !deviceIdentification.IsChecksumOK)
        return;
      this.ConnectedMeter.DBDeviceInfo.MeterInfo = PDC_Database.GetMeterInfo(deviceIdentification.MeterInfoID);
      this.ConnectedMeter.DBDeviceInfo.HardwareType = MeterDatabase.GetHardwareType((int) version.HardwareTypeID);
      try
      {
        this.TypeMeter = PDC_HandlerFunctions.LoadType((int) deviceIdentification.MeterInfoID);
      }
      catch (Exception ex)
      {
        PDC_HandlerFunctions.logger.Fatal("Can not load type! Error: " + ex.Message);
      }
    }

    private bool ReadMemory(PDC_MemoryMap map, RangeSet<ushort> blocksToRead)
    {
      int num = 0;
      foreach (Range<ushort> range in (List<Range<ushort>>) blocksToRead)
      {
        byte[] buffer;
        if (!this.ReadMemory(range.Start, (int) range.End + 1 - (int) range.Start, out buffer))
          return false;
        map.SetMemoryBytes(range.Start, buffer);
        if (this.OnProgress != null)
        {
          int progress = 100;
          if (blocksToRead.IndexOf(range) != blocksToRead.Count - 1)
            progress = Convert.ToInt32((double) num++ / (double) blocksToRead.Count * 100.0);
          this.OnProgressAsynchronously(progress);
        }
      }
      return true;
    }

    private static RangeSet<ushort> GetLoggerBlocksToRead(DeviceVersion version, PDC_MemoryMap map)
    {
      ushort uint16_1 = BitConverter.ToUInt16(map.GetMemoryBytes(PDC_MemoryMap.GetParameter(version, "log_halfmonth_address")), 0);
      ushort uint16_2 = BitConverter.ToUInt16(map.GetMemoryBytes(PDC_MemoryMap.GetParameter(version, "log_fullmonth_address")), 0);
      ushort uint16_3 = BitConverter.ToUInt16(map.GetMemoryBytes(PDC_MemoryMap.GetParameter(version, "log_stichtag_address")), 0);
      bool[] points = new bool[(int) ushort.MaxValue];
      for (int index = 0; index < points.Length; ++index)
      {
        if (index >= 32768 && index <= 35071 && uint16_1 > (ushort) 32768 && index <= (int) uint16_1)
          points[index] = true;
        else if (index >= 35072 && index <= 37375 && uint16_2 > (ushort) 35072 && index <= (int) uint16_2)
        {
          points[index] = true;
        }
        else
        {
          int num = index < 37376 || index > 37759 || uint16_3 <= (ushort) 37376 ? 0 : (index <= (int) uint16_3 ? 1 : 0);
          points[index] = num != 0;
        }
      }
      ushort addressOfRamLogger = LoggerManager.GetStartAddressOfRamLogger(version);
      for (int index = (int) addressOfRamLogger; index <= (int) addressOfRamLogger + (int) LoggerManager.RAM_LOGGER_SIZE; ++index)
        points[index] = true;
      return PDC_MemoryMap.ConvertBoolArrayToRangeSet(points);
    }

    public bool WriteDevice() => this.WriteDevice(true, true);

    internal bool WriteDevice(bool doRamBackup, bool doDeviceReset)
    {
      this.MyDeviceCollector.BreakRequest = false;
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WriteDevice: WorkMeter can not be null!");
      DeviceVersion a = this.ManageIrDaWakeUpAndReadVersion();
      if (a == null)
        return false;
      SortedList<ushort, byte[]> changedRamBlocks = this.WorkMeter.GetChangedRamBlocks(this.ConnectedMeter);
      List<ushort> segmentsToErase;
      SortedList<ushort, byte[]> changedFlashBlocks = this.WorkMeter.GetChangedFlashBlocks(this.ConnectedMeter, out segmentsToErase);
      bool flag1 = changedRamBlocks != null && changedRamBlocks.Count > 0;
      bool flag2 = changedFlashBlocks != null && changedFlashBlocks.Count > 0 && segmentsToErase != null && segmentsToErase.Count > 0;
      if (!flag1 && !flag2)
        return true;
      DeviceVersion version = this.WorkMeter.Version;
      if (!DeviceVersion.IsEqual(a, version))
        throw new Exception("Can not write to connected device! Two different firmware versions detected." + Environment.NewLine + " Actual version is: " + a?.ToString() + Environment.NewLine + " Expected version is: " + version?.ToString() + Environment.NewLine);
      if (changedFlashBlocks != null && changedFlashBlocks.Count > 0 && segmentsToErase != null && segmentsToErase.Count > 0)
      {
        foreach (ushort address in segmentsToErase)
        {
          if (!this.MyDeviceCollector.PDCHandler.EraseFLASHSegment(address))
            return false;
        }
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedFlashBlocks)
        {
          if (!this.MyDeviceCollector.PDCHandler.WriteFLASH(keyValuePair.Key, keyValuePair.Value))
            return false;
        }
      }
      if (changedRamBlocks != null && changedRamBlocks.Count > 0)
      {
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedRamBlocks)
        {
          if (!this.MyDeviceCollector.PDCHandler.WriteRAM(keyValuePair.Key, keyValuePair.Value))
            return false;
        }
        if (doRamBackup && !this.MyDeviceCollector.PDCHandler.RunRAMBackup() || doDeviceReset && !this.MyDeviceCollector.PDCHandler.ResetDevice())
          return false;
      }
      return true;
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(int channel)
    {
      return this.GetValues(channel, (List<long>) null);
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(
      int channel,
      List<long> filter)
    {
      return this.WorkMeter == null ? (SortedList<long, SortedList<DateTime, ReadingValue>>) null : this.WorkMeter.GetValues(channel, filter);
    }

    public bool SaveDevice() => this.SaveDevice(out DateTime? _);

    public bool SaveDevice(out DateTime? timepoint)
    {
      timepoint = new DateTime?();
      if (this.WorkMeter == null)
        return false;
      DeviceIdentification deviceIdentification = this.WorkMeter.GetDeviceIdentification();
      if (deviceIdentification == null || !deviceIdentification.IsChecksumOK)
        throw new ArgumentNullException("The 'MeterID' can not be null!");
      if (deviceIdentification.MeterID == 0U || deviceIdentification.MeterID == uint.MaxValue)
        throw new ArgumentException("The 'MeterID' is invalid! Value: " + deviceIdentification.MeterID.ToString(), "MeterID");
      if (deviceIdentification.HardwareTypeID == 0U || deviceIdentification.HardwareTypeID == uint.MaxValue)
        throw new ArgumentException("The 'HardwareTypeID' is invalid! Value: " + deviceIdentification.HardwareTypeID.ToString(), "HardwareTypeID");
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      BaseTables.HardwareTypeRow hardwareType = GmmDbLib.HardwareType.GetHardwareType(baseDbConnection, (int) deviceIdentification.HardwareTypeID);
      if (hardwareType == null)
        throw new ArgumentException("The 'HardwareTypeID' is invalid! This id is not existing in database. Value: " + deviceIdentification.HardwareTypeID.ToString(), "HardwareTypeID");
      if (hardwareType.HardwareName != PDC_DeviceIdentity.PDC_WmBus.ToString())
        throw new ArgumentException("The 'HardwareTypeID' is not the PDC device! DB: " + hardwareType?.ToString(), "HardwareTypeID");
      int meterId = (int) deviceIdentification.MeterID;
      int meterInfoId = (int) deviceIdentification.MeterInfoID;
      uint hardwareTypeId = deviceIdentification.HardwareTypeID;
      string serialnumberFull = this.WorkMeter.GetSerialnumberFull();
      string orderNr = deviceIdentification.SapProductionOrderNumber != 0U ? deviceIdentification.SapProductionOrderNumber.ToString() : string.Empty;
      byte[] deviceMemory = this.WorkMeter.Zip();
      timepoint = Device.Save(baseDbConnection, meterId, meterInfoId, hardwareTypeId, serialnumberFull, orderNr, deviceMemory, true);
      this.BackupMeter = this.WorkMeter.DeepCopy();
      return true;
    }

    public bool OpenDevice(int meterId, DateTime timePoint)
    {
      ZR_ClassLibrary.MeterData meterData = PDC_Database.LoadMeterData(meterId, timePoint);
      return meterData != null && this.OpenDevice(meterData.PValueBinary);
    }

    public bool OpenDevice(byte[] zippedBuffer)
    {
      this.BackupMeter = zippedBuffer != null ? PDC_Meter.Unzip(zippedBuffer) : throw new ArgumentNullException(nameof (zippedBuffer));
      this.WorkMeter = this.BackupMeter.DeepCopy();
      if (this.WorkMeter.DBDeviceInfo != null && this.WorkMeter.DBDeviceInfo.MeterInfo != null)
        this.TypeMeter = PDC_HandlerFunctions.LoadType(this.WorkMeter.DBDeviceInfo.MeterInfo.MeterInfoID);
      return true;
    }

    public bool OpenType(int meterInfoID)
    {
      this.TypeMeter = PDC_HandlerFunctions.LoadType(meterInfoID);
      if (this.TypeMeter == null)
        return false;
      if (this.WorkMeter == null)
        this.WorkMeter = this.TypeMeter.DeepCopy();
      return true;
    }

    internal static PDC_Meter LoadType(int meterInfoID)
    {
      MeterTypeData meterTypeData = PDC_Database.LoadType(meterInfoID);
      return meterTypeData == null ? (PDC_Meter) null : PDC_Meter.Unzip(meterTypeData.EEPdata);
    }

    public bool CreateType(
      IDbCommand cmd,
      string sapMaterialNumber,
      PDC_HardwareIdentification hardwareIdent,
      string typeDescription)
    {
      if (hardwareIdent == null)
        throw new ArgumentNullException(nameof (hardwareIdent));
      if (this.WorkMeter == null)
        throw new ArgumentNullException("WorkMeter");
      DeviceVersion deviceVersion = this.WorkMeter.Version != null ? this.WorkMeter.Version : throw new ArgumentNullException("WorkMeter.Version");
      string key = hardwareIdent.CreateKey();
      return this.CreateType(cmd, sapMaterialNumber, (GmmDbLib.HardwareType.GetHardwareType(DbBasis.PrimaryDB.BaseDbConnection, key, (int) deviceVersion.Version) ?? throw new Exception("The hardware type does not exist in database! Type: " + key + ", Version: " + deviceVersion.Version.ToString())).HardwareTypeID, typeDescription);
    }

    public bool CreateType(
      string sapMaterialNumber,
      PDC_HardwareIdentification hardwareIdent,
      string typeDescription)
    {
      if (hardwareIdent == null)
        return false;
      if (this.WorkMeter == null)
        throw new ArgumentNullException("CreateType: WorkMeter can not be null!");
      if (this.WorkMeter.Version == null)
        throw new ArgumentNullException("CreateType: Version can not be null!");
      string key = hardwareIdent.CreateKey();
      BaseTables.HardwareTypeRow hardwareType = GmmDbLib.HardwareType.GetHardwareType(DbBasis.PrimaryDB.BaseDbConnection, key, (int) this.WorkMeter.Version.Version);
      return hardwareType != null && this.CreateType(sapMaterialNumber, hardwareType.HardwareTypeID, typeDescription);
    }

    public bool CreateType(
      IDbCommand cmd,
      string sapMaterialNumber,
      int hardwareTypeID,
      string typeDescription)
    {
      return PDC_HandlerFunctions.CreateType(cmd, sapMaterialNumber, hardwareTypeID, typeDescription, this.WorkMeter);
    }

    public bool CreateType(string sapMaterialNumber, int hardwareTypeID, string typeDescription)
    {
      return PDC_HandlerFunctions.CreateType(sapMaterialNumber, hardwareTypeID, typeDescription, this.WorkMeter);
    }

    internal static bool CreateType(
      IDbCommand cmd,
      string sapMaterialNumber,
      int hardwareTypeID,
      string typeDescription,
      PDC_Meter meter)
    {
      return PDC_Database.CreateType(cmd, sapMaterialNumber, hardwareTypeID, typeDescription, meter);
    }

    internal static bool CreateType(
      string sapMaterialNumber,
      int hardwareTypeID,
      string typeDescription,
      PDC_Meter meter)
    {
      return PDC_Database.CreateType(sapMaterialNumber, hardwareTypeID, typeDescription, meter);
    }

    public bool Overwrite(OverwritePart parts)
    {
      if (this.TypeMeter == null)
        throw new Exception("Failed overwrite with base type! The 'TypeMeter' is null.");
      if (this.WorkMeter == null)
        throw new Exception("Failed overwrite with base type! The 'WorkMeter' is null.");
      return this.WorkMeter.Overwrite(this.TypeMeter, parts);
    }

    private DeviceVersion ManageIrDaWakeUpAndReadVersion()
    {
      this.MyDeviceCollector.AsyncCom.WakeupTemporaryOff = true;
      int maxRequestRepeat = this.MyDeviceCollector.MaxRequestRepeat;
      try
      {
        this.MyDeviceCollector.MaxRequestRepeat = 1;
        DeviceVersion deviceVersion1 = this.ReadVersion();
        if (deviceVersion1 != null)
          return deviceVersion1;
        this.MyDeviceCollector.AsyncCom.ClearWakeup();
        this.MyDeviceCollector.MaxRequestRepeat = maxRequestRepeat;
        DeviceVersion deviceVersion2 = this.ReadVersion();
        if (deviceVersion2 != null)
          return deviceVersion2;
      }
      finally
      {
        this.MyDeviceCollector.MaxRequestRepeat = maxRequestRepeat;
      }
      return (DeviceVersion) null;
    }

    public DateTime? ReadSystemTime()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadSystemTime();
    }

    public bool WriteSystemTime(DateTime value)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteSystemTime(value);
    }

    public int? ReadMeterValue(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMeterValue(channel);
    }

    public bool WriteMeterValue(byte channel, uint value)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteMeterValue(channel, value);
    }

    public DeviceVersion ReadVersion()
    {
      this.MyDeviceCollector.BreakRequest = false;
      ReadVersionData versionData;
      if (!this.MyDeviceCollector.PDCHandler.ReadVersion(out versionData) || versionData == null)
        return (DeviceVersion) null;
      return new DeviceVersion()
      {
        Version = versionData.Version,
        HardwareTypeID = versionData.HardwareIdentification,
        SvnRevision = versionData.BuildRevision,
        BuildTime = new DateTime?(versionData.BuildTime),
        Signatur = versionData.FirmwareSignature
      };
    }

    internal bool ReadMemory(ushort startAddress, int size, out byte[] buffer)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMemory(startAddress, size, out buffer);
    }

    internal bool WriteMemory(ushort address, byte[] buffer)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteRAM(address, buffer);
    }

    public bool ResetDevice()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ResetDevice();
    }

    private bool UpdateModeEnter()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.UpdateModeEnter();
    }

    private bool UpdateModeExit()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.UpdateModeExit();
    }

    private bool UpdateModeEraseFlash(uint address)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.UpdateModeEraseFlash(address);
    }

    private bool UpdateModeWriteFlash(uint address, byte[] memory_128byte)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.UpdateModeWriteFlash(address, memory_128byte);
    }

    private byte[] UpdateModeReadFlash(uint address, byte count)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.UpdateModeReadFlash(address, count);
    }

    public bool RunRAMBackup()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RunRAMBackup();
    }

    public bool PulseDisable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.PulseDisable();
    }

    public bool PulseEnable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.PulseEnable();
    }

    public uint? ReadPulseSettings()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.PulseSettingsRead();
    }

    public uint? WritePulseSettings(ushort period, byte ontime)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.PulseSettingsWrite(period, ontime);
    }

    public bool SendSND_NKE()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.SendSND_NKE();
    }

    public bool RadioDisable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RadioDisable();
    }

    public bool RadioNormal()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RadioNormal();
    }

    public bool RadioOOK()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RadioDisable() && this.MyDeviceCollector.PDCHandler.RadioOOK();
    }

    public bool RadioOOK(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RadioDisable() && this.MyDeviceCollector.PDCHandler.RadioOOK(mode, offset, timeoutInSeconds);
    }

    public bool RadioPN9()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RadioDisable() && this.MyDeviceCollector.PDCHandler.RadioPN9();
    }

    public bool RadioPN9(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RadioDisable() && this.MyDeviceCollector.PDCHandler.RadioPN9(mode, offset, timeoutInSeconds);
    }

    public bool RadioReceive(
      out RadioPacket packet,
      out byte[] buffer,
      out int rssi_dBm,
      out int lqi,
      uint timeout)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.RadioReceive(out packet, out buffer, out rssi_dBm, out lqi, timeout);
    }

    public bool StartRadioReceiver()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.StartRadioReceiver();
    }

    private void OnProgressAsynchronously(int progress)
    {
      if (this.OnProgress == null)
        return;
      if (this.asyncOperation == null)
        this.asyncOperation = AsyncOperationManager.CreateOperation((object) null);
      this.asyncOperation.Post((SendOrPostCallback) (state =>
      {
        try
        {
          this.OnProgress((object) this, progress);
        }
        catch (Exception ex)
        {
          string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          PDC_HandlerFunctions.logger.ErrorException(message, ex);
        }
      }), (object) null);
    }

    public bool EventLogClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.EventLogClear();
    }

    public bool SystemLogClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.SystemLogClear();
    }

    public string ValidateMeter(PDC_Meter meter)
    {
      return meter == null ? string.Empty : new StringBuilder().ToString();
    }

    public ushort? ReadStatusFlags(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.StatusFlagsRead(channel);
    }

    public ushort? ClearStatusFlags(byte channel, ushort flags)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.StatusFlagsClear(channel, flags);
    }

    public ushort? ReadConfigFlags()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadConfigFlags();
    }

    public ushort? WriteConfigFlags(ushort flags)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteConfigFlags(flags);
    }

    public ushort? ModifyConfigFlags(ushort flags_set, ushort flags_clear)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ModifyConfigFlags(flags_set, flags_clear);
    }

    public byte? ReadRadioFlags()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadRadioFlags();
    }

    public byte? WriteRadioFlags(byte flags)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteRadioFlags(flags);
    }

    public byte? ModifyRadioFlags(byte flags_set, byte flags_clear)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ModifyRadioFlags(flags_set, flags_clear);
    }

    public uint? ReadSerialNumber(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadSerial(channel);
    }

    public uint? WriteSerialNumber(byte channel, uint serial)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteSerial(channel, serial);
    }

    public byte? ReadMBusAddress(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMBusAddress(channel);
    }

    public byte? WriteMBusAddress(byte channel, byte address)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteMBusAddress(channel, address);
    }

    public byte? ReadMBusVersion(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMBusVersion(channel);
    }

    public byte? WriteMBusVersion(byte channel, byte version)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteMBusVersion(channel, version);
    }

    public byte? ReadMBusType(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMBusMedium(channel);
    }

    public byte? WriteMBusType(byte channel, byte medium)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteMBusMedium(channel, medium);
    }

    public ushort? ReadMBusManId(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMBusManid(channel);
    }

    public ushort? WriteMBusManId(byte channel, ushort manid)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteMBusManid(channel, manid);
    }

    public byte? ReadObisCode(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMBusMedium(channel);
    }

    public byte? WriteObisCode(byte channel, byte code)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteObisCode(channel, code);
    }

    public byte? ReadVIF(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadVIF(channel);
    }

    public byte? WriteVIF(byte channel, byte code)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteVIF(channel, code);
    }

    public ushort? ReadMantissa(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMantissa(channel);
    }

    public ushort? WriteMantissa(byte channel, ushort code)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteMantissa(channel, code);
    }

    public sbyte? ReadExponent(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadExponent(channel);
    }

    public sbyte? WriteExponent(byte channel, sbyte code)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteExponent(channel, code);
    }

    public ushort? ReadFlowBlock(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadFlowBlock(channel);
    }

    public ushort? WriteFlowBlock(byte channel, ushort code)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteFlowBlock(channel, code);
    }

    public ulong? ReadFlowLeak(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadFlowLeak(channel);
    }

    public ulong? WriteFlowLeak(
      byte channel,
      ushort leak,
      ushort unleak,
      ushort upper,
      ushort lower)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteFlowLeak(channel, leak, unleak, upper, lower);
    }

    public uint? ReadFlowBurst(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadFlowBurst(channel);
    }

    public uint? WriteFlowBurst(byte channel, ushort diff, ushort limit)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteFlowBurst(channel, diff, limit);
    }

    public uint? ReadFlowOversize(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadFlowOversize(channel);
    }

    public uint? WriteFlowOversize(byte channel, ushort diff, ushort limit)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteFlowOversize(channel, diff, limit);
    }

    public uint? ReadFlowUndersize(byte channel)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadFlowUndersize(channel);
    }

    public uint? WriteFlowUndersize(byte channel, ushort diff, ushort limit)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteFlowUndersize(channel, diff, limit);
    }

    public ushort? ReadKeydate()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadKeyDate();
    }

    public ushort? WriteKeydate(byte month, byte day)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteKeyDate(month, day);
    }

    public byte? ReadRadioList()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadRadioList();
    }

    public byte? WriteRadioList(byte list)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteRadioList(list);
    }

    public uint? QueryRadioList()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ListQuery();
    }

    public bool ResetToDelivery()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.DeliveryState();
    }

    public bool Depassivate()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.Depassivate();
    }

    public uint? ReadDepass()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadDepass();
    }

    public uint? WriteDepass(ushort timeout, ushort period)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteDepass(timeout, period);
    }

    public byte? QueryMBusState()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.ReadMBusStatus();
    }

    public byte? WriteMBusState(byte state)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.PDCHandler.WriteMBusStatus(state);
    }

    public IMeter CreateMeter(byte[] zippedBuffer)
    {
      return zippedBuffer != null ? (IMeter) PDC_Meter.Unzip(zippedBuffer) : throw new ArgumentNullException(nameof (zippedBuffer));
    }

    public void Dispose() => this.GMM_Dispose();
  }
}
