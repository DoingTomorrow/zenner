// Decompiled with JetBrains decompiler
// Type: EDC_Handler.EDC_HandlerFunctions
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using DeviceCollector;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public sealed class EDC_HandlerFunctions : ICreateMeter, IHandlerForEDC_Test, IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (EDC_HandlerFunctions));
    public static readonly DateTime DateTimeNull = new DateTime(2000, 1, 1);
    internal DeviceCollectorFunctions MyDeviceCollector;
    internal EDC_Meter ConnectedMeter;
    internal EDC_Meter WorkMeter;
    internal EDC_Meter TypeMeter;
    internal EDC_Meter BackupMeter;
    private Stopwatch stopwatch;
    private Queue<byte> volumeMonitorQueue;
    private int? maxCountofVolumeMonitorData;
    private List<VolumeMonitorEventArgs> volumeMonitorData;
    private EDC_HandlerWindow MyWindow;
    private AsyncOperation asyncOperation = (AsyncOperation) null;

    public EDC_HandlerFunctions()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      if (!ZR_Component.CommonGmmInterface.LoadedComponentsList.ContainsKey(GMM_Components.DeviceCollector))
        return;
      this.MyDeviceCollector = ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector] as DeviceCollectorFunctions;
    }

    public EDC_HandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.MyDeviceCollector = deviceCollector as DeviceCollectorFunctions;
    }

    public EDC_Meter Meter => this.WorkMeter;

    public event ValueEventHandler<int> OnProgress;

    public event VolumeMonitorEventHandler OnVolumeMonitorDataReceived;

    public string ShowHandlerWindow()
    {
      this.MyWindow = new EDC_HandlerWindow(this);
      this.MyWindow.BringToFront();
      int num = (int) this.MyWindow.ShowDialog();
      string nextComponentName = this.MyWindow.NextComponentName;
      this.MyWindow.Dispose();
      return nextComponentName;
    }

    public void GMM_Dispose()
    {
      if (this.MyDeviceCollector == null)
        return;
      this.MyDeviceCollector.ComClose();
      this.MyDeviceCollector.Dispose();
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
      EDC_HandlerFunctions.logger.Debug("Clear all values in handler.");
      this.ConnectedMeter = (EDC_Meter) null;
      this.WorkMeter = (EDC_Meter) null;
      this.BackupMeter = (EDC_Meter) null;
      this.TypeMeter = (EDC_Meter) null;
    }

    public List<VolumeMonitorEventArgs> StartVolumeMonitor(int count)
    {
      this.MyDeviceCollector.BreakRequest = false;
      this.maxCountofVolumeMonitorData = new int?(count);
      try
      {
        if (!this.StartVolumeMonitor())
          return (List<VolumeMonitorEventArgs>) null;
        if (this.volumeMonitorData != null && this.volumeMonitorData.Count > count)
          this.volumeMonitorData.RemoveRange(count, this.volumeMonitorData.Count - count);
        return this.volumeMonitorData;
      }
      finally
      {
        this.maxCountofVolumeMonitorData = new int?();
      }
    }

    public bool StartVolumeMonitor()
    {
      ZR_ClassLibMessages.ClearErrors();
      DeviceVersion deviceVersion = this.ManageIrDaWakeUpAndReadVersion();
      if (deviceVersion == null || deviceVersion.Type == EDC_Hardware.EDC_Radio && !this.RadioDisable())
        return false;
      this.stopwatch = new Stopwatch();
      if (this.maxCountofVolumeMonitorData.HasValue)
        this.volumeMonitorData = new List<VolumeMonitorEventArgs>();
      this.volumeMonitorQueue = new Queue<byte>();
      short num1 = 0;
      ushort num2 = 0;
      ushort num3 = 0;
      if (!this.MyDeviceCollector.EDCHandler.StartVolumeMonitor())
        return false;
      this.stopwatch.Reset();
      this.stopwatch.Start();
      while (true)
      {
        do
        {
          byte[] buffer;
          do
          {
            if (!this.maxCountofVolumeMonitorData.HasValue || this.maxCountofVolumeMonitorData.Value > this.volumeMonitorData.Count)
            {
              if (!this.stopwatch.IsRunning || this.stopwatch.ElapsedMilliseconds > 5000L)
                goto label_9;
            }
            else
              goto label_7;
          }
          while (!this.MyDeviceCollector.AsyncCom.GetCurrentInputBuffer(out buffer) || buffer == null);
          for (int index = 0; index < buffer.Length; ++index)
            this.volumeMonitorQueue.Enqueue(buffer[index]);
        }
        while (this.volumeMonitorQueue.Count < 12);
        bool flag = false;
        while (!flag)
        {
          while (this.volumeMonitorQueue.Count > 0 && this.volumeMonitorQueue.Peek() != (byte) 2 && !flag)
          {
            byte num4 = this.volumeMonitorQueue.Dequeue();
            if (EDC_HandlerFunctions.logger.IsWarnEnabled)
              EDC_HandlerFunctions.logger.Warn("Synchronise... 0x" + num4.ToString("X2"));
          }
          if (this.volumeMonitorQueue.Count != 0 && this.volumeMonitorQueue.Count >= 12)
          {
            int num5 = (int) this.volumeMonitorQueue.Dequeue();
            string empty = string.Empty;
            byte num6;
            byte num7;
            byte num8;
            byte num9;
            byte num10;
            try
            {
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num6 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num7 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num8 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num9 = Convert.ToByte(empty, 16);
              empty = Encoding.ASCII.GetString(new byte[2]
              {
                this.volumeMonitorQueue.Dequeue(),
                this.volumeMonitorQueue.Dequeue()
              }, 0, 2);
              num10 = Convert.ToByte(empty, 16);
            }
            catch (Exception ex)
            {
              EDC_HandlerFunctions.logger.Error("Error: {0}, VolumeMonitorQueue.Count: {1}, Last ASCII: {2}, " + Environment.NewLine + " Trace: {3}", new object[4]
              {
                (object) ex.Message,
                (object) this.volumeMonitorQueue.Count,
                (object) empty,
                (object) ex.StackTrace
              });
              if (flag)
                return this.MyDeviceCollector.EDCHandler.StopVolumeMonitor_SendE5();
              this.stopwatch.Reset();
              this.stopwatch.Start();
              break;
            }
            if (this.volumeMonitorQueue.Dequeue() == (byte) 3 && (int) (byte) ((uint) num6 + (uint) num7 + (uint) num8 + (uint) num9) == (int) num10)
            {
              VolumeMonitorEventArgs e = new VolumeMonitorEventArgs();
              e.CoilDetectionResult_A = num6;
              e.CoilDetectionResult_B = num7;
              e.StateMachineValue = num8;
              e.PollCounter = num9;
              switch (e.CoilSampleResult)
              {
                case 1:
                  ++num1;
                  break;
                case 2:
                  --num1;
                  ++num2;
                  break;
                case 3:
                  ++num3;
                  break;
                case 4:
                  --num1;
                  ++num2;
                  break;
                case 6:
                  ++num3;
                  break;
                case 7:
                  ++num1;
                  break;
                case 8:
                  ++num1;
                  break;
                case 9:
                  ++num3;
                  break;
                case 11:
                  --num1;
                  ++num2;
                  break;
                case 12:
                  ++num3;
                  break;
                case 13:
                  --num1;
                  ++num2;
                  break;
                case 14:
                  ++num1;
                  break;
              }
              e.PulseForwardCount = num1;
              e.PulseReturnCount = num2;
              e.PulseErrorCount = num3;
              if (this.maxCountofVolumeMonitorData.HasValue)
                this.volumeMonitorData.Add(e);
              if (this.OnVolumeMonitorDataReceived != null)
                this.OnVolumeMonitorDataReceived((object) this, e);
              flag = e.Cancel;
              if (flag)
                return this.MyDeviceCollector.EDCHandler.StopVolumeMonitor_SendE5();
              this.stopwatch.Reset();
              this.stopwatch.Start();
            }
            else
              break;
          }
          else
            break;
        }
      }
label_7:
      return this.MyDeviceCollector.EDCHandler.StopVolumeMonitor_SendE5();
label_9:
      ZR_ClassLibMessages.AddErrorDescription("Timeout 5000 ms", EDC_HandlerFunctions.logger);
      return this.MyDeviceCollector.EDCHandler.StopVolumeMonitor_SendE5();
    }

    public bool StopVolumeMonitor() => this.MyDeviceCollector.EDCHandler.StopVolumeMonitor_SendE5();

    public void ClearVolumeMonitorData()
    {
      if (this.volumeMonitorQueue != null)
        this.volumeMonitorQueue.Clear();
      if (this.volumeMonitorData == null)
        return;
      this.volumeMonitorData.Clear();
    }

    public List<VolumeMonitorEventArgs> GetVolumeMonitorData() => this.volumeMonitorData;

    public DateTime SaveVolumeMonitorData(int meterID, List<VolumeMonitorEventArgs> data)
    {
      if (data == null || data.Count == 0)
        throw new Exception("No volume monitor data to save!");
      List<byte> byteList = new List<byte>();
      foreach (VolumeMonitorEventArgs monitorEventArgs in data)
        byteList.AddRange(monitorEventArgs.ToByteArray());
      byte[] zippedBuffer = Util.Zip(byteList.ToArray());
      return GmmDbLib.MeterData.InsertData(DbBasis.PrimaryDB.BaseDbConnection, meterID, GmmDbLib.MeterData.Special.EdcEncabulator, zippedBuffer);
    }

    public bool UpgradeFirmware(byte[] firmware)
    {
      if (firmware == null)
        throw new ArgumentNullException("Firmware can not be null!");
      if (this.OnProgress != null)
        this.OnProgress((object) this, 1);
      DeviceVersion deviceVersion = this.ManageIrDaWakeUpAndReadVersion();
      if (deviceVersion != null && deviceVersion.Type == EDC_Hardware.EDC_Radio)
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
          EDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 1 of 5");
          if (!this.UpdateFirmwareBlock(firmware, address))
          {
            Thread.Sleep(100);
            EDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 2 of 5");
            if (!this.UpdateFirmwareBlock(firmware, address))
            {
              Thread.Sleep(100);
              EDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 3 of 5");
              if (!this.UpdateFirmwareBlock(firmware, address))
              {
                Thread.Sleep(100);
                EDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 4 of 5");
                if (!this.UpdateFirmwareBlock(firmware, address))
                {
                  EDC_HandlerFunctions.logger.Warn("Failed to update 512 bytes! Attempt 5 of 5");
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
        EDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 1 of 4");
        if (!this.UpdateModeEraseFlash(address))
        {
          Thread.Sleep(100);
          EDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 2 of 4");
          if (!this.UpdateModeEraseFlash(address))
          {
            Thread.Sleep(100);
            EDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 3 of 4");
            if (!this.UpdateModeEraseFlash(address))
            {
              EDC_HandlerFunctions.logger.Warn("Failed to erase 512 bytes! Attempt 4 of 4");
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
          EDC_HandlerFunctions.logger.Warn("Failed to write 128 bytes! Attempt 1 of 2");
          if (!this.UpdateModeWriteFlash(index1, numArray1))
          {
            EDC_HandlerFunctions.logger.Warn("Failed to write 128 bytes! Attempt 2 of 2");
            return false;
          }
        }
        byte[] numArray2 = this.UpdateModeReadFlash(index1, (byte) 128);
        if (numArray2 == null)
        {
          Thread.Sleep(100);
          EDC_HandlerFunctions.logger.Warn("Failed to read 128 bytes! Attempt 1 of 3");
          numArray2 = this.UpdateModeReadFlash(index1, (byte) 128);
          if (numArray2 == null)
          {
            Thread.Sleep(100);
            EDC_HandlerFunctions.logger.Warn("Failed to read 128 bytes! Attempt 2 of 3");
            numArray2 = this.UpdateModeReadFlash(index1, (byte) 128);
            if (numArray2 == null)
            {
              EDC_HandlerFunctions.logger.Warn("Can not read 128 bytes. Attempt 3 of 3");
              return false;
            }
          }
        }
        for (int index2 = 0; index2 < numArray2.Length; ++index2)
        {
          if ((int) numArray1[index2] != (int) numArray2[index2])
          {
            EDC_HandlerFunctions.logger.Warn("Corrupt block detected! Write error.");
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
      RangeSet<ushort> rangeSet = version.Type == EDC_Hardware.EDC_Radio || version.Type == EDC_Hardware.EDC_mBus || version.Type == EDC_Hardware.EDC_ModBus || version.Type == EDC_Hardware.EDC_mBus_Modbus || version.Type == EDC_Hardware.EDC_mBus_CJ188 || version.Type == EDC_Hardware.EDC_RS485_Modbus || version.Type == EDC_Hardware.EDC_RS485_CJ188 ? EDC_MemoryMap.GetMemoryBlocksToRead(version, readLoggerToo) : throw new Exception(Ot.Gtt(Tg.Common, "NotEDC1", "Connected device is not EDC"));
      if (rangeSet == null)
        throw new NotSupportedException("This firmware version is not supported! Version: " + version.VersionString);
      if (version.Type == EDC_Hardware.EDC_mBus || version.Type == EDC_Hardware.EDC_ModBus || version.Type == EDC_Hardware.EDC_mBus_Modbus || version.Type == EDC_Hardware.EDC_mBus_CJ188 || version.Type == EDC_Hardware.EDC_RS485_Modbus || version.Type == EDC_Hardware.EDC_RS485_CJ188)
        this.ReadMeterValue();
      EDC_MemoryMap map = new EDC_MemoryMap();
      int num = 0;
      foreach (Range<ushort> range in (List<Range<ushort>>) rangeSet)
      {
        byte[] buffer;
        if (!this.ReadMemory(range.Start, (int) range.End + 1 - (int) range.Start, out buffer))
          return false;
        map.SetMemoryBytes(range.Start, buffer);
        if (this.OnProgress != null)
        {
          int progress = 100;
          if (rangeSet.IndexOf(range) != rangeSet.Count - 1)
            progress = Convert.ToInt32((double) num++ / (double) rangeSet.Count * 100.0);
          this.OnProgressAsynchronously(progress);
        }
      }
      this.ConnectedMeter = !map.IsEmpty() ? new EDC_Meter(map) : throw new Exception("The MAP is empty! Version:" + version.VersionString);
      this.ConnectedMeter.Version = version;
      DeviceIdentification deviceIdentification = this.ConnectedMeter.GetDeviceIdentification();
      if (deviceIdentification != null && deviceIdentification.IsChecksumOK)
      {
        this.ConnectedMeter.DBDeviceInfo.MeterInfo = EDC_Database.GetMeterInfo(deviceIdentification.MeterInfoID);
        this.ConnectedMeter.DBDeviceInfo.HardwareType = MeterDatabase.GetHardwareType((int) version.HardwareTypeID);
        try
        {
          this.TypeMeter = this.LoadType((int) deviceIdentification.MeterInfoID);
        }
        catch (Exception ex)
        {
          EDC_HandlerFunctions.logger.Fatal("Can not load type! Error: " + ex.Message);
        }
      }
      this.WorkMeter = this.ConnectedMeter.DeepCopy();
      return true;
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
          if (!this.MyDeviceCollector.EDCHandler.EraseFLASHSegment(address))
            return false;
        }
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedFlashBlocks)
        {
          if (!this.MyDeviceCollector.EDCHandler.WriteFLASH(keyValuePair.Key, keyValuePair.Value))
            return false;
        }
      }
      if (changedRamBlocks != null && changedRamBlocks.Count > 0)
      {
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedRamBlocks)
        {
          if (!this.MyDeviceCollector.EDCHandler.WriteRAM(keyValuePair.Key, keyValuePair.Value))
            return false;
        }
        if (doRamBackup)
        {
          int num = 3;
          bool flag3 = false;
          while (this.MyDeviceCollector.EDCHandler.RunRAMBackup())
          {
            Thread.Sleep(500);
            byte[] buffer;
            this.ReadMemory((ushort) 6144, 128, out buffer);
            if ((int) BitConverter.ToUInt16(buffer, 126) != (int) Util.CalculatesCRC16_CC430(buffer, 0, buffer.Length - 2))
            {
              EDC_HandlerFunctions.logger.Fatal("The CRC of the configuration parameter is wrong! FLASH Info D Buffer: " + Util.ByteArrayToHexString(buffer));
              EDC_HandlerFunctions.logger.Fatal("Try again!");
            }
            else
              flag3 = true;
            --num;
            if (flag3 && num > 0)
            {
              if (!flag3)
              {
                EDC_HandlerFunctions.logger.Fatal("Save config does not work!");
                return false;
              }
              goto label_42;
            }
          }
          return false;
        }
label_42:
        if (doDeviceReset && !this.MyDeviceCollector.EDCHandler.ResetDevice())
          return false;
      }
      return true;
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
      if (!(hardwareType.HardwareName == EDC_Hardware.EDC_Radio.ToString()) && !(hardwareType.HardwareName == EDC_Hardware.EDC_mBus.ToString()) && !(hardwareType.HardwareName == EDC_Hardware.EDC_ModBus.ToString()) && !(hardwareType.HardwareName == EDC_Hardware.EDC_mBus_Modbus.ToString()) && !(hardwareType.HardwareName == EDC_Hardware.EDC_mBus_CJ188.ToString()) && !(hardwareType.HardwareName == EDC_Hardware.EDC_RS485_Modbus.ToString()) && !(hardwareType.HardwareName == EDC_Hardware.EDC_RS485_CJ188.ToString()))
        throw new ArgumentException("The 'HardwareTypeID' is not the EDC device! DB: " + hardwareType?.ToString(), "HardwareTypeID");
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
      MeterData meterData = EDC_Database.LoadMeterData(meterId, timePoint);
      return meterData != null && this.OpenDevice(meterData.Buffer);
    }

    public bool OpenDevice(byte[] zippedBuffer)
    {
      this.BackupMeter = zippedBuffer != null ? EDC_Meter.Unzip(zippedBuffer) : throw new ArgumentNullException(nameof (zippedBuffer));
      this.WorkMeter = this.BackupMeter.DeepCopy();
      if (this.WorkMeter.DBDeviceInfo != null && this.WorkMeter.DBDeviceInfo.MeterInfo != null)
        this.TypeMeter = this.LoadType(this.WorkMeter.DBDeviceInfo.MeterInfo.MeterInfoID);
      return true;
    }

    public bool OpenType(int meterInfoID)
    {
      this.TypeMeter = this.LoadType(meterInfoID);
      if (this.TypeMeter == null)
        return false;
      if (this.WorkMeter == null)
        this.WorkMeter = this.TypeMeter.DeepCopy();
      return true;
    }

    internal EDC_Meter LoadType(int meterInfoID)
    {
      MeterTypeData meterTypeData = EDC_Database.LoadType(meterInfoID);
      if (meterTypeData == null)
        return (EDC_Meter) null;
      try
      {
        return EDC_Meter.Unzip(meterTypeData.EEPdata);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to open base type! MeterInfoID = " + meterInfoID.ToString() + " " + Environment.NewLine + ex.Message, ex);
      }
    }

    public bool CreateType(
      IDbCommand cmd,
      string sapMaterialNumber,
      EDC_HardwareIdentification hardwareIdent,
      string typeDescription)
    {
      if (hardwareIdent == null)
        return false;
      if (this.WorkMeter == null)
        throw new ArgumentNullException("CreateType: WorkMeter can not be null!");
      DeviceVersion deviceVersion = this.WorkMeter.Version != null ? this.WorkMeter.Version : throw new ArgumentNullException("CreateType: Version can not be null!");
      string key = hardwareIdent.CreateKey();
      return this.CreateType(cmd, sapMaterialNumber, (GmmDbLib.HardwareType.GetHardwareType(DbBasis.PrimaryDB.BaseDbConnection, key, (int) deviceVersion.Version) ?? throw new Exception("Can not find the hardware type! Firmware version: " + deviceVersion?.ToString() + " (" + deviceVersion.Version.ToString() + "), Hardware Ident Key: " + key)).HardwareTypeID, typeDescription);
    }

    public bool CreateType(
      string sapMaterialNumber,
      EDC_HardwareIdentification hardwareIdent,
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
      return this.CreateType(sapMaterialNumber, hardwareTypeID, typeDescription, this.WorkMeter);
    }

    public bool CreateType(string sapMaterialNumber, int hardwareTypeID, string typeDescription)
    {
      return this.CreateType(sapMaterialNumber, hardwareTypeID, typeDescription, this.WorkMeter);
    }

    internal bool CreateType(
      IDbCommand cmd,
      string sapMaterialNumber,
      int hardwareTypeID,
      string typeDescription,
      EDC_Meter meter)
    {
      return EDC_Database.CreateType(cmd, sapMaterialNumber, hardwareTypeID, typeDescription, meter);
    }

    internal bool CreateType(
      string sapMaterialNumber,
      int hardwareTypeID,
      string typeDescription,
      EDC_Meter meter)
    {
      return EDC_Database.CreateType(sapMaterialNumber, hardwareTypeID, typeDescription, meter);
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
      return this.MyDeviceCollector.EDCHandler.ReadSystemTime();
    }

    public bool WriteSystemTime(DateTime value)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteSystemTime(value);
    }

    public uint? ReadMeterValue()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.ReadMeterValue();
    }

    public bool WriteMeterValue(uint value)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteMeterValue(value);
    }

    public DeviceVersion ReadVersion()
    {
      this.MyDeviceCollector.BreakRequest = false;
      ReadVersionData versionData;
      if (!this.MyDeviceCollector.EDCHandler.ReadVersion(out versionData) || versionData == null)
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
      return this.MyDeviceCollector.EDCHandler.ReadMemory(startAddress, size, out buffer);
    }

    internal bool WriteMemory(ushort address, byte[] buffer)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteRAM(address, buffer);
    }

    public bool ResetDevice()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.ResetDevice();
    }

    private bool UpdateModeEnter()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.UpdateModeEnter();
    }

    private bool UpdateModeExit()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.UpdateModeExit();
    }

    private bool UpdateModeEraseFlash(uint address)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.UpdateModeEraseFlash(address);
    }

    private bool UpdateModeWriteFlash(uint address, byte[] memory_128byte)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.UpdateModeWriteFlash(address, memory_128byte);
    }

    private byte[] UpdateModeReadFlash(uint address, byte count)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.UpdateModeReadFlash(address, count);
    }

    public bool RunRAMBackup()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RunRAMBackup();
    }

    public bool PulseDisable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.PulseDisable();
    }

    public bool PulseEnable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.PulseEnable();
    }

    public bool StartDepassivation()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.StartDepassivation();
    }

    public bool SendSND_NKE()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.SendSND_NKE();
    }

    public bool RadioDisable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RadioDisable();
    }

    public bool RadioNormal()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RadioNormal();
    }

    public bool RadioOOK()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RadioDisable() && this.MyDeviceCollector.EDCHandler.RadioOOK();
    }

    public bool RadioOOK(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RadioDisable() && this.MyDeviceCollector.EDCHandler.RadioOOK(mode, offset, timeoutInSeconds);
    }

    public bool RadioPN9()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RadioDisable() && this.MyDeviceCollector.EDCHandler.RadioPN9();
    }

    public bool RadioPN9(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RadioDisable() && this.MyDeviceCollector.EDCHandler.RadioPN9(mode, offset, timeoutInSeconds);
    }

    public bool RadioReceive(
      out RadioPacket packet,
      out byte[] buffer,
      out int rssi_dBm,
      out int lqi,
      uint timeout)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RadioReceive(out packet, out buffer, out rssi_dBm, out lqi, timeout);
    }

    public bool StartRadioReceiver()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.StartRadioReceiver();
    }

    public bool WritePulseoutQueue(short value, bool clearQueue)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WritePulseoutQueue(value, clearQueue);
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
          EDC_HandlerFunctions.logger.ErrorException(message, ex);
        }
      }), (object) null);
    }

    public bool EventLogClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.EventLogClear();
    }

    public bool SystemLogClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.SystemLogClear();
    }

    public bool RemovalFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.RemovalFlagClear();
    }

    public bool TamperFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.TamperFlagClear();
    }

    public bool BackflowFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.BackflowFlagClear();
    }

    public bool LeakFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.LeakFlagClear();
    }

    public bool BlockFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.BlockFlagClear();
    }

    public bool OversizeFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.OversizeFlagClear();
    }

    public bool UndersizeFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.UndersizeFlagClear();
    }

    public bool BurstFlagClear()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.BurstFlagClear();
    }

    public bool LogClearAndDisableLog()
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.LogClearAndDisableLog();
    }

    public bool LogEnable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      DeviceVersion deviceVersion = this.ManageIrDaWakeUpAndReadVersion();
      if (deviceVersion == null)
        return false;
      if (deviceVersion.Type == EDC_Hardware.EDC_Radio)
        return this.MyDeviceCollector.EDCHandler.LogEnable();
      if (deviceVersion.Type != EDC_Hardware.EDC_mBus && deviceVersion.Type != EDC_Hardware.EDC_ModBus && deviceVersion.Type != EDC_Hardware.EDC_mBus_Modbus && deviceVersion.Type != EDC_Hardware.EDC_mBus_CJ188 && deviceVersion.Type != EDC_Hardware.EDC_RS485_Modbus && deviceVersion.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotImplementedException("LogEnable()");
      ushort? nullable = this.MyDeviceCollector.EDCHandler.ReadConfigFlags();
      return nullable.HasValue && this.MyDeviceCollector.EDCHandler.WriteConfigFlags((ushort) ((int) nullable.Value | 4));
    }

    public bool LogDisable()
    {
      this.MyDeviceCollector.BreakRequest = false;
      DeviceVersion deviceVersion = this.ManageIrDaWakeUpAndReadVersion();
      if (deviceVersion == null)
        return false;
      if (deviceVersion.Type == EDC_Hardware.EDC_Radio)
        return this.MyDeviceCollector.EDCHandler.LogDisable();
      if (deviceVersion.Type != EDC_Hardware.EDC_mBus && deviceVersion.Type != EDC_Hardware.EDC_ModBus && deviceVersion.Type != EDC_Hardware.EDC_mBus_Modbus && deviceVersion.Type != EDC_Hardware.EDC_mBus_CJ188 && deviceVersion.Type != EDC_Hardware.EDC_RS485_Modbus && deviceVersion.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotImplementedException("LogDisable()");
      ushort? nullable = this.MyDeviceCollector.EDCHandler.ReadConfigFlags();
      return nullable.HasValue && this.MyDeviceCollector.EDCHandler.WriteConfigFlags((ushort) ((int) nullable.Value & 65531));
    }

    public string ValidateMeter(EDC_Meter meter)
    {
      return meter == null ? string.Empty : new StringBuilder().ToString();
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> GetValues()
    {
      return this.GetValues((List<long>) null);
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(List<long> filter)
    {
      return this.WorkMeter == null ? (SortedList<long, SortedList<DateTime, ReadingValue>>) null : this.WorkMeter.GetValues(filter);
    }

    public IMeter CreateMeter(byte[] zippedBuffer)
    {
      return zippedBuffer != null ? (IMeter) EDC_Meter.Unzip(zippedBuffer) : throw new ArgumentNullException(nameof (zippedBuffer));
    }

    public void Dispose() => this.GMM_Dispose();

    public uint? ReadSerialnumber(ID id)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.ReadSerialnumber((byte) id);
    }

    public bool WriteSerialnumber(ID id, uint serial)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteSerialnumber((byte) id, serial);
    }

    public byte? ReadAddress(ID id)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.ReadAddress((byte) id);
    }

    public bool WriteAddress(ID id, byte address)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteAddress((byte) id, address);
    }

    public byte? ReadGeneration(ID id)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.ReadGeneration((byte) id);
    }

    public bool WriteGeneration(ID id, byte generation)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteGeneration((byte) id, generation);
    }

    public MBusDeviceType? ReadMedium(ID id)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return new MBusDeviceType?((MBusDeviceType) Enum.ToObject(typeof (MBusDeviceType), (object) this.MyDeviceCollector.EDCHandler.ReadMedium((byte) id)));
    }

    public bool WriteMedium(ID id, MBusDeviceType medium)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteMedium((byte) id, (byte) medium);
    }

    public string ReadManufacturer(ID id)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return MBusDevice.GetManufacturer((short) this.MyDeviceCollector.EDCHandler.ReadManufacturer((byte) id).Value);
    }

    public bool WriteManufacturer(ID id, string manufacturer)
    {
      if (string.IsNullOrEmpty(manufacturer))
        return false;
      ushort manufacturerCode = MBusDevice.GetManufacturerCode(manufacturer);
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteManufacturer((byte) id, manufacturerCode);
    }

    public byte? ReadObis(ID id)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.ReadObis((byte) id);
    }

    public bool WriteObis(ID id, byte obis)
    {
      this.MyDeviceCollector.BreakRequest = false;
      return this.MyDeviceCollector.EDCHandler.WriteObis((byte) id, obis);
    }

    public DeviceIdentification GetDeviceIdentification()
    {
      return this.WorkMeter.GetDeviceIdentification();
    }

    public Warning? GetWarnings() => this.GetWarnings();

    public string GetSerialnumberFull() => this.WorkMeter.GetSerialnumberFull();

    public bool SetWarnings(Warning value) => this.SetWarnings(value);

    public bool SetRemovalDetectionState(bool isDisabled)
    {
      return this.WorkMeter.SetRemovalDetectionState(isDisabled);
    }

    public ushort? GetSensorTimeout() => this.WorkMeter.GetSensorTimeout();

    public ushort? GetPulsePeriod() => this.WorkMeter.GetPulsePeriod();

    public sbyte? GetCoilErrorThreshold() => this.WorkMeter.GetCoilErrorThreshold();

    public sbyte? GetCoilAmplitudeLimit() => this.WorkMeter.GetCoilAmplitudeLimit();

    public bool? GetFlowCheckIntervalState() => this.WorkMeter.GetFlowCheckIntervalState();

    public bool? GetDataLoggingState() => this.WorkMeter.GetDataLoggingState();

    public HardwareError? GetHardwareErrors() => this.GetHardwareErrors();

    public bool? GetRadioState() => this.WorkMeter.GetRadioState();

    public sbyte? GetCoilB_offset() => this.WorkMeter.GetCoilB_offset();

    public DeviceVersion Version { get; set; }

    public uint? GetSerialnumberPrimary() => this.WorkMeter.GetSerialnumberPrimary();

    public byte? GetCogCount() => this.WorkMeter.GetCogCount();

    public bool? GetRemovalDetectionState() => this.WorkMeter.GetRemovalDetectionState();

    public bool? GetMagnetDetectionState() => this.WorkMeter.GetMagnetDetectionState();

    public bool? GetCoilSampling() => this.WorkMeter.GetCoilSampling();

    public byte? GetPulseActivateRadio() => this.WorkMeter.GetPulseActivateRadio();

    public ushort? GetDepassPeriod() => this.WorkMeter.GetDepassPeriod();

    public bool SetFlowCheckIntervalState(bool enable)
    {
      return this.WorkMeter.SetFlowCheckIntervalState(enable);
    }

    public bool SetSensorTimeout(ushort seconds) => this.WorkMeter.SetSensorTimeout(seconds);

    public bool SetRadioState(bool enable) => this.WorkMeter.SetRadioState(enable);

    public bool SetDepassPeriod(ushort value) => this.WorkMeter.SetDepassPeriod(value);

    public bool SetFrequencyOffset(short value) => this.WorkMeter.SetFrequencyOffset(value);

    public bool SetPulseActivateRadio(byte value) => this.WorkMeter.SetPulseActivateRadio(value);

    public bool SetCoilAmplitudeLimit(sbyte value) => this.WorkMeter.SetCoilAmplitudeLimit(value);

    public short? GetFrequencyOffset() => this.WorkMeter.GetFrequencyOffset();

    public int? GetMeterValue() => this.WorkMeter.GetMeterValue();

    public bool SetDataLoggingState(bool enable) => this.WorkMeter.SetDataLoggingState(enable);

    public bool SetCoilSampling(bool enable) => this.WorkMeter.SetCoilSampling(enable);

    public bool SetWMBusEncryptionState(bool enable)
    {
      return this.WorkMeter.SetWMBusEncryptionState(enable);
    }

    public bool SetBatteryEndDate(DateTime value) => this.WorkMeter.SetBatteryEndDate(value);

    public bool SetStartModule(DateTime value) => this.WorkMeter.SetStartModule(value);

    public bool SetCoilMinThreshold(sbyte value) => this.WorkMeter.SetCoilMinThreshold(value);

    public bool SetCoilMaxThreshold(sbyte value) => this.WorkMeter.SetCoilMaxThreshold(value);

    public bool SetSerialnumberSecondary(uint value)
    {
      return this.WorkMeter.SetSerialnumberSecondary(value);
    }

    public bool SetObisSecondary(byte value) => this.WorkMeter.SetObisSecondary(value);

    public byte? GetMBusGenerationPrimary() => this.WorkMeter.GetMBusGenerationPrimary();

    public bool SetMBusGenerationSecondary(byte value)
    {
      return this.WorkMeter.SetMBusGenerationSecondary(value);
    }

    public byte? GetMBusGenerationSecondary() => this.WorkMeter.GetMBusGenerationSecondary();

    public byte? GetObisPrimary() => this.WorkMeter.GetObisPrimary();

    public bool SetCoilB_offset(sbyte value) => this.WorkMeter.SetCoilB_offset(value);

    public bool SetStartMeter(DateTime value) => this.WorkMeter.SetStartMeter(value);

    public DatabaseDeviceInfo DBDeviceInfo { get; set; }

    public bool SetDeviceIdentification(DeviceIdentification ident)
    {
      return this.WorkMeter.SetDeviceIdentification(ident);
    }

    public bool SetAESkey(object value) => this.WorkMeter.SetAESkey(value);

    public bool SetMagnetDetectionState(bool enable)
    {
      return this.WorkMeter.SetMagnetDetectionState(enable);
    }

    public bool SetCogCount(byte value) => this.WorkMeter.SetCogCount(value);

    public bool? GetWMBusSynchronousTransmissioModeState()
    {
      return this.WorkMeter.GetWMBusSynchronousTransmissioModeState();
    }

    public bool? GetWMBusInstallationPacketsState()
    {
      return this.WorkMeter.GetWMBusInstallationPacketsState();
    }

    public ushort? GetDepassTimeout() => this.WorkMeter.GetDepassTimeout();

    public MbusBaud? GetMbusBaud() => this.WorkMeter.GetMbusBaud();

    public PulseoutMode? GetPulseoutMode() => this.WorkMeter.GetPulseoutMode();

    public string GetMBusListType() => this.WorkMeter.GetMBusListType();

    public MBusDeviceType? GetMediumSecondary() => this.WorkMeter.GetMediumSecondary();

    public string GetManufacturerPrimary() => this.WorkMeter.GetManufacturerPrimary();

    public string GetManufacturerSecondary() => this.WorkMeter.GetManufacturerSecondary();

    public bool SetObisPrimary(byte value) => this.WorkMeter.SetObisPrimary(value);

    public byte? GetObisSecondary() => this.WorkMeter.GetObisSecondary();

    public bool SetMBusGenerationPrimary(byte value)
    {
      return this.WorkMeter.SetMBusGenerationPrimary(value);
    }

    public bool SetDepassTimeout(ushort value) => this.WorkMeter.SetDepassTimeout(value);

    public bool SetRadioMode(RadioMode type) => this.WorkMeter.SetRadioMode(type);

    public bool SetRadioPower(RadioPower value) => this.WorkMeter.SetRadioPower(value);

    public bool SetRadioTransmitInterval(ushort interval)
    {
      return this.WorkMeter.SetRadioTransmitInterval(interval);
    }

    public bool SetPulseMultiplier(byte value) => this.WorkMeter.SetPulseMultiplier(value);

    public RadioMode? GetRadioMode() => this.WorkMeter.GetRadioMode();

    public RadioPower? GetRadioPower() => this.WorkMeter.GetRadioPower();

    public bool SetPulseoutMode(PulseoutMode value) => this.WorkMeter.SetPulseoutMode(value);

    public bool SetPulseoutWidth(ushort value) => this.WorkMeter.SetPulseoutWidth(value);

    public bool SetSerialnumberPrimary(uint value) => this.WorkMeter.SetSerialnumberPrimary(value);

    public bool SetSerialnumberFull(string value) => this.WorkMeter.SetSerialnumberFull(value);

    public RuntimeFlags? GetRuntimeFlags() => this.WorkMeter.GetRuntimeFlags();

    public bool SetHardwareErrors(HardwareError value) => this.WorkMeter.SetHardwareErrors(value);

    public bool SetParameterValue<T>(string parameterName, T newValue)
    {
      return this.WorkMeter.SetParameterValue<T>(parameterName, newValue);
    }
  }
}
