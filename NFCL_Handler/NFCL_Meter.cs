// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.NFCL_Meter
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using GmmDbLib;
using HandlerLib;

#nullable disable
namespace NFCL_Handler
{
  internal class NFCL_Meter : IMeter
  {
    internal NFCL_DeviceIdentification deviceIdentification;
    internal NFCL_DeviceMemory meterMemory;

    internal BaseType BaseType { get; set; }

    public NFCL_Meter()
    {
    }

    public NFCL_Meter(uint firmwareVersion)
    {
      this.meterMemory = new NFCL_DeviceMemory(firmwareVersion);
    }

    public NFCL_Meter(DeviceIdentification deviceIdentification)
    {
      this.meterMemory = new NFCL_DeviceMemory(deviceIdentification.FirmwareVersion.Value);
      this.deviceIdentification = new NFCL_DeviceIdentification(this.meterMemory, deviceIdentification);
    }

    public NFCL_Meter(
      NFCL_DeviceMemory meterMemory,
      DeviceIdentification deviceIdentification,
      BaseType baseType)
    {
      this.meterMemory = meterMemory.Clone();
      this.deviceIdentification = new NFCL_DeviceIdentification(this.meterMemory, deviceIdentification);
      if (baseType == null)
        return;
      this.BaseType = baseType.DeepCopy();
    }

    internal IMeter CreateFromData(byte[] compresseddata)
    {
      this.meterMemory = new NFCL_DeviceMemory(compresseddata);
      this.deviceIdentification = new NFCL_DeviceIdentification(this.meterMemory);
      return (IMeter) this;
    }

    internal NFCL_Meter Clone()
    {
      return new NFCL_Meter(this.meterMemory, (DeviceIdentification) this.deviceIdentification, this.BaseType);
    }

    public string GetInfo()
    {
      return this.meterMemory != null ? this.meterMemory.GetParameterInfo() : string.Empty;
    }
  }
}
