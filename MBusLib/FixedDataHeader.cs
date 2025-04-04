// Decompiled with JetBrains decompiler
// Type: MBusLib.FixedDataHeader
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class FixedDataHeader : FrameHeader, IPrintable
  {
    public uint ID_BCD { get; set; }

    public ushort Manufacturer { get; set; }

    public byte Generation { get; set; }

    public byte Medium { get; set; }

    public byte ACC { get; set; }

    public byte Status { get; set; }

    public ushort Signature { get; set; }

    public uint ID => Util.ConvertBcdUInt32ToUInt32(this.ID_BCD);

    public new string ManufacturerString => MBusUtil.GetManufacturer(this.Manufacturer);

    public string MediumString => MBusUtil.GetMedium(this.Medium);

    public override string ToString() => this.Print(0);

    public string Print(int spaces = 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(' ', spaces).Append("ID: ").Append(this.ID.ToString());
      stringBuilder.Append(" MAN:").Append(this.ManufacturerString);
      stringBuilder.Append(" MED:").Append(this.MediumString);
      stringBuilder.Append(" GEN:").Append(this.Generation.ToString());
      stringBuilder.Append(" ACC:").Append(this.ACC.ToString());
      stringBuilder.Append(" STA:").Append(this.Status.ToString());
      stringBuilder.Append(" SIG:").AppendLine(this.Signature.ToString());
      return stringBuilder.ToString();
    }

    public byte[] ToByteArray()
    {
      List<byte> byteList = new List<byte>(12);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.ID_BCD));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Manufacturer));
      byteList.Add(this.Generation);
      byteList.Add(this.Medium);
      byteList.Add(this.ACC);
      byteList.Add(this.Status);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Signature));
      return byteList.ToArray();
    }

    public static FixedDataHeader Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      return buffer.Length >= 12 ? new FixedDataHeader()
      {
        ID_BCD = BitConverter.ToUInt32(buffer, 0),
        Manufacturer = BitConverter.ToUInt16(buffer, 4),
        Generation = buffer[6],
        Medium = buffer[7],
        ACC = buffer[8],
        Status = buffer[9],
        Signature = BitConverter.ToUInt16(buffer, 10)
      } : throw new ArgumentException("Buffer has wrong size. Expected: 8 bytes, actual: " + buffer.Length.ToString() + " bytes.");
    }
  }
}
