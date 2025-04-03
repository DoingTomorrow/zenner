// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceIdentification
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  [Serializable]
  public class DeviceIdentification
  {
    protected uint? firmwareVersion;
    protected byte[] unique_ID;
    protected ushort? signatur;
    protected uint? svnRevision;
    protected DateTime? buildTime;
    protected uint? hardwareID;
    protected uint? deviceStatusFlags;
    protected uint? ndc_lib_version;
    protected byte[] printedSerialNumber;
    protected uint? iD_BCD;
    protected uint? fD_ID_BCD;
    protected ushort? manufacturer;
    protected ushort? fD_Manufacturer;
    protected byte? generation;
    protected byte? fD_Generation;
    protected byte? medium;
    protected byte? fD_Medium;
    protected byte[] aes_Key;
    protected byte[] fD_AES_Key;
    protected byte? primaryAddress;
    protected char? obisMedium;
    protected char? fD_ObisMedium;
    protected ulong? loRa_DevEUI;
    protected ulong? fD_FD_LoRa_DevEUI;
    protected ulong? loRa_JoinEUI;
    protected ulong? fD_LoRa_JoinEUI;
    protected byte[] loRa_AppKey;
    protected byte[] fD_LoRa_AppKey;
    protected uint? meterID;
    protected uint? hardwareTypeID;
    protected uint? meterInfoID;
    protected uint? meterTypeID;
    protected uint? baseTypeID;
    protected uint? subPartNumber;
    protected uint? sAP_MaterialNumber;
    protected string sAP_ProductionOrderNumber;

    public DeviceIdentification()
    {
    }

    public DeviceIdentification(uint firmwareVersion)
    {
      this.firmwareVersion = new uint?(firmwareVersion);
    }

    public DeviceIdentification(DeviceIdentification source)
    {
      this.firmwareVersion = source.FirmwareVersion;
      this.hardwareID = source.HardwareID;
      if (source.Unique_ID != null)
        this.unique_ID = (byte[]) source.Unique_ID.Clone();
      this.deviceStatusFlags = source.deviceStatusFlags;
      this.signatur = source.Signatur;
      this.svnRevision = source.SvnRevision;
      this.buildTime = source.BuildTime;
      this.iD_BCD = source.ID_BCD;
      this.manufacturer = source.Manufacturer;
      this.generation = source.Generation;
      this.medium = source.Medium;
      this.primaryAddress = source.PrimaryAddress;
      this.obisMedium = source.ObisMedium;
      this.meterID = source.MeterID;
      this.hardwareTypeID = source.HardwareTypeID;
      this.meterInfoID = source.MeterInfoID;
      this.meterTypeID = source.MeterTypeID;
      this.baseTypeID = source.BaseTypeID;
      this.sAP_MaterialNumber = source.SAP_MaterialNumber;
      this.sAP_ProductionOrderNumber = source.SAP_ProductionOrderNumber;
      this.loRa_DevEUI = source.LoRa_DevEUI;
      this.loRa_JoinEUI = source.LoRa_JoinEUI;
      if (source.LoRa_AppKey == null)
        return;
      this.loRa_AppKey = (byte[]) source.LoRa_AppKey.Clone();
    }

    public virtual DeviceIdentification Clone() => this.MemberwiseClone() as DeviceIdentification;

    public virtual uint? FirmwareVersion
    {
      get => this.firmwareVersion;
      set => throw new Exception("Write to FirmwareVersion not defined");
    }

    public string GetFirmwareVersionString()
    {
      return !this.FirmwareVersion.HasValue ? (string) null : new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion.Value).ToString();
    }

    public ZENNER.CommonLibrary.FirmwareVersion FirmwareVersionObj
    {
      get
      {
        return this.FirmwareVersion.HasValue ? new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion.Value) : throw new Exception("Firmware version not defined");
      }
    }

    public virtual byte[] Unique_ID => this.unique_ID;

    public string GetUnique_ID_String()
    {
      return this.Unique_ID == null ? (string) null : Util.ByteArrayToHexString(this.Unique_ID);
    }

    public virtual ushort? Signatur
    {
      get => this.signatur;
      set => throw new Exception("Write to Signatur not defined");
    }

    public string GetSignaturString()
    {
      if (!this.Signatur.HasValue)
        return (string) null;
      ushort? signatur = this.Signatur;
      string str1 = signatur.Value.ToString();
      signatur = this.Signatur;
      string str2 = signatur.Value.ToString("X04");
      return str1 + " = 0x" + str2;
    }

    public virtual uint? SvnRevision
    {
      get => this.svnRevision;
      set => throw new Exception("Write to SvnRevision not defined");
    }

    public string GetSvnRevisionString()
    {
      return !this.SvnRevision.HasValue ? (string) null : this.SvnRevision.ToString();
    }

    public virtual DateTime? BuildTime
    {
      get => this.buildTime;
      set => throw new Exception("Write to BuildTime not defined");
    }

    public string GetBuildTimeString()
    {
      if (!this.BuildTime.HasValue)
        return (string) null;
      DateTime? buildTime = this.BuildTime;
      string longDateString = buildTime.Value.ToLongDateString();
      buildTime = this.BuildTime;
      string longTimeString = buildTime.Value.ToLongTimeString();
      return longDateString + " " + longTimeString;
    }

    public virtual uint? HardwareID
    {
      get => this.hardwareID;
      set => throw new Exception("Write to HardwareID not defined");
    }

    public string GetHardwareIDString()
    {
      return !this.HardwareID.HasValue ? (string) null : this.HardwareID.Value.ToString("X08");
    }

    public virtual uint? DeviceStatusFlags
    {
      get => this.deviceStatusFlags;
      set => this.deviceStatusFlags = value;
    }

    public int[] SubChannels { get; set; }

    public virtual uint? NDC_Lib_Version
    {
      get => this.ndc_lib_version;
      set => this.ndc_lib_version = value;
    }

    public virtual byte[] PrintedSerialNumber
    {
      get => this.printedSerialNumber;
      set => throw new Exception("Write to PrintedSerialNumber not defined");
    }

    public string PrintedSerialNumberAsString
    {
      get => Utility.ZeroTerminatedAsciiStringToString(this.PrintedSerialNumber);
      set => this.PrintedSerialNumber = Utility.StringToZeroTerminatedAsciiString(value, 17);
    }

    public virtual uint? ID_BCD
    {
      get => this.iD_BCD;
      set => throw new Exception("Write to ID_BCD not defined");
    }

    public string ID_BCD_AsString
    {
      get => !this.ID_BCD.HasValue ? (string) null : this.ID_BCD.Value.ToString("X08");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.ID_BCD = new uint?();
        uint result;
        if (!uint.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new Exception("Illegal ID_BCD string");
        this.ID_BCD = new uint?(result);
      }
    }

    public virtual uint? FD_ID_BCD
    {
      get => this.fD_ID_BCD;
      set => this.fD_ID_BCD = new uint?();
    }

    public string FD_ID_BCD_AsString
    {
      get => !this.FD_ID_BCD.HasValue ? (string) null : this.FD_ID_BCD.Value.ToString("X08");
    }

    public virtual ushort? Manufacturer
    {
      get => this.manufacturer;
      set => throw new Exception("Write to Manufacturer not defined");
    }

    public string ManufacturerAsString
    {
      get => !this.Manufacturer.HasValue ? (string) null : this.Manufacturer.Value.ToString("X04");
      set
      {
        this.Manufacturer = value.Length == 4 ? new ushort?(ushort.Parse(value, NumberStyles.HexNumber)) : throw new Exception("Illegal ManufacturerString length");
      }
    }

    public string ManufacturerName
    {
      get
      {
        return !this.Manufacturer.HasValue ? (string) null : MBusUtil.GetManufacturer(this.Manufacturer.Value);
      }
      set
      {
        this.Manufacturer = value.Length == 3 ? new ushort?(MBusUtil.GetManufacturerCode(value)) : throw new Exception("Illegal ManufacturerName length");
      }
    }

    public virtual ushort? FD_Manufacturer
    {
      get => this.fD_Manufacturer;
      set => this.fD_Manufacturer = new ushort?();
    }

    public string FD_ManufacturerAsString
    {
      get
      {
        return !this.FD_Manufacturer.HasValue ? (string) null : this.FD_Manufacturer.Value.ToString("X04");
      }
      set
      {
        this.FD_Manufacturer = value.Length == 4 ? new ushort?(ushort.Parse(value, NumberStyles.HexNumber)) : throw new Exception("Illegal FD_ManufacturerString length");
      }
    }

    public string FD_ManufacturerName
    {
      get
      {
        return !this.FD_Manufacturer.HasValue ? (string) null : MBusUtil.GetManufacturer(this.FD_Manufacturer.Value);
      }
      set
      {
        this.FD_Manufacturer = value.Length == 3 ? new ushort?(MBusUtil.GetManufacturerCode(value)) : throw new Exception("Illegal FD_ManufacturerName length");
      }
    }

    public virtual byte? Generation
    {
      get => this.generation;
      set => throw new Exception("Write to Generation not defined");
    }

    public string GenerationAsString
    {
      get => !this.Generation.HasValue ? (string) null : this.Generation.Value.ToString("X02");
      set
      {
        if (string.IsNullOrEmpty(value))
          this.Generation = new byte?();
        byte result;
        if (!byte.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new Exception("Illegal Generation string");
        this.Generation = new byte?(result);
      }
    }

    public virtual byte? FD_Generation
    {
      get => this.fD_Generation;
      set => this.fD_Generation = new byte?();
    }

    public string FD_GenerationAsString
    {
      get
      {
        return !this.FD_Generation.HasValue ? (string) null : this.FD_Generation.Value.ToString("X02");
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          this.FD_Generation = new byte?();
        byte result;
        if (!byte.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new Exception("Illegal FD_Generation string");
        this.FD_Generation = new byte?(result);
      }
    }

    public virtual byte? Medium
    {
      get => this.medium;
      set => throw new Exception("Write to Medium not defined");
    }

    public string MediumAsString
    {
      get => this.Medium.HasValue ? this.Medium.Value.ToString("X02") : string.Empty;
      set => this.Medium = new byte?(byte.Parse(this.MediumAsString, NumberStyles.HexNumber));
    }

    public string GetMediumAsText()
    {
      return this.Medium.HasValue ? MBusUtil.GetMedium(this.Medium.Value) : string.Empty;
    }

    public virtual byte? FD_Medium
    {
      get => this.fD_Medium;
      set => this.fD_Medium = new byte?();
    }

    public string FD_MediumAsString
    {
      get => this.FD_Medium.HasValue ? this.FD_Medium.Value.ToString("X02") : string.Empty;
      set => this.FD_Medium = new byte?(byte.Parse(this.FD_MediumAsString, NumberStyles.HexNumber));
    }

    public string GetFD_MediumAsText()
    {
      return this.FD_Medium.HasValue ? MBusUtil.GetMedium(this.FD_Medium.Value) : string.Empty;
    }

    public virtual byte[] AES_Key
    {
      get => this.aes_Key;
      set => throw new Exception("Write of AES_Key not defined");
    }

    public string AES_Key_AsString
    {
      get => this.AES_Key == null ? (string) null : Util.ByteArrayToHexString(this.AES_Key);
      set
      {
        if (value == null)
          this.AES_Key = (byte[]) null;
        else
          this.AES_Key = Util.HexStringToByteArray(value);
      }
    }

    public virtual byte[] FD_AES_Key
    {
      get => this.fD_AES_Key;
      set => this.fD_AES_Key = (byte[]) null;
    }

    public string FD_AES_Key_AsString
    {
      get => this.FD_AES_Key == null ? (string) null : Util.ByteArrayToHexString(this.FD_AES_Key);
      set
      {
        if (value == null)
          this.FD_AES_Key = (byte[]) null;
        else
          this.FD_AES_Key = Util.HexStringToByteArray(value);
      }
    }

    public virtual byte? PrimaryAddress
    {
      get => this.primaryAddress;
      set => throw new Exception("Write to PrimaryAddress not defined");
    }

    public virtual char? ObisMedium
    {
      get => this.obisMedium.HasValue ? this.obisMedium : new char?();
      set => this.obisMedium = value;
    }

    public string GetObisMediumString()
    {
      return !this.ObisMedium.HasValue ? (string) null : this.ObisMedium.ToString();
    }

    public virtual char? FD_ObisMedium
    {
      get => this.fD_ObisMedium.HasValue ? this.fD_ObisMedium : new char?();
      set => this.fD_ObisMedium = value;
    }

    public string GetFD_ObisMediumString()
    {
      return !this.FD_ObisMedium.HasValue ? (string) null : this.FD_ObisMedium.ToString();
    }

    public virtual string IdentificationPrefix
    {
      get
      {
        try
        {
          char? obisMedium = this.ObisMedium;
          if (!obisMedium.HasValue || this.ManufacturerName == null || !this.Generation.HasValue)
            return (string) null;
          obisMedium = this.ObisMedium;
          return obisMedium.Value.ToString() + this.ManufacturerName + this.GenerationAsString;
        }
        catch
        {
        }
        return (string) null;
      }
      set
      {
        this.ObisMedium = value.Length == 6 ? new char?(value[0]) : throw new Exception("Illegal IdentificationPrefix length");
        this.ManufacturerName = value.Substring(1, 3);
        this.GenerationAsString = value.Substring(4, 2);
      }
    }

    public virtual string FD_IdentificationPrefix
    {
      get
      {
        return !this.FD_ObisMedium.HasValue || this.FD_ManufacturerName == null || !this.FD_Generation.HasValue ? (string) null : this.FD_ObisMedium.Value.ToString() + this.FD_ManufacturerName + this.FD_GenerationAsString;
      }
    }

    public virtual int? FabricationNumber
    {
      get
      {
        if (!this.ID_BCD.HasValue)
          return new int?();
        int result;
        return !int.TryParse(this.ID_BCD.Value.ToString("X08"), out result) ? new int?() : new int?(result);
      }
      set
      {
        int num1;
        if (value.HasValue)
        {
          int? nullable = value;
          int num2 = 0;
          if (!(nullable.GetValueOrDefault() < num2 & nullable.HasValue))
          {
            nullable = value;
            int num3 = 99999999;
            num1 = nullable.GetValueOrDefault() > num3 & nullable.HasValue ? 1 : 0;
            goto label_4;
          }
        }
        num1 = 1;
label_4:
        if (num1 != 0)
          throw new Exception("Illegal FabricationNumber");
        this.ID_BCD = new uint?(uint.Parse(value.ToString(), NumberStyles.HexNumber));
      }
    }

    public string FabricationNumberAsString
    {
      get
      {
        try
        {
          int? fabricationNumber = this.FabricationNumber;
          if (!fabricationNumber.HasValue)
            return (string) null;
          fabricationNumber = this.FabricationNumber;
          return fabricationNumber.Value.ToString("d08");
        }
        catch (Exception ex)
        {
          return "Err:" + ex.Message;
        }
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          this.FabricationNumber = new int?();
        else
          this.FabricationNumber = new int?(int.Parse(value));
      }
    }

    public virtual int? FD_FabricationNumber
    {
      get
      {
        if (!this.FD_ID_BCD.HasValue)
          return new int?();
        int result;
        return !int.TryParse(this.FD_ID_BCD.Value.ToString("X08"), out result) ? new int?() : new int?(result);
      }
    }

    public string FD_FabricationNumberAsString
    {
      get
      {
        return !this.FD_FabricationNumber.HasValue ? (string) null : this.FD_FabricationNumber.Value.ToString("d08");
      }
    }

    public virtual string FullSerialNumber
    {
      get
      {
        int? fabricationNumber;
        int num;
        if (this.IdentificationPrefix != null)
        {
          fabricationNumber = this.FabricationNumber;
          num = !fabricationNumber.HasValue ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0)
          return (string) null;
        string identificationPrefix = this.IdentificationPrefix;
        fabricationNumber = this.FabricationNumber;
        string str = fabricationNumber.Value.ToString("d08");
        return identificationPrefix + str;
      }
      set
      {
        if (value.Length != 14)
          throw new Exception("Illegal FullSerialNumber length");
        int result;
        this.IdentificationPrefix = int.TryParse(value.Substring(6), out result) ? value.Substring(0, 6) : throw new Exception("Illegal FullSerialNumber format in FabricationNumber");
        this.FabricationNumber = new int?(result);
      }
    }

    public virtual string FD_FullSerialNumber
    {
      get
      {
        try
        {
          return this.FD_IdentificationPrefix == null || !this.FD_FabricationNumber.HasValue ? (string) null : this.FD_IdentificationPrefix + this.FD_FabricationNumberAsString;
        }
        catch
        {
        }
        return (string) null;
      }
    }

    public virtual ulong? LoRa_DevEUI
    {
      get => this.loRa_DevEUI;
      set => throw new Exception("Write of LoRa_DevEUI not defined");
    }

    public string LoRa_DevEUI_AsString
    {
      get => !this.LoRa_DevEUI.HasValue ? (string) null : this.LoRa_DevEUI.Value.ToString("X016");
      set
      {
        ulong result;
        if (!ulong.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new NotSupportedException("Illegal number format");
        this.LoRa_DevEUI = new ulong?(result);
      }
    }

    public virtual ulong? FD_LoRa_DevEUI
    {
      get => this.fD_FD_LoRa_DevEUI;
      set => this.fD_FD_LoRa_DevEUI = new ulong?();
    }

    public string FD_LoRa_DevEUI_AsString
    {
      get
      {
        return !this.FD_LoRa_DevEUI.HasValue ? (string) null : this.FD_LoRa_DevEUI.Value.ToString("X016");
      }
      set
      {
        ulong result;
        if (!ulong.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new NotSupportedException("Illegal number format");
        this.FD_LoRa_DevEUI = new ulong?(result);
      }
    }

    public virtual ulong? LoRa_JoinEUI
    {
      get => this.loRa_JoinEUI;
      set => throw new Exception("Write of LoRa_JoinEUI not defined");
    }

    public string LoRa_JoinEUI_AsString
    {
      get => !this.LoRa_JoinEUI.HasValue ? (string) null : this.LoRa_JoinEUI.Value.ToString("X016");
      set
      {
        ulong result;
        if (!ulong.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new NotSupportedException("Illegal JoinEUI number format");
        this.LoRa_JoinEUI = new ulong?(result);
      }
    }

    public virtual ulong? FD_LoRa_JoinEUI
    {
      get => this.fD_LoRa_JoinEUI;
      set => this.fD_LoRa_JoinEUI = new ulong?();
    }

    public string FD_LoRa_JoinEUI_AsString
    {
      get
      {
        return !this.FD_LoRa_JoinEUI.HasValue ? (string) null : this.FD_LoRa_JoinEUI.Value.ToString("X016");
      }
      set
      {
        ulong result;
        if (!ulong.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new NotSupportedException("Illegal number format");
        this.FD_LoRa_JoinEUI = new ulong?(result);
      }
    }

    public virtual byte[] LoRa_AppKey
    {
      get => this.loRa_AppKey;
      set => throw new Exception("Write of LoRa_AppKey not defined");
    }

    public string LoRa_AppKey_AsString
    {
      get => this.LoRa_AppKey == null ? (string) null : Util.ByteArrayToHexString(this.LoRa_AppKey);
      set => this.LoRa_AppKey = Util.HexStringToByteArray(value);
    }

    public virtual byte[] FD_LoRa_AppKey
    {
      get => this.fD_LoRa_AppKey;
      set => this.fD_LoRa_AppKey = (byte[]) null;
    }

    public string FD_LoRa_AppKey_AsString
    {
      get
      {
        return this.FD_LoRa_AppKey == null ? (string) null : Util.ByteArrayToHexString(this.FD_LoRa_AppKey);
      }
      set => this.FD_LoRa_AppKey = Util.HexStringToByteArray(value);
    }

    public virtual uint? MeterID
    {
      get => this.meterID;
      set => throw new Exception("Write of MeterID not defined");
    }

    public string GetMeterID_String()
    {
      return !this.MeterID.HasValue ? (string) null : this.MeterID.ToString();
    }

    public virtual uint? HardwareTypeID
    {
      get => this.hardwareTypeID;
      set => throw new Exception("Write of HardwareTypeID not defined");
    }

    public string GetHardwareTypeID_String()
    {
      return !this.HardwareTypeID.HasValue ? (string) null : this.HardwareTypeID.ToString();
    }

    public virtual uint? MeterInfoID
    {
      get => this.meterInfoID;
      set => throw new Exception("Write of MeterInfoID not defined");
    }

    public string MeterInfoID_AsString
    {
      get => !this.MeterInfoID.HasValue ? (string) null : this.MeterInfoID.ToString();
      set
      {
        if (value == null)
          this.MeterInfoID = new uint?(0U);
        else if (string.IsNullOrEmpty(value.Trim()))
          this.MeterInfoID = new uint?(0U);
        else
          this.MeterInfoID = new uint?(uint.Parse(value));
      }
    }

    public virtual uint? MeterTypeID
    {
      get => this.meterTypeID;
      set => throw new Exception("Write of MeterTypeID not defined");
    }

    public string GetMeterTypeID_String()
    {
      return !this.MeterTypeID.HasValue ? (string) null : this.MeterTypeID.ToString();
    }

    public virtual uint? BaseTypeID
    {
      get => this.baseTypeID;
      set => throw new Exception("Write of BaseTypeID not defined");
    }

    public string GetBaseTypeID_String()
    {
      return !this.BaseTypeID.HasValue ? (string) null : this.BaseTypeID.ToString();
    }

    public virtual uint? SubPartNumber
    {
      get => this.subPartNumber;
      set => this.subPartNumber = value;
    }

    public string GetSubPartNumber_String()
    {
      return !this.SubPartNumber.HasValue ? (string) null : this.SubPartNumber.ToString();
    }

    public virtual uint? SAP_MaterialNumber
    {
      get => this.sAP_MaterialNumber;
      set => throw new Exception("Write of SAP_MaterialNumber not defined");
    }

    public string GetSAP_MaterialNumberString()
    {
      return !this.SAP_MaterialNumber.HasValue ? (string) null : this.SAP_MaterialNumber.ToString();
    }

    public virtual string SAP_ProductionOrderNumber
    {
      get => this.sAP_ProductionOrderNumber;
      set => throw new Exception("Write of SAP_ProductionOrderNumber not defined");
    }

    public virtual ushort? ApprovalRevision
    {
      get => new ushort?();
      set => throw new Exception("Write of ApprovalRevision not defined");
    }

    public string ApprovalRevisionAsString
    {
      get => !this.ApprovalRevision.HasValue ? "" : this.ApprovalRevision.Value.ToString();
      set => throw new Exception("Write of ApprovalRevisionAsString not defined");
    }

    public virtual void Set_FD_Values()
    {
      this.FD_ID_BCD = this.ID_BCD;
      this.FD_Generation = this.Generation;
      this.FD_Manufacturer = this.Manufacturer;
      this.FD_Medium = this.Medium;
      this.FD_ObisMedium = this.ObisMedium;
      this.FD_AES_Key = this.AES_Key;
      this.FD_LoRa_DevEUI = this.LoRa_DevEUI;
      this.FD_LoRa_JoinEUI = this.LoRa_JoinEUI;
      this.FD_LoRa_AppKey = this.LoRa_AppKey;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.FullSerialNumber != null)
        stringBuilder.AppendLine("SerialNumber: ...... " + this.FullSerialNumber);
      if (this.FirmwareVersion.HasValue)
        stringBuilder.AppendLine("Firmware version: " + this.GetFirmwareVersionString());
      if (this.HardwareID.HasValue)
        stringBuilder.AppendLine("HardwareID: 0x" + this.GetHardwareIDString());
      if (this.SvnRevision.HasValue)
        stringBuilder.AppendLine("SvnRevision: " + this.SvnRevision.Value.ToString());
      if (this.MeterID.HasValue)
        stringBuilder.AppendLine("MeterID: " + this.MeterID.Value.ToString());
      return stringBuilder.ToString();
    }

    public string ToString(string format, string firmwareInfo = null)
    {
      StringBuilder InfoString = new StringBuilder();
      switch (format)
      {
        case "f":
          if (this.IsAvailable("Firmware version: ", (object) this.FirmwareVersion, InfoString))
            InfoString.AppendLine(this.FirmwareVersionObj.ToString());
          uint? nullable1;
          if (this.IsAvailable("HardwareID: 0x", (object) this.HardwareID, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable1 = this.HardwareID;
            string str = nullable1.Value.ToString("X");
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("Unique_ID: ", (object) this.Unique_ID, InfoString))
            InfoString.AppendLine(Util.ByteArrayToHexString(this.Unique_ID));
          ushort? nullable2;
          if (this.IsAvailable("Signatur: ", (object) this.Signatur, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable2 = this.Signatur;
            string str = nullable2.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("SvnRevision: ", (object) this.SvnRevision, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable1 = this.SvnRevision;
            string str = nullable1.Value.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("BuildTime: ", (object) this.BuildTime, InfoString))
            InfoString.AppendLine(this.BuildTime.Value.ToString());
          if (this.IsAvailable("ID_BCD: 0x", (object) this.ID_BCD, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable1 = this.ID_BCD;
            string str = nullable1.Value.ToString("X08");
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("Manufacturer: 0x", (object) this.Manufacturer, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable2 = this.Manufacturer;
            string str = nullable2.Value.ToString("X04");
            stringBuilder.AppendLine(str);
          }
          byte? nullable3;
          if (this.IsAvailable("Generation: 0x", (object) this.Generation, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable3 = this.Generation;
            string str = nullable3.Value.ToString("X02");
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("Medium: 0x", (object) this.Medium, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable3 = this.Medium;
            string str = nullable3.Value.ToString("X02");
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("ObisMedium: ", (object) this.ObisMedium, InfoString))
            InfoString.AppendLine(this.ObisMedium.Value.ToString());
          try
          {
            if (this.IsAvailable("FullSerialNumber: ", (object) this.FullSerialNumber, InfoString))
              InfoString.AppendLine(this.FullSerialNumber);
          }
          catch (Exception ex)
          {
          }
          uint? nullable4;
          if (this.IsAvailable("MeterID: ", (object) this.MeterID, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable4 = this.MeterID;
            string str = nullable4.Value.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("HardwareTypeID: ", (object) this.HardwareTypeID, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable4 = this.HardwareTypeID;
            string str = nullable4.Value.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("MeterInfoID: ", (object) this.MeterInfoID, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable4 = this.MeterInfoID;
            string str = nullable4.Value.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("MeterTypeID: ", (object) this.MeterTypeID, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable4 = this.MeterTypeID;
            string str = nullable4.Value.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("BaseTypeID: ", (object) this.BaseTypeID, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable4 = this.BaseTypeID;
            string str = nullable4.Value.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("SAP_MaterialNumber: ", (object) this.SAP_MaterialNumber, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable4 = this.SAP_MaterialNumber;
            string str = nullable4.Value.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.IsAvailable("SAP_ProductionOrderNumber: ", (object) this.SAP_ProductionOrderNumber, InfoString))
          {
            InfoString.AppendLine(this.SAP_ProductionOrderNumber);
            break;
          }
          break;
        case "h":
          uint? nullable5;
          uint num1;
          if (this.FirmwareVersion.HasValue && this.HardwareID.HasValue && this.SvnRevision.HasValue)
          {
            StringBuilder stringBuilder = InfoString;
            string[] strArray = new string[6]
            {
              "Firmware version: ",
              this.FirmwareVersionObj.ToString(),
              " ; HardwareID: 0x",
              this.HardwareID.Value.ToString("X"),
              " ; SvnRevision: ",
              null
            };
            nullable5 = this.SvnRevision;
            num1 = nullable5.Value;
            strArray[5] = num1.ToString();
            string str = string.Concat(strArray);
            stringBuilder.AppendLine(str);
          }
          else
          {
            if (this.IsAvailable("Firmware version: ", (object) this.FirmwareVersion, InfoString))
              InfoString.AppendLine(this.FirmwareVersionObj.ToString());
            if (this.IsAvailable("HardwareID: 0x", (object) this.HardwareID, InfoString))
            {
              StringBuilder stringBuilder = InfoString;
              nullable5 = this.HardwareID;
              string str = nullable5.Value.ToString("X");
              stringBuilder.AppendLine(str);
            }
            if (this.IsAvailable("SvnRevision: ", (object) this.SvnRevision, InfoString))
            {
              StringBuilder stringBuilder = InfoString;
              nullable5 = this.SvnRevision;
              string str = nullable5.Value.ToString();
              stringBuilder.AppendLine(str);
            }
          }
          if (firmwareInfo != null)
            InfoString.AppendLine("FirmwareInfo: " + firmwareInfo);
          nullable5 = this.MeterID;
          int num2;
          if (nullable5.HasValue && this.Medium.HasValue && this.Manufacturer.HasValue && this.Generation.HasValue)
          {
            nullable5 = this.ID_BCD;
            num2 = nullable5.HasValue ? 1 : 0;
          }
          else
            num2 = 0;
          if (num2 != 0)
          {
            StringBuilder stringBuilder = InfoString;
            nullable5 = this.MeterID;
            string str = "MeterID: " + nullable5.ToString() + " ; SerialNumber: " + this.FullSerialNumber;
            stringBuilder.AppendLine(str);
          }
          else if (this.IsAvailable("MeterID: ", (object) this.MeterID, InfoString))
          {
            StringBuilder stringBuilder = InfoString;
            nullable5 = this.MeterID;
            num1 = nullable5.Value;
            string str = num1.ToString();
            stringBuilder.AppendLine(str);
          }
          if (this.Unique_ID != null)
          {
            InfoString.AppendLine("ARM Unique_ID: " + Util.ByteArrayToHexString(this.Unique_ID));
            break;
          }
          break;
        default:
          InfoString.AppendLine("Illegal output format: '" + format + "'");
          break;
      }
      return InfoString.ToString();
    }

    private bool IsAvailable(string name, object testObject, StringBuilder InfoString)
    {
      InfoString.Append(name);
      if (testObject != null)
        return true;
      InfoString.AppendLine(" = null");
      return false;
    }

    public string Print(int spaces = 0) => Utility.PrintAvailableObjectProperties((object) this);

    public void SetObisMediumFromMBusMedium()
    {
      if (!this.Medium.HasValue)
        throw new Exception("MBus medium not defined");
      this.ObisMedium = new char?(DeviceIdentification.GetObisMediumFromMBusMedium(this.medium.Value));
    }

    public void PrintId(StringBuilder printText)
    {
      printText.AppendLine("Hardware: .......... " + this.GetHardwareIDString());
      printText.AppendLine("FirmwareVersion: ... " + this.GetFirmwareVersionString());
      printText.AppendLine("SerialNumber: ...... " + this.FullSerialNumber);
      printText.Append("PrintedSerialNumber: ");
      if (this.PrintedSerialNumber != null)
        printText.AppendLine(this.PrintedSerialNumberAsString);
      else
        printText.AppendLine("null");
      if (this.LoRa_DevEUI.HasValue)
        printText.AppendLine("LoRa_DevEUI: ....... " + this.LoRa_DevEUI_AsString);
      if (this.LoRa_JoinEUI.HasValue)
        printText.AppendLine("JoinEUI: ........... " + this.LoRa_JoinEUI_AsString);
      printText.AppendLine("SAP_Number: ........ " + this.GetSAP_MaterialNumberString());
      printText.AppendLine("MeterId: ........... " + this.MeterID.ToString());
      printText.AppendLine("MeterInfoId: ....... " + this.MeterInfoID.ToString());
      printText.AppendLine("BaseTypeId: ........ " + this.BaseTypeID.ToString());
    }

    public static char GetObisMediumFromMBusMedium(byte MBusMedium)
    {
      switch ((MBusLib.Medium) MBusMedium)
      {
        case MBusLib.Medium.OTHER:
          return 'F';
        case MBusLib.Medium.OIL:
          return 'F';
        case MBusLib.Medium.ELECTRICITY:
          return '1';
        case MBusLib.Medium.GAS:
          return '7';
        case MBusLib.Medium.HEAT_OUTLET:
          return '6';
        case MBusLib.Medium.STEAM:
          return 'F';
        case MBusLib.Medium.HOT_WATER:
          return '9';
        case MBusLib.Medium.WATER:
          return '8';
        case MBusLib.Medium.HCA:
          return '4';
        case MBusLib.Medium.COMPRESSED_AIR:
          return 'F';
        case MBusLib.Medium.COOL_OUTLET:
          return '5';
        case MBusLib.Medium.COOL_INLET:
          return '5';
        case MBusLib.Medium.HEAT_INLET:
          return '6';
        case MBusLib.Medium.HEAT_AND_COOL:
          return '6';
        case MBusLib.Medium.BUS_SYSTEM:
          return 'F';
        case MBusLib.Medium.UNKNOWN:
          return 'F';
        case MBusLib.Medium.HOT_WATER_90:
          return '9';
        case MBusLib.Medium.COLD_WATER:
          return '8';
        case MBusLib.Medium.HOT_AND_COLD_WATER:
          return '8';
        case MBusLib.Medium.PRESSURE:
          return 'F';
        case MBusLib.Medium.AD_CONVERTER:
          return 'F';
        case MBusLib.Medium.RF_Adapter:
          return 'F';
        default:
          throw new Exception("MBus medium unknown");
      }
    }

    public static MBusLib.Medium GetMBusMediumFromObisMedium(char ObisMedium)
    {
      switch (ObisMedium)
      {
        case '8':
          return MBusLib.Medium.WATER;
        case '9':
          return MBusLib.Medium.HOT_WATER;
        default:
          return MBusLib.Medium.OTHER;
      }
    }

    public static byte[] GetArmUniqueID(byte[] uniqueIdRange)
    {
      if (uniqueIdRange.Length != 24)
        throw new ArgumentException("Illegal UniqueIdRange length");
      byte[] dst = new byte[12];
      Buffer.BlockCopy((Array) uniqueIdRange, 0, (Array) dst, 0, 8);
      Buffer.BlockCopy((Array) uniqueIdRange, 20, (Array) dst, 8, 4);
      return dst;
    }

    public static string GetMacAddress(FirmwareType firmwareType, int nextID)
    {
      switch (firmwareType)
      {
        case FirmwareType.IDU:
          byte[] source1 = new byte[6]
          {
            (byte) 4,
            (byte) 182,
            (byte) 72,
            (byte) 16,
            (byte) 0,
            (byte) 0
          };
          byte[] bytes1 = BitConverter.GetBytes(nextID);
          source1[5] = bytes1[0];
          source1[4] = bytes1[1];
          source1[3] |= bytes1[2];
          return string.Join(":", ((IEnumerable<byte>) source1).Select<byte, string>((Func<byte, string>) (b => b.ToString("X2"))));
        case FirmwareType.ODU:
          byte[] source2 = new byte[6]
          {
            (byte) 4,
            (byte) 182,
            (byte) 72,
            (byte) 32,
            (byte) 0,
            (byte) 0
          };
          byte[] bytes2 = BitConverter.GetBytes(nextID);
          source2[5] = bytes2[0];
          source2[4] = bytes2[1];
          source2[3] |= bytes2[2];
          return string.Join(":", ((IEnumerable<byte>) source2).Select<byte, string>((Func<byte, string>) (b => b.ToString("X2"))));
        default:
          throw new NotImplementedException(firmwareType.ToString());
      }
    }
  }
}
