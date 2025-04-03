// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonRadioCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public sealed class CommonRadioCommands : IZRCommand
  {
    private static Logger logger = LogManager.GetLogger(nameof (CommonRadioCommands));
    public const ushort DefaultSyncWord = 37331;
    private Common32BitCommands commonCMD;
    private bool crypt = false;
    private string AESKey = (string) null;

    public static byte[] DefaultAbitraryData { get; private set; }

    public bool enDeCrypt
    {
      get => this.crypt;
      set
      {
        this.crypt = value;
        if (this.commonCMD == null)
          return;
        this.commonCMD.enDeCrypt = value;
      }
    }

    public string AES_Key
    {
      get => this.AESKey;
      set
      {
        this.AESKey = value;
        if (this.commonCMD == null)
          return;
        this.commonCMD.AES_Key = value;
      }
    }

    static CommonRadioCommands()
    {
      CommonRadioCommands.DefaultAbitraryData = new byte[28];
      for (int index = 0; index < CommonRadioCommands.DefaultAbitraryData.Length; ++index)
        CommonRadioCommands.DefaultAbitraryData[index] = (byte) 85;
    }

    public CommonRadioCommands(Common32BitCommands commonCMD)
    {
      this.commonCMD = commonCMD;
      this.setCryptValuesFromBaseClass();
    }

    public void setCryptValuesFromBaseClass()
    {
      this.enDeCrypt = this.commonCMD.enDeCrypt;
      this.AES_Key = this.commonCMD.AES_Key;
    }

    public async Task<ushort> GetRadioVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte FC = 47;
      byte EFC = 0;
      byte[] resultData = await this.commonCMD.TransmitAndReceiveVersionData(FC, EFC, progress, cancelToken);
      if (resultData.Length != 4)
        throw new Exception("Illegal result length by GetRadioVersionAsync");
      if ((int) resultData[0] != (int) FC || (int) resultData[1] != (int) EFC)
        throw new Exception("Illegal FC,EFC by GetRadioVersionAsync");
      ushort uint16 = BitConverter.ToUInt16(resultData, 2);
      resultData = (byte[]) null;
      return uint16;
    }

    public async Task<ushort> GetTransmitPowerAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetTransmitPower_0x05, progress, cancelToken);
      ushort uint16 = BitConverter.ToUInt16(theData, 0);
      theData = (byte[]) null;
      return uint16;
    }

    public async Task SetTransmitPowerAsync(
      ushort transmitPower,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetTransmitPower_0x05, BitConverter.GetBytes(transmitPower), progress, cancelToken);
    }

    public async Task<uint> GetCenterFrequencyAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetCenterFrequency_0x06, progress, cancelToken);
      uint uint32 = BitConverter.ToUInt32(theData, 0);
      theData = (byte[]) null;
      return uint32;
    }

    public async Task SetCenterFrequencyAsync(
      uint frequency,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetCenterFrequency_0x06, BitConverter.GetBytes(frequency), progress, cancelToken);
    }

    public async Task<ushort> GetFrequencyDeviationAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetFrequencyDeviation_0x09, progress, cancelToken);
      ushort uint16 = BitConverter.ToUInt16(theData, 0);
      theData = (byte[]) null;
      return uint16;
    }

    public async Task SetFrequencyDeviationAsync(
      ushort frequencyDeviation,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetFrequencyDeviation_0x09, BitConverter.GetBytes(frequencyDeviation), progress, cancelToken);
    }

    public async Task<byte> GetCarrierModeAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetCarrierMode_0x08, progress, cancelToken);
      byte carrierModeAsync = theData[0];
      theData = (byte[]) null;
      return carrierModeAsync;
    }

    public async Task SetCarrierModeAsync(
      byte mode,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetCarrierMode_0x08, new byte[1]
      {
        mode
      }, progress, cancelToken);
    }

    public async Task<int> GetFrequencyIncrementAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetFrequencyIncrement_0x07, progress, cancelToken);
      int int32 = BitConverter.ToInt32(theData, 0);
      theData = (byte[]) null;
      return int32;
    }

    public async Task SetFrequencyIncrementAsync(
      int frequency_Hz,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetFrequencyIncrement_0x07, BitConverter.GetBytes(frequency_Hz), progress, cancelToken);
    }

    public async Task MonitorRadioAsync(
      ushort timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.MonitorRadioToOutput_0x26, BitConverter.GetBytes(timeout), progress, cancelToken);
    }

    public async Task ReceiveAndStreamRadio3Scenario3TelegramsAsync(
      byte telegramSize,
      ushort timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<byte> cmd = new List<byte>();
      cmd.Add(telegramSize);
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeout));
      byte[] parameter = cmd.ToArray();
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.ReceiveAndStreamR3S3_Telegrams_0x25, parameter, progress, cancelToken);
      cmd = (List<byte>) null;
      parameter = (byte[]) null;
    }

    public async Task<double?> ReceiveTestPacketAsync(
      byte timeoutInSec,
      uint deviceID,
      ProgressHandler progress,
      CancellationToken token,
      string syncWord)
    {
      if (syncWord == null || syncWord.Length != 4)
        throw new Exception("Invalid SYNC WORD!");
      if (syncWord == "0000" || syncWord == "5555" || syncWord == "AAAA")
        throw new Exception("SYNC WORD " + syncWord + " is not allowed");
      DeviceVersionMBus deviceVersionMbus = await this.commonCMD.DeviceCMD.ReadVersionAsync(progress, token);
      byte SYNC1 = Convert.ToByte(syncWord.Substring(0, 2), 16);
      byte SYNC2 = Convert.ToByte(syncWord.Substring(2, 2), 16);
      DateTime start = DateTime.Now;
      DateTime end = start.AddSeconds((double) timeoutInSec);
      while (DateTime.Now <= end)
      {
        try
        {
          byte[] buffer = await this.ReceiveRadio3Scenario3TelegramViaRadioAsync((byte) 33, new byte[2]
          {
            SYNC1,
            SYNC2
          }, timeoutInSec, progress, token);
          if (buffer != null && buffer.Length != 0)
          {
            string data = Encoding.ASCII.GetString(buffer, 3, buffer.Length - 3);
            if (data.StartsWith("TEST_PACKET_") && data.EndsWith("_TEST_PACKET"))
            {
              string id = data.Substring(12, 8).TrimStart('0');
              if ((int) uint.Parse(id) == (int) deviceID)
              {
                CommonRadioCommands.logger.Trace(Utility.ByteArrayToHexString(buffer));
                byte RegRssiValue = buffer[0];
                double rssi = (double) -RegRssiValue / 2.0;
                return new double?(rssi);
              }
              data = (string) null;
              id = (string) null;
            }
            else
              continue;
          }
          buffer = (byte[]) null;
        }
        catch (Exception ex)
        {
          CommonRadioCommands.logger.Error(ex.Message);
        }
      }
      return new double?();
    }

    public double? ReceiveTestPacket(
      byte timeoutInSec,
      uint deviceID,
      ProgressHandler progress,
      CancellationToken token,
      string syncWord)
    {
      if (syncWord == null || syncWord.Length != 4)
        throw new Exception("Invalid SYNC WORD!");
      if (syncWord == "0000" || syncWord == "5555" || syncWord == "AAAA")
        throw new Exception("SYNC WORD " + syncWord + " is not allowed");
      this.commonCMD.DeviceCMD.ReadVersion(progress, token);
      byte num1 = Convert.ToByte(syncWord.Substring(0, 2), 16);
      byte num2 = Convert.ToByte(syncWord.Substring(2, 2), 16);
      DateTime dateTime = DateTime.Now.AddSeconds((double) timeoutInSec);
      while (DateTime.Now <= dateTime)
      {
        try
        {
          byte[] telegramViaRadio = this.ReceiveRadio3Scenario3TelegramViaRadio((byte) 33, new byte[2]
          {
            num1,
            num2
          }, timeoutInSec, progress, token);
          if (telegramViaRadio != null && telegramViaRadio.Length != 0)
          {
            string str = Encoding.ASCII.GetString(telegramViaRadio, 3, telegramViaRadio.Length - 3);
            if (str.StartsWith("TEST_PACKET_") && str.EndsWith("_TEST_PACKET"))
            {
              if ((int) uint.Parse(str.Substring(12, 8).TrimStart('0')) == (int) deviceID)
              {
                CommonRadioCommands.logger.Trace(Utility.ByteArrayToHexString(telegramViaRadio));
                return new double?((double) -telegramViaRadio[0] / 2.0);
              }
            }
          }
        }
        catch (Exception ex)
        {
          CommonRadioCommands.logger.Error(ex.Message);
        }
      }
      return new double?();
    }

    public byte[] ReceiveRadio3Scenario3TelegramViaRadio(
      byte telegramSize,
      byte[] syncWord,
      byte timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonRadioCommands.logger.Debug("ReceiveRadio3Scenario3TelegramViaRadioAsync: SyncWord: " + Utility.ByteArrayToHexString(syncWord) + ", Timeout: " + timeout.ToString());
      int num1 = 0;
      int num2 = 0;
      ConfigList readoutConfiguration = this.commonCMD.DeviceCMD.MBus.Repeater.Port.GetReadoutConfiguration();
      try
      {
        num1 = readoutConfiguration.RecTime_BeforFirstByte;
        num2 = readoutConfiguration.MaxRequestRepeat;
        readoutConfiguration.RecTime_BeforFirstByte = (int) timeout * 1000 + 300;
        readoutConfiguration.MaxRequestRepeat = 1;
        CommonRadioCommands.logger.Debug("Set new value for RecTime_BeforFirstByte: " + readoutConfiguration.RecTime_BeforFirstByte.ToString());
        List<byte> byteList = new List<byte>();
        byteList.Add(telegramSize);
        byteList.AddRange((IEnumerable<byte>) syncWord);
        byteList.Add(timeout);
        return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.ReceiveOneR3S3_Telegram_0x24, byteList.ToArray(), progress, cancelToken);
      }
      finally
      {
        CommonRadioCommands.logger.Debug("Set old value for RecTime_BeforFirstByte: " + num1.ToString());
        readoutConfiguration.RecTime_BeforFirstByte = num1;
        readoutConfiguration.MaxRequestRepeat = num2;
      }
    }

    public async Task<byte[]> ReceiveRadio3Scenario3TelegramViaRadioAsync(
      byte telegramSize,
      byte[] syncWord,
      byte timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonRadioCommands.logger.Debug("ReceiveRadio3Scenario3TelegramViaRadioAsync: SyncWord: " + Utility.ByteArrayToHexString(syncWord) + ", Timeout: " + timeout.ToString());
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] telegramViaRadioAsync;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = (int) timeout * 1000 + 300;
        cfgList.MaxRequestRepeat = 1;
        CommonRadioCommands.logger.Debug("Set new value for RecTime_BeforFirstByte: " + cfgList.RecTime_BeforFirstByte.ToString());
        List<byte> cmd = new List<byte>();
        cmd.Add(telegramSize);
        cmd.AddRange((IEnumerable<byte>) syncWord);
        cmd.Add(timeout);
        byte[] parameter = cmd.ToArray();
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.ReceiveOneR3S3_Telegram_0x24, parameter, progress, cancelToken);
        telegramViaRadioAsync = theData;
      }
      finally
      {
        CommonRadioCommands.logger.Debug("Set old value for RecTime_BeforFirstByte: " + oldTimeout.ToString());
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return telegramViaRadioAsync;
    }

    public async Task EchoRadio3TelegramViaRadioAsync(
      byte telegramSize,
      byte[] syncWord,
      ushort timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (syncWord == null)
        throw new ArgumentNullException("EchoRadio3TelegramViaRadioAsync::syncWord");
      if (syncWord.Length != 2)
        throw new ArgumentException("syncWord.Length != 2");
      List<byte> cmd = new List<byte>();
      cmd.Add(telegramSize);
      cmd.AddRange((IEnumerable<byte>) syncWord);
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeout));
      byte[] parameter = cmd.ToArray();
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.EchoRadio3viaRadio_0x27, parameter, progress, cancelToken);
      cmd = (List<byte>) null;
      parameter = (byte[]) null;
    }

    public async Task SendTestPacketAsync(
      ushort interval,
      ushort timeout,
      uint deviceID,
      byte[] arbitraryData,
      ProgressHandler progress,
      CancellationToken token,
      string syncWordString)
    {
      ushort syncWord = syncWordString != null && syncWordString.Length == 4 ? ushort.Parse(syncWordString, NumberStyles.HexNumber) : throw new Exception("Invalid SYNC WORD!");
      byte[] packetBytes = CommonRadioCommands.GetSendTestPacketData(interval, timeout, deviceID, arbitraryData, syncWord);
      await this.SendTestPacketAsync(interval, timeout, packetBytes, progress, token);
      packetBytes = (byte[]) null;
    }

    public static byte[] GetSendTestPacketData(
      ushort interval,
      ushort timeout,
      uint deviceID,
      byte[] arbitraryData,
      ushort syncWord)
    {
      if (arbitraryData == null)
        throw new ArgumentNullException("myDeviceCommands");
      if (syncWord == (ushort) 0 || syncWord == (ushort) 21845 || syncWord == (ushort) 43690)
        throw new Exception("SYNC WORD " + syncWord.ToString("x04") + " is not allowed");
      DateTime now = DateTime.Now;
      byte hour = (byte) now.Hour;
      byte minute = (byte) now.Minute;
      ushort yyyymmmmyyyddddd = MBusUtil.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(now);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) ((uint) syncWord >> 8));
      byteList.Add((byte) syncWord);
      byteList.Add((byte) 38);
      byteList.Add((byte) 1);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(Util.ConvertUnt32ToBcdUInt32(deviceID)));
      byteList.Add((byte) 0);
      byteList.Add(hour);
      byteList.Add(minute);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(yyyymmmmyyyddddd));
      byteList.AddRange((IEnumerable<byte>) arbitraryData);
      byte[] array = byteList.ToArray();
      if (CommonRadioCommands.logger.IsDebugEnabled)
        CommonRadioCommands.logger.Debug(string.Format("SendTestPacket => Interval:{0}, Timeout:{1} Data: {2}", (object) interval, (object) timeout, (object) Utility.ByteArrayToHexString(array)));
      return array;
    }

    public async Task SendTestPacketAsync(
      ushort interval,
      ushort timeout,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<byte> cmd = new List<byte>();
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(interval));
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeout));
      cmd.AddRange((IEnumerable<byte>) data);
      byte[] parameter = cmd.ToArray();
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.SendTestPacket_0x23, parameter, progress, cancelToken);
      cmd = (List<byte>) null;
      parameter = (byte[]) null;
    }

    public async Task TransmitModulatedCarrierAsync(
      ushort timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.TransmitModulatedCarrier_0x22, BitConverter.GetBytes(timeout), progress, cancelToken);
    }

    public async Task TransmitUnmodulatedCarrierAsync(
      ushort timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.TransmitUnmodulatedCarrier_0x21, BitConverter.GetBytes(timeout), progress, cancelToken);
    }

    public async Task StopRadioTests(ProgressHandler progress, CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.StopRadioTest_0x20, progress, cancelToken);
    }

    public async Task<uint> GetTxDataRateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetTxDataRate_0x0b, progress, cancelToken);
      uint uint32 = BitConverter.ToUInt32(theData, 0);
      theData = (byte[]) null;
      return uint32;
    }

    public async Task SetTxDataRateAsync(
      uint dataRate,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetTxDataRate_0x0b, BitConverter.GetBytes(dataRate), progress, cancelToken);
    }

    public async Task<uint> GetRxDataRateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetRxDataRate_0x0c, progress, cancelToken);
      uint uint32 = BitConverter.ToUInt32(theData, 0);
      theData = (byte[]) null;
      return uint32;
    }

    public async Task SetRxDataRateAsync(
      uint dataRate,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetRxDataRate_0x0c, BitConverter.GetBytes(dataRate), progress, cancelToken);
    }

    public async Task<CommonRadioCommands.RadioBandWidth> GetBandWidthAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetBandWidth_0x0a, progress, cancelToken);
      CommonRadioCommands.RadioBandWidth retVal = new CommonRadioCommands.RadioBandWidth();
      retVal.BandWidth = BitConverter.ToUInt32(theData, 0);
      retVal.AFC = BitConverter.ToUInt32(theData, 4);
      retVal.basedata = theData;
      CommonRadioCommands.RadioBandWidth bandWidthAsync = retVal;
      theData = (byte[]) null;
      retVal = (CommonRadioCommands.RadioBandWidth) null;
      return bandWidthAsync;
    }

    public async Task SetBandWidthAsync(
      uint bandWidth,
      uint AFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<byte> cmd = new List<byte>();
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(bandWidth));
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(AFC));
      byte[] parameter = cmd.ToArray();
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetBandWidth_0x0a, parameter, progress, cancelToken);
      cmd = (List<byte>) null;
      parameter = (byte[]) null;
    }

    public async Task<ushort> GetTxBandWidthAsync(ProgressHandler progress, CancellationToken token)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetTxBandWidth_0x28, progress, token);
      ushort uint16 = BitConverter.ToUInt16(theData, 0);
      theData = (byte[]) null;
      return uint16;
    }

    public async Task SetTxBandWidthAsync(
      ushort bandWidth,
      ProgressHandler progress,
      CancellationToken token)
    {
      List<byte> cmd = new List<byte>();
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(bandWidth));
      byte[] parameter = cmd.ToArray();
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.GetSetTxBandWidth_0x28, parameter, progress, token);
      cmd = (List<byte>) null;
      parameter = (byte[]) null;
    }

    public async Task StartTransmissionCycleAsync(
      byte numberOfFirstChannel,
      byte totalChannels,
      byte lenghtOfPayload,
      byte cycles,
      byte spreadFactor,
      ushort bandwidth,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (numberOfFirstChannel > (byte) 71 || numberOfFirstChannel < (byte) 0)
        throw new Exception("Number of first channel has to be between 0 and 71.");
      if (totalChannels > (byte) 72 || totalChannels < (byte) 1)
        throw new Exception("Number of total channels has to be between 1 and 72.");
      if (lenghtOfPayload > (byte) 60 || lenghtOfPayload < (byte) 1)
        throw new Exception("Length of payload has to be between 1 and 60.");
      List<byte> cmd = new List<byte>();
      cmd.Add(numberOfFirstChannel);
      cmd.Add(totalChannels);
      cmd.Add(lenghtOfPayload);
      cmd.Add(cycles);
      cmd.Add(spreadFactor);
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(bandwidth));
      byte[] parameter = cmd.ToArray();
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.StartTransmissionCylcle_0x29, parameter, progress, cancelToken);
      cmd = (List<byte>) null;
      parameter = (byte[]) null;
    }

    public async Task SetNFCFieldAsync(
      byte function,
      ushort timeout,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<byte> cmd = new List<byte>();
      cmd.Add(function);
      cmd.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeout));
      byte[] parameters = cmd.ToArray();
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonRadioCommands_0x2f, CommonRadioCommands_EFC.SetNFCField_0x2a, parameters, progress, cancelToken);
      cmd = (List<byte>) null;
      parameters = (byte[]) null;
    }

    public class RadioBandWidth : ReturnValue
    {
      public uint BandWidth;
      public uint AFC;
    }
  }
}
