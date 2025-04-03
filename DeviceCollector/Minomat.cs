// Decompiled with JetBrains decompiler
// Type: DeviceCollector.Minomat
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public abstract class Minomat
  {
    protected ulong _minomatSerial;
    protected ulong _minomatPassword;

    public virtual string minomatPassword
    {
      set => this.getUserPassword(value, ref this._minomatPassword);
    }

    public virtual string minomatSerial
    {
      set => this.getSerialNo(value, ref this._minomatSerial, true, false);
    }

    internal abstract bool GetDateTime(out DateTime dateTime);

    internal abstract bool SetDateTime(DateTime dateTime);

    internal abstract bool Read(
      byte dataType,
      byte slaveIndex,
      int months,
      int monthOffset,
      out Dictionary<string, Minomat.MinomatDeviceValue> processedMinomatDevices,
      out Dictionary<string, byte> hkveSerialNumbersAsStrings);

    internal abstract bool GetAllRegisteredDevices(
      out List<MinomatDevice> deviceList,
      byte startAddress,
      byte endAddress);

    internal virtual bool FillDevicesWithDetails(List<MinomatDevice> devices) => false;

    internal abstract bool GetSystemStatus(out object systemStatus);

    internal abstract bool FindHKVE(ulong serialOfHKVE, out string answer);

    internal abstract bool RegisterHKVE(List<MinomatDevice> deviceList);

    internal bool DeRegisterHKVE(MinomatDevice device)
    {
      if (device == null)
        return false;
      return this.DeRegisterHKVE(new List<MinomatDevice>()
      {
        device
      });
    }

    internal abstract bool DeRegisterHKVE(List<MinomatDevice> deviceList);

    internal abstract bool CheckHKVERegistration(byte index, out ulong serialNo);

    internal abstract bool GetConfiguration(out object config);

    internal abstract bool StopReception();

    internal abstract bool SystemInit();

    internal abstract bool StartHKVEReceptionWindow();

    internal abstract bool Connect();

    internal abstract bool SetConfiguration(MinomatV2.Configuration configuration);

    internal virtual bool getSerialNo(
      string input,
      ref ulong serialNo,
      bool useBCDEncoding,
      bool addZeroes)
    {
      string str = input;
      serialNo = 0UL;
      if (addZeroes)
      {
        if (str.Length < 8)
        {
          int num = 8 - str.Length;
          for (int index = 0; index < num; ++index)
            str = "0" + str;
        }
      }
      else if (str.Length != 8)
        return false;
      if (useBCDEncoding)
      {
        long num = Util.ToLong((object) str);
        serialNo = (ulong) Util.ConvertInt64ToBcdInt64(num);
        return serialNo > 0UL;
      }
      return str == "00000000";
    }

    internal virtual bool getUserPassword(string input, ref ulong userPassword)
    {
      string str = input;
      if (str.Length != 4)
        return false;
      userPassword = (ulong) ((int) (byte) str[0] << 24 | (int) (byte) str[1] << 16 | (int) (byte) str[2] << 8 | (int) (byte) str[3]);
      return true;
    }

    public class MinomatInfo
    {
      public object systemStatus;
      public object configuration;
      public DateTime systemTime;
    }

    protected class EventDataset
    {
      public Minomat.CCommandDate date1;
      public ulong eventReading1;
      public Minomat.CCommandDate date2;
      public ulong eventReading2;
      public ulong hkveSerialNo;

      public EventDataset()
      {
        this.eventReading1 = 0UL;
        this.eventReading2 = 0UL;
      }

      public bool isEmpty()
      {
        return this.eventReading1 == 0UL && this.eventReading2 == 0UL && this.date1.getEncodedDate() == (ushort) 0 && this.date2.getEncodedDate() == (ushort) 0;
      }
    }

    protected class DailyDataset
    {
      public Minomat.CCommandDate date;
      public ulong dailyReading;

      public DailyDataset() => this.dailyReading = 0UL;
    }

    protected class MonthlyDataset
    {
      public ulong hkveSerialNo;
      public byte status;
      public Minomat.CCommandDate date;
      public ushort fieldForceSum;
      public ushort hkveProtocols;
      public byte deviceType;
      public ulong fullMonthReading;
      public ushort factor;
      public ulong halfMonthReading;

      public MonthlyDataset()
      {
        this.hkveSerialNo = 0UL;
        this.status = (byte) 0;
        this.fieldForceSum = (ushort) 0;
        this.hkveProtocols = (ushort) 0;
        this.deviceType = (byte) 0;
        this.fullMonthReading = 0UL;
        this.factor = (ushort) 0;
        this.halfMonthReading = 0UL;
      }
    }

    public enum DataType
    {
      EventData = 1,
      MonthlyData = 2,
      DailyData = 4,
      HalfMonthlyData = 8,
    }

    public class MinomatDeviceValue
    {
      public SortedList<DateTime, List<Minomat.ProcessedData>> readoutValues = new SortedList<DateTime, List<Minomat.ProcessedData>>();
      public SortedList<OverrideID, ConfigurationParameter> configValues = new SortedList<OverrideID, ConfigurationParameter>();
    }

    public class ProcessedData
    {
      public Minomat.DataType DataType;
      public string ParameterName;
      public string ParameterValue;

      public ProcessedData(string parameterName, string parameterValue, Minomat.DataType dataType)
      {
        this.ParameterName = parameterName;
        this.ParameterValue = parameterValue;
        this.DataType = dataType;
      }
    }

    public class CCommandTime
    {
      private byte m_hours;
      private byte m_minutes;
      private byte m_seconds;

      public CCommandTime(byte hours, byte minutes, byte seconds)
      {
        this.m_hours = hours;
        this.m_minutes = minutes;
        this.m_seconds = seconds;
      }

      public byte getHours() => this.m_hours;

      public byte getMinutes() => this.m_minutes;

      public byte getSeconds() => this.m_seconds;

      public bool isValid()
      {
        return this.m_hours >= (byte) 0 && this.m_hours <= (byte) 23 && this.m_minutes >= (byte) 0 && this.m_minutes <= (byte) 59 && this.m_seconds >= (byte) 0 && this.m_seconds <= (byte) 59;
      }

      public override string ToString()
      {
        return this.m_hours.ToString() + ":" + this.m_minutes.ToString() + ":" + this.m_seconds.ToString();
      }
    }

    public class CCommandDate
    {
      private byte m_day;
      private byte m_month;
      private byte m_year;

      internal CCommandDate(byte day, byte month, byte year)
      {
        this.m_day = day;
        this.m_month = month;
        this.m_year = year;
      }

      internal CCommandDate(ushort encodedDate)
      {
        if (encodedDate == (ushort) 0)
        {
          this.m_year = this.m_month = this.m_day = (byte) 0;
        }
        else
        {
          this.m_year = (byte) (((int) encodedDate >> 12 & 15) << 3 | (int) encodedDate >> 5 & 7);
          this.m_month = (byte) ((int) encodedDate >> 8 & 15);
          this.m_day = (byte) ((uint) encodedDate & 31U);
        }
      }

      public byte getDay() => this.m_day;

      public byte getMonth() => this.m_month;

      public byte getYear() => this.m_year;

      internal void setDay(byte day) => this.m_day = day;

      internal void setMonth(byte month) => this.m_month = month;

      internal void setYear(byte year) => this.m_year = year;

      internal ushort getEncodedDate()
      {
        return this.m_year == (byte) 0 && this.m_month == (byte) 0 && this.m_day == (byte) 0 ? (ushort) 0 : (ushort) ((uint) ((int) this.m_year << 9 & 61440 | (int) this.m_month << 8 | (int) this.m_year << 5 & 224) | (uint) this.m_day);
      }

      public override string ToString()
      {
        return this.m_day.ToString() + "." + this.m_month.ToString() + "." + this.m_year.ToString();
      }
    }
  }
}
