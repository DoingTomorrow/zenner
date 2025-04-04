// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.EDCL_DeviceIdentification_1
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using HandlerLib;
using System;

#nullable disable
namespace EDCL_Handler
{
  internal class EDCL_DeviceIdentification_1 : DeviceIdentification
  {
    private EDCL_DeviceMemory deviceMemory;

    internal EDCL_DeviceIdentification_1(
      EDCL_DeviceMemory deviceMemory,
      DeviceIdentification deviceIdentification)
      : base(deviceIdentification)
    {
      this.deviceMemory = deviceMemory;
    }

    internal EDCL_DeviceIdentification_1(EDCL_DeviceMemory deviceMemory)
      : base(new DeviceIdentification(deviceMemory.FirmwareVersion))
    {
      this.deviceMemory = deviceMemory;
    }

    public override string FullSerialNumber
    {
      get
      {
        string empty = string.Empty;
        try
        {
          return base.FullSerialNumber;
        }
        catch
        {
        }
        return string.Empty;
      }
      set
      {
        this.ObisMedium = value.Length == 14 ? new char?(value.Substring(0, 1)[0]) : throw new ArgumentException("Wrong length of the full serialnumber detected! Expected 14, Value: " + value);
        this.ManufacturerName = value.Substring(1, 3);
        this.GenerationAsString = value.Substring(4, 2);
        this.ID_BCD_AsString = value.Substring(6);
      }
    }

    public override uint? ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.Mbus_ID_meter))
        {
          uint parameterValue = this.deviceMemory.GetParameterValue<uint>(EDCL_Params.Mbus_ID_meter);
          return parameterValue == uint.MaxValue ? new uint?() : new uint?(parameterValue);
        }
        uint? idBcd = base.ID_BCD;
        if (idBcd.HasValue)
          return base.ID_BCD;
        idBcd = new uint?();
        return idBcd;
      }
      set => this.deviceMemory.SetParameterValue<uint>(EDCL_Params.Mbus_ID_meter, value.Value);
    }

    public override uint? FD_ID_BCD
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.FD_Mbus_ID_meter))
          return new uint?(this.deviceMemory.GetParameterValue<uint>(EDCL_Params.FD_Mbus_ID_meter));
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
        this.deviceMemory.SetParameterValue<uint>(EDCL_Params.FD_Mbus_ID_meter, value.Value);
      }
    }

    public override ushort? Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.Mbus_Manufacturer_meter))
          return new ushort?(this.deviceMemory.GetParameterValue<ushort>(EDCL_Params.Mbus_Manufacturer_meter));
        ushort? manufacturer = base.Manufacturer;
        if (manufacturer.HasValue)
          return base.Manufacturer;
        manufacturer = new ushort?();
        return manufacturer;
      }
      set
      {
        this.deviceMemory.SetParameterValue<ushort>(EDCL_Params.Mbus_Manufacturer_meter, value.Value);
      }
    }

    public override ushort? FD_Manufacturer
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.FD_Mbus_Manufacturer_meter))
          return new ushort?(this.deviceMemory.GetParameterValue<ushort>(EDCL_Params.FD_Mbus_Manufacturer_meter));
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
        this.deviceMemory.SetParameterValue<ushort>(EDCL_Params.FD_Mbus_Manufacturer_meter, value.Value);
      }
    }

    public override byte? Medium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.Mbus_Medium_meter))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(EDCL_Params.Mbus_Medium_meter));
        byte? medium = base.Medium;
        if (medium.HasValue)
        {
          medium = base.Medium;
          return new byte?(medium.Value);
        }
        medium = new byte?();
        return medium;
      }
      set => this.deviceMemory.SetParameterValue<byte>(EDCL_Params.Mbus_Medium_meter, value.Value);
    }

    public override byte? FD_Medium
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.FD_Mbus_Medium_meter))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(EDCL_Params.FD_Mbus_Medium_meter));
        byte? fdMedium = base.FD_Medium;
        if (fdMedium.HasValue)
          return base.FD_Medium;
        fdMedium = new byte?();
        return fdMedium;
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<byte>(EDCL_Params.FD_Mbus_Medium_meter, value.Value);
      }
    }

    public override byte? Generation
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.Mbus_Generation_meter))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(EDCL_Params.Mbus_Generation_meter));
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
        this.deviceMemory.SetParameterValue<byte>(EDCL_Params.Mbus_Generation_meter, value.Value);
      }
    }

    public override byte? FD_Generation
    {
      get
      {
        if (this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.FD_Mbus_Generation_meter))
          return new byte?(this.deviceMemory.GetParameterValue<byte>(EDCL_Params.FD_Mbus_Generation_meter));
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
        this.deviceMemory.SetParameterValue<byte>(EDCL_Params.FD_Mbus_Generation_meter, value.Value);
      }
    }

    public override char? ObisMedium
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.Mbus_Obis_code_meter) ? new char?((char) this.deviceMemory.GetParameterValue<byte>(EDCL_Params.Mbus_Obis_code_meter)) : new char?();
      }
      set
      {
        this.deviceMemory.SetParameterValue<byte>(EDCL_Params.Mbus_Obis_code_meter, (byte) value.Value);
      }
    }

    public override char? FD_ObisMedium
    {
      get
      {
        return this.deviceMemory != null && this.deviceMemory.IsParameterAvailable(EDCL_Params.FD_Obis_code_meter) ? new char?((char) this.deviceMemory.GetParameterValue<byte>(EDCL_Params.FD_Obis_code_meter)) : new char?();
      }
      set
      {
        if (!value.HasValue)
          return;
        this.deviceMemory.SetParameterValue<byte>(EDCL_Params.FD_Obis_code_meter, (byte) value.Value);
      }
    }

    public override string ToString() => this.ToString("f");
  }
}
