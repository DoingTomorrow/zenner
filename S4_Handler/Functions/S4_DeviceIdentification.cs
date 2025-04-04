// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_DeviceIdentification
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  public class S4_DeviceIdentification : DeviceIdentification
  {
    private static SortedList<uint, string> SupportedFirmwareVersions = new SortedList<uint, string>();
    private S4_DeviceMemory deviceMemory;
    internal S4_PCB_AssemblyDetection PCB_Assembly;
    protected byte? nfcIdentFrameVersion;
    protected byte? nfcProtocolVersion;
    protected uint? compilerVersion;
    protected byte? numberOfAvailableParameterGroups;
    protected ushort? numberOfAvailableParameters;
    protected byte? numberOfSelectedParameterGroups;
    protected ushort? numberOfSelectedParameters;
    protected ushort? maximumRecordLength;

    static S4_DeviceIdentification()
    {
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(16920582U, "First bulk meters");
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(17043462U, "First residential meters for USA");
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(17047558U, "Residential meters for USA, improved");
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(17108998U, "MID Bulk meters, including loggers and smart functions");
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(17113094U, "MID Bulk meters, improved");
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(17240070U, "MID and internal radio. Only used for residential radio.");
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(17244166U, "All meters, MID and internal radio");
      S4_DeviceIdentification.SupportedFirmwareVersions.Add(17272838U, "All meters, MID and internal radio, internal current");
    }

    private static string GetFirmwareInfo(uint firmware)
    {
      return S4_DeviceIdentification.SupportedFirmwareVersions.ContainsKey(firmware) ? S4_DeviceIdentification.SupportedFirmwareVersions[firmware] : "Not supervised firmware version";
    }

    internal S4_DeviceIdentification(
      S4_DeviceMemory deviceMemory,
      DeviceIdentification deviceIdentification)
      : this(deviceIdentification)
    {
      this.deviceMemory = deviceMemory;
      this.PCB_Assembly = new S4_PCB_AssemblyDetection(this.deviceMemory);
    }

    internal S4_DeviceIdentification(
      S4_DeviceMemory deviceMemory,
      int? hardwareTypeID,
      HardwareTypeSupport hardwareTypeSupport)
      : base(new DeviceIdentification(deviceMemory.FirmwareVersion))
    {
      this.deviceMemory = deviceMemory;
      if (hardwareTypeID.HasValue)
        this.hardwareID = new uint?((uint) (hardwareTypeSupport.GetHardwareAndFirmwareInfo(hardwareTypeID.Value) ?? throw new Exception("HardwareAndFirmwareInfo not found for HardwareTypeID = 0x" + hardwareTypeID.Value.ToString("x"))).HardwareVersion);
      this.PCB_Assembly = new S4_PCB_AssemblyDetection(this.deviceMemory);
    }

    internal S4_DeviceIdentification(DeviceIdentification deviceIdentification)
      : base(deviceIdentification)
    {
      switch (deviceIdentification)
      {
        case NfcDeviceIdentification _:
          NfcDeviceIdentification deviceIdentification1 = (NfcDeviceIdentification) deviceIdentification;
          this.nfcIdentFrameVersion = deviceIdentification1.NfcIdentFrameVersion;
          this.nfcProtocolVersion = deviceIdentification1.NfcProtocolVersion;
          this.compilerVersion = deviceIdentification1.CompilerVersion;
          this.numberOfAvailableParameterGroups = deviceIdentification1.NumberOfAvailableParameterGroups;
          this.numberOfAvailableParameters = deviceIdentification1.NumberOfAvailableParameters;
          this.numberOfSelectedParameterGroups = deviceIdentification1.NumberOfSelectedParameterGroups;
          this.numberOfSelectedParameters = deviceIdentification1.NumberOfSelectedParameters;
          this.maximumRecordLength = deviceIdentification1.MaximumRecordLength;
          break;
        case S4_DeviceIdentification _:
          S4_DeviceIdentification deviceIdentification2 = (S4_DeviceIdentification) deviceIdentification;
          this.nfcIdentFrameVersion = deviceIdentification2.NfcIdentFrameVersion;
          this.nfcProtocolVersion = deviceIdentification2.NfcProtocolVersion;
          this.compilerVersion = deviceIdentification2.CompilerVersion;
          this.numberOfAvailableParameterGroups = deviceIdentification2.NumberOfAvailableParameterGroups;
          this.numberOfAvailableParameters = deviceIdentification2.NumberOfAvailableParameters;
          this.numberOfSelectedParameterGroups = deviceIdentification2.NumberOfSelectedParameterGroups;
          this.numberOfSelectedParameters = deviceIdentification2.NumberOfSelectedParameters;
          this.maximumRecordLength = deviceIdentification2.MaximumRecordLength;
          break;
      }
    }

    public virtual byte? NfcIdentFrameVersion
    {
      get => this.nfcIdentFrameVersion;
      set => throw new Exception("Write to NfcIdentFrameVersion not defined");
    }

    public virtual byte? NfcProtocolVersion
    {
      get => this.nfcProtocolVersion;
      set => throw new Exception("Write to NfcProtocolVersion not defined");
    }

    public virtual uint? CompilerVersion
    {
      get => this.compilerVersion;
      set => throw new Exception("Write to CompilerVersion not defined");
    }

    public virtual byte? NumberOfAvailableParameterGroups
    {
      get => this.numberOfAvailableParameterGroups;
      set => throw new Exception("Write to NumberOfAvailableParameterGroups not defined");
    }

    public virtual ushort? NumberOfAvailableParameters
    {
      get => this.numberOfAvailableParameters;
      set => throw new Exception("Write to NumberOfAvailableParameters not defined");
    }

    public virtual byte? NumberOfSelectedParameterGroups
    {
      get => this.numberOfSelectedParameterGroups;
      set => throw new Exception("Write to NumberOfSelectedParameterGroups not defined");
    }

    public virtual ushort? NumberOfSelectedParameters
    {
      get => this.numberOfSelectedParameters;
      set => throw new Exception("Write to NumberOfSelectedParameters not defined");
    }

    public virtual ushort? MaximumRecordLength
    {
      get => this.maximumRecordLength;
      set => throw new Exception("Write to MaximumRecordLength not defined");
    }

    public override uint? SvnRevision
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.SvnRevision))
          return new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.SvnRevision));
        uint? svnRevision = base.SvnRevision;
        if (svnRevision.HasValue)
        {
          svnRevision = base.SvnRevision;
          return new uint?(svnRevision.Value);
        }
        svnRevision = new uint?();
        return svnRevision;
      }
      set => throw new Exception("Write to SvnRevision not defined");
    }

    public string GetHardwareIdText()
    {
      if (!this.HardwareID.HasValue)
        return "Not defined";
      ZENNER.CommonLibrary.FirmwareVersion firmwareVersion = new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion.Value);
      if (firmwareVersion <= (object) "0.3.7 IUW")
        return "Not supported";
      StringBuilder stringBuilder = new StringBuilder();
      uint num1 = this.HardwareID.Value;
      if ((num1 & 1U) > 0U)
        stringBuilder.Append("Ch2");
      else
        stringBuilder.Append("Ch1");
      if ((num1 & 2U) > 0U)
        stringBuilder.Append("; NFC");
      if ((num1 & 4U) > 0U)
        stringBuilder.Append("; LoRa");
      if ((num1 & 8U) > 0U)
        stringBuilder.Append("; Sen");
      if ((num1 & 16U) > 0U)
        stringBuilder.Append("; Osc1");
      if ((num1 & 32U) > 0U)
        stringBuilder.Append("; Osc2");
      if (firmwareVersion >= (object) "1.7.2 IUW")
      {
        uint num2 = (num1 & 3840U) >> 8;
        stringBuilder.Append("; Ass" + num2.ToString());
        if (this.PCB_Assembly != null && !double.IsNaN(this.PCB_Assembly.RangeDisplacementPercent))
          stringBuilder.Append("; Rd%:" + this.PCB_Assembly.RangeDisplacementPercent.ToString("0.00"));
      }
      return stringBuilder.ToString();
    }

    public bool? IsRadioDevice
    {
      get => !this.HardwareID.HasValue ? new bool?() : new bool?((this.HardwareID.Value & 4U) > 0U);
    }

    public bool? IsTwoTransducerChannels
    {
      get => !this.HardwareID.HasValue ? new bool?() : new bool?((this.HardwareID.Value & 1U) > 0U);
    }

    public int? AssemblyOption
    {
      get
      {
        return !this.HardwareID.HasValue ? new int?() : new int?(((int) this.HardwareID.Value & 3840) >> 8);
      }
    }

    public override byte[] PrintedSerialNumber
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.PrintedSerialNumber) ? this.deviceMemory.GetData(S4_Params.PrintedSerialNumber) : (byte[]) null;
      }
      set
      {
        if (value == null || value.Length > 20)
          throw new ArgumentException(nameof (PrintedSerialNumber));
        if (this.deviceMemory == null || !this.deviceMemory.IsParameterAvailable(S4_Params.PrintedSerialNumber))
          return;
        this.CheckCurrentWorkObject();
        this.deviceMemory.SetData(S4_Params.PrintedSerialNumber, value);
      }
    }

    public override ushort? ApprovalRevision
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.ApprovalRevision) ? new ushort?(this.deviceMemory.GetParameterValue<ushort>(S4_Params.ApprovalRevision)) : new ushort?();
      }
      set
      {
        if (this.deviceMemory == null || !this.deviceMemory.IsParameterAvailable(S4_Params.ApprovalRevision))
          return;
        this.CheckCurrentWorkObject();
        if (!value.HasValue)
          this.deviceMemory.SetParameterValue<int>(S4_Params.ApprovalRevision, 0);
        else
          this.deviceMemory.SetParameterValue<ushort>(S4_Params.ApprovalRevision, value.Value);
      }
    }

    public override uint? ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.SerialNumber))
          return new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.SerialNumber));
        uint? idBcd = base.ID_BCD;
        if (idBcd.HasValue)
        {
          idBcd = base.ID_BCD;
          return new uint?(idBcd.Value);
        }
        idBcd = new uint?();
        return idBcd;
      }
      set => this.deviceMemory.SetParameterValue<uint>(S4_Params.SerialNumber, value.Value);
    }

    public override uint? FD_ID_BCD
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_SerialNumber) ? new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.FD_SerialNumber)) : new uint?();
      }
      set
      {
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_SerialNumber))
          return;
        this.deviceMemory.SetParameterValue<uint>(S4_Params.FD_SerialNumber, value.Value);
      }
    }

    public override ushort? Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.MBus_ManuFacturer))
          return new ushort?(this.deviceMemory.GetParameterValue<ushort>(S4_Params.MBus_ManuFacturer));
        ushort? manufacturer = base.Manufacturer;
        if (manufacturer.HasValue)
        {
          manufacturer = base.Manufacturer;
          return new ushort?(manufacturer.Value);
        }
        manufacturer = new ushort?();
        return manufacturer;
      }
      set => this.deviceMemory.SetParameterValue<ushort>(S4_Params.MBus_ManuFacturer, value.Value);
    }

    public override ushort? FD_Manufacturer
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_MBus_ManuFacturer) ? new ushort?(this.deviceMemory.GetParameterValue<ushort>(S4_Params.FD_MBus_ManuFacturer)) : new ushort?();
      }
      set
      {
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_MBus_ManuFacturer))
          return;
        this.deviceMemory.SetParameterValue<ushort>(S4_Params.FD_MBus_ManuFacturer, value.Value);
      }
    }

    public override byte? Medium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.MBus_Medium))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(S4_Params.MBus_Medium));
        byte? medium = base.Medium;
        if (medium.HasValue)
        {
          medium = base.Medium;
          return new byte?(medium.Value);
        }
        medium = new byte?();
        return medium;
      }
      set => this.deviceMemory.SetParameterValue<byte>(S4_Params.MBus_Medium, value.Value);
    }

    public override byte? FD_Medium
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_MBus_Medium) ? new byte?(this.deviceMemory.GetParameterValue<byte>(S4_Params.FD_MBus_Medium)) : new byte?();
      }
      set
      {
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_MBus_Medium))
          return;
        this.deviceMemory.SetParameterValue<byte>(S4_Params.FD_MBus_Medium, value.Value);
      }
    }

    public override byte? Generation
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.Generation))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(S4_Params.Generation));
        byte? generation = base.Generation;
        if (generation.HasValue)
        {
          generation = base.Generation;
          return new byte?(generation.Value);
        }
        generation = new byte?();
        return generation;
      }
      set => this.deviceMemory.SetParameterValue<byte>(S4_Params.Generation, value.Value);
    }

    public override byte? FD_Generation
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_Generation) ? new byte?(this.deviceMemory.GetParameterValue<byte>(S4_Params.FD_Generation)) : new byte?();
      }
      set
      {
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_Generation))
          return;
        this.deviceMemory.SetParameterValue<byte>(S4_Params.FD_Generation, value.Value);
      }
    }

    public override byte[] Unique_ID
    {
      get
      {
        return this.deviceMemory == null || this.deviceMemory.ArmIdRange == null || !this.deviceMemory.AreDataAvailable(this.deviceMemory.ArmIdRange) ? (byte[]) null : DeviceIdentification.GetArmUniqueID(this.deviceMemory.GetData(this.deviceMemory.ArmIdRange));
      }
    }

    public override char? ObisMedium
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.Obis_Medium) ? new char?((char) this.deviceMemory.GetParameterValue<byte>(S4_Params.Obis_Medium)) : base.ObisMedium;
      }
      set
      {
        this.CheckCurrentWorkObject();
        this.deviceMemory.SetParameterValue<byte>(S4_Params.Obis_Medium, (byte) value.Value);
        if (!this.Medium.HasValue || this.Medium.Value != (byte) 0)
          return;
        this.Medium = new byte?((byte) DeviceIdentification.GetMBusMediumFromObisMedium(value.Value));
      }
    }

    public override char? FD_ObisMedium
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_Obis_Medium) ? new char?((char) this.deviceMemory.GetParameterValue<byte>(S4_Params.FD_Obis_Medium)) : new char?();
      }
      set
      {
        this.CheckCurrentWorkObject();
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_Obis_Medium))
          return;
        this.deviceMemory.SetParameterValue<byte>(S4_Params.FD_Obis_Medium, (byte) value.Value);
      }
    }

    public override byte[] AES_Key
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.WMBus_AesKey) ? this.deviceMemory.GetData(S4_Params.WMBus_AesKey) : base.AES_Key;
      }
      set
      {
        this.CheckCurrentWorkObject();
        if (this.deviceMemory.IsParameterInMap(S4_Params.FD_WMBus_AesKey))
          this.deviceMemory.SetData(S4_Params.WMBus_AesKey, value);
        else
          base.AES_Key = value;
      }
    }

    public override byte[] FD_AES_Key
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_WMBus_AesKey) ? this.deviceMemory.GetData(S4_Params.FD_WMBus_AesKey) : (byte[]) null;
      }
      set
      {
        this.CheckCurrentWorkObject();
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_WMBus_AesKey))
          return;
        this.deviceMemory.SetData(S4_Params.FD_WMBus_AesKey, value);
      }
    }

    public override ulong? LoRa_DevEUI
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.LoRa_DevEUI) ? new ulong?(this.deviceMemory.GetParameterValue<ulong>(S4_Params.LoRa_DevEUI)) : base.LoRa_DevEUI;
      }
      set
      {
        this.CheckCurrentWorkObject();
        this.deviceMemory.SetParameterValue<ulong>(S4_Params.LoRa_DevEUI, value.Value);
      }
    }

    public override ulong? FD_LoRa_DevEUI
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_LoRa_DevEUI) ? new ulong?(this.deviceMemory.GetParameterValue<ulong>(S4_Params.FD_LoRa_DevEUI)) : new ulong?();
      }
      set
      {
        this.CheckCurrentWorkObject();
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_LoRa_DevEUI))
          return;
        this.deviceMemory.SetParameterValue<ulong>(S4_Params.FD_LoRa_DevEUI, value.Value);
      }
    }

    public override ulong? LoRa_JoinEUI
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.LoRa_AppEUI) ? new ulong?(this.deviceMemory.GetParameterValue<ulong>(S4_Params.LoRa_AppEUI)) : base.LoRa_JoinEUI;
      }
      set
      {
        this.CheckCurrentWorkObject();
        this.deviceMemory.SetParameterValue<ulong>(S4_Params.LoRa_AppEUI, value.Value);
      }
    }

    public override ulong? FD_LoRa_JoinEUI
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_LoRa_AppEUI) ? new ulong?(this.deviceMemory.GetParameterValue<ulong>(S4_Params.FD_LoRa_AppEUI)) : new ulong?();
      }
      set
      {
        this.CheckCurrentWorkObject();
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_LoRa_AppEUI))
          return;
        this.deviceMemory.SetParameterValue<ulong>(S4_Params.FD_LoRa_AppEUI, value.Value);
      }
    }

    public override byte[] LoRa_AppKey
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.LoRa_AppKey) ? this.deviceMemory.GetData(S4_Params.LoRa_AppKey) : base.LoRa_AppKey;
      }
      set
      {
        this.CheckCurrentWorkObject();
        this.deviceMemory.SetData(S4_Params.LoRa_AppKey, value);
      }
    }

    public override byte[] FD_LoRa_AppKey
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.FD_LoRa_AppKey) ? this.deviceMemory.GetData(S4_Params.FD_LoRa_AppKey) : (byte[]) null;
      }
      set
      {
        this.CheckCurrentWorkObject();
        if (!this.deviceMemory.IsParameterInMap(S4_Params.FD_LoRa_AppKey))
          return;
        this.deviceMemory.SetData(S4_Params.FD_LoRa_AppKey, value);
      }
    }

    public override uint? MeterID
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.Meter_ID) ? new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.Meter_ID)) : base.MeterID;
      }
      set
      {
        this.CheckCurrentWorkObject();
        this.deviceMemory.SetParameterValue<uint>(S4_Params.Meter_ID, value.Value);
      }
    }

    public override uint? HardwareTypeID
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.HardwareType_ID) ? new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.HardwareType_ID)) : base.HardwareTypeID;
      }
      set
      {
        this.CheckCurrentWorkObject();
        this.deviceMemory.SetParameterValue<uint>(S4_Params.HardwareType_ID, value.Value);
      }
    }

    public override uint? MeterInfoID
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.MeterInfo_ID) ? new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.MeterInfo_ID)) : base.MeterInfoID;
      }
      set => this.deviceMemory.SetParameterValue<uint>(S4_Params.MeterInfo_ID, value.Value);
    }

    public override uint? MeterTypeID
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.MeterType_ID) ? new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.MeterType_ID)) : base.MeterTypeID;
      }
      set => this.deviceMemory.SetParameterValue<uint>(S4_Params.MeterType_ID, value.Value);
    }

    public override uint? BaseTypeID
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.BaseType_ID) ? new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.BaseType_ID)) : base.BaseTypeID;
      }
      set => this.deviceMemory.SetParameterValue<uint>(S4_Params.BaseType_ID, value.Value);
    }

    public override uint? SubPartNumber
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.SubPartNumber) ? new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.SubPartNumber)) : base.SubPartNumber;
      }
      set
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterInMap(S4_Params.SubPartNumber))
          this.deviceMemory.SetParameterValue<uint>(S4_Params.SubPartNumber, value.Value);
        base.SubPartNumber = value;
      }
    }

    public override uint? SAP_MaterialNumber
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.SAP_Number))
          return new uint?(this.deviceMemory.GetParameterValue<uint>(S4_Params.SAP_Number));
        uint? sapMaterialNumber = base.SAP_MaterialNumber;
        if (sapMaterialNumber.HasValue)
        {
          sapMaterialNumber = base.SAP_MaterialNumber;
          return new uint?(sapMaterialNumber.Value);
        }
        sapMaterialNumber = new uint?();
        return sapMaterialNumber;
      }
      set => this.deviceMemory.SetParameterValue<uint>(S4_Params.SAP_Number, value.Value);
    }

    public override string SAP_ProductionOrderNumber
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.ProductionOrderNumber) ? Utility.ZeroTerminatedAsciiStringToString(this.deviceMemory.GetData(S4_Params.ProductionOrderNumber)) : (string) null;
      }
      set
      {
        if (this.deviceMemory == null || !this.deviceMemory.IsParameterAvailable(S4_Params.ProductionOrderNumber))
          return;
        AddressRange parameterAddressRange = this.deviceMemory.GetParameterAddressRange(S4_Params.ProductionOrderNumber);
        this.deviceMemory.SetData(S4_Params.ProductionOrderNumber, Utility.StringToZeroTerminatedAsciiString(value, (int) parameterAddressRange.ByteSize - 1));
      }
    }

    public bool? IsMeterKeyDefined
    {
      get
      {
        return this.FirmwareVersion.HasValue && new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion.Value) >= (object) "1.4.11 IUW" && this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.sysState) ? new bool?(S4_SystemState.GetMeterKeyDefined(this.deviceMemory.GetParameterValue<uint>(S4_Params.sysState))) : new bool?();
      }
    }

    public bool IsProtected
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.Meter_IsProtected))
          return this.deviceMemory.GetParameterValue<byte>(S4_Params.Meter_IsProtected) > (byte) 0;
        byte? identFrameVersion = this.NfcIdentFrameVersion;
        uint? nullable;
        int num;
        if (identFrameVersion.HasValue)
        {
          nullable = this.DeviceStatusFlags;
          num = nullable.HasValue ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          identFrameVersion = this.NfcIdentFrameVersion;
          switch (identFrameVersion.Value)
          {
            case 0:
            case 1:
              nullable = this.FirmwareVersion;
              if (nullable.HasValue)
              {
                ZENNER.CommonLibrary.FirmwareVersion firmwareVersion;
                ref ZENNER.CommonLibrary.FirmwareVersion local = ref firmwareVersion;
                nullable = this.FirmwareVersion;
                int versionValue = (int) nullable.Value;
                local = new ZENNER.CommonLibrary.FirmwareVersion((uint) versionValue);
                if (firmwareVersion <= (object) "1.4.5 IUW")
                {
                  nullable = this.DeviceStatusFlags;
                  return (nullable.Value & 1024U) > 0U;
                }
                if (firmwareVersion >= (object) "1.4.11 IUW")
                {
                  nullable = this.DeviceStatusFlags;
                  return !new S4_FunctionalState((ushort) nullable.Value).NotProtected;
                }
              }
              break;
            case 2:
              return false;
            default:
              identFrameVersion = this.NfcIdentFrameVersion;
              throw new Exception("Not supported NfcIdentFrameVersion: " + identFrameVersion.Value.ToString());
          }
        }
        return false;
      }
    }

    public override void Set_FD_Values()
    {
      this.CheckCurrentWorkObject();
      base.Set_FD_Values();
    }

    private void CheckCurrentWorkObject()
    {
      if (this.deviceMemory.CloneCreated)
        throw new Exception("Access to obsolet DeviceIdentification object");
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Hardware: " + this.GetHardwareIdText());
      stringBuilder.Append(this.ToString("h", S4_DeviceIdentification.GetFirmwareInfo(this.FirmwareVersion.Value)));
      if (this.IsMeterKeyDefined.HasValue && !this.IsMeterKeyDefined.Value)
        stringBuilder.Append("Protection key not set; ");
      if (this.IsProtected)
        stringBuilder.Append("Protected!");
      else
        stringBuilder.Append("Not protected!");
      if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(S4_Params.Meter_ProtectionDate))
      {
        uint parameterValue = this.deviceMemory.GetParameterValue<uint>(S4_Params.Meter_ProtectionDate);
        if (parameterValue > 0U)
        {
          DateTime? fromFirmwareDateBcd = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(parameterValue);
          if (fromFirmwareDateBcd.HasValue)
            stringBuilder.Append(" Since " + fromFirmwareDateBcd.Value.ToShortDateString());
          else
            stringBuilder.Append(" activation time not available");
        }
      }
      if (this.deviceMemory == null)
        stringBuilder.Insert(0, "!!! Map not available !!!" + Environment.NewLine);
      return stringBuilder.ToString();
    }

    [Flags]
    internal enum HardwareIdBits
    {
      US_2_CHANNELS = 1,
      NFC = 2,
      LORA = 4,
      SENSUS = 8,
      OSC32_TDC1 = 16, // 0x00000010
      OSC32_TDC2 = 32, // 0x00000020
      AssemblyOptionBits = 3840, // 0x00000F00
    }
  }
}
