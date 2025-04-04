// Decompiled with JetBrains decompiler
// Type: M8_Handler.M8_Meter
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

using GmmDbLib;
using HandlerLib;

#nullable disable
namespace M8_Handler
{
  internal class M8_Meter : IMeter
  {
    internal M8_DeviceIdentification deviceIdentification;
    internal M8_DeviceMemory meterMemory;

    internal BaseType BaseType { get; set; }

    public M8_Meter()
    {
    }

    public M8_Meter(uint firmwareVersion)
    {
      this.meterMemory = new M8_DeviceMemory(firmwareVersion);
    }

    public M8_Meter(DeviceIdentification deviceIdentification)
    {
      this.meterMemory = new M8_DeviceMemory(deviceIdentification.FirmwareVersion.Value);
      this.deviceIdentification = new M8_DeviceIdentification(this.meterMemory, deviceIdentification);
    }

    public M8_Meter(
      M8_DeviceMemory meterMemory,
      DeviceIdentification deviceIdentification,
      BaseType baseType)
    {
      this.meterMemory = meterMemory.Clone();
      this.deviceIdentification = new M8_DeviceIdentification(this.meterMemory, deviceIdentification);
      if (baseType == null)
        return;
      this.BaseType = baseType.DeepCopy();
    }

    internal IMeter CreateFromData(byte[] compresseddata)
    {
      this.meterMemory = new M8_DeviceMemory(compresseddata);
      this.deviceIdentification = new M8_DeviceIdentification(this.meterMemory);
      return (IMeter) this;
    }

    internal M8_Meter Clone()
    {
      return new M8_Meter(this.meterMemory, (DeviceIdentification) this.deviceIdentification, this.BaseType);
    }

    public string GetInfo()
    {
      return this.meterMemory != null ? this.meterMemory.GetParameterInfo() : string.Empty;
    }
  }
}
