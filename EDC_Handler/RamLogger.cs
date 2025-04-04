// Decompiled with JetBrains decompiler
// Type: EDC_Handler.RamLogger
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.Collections.Generic;
using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public sealed class RamLogger
  {
    public RamLoggerType Type { get; set; }

    public RamLoggerHeader Header { get; set; }

    public List<uint> Data { get; set; }

    public SortedList<DateTime, uint> Values
    {
      get
      {
        if (this.Data == null || this.Header == null || !this.Header.LastDate.HasValue || this.Header.Length < (byte) 1)
          return (SortedList<DateTime, uint>) null;
        SortedList<DateTime, uint> values = new SortedList<DateTime, uint>((int) this.Header.Length);
        DateTime dateTime1 = this.Header.LastDate.Value;
        DateTime dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, dateTime1.Hour, dateTime1.Minute, 0);
        int num = (int) this.Header.FifoEnd - 1;
        switch (this.Type)
        {
          case RamLoggerType.QuarterHour:
            for (int index = 0; index < (int) this.Header.Length; ++index)
            {
              if (index <= num)
                values.Add(dateTime2.AddMinutes((double) (-15 * (num - index))), this.Data[index]);
              else
                values.Add(dateTime2.AddMinutes((double) (-15 * ((int) this.Header.Length - (index - num)))), this.Data[index]);
            }
            break;
          case RamLoggerType.Daily:
            for (int index = 0; index < (int) this.Header.Length; ++index)
            {
              if (index <= num)
                values.Add(dateTime2.AddDays((double) (-1 * (num - index))), this.Data[index]);
              else
                values.Add(dateTime2.AddDays((double) (-1 * ((int) this.Header.Length - (index - num)))), this.Data[index]);
            }
            break;
          case RamLoggerType.Halfmonth:
            for (int index = 0; index < (int) this.Header.Length; ++index)
            {
              if (index <= num)
                values.Add(dateTime2.AddMonths(-1 * (num - index)), this.Data[index]);
              else
                values.Add(dateTime2.AddMonths(-1 * ((int) this.Header.Length - (index - num))), this.Data[index]);
            }
            break;
          case RamLoggerType.Fullmonth:
            for (int index = 0; index < (int) this.Header.Length; ++index)
            {
              if (index <= num)
                values.Add(dateTime2.AddMonths(-1 * (num - index)), this.Data[index]);
              else
                values.Add(dateTime2.AddMonths(-1 * ((int) this.Header.Length - (index - num))), this.Data[index]);
            }
            break;
          case RamLoggerType.DueDate:
            for (int index = 0; index < (int) this.Header.Length; ++index)
            {
              if (index <= num)
                values.Add(dateTime2.AddYears(-1 * (num - index)), this.Data[index]);
              else
                values.Add(dateTime2.AddYears(-1 * ((int) this.Header.Length - (index - num))), this.Data[index]);
            }
            break;
        }
        return values;
      }
    }

    public bool? IsCrcOK
    {
      get
      {
        if (this.Header == null || this.Data == null)
          return new bool?();
        List<byte> buffer = new List<byte>();
        this.Data.ForEach((Action<uint>) (x => buffer.AddRange((IEnumerable<byte>) BitConverter.GetBytes(x))));
        return new bool?((int) this.Header.CRC == (int) Util.CalculatesCRC16_CC430(buffer.ToArray(), 0, (int) this.Header.MaxLength * 4));
      }
    }

    public RamLogger() => this.Data = new List<uint>();

    public override string ToString()
    {
      return this.Type.ToString() + " " + this.Data.Count.ToString() + " items";
    }

    public DataTable CreateTableValues()
    {
      SortedList<DateTime, uint> values = this.Values;
      if (values == null)
        return (DataTable) null;
      DataTable tableValues = new DataTable();
      tableValues.Columns.Add("#", typeof (int));
      tableValues.Columns.Add("Date", typeof (DateTime));
      tableValues.Columns.Add("Value", typeof (uint));
      int num = 1;
      foreach (KeyValuePair<DateTime, uint> keyValuePair in values)
        tableValues.Rows.Add((object) num++, (object) keyValuePair.Key, (object) keyValuePair.Value);
      return tableValues;
    }

    public DataTable CreateTableRawData()
    {
      if (this.Header == null || this.Data == null || this.Data.Count == 0)
        return (DataTable) null;
      DataTable tableRawData = new DataTable();
      tableRawData.Columns.Add("Position", typeof (int));
      tableRawData.Columns.Add("Address", typeof (string));
      tableRawData.Columns.Add("Value", typeof (uint));
      for (int index = 0; index < (int) this.Header.MaxLength; ++index)
        tableRawData.Rows.Add((object) (index + 1), (object) ("0x" + ((int) this.Header.Address + index * 4).ToString("X4")), (object) this.Data[index]);
      return tableRawData;
    }
  }
}
