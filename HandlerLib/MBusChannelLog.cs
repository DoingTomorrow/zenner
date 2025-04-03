// Decompiled with JetBrains decompiler
// Type: HandlerLib.MBusChannelLog
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace HandlerLib
{
  public sealed class MBusChannelLog : ReturnValue
  {
    public byte Channel { get; set; }

    public byte LogSelected { get; set; }

    public byte StartIndex { get; set; }

    public byte EndIndex { get; set; }

    public byte Lenght { get; set; }

    public byte Year { get; set; }

    public byte Month { get; set; }

    public byte Day { get; set; }

    public byte Hour { get; set; }

    public byte Minute { get; set; }

    public byte Second { get; set; }

    public byte[] LogValues { get; set; }

    public IReadOnlyDictionary<DateTime, int> GetValues()
    {
      if (this.LogValues == null)
        return (IReadOnlyDictionary<DateTime, int>) null;
      DateTime result;
      if (!DateTime.TryParse(((int) this.Year + 2000).ToString() + "-" + this.Month.ToString("D2") + "-" + this.Day.ToString("D2") + " " + this.Hour.ToString("D2") + ":" + this.Minute.ToString("D2") + ":" + this.Second.ToString("D2"), out result))
        return (IReadOnlyDictionary<DateTime, int>) null;
      int[] numArray = new int[(int) this.Lenght];
      int startIndex = 0;
      int index = 0;
      while (index < (int) this.Lenght)
      {
        numArray[index] = BitConverter.ToInt32(this.LogValues, startIndex);
        ++index;
        startIndex += 4;
      }
      Dictionary<DateTime, int> values = new Dictionary<DateTime, int>();
      foreach (int num in numArray)
      {
        values.Add(result, num);
        switch (this.LogSelected)
        {
          case 0:
            result = result.AddMinutes(-15.0);
            break;
          case 1:
            result = result.AddDays(-1.0);
            break;
          case 2:
          case 3:
            result = result.AddMonths(-1);
            break;
          case 4:
            result = result.AddYears(-1);
            break;
          default:
            throw new NotImplementedException(this.LogSelected.ToString());
        }
      }
      return (IReadOnlyDictionary<DateTime, int>) values;
    }
  }
}
