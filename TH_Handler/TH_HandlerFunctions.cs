// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH_HandlerFunctions
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using AsyncCom;
using DeviceCollector;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public sealed class TH_HandlerFunctions : ICreateMeter, IReadoutConfig
  {
    private static Logger logger = LogManager.GetLogger(nameof (TH_HandlerFunctions));

    internal TH_Meter ConnectedMeter { get; set; }

    internal TH_Meter BackupMeter { get; set; }

    public TH CMD { get; set; }

    public IDeviceCollector DeviceCollector { get; set; }

    public TH_Meter WorkMeter { get; set; }

    public event ValueEventHandler<int> OnProgress;

    public TH_HandlerFunctions()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      if (ZR_Component.CommonGmmInterface.LoadedComponentsList.ContainsKey(GMM_Components.DeviceCollector))
        this.DeviceCollector = ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector] as IDeviceCollector;
      if (this.DeviceCollector != null)
        this.CMD = new TH(this.DeviceCollector as DeviceCollectorFunctions);
      else
        this.CMD = new TH(new DeviceCollectorFunctions((IAsyncFunctions) new AsyncFunctions(true)));
    }

    public TH_HandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.DeviceCollector = deviceCollector;
      if (this.DeviceCollector != null)
        this.CMD = new TH(this.DeviceCollector as DeviceCollectorFunctions);
      else
        this.CMD = new TH(new DeviceCollectorFunctions((IAsyncFunctions) new AsyncFunctions(true)));
    }

    public bool ReadDevice() => this.ReadDevice(false);

    public bool ReadDevice(bool readLoggerToo)
    {
      if (this.OnProgress != null)
        this.OnProgress((object) this, 5);
      TH_Version version = this.CMD.ManageIrDaWakeUpAndReadVersion();
      if (version == null)
        return false;
      if (this.OnProgress != null)
        this.OnProgress((object) this, 10);
      TH_MemoryMap map = version.TypeValue == (ushort) 22 ? TH_MemoryMap.Create(version) : throw new Exception(Ot.Gtm(Tg.Handler_UI, "DeviceNotSupportet", "Device not supported.") + " " + version?.ToString());
      RangeSet<ushort> memoryBlocksToRead = map.GetMemoryBlocksToRead(readLoggerToo);
      int num = 0;
      foreach (Range<ushort> range in (List<Range<ushort>>) memoryBlocksToRead)
      {
        byte[] buffer = this.CMD.ReadMemory(range.Start, (int) range.End + 1 - (int) range.Start);
        if (buffer == null)
          return false;
        map.SetMemoryBytes(range.Start, buffer);
        if (this.OnProgress != null)
        {
          int e = 100;
          if (memoryBlocksToRead.IndexOf(range) != memoryBlocksToRead.Count - 1)
            e = Convert.ToInt32((double) num++ / (double) memoryBlocksToRead.Count * 100.0);
          this.OnProgress((object) this, e);
        }
      }
      this.ConnectedMeter = new TH_Meter(map);
      this.WorkMeter = this.ConnectedMeter.DeepCopy();
      return true;
    }

    public bool WriteDevice()
    {
      if (this.WorkMeter == null)
        return false;
      TH_Version a = this.CMD.ManageIrDaWakeUpAndReadVersion();
      if (a == null)
        return false;
      SortedList<ushort, byte[]> changedRamBlocks = this.WorkMeter.Map.GetChangedRamBlocks(this.ConnectedMeter);
      List<ushort> segmentsToErase;
      SortedList<ushort, byte[]> changedFlashBlocks = this.WorkMeter.Map.GetChangedFlashBlocks(this.ConnectedMeter, out segmentsToErase);
      bool flag1 = changedRamBlocks != null && changedRamBlocks.Count > 0;
      bool flag2 = changedFlashBlocks != null && changedFlashBlocks.Count > 0 && segmentsToErase != null && segmentsToErase.Count > 0;
      if (!flag1 && !flag2)
        return true;
      TH_Version version = this.WorkMeter.Map.Version;
      if (!TH_Version.IsEqual(a, version))
        throw new Exception(Ot.Gtm(Tg.HandlerLogic, "CanNotWriteToDifferentFirmwareVersion", "Can not write to connected device! Two different firmware versions detected.") + " " + a?.ToString() + " <> " + version?.ToString());
      if (changedFlashBlocks != null && changedFlashBlocks.Count > 0 && segmentsToErase != null && segmentsToErase.Count > 0)
      {
        foreach (ushort startAddress in segmentsToErase)
        {
          if (!this.CMD.EraseFLASHSegment(startAddress))
            return false;
        }
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedFlashBlocks)
        {
          if (!this.CMD.WriteFLASH(keyValuePair.Key, keyValuePair.Value))
            return false;
        }
      }
      if (changedRamBlocks != null && changedRamBlocks.Count > 0)
      {
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedRamBlocks)
        {
          if (!this.CMD.WriteRAM(keyValuePair.Key, keyValuePair.Value))
            return false;
        }
        int num = 3;
        bool flag3 = false;
        while (this.CMD.SaveConfig())
        {
          Thread.Sleep(500);
          byte[] buffer = this.CMD.ReadMemory((ushort) 6144, 128);
          if (buffer != null)
          {
            if ((int) BitConverter.ToUInt16(buffer, 126) != (int) Util.CalculatesCRC16_CC430(buffer, 0, buffer.Length - 2))
            {
              TH_HandlerFunctions.logger.Fatal("The CRC of the configuration parameter is wrong! FLASH Info D Buffer: " + Util.ByteArrayToHexString(buffer));
              TH_HandlerFunctions.logger.Fatal("Try again!");
            }
            else
              flag3 = true;
            --num;
          }
          if (flag3 && num > 0)
          {
            if (!flag3)
            {
              TH_HandlerFunctions.logger.Fatal("Save configuration does not work!");
              return false;
            }
            if (!this.CMD.ResetDevice())
              return false;
            goto label_45;
          }
        }
        return false;
      }
label_45:
      return true;
    }

    public bool SaveDevice() => this.SaveDevice(out DateTime? _);

    public bool SaveDevice(out DateTime? timepoint)
    {
      timepoint = new DateTime?();
      TH_DeviceIdentification deviceIdentification = this.WorkMeter != null ? this.WorkMeter.GetDeviceIdentification() : throw new Exception(Ot.Gtm(Tg.HandlerLogic, "NoDataForBackup", "No data for backup."));
      uint? nullable;
      int num1;
      if (deviceIdentification != null)
      {
        nullable = deviceIdentification.MeterID;
        if (nullable.HasValue)
        {
          nullable = deviceIdentification.MeterID;
          uint num2 = 0;
          if (!((int) nullable.GetValueOrDefault() == (int) num2 & nullable.HasValue))
          {
            nullable = deviceIdentification.MeterID;
            uint maxValue = uint.MaxValue;
            num1 = (int) nullable.GetValueOrDefault() == (int) maxValue & nullable.HasValue ? 1 : 0;
            goto label_7;
          }
        }
      }
      num1 = 1;
label_7:
      if (num1 != 0)
        throw new ArgumentException(Ot.Gtm(Tg.HandlerLogic, "InvalidMeterID", "MeterID is invalid."));
      nullable = deviceIdentification.HardwareTypeID;
      int num3;
      if (nullable.HasValue)
      {
        nullable = deviceIdentification.HardwareTypeID;
        uint num4 = 0;
        if (!((int) nullable.GetValueOrDefault() == (int) num4 & nullable.HasValue))
        {
          nullable = deviceIdentification.HardwareTypeID;
          uint maxValue = uint.MaxValue;
          num3 = (int) nullable.GetValueOrDefault() == (int) maxValue & nullable.HasValue ? 1 : 0;
          goto label_13;
        }
      }
      num3 = 1;
label_13:
      if (num3 != 0)
        throw new ArgumentException(Ot.Gtm(Tg.HandlerLogic, "InvalidHardwareTypeID", "HardwareTypeID is invalid."));
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      BaseDbConnection db = baseDbConnection;
      nullable = deviceIdentification.HardwareTypeID;
      int hardwareTypeID = (int) nullable.Value;
      BaseTables.HardwareTypeRow hardwareType = GmmDbLib.HardwareType.GetHardwareType(db, hardwareTypeID);
      if (hardwareType == null)
      {
        string str1 = Ot.Gtm(Tg.HandlerLogic, "HardwareTypeIDNotExistsInDB", "HardwareTypeID is not existing in the database. ");
        nullable = deviceIdentification.HardwareTypeID;
        string str2 = nullable.ToString();
        throw new ArgumentException(str1 + " " + str2);
      }
      if (hardwareType.HardwareName != "TH")
        throw new ArgumentException(Ot.Gtm(Tg.HandlerLogic, "WrongHardwareName", "Wrong hardware name.") + " " + hardwareType.HardwareName);
      nullable = deviceIdentification.MeterID;
      int meterID = (int) nullable.Value;
      nullable = deviceIdentification.MeterInfoID;
      int meterInfoID = (int) nullable.Value;
      nullable = deviceIdentification.HardwareTypeID;
      uint hardwareTypeID_OR_firmwareVersion = nullable.Value;
      string fullserialnumber = deviceIdentification.Fullserialnumber;
      nullable = deviceIdentification.SapProductionOrderNumber;
      uint num5 = 0;
      string empty;
      if ((int) nullable.GetValueOrDefault() == (int) num5 & nullable.HasValue)
      {
        empty = string.Empty;
      }
      else
      {
        nullable = deviceIdentification.SapProductionOrderNumber;
        empty = nullable.ToString();
      }
      string orderNr = empty;
      byte[] deviceMemory = this.WorkMeter.Map.Zip();
      timepoint = Device.Save(baseDbConnection, meterID, meterInfoID, hardwareTypeID_OR_firmwareVersion, fullserialnumber, orderNr, deviceMemory, true);
      this.BackupMeter = this.WorkMeter.DeepCopy();
      return true;
    }

    public bool OpenDevice(int meterId, DateTime timePoint)
    {
      BaseTables.MeterDataRow meterData = GmmDbLib.MeterData.GetMeterData(DbBasis.PrimaryDB.BaseDbConnection, meterId, timePoint);
      return meterData != null && this.OpenDevice(meterData.PValueBinary);
    }

    public bool OpenDevice(byte[] zippedBuffer)
    {
      this.BackupMeter = zippedBuffer != null ? TH_Meter.Unzip(zippedBuffer) : throw new ArgumentNullException(nameof (zippedBuffer));
      this.WorkMeter = this.BackupMeter.DeepCopy();
      return this.WorkMeter != null;
    }

    public void ClearAllData()
    {
      this.DeviceCollector.BreakRequest = false;
      TH_HandlerFunctions.logger.Debug("Clear all values in handler.");
      this.ConnectedMeter = (TH_Meter) null;
      this.WorkMeter = (TH_Meter) null;
      this.BackupMeter = (TH_Meter) null;
    }

    public IMeter CreateMeter(byte[] zippedBuffer)
    {
      return zippedBuffer == null ? (IMeter) null : (IMeter) TH_Meter.Unzip(zippedBuffer);
    }

    public void SetBackupMeter(IMeter backupMeter)
    {
    }

    public void Dispose()
    {
      this.ClearAllData();
      if (this.DeviceCollector == null)
        return;
      this.DeviceCollector.Dispose();
      this.DeviceCollector.GMM_Dispose();
    }

    public void SetReadoutConfiguration(ConfigList configList)
    {
      if (configList == null)
        throw new ArgumentNullException(nameof (configList));
      if (this.DeviceCollector == null)
        throw new ArgumentNullException("DeviceCollector");
      SortedList<string, string> sortedList = configList.GetSortedList();
      this.DeviceCollector.SetDeviceCollectorSettings(sortedList);
      this.DeviceCollector.SetAsyncComSettings(sortedList);
    }

    public ConfigList GetReadoutConfiguration() => (ConfigList) null;
  }
}
