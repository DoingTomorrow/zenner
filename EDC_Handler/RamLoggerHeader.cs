// Decompiled with JetBrains decompiler
// Type: EDC_Handler.RamLoggerHeader
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  public sealed class RamLoggerHeader
  {
    public ushort Address { get; set; }

    public byte MaxLength { get; set; }

    public byte Length { get; set; }

    public byte FifoEnd { get; set; }

    public RamLoggerFifoType Flags { get; set; }

    public DateTime? LastDate { get; set; }

    public ushort CRC { get; set; }

    public override string ToString()
    {
      return string.Format("{0} ADDR:0x{1:X4}, MAX:{2}, LENGTH:{3}, FIFO_END:{4}, LAST_DATE:{5}, CRC:0x{6:X4}", (object) this.Flags, (object) this.Address, (object) this.MaxLength, (object) this.Length, (object) this.FifoEnd, (object) this.LastDate, (object) this.CRC);
    }

    internal byte[] ToByteArray()
    {
      byte num1 = byte.MaxValue;
      byte num2 = byte.MaxValue;
      byte num3 = byte.MaxValue;
      byte num4 = byte.MaxValue;
      byte num5 = byte.MaxValue;
      byte num6 = byte.MaxValue;
      if (this.LastDate.HasValue)
      {
        DateTime? lastDate = this.LastDate;
        num1 = (byte) (lastDate.Value.Year - 2000);
        lastDate = this.LastDate;
        num2 = (byte) lastDate.Value.Month;
        lastDate = this.LastDate;
        num3 = (byte) lastDate.Value.Day;
        lastDate = this.LastDate;
        DateTime dateTime = lastDate.Value;
        num4 = (byte) dateTime.Hour;
        lastDate = this.LastDate;
        dateTime = lastDate.Value;
        num5 = (byte) dateTime.Minute;
        lastDate = this.LastDate;
        dateTime = lastDate.Value;
        num6 = (byte) dateTime.Second;
      }
      return new byte[14]
      {
        (byte) this.Address,
        (byte) ((uint) this.Address >> 8),
        this.MaxLength,
        this.Length,
        this.FifoEnd,
        (byte) this.Flags,
        num1,
        num2,
        num3,
        num4,
        num5,
        num6,
        (byte) this.CRC,
        (byte) ((uint) this.CRC >> 8)
      };
    }

    internal static RamLoggerHeader Parse(byte[] buffer, ref int offset)
    {
      int startIndex = offset;
      offset += 14;
      return new RamLoggerHeader()
      {
        Address = BitConverter.ToUInt16(buffer, startIndex),
        MaxLength = buffer[startIndex + 2],
        Length = buffer[startIndex + 3],
        FifoEnd = buffer[startIndex + 4],
        Flags = (RamLoggerFifoType) buffer[startIndex + 5],
        LastDate = RamLoggerHeader.ParseDateTime(buffer, startIndex + 6),
        CRC = BitConverter.ToUInt16(buffer, startIndex + 12)
      };
    }

    private static DateTime? ParseDateTime(byte[] buffer, int offset)
    {
      byte num = buffer[offset];
      byte month = buffer[1 + offset];
      byte day = buffer[2 + offset];
      byte hour = buffer[3 + offset];
      byte minute = buffer[4 + offset];
      byte second = buffer[5 + offset];
      if (num == byte.MaxValue || month == byte.MaxValue || day == byte.MaxValue || hour == byte.MaxValue || minute == byte.MaxValue || second == byte.MaxValue)
        return new DateTime?();
      if (num > byte.MaxValue || month > (byte) 12 || day > (byte) 31 || minute > (byte) 59 || second > (byte) 59)
        return new DateTime?();
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num, (int) month, (int) day, (int) hour, (int) minute, (int) second));
      }
      catch
      {
        return new DateTime?();
      }
    }
  }
}
