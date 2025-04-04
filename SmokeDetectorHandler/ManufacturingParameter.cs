// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.ManufacturingParameter
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class ManufacturingParameter
  {
    public const int BUFFER_SIZE = 12;

    public byte[] Buffer { get; private set; }

    public uint MeterID { get; set; }

    public uint MeterInfoID { get; set; }

    public DateTime? PCBDate { get; set; }

    public ushort PCBNumberOfDate { get; set; }

    internal static ManufacturingParameter Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new NullReferenceException("Can not parse the manufacturing parameters of smoke detector! The buffer is null.");
      return buffer.Length == 12 ? new ManufacturingParameter()
      {
        Buffer = buffer,
        MeterID = BitConverter.ToUInt32(buffer, 0),
        MeterInfoID = BitConverter.ToUInt32(buffer, 4),
        PCBDate = Util.ConvertToDate_MBus_CP16_TypeG(buffer, 8),
        PCBNumberOfDate = BitConverter.ToUInt16(buffer, 10)
      } : throw new ArgumentException("Can not parse the manufacturing parameters of smoke detector! Unknown length of buffer. Length: " + buffer.Length.ToString() + " bytes.");
    }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("MeterID: ".PadRight(spaces, ' ')).AppendLine(this.MeterID.ToString());
      stringBuilder.Append("MeterInfoID: ".PadRight(spaces, ' ')).AppendLine(this.MeterInfoID.ToString());
      stringBuilder.Append("PCBDate: ".PadRight(spaces, ' ')).AppendLine(!this.PCBDate.HasValue ? "null" : this.PCBDate.Value.ToShortDateString());
      stringBuilder.Append("PCBNumberOfDate: ".PadRight(spaces, ' ')).AppendLine(this.PCBNumberOfDate.ToString());
      stringBuilder.Append("Buffer: ").AppendLine(Util.ByteArrayToHexString(this.Buffer));
      return stringBuilder.ToString();
    }

    internal ManufacturingParameter DeepCopy()
    {
      return new ManufacturingParameter()
      {
        Buffer = this.Buffer,
        MeterID = this.MeterID,
        MeterInfoID = this.MeterInfoID,
        PCBDate = this.PCBDate,
        PCBNumberOfDate = this.PCBNumberOfDate
      };
    }

    internal byte[] CreateWriteBuffer()
    {
      DateTime? pcbDate;
      int num1;
      if (this.PCBDate.HasValue)
      {
        pcbDate = this.PCBDate;
        num1 = pcbDate.Value.Year < 2000 ? 1 : 0;
      }
      else
        num1 = 0;
      if (num1 != 0)
        throw new ArgumentOutOfRangeException("The year of 'PCBDate' should be greater or equal as 2000");
      byte[] collection = new byte[2];
      pcbDate = this.PCBDate;
      if (pcbDate.HasValue)
      {
        pcbDate = this.PCBDate;
        DateTime dateTime = pcbDate.Value;
        collection[0] |= (byte) dateTime.Day;
        collection[1] |= (byte) dateTime.Month;
        int num2 = dateTime.Year - 2000;
        collection[0] |= (byte) (num2 << 5);
        collection[1] |= (byte) ((num2 & 120) << 1);
      }
      else
      {
        collection[0] = byte.MaxValue;
        collection[1] = byte.MaxValue;
      }
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.MeterID));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.MeterInfoID));
      byteList.AddRange((IEnumerable<byte>) collection);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.PCBNumberOfDate));
      return byteList.ToArray();
    }
  }
}
