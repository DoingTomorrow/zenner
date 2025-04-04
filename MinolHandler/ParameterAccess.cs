// Decompiled with JetBrains decompiler
// Type: MinolHandler.ParameterAccess
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using NLog;
using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  public class ParameterAccess
  {
    private static Logger logger = LogManager.GetLogger(nameof (ParameterAccess));
    private const long SECONDS_PER_DAY = 86400;
    internal TelegramParameter TelegramPara;
    internal ParameterAccess BitAccesParent;
    internal string RangeName;
    internal string Name;

    internal string GetParameterMapDataHex(short[] Map)
    {
      StringBuilder stringBuilder = new StringBuilder(20);
      if (this.TelegramPara.ByteLength > 0)
      {
        for (int index = 0; index < this.TelegramPara.ByteLength; ++index)
        {
          short num = Map[this.TelegramPara.Address + index];
          if (((int) num & 256) != 0)
          {
            stringBuilder.Length = 0;
            break;
          }
          string str = ((byte) num).ToString("X02");
          stringBuilder.Insert(0, str);
        }
      }
      return stringBuilder.ToString();
    }

    public object GetReadValue(short[] Map) => this.GetReadValue(Map, false);

    internal object GetReadValue(short[] Map, bool ignoreNullTest)
    {
      if (!ignoreNullTest && this.IsValueNull(Map))
        return (object) null;
      if (this.TelegramPara.ParameterType == (Type) null)
        return this.TelegramPara.RD_Type == "BCD" ? (object) this.GetMapValueIntFromBCD(Map) : (object) this.GetMapValueLong(Map);
      if (this.TelegramPara.ParameterType == typeof (long))
      {
        if (this.TelegramPara.RD_Type == "BCD")
          return (object) this.GetMapValueIntFromBCD(Map);
        return this.TelegramPara.Name == "Ticker" ? (object) (86400L - this.GetMapValueLong(Map) * 2L) : (object) this.GetMapValueLong(Map);
      }
      if (this.TelegramPara.ParameterType == typeof (Decimal) || this.TelegramPara.ParameterType == typeof (double))
      {
        long num1 = !(this.TelegramPara.RD_Type == "BCD") ? this.GetMapValueLong(Map) : this.GetMapValueIntFromBCD(Map);
        if (this.TelegramPara.ByteLength == 2 && this.TelegramPara.Name == "TotalFlowAdj")
          num1 = (long) (short) num1;
        Decimal num2 = (Decimal) num1;
        if (this.TelegramPara.UseMulDiv)
          num2 = num2 / (Decimal) this.TelegramPara.RD_Divisor * (Decimal) this.TelegramPara.RD_Factor;
        return (object) Convert.ToDecimal(num2);
      }
      return this.TelegramPara.ParameterType == typeof (DateTime) ? (object) this.GetMapValueDate(Map) : (object) null;
    }

    internal bool GetBitValue(short[] Map)
    {
      if (this.BitAccesParent == null)
        throw new Exception("Bit not available");
      return ((int) (byte) this.BitAccesParent.GetMapValueLong(Map) & this.TelegramPara.BitMask) != 0;
    }

    internal byte[] GetValueAsByteArray(object obj)
    {
      byte[] valueAsByteArray = new byte[this.TelegramPara.ByteLength];
      if (this.TelegramPara.ParameterType == (Type) null && this.TelegramPara.RD_Type == "BCD")
      {
        long bcdInt64 = Util.ConvertInt64ToBcdInt64(Util.ToLong(obj));
        for (int index = 0; index < valueAsByteArray.Length; ++index)
          valueAsByteArray[index] = (byte) (bcdInt64 >> index * 8);
      }
      else if (this.TelegramPara.ParameterType == (Type) null && this.TelegramPara.RD_Type == "UDEC")
      {
        long num = Util.ToLong(obj);
        for (int index = 0; index < valueAsByteArray.Length; ++index)
          valueAsByteArray[index] = (byte) (num >> index * 8);
      }
      else if (this.TelegramPara.ParameterType == (Type) null && this.TelegramPara.RD_Type == "IDEC")
      {
        long num = Util.ToLong((object) Math.Round(Util.ToDouble(obj) / (double) this.TelegramPara.RD_Factor * (double) this.TelegramPara.RD_Divisor));
        for (int index = 0; index < valueAsByteArray.Length; ++index)
          valueAsByteArray[index] = (byte) (num >> index * 8);
      }
      else if (this.TelegramPara.ParameterType == (Type) null && this.TelegramPara.RD_Type == "FLG")
        valueAsByteArray[0] = Convert.ToByte(obj);
      else if (this.TelegramPara.ParameterType == typeof (double))
      {
        double a = Util.ToDouble(obj);
        if (this.TelegramPara.UseMulDiv)
          a = a / (double) this.TelegramPara.RD_Factor * (double) this.TelegramPara.RD_Divisor;
        long bcdInt64 = Util.ToLong((object) Math.Round(a));
        if (this.TelegramPara.RD_Type == "BCD")
          bcdInt64 = Util.ConvertInt64ToBcdInt64(bcdInt64);
        for (int index = 0; index < valueAsByteArray.Length; ++index)
          valueAsByteArray[index] = (byte) (bcdInt64 >> index * 8);
      }
      else if (this.TelegramPara.ParameterType == typeof (Decimal))
      {
        long int64 = Convert.ToInt64(obj);
        if (this.TelegramPara.ByteLength == 2 && (byte) int64 == byte.MaxValue && (byte) (int64 >> 8) == byte.MaxValue && this.TelegramPara.Name != "TotalFlowAdj")
        {
          valueAsByteArray[0] = byte.MaxValue;
          valueAsByteArray[1] = byte.MaxValue;
        }
        else if (this.TelegramPara.ByteLength == 4 && (byte) int64 == byte.MaxValue && (byte) (int64 >> 8) == byte.MaxValue && (byte) (int64 >> 16) == byte.MaxValue && (byte) (int64 >> 24) == byte.MaxValue)
        {
          valueAsByteArray[0] = byte.MaxValue;
          valueAsByteArray[1] = byte.MaxValue;
          valueAsByteArray[2] = byte.MaxValue;
          valueAsByteArray[3] = byte.MaxValue;
        }
        else
        {
          Decimal d = Util.ToDecimal(obj);
          if (this.TelegramPara.UseMulDiv)
            d = d / (Decimal) this.TelegramPara.RD_Factor * (Decimal) this.TelegramPara.RD_Divisor;
          long bcdInt64 = Util.ToLong((object) Math.Round(d));
          if (this.TelegramPara.RD_Type == "BCD")
            bcdInt64 = Util.ConvertInt64ToBcdInt64(bcdInt64);
          for (int index = 0; index < valueAsByteArray.Length; ++index)
            valueAsByteArray[index] = (byte) (bcdInt64 >> index * 8);
        }
      }
      else if (this.TelegramPara.ParameterType == typeof (long))
      {
        long num;
        if (this.TelegramPara.Name == "Ticker")
        {
          DateTime dateTime = Util.ToDateTime(obj);
          num = (86400L - (0L + (long) (dateTime.Hour * 3600) + (long) (dateTime.Minute * 60) + (long) dateTime.Second)) / 2L;
          for (int index = 0; index < valueAsByteArray.Length; ++index)
            valueAsByteArray[index] = (byte) (num >> index * 8);
        }
        else
          num = !(this.TelegramPara.Name == "Year") ? (!(this.TelegramPara.Name == "Month") ? (!(this.TelegramPara.Name == "Day") ? (!(this.TelegramPara.Name == "Hour") ? (!(this.TelegramPara.Name == "Minute") ? (!(this.TelegramPara.Name == "Second") ? Util.ToLong(obj) : Util.ToLong((long) Util.ToDateTime(obj).Second)) : Util.ToLong((long) Util.ToDateTime(obj).Minute)) : Util.ToLong((long) Util.ToDateTime(obj).Hour)) : Util.ToLong((long) Util.ToDateTime(obj).Day)) : Util.ToLong((long) Util.ToDateTime(obj).Month)) : Util.ToLong((long) (Util.ToDateTime(obj).Year % 100));
        for (int index = 0; index < valueAsByteArray.Length; ++index)
          valueAsByteArray[index] = (byte) (num >> index * 8);
      }
      else
      {
        if (!(this.TelegramPara.ParameterType == typeof (DateTime)))
          throw new ArgumentException("(GetValueAsByteArray) Type is not supported!");
        if (this.TelegramPara.ByteLength != 2)
          ParameterAccess.logger.Fatal("Wrong length of DateTime! Expected: 2 bytes, Actual: {0} bytes", this.TelegramPara.ByteLength);
        DateTime dateTime = Util.ToDateTime(obj);
        if (dateTime == DateTime.MinValue)
        {
          valueAsByteArray[0] = byte.MaxValue;
          valueAsByteArray[1] = byte.MaxValue;
        }
        else
        {
          long num1 = (long) dateTime.Day | (long) dateTime.Month << 8;
          long num2 = (long) dateTime.Year % 100L;
          long num3 = num1 | (num2 & 120L) << 9 | (num2 & 7L) << 5;
          for (int index = 0; index < valueAsByteArray.Length; ++index)
            valueAsByteArray[index] = (byte) (num3 >> index * 8);
        }
      }
      return valueAsByteArray;
    }

    public bool IsValueNull(short[] Map)
    {
      if (this.TelegramPara.ByteLength == 1)
        return false;
      if (((int) Map[this.TelegramPara.Address] & 256) != 0)
        return true;
      for (int index = 0; index < this.TelegramPara.ByteLength; ++index)
      {
        short num = Map[this.TelegramPara.Address + index];
        if (((int) num & 256) != 0)
          return true;
        if ((short) ((int) num & (int) byte.MaxValue) != (short) byte.MaxValue)
          return false;
      }
      return true;
    }

    internal bool IsAvailable(short[] Map)
    {
      for (int index = 0; index < this.TelegramPara.ByteLength; ++index)
      {
        if (((int) Map[this.TelegramPara.Address + index] & 256) == 256)
          return false;
      }
      return true;
    }

    private DateTime GetMapValueDate(short[] Map)
    {
      long timeValue = !(this.TelegramPara.RD_Type == "BCD") ? this.GetMapValueLong(Map) : this.GetMapValueIntFromBCD(Map);
      return timeValue == 0L || timeValue == (long) ushort.MaxValue ? DateTime.MinValue : ParameterAccess.ConvertInt64_YYYYMMMMYYYDDDDD_ToDateTime(timeValue);
    }

    public static DateTime ConvertInt64_YYYYMMMMYYYDDDDD_ToDateTime(long timeValue)
    {
      long day = timeValue & 31L;
      long month = (timeValue & 3840L) >> 8;
      long num = (timeValue & 224L) >> 5 | (timeValue & 61440L) >> 9;
      long year = num < 80L ? num + 2000L : num + 1900L;
      try
      {
        return day == 0L || !Util.IsValidTimePoint((int) year, (int) month, (int) day, 0, 0) ? DateTime.MinValue : new DateTime((int) year, (int) month, (int) day);
      }
      catch
      {
        return DateTime.MinValue;
      }
    }

    public static long ConvertDateTimeTo_YYYYMMMMYYYDDDDD(DateTime date)
    {
      long num1 = (long) date.Day | (long) date.Month << 8;
      long num2 = (long) date.Year % 100L;
      return num1 | (num2 & 120L) << 9 | (num2 & 7L) << 5;
    }

    private long GetMapValueLong(short[] Map)
    {
      long mapValueLong = 0;
      for (int index = 0; index < this.TelegramPara.ByteLength; ++index)
      {
        short num1 = Map[this.TelegramPara.Address + index];
        short num2 = ((int) num1 & 256) == 0 ? (short) (byte) num1 : throw new Exception("Read on uninitialized data");
        mapValueLong += (long) num2 << index * 8;
      }
      return mapValueLong;
    }

    private long GetMapValueIntFromBCD(short[] Map)
    {
      int mapValueIntFromBcd = 0;
      int num1 = 1;
      for (int index = 0; index < this.TelegramPara.ByteLength; ++index)
      {
        short num2 = Map[this.TelegramPara.Address + index];
        if (((int) num2 & 256) != 0)
          throw new Exception("Read on uninitialized data");
        short num3 = (short) ((int) num2 & (int) byte.MaxValue);
        int num4 = ((int) num3 & 15) * num1;
        int num5 = mapValueIntFromBcd + num4;
        int num6 = num1 * 10;
        int num7 = ((int) num3 >> 4) * num6;
        mapValueIntFromBcd = num5 + num7;
        num1 = num6 * 10;
      }
      return (long) mapValueIntFromBcd;
    }
  }
}
