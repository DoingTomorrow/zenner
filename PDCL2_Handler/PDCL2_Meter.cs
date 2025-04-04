// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.PDCL2_Meter
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using GmmDbLib;
using HandlerLib;

#nullable disable
namespace PDCL2_Handler
{
  internal class PDCL2_Meter : IMeter
  {
    internal PDCL2_DeviceIdentification deviceIdentification;
    internal PDCL2_DeviceIdentification_1 deviceIdentification1;
    internal PDCL2_DeviceIdentification_2 deviceIdentification2;
    internal PDCL2_DeviceMemory meterMemory;

    internal BaseType BaseType { get; set; }

    public PDCL2_Meter()
    {
    }

    public PDCL2_Meter(uint firmwareVersion)
    {
      this.meterMemory = new PDCL2_DeviceMemory(firmwareVersion);
    }

    public PDCL2_Meter(DeviceIdentification deviceIdentification)
    {
      this.meterMemory = new PDCL2_DeviceMemory(deviceIdentification.FirmwareVersion.Value);
      this.deviceIdentification = new PDCL2_DeviceIdentification(this.meterMemory, deviceIdentification);
      this.deviceIdentification1 = new PDCL2_DeviceIdentification_1(this.meterMemory);
      this.deviceIdentification2 = new PDCL2_DeviceIdentification_2(this.meterMemory);
    }

    public PDCL2_Meter(
      PDCL2_DeviceMemory meterMemory,
      DeviceIdentification deviceIdentification,
      DeviceIdentification deviceIdentification1,
      DeviceIdentification deviceIdentification2,
      BaseType baseType)
    {
      this.meterMemory = meterMemory.Clone();
      this.deviceIdentification = new PDCL2_DeviceIdentification(this.meterMemory, deviceIdentification);
      this.deviceIdentification1 = new PDCL2_DeviceIdentification_1(this.meterMemory, deviceIdentification1);
      this.deviceIdentification2 = new PDCL2_DeviceIdentification_2(this.meterMemory, deviceIdentification2);
      if (baseType == null)
        return;
      this.BaseType = baseType.DeepCopy();
    }

    internal IMeter CreateFromData(byte[] compresseddata)
    {
      this.meterMemory = new PDCL2_DeviceMemory(compresseddata);
      this.deviceIdentification = new PDCL2_DeviceIdentification(this.meterMemory);
      this.deviceIdentification1 = new PDCL2_DeviceIdentification_1(this.meterMemory);
      this.deviceIdentification2 = new PDCL2_DeviceIdentification_2(this.meterMemory);
      return (IMeter) this;
    }

    internal PDCL2_Meter Clone()
    {
      return new PDCL2_Meter(this.meterMemory, (DeviceIdentification) this.deviceIdentification, (DeviceIdentification) this.deviceIdentification1, (DeviceIdentification) this.deviceIdentification2, this.BaseType);
    }

    public string GetInfo()
    {
      return this.meterMemory != null ? this.meterMemory.GetParameterInfo() : string.Empty;
    }
  }
}
