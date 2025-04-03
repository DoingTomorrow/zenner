// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonLoRaCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class CommonLoRaCommands : IZRCommand
  {
    internal static Logger CommonLoRaCommands_Logger = LogManager.GetLogger(nameof (CommonLoRaCommands));
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

    public CommonLoRaCommands(Common32BitCommands commonCMD)
    {
      this.commonCMD = commonCMD;
      this.setCryptValuesFromBaseClass();
    }

    public void setCryptValuesFromBaseClass()
    {
      this.enDeCrypt = this.commonCMD.enDeCrypt;
      this.AES_Key = this.commonCMD.AES_Key;
    }

    public async Task<LoRaFcVersion> GetLoRaFC_VersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte FC = 53;
      byte EFC = 0;
      byte[] resultData = await this.commonCMD.TransmitAndReceiveVersionData(FC, EFC, progress, cancelToken);
      if (resultData.Length != 4)
        throw new Exception("Illegal result length by GetLoRaVersionAsync");
      if ((int) resultData[0] != (int) FC || (int) resultData[1] != (int) EFC)
        throw new Exception("Illegal FC,EFC by GetLoRaVersionAsync");
      LoRaFcVersion retVal = new LoRaFcVersion();
      retVal.Version = BitConverter.ToUInt16(resultData, 2);
      retVal.basedata = resultData;
      LoRaFcVersion raFcVersionAsync = retVal;
      resultData = (byte[]) null;
      retVal = (LoRaFcVersion) null;
      return raFcVersionAsync;
    }

    public LoRaFcVersion GetLoRaFC_Version(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] data = this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetLoRaFC_Version_0x00, progress, cancelToken);
      if (data.Length != 2)
        throw new Exception("Illegal result length by GetLoRaVersion");
      LoRaFcVersion loRaFcVersion = new LoRaFcVersion();
      loRaFcVersion.Version = BitConverter.ToUInt16(data, 0);
      loRaFcVersion.basedata = data;
      return loRaFcVersion;
    }

    public async Task<LoRaWANVersion> GetLoRaWAN_VersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte FC = 53;
      byte EFC = 1;
      byte[] resultData = await this.commonCMD.TransmitAndReceiveVersionData(FC, EFC, progress, cancelToken);
      if (resultData.Length != 5)
        throw new Exception("Illegal result length by GetLoRaWANVersionAsync");
      if ((int) resultData[0] != (int) FC || (int) resultData[1] != (int) EFC)
        throw new Exception("Illegal FC,EFC by GetLoRaWANVersionAsync");
      LoRaWANVersion retVal = new LoRaWANVersion();
      retVal.MainVersion = resultData[2];
      retVal.MinorVersion = resultData[3];
      retVal.ReleaseNr = resultData[4];
      retVal.basedata = resultData;
      LoRaWANVersion raWanVersionAsync = retVal;
      resultData = (byte[]) null;
      retVal = (LoRaWANVersion) null;
      return raWanVersionAsync;
    }

    public LoRaWANVersion GetLoRaWAN_Version(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetLoRaWAN_Version_0x01, progress, cancelToken);
      if (data.Length != 3)
        throw new Exception("Illegal result length by GetLoRaWANVersion");
      LoRaWANVersion loRaWanVersion = new LoRaWANVersion();
      loRaWanVersion.MainVersion = data[0];
      loRaWanVersion.MinorVersion = data[1];
      loRaWanVersion.ReleaseNr = data[2];
      loRaWanVersion.basedata = data;
      return loRaWanVersion;
    }

    public async Task SendJoinRequestAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.SendJoinRequest_0x06, progress, cancelToken);
    }

    public async Task<LoRaCheckJoinAccept> CheckJoinAcceptAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.CheckJoinAccept_0x07, progress, cancelToken);
      LoRaCheckJoinAccept retVal = new LoRaCheckJoinAccept();
      DateTime? localDateTime = new DateTime?();
      localDateTime = MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(theData);
      retVal.Timestamp = localDateTime.HasValue ? localDateTime.Value : DateTime.MinValue;
      Buffer.BlockCopy((Array) theData, 4, (Array) (retVal.NetID = new byte[3]), 0, 3);
      retVal.DeviceAddress = BitConverter.ToUInt32(theData, 7);
      retVal.basedata = theData;
      LoRaCheckJoinAccept raCheckJoinAccept = retVal;
      theData = (byte[]) null;
      retVal = (LoRaCheckJoinAccept) null;
      return raCheckJoinAccept;
    }

    public async Task SendUnconfirmedDataAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 50;
      int dataBytes = data.Length > maxBytes ? maxBytes : data.Length;
      byte[] ba = new byte[dataBytes];
      Buffer.BlockCopy((Array) data, 0, (Array) ba, 0, dataBytes);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.SendUnconfirmedData_0x08, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task SendConfirmedDataAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 50;
      int dataBytes = data.Length > maxBytes ? maxBytes : data.Length;
      byte[] ba = new byte[dataBytes];
      Buffer.BlockCopy((Array) data, 0, (Array) ba, 0, dataBytes);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.SendConfirmedData_0x09, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task SetNetIDAsync(
      byte[] id,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetNetID_0x20, id, progress, cancelToken);
    }

    public async Task<byte[]> GetNetIDAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetNetIDAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetNetID_0x20, progress, cancelToken);
      byte[] netIdAsync = theData;
      theData = (byte[]) null;
      return netIdAsync;
    }

    public async Task SetAppEUIAsync(
      byte[] appEUI,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 8;
      byte[] ba = new byte[maxBytes];
      Buffer.BlockCopy((Array) appEUI, 0, (Array) ba, 0, appEUI.Length > maxBytes ? maxBytes : appEUI.Length);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppEUI_0x21, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<byte[]> GetAppEUIAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetAppEUIAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppEUI_0x21, progress, cancelToken);
      byte[] appEuiAsync = theData;
      theData = (byte[]) null;
      return appEuiAsync;
    }

    public byte[] GetAppEUI(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppEUI_0x21, progress, cancelToken);
    }

    public async Task SetNwkSKeyAsync(
      byte[] nwkSKey,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 16;
      byte[] ba = new byte[maxBytes];
      Buffer.BlockCopy((Array) nwkSKey, 0, (Array) ba, 0, nwkSKey.Length > maxBytes ? maxBytes : nwkSKey.Length);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetNwkSKey_0x22, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<byte[]> GetNwkSKeyAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetNwkSKeyAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetNwkSKey_0x22, progress, cancelToken);
      byte[] nwkSkeyAsync = theData;
      theData = (byte[]) null;
      return nwkSkeyAsync;
    }

    public byte[] GetNwkSKey(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetNwkSKey_0x22, progress, cancelToken);
    }

    public async Task SetAppSKeyAsync(
      byte[] appSKey,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 16;
      byte[] ba = new byte[maxBytes];
      Buffer.BlockCopy((Array) appSKey, 0, (Array) ba, 0, appSKey.Length > maxBytes ? maxBytes : appSKey.Length);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppSKey_0x23, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<byte[]> GetAppSKeyAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetAppSKeyAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppSKey_0x23, progress, cancelToken);
      byte[] appSkeyAsync = theData;
      theData = (byte[]) null;
      return appSkeyAsync;
    }

    public byte[] GetAppSKey(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppSKey_0x23, progress, cancelToken);
    }

    public async Task SetOTAA_ABPAsync(
      OTAA_ABP flag,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ba = new byte[1]{ (byte) flag };
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetOTAA_ABP_0x24, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<OTAA_ABP> GetOTAA_ABPAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetOTAA_ABP_0x24, progress, cancelToken);
      OTAA_ABP otaaAbpAsync = (OTAA_ABP) theData[0];
      theData = (byte[]) null;
      return otaaAbpAsync;
    }

    public OTAA_ABP GetOTAA_ABP(ProgressHandler progress, CancellationToken cancelToken)
    {
      return (OTAA_ABP) this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetOTAA_ABP_0x24, progress, cancelToken)[0];
    }

    public async Task SetDevEUIAsync(
      byte[] devEUI,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (devEUI == null || devEUI.Length != 8)
        throw new Exception("SetDevEUIAsync: illegal devEUI");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetDevEUI_0x25, devEUI, progress, cancelToken);
    }

    public async Task<byte[]> GetDevEUIAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetDevEUIAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetDevEUI_0x25, progress, cancelToken);
      byte[] devEuiAsync = theData;
      theData = (byte[]) null;
      return devEuiAsync;
    }

    public byte[] GetDevEUI(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetDevEUI_0x25, progress, cancelToken);
    }

    public async Task SetDevAddrAsync(
      byte[] devAddr,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (devAddr == null || devAddr.Length != 4)
        throw new Exception("SetDevAddrAsync: illegal device address");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetDevAddr_0x27, devAddr, progress, cancelToken);
    }

    public async Task<byte[]> GetDevAddrAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetDevAddrAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetDevAddr_0x27, progress, cancelToken);
      byte[] devAddrAsync = theData;
      theData = (byte[]) null;
      return devAddrAsync;
    }

    public async Task SetAppKeyAsync(
      byte[] appKey,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (appKey == null || appKey.Length != 16)
        throw new Exception("SetAppKeyAsync: illegal appKey");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppKey_0x26, appKey, progress, cancelToken);
    }

    public async Task<byte[]> GetAppKeyAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetAppKeyAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppKey_0x26, progress, cancelToken);
      byte[] appKeyAsync = theData;
      theData = (byte[]) null;
      return appKeyAsync;
    }

    public byte[] GetAppKey(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetAppKey_0x26, progress, cancelToken);
    }

    public async Task SetTransmissionScenarioAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (data == null || data.Length > 1)
        throw new Exception("SetTransmissionScenarioAsync: illegal data lenght");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetTransmissionScenario_0x28, data, progress, cancelToken);
    }

    public async Task<byte> GetTransmissionScenarioAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetTransmissionScenario_0x28, progress, cancelToken);
      byte transmissionScenarioAsync = theData[0];
      theData = (byte[]) null;
      return transmissionScenarioAsync;
    }

    public byte GetTransmissionScenario(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetTransmissionScenario_0x28, progress, cancelToken)[0];
    }

    public async Task<byte> GetAdrAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      CommonLoRaCommands.CommonLoRaCommands_Logger.Trace(nameof (GetAdrAsync));
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetADR_0x2A, progress, cancelToken);
      byte adrAsync = theData[0];
      theData = (byte[]) null;
      return adrAsync;
    }

    public async Task SetAdrAsync(
      byte ADR,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = new byte[1]{ ADR };
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetADR_0x2A, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public byte GetAdr(ProgressHandler progress, CancellationToken cancelToken)
    {
      return this.commonCMD.TransmitAndGetData(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.GetSetADR_0x2A, progress, cancelToken)[0];
    }

    public async Task SendConfigurationPaketAsync(
      byte[] PacketType,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.SendConfigurationPacket_0x29, PacketType, progress, cancelToken);
    }

    public async Task TriggerSystemDiagnosticAsync(
      LoRaSystemDiagnostic lsd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 3;
      byte[] ba = new byte[maxBytes];
      ba[0] = lsd.DiagnosticConfig;
      ba[1] = lsd.DailyStartTime;
      ba[2] = lsd.DailyEndTime;
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.TriggerSystemDiagnostic_0x2B, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task<LoRaSystemDiagnostic> SystemDiagnosticStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      bool test = false)
    {
      if (test)
        return new LoRaSystemDiagnostic()
        {
          DiagnosticConfig = 83,
          DailyStartTime = 40,
          DailyEndTime = 133,
          RemainigActivity = 33,
          RemainigDiagnostic = 8704
        };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonLoRaCommands_0x35, CommonLoRaCommands_EFC.TriggerSystemDiagnostic_0x2B, progress, cancelToken);
      LoRaSystemDiagnostic lsd = new LoRaSystemDiagnostic();
      lsd.basedata = theData;
      if (theData.Length == 9)
      {
        lsd.DiagnosticConfig = theData[0];
        lsd.DailyStartTime = theData[1];
        lsd.DailyEndTime = theData[2];
        lsd.RemainigActivity = BitConverter.ToUInt16(theData, 3);
        lsd.RemainigDiagnostic = BitConverter.ToUInt32(theData, 5);
      }
      else
      {
        lsd.DiagnosticConfig = (byte) 0;
        lsd.DailyStartTime = (byte) 0;
        lsd.DailyEndTime = (byte) 0;
        lsd.RemainigActivity = (ushort) 0;
        lsd.RemainigDiagnostic = 0U;
      }
      return lsd;
    }
  }
}
