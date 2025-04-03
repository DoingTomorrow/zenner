// Decompiled with JetBrains decompiler
// Type: PDC_Handler.PDC_Meter
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

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
namespace PDC_Handler
{
  public sealed class PDC_Meter : IMeter
  {
    private static Logger logger = LogManager.GetLogger(nameof (PDC_Meter));
    private readonly byte[] EMPTY_AES_KEY = new byte[16]
    {
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue
    };
    public const int COL_INDEX_VALUE_NAME_KEY = 1;
    public const int COL_INDEX_VALUE_NAME = 2;
    public const int COL_INDEX_VALUE_SIZE = 3;
    public const int COL_INDEX_VALUE_TYPE = 4;
    public const int COL_INDEX_VALUE_ADDRESS_HEX = 5;
    public const int COL_INDEX_VALUE_HEX = 6;
    public const int COL_INDEX_VALUE_DEC = 7;
    public const int COL_INDEX_VALUE_DIF_VIF = 8;

    public PDC_Meter(PDC_MemoryMap map)
    {
      this.Map = map;
      this.DBDeviceInfo = new DatabaseDeviceInfo();
    }

    public DeviceVersion Version { get; set; }

    public DatabaseDeviceInfo DBDeviceInfo { get; set; }

    public PDC_MemoryMap Map { get; private set; }

    public PDC_Meter DeepCopy()
    {
      return new PDC_Meter(this.Map.DeepCopy())
      {
        DBDeviceInfo = this.DBDeviceInfo != null ? this.DBDeviceInfo.DeepCopy() : (DatabaseDeviceInfo) null,
        Map = this.Map != null ? this.Map.DeepCopy() : (PDC_MemoryMap) null,
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
      List<Parameter> parameter = PDC_MemoryMap.GetParameter(this.Version);
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
        PDC_MemoryMap.CacheParameter(this.Version);
        if (!PDC_MemoryMap.ExistParameter(this.Version))
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
        foreach (Parameter parameter in PDC_MemoryMap.GetParameter(this.Version))
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

    public SortedList<ushort, byte[]> GetChangedRamBlocks(PDC_Meter sourceMeter)
    {
      return this.Map.GetChangedRamBlocks(this.Version, sourceMeter);
    }

    public SortedList<ushort, byte[]> GetChangedFlashBlocks(
      PDC_Meter sourceMeter,
      out List<ushort> segmentsToErase)
    {
      segmentsToErase = (List<ushort>) null;
      SortedList<ushort, byte[]> changedFlashBlocks1 = new SortedList<ushort, byte[]>();
      List<ushort> ushortList = new List<ushort>();
      Parameter parameter = this.GetParameter("INFOA");
      if (parameter == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter INFOA.");
      if (this.GetParameter("PARAM_CONST_LIST") == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter PARAM_CONST_LIST.");
      SortedList<ushort, byte[]> changedFlashBlocks2 = this.Map.GetChangedFlashBlocks(sourceMeter, (int) parameter.Address, (int) parameter.Address + 128, 128, out segmentsToErase);
      if (changedFlashBlocks2 != null)
      {
        foreach (KeyValuePair<ushort, byte[]> keyValuePair in changedFlashBlocks2)
          changedFlashBlocks1.Add(keyValuePair.Key, keyValuePair.Value);
        ushortList.AddRange((IEnumerable<ushort>) segmentsToErase);
      }
      SortedList<ushort, byte[]> changedFlashBlocks3 = this.Map.GetChangedFlashBlocks(sourceMeter, 32768, 4991, 512, out segmentsToErase);
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
      return string.IsNullOrEmpty(parameterName) || !PDC_MemoryMap.ExistParameter(this.Version) ? (Parameter) null : PDC_MemoryMap.GetParameter(this.Version, parameterName);
    }

    public T GetParameterValue<T>(string parameterName)
    {
      PDC_MemoryMap.CacheParameter(this.Version);
      if (!PDC_MemoryMap.ExistParameter(this.Version))
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
        return (T) (System.ValueType) PDC_HandlerFunctions.DateTimeNull;
      if (num > byte.MaxValue || month > (byte) 12 || day > (byte) 31 || minute > (byte) 59 || second > (byte) 59)
        return (T) (System.ValueType) PDC_HandlerFunctions.DateTimeNull;
      try
      {
        return (T) (System.ValueType) new DateTime(2000 + (int) num, (int) month, (int) day, (int) hour, (int) minute, (int) second);
      }
      catch
      {
        return (T) (System.ValueType) PDC_HandlerFunctions.DateTimeNull;
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
      return this.ChangeBit(parameterName, true, PDC_Meter.GetBytes<T>(mask));
    }

    private bool ClearBit<T>(string parameterName, T mask) where T : struct
    {
      return this.ChangeBit(parameterName, false, PDC_Meter.GetBytes<T>(mask));
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
      return this.GetBit(parameterName, PDC_Meter.GetBytes<T>(mask));
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

    private static byte[] GetBytes<T>(T type) where T : struct
    {
      if (typeof (T) == typeof (byte))
        return new byte[1]{ Convert.ToByte((object) type) };
      if (typeof (T) == typeof (ushort))
        return BitConverter.GetBytes(Convert.ToUInt16((object) type));
      throw new NotImplementedException("INTERNAL ERROR: Can not get bytes of the generic type. The type is not implemented. Type: " + typeof (T)?.ToString());
    }

    public byte[] Zip() => this.Map.Zip(this.Version);

    public static bool IsValidZipBuffer(byte[] buffer)
    {
      if (buffer == null)
        return false;
      PDC_MemoryMap pdcMemoryMap = new PDC_MemoryMap();
      try
      {
        if (!pdcMemoryMap.Unzip(buffer, out DeviceVersion _))
          return false;
        if (pdcMemoryMap.IsEmpty())
          return false;
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static PDC_Meter Unzip(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException("The parameter 'buffer' can not be null!");
      PDC_MemoryMap map = new PDC_MemoryMap();
      DeviceVersion version;
      if (!map.Unzip(buffer, out version))
        throw new Exception("Can not unzip MAP file!");
      PDC_Meter pdcMeter = !map.IsEmpty() ? new PDC_Meter(map) : throw new Exception("The MAP is empty!");
      pdcMeter.Version = version;
      DeviceIdentification deviceIdentification = pdcMeter.GetDeviceIdentification();
      if (deviceIdentification != null && deviceIdentification.IsChecksumOK)
      {
        pdcMeter.DBDeviceInfo.MeterInfo = PDC_Database.GetMeterInfo(deviceIdentification.MeterInfoID);
        pdcMeter.DBDeviceInfo.HardwareType = MeterDatabase.GetHardwareType((int) version.HardwareTypeID);
      }
      return pdcMeter;
    }

    public bool Overwrite(PDC_Meter meter, OverwritePart parts)
    {
      if (meter == null)
        throw new ArgumentException("Overwrite failed! Input parameter 'meter' is null.");
      if (meter.Version.Type != this.Version.Type)
        throw new ArgumentException("Overwrite failed! Wrong device type. Actual: " + meter.Version.Type.ToString() + ", Expected: " + this.Version.Type.ToString());
      return ((parts & OverwritePart.TypeIdentification) != OverwritePart.TypeIdentification || this.OverwriteTypeIdentification(meter)) && ((parts & OverwritePart.RadioSettings) != OverwritePart.RadioSettings || this.OverwriteRadioSettings(meter)) && ((parts & OverwritePart.DeviceSettings) != OverwritePart.DeviceSettings || this.OverwriteDeviceSettings(meter)) && ((parts & OverwritePart.Constants) != OverwritePart.Constants || this.OverwriteConstants(meter));
    }

    private bool OverwriteTypeIdentification(PDC_Meter meter)
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
      if (!this.SetDeviceIdentification(ident) || !this.SetManufacturerPDC(meter.GetManufacturerPDC()) || !this.SetManufacturerInputA(meter.GetManufacturerInputA()) || !this.SetManufacturerInputB(meter.GetManufacturerInputB()))
        return false;
      MBusDeviceType? nullable1 = meter.GetMediumPDC();
      if (!this.SetMediumPDC(nullable1.Value))
        return false;
      nullable1 = meter.GetMediumInputA();
      if (!this.SetMediumInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetMediumInputB();
      if (!this.SetMediumInputB(nullable1.Value))
        return false;
      byte? nullable2 = meter.GetVIFInputA();
      if (!this.SetVIFInputA(nullable2.Value))
        return false;
      nullable2 = meter.GetVIFInputB();
      if (!this.SetVIFInputB(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusGenerationPDC();
      if (!this.SetMBusGenerationPDC(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusGenerationInputA();
      if (!this.SetMBusGenerationInputA(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusGenerationInputB();
      if (!this.SetMBusGenerationInputB(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusAddressPDC();
      if (!this.SetMBusAddressPDC(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusAddressInputA();
      if (!this.SetMBusAddressInputA(nullable2.Value))
        return false;
      nullable2 = meter.GetMBusAddressInputB();
      return this.SetMBusAddressInputB(nullable2.Value) && this.SetObisPDC(meter.GetObisPDC()) && this.SetObisInputA(meter.GetObisInputA()) && this.SetObisInputB(meter.GetObisInputB());
    }

    private bool OverwriteRadioSettings(PDC_Meter meter)
    {
      if (meter == null)
        return false;
      if (meter.Version.Type == PDC_DeviceIdentity.PDC_WmBus)
      {
        if (!this.SetRadioFlagsPDCwMBus(meter.GetRadioFlagsPDCwMBus().Value) || !this.SetRadioMode(meter.GetRadioMode().Value) || !this.SetRadioPower(meter.GetRadioPower().Value))
          return false;
        short? nullable1 = meter.GetRadioTimeBias();
        if (!this.SetRadioTimeBias(nullable1.Value))
          return false;
        ushort? nullable2 = meter.GetRadioTransmitInterval();
        if (!this.SetRadioTransmitInterval(nullable2.Value))
          return false;
        nullable2 = meter.GetRadioInstallInterval();
        if (!this.SetRadioInstallInterval(nullable2.Value))
          return false;
        byte? nullable3 = meter.GetRadioInstallCount();
        if (!this.SetRadioInstallCount(nullable3.Value) || !this.SetAESkey(meter.GetAESkey()))
          return false;
        nullable3 = meter.GetPulseActivateRadio();
        if (!this.SetPulseActivateRadio(nullable3.Value))
          return false;
        nullable1 = meter.GetRadioPacketBOffset();
        if (!this.SetRadioPacketBOffset(nullable1.Value) || !this.SetRadioListType(meter.GetRadioListType().Value))
          return false;
      }
      return true;
    }

    private bool OverwriteDeviceSettings(PDC_Meter meter)
    {
      if (meter == null || !this.SetDueDate(meter.GetDueDate().Value) || !this.SetConfigFlagsPDCwMBus(meter.GetConfigFlagsPDCwMBus().Value) || !this.SetTimeZone(meter.GetTimeZone().Value))
        return false;
      ushort? nullable1 = meter.GetPulsePeriod();
      if (!this.SetPulsePeriod(nullable1.Value) || !this.SetPulseOn(meter.GetPulseOn().Value))
        return false;
      nullable1 = meter.GetScaleMantissaInputA();
      if (!this.SetScaleMantissaInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetScaleMantissaInputB();
      if (!this.SetScaleMantissaInputB(nullable1.Value))
        return false;
      sbyte? nullable2 = meter.GetScaleExponentInputA();
      if (!this.SetScaleExponentInputA(nullable2.Value))
        return false;
      nullable2 = meter.GetScaleExponentInputB();
      return this.SetScaleExponentInputB(nullable2.Value);
    }

    private bool OverwriteConstants(PDC_Meter meter)
    {
      if (meter == null)
        return false;
      ushort? nullable1 = meter.GetPulseBlockLimitInputA();
      if (!this.SetPulseBlockLimitInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseLeakLimitInputA();
      if (!this.SetPulseLeakLimitInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseUnleakLimitInputA();
      if (!this.SetPulseUnleakLimitInputA(nullable1.Value))
        return false;
      short? nullable2 = meter.GetPulseLeakLowerInputA();
      if (!this.SetPulseLeakLowerInputA(nullable2.Value))
        return false;
      nullable2 = meter.GetPulseLeakUpperInputA();
      if (!this.SetPulseLeakUpperInputA(nullable2.Value))
        return false;
      nullable1 = meter.GetOversizeDiffInputA();
      if (!this.SetOversizeDiffInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetOversizeLimitInputA();
      if (!this.SetOversizeLimitInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetUndersizeDiffInputA();
      if (!this.SetUndersizeDiffInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetUndersizeLimitInputA();
      if (!this.SetUndersizeLimitInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetBurstDiffInputA();
      if (!this.SetBurstDiffInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetBurstLimitInputA();
      if (!this.SetBurstLimitInputA(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseBlockLimitInputB();
      if (!this.SetPulseBlockLimitInputB(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseLeakLimitInputB();
      if (!this.SetPulseLeakLimitInputB(nullable1.Value))
        return false;
      nullable1 = meter.GetPulseUnleakLimitInputB();
      if (!this.SetPulseUnleakLimitInputB(nullable1.Value))
        return false;
      nullable2 = meter.GetPulseLeakLowerInputB();
      if (!this.SetPulseLeakLowerInputB(nullable2.Value))
        return false;
      nullable2 = meter.GetPulseLeakUpperInputB();
      if (!this.SetPulseLeakUpperInputB(nullable2.Value))
        return false;
      nullable1 = meter.GetOversizeDiffInputB();
      if (!this.SetOversizeDiffInputB(nullable1.Value))
        return false;
      nullable1 = meter.GetOversizeLimitInputB();
      if (!this.SetOversizeLimitInputB(nullable1.Value))
        return false;
      nullable1 = meter.GetUndersizeDiffInputB();
      if (!this.SetUndersizeDiffInputB(nullable1.Value))
        return false;
      nullable1 = meter.GetUndersizeLimitInputB();
      if (!this.SetUndersizeLimitInputB(nullable1.Value))
        return false;
      nullable1 = meter.GetBurstDiffInputB();
      if (!this.SetBurstDiffInputB(nullable1.Value))
        return false;
      nullable1 = meter.GetBurstLimitInputB();
      return this.SetBurstLimitInputB(nullable1.Value);
    }

    public uint? GetMeterValueA() => new uint?(this.GetParameterValue<uint>("pulseReadingA"));

    public uint? GetMeterValueB() => new uint?(this.GetParameterValue<uint>("pulseReadingB"));

    public DateTime? GetSystemTime()
    {
      DateTime parameterValue = this.GetParameterValue<DateTime>("hwSystemDate");
      return parameterValue == PDC_HandlerFunctions.DateTimeNull ? new DateTime?() : new DateTime?(parameterValue);
    }

    public bool SetRadioMode(RadioMode type)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return this.SetParameterValue<byte>("cfg_radio_mode", (byte) type);
    }

    public RadioMode? GetRadioMode()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
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
      Parameter parameter = PDC_MemoryMap.GetParameter(this.Version, "Con_MeterId");
      if (parameter == null)
        throw new Exception("Can not set device identification! Parameter 'Con_MeterId' is in this MAP not available.");
      byte[] buffer = ident.Buffer;
      return this.Map.SetMemoryBytes(parameter.Address, buffer);
    }

    public bool SetRadioListType(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new NullReferenceException("Radio list type is empty!");
      return Enum.IsDefined(typeof (RadioList), (object) value) ? this.SetParameterValue<byte>("cfg_list", (byte) Enum.Parse(typeof (RadioList), value, true)) : throw new ArgumentException(value);
    }

    public bool SetRadioListType(RadioList value)
    {
      return this.SetParameterValue<byte>("cfg_list", (byte) value);
    }

    public RadioList? GetRadioListType()
    {
      return new RadioList?((RadioList) Enum.ToObject(typeof (RadioList), this.GetParameterValue<byte>("cfg_list")));
    }

    public bool SetRadioTimeBias(short interval)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return this.SetParameterValue<short>("cfg_radio_time_bias", interval);
    }

    public short? GetRadioTimeBias()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new short?(this.GetParameterValue<short>("cfg_radio_time_bias"));
    }

    public bool SetRadioTransmitInterval(ushort interval)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return interval >= (ushort) 2 && this.SetParameterValue<ushort>("cfg_radio_normal_basetime", interval);
    }

    public ushort? GetRadioTransmitInterval()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new ushort?(this.GetParameterValue<ushort>("cfg_radio_normal_basetime"));
    }

    public bool SetRadioInstallInterval(ushort interval)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return interval >= (ushort) 2 ? this.SetParameterValue<ushort>("cfg_radio_install_basetime", interval) : throw new ArgumentOutOfRangeException("The value must be at least 2 to ensure proper device operation.");
    }

    public ushort? GetRadioInstallInterval()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new ushort?(this.GetParameterValue<ushort>("cfg_radio_install_basetime"));
    }

    public bool SetRadioInstallCount(byte value)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return this.SetParameterValue<byte>("cfg_radio_install_count", value);
    }

    public byte? GetRadioInstallCount()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new byte?(this.GetParameterValue<byte>("cfg_radio_install_count"));
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

    public bool SetMediumPDC(MBusDeviceType type)
    {
      return this.SetParameterValue<byte>("cfg_mbus_medium_c", (byte) type);
    }

    public MBusDeviceType? GetMediumPDC()
    {
      return new MBusDeviceType?((MBusDeviceType) Enum.ToObject(typeof (MBusDeviceType), this.GetParameterValue<byte>("cfg_mbus_medium_c")));
    }

    public bool SetConfigFlagsPDCwMBus(ConfigFlagsPDCwMBus flags)
    {
      return this.SetParameterValue<ushort>("cfg_config_flags", (ushort) flags);
    }

    public ConfigFlagsPDCwMBus? GetConfigFlagsPDCwMBus()
    {
      return new ConfigFlagsPDCwMBus?((ConfigFlagsPDCwMBus) Enum.ToObject(typeof (ConfigFlagsPDCwMBus), this.GetParameterValue<ushort>("cfg_config_flags")));
    }

    public bool SetRadioFlagsPDCwMBus(RadioFlagsPDCwMBus flags)
    {
      return this.SetParameterValue<byte>("cfg_radio_flags", (byte) flags);
    }

    public RadioFlagsPDCwMBus? GetRadioFlagsPDCwMBus()
    {
      return new RadioFlagsPDCwMBus?((RadioFlagsPDCwMBus) Enum.ToObject(typeof (RadioFlagsPDCwMBus), this.GetParameterValue<byte>("cfg_radio_flags")));
    }

    public bool SetMediumInputA(MBusDeviceType type)
    {
      return this.SetParameterValue<byte>("cfg_mbus_medium_a", (byte) type);
    }

    public MBusDeviceType? GetMediumInputA()
    {
      return new MBusDeviceType?((MBusDeviceType) Enum.ToObject(typeof (MBusDeviceType), this.GetParameterValue<byte>("cfg_mbus_medium_a")));
    }

    public bool SetMediumInputB(MBusDeviceType type)
    {
      return this.SetParameterValue<byte>("cfg_mbus_medium_b", (byte) type);
    }

    public MBusDeviceType? GetMediumInputB()
    {
      return new MBusDeviceType?((MBusDeviceType) Enum.ToObject(typeof (MBusDeviceType), this.GetParameterValue<byte>("cfg_mbus_medium_b")));
    }

    public bool SetRadioPacketBOffset(short value)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return this.SetParameterValue<short>("cfg_radio_packetb_offset", value);
    }

    public short? GetRadioPacketBOffset()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new short?(this.GetParameterValue<short>("cfg_radio_packetb_offset"));
    }

    public bool SetFrequencyOffset(short value)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return this.SetParameterValue<short>("cfg_radio_freq_offset", value);
    }

    public short? GetFrequencyOffset()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new short?(this.GetParameterValue<short>("cfg_radio_freq_offset"));
    }

    public bool SetMBusAddressPDC(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_address_c", value);
    }

    public byte? GetMBusAddressPDC()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_address_c"));
    }

    public bool SetMBusAddressInputA(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_address_a", value);
    }

    public byte? GetMBusAddressInputA()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_address_a"));
    }

    public bool SetMBusAddressInputB(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_address_b", value);
    }

    public byte? GetMBusAddressInputB()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_address_b"));
    }

    public bool SetRadioPower(RadioPower value)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return this.SetParameterValue<byte>("cfg_radio_power", (byte) value);
    }

    public RadioPower? GetRadioPower()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new RadioPower?((RadioPower) Enum.ToObject(typeof (RadioPower), this.GetParameterValue<byte>("cfg_radio_power")));
    }

    public bool SetAESkey(object value)
    {
      if (value == null)
        value = (object) this.EMPTY_AES_KEY;
      return value is byte[] ? this.SetAESkey((byte[]) value) : this.SetAESkey(Util.HexStringToByteArray(value.ToString()));
    }

    public bool SetAESkey(byte[] value)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      if (value == null)
        value = this.EMPTY_AES_KEY;
      return value.Length == 16 ? this.SetParameterValue<byte[]>("cfg_key", value) : throw new ArgumentException("The value of AES key has wrong length! Length: " + value.Length.ToString());
    }

    public byte[] GetAESkey()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      byte[] parameterValue = this.GetParameterValue<byte[]>("cfg_key");
      return Util.ByteArrayCompare(parameterValue, this.EMPTY_AES_KEY) ? (byte[]) null : parameterValue;
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
        return new DateTime?(PDC_HandlerFunctions.DateTimeNull);
      try
      {
        return new DateTime?(new DateTime(2000, (int) parameterValue1, (int) parameterValue2));
      }
      catch
      {
        return new DateTime?(PDC_HandlerFunctions.DateTimeNull);
      }
    }

    public bool SetPulseActivateRadio(byte value)
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return this.SetParameterValue<byte>("cfg_pulse_activate", value);
    }

    public byte? GetPulseActivateRadio()
    {
      if (this.Version == null || this.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        throw new NotSupportedException("Radio functions not supported!");
      return new byte?(this.GetParameterValue<byte>("cfg_pulse_activate"));
    }

    public bool SetPulsePeriod(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_period", value);
    }

    public ushort? GetPulsePeriod()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_period"));
    }

    public bool SetPulseOn(byte value) => this.SetParameterValue<byte>("cfg_pulse_on", value);

    public byte? GetPulseOn() => new byte?(this.GetParameterValue<byte>("cfg_pulse_on"));

    public bool SetMBusGenerationPDC(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_version_c", value);
    }

    public byte? GetMBusGenerationPDC()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_version_c"));
    }

    public bool SetMBusGenerationInputA(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_version_a", value);
    }

    public byte? GetMBusGenerationInputA()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_version_a"));
    }

    public bool SetMBusGenerationInputB(byte value)
    {
      return this.SetParameterValue<byte>("cfg_mbus_version_b", value);
    }

    public byte? GetMBusGenerationInputB()
    {
      return new byte?(this.GetParameterValue<byte>("cfg_mbus_version_b"));
    }

    public bool SetManufacturerPDC(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set manufacturer! Input parameter 'value' is null.");
      if (value == "ZENNER")
        value = "ZRI";
      return value.Length == 3 ? this.SetParameterValue<ushort>("cfg_mbus_manid_c", MBusDevice.GetManufacturerCode(value)) : throw new ArgumentException("Can not set manufacturer! The length of input parameter 'value' is not 3 chars.");
    }

    public string GetManufacturerPDC()
    {
      return MBusDevice.GetManufacturer((short) this.GetParameterValue<ushort>("cfg_mbus_manid_c"));
    }

    public bool SetManufacturerInputA(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set manufacturer! Input parameter 'value' is null.");
      if (value == "ZENNER")
        value = "ZRI";
      return value.Length == 3 ? this.SetParameterValue<ushort>("cfg_mbus_manid_a", MBusDevice.GetManufacturerCode(value)) : throw new ArgumentException("Can not set manufacturer! The length of input parameter 'value' is not 3 chars.");
    }

    public string GetManufacturerInputA()
    {
      return MBusDevice.GetManufacturer((short) this.GetParameterValue<ushort>("cfg_mbus_manid_a"));
    }

    public bool SetManufacturerInputB(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set manufacturer! Input parameter 'value' is null.");
      if (value == "ZENNER")
        value = "ZRI";
      return value.Length == 3 ? this.SetParameterValue<ushort>("cfg_mbus_manid_b", MBusDevice.GetManufacturerCode(value)) : throw new ArgumentException("Can not set manufacturer! The length of input parameter 'value' is not 3 chars.");
    }

    public string GetManufacturerInputB()
    {
      return MBusDevice.GetManufacturer((short) this.GetParameterValue<ushort>("cfg_mbus_manid_b"));
    }

    public bool SetSerialnumberFull(string value)
    {
      return this.SetSerialnumberFull(value, "Con_fullserialnumber", true);
    }

    public string GetSerialnumberFull() => this.GetSerialnumberFull("Con_fullserialnumber");

    public bool SetSerialnumberFullInputA(string value)
    {
      return this.SetSerialnumberFull(value, "Con_fullserialnumberA", false);
    }

    public string GetSerialnumberFullInputA() => this.GetSerialnumberFull("Con_fullserialnumberA");

    public bool SetSerialnumberFullInputB(string value)
    {
      return this.SetSerialnumberFull(value, "Con_fullserialnumberB", false);
    }

    public string GetSerialnumberFullInputB() => this.GetSerialnumberFull("Con_fullserialnumberB");

    private bool SetSerialnumberFull(string value, string key, bool checkOBIS)
    {
      if (string.IsNullOrEmpty(value))
        return this.SetParameterValue<byte[]>(key, new byte[8]
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
        throw new ArgumentException("Wrong length of the full serial number detected! Expected 14, Value: " + value);
      string s = !checkOBIS || value[0] == 'E' ? value.Substring(0, 1) : throw new ArgumentException("Wrong start char of the full serial number detected! Expected E, Actual: " + value[0].ToString());
      string Manufacturer = value.Substring(1, 3);
      byte num = byte.Parse(value.Substring(4, 2));
      string str = value.Substring(6);
      if (num > (byte) 99)
        throw new ArgumentOutOfRangeException("Wrong generation in full serial number detected! Valid range are: 0-99");
      if (!Util.IsValidBCD(str))
        throw new ArgumentException("Wrong full serial number detected! The last 8 chars should contains only numbers. Value: " + value);
      List<byte> byteList = new List<byte>(14);
      byteList.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(s));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(MBusDevice.GetManufacturerCode(Manufacturer)));
      byteList.Add(num);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(Util.ConvertUnt32ToBcdUInt32(uint.Parse(str))));
      return byteList.Count == 8 ? this.SetParameterValue<byte[]>(key, byteList.ToArray()) : throw new ArgumentOutOfRangeException("Wrong full serial number was generated!");
    }

    private string GetSerialnumberFull(string key)
    {
      byte[] parameterValue = this.GetParameterValue<byte[]>(key);
      if (parameterValue == null || parameterValue.Length != 8)
        throw new ArgumentException("The parameter '" + key + "' is invalid!");
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

    public bool SetPulseBlockLimitInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_a_block_limit", value);
    }

    public ushort? GetPulseBlockLimitInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_a_block_limit"));
    }

    public bool SetPulseLeakLimitInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_a_leak_limit", value);
    }

    public ushort? GetPulseLeakLimitInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_a_leak_limit"));
    }

    public bool SetPulseUnleakLimitInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_a_unleak_limit", value);
    }

    public ushort? GetPulseUnleakLimitInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_a_unleak_limit"));
    }

    public bool SetPulseLeakLowerInputA(short value)
    {
      return this.SetParameterValue<short>("cfg_pulse_a_leak_lower", value);
    }

    public short? GetPulseLeakLowerInputA()
    {
      return new short?(this.GetParameterValue<short>("cfg_pulse_a_leak_lower"));
    }

    public bool SetPulseLeakUpperInputA(short value)
    {
      return this.SetParameterValue<short>("cfg_pulse_a_leak_upper", value);
    }

    public short? GetPulseLeakUpperInputA()
    {
      return new short?(this.GetParameterValue<short>("cfg_pulse_a_leak_upper"));
    }

    public bool SetOversizeDiffInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_oversize_a_diff", value);
    }

    public ushort? GetOversizeDiffInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_oversize_a_diff"));
    }

    public bool SetOversizeLimitInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_oversize_a_limit", value);
    }

    public ushort? GetOversizeLimitInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_oversize_a_limit"));
    }

    public bool SetUndersizeDiffInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_undersize_a_diff", value);
    }

    public ushort? GetUndersizeDiffInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_undersize_a_diff"));
    }

    public bool SetUndersizeLimitInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_undersize_a_limit", value);
    }

    public ushort? GetUndersizeLimitInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_undersize_a_limit"));
    }

    public bool SetBurstDiffInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_burst_a_diff", value);
    }

    public ushort? GetBurstDiffInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_burst_a_diff"));
    }

    public bool SetBurstLimitInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_burst_a_limit", value);
    }

    public ushort? GetBurstLimitInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_burst_a_limit"));
    }

    public bool SetPulseBlockLimitInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_b_block_limit", value);
    }

    public ushort? GetPulseBlockLimitInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_b_block_limit"));
    }

    public bool SetPulseLeakLimitInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_b_leak_limit", value);
    }

    public ushort? GetPulseLeakLimitInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_b_leak_limit"));
    }

    public bool SetPulseUnleakLimitInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_pulse_b_unleak_limit", value);
    }

    public ushort? GetPulseUnleakLimitInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_pulse_b_unleak_limit"));
    }

    public bool SetPulseLeakLowerInputB(short value)
    {
      return this.SetParameterValue<short>("cfg_pulse_b_leak_lower", value);
    }

    public short? GetPulseLeakLowerInputB()
    {
      return new short?(this.GetParameterValue<short>("cfg_pulse_b_leak_lower"));
    }

    public bool SetPulseLeakUpperInputB(short value)
    {
      return this.SetParameterValue<short>("cfg_pulse_b_leak_upper", value);
    }

    public short? GetPulseLeakUpperInputB()
    {
      return new short?(this.GetParameterValue<short>("cfg_pulse_b_leak_upper"));
    }

    public bool SetOversizeDiffInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_oversize_b_diff", value);
    }

    public ushort? GetOversizeDiffInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_oversize_b_diff"));
    }

    public bool SetOversizeLimitInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_oversize_b_limit", value);
    }

    public ushort? GetOversizeLimitInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_oversize_b_limit"));
    }

    public bool SetUndersizeDiffInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_undersize_b_diff", value);
    }

    public ushort? GetUndersizeDiffInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_undersize_b_diff"));
    }

    public bool SetUndersizeLimitInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_undersize_b_limit", value);
    }

    public ushort? GetUndersizeLimitInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_undersize_b_limit"));
    }

    public bool SetBurstDiffInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_burst_b_diff", value);
    }

    public ushort? GetBurstDiffInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_burst_b_diff"));
    }

    public bool SetBurstLimitInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_burst_b_limit", value);
    }

    public ushort? GetBurstLimitInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_burst_b_limit"));
    }

    public bool SetVIFInputA(byte value) => this.SetParameterValue<byte>("cfg_vif_a", value);

    public byte? GetVIFInputA() => new byte?(this.GetParameterValue<byte>("cfg_vif_a"));

    public bool SetVIFInputB(byte value) => this.SetParameterValue<byte>("cfg_vif_b", value);

    public byte? GetVIFInputB() => new byte?(this.GetParameterValue<byte>("cfg_vif_b"));

    public bool SetScaleMantissaInputA(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_scale_mantissa_a", value);
    }

    public ushort? GetScaleMantissaInputA()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_scale_mantissa_a"));
    }

    public bool SetScaleMantissaInputB(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_scale_mantissa_b", value);
    }

    public ushort? GetScaleMantissaInputB()
    {
      return new ushort?(this.GetParameterValue<ushort>("cfg_scale_mantissa_b"));
    }

    public bool SetScaleExponentInputA(sbyte value)
    {
      return (double) value <= 4.0 && (double) value >= -4.0 ? this.SetParameterValue<sbyte>("cfg_scale_exponent_a", value) : throw new ArgumentOutOfRangeException("cfg_scale_exponent_a", "Invalid exponent value! Valid range +-4.");
    }

    public sbyte? GetScaleExponentInputA()
    {
      return new sbyte?(this.GetParameterValue<sbyte>("cfg_scale_exponent_a"));
    }

    public bool SetScaleExponentInputB(sbyte value)
    {
      return (double) value <= 4.0 && (double) value >= -4.0 ? this.SetParameterValue<sbyte>("cfg_scale_exponent_b", value) : throw new ArgumentOutOfRangeException("cfg_scale_exponent_b", "Invalid exponent value! Valid range +-4.");
    }

    public sbyte? GetScaleExponentInputB()
    {
      return new sbyte?(this.GetParameterValue<sbyte>("cfg_scale_exponent_b"));
    }

    public bool SetScaleFactorInputA(double value)
    {
      if (value <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (value), "Negative value  is not allowed!");
      sbyte num1 = 0;
      double num2 = value;
      while (num2 % 1.0 != 0.0)
      {
        num2 *= 10.0;
        --num1;
      }
      return this.SetScaleExponentInputA(num1) && this.SetScaleMantissaInputA(Convert.ToUInt16(num2));
    }

    public double? GetScaleFactorInputA()
    {
      sbyte? scaleExponentInputA = this.GetScaleExponentInputA();
      if (!scaleExponentInputA.HasValue)
        return new double?();
      ushort? scaleMantissaInputA = this.GetScaleMantissaInputA();
      if (!scaleMantissaInputA.HasValue)
        return new double?();
      ushort? nullable1 = scaleMantissaInputA;
      double? nullable2 = nullable1.HasValue ? new double?((double) nullable1.GetValueOrDefault()) : new double?();
      double num = Math.Pow(10.0, (double) scaleExponentInputA.Value);
      return new double?(Convert.ToDouble((object) (nullable2.HasValue ? new double?(nullable2.GetValueOrDefault() * num) : new double?())));
    }

    public bool SetScaleFactorInputB(double value)
    {
      if (value <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (value), "Negative value  is not allowed!");
      sbyte num1 = 0;
      double num2 = value;
      while (num2 % 1.0 != 0.0)
      {
        num2 *= 10.0;
        --num1;
      }
      return this.SetScaleExponentInputB(num1) && this.SetScaleMantissaInputB(Convert.ToUInt16(num2));
    }

    public double? GetScaleFactorInputB()
    {
      sbyte? scaleExponentInputB = this.GetScaleExponentInputB();
      if (!scaleExponentInputB.HasValue)
        return new double?();
      ushort? scaleMantissaInputB = this.GetScaleMantissaInputB();
      if (!scaleMantissaInputB.HasValue)
        return new double?();
      ushort? nullable1 = scaleMantissaInputB;
      double? nullable2 = nullable1.HasValue ? new double?((double) nullable1.GetValueOrDefault()) : new double?();
      double num = Math.Pow(10.0, (double) scaleExponentInputB.Value);
      return new double?(Convert.ToDouble((object) (nullable2.HasValue ? new double?(nullable2.GetValueOrDefault() * num) : new double?())));
    }

    public void SetNominalFlowB(double value)
    {
      double? scaleFactorInputB = this.GetScaleFactorInputB();
      double? nullable1 = scaleFactorInputB;
      double num1 = 1.0;
      if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
      {
        if (2.5 == value)
          this.SetValuesFromNominalFlowB((ushort) 62, (ushort) 625, (ushort) 187);
        if (4.0 == value)
          this.SetValuesFromNominalFlowB((ushort) 100, (ushort) 1000, (ushort) 300);
        if (6.3 == value)
          this.SetValuesFromNominalFlowB((ushort) 157, (ushort) 1575, (ushort) 472);
        if (10.0 == value)
          this.SetValuesFromNominalFlowB((ushort) 250, (ushort) 2500, (ushort) 750);
        if (16.0 == value)
          this.SetValuesFromNominalFlowB((ushort) 400, (ushort) 4000, (ushort) 1200);
        if (25.0 != value)
          return;
        this.SetValuesFromNominalFlowB((ushort) 625, (ushort) 6250, (ushort) 1875);
      }
      else
      {
        double? nullable2 = scaleFactorInputB;
        double num2 = 10.0;
        if (nullable2.GetValueOrDefault() == num2 & nullable2.HasValue)
        {
          if (25.0 == value)
            this.SetValuesFromNominalFlowB((ushort) 62, (ushort) 625, (ushort) 187);
          if (40.0 == value)
            this.SetValuesFromNominalFlowB((ushort) 100, (ushort) 1000, (ushort) 300);
          if (63.0 == value)
            this.SetValuesFromNominalFlowB((ushort) 157, (ushort) 1575, (ushort) 472);
          if (100.0 != value)
            return;
          this.SetValuesFromNominalFlowB((ushort) 250, (ushort) 2500, (ushort) 750);
        }
        else
        {
          nullable2 = scaleFactorInputB;
          double num3 = 100.0;
          if (nullable2.GetValueOrDefault() == num3 & nullable2.HasValue)
          {
            if (160.0 == value)
              this.SetValuesFromNominalFlowB((ushort) 40, (ushort) 400, (ushort) 120);
            if (250.0 == value)
              this.SetValuesFromNominalFlowB((ushort) 62, (ushort) 625, (ushort) 187);
            if (400.0 == value)
              this.SetValuesFromNominalFlowB((ushort) 100, (ushort) 1000, (ushort) 300);
            if (630.0 == value)
              this.SetValuesFromNominalFlowB((ushort) 157, (ushort) 1575, (ushort) 472);
            if (1000.0 != value)
              return;
            this.SetValuesFromNominalFlowB((ushort) 250, (ushort) 2500, (ushort) 750);
          }
          else
          {
            nullable2 = scaleFactorInputB;
            double num4 = 0.5;
            int num5;
            if (!(nullable2.GetValueOrDefault() == num4 & nullable2.HasValue))
            {
              nullable2 = scaleFactorInputB;
              double num6 = 5.0;
              num5 = nullable2.GetValueOrDefault() == num6 & nullable2.HasValue ? 1 : 0;
            }
            else
              num5 = 1;
            if (num5 != 0)
            {
              double num7 = value * 1000.0 / 4.0;
              this.SetValuesFromNominalFlowB(Convert.ToUInt16(num7 * 0.1 / scaleFactorInputB.Value), Convert.ToUInt16(num7 * 1.0 / scaleFactorInputB.Value), Convert.ToUInt16(num7 * 0.3 / scaleFactorInputB.Value));
            }
            else
            {
              string str1 = value.ToString();
              nullable2 = scaleFactorInputB;
              string str2 = nullable2.ToString();
              throw new NotImplementedException("Nominal flow: " + str1 + ", pulseMultiplierA: " + str2);
            }
          }
        }
      }
    }

    private void SetValuesFromNominalFlowB(
      ushort oversizeDiff,
      ushort undersizeDiff,
      ushort burstDiff)
    {
      this.SetOversizeDiffInputB(oversizeDiff);
      this.SetUndersizeDiffInputB(undersizeDiff);
      this.SetBurstDiffInputB(burstDiff);
    }

    public string GetNominalFlowB()
    {
      double? scaleFactorInputB = this.GetScaleFactorInputB();
      ushort? oversizeDiffInputB = this.GetOversizeDiffInputB();
      ushort? undersizeDiffInputB = this.GetUndersizeDiffInputB();
      ushort? burstDiffInputB = this.GetBurstDiffInputB();
      double? nullable1 = scaleFactorInputB;
      double num1 = 1.0;
      if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
      {
        ushort? nullable2 = oversizeDiffInputB;
        int? nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num2 = 62;
        int num3;
        if (nullable3.GetValueOrDefault() == num2 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputB;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num4 = 625;
          if (nullable3.GetValueOrDefault() == num4 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputB;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num5 = 187;
            num3 = nullable3.GetValueOrDefault() == num5 & nullable3.HasValue ? 1 : 0;
            goto label_5;
          }
        }
        num3 = 0;
label_5:
        if (num3 != 0)
          return 2.5.ToString();
        nullable2 = oversizeDiffInputB;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num6 = 100;
        int num7;
        if (nullable3.GetValueOrDefault() == num6 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputB;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num8 = 1000;
          if (nullable3.GetValueOrDefault() == num8 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputB;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num9 = 300;
            num7 = nullable3.GetValueOrDefault() == num9 & nullable3.HasValue ? 1 : 0;
            goto label_11;
          }
        }
        num7 = 0;
label_11:
        if (num7 != 0)
          return 4.ToString();
        nullable2 = oversizeDiffInputB;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num10 = 157;
        int num11;
        if (nullable3.GetValueOrDefault() == num10 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputB;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num12 = 1575;
          if (nullable3.GetValueOrDefault() == num12 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputB;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num13 = 472;
            num11 = nullable3.GetValueOrDefault() == num13 & nullable3.HasValue ? 1 : 0;
            goto label_17;
          }
        }
        num11 = 0;
label_17:
        if (num11 != 0)
          return 6.3.ToString();
        nullable2 = oversizeDiffInputB;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num14 = 250;
        int num15;
        if (nullable3.GetValueOrDefault() == num14 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputB;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num16 = 2500;
          if (nullable3.GetValueOrDefault() == num16 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputB;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num17 = 750;
            num15 = nullable3.GetValueOrDefault() == num17 & nullable3.HasValue ? 1 : 0;
            goto label_23;
          }
        }
        num15 = 0;
label_23:
        if (num15 != 0)
          return 10.ToString();
        nullable2 = oversizeDiffInputB;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num18 = 400;
        int num19;
        if (nullable3.GetValueOrDefault() == num18 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputB;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num20 = 4000;
          if (nullable3.GetValueOrDefault() == num20 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputB;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num21 = 1200;
            num19 = nullable3.GetValueOrDefault() == num21 & nullable3.HasValue ? 1 : 0;
            goto label_29;
          }
        }
        num19 = 0;
label_29:
        if (num19 != 0)
          return 16.ToString();
        nullable2 = oversizeDiffInputB;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num22 = 625;
        int num23;
        if (nullable3.GetValueOrDefault() == num22 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputB;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num24 = 6250;
          if (nullable3.GetValueOrDefault() == num24 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputB;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num25 = 1875;
            num23 = nullable3.GetValueOrDefault() == num25 & nullable3.HasValue ? 1 : 0;
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
        double? nullable4 = scaleFactorInputB;
        double num26 = 10.0;
        if (nullable4.GetValueOrDefault() == num26 & nullable4.HasValue)
        {
          ushort? nullable5 = oversizeDiffInputB;
          int? nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num27 = 62;
          int num28;
          if (nullable6.GetValueOrDefault() == num27 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputB;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num29 = 625;
            if (nullable6.GetValueOrDefault() == num29 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputB;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num30 = 187;
              num28 = nullable6.GetValueOrDefault() == num30 & nullable6.HasValue ? 1 : 0;
              goto label_42;
            }
          }
          num28 = 0;
label_42:
          if (num28 != 0)
            return 25.ToString();
          nullable5 = oversizeDiffInputB;
          nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num31 = 100;
          int num32;
          if (nullable6.GetValueOrDefault() == num31 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputB;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num33 = 1000;
            if (nullable6.GetValueOrDefault() == num33 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputB;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num34 = 300;
              num32 = nullable6.GetValueOrDefault() == num34 & nullable6.HasValue ? 1 : 0;
              goto label_48;
            }
          }
          num32 = 0;
label_48:
          if (num32 != 0)
            return 40.ToString();
          nullable5 = oversizeDiffInputB;
          nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num35 = 157;
          int num36;
          if (nullable6.GetValueOrDefault() == num35 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputB;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num37 = 1575;
            if (nullable6.GetValueOrDefault() == num37 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputB;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num38 = 472;
              num36 = nullable6.GetValueOrDefault() == num38 & nullable6.HasValue ? 1 : 0;
              goto label_54;
            }
          }
          num36 = 0;
label_54:
          if (num36 != 0)
            return 63.ToString();
          nullable5 = oversizeDiffInputB;
          nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num39 = 250;
          int num40;
          if (nullable6.GetValueOrDefault() == num39 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputB;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num41 = 2500;
            if (nullable6.GetValueOrDefault() == num41 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputB;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num42 = 750;
              num40 = nullable6.GetValueOrDefault() == num42 & nullable6.HasValue ? 1 : 0;
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
          double? nullable7 = scaleFactorInputB;
          double num43 = 100.0;
          if (nullable7.GetValueOrDefault() == num43 & nullable7.HasValue)
          {
            ushort? nullable8 = oversizeDiffInputB;
            int? nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num44 = 40;
            int num45;
            if (nullable9.GetValueOrDefault() == num44 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputB;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num46 = 400;
              if (nullable9.GetValueOrDefault() == num46 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputB;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num47 = 120;
                num45 = nullable9.GetValueOrDefault() == num47 & nullable9.HasValue ? 1 : 0;
                goto label_67;
              }
            }
            num45 = 0;
label_67:
            if (num45 != 0)
              return 160.ToString();
            nullable8 = oversizeDiffInputB;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num48 = 62;
            int num49;
            if (nullable9.GetValueOrDefault() == num48 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputB;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num50 = 625;
              if (nullable9.GetValueOrDefault() == num50 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputB;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num51 = 187;
                num49 = nullable9.GetValueOrDefault() == num51 & nullable9.HasValue ? 1 : 0;
                goto label_73;
              }
            }
            num49 = 0;
label_73:
            if (num49 != 0)
              return 250.ToString();
            nullable8 = oversizeDiffInputB;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num52 = 100;
            int num53;
            if (nullable9.GetValueOrDefault() == num52 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputB;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num54 = 1000;
              if (nullable9.GetValueOrDefault() == num54 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputB;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num55 = 300;
                num53 = nullable9.GetValueOrDefault() == num55 & nullable9.HasValue ? 1 : 0;
                goto label_79;
              }
            }
            num53 = 0;
label_79:
            if (num53 != 0)
              return 400.ToString();
            nullable8 = oversizeDiffInputB;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num56 = 157;
            int num57;
            if (nullable9.GetValueOrDefault() == num56 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputB;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num58 = 1575;
              if (nullable9.GetValueOrDefault() == num58 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputB;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num59 = 472;
                num57 = nullable9.GetValueOrDefault() == num59 & nullable9.HasValue ? 1 : 0;
                goto label_85;
              }
            }
            num57 = 0;
label_85:
            if (num57 != 0)
              return 630.ToString();
            nullable8 = oversizeDiffInputB;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num60 = 250;
            int num61;
            if (nullable9.GetValueOrDefault() == num60 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputB;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num62 = 2500;
              if (nullable9.GetValueOrDefault() == num62 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputB;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num63 = 750;
                num61 = nullable9.GetValueOrDefault() == num63 & nullable9.HasValue ? 1 : 0;
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

    public string[] GetNominalFlowAllowedValuesB()
    {
      double? scaleFactorInputB = this.GetScaleFactorInputB();
      double? nullable1 = scaleFactorInputB;
      double num1 = 1.0;
      if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
      {
        string[] flowAllowedValuesB = new string[7];
        double num2 = 2.5;
        flowAllowedValuesB[0] = num2.ToString();
        flowAllowedValuesB[1] = 4.ToString();
        num2 = 6.3;
        flowAllowedValuesB[2] = num2.ToString();
        int num3 = 10;
        flowAllowedValuesB[3] = num3.ToString();
        num3 = 16;
        flowAllowedValuesB[4] = num3.ToString();
        num3 = 25;
        flowAllowedValuesB[5] = num3.ToString();
        flowAllowedValuesB[6] = string.Empty;
        return flowAllowedValuesB;
      }
      double? nullable2 = scaleFactorInputB;
      double num4 = 10.0;
      if (nullable2.GetValueOrDefault() == num4 & nullable2.HasValue)
      {
        string[] flowAllowedValuesB = new string[5];
        int num5 = 25;
        flowAllowedValuesB[0] = num5.ToString();
        num5 = 40;
        flowAllowedValuesB[1] = num5.ToString();
        num5 = 63;
        flowAllowedValuesB[2] = num5.ToString();
        num5 = 100;
        flowAllowedValuesB[3] = num5.ToString();
        flowAllowedValuesB[4] = string.Empty;
        return flowAllowedValuesB;
      }
      nullable2 = scaleFactorInputB;
      double num6 = 100.0;
      if (!(nullable2.GetValueOrDefault() == num6 & nullable2.HasValue))
        return new string[0];
      string[] flowAllowedValuesB1 = new string[6];
      int num7 = 160;
      flowAllowedValuesB1[0] = num7.ToString();
      num7 = 250;
      flowAllowedValuesB1[1] = num7.ToString();
      num7 = 400;
      flowAllowedValuesB1[2] = num7.ToString();
      num7 = 630;
      flowAllowedValuesB1[3] = num7.ToString();
      num7 = 1000;
      flowAllowedValuesB1[4] = num7.ToString();
      flowAllowedValuesB1[5] = string.Empty;
      return flowAllowedValuesB1;
    }

    public void SetNominalFlowA(double value)
    {
      double? scaleFactorInputA = this.GetScaleFactorInputA();
      double? nullable1 = scaleFactorInputA;
      double num1 = 1.0;
      if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
      {
        if (2.5 == value)
          this.SetValuesFromNominalFlowA((ushort) 62, (ushort) 625, (ushort) 187);
        if (4.0 == value)
          this.SetValuesFromNominalFlowA((ushort) 100, (ushort) 1000, (ushort) 300);
        if (6.3 == value)
          this.SetValuesFromNominalFlowA((ushort) 157, (ushort) 1575, (ushort) 472);
        if (10.0 == value)
          this.SetValuesFromNominalFlowA((ushort) 250, (ushort) 2500, (ushort) 750);
        if (16.0 == value)
          this.SetValuesFromNominalFlowA((ushort) 400, (ushort) 4000, (ushort) 1200);
        if (25.0 != value)
          return;
        this.SetValuesFromNominalFlowA((ushort) 625, (ushort) 6250, (ushort) 1875);
      }
      else
      {
        double? nullable2 = scaleFactorInputA;
        double num2 = 10.0;
        if (nullable2.GetValueOrDefault() == num2 & nullable2.HasValue)
        {
          if (25.0 == value)
            this.SetValuesFromNominalFlowA((ushort) 62, (ushort) 625, (ushort) 187);
          if (40.0 == value)
            this.SetValuesFromNominalFlowA((ushort) 100, (ushort) 1000, (ushort) 300);
          if (63.0 == value)
            this.SetValuesFromNominalFlowA((ushort) 157, (ushort) 1575, (ushort) 472);
          if (100.0 != value)
            return;
          this.SetValuesFromNominalFlowA((ushort) 250, (ushort) 2500, (ushort) 750);
        }
        else
        {
          nullable2 = scaleFactorInputA;
          double num3 = 100.0;
          if (nullable2.GetValueOrDefault() == num3 & nullable2.HasValue)
          {
            if (160.0 == value)
              this.SetValuesFromNominalFlowA((ushort) 40, (ushort) 400, (ushort) 120);
            if (250.0 == value)
              this.SetValuesFromNominalFlowA((ushort) 62, (ushort) 625, (ushort) 187);
            if (400.0 == value)
              this.SetValuesFromNominalFlowA((ushort) 100, (ushort) 1000, (ushort) 300);
            if (630.0 == value)
              this.SetValuesFromNominalFlowA((ushort) 157, (ushort) 1575, (ushort) 472);
            if (1000.0 != value)
              return;
            this.SetValuesFromNominalFlowA((ushort) 250, (ushort) 2500, (ushort) 750);
          }
          else
          {
            nullable2 = scaleFactorInputA;
            double num4 = 0.5;
            int num5;
            if (!(nullable2.GetValueOrDefault() == num4 & nullable2.HasValue))
            {
              nullable2 = scaleFactorInputA;
              double num6 = 5.0;
              num5 = nullable2.GetValueOrDefault() == num6 & nullable2.HasValue ? 1 : 0;
            }
            else
              num5 = 1;
            if (num5 != 0)
            {
              double num7 = value * 1000.0 / 4.0;
              this.SetValuesFromNominalFlowA(Convert.ToUInt16(num7 * 0.1 / scaleFactorInputA.Value), Convert.ToUInt16(num7 * 1.0 / scaleFactorInputA.Value), Convert.ToUInt16(num7 * 0.3 / scaleFactorInputA.Value));
            }
            else
            {
              string str1 = value.ToString();
              nullable2 = scaleFactorInputA;
              string str2 = nullable2.ToString();
              throw new NotImplementedException("Nominal flow: " + str1 + ", pulseMultiplierA: " + str2);
            }
          }
        }
      }
    }

    private void SetValuesFromNominalFlowA(
      ushort oversizeDiff,
      ushort undersizeDiff,
      ushort burstDiff)
    {
      this.SetOversizeDiffInputA(oversizeDiff);
      this.SetUndersizeDiffInputA(undersizeDiff);
      this.SetBurstDiffInputA(burstDiff);
    }

    public string GetNominalFlowA()
    {
      double? scaleFactorInputA = this.GetScaleFactorInputA();
      ushort? oversizeDiffInputA = this.GetOversizeDiffInputA();
      ushort? undersizeDiffInputA = this.GetUndersizeDiffInputA();
      ushort? burstDiffInputA = this.GetBurstDiffInputA();
      double? nullable1 = scaleFactorInputA;
      double num1 = 1.0;
      if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
      {
        ushort? nullable2 = oversizeDiffInputA;
        int? nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num2 = 62;
        int num3;
        if (nullable3.GetValueOrDefault() == num2 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputA;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num4 = 625;
          if (nullable3.GetValueOrDefault() == num4 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputA;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num5 = 187;
            num3 = nullable3.GetValueOrDefault() == num5 & nullable3.HasValue ? 1 : 0;
            goto label_5;
          }
        }
        num3 = 0;
label_5:
        if (num3 != 0)
          return 2.5.ToString();
        nullable2 = oversizeDiffInputA;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num6 = 100;
        int num7;
        if (nullable3.GetValueOrDefault() == num6 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputA;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num8 = 1000;
          if (nullable3.GetValueOrDefault() == num8 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputA;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num9 = 300;
            num7 = nullable3.GetValueOrDefault() == num9 & nullable3.HasValue ? 1 : 0;
            goto label_11;
          }
        }
        num7 = 0;
label_11:
        if (num7 != 0)
          return 4.ToString();
        nullable2 = oversizeDiffInputA;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num10 = 157;
        int num11;
        if (nullable3.GetValueOrDefault() == num10 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputA;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num12 = 1575;
          if (nullable3.GetValueOrDefault() == num12 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputA;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num13 = 472;
            num11 = nullable3.GetValueOrDefault() == num13 & nullable3.HasValue ? 1 : 0;
            goto label_17;
          }
        }
        num11 = 0;
label_17:
        if (num11 != 0)
          return 6.3.ToString();
        nullable2 = oversizeDiffInputA;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num14 = 250;
        int num15;
        if (nullable3.GetValueOrDefault() == num14 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputA;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num16 = 2500;
          if (nullable3.GetValueOrDefault() == num16 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputA;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num17 = 750;
            num15 = nullable3.GetValueOrDefault() == num17 & nullable3.HasValue ? 1 : 0;
            goto label_23;
          }
        }
        num15 = 0;
label_23:
        if (num15 != 0)
          return 10.ToString();
        nullable2 = oversizeDiffInputA;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num18 = 400;
        int num19;
        if (nullable3.GetValueOrDefault() == num18 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputA;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num20 = 4000;
          if (nullable3.GetValueOrDefault() == num20 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputA;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num21 = 1200;
            num19 = nullable3.GetValueOrDefault() == num21 & nullable3.HasValue ? 1 : 0;
            goto label_29;
          }
        }
        num19 = 0;
label_29:
        if (num19 != 0)
          return 16.ToString();
        nullable2 = oversizeDiffInputA;
        nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        int num22 = 625;
        int num23;
        if (nullable3.GetValueOrDefault() == num22 & nullable3.HasValue)
        {
          nullable2 = undersizeDiffInputA;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
          int num24 = 6250;
          if (nullable3.GetValueOrDefault() == num24 & nullable3.HasValue)
          {
            nullable2 = burstDiffInputA;
            nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
            int num25 = 1875;
            num23 = nullable3.GetValueOrDefault() == num25 & nullable3.HasValue ? 1 : 0;
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
        double? nullable4 = scaleFactorInputA;
        double num26 = 10.0;
        if (nullable4.GetValueOrDefault() == num26 & nullable4.HasValue)
        {
          ushort? nullable5 = oversizeDiffInputA;
          int? nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num27 = 62;
          int num28;
          if (nullable6.GetValueOrDefault() == num27 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputA;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num29 = 625;
            if (nullable6.GetValueOrDefault() == num29 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputA;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num30 = 187;
              num28 = nullable6.GetValueOrDefault() == num30 & nullable6.HasValue ? 1 : 0;
              goto label_42;
            }
          }
          num28 = 0;
label_42:
          if (num28 != 0)
            return 25.ToString();
          nullable5 = oversizeDiffInputA;
          nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num31 = 100;
          int num32;
          if (nullable6.GetValueOrDefault() == num31 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputA;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num33 = 1000;
            if (nullable6.GetValueOrDefault() == num33 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputA;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num34 = 300;
              num32 = nullable6.GetValueOrDefault() == num34 & nullable6.HasValue ? 1 : 0;
              goto label_48;
            }
          }
          num32 = 0;
label_48:
          if (num32 != 0)
            return 40.ToString();
          nullable5 = oversizeDiffInputA;
          nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num35 = 157;
          int num36;
          if (nullable6.GetValueOrDefault() == num35 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputA;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num37 = 1575;
            if (nullable6.GetValueOrDefault() == num37 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputA;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num38 = 472;
              num36 = nullable6.GetValueOrDefault() == num38 & nullable6.HasValue ? 1 : 0;
              goto label_54;
            }
          }
          num36 = 0;
label_54:
          if (num36 != 0)
            return 63.ToString();
          nullable5 = oversizeDiffInputA;
          nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
          int num39 = 250;
          int num40;
          if (nullable6.GetValueOrDefault() == num39 & nullable6.HasValue)
          {
            nullable5 = undersizeDiffInputA;
            nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
            int num41 = 2500;
            if (nullable6.GetValueOrDefault() == num41 & nullable6.HasValue)
            {
              nullable5 = burstDiffInputA;
              nullable6 = nullable5.HasValue ? new int?((int) nullable5.GetValueOrDefault()) : new int?();
              int num42 = 750;
              num40 = nullable6.GetValueOrDefault() == num42 & nullable6.HasValue ? 1 : 0;
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
          double? nullable7 = scaleFactorInputA;
          double num43 = 100.0;
          if (nullable7.GetValueOrDefault() == num43 & nullable7.HasValue)
          {
            ushort? nullable8 = oversizeDiffInputA;
            int? nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num44 = 40;
            int num45;
            if (nullable9.GetValueOrDefault() == num44 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputA;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num46 = 400;
              if (nullable9.GetValueOrDefault() == num46 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputA;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num47 = 120;
                num45 = nullable9.GetValueOrDefault() == num47 & nullable9.HasValue ? 1 : 0;
                goto label_67;
              }
            }
            num45 = 0;
label_67:
            if (num45 != 0)
              return 160.ToString();
            nullable8 = oversizeDiffInputA;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num48 = 62;
            int num49;
            if (nullable9.GetValueOrDefault() == num48 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputA;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num50 = 625;
              if (nullable9.GetValueOrDefault() == num50 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputA;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num51 = 187;
                num49 = nullable9.GetValueOrDefault() == num51 & nullable9.HasValue ? 1 : 0;
                goto label_73;
              }
            }
            num49 = 0;
label_73:
            if (num49 != 0)
              return 250.ToString();
            nullable8 = oversizeDiffInputA;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num52 = 100;
            int num53;
            if (nullable9.GetValueOrDefault() == num52 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputA;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num54 = 1000;
              if (nullable9.GetValueOrDefault() == num54 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputA;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num55 = 300;
                num53 = nullable9.GetValueOrDefault() == num55 & nullable9.HasValue ? 1 : 0;
                goto label_79;
              }
            }
            num53 = 0;
label_79:
            if (num53 != 0)
              return 400.ToString();
            nullable8 = oversizeDiffInputA;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num56 = 157;
            int num57;
            if (nullable9.GetValueOrDefault() == num56 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputA;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num58 = 1575;
              if (nullable9.GetValueOrDefault() == num58 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputA;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num59 = 472;
                num57 = nullable9.GetValueOrDefault() == num59 & nullable9.HasValue ? 1 : 0;
                goto label_85;
              }
            }
            num57 = 0;
label_85:
            if (num57 != 0)
              return 630.ToString();
            nullable8 = oversizeDiffInputA;
            nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
            int num60 = 250;
            int num61;
            if (nullable9.GetValueOrDefault() == num60 & nullable9.HasValue)
            {
              nullable8 = undersizeDiffInputA;
              nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
              int num62 = 2500;
              if (nullable9.GetValueOrDefault() == num62 & nullable9.HasValue)
              {
                nullable8 = burstDiffInputA;
                nullable9 = nullable8.HasValue ? new int?((int) nullable8.GetValueOrDefault()) : new int?();
                int num63 = 750;
                num61 = nullable9.GetValueOrDefault() == num63 & nullable9.HasValue ? 1 : 0;
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

    public string[] GetNominalFlowAllowedValuesA()
    {
      double? scaleFactorInputA = this.GetScaleFactorInputA();
      double? nullable1 = scaleFactorInputA;
      double num1 = 1.0;
      if (nullable1.GetValueOrDefault() == num1 & nullable1.HasValue)
      {
        string[] flowAllowedValuesA = new string[7];
        double num2 = 2.5;
        flowAllowedValuesA[0] = num2.ToString();
        flowAllowedValuesA[1] = 4.ToString();
        num2 = 6.3;
        flowAllowedValuesA[2] = num2.ToString();
        int num3 = 10;
        flowAllowedValuesA[3] = num3.ToString();
        num3 = 16;
        flowAllowedValuesA[4] = num3.ToString();
        num3 = 25;
        flowAllowedValuesA[5] = num3.ToString();
        flowAllowedValuesA[6] = string.Empty;
        return flowAllowedValuesA;
      }
      double? nullable2 = scaleFactorInputA;
      double num4 = 10.0;
      if (nullable2.GetValueOrDefault() == num4 & nullable2.HasValue)
      {
        string[] flowAllowedValuesA = new string[5];
        int num5 = 25;
        flowAllowedValuesA[0] = num5.ToString();
        num5 = 40;
        flowAllowedValuesA[1] = num5.ToString();
        num5 = 63;
        flowAllowedValuesA[2] = num5.ToString();
        num5 = 100;
        flowAllowedValuesA[3] = num5.ToString();
        flowAllowedValuesA[4] = string.Empty;
        return flowAllowedValuesA;
      }
      nullable2 = scaleFactorInputA;
      double num6 = 100.0;
      if (!(nullable2.GetValueOrDefault() == num6 & nullable2.HasValue))
        return new string[0];
      string[] flowAllowedValuesA1 = new string[6];
      int num7 = 160;
      flowAllowedValuesA1[0] = num7.ToString();
      num7 = 250;
      flowAllowedValuesA1[1] = num7.ToString();
      num7 = 400;
      flowAllowedValuesA1[2] = num7.ToString();
      num7 = 630;
      flowAllowedValuesA1[3] = num7.ToString();
      num7 = 1000;
      flowAllowedValuesA1[4] = num7.ToString();
      flowAllowedValuesA1[5] = string.Empty;
      return flowAllowedValuesA1;
    }

    public bool SetWarningsInputA(Warning value)
    {
      return this.SetParameterValue<ushort>("persistentFlagsA", (ushort) value);
    }

    public Warning? GetWarningsInputA()
    {
      return new Warning?((Warning) this.GetParameterValue<ushort>("persistentFlagsA"));
    }

    public bool SetWarningsInputB(Warning value)
    {
      return this.SetParameterValue<ushort>("persistentFlagsB", (ushort) value);
    }

    public Warning? GetWarningsInputB()
    {
      return new Warning?((Warning) this.GetParameterValue<ushort>("persistentFlagsB"));
    }

    public bool SetHardwareErrors(HardwareError value)
    {
      return this.SetParameterValue<ushort>("hwStatusFlags", (ushort) value);
    }

    public HardwareError? GetHardwareErrors()
    {
      return new HardwareError?((HardwareError) this.GetParameterValue<ushort>("hwStatusFlags"));
    }

    public bool SetBatteryEndDate(DateTime value)
    {
      if (value.Year < 2000 || value.Year > 2255)
        throw new ArgumentException("Can not set BatteryEndDate! The year of new value is invalid (Valid are: 2000-2255). Value: " + value.ToString());
      if (this.GetParameter("cfg_lowbatt_year") == null)
        throw new Exception("The firmware " + this.Version?.ToString() + " not supports BatteryEndDate parameter!");
      return this.SetParameterValue<byte>("cfg_lowbatt_year", (byte) (value.Year - 2000)) && this.SetParameterValue<byte>("cfg_lowbatt_month", (byte) value.Month) && this.SetParameterValue<byte>("cfg_lowbatt_day", (byte) value.Day);
    }

    public DateTime GetBatteryEndDate()
    {
      if (this.GetParameter("cfg_lowbatt_year") == null)
        throw new Exception("The firmware " + this.Version?.ToString() + " not supports BatteryEndDate parameter!");
      byte parameterValue1 = this.GetParameterValue<byte>("cfg_lowbatt_year");
      byte parameterValue2 = this.GetParameterValue<byte>("cfg_lowbatt_month");
      byte parameterValue3 = this.GetParameterValue<byte>("cfg_lowbatt_day");
      if (parameterValue1 == byte.MaxValue || parameterValue2 == byte.MaxValue || parameterValue3 == byte.MaxValue)
        return PDC_HandlerFunctions.DateTimeNull;
      if (parameterValue1 > byte.MaxValue || parameterValue2 > (byte) 12 || parameterValue3 > (byte) 31)
        return PDC_HandlerFunctions.DateTimeNull;
      try
      {
        return new DateTime(2000 + (int) parameterValue1, (int) parameterValue2, (int) parameterValue3);
      }
      catch
      {
        return PDC_HandlerFunctions.DateTimeNull;
      }
    }

    public bool SetObisPDC(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set manufacturer! Input parameter 'value' is null.");
      return value.Length == 1 ? this.SetParameterValue<byte>("cfg_obis_c", Convert.ToByte(value, 16)) : throw new ArgumentException("Can not set manufacturer! The length of input parameter 'value' is not 1 chars.");
    }

    public string GetObisPDC()
    {
      return Convert.ToString(this.GetParameterValue<byte>("cfg_obis_c"), 16).ToUpper();
    }

    public bool SetObisInputA(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set obis number! Input parameter 'value' is null.");
      return value.Length == 1 ? this.SetParameterValue<byte>("cfg_obis_a", Convert.ToByte(value, 16)) : throw new ArgumentException("Can not set obis number! The length of input parameter 'value' is not 1 chars.");
    }

    public string GetObisInputA()
    {
      return Convert.ToString(this.GetParameterValue<byte>("cfg_obis_a"), 16).ToUpper();
    }

    public bool SetObisInputB(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Can not set obis number! Input parameter 'value' is null.");
      return value.Length == 1 ? this.SetParameterValue<byte>("cfg_obis_b", Convert.ToByte(value, 16)) : throw new ArgumentException("Can not set obis number! The length of input parameter 'value' is not 1 chars.");
    }

    public string GetObisInputB()
    {
      return Convert.ToString(this.GetParameterValue<byte>("cfg_obis_b"), 16).ToUpper();
    }

    public bool SetSerialMBusPDC(uint value)
    {
      return this.SetParameterValue<uint>("cfg_serial_c", Util.ConvertUnt32ToBcdUInt32(value));
    }

    public uint? GetSerialMBusPDC()
    {
      return new uint?(Util.ConvertBcdUInt32ToUInt32(this.GetParameterValue<uint>("cfg_serial_c")));
    }

    public bool SetSerialMBusInputA(uint value)
    {
      return this.SetParameterValue<uint>("cfg_serial_a", Util.ConvertUnt32ToBcdUInt32(value));
    }

    public uint? GetSerialMBusInputA()
    {
      return new uint?(Util.ConvertBcdUInt32ToUInt32(this.GetParameterValue<uint>("cfg_serial_a")));
    }

    public bool SetSerialMBusInputB(uint value)
    {
      return this.SetParameterValue<uint>("cfg_serial_b", Util.ConvertUnt32ToBcdUInt32(value));
    }

    public uint? GetSerialMBusInputB()
    {
      return new uint?(Util.ConvertBcdUInt32ToUInt32(this.GetParameterValue<uint>("cfg_serial_b")));
    }

    public ConfigInit? GetConfigInitIndicator()
    {
      return new ConfigInit?((ConfigInit) this.GetParameterValue<byte>("configInitIndicator"));
    }

    public bool DisableLeakDetectionInputA() => this.SetPulseLeakLimitInputA((ushort) 0);

    public bool DisableBurstDetectionInputA() => this.SetBurstLimitInputA((ushort) 0);

    public bool DisableStandstillDetectionInputA() => this.SetPulseBlockLimitInputA((ushort) 0);

    public bool DisableUndersizeDetectionInputA() => this.SetUndersizeLimitInputA((ushort) 0);

    public bool DisableOversizeDetectionInputA() => this.SetOversizeLimitInputA((ushort) 0);

    public bool DisableLeakDetectionInputB() => this.SetPulseLeakLimitInputB((ushort) 0);

    public bool DisableBurstDetectionInputB() => this.SetBurstLimitInputB((ushort) 0);

    public bool DisableStandstillDetectionInputB() => this.SetPulseBlockLimitInputB((ushort) 0);

    public bool DisableUndersizeDetectionInputB() => this.SetUndersizeLimitInputB((ushort) 0);

    public bool DisableOversizeDetectionInputB() => this.SetOversizeLimitInputB((ushort) 0);

    internal SortedList<long, SortedList<DateTime, ReadingValue>> GetValues(
      int channel,
      List<long> filter)
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      DateTime? systemTime = this.GetSystemTime();
      if (!systemTime.HasValue)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      DateTime timePoint = systemTime.Value;
      Warning? warnings = new Warning?();
      MBusDeviceType? nullable1 = new MBusDeviceType?();
      byte? nullable2 = new byte?();
      switch (channel)
      {
        case 1:
          warnings = this.GetWarningsInputA();
          nullable1 = this.GetMediumInputA();
          nullable2 = this.GetVIFInputA();
          break;
        case 2:
          warnings = this.GetWarningsInputB();
          nullable1 = this.GetMediumInputB();
          nullable2 = this.GetVIFInputB();
          break;
      }
      if (!nullable1.HasValue)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      ValueIdent.ValueIdPart_MeterType meterType = ValueIdent.ConvertToMeterType(nullable1.Value);
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ConvertToPhysicalQuantity(nullable2.Value);
      HardwareError? hardwareErrors = this.GetHardwareErrors();
      if (this.IsWarning(warnings, Warning.WARNING_BATT_LOW))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(meterType, ValueIdent.ValueIdentWarning.BatteryLow, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.WARNING_LEAK))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(meterType, ValueIdent.ValueIdentWarning.Leak, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.WARNING_BLOCK))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(meterType, ValueIdent.ValueIdentWarning.Blockage, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.WARNING_OVERSIZE))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(meterType, ValueIdent.ValueIdentWarning.Oversized, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.WARNING_UNDERSIZE))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfWarninig(meterType, ValueIdent.ValueIdentWarning.Undersized, ValueIdent.ValueIdPart_Creation.Meter), (object) 1);
      if (this.IsWarning(warnings, Warning.WARNING_ABNORMAL) || this.IsWarning(warnings, Warning.WARNING_PERMANENT_ERROR) || this.IsError(hardwareErrors, HardwareError.HW_ERROR_OSCILLATOR))
        ValueIdent.AddValueToValueIdentList(ref valueList, new DateTime(timePoint.Year, timePoint.Month, timePoint.Day), ValueIdent.GetValueIdentOfError(meterType, ValueIdent.ValueIdentError.DeviceError), (object) 1);
      long valueIdForValueEnum = ValueIdent.GetValueIdForValueEnum(physicalQuantity, meterType, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
      if (ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum))
      {
        uint? nullable3 = new uint?();
        switch (channel)
        {
          case 1:
            nullable3 = this.GetMeterValueA();
            break;
          case 2:
            nullable3 = this.GetMeterValueB();
            break;
        }
        if (nullable3.HasValue)
          ValueIdent.AddValueToValueIdentList(ref valueList, timePoint, valueIdForValueEnum, (object) nullable3.Value);
      }
      List<RamLogger> ramLogger = LoggerManager.ParseRamLogger(this);
      if (ramLogger == null)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      this.CreateValues(channel, meterType, physicalQuantity, ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.QuarterHour, RamLoggerType.QuarterHour);
      this.CreateValues(channel, meterType, physicalQuantity, ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.Day, RamLoggerType.Daily);
      this.CreateValues(channel, meterType, physicalQuantity, ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.HalfMonth, RamLoggerType.Halfmonth);
      this.CreateValues(channel, meterType, physicalQuantity, ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.Month, RamLoggerType.Fullmonth);
      this.CreateValues(channel, meterType, physicalQuantity, ref valueList, ramLogger, filter, ValueIdent.ValueIdPart_StorageInterval.DueDate, RamLoggerType.DueDate);
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
      int channel,
      ValueIdent.ValueIdPart_MeterType medium,
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      List<RamLogger> loggers,
      List<long> filter,
      ValueIdent.ValueIdPart_StorageInterval interval,
      RamLoggerType type)
    {
      long valueIdForValueEnum = ValueIdent.GetValueIdForValueEnum(physicalQuantity, medium, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, interval, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
      if (!ValueIdent.IsExpectedValueIdent(filter, valueIdForValueEnum))
        return;
      RamLogger ramLogger = loggers.Find((Predicate<RamLogger>) (x => x.Type == type));
      if (ramLogger != null)
      {
        SortedList<DateTime, ReadValue> values = ramLogger.Values;
        if (values != null)
        {
          foreach (KeyValuePair<DateTime, ReadValue> keyValuePair in values)
          {
            switch (channel)
            {
              case 1:
                ValueIdent.AddValueToValueIdentList(ref valueList, keyValuePair.Key, valueIdForValueEnum, (object) keyValuePair.Value.A);
                continue;
              case 2:
                ValueIdent.AddValueToValueIdentList(ref valueList, keyValuePair.Key, valueIdForValueEnum, (object) keyValuePair.Value.B);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    public string GetInfo() => this.ToString();
  }
}
