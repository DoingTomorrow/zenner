// Decompiled with JetBrains decompiler
// Type: EDC_Handler.DeviceIdentification
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EDC_Handler
{
  public sealed class DeviceIdentification
  {
    private const byte BLOCK_SIZE = 30;

    public uint MeterID { get; set; }

    public uint HardwareTypeID { get; set; }

    public uint MeterInfoID { get; set; }

    public uint BaseTypeID { get; set; }

    public uint MeterTypeID { get; set; }

    public uint SapMaterialNumber { get; set; }

    public uint SapProductionOrderNumber { get; set; }

    public ushort IdentificationChecksum { get; set; }

    public byte[] Buffer
    {
      get
      {
        List<byte> byteList = new List<byte>(30);
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.MeterID));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.HardwareTypeID));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.MeterInfoID));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.BaseTypeID));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.MeterTypeID));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.SapMaterialNumber));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.SapProductionOrderNumber));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.IdentificationChecksum));
        return byteList.ToArray();
      }
    }

    public bool IsChecksumOK => (int) this.CalculateChecksum() == (int) this.IdentificationChecksum;

    public ushort CalculateChecksum()
    {
      ushort checksum = 0;
      byte[] buffer = this.Buffer;
      for (int index = 0; index < buffer.Length - 2; ++index)
        checksum += (ushort) buffer[index];
      return checksum;
    }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("MeterID: ".PadRight(spaces)).AppendLine(this.MeterID.ToString());
      stringBuilder.Append("HardwareTypeID: ".PadRight(spaces)).AppendLine(this.HardwareTypeID.ToString());
      stringBuilder.Append("MeterInfoID: ".PadRight(spaces)).AppendLine(this.MeterInfoID.ToString());
      stringBuilder.Append("BaseTypeID: ".PadRight(spaces)).AppendLine(this.BaseTypeID.ToString());
      stringBuilder.Append("MeterTypeID: ".PadRight(spaces)).AppendLine(this.MeterTypeID.ToString());
      stringBuilder.Append("SAP_MaterialNumber: ".PadRight(spaces)).AppendLine(this.SapMaterialNumber.ToString());
      stringBuilder.Append("SAP_ProductionOrderNumber: ".PadRight(spaces)).AppendLine(this.SapProductionOrderNumber.ToString());
      stringBuilder.Append("IsChecksumOK: ".PadRight(spaces)).AppendLine(this.IsChecksumOK.ToString());
      return stringBuilder.ToString();
    }
  }
}
