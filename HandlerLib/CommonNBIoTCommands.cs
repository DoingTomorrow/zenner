// Decompiled with JetBrains decompiler
// Type: HandlerLib.CommonNBIoTCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public sealed class CommonNBIoTCommands : IZRCommand
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

    public CommonNBIoTCommands(Common32BitCommands commonCMD)
    {
      this.commonCMD = commonCMD;
      this.setCryptValuesFromBaseClass();
    }

    public void setCryptValuesFromBaseClass()
    {
      this.enDeCrypt = this.commonCMD.enDeCrypt;
      this.AES_Key = this.commonCMD.AES_Key;
    }

    public async Task<byte[]> GetNBIoT_ModulePartNumberAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetNBIoTModulePartNumber_0x00, progress, cancelToken);
      byte[] modulePartNumberAsync = theData;
      theData = (byte[]) null;
      return modulePartNumberAsync;
    }

    public async Task<byte[]> GetNBIoT_FirmwareVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetNBIoTFirmwareVersion_0x01, progress, cancelToken);
      byte[] firmwareVersionAsync = theData;
      theData = (byte[]) null;
      return firmwareVersionAsync;
    }

    public async Task<byte[]> GetNBIoT_IMEI_IMSI_NBVER_RAMAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetIMEI_IMSI_RAM_0x22, progress, cancelToken);
      byte[] imsiNbverRamAsync = theData;
      theData = (byte[]) null;
      return imsiNbverRamAsync;
    }

    public async Task<byte[]> GetNBIoT_ICCID_IMSI_RAMAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetICCID_IMSI_RAM_0x23, progress, cancelToken);
      byte[] iccidImsiRamAsync = theData;
      theData = (byte[]) null;
      return iccidImsiRamAsync;
    }

    public async Task SetNBIoT_IMEI_IMSI_NBVER_RAMAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetIMEI_IMSI_RAM_0x22, data, progress, cancelToken);
    }

    public async Task<byte[]> GetNBIoT_IMEI_NBAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetNBIoTIMEI_NB_0x02, progress, cancelToken);
      byte[] nbIoTImeiNbAsync = theData;
      theData = (byte[]) null;
      return nbIoTImeiNbAsync;
    }

    public async Task<byte[]> Get_SIM_IMSI_NBAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] simImsiNbAsync;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 20300;
        cfgList.MaxRequestRepeat = 1;
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSIM_IMSI_NB_0x03, progress, cancelToken);
        simImsiNbAsync = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return simImsiNbAsync;
    }

    public async Task<byte[]> GetNBIoT_Protocol(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetProtocol_0x04, progress, cancelToken);
      byte[] nbIoTProtocol = theData;
      theData = (byte[]) null;
      return nbIoTProtocol;
    }

    public async Task SetNBIoT_Protocol(
      byte[] id,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetProtocol_0x04, id, progress, cancelToken);
    }

    public async Task<byte[]> GetNBIoT_Band(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetBand_0x05, progress, cancelToken);
      byte[] nbIoTBand = theData;
      theData = (byte[]) null;
      return nbIoTBand;
    }

    public async Task SetNBIoT_Band(
      byte[] id,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetBand_0x05, id, progress, cancelToken);
    }

    public async Task<byte[]> GetNBIoT_SecondaryBand(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetSecondaryBand_0x0E, progress, cancelToken);
      byte[] ioTSecondaryBand = theData;
      theData = (byte[]) null;
      return ioTSecondaryBand;
    }

    public async Task SetNBIoT_SecondaryBand(
      byte[] id,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetSecondaryBand_0x0E, id, progress, cancelToken);
    }

    public async Task<byte[]> GetNBIoT_RemoteIP(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetRemoteIP_0x06, progress, cancelToken);
      byte[] nbIoTRemoteIp = theData;
      theData = (byte[]) null;
      return nbIoTRemoteIp;
    }

    public async Task SetNBIoT_RemoteIP(
      byte[] id,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetRemoteIP_0x06, id, progress, cancelToken);
    }

    public async Task<byte[]> GetNBIoT_RemotePort(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetRemotePort_0x07, progress, cancelToken);
      byte[] nbIoTRemotePort = theData;
      theData = (byte[]) null;
      return nbIoTRemotePort;
    }

    public async Task SetNBIoT_RemotePort(
      byte[] id,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetRemotePort_0x07, id, progress, cancelToken);
    }

    public async Task<byte[]> GetNBIoT_Operator(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetOperator_0x08, progress, cancelToken);
      byte[] nbIoTOperator = theData;
      theData = (byte[]) null;
      return nbIoTOperator;
    }

    public async Task SetNBIoT_Operator(
      byte[] id,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetOperator_0x08, id, progress, cancelToken);
    }

    public async Task SetNBIoT_PowerON(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] data = new byte[1]{ (byte) 1 };
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetNBIoTPowerOn_Off_0x20, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public async Task SetNBIoT_PowerOFF(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] data = new byte[1];
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetNBIoTPowerOn_Off_0x20, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public async Task<byte[]> NBIoT_CommonCommand(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] numArray;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 20300;
        cfgList.MaxRequestRepeat = 1;
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.NBIoTCommonCommand_0x21, data, progress, cancelToken);
        numArray = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return numArray;
    }

    public async Task<byte[]> NBIoT_SendTestData(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] numArray;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 60300;
        cfgList.MaxRequestRepeat = 1;
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SendTestData_0x0B, progress, cancelToken);
        numArray = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return numArray;
    }

    public async Task<byte[]> SetNBIoT_RadioFullFunctionOn(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] numArray;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 20300;
        cfgList.MaxRequestRepeat = 1;
        byte[] data = new byte[1]{ (byte) 1 };
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SendRadioFullFunctionOn_Off_0x0C, data, progress, cancelToken);
        numArray = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return numArray;
    }

    public async Task<byte[]> SetNBIoT_RadioFullFunctionOff(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] numArray;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 2300;
        cfgList.MaxRequestRepeat = 1;
        byte[] data = new byte[1];
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SendRadioFullFunctionOn_Off_0x0C, data, progress, cancelToken);
        numArray = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return numArray;
    }

    public async Task<byte[]> ResetNBModem(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte data = 0)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] numArray;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 2300;
        cfgList.MaxRequestRepeat = 1;
        byte[] Data = new byte[1]{ data };
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.ResetNBModem_0x11, Data, progress, cancelToken);
        numArray = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return numArray;
    }

    public async Task<byte[]> SetNBModemManualOrAutoConnect(
      byte data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] numArray;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 2300;
        cfgList.MaxRequestRepeat = 1;
        byte[] Data = new byte[1]{ data };
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetNBModemAutoOrManualConnect_0x12, Data, progress, cancelToken);
        numArray = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return numArray;
    }

    public async Task SendUnconfirmedDataAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 50;
      byte[] ba = new byte[maxBytes];
      Buffer.BlockCopy((Array) data, 0, (Array) ba, 0, data.Length > maxBytes ? maxBytes : data.Length);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SendUnconfirmedData_0x0A, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task SendConfirmedDataAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int maxBytes = 50;
      byte[] ba = new byte[maxBytes];
      Buffer.BlockCopy((Array) data, 0, (Array) ba, 0, data.Length > maxBytes ? maxBytes : data.Length);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SendConfirmedData_0x09, ba, progress, cancelToken);
      ba = (byte[]) null;
    }

    public async Task SetTransmissionScenarioAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (data == null || data.Length > 1)
        throw new Exception("SetTransmissionScenarioAsync: illegal data lenght");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetTransmissionScenario_0x28, data, progress, cancelToken);
    }

    public async Task<byte> GetTransmissionScenarioAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetTransmissionScenario_0x28, progress, cancelToken);
      byte transmissionScenarioAsync = theData[0];
      theData = (byte[]) null;
      return transmissionScenarioAsync;
    }

    public async Task NBIoT_SendActivePacket(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SendActivePacket_0x31, progress, cancelToken);
    }

    public async Task SetDevEUIAsync(
      byte[] devEUI,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (devEUI == null || devEUI.Length != 8)
        throw new Exception("SetDevEUIAsync: illegal devEUI");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetDevEUI_0x25, devEUI, progress, cancelToken);
    }

    public async Task<byte[]> GetDevEUIAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetDevEUI_0x25, progress, cancelToken);
      byte[] devEuiAsync = theData;
      theData = (byte[]) null;
      return devEuiAsync;
    }

    public async Task<byte[]> GetSIM_ICCID_NBAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int oldTimeout = 0;
      int oldMaxRequestRepeat = 0;
      IPort port = this.commonCMD.DeviceCMD.MBus.Repeater.Port;
      ConfigList cfgList = port.GetReadoutConfiguration();
      byte[] simIccidNbAsync;
      try
      {
        oldTimeout = cfgList.RecTime_BeforFirstByte;
        oldMaxRequestRepeat = cfgList.MaxRequestRepeat;
        cfgList.RecTime_BeforFirstByte = 20300;
        cfgList.MaxRequestRepeat = 1;
        byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSIM_ICCID_NB_0x0D, progress, cancelToken);
        simIccidNbAsync = theData;
      }
      finally
      {
        cfgList.RecTime_BeforFirstByte = oldTimeout;
        cfgList.MaxRequestRepeat = oldMaxRequestRepeat;
      }
      port = (IPort) null;
      cfgList = (ConfigList) null;
      return simIccidNbAsync;
    }

    public async Task<byte[]> GetDNSNameAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetDNSName_0x0F, progress, cancelToken);
      byte[] dnsNameAsync = theData;
      theData = (byte[]) null;
      return dnsNameAsync;
    }

    public async Task SetDNSNameAsync(
      byte[] dnsName,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (dnsName == null || dnsName.Length < 1)
        throw new Exception("SetDNSNameAsync: illegal DNSName");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetSetDNSName_0x0F, dnsName, progress, cancelToken);
    }

    public async Task<byte[]> GetRadioSendingState(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.GetRadioSendingState_0x10, progress, cancelToken);
      byte[] radioSendingState = theData;
      theData = (byte[]) null;
      return radioSendingState;
    }

    public async Task<byte[]> GetNBModemAPN(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetNBModemAPN_0x13, progress, cancelToken);
      byte[] nbModemApn = theData;
      theData = (byte[]) null;
      return nbModemApn;
    }

    public async Task SetNBModemAPN(
      byte[] APN,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (APN == null || APN.Length < 1)
        throw new Exception("SetNBModemAPN: illegal APN");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetNBModemAPN_0x13, APN, progress, cancelToken);
    }

    public async Task SetDNSServerIP(
      byte[] ServerIP,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (ServerIP == null || ServerIP.Length < 8)
        throw new Exception("SetDNSServerIP: illegal ServerIP");
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetDNSServerIP_0x14, ServerIP, progress, cancelToken);
    }

    public async Task<byte[]> GetDNSServerIP(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ReturnData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetDNSServerIP_0x14, progress, cancelToken);
      byte[] dnsServerIp = ReturnData;
      ReturnData = (byte[]) null;
      return dnsServerIp;
    }

    public async Task SetDNSEnableByte(
      byte EnableByte,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetDNSEnableByte_0x29, new byte[1]
      {
        EnableByte
      }, progress, cancelToken);
    }

    public async Task<byte> GetDNSEnableByte(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ReturnData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetDNSEnableByte_0x29, progress, cancelToken);
      byte dnsEnableByte = ReturnData != null && ReturnData.Length != 0 ? ReturnData[0] : throw new Exception("GetDNSEnableByte: Return data is null or empty");
      ReturnData = (byte[]) null;
      return dnsEnableByte;
    }

    public async Task SetAPNEanbleByte(
      byte EnableByte,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetAPNEnableByte_0x30, new byte[1]
      {
        EnableByte
      }, progress, cancelToken);
    }

    public async Task<byte> GetAPNEnabledByte(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] EnableByte = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.CommonNBIoTCommands_0x37, CommonNBIoTCommands_EFC.SetGetAPNEnableByte_0x30, progress, cancelToken);
      byte apnEnabledByte = EnableByte.Length != 0 ? EnableByte[0] : throw new Exception("GetAPNEnableByte: return data is empty");
      EnableByte = (byte[]) null;
      return apnEnabledByte;
    }

    private enum Switch : byte
    {
      OFF,
      ON,
    }
  }
}
