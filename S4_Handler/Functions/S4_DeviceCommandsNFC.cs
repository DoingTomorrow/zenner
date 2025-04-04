// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_DeviceCommandsNFC
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using HandlerLib.NFC;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_DeviceCommandsNFC : BaseMemoryAccess
  {
    internal static Logger Base_S4_DeviceCommandsNFC_Logger = LogManager.GetLogger(nameof (S4_DeviceCommandsNFC));
    internal ChannelLogger S4_DeviceCommandsNFC_Logger;
    internal Common32BitCommands CMDs_Device = (Common32BitCommands) null;
    internal CommonLoRaCommands CMDs_LoRa = (CommonLoRaCommands) null;
    internal CommonMBusCommands CMDs_MBus = (CommonMBusCommands) null;
    internal CommonRadioCommands CMDs_Radio = (CommonRadioCommands) null;
    internal SpecialCommands CMDs_Special = (SpecialCommands) null;
    private S4_FunctionalState LastFunctionalState;

    internal NfcDeviceCommands CommonNfcCommands { get; set; }

    internal MiConConnector myMiConConnector { get; set; }

    internal S4_DeviceCommandsNFC(CommunicationPortFunctions myPort)
    {
      this.CommonNfcCommands = new NfcDeviceCommands(myPort);
      this.myMiConConnector = new MiConConnector(this.CommonNfcCommands);
      this.CMDs_Device = (Common32BitCommands) new S4_NFC_Common32BitCommands(this);
      this.CMDs_MBus = new CommonMBusCommands(this.CMDs_Device);
      this.CMDs_LoRa = new CommonLoRaCommands(this.CMDs_Device);
      this.CMDs_Radio = new CommonRadioCommands(this.CMDs_Device);
      this.CMDs_Special = new SpecialCommands(this.CMDs_Device);
      this.S4_DeviceCommandsNFC_Logger = new ChannelLogger(S4_DeviceCommandsNFC.Base_S4_DeviceCommandsNFC_Logger, this.CommonNfcCommands.myNfcRepeater.myConfig);
    }

    public override async Task<DeviceIdentification> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DeviceIdentification ident = await this.CommonNfcCommands.ReadVersionAsync(progress, token);
      DeviceIdentification deviceIdentification = ident;
      ident = (DeviceIdentification) null;
      return deviceIdentification;
    }

    public override async Task ReadMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.CommonNfcCommands.ReadMemoryAsync(addressRange, deviceMemory, progress, cancelToken);
    }

    public override async Task WriteMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.CommonNfcCommands.WriteMemoryAsync(addressRange, deviceMemory, progress, cancelToken);
    }

    public override async Task InterruptConnection(
      ProgressHandler progress,
      CancellationToken token)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (InterruptConnection));
      byte[] numArray = await this.CommonNfcCommands.mySubunitCommands.SetRfOffAsync(progress, token);
    }

    internal async Task PrepareForFlyingTestAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (PrepareForFlyingTestAsync));
      byte[] tmp = new byte[1]{ (byte) 145 };
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.SetTestMode, tmp, this.CommonNfcCommands.myNfcRepeater.myConfig.ReadingChannelIdentification, this.CommonNfcCommands.myNfcRepeater.CrcInitValue);
      byte[] numArray1 = await this.CommonNfcCommands.mySubunitCommands.SetTestStart(nfcFrame.NfcRequestFrame, progress, cancelToken);
      tmp[0] = (byte) 147;
      nfcFrame = new NfcFrame(NfcCommands.SetTestMode, tmp, this.CommonNfcCommands.myNfcRepeater.myConfig.ReadingChannelIdentification, this.CommonNfcCommands.myNfcRepeater.CrcInitValue);
      byte[] numArray2 = await this.CommonNfcCommands.mySubunitCommands.SetTestStop(nfcFrame.NfcRequestFrame, progress, cancelToken);
      await this.CommonNfcCommands.SetModeAsync(S4_DeviceModes.TestModePrepared, progress, cancelToken);
      tmp = (byte[]) null;
      nfcFrame = (NfcFrame) null;
    }

    internal async Task<FlyingTestData> ReadFlyingTestResultsAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (ReadFlyingTestResultsAsync));
      deviceMemory.GarantMemoryAvailable(addressRange);
      await this.CommonNfcCommands.ReadMemoryAsync(addressRange, deviceMemory, progress, cancelToken);
      FlyingTestData theData = new FlyingTestData();
      theData.flowStart = Parameter32bit.GetValue<float>(addressRange.StartAddress, deviceMemory);
      theData.flowStop = Parameter32bit.GetValue<float>(addressRange.StartAddress + 4U, deviceMemory);
      theData.nfcTimeStart = Parameter32bit.GetValue<float>(addressRange.StartAddress + 8U, deviceMemory);
      theData.nfcTimeStop = Parameter32bit.GetValue<float>(addressRange.StartAddress + 12U, deviceMemory);
      theData.tdcMeasCounter = Parameter32bit.GetValue<uint>(addressRange.StartAddress + 16U, deviceMemory);
      theData.nfcTotalTime = Parameter32bit.GetValue<float>(addressRange.StartAddress + 20U, deviceMemory);
      theData.tdcTotalTime = Parameter32bit.GetValue<float>(addressRange.StartAddress + 24U, deviceMemory);
      theData.flyingTestVol = Parameter32bit.GetValue<float>(addressRange.StartAddress + 28U, deviceMemory);
      theData.flyingTestResultVol = Parameter32bit.GetValue<float>(addressRange.StartAddress + 32U, deviceMemory);
      FlyingTestData flyingTestData = theData;
      theData = (FlyingTestData) null;
      return flyingTestData;
    }

    internal async Task StopAllTestAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (StopAllTestAsync));
      await Task.Delay(10);
      await this.CommonNfcCommands.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
      byte[] numArray = await this.CommonNfcCommands.mySubunitCommands.SetTestOff(progress, cancelToken);
    }

    internal async Task SwitchLcd(ProgressHandler progress, CancellationToken cancellationToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (SwitchLcd));
      byte[] numArray1 = await this.CommonNfcCommands.mySubunitCommands.SetRfOnAsync(progress, cancellationToken);
      await Task.Delay(550, cancellationToken);
      byte[] numArray2 = await this.CommonNfcCommands.mySubunitCommands.SetRfOffAsync(progress, cancellationToken);
      await Task.Delay(1000, cancellationToken);
    }

    internal async Task<S4_SystemState> GetDeviceStatesAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (GetDeviceStatesAsync));
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.GetSystemState, this.CommonNfcCommands.myNfcRepeater.myConfig.ReadingChannelIdentification, this.CommonNfcCommands.myNfcRepeater.CrcInitValue);
      await this.CommonNfcCommands.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      S4_SystemState deviceState = new S4_SystemState(nfcFrame.NfcResponseFrame);
      S4_SystemState deviceStatesAsync = deviceState;
      nfcFrame = (NfcFrame) null;
      deviceState = (S4_SystemState) null;
      return deviceStatesAsync;
    }

    internal S4_SystemState GetDeviceStates(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (GetDeviceStates));
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.GetSystemState, this.CommonNfcCommands.myNfcRepeater.myConfig.ReadingChannelIdentification, this.CommonNfcCommands.myNfcRepeater.CrcInitValue);
      this.CommonNfcCommands.myNfcRepeater.GetResultFrame(nfcFrame, progress, cancelToken);
      return new S4_SystemState(nfcFrame.NfcResponseFrame);
    }

    internal async Task<S4_CurrentData> ReadCurrentDataAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(" ReadCurrentDataAsync");
      byte[] result = await this.CommonNfcCommands.StandardCommandAsync(progress, cancelToken, NfcCommands.GetVolumeAndFlow, NfcDeviceCommands.FillData);
      S4_CurrentData s4CurrentData = new S4_CurrentData(result);
      result = (byte[]) null;
      return s4CurrentData;
    }

    public async Task<S4_FunctionalState> ReadAlliveAndStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      S4_DeviceCommandsNFC.FunctionalStateRequest request = S4_DeviceCommandsNFC.FunctionalStateRequest.current)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (ReadAlliveAndStateAsync));
      byte[] requestData;
      if (request == S4_DeviceCommandsNFC.FunctionalStateRequest.current)
      {
        requestData = NfcDeviceCommands.FillData;
      }
      else
      {
        requestData = new byte[1];
        byte? stateNumber;
        int num1;
        if (request != S4_DeviceCommandsNFC.FunctionalStateRequest.next && this.LastFunctionalState != null)
        {
          stateNumber = this.LastFunctionalState.StateNumber;
          num1 = !stateNumber.HasValue ? 1 : 0;
        }
        else
          num1 = 1;
        if (num1 != 0)
        {
          requestData[0] = byte.MaxValue;
        }
        else
        {
          byte[] numArray = requestData;
          stateNumber = this.LastFunctionalState.StateNumber;
          int num2 = (int) stateNumber.Value;
          numArray[0] = (byte) num2;
          if (requestData[0] > (byte) 99)
            requestData[0] = (byte) 0;
        }
      }
      byte[] result = await this.CommonNfcCommands.StandardCommandAsync(progress, cancelToken, NfcCommands.GetAliveAndStatus, requestData);
      this.LastFunctionalState = new S4_FunctionalState(result);
      S4_FunctionalState lastFunctionalState = this.LastFunctionalState;
      requestData = (byte[]) null;
      result = (byte[]) null;
      return lastFunctionalState;
    }

    public async Task<DeviceStateCounter> ReadStateCounterAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (ReadStateCounterAsync));
      byte[] result = await this.CommonNfcCommands.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetStateCounters);
      DeviceStateCounter deviceStateCounter = new DeviceStateCounter(result);
      result = (byte[]) null;
      return deviceStateCounter;
    }

    public async Task<S4_SmartFunctionInfo> ReadSmartFunctionInfoAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (ReadSmartFunctionInfoAsync));
      byte[] result = await this.CommonNfcCommands.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetSmartFunctionInfo);
      S4_SmartFunctionInfo smartFunctionInfo = new S4_SmartFunctionInfo(result);
      result = (byte[]) null;
      return smartFunctionInfo;
    }

    public async Task DeleteAllSmartFunctionsAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (DeleteAllSmartFunctionsAsync));
      if (new FirmwareVersion(this.CommonNfcCommands.ConnectedDeviceVersion.FirmwareVersion.Value) < (object) "1.4.9 IUW")
        return;
      byte[] numArray = await this.CommonNfcCommands.StandardCommandAsync(progress, cancelToken, NfcCommands.DeleteAllSmartFunctions, NfcDeviceCommands.FillData);
    }

    public async Task Delete_wMBus_first_day_flag(
      ProgressHandler progress,
      CancellationToken cancelToken,
      S4_DeviceMemory theMemory)
    {
      uint WmBus_Parameter_Adr = theMemory.GetParameterAddress(S4_Params.WmBus_Parameter);
      WmBus_Parameter_Adr += 42U;
      byte[] clearByte = new byte[1];
      this.S4_DeviceCommandsNFC_Logger.Debug("Delete_wMBus_first_day_flag. Adr: 0x" + WmBus_Parameter_Adr.ToString("x08"));
      byte[] numArray = await this.CommonNfcCommands.myNfcMemoryTransceiver.WriteMemoryAsync(WmBus_Parameter_Adr, clearByte, progress, cancelToken);
      clearByte = (byte[]) null;
    }

    public async Task ResetNDCModuleStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.S4_DeviceCommandsNFC_Logger.Debug(nameof (ResetNDCModuleStateAsync));
      int ModuleStateOFF = 0;
      byte[] data = new byte[1]{ (byte) ModuleStateOFF };
      byte[] result = await this.CommonNfcCommands.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SetNDC_ModuleState, data);
      data = (byte[]) null;
      result = (byte[]) null;
    }

    public enum FunctionalStateRequest
    {
      current,
      next,
      last,
    }
  }
}
