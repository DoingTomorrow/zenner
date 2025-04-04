// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_RadioTests
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommunicationPort;
using CommunicationPort.Functions;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  public class S4_RadioTests
  {
    internal static Logger S4_BaseRadioTestsLogger = LogManager.GetLogger(nameof (S4_RadioTests));
    private ChannelLogger S4_RadioTestsLogger;
    internal static Logger S4_RadioTestResultLogger = LogManager.GetLogger("S4_RadioTestsResults");
    private S4_HandlerFunctions MyFunctions;
    private RadioTestLog TestResultLog;

    internal S4_RadioTests(S4_HandlerFunctions myFunctions)
    {
      this.MyFunctions = myFunctions;
      this.S4_RadioTestsLogger = new ChannelLogger(S4_RadioTests.S4_BaseRadioTestsLogger, this.MyFunctions.configList);
    }

    private void NLOG_Trace(string traceMessage) => this.S4_RadioTestsLogger.Trace(traceMessage);

    private void NLOG_Debug(string traceMessage) => this.S4_RadioTestsLogger.Debug(traceMessage);

    private void NLOG_Info(string traceMessage) => this.S4_RadioTestsLogger.Info(traceMessage);

    public async Task<RadioTestLoopResults> RadioSendTestAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      RadioTestByDevice radioDevice,
      ushort timeoutSeconds,
      uint cycleMilliSeconds,
      int requiredReceiveCount)
    {
      Exception Ex = (Exception) null;
      RadioTestLoopResults testLoopResults = new RadioTestLoopResults();
      DateTime startTime = DateTime.Now;
      DateTime endTime = startTime.AddSeconds((double) timeoutSeconds);
      ushort receiveTimeoutSeconds = (ushort) (cycleMilliSeconds / 1000U);
      if (receiveTimeoutSeconds < (ushort) 1)
        ++receiveTimeoutSeconds;
      try
      {
        uint DeviceID = radioDevice.GetRandomDeviceID();
        uint DeviceID_BCD = Util.ConvertUnt32ToBcdUInt32(DeviceID);
        byte[] sendPacketBytes = CommonRadioCommands.GetSendTestPacketData((ushort) cycleMilliSeconds, timeoutSeconds, DeviceID, CommonRadioCommands.DefaultAbitraryData, radioDevice.TestParameters.SyncWord);
        byte[] testPacketBytes = new byte[sendPacketBytes.Length - 2];
        Buffer.BlockCopy((Array) sendPacketBytes, 2, (Array) testPacketBytes, 0, testPacketBytes.Length);
        radioDevice.CancelToken = cancelToken;
        FirmwareVersion firmwareVersion = new FirmwareVersion(this.MyFunctions.myMeters.ConnectedMeter.deviceIdentification.FirmwareVersion.Value);
        await radioDevice.SetTestParameterAsync(progress, firmwareVersion >= (object) "1.7.1 IUW");
        if (firmwareVersion >= (object) "1.7.1 IUW")
          await this.MyFunctions.checkedCommands.CMDs_Radio.SetCenterFrequencyAsync((uint) (radioDevice.TestParameters.TestFrequency * 1000000.0), progress, cancelToken);
        if (this.S4_RadioTestsLogger.IsTraceEnabled)
        {
          string randomDeviceID = "RandomDeviceID: " + DeviceID.ToString();
          string arbitraryDate = "ArbitraryData: 0x" + Util.ByteArrayToHexString(CommonRadioCommands.DefaultAbitraryData);
          string sync = "SyncWord: 0x" + radioDevice.TestParameters.SyncWord.ToString("X04");
          string sycle = "Cycle[ms]: " + cycleMilliSeconds.ToString();
          progress.Report(randomDeviceID);
          progress.Report(arbitraryDate);
          progress.Report(sync);
          progress.Report(sycle);
          this.NLOG_Trace(randomDeviceID);
          this.NLOG_Trace(arbitraryDate);
          this.NLOG_Trace(sync);
          this.NLOG_Trace(sycle);
          randomDeviceID = (string) null;
          arbitraryDate = (string) null;
          sync = (string) null;
          sycle = (string) null;
        }
        int exceptionCount = 0;
        while (testLoopResults.ReceiveCount < requiredReceiveCount)
        {
          try
          {
            DateTime currentTestTime = startTime.AddMilliseconds((double) ((long) testLoopResults.TestCount * (long) cycleMilliSeconds));
            if (currentTestTime <= DateTime.Now)
            {
              currentTestTime = DateTime.Now;
            }
            else
            {
              int waitMilliSeconds = (int) currentTestTime.Subtract(DateTime.Now).TotalMilliseconds;
              if (waitMilliSeconds > 50)
                await Task.Delay(waitMilliSeconds, radioDevice.CancelToken);
            }
            ++testLoopResults.TestCount;
            bool done = false;
            RadioTestResult radioTestResult = (RadioTestResult) null;
            Task<RadioTestResult> resultReceiverTask = (Task<RadioTestResult>) null;
            if (radioDevice.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.MinoConnect)
            {
              Task taskReceiver = Task.Run((Action) (() =>
              {
                radioTestResult = radioDevice.ReceiveOnePacket((int) DeviceID, receiveTimeoutSeconds, radioDevice.TestParameters.SyncWord.ToString("x04"));
                done = true;
              }));
              this.NLOG_Debug("TestDevice MinoConnect ReceiveOnePacket active");
              taskReceiver = (Task) null;
            }
            else
            {
              resultReceiverTask = radioDevice.ReceiveOnePacketAsync(progress, DeviceID, receiveTimeoutSeconds, testPacketBytes);
              await Task.Delay(100, cancelToken);
              this.NLOG_Debug("TestDevice IUWS ReceiveOnePacket active");
            }
            DateTime nextReceiveTimeoutTime = DateTime.Now.AddSeconds((double) receiveTimeoutSeconds);
            while (!done)
            {
              if (DateTime.Now > nextReceiveTimeoutTime)
              {
                this.NLOG_Info("TestDevice receive timeout");
                break;
              }
              if (DateTime.Now > endTime)
              {
                string m = "Timeout by test time over.";
                this.NLOG_Info(m);
                throw new TimeoutException(m);
              }
              this.NLOG_Debug("Set IUW mode SendOneTestPacket");
              await this.MyFunctions.SetModeAsync(progress, cancelToken, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.RadioTestSendTestPacket, timeoutSeconds: (ushort) 1, arbitraryData: sendPacketBytes);
              ++testLoopResults.SendCount;
              progress.Report(tag: (object) testLoopResults);
              for (int p = 0; p < 6; ++p)
              {
                this.NLOG_Trace("Check receiver finished");
                ++testLoopResults.PollingCount;
                if (done)
                {
                  this.NLOG_Trace("MinoConnect TestDevice receiver: done");
                  break;
                }
                if (resultReceiverTask != null && resultReceiverTask.IsCompleted)
                {
                  radioTestResult = resultReceiverTask.Result;
                  this.NLOG_Debug("IUWS TestDevice receiver: done");
                  done = true;
                  break;
                }
                this.NLOG_Trace("TestDevice receiver: no data");
                await Task.Delay(50, radioDevice.CancelToken);
              }
              if (radioTestResult == null)
                this.LogTestResult(radioDevice, true, RadioTestLog.ReceiveInfo.timeout);
            }
            if (radioTestResult != null)
            {
              if (radioTestResult.Payload != null || radioTestResult.Payload.Length < testPacketBytes.Length)
              {
                for (int x = 0; x < testPacketBytes.Length; ++x)
                {
                  if ((int) testPacketBytes[x] != (int) radioTestResult.Payload[x])
                  {
                    this.NLOG_Debug("Illegal test data received.");
                    throw new Exception("Illegal test data received.");
                  }
                }
                testLoopResults.AddRSSI(radioTestResult.RSSI, new int?((int) radioTestResult.LQI));
                progress.Report("Packet received and ok");
                this.NLOG_Debug("Packet received and ok");
                this.LogTestResult(radioDevice, true, RadioTestLog.ReceiveInfo.ok, radioTestResult.RSSI);
              }
              else
              {
                this.NLOG_Debug("Wrong data received.");
                this.LogTestResult(radioDevice, true, RadioTestLog.ReceiveInfo.error);
                throw new Exception("Wrong data received.");
              }
            }
            else
            {
              this.NLOG_Debug("No data received");
              ++testLoopResults.NoDataCount;
            }
            progress.Report(tag: (object) testLoopResults);
            resultReceiverTask = (Task<RadioTestResult>) null;
          }
          catch (Exception ex)
          {
            if (ex is TaskCanceledException)
              throw ex;
            ++exceptionCount;
            if (exceptionCount > 2 || DateTime.Now > endTime)
            {
              Ex = ex;
              break;
            }
          }
        }
        if (testLoopResults.ReceiveCount < requiredReceiveCount)
          throw new Exception("ReceiveCount < RequiredReceiveCount");
        await radioDevice.StopRadioAsync(progress);
        await this.MyFunctions.SetModeAsync(progress, new CancellationTokenSource().Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.OperationMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
        if (Ex != null)
          throw Ex;
        sendPacketBytes = (byte[]) null;
        testPacketBytes = (byte[]) null;
        firmwareVersion = new FirmwareVersion();
      }
      catch (Exception ex)
      {
        if (!(ex is TaskCanceledException))
          throw new Exception("RadioReceiveTestAsync stopped by exception", ex);
      }
      RadioTestLoopResults radioTestLoopResults = testLoopResults;
      Ex = (Exception) null;
      testLoopResults = (RadioTestLoopResults) null;
      return radioTestLoopResults;
    }

    public async Task<RadioTestLoopResults> RadioReceiveTestAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      RadioTestByDevice radioDevice,
      ushort timeoutSeconds,
      uint cycleMilliSeconds,
      int requiredReceiveCount)
    {
      Exception Ex = (Exception) null;
      RadioTestLoopResults testLoopResults = new RadioTestLoopResults();
      DateTime startTime = DateTime.Now;
      DateTime endTime = startTime.AddSeconds((double) timeoutSeconds);
      try
      {
        uint deviceID = radioDevice.GetRandomDeviceID();
        string testPacketString = CommunicationByMinoConnect.GetMiConRadioTestPacket(deviceID);
        byte[] sendPacketBytes = (byte[]) null;
        byte[] testPacketBytes;
        if (radioDevice.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.MinoConnect)
        {
          testPacketBytes = Encoding.ASCII.GetBytes(testPacketString);
        }
        else
        {
          sendPacketBytes = CommonRadioCommands.GetSendTestPacketData((ushort) 200, timeoutSeconds, deviceID, CommonRadioCommands.DefaultAbitraryData, radioDevice.TestParameters.SyncWord);
          testPacketBytes = new byte[sendPacketBytes.Length - 2];
          Buffer.BlockCopy((Array) sendPacketBytes, 2, (Array) testPacketBytes, 0, testPacketBytes.Length);
        }
        radioDevice.CancelToken = cancelToken;
        FirmwareVersion firmwareVersion = new FirmwareVersion(this.MyFunctions.myMeters.ConnectedMeter.deviceIdentification.FirmwareVersion.Value);
        await radioDevice.SetTestParameterAsync(progress, firmwareVersion >= (object) "1.7.1 IUW");
        if (firmwareVersion >= (object) "1.7.1 IUW")
          await this.MyFunctions.checkedCommands.CMDs_Radio.SetCenterFrequencyAsync((uint) (radioDevice.TestParameters.TestFrequency * 1000000.0), progress, cancelToken);
        if (this.S4_RadioTestsLogger.IsTraceEnabled)
        {
          string randomDeviceID = "RandomDeviceID: " + deviceID.ToString();
          string arbitrary = "ArbitraryData: 0x" + Util.ByteArrayToHexString(testPacketBytes);
          string sync = "SyncWord: 0x" + radioDevice.TestParameters.SyncWord.ToString("X04");
          string sycle = "Cycle[ms]: " + cycleMilliSeconds.ToString();
          progress.Report(randomDeviceID);
          progress.Report(arbitrary);
          progress.Report(sync);
          progress.Report(sycle);
          this.NLOG_Trace(randomDeviceID);
          this.NLOG_Trace(arbitrary);
          this.NLOG_Trace(sync);
          this.NLOG_Trace(sycle);
          randomDeviceID = (string) null;
          arbitrary = (string) null;
          sync = (string) null;
          sycle = (string) null;
        }
        int exceptionCount = 0;
        while (testLoopResults.ReceiveCount < requiredReceiveCount)
        {
          try
          {
            DateTime currentTestTime = startTime.AddMilliseconds((double) ((long) testLoopResults.TestCount * (long) cycleMilliSeconds));
            if (currentTestTime <= DateTime.Now)
            {
              currentTestTime = DateTime.Now;
            }
            else
            {
              int waitMilliSeconds = (int) currentTestTime.Subtract(DateTime.Now).TotalMilliseconds;
              if (waitMilliSeconds > 50)
                await Task.Delay(waitMilliSeconds, radioDevice.CancelToken);
            }
            DateTime nextTestTime = currentTestTime.AddMilliseconds((double) cycleMilliSeconds);
            DateTime minNextTestTime = DateTime.Now.AddMilliseconds(300.0);
            if (nextTestTime < minNextTestTime)
              nextTestTime = minNextTestTime;
            ++testLoopResults.TestCount;
            bool done = false;
            this.NLOG_Trace("Start IUWS for receive one packet");
            S4_HandlerFunctions functions = this.MyFunctions;
            ProgressHandler progress1 = progress;
            CancellationToken cancelToken1 = cancelToken;
            // ISSUE: variable of a boxed type
            __Boxed<HandlerFunctionsForProduction.CommonDeviceModes> mode = (Enum) HandlerFunctionsForProduction.CommonDeviceModes.RadioTestReceiveTestPacket;
            ushort syncWord1 = radioDevice.TestParameters.SyncWord;
            int timeoutSeconds1 = (int) timeoutSeconds;
            int syncWord2 = (int) syncWord1;
            byte[] arbitraryData = testPacketBytes;
            await functions.SetModeAsync(progress1, cancelToken1, (Enum) mode, timeoutSeconds: (ushort) timeoutSeconds1, syncWord: (ushort) syncWord2, arbitraryData: arbitraryData);
            while (!done && DateTime.Now <= nextTestTime)
            {
              if (DateTime.Now > endTime)
                throw new TimeoutException("Timeout by test time over.");
              if (radioDevice.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.MinoConnect)
              {
                this.NLOG_Trace("SendTestPacket by MinoConnect done");
                radioDevice.SendTestPacket(deviceID, (byte) 7, radioDevice.TestParameters.SyncWord.ToString("X04"), testPacketString);
                await Task.Delay(100, radioDevice.CancelToken);
              }
              else
              {
                this.NLOG_Trace("SendTestPacket by IUWS done");
                await radioDevice.SendTestPacketAsync(progress, deviceID, (ushort) 2, sendPacketBytes);
              }
              ++testLoopResults.SendCount;
              progress.Report(tag: (object) testLoopResults);
              bool packageReceived = false;
              for (int p = 0; p < 2; ++p)
              {
                this.NLOG_Trace("Read IUWS state");
                S4_SystemState iuwState = await this.MyFunctions.GetDeviceState(progress, cancelToken);
                this.NLOG_Trace("IUWS state received");
                ++testLoopResults.PollingCount;
                if (iuwState.DeviceMode != S4_DeviceModes.RadioTestReceiveTestPacket)
                {
                  if (iuwState.DeviceMode == S4_DeviceModes.RadioTestReceiveTestPacketDone)
                  {
                    if (iuwState.ModeResultData != null && iuwState.ModeResultData.Length > 5)
                    {
                      int rssi = Util.RssiToRssi_dBm(iuwState.ModeResultData[0]);
                      testLoopResults.AddRSSI(rssi);
                      byte[] receivedPacketBytes = new byte[iuwState.ModeResultData.Length - 4];
                      Buffer.BlockCopy((Array) iuwState.ModeResultData, 2, (Array) receivedPacketBytes, 0, receivedPacketBytes.Length);
                      if (((IEnumerable<byte>) receivedPacketBytes).SequenceEqual<byte>((IEnumerable<byte>) testPacketBytes))
                      {
                        this.NLOG_Trace("Packet received and ok");
                        progress.Report("Packet received and ok");
                        done = true;
                        this.LogTestResult(radioDevice, false, RadioTestLog.ReceiveInfo.ok, rssi);
                      }
                      else
                      {
                        this.NLOG_Trace("Packet received, data different");
                        progress.Report("Packet data different. Use unique sync word for test benches!");
                        this.LogTestResult(radioDevice, false, RadioTestLog.ReceiveInfo.error);
                      }
                      receivedPacketBytes = (byte[]) null;
                    }
                    else
                    {
                      this.NLOG_Trace("Packet received, but no data");
                      progress.Report("Packet received but no data");
                      this.LogTestResult(radioDevice, false, RadioTestLog.ReceiveInfo.error);
                    }
                  }
                  else if (iuwState.DeviceMode == S4_DeviceModes.RadioTestReceiveTestPacketTimeout)
                  {
                    this.NLOG_Trace("Receive timeout by device");
                    progress.Report("Receive timeout by device");
                    this.LogTestResult(radioDevice, false, RadioTestLog.ReceiveInfo.timeout);
                  }
                  else
                  {
                    this.NLOG_Trace("Illegal state change");
                    progress.Report("Illegal state change");
                    this.LogTestResult(radioDevice, false, RadioTestLog.ReceiveInfo.error);
                  }
                  progress.Report(tag: (object) testLoopResults);
                  packageReceived = true;
                  break;
                }
                progress.Report(tag: (object) testLoopResults);
                iuwState = (S4_SystemState) null;
              }
              if (!packageReceived)
                this.LogTestResult(radioDevice, false, RadioTestLog.ReceiveInfo.timeout);
            }
          }
          catch (Exception ex)
          {
            if (ex is TaskCanceledException)
              throw ex;
            ++exceptionCount;
            if (exceptionCount > 2 || DateTime.Now > endTime)
            {
              Ex = ex;
              break;
            }
          }
        }
        if (testLoopResults.ReceiveCount < requiredReceiveCount)
          throw new Exception("ReceiveCount < RequiredReceiveCount");
        this.NLOG_Trace("Stop radio");
        await radioDevice.StopRadioAsync(progress);
        await this.MyFunctions.SetModeAsync(progress, new CancellationTokenSource().Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.OperationMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
        if (Ex != null)
          throw Ex;
        testPacketString = (string) null;
        testPacketBytes = (byte[]) null;
        sendPacketBytes = (byte[]) null;
        firmwareVersion = new FirmwareVersion();
      }
      catch (Exception ex)
      {
        if (!(ex is TaskCanceledException))
          throw new Exception("RadioReceiveTestAsync stopped by exception", ex);
      }
      RadioTestLoopResults testAsync = testLoopResults;
      Ex = (Exception) null;
      testLoopResults = (RadioTestLoopResults) null;
      return testAsync;
    }

    public async Task SetModeReceiveOnePacketAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort syncWord,
      int packedLength,
      ushort timeoutSeconds)
    {
      this.NLOG_Trace("Start IUWS for receive one packet");
      byte[] testPacketBytes = new byte[packedLength];
      S4_HandlerFunctions functions = this.MyFunctions;
      ProgressHandler progress1 = progress;
      CancellationToken cancelToken1 = cancelToken;
      // ISSUE: variable of a boxed type
      __Boxed<HandlerFunctionsForProduction.CommonDeviceModes> mode = (Enum) HandlerFunctionsForProduction.CommonDeviceModes.RadioTestReceiveTestPacket;
      ushort num = syncWord;
      int timeoutSeconds1 = (int) timeoutSeconds;
      int syncWord1 = (int) num;
      byte[] arbitraryData = testPacketBytes;
      await functions.SetModeAsync(progress1, cancelToken1, (Enum) mode, timeoutSeconds: (ushort) timeoutSeconds1, syncWord: (ushort) syncWord1, arbitraryData: arbitraryData);
      testPacketBytes = (byte[]) null;
    }

    public async Task<string> GetReceiveOnePacketResultAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.NLOG_Trace("IUWS state called");
      S4_SystemState iuwState = await this.MyFunctions.checkedNfcCommands.GetDeviceStatesAsync(progress, cancelToken);
      this.NLOG_Trace("IUWS state received");
      StringBuilder result = new StringBuilder();
      if (iuwState.DeviceMode != S4_DeviceModes.RadioTestReceiveTestPacket)
      {
        if (iuwState.DeviceMode == S4_DeviceModes.RadioTestReceiveTestPacketDone)
        {
          if (iuwState.ModeResultData != null && iuwState.ModeResultData.Length > 5)
          {
            byte[] receivedPacketBytes = new byte[iuwState.ModeResultData.Length - 4];
            Buffer.BlockCopy((Array) iuwState.ModeResultData, 2, (Array) receivedPacketBytes, 0, receivedPacketBytes.Length);
            this.NLOG_Debug("Received packet == testPacket");
            result.AppendLine("RSSI: " + Util.RssiToRssi_dBm(iuwState.ModeResultData[0]).ToString());
            result.AppendLine("Received: " + Util.ByteArrayToHexString(receivedPacketBytes));
            receivedPacketBytes = (byte[]) null;
          }
          else
            result.AppendLine("Packet received but no data");
        }
        else if (iuwState.DeviceMode == S4_DeviceModes.RadioTestReceiveTestPacketTimeout)
          progress.Report("Receive timeout by device");
        else
          progress.Report("Reset from mode: " + iuwState.DeviceMode.ToString());
      }
      else
        result.AppendLine("No packet received");
      string packetResultAsync = result.ToString();
      iuwState = (S4_SystemState) null;
      result = (StringBuilder) null;
      return packetResultAsync;
    }

    public string SendOnePacketByMiCon(
      ProgressHandler progress,
      CancellationToken cancelToken,
      RadioTestByDevice radioDevice)
    {
      if (radioDevice.TestParameters.TestDevice != 0)
        throw new Exception("Only MinoConnect as test device prepared");
      StringBuilder stringBuilder = new StringBuilder();
      uint randomDeviceId = radioDevice.GetRandomDeviceID();
      string conRadioTestPacket = CommunicationByMinoConnect.GetMiConRadioTestPacket(randomDeviceId);
      string str = radioDevice.TestParameters.SyncWord.ToString("X04");
      radioDevice.SendTestPacket(randomDeviceId, (byte) 7, radioDevice.TestParameters.SyncWord.ToString("X04"), conRadioTestPacket);
      this.NLOG_Trace("SendTestPacket by MinoConnect done");
      byte[] bytes = Encoding.ASCII.GetBytes(conRadioTestPacket);
      stringBuilder.AppendLine("SyncWord: 0x" + str);
      stringBuilder.AppendLine("TestPacket: 0x" + Util.ByteArrayToHexString(bytes));
      stringBuilder.AppendLine("Required receive count: " + bytes.Length.ToString());
      stringBuilder.AppendLine("SendTestPacket by MinoConnect done");
      return stringBuilder.ToString();
    }

    public async Task<int> GetFrequencyIncrementAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int frequencyIncrementAsync = await this.MyFunctions.myDeviceCommands.CMDs_Radio.GetFrequencyIncrementAsync(progress, cancelToken);
      return frequencyIncrementAsync;
    }

    public async Task SetFrequencyIncrementAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      int frequency_Hz)
    {
      await this.MyFunctions.myDeviceCommands.CMDs_Radio.SetFrequencyIncrementAsync(frequency_Hz, progress, cancelToken);
    }

    public async Task SetCenterFrequencyAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint frequency_Hz)
    {
      await this.MyFunctions.myDeviceCommands.CMDs_Radio.SetCenterFrequencyAsync(frequency_Hz, progress, cancelToken);
    }

    public async Task<uint> GetCenterFrequencyAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      uint centerFrequencyAsync = await this.MyFunctions.myDeviceCommands.CMDs_Radio.GetCenterFrequencyAsync(progress, cancelToken);
      return centerFrequencyAsync;
    }

    public async Task SetRadioPowerAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint radioPower)
    {
      uint centerFreq = await this.GetCenterFrequencyAsync(progress, cancelToken);
      this.MyFunctions.myMeters.ConnectedMeter.meterMemory.SetParameterValue<uint>(S4_Params.cfg_radio_power, radioPower);
      AddressRange adrrange = this.MyFunctions.myMeters.ConnectedMeter.meterMemory.GetAddressRange(S4_Params.cfg_radio_power.ToString());
      await this.MyFunctions.myDeviceCommands.WriteMemoryAsync(adrrange, (DeviceMemory) this.MyFunctions.myMeters.ConnectedMeter.meterMemory, progress, cancelToken);
      await this.SetCenterFrequencyAsync(progress, cancelToken, centerFreq);
      adrrange = (AddressRange) null;
    }

    public async Task<uint> GetRadioPowerAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      AddressRange adrrange = this.MyFunctions.myMeters.ConnectedMeter.meterMemory.GetAddressRange(S4_Params.cfg_radio_power.ToString());
      await this.MyFunctions.myDeviceCommands.ReadMemoryAsync(adrrange, (DeviceMemory) this.MyFunctions.myMeters.ConnectedMeter.meterMemory, progress, cancelToken);
      uint parameterValue = this.MyFunctions.myMeters.ConnectedMeter.meterMemory.GetParameterValue<uint>(S4_Params.cfg_radio_power);
      adrrange = (AddressRange) null;
      return parameterValue;
    }

    public async Task SendUnmodulatedCarrier(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort timeoutSeconds)
    {
      await this.MyFunctions.SetModeAsync(progress, cancelToken, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitUnmodulatedCarrier, timeoutSeconds: timeoutSeconds);
    }

    public async Task SendModulatedCarrier(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort timeoutSeconds)
    {
      await this.MyFunctions.SetModeAsync(progress, cancelToken, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitModulatedCarrier, timeoutSeconds: timeoutSeconds);
    }

    public void ClearTestResultLog() => this.TestResultLog = new RadioTestLog();

    public void ShowLog() => this.TestResultLog.ShowLog("IUWS Radio Tests");

    public void LogTestResult(
      RadioTestByDevice radioDevice,
      bool DUT_transmit,
      RadioTestLog.ReceiveInfo receiveInfo,
      int RSSI = 0)
    {
      RadioTestLog.RadioTestDirection direction = radioDevice.TestParameters.TestDevice != RadioTestByDevice.RadioTestDevice.MinoConnect ? (!DUT_transmit ? RadioTestLog.RadioTestDirection.IUWS_To_DUT : RadioTestLog.RadioTestDirection.DUT_To_IUWS) : (!DUT_transmit ? RadioTestLog.RadioTestDirection.MinoConnect_To_DUT : RadioTestLog.RadioTestDirection.DUT_To_MinoConnect);
      if (receiveInfo == RadioTestLog.ReceiveInfo.ok)
        S4_RadioTests.S4_RadioTestResultLogger.Trace(direction.ToString() + "\t" + radioDevice.TestParameters.TestFrequency.ToString("0.00") + "\t" + receiveInfo.ToString() + "\t" + RSSI.ToString());
      else
        S4_RadioTests.S4_RadioTestResultLogger.Trace(direction.ToString() + "\t" + radioDevice.TestParameters.TestFrequency.ToString("0.00") + "\t" + receiveInfo.ToString());
      if (this.TestResultLog == null)
        return;
      this.TestResultLog.AddTest(radioDevice.TestParameters.TestFrequency, direction, receiveInfo, RSSI);
    }
  }
}
