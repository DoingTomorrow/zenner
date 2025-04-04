// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.EDCL_Meter
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using GmmDbLib;
using HandlerLib;

#nullable disable
namespace EDCL_Handler
{
  internal class EDCL_Meter : IMeter
  {
    internal EDCL_DeviceIdentification deviceIdentification;
    internal EDCL_DeviceIdentification_1 deviceIdentification1;
    internal EDCL_DeviceMemory meterMemory;

    internal BaseType BaseType { get; set; }

    public EDCL_Meter()
    {
    }

    public EDCL_Meter(uint firmwareVersion)
    {
      this.meterMemory = new EDCL_DeviceMemory(firmwareVersion);
    }

    public EDCL_Meter(DeviceIdentification deviceIdentification)
    {
      this.meterMemory = new EDCL_DeviceMemory(deviceIdentification.FirmwareVersion.Value);
      this.deviceIdentification = new EDCL_DeviceIdentification(this.meterMemory, deviceIdentification);
      this.deviceIdentification1 = new EDCL_DeviceIdentification_1(this.meterMemory);
    }

    public EDCL_Meter(
      EDCL_DeviceMemory meterMemory,
      DeviceIdentification deviceIdentification,
      DeviceIdentification deviceIdentification1,
      BaseType baseType)
    {
      this.meterMemory = meterMemory.Clone();
      this.deviceIdentification = new EDCL_DeviceIdentification(this.meterMemory, deviceIdentification);
      this.deviceIdentification1 = new EDCL_DeviceIdentification_1(this.meterMemory, deviceIdentification1);
      if (baseType == null)
        return;
      this.BaseType = baseType.DeepCopy();
    }

    internal IMeter CreateFromData(byte[] compresseddata)
    {
      this.meterMemory = new EDCL_DeviceMemory(compresseddata);
      this.deviceIdentification = new EDCL_DeviceIdentification(this.meterMemory);
      this.deviceIdentification1 = new EDCL_DeviceIdentification_1(this.meterMemory);
      return (IMeter) this;
    }

    internal EDCL_Meter Clone()
    {
      return new EDCL_Meter(this.meterMemory, (DeviceIdentification) this.deviceIdentification, (DeviceIdentification) this.deviceIdentification1, this.BaseType);
    }

    public string GetInfo()
    {
      return this.meterMemory != null ? this.meterMemory.GetParameterInfo() : string.Empty;
    }
  }
}
