// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonMBusCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public sealed class CommonMBusCommands : IZRCommand
  {
    private Common32BitCommands commonCMD;
    private bool crypt = false;
    private string AESKey = (string) null;

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

    public CommonMBusCommands(Common32BitCommands commonCMD)
    {
      this.commonCMD = commonCMD;
      this.setCryptValuesFromBaseClass();
    }

    public void setCryptValuesFromBaseClass()
    {
      this.enDeCrypt = this.commonCMD.enDeCrypt;
      this.AES_Key = this.commonCMD.AES_Key;
    }

    public async Task<MBusChannelAddress> GetMBusChannelAddressAsync(
      byte channel,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetMBusChannelAddress_0x01, new byte[1]
      {
        channel
      }, progress, cancelToken);
      MBusChannelAddress retVal = new MBusChannelAddress();
      retVal.Channel = theData[0];
      retVal.Address = theData[1];
      MBusChannelAddress channelAddressAsync = retVal;
      theData = (byte[]) null;
      retVal = (MBusChannelAddress) null;
      return channelAddressAsync;
    }

    public async Task SetMBusChannelAddressAsync(
      byte channel,
      byte address,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[2]{ channel, address };
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetMBusChannelAddress_0x01, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<MBusChannelIdentification> GetChannelIdentificationAsync(
      byte channel,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[1]{ channel };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelIdentification_0x02, ba, progress, cancelToken);
      MBusChannelIdentification retVal = new MBusChannelIdentification();
      retVal.Channel = theData[0];
      byte[] baTemp = new byte[4];
      Buffer.BlockCopy((Array) theData, 1, (Array) baTemp, 0, 4);
      retVal.SerialNumber = (long) Util.ConvertBcdUInt32ToUInt32(BitConverter.ToUInt32(baTemp, 0));
      baTemp = new byte[2];
      Buffer.BlockCopy((Array) theData, 5, (Array) baTemp, 0, 2);
      retVal.Manufacturer = MBusUtil.GetManufacturer(BitConverter.ToUInt16(baTemp, 0));
      retVal.Generation = theData[7];
      retVal.Medium = MBusUtil.GetMedium(theData[8]);
      MBusChannelIdentification identificationAsync = retVal;
      ba = (byte[]) null;
      theData = (byte[]) null;
      retVal = (MBusChannelIdentification) null;
      baTemp = (byte[]) null;
      return identificationAsync;
    }

    public MBusChannelIdentification GetChannelIdentification(
      byte channel,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelIdentification_0x02, new byte[1]
      {
        channel
      }, progress, cancelToken);
      MBusChannelIdentification channelIdentification = new MBusChannelIdentification();
      channelIdentification.Channel = data[0];
      byte[] dst1 = new byte[4];
      Buffer.BlockCopy((Array) data, 1, (Array) dst1, 0, 4);
      channelIdentification.SerialNumber = (long) Util.ConvertBcdUInt32ToUInt32(BitConverter.ToUInt32(dst1, 0));
      byte[] dst2 = new byte[2];
      Buffer.BlockCopy((Array) data, 5, (Array) dst2, 0, 2);
      channelIdentification.Manufacturer = MBusUtil.GetManufacturer(BitConverter.ToUInt16(dst2, 0));
      channelIdentification.Generation = data[7];
      channelIdentification.Medium = MBusUtil.GetMedium(data[8]);
      return channelIdentification;
    }

    public async Task SetChannelIdentificationAsync(
      MBusChannelIdentification mbChannelIdent,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[9];
      ba[0] = mbChannelIdent.Channel;
      uint serial = (uint) mbChannelIdent.SerialNumber;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(serial), 0, (Array) ba, 1, 4);
      ushort manu = MBusUtil.GetManufacturerCode(mbChannelIdent.Manufacturer);
      Buffer.BlockCopy((Array) BitConverter.GetBytes(manu), 0, (Array) ba, 5, 2);
      ba[7] = mbChannelIdent.Generation;
      ba[8] = byte.Parse(Enum.Format(typeof (Medium), Enum.Parse(typeof (Medium), mbChannelIdent.Medium), "d"));
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelIdentification_0x02, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<MBusChannelOBIS> GetChannelOBISCodeAsync(
      byte channel,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[1]{ channel };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelOBIS_Code_0x03, ba, progress, cancelToken);
      MBusChannelOBIS retVal = new MBusChannelOBIS();
      retVal.Channel = theData[0];
      retVal.OBIS_code = theData[1];
      MBusChannelOBIS channelObisCodeAsync = retVal;
      ba = (byte[]) null;
      theData = (byte[]) null;
      retVal = (MBusChannelOBIS) null;
      return channelObisCodeAsync;
    }

    public async Task SetChannelOBISCodeAsync(
      byte channel,
      byte code,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[2]{ channel, code };
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelOBIS_Code_0x03, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task SetChannelConfigurationAsync(
      MBusChannelConfiguration mbChannelConfig,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[5]
      {
        mbChannelConfig.Channel,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      Buffer.BlockCopy((Array) mbChannelConfig.Mantissa, 0, (Array) ba, 1, 2);
      ba[3] = (byte) mbChannelConfig.Exponent;
      ba[4] = mbChannelConfig.VIF;
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelConfiguration_0x04, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<MBusChannelConfiguration> GetChannelConfigurationAsync(
      byte channel,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[1]{ channel };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelConfiguration_0x04, ba, progress, cancelToken);
      MBusChannelConfiguration retVal = new MBusChannelConfiguration();
      retVal.Channel = theData[0];
      Buffer.BlockCopy((Array) theData, 1, (Array) (retVal.Mantissa = new byte[2]), 0, 2);
      retVal.Exponent = (sbyte) theData[3];
      retVal.VIF = theData[4];
      MBusChannelConfiguration configurationAsync = retVal;
      ba = (byte[]) null;
      theData = (byte[]) null;
      retVal = (MBusChannelConfiguration) null;
      return configurationAsync;
    }

    public async Task SetChannelValueAsync(
      byte channel,
      uint value,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[5]
      {
        channel,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) ba, 1, 4);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelValue_0x05, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<uint> GetChannelValueAsync(
      byte channel,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] parameter = new byte[1]{ channel };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetChannelValue_0x05, parameter, progress, cancelToken);
      uint uint32 = BitConverter.ToUInt32(theData, 1);
      parameter = (byte[]) null;
      theData = (byte[]) null;
      return uint32;
    }

    public async Task<MBusChannelLog> ReadChannelLogValueAsync(
      byte channel,
      byte logSelect,
      byte startIndex,
      byte endIndex,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[4]
      {
        channel,
        logSelect,
        startIndex,
        endIndex
      };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.ReadChannelLogValue_0x10, ba, progress, cancelToken);
      if (theData == null || theData.Length == 0)
        return (MBusChannelLog) null;
      MBusChannelLog retVal = new MBusChannelLog();
      retVal.Channel = theData[0];
      retVal.LogSelected = theData[1];
      retVal.StartIndex = theData[2];
      retVal.EndIndex = theData[3];
      retVal.Lenght = theData[4];
      retVal.Year = theData[5];
      retVal.Month = theData[6];
      retVal.Day = theData[7];
      retVal.Hour = theData[8];
      retVal.Minute = theData[9];
      retVal.Second = theData[10];
      Buffer.BlockCopy((Array) theData, 11, (Array) (retVal.LogValues = new byte[theData.Length - 11]), 0, theData.Length - 11);
      return retVal;
    }

    public async Task<MBusEventLog> ReadEventLogAsync(
      byte flowControl,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[1]{ flowControl };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.ReadEventLog_0x11, ba, progress, cancelToken);
      MBusEventLog retVal = new MBusEventLog();
      retVal.FlowControl = theData[0];
      retVal.EntryFormat = theData[1];
      retVal.SystemEventType = theData[2];
      Buffer.BlockCopy((Array) theData, 3, (Array) (retVal.EventTime = new byte[5]), 0, 5);
      Buffer.BlockCopy((Array) theData, 8, (Array) (retVal.Channel0Value = new byte[4]), 0, 4);
      Buffer.BlockCopy((Array) theData, 12, (Array) (retVal.Channel1Value = new byte[4]), 0, 4);
      MBusEventLog mbusEventLog = retVal;
      ba = (byte[]) null;
      theData = (byte[]) null;
      retVal = (MBusEventLog) null;
      return mbusEventLog;
    }

    public async Task ClearEventLogAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.ClearEventLog_0x12, progress, cancelToken);
    }

    public async Task<MBusSystemLog> ReadSystemLogAsync(
      byte flowControl,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[1]{ flowControl };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.ReadSystemLog_0x13, ba, progress, cancelToken);
      MBusSystemLog retVal = new MBusSystemLog();
      retVal.FlowControl = theData[0];
      retVal.EntryFormat = theData[1];
      retVal.SystemEventType = theData[2];
      Buffer.BlockCopy((Array) theData, 3, (Array) (retVal.EventTime = new byte[5]), 0, 5);
      Buffer.BlockCopy((Array) theData, 8, (Array) (retVal.Channel0Value = new byte[4]), 0, 4);
      Buffer.BlockCopy((Array) theData, 12, (Array) (retVal.Channel1Value = new byte[4]), 0, 4);
      MBusSystemLog mbusSystemLog = retVal;
      ba = (byte[]) null;
      theData = (byte[]) null;
      retVal = (MBusSystemLog) null;
      return mbusSystemLog;
    }

    public async Task ClearSystemLogAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.ClearSystemLog_0x14, progress, cancelToken);
    }

    public async Task<MBusChannelSingleLogValue> ReadChannelSingleLogValueAsync(
      byte channel,
      byte[] date,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      ushort pos = 0;
      byte[] ba = new byte[3];
      ba[(int) pos++] = channel;
      Buffer.BlockCopy((Array) date, 0, (Array) ba, (int) pos, 2);
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.ReadChannelSingleLogValue_0x15, ba, progress, cancelToken);
      MBusChannelSingleLogValue retVal = new MBusChannelSingleLogValue();
      retVal.basedata = theData;
      retVal.Channel = theData[0];
      retVal.Date = new byte[2];
      Buffer.BlockCopy((Array) theData, 1, (Array) retVal.Date, 0, 2);
      retVal.DateAndTime = Util.ConvertToDate_MBus_CP16_TypeG(retVal.Date, 0).Value;
      byte[] localValue = new byte[4];
      Buffer.BlockCopy((Array) theData, 3, (Array) localValue, 0, 4);
      retVal.Value = BitConverter.ToUInt32(localValue, 0);
      MBusChannelSingleLogValue channelSingleLogValue = retVal;
      ba = (byte[]) null;
      theData = (byte[]) null;
      retVal = (MBusChannelSingleLogValue) null;
      localValue = (byte[]) null;
      return channelSingleLogValue;
    }

    public async Task SetRadioListAsync(
      byte radiolist,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[1]{ radiolist };
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetRadioList_0x16, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<byte[]> GetRadioListAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] retVal = new byte[1];
      retVal = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetRadioList_0x16, progress, cancelToken);
      byte[] radioListAsync = retVal;
      retVal = (byte[]) null;
      return radioListAsync;
    }

    public async Task SetTXTimingsAsync(
      MBusTXTimings values,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.SetTXTimingsAsync(values.Interval, values.NightTimeStart, values.NightTimeEnd, values.RadioSuppressionDays, values.Reserved, progress, cancelToken);
    }

    public async Task SetTXTimingsAsync(
      ushort interval,
      byte beginHour,
      byte endHour,
      byte radioSuppressionDays,
      uint reserved,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      ushort pos = 0;
      byte[] ba = new byte[9];
      Buffer.BlockCopy((Array) BitConverter.GetBytes(interval), 0, (Array) ba, (int) pos, 2);
      pos += (ushort) 2;
      ba[(int) pos++] = beginHour;
      ba[(int) pos++] = endHour;
      ba[(int) pos++] = radioSuppressionDays;
      Buffer.BlockCopy((Array) BitConverter.GetBytes(reserved), 0, (Array) ba, (int) pos, 4);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetTXTimings_0x17, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<MBusTXTimings> GetTXTimingsAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetTXTimings_0x17, progress, cancelToken);
      MBusTXTimings retVal = new MBusTXTimings();
      retVal.basedata = theData;
      retVal.Interval = BitConverter.ToUInt16(theData, 0);
      retVal.NightTimeStart = theData[2];
      retVal.NightTimeEnd = theData[3];
      retVal.RadioSuppressionDays = theData[4];
      retVal.Reserved = BitConverter.ToUInt32(theData, 5);
      MBusTXTimings txTimingsAsync = retVal;
      theData = (byte[]) null;
      retVal = (MBusTXTimings) null;
      return txTimingsAsync;
    }

    public async Task<byte[]> GetMBusKeyAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetMBusKey_0x18, progress, cancelToken);
      byte[] mbusKeyAsync = theData;
      theData = (byte[]) null;
      return mbusKeyAsync;
    }

    public byte[] GetMBusKey(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetMBusKey_0x18, progress, cancelToken);
    }

    public async Task SetMBusKeyAsync(
      byte[] mbusKey,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (mbusKey == null)
        throw new ArgumentNullException(nameof (mbusKey));
      if (mbusKey.Length != 16)
        throw new ArgumentOutOfRangeException("The length of 'mbusKey' must be 16 bytes");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonMBusCommands_0x34, CommonMBusCommands_EFC.GetSetMBusKey_0x18, mbusKey, progress, cancelToken);
    }
  }
}
