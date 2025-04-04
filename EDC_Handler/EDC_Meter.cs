// Decompiled with JetBrains decompiler
// Type: EDC_Handler.EDC_Meter
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using DeviceCollector;
using GmmDbLib;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public sealed class EDC_Meter : IMeter
  {
    private static Logger logger = LogManager.GetLogger(nameof (EDC_Meter));
    public const int COL_INDEX_VALUE_NAME_KEY = 1;
    public const int COL_INDEX_VALUE_NAME = 2;
    public const int COL_INDEX_VALUE_SIZE = 3;
    public const int COL_INDEX_VALUE_TYPE = 4;
    public const int COL_INDEX_VALUE_ADDRESS_HEX = 5;
    public const int COL_INDEX_VALUE_HEX = 6;
    public const int COL_INDEX_VALUE_DEC = 7;
    public const int COL_INDEX_VALUE_DIF_VIF = 8;

    public EDC_Meter(EDC_MemoryMap map)
    {
      this.Map = map;
      this.DBDeviceInfo = new DatabaseDeviceInfo();
    }

    public DeviceVersion Version { get; set; }

    public DatabaseDeviceInfo DBDeviceInfo { get; set; }

    public EDC_MemoryMap Map { get; private set; }

    public EDC_Meter DeepCopy()
    {
      return new EDC_Meter(this.Map.DeepCopy())
      {
        DBDeviceInfo = this.DBDeviceInfo != null ? this.DBDeviceInfo.DeepCopy() : (DatabaseDeviceInfo) null,
        Map = this.Map != null ? this.Map.DeepCopy() : (EDC_MemoryMap) null,
        Version = this.Version != null ? this.Version.DeepCopy() : (DeviceVersion) null
      };
    }

    public DataTable CreateParameterTable()
    {
      DataTable parameterTable = new DataTable();
      parameterTable.Columns.Add("#", typeof (int));
      parameterTable.Columns.Add("Key", typeof (string));
      parameterTable.Columns.Add("Name", typeof (string));
      parameterTable.Columns.Add("Bytes", typeof (int));
      parameterTable.Columns.Add("Type", typeof (S3_VariableTypes));
      parameterTable.Columns.Add("Address", typeof (string));
      parameterTable.Columns.Add("Value Hex", typeof (string));
      parameterTable.Columns.Add("Value Dec", typeof (object));
      parameterTable.Columns.Add("DifVif", typeof (string));
      List<Parameter> parameter = EDC_MemoryMap.GetParameter(this.Version);
      if (parameter == null)
        return (DataTable) null;
      foreach (Parameter p in parameter)
      {
        string str1 = Ot.GetTranslatedLanguageText("S3ParaName", p.Name);
        string str2 = string.Empty;
        object obj = (object) null;
        if (p.Type != S3_VariableTypes.Address)
        {
          byte[] memoryBytes = this.Map.GetMemoryBytes(p);
          str2 = Util.ByteArrayToHexString(memoryBytes);
          try
          {
            switch (p.Type)
            {
              case S3_VariableTypes.INT8:
                obj = (object) ((((int) memoryBytes[0] & 128) == 128 ? -1 : 1) * ((int) memoryBytes[0] & (int) sbyte.MaxValue));
                break;
              case S3_VariableTypes.UINT8:
                obj = (object) memoryBytes[0];
                break;
              case S3_VariableTypes.UINT16:
                obj = (object) BitConverter.ToUInt16(memoryBytes, 0);
                break;
              case S3_VariableTypes.INT16:
                obj = (object) BitConverter.ToInt16(memoryBytes, 0);
                break;
              case S3_VariableTypes.UINT32:
                obj = (object) BitConverter.ToUInt32(memoryBytes, 0);
                break;
              case S3_VariableTypes.INT32:
                obj = (object) BitConverter.ToInt32(memoryBytes, 0);
                break;
              case S3_VariableTypes.REAL32:
                obj = (object) BitConverter.ToSingle(memoryBytes, 0);
                break;
              case S3_VariableTypes.INT64:
                obj = (object) BitConverter.ToInt64(memoryBytes, 0);
                break;
              case S3_VariableTypes.UINT64:
                obj = (object) BitConverter.ToUInt64(memoryBytes, 0);
                break;
              case S3_VariableTypes.REAL64:
                obj = (object) BitConverter.ToDouble(memoryBytes, 0);
                break;
              case S3_VariableTypes.MeterTime1980:
                obj = (object) ZR_Calendar.Cal_GetDateTime(BitConverter.ToUInt32(memoryBytes, 0));
                break;
              case S3_VariableTypes.Address:
                obj = (object) string.Empty;
                break;
              case S3_VariableTypes.ByteArray:
                obj = (object) string.Empty;
                break;
              default:
                obj = (object) "???";
                break;
            }
          }
          catch (Exception ex)
          {
            throw new Exception("Can not convert parameter: " + p?.ToString(), ex);
          }
        }
        if (str1 == "S3ParaName" + p.Name)
          str1 = string.Empty;
        parameterTable.Rows.Add((object) parameter.IndexOf(p), (object) p.Name, (object) str1, (object) (p.Type != S3_VariableTypes.Address ? p.Size : 0), (object) p.Type, (object) ("0x" + p.Address.ToString("X4")), (object) str2, obj, (object) p.DifVif);
      }
      return parameterTable;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      int totalWidth = 60;
      int num = 37;
      if (this.Version == null || this.Map == null)
        return string.Empty;
      try
      {
        EDC_MemoryMap.CacheParameter(this.Version);
        if (!EDC_MemoryMap.ExistParameter(this.Version))
          return "No MAP available into database! Version: " + this.Version?.ToString();
        string serialnumberFull = this.GetSerialnumberFull();
        if (!string.IsNullOrEmpty(serialnumberFull))
          stringBuilder.Append("Full serial number: ".PadRight(num, ' ')).AppendLine(serialnumberFull);
        else
          stringBuilder.Append("Full serial number: ".PadRight(num, ' ')).AppendLine("null");
        stringBuilder.AppendLine(this.Version.ToString(num));
        if (this.DBDeviceInfo.HardwareType != null)
        {
          stringBuilder.AppendLine("Database Hardware Info ".PadRight(totalWidth, '-'));
          stringBuilder.AppendLine(this.DBDeviceInfo.HardwareType.ToString(num));
        }
        if (this.DBDeviceInfo.MeterInfo != null)
        {
          stringBuilder.AppendLine("Database Meter Info ".PadRight(totalWidth, '-'));
          stringBuilder.AppendLine(this.DBDeviceInfo.MeterInfo.ToString(num));
        }
        DeviceIdentification deviceIdentification = this.GetDeviceIdentification();
        if (deviceIdentification != null)
        {
          stringBuilder.AppendLine("Device Identification ".PadRight(totalWidth, '-'));
          stringBuilder.AppendLine(deviceIdentification.ToString(num));
        }
        else
        {
          stringBuilder.AppendLine("Device Identification ".PadRight(totalWidth, '-'));
          stringBuilder.AppendLine("not defined");
        }
        stringBuilder.AppendLine("Parameter ".PadRight(totalWidth, '-'));
        foreach (Parameter parameter in EDC_MemoryMap.GetParameter(this.Version))
        {
          if (parameter.Type != S3_VariableTypes.Address)
          {
            if (!this.Map.AreBytesDefined(parameter.Address, parameter.Size))
            {
              stringBuilder.Append(parameter.Name.PadRight(num, ' ')).AppendLine("!!!!!! NOT AVAILABLE !!!!!!");
            }
            else
            {
              byte[] parameterValue = this.GetParameterValue<byte[]>(parameter.Name);
              if (parameterValue != null)
              {
                stringBuilder.Append(parameter.Name.PadRight(num, ' '));
                for (int index = parameterValue.Length - 1; index >= 0; --index)
                  stringBuilder.Append(parameterValue[index].ToString("X2"));
                stringBuilder.AppendLine();
              }
              else
                stringBuilder.Append(parameter.Name.PadRight(num, ' ')).AppendLine(" ERROR ");
            }
          }
        }
        stringBuilder.Append("Read ").Append(this.Map.GetDefinedBytesCount().ToString()).AppendLine(" bytes.");
      }
      catch (Exception ex)
      {
        stringBuilder.AppendLine("INTERNAL ERROR: ").AppendLine(ex.Message);
      }
      return stringBuilder.ToString();
    }

    public SortedList<ushort, byte[]> GetChangedRamBlocks(EDC_Meter sourceMeter)
    {
      return this.Map.GetChangedRamBlocks(this.Version, sourceMeter);
    }

    public SortedList<ushort, byte[]> GetChangedFlashBlocks(
      EDC_Meter sourceMeter,
      out List<ushort> segmentsToErase)
    {
      segmentsToErase = (List<ushort>) null;
      SortedList<ushort, byte[]> changedFlashBlocks1 = new SortedList<ushort, byte[]>();
      List<ushort> ushortList = new List<ushort>();
      Parameter parameter1 = this.GetParameter("INFOA");
      if (parameter1 == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter INFOA.");
      Parameter parameter2 = this.GetParameter("PARAM_CONST_LIST");
      if (parameter2 == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter PARAM_CONST_LIST.");
      SortedList<ushort, byte[]> changedFlashBlocks2 = this.Map.GetChangedFlashBlocks(sourceMeter, (int) parameter1.Address, (int) parameter1.Address + 128, 128, out segmentsToErase);
      if (changedFlashBlocks2 != null)
      {
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedFlashBlocks2)
          changedFlashBlocks1.Add(keyValuePair.Key, keyValuePair.Value);
        ushortList.AddRange((IEnumerable<ushort>) segmentsToErase);
      }
      SortedList<ushort, byte[]> changedFlashBlocks3 = this.Map.GetChangedFlashBlocks(sourceMeter, 32768, (int) parameter2.Address + 512, 512, out segmentsToErase);
      if (changedFlashBlocks3 != null)
      {
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedFlashBlocks3)
          changedFlashBlocks1.Add(keyValuePair.Key, keyValuePair.Value);
        ushortList.AddRange((IEnumerable<ushort>) segmentsToErase);
      }
      segmentsToErase = ushortList;
      return changedFlashBlocks1;
    }

    public Parameter GetParameter(string parameterName)
    {
      return string.IsNullOrEmpty(parameterName) || !EDC_MemoryMap.ExistParameter(this.Version) ? (Parameter) null : EDC_MemoryMap.GetParameter(this.Version, parameterName);
    }

    public T GetParameterValue<T>(string parameterName)
    {
      if (string.IsNullOrEmpty(parameterName))
        throw new ArgumentException(nameof (parameterName));
      EDC_MemoryMap.CacheParameter(this.Version);
      if (!EDC_MemoryMap.ExistParameter(this.Version))
        throw new ArgumentException("The map file info is not available into database for this firmware! Version: " + this.Version?.ToString());
      Parameter parameter = this.GetParameter(parameterName);
      if (parameter == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (!this.Map.AreBytesDefined(parameter.Address, parameter.Size))
        throw new ArgumentException("No bytes are available at this address! Parameter name: " + parameterName);
      if (typeof (T) == typeof (short))
        return (T) (System.ValueType) BitConverter.ToInt16(this.Map.GetMemoryBytes(parameter), 0);
      if (typeof (T) == typeof (ushort))
        return (T) (System.ValueType) BitConverter.ToUInt16(this.Map.GetMemoryBytes(parameter), 0);
      if (typeof (T) == typeof (byte))
        return (T) (System.ValueType) this.Map.GetMemoryBytes(parameter)[0];
      if (typeof (T) == typeof (sbyte))
        return (T) (System.ValueType) (sbyte) this.Map.GetMemoryBytes(parameter)[0];
      if (typeof (T) == typeof (byte[]))
        return (T) this.Map.GetMemoryBytes(parameter);
      if (typeof (T) == typeof (uint))
        return (T) (System.ValueType) BitConverter.ToUInt32(this.Map.GetMemoryBytes(parameter), 0);
      if (typeof (T) == typeof (int))
        return (T) (System.ValueType) BitConverter.ToInt32(this.Map.GetMemoryBytes(parameter), 0);
      if (typeof (T) == typeof (ulong))
        return (T) (System.ValueType) BitConverter.ToUInt64(this.Map.GetMemoryBytes(parameter), 0);
      if (typeof (T) == typeof (long))
        return (T) (System.ValueType) BitConverter.ToInt64(this.Map.GetMemoryBytes(parameter), 0);
      if (!(typeof (T) == typeof (DateTime)))
        throw new NotImplementedException("INTERNAL ERROR: Can not cast the Value. The type is not implemented. Type: " + typeof (T)?.ToString());
      byte[] memoryBytes = this.Map.GetMemoryBytes(parameter);
      byte num = memoryBytes[0];
      byte month = memoryBytes[1];
      byte day = memoryBytes[2];
      byte hour = memoryBytes[3];
      byte minute = memoryBytes[4];
      byte second = memoryBytes[5];
      if (num == byte.MaxValue || month == byte.MaxValue || day == byte.MaxValue || hour == byte.MaxValue || minute == byte.MaxValue || second == byte.MaxValue)
        return (T) (System.ValueType) EDC_HandlerFunctions.DateTimeNull;
      if (num > byte.MaxValue || month > (byte) 12 || day > (byte) 31 || minute > (byte) 59 || second > (byte) 59)
        return (T) (System.ValueType) EDC_HandlerFunctions.DateTimeNull;
      try
      {
        return (T) (System.ValueType) new DateTime(2000 + (int) num, (int) month, (int) day, (int) hour, (int) minute, (int) second);
      }
      catch
      {
        return (T) (System.ValueType) EDC_HandlerFunctions.DateTimeNull;
      }
    }

    public bool SetParameterValue<T>(string parameterName, T newValue)
    {
      CultureInfo invariantCulture = CultureInfo.InvariantCulture;
      Parameter parameter = this.GetParameter(parameterName);
      if (parameter == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (!this.Map.AreBytesDefined(parameter.Address, parameter.Size))
        throw new ArgumentException("No bytes are available at this address! Parameter name: " + parameterName);
      if (typeof (T) == typeof (short))
      {
        byte[] bytes = BitConverter.GetBytes((short) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (ushort))
      {
        byte[] bytes = BitConverter.GetBytes((ushort) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (int))
      {
        byte[] bytes = BitConverter.GetBytes((int) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (uint))
      {
        byte[] bytes = BitConverter.GetBytes((uint) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (byte))
      {
        byte num = (byte) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
        return this.Map.SetMemoryBytes(parameter.Address, new byte[1]
        {
          num
        });
      }
      if (typeof (T) == typeof (sbyte))
      {
        sbyte num = (sbyte) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
        return this.Map.SetMemoryBytes(parameter.Address, new byte[1]
        {
          (byte) num
        });
      }
      if (typeof (T) == typeof (byte[]))
      {
        byte[] buffer = (byte[]) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
        return this.Map.SetMemoryBytes(parameter.Address, buffer);
      }
      if (typeof (T) == typeof (long))
      {
        byte[] bytes = BitConverter.GetBytes((long) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (ulong))
      {
        byte[] bytes = BitConverter.GetBytes((ulong) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (!(typeof (T) == typeof (DateTime)))
        throw new NotImplementedException("INTERNAL ERROR: Can not convert value to byte array. The type is not implemented. Type: " + typeof (T)?.ToString());
      DateTime dateTime = (DateTime) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
      if (dateTime.Year < 2000)
        throw new ArgumentOutOfRangeException("Invalid date time! The year should be greater or equal to 2000. Value: " + dateTime.ToString("G"));
      if (dateTime.Year > 2255)
        throw new ArgumentOutOfRangeException("Invalid date time! The year should be smaller as 2255. Value: " + dateTime.ToString("G"));
      byte[] buffer1 = new byte[6]
      {
        (byte) (dateTime.Year - 2000),
        (byte) dateTime.Month,
        (byte) dateTime.Day,
        (byte) dateTime.Hour,
        (byte) dateTime.Minute,
        (byte) dateTime.Second
      };
      return this.Map.SetMemoryBytes(parameter.Address, buffer1);
    }

    private bool SetBit<T>(string parameterName, T mask) where T : struct
    {
      return this.ChangeBit(parameterName, true, this.GetBytes<T>(mask));
    }

    private bool ClearBit<T>(string parameterName, T mask) where T : struct
    {
      return this.ChangeBit(parameterName, false, this.GetBytes<T>(mask));
    }

    private bool ChangeBit(string parameterName, bool isSet, byte[] mask)
    {
      Parameter parameter = this.GetParameter(parameterName);
      if (parameter == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (parameter.Size != mask.Length)
        throw new ArgumentException("Invalid size! Name: " + parameterName);
      byte[] memoryBytes = this.Map.GetMemoryBytes(parameter);
      if (memoryBytes == null)
        return false;
      for (int index = 0; index < memoryBytes.Length; ++index)
      {
        if (isSet)
          memoryBytes[index] |= mask[index];
        else
          memoryBytes[index] &= ~mask[index];
      }
      this.Map.SetMemoryBytes(parameter.Address, memoryBytes);
      return true;
    }

    private bool GetBit<T>(string parameterName, T mask) where T : struct
    {
      return this.GetBit(parameterName, this.GetBytes<T>(mask));
    }

    private bool GetBit(string parameterName, byte[] mask)
    {
      Parameter parameter = this.GetParameter(parameterName);
      if (parameter == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (parameter.Size != mask.Length)
        throw new ArgumentException("Invalid size! Name: " + parameterName);
      byte[] memoryBytes = this.Map.GetMemoryBytes(parameter);
      if (memoryBytes == null)
        return false;
      for (int index = 0; index < memoryBytes.Length; ++index)
      {
        if (mask[index] != (byte) 0 && (int) (byte) ((uint) memoryBytes[index] & (uint) mask[index]) == (int) mask[index])
          return true;
      }
      return false;
    }

    private byte[] GetBytes<T>(T type) where T : struct
    {
      if (typeof (T) == typeof (byte))
        return new byte[1]{ Convert.ToByte((object) type) };
      if (typeof (T) == typeof (ushort))
        return BitConverter.GetBytes(Convert.ToUInt16((object) type));
      throw new NotImplementedException("INTERNAL ERROR: Can not get bytes of the generic type. The type is not implemented. Type: " + typeof (T)?.ToString());
    }

    public byte[] Zip() => this.Map.Zip(this.Version);

    public static bool IsValidEdcZipBuffer(byte[] buffer)
    {
      if (buffer == null)
        return false;
      EDC_MemoryMap edcMemoryMap = new EDC_MemoryMap();
      try
      {
        if (!edcMemoryMap.Unzip(buffer, out DeviceVersion _))
          return false;
        if (edcMemoryMap.IsEmpty())
          return false;
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static EDC_Meter Unzip(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException("The parameter 'buffer' can not be null!");
      EDC_MemoryMap map = new EDC_MemoryMap();
      DeviceVersion version;
      if (!map.Unzip(buffer, out version))
        throw new Exception("Can not unzip MAP file!");
      EDC_Meter edcMeter = !map.IsEmpty() ? new EDC_Meter(map) : throw new Exception("The MAP is empty!");
      edcMeter.Version = version;
      DeviceIdentification deviceIdentification = edcMeter.GetDeviceIdentification();
      if (deviceIdentification != null && deviceIdentification.IsChecksumOK)
      {
        edcMeter.DBDeviceInfo.MeterInfo = EDC_Database.GetMeterInfo(deviceIdentification.MeterInfoID);
        edcMeter.DBDeviceInfo.HardwareType = MeterDatabase.GetHardwareType((int) version.HardwareTypeID);
      }
      return edcMeter;
    }

    public bool Overwrite(EDC_Meter meter, OverwritePart parts)
    {
      if (meter == null)
        throw new ArgumentException("Overwrite failed! Input parameter 'meter' is null.");
      if (meter.Version.Type != this.Version.Type)
        throw new ArgumentException("Overwrite failed! Wrong device type. Actual: " + meter.Version.Type.ToString() + ", Expected: " + this.Version.Type.ToString());
      return ((parts & OverwritePart.TypeIdentification) != OverwritePart.TypeIdentification || this.OverwriteTypeIdentification(meter)) && ((parts & OverwritePart.RadioSettings) != OverwritePart.RadioSettings || this.OverwriteRadioSettings(meter)) && ((parts & OverwritePart.DeviceSettings) != OverwritePart.DeviceSettings || this.OverwriteDeviceSettings(meter)) && ((parts & OverwritePart.Constants) != OverwritePart.Constants || this.OverwriteConstants(meter));
    }

    private bool OverwriteTypeIdentification(EDC_Meter meter)
    {
      DeviceIdentification deviceIdentification = meter != null ? meter.GetDeviceIdentification() : throw new ArgumentException("Failed overwrite type identification! Input parameter 'meter' is null.");
      if (deviceIdentification == null)
        throw new Exception("Failed overwrite type identification! Identification block of the base type is null.");
      DeviceIdentification ident = this.GetDeviceIdentification() ?? new DeviceIdentification();
      ident.HardwareTypeID = deviceIdentification.HardwareTypeID;
      ident.MeterInfoID = deviceIdentification.MeterInfoID;
      ident.BaseTypeID = deviceIdentification.BaseTypeID;
      ident.MeterTypeID = deviceIdentification.MeterTypeID;
      ident.SapMaterialNumber = deviceIdentification.SapMaterialNumber;
      ident.SapProductionOrderNumber = deviceIdentification.SapProductionOrderNumber;
      if (ident.MeterID == 0U || ident.MeterID == uint.MaxValue)
      {
        int? nextUniqueId = MeterDatabase.GetNextUniqueID("Meter", "MeterID");
        if (!nextUniqueId.HasValue)
          throw new Exception("Failed overwrite type identification! Can not get new MeterID from database.");
        ident.MeterID = MeterDatabase.SetNextUniqueID("Meter", "MeterID", nextUniqueId.Value + 1) ? (uint) nextUniqueId.Value : throw new Exception("Failed overwrite type identification! Can not set new MeterID to database.");
      }
      if (!this.SetDeviceIdentification(ident) || !this.SetManufacturerPrimary(meter.GetManufacturerPrimary()) || !this.SetManufacturerSecondary(meter.GetManufacturerSecondary()))
        return false;
      MBusDeviceType? nullable1 = meter.GetMediumPrimary();
      if (!this.SetMediumPrimary(nullable1.Value))
        return false;
      nullable1 = meter.GetMediumSecondary();
      if (!this.SetMediumSecondary(nullable1.Value))
        return false;
      byte? nullable2 = meter.GetMBusGenerationPrimary();
      if (!this.SetMBusGenerationPrimary(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusGenerationSecondary();
      if (!this.SetMBusGenerationSecondary(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusAddressPrimary();
      if (!this.SetMBusAddressPrimary(nullable2.Value))
        return false;
      if (meter.Version.Type == EDC_Hardware.EDC_mBus || meter.Version.Type == EDC_Hardware.EDC_ModBus || this.Version.Type == EDC_Hardware.EDC_mBus_Modbus || this.Version.Type == EDC_Hardware.EDC_mBus_CJ188 || this.Version.Type == EDC_Hardware.EDC_RS485_Modbus || this.Version.Type == EDC_Hardware.EDC_RS485_CJ188)
      {
        nullable2 = meter.GetMBusAddressPrimary();
        if (!this.SetMBusAddressPrimary(nullable2.Value))
          return false;
        nullable2 = meter.GetMBusAddressSecondary();
        if (!this.SetMBusAddressSecondary(nullable2.Value))
          return false;
      }
      return true;
    }

    private bool OverwriteRadioSettings(EDC_Meter meter)
    {
      if (meter == null)
        return false;
      if (meter.Version.Type != EDC_Hardware.EDC_Radio)
        return this.SetPulseActivateRadio(meter.GetPulseActivateRadio().Value);
      if (!this.SetMBusListType(meter.GetMBusListType()))
        return false;
      bool? nullable1 = meter.GetRadioState();
      if (!this.SetRadioState(nullable1.Value) || !this.SetRadioMode(meter.GetRadioMode().Value) || !this.SetRadioPower(meter.GetRadioPower().Value) || !this.SetRadioScenario(meter.GetRadioScenario().Value))
        return false;
      ushort? nullable2 = meter.GetRadioTransmitInterval();
      if (!this.SetRadioTransmitInterval(nullable2.Value))
        return false;
      nullable2 = meter.GetRadioInstallInterval();
      if (!this.SetRadioInstallInterval(nullable2.Value))
        return false;
      nullable2 = meter.GetRadioErrorInterval();
      if (!this.SetRadioErrorInterval(nullable2.Value))
        return false;
      byte? nullable3 = meter.GetRadioInstallCount();
      if (!this.SetRadioInstallCount(nullable3.Value) || !this.SetAESkey(meter.GetAESkey()))
        return false;
      nullable1 = meter.GetWMBusLongHeaderState();
      if (!this.SetWMBusLongHeaderState(nullable1.Value))
        return false;
      nullable1 = meter.GetWMBusEncryptionState();
      if (!this.SetWMBusEncryptionState(nullable1.Value))
        return false;
      nullable1 = meter.GetWMBusSynchronousTransmissioModeState();
      if (!this.SetWMBusSynchronousTransmissioModeState(nullable1.Value))
        return false;
      nullable1 = meter.GetWMBusInstallationPacketsState();
      if (!this.SetWMBusInstallationPacketsState(nullable1.Value))
        return false;
      nullable3 = meter.GetPulseActivateRadio();
      return this.SetPulseActivateRadio(nullable3.Value);
    }

    private bool OverwriteDeviceSettings(EDC_Meter meter)
    {
      if (meter == null)
        return false;
      byte? nullable1 = meter.GetPulseMultiplier();
      if (!this.SetPulseMultiplier(nullable1.Value))
        return false;
      ushort? nullable2 = meter.GetSensorTimeout();
      if (!this.SetSensorTimeout(nullable2.Value) || !this.SetDueDate(meter.GetDueDate().Value))
        return false;
      bool? nullable3 = meter.GetCoilSampling();
      if (!this.SetCoilSampling(nullable3.Value))
        return false;
      nullable3 = meter.GetFlowCheckIntervalState();
      if (!this.SetFlowCheckIntervalState(nullable3.Value))
        return false;
      nullable3 = meter.GetMagnetDetectionState();
      if (!this.SetMagnetDetectionState(nullable3.Value))
        return false;
      nullable3 = meter.GetDataLoggingState();
      if (!this.SetDataLoggingState(nullable3.Value) || !this.SetTimeZone(meter.GetTimeZone().Value))
        return false;
      nullable2 = meter.GetPulsePeriod();
      if (!this.SetPulsePeriod(nullable2.Value))
        return false;
      nullable1 = meter.GetCogCount();
      if (!this.SetCogCount(nullable1.Value))
        return false;
      nullable3 = meter.GetRemovalDetectionState();
      if (!this.SetRemovalDetectionState(nullable3.Value))
        return false;
      sbyte? nullable4 = meter.GetCoilErrorThreshold();
      if (!this.SetCoilErrorThreshold(nullable4.Value))
        return false;
      nullable4 = meter.GetCoilMaxThreshold();
      if (!this.SetCoilMaxThreshold(nullable4.Value))
        return false;
      nullable4 = meter.GetCoilMinThreshold();
      if (!this.SetCoilMinThreshold(nullable4.Value))
        return false;
      nullable4 = meter.GetCoilAmplitudeLimit();
      if (!this.SetCoilAmplitudeLimit(nullable4.Value))
        return false;
      if (meter.Version.Type == EDC_Hardware.EDC_mBus && this.Version.Type == EDC_Hardware.EDC_mBus || meter.Version.Type == EDC_Hardware.EDC_ModBus && this.Version.Type == EDC_Hardware.EDC_ModBus || meter.Version.Type == EDC_Hardware.EDC_mBus_Modbus && this.Version.Type == EDC_Hardware.EDC_mBus_Modbus || meter.Version.Type == EDC_Hardware.EDC_mBus_CJ188 && this.Version.Type == EDC_Hardware.EDC_mBus_CJ188 || meter.Version.Type == EDC_Hardware.EDC_RS485_Modbus && this.Version.Type == EDC_Hardware.EDC_RS485_Modbus || meter.Version.Type == EDC_Hardware.EDC_RS485_CJ188 && this.Version.Type == EDC_Hardware.EDC_RS485_CJ188)
      {
        if (!this.SetPulseoutMode(meter.GetPulseoutMode().Value))
          return false;
        nullable2 = meter.GetPulseoutWidth();
        if (!this.SetPulseoutWidth(nullable2.Value))
          return false;
        nullable2 = meter.GetDepassPeriod();
        if (!this.SetDepassPeriod(nullable2.Value))
          return false;
        nullable2 = meter.GetDepassTimeout();
        if (!this.SetDepassTimeout(nullable2.Value) || !this.SetMbusBaud(meter.GetMbusBaud().Value) || !this.SetMBusListType(meter.GetMBusListType().ToString()) || !this.SetPulseoutPPP(meter.GetPulseoutPPP().Value))
          return false;
      }
      return true;
    }

    private bool OverwriteConstants(EDC_Meter meter)
    {
      if (meter == null)
        return false;
      ushort? nullable1 = meter.GetPulseBlockLimit();
      if (!this.SetPulseBlockLimit(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseLeakLimit();
      if (!this.SetPulseLeakLimit(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseUnleakLimit();
      if (!this.SetPulseUnleakLimit(nullable1.Value))
        return false;
      short? nullable2 = meter.GetPulseLeakLower();
      if (!this.SetPulseLeakLower(nullable2.Value))
        return false;
      nullable2 = meter.GetPulseLeakUpper();
      if (!this.SetPulseLeakUpper(nullable2.Value))
        return false;
      nullable1 = meter.GetPulseBackLimit();
      if (!this.SetPulseBackLimit(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseUnbackLimit();
      if (!this.SetPulseUnbackLimit(nullable1.Value))
        return false;
      nullable1 = meter.GetOversizeDiff();
      if (!this.SetOversizeDiff(nullable1.Value))
        return false;
      nullable1 = meter.GetOversizeLimit();
      if (!this.SetOversizeLimit(nullable1.Value))
        return false;
      nullable1 = meter.GetUndersizeDiff();
      if (!this.SetUndersizeDiff(nullable1.Value))
        return false;
      nullable1 = meter.GetUndersizeLimit();
      if (!this.SetUndersizeLimit(nullable1.Value))
        return false;
      nullable1 = meter.GetBurstDiff();
      if (!this.SetBurstDiff(nullable1.Value))
        return false;
      nullable1 = meter.GetBurstLimit();
      if (!this.SetBurstLimit(nullable1.Value))
        return false;
      byte? nullable3 = meter.GetUARTwatchdog();
      if (!this.SetUARTwatchdog(nullable3.Value))
        return false;
      nullable3 = meter.GetPulseErrorThreshold();
      return this.SetPulseErrorThreshold(nullable3.Value);
    }

    public int? GetMeterValue() => new int?(this.GetParameterValue<int>("pulseReading"));

    public long? GetPulseTotalForwardCount()
    {
      return new long?(this.GetParameterValue<long>("pulseTotalForwardCount64"));
    }

    public DateTime? GetSystemTime()
    {
      DateTime parameterValue = this.GetParameterValue<DateTime>("hwSystemDate");
      return parameterValue == EDC_HandlerFunctions.DateTimeNull ? new DateTime?() : new DateTime?(parameterValue);
    }

    public string GetMBusListType()
    {
      if (this.Version == null)
        return (string) null;
      if (this.Version.Type == EDC_Hardware.EDC_Radio)
        return (this.GetMBusListStructure() ?? throw new ArgumentNullException("The meter has no wireless M-Bus lists!")).GetNameOfSelectedTransmitList();
      if (this.Version.Type == EDC_Hardware.EDC_mBus || this.Version.Type == EDC_Hardware.EDC_ModBus || this.Version.Type == EDC_Hardware.EDC_mBus_Modbus || this.Version.Type == EDC_Hardware.EDC_mBus_CJ188 || this.Version.Type == EDC_Hardware.EDC_RS485_Modbus || this.Version.Type == EDC_Hardware.EDC_RS485_CJ188)
        return "LIST_" + ((char) ((uint) this.GetParameterValue<byte>("cfg_list") + 65U)).ToString();
      throw new NotImplementedException(nameof (GetMBusListType));
    }

    public bool SetMBusListType(string listName)
    {
      if (this.Version == null)
        throw new ArgumentNullException(nameof (listName));
      if (this.Version.Type == EDC_Hardware.EDC_Radio)
      {
        if (string.IsNullOrEmpty(listName))
          throw new ArgumentNullException("Can not change the wireless M-Bus radio list. The name of the radio list can not be null or empty!");
        return this.Map.SetMemoryBytes((this.GetParameter("PARAM_CONST_LIST") ?? throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter PARAM_CONST_LIST.")).Address, BitConverter.GetBytes(((this.GetMBusListStructure() ?? throw new ArgumentNullException("Can not change the wireless M-Bus radio list. The meter does not have any lists!")).Find(listName) ?? throw new Exception("Can not change the wireless M-Bus radio list. New list '" + listName + "' does not exist in the meter!")).StartAddress));
      }
      if (this.Version.Type == EDC_Hardware.EDC_mBus || this.Version.Type == EDC_Hardware.EDC_ModBus || this.Version.Type == EDC_Hardware.EDC_mBus_Modbus || this.Version.Type == EDC_Hardware.EDC_mBus_CJ188 || this.Version.Type == EDC_Hardware.EDC_RS485_Modbus || this.Version.Type == EDC_Hardware.EDC_RS485_CJ188)
        return this.SetParameterValue<byte>("cfg_list", Convert.ToByte((int) Convert.ToChar(listName.Replace("LIST_", string.Empty)) - 65));
      throw new NotImplementedException(nameof (SetMBusListType));
    }

    public MBusListStructure GetMBusListStructure()
    {
      Parameter parameter = this.GetParameter("PARAM_CONST_LIST");
      if (parameter == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter PARAM_CONST_LIST.");
      byte[] memoryBytes = this.Map.GetMemoryBytes(parameter.Address, 512);
      return memoryBytes == null || memoryBytes.Length == 0 ? (MBusListStructure) null : MBusListStructure.Parse(memoryBytes, parameter.Address, 512, this.Version);
    }

    public bool SetRadioMode(RadioMode type)
    {
      return this.SetParameterValue<byte>("cfg_radio_mode", (byte) type);
    }

    public RadioMode? GetRadioMode()
    {
      return new RadioMode?((RadioMode) Enum.ToObject(typeof (RadioMode), this.GetParameterValue<byte>("cfg_radio_mode")));
    }

    public DeviceIdentification GetDeviceIdentification()
    {
      DeviceIdentification deviceIdentification = new DeviceIdentification();
      deviceIdentification.MeterID = this.GetParameterValue<uint>("Con_MeterId");
      deviceIdentification.HardwareTypeID = this.GetParameterValue<uint>("Con_HardwareTypeId");
      deviceIdentification.MeterInfoID = this.GetParameterValue<uint>("Con_MeterInfoId");
      deviceIdentification.BaseTypeID = this.GetParameterValue<uint>("Con_BaseTypeId");
      deviceIdentification.MeterTypeID = this.GetParameterValue<uint>("Con_MeterTypeId");
      deviceIdentification.SapMaterialNumber = this.GetParameterValue<uint>("Con_SAP_MaterialNumber");
      deviceIdentification.SapProductionOrderNumber = this.GetParameterValue<uint>("Con_SAP_ProductionOrderNumber");
      deviceIdentification.IdentificationChecksum = this.GetParameterValue<ushort>("Con_IdentificationChecksum");
      return deviceIdentification.IsChecksumOK && (deviceIdentification.MeterID != 0U || deviceIdentification.HardwareTypeID != 0U || deviceIdentification.MeterInfoID != 0U || deviceIdentification.BaseTypeID != 0U || deviceIdentification.MeterTypeID != 0U || deviceIdentification.SapMaterialNumber != 0U || deviceIdentification.SapProductionOrderNumber > 0U) ? deviceIdentification : (DeviceIdentification) null;
    }

    public bool SetDeviceIdentification(DeviceIdentification ident)
    {
      ident.IdentificationChecksum = ident != null ? ident.CalculateChecksum() : throw new ArgumentException("Can not set device identification! Input parameter 'ident' is null.");
      Parameter parameter = EDC_MemoryMap.GetParameter(this.Version, "Con_MeterId");
      if (parameter == null)
        throw new Exception("Can not set device identification! Parameter 'Con_MeterId' is in this MAP not available.");
      byte[] buffer = ident.Buffer;
      return this.Map.SetMemoryBytes(parameter.Address, buffer);
    }

    public bool SetRadioTransmitInterval(ushort interval)
    {
      return interval >= (ushort) 2 && this.SetParameterValue<ushort>("cfg_radio_normal_basetime", interval);
    }

    public ushort? GetRadioTransmitInterval()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_radio_normal_basetime"));
    }

    public bool SetRadioInstallInterval(ushort interval)
    {
      return interval >= (ushort) 2 ? this.SetParameterValue<ushort>("cfg_radio_install_basetime", interval) : throw new ArgumentOutOfRangeException("The value must be at least 2 to ensure proper device operation.");
    }

    public ushort? GetRadioInstallInterval()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_radio_install_basetime"));
    }

    public bool SetRadioErrorInterval(ushort interval)
    {
      return interval >= (ushort) 2 ? this.SetParameterValue<ushort>("cfg_radio_error_basetime", interval) : throw new ArgumentOutOfRangeException("The value must be at least 2 to ensure proper device operation.");
    }

    public ushort? GetRadioErrorInterval()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_radio_error_basetime"));
    }

    public bool SetRadioInstallCount(byte value)
    {
      return this.SetParameterValue<byte>("cfg_radio_install_count", value);
    }

    public byte? GetRadioInstallCount()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_radio_install_count"));
    }

    public bool SetCoilSampling(bool enable)
    {
      return enable ? this.SetBit<ushort>("cfg_config_flags", (ushort) 1) : this.ClearBit<ushort>("cfg_config_flags", (ushort) 1);
    }

    public bool? GetCoilSampling()
    {
      return new bool?(this.GetBit<ushort>("cfg_config_flags", (ushort) 1));
    }

    public bool SetPulseMultiplier(byte value)
    {
      if (!this.SetParameterValue<byte>("cfg_pulse_multiplier", value))
        return false;
      return this.Version.Type != EDC_Hardware.EDC_Radio || this.SetParameterValue<ushort>("cfg_bewertungsfaktor", (ushort) (1U | (uint) (ushort) ((uint) value << 4)));
    }

    public byte? GetPulseMultiplier()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_pulse_multiplier"));
    }

    public bool SetWarnings(Warning value)
    {
      return this.SetParameterValue<ushort>("persistent_warning_flags", (ushort) value);
    }

    public Warning? GetWarnings()
    {
      return new Warning?((Warning) this.GetParameterValue<ushort>("persistent_warning_flags"));
    }

    public RuntimeFlags? GetRuntimeFlags()
    {
      return new RuntimeFlags?((RuntimeFlags) this.GetParameterValue<ushort>("runtime_flags"));
    }

    public bool SetSensorTimeout(ushort seconds)
    {
      return this.SetParameterValue<ushort>("cfg_sensor_timeout", seconds);
    }

    public ushort? GetSensorTimeout()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_sensor_timeout"));
    }

    public bool SetTimeZone(int valueInQuarterHours)
    {
      if (valueInQuarterHours > 56)
        throw new ArgumentOutOfRangeException("Invalid value of 'Timezone'! Too big. Max. UTC+14:00 (14*4=56), Min. UTC-12:00 (-12*4=-48), Actual value is: " + valueInQuarterHours.ToString());
      return valueInQuarterHours >= -48 ? this.SetParameterValue<byte>("Bak_TimeZoneInQuarterHours", (byte) valueInQuarterHours) : throw new ArgumentOutOfRangeException("Invalid value of 'Timezone'! Too small. Max. UTC+14:00 (14*4=56), Min. UTC-12:00 (-12*4=-48), Actual value is: " + valueInQuarterHours.ToString());
    }

    public int? GetTimeZone()
    {
      byte parameterValue = this.GetParameterValue<byte>("Bak_TimeZoneInQuarterHours");
      int num = ((int) parameterValue & 128) != 128 ? (int) parameterValue : (int) parameterValue | -256;
      return num > 56 || num < -48 ? new int?() : new int?(num);
    }

    public bool SetMediumPrimary(MBusDeviceType type)
    {
      return this.SetParameterValue<byte>("cfg_mbus_medium", (byte) type);
    }

    public MBusDeviceType? GetMediumPrimary()
    {
      return new MBusDeviceType?((MBusDeviceType) Enum.ToObject(typeof (MBusDeviceType), this.GetParameterValue<byte>("cfg_mbus_medium")));
    }

    public bool SetMediumSecondary(MBusDeviceType type)
    {
      return this.SetParameterValue<byte>("cfg_mbus_medium_secondary", (byte) type);
    }

    public MBusDeviceType? GetMediumSecondary()
    {
      return new MBusDeviceType?((MBusDeviceType) Enum.ToObject(typeof (MBusDeviceType), this.GetParameterValue<byte>("cfg_mbus_medium_secondary")));
    }

    public bool SetFrequencyOffset(short value)
    {
      return this.SetParameterValue<short>("cfg_radio_freq_offset", value);
    }

    public short? GetFrequencyOffset()
    {
      return new short?(this.GetParameterValue<short>("cfg_radio_freq_offset"));
    }

    public bool SetMBusAddressPrimary(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_address", value);
    }

    public byte? GetMBusAddressPrimary()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_address"));
    }

    public bool SetObisPrimary(byte value) => this.SetParameterValue<byte>("cfg_obis", value);

    public byte? GetObisPrimary() => new byte?(this.GetParameterValue<byte>("cfg_obis"));

    public bool SetObisSecondary(byte value)
    {
      return this.SetParameterValue<byte>("cfg_obis_secondary", value);
    }

    public byte? GetObisSecondary()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_obis_secondary"));
    }

    public bool SetMBusAddressSecondary(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_address_secondary", value);
    }

    public byte? GetMBusAddressSecondary()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_address_secondary"));
    }

    public bool SetRadioPower(RadioPower value)
    {
      return this.SetParameterValue<byte>("cfg_radio_power", (byte) value);
    }

    public RadioPower? GetRadioPower()
    {
      return new RadioPower?((RadioPower) Enum.ToObject(typeof (RadioPower), this.GetParameterValue<byte>("cfg_radio_power")));
    }

    public bool SetAESkey(object value)
    {
      if (value == null)
        value = (object) new byte[16];
      return value is byte[] ? this.SetAESkey((byte[]) value) : this.SetAESkey(Util.HexStringToByteArray(value.ToString()));
    }

    public bool SetAESkey(byte[] value)
    {
      if (value == null)
        value = new byte[16];
      return value.Length == 16 ? this.SetParameterValue<byte[]>("cfg_key", value) : throw new ArgumentException("The value of AES key has wrong length! Length: " + value.Length.ToString());
    }

    public byte[] GetAESkey() => this.GetParameterValue<byte[]>("cfg_key");

    public bool SetSerialnumberPrimary(uint value)
    {
      return this.SetParameterValue<uint>("cfg_serial_primary", Util.ConvertUnt32ToBcdUInt32(value));
    }

    public uint? GetSerialnumberPrimary()
    {
      return new uint?(Util.ConvertBcdUInt32ToUInt32(this.GetParameterValue<uint>("cfg_serial_primary")));
    }

    public bool SetSerialnumberSecondary(uint value)
    {
      return this.SetParameterValue<uint>("cfg_serial_secondary", Util.ConvertUnt32ToBcdUInt32(value));
    }

    public uint? GetSerialnumberSecondary()
    {
      return new uint?(Util.ConvertBcdUInt32ToUInt32(this.GetParameterValue<uint>("cfg_serial_secondary")));
    }

    public bool SetDueDate(DateTime value)
    {
      if (value.Year < 2000 || value.Year > 2255)
        throw new ArgumentException("Can not set DueDate! The year of new value is invalid (Valid are: 2000-2255). Value: " + value.ToString());
      return this.SetParameterValue<byte>("cfg_stichtag_month", (byte) value.Month) && this.SetParameterValue<byte>("cfg_stichtag_day", (byte) value.Day);
    }

    public DateTime? GetDueDate()
    {
      byte parameterValue1 = this.GetParameterValue<byte>("cfg_stichtag_month");
      byte parameterValue2 = this.GetParameterValue<byte>("cfg_stichtag_day");
      if (parameterValue1 > (byte) 12 || parameterValue2 > (byte) 31)
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      try
      {
        return new DateTime?(new DateTime(2000, (int) parameterValue1, (int) parameterValue2));
      }
      catch
      {
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      }
    }

    public bool SetFlowCheckIntervalState(bool enable)
    {
      return enable ? this.SetBit<ushort>("cfg_config_flags", (ushort) 16) : this.ClearBit<ushort>("cfg_config_flags", (ushort) 16);
    }

    public bool? GetFlowCheckIntervalState()
    {
      return new bool?(this.GetBit<ushort>("cfg_config_flags", (ushort) 16));
    }

    public bool SetMagnetDetectionState(bool enable)
    {
      return enable ? this.SetBit<ushort>("cfg_config_flags", (ushort) 32) : this.ClearBit<ushort>("cfg_config_flags", (ushort) 32);
    }

    public bool? GetMagnetDetectionState()
    {
      return new bool?(this.GetBit<ushort>("cfg_config_flags", (ushort) 32));
    }

    public bool SetRemovalDetectionState(bool isDisabled)
    {
      return isDisabled ? this.SetBit<ushort>("cfg_config_flags", (ushort) 8) : this.ClearBit<ushort>("cfg_config_flags", (ushort) 8);
    }

    public bool? GetRemovalDetectionState()
    {
      return new bool?(this.GetBit<ushort>("cfg_config_flags", (ushort) 8));
    }

    public bool SetDataLoggingState(bool enable)
    {
      return enable ? this.SetBit<ushort>("cfg_config_flags", (ushort) 4) : this.ClearBit<ushort>("cfg_config_flags", (ushort) 4);
    }

    public bool? GetDataLoggingState()
    {
      return new bool?(this.GetBit<ushort>("cfg_config_flags", (ushort) 4));
    }

    public bool SetRadioState(bool enable)
    {
      return enable ? this.SetBit<ushort>("cfg_config_flags", (ushort) 2) : this.ClearBit<ushort>("cfg_config_flags", (ushort) 2);
    }

    public bool? GetRadioState()
    {
      return this.Version == null || this.Version.Type != EDC_Hardware.EDC_Radio ? new bool?() : new bool?(this.GetBit<ushort>("cfg_config_flags", (ushort) 2));
    }

    public bool SetPulseActivateRadio(byte value)
    {
      return this.SetParameterValue<byte>("cfg_pulse_activate", value);
    }

    public byte? GetPulseActivateRadio()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_pulse_activate"));
    }

    public ushort? GetBewertungsfaktor()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_bewertungsfaktor"));
    }

    public bool SetPulsePeriod(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_period", value);
    }

    public ushort? GetPulsePeriod()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_period"));
    }

    public bool SetMBusGenerationPrimary(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_version", value);
    }

    public byte? GetMBusGenerationPrimary()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_version"));
    }

    public bool SetWMBusLongHeaderState(bool enable)
    {
      return enable ? this.SetBit<byte>("cfg_radio_flags", (byte) 1) : this.ClearBit<byte>("cfg_radio_flags", (byte) 1);
    }

    public bool? GetWMBusLongHeaderState()
    {
      return new bool?(this.GetBit<byte>("cfg_radio_flags", (byte) 1));
    }

    public bool SetWMBusEncryptionState(bool enable)
    {
      return enable ? this.SetBit<byte>("cfg_radio_flags", (byte) 2) : this.ClearBit<byte>("cfg_radio_flags", (byte) 2);
    }

    public bool? GetWMBusEncryptionState()
    {
      return new bool?(this.GetBit<byte>("cfg_radio_flags", (byte) 2));
    }

    public bool SetWMBusSynchronousTransmissioModeState(bool enable)
    {
      return enable ? this.SetBit<byte>("cfg_radio_flags", (byte) 4) : this.ClearBit<byte>("cfg_radio_flags", (byte) 4);
    }

    public bool? GetWMBusSynchronousTransmissioModeState()
    {
      return new bool?(this.GetBit<byte>("cfg_radio_flags", (byte) 4));
    }

    public bool SetWMBusInstallationPacketsState(bool enable)
    {
      return enable ? this.SetBit<byte>("cfg_radio_flags", (byte) 8) : this.ClearBit<byte>("cfg_radio_flags", (byte) 8);
    }

    public bool? GetWMBusInstallationPacketsState()
    {
      return new bool?(this.GetBit<byte>("cfg_radio_flags", (byte) 8));
    }

    public bool SetManufacturerPrimary(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set manufacturer! Input parameter 'value' is null.");
      if (value == "ZENNER")
        value = "ZRI";
      return value.Length == 3 ? this.SetParameterValue<ushort>("cfg_mbus_manid", MBusDevice.GetManufacturerCode(value)) : throw new ArgumentException("Can not set manufacturer! The length of input parameter 'value' is not 3 chars.");
    }

    public string GetManufacturerPrimary()
    {
      return MBusDevice.GetManufacturer((short) this.GetParameterValue<ushort>("cfg_mbus_manid"));
    }

    public bool SetSerialnumberFull(string value)
    {
      if (string.IsNullOrEmpty(value))
        return this.SetParameterValue<byte[]>("Con_fullserialnumber", new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        });
      if (value.Length != 14)
        throw new ArgumentException("Wrong length of the full serialnumber detected! Expected 14, Value: " + value);
      string s = value[0] == 'E' ? value.Substring(0, 1) : throw new ArgumentException("Wrong start char of the full serialnumber detected! Expected E, Actual: " + value[0].ToString());
      string Manufacturer = value.Substring(1, 3);
      byte num = byte.Parse(value.Substring(4, 2));
      string str = value.Substring(6);
      if (num > (byte) 99)
        throw new ArgumentOutOfRangeException("Wrong generation in full serialnumber detected! Valid range are: 0-99");
      if (!Util.IsValidBCD(str))
        throw new ArgumentException("Wrong full serialnumber detected! The last 8 chars should contains only numbers. Value: " + value);
      List<byte> byteList = new List<byte>(14);
      byteList.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(s));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(MBusDevice.GetManufacturerCode(Manufacturer)));
      byteList.Add(num);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(Util.ConvertUnt32ToBcdUInt32(uint.Parse(str))));
      return byteList.Count == 8 ? this.SetParameterValue<byte[]>("Con_fullserialnumber", byteList.ToArray()) : throw new ArgumentOutOfRangeException("Wrong full serialnumber was generated!");
    }

    public string GetSerialnumberFull()
    {
      byte[] parameterValue = this.GetParameterValue<byte[]>("Con_fullserialnumber");
      if (parameterValue == null || parameterValue.Length != 8)
        throw new ArgumentException("The parameter 'Con_fullserialnumber' is invalid!");
      if (parameterValue[0] == byte.MaxValue && parameterValue[1] == byte.MaxValue && parameterValue[2] == byte.MaxValue && parameterValue[3] == byte.MaxValue && parameterValue[4] == byte.MaxValue && parameterValue[5] == byte.MaxValue && parameterValue[6] == byte.MaxValue && parameterValue[7] == byte.MaxValue)
        return string.Empty;
      if (parameterValue[0] == (byte) 0 && parameterValue[1] == (byte) 0 && parameterValue[2] == (byte) 0 && parameterValue[3] == (byte) 0 && parameterValue[4] == (byte) 0 && parameterValue[5] == (byte) 0 && parameterValue[6] == (byte) 0 && parameterValue[7] == (byte) 0)
        return string.Empty;
      try
      {
        return string.Format("{0}{1}{2:00}{3}", (object) Encoding.ASCII.GetString(parameterValue, 0, 1), (object) MBusDevice.GetManufacturer(BitConverter.ToInt16(parameterValue, 1)), (object) parameterValue[3], (object) BitConverter.ToUInt32(parameterValue, 4).ToString("X8"));
      }
      catch
      {
        return string.Empty;
      }
    }

    public bool SetSerialnumberRadioMinol(uint value)
    {
      return this.SetParameterValue<uint>("cfg_serial_radio_minol", Util.ConvertUnt32ToBcdUInt32(value));
    }

    public uint? GetSerialnumberRadioMinol()
    {
      uint parameterValue = this.GetParameterValue<uint>("cfg_serial_radio_minol");
      if (parameterValue == uint.MaxValue)
        return new uint?();
      uint uint32 = Util.ConvertBcdUInt32ToUInt32(parameterValue);
      return uint32 > 99999999U ? new uint?() : new uint?(uint32);
    }

    public bool SetManufacturerSecondary(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set secondary manufacturer! Input parameter 'value' is null.");
      if (value == "ZENNER")
        value = "ZRI";
      return value.Length == 3 ? this.SetParameterValue<ushort>("cfg_mbus_manid_secondary", MBusDevice.GetManufacturerCode(value)) : throw new ArgumentException("Can not set secondary manufacturer! The length of input parameter 'value' is not 3 chars.");
    }

    public string GetManufacturerSecondary()
    {
      return MBusDevice.GetManufacturer((short) this.GetParameterValue<ushort>("cfg_mbus_manid_secondary"));
    }

    public bool SetMBusGenerationSecondary(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_version_secondary", value);
    }

    public byte? GetMBusGenerationSecondary()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_version_secondary"));
    }

    public bool SetCogCount(byte value) => this.SetParameterValue<byte>("cfg_cog_count", value);

    public byte? GetCogCount() => new byte?(this.GetParameterValue<byte>("cfg_cog_count"));

    public bool SetRadioScenario(RadioProtocol value)
    {
      return value != RadioProtocol.Undefined && this.SetParameterValue<byte>("cfg_radio_scenario", (byte) value);
    }

    public RadioProtocol? GetRadioScenario()
    {
      return new RadioProtocol?((RadioProtocol) Enum.ToObject(typeof (RadioProtocol), this.GetParameterValue<byte>("cfg_radio_scenario")));
    }

    public bool SetPulseoutMode(PulseoutMode value)
    {
      return this.SetParameterValue<byte>("cfg_pulseout_mode", (byte) value);
    }

    public PulseoutMode? GetPulseoutMode()
    {
      return new PulseoutMode?((PulseoutMode) Enum.ToObject(typeof (PulseoutMode), this.GetParameterValue<byte>("cfg_pulseout_mode")));
    }

    public bool SetMbusBaud(MbusBaud value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_baud", (byte) value);
    }

    public MbusBaud? GetMbusBaud()
    {
      return new MbusBaud?((MbusBaud) Enum.ToObject(typeof (MbusBaud), this.GetParameterValue<byte>("cfg_mbus_baud")));
    }

    public bool SetParity(MbusCNParity value)
    {
      return this.SetParameterValue<byte>("cfg_protocol_Parity", (byte) value);
    }

    public MbusCNParity? GetParity()
    {
      return new MbusCNParity?((MbusCNParity) Enum.ToObject(typeof (MbusCNParity), this.GetParameterValue<byte>("cfg_protocol_Parity")));
    }

    public bool SetPulseoutWidth(ushort value)
    {
      if (this.Version == null || this.Version.Type != EDC_Hardware.EDC_mBus && this.Version.Type != EDC_Hardware.EDC_ModBus && this.Version.Type != EDC_Hardware.EDC_mBus_Modbus && this.Version.Type != EDC_Hardware.EDC_mBus_CJ188 && this.Version.Type != EDC_Hardware.EDC_RS485_Modbus && this.Version.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotSupportedException("MBus functions not supported!");
      return this.SetParameterValue<ushort>("cfg_pulseout_width", value);
    }

    public ushort? GetPulseoutWidth()
    {
      if (this.Version == null || this.Version.Type != EDC_Hardware.EDC_mBus && this.Version.Type != EDC_Hardware.EDC_ModBus && this.Version.Type != EDC_Hardware.EDC_mBus_Modbus && this.Version.Type != EDC_Hardware.EDC_mBus_CJ188 && this.Version.Type != EDC_Hardware.EDC_RS485_Modbus && this.Version.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotSupportedException("MBus functions not supported!");
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulseout_width"));
    }

    public bool SetPulseoutPPP(short value)
    {
      return this.SetParameterValue<short>("cfg_pulseout_ppp", value);
    }

    public short? GetPulseoutPPP() => new short?(this.GetParameterValue<short>("cfg_pulseout_ppp"));

    public bool SetDepassPeriod(ushort value)
    {
      if (this.Version == null || this.Version.Type != EDC_Hardware.EDC_mBus && this.Version.Type != EDC_Hardware.EDC_ModBus && this.Version.Type != EDC_Hardware.EDC_mBus_Modbus && this.Version.Type != EDC_Hardware.EDC_mBus_CJ188 && this.Version.Type != EDC_Hardware.EDC_RS485_Modbus && this.Version.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotSupportedException("MBus functions not supported!");
      return this.SetParameterValue<ushort>("cfg_depass_period", value);
    }

    public ushort? GetDepassPeriod()
    {
      if (this.Version == null || this.Version.Type != EDC_Hardware.EDC_mBus && this.Version.Type != EDC_Hardware.EDC_ModBus && this.Version.Type != EDC_Hardware.EDC_mBus_Modbus && this.Version.Type != EDC_Hardware.EDC_mBus_CJ188 && this.Version.Type != EDC_Hardware.EDC_RS485_Modbus && this.Version.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotSupportedException("MBus functions not supported!");
      return new ushort?(this.GetParameterValue<ushort>("cfg_depass_period"));
    }

    public double GetVolumeAccumulatedNegativ()
    {
      return (double) this.GetParameterValue<uint>("pulseTotalReturnCount") / 4.0 * (double) this.GetParameterValue<byte>("cfg_pulse_multiplier");
    }

    public void SetNominalFlow(string value)
    {
      if (string.IsNullOrEmpty(value))
        return;
      byte? pulseMultiplier = this.GetPulseMultiplier();
      byte? nullable1 = pulseMultiplier;
      int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      int num1 = 1;
      if (nullable2.GetValueOrDefault() == num1 & nullable2.HasValue)
      {
        double num2 = 2.5;
        if (num2.ToString() == value)
          this.SetValuesFromNominalFlow((ushort) 62, (ushort) 625, (ushort) 187);
        if (4.ToString() == value)
          this.SetValuesFromNominalFlow((ushort) 100, (ushort) 1000, (ushort) 300);
        num2 = 6.3;
        if (num2.ToString() == value)
          this.SetValuesFromNominalFlow((ushort) 157, (ushort) 1575, (ushort) 472);
        if (10.ToString() == value)
          this.SetValuesFromNominalFlow((ushort) 250, (ushort) 2500, (ushort) 750);
        if (16.ToString() == value)
          this.SetValuesFromNominalFlow((ushort) 400, (ushort) 4000, (ushort) 1200);
        if (!(25.ToString() == value))
          return;
        this.SetValuesFromNominalFlow((ushort) 625, (ushort) 6250, (ushort) 1875);
      }
      else
      {
        nullable1 = pulseMultiplier;
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        int num3 = 10;
        if (nullable2.GetValueOrDefault() == num3 & nullable2.HasValue)
        {
          int num4 = 25;
          if (num4.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 62, (ushort) 625, (ushort) 187);
          num4 = 40;
          if (num4.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 100, (ushort) 1000, (ushort) 300);
          num4 = 63;
          if (num4.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 157, (ushort) 1575, (ushort) 472);
          num4 = 100;
          if (!(num4.ToString() == value))
            return;
          this.SetValuesFromNominalFlow((ushort) 250, (ushort) 2500, (ushort) 750);
        }
        else
        {
          nullable1 = pulseMultiplier;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num5 = 100;
          if (!(nullable2.GetValueOrDefault() == num5 & nullable2.HasValue))
            return;
          int num6 = 160;
          if (num6.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 40, (ushort) 400, (ushort) 120);
          num6 = 250;
          if (num6.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 62, (ushort) 625, (ushort) 187);
          num6 = 400;
          if (num6.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 100, (ushort) 1000, (ushort) 300);
          num6 = 630;
          if (num6.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 157, (ushort) 1575, (ushort) 472);
          num6 = 1000;
          if (num6.ToString() == value)
            this.SetValuesFromNominalFlow((ushort) 250, (ushort) 2500, (ushort) 750);
        }
      }
    }

    private void SetValuesFromNominalFlow(
      ushort oversizeDiff,
      ushort undersizeDiff,
      ushort burstDiff)
    {
      this.SetOversizeDiff(oversizeDiff);
      this.SetUndersizeDiff(undersizeDiff);
      this.SetBurstDiff(burstDiff);
    }

    public string GetNominalFlow()
    {
      byte? pulseMultiplier = this.GetPulseMultiplier();
      ushort? oversizeDiff = this.GetOversizeDiff();
      ushort? undersizeDiff = this.GetUndersizeDiff();
      ushort? burstDiff = this.GetBurstDiff();
      byte? nullable1 = pulseMultiplier;
      int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      int num1 = 1;
      if (nullable2.GetValueOrDefault() == num1 & nullable2.HasValue)
      {
        ushort? nullable3 = oversizeDiff;
        nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        int num2 = 62;
        int num3;
        if (nullable2.GetValueOrDefault() == num2 & nullable2.HasValue)
        {
          nullable3 = undersizeDiff;
          nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num4 = 625;
          if (nullable2.GetValueOrDefault() == num4 & nullable2.HasValue)
          {
            nullable3 = burstDiff;
            nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num5 = 187;
            num3 = nullable2.GetValueOrDefault() == num5 & nullable2.HasValue ? 1 : 0;
            goto label_5;
          }
        }
        num3 = 0;
label_5:
        if (num3 != 0)
          return 2.5.ToString();
        nullable3 = oversizeDiff;
        nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        int num6 = 100;
        int num7;
        if (nullable2.GetValueOrDefault() == num6 & nullable2.HasValue)
        {
          nullable3 = undersizeDiff;
          nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num8 = 1000;
          if (nullable2.GetValueOrDefault() == num8 & nullable2.HasValue)
          {
            nullable3 = burstDiff;
            nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num9 = 300;
            num7 = nullable2.GetValueOrDefault() == num9 & nullable2.HasValue ? 1 : 0;
            goto label_11;
          }
        }
        num7 = 0;
label_11:
        if (num7 != 0)
          return 4.ToString();
        nullable3 = oversizeDiff;
        nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        int num10 = 157;
        int num11;
        if (nullable2.GetValueOrDefault() == num10 & nullable2.HasValue)
        {
          nullable3 = undersizeDiff;
          nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num12 = 1575;
          if (nullable2.GetValueOrDefault() == num12 & nullable2.HasValue)
          {
            nullable3 = burstDiff;
            nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num13 = 472;
            num11 = nullable2.GetValueOrDefault() == num13 & nullable2.HasValue ? 1 : 0;
            goto label_17;
          }
        }
        num11 = 0;
label_17:
        if (num11 != 0)
          return 6.3.ToString();
        nullable3 = oversizeDiff;
        nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        int num14 = 250;
        int num15;
        if (nullable2.GetValueOrDefault() == num14 & nullable2.HasValue)
        {
          nullable3 = undersizeDiff;
          nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num16 = 2500;
          if (nullable2.GetValueOrDefault() == num16 & nullable2.HasValue)
          {
            nullable3 = burstDiff;
            nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num17 = 750;
            num15 = nullable2.GetValueOrDefault() == num17 & nullable2.HasValue ? 1 : 0;
            goto label_23;
          }
        }
        num15 = 0;
label_23:
        if (num15 != 0)
          return 10.ToString();
        nullable3 = oversizeDiff;
        nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        int num18 = 400;
        int num19;
        if (nullable2.GetValueOrDefault() == num18 & nullable2.HasValue)
        {
          nullable3 = undersizeDiff;
          nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num20 = 4000;
          if (nullable2.GetValueOrDefault() == num20 & nullable2.HasValue)
          {
            nullable3 = burstDiff;
            nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num21 = 1200;
            num19 = nullable2.GetValueOrDefault() == num21 & nullable2.HasValue ? 1 : 0;
            goto label_29;
          }
        }
        num19 = 0;
label_29:
        if (num19 != 0)
          return 16.ToString();
        nullable3 = oversizeDiff;
        nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        int num22 = 625;
        int num23;
        if (nullable2.GetValueOrDefault() == num22 & nullable2.HasValue)
        {
          nullable3 = undersizeDiff;
          nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          int num24 = 6250;
          if (nullable2.GetValueOrDefault() == num24 & nullable2.HasValue)
          {
            nullable3 = burstDiff;
            nullable2 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int num25 = 1875;
            num23 = nullable2.GetValueOrDefault() == num25 & nullable2.HasValue ? 1 : 0;
            goto label_35;
          }
        }
        num23 = 0;
label_35:
        if (num23 != 0)
          return 25.ToString();
      }
      else
      {
        nullable1 = pulseMultiplier;
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        int num26 = 10;
        if (nullable2.GetValueOrDefault() == num26 & nullable2.HasValue)
        {
          ushort? nullable4 = oversizeDiff;
          nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
          int num27 = 62;
          int num28;
          if (nullable2.GetValueOrDefault() == num27 & nullable2.HasValue)
          {
            nullable4 = undersizeDiff;
            nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
            int num29 = 625;
            if (nullable2.GetValueOrDefault() == num29 & nullable2.HasValue)
            {
              nullable4 = burstDiff;
              nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
              int num30 = 187;
              num28 = nullable2.GetValueOrDefault() == num30 & nullable2.HasValue ? 1 : 0;
              goto label_42;
            }
          }
          num28 = 0;
label_42:
          if (num28 != 0)
            return 25.ToString();
          nullable4 = oversizeDiff;
          nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
          int num31 = 100;
          int num32;
          if (nullable2.GetValueOrDefault() == num31 & nullable2.HasValue)
          {
            nullable4 = undersizeDiff;
            nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
            int num33 = 1000;
            if (nullable2.GetValueOrDefault() == num33 & nullable2.HasValue)
            {
              nullable4 = burstDiff;
              nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
              int num34 = 300;
              num32 = nullable2.GetValueOrDefault() == num34 & nullable2.HasValue ? 1 : 0;
              goto label_48;
            }
          }
          num32 = 0;
label_48:
          if (num32 != 0)
            return 40.ToString();
          nullable4 = oversizeDiff;
          nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
          int num35 = 157;
          int num36;
          if (nullable2.GetValueOrDefault() == num35 & nullable2.HasValue)
          {
            nullable4 = undersizeDiff;
            nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
            int num37 = 1575;
            if (nullable2.GetValueOrDefault() == num37 & nullable2.HasValue)
            {
              nullable4 = burstDiff;
              nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
              int num38 = 472;
              num36 = nullable2.GetValueOrDefault() == num38 & nullable2.HasValue ? 1 : 0;
              goto label_54;
            }
          }
          num36 = 0;
label_54:
          if (num36 != 0)
            return 63.ToString();
          nullable4 = oversizeDiff;
          nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
          int num39 = 250;
          int num40;
          if (nullable2.GetValueOrDefault() == num39 & nullable2.HasValue)
          {
            nullable4 = undersizeDiff;
            nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
            int num41 = 2500;
            if (nullable2.GetValueOrDefault() == num41 & nullable2.HasValue)
            {
              nullable4 = burstDiff;
              nullable2 = nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?();
              int num42 = 750;
              num40 = nullable2.GetValueOrDefault() == num42 & nullable2.HasValue ? 1 : 0;
              goto label_60;
            }
          }
          num40 = 0;
label_60:
          if (num40 != 0)
            return 100.ToString();
        }
        else
        {
          nullable1 = pulseMultiplier;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num43 = 100;
          if (nullable2.GetValueOrDefault() == num43 & nullable2.HasValue)
          {
            ushort? nullable5 = oversizeDiff;
            nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num44 = 40;
            int num45;
            if (nullable2.GetValueOrDefault() == num44 & nullable2.HasValue)
            {
              nullable5 = undersizeDiff;
              nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num46 = 400;
              if (nullable2.GetValueOrDefault() == num46 & nullable2.HasValue)
              {
                nullable5 = burstDiff;
                nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
                int num47 = 120;
                num45 = nullable2.GetValueOrDefault() == num47 & nullable2.HasValue ? 1 : 0;
                goto label_67;
              }
            }
            num45 = 0;
label_67:
            if (num45 != 0)
              return 160.ToString();
            nullable5 = oversizeDiff;
            nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num48 = 62;
            int num49;
            if (nullable2.GetValueOrDefault() == num48 & nullable2.HasValue)
            {
              nullable5 = undersizeDiff;
              nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num50 = 625;
              if (nullable2.GetValueOrDefault() == num50 & nullable2.HasValue)
              {
                nullable5 = burstDiff;
                nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
                int num51 = 187;
                num49 = nullable2.GetValueOrDefault() == num51 & nullable2.HasValue ? 1 : 0;
                goto label_73;
              }
            }
            num49 = 0;
label_73:
            if (num49 != 0)
              return 250.ToString();
            nullable5 = oversizeDiff;
            nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num52 = 100;
            int num53;
            if (nullable2.GetValueOrDefault() == num52 & nullable2.HasValue)
            {
              nullable5 = undersizeDiff;
              nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num54 = 1000;
              if (nullable2.GetValueOrDefault() == num54 & nullable2.HasValue)
              {
                nullable5 = burstDiff;
                nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
                int num55 = 300;
                num53 = nullable2.GetValueOrDefault() == num55 & nullable2.HasValue ? 1 : 0;
                goto label_79;
              }
            }
            num53 = 0;
label_79:
            if (num53 != 0)
              return 400.ToString();
            nullable5 = oversizeDiff;
            nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num56 = 157;
            int num57;
            if (nullable2.GetValueOrDefault() == num56 & nullable2.HasValue)
            {
              nullable5 = undersizeDiff;
              nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num58 = 1575;
              if (nullable2.GetValueOrDefault() == num58 & nullable2.HasValue)
              {
                nullable5 = burstDiff;
                nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
                int num59 = 472;
                num57 = nullable2.GetValueOrDefault() == num59 & nullable2.HasValue ? 1 : 0;
                goto label_85;
              }
            }
            num57 = 0;
label_85:
            if (num57 != 0)
              return 630.ToString();
            nullable5 = oversizeDiff;
            nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num60 = 250;
            int num61;
            if (nullable2.GetValueOrDefault() == num60 & nullable2.HasValue)
            {
              nullable5 = undersizeDiff;
              nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num62 = 2500;
              if (nullable2.GetValueOrDefault() == num62 & nullable2.HasValue)
              {
                nullable5 = burstDiff;
                nullable2 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
                int num63 = 750;
                num61 = nullable2.GetValueOrDefault() == num63 & nullable2.HasValue ? 1 : 0;
                goto label_91;
              }
            }
            num61 = 0;
label_91:
            if (num61 != 0)
              return 1000.ToString();
          }
        }
      }
      return string.Empty;
    }

    public string[] GetNominalFlowAllowedValues()
    {
      byte? pulseMultiplier = this.GetPulseMultiplier();
      byte? nullable1 = pulseMultiplier;
      int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      int num1 = 1;
      if (nullable2.GetValueOrDefault() == num1 & nullable2.HasValue)
      {
        string[] flowAllowedValues = new string[7]
        {
          2.5.ToString(),
          4.ToString(),
          6.3.ToString(),
          null,
          null,
          null,
          null
        };
        int num2 = 10;
        flowAllowedValues[3] = num2.ToString();
        num2 = 16;
        flowAllowedValues[4] = num2.ToString();
        num2 = 25;
        flowAllowedValues[5] = num2.ToString();
        flowAllowedValues[6] = string.Empty;
        return flowAllowedValues;
      }
      nullable1 = pulseMultiplier;
      nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      int num3 = 10;
      if (nullable2.GetValueOrDefault() == num3 & nullable2.HasValue)
        return new string[5]
        {
          25.ToString(),
          40.ToString(),
          63.ToString(),
          100.ToString(),
          string.Empty
        };
      nullable1 = pulseMultiplier;
      nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      int num4 = 100;
      if (!(nullable2.GetValueOrDefault() == num4 & nullable2.HasValue))
        return new string[0];
      string[] flowAllowedValues1 = new string[6]
      {
        160.ToString(),
        250.ToString(),
        400.ToString(),
        null,
        null,
        null
      };
      int num5 = 630;
      flowAllowedValues1[3] = num5.ToString();
      num5 = 1000;
      flowAllowedValues1[4] = num5.ToString();
      flowAllowedValues1[5] = string.Empty;
      return flowAllowedValues1;
    }

    public bool SetDepassTimeout(ushort value)
    {
      if (this.Version == null || this.Version.Type != EDC_Hardware.EDC_mBus && this.Version.Type != EDC_Hardware.EDC_ModBus && this.Version.Type != EDC_Hardware.EDC_mBus_Modbus && this.Version.Type != EDC_Hardware.EDC_mBus_CJ188 && this.Version.Type != EDC_Hardware.EDC_RS485_Modbus && this.Version.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotSupportedException("MBus functions not supported!");
      return this.SetParameterValue<ushort>("cfg_depass_timeout", value);
    }

    public ushort? GetDepassTimeout()
    {
      if (this.Version == null || this.Version.Type != EDC_Hardware.EDC_mBus && this.Version.Type != EDC_Hardware.EDC_ModBus && this.Version.Type != EDC_Hardware.EDC_mBus_Modbus && this.Version.Type != EDC_Hardware.EDC_mBus_CJ188 && this.Version.Type != EDC_Hardware.EDC_RS485_Modbus && this.Version.Type != EDC_Hardware.EDC_RS485_CJ188)
        throw new NotSupportedException("MBus functions not supported!");
      return new ushort?(this.GetParameterValue<ushort>("cfg_depass_timeout"));
    }

    public bool SetPulseBlockLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_block_limit", value);
    }

    public ushort? GetPulseBlockLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_block_limit"));
    }

    public bool SetPulseLeakLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_leak_limit", value);
    }

    public ushort? GetPulseLeakLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_leak_limit"));
    }

    public bool SetPulseUnleakLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_unleak_limit", value);
    }

    public ushort? GetPulseUnleakLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_unleak_limit"));
    }

    public bool SetPulseLeakLower(short value)
    {
      return this.SetParameterValue<short>("cfg_pulse_leak_lower", value);
    }

    public short? GetPulseLeakLower()
    {
      return new short?(this.GetParameterValue<short>("cfg_pulse_leak_lower"));
    }

    public bool SetPulseLeakUpper(short value)
    {
      return this.SetParameterValue<short>("cfg_pulse_leak_upper", value);
    }

    public short? GetPulseLeakUpper()
    {
      return new short?(this.GetParameterValue<short>("cfg_pulse_leak_upper"));
    }

    public bool SetPulseBackLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_back_limit", value);
    }

    public ushort? GetPulseBackLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_back_limit"));
    }

    public bool SetPulseUnbackLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_unback_limit", value);
    }

    public ushort? GetPulseUnbackLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_unback_limit"));
    }

    public bool SetHardwareErrors(HardwareError value)
    {
      return this.SetParameterValue<ushort>("hwStatusFlags", (ushort) value);
    }

    public HardwareError? GetHardwareErrors()
    {
      return new HardwareError?((HardwareError) this.GetParameterValue<ushort>("hwStatusFlags"));
    }

    public bool SetOversizeDiff(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_oversize_diff", value);
    }

    public ushort? GetOversizeDiff()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_oversize_diff"));
    }

    public bool SetOversizeLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_oversize_limit", value);
    }

    public ushort? GetOversizeLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_oversize_limit"));
    }

    public bool SetUndersizeDiff(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_undersize_diff", value);
    }

    public ushort? GetUndersizeDiff()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_undersize_diff"));
    }

    public bool SetUndersizeLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_undersize_limit", value);
    }

    public ushort? GetUndersizeLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_undersize_limit"));
    }

    public bool SetBurstDiff(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_burst_diff", value);
    }

    public ushort? GetBurstDiff() => new ushort?(this.GetParameterValue<ushort>("cfg_burst_diff"));

    public bool SetBurstLimit(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_burst_limit", value);
    }

    public ushort? GetBurstLimit()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_burst_limit"));
    }

    public bool SetUARTwatchdog(byte value)
    {
      return this.SetParameterValue<byte>("cfg_uart_watchdog", value);
    }

    public byte? GetUARTwatchdog() => new byte?(this.GetParameterValue<byte>("cfg_uart_watchdog"));

    public bool SetPulseErrorThreshold(byte value)
    {
      return this.SetParameterValue<byte>("cfg_pulse_error_threshold", value);
    }

    public byte? GetPulseErrorThreshold()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_pulse_error_threshold"));
    }

    public bool? GetDeviceErrorState()
    {
      Warning? warnings = this.GetWarnings();
      if (!warnings.HasValue)
        return new bool?();
      HardwareError? hardwareErrors = this.GetHardwareErrors();
      return !hardwareErrors.HasValue ? new bool?() : new bool?((hardwareErrors.Value & HardwareError.COILFAIL) == HardwareError.COILFAIL || (warnings.Value & Warning.ABNORMAL) == Warning.ABNORMAL || (warnings.Value & Warning.PERMANENT_ERROR) == Warning.PERMANENT_ERROR);
    }

    public bool SetBatteryEndDate(DateTime value)
    {
      if (value.Year < 2000 || value.Year > 2255)
        throw new ArgumentException("Can not set BatteryEndDate! The year of new value is invalid (Valid are: 2000-2255). Value: " + value.ToString());
      if (this.GetParameter("cfg_lowbatt_year") == null)
        throw new Exception("The firmware " + this.Version?.ToString() + " not supports BatteryEndDate parameter!");
      return this.SetParameterValue<byte>("cfg_lowbatt_year", (byte) (value.Year - 2000)) && this.SetParameterValue<byte>("cfg_lowbatt_month", (byte) value.Month) && this.SetParameterValue<byte>("cfg_lowbatt_day", (byte) value.Day);
    }

    public DateTime? GetBatteryEndDate()
    {
      if (this.GetParameter("cfg_lowbatt_year") == null)
        throw new Exception("The firmware " + this.Version?.ToString() + " not supports BatteryEndDate parameter!");
      byte parameterValue1 = this.GetParameterValue<byte>("cfg_lowbatt_year");
      byte parameterValue2 = this.GetParameterValue<byte>("cfg_lowbatt_month");
      byte parameterValue3 = this.GetParameterValue<byte>("cfg_lowbatt_day");
      if (parameterValue1 == byte.MaxValue || parameterValue2 == byte.MaxValue || parameterValue3 == byte.MaxValue)
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      if (parameterValue1 > byte.MaxValue || parameterValue2 > (byte) 12 || parameterValue3 > (byte) 31)
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      try
      {
        return new DateTime?(new DateTime(2000 + (int) parameterValue1, (int) parameterValue2, (int) parameterValue3));
      }
      catch
      {
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      }
    }

    public bool SetCoilErrorThreshold(sbyte value)
    {
      return this.SetParameterValue<sbyte>("cfg_coil_error_threshold", value);
    }

    public sbyte? GetCoilErrorThreshold()
    {
      return new sbyte?(this.GetParameterValue<sbyte>("cfg_coil_error_threshold"));
    }

    public bool SetCoilMaxThreshold(sbyte value)
    {
      return this.SetParameterValue<sbyte>("cfg_coil_max_threshold", value);
    }

    public sbyte? GetCoilMaxThreshold()
    {
      return new sbyte?(this.GetParameterValue<sbyte>("cfg_coil_max_threshold"));
    }

    public bool SetCoilMinThreshold(sbyte value)
    {
      return this.SetParameterValue<sbyte>("cfg_coil_min_threshold", value);
    }

    public sbyte? GetCoilMinThreshold()
    {
      return new sbyte?(this.GetParameterValue<sbyte>("cfg_coil_min_threshold"));
    }

    public bool SetCoilAmplitudeLimit(sbyte value)
    {
      return this.SetParameterValue<sbyte>("cfg_coil_amplitude_limit", value);
    }

    public sbyte? GetCoilAmplitudeLimit()
    {
      return new sbyte?(this.GetParameterValue<sbyte>("cfg_coil_amplitude_limit"));
    }

    public bool SetCoilB_offset(sbyte value)
    {
      return this.SetParameterValue<sbyte>("cfg_coil_b_offset", value);
    }

    public sbyte? GetCoilB_offset()
    {
      return new sbyte?(this.GetParameterValue<sbyte>("cfg_coil_b_offset"));
    }

    public List<FlashLoggerEntry> GetLoggerData() => LoggerManager.ParseFlashLogger(this);

    public bool SetStartModule(DateTime value)
    {
      if (value == EDC_HandlerFunctions.DateTimeNull)
        return this.SetParameterValue<byte>("cfg_start_module_year", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_module_month", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_module_day", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_module_hour", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_module_minute", byte.MaxValue);
      if (value.Year < 2000 || value.Year > 2255)
        throw new ArgumentException("Can not set StartModule! The year of new value is invalid (Valid are: 2000-2255). Value: " + value.ToString());
      return this.SetParameterValue<byte>("cfg_start_module_year", (byte) (value.Year - 2000)) && this.SetParameterValue<byte>("cfg_start_module_month", (byte) value.Month) && this.SetParameterValue<byte>("cfg_start_module_day", (byte) value.Day) && this.SetParameterValue<byte>("cfg_start_module_hour", (byte) value.Hour) && this.SetParameterValue<byte>("cfg_start_module_minute", (byte) value.Minute);
    }

    public DateTime? GetStartModule()
    {
      byte parameterValue1 = this.GetParameterValue<byte>("cfg_start_module_year");
      byte parameterValue2 = this.GetParameterValue<byte>("cfg_start_module_month");
      byte parameterValue3 = this.GetParameterValue<byte>("cfg_start_module_day");
      byte parameterValue4 = this.GetParameterValue<byte>("cfg_start_module_hour");
      byte parameterValue5 = this.GetParameterValue<byte>("cfg_start_module_minute");
      if (parameterValue1 == byte.MaxValue || parameterValue2 == byte.MaxValue || parameterValue3 == byte.MaxValue || parameterValue4 == byte.MaxValue || parameterValue5 == byte.MaxValue)
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      if (parameterValue1 > byte.MaxValue || parameterValue2 > (byte) 12 || parameterValue3 > (byte) 31 || parameterValue4 > (byte) 23 || parameterValue5 > (byte) 59)
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      try
      {
        return new DateTime?(new DateTime(2000 + (int) parameterValue1, (int) parameterValue2, (int) parameterValue3, (int) parameterValue4, (int) parameterValue5, 1));
      }
      catch
      {
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      }
    }

    public bool SetStartMeter(DateTime value)
    {
      if (value == EDC_HandlerFunctions.DateTimeNull)
        return this.SetParameterValue<byte>("cfg_start_meter_year", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_meter_month", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_meter_day", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_meter_hour", byte.MaxValue) && this.SetParameterValue<byte>("cfg_start_meter_minute", byte.MaxValue);
      if (value.Year < 2000 || value.Year > 2255)
        throw new ArgumentException("Can not set StartMeter! The year of new value is invalid (Valid are: 2000-2255). Value: " + value.ToString());
      return this.SetParameterValue<byte>("cfg_start_meter_year", (byte) (value.Year - 2000)) && this.SetParameterValue<byte>("cfg_start_meter_month", (byte) value.Month) && this.SetParameterValue<byte>("cfg_start_meter_day", (byte) value.Day) && this.SetParameterValue<byte>("cfg_start_meter_hour", (byte) value.Hour) && this.SetParameterValue<byte>("cfg_start_meter_minute", (byte) value.Minute);
    }

    public DateTime? GetStartMeter()
    {
      byte parameterValue1 = this.GetParameterValue<byte>("cfg_start_meter_year");
      byte parameterValue2 = this.GetParameterValue<byte>("cfg_start_meter_month");
      byte parameterValue3 = this.GetParameterValue<byte>("cfg_start_meter_day");
      byte parameterValue4 = this.GetParameterValue<byte>("cfg_start_meter_hour");
      byte parameterValue5 = this.GetParameterValue<byte>("cfg_start_meter_minute");
      if (parameterValue1 == byte.MaxValue || parameterValue2 == byte.MaxValue || parameterValue3 == byte.MaxValue || parameterValue4 == byte.MaxValue || parameterValue5 == byte.MaxValue)
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      if (parameterValue1 > byte.MaxValue || parameterValue2 > (byte) 12 || parameterValue3 > (byte) 31 || parameterValue4 > (byte) 23 || parameterValue5 > (byte) 59)
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      try
      {
        return new DateTime?(new DateTime(2000 + (int) parameterValue1, (int) parameterValue2, (int) parameterValue3, (int) parameterValue4, (int) parameterValue5, 1));
      }
      catch
      {
        return new DateTime?(EDC_HandlerFunctions.DateTimeNull);
      }
    }

    public bool DisableLeakDetection()
    {
      return this.SetPulseLeakLimit((ushort) 0) && this.SetPulseLeakLower((short) 0) && this.SetPulseLeakUpper((short) 0);
    }

    public bool DisableBurstDetection() => this.SetBurstLimit((ushort) 0);

    public bool DisableBackflowDetection() => this.SetPulseBackLimit((ushort) 0);

    public bool DisableStandstillDetection() => this.SetPulseBlockLimit((ushort) 0);

    public bool DisableCoilManipulationDetection() => throw new NotImplementedException();

    public bool DisableUndersizeDetection() => this.SetUndersizeLimit((ushort) 0);

    public bool DisableOversizeDetection() => this.SetOversizeLimit((ushort) 0);

    internal SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(List<long> filter)
    {
      List<RamLogger> ramLogger = LoggerManager.ParseRamLogger(this);
      if (ramLogger == null)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      DateTime? systemTime = this.GetSystemTime();
      if (!systemTime.HasValue)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      DateTime timePoint = systemTime.Value;
      Warning? warnings = this.GetWarnings();
      HardwareError? hardwareErrors = this.GetHardwareErrors();
      if (this.IsWarning(warnings, Warning.BATT_LOW))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.BatteryLow, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.LEAK) || this.IsWarning(warnings, Warning.LEAK_A))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Leak, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.BLOCK_A))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Blockage, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.BACKFLOW) || this.IsWarning(warnings, Warning.BACKFLOW_A))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Backflow, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.OVERSIZE))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Oversized, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.UNDERSIZE))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Undersized, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.TAMPER_A) || this.IsError(hardwareErrors, HardwareError.TAMPER))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Tamper, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.REMOVAL_A) || this.IsError(hardwareErrors, HardwareError.REMOVAL))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Removal, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.BURST))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.Burst, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.TEMPORARY_ERROR))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.TemporaryError, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.ABNORMAL) || this.IsWarning(warnings, Warning.PERMANENT_ERROR) || this.IsError(hardwareErrors, HardwareError.COILFAIL) || this.IsError(hardwareErrors, HardwareError.COILWARNING) || this.IsError(hardwareErrors, HardwareError.ERROR_OSCILLATOR) || this.IsError(hardwareErrors, HardwareError.ERROR_CALLBACK) || this.IsError(hardwareErrors, HardwareError.ERROR_RADIOCAL))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfError(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentError.DeviceError), (object) 1);
      bool? radioState = this.GetRadioState();
      if (radioState.HasValue && !radioState.Value)
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.RadioDisabled, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      bool? coilSampling = this.GetCoilSampling();
      if (coilSampling.HasValue && !coilSampling.Value)
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.PulseDisabled, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      bool? dataLoggingState = this.GetDataLoggingState();
      if (dataLoggingState.HasValue && !dataLoggingState.Value)
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentWarning.LoggerDisabled, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      long valueIdForValueEnum = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Volume, ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
      if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum))
      {
        int? meterValue = this.GetMeterValue();
        if (meterValue.HasValue)
          ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, valueIdForValueEnum, (object) ((double) meterValue.Value / 1000.0));
      }
      this.CreateValues(ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.QuarterHour, RamLoggerType.QuarterHour);
      this.CreateValues(ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.Day, RamLoggerType.Daily);
      this.CreateValues(ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.HalfMonth, RamLoggerType.Halfmonth);
      this.CreateValues(ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.Month, RamLoggerType.Fullmonth);
      this.CreateValues(ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.DueDate, RamLoggerType.DueDate);
      ValueIdent.CleanUpEmptyValueIdents(valueList);
      return valueList;
    }

    private bool IsError(HardwareError? errors, HardwareError error)
    {
      return errors.HasValue && (errors.Value & error) == error;
    }

    private bool IsWarning(Warning? warnings, Warning warning)
    {
      return warnings.HasValue && (warnings.Value & warning) == warning;
    }

    private void CreateValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      List<RamLogger> loggers,
      List<long> filter,
      ValueIdent.ValueIdPart_StorageInterval interval,
      RamLoggerType type)
    {
      long valueIdForValueEnum = ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Volume, ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, interval, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
      if (!ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum))
        return;
      RamLogger ramLogger = loggers.Find((Predicate<RamLogger>) (x => x.Type == type));
      if (ramLogger != null)
      {
        SortedList<DateTime, uint> values = ramLogger.Values;
        if (values != null)
        {
          foreach (KeyValuePair<DateTime, uint> keyValuePair in values)
            ValueIdent.AddValueToValueIdentList(ref valueList, keyValuePair.Key, valueIdForValueEnum, (object) ((double) keyValuePair.Value / 1000.0));
        }
      }
    }

    public string GetInfo() => this.ToString();
  }
}
