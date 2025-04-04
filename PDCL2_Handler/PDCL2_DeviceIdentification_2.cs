// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.PDCL2_DeviceIdentification_2
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using HandlerLib;
using System;

#nullable disable
namespace PDCL2_Handler
{
  internal class PDCL2_DeviceIdentification_2 : DeviceIdentification
  {
    private PDCL2_DeviceMemory deviceMemory;

    internal PDCL2_DeviceIdentification_2(
      PDCL2_DeviceMemory deviceMemory,
      DeviceIdentification deviceIdentification)
      : base(deviceIdentification)
    {
      this.deviceMemory = deviceMemory;
    }

    internal PDCL2_DeviceIdentification_2(PDCL2_DeviceMemory deviceMemory)
      : base(new DeviceIdentification(deviceMemory.FirmwareVersion))
    {
      this.deviceMemory = deviceMemory;
    }

    public override string FullSerialNumber
    {
      get
      {
        if (string.IsNullOrEmpty(base.FullSerialNumber))
          return string.Empty;
        string empty = string.Empty;
        try
        {
          return base.FullSerialNumber.StartsWith("6YYY00") ? base.FullSerialNumber.Substring(6) : base.FullSerialNumber;
        }
        catch
        {
        }
        return string.Empty;
      }
      set
      {
        if (value.Length == 14)
        {
          this.ObisMedium = new char?(value.Substring(0, 1)[0]);
          this.ManufacturerName = value.Substring(1, 3);
          this.GenerationAsString = value.Substring(4, 2);
          this.ID_BCD_AsString = value.Substring(6);
        }
        else
        {
          if (value.Length != 8)
            throw new ArgumentException("Wrong serialnumber format for channel 2!");
          this.ObisMedium = new char?('6');
          this.ManufacturerName = "YYY";
          this.GenerationAsString = "00";
          this.ID_BCD_AsString = value;
        }
      }
    }

    public override uint? ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.Mbus_ID_meter_ChB))
        {
          uint parameterValue = this.deviceMemory.GetParameterValue<uint>(PDCL2_Params.Mbus_ID_meter_ChB);
          return parameterValue == uint.MaxValue ? new uint?() : new uint?(parameterValue);
        }
        uint? idBcd = base.ID_BCD;
        if (idBcd.HasValue)
          return base.ID_BCD;
        idBcd = new uint?();
        return idBcd;
      }
      set => this.deviceMemory.SetParameterValue<uint>(PDCL2_Params.Mbus_ID_meter_ChB, value.Value);
    }

    public override uint? FD_ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.FD_Mbus_ID_meter_ChB))
          return new uint?(this.deviceMemory.GetParameterValue<uint>(PDCL2_Params.FD_Mbus_ID_meter_ChB));
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
        this.deviceMemory.SetParameterValue<uint>(PDCL2_Params.FD_Mbus_ID_meter_ChB, value.Value);
      }
    }

    public override ushort? Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.Mbus_Manufacturer_meter_ChB))
          return new ushort?(this.deviceMemory.GetParameterValue<ushort>(PDCL2_Params.Mbus_Manufacturer_meter_ChB));
        ushort? manufacturer = base.Manufacturer;
        if (manufacturer.HasValue)
          return base.Manufacturer;
        manufacturer = new ushort?();
        return manufacturer;
      }
      set
      {
        this.deviceMemory.SetParameterValue<ushort>(PDCL2_Params.Mbus_Manufacturer_meter_ChB, value.Value);
      }
    }

    public override ushort? FD_Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.FD_Mbus_Manufacturer_meter_ChB))
          return new ushort?(this.deviceMemory.GetParameterValue<ushort>(PDCL2_Params.FD_Mbus_Manufacturer_meter_ChB));
        ushort? fdManufacturer = base.FD_Manufacturer;
        if (fdManufacturer.HasValue)
          return base.FD_Manufacturer;
        fdManufacturer = new ushort?();
        return fdManufacturer;
      }
      set
      {
        this.deviceMemory.SetParameterValue<ushort>(PDCL2_Params.FD_Mbus_Manufacturer_meter_ChB, value.Value);
      }
    }

    public override byte? Medium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.Mbus_Medium_meter_ChB))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(PDCL2_Params.Mbus_Medium_meter_ChB));
        byte? medium = base.Medium;
        if (medium.HasValue)
        {
          medium = base.Medium;
          return new byte?(medium.Value);
        }
        medium = new byte?();
        return medium;
      }
      set
      {
        this.deviceMemory.SetParameterValue<byte>(PDCL2_Params.Mbus_Medium_meter_ChB, value.Value);
      }
    }

    public override byte? FD_Medium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.FD_Mbus_Medium_meter_ChB))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(PDCL2_Params.FD_Mbus_Medium_meter_ChB));
        byte? fdMedium = base.FD_Medium;
        if (fdMedium.HasValue)
        {
          fdMedium = base.FD_Medium;
          return new byte?(fdMedium.Value);
        }
        fdMedium = new byte?();
        return fdMedium;
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<byte>(PDCL2_Params.FD_Mbus_Medium_meter_ChB, value.Value);
      }
    }

    public override byte? Generation
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.Mbus_Generation_meter_ChB))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(PDCL2_Params.Mbus_Generation_meter_ChB));
        byte? generation = base.Generation;
        if (generation.HasValue)
        {
          generation = base.Generation;
          return new byte?(generation.Value);
        }
        generation = new byte?();
        return generation;
      }
      set
      {
        this.deviceMemory.SetParameterValue<byte>(PDCL2_Params.Mbus_Generation_meter_ChB, value.Value);
      }
    }

    public override byte? FD_Generation
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.FD_Mbus_Generation_meter_ChB))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(PDCL2_Params.FD_Mbus_Generation_meter_ChB));
        byte? fdGeneration = base.FD_Generation;
        if (fdGeneration.HasValue)
        {
          fdGeneration = base.FD_Generation;
          return new byte?(fdGeneration.Value);
        }
        fdGeneration = new byte?();
        return fdGeneration;
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<byte>(PDCL2_Params.FD_Mbus_Generation_meter_ChB, value.Value);
      }
    }

    public override char? ObisMedium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.Mbus_Obis_code_meter_ChB))
          return new char?((char) this.deviceMemory.GetParameterValue<byte>(PDCL2_Params.Mbus_Obis_code_meter_ChB));
        char? obisMedium = base.ObisMedium;
        if (obisMedium.HasValue)
        {
          obisMedium = base.ObisMedium;
          return new char?(obisMedium.Value);
        }
        obisMedium = new char?();
        return obisMedium;
      }
      set
      {
        this.deviceMemory.SetParameterValue<byte>(PDCL2_Params.Mbus_Obis_code_meter_ChB, (byte) value.Value);
      }
    }

    public override char? FD_ObisMedium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(PDCL2_Params.FD_Obis_code_meter_ChB))
          return new char?((char) this.deviceMemory.GetParameterValue<byte>(PDCL2_Params.FD_Obis_code_meter_ChB));
        char? fdObisMedium = base.FD_ObisMedium;
        if (fdObisMedium.HasValue)
        {
          fdObisMedium = base.FD_ObisMedium;
          return new char?(fdObisMedium.Value);
        }
        fdObisMedium = new char?();
        return fdObisMedium;
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<byte>(PDCL2_Params.FD_Obis_code_meter_ChB, (byte) value.Value);
      }
    }

    public override string ToString() => this.ToString("f");
  }
}
