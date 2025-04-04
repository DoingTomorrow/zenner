// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.InternetTime
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

#nullable disable
namespace ZR_ClassLibrary
{
  public class InternetTime
  {
    private const byte SNTPDataLength = 48;
    private byte[] SNTPData = new byte[48];
    private const byte offReferenceID = 12;
    private const byte offReferenceTimestamp = 16;
    private const byte offOriginateTimestamp = 24;
    private const byte offReceiveTimestamp = 32;
    private const byte offTransmitTimestamp = 40;
    public DateTime DestinationTimestamp;
    private string TimeServer;

    public _LeapIndicator LeapIndicator
    {
      get
      {
        switch ((byte) ((uint) this.SNTPData[0] >> 6))
        {
          case 0:
            return _LeapIndicator.NoWarning;
          case 1:
            return _LeapIndicator.LastMinute61;
          case 2:
            return _LeapIndicator.LastMinute59;
          default:
            return _LeapIndicator.Alarm;
        }
      }
    }

    public byte VersionNumber => (byte) (((int) this.SNTPData[0] & 56) >> 3);

    public _Mode Mode
    {
      get
      {
        switch ((byte) ((uint) this.SNTPData[0] & 7U))
        {
          case 1:
            return _Mode.SymmetricActive;
          case 2:
            return _Mode.SymmetricPassive;
          case 3:
            return _Mode.Client;
          case 4:
            return _Mode.Server;
          case 5:
            return _Mode.Broadcast;
          default:
            return _Mode.Unknown;
        }
      }
    }

    public _Stratum Stratum
    {
      get
      {
        byte num = this.SNTPData[1];
        if (num == (byte) 0)
          return _Stratum.Unspecified;
        if (num == (byte) 1)
          return _Stratum.PrimaryReference;
        return num <= (byte) 15 ? _Stratum.SecondaryReference : _Stratum.Reserved;
      }
    }

    public uint PollInterval => (uint) Math.Pow(2.0, (double) (sbyte) this.SNTPData[2]);

    public double Precision => Math.Pow(2.0, (double) (sbyte) this.SNTPData[3]);

    public double RootDelay
    {
      get
      {
        return 1000.0 * ((double) (256 * (256 * (256 * (int) this.SNTPData[4] + (int) this.SNTPData[5]) + (int) this.SNTPData[6]) + (int) this.SNTPData[7]) / 65536.0);
      }
    }

    public double RootDispersion
    {
      get
      {
        return 1000.0 * ((double) (256 * (256 * (256 * (int) this.SNTPData[8] + (int) this.SNTPData[9]) + (int) this.SNTPData[10]) + (int) this.SNTPData[11]) / 65536.0);
      }
    }

    public string ReferenceID
    {
      get
      {
        string referenceId = "";
        switch (this.Stratum)
        {
          case _Stratum.Unspecified:
          case _Stratum.PrimaryReference:
            referenceId = referenceId + ((char) this.SNTPData[12]).ToString() + ((char) this.SNTPData[13]).ToString() + ((char) this.SNTPData[14]).ToString() + ((char) this.SNTPData[15]).ToString();
            break;
          case _Stratum.SecondaryReference:
            switch (this.VersionNumber)
            {
              case 3:
                string hostNameOrAddress = this.SNTPData[12].ToString() + "." + this.SNTPData[13].ToString() + "." + this.SNTPData[14].ToString() + "." + this.SNTPData[15].ToString();
                try
                {
                  referenceId = Dns.GetHostEntry(hostNameOrAddress).HostName + " (" + hostNameOrAddress + ")";
                  break;
                }
                catch (Exception ex)
                {
                  referenceId = "N/A";
                  break;
                }
              case 4:
                referenceId = (this.ComputeDate(this.GetMilliSeconds((byte) 12)) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).ToString();
                break;
              default:
                referenceId = "N/A";
                break;
            }
            break;
        }
        return referenceId;
      }
    }

    public DateTime ReferenceTimestamp
    {
      get
      {
        return this.ComputeDate(this.GetMilliSeconds((byte) 16)) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
      }
    }

    public DateTime OriginateTimestamp => this.ComputeDate(this.GetMilliSeconds((byte) 24));

    public DateTime ReceiveTimestamp
    {
      get
      {
        return this.ComputeDate(this.GetMilliSeconds((byte) 32)) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
      }
    }

    public DateTime TransmitTimestamp
    {
      get
      {
        return this.ComputeDate(this.GetMilliSeconds((byte) 40)) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
      }
      set => this.SetDate((byte) 40, value);
    }

    public double RoundTripDelay
    {
      get
      {
        return (this.DestinationTimestamp - this.OriginateTimestamp - (this.ReceiveTimestamp - this.TransmitTimestamp)).TotalMilliseconds;
      }
    }

    public double LocalClockOffset
    {
      get
      {
        return (this.ReceiveTimestamp - this.OriginateTimestamp + (this.TransmitTimestamp - this.DestinationTimestamp)).TotalMilliseconds / 2.0;
      }
    }

    private DateTime ComputeDate(ulong milliseconds)
    {
      return new DateTime(1900, 1, 1) + TimeSpan.FromMilliseconds((double) milliseconds);
    }

    private ulong GetMilliSeconds(byte offset)
    {
      ulong num1 = 0;
      ulong num2 = 0;
      for (int index = 0; index <= 3; ++index)
        num1 = 256UL * num1 + (ulong) this.SNTPData[(int) offset + index];
      for (int index = 4; index <= 7; ++index)
        num2 = 256UL * num2 + (ulong) this.SNTPData[(int) offset + index];
      return num1 * 1000UL + num2 * 1000UL / 4294967296UL;
    }

    private void SetDate(byte offset, DateTime date)
    {
      DateTime dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
      ulong totalMilliseconds = (ulong) (date - dateTime).TotalMilliseconds;
      ulong num1 = totalMilliseconds / 1000UL;
      ulong num2 = totalMilliseconds % 1000UL * 4294967296UL / 1000UL;
      ulong num3 = num1;
      for (int index = 3; index >= 0; --index)
      {
        this.SNTPData[(int) offset + index] = (byte) (num3 % 256UL);
        num3 /= 256UL;
      }
      ulong num4 = num2;
      for (int index = 7; index >= 4; --index)
      {
        this.SNTPData[(int) offset + index] = (byte) (num4 % 256UL);
        num4 /= 256UL;
      }
    }

    private void Initialize()
    {
      this.SNTPData[0] = (byte) 27;
      for (int index = 1; index < 48; ++index)
        this.SNTPData[index] = (byte) 0;
      this.TransmitTimestamp = DateTime.Now;
    }

    public InternetTime(string host) => this.TimeServer = host;

    public DateTime Connect(bool UpdateSystemTime)
    {
      try
      {
        IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostEntry(this.TimeServer).AddressList[0], 123);
        UdpClient udpClient = new UdpClient();
        udpClient.Connect(remoteEP);
        this.Initialize();
        udpClient.Client.ReceiveTimeout = 5000;
        udpClient.Send(this.SNTPData, this.SNTPData.Length);
        this.SNTPData = udpClient.Receive(ref remoteEP);
        if (!this.IsResponseValid())
          throw new Exception("Invalid response from " + this.TimeServer);
        this.DestinationTimestamp = DateTime.Now;
      }
      catch (SocketException ex)
      {
        throw new Exception(ex.Message);
      }
      if (!UpdateSystemTime)
        return DateTime.Now.AddMilliseconds(this.LocalClockOffset);
      this.SetTime();
      return DateTime.Now;
    }

    public bool IsResponseValid() => this.SNTPData.Length >= 48 && this.Mode == _Mode.Server;

    public override string ToString()
    {
      string str1 = "Leap Indicator: ";
      switch (this.LeapIndicator)
      {
        case _LeapIndicator.NoWarning:
          str1 += "No warning";
          break;
        case _LeapIndicator.LastMinute61:
          str1 += "Last minute has 61 seconds";
          break;
        case _LeapIndicator.LastMinute59:
          str1 += "Last minute has 59 seconds";
          break;
        case _LeapIndicator.Alarm:
          str1 += "Alarm Condition (clock not synchronized)";
          break;
      }
      string str2 = str1 + "\r\nVersion number: " + this.VersionNumber.ToString() + "\r\n" + "Mode: ";
      switch (this.Mode)
      {
        case _Mode.SymmetricActive:
          str2 += "Symmetric Active";
          break;
        case _Mode.SymmetricPassive:
          str2 += "Symmetric Pasive";
          break;
        case _Mode.Client:
          str2 += "Client";
          break;
        case _Mode.Server:
          str2 += "Server";
          break;
        case _Mode.Broadcast:
          str2 += "Broadcast";
          break;
        case _Mode.Unknown:
          str2 += "Unknown";
          break;
      }
      string str3 = str2 + "\r\nStratum: ";
      switch (this.Stratum)
      {
        case _Stratum.Unspecified:
        case _Stratum.Reserved:
          str3 += "Unspecified";
          break;
        case _Stratum.PrimaryReference:
          str3 += "Primary Reference";
          break;
        case _Stratum.SecondaryReference:
          str3 += "Secondary Reference";
          break;
      }
      string str4 = str3 + "\r\nLocal time: " + this.TransmitTimestamp.ToString();
      double num = this.Precision;
      string str5 = num.ToString();
      string str6 = str4 + "\r\nPrecision: " + str5 + " s" + "\r\nPoll Interval: " + this.PollInterval.ToString() + " s" + "\r\nReference ID: " + this.ReferenceID.ToString();
      num = this.RootDelay;
      string str7 = num.ToString();
      string str8 = str6 + "\r\nRoot Delay: " + str7 + " ms";
      num = this.RootDispersion;
      string str9 = num.ToString();
      string str10 = str8 + "\r\nRoot Dispersion: " + str9 + " ms";
      num = this.RoundTripDelay;
      string str11 = num.ToString();
      string str12 = str10 + "\r\nRound Trip Delay: " + str11 + " ms";
      num = this.LocalClockOffset;
      string str13 = num.ToString();
      return str12 + "\r\nLocal Clock Offset: " + str13 + " ms" + "\r\n";
    }

    [DllImport("kernel32.dll")]
    private static extern bool SetLocalTime(ref InternetTime.SYSTEMTIME time);

    private void SetTime()
    {
      DateTime dateTime = DateTime.Now.AddMilliseconds(this.LocalClockOffset);
      InternetTime.SYSTEMTIME time;
      time.year = (short) dateTime.Year;
      time.month = (short) dateTime.Month;
      time.dayOfWeek = (short) dateTime.DayOfWeek;
      time.day = (short) dateTime.Day;
      time.hour = (short) dateTime.Hour;
      time.minute = (short) dateTime.Minute;
      time.second = (short) dateTime.Second;
      time.milliseconds = (short) dateTime.Millisecond;
      InternetTime.SetLocalTime(ref time);
    }

    private struct SYSTEMTIME
    {
      public short year;
      public short month;
      public short dayOfWeek;
      public short day;
      public short hour;
      public short minute;
      public short second;
      public short milliseconds;
    }
  }
}
