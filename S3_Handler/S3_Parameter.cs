// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_Parameter
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using GmmDbLib;
using NLog;
using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_Parameter : S3_MemoryBlock
  {
    internal static Logger S3_ParameterLogger = LogManager.GetLogger(nameof (S3_ParameterLogger));
    public const string C5_Manufacturer = "ZRI";
    public const string C5_Medium = "HEAT";
    public const int C5_MBusVersion = 136;
    internal S3_ParameterStatics Statics;
    internal string Name;
    internal int SubDevice;

    internal bool IsProtected
    {
      get
      {
        for (int index = 0; index < this.ByteSize; ++index)
        {
          if (this.MyMeter.MyWriteProtTableManager.IsByteProtected((ushort) (this.BlockStartAddress + index)))
            return true;
        }
        return false;
      }
    }

    internal byte[] PreparedParameterValue { get; set; }

    internal S3_Parameter(
      string UniqueS3ParameterName,
      S3_VariableTypes S3_VarType,
      S3_MemorySegment SegmentType)
      : base((S3_Meter) null, SegmentType)
    {
      this.Statics = new S3_ParameterStatics(S3_VarType);
      this.Name = UniqueS3ParameterName;
      this.ByteSize = ParameterService.GetNumberOfBytes(S3_VarType);
    }

    internal S3_Parameter(
      S3_Meter MyMeter,
      string UniqueS3ParameterName,
      S3_VariableTypes S3_VarType,
      S3_MemorySegment SegmentType)
      : base(MyMeter, SegmentType)
    {
      this.Statics = new S3_ParameterStatics(S3_VarType);
      this.Name = UniqueS3ParameterName;
      this.ByteSize = ParameterService.GetNumberOfBytes(S3_VarType);
    }

    internal S3_Parameter(
      S3_Meter MyMeter,
      string UniqueS3ParameterName,
      S3_ParameterStatics theStatics,
      S3_MemorySegment SegmentType)
      : base(MyMeter, SegmentType)
    {
      this.Statics = theStatics;
      this.Name = UniqueS3ParameterName;
      this.ByteSize = ParameterService.GetNumberOfBytes(theStatics.S3_VarType);
    }

    internal S3_Parameter(
      S3_Meter MyMeter,
      string UniqueS3ParameterName,
      S3_VariableTypes S3_VarType,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, ParameterService.GetNumberOfBytes(S3_VarType), SegmentType, parentMemoryBlock)
    {
      this.Statics = new S3_ParameterStatics(S3_VarType);
      this.Name = UniqueS3ParameterName;
    }

    internal S3_Parameter(
      S3_Meter MyMeter,
      string UniqueS3ParameterName,
      S3_ParameterStatics theStatics,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, ParameterService.GetNumberOfBytes(theStatics.S3_VarType), SegmentType, parentMemoryBlock)
    {
      this.Statics = theStatics;
      this.Name = UniqueS3ParameterName;
    }

    internal S3_Parameter Clone(S3_Meter TheMeter = null)
    {
      S3_Parameter s3Parameter = TheMeter != null ? new S3_Parameter(TheMeter, this.Name, this.Statics, this.SegmentType) : new S3_Parameter(this.MyMeter, this.Name, this.Statics, this.SegmentType);
      s3Parameter.SubDevice = this.SubDevice;
      s3Parameter.IsHardLinkedAddress = this.IsHardLinkedAddress;
      s3Parameter.IsNotLinked = this.IsNotLinked;
      s3Parameter.Alignment = this.Alignment;
      s3Parameter.PreparedParameterValue = this.PreparedParameterValue;
      return s3Parameter;
    }

    internal S3_Parameter Clone(S3_Meter TheMeter, int blockStartAddress)
    {
      S3_Parameter s3Parameter = new S3_Parameter(TheMeter, this.Name, this.Statics, this.SegmentType);
      s3Parameter.BlockStartAddress = blockStartAddress;
      s3Parameter.SubDevice = this.SubDevice;
      s3Parameter.IsHardLinkedAddress = this.IsHardLinkedAddress;
      s3Parameter.IsNotLinked = this.IsNotLinked;
      s3Parameter.Alignment = this.Alignment;
      s3Parameter.PreparedParameterValue = this.PreparedParameterValue;
      return s3Parameter;
    }

    internal S3_Parameter Clone(
      S3_Meter TheMeter,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock)
    {
      S3_Parameter s3Parameter = new S3_Parameter(TheMeter, this.Name, this.Statics, SegmentType, parentMemoryBlock);
      s3Parameter.SubDevice = this.SubDevice;
      s3Parameter.IsHardLinkedAddress = this.IsHardLinkedAddress;
      s3Parameter.IsNotLinked = this.IsNotLinked;
      s3Parameter.Alignment = this.Alignment;
      s3Parameter.PreparedParameterValue = this.PreparedParameterValue;
      return s3Parameter;
    }

    internal S3_Parameter Clone(S3_Meter TheMeter, S3_MemoryBlock cloneParentMemoryBlock)
    {
      S3_Parameter s3Parameter = new S3_Parameter(TheMeter, this.Name, this.Statics, this.SegmentType, cloneParentMemoryBlock);
      s3Parameter.BlockStartAddress = this.BlockStartAddress;
      s3Parameter.SubDevice = this.SubDevice;
      s3Parameter.IsHardLinkedAddress = this.IsHardLinkedAddress;
      s3Parameter.IsNotLinked = this.IsNotLinked;
      s3Parameter.Alignment = this.Alignment;
      s3Parameter.PreparedParameterValue = this.PreparedParameterValue;
      return s3Parameter;
    }

    internal void SetToDefaultValue()
    {
      try
      {
        byte[] numArray;
        switch (this.Statics.S3_VarType)
        {
          case S3_VariableTypes.INT8:
          case S3_VariableTypes.INT16:
          case S3_VariableTypes.INT32:
          case S3_VariableTypes.INT64:
            numArray = BitConverter.GetBytes((long) this.Statics.DefaultValue);
            break;
          case S3_VariableTypes.UINT8:
          case S3_VariableTypes.UINT16:
          case S3_VariableTypes.UINT32:
          case S3_VariableTypes.UINT64:
            numArray = BitConverter.GetBytes((ulong) this.Statics.DefaultValue);
            break;
          case S3_VariableTypes.REAL32:
            numArray = BitConverter.GetBytes((float) this.Statics.DefaultValue);
            break;
          case S3_VariableTypes.REAL64:
            numArray = BitConverter.GetBytes(this.Statics.DefaultValue);
            break;
          case S3_VariableTypes.MeterTime1980:
            numArray = new byte[8];
            break;
          default:
            throw new Exception("Unsupported parameter type");
        }
        this.MyMeter.MyDeviceMemory.SetValue(BitConverter.ToUInt64(numArray, 0), this.BlockStartAddress, this.ByteSize);
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Set default value");
      }
    }

    internal byte GetByteValue()
    {
      return this.MyMeter.MyDeviceMemory.GetByteValue(this.BlockStartAddress);
    }

    internal byte[] GetByteArray(int Size)
    {
      return this.MyMeter.MyDeviceMemory.GetByteArray(this.BlockStartAddress, Size);
    }

    internal int GetFromSignedByteValue()
    {
      byte byteValue = this.MyMeter.MyDeviceMemory.GetByteValue(this.BlockStartAddress);
      return ((uint) byteValue & 128U) > 0U ? ((int) byteValue & (int) sbyte.MaxValue) - 128 : (int) byteValue;
    }

    internal bool SetByteValue(byte NewValue)
    {
      return this.MyMeter.MyDeviceMemory.SetByteValue(this.BlockStartAddress, NewValue);
    }

    internal bool SetByteArray(byte[] Value)
    {
      return this.MyMeter.MyDeviceMemory.SetByteArray(this.BlockStartAddress, Value);
    }

    internal ushort GetUshortValue()
    {
      return this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
    }

    internal bool SetUshortValue(ushort NewValue)
    {
      if (this.ByteSize != 2)
        throw new Exception("Illegal byte size on S3_Parameter access");
      return this.MyMeter.MyDeviceMemory.SetUlongValue(this.BlockStartAddress, 2, (ulong) NewValue);
    }

    internal short GetShortValue()
    {
      return (short) this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
    }

    internal bool SetShortValue(short NewValue)
    {
      if (this.ByteSize != 2)
        throw new Exception("Illegal byte size on S3_Parameter access");
      return this.MyMeter.MyDeviceMemory.SetUlongValue(this.BlockStartAddress, 2, (ulong) (ushort) NewValue);
    }

    internal uint GetUintValue()
    {
      return this.MyMeter.MyDeviceMemory.GetUintValue(this.BlockStartAddress);
    }

    internal DateTime GetDateTimeValue()
    {
      return this.MyMeter.MyDeviceMemory.GetDateTimeValue(this.BlockStartAddress);
    }

    internal bool SetUintValue(uint NewValue)
    {
      if (this.ByteSize != 4)
        throw new Exception("Illegal byte size on S3_Parameter access");
      return this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress, NewValue);
    }

    internal bool SetDateTimeValue(DateTime NewValue)
    {
      if (this.ByteSize != 4)
        throw new Exception("Illegal byte size on S3_Parameter access");
      return this.MyMeter.MyDeviceMemory.SetDateTimeValue(this.BlockStartAddress, NewValue);
    }

    internal int GetIntValue()
    {
      return (int) this.MyMeter.MyDeviceMemory.GetUintValue(this.BlockStartAddress);
    }

    internal bool SetIntValue(int NewValue)
    {
      if (this.ByteSize != 4)
        throw new Exception("Illegal byte size on S3_Parameter access");
      return this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress, (uint) NewValue);
    }

    internal ulong GetUlongValue()
    {
      return this.MyMeter.MyDeviceMemory.GetUlongValue(this.BlockStartAddress, this.ByteSize);
    }

    internal bool SetUlongValue(ulong NewValue)
    {
      return this.MyMeter.MyDeviceMemory.SetUlongValue(this.BlockStartAddress, this.ByteSize, NewValue);
    }

    internal float GetFloatValue()
    {
      return this.MyMeter.MyDeviceMemory.GetFloatValue(this.BlockStartAddress);
    }

    internal bool SetFloatValue(float NewValue)
    {
      if (this.ByteSize != 4)
        throw new Exception("Illegal byte size on S3_Parameter access");
      return this.MyMeter.MyDeviceMemory.SetFloatValue(this.BlockStartAddress, NewValue);
    }

    internal double GetDoubleValue()
    {
      return this.MyMeter.MyDeviceMemory.GetDoubleValue(this.BlockStartAddress);
    }

    internal bool SetDoubleValue(double NewValue)
    {
      return this.MyMeter.MyDeviceMemory.SetDoubleValue(this.BlockStartAddress, NewValue);
    }

    internal bool IsCacheInitialised()
    {
      return this.MyMeter.MyDeviceMemory.IsCacheInitialisedNoWarning(this.BlockStartAddress, this.ByteSize);
    }

    internal bool WriteParameterToConnectedDevice()
    {
      return this.MyMeter.MyDeviceMemory.WriteDataToConnectedDevice(this.BlockStartAddress, this.ByteSize);
    }

    internal bool ReadParameterFromConnectedDevice()
    {
      return this.MyMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.BlockStartAddress, this.ByteSize);
    }

    internal string GetDisplayString()
    {
      if (this.Statics.S3_VarType == S3_VariableTypes.TDC_Matrix || this.Statics.S3_VarType == S3_VariableTypes.ByteArray)
        return "";
      ulong ulongValue = this.GetUlongValue();
      return this.Statics.S3_VarType == S3_VariableTypes.MeterTime1980 ? ZR_Calendar.Cal_GetDateTime((uint) ulongValue).ToString("dd.MM.yyyy HH:mm:ss") : this.GetDisplayStringFromUlongValue(ulongValue);
    }

    internal string GetDisplayStringFromUlongValue(ulong UlongValue)
    {
      byte[] bytes = BitConverter.GetBytes(UlongValue);
      switch (this.Statics.S3_VarType)
      {
        case S3_VariableTypes.INT8:
          return ((sbyte) bytes[0]).ToString();
        case S3_VariableTypes.UINT8:
        case S3_VariableTypes.UINT16:
        case S3_VariableTypes.UINT32:
        case S3_VariableTypes.UINT64:
          return UlongValue.ToString();
        case S3_VariableTypes.INT16:
          return BitConverter.ToInt16(bytes, 0).ToString();
        case S3_VariableTypes.INT32:
          return BitConverter.ToInt32(bytes, 0).ToString();
        case S3_VariableTypes.REAL32:
          return BitConverter.ToSingle(bytes, 0).ToString();
        case S3_VariableTypes.INT64:
          return BitConverter.ToInt64(bytes, 0).ToString();
        case S3_VariableTypes.REAL64:
          return BitConverter.ToDouble(bytes, 0).ToString();
        case S3_VariableTypes.MeterTime1980:
          return ZR_Calendar.Cal_GetDateTime((uint) UlongValue).ToString("dd.MM.yyyy HH:mm:ss");
        default:
          int num = (int) GMM_MessageBox.ShowMessage(nameof (S3_Parameter), "Unsupported parameter type");
          return "0";
      }
    }

    internal bool SetFromDisplayString(string DisplayString)
    {
      ulong TheValue;
      return this.GetUlongValueFromDisplayString(DisplayString, out TheValue) && this.SetUlongValue(TheValue);
    }

    internal bool GetUlongValueFromDisplayString(string DisplayString, out ulong TheValue)
    {
      TheValue = 0UL;
      try
      {
        switch (this.Statics.S3_VarType)
        {
          case S3_VariableTypes.INT8:
          case S3_VariableTypes.INT16:
            byte[] bytes1 = BitConverter.GetBytes((long) short.Parse(DisplayString));
            TheValue = BitConverter.ToUInt64(bytes1, 0);
            break;
          case S3_VariableTypes.UINT8:
          case S3_VariableTypes.UINT16:
          case S3_VariableTypes.UINT32:
          case S3_VariableTypes.UINT64:
            TheValue = ulong.Parse(DisplayString);
            break;
          case S3_VariableTypes.INT32:
            byte[] bytes2 = BitConverter.GetBytes(int.Parse(DisplayString));
            TheValue = (ulong) BitConverter.ToUInt32(bytes2, 0);
            break;
          case S3_VariableTypes.REAL32:
            byte[] bytes3 = BitConverter.GetBytes(float.Parse(DisplayString));
            TheValue = (ulong) BitConverter.ToUInt32(bytes3, 0);
            break;
          case S3_VariableTypes.INT64:
            byte[] bytes4 = BitConverter.GetBytes(long.Parse(DisplayString));
            TheValue = BitConverter.ToUInt64(bytes4, 0);
            break;
          case S3_VariableTypes.REAL64:
            byte[] bytes5 = BitConverter.GetBytes(double.Parse(DisplayString));
            TheValue = BitConverter.ToUInt64(bytes5, 0);
            break;
          case S3_VariableTypes.MeterTime1980:
            DateTime TheTime = DateTime.Parse(DisplayString);
            TheValue = (ulong) ZR_Calendar.Cal_GetMeterTime(TheTime);
            break;
          default:
            int num = (int) GMM_MessageBox.ShowMessage(nameof (S3_Parameter), "Unsupported parameter type");
            break;
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    public string GetParameterInfo() => this.GetParameterInfo(out bool _);

    public string GetParameterInfo(out bool isParameterOk)
    {
      isParameterOk = true;
      int totalWidth = 12;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.GetTranslatedParameterName());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(this.GetTranslatedParameterDescription());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Name: ".PadRight(totalWidth) + this.Name);
      stringBuilder.AppendLine("Type: ".PadRight(totalWidth) + this.Statics.S3_VarType.ToString());
      stringBuilder.AppendLine("Address: ".PadRight(totalWidth) + "0x" + this.BlockStartAddress.ToString("X4"));
      if (this.Statics != null)
      {
        if (this.Statics.IsResource != null)
          stringBuilder.AppendLine("Is resource: " + this.Statics.IsResource);
        if (this.Statics.NeedResource != null)
          stringBuilder.AppendLine("Need resource: " + this.Statics.NeedResource);
        if (this.Statics.DefaultDifVif == null || this.Statics.DefaultDifVif.Length == 0)
        {
          isParameterOk = false;
          stringBuilder.AppendLine("Database Dif/Vif: ".PadRight(totalWidth) + "!!! ERROR !!! Can not be empty! Please, go to the Database tool and fix it.");
        }
        else
          stringBuilder.AppendLine("Database Dif/Vif: ".PadRight(totalWidth) + "0x" + Util.ByteArrayToHexString(this.Statics.DefaultDifVif));
        if (this.Statics.DefaultDifVif != null && this.Statics.DefaultDifVif.Length != 0)
        {
          string zrMbusParameterId1 = MBusDevice.GetZR_MBusParameterID(this.Statics.DefaultDifVif);
          if (string.IsNullOrEmpty(zrMbusParameterId1))
          {
            isParameterOk = false;
            stringBuilder.AppendLine("Database ZDF: ".PadRight(totalWidth) + "!!! ERROR !!! Unknown DIF/VIF detected");
          }
          else
            stringBuilder.AppendLine("Database ZDF: ".PadRight(totalWidth) + zrMbusParameterId1);
          byte[] newDifVif;
          if (this.MyMeter.MyMeterScaling.MBusDifVifAdjustUnit(this.Statics.DefaultDifVif, 0, this.Name, out newDifVif, out string _))
          {
            if (newDifVif != null)
            {
              stringBuilder.AppendLine("After scaling Dif/Vif: ".PadRight(totalWidth) + "0x" + Util.ByteArrayToHexString(newDifVif));
              string zrMbusParameterId2 = MBusDevice.GetZR_MBusParameterID(newDifVif);
              if (string.IsNullOrEmpty(zrMbusParameterId2))
              {
                isParameterOk = false;
                stringBuilder.AppendLine("After scaling ZDF: ".PadRight(totalWidth) + "!!! ERROR !!! Unknown DIF/VIF detected");
              }
              else
                stringBuilder.AppendLine("After scaling ZDF: ".PadRight(totalWidth) + zrMbusParameterId2);
            }
          }
          else
          {
            stringBuilder.AppendLine("After scaling ZDF: ".PadRight(totalWidth) + "!!! ERROR !!! Can not scaling this DIF/VIF");
            ZR_ClassLibMessages.ClearErrors();
          }
          long valueIdent = TranslationRulesManager.GetValueIdent("ZRI", "HEAT", 136, zrMbusParameterId1);
          if (valueIdent == -1L)
          {
            isParameterOk = false;
            stringBuilder.AppendLine("ValueIdent: ".PadRight(totalWidth));
          }
          else
            stringBuilder.AppendLine("ValueIdent: ".PadRight(totalWidth) + ValueIdent.GetTranslatedValueNameForValueId(valueIdent, true));
        }
      }
      return stringBuilder.ToString();
    }

    internal string GetUnit()
    {
      string unit = "";
      if (this.Statics.ParameterUnit > S3_ParameterUnits.None)
      {
        if (this.Statics.ParameterUnit == S3_ParameterUnits.Energy)
          unit = this.MyMeter.MyMeterScaling.energyResolutionInfo.resolutionString;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.Power)
          unit = MeterScaling.C5EnergyResolutions[this.MyMeter.MyMeterScaling.energyResolutionInfo.resolutionString].DifferentialUnit;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.PowerMax)
          unit = MeterScaling.C5EnergyResolutions[this.MyMeter.MyMeterScaling.energyResolutionInfo.resolutionString].DifferentialUnitMax;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.EnergyHighRes)
          unit = MeterScaling.C5EnergyResolutions[this.MyMeter.MyMeterScaling.energyResolutionInfo.resolutionString].HighResolution;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.Volume)
          unit = this.MyMeter.MyMeterScaling.volumeResolutionInfo.resolutionString;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.Flow)
          unit = MeterScaling.C5VolumeResolutions[this.MyMeter.MyMeterScaling.volumeResolutionInfo.resolutionString].DifferentialUnit;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.FlowMax)
          unit = MeterScaling.C5VolumeResolutions[this.MyMeter.MyMeterScaling.volumeResolutionInfo.resolutionString].DifferentialUnitMax;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.VolumeHighRes)
          unit = MeterScaling.C5VolumeResolutions[this.MyMeter.MyMeterScaling.volumeResolutionInfo.resolutionString].HighResolution;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.Input1)
          unit = this.MyMeter.MyMeterScaling.inpData[0].inputResolutionString;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.Input2)
          unit = this.MyMeter.MyMeterScaling.inpData[1].inputResolutionString;
        else if (this.Statics.ParameterUnit == S3_ParameterUnits.Input3)
          unit = this.MyMeter.MyMeterScaling.inpData[2].inputResolutionString;
      }
      return unit;
    }

    internal ResolutionData GetResolution()
    {
      string unit = this.GetUnit();
      return unit.Length == 0 ? (ResolutionData) null : MeterUnits.resolutionDataFromResolutionString[unit];
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Name + " = ");
      if (this.Statics.S3_VarType == S3_VariableTypes.MeterTime1980)
        stringBuilder.Append(ZR_Calendar.Cal_GetDateTime(this.GetUintValue()).ToString("dd.MM.yyyy HH:mm:ss"));
      else if (this.Name == S3_ParameterNames.Bak_HardwareAndRestrictions.ToString())
      {
        ushort ushortValue = this.GetUshortValue();
        stringBuilder.Append("0x" + ushortValue.ToString("x"));
        stringBuilder.Append(" -> WrPerm:");
        if (((uint) ushortValue & 4096U) > 0U)
          stringBuilder.Append("1");
        else
          stringBuilder.Append("0");
        stringBuilder.Append(";" + ParameterService.GetHardwareString((uint) ushortValue));
      }
      else if (this.Statics.S3_VarType == S3_VariableTypes.REAL32)
      {
        float single = BitConverter.ToSingle(this.MyMeter.MyDeviceMemory.MemoryBytes, this.BlockStartAddress);
        stringBuilder.Append(single.ToString());
      }
      else if (this.Statics.S3_VarType == S3_VariableTypes.REAL64)
      {
        double num = BitConverter.ToDouble(this.MyMeter.MyDeviceMemory.MemoryBytes, this.BlockStartAddress);
        stringBuilder.Append(num.ToString());
      }
      else
      {
        ulong ulongValue = this.GetUlongValue();
        stringBuilder.Append(ulongValue.ToString());
        stringBuilder.Append(" = 0x" + ulongValue.ToString("x"));
      }
      return stringBuilder.ToString();
    }

    internal void InsertData()
    {
      string str1 = "?";
      if (this.MyMeter.CloneFlowName != null)
        str1 = this.MyMeter.CloneFlowName;
      string str2 = str1 + "/" + this.Name;
      string str3 = "?";
      if (this.sourceMemoryBlock != null)
      {
        if (this.sourceMemoryBlock.MyMeter.CloneFlowName != null)
          str3 = this.sourceMemoryBlock.MyMeter.CloneFlowName;
        str3 = str3 + "/" + this.Name;
      }
      if (this.PreparedParameterValue != null)
      {
        if (this.PreparedParameterValue.Length != this.ByteSize)
          throw new Exception("Illegal prepared parameter value");
        if (S3_Parameter.S3_ParameterLogger.IsTraceEnabled)
          S3_Parameter.S3_ParameterLogger.Trace(str2 + " = from prepared value");
        for (int index = 0; index < this.PreparedParameterValue.Length; ++index)
          this.MyMeter.MyDeviceMemory.SetByteValue(this.BlockStartAddress + index, this.PreparedParameterValue[index]);
        this.PreparedParameterValue = (byte[]) null;
      }
      else if (this.sourceMemoryBlock == null)
        S3_Parameter.S3_ParameterLogger.Error(str2 + " = No source information available!");
      else if (this.sourceMemoryBlock.ByteSize != this.ByteSize)
      {
        S3_Parameter.S3_ParameterLogger.Error(str2 + " and " + str3 + " has different size");
      }
      else
      {
        if (S3_Parameter.S3_ParameterLogger.IsTraceEnabled)
          S3_Parameter.S3_ParameterLogger.Trace(str2 + " = " + str3);
        for (int index = 0; index < this.ByteSize; ++index)
          this.MyMeter.MyDeviceMemory.SetByteValue(this.BlockStartAddress + index, this.sourceMemoryBlock.MyMeter.MyDeviceMemory.GetByteValue(this.sourceMemoryBlock.BlockStartAddress + index));
      }
    }

    internal string GetTranslatedParameterName()
    {
      string str = "S3ParaName" + this.Name;
      return Ot.Gtt(Tg.Common, str, str);
    }

    internal string GetTranslatedParameterDescription()
    {
      string str = "S3ParaDesc" + this.Name;
      return Ot.Gtt(Tg.Common, str, str);
    }

    internal static string GetTranslatedParameterNameByName(string theName)
    {
      string str = "S3ParaName" + theName;
      return Ot.Gtt(Tg.Common, str, str);
    }

    internal static string GetTranslatedParameterDescription(string theName)
    {
      string str = "S3ParaDesc" + theName;
      return Ot.Gtt(Tg.Common, str, str);
    }
  }
}
