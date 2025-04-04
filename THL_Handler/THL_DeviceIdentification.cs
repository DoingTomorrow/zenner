// Decompiled with JetBrains decompiler
// Type: THL_Handler.THL_DeviceIdentification
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using HandlerLib;
using MBusLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace THL_Handler
{
  internal class THL_DeviceIdentification : DeviceIdentification
  {
    private THL_DeviceMemory deviceMemory;

    internal THL_DeviceIdentification(
      THL_DeviceMemory deviceMemory,
      DeviceIdentification deviceIdentification)
      : base(deviceIdentification)
    {
      this.deviceMemory = deviceMemory;
    }

    internal THL_DeviceIdentification(THL_DeviceMemory deviceMemory)
      : base(new DeviceIdentification(deviceMemory.FirmwareVersion))
    {
      this.deviceMemory = deviceMemory;
    }

    public override uint? MeterID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.MeterId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(THL_Params.MeterId));
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
        this.deviceMemory.SetParameterValue<uint>(THL_Params.MeterId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? HardwareTypeID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.HardwareTypeId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(THL_Params.HardwareTypeId));
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
        this.deviceMemory.SetParameterValue<uint>(THL_Params.HardwareTypeId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? MeterInfoID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.MeterInfoId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(THL_Params.MeterInfoId));
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
        this.deviceMemory.SetParameterValue<uint>(THL_Params.MeterInfoId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? BaseTypeID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.BaseTypeId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(THL_Params.BaseTypeId));
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
        this.deviceMemory.SetParameterValue<uint>(THL_Params.BaseTypeId, value.Value);
        this.SetCRC();
      }
    }

    public override uint? MeterTypeID
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.MeterTypeId))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(THL_Params.MeterTypeId));
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
        this.deviceMemory.SetParameterValue<uint>(THL_Params.MeterTypeId, value.Value);
        this.SetCRC();
      }
    }

    public override byte[] AES_Key
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.Mbus_aes_key) ? this.deviceMemory.GetData(THL_Params.Mbus_aes_key) : base.AES_Key;
      }
      set => this.deviceMemory.SetData(THL_Params.Mbus_aes_key, value);
    }

    public override uint? SAP_MaterialNumber
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.SAP_MaterialNumber))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(THL_Params.SAP_MaterialNumber));
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
        this.deviceMemory.SetParameterValue<uint>(THL_Params.SAP_MaterialNumber, value.Value);
        this.SetCRC();
      }
    }

    public override string SAP_ProductionOrderNumber
    {
      get => base.SAP_ProductionOrderNumber;
      set
      {
        this.sAP_ProductionOrderNumber = value;
        this.deviceMemory.SetParameterValue<uint>(THL_Params.SAP_ProductionOrderNumber, 0U);
        this.SetCRC();
      }
    }

    public override byte[] PrintedSerialNumber
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.Printed_serialnumber) && this.ChecksumOK() ? this.deviceMemory.GetData(THL_Params.Printed_serialnumber) : (byte[]) null;
      }
      set
      {
        this.deviceMemory.SetData(THL_Params.Printed_serialnumber, value);
        this.SetCRC();
      }
    }

    public override uint? ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.cfg_Mbus_ID))
        {
          uint parameterValue = this.deviceMemory.GetParameterValue<uint>(THL_Params.cfg_Mbus_ID);
          return parameterValue == uint.MaxValue ? new uint?() : new uint?(parameterValue);
        }
        uint? idBcd = base.ID_BCD;
        if (idBcd.HasValue)
          return base.ID_BCD;
        idBcd = new uint?();
        return idBcd;
      }
      set => this.deviceMemory.SetParameterValue<uint>(THL_Params.cfg_Mbus_ID, value.Value);
    }

    public override uint? FD_ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.FD_Mbus_ID))
          return !this.ChecksumOK() ? new uint?() : new uint?(this.deviceMemory.GetParameterValue<uint>(THL_Params.FD_Mbus_ID));
        uint? fdIdBcd = base.FD_ID_BCD;
        if (fdIdBcd.HasValue)
          return base.FD_ID_BCD;
        fdIdBcd = new uint?();
        return fdIdBcd;
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<uint>(THL_Params.FD_Mbus_ID, value.Value);
        this.SetCRC();
      }
    }

    public override ushort? Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.Mbus_Manufacturer))
          return !this.ChecksumOK() ? new ushort?() : new ushort?(this.deviceMemory.GetParameterValue<ushort>(THL_Params.Mbus_Manufacturer));
        ushort? manufacturer = base.Manufacturer;
        if (manufacturer.HasValue)
          return base.Manufacturer;
        manufacturer = new ushort?();
        return manufacturer;
      }
      set
      {
        this.deviceMemory.SetParameterValue<ushort>(THL_Params.Mbus_Manufacturer, value.Value);
        this.SetCRC();
      }
    }

    public override ushort? FD_Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.FD_Mbus_Manufacturer))
          return !this.ChecksumOK() ? new ushort?() : new ushort?(this.deviceMemory.GetParameterValue<ushort>(THL_Params.FD_Mbus_Manufacturer));
        ushort? fdManufacturer = base.FD_Manufacturer;
        if (fdManufacturer.HasValue)
          return base.FD_Manufacturer;
        fdManufacturer = new ushort?();
        return fdManufacturer;
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<ushort>(THL_Params.FD_Mbus_Manufacturer, value.Value);
        this.SetCRC();
      }
    }

    public override byte? Medium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.Mbus_Medium))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(THL_Params.Mbus_Medium));
        byte? medium = base.Medium;
        if (medium.HasValue)
        {
          medium = base.Medium;
          return new byte?(medium.Value);
        }
        medium = new byte?();
        return medium;
      }
      set => this.deviceMemory.SetParameterValue<byte>(THL_Params.Mbus_Medium, value.Value);
    }

    public override byte? Generation
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.Mbus_Generation))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(THL_Params.Mbus_Generation));
        byte? generation = base.Generation;
        if (generation.HasValue)
        {
          generation = base.Generation;
          return new byte?(generation.Value);
        }
        generation = new byte?();
        return generation;
      }
      set => this.deviceMemory.SetParameterValue<byte>(THL_Params.Mbus_Generation, value.Value);
    }

    public override char? ObisMedium
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.Mbus_Obis_code) ? new char?((char) this.deviceMemory.GetParameterValue<byte>(THL_Params.Mbus_Obis_code)) : new char?();
      }
      set
      {
        this.deviceMemory.SetParameterValue<byte>(THL_Params.Mbus_Obis_code, (byte) value.Value);
      }
    }

    public override ulong? LoRa_DevEUI
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.cfg_lora_deveui))
          return new ulong?(this.deviceMemory.GetParameterValue<ulong>(THL_Params.cfg_lora_deveui));
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
        if (!this.deviceMemory.IsParameterAvailable(THL_Params.cfg_lora_deveui))
          return;
        this.deviceMemory.SetParameterValue<ulong>(THL_Params.cfg_lora_deveui, value.Value);
      }
    }

    public override ulong? LoRa_JoinEUI
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.cfg_lora_appeui))
          return new ulong?(this.deviceMemory.GetParameterValue<ulong>(THL_Params.cfg_lora_appeui));
        ulong? loRaJoinEui = base.LoRa_DevEUI;
        if (loRaJoinEui.HasValue)
        {
          loRaJoinEui = base.LoRa_DevEUI;
          return new ulong?(loRaJoinEui.Value);
        }
        loRaJoinEui = new ulong?();
        return loRaJoinEui;
      }
      set
      {
        if (!this.deviceMemory.IsParameterAvailable(THL_Params.cfg_lora_appeui))
          return;
        this.deviceMemory.SetParameterValue<ulong>(THL_Params.cfg_lora_appeui, value.Value);
      }
    }

    public override byte[] LoRa_AppKey
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.cfb_lora_AppKey))
          return this.deviceMemory.GetData(THL_Params.cfb_lora_AppKey);
        return base.LoRa_AppKey != null ? base.LoRa_AppKey : (byte[]) null;
      }
      set
      {
        if (!this.deviceMemory.IsParameterAvailable(THL_Params.cfb_lora_AppKey))
          return;
        this.deviceMemory.SetData(THL_Params.cfb_lora_AppKey, value);
      }
    }

    public override ulong? FD_LoRa_DevEUI
    {
      get
      {
        if (this.deviceMemory == null || !this.deviceMemory.IsParameterAvailable(THL_Params.FD_Lora_DevEUI))
          return new ulong?();
        return !this.ChecksumOK() ? new ulong?() : new ulong?(this.deviceMemory.GetParameterValue<ulong>(THL_Params.FD_Lora_DevEUI));
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<ulong>(THL_Params.FD_Lora_DevEUI, value.Value);
        this.SetCRC();
      }
    }

    public override ulong? FD_LoRa_JoinEUI
    {
      get
      {
        if (this.deviceMemory == null || !this.deviceMemory.IsParameterAvailable(THL_Params.FD_LoRa_AppEUI))
          return new ulong?();
        return !this.ChecksumOK() ? new ulong?() : new ulong?(this.deviceMemory.GetParameterValue<ulong>(THL_Params.FD_LoRa_AppEUI));
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<ulong>(THL_Params.FD_LoRa_AppEUI, value.Value);
        this.SetCRC();
      }
    }

    public override byte[] FD_LoRa_AppKey
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.FD_LoRa_AppKey))
          return this.deviceMemory.GetData(THL_Params.FD_LoRa_AppKey);
        return base.LoRa_AppKey != null ? base.LoRa_AppKey : (byte[]) null;
      }
      set
      {
        if (value == null)
          return;
        this.deviceMemory.SetData(THL_Params.FD_LoRa_AppKey, value);
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
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(THL_Params.BuildTime) ? MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(this.deviceMemory.GetData(THL_Params.BuildTime), 0) : new DateTime?();
      }
      set
      {
      }
    }

    public override string ToString() => this.ToString("f");

    private bool ChecksumOK()
    {
      ushort? crc = this.CalculateCRC();
      if (!crc.HasValue)
        return false;
      ushort parameterValue = this.deviceMemory.GetParameterValue<ushort>(THL_Params.IdentificationChecksum);
      ushort? nullable1 = crc;
      int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      int num = (int) parameterValue;
      return nullable2.GetValueOrDefault() == num & nullable2.HasValue;
    }

    private void SetCRC()
    {
      ushort? crc = this.CalculateCRC();
      if (!crc.HasValue)
        return;
      this.deviceMemory.SetParameterValue<ushort>(THL_Params.IdentificationChecksum, crc.Value);
    }

    private ushort? CalculateCRC()
    {
      if (!this.deviceMemory.IsParameterAvailable(THL_Params.MeterId) || !this.deviceMemory.IsParameterAvailable(THL_Params.HardwareTypeId) || !this.deviceMemory.IsParameterAvailable(THL_Params.MeterInfoId) || !this.deviceMemory.IsParameterAvailable(THL_Params.BaseTypeId) || !this.deviceMemory.IsParameterAvailable(THL_Params.MeterTypeId) || !this.deviceMemory.IsParameterAvailable(THL_Params.SAP_MaterialNumber) || !this.deviceMemory.IsParameterAvailable(THL_Params.SAP_ProductionOrderNumber))
        return new ushort?();
      List<byte> buffer = new List<byte>();
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(THL_Params.MeterId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(THL_Params.HardwareTypeId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(THL_Params.MeterInfoId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(THL_Params.BaseTypeId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(THL_Params.MeterTypeId));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(THL_Params.SAP_MaterialNumber));
      buffer.AddRange((IEnumerable<byte>) this.deviceMemory.GetData(THL_Params.SAP_ProductionOrderNumber));
      return new ushort?(Util.CalculatesCRC16_CC430(buffer));
    }
  }
}
