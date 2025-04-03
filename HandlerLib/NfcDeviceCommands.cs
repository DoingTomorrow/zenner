// Decompiled with JetBrains decompiler
// Type: HandlerLib.NfcDeviceCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using HandlerLib.NFC;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class NfcDeviceCommands : IZRCommand
  {
    public static Logger Base_DeviceCommandsLogger = LogManager.GetLogger("DeviceCommandsNFC");
    public ChannelLogger DeviceCommandsLogger;
    private NfcDeviceIdentification connectedDeviceVersion;
    public static byte[] FillData = new byte[2]
    {
      (byte) 90,
      (byte) 165
    };
    private bool ReadOnly = false;
    public bool NFC_BlockMode = true;

    public CommunicationPortFunctions Port { get; private set; }

    public NfcRepeater myNfcRepeater { get; set; }

    public NfcFrame nfcFrame { get; set; }

    public NfcMemoryTransceiver myNfcMemoryTransceiver { get; set; }

    public NfcSubunitCommands mySubunitCommands { get; set; }

    public bool useSubUnitCommands { get; set; }

    public NfcDeviceIdentification ConnectedDeviceVersion
    {
      get => this.connectedDeviceVersion;
      set => this.connectedDeviceVersion = value;
    }

    public byte[] ConnectedReducedID { get; private set; }

    public bool IsDeviceIdentified => this.ConnectedDeviceVersion != null;

    public NfcDeviceCommands(CommunicationPortFunctions port)
    {
      if (port == null)
        throw new ArgumentNullException(nameof (port));
      this.useSubUnitCommands = false;
      this.Port = port;
      this.myNfcRepeater = new NfcRepeater(port);
      this.mySubunitCommands = new NfcSubunitCommands(port);
      this.myNfcMemoryTransceiver = new NfcMemoryTransceiver(this.myNfcRepeater);
      this.ReadOnly = UserManager.CheckPermission(UserManager.Right_ReadOnly);
      this.DeviceCommandsLogger = new ChannelLogger(NfcDeviceCommands.Base_DeviceCommandsLogger, this.myNfcRepeater.myConfig);
    }

    public void SetIdentificationLikeInFirmware(
      uint serialNumberBCD,
      ushort manufacturerCode,
      byte generation,
      byte mediumCode)
    {
      this.CheckReadOnly();
      this.DeviceCommandsLogger.Debug(nameof (SetIdentificationLikeInFirmware));
      this.ConnectedDeviceVersion = new NfcDeviceIdentification(serialNumberBCD, manufacturerCode, generation, mediumCode);
    }

    private void CheckReadOnly()
    {
      if (this.ReadOnly)
        throw new Exception("Right ReadOnly is set");
    }

    public async Task<DeviceIdentification> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      this.DeviceCommandsLogger.Debug(nameof (ReadVersionAsync));
      NfcDeviceIdentification nfcDVI = new NfcDeviceIdentification();
      this.myNfcRepeater.ClearCrcInitValue();
      this.nfcFrame = new NfcFrame(NfcCommands.GetIdentification, this.myNfcRepeater.myConfig.ReadingChannelIdentification);
      await this.myNfcRepeater.GetResultFrameAsync(this.nfcFrame, progress, token);
      nfcDVI = new NfcDeviceIdentification(this.nfcFrame.NfcResponseFrame);
      this.myNfcRepeater.SetCrcInitValue(nfcDVI.MeterID.Value);
      this.ConnectedDeviceVersion = nfcDVI;
      this.myNfcMemoryTransceiver.MaxBufferSize = this.NFC_BlockMode && this.ConnectedDeviceVersion.FirmwareVersion.Value >= 17039366U ? (this.myNfcRepeater.IrDaCommands == null && this.myNfcRepeater.IrDaWrapper == 0 ? 512U : 220U) : 64U;
      DeviceIdentification deviceIdentification = (DeviceIdentification) nfcDVI;
      nfcDVI = (NfcDeviceIdentification) null;
      return deviceIdentification;
    }

    public async Task ReadMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug("ReadMemoryAsync " + addressRange.ToString());
      byte[] readData = await this.myNfcMemoryTransceiver.ReadMemoryAsync(addressRange.StartAddress, addressRange.ByteSize, progress, cancelToken);
      deviceMemory.SetData(addressRange.StartAddress, readData);
      readData = (byte[]) null;
    }

    public async Task WriteMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug("WriteMemoryAsync " + addressRange.ToString());
      this.CheckReadOnly();
      byte[] writeData = deviceMemory.GetData(addressRange);
      if (writeData == null)
        throw new Exception("Write data not complete for AddressRange: " + addressRange.ToString());
      byte[] numArray = await this.myNfcMemoryTransceiver.WriteMemoryAsync(addressRange.StartAddress, writeData, progress, cancelToken);
      writeData = (byte[]) null;
    }

    public async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAdress,
      uint size)
    {
      this.DeviceCommandsLogger.Debug(nameof (ReadMemoryAsync));
      if (!this.useSubUnitCommands)
      {
        byte[] numArray = await this.myNfcMemoryTransceiver.ReadMemoryAsync(startAdress, size, progress, cancelToken);
        return numArray;
      }
      byte[] numArray1 = await this.mySubunitCommands.ReadNdcMemory_Async(startAdress, size, progress, cancelToken);
      return numArray1;
    }

    public async Task WriteMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAdress,
      byte[] bytes)
    {
      this.DeviceCommandsLogger.Debug(nameof (WriteMemoryAsync));
      this.CheckReadOnly();
      if (bytes == null)
        throw new Exception("Write data not complete for AddressRange: " + startAdress.ToString());
      byte[] numArray = await this.myNfcMemoryTransceiver.WriteMemoryAsync(startAdress, bytes, progress, cancelToken);
    }

    public async Task ResetDeviceAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      bool loadBackup = false)
    {
      this.DeviceCommandsLogger.Debug(nameof (ResetDeviceAsync));
      NfcFrame nfcFrame;
      if (loadBackup)
        nfcFrame = new NfcFrame(NfcCommands.ResetDevice, new byte[1]
        {
          (byte) 1
        }, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      else
        nfcFrame = new NfcFrame(NfcCommands.ResetDevice, new byte[1], this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.TransmitFrameAsync(nfcFrame, progress, cancelToken);
      nfcFrame = (NfcFrame) null;
    }

    public async Task BackupDeviceAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (BackupDeviceAsync));
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.SaveBackup, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      nfcFrame = (NfcFrame) null;
    }

    public async Task UnlockDevice(
      uint key,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (UnlockDevice));
      this.CheckReadOnly();
      byte[] data = BitConverter.GetBytes(key);
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.UnlockDevice, data, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      data = (byte[]) null;
      nfcFrame = (NfcFrame) null;
    }

    public async Task LockDevice(uint key, ProgressHandler progress, CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (LockDevice));
      this.CheckReadOnly();
      byte[] data = BitConverter.GetBytes(key);
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.LockDevice, data, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      data = (byte[]) null;
      nfcFrame = (NfcFrame) null;
    }

    public async Task<byte[]> VerifyMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAddress,
      uint endAddress)
    {
      this.DeviceCommandsLogger.Debug("verifyMemory");
      List<byte> ldata = new List<byte>();
      ldata.Add((byte) 4);
      ldata.AddRange((IEnumerable<byte>) BitConverter.GetBytes(startAddress));
      ldata.AddRange((IEnumerable<byte>) BitConverter.GetBytes(endAddress));
      byte[] data = ldata.ToArray();
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.ChecksumManagement, data, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      byte[] nfcResponseFrame = nfcFrame.NfcResponseFrame;
      ldata = (List<byte>) null;
      data = (byte[]) null;
      nfcFrame = (NfcFrame) null;
      return nfcResponseFrame;
    }

    public async Task<byte[]> StandardCommandAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      NfcCommands command,
      byte[] data)
    {
      this.DeviceCommandsLogger.Debug(nameof (StandardCommandAsync));
      NfcFrame nfcFrame = new NfcFrame(command, data, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      byte[] nfcResponseFrame = nfcFrame.NfcResponseFrame;
      nfcFrame = (NfcFrame) null;
      return nfcResponseFrame;
    }

    public async Task StandardCommandAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      NfcCommands command)
    {
      this.DeviceCommandsLogger.Debug(nameof (StandardCommandAsync));
      NfcFrame nfcFrame = new NfcFrame(command, NfcDeviceCommands.FillData, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      nfcFrame = (NfcFrame) null;
    }

    public byte[] StandardCommand(
      ProgressHandler progress,
      CancellationToken cancelToken,
      NfcCommands command,
      byte[] data)
    {
      this.DeviceCommandsLogger.Debug(nameof (StandardCommand));
      NfcFrame nfcFrame = new NfcFrame(command, data, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      this.myNfcRepeater.GetResultFrame(nfcFrame, progress, cancelToken);
      return nfcFrame.NfcResponseFrame;
    }

    public void StandardCommand(
      ProgressHandler progress,
      CancellationToken cancelToken,
      NfcCommands command)
    {
      this.DeviceCommandsLogger.Debug(nameof (StandardCommand));
      this.myNfcRepeater.GetResultFrame(new NfcFrame(command, NfcDeviceCommands.FillData, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue), progress, cancelToken);
    }

    public async Task<byte[]> SendCommandAndGetResultAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      NfcCommands command,
      byte[] data = null)
    {
      this.DeviceCommandsLogger.Debug(nameof (SendCommandAndGetResultAsync));
      NfcFrame nfcFrame = data != null ? new NfcFrame(command, data, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue) : new NfcFrame(command, NfcDeviceCommands.FillData, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      byte[] nfcResult = nfcFrame.NfcResponseFrame;
      int headerLength = nfcResult[0] >= byte.MaxValue ? 4 : 2;
      if ((NfcCommands) nfcResult[headerLength - 1] != command)
        throw new Exception("Received commend != command");
      byte[] resultDataOnly = new byte[nfcResult.Length - headerLength - 2];
      Buffer.BlockCopy((Array) nfcResult, headerLength, (Array) resultDataOnly, 0, resultDataOnly.Length);
      byte[] resultAsync = resultDataOnly;
      nfcFrame = (NfcFrame) null;
      nfcResult = (byte[]) null;
      resultDataOnly = (byte[]) null;
      return resultAsync;
    }

    public async Task<byte[]> SendIrCommandAndGetResultAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      Manufacturer_FC command,
      byte[] data = null)
    {
      this.DeviceCommandsLogger.Debug(nameof (SendIrCommandAndGetResultAsync));
      byte[] frameData;
      if (data == null)
      {
        frameData = new byte[1]{ (byte) command };
      }
      else
      {
        frameData = new byte[data.Length + 1];
        frameData[0] = (byte) command;
        data.CopyTo((Array) frameData, 1);
      }
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.IrDa_Compatible_Command, frameData, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      byte[] nfcResult = nfcFrame.NfcResponseFrame;
      int headerLength = nfcResult[0] >= byte.MaxValue ? 4 : 2;
      if (nfcResult[headerLength - 1] != (byte) 32)
        throw new Exception("Received commend != IrDa_Compatible_Command");
      if ((Manufacturer_FC) nfcResult[headerLength] != command)
      {
        if (nfcResult[headerLength] != (byte) 254)
        {
          if (nfcResult[headerLength] != byte.MaxValue)
            throw new Exception("Received IrDa commend structure error. Response command byte != request command byte and != ACK and != NACK");
          if ((Manufacturer_FC) nfcResult[headerLength + 1] != command)
            throw new Exception("IrDa NACK received. Following IrDa command byte error. Received: 0x" + nfcResult[headerLength + 1].ToString("x02"));
          int nackMessageIndex;
          if (Enum.IsDefined(typeof (SubCommands_FC), (object) nfcResult[headerLength + 1]))
          {
            if (frameData.Length < 2)
              throw new Exception("IrDa NACK received. Extended command byte received but not expected.");
            if ((int) nfcResult[headerLength + 2] != (int) frameData[1])
              throw new Exception("IrDa NACK received. Illegal extended command byte. Received: 0x" + nfcResult[headerLength + 2].ToString("x02"));
            nackMessageIndex = headerLength + 3;
          }
          else
            nackMessageIndex = headerLength + 2;
          throw new Exception("IrDa NACK received. IrDa coded error from device: " + ((NACK_Messages) nfcResult[nackMessageIndex]).ToString());
        }
        if ((Manufacturer_FC) nfcResult[headerLength + 1] == command)
          return new byte[0];
        throw new Exception("Illegal IrDa ACK. Command byte error");
      }
      byte[] resultDataOnly = new byte[nfcResult.Length - headerLength - 3];
      Buffer.BlockCopy((Array) nfcResult, headerLength + 1, (Array) resultDataOnly, 0, resultDataOnly.Length);
      return resultDataOnly;
    }

    public async Task<DateTimeOffset> GetSystemDateTime(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (GetSystemDateTime));
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.GetSystemDateTime, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      int scanOffset = 2;
      DateTimeOffset systemDateTime = ByteArrayScanner.ScanDateTimeOffset(nfcFrame.NfcResponseFrame, ref scanOffset);
      nfcFrame = (NfcFrame) null;
      return systemDateTime;
    }

    public async Task<DateTimeOffset> SetSystemDateTime(
      DateTimeOffset dateTimeOffset,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug("SetSystemDateTime: " + dateTimeOffset.ToString());
      int scanOffset = 0;
      byte[] dateTimeBytes = new byte[7];
      ByteArrayScanner.ScanInDateTimeOffset(dateTimeBytes, dateTimeOffset, ref scanOffset);
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.SetSystemDateTime, dateTimeBytes, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      DateTimeOffset dateTimeOffset1 = dateTimeOffset;
      dateTimeBytes = (byte[]) null;
      nfcFrame = (NfcFrame) null;
      return dateTimeOffset1;
    }

    public async Task SetRTCCalibrationValue(
      ushort clocks,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (SetRTCCalibrationValue));
      byte[] bytes = BitConverter.GetBytes(clocks);
      NfcFrame frame = new NfcFrame(NfcCommands.SetRtcCalibrationValue, bytes, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(frame, progress, cancelToken);
      bytes = (byte[]) null;
      frame = (NfcFrame) null;
    }

    public async Task ClearEventLogger(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (ClearEventLogger));
      await this.StandardCommandAsync(progress, cancelToken, NfcCommands.ClearEventLogger);
    }

    public async Task SetAccumulatedValues(
      ProgressHandler progress,
      CancellationToken cancelToken,
      List<double> values)
    {
      StringBuilder nlogInfo = new StringBuilder("SetAccumulatedValues ");
      if (values == null || values.Count < 1)
        throw new ArgumentException("Values not defined");
      byte[] commandData = values.Count <= 5 ? new byte[values.Count * 8] : throw new ArgumentException("Not supported number of values");
      for (int i = 0; i < values.Count; ++i)
      {
        if (double.IsNaN(values[i]))
          throw new ArgumentException("NaN not allowed");
        if (i > 0)
          nlogInfo.Append(';');
        nlogInfo.Append(values[i].ToString());
        Buffer.BlockCopy((Array) BitConverter.GetBytes(values[i]), 0, (Array) commandData, i * 8, 8);
      }
      this.DeviceCommandsLogger.Debug(nlogInfo.ToString());
      byte[] numArray = await this.StandardCommandAsync(progress, cancelToken, NfcCommands.ClearEventLogger, commandData);
      nlogInfo = (StringBuilder) null;
      commandData = (byte[]) null;
    }

    public async Task<NfcDeviceCommands.BatteryEndDateData> GetBatteryEndDateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (GetBatteryEndDateAsync));
      NfcDeviceCommands.BatteryEndDateData result = new NfcDeviceCommands.BatteryEndDateData();
      byte[] resultBytes = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetBatteryEndDate, NfcDeviceCommands.FillData);
      if (resultBytes.Length == 7)
        result.BatteryCapacity_mAh = new ushort?(BitConverter.ToUInt16(resultBytes, 5));
      if (resultBytes.Length == 5 || resultBytes.Length == 7)
      {
        result.BatteryDurabilityMonths = new byte?(resultBytes[3]);
        result.BatteryPreWaringMonths = new sbyte?((sbyte) resultBytes[4]);
      }
      else if (resultBytes.Length != 3)
        throw new Exception("Illegal result length");
      result.EndDate = new DateTime((int) resultBytes[0] + 2000, (int) resultBytes[1], (int) resultBytes[2]);
      NfcDeviceCommands.BatteryEndDateData batteryEndDateAsync = result;
      result = (NfcDeviceCommands.BatteryEndDateData) null;
      resultBytes = (byte[]) null;
      return batteryEndDateAsync;
    }

    public async Task SetBatteryEndDateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      DateTime endDate,
      byte? batteryDurabilityMonth = null,
      sbyte? batteryPreWaringMonth = null,
      ushort? batteryCapacity = null)
    {
      byte[] commandData;
      if (!batteryDurabilityMonth.HasValue)
      {
        commandData = new byte[3];
      }
      else
      {
        if (!batteryCapacity.HasValue)
        {
          commandData = new byte[5];
        }
        else
        {
          commandData = new byte[7];
          BitConverter.GetBytes(batteryCapacity.Value).CopyTo((Array) commandData, 5);
        }
        commandData[3] = batteryDurabilityMonth.Value;
        commandData[4] = (byte) batteryPreWaringMonth.Value;
      }
      commandData[0] = (byte) (endDate.Year - 2000);
      commandData[1] = (byte) endDate.Month;
      commandData[2] = (byte) endDate.Day;
      byte[] resultAsync = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SetBatteryEndDate, commandData);
      commandData = (byte[]) null;
    }

    public async Task<List<SmartFunctionIdentResultAndCalls>> GetSmartFunctionsList(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (GetSmartFunctionsList));
      byte[] result = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetSmartFunctionsList);
      List<SmartFunctionIdentResultAndCalls> FunctionsInDevice = new List<SmartFunctionIdentResultAndCalls>();
      int scanOffset = 0;
      while (scanOffset < result.Length)
      {
        SmartFunctionIdentResultAndCalls func = new SmartFunctionIdentResultAndCalls(result, ref scanOffset);
        FunctionsInDevice.Add(func);
        func = (SmartFunctionIdentResultAndCalls) null;
      }
      List<SmartFunctionIdentResultAndCalls> smartFunctionsList = FunctionsInDevice;
      result = (byte[]) null;
      FunctionsInDevice = (List<SmartFunctionIdentResultAndCalls>) null;
      return smartFunctionsList;
    }

    public async Task<SmartFunctionResult> SetSmartFunctionActivationAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      string functionName,
      bool active)
    {
      byte[] byteData = new byte[50];
      int offset = 0;
      ByteArrayScanner.ScanInString(byteData, functionName, ref offset);
      byteData[offset] = !active ? (byte) 0 : (byte) 1;
      Array.Resize<byte>(ref byteData, offset + 1);
      byte[] result = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SetSmartFunctionActivation, byteData);
      SmartFunctionResult smartFunctionResult = result.Length == 2 ? (SmartFunctionResult) BitConverter.ToUInt16(result, 0) : throw new Exception("Unexpected number of result bytes at SetSmartFunctionActivationAsync");
      byteData = (byte[]) null;
      result = (byte[]) null;
      return smartFunctionResult;
    }

    public async Task<S4_SystemState> GetDeviceStatesAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug(nameof (GetDeviceStatesAsync));
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.GetSystemState, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      S4_SystemState deviceState = new S4_SystemState(nfcFrame.NfcResponseFrame);
      S4_SystemState deviceStatesAsync = deviceState;
      nfcFrame = (NfcFrame) null;
      deviceState = (S4_SystemState) null;
      return deviceStatesAsync;
    }

    public async Task SetModeAsync(
      S4_DeviceModes mode,
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte[] optionBytes = null)
    {
      NfcFrame nfcFrame = this.GetModeFrame(mode, optionBytes);
      await this.myNfcRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      nfcFrame = (NfcFrame) null;
    }

    public void SetMode(
      S4_DeviceModes mode,
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte[] optionBytes = null)
    {
      this.myNfcRepeater.GetResultFrame(this.GetModeFrame(mode, optionBytes), progress, cancelToken);
    }

    private NfcFrame GetModeFrame(S4_DeviceModes mode, byte[] optionBytes = null)
    {
      if (!Enum.IsDefined(typeof (S4_DeviceModes), (object) mode))
        throw new ArgumentException("undefined S4_SystemState.DeviceModes mode");
      this.DeviceCommandsLogger.Debug("SetModeAsync");
      string str = "SetModeAsync. Mode:" + mode.ToString();
      byte[] numArray;
      if (optionBytes != null)
      {
        numArray = new byte[1 + optionBytes.Length];
        numArray[0] = (byte) mode;
        Buffer.BlockCopy((Array) optionBytes, 0, (Array) numArray, 1, optionBytes.Length);
        this.DeviceCommandsLogger.Debug(str + "; 0x" + Util.ByteArrayToHexString(numArray));
      }
      else
      {
        numArray = new byte[1]{ (byte) mode };
        this.DeviceCommandsLogger.Debug(str + "; 0x" + numArray[0].ToString("x02"));
      }
      return new NfcFrame(NfcCommands.SetTestMode, numArray, this.myNfcRepeater.myConfig.ReadingChannelIdentification, this.myNfcRepeater.CrcInitValue);
    }

    public async Task<double> GetCenterFrequencyMHz(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] commandData = new byte[2]
      {
        (byte) 47,
        (byte) 6
      };
      byte[] result = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.IrDa_Compatible_Command, commandData);
      uint frequencyHz = result.Length == 6 ? BitConverter.ToUInt32(result, 2) : throw new Exception("Unexpected number of result bytes at GetCenterFrequency");
      double centerFrequencyMhz = (double) frequencyHz / 1000000.0;
      commandData = (byte[]) null;
      result = (byte[]) null;
      return centerFrequencyMhz;
    }

    public async Task SetCenterFrequencyMHz(
      ProgressHandler progress,
      CancellationToken cancelToken,
      double frequencyMHz)
    {
      byte[] commandData = new byte[6]
      {
        (byte) 47,
        (byte) 6,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      BitConverter.GetBytes((uint) (frequencyMHz * 1000000.0)).CopyTo((Array) commandData, 2);
      byte[] result = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.IrDa_Compatible_Command, commandData);
      if (result.Length != 3)
        throw new Exception("Unexpected number of result bytes at GetCenterFrequency");
      commandData = (byte[]) null;
      result = (byte[]) null;
    }

    public async Task DeleteAllModuleConfigurations(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = new byte[2]
      {
        byte.MaxValue,
        byte.MaxValue
      };
      byte[] resultAsync = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SetModuleConfiguration, data);
      data = (byte[]) null;
    }

    public async Task ClearSysStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint sysStateAddress,
      byte[] buffer)
    {
      uint clearFlag = 4278190080;
      byte[] SysStateBuffer = await this.ReadMemoryAsync(progress, cancelToken, sysStateAddress, (uint) (byte) buffer.Length);
      uint sysState = BitConverter.ToUInt32(SysStateBuffer, 0);
      sysState &= clearFlag;
      await this.WriteMemoryAsync(progress, cancelToken, sysStateAddress, SysStateBuffer);
      SysStateBuffer = (byte[]) null;
    }

    public async Task<byte[]> CallTestFunctionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte[] transmitData)
    {
      this.DeviceCommandsLogger.Debug("CallTestFunction");
      byte[] result = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.CallTestFunction, transmitData);
      byte[] numArray = result;
      result = (byte[]) null;
      return numArray;
    }

    public async Task<List<BusModuleInfo>> ReadBusModuleListAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.DeviceCommandsLogger.Debug("ReadBusModuleList");
      byte[] result = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetBusModuleList);
      List<BusModuleInfo> infoList = BusModuleInfo.GetBusModuleInfoList(result, 0);
      List<BusModuleInfo> busModuleInfoList = infoList;
      result = (byte[]) null;
      infoList = (List<BusModuleInfo>) null;
      return busModuleInfoList;
    }

    public async Task<byte[]> SendTransparentToModuleAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      BusModuleInfo moduleInfo,
      BusModuleCommand moduleCommand,
      byte[] transmitData = null)
    {
      this.DeviceCommandsLogger.Debug("SendTransparentToModule");
      byte[] sendData;
      if (transmitData == null)
      {
        sendData = new byte[7];
      }
      else
      {
        sendData = new byte[transmitData.Length + 7];
        transmitData.CopyTo((Array) sendData, 7);
      }
      BitConverter.GetBytes((ushort) moduleInfo.BusModuleType).CopyTo((Array) sendData, 0);
      BitConverter.GetBytes(moduleInfo.BusModuleSerialNumber).CopyTo((Array) sendData, 2);
      sendData[6] = (byte) moduleCommand;
      byte[] result = await this.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SendToBusModule, sendData);
      byte[] moduleAsync = result;
      sendData = (byte[]) null;
      result = (byte[]) null;
      return moduleAsync;
    }

    public class BatteryEndDateData
    {
      public DateTime EndDate;
      public byte? BatteryDurabilityMonths;
      public sbyte? BatteryPreWaringMonths;
      public ushort? BatteryCapacity_mAh;
    }
  }
}
