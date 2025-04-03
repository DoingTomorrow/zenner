// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DeviceDescriptor
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class S3_DeviceDescriptor
  {
    public S3_DeviceDescriptor.ChipDevices Chip { get; private set; }

    public uint Lot { get; private set; }

    internal S3_DeviceDescriptor(ByteField deviceDescriptorBytes)
    {
      ushort uint16 = BitConverter.ToUInt16(deviceDescriptorBytes.Data, 4);
      this.Lot = BitConverter.ToUInt32(deviceDescriptorBytes.Data, 10);
      switch (uint16)
      {
        case 32856:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6720;
          break;
        case 32857:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6721;
          break;
        case 32865:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6723;
          break;
        case 32866:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6730;
          break;
        case 32867:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6731;
          break;
        case 32869:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6733;
          break;
        case 33130:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6734;
          break;
        case 33131:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6735;
          break;
        case 33132:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6736;
          break;
        case 33133:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6724;
          break;
        case 33134:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6725;
          break;
        case 33135:
          this.Chip = S3_DeviceDescriptor.ChipDevices.F6726;
          break;
      }
    }

    public enum ChipDevices
    {
      F6736,
      F6735,
      F6734,
      F6733,
      F6731,
      F6730,
      F6726,
      F6725,
      F6724,
      F6723,
      F6721,
      F6720,
    }
  }
}
