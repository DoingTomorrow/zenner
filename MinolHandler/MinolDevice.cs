// Decompiled with JetBrains decompiler
// Type: MinolHandler.MinolDevice
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  public class MinolDevice
  {
    private static Logger logger = LogManager.GetLogger("MinolHandler.MinolDevice");
    private const int MapSize = 20480;
    internal byte FirmwareVersion;
    internal int SerialNumber;
    internal int HardwareTypeID;
    internal int MapID;
    internal int Signature;
    internal ushort Manufacturer;
    internal int? MeterID;
    internal DeviceTypes DeviceType;
    internal MinolHandlerFunctions MyFunctions;
    protected ReadingValueState DeviceState;
    protected SortedList<OverrideID, ConfigurationParameter> ParameterList;

    public short[] Map { get; private set; }

    internal GlobalDeviceId SelectedDevice { get; set; }

    internal MinolDevice(MinolHandlerFunctions Functions)
    {
      this.MyFunctions = Functions;
      this.DeviceState = ReadingValueState.error;
    }

    internal void SetMap(short[] map) => this.Map = map;

    internal bool WriteMapDataFromTelegram(
      MemoryLocation memoryLocation,
      List<TelegramParameter> TeleParamList,
      ByteField ResceivedData)
    {
      if (this.Map == null)
      {
        this.Map = new short[20480];
        for (int index = 0; index < this.Map.Length; ++index)
          this.Map[index] |= (short) 256;
      }
      int num1 = 0;
      foreach (TelegramParameter teleParam in TeleParamList)
      {
        if (teleParam.Name == "STOP")
        {
          if (num1 == ResceivedData.Count - 1)
            return true;
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Protocol length error");
          return false;
        }
        if (ResceivedData.Data.Length < num1 + teleParam.ByteLength)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Protocol length error");
          return false;
        }
        if (teleParam.Address >= 0)
        {
          for (int index = 0; index < teleParam.ByteLength; ++index)
          {
            short num2 = (short) ResceivedData.Data[num1 + index];
            short num3 = 0;
            if (memoryLocation == MemoryLocation.FLASH)
              num3 |= (short) 1024;
            short num4 = (short) ((int) num2 | (int) num3);
            this.Map[teleParam.Address + index] = num4;
          }
        }
        num1 += teleParam.ByteLength;
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Protocol end not found");
      return false;
    }

    internal string GetParameterDataHexString(string ParameterName)
    {
      if (!this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(ParameterName))
        throw new ArgumentException("Unknown parameter detected: " + ParameterName);
      return this.MyFunctions.MyDevices.ParameterAccessByName[ParameterName].GetParameterMapDataHex(this.Map);
    }

    internal virtual RangeSet<int> GetAddresses(AddressRange range)
    {
      throw new NotImplementedException();
    }

    internal virtual bool CreateDeviceData() => throw new NotImplementedException();

    internal virtual GlobalDeviceId GetGlobalDeviceId() => throw new NotImplementedException();

    internal virtual object GetReadValue(string parameterKey)
    {
      throw new NotImplementedException();
    }

    internal virtual bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      throw new NotImplementedException();
    }

    internal virtual bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      throw new NotImplementedException();
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType)
    {
      return this.GetConfigurationParameters(ConfigurationType, 0);
    }

    internal virtual SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      throw new NotImplementedException();
    }

    internal virtual int GetMaxSizeOfRequestBuffer() => throw new NotImplementedException();

    internal virtual bool SetTestParameter(TestParameter testParameter)
    {
      throw new NotImplementedException();
    }

    internal DataTable GetParameterTable()
    {
      if (this.MyFunctions.MyDevices.ParameterAccessByName == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "The call the function GetParameterTable is failed! this.MyFunctions.MyDevices.ParameterAccessByName == null");
        return (DataTable) null;
      }
      DataTable parameterTable = new DataTable();
      DataColumn column1 = new DataColumn("Parameter name", typeof (string));
      parameterTable.Columns.Add(column1);
      DataColumn column2 = new DataColumn("Address", typeof (int));
      parameterTable.Columns.Add(column2);
      DataColumn column3 = new DataColumn("ReadData(hex)", typeof (string));
      parameterTable.Columns.Add(column3);
      DataColumn column4 = new DataColumn("ReadData(dec)", typeof (long));
      parameterTable.Columns.Add(column4);
      DataColumn column5 = new DataColumn("ReadValue", typeof (double));
      parameterTable.Columns.Add(column5);
      DataColumn column6 = new DataColumn("ReadDate", typeof (DateTime));
      parameterTable.Columns.Add(column6);
      DataColumn column7 = new DataColumn("Description", typeof (string));
      parameterTable.Columns.Add(column7);
      foreach (ParameterAccess parameterAccess in (IEnumerable<ParameterAccess>) this.MyFunctions.MyDevices.ParameterAccessByName.Values)
      {
        if (parameterAccess.IsAvailable(this.Map))
        {
          string parameterMapDataHex = parameterAccess.GetParameterMapDataHex(this.Map);
          long num = 0;
          if (parameterMapDataHex.Length > 1 && parameterMapDataHex.Length < 16)
            num = long.Parse(parameterMapDataHex, NumberStyles.HexNumber);
          DataRow row = parameterTable.NewRow();
          row[0] = (object) parameterAccess.Name;
          row[1] = (object) parameterAccess.TelegramPara.Address;
          row[2] = (object) parameterMapDataHex;
          row[3] = (object) num;
          object readValue = this.GetReadValue(parameterAccess.Name);
          if (readValue != null)
          {
            if (readValue.GetType() == typeof (double) || readValue.GetType() == typeof (Decimal))
              row[4] = readValue;
            else if (readValue.GetType() == typeof (DateTime))
              row[5] = readValue;
          }
          row[6] = (object) parameterAccess.TelegramPara.Description;
          parameterTable.Rows.Add(row);
        }
      }
      return parameterTable;
    }

    internal MinolDevice DeepCopy()
    {
      MinolDevice minolDevice = (MinolDevice) this.MemberwiseClone();
      short[] numArray = new short[this.Map.Length];
      this.Map.CopyTo((Array) numArray, 0);
      minolDevice.MyFunctions = this.MyFunctions;
      minolDevice.DeviceState = this.DeviceState;
      minolDevice.DeviceType = this.DeviceType;
      minolDevice.FirmwareVersion = this.FirmwareVersion;
      minolDevice.HardwareTypeID = this.HardwareTypeID;
      minolDevice.Map = numArray;
      minolDevice.SerialNumber = this.SerialNumber;
      minolDevice.ParameterList = this.ParameterList;
      minolDevice.MeterID = this.MeterID;
      minolDevice.Signature = this.Signature;
      minolDevice.Manufacturer = this.Manufacturer;
      return minolDevice;
    }

    internal bool IsEqual(MinolDevice device)
    {
      if (device == null)
        return false;
      for (int index = 0; index < this.Map.Length; ++index)
      {
        if ((int) (byte) this.Map[index] != (int) (byte) device.Map[index])
          return false;
      }
      return true;
    }

    internal Dictionary<MemoryLocation, Dictionary<int, byte[]>> GenerateWriteParameters()
    {
      Dictionary<MemoryLocation, Dictionary<int, byte[]>> writeParameters = new Dictionary<MemoryLocation, Dictionary<int, byte[]>>();
      Dictionary<int, byte[]> ramWriteParameters = this.GenerateRamWriteParameters();
      if (ramWriteParameters != null && ramWriteParameters.Count > 0)
        writeParameters.Add(MemoryLocation.RAM, ramWriteParameters);
      Dictionary<int, byte[]> flashWriteParameters = this.GenerateFlashWriteParameters();
      if (flashWriteParameters != null && flashWriteParameters.Count > 0)
        writeParameters.Add(MemoryLocation.FLASH, flashWriteParameters);
      return writeParameters;
    }

    private Dictionary<int, byte[]> GenerateFlashWriteParameters()
    {
      Dictionary<int, byte[]> flashWriteParameters = new Dictionary<int, byte[]>();
      List<byte> byteList = new List<byte>();
      bool flag1 = false;
      for (int index = 0; index < this.Map.Length; ++index)
      {
        bool flag2 = ((int) this.Map[index] & 256) == 0;
        bool flag3 = ((uint) this.Map[index] & 512U) > 0U;
        bool flag4 = ((uint) this.Map[index] & 1024U) > 0U;
        if (flag2 && flag4)
        {
          if (flag3)
            flag1 = true;
          int num = 0;
          while (!this.MyFunctions.MyDevices.ParameterAccessByAddress.ContainsKey(index - num))
            ++num;
          if (this.MyFunctions.MyDevices.ParameterAccessByAddress[index - num].TelegramPara.Parent == "CRC_INFO")
            byteList.Add((byte) this.Map[index]);
        }
      }
      if (!flag1)
        return (Dictionary<int, byte[]>) null;
      int address = this.MyFunctions.MyDevices.ParameterAccessByName["INFO;CRC_INFO"].TelegramPara.Address;
      byte[] array = byteList.ToArray();
      int checksumReversed = (int) CRC.calculateChecksumReversed(array, (uint) array.Length, 0U);
      byte[] buffer = new byte[2]
      {
        (byte) checksumReversed,
        (byte) (checksumReversed >> 8)
      };
      if (MinolDevice.logger.IsDebugEnabled)
        MinolDevice.logger.Debug("Set new CRC for FLASH: 0x" + checksumReversed.ToString("X2"));
      this.SetMapValue(address, buffer);
      int capacity = this.GetMaxSizeOfRequestBuffer() - 14;
      List<short> shortList = new List<short>(capacity);
      int key = -1;
      for (int index1 = 0; index1 < this.Map.Length; ++index1)
      {
        bool flag5 = ((int) this.Map[index1] & 256) == 0;
        bool flag6 = ((uint) this.Map[index1] & 1024U) > 0U;
        if (flag5 && flag6)
        {
          if (key == -1)
            key = index1;
          shortList.Add(this.Map[index1]);
          bool flag7 = shortList.Count == capacity;
          bool flag8 = index1 + 1 == this.Map.Length;
          bool flag9 = !flag8 && ((int) this.Map[index1 + 1] & 1024) == 0;
          if (flag7 | flag8 | flag9)
          {
            byte[] numArray = new byte[shortList.Count];
            for (int index2 = 0; index2 < shortList.Count; ++index2)
              numArray[index2] = (byte) shortList[index2];
            flashWriteParameters.Add(key, numArray);
            shortList.Clear();
            key = -1;
          }
        }
      }
      return flashWriteParameters;
    }

    private Dictionary<int, byte[]> GenerateRamWriteParameters()
    {
      Dictionary<int, byte[]> ramWriteParameters = new Dictionary<int, byte[]>();
      int capacity = this.GetMaxSizeOfRequestBuffer() - 14;
      List<short> shortList = new List<short>(capacity);
      int key = -1;
      for (int index1 = 0; index1 < this.Map.Length; ++index1)
      {
        bool flag1 = ((int) this.Map[index1] & 256) == 0;
        if (!(((uint) this.Map[index1] & 512U) <= 0U & shortList.Count == 0))
        {
          bool flag2 = ((int) this.Map[index1] & 1024) == 0;
          if (flag1 && flag2)
          {
            if (key == -1)
              key = index1;
            shortList.Add(this.Map[index1]);
            bool flag3 = shortList.Count == capacity;
            bool flag4 = index1 + 1 == this.Map.Length;
            bool flag5 = !flag4 && ((int) this.Map[index1 + 1] & 1024) != 0 || !flag4 && ((uint) this.Map[index1 + 1] & 256U) > 0U;
            if (flag3 | flag4 | flag5)
            {
              for (int index2 = shortList.Count - 1; index2 >= 0 && ((uint) shortList[index2] & 512U) <= 0U; --index2)
                shortList.RemoveAt(index2);
              byte[] numArray = new byte[shortList.Count];
              for (int index3 = 0; index3 < shortList.Count; ++index3)
                numArray[index3] = (byte) shortList[index3];
              ramWriteParameters.Add(key, numArray);
              shortList.Clear();
              key = -1;
            }
          }
        }
      }
      return ramWriteParameters;
    }

    internal string GetIdentificationString()
    {
      return string.Format("{0}_{1}_{2}_{3}", (object) this.ToString().Replace("MinolHandler.", ""), (object) this.FirmwareVersion, (object) this.DeviceType, (object) this.SerialNumber);
    }

    protected void AddParameter(int subDevice, OverrideID parameterID)
    {
      if (UserManager.IsNewLicenseModel() && !UserManager.IsConfigParamVisible(parameterID))
        return;
      switch (parameterID)
      {
        case OverrideID.SerialNumber:
          this.AddParameter(true, parameterID, new ConfigurationParameter(parameterID, this.SerialNumber.ToString(), true)
          {
            SubDevice = subDevice
          });
          break;
        case OverrideID.DeviceName:
          this.AddParameter(false, parameterID, new ConfigurationParameter(parameterID, this.DeviceType.ToString(), true)
          {
            SubDevice = subDevice
          });
          break;
        case OverrideID.FirmwareVersion:
          this.AddParameter(false, parameterID, new ConfigurationParameter(parameterID, (object) this.FirmwareVersion.ToString())
          {
            SubDevice = subDevice
          });
          break;
        case OverrideID.Signature:
          this.AddParameter(false, parameterID, new ConfigurationParameter(parameterID, (object) ("0x" + this.Signature.ToString("X4")))
          {
            SubDevice = subDevice
          });
          break;
        case OverrideID.Manufacturer:
          this.AddParameter(false, parameterID, new ConfigurationParameter(parameterID, (object) ("0x" + this.Manufacturer.ToString("X4")))
          {
            SubDevice = subDevice
          });
          break;
        default:
          throw new ArgumentException("Unknown parameter! OverrideID: " + parameterID.ToString());
      }
    }

    protected void AddParameter(
      bool canChanged,
      int subDevice,
      OverrideID parameterID,
      string parameterKey)
    {
      this.AddParameter(canChanged, subDevice, parameterID, parameterKey, false, "");
    }

    protected void AddParameter(
      bool canChanged,
      int subDevice,
      OverrideID parameterID,
      string parameterKey,
      bool hasWritePermission)
    {
      this.AddParameter(canChanged, subDevice, parameterID, parameterKey, hasWritePermission, "");
    }

    protected void AddParameter(
      bool canChanged,
      int subDevice,
      OverrideID parameterID,
      string parameterKey,
      bool hasWritePermission,
      string physicalUnit)
    {
      this.AddParameter(canChanged, subDevice, parameterID, parameterKey, hasWritePermission, physicalUnit, (string[]) null);
    }

    protected void AddParameter(
      bool canChanged,
      int subDevice,
      OverrideID parameterID,
      string parameterKey,
      bool hasWritePermission,
      string physicalUnit,
      string[] allowedValues)
    {
      this.AddParameter(canChanged, subDevice, parameterID, parameterKey, hasWritePermission, physicalUnit, (string[]) null, (object) null, (object) null);
    }

    protected void AddParameter(
      bool canChanged,
      int subDevice,
      OverrideID parameterID,
      string parameterKey,
      bool hasWritePermission,
      string physicalUnit,
      string[] allowedValues,
      object minValue,
      object maxValue)
    {
      object readValue = this.GetReadValue(parameterKey);
      if (UserManager.IsNewLicenseModel())
      {
        if (!UserManager.IsConfigParamVisible(parameterID))
          return;
        this.ParameterList.Add(parameterID, new ConfigurationParameter(parameterID, readValue)
        {
          ParameterKey = parameterKey,
          SubDevice = subDevice,
          MinParameterValue = minValue,
          MaxParameterValue = maxValue,
          HasWritePermission = canChanged && UserManager.IsConfigParamEditable(parameterID),
          AllowedValues = allowedValues
        });
      }
      else
        this.ParameterList.Add(parameterID, new ConfigurationParameter(parameterID, readValue)
        {
          HasWritePermission = hasWritePermission,
          ParameterKey = parameterKey,
          SubDevice = subDevice,
          AllowedValues = allowedValues,
          MinParameterValue = minValue,
          MaxParameterValue = maxValue
        });
    }

    protected void AddParameter(
      bool canChanged,
      OverrideID overrideID,
      ConfigurationParameter configurationParameter)
    {
      if (UserManager.IsNewLicenseModel())
      {
        if (!UserManager.IsConfigParamVisible(overrideID))
          return;
        configurationParameter.HasWritePermission = canChanged && UserManager.IsConfigParamEditable(overrideID);
      }
      this.ParameterList.Add(overrideID, configurationParameter);
    }

    protected void AddValue(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      DateTime timePoint,
      long valueIdent,
      object obj)
    {
      ReadingValue readingValue = new ReadingValue();
      readingValue.value = Util.ToDouble(obj);
      readingValue.state = this.DeviceState;
      if (valueList.ContainsKey(valueIdent))
      {
        if (valueList[valueIdent].ContainsKey(timePoint))
          return;
        valueList[valueIdent].Add(timePoint, readingValue);
      }
      else
        valueList.Add(valueIdent, new SortedList<DateTime, ReadingValue>()
        {
          {
            timePoint,
            readingValue
          }
        });
    }

    internal bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      return this.SetConfigurationParameters(parameterList, 0);
    }

    internal virtual bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ParameterList,
      int SubDevice)
    {
      throw new Exception("MinolHandler: Function SetConfigurationParameters not implemented!");
    }

    internal void SetConfigurationParameters(int address, byte[] value)
    {
      if (this.MyFunctions.MyDevices.WorkDevice == null)
        throw new ArgumentNullException("WorkDevice is null!");
      bool flag = false;
      for (int index = 0; index < value.Length; ++index)
      {
        if ((int) (byte) this.MyFunctions.MyDevices.WorkDevice.Map[address + index] != (int) value[index])
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      for (int index = 0; index < value.Length; ++index)
      {
        this.Map[address + index] = (short) ((int) this.Map[address + index] & 65280);
        this.Map[address + index] = (short) ((int) this.Map[address + index] | (int) value[index]);
        this.Map[address + index] |= (short) 512;
      }
    }

    internal bool SetReadValue(string parameterKey, object parameterValue)
    {
      if (string.IsNullOrEmpty(parameterKey))
        return false;
      try
      {
        if (this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
        {
          ParameterAccess parameterAccess = this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey];
          ParameterAccess bitAccesParent = parameterAccess.BitAccesParent;
          int address = bitAccesParent.TelegramPara.Address;
          long readValue = (long) bitAccesParent.GetReadValue(this.Map);
          long num = !Util.ToBoolean(parameterValue) ? readValue & (long) ~parameterAccess.TelegramPara.BitMask : (long) ((int) readValue | parameterAccess.TelegramPara.BitMask);
          this.SetConfigurationParameters(address, new byte[1]
          {
            (byte) num
          });
        }
        else
        {
          ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey];
          this.SetConfigurationParameters(parameterAccess.TelegramPara.Address, parameterAccess.GetValueAsByteArray(parameterValue));
        }
        return true;
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MinolDevice.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
    }

    internal void SetMapValue(int address, short newValue)
    {
      this.Map[address] = newValue;
      this.Map[address] |= (short) 512;
    }

    internal void SetMapValue(int address, byte newValue)
    {
      short num = (short) ((int) (short) ((int) (short) newValue | (int) (short) ((int) this.Map[address] & 65280)) | 512);
      this.Map[address] = num;
    }

    internal bool SetMapValue(int address, byte[] buffer)
    {
      for (int index = 0; index < buffer.Length; ++index)
        this.SetMapValue(address + index, buffer[index]);
      return true;
    }

    internal SortedList<int, short> GetMemoryMap()
    {
      SortedList<int, short> memoryMap = new SortedList<int, short>();
      for (int key = 0; key < this.Map.Length; ++key)
      {
        if (((int) this.Map[key] & 256) == 0)
          memoryMap.Add(key, this.Map[key]);
      }
      return memoryMap;
    }

    protected byte ConvertInputUnitsIndexToMinolUnit(string inputUnitsIndexAsString)
    {
      return this.ConvertInputUnitsIndexToMinolUnit((InputUnitsIndex) Enum.Parse(typeof (InputUnitsIndex), inputUnitsIndexAsString, true));
    }

    protected byte ConvertInputUnitsIndexToMinolUnit(InputUnitsIndex inputUnitsIndex)
    {
      switch (inputUnitsIndex)
      {
        case InputUnitsIndex.ImpUnit_0Wh:
          return 3;
        case InputUnitsIndex.ImpUnit_0kWh:
          return 4;
        case InputUnitsIndex.ImpUnit_0MWh:
          return 5;
        case InputUnitsIndex.ImpUnit_0J:
          return 6;
        case InputUnitsIndex.ImpUnit_0kJ:
          return 7;
        case InputUnitsIndex.ImpUnit_0MJ:
          return 8;
        case InputUnitsIndex.ImpUnit_0GJ:
          return 9;
        case InputUnitsIndex.ImpUnit_0L:
          return 1;
        case InputUnitsIndex.ImpUnit_0qm:
          return 2;
        default:
          return 0;
      }
    }

    protected void ClearSavedLoggerValuesOfRadio3Device()
    {
      for (int index = 0; index <= 31; ++index)
      {
        this.SetReadValue("LOGD;DateStampD" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGD;ReadingD" + index.ToString("00"), (object) uint.MaxValue);
      }
      for (int index = 0; index <= 31; ++index)
      {
        this.SetReadValue("LOGH0;DateStamp" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGH0;Reading" + index.ToString("00"), (object) uint.MaxValue);
      }
      for (int index = 32; index <= 63; ++index)
      {
        this.SetReadValue("LOGH1;DateStamp" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGH1;Reading" + index.ToString("00"), (object) uint.MaxValue);
      }
      for (int index = 64; index <= 95; ++index)
      {
        this.SetReadValue("LOGH2;DateStamp" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGH2;Reading" + index.ToString("00"), (object) uint.MaxValue);
      }
    }

    public static int CalculateRadio3EpsilonValue(int funkId)
    {
      int num1 = 5759 - Convert.ToInt32(funkId.ToString("00000000").Substring(4)) % 5000;
      int num2 = num1 / 64;
      int num3 = num1 % 64;
      return 90 - num2 - Math.Sign(num3) << 8 | (64 - num3) % 64 * 4;
    }

    protected void AddErrorValue(
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdentError error,
      DateTime? timepoint)
    {
      long valueIdentOfError = ValueIdent.GetValueIdentOfError(meterType, error);
      ReadingValue readingValue = new ReadingValue();
      readingValue.value = 1.0;
      SortedList<DateTime, ReadingValue> sortedList = new SortedList<DateTime, ReadingValue>();
      if (timepoint.HasValue)
      {
        sortedList.Add(timepoint.Value, readingValue);
      }
      else
      {
        DateTime now = DateTime.Now;
        sortedList.Add(new DateTime(now.Year, now.Month, now.Day), readingValue);
      }
      ValueList.Add(valueIdentOfError, sortedList);
    }
  }
}
