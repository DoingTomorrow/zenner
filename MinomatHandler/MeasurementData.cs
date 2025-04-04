// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MeasurementData
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class MeasurementData : EventArgs
  {
    public MeasurementDataHeader Header { get; set; }

    public Dictionary<DateTime, Decimal?> Data { get; set; }

    public MeasurementData() => this.Data = new Dictionary<DateTime, Decimal?>();

    public static MeasurementData Parse(
      MeasurementDataHeader header,
      byte[] payload,
      ref int offset)
    {
      DateTime key = header.TimepointOfFirstValue;
      if (header.Type == MeasurementDataType.MonthAndHalfMonth)
        key = new DateTime(key.Year, key.Month, key.Day).AddMonths(1);
      else if (header.Type == MeasurementDataType.Day)
        key = new DateTime(key.Year, key.Month, key.Day).AddDays(1.0);
      MeasurementData measurementData = new MeasurementData();
      measurementData.Header = header;
      DeviceTypes typeOfMinolDevice = NumberRanges.GetTypeOfMinolDevice((long) header.ID);
      int num = offset + header.CountOfExpectedBytes;
      while (offset < num)
      {
        Decimal? nullable1 = MeasurementData.GetValue(typeOfMinolDevice, payload, offset, header.SizeOfValue);
        if (nullable1.HasValue)
          measurementData.Data.Add(key, nullable1);
        if (header.Type == MeasurementDataType.Day)
          key = key.AddDays(1.0);
        else if (header.Type == MeasurementDataType.DueDate)
          key = key.AddYears(1);
        else if (header.Type == MeasurementDataType.MonthAndHalfMonth)
        {
          offset += header.SizeOfValue;
          Decimal? nullable2 = MeasurementData.GetValue(typeOfMinolDevice, payload, offset, header.SizeOfValue);
          if (nullable2.HasValue)
            measurementData.Data.Add(key.AddDays(15.0), nullable2);
          key = key.AddMonths(1);
        }
        else if (header.Type == MeasurementDataType.Quarter)
          key = key.AddMinutes(15.0);
        else if (header.Type == MeasurementDataType.Timediff)
          throw new NotImplementedException("MeasurementDataType.Timediff handler is not implemented!");
        offset += header.SizeOfValue;
      }
      return measurementData;
    }

    private static Decimal? GetValue(DeviceTypes deviceType, byte[] buffer, int offset, int size)
    {
      if (buffer.Length < offset + size)
        throw new ArgumentOutOfRangeException("Wrong offset!");
      if (size == 2)
      {
        ushort uint16 = BitConverter.ToUInt16(buffer, offset);
        if (uint16 == ushort.MaxValue)
          return new Decimal?();
        return deviceType == DeviceTypes.HumiditySensor || deviceType == DeviceTypes.TemperatureSensor ? new Decimal?((Decimal) uint16 / 10M) : MeasurementData.GetDecimalValue(buffer[offset + 1], buffer[offset]);
      }
      if (size != 4)
        throw new NotSupportedException(string.Format("Value size {0} is not supported!", (object) size));
      uint uint32 = BitConverter.ToUInt32(buffer, offset);
      if (uint32 == uint.MaxValue)
        return new Decimal?();
      return deviceType == DeviceTypes.EHCA_M6_Radio3 ? new Decimal?((Decimal) uint32 / 4M) : new Decimal?((Decimal) uint32);
    }

    private static Decimal? GetDecimalValue(byte byte1, byte byte2)
    {
      if (byte1 == byte.MaxValue && byte2 == byte.MaxValue)
        return new Decimal?();
      if (byte1 == (byte) 0 && byte2 == (byte) 0)
        return new Decimal?(0M);
      long num1 = (long) ((int) byte1 << 8) | (long) byte2;
      byte num2 = (byte) ((ulong) num1 & 3UL);
      Decimal num3 = (Decimal) (num1 >> 2);
      switch (num2)
      {
        case 0:
          return new Decimal?(num3);
        case 1:
          return new Decimal?(num3 + 0.25M);
        case 2:
          return new Decimal?(num3 + 0.50M);
        case 3:
          return new Decimal?(num3 + 0.75M);
        default:
          throw new NotImplementedException();
      }
    }
  }
}
