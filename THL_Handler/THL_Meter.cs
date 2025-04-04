// Decompiled with JetBrains decompiler
// Type: THL_Handler.THL_Meter
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using GmmDbLib;
using HandlerLib;

#nullable disable
namespace THL_Handler
{
  internal class THL_Meter : IMeter
  {
    internal THL_DeviceIdentification deviceIdentification;
    internal THL_DeviceMemory meterMemory;

    internal BaseType BaseType { get; set; }

    public THL_Meter()
    {
    }

    public THL_Meter(uint firmwareVersion)
    {
      this.meterMemory = new THL_DeviceMemory(firmwareVersion);
    }

    public THL_Meter(DeviceIdentification deviceIdentification)
    {
      this.meterMemory = new THL_DeviceMemory(deviceIdentification.FirmwareVersion.Value);
      this.deviceIdentification = new THL_DeviceIdentification(this.meterMemory, deviceIdentification);
    }

    public THL_Meter(
      THL_DeviceMemory meterMemory,
      DeviceIdentification deviceIdentification,
      BaseType baseType)
    {
      this.meterMemory = meterMemory.Clone();
      this.deviceIdentification = new THL_DeviceIdentification(this.meterMemory, deviceIdentification);
      if (baseType == null)
        return;
      this.BaseType = baseType.DeepCopy();
    }

    internal IMeter CreateFromData(byte[] compresseddata)
    {
      this.meterMemory = new THL_DeviceMemory(compresseddata);
      this.deviceIdentification = new THL_DeviceIdentification(this.meterMemory);
      return (IMeter) this;
    }

    internal THL_Meter Clone()
    {
      return new THL_Meter(this.meterMemory, (DeviceIdentification) this.deviceIdentification, this.BaseType);
    }

    public string GetInfo()
    {
      return this.meterMemory != null ? this.meterMemory.GetParameterInfo() : string.Empty;
    }
  }
}
