// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.LoRaEventData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public class LoRaEventData
  {
    public static List<KeyValuePair<string, int>> PrintColumns = new List<KeyValuePair<string, int>>();
    public DateTime PC_TimeAtDeviceRead;
    public DateTime DeviceTimeAtDeviceRead;
    public DateTime NextSendTime;
    public LoRaPackets NextPacket;
    public byte NextPacketNumber;
    public int SumOfShiftSeconds;
    public string NextPacketInfo;
    private ChannelLogger NLog_cl;

    static LoRaEventData()
    {
      LoRaEventData.PrintColumns.Add(new KeyValuePair<string, int>("Device time", 20));
      LoRaEventData.PrintColumns.Add(new KeyValuePair<string, int>("Next LoRa time", 20));
      LoRaEventData.PrintColumns.Add(new KeyValuePair<string, int>("Packet", 18));
      LoRaEventData.PrintColumns.Add(new KeyValuePair<string, int>("Nr", 4));
      LoRaEventData.PrintColumns.Add(new KeyValuePair<string, int>("TimeSpan", 9));
    }

    public LoRaEventData(ChannelLogger nLog_cl) => this.NLog_cl = nLog_cl;

    public void SetBy_radio_transmitter(
      DateTime deviceTime,
      byte[] radio_transmitter_Array,
      bool onlyNLog = false)
    {
      try
      {
        byte radioTransmitter1 = radio_transmitter_Array[0];
        LoRaPackets radioTransmitter2 = (LoRaPackets) radio_transmitter_Array[1];
        byte radioTransmitter3 = radio_transmitter_Array[5];
        byte radioTransmitter4 = radio_transmitter_Array[6];
        byte radioTransmitter5 = radio_transmitter_Array[7];
        byte radioTransmitter6 = radio_transmitter_Array[8];
        this.NLog_cl.Trace("transmitter-> data,day,hour,minute,sedond,nextPacket: " + radioTransmitter1.ToString() + "," + radioTransmitter3.ToString() + "," + radioTransmitter4.ToString() + "," + radioTransmitter5.ToString() + "," + radioTransmitter6.ToString() + "," + radioTransmitter2.ToString());
        if (onlyNLog)
          return;
        this.SumOfShiftSeconds = 0;
        this.PC_TimeAtDeviceRead = DateTime.Now;
        this.DeviceTimeAtDeviceRead = deviceTime;
        this.NextSendTime = new DateTime(this.DeviceTimeAtDeviceRead.Year, this.DeviceTimeAtDeviceRead.Month, this.DeviceTimeAtDeviceRead.Day, (int) radioTransmitter4, (int) radioTransmitter5, (int) radioTransmitter6);
        this.NextPacket = radioTransmitter2;
        this.NextPacketNumber = (byte) 0;
        if (this.NextSendTime < this.DeviceTimeAtDeviceRead)
          this.NextSendTime = this.NextSendTime.AddDays(1.0);
        this.NextPacketInfo = this.NextPacket.ToString();
      }
      catch (Exception ex)
      {
        throw new Exception("LoRa data not defined.", ex);
      }
    }

    public void SetBy_time_to_send_data(DateTime deviceTime, byte[] time_to_send_data_Array)
    {
      try
      {
        byte timeToSendData1 = time_to_send_data_Array[0];
        byte timeToSendData2 = time_to_send_data_Array[1];
        byte timeToSendData3 = time_to_send_data_Array[2];
        byte timeToSendData4 = time_to_send_data_Array[3];
        LoRaPackets timeToSendData5 = (LoRaPackets) time_to_send_data_Array[20];
        byte timeToSendData6 = time_to_send_data_Array[21];
        this.NLog_cl.Trace("sendData-> day,hour,minute,sedond,nextPacket,packetNumber: " + timeToSendData1.ToString() + "," + timeToSendData2.ToString() + "," + timeToSendData3.ToString() + "," + timeToSendData4.ToString() + "," + timeToSendData5.ToString() + "," + timeToSendData6.ToString());
        this.SumOfShiftSeconds = 0;
        this.PC_TimeAtDeviceRead = DateTime.Now;
        this.DeviceTimeAtDeviceRead = deviceTime;
        this.NextSendTime = new DateTime(this.DeviceTimeAtDeviceRead.Year, this.DeviceTimeAtDeviceRead.Month, this.DeviceTimeAtDeviceRead.Day, (int) timeToSendData2, (int) timeToSendData3, (int) timeToSendData4);
        this.NextPacket = timeToSendData5;
        this.NextPacketNumber = timeToSendData6;
        if (this.NextSendTime < this.DeviceTimeAtDeviceRead)
        {
          this.NextSendTime = this.NextSendTime.AddDays(1.0);
          this.NLog_cl.Trace("Add one day");
        }
        if (this.NextSendTime.Date > this.DeviceTimeAtDeviceRead.Date)
        {
          this.NLog_cl.Trace("Insert midnight stop");
          this.NextSendTime = new DateTime(this.DeviceTimeAtDeviceRead.Year, this.DeviceTimeAtDeviceRead.Month, this.DeviceTimeAtDeviceRead.Day).AddDays(1.0);
          this.NextPacketInfo = "-> Midnight stop";
        }
        else
          this.NextPacketInfo = this.NextPacket.ToString();
      }
      catch (Exception ex)
      {
        throw new Exception("LoRa data not defined.", ex);
      }
    }

    private TimeSpan GetTimeSpanToNextEvent()
    {
      return this.NextSendTime.Subtract(this.DeviceTimeAtDeviceRead).Subtract(new TimeSpan(0, 0, this.SumOfShiftSeconds));
    }

    public DateTime GetCalculatedDeviceTime()
    {
      return this.DeviceTimeAtDeviceRead.Add(DateTime.Now.Subtract(this.PC_TimeAtDeviceRead).Add(new TimeSpan(0, 0, this.SumOfShiftSeconds)));
    }

    public uint GetTimeShiftSecondsToNextDeviceHour(int waitSeconds)
    {
      DateTime calculatedDeviceTime = this.GetCalculatedDeviceTime();
      TimeSpan timeSpan = calculatedDeviceTime.Date.AddHours((double) (calculatedDeviceTime.Hour + 1)).Subtract(calculatedDeviceTime).Subtract(new TimeSpan(0, 0, 5));
      return timeSpan.TotalSeconds > (double) waitSeconds ? (uint) timeSpan.TotalSeconds : 0U;
    }

    public string GetHeader()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, int> printColumn in LoRaEventData.PrintColumns)
        stringBuilder.Append(printColumn.Key.PadLeft(printColumn.Value));
      return stringBuilder.ToString();
    }

    public string GetLine()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      int num1 = 0;
      StringBuilder stringBuilder2 = stringBuilder1;
      string str1 = this.DeviceTimeAtDeviceRead.ToString("dd.MM.yyyy HH:mm:ss");
      List<KeyValuePair<string, int>> printColumns1 = LoRaEventData.PrintColumns;
      int index1 = num1;
      int num2 = index1 + 1;
      int totalWidth1 = printColumns1[index1].Value;
      string str2 = str1.PadLeft(totalWidth1);
      stringBuilder2.Append(str2);
      StringBuilder stringBuilder3 = stringBuilder1;
      string str3 = this.NextSendTime.ToString("dd.MM.yyyy HH:mm:ss");
      List<KeyValuePair<string, int>> printColumns2 = LoRaEventData.PrintColumns;
      int index2 = num2;
      int num3 = index2 + 1;
      int totalWidth2 = printColumns2[index2].Value;
      string str4 = str3.PadLeft(totalWidth2);
      stringBuilder3.Append(str4);
      StringBuilder stringBuilder4 = stringBuilder1;
      string nextPacketInfo = this.NextPacketInfo;
      List<KeyValuePair<string, int>> printColumns3 = LoRaEventData.PrintColumns;
      int index3 = num3;
      int num4 = index3 + 1;
      int totalWidth3 = printColumns3[index3].Value;
      string str5 = nextPacketInfo.PadLeft(totalWidth3);
      stringBuilder4.Append(str5);
      StringBuilder stringBuilder5 = stringBuilder1;
      string str6 = this.NextPacketNumber.ToString();
      List<KeyValuePair<string, int>> printColumns4 = LoRaEventData.PrintColumns;
      int index4 = num4;
      int num5 = index4 + 1;
      int totalWidth4 = printColumns4[index4].Value;
      string str7 = str6.PadLeft(totalWidth4);
      stringBuilder5.Append(str7);
      StringBuilder stringBuilder6 = stringBuilder1;
      string str8 = this.GetTimeSpanToNextEvent().ToString();
      List<KeyValuePair<string, int>> printColumns5 = LoRaEventData.PrintColumns;
      int index5 = num5;
      int num6 = index5 + 1;
      int totalWidth5 = printColumns5[index5].Value;
      string str9 = str8.PadLeft(totalWidth5);
      stringBuilder6.Append(str9);
      return stringBuilder1.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Device time: ........... " + this.DeviceTimeAtDeviceRead.ToString("yyyy.MM.dd HH:mm:ss.sss"));
      stringBuilder.AppendLine("Next LoRa event time: .. " + this.NextSendTime.ToString("yyyy.MM.dd HH:mm:ss.sss"));
      stringBuilder.AppendLine("Next packet: ........... " + this.NextPacketInfo);
      stringBuilder.AppendLine("Next packet number: .... " + this.NextPacketNumber.ToString());
      stringBuilder.AppendLine("Time span to next event: " + this.GetTimeSpanToNextEvent().ToString());
      return stringBuilder.ToString();
    }
  }
}
