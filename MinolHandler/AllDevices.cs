// Decompiled with JetBrains decompiler
// Type: MinolHandler.AllDevices
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  public class AllDevices
  {
    private static Logger logger = LogManager.GetLogger(nameof (AllDevices));
    private MinolHandlerFunctions MyFunctions;
    internal SortedList<string, ByteField> ReadTelegram;
    internal SortedList<string, List<TelegramParameter>> AllTelegramParameter;
    public SortedList<string, ParameterAccess> ParameterAccessByName;
    internal SortedList<int, ParameterAccess> ParameterAccessByAddress;
    internal SortedList<long, SortedList<string, ParameterAccess>> ParameterAccessByValueIdent;
    internal SortedList<string, ParameterAccess> BitAccessByBitName;

    public virtual event EventHandlerEx<int> OnProgress;

    internal MinolDevice ReadDevice { get; private set; }

    public MinolDevice WorkDevice { get; private set; }

    internal MinolDevice DbDevice { get; private set; }

    internal MinolDevice TypeDevice { get; private set; }

    internal AllDevices(MinolHandlerFunctions MyFunctions) => this.MyFunctions = MyFunctions;

    internal bool ReadAndCreateDevice() => this.ReadAndCreateDevice(false, out DateTime _);

    internal bool ReadAndCreateDevice(ReadMode mode)
    {
      return this.ReadAndCreateDevice(mode, false, out DateTime _);
    }

    internal bool ReadAndCreateDevice(bool saveToDatabase, out DateTime backupTimePoint)
    {
      return this.ReadAndCreateDevice(ReadMode.Complete, saveToDatabase, out backupTimePoint);
    }

    internal bool ReadAndCreateDevice(
      ReadMode mode,
      bool saveToDatabase,
      out DateTime backupTimePoint)
    {
      backupTimePoint = DateTime.MinValue;
      try
      {
        if (this.OnProgress != null)
          this.OnProgress((object) this, 10);
        if (!this.MyFunctions.MyCom.Open())
          return false;
        if (this.OnProgress != null)
          this.OnProgress((object) this, 20);
        this.ReadTelegram = new SortedList<string, ByteField>();
        ByteField MemoryData;
        if (!this.MyFunctions.MyBus.ReadMemory(MemoryLocation.RAM, 0, (int) byte.MaxValue, out MemoryData, true))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read RAM error!");
          return false;
        }
        this.ReadTelegram.Add("RAM", MemoryData);
        int Length = (int) MemoryData.Data[1];
        int Version = (int) MemoryData.Data[13];
        int Signature_RD_Data = (int) MemoryData.Data[17] + ((int) MemoryData.Data[18] << 8);
        string DeviceName;
        int HardwareTypeID;
        int MapID;
        if (!this.MyFunctions.MyDatabaseAccess.GetTelegramParameters(Signature_RD_Data, Version, Length, out DeviceName, out HardwareTypeID, out MapID, out this.AllTelegramParameter))
        {
          string str = "Device not found in database! Signatur: 0x" + Signature_RD_Data.ToString("X04") + " Length: 0x" + Length.ToString("X02") + " Version: 0x" + Version.ToString("X02");
          AllDevices.logger.Error(str);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, str);
          return false;
        }
        if (this.AllTelegramParameter.ContainsKey("INFO") && !this.ReadFrom(MemoryLocation.FLASH, "INFO", 1, false))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read INFO error!");
          return false;
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 40);
        if ((mode & ReadMode.LoggerMonth) == ReadMode.LoggerMonth)
        {
          if (this.AllTelegramParameter.ContainsKey("LOGM") && !this.ReadFrom(MemoryLocation.RAM, "LOGM", 2, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGM error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGM_A") && !this.ReadFrom(MemoryLocation.RAM, "LOGM_A", 2, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGM_A error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGM_B") && !this.ReadFrom(MemoryLocation.RAM, "LOGM_B", 3, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGM_B error!");
            return false;
          }
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 50);
        if ((mode & ReadMode.LoggerDay) == ReadMode.LoggerDay && this.AllTelegramParameter.ContainsKey("LOGD") && !this.ReadFrom(MemoryLocation.RAM, "LOGD", 4, false))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGD error!");
          return false;
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 60);
        if ((mode & ReadMode.LoggerHour) == ReadMode.LoggerHour)
        {
          if (this.AllTelegramParameter.ContainsKey("LOGH0") && !this.ReadFrom(MemoryLocation.RAM, "LOGH0", 6, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGH0 error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGH1") && !this.ReadFrom(MemoryLocation.RAM, "LOGH1", 7, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGH1 error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGH2") && !this.ReadFrom(MemoryLocation.RAM, "LOGH2", 8, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGH2 error!");
            return false;
          }
        }
        this.CreateParameterAccessLists();
        if (this.OnProgress != null)
          this.OnProgress((object) this, 70);
        this.ReadDevice = this.CreateMinolDevice(DeviceName, HardwareTypeID, MapID);
        this.ReadDevice.Signature = Signature_RD_Data;
        this.ReadDevice.Manufacturer = BitConverter.ToUInt16(MemoryData.Data, 11);
        if (this.ReadDevice == null)
          return false;
        if ((this.ReadDevice.DeviceType == DeviceTypes.AquaMicroRadio3 || this.ReadDevice.DeviceType == DeviceTypes.EHCA_M6_Radio3 || this.ReadDevice.DeviceType == DeviceTypes.MinotelContactRadio3) && !UserManager.CheckPermission(UserRights.Rights.Radio3))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
          return false;
        }
        this.ReadDevice.FirmwareVersion = MemoryData.Data[13];
        if (!this.ReadDevice.WriteMapDataFromTelegram(MemoryLocation.RAM, this.AllTelegramParameter["RAM"], MemoryData))
          return false;
        if (this.AllTelegramParameter.ContainsKey("INFO") && !this.CreateMapData(MemoryLocation.FLASH, "INFO", 1, false))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read INFO error!");
          return false;
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 80);
        if ((mode & ReadMode.LoggerMonth) == ReadMode.LoggerMonth)
        {
          if (this.AllTelegramParameter.ContainsKey("LOGM") && !this.CreateMapData(MemoryLocation.RAM, "LOGM", 2, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGM error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGM_A") && !this.CreateMapData(MemoryLocation.RAM, "LOGM_A", 2, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGM_A error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGM_B") && !this.CreateMapData(MemoryLocation.RAM, "LOGM_B", 3, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGM_B error!");
            return false;
          }
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 85);
        if ((mode & ReadMode.LoggerDay) == ReadMode.LoggerDay && this.AllTelegramParameter.ContainsKey("LOGD") && !this.CreateMapData(MemoryLocation.RAM, "LOGD", 4, false))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGD error!");
          return false;
        }
        if (this.OnProgress != null)
          this.OnProgress((object) this, 90);
        if ((mode & ReadMode.LoggerHour) == ReadMode.LoggerHour)
        {
          if (this.AllTelegramParameter.ContainsKey("LOGH0") && !this.CreateMapData(MemoryLocation.RAM, "LOGH0", 6, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGH0 error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGH1") && !this.CreateMapData(MemoryLocation.RAM, "LOGH1", 7, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGH1 error!");
            return false;
          }
          if (this.AllTelegramParameter.ContainsKey("LOGH2") && !this.CreateMapData(MemoryLocation.RAM, "LOGH2", 8, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read LOGH2 error!");
            return false;
          }
        }
        if (!this.ReadDevice.CreateDeviceData())
          return false;
        if (this.OnProgress != null)
          this.OnProgress((object) this, 95);
        if (this.ReadDevice.DeviceType == DeviceTypes.EHCA_M5)
        {
          object readValue = this.ReadDevice.GetReadValue("RAM;SerNo");
          if (readValue != null)
            this.ReadDevice.SerialNumber = Convert.ToInt32(readValue.ToString());
        }
        else
        {
          long? int64 = Util.ConvertBcdInt64ToInt64(MemoryData.Data[10], MemoryData.Data[9], MemoryData.Data[8], MemoryData.Data[7]);
          if (int64.HasValue)
            this.ReadDevice.SerialNumber = (int) int64.Value;
        }
        this.TryReadMeterIDOfISF();
        if (saveToDatabase && !this.MyFunctions.MyDatabaseAccess.Save(this.ReadDevice, out backupTimePoint))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Can not save the device data!");
          return false;
        }
        this.WorkDevice = this.ReadDevice.DeepCopy();
        if (this.OnProgress != null)
          this.OnProgress((object) this, 100);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, ex.Message);
        return false;
      }
    }

    private bool ReadFrom(
      MemoryLocation location,
      string memoryName,
      int mbusAddress,
      bool useOnlyLongWakeUpSequence)
    {
      ByteField MemoryData;
      if (!this.MyFunctions.MyBus.ReadMemory(location, mbusAddress, (int) byte.MaxValue, out MemoryData, useOnlyLongWakeUpSequence))
        return false;
      this.ReadTelegram.Add(memoryName, MemoryData);
      return true;
    }

    private bool CreateMapData(
      MemoryLocation location,
      string memoryName,
      int mbusAddress,
      bool useOnlyLongWakeUpSequence)
    {
      ByteField ResceivedData = this.ReadTelegram[memoryName];
      return this.ReadDevice.WriteMapDataFromTelegram(location, this.AllTelegramParameter[memoryName], ResceivedData);
    }

    private void CreateParameterAccessLists()
    {
      this.ParameterAccessByName = new SortedList<string, ParameterAccess>();
      this.BitAccessByBitName = new SortedList<string, ParameterAccess>();
      for (int index1 = 0; index1 < this.AllTelegramParameter.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.AllTelegramParameter.Values[index1].Count; ++index2)
        {
          TelegramParameter telegramParameter = this.AllTelegramParameter.Values[index1][index2];
          if (telegramParameter.Address > 0)
          {
            if (telegramParameter.Type == "BIT" && telegramParameter.Parent.Length > 0)
            {
              ParameterAccess parameterAccess = new ParameterAccess()
              {
                TelegramPara = telegramParameter,
                RangeName = this.AllTelegramParameter.Keys[index1]
              };
              parameterAccess.Name = parameterAccess.RangeName + ";" + parameterAccess.TelegramPara.Name;
              if (!this.BitAccessByBitName.ContainsKey(parameterAccess.Name))
                this.BitAccessByBitName.Add(parameterAccess.Name, parameterAccess);
            }
            else
            {
              ParameterAccess parameterAccess = new ParameterAccess()
              {
                TelegramPara = telegramParameter,
                RangeName = this.AllTelegramParameter.Keys[index1]
              };
              parameterAccess.Name = parameterAccess.RangeName + ";" + parameterAccess.TelegramPara.Name;
              if (!this.ParameterAccessByName.ContainsKey(parameterAccess.Name))
                this.ParameterAccessByName.Add(parameterAccess.Name, parameterAccess);
            }
          }
        }
      }
      this.ParameterAccessByAddress = new SortedList<int, ParameterAccess>();
      this.ParameterAccessByValueIdent = new SortedList<long, SortedList<string, ParameterAccess>>();
      foreach (ParameterAccess parameterAccess in (IEnumerable<ParameterAccess>) this.ParameterAccessByName.Values)
      {
        if (parameterAccess.TelegramPara.ByteLength > 0 && !this.ParameterAccessByAddress.ContainsKey(parameterAccess.TelegramPara.Address))
          this.ParameterAccessByAddress.Add(parameterAccess.TelegramPara.Address, parameterAccess);
        if (parameterAccess.TelegramPara.ValueIdent >= 0L)
        {
          int index = this.ParameterAccessByValueIdent.IndexOfKey(parameterAccess.TelegramPara.ValueIdent);
          if (index < 0)
          {
            SortedList<string, ParameterAccess> sortedList = new SortedList<string, ParameterAccess>();
            this.ParameterAccessByValueIdent.Add(parameterAccess.TelegramPara.ValueIdent, sortedList);
            index = this.ParameterAccessByValueIdent.IndexOfKey(parameterAccess.TelegramPara.ValueIdent);
          }
          this.ParameterAccessByValueIdent.Values[index].Add(parameterAccess.Name, parameterAccess);
        }
      }
      foreach (ParameterAccess parameterAccess1 in (IEnumerable<ParameterAccess>) this.BitAccessByBitName.Values)
      {
        string key = parameterAccess1.RangeName + ";" + parameterAccess1.TelegramPara.Parent;
        if (this.ParameterAccessByName.ContainsKey(key))
        {
          ParameterAccess parameterAccess2 = this.ParameterAccessByName[key];
          parameterAccess1.BitAccesParent = parameterAccess2;
        }
        else
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Bit not found: " + parameterAccess1.Name + " Key: " + key);
      }
    }

    public MinolDevice CreateMinolDevice(string deviceName, int hardwareTypeID, int mapID)
    {
      string str = deviceName;
      if (str != null)
      {
        MinolDevice minolDevice;
        switch (str.Length)
        {
          case 2:
            switch (str[1])
            {
              case '5':
                if (str == "M5")
                {
                  minolDevice = (MinolDevice) new M5(this.MyFunctions);
                  break;
                }
                goto label_24;
              case '6':
                if (str == "M6")
                {
                  minolDevice = (MinolDevice) new M6(this.MyFunctions);
                  break;
                }
                goto label_24;
              default:
                goto label_24;
            }
            break;
          case 3:
            switch (str[0])
            {
              case 'I':
                if (str == "ISF")
                {
                  minolDevice = (MinolDevice) new ISF(this.MyFunctions);
                  break;
                }
                goto label_24;
              case 'M':
                if (str == "M5+")
                {
                  minolDevice = (MinolDevice) new M5p(this.MyFunctions);
                  break;
                }
                goto label_24;
              default:
                goto label_24;
            }
            break;
          case 8:
            if (str == "M6Radio3")
            {
              minolDevice = (MinolDevice) new M6Radio3(this.MyFunctions);
              break;
            }
            goto label_24;
          case 11:
            if (str == "MinotelAqua")
            {
              minolDevice = (MinolDevice) new Aqua(this.MyFunctions);
              break;
            }
            goto label_24;
          case 14:
            if (str == "MinotelContact")
            {
              minolDevice = (MinolDevice) new MinotelContact(this.MyFunctions);
              break;
            }
            goto label_24;
          case 16:
            if (str == "MinotelAquaMicro")
            {
              minolDevice = (MinolDevice) new AquaMicro(this.MyFunctions);
              break;
            }
            goto label_24;
          case 18:
            if (str == "MinotelMicroRadio3")
            {
              minolDevice = (MinolDevice) new AquaMicroRadio3(this.MyFunctions);
              break;
            }
            goto label_24;
          case 20:
            if (str == "MinotelContactRadio3")
            {
              minolDevice = (MinolDevice) new MinotelContactRadio3(this.MyFunctions);
              break;
            }
            goto label_24;
          default:
            goto label_24;
        }
        minolDevice.HardwareTypeID = hardwareTypeID;
        minolDevice.MapID = mapID;
        return minolDevice;
      }
label_24:
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "Device not supported! The name of unknown device is " + deviceName);
      return (MinolDevice) null;
    }

    internal bool WriteDevice(bool saveToDatabase, out DateTime backupTimePoint)
    {
      backupTimePoint = DateTime.MinValue;
      if (this.ReadDevice == null)
        return false;
      if (this.WorkDevice.DeviceType == DeviceTypes.ISF)
      {
        ISF_TestStationData testStationData = ISF_TestStationData.GetTestStationData(this.MyFunctions.MyDevices.WorkDevice.Map);
        int num;
        if (testStationData != null)
        {
          int? meterId = testStationData.MeterID;
          if (meterId.HasValue)
          {
            meterId = testStationData.MeterID;
            if (meterId.HasValue)
            {
              meterId = testStationData.MeterID;
              if (meterId.Value != 0)
                goto label_9;
            }
            else
              goto label_9;
          }
          if (this.MyFunctions.MyDevices.WorkDevice.MeterID.HasValue)
          {
            num = this.MyFunctions.MyDevices.WorkDevice.MeterID.Value != 0 ? 1 : 0;
            goto label_10;
          }
        }
label_9:
        num = 0;
label_10:
        if (num != 0)
          ISF_TestStationData.SetMeterIDToISF(this.MyFunctions.MyDevices.WorkDevice);
      }
      try
      {
        Dictionary<MemoryLocation, Dictionary<int, byte[]>> writeParameters = this.WorkDevice.GenerateWriteParameters();
        if (writeParameters.ContainsKey(MemoryLocation.RAM) && !this.WriteToRAM(writeParameters))
          return false;
        if (writeParameters.ContainsKey(MemoryLocation.FLASH))
        {
          if (!this.WriteToFLASH(writeParameters))
            return false;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Failed write the device! Error: " + ex.Message);
        return false;
      }
      finally
      {
        this.ReadDevice = (MinolDevice) null;
      }
      this.ReadDevice = this.WorkDevice.DeepCopy();
      for (int index = 0; index < this.ReadDevice.Map.Length; ++index)
      {
        if (((uint) this.ReadDevice.Map[index] & 256U) <= 0U)
          this.ReadDevice.Map[index] = (short) ((int) this.ReadDevice.Map[index] & 65023);
      }
      this.WorkDevice = this.ReadDevice.DeepCopy();
      return !saveToDatabase || this.ReadAndCreateDevice(saveToDatabase, out backupTimePoint);
    }

    private bool WriteToRAM(
      Dictionary<MemoryLocation, Dictionary<int, byte[]>> requestSets)
    {
      foreach (KeyValuePair<MemoryLocation, Dictionary<int, byte[]>> requestSet in requestSets)
      {
        if (requestSet.Key == MemoryLocation.RAM && !this.WriteTo(requestSet.Key, requestSet.Value))
          return false;
      }
      return true;
    }

    private bool WriteToFLASH(
      Dictionary<MemoryLocation, Dictionary<int, byte[]>> requestSets)
    {
      if (!this.MyFunctions.MyBus.EraseFlash(0, 0))
        return false;
      foreach (KeyValuePair<MemoryLocation, Dictionary<int, byte[]>> requestSet in requestSets)
      {
        if (requestSet.Key == MemoryLocation.FLASH && !this.WriteTo(requestSet.Key, requestSet.Value))
          return false;
      }
      return true;
    }

    private bool WriteTo(MemoryLocation location, Dictionary<int, byte[]> requests)
    {
      foreach (KeyValuePair<int, byte[]> request in requests)
      {
        int key = request.Key;
        ByteField data = new ByteField(request.Value);
        if (!this.MyFunctions.MyBus.WriteMemory(location, key, data))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Failed write to " + location.ToString() + "!");
          return false;
        }
      }
      return true;
    }

    internal bool LoadFromDatabase(int MeterId, DateTime TimePoint)
    {
      this.DbDevice = this.MyFunctions.MyDatabaseAccess.Load(MeterId, TimePoint);
      if (this.DbDevice == null)
        return false;
      if (!this.MyFunctions.MyDatabaseAccess.GetTelegramParameters(this.DbDevice.MapID, out string _, out int _, out this.AllTelegramParameter))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "GetTelegramParameters from DB is failed!");
        return false;
      }
      this.CreateParameterAccessLists();
      for (int index = 0; index < this.DbDevice.Map.Length; ++index)
      {
        if (((int) this.DbDevice.Map[index] & 256) == 0)
          this.DbDevice.Map[index] |= (short) 512;
      }
      this.WorkDevice = this.DbDevice.DeepCopy();
      return true;
    }

    public bool LoadTypeFromDatabase(int MeterInfoID)
    {
      this.TypeDevice = this.MyFunctions.MyDatabaseAccess.LoadType(MeterInfoID);
      if (this.TypeDevice == null)
        return false;
      if (!this.MyFunctions.MyDatabaseAccess.GetTelegramParameters(this.TypeDevice.MapID, out string _, out int _, out this.AllTelegramParameter))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "GetTelegramParameters from DB is failed!");
        return false;
      }
      this.CreateParameterAccessLists();
      this.WorkDevice = this.TypeDevice.DeepCopy();
      return true;
    }

    internal bool SaveToDatabase(out DateTime backupTimePoint)
    {
      return this.MyFunctions.MyDatabaseAccess.Save(this.WorkDevice, out backupTimePoint);
    }

    internal bool SaveToDatabase(
      int meterInfoID,
      string OrderNumber,
      string TheSerialNumber,
      out DateTime backupTimePoint)
    {
      return this.MyFunctions.MyDatabaseAccess.Save(this.WorkDevice, out backupTimePoint, new int?(meterInfoID), OrderNumber, TheSerialNumber);
    }

    public bool SaveTypeToDatabase(int MeterTypeID)
    {
      return this.MyFunctions.MyDatabaseAccess.SaveType(this.WorkDevice, MeterTypeID);
    }

    internal void SetWorkDevice(MinolDevice workDevice) => this.WorkDevice = workDevice;

    internal bool Merge(AddressRange addressRange, MinolDevice deviceToMerge)
    {
      if (this.WorkDevice == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Merge operation failed! The MinolDevice.WorkDevice is null!");
        return false;
      }
      if (deviceToMerge == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Merge operation failed! The MinolDevice.DeviceToMerge is null!");
        return false;
      }
      if (deviceToMerge.GetType() != this.WorkDevice.GetType())
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Merge operation failed! The MinolDevice.DeviceToMerge is not equal to MinolDevice.WorkDevice!");
        return false;
      }
      RangeSet<int> addresses = deviceToMerge.GetAddresses(addressRange);
      for (int start = addresses.Start; start < addresses.End; ++start)
      {
        if (addresses.Contains(start))
          this.WorkDevice.SetMapValue(start, deviceToMerge.Map[start]);
      }
      return true;
    }

    internal bool Merge(AddressRange mergeMode, short[] map) => throw new NotImplementedException();

    private void TryReadMeterIDOfISF()
    {
      if (this.ReadDevice.DeviceType != DeviceTypes.ISF)
        return;
      ISF_TestStationData testStationData = ISF_TestStationData.GetTestStationData(this.ReadDevice.Map);
      if (testStationData != null && !testStationData.MeterID.HasValue)
        this.ReadDevice.MeterID = new int?();
      else if (testStationData != null && testStationData.MeterID.HasValue)
        this.ReadDevice.MeterID = testStationData.MeterID;
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "ISF hat keinen gültigen Datensatz von Löt- und Pulsprüfplatz (L&P)!");
    }
  }
}
