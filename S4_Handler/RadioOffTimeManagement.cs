// Decompiled with JetBrains decompiler
// Type: S4_Handler.RadioOffTimeManagement
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Text;

#nullable disable
namespace S4_Handler
{
  public class RadioOffTimeManagement
  {
    private const int DaySeconds = 86400;

    public RadioOffTimeManagement.Day DaySelection { get; private set; }

    public byte DailyOnHour { get; private set; }

    public byte DailyOffHour { get; private set; }

    public RadioOffTimeManagement.Month MonthSelection { get; private set; }

    public byte MonthlyOnDay { get; private set; }

    public byte MonthlyOffDay { get; private set; }

    public RadioOffTimeManagement(
      RadioOffTimeManagement.Day daySelection,
      byte dailyOnHour,
      byte dailyOffHour,
      RadioOffTimeManagement.Month monthSelection,
      byte monthlyOnDay,
      byte monthlyOffDay)
    {
      this.DaySelection = daySelection;
      this.DailyOnHour = dailyOnHour;
      this.DailyOffHour = dailyOffHour;
      this.MonthSelection = monthSelection;
      this.MonthlyOnDay = monthlyOnDay;
      this.MonthlyOffDay = monthlyOffDay;
      this.CheckParameters();
    }

    public RadioOffTimeManagement(byte[] configBytes, int startIndex)
    {
      if (startIndex + 7 > configBytes.Length)
        throw new Exception("Config not complete");
      this.DaySelection = (RadioOffTimeManagement.Day) ByteArrayScanner.ScanByte(configBytes, ref startIndex);
      this.DailyOnHour = ByteArrayScanner.ScanByte(configBytes, ref startIndex);
      this.DailyOffHour = ByteArrayScanner.ScanByte(configBytes, ref startIndex);
      this.MonthSelection = (RadioOffTimeManagement.Month) ByteArrayScanner.ScanUInt16(configBytes, ref startIndex);
      this.MonthlyOnDay = ByteArrayScanner.ScanByte(configBytes, ref startIndex);
      this.MonthlyOffDay = ByteArrayScanner.ScanByte(configBytes, ref startIndex);
      this.CheckParameters();
    }

    public RadioOffTimeManagement(string configText)
    {
      bool flag = false;
      try
      {
        configText = configText.Replace(" ", "");
        string str1 = configText;
        char[] separator = new char[1]{ ';' };
        foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
          if (str2 == RadioOffTimeManagement.DayShort.always.ToString())
          {
            this.DaySelection = RadioOffTimeManagement.Day.AllDays;
            this.DailyOnHour = (byte) 0;
            this.DailyOffHour = (byte) 0;
          }
          else if (str2 == RadioOffTimeManagement.MonthShort.AllMonth.ToString())
          {
            this.MonthSelection = RadioOffTimeManagement.Month.AllMonth;
            this.MonthlyOnDay = (byte) 1;
            this.MonthlyOffDay = (byte) 31;
          }
          else
          {
            string[] strArray1 = str2.Split(':');
            string[] strArray2 = strArray1.Length >= 1 && strArray1.Length <= 2 ? strArray1[0].Split(new char[1]
            {
              '-'
            }, StringSplitOptions.RemoveEmptyEntries) : throw new Exception("Config need two defines splitted by ':'");
            if (strArray2.Length < 1 || strArray2.Length > 2)
              throw new Exception("Illegal text: " + strArray1[0]);
            string[] strArray3 = (string[]) null;
            if (strArray1.Length == 2)
            {
              strArray3 = strArray1[1].Split(new char[1]
              {
                '-'
              }, StringSplitOptions.RemoveEmptyEntries);
              if (strArray3.Length != 2)
                throw new Exception("Config needs always a number range");
            }
            RadioOffTimeManagement.DayShort result1;
            if (Enum.TryParse<RadioOffTimeManagement.DayShort>(strArray2[0], out result1))
            {
              RadioOffTimeManagement.DayShort result2 = result1;
              if (strArray2.Length == 2 && !Enum.TryParse<RadioOffTimeManagement.DayShort>(strArray2[1], out result2))
                throw new Exception("Illegal day text: " + strArray2[1]);
              if (result2 < result1)
                throw new Exception("Illegal day range: " + strArray1[0]);
              for (; result1 <= result2; result1 = (RadioOffTimeManagement.DayShort) ((uint) result1 << 1))
                this.DaySelection |= (RadioOffTimeManagement.Day) result1;
              if (strArray3 != null)
              {
                byte result3;
                if (!byte.TryParse(strArray3[0], out result3))
                  throw new Exception("Illegal start hour: " + strArray3[0]);
                byte result4;
                if (!byte.TryParse(strArray3[1], out result4))
                  throw new Exception("Illegal end hour: " + strArray3[1]);
                if (result3 > (byte) 23 || result4 > (byte) 23)
                  throw new Exception("Hour > 23 not possible: " + strArray1[1]);
                if (flag && ((int) result3 != (int) this.DailyOnHour || (int) result4 != (int) this.DailyOffHour))
                  throw new Exception("Illegal hours range redefinition");
                this.DailyOnHour = result3;
                this.DailyOffHour = result4;
                flag = true;
              }
            }
            else
            {
              RadioOffTimeManagement.MonthShort result5;
              if (!Enum.TryParse<RadioOffTimeManagement.MonthShort>(strArray2[0], out result5))
                throw new Exception("Illegal text: " + strArray2[0]);
              RadioOffTimeManagement.MonthShort result6 = result5;
              if (strArray2.Length == 2 && !Enum.TryParse<RadioOffTimeManagement.MonthShort>(strArray2[1], out result6))
                throw new Exception("Illegal month text: " + strArray2[1]);
              if (result6 < result5)
                throw new Exception("Illegal month range: " + strArray1[0]);
              for (; result5 <= result6; result5 = (RadioOffTimeManagement.MonthShort) ((uint) result5 << 1))
                this.MonthSelection |= (RadioOffTimeManagement.Month) result5;
              if (strArray3 != null)
              {
                byte result7;
                if (!byte.TryParse(strArray3[0], out result7))
                  throw new Exception("Illegal start Day: " + strArray3[0]);
                byte result8;
                if (!byte.TryParse(strArray3[1], out result8))
                  throw new Exception("Illegal end Day: " + strArray3[1]);
                if ((int) result8 < (int) result7 || result7 < (byte) 1 || result7 > (byte) 31 || result8 < (byte) 1 || result8 > (byte) 31)
                  throw new Exception("Illegal days range: " + strArray1[1]);
                if (this.MonthlyOnDay > (byte) 0 && ((int) result7 != (int) this.MonthlyOnDay || (int) result8 != (int) this.MonthlyOffDay))
                  throw new Exception("Illegal days range redefinition");
                this.MonthlyOnDay = result7;
                this.MonthlyOffDay = result8;
              }
            }
          }
        }
        if (this.DaySelection == (RadioOffTimeManagement.Day) 0)
          throw new Exception("Not a complete day selection found");
        if (!flag)
        {
          this.DailyOnHour = (byte) 0;
          this.DailyOffHour = (byte) 0;
        }
        if (this.MonthSelection == (RadioOffTimeManagement.Month) 0)
        {
          this.MonthSelection = RadioOffTimeManagement.Month.AllMonth;
          this.MonthlyOnDay = (byte) 1;
          this.MonthlyOffDay = (byte) 31;
        }
        else
        {
          if (this.MonthlyOnDay != (byte) 0)
            return;
          this.MonthlyOnDay = (byte) 1;
          this.MonthlyOffDay = (byte) 31;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("ActiveTime format error", ex);
      }
    }

    private void CheckParameters()
    {
      if ((this.DaySelection & RadioOffTimeManagement.Day.AllDays) == (RadioOffTimeManagement.Day) 0)
        throw new Exception("Missing day selection");
      if (this.DailyOnHour > (byte) 96)
        throw new Exception("Illegal dayStartTime");
      if (this.DailyOffHour > (byte) 96)
        throw new Exception("Illegal dayEndTime");
      if ((this.MonthSelection & RadioOffTimeManagement.Month.AllMonth) == (RadioOffTimeManagement.Month) 0)
        throw new Exception("Missing day selection");
      if (this.MonthlyOnDay == (byte) 0 || this.MonthlyOnDay > (byte) 31)
        throw new Exception("Illegal monthStartDay");
      if (this.MonthlyOffDay == (byte) 0 || this.MonthlyOffDay > (byte) 31)
        throw new Exception("Illegal monthLastDay");
      if ((int) this.MonthlyOffDay < (int) this.MonthlyOnDay)
        throw new Exception("monthLastDay < monthStartDay");
    }

    public byte[] GetConfigBytes()
    {
      return new byte[7]
      {
        (byte) this.DaySelection,
        this.DailyOnHour,
        this.DailyOffHour,
        (byte) this.MonthSelection,
        (byte) ((uint) this.MonthSelection >> 8),
        this.MonthlyOnDay,
        this.MonthlyOffDay
      };
    }

    public DateTime GetNextOnTime(DateTime theTime)
    {
      DateTime nextMonthTime = this.GetNextMonthTime(this.MonthlyOnDay, theTime);
      DateTime dateTime1 = this.GetNextMonthTime(this.MonthlyOffDay, theTime).AddDays(1.0);
      if (nextMonthTime <= theTime || dateTime1 < nextMonthTime)
      {
        DateTime nextWeekTime1 = this.GetNextWeekTime(this.DailyOnHour, theTime);
        DateTime dateTime2 = this.GetNextWeekTime(this.DailyOffHour, theTime).AddHours(1.0);
        if (nextWeekTime1 <= theTime || dateTime2 < nextWeekTime1)
          return theTime;
        if (nextWeekTime1 < dateTime1)
          return nextWeekTime1;
        DateTime nextWeekTime2 = this.GetNextWeekTime(this.DailyOnHour, nextMonthTime);
        this.GetNextWeekTime(this.DailyOffHour, nextMonthTime).AddHours(1.0);
        return nextWeekTime2 >= dateTime1 ? nextMonthTime : nextWeekTime2;
      }
      DateTime nextWeekTime = this.GetNextWeekTime(this.DailyOnHour, nextMonthTime);
      return nextWeekTime >= dateTime1 ? nextMonthTime : nextWeekTime;
    }

    public DateTime GetNextOffTime(DateTime theTime)
    {
      DateTime nextMonthTime = this.GetNextMonthTime(this.MonthlyOnDay, theTime);
      DateTime dateTime1 = this.GetNextMonthTime(this.MonthlyOffDay, theTime).AddDays(1.0);
      if (!(nextMonthTime <= theTime) && !(dateTime1 < nextMonthTime))
        return theTime;
      DateTime nextWeekTime = this.GetNextWeekTime(this.DailyOnHour, theTime);
      DateTime dateTime2 = this.GetNextWeekTime(this.DailyOffHour, theTime).AddHours(1.0);
      if (!(nextWeekTime <= theTime) && !(dateTime2 < nextWeekTime))
        return theTime;
      return dateTime1 < dateTime2 ? dateTime1 : dateTime2;
    }

    private DateTime GetNextMonthTime(byte monthDay, DateTime theTime)
    {
      try
      {
        bool flag = false;
        for (RadioOffTimeManagement.Month month = (RadioOffTimeManagement.Month) (1 << theTime.Month - 1); month <= RadioOffTimeManagement.Month.AllMonth; month = (RadioOffTimeManagement.Month) ((uint) month << 1))
        {
          if ((this.MonthSelection & month) != 0)
          {
            if (flag || theTime.Day <= (int) monthDay)
            {
              byte day = (byte) DateTime.DaysInMonth(theTime.Year, theTime.Month);
              return (int) day < (int) monthDay ? new DateTime(theTime.Year, theTime.Month, (int) day) : new DateTime(theTime.Year, theTime.Month, (int) monthDay);
            }
            flag = true;
          }
          theTime = theTime.AddMonths(1);
        }
        for (RadioOffTimeManagement.Month month = RadioOffTimeManagement.Month.January; month <= RadioOffTimeManagement.Month.AllMonth; month = (RadioOffTimeManagement.Month) ((uint) month << 1))
        {
          if ((this.MonthSelection & month) != 0)
          {
            byte day = (byte) DateTime.DaysInMonth(theTime.Year, theTime.Month);
            return (int) day < (int) monthDay ? new DateTime(theTime.Year, theTime.Month, (int) day) : new DateTime(theTime.Year, theTime.Month, (int) monthDay);
          }
          theTime = theTime.AddMonths(1);
        }
        throw new Exception("Internal error by TimeCode analysis");
      }
      catch (Exception ex)
      {
        throw new Exception("NextMonthTime calculation error", ex);
      }
    }

    private DateTime GetNextWeekTime(byte dailyHour, DateTime theTime)
    {
      int num = (int) (theTime.DayOfWeek - 1);
      if (num < 0)
        num = 6;
      bool flag = false;
      for (RadioOffTimeManagement.Day day = (RadioOffTimeManagement.Day) (1 << num); day <= RadioOffTimeManagement.Day.AllDays; day = (RadioOffTimeManagement.Day) ((uint) day << 1))
      {
        if ((this.DaySelection & day) != 0)
        {
          if (flag || theTime.Hour <= (int) dailyHour)
            return new DateTime(theTime.Year, theTime.Month, theTime.Day, (int) dailyHour, 0, 0);
          flag = true;
        }
        theTime = theTime.AddDays(1.0);
      }
      for (RadioOffTimeManagement.Day day = RadioOffTimeManagement.Day.Monday; day <= RadioOffTimeManagement.Day.AllDays; day = (RadioOffTimeManagement.Day) ((uint) day << 1))
      {
        if ((this.DaySelection & day) != 0)
        {
          if (flag || theTime.Hour <= (int) dailyHour)
            return new DateTime(theTime.Year, theTime.Month, theTime.Day, (int) dailyHour, 0, 0);
          flag = true;
        }
        theTime = theTime.AddDays(1.0);
      }
      throw new Exception("Internal error by TimeCode analysis");
    }

    public override string ToString()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      int num1 = 1;
      for (int index = 0; index < 7; ++index)
      {
        if ((this.DaySelection & (RadioOffTimeManagement.Day) num1) > (RadioOffTimeManagement.Day) 0)
        {
          if (stringBuilder1.Length > 0)
            stringBuilder1.Append(",");
          stringBuilder1.Append(((RadioOffTimeManagement.DayShort) num1).ToString());
        }
        num1 <<= 1;
      }
      stringBuilder1.Append(";" + this.DailyOnHour.ToString());
      StringBuilder stringBuilder2 = stringBuilder1;
      byte num2 = this.DailyOffHour;
      string str1 = ";" + num2.ToString();
      stringBuilder2.Append(str1);
      int num3 = 1;
      for (int index = 0; index < 12; ++index)
      {
        if ((this.MonthSelection & (RadioOffTimeManagement.Month) num3) > (RadioOffTimeManagement.Month) 0)
        {
          if (stringBuilder1.Length > 0)
            stringBuilder1.Append(",");
          stringBuilder1.Append(((RadioOffTimeManagement.MonthShort) num3).ToString());
        }
        num3 <<= 1;
      }
      StringBuilder stringBuilder3 = stringBuilder1;
      num2 = this.MonthlyOnDay;
      string str2 = ";" + num2.ToString();
      stringBuilder3.Append(str2);
      StringBuilder stringBuilder4 = stringBuilder1;
      num2 = this.MonthlyOffDay;
      string str3 = ";" + num2.ToString();
      stringBuilder4.Append(str3);
      return stringBuilder1.ToString();
    }

    public enum Day : byte
    {
      Monday = 1,
      Tuesday = 2,
      Wednesday = 4,
      Thursday = 8,
      Friday = 16, // 0x10
      Saturday = 32, // 0x20
      Sunday = 64, // 0x40
      AllDays = 127, // 0x7F
    }

    public enum DayShort : byte
    {
      Mo = 1,
      Tu = 2,
      We = 4,
      Th = 8,
      Fr = 16, // 0x10
      Sa = 32, // 0x20
      Su = 64, // 0x40
      always = 127, // 0x7F
    }

    public enum Month : ushort
    {
      January = 1,
      February = 2,
      March = 4,
      April = 8,
      May = 16, // 0x0010
      June = 32, // 0x0020
      July = 64, // 0x0040
      August = 128, // 0x0080
      September = 256, // 0x0100
      October = 512, // 0x0200
      November = 1024, // 0x0400
      December = 2048, // 0x0800
      AllMonth = 4095, // 0x0FFF
    }

    public enum MonthShort : ushort
    {
      Jan = 1,
      Feb = 2,
      Mar = 4,
      Apr = 8,
      May = 16, // 0x0010
      Jun = 32, // 0x0020
      Jul = 64, // 0x0040
      Aug = 128, // 0x0080
      Sep = 256, // 0x0100
      Oct = 512, // 0x0200
      Nov = 1024, // 0x0400
      Dec = 2048, // 0x0800
      AllMonth = 4095, // 0x0FFF
    }
  }
}
