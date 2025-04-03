// Decompiled with JetBrains decompiler
// Type: PDC_Handler.RamLogger
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;
using System.Collections.Generic;
using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public sealed class RamLogger
  {
    public RamLoggerType Type { get; set; }

    public RamLoggerHeader Header { get; set; }

    public List<ReadValue> Data { get; set; }

    public SortedList<DateTime, ReadValue> Values
    {
      get
      {
        if (this.Data == null || this.Header == null || !this.Header.LastDate.HasValue || this.Header.Length < (byte) 1)
          return (SortedList<DateTime, ReadValue>) null;
        SortedList<DateTime, ReadValue> values = new SortedList<DateTime, ReadValue>((int) this.Header.Length);
        DateTime dateTime1 = this.Header.LastDate.Value;
        DateTime dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, dateTime1.Hour, dateTime1.Minute, 0);
        int num = (int) this.Header.FifoEnd - 1;
        for (int index = 0; index < (int) this.Header.Length; ++index)
        {
          if (index <= num)
          {
            switch (this.Type)
            {
              case RamLoggerType.QuarterHour:
                values.Add(dateTime2.AddMinutes((double) (-15 * (num - index))), this.Data[index]);
                continue;
              case RamLoggerType.Daily:
                values.Add(dateTime2.AddDays((double) (-1 * (num - index))), this.Data[index]);
                continue;
              case RamLoggerType.Halfmonth:
                values.Add(dateTime2.AddMonths(-1 * (num - index)), this.Data[index]);
                continue;
              case RamLoggerType.Fullmonth:
                values.Add(dateTime2.AddMonths(-1 * (num - index)), this.Data[index]);
                continue;
              case RamLoggerType.DueDate:
                values.Add(dateTime2.AddYears(-1 * (num - index)), this.Data[index]);
                continue;
              default:
                throw new NotSupportedException(this.Type.ToString());
            }
          }
          else
          {
            switch (this.Type)
            {
              case RamLoggerType.QuarterHour:
                values.Add(dateTime2.AddMinutes((double) (-15 * ((int) this.Header.Length - (index - num)))), this.Data[index]);
                break;
              case RamLoggerType.Daily:
                values.Add(dateTime2.AddDays((double) (-1 * ((int) this.Header.Length - (index - num)))), this.Data[index]);
                break;
              case RamLoggerType.Halfmonth:
                values.Add(dateTime2.AddMonths(-1 * ((int) this.Header.Length - (index - num))), this.Data[index]);
                break;
              case RamLoggerType.Fullmonth:
                values.Add(dateTime2.AddMonths(-1 * ((int) this.Header.Length - (index - num))), this.Data[index]);
                break;
              case RamLoggerType.DueDate:
                values.Add(dateTime2.AddYears(-1 * ((int) this.Header.Length - (index - num))), this.Data[index]);
                break;
              default:
                throw new NotSupportedException(this.Type.ToString());
            }
          }
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
        this.Data.ForEach((Action<ReadValue>) (x => buffer.AddRange((IEnumerable<byte>) x.Buffer)));
        return new bool?((int) this.Header.CRC == (int) Util.CalculatesCRC16_CC430(buffer.ToArray(), 0, (int) this.Header.MaxLength * 4));
      }
    }

    public RamLogger() => this.Data = new List<ReadValue>();

    public override string ToString()
    {
      return this.Type.ToString() + " " + this.Data.Count.ToString() + " items";
    }

    public DataTable CreateTableValues()
    {
      SortedList<DateTime, ReadValue> values = this.Values;
      if (values == null)
        return (DataTable) null;
      DataTable tableValues = new DataTable();
      tableValues.Columns.Add("#", typeof (int));
      tableValues.Columns.Add("Date", typeof (DateTime));
      tableValues.Columns.Add("ValueInputA", typeof (uint));
      tableValues.Columns.Add("ValueinputB", typeof (uint));
      int num = 1;
      foreach (KeyValuePair<DateTime, ReadValue> keyValuePair in values)
        tableValues.Rows.Add((object) num++, (object) keyValuePair.Key, (object) keyValuePair.Value.A, (object) keyValuePair.Value.B);
      return tableValues;
    }

    public DataTable CreateTableRawData()
    {
      if (this.Header == null || this.Data == null || this.Data.Count == 0)
        return (DataTable) null;
      DataTable tableRawData = new DataTable();
      tableRawData.Columns.Add("Position", typeof (int));
      tableRawData.Columns.Add("Address", typeof (string));
      tableRawData.Columns.Add("ValueInputA", typeof (uint));
      tableRawData.Columns.Add("ValueInputB", typeof (uint));
      for (int index = 0; index < (int) this.Header.MaxLength; ++index)
        tableRawData.Rows.Add((object) (index + 1), (object) ("0x" + ((int) this.Header.Address + index * 4).ToString("X4")), (object) this.Data[index].A, (object) this.Data[index].B);
      return tableRawData;
    }
  }
}
