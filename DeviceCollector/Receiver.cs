// Decompiled with JetBrains decompiler
// Type: DeviceCollector.Receiver
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class Receiver
  {
    private DeviceCollectorFunctions MyBus;
    internal string ReceiverVersion = "";
    internal string ReceiverType = "";
    internal int ReceiverLevel = 0;
    public DeviceInfo TheDeviceInfo = (DeviceInfo) null;
    private string RecString;
    private ByteField RecData;
    private int ReceiveCounter;
    private static readonly char[] RotateCharacters = new char[4]
    {
      '-',
      '\\',
      '|',
      '/'
    };
    private int RotateIndex = 0;

    public Receiver(DeviceCollectorFunctions TheBus) => this.MyBus = TheBus;

    internal bool InitCom()
    {
      this.MyBus.MyCom.Close();
      this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, "9600");
      this.MyBus.MyCom.SingleParameter(CommParameter.Parity, "no");
      return this.MyBus.MyCom.Open();
    }

    internal bool ConnectReceiver()
    {
      for (int index = 0; index < 3 && this.InitCom(); ++index)
      {
        Thread.Sleep(1000);
        this.MyBus.MyCom.TransmitBlock("pwr 0\r");
        Thread.Sleep(200);
        this.MyBus.MyCom.ClearCom();
        this.MyBus.MyCom.TransmitBlock("ver\r");
        Thread.Sleep(300);
        this.MyBus.MyCom.ReceiveLine(out this.RecString);
        string[] strArray = this.RecString.Split(' ');
        if (strArray.Length == 2 && strArray[0] == "RS232RCV")
        {
          this.ReceiverType = strArray[0];
          this.ReceiverVersion = strArray[1];
          return true;
        }
      }
      return false;
    }

    internal bool StartReceiver()
    {
      if (!this.ConnectReceiver())
        return false;
      this.MyBus.MyCom.TransmitBlock("pwr 1\r");
      Thread.Sleep(100);
      int num = 97;
      if (this.ReceiverLevel > 0)
        num = this.ReceiverLevel;
      this.MyBus.MyCom.TransmitBlock("pgl " + num.ToString() + "\r");
      this.MyBus.MyCom.ReceiveLine(out string _);
      this.MyBus.MyCom.ClearCom();
      this.ReceiveCounter = 0;
      this.MyBus.BreakRequest = false;
      return true;
    }

    internal bool StopReceiver()
    {
      this.MyBus.BreakRequest = true;
      this.MyBus.MyCom.SetHandshakeState(HandshakeStates.RTS_OFF_DTR_OFF);
      Thread.Sleep(500);
      this.MyBus.MyCom.TransmitBlock("pwr -1\r");
      Thread.Sleep(10);
      this.MyBus.MyCom.TransmitBlock("pwr -1\r");
      return true;
    }

    internal bool ReceiveTelegram()
    {
      int num = 1000;
      while (!this.MyBus.BreakRequest)
      {
        if (!this.MyBus.MyCom.ReceiveLine(out this.RecString))
        {
          if (num++ >= 2)
          {
            num = 0;
            this.MyBus.SendMessage("Receive data[" + this.GetRotateChar().ToString() + "]. Received: ", this.ReceiveCounter, GMM_EventArgs.MessageType.StandardMessage);
          }
          Application.DoEvents();
          Thread.Sleep(100);
        }
        else
        {
          ++this.ReceiveCounter;
          return this.RecString.IndexOf("RSSI-on") > 0 ? this.DecodeTelegramRSSI() : this.DecodeTelegram();
        }
      }
      return false;
    }

    private bool DecodeTelegram()
    {
      int offset = 0;
      int hexByte1 = this.GetHexByte(ref offset);
      int hexByte2 = this.GetHexByte(ref offset);
      int hexByte3 = this.GetHexByte(ref offset);
      int num1 = hexByte3 & 31;
      if (num1 < 1 || hexByte3 >> 5 != hexByte1)
        return false;
      int num2 = 0;
      byte[] ProtocolData = new byte[num1 - 1];
      for (int index = 0; index < ProtocolData.Length; ++index)
      {
        ProtocolData[index] = (byte) this.GetHexByte(ref offset);
        num2 += (int) ProtocolData[index];
      }
      int hexByte4 = this.GetHexByte(ref offset);
      int hexByte5 = this.GetHexByte(ref offset);
      int hexByte6 = this.GetHexByte(ref offset);
      if (this.RecString.Substring(offset) != "OK")
        return false;
      int num3 = num2 + hexByte1 + hexByte2 + hexByte3 + hexByte4 + hexByte5 + hexByte6 & (int) byte.MaxValue;
      int num4 = 0;
      for (int index = 0; index < hexByte2 - 4; ++index)
        num4 += (int) this.RecString[index];
      this.TheDeviceInfo = new DeviceInfo();
      this.TheDeviceInfo.ParameterList = new List<DeviceInfo.MBusParamStruct>();
      this.TheDeviceInfo.LastReadingDate = ParameterService.GetNow();
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RTIME", this.TheDeviceInfo.LastReadingDate.ToString("dd.MM.yyyy HH:mm:ss")));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RCL", hexByte5.ToString()));
      bool flag = false;
      switch (hexByte3)
      {
        case 80:
          flag = this.DecodeTelegramm_0x50(ProtocolData);
          break;
        case 86:
          flag = this.DecodeTelegramm_0x56(ProtocolData);
          break;
        case 90:
          flag = this.DecodeTelegramm_0x5a(ProtocolData);
          break;
      }
      if (!flag)
        return false;
      IdentDevice NewDevice = new IdentDevice(this.MyBus);
      NewDevice.Info = this.TheDeviceInfo;
      this.MyBus.MyDeviceList.AddDevice((object) NewDevice, true);
      return true;
    }

    private bool DecodeTelegramm_0x50(byte[] ProtocolData)
    {
      if (ProtocolData.Length != 15)
        return false;
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", "HCA_R"));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", "48"));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", "ZR_"));
      this.TheDeviceInfo.MeterNumber = MBusDevice.TranslateBcdToBin((long) ProtocolData[0] + ((long) ProtocolData[1] << 8) + ((long) ProtocolData[2] << 16) + ((long) ProtocolData[3] << 24)).ToString();
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", this.TheDeviceInfo.MeterNumber));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("HCA", ((uint) ((int) ProtocolData[4] + ((int) ProtocolData[5] << 8) + ((int) ProtocolData[6] << 16) + ((int) ProtocolData[7] << 24))).ToString("d08")));
      int num = (int) ProtocolData[12] + ((int) ProtocolData[13] << 8);
      if (num > 0)
      {
        int day = num & 31;
        int month = (num & 480) >> 5;
        int year = ((num & 65024) >> 9) + 2000;
        try
        {
          this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("TIMP[8]", new DateTime(year, month, day).ToString("dd.MM.yyyy")));
        }
        catch
        {
        }
        this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("HCA[8]", ((long) ProtocolData[8] + ((long) ProtocolData[9] << 8) + ((long) ProtocolData[10] << 16) + ((long) ProtocolData[11] << 24)).ToString("d08")));
      }
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("FLAGS", ProtocolData[14].ToString("x02")));
      return true;
    }

    private double GetDouble(uint Value)
    {
      uint num1 = Value & 8388607U;
      uint num2 = (Value & 2139095040U) >> 23;
      uint num3 = Value & 2147483648U;
      double y = (double) num2 - 128.0;
      double num4 = ((double) num1 / 8388608.0 + 1.0) * Math.Pow(2.0, y);
      if (num3 > 0U)
        num4 *= -1.0;
      return num4;
    }

    private bool DecodeTelegramm_0x56(byte[] ProtocolData)
    {
      if (ProtocolData.Length != 21)
        return false;
      this.TheDeviceInfo.MeterNumber = MBusDevice.TranslateBcdToBin((long) ProtocolData[3] + ((long) ProtocolData[2] << 8) + ((long) ProtocolData[1] << 16) + ((long) ProtocolData[0] << 24)).ToString();
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", this.TheDeviceInfo.MeterNumber));
      this.TheDeviceInfo.Medium = ProtocolData[4];
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", MBusDevice.GetMediaString(this.TheDeviceInfo.Medium)));
      this.TheDeviceInfo.Version = ProtocolData[5];
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", this.TheDeviceInfo.Version.ToString()));
      this.TheDeviceInfo.Manufacturer = MBusDevice.GetManufacturer((short) ((int) ProtocolData[6] + ((int) ProtocolData[7] << 8)));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", this.TheDeviceInfo.Manufacturer));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("TYPE", ((long) ProtocolData[8] + ((long) ProtocolData[9] << 8) + ((long) ProtocolData[10] << 16) + ((long) ProtocolData[11] << 24)).ToString("x08")));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("VARIANCE", ((int) ProtocolData[12] + ((int) ProtocolData[13] << 8)).ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("CYCLE", ((int) ProtocolData[14] + ((int) ProtocolData[15] << 8)).ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("START_H", ProtocolData[16].ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("STOP_H", ProtocolData[17].ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("T_MASK", ((int) ProtocolData[18] + ((int) ProtocolData[19] << 8) + ((int) ProtocolData[20] << 16)).ToString("x06")));
      return true;
    }

    private bool DecodeTelegramm_0x5a(byte[] ProtocolData)
    {
      if (ProtocolData.Length != 25)
        return false;
      this.TheDeviceInfo.MeterNumber = MBusDevice.TranslateBcdToBin((long) ProtocolData[0] + ((long) ProtocolData[1] << 8) + ((long) ProtocolData[2] << 16) + ((long) ProtocolData[3] << 24)).ToString();
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", this.TheDeviceInfo.MeterNumber));
      this.TheDeviceInfo.Medium = ProtocolData[4];
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", MBusDevice.GetMediaString(this.TheDeviceInfo.Medium) + "_R"));
      this.TheDeviceInfo.Version = ProtocolData[5];
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", this.TheDeviceInfo.Version.ToString()));
      this.TheDeviceInfo.Manufacturer = MBusDevice.GetManufacturer((short) ((int) ProtocolData[6] + ((int) ProtocolData[7] << 8)));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", this.TheDeviceInfo.Manufacturer));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("TYPE", ((long) ProtocolData[8] + ((long) ProtocolData[9] << 8) + ((long) ProtocolData[10] << 16) + ((long) ProtocolData[11] << 24)).ToString("x08")));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("VARIANCE", ((int) ProtocolData[12] + ((int) ProtocolData[13] << 8)).ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("CYCLE", ((int) ProtocolData[14] + ((int) ProtocolData[15] << 8)).ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("START_H", ProtocolData[16].ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("STOP_H", ProtocolData[17].ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("T_MASK", ((int) ProtocolData[18] + ((int) ProtocolData[19] << 8) + ((int) ProtocolData[20] << 16)).ToString("x06")));
      uint num1 = (uint) ((int) ProtocolData[21] + ((int) ProtocolData[23] << 8) + ((int) ProtocolData[22] << 16) + ((int) ProtocolData[24] << 24));
      int second = (int) num1 & 63;
      int minute = (int) ((num1 & 16515072U) >> 18);
      int hour = (int) ((num1 & 253952U) >> 13);
      int day = (int) ((num1 & 7936U) >> 8);
      int month = (int) ((num1 & 4026531840U) >> 28);
      int num2 = (int) ((num1 & 251658240U) >> 24);
      int year1 = SystemValues.DateTimeNow.Year;
      int num3 = year1 / 10 * 10;
      int year2 = num2 + num3;
      if (year2 - year1 > 5)
        year2 -= 10;
      if (year2 - year1 < -5)
        year2 += 10;
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("TIMP", new DateTime(year2, month, day, hour, minute, second).ToString("dd.MM.yyyy HH:mm:ss")));
      return true;
    }

    private bool DecodeTelegramRSSI()
    {
      this.RecData = new ByteField(32);
      int offset = 0;
      for (int index = 0; index < 16; ++index)
      {
        int hexByte = this.GetHexByte(ref offset);
        if (hexByte < 0)
          return false;
        this.RecData.Add(hexByte);
      }
      int num1 = this.RecString.IndexOf("RSSI-on");
      if (num1 <= 0)
        return false;
      int startIndex = num1 + 7;
      int num2 = this.RecString.IndexOf("OK", startIndex);
      if (num2 <= 0)
        return false;
      int num3 = int.Parse(this.RecString.Substring(startIndex, num2 - startIndex));
      if (num3 < 0)
        return false;
      this.TheDeviceInfo = new DeviceInfo();
      this.TheDeviceInfo.ParameterList = new List<DeviceInfo.MBusParamStruct>();
      this.TheDeviceInfo.LastReadingDate = ParameterService.GetNow();
      this.TheDeviceInfo.Manufacturer = "ZR_";
      this.TheDeviceInfo.Medium = (byte) 7;
      this.TheDeviceInfo.MeterNumber = this.GetParam(2, 4).ToString();
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RTIME", this.TheDeviceInfo.LastReadingDate.ToString("dd.MM.yyyy HH:mm:ss")));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", this.TheDeviceInfo.MeterNumber));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", this.TheDeviceInfo.Manufacturer));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", this.TheDeviceInfo.Version.ToString()));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", MBusDevice.GetMediaString(this.TheDeviceInfo.Medium)));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MPC", this.GetParam(1, 1).ToString()));
      StringBuilder PValue = new StringBuilder(this.GetParam(6, 4).ToString(), 30);
      MBusDevice.SetStringExpo(ref PValue, -2);
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("CNT", PValue.ToString()));
      PValue.Length = 0;
      PValue.Append(this.GetParam(10, 4).ToString());
      MBusDevice.SetStringExpo(ref PValue, -2);
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("CNT[1]", PValue.ToString()));
      TimeSpan timeSpan1 = new TimeSpan(0, 0, (int) this.GetParam(14, 2), 0);
      DateTime dateTime1;
      ref DateTime local = ref dateTime1;
      DateTime dateTimeNow = SystemValues.DateTimeNow;
      int year = dateTimeNow.Year;
      dateTimeNow = SystemValues.DateTimeNow;
      int month = dateTimeNow.Month;
      local = new DateTime(year, month, 1);
      DateTime dateTime2 = dateTime1.AddMonths(-1);
      DateTime dateTime3 = dateTime1.AddMonths(1);
      dateTime2 = dateTime2.Add(timeSpan1);
      dateTime1 = dateTime1.Add(timeSpan1);
      DateTime dateTime4 = dateTime3.Add(timeSpan1);
      TimeSpan timeSpan2 = SystemValues.DateTimeNow - dateTime2;
      TimeSpan timeSpan3 = SystemValues.DateTimeNow - dateTime1;
      TimeSpan timeSpan4 = SystemValues.DateTimeNow - dateTime4;
      timeSpan2 = timeSpan2.Duration();
      timeSpan3 = timeSpan3.Duration();
      timeSpan4 = timeSpan4.Duration();
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("TIMP", (!(timeSpan2 < timeSpan3) ? (!(timeSpan4 < timeSpan3) ? dateTime1 : dateTime4) : (!(timeSpan2 < timeSpan4) ? dateTime4 : dateTime2)).ToString("dd.MM.yyyy HH:mm:ss")));
      this.TheDeviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RCL", num3.ToString()));
      IdentDevice NewDevice = new IdentDevice(this.MyBus);
      NewDevice.Info = this.TheDeviceInfo;
      this.MyBus.MyDeviceList.AddDevice((object) NewDevice, true);
      return true;
    }

    private uint GetParam(int BytePosition, int ByteSize)
    {
      uint num = 0;
      for (int index = 0; index < ByteSize; ++index)
        num = (num << 8) + (uint) this.RecData.Data[BytePosition + index];
      return num;
    }

    private int GetHexByte(ref int offset)
    {
      while (this.RecString.Length > offset && this.RecString[offset] == ' ')
        ++offset;
      int hexByte = -1;
      if (this.RecString.Length > offset)
      {
        hexByte = this.GetHexValue(this.RecString[offset++]);
        if (hexByte < 0)
          return -1;
      }
      if (this.RecString.Length <= offset)
        return -1;
      if (this.RecString[offset] == ' ')
      {
        ++offset;
        return hexByte;
      }
      int hexValue = this.GetHexValue(this.RecString[offset++]);
      return hexValue < 0 ? -1 : hexValue + (hexByte << 4);
    }

    private int GetHexValue(char CharIn)
    {
      CharIn = char.ToUpper(CharIn);
      if (CharIn < '0')
        return -1;
      if (CharIn <= '9')
        return (int) CharIn - 48;
      return CharIn < 'A' || CharIn > 'F' ? -1 : (int) CharIn - 65 + 10;
    }

    private char GetRotateChar()
    {
      if (this.RotateIndex > 3)
        this.RotateIndex = 0;
      return Receiver.RotateCharacters[this.RotateIndex++];
    }
  }
}
