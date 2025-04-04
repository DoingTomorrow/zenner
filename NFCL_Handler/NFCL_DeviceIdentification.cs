// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.NFCL_DeviceIdentification
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using HandlerLib;
using MBusLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace NFCL_Handler
{
  internal class NFCL_DeviceIdentification : DeviceIdentification
  {
    private NFCL_DeviceMemory deviceMemory;

    internal NFCL_DeviceIdentification(
      NFCL_DeviceMemory deviceMemory,
      DeviceIdentification deviceIdentification)
      : base(deviceIdentification)
    {
      this.deviceMemory = deviceMemory;
    }

    internal NFCL_DeviceIdentification(NFCL_DeviceMemory deviceMemory)
      : base(new DeviceIdentification(deviceMemory.FirmwareVersion))
    {
      this.deviceMemory = deviceMemory;
    }

    public override uint? MeterID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.MeterId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(NFCL_Params.MeterId));
        uint? meterId = base.MeterID;
        if (meterId.HasValue)
        {
          meterId = base.MeterID;
          return new uint?(meterId.Value);
        }
        meterId = new uint?();
        return meterId;
      }
      set
      {
        this.deviceMemory.SetParameterValue<uint>(NFCL_Params.MeterId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? HardwareTypeID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.HardwareTypeId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(NFCL_Params.HardwareTypeId));
        uint? hardwareTypeId = base.HardwareTypeID;
        if (hardwareTypeId.HasValue)
        {
          hardwareTypeId = base.HardwareTypeID;
          return new uint?(hardwareTypeId.Value);
        }
        hardwareTypeId = new uint?();
        return hardwareTypeId;
      }
      set
      {
        this.deviceMemory.SetParameterValue<uint>(NFCL_Params.HardwareTypeId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? MeterInfoID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.MeterInfoId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(NFCL_Params.MeterInfoId));
        uint? meterInfoId = base.MeterInfoID;
        if (meterInfoId.HasValue)
        {
          meterInfoId = base.MeterInfoID;
          return new uint?(meterInfoId.Value);
        }
        meterInfoId = new uint?();
        return meterInfoId;
      }
      set
      {
        this.deviceMemory.SetParameterValue<uint>(NFCL_Params.MeterInfoId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? BaseTypeID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.BaseTypeId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(NFCL_Params.BaseTypeId));
        uint? baseTypeId = base.BaseTypeID;
        if (baseTypeId.HasValue)
        {
          baseTypeId = base.BaseTypeID;
          return new uint?(baseTypeId.Value);
        }
        baseTypeId = new uint?();
        return baseTypeId;
      }
      set
      {
        this.deviceMemory.SetParameterValue<uint>(NFCL_Params.BaseTypeId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? MeterTypeID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.MeterTypeId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(NFCL_Params.MeterTypeId));
        uint? meterTypeId = base.MeterTypeID;
        if (meterTypeId.HasValue)
        {
          meterTypeId = base.MeterTypeID;
          return new uint?(meterTypeId.Value);
        }
        meterTypeId = new uint?();
        return meterTypeId;
      }
      set
      {
        this.deviceMemory.SetParameterValue<uint>(NFCL_Params.MeterTypeId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? SAP_MaterialNumber
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.SAP_MaterialNumber))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(NFCL_Params.SAP_MaterialNumber));
        uint? sapMaterialNumber = base.SAP_MaterialNumber;
        if (sapMaterialNumber.HasValue)
        {
          sapMaterialNumber = base.SAP_MaterialNumber;
          return new uint?(sapMaterialNumber.Value);
        }
        sapMaterialNumber = new uint?();
        return sapMaterialNumber;
      }
      set
      {
        this.deviceMemory.SetParameterValue<uint>(NFCL_Params.SAP_MaterialNumber, value.Value);
        this.SetCRC();
      }
    }

    public override string SAP_ProductionOrderNumber
    {
      get => base.SAP_ProductionOrderNumber;
      set
      {
        this.sAP_ProductionOrderNumber = value;
        this.deviceMemory.SetParameterValue<uint>(NFCL_Params.SAP_ProductionOrderNumber, 0U);
        this.SetCRC();
      }
    }

    public override byte[] PrintedSerialNumber
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.Printed_serialnumber) && this.ChecksumOK() ? this.deviceMemory.GetData(NFCL_Params.Printed_serialnumber) : (byte[]) null;
      }
      set
      {
        this.deviceMemory.SetData(NFCL_Params.Printed_serialnumber, value);
        this.SetCRC();
      }
    }

    public override uint? ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.Mbus_ID))
        {
          uint parameterValue = this.deviceMemory.GetParameterValue<uint>(NFCL_Params.Mbus_ID);
          return parameterValue == uint.MaxValue ? new uint?() : new uint?(parameterValue);
        }
        uint? idBcd = base.ID_BCD;
        if (idBcd.HasValue)
          return base.ID_BCD;
        idBcd = new uint?();
        return idBcd;
      }
      set => this.deviceMemory.SetParameterValue<uint>(NFCL_Params.Mbus_ID, value.Value);
    }

    public override ushort? Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.Mbus_Manufacturer))
          return !this.ChecksumOK() ? new ushort?() : new ushort?(this.deviceMemory.GetParameterValue<ushort>(NFCL_Params.Mbus_Manufacturer));
        ushort? manufacturer = base.Manufacturer;
        if (manufacturer.HasValue)
          return base.Manufacturer;
        manufacturer = new ushort?();
        return manufacturer;
      }
      set
      {
        this.deviceMemory.SetParameterValue<ushort>(NFCL_Params.Mbus_Manufacturer, value.Value);
        this.SetCRC();
      }
    }

    public override byte? Medium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.Mbus_Medium))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(NFCL_Params.Mbus_Medium));
        byte? medium = base.Medium;
        if (medium.HasValue)
        {
          medium = base.Medium;
          return new byte?(medium.Value);
        }
        medium = new byte?();
        return medium;
      }
      set => this.deviceMemory.SetParameterValue<byte>(NFCL_Params.Mbus_Medium, value.Value);
    }

    public override byte? Generation
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.Mbus_Generation))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(NFCL_Params.Mbus_Generation));
        byte? generation = base.Generation;
        if (generation.HasValue)
        {
          generation = base.Generation;
          return new byte?(generation.Value);
        }
        generation = new byte?();
        return generation;
      }
      set => this.deviceMemory.SetParameterValue<byte>(NFCL_Params.Mbus_Generation, value.Value);
    }

    public override char? ObisMedium
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.Mbus_Obis_code) ? new char?((char) this.deviceMemory.GetParameterValue<byte>(NFCL_Params.Mbus_Obis_code)) : new char?();
      }
      set
      {
        this.deviceMemory.SetParameterValue<byte>(NFCL_Params.Mbus_Obis_code, (byte) value.Value);
      }
    }

    public override byte[] Unique_ID
    {
      get
      {
        return this.deviceMemory.AreDataAvailable(this.deviceMemory.ArmIdRange) ? DeviceIdentification.GetArmUniqueID(this.deviceMemory.GetData(this.deviceMemory.ArmIdRange)) : (byte[]) null;
      }
    }

    public override DateTime? BuildTime
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.BuildTime) ? MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(this.deviceMemory.GetData(NFCL_Params.BuildTime), 0) : new DateTime?();
      }
      set
      {
      }
    }

    public override string ToString() => this.ToString("f");

    private bool ChecksumOK() => true;

    private void SetCRC()
    {
      ushort? crc = this.CalculateCRC();
      if (!crc.HasValue)
        return;
      this.deviceMemory.SetParameterValue<ushort>(NFCL_Params.IdentificationChecksum, crc.Value);
    }

    private ushort? CalculateCRC()
    {
      if (!this.deviceMemory.IsParameterAvailable(NFCL_Params.MeterId) || !this.deviceMemory.IsParameterAvailable(NFCL_Params.HardwareTypeId) || !this.deviceMemory.IsParameterAvailable(NFCL_Params.MeterInfoId) || !this.deviceMemory.IsParameterAvailable(NFCL_Params.BaseTypeId) || !this.deviceMemory.IsParameterAvailable(NFCL_Params.MeterTypeId) || !this.deviceMemory.IsParameterAvailable(NFCL_Params.SAP_MaterialNumber) || !this.deviceMemory.IsParameterAvailable(NFCL_Params.SAP_ProductionOrderNumber))
        return new ushort?();
      List<byte> buffer = new List<byte>();
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(NFCL_Params.MeterId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(NFCL_Params.HardwareTypeId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(NFCL_Params.MeterInfoId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(NFCL_Params.BaseTypeId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(NFCL_Params.MeterTypeId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(NFCL_Params.SAP_MaterialNumber));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(NFCL_Params.SAP_ProductionOrderNumber));
      return new ushort?(Util.CalculatesCRC16_CC430(buffer));
    }

    public override ulong? LoRa_DevEUI
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.cfg_lora_deveui))
          return new ulong?(this.deviceMemory.GetParameterValue<ulong>(NFCL_Params.cfg_lora_deveui));
        ulong? loRaDevEui = base.LoRa_DevEUI;
        if (loRaDevEui.HasValue)
        {
          loRaDevEui = base.LoRa_DevEUI;
          return new ulong?(loRaDevEui.Value);
        }
        loRaDevEui = new ulong?();
        return loRaDevEui;
      }
      set
      {
        throw new Exception("The parameter DevEUI is read-only! You cannot change it on NDC device! Please change DevEUI on IUW device.");
      }
    }

    public override ulong? LoRa_JoinEUI
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.cfg_lora_appeui))
          return new ulong?(this.deviceMemory.GetParameterValue<ulong>(NFCL_Params.cfg_lora_appeui));
        return this.loRa_JoinEUI.HasValue ? new ulong?(this.loRa_JoinEUI.Value) : new ulong?();
      }
      set
      {
        throw new Exception("The parameter JoinEUI is read-only! You cannot change it on NDC device! Please change JoinEUI on IUW device.");
      }
    }

    public override byte[] LoRa_AppKey
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.cfg_lora_AppKey))
          return this.deviceMemory.GetData(NFCL_Params.cfg_lora_AppKey);
        return base.LoRa_AppKey != null ? base.LoRa_AppKey : (byte[]) null;
      }
      set
      {
        throw new Exception("The parameter AppKey is read-only! You cannot change it on NDC device! Please change AppKey on IUW device.");
      }
    }

    public override byte[] AES_Key
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(NFCL_Params.Mbus_aes_key) ? this.deviceMemory.GetData(NFCL_Params.Mbus_aes_key) : base.AES_Key;
      }
      set => this.deviceMemory.SetData(NFCL_Params.Mbus_aes_key, value);
    }
  }
}
