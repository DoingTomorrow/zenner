// Decompiled with JetBrains decompiler
// Type: HandlerLib.RadioTestByDevice
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort;
using CommunicationPort.Functions;
using NLog;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class RadioTestByDevice
  {
    internal static Logger RadioTestByDeviceLogger = LogManager.GetLogger(nameof (RadioTestByDevice));
    private CommunicationPortFunctions RadioMinoConnectCommunication;
    private CommunicationByMinoConnect MiCon;
    private NfcDeviceCommands NfcCmd;
    private ConfigList RadioDeviceCannelConfigList;
    public DeviceIdentification devIdent;
    private const int PollingMs = 200;
    private Random DeviceID_Random;
    public RadioTestParameters TestParameters;
    private string RadioCOMPort;
    private double LastFrequencyMHz = 0.0;

    public CancellationToken CancelToken
    {
      get => this.MiCon.CancelToken;
      set => this.MiCon.CancelToken = value;
    }

    public RadioTestByDevice(string radioTestComPort, RadioTestParameters testParameters = null)
    {
      this.RadioCOMPort = radioTestComPort;
      this.devIdent = (DeviceIdentification) null;
      this.TestParameters = testParameters != null ? testParameters : new RadioTestParameters();
      if (this.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.MinoConnect)
      {
        if (this.TestParameters.TestFrequency != 868.3 && this.TestParameters.TestFrequency != 868.95)
          throw new Exception("MinoConnect test only supported for 868.3 and 868.95 MHz");
        this.RadioDeviceCannelConfigList = ReadoutPreferences.GetConfigListFromProfileId(32);
      }
      else
      {
        if (this.TestParameters.TestDevice != RadioTestByDevice.RadioTestDevice.IUWS)
          throw new ArgumentException("TestDevice");
        this.RadioDeviceCannelConfigList = ReadoutPreferences.GetConfigListFromProfileId(344);
      }
      this.RadioDeviceCannelConfigList.Port = this.RadioCOMPort;
      this.RadioDeviceCannelConfigList.MinoConnectPowerOffTime = 0;
      this.RadioMinoConnectCommunication = new CommunicationPortFunctions();
      this.RadioMinoConnectCommunication.SetReadoutConfiguration(this.RadioDeviceCannelConfigList);
      this.MiCon = this.RadioMinoConnectCommunication.GetCommunicationByMinoConnect();
      this.OpenMinoConnect();
      this.DeviceID_Random = new Random(DateTime.Now.Millisecond);
      if (this.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.IUWS)
        this.NfcCmd = new NfcDeviceCommands(this.RadioMinoConnectCommunication);
      this.NLOG_Trace("RadioTestByDevice initialised for TestDevice: " + this.TestParameters.TestDevice.ToString());
    }

    private void NLOG_Trace(string traceMessage)
    {
      RadioTestByDevice.RadioTestByDeviceLogger.Trace(this.RadioDeviceCannelConfigList.ReadingChannelIdentification + ": " + traceMessage);
    }

    private void NLOG_Debug(string traceMessage)
    {
      RadioTestByDevice.RadioTestByDeviceLogger.Debug(this.RadioDeviceCannelConfigList.ReadingChannelIdentification + ": " + traceMessage);
    }

    private void NLOG_Info(string traceMessage)
    {
      RadioTestByDevice.RadioTestByDeviceLogger.Info(this.RadioDeviceCannelConfigList.ReadingChannelIdentification + ": " + traceMessage);
    }

    public CommunicationPortFunctions GetMinoConnectPortFunctions()
    {
      return this.RadioMinoConnectCommunication;
    }

    public void OpenMinoConnect() => this.MiCon.Open();

    public void CloseMinoConnect() => this.MiCon.Close();

    public void SetChannelName(string channelName)
    {
      this.RadioDeviceCannelConfigList.ReadingChannelIdentification = channelName;
    }

    public uint GetRandomDeviceID() => (uint) this.DeviceID_Random.Next(0, 99999999);

    public string GetMiConDeviceInfo() => this.MiCon.GetDeviceInfo();

    public string GetMinoDeviceInfo() => this.RadioMinoConnectCommunication.TransceiverDeviceInfo;

    public async Task SetTestParameterAsync(ProgressHandler progress, bool SetFrequency)
    {
      if (this.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.MinoConnect)
      {
        if (this.TestParameters.TestFrequency == 868.3)
          return;
        if (this.TestParameters.TestFrequency != 868.95)
          throw new Exception("MinoConnect only supports 868.3 and 868.95 MHz");
        if (this.MiCon.VersionValue < 2.603M)
          throw new Exception("MinoConnect version < 2.6.3 doesn't support 868.95 MHz");
      }
      else
      {
        if (this.TestParameters.TestDevice != RadioTestByDevice.RadioTestDevice.IUWS)
          throw new Exception("Test device not supported");
        if (!this.NfcCmd.myNfcRepeater.CrcInitValue.HasValue)
        {
          DeviceIdentification deviceIdentification = await this.NfcCmd.ReadVersionAsync(progress, this.CancelToken);
          this.devIdent = deviceIdentification;
          deviceIdentification = (DeviceIdentification) null;
        }
        if (!SetFrequency || this.LastFrequencyMHz == this.TestParameters.TestFrequency)
          return;
        await this.NfcCmd.SetCenterFrequencyMHz(progress, this.CancelToken, this.TestParameters.TestFrequency);
        this.LastFrequencyMHz = this.TestParameters.TestFrequency;
      }
    }

    public async Task StopRadioAsync(ProgressHandler progress)
    {
      if (this.MiCon == null)
        return;
      if (this.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.MinoConnect)
        this.MiCon.StopRadio();
      else
        await this.NfcCmd.SetModeAsync(S4_DeviceModes.OperationMode, progress, this.CancelToken);
    }

    public RadioTestResult ReceiveOnePacket(int serialnumber, ushort timeoutInSec, string syncWord)
    {
      if (this.TestParameters.TestDevice != 0)
        throw new Exception("Function only allowed for MinoConnect as test device");
      if (this.TestParameters.TestFrequency == 868.3)
        return this.MiCon.ReceiveOnePacket(RadioMode.Radio3, serialnumber, timeoutInSec, syncWord);
      if (this.TestParameters.TestFrequency == 868.95)
        return this.MiCon.ReceiveOnePacket(RadioMode.Radio3_868_95, serialnumber, timeoutInSec, syncWord);
      throw new Exception("Not supported radio frequency");
    }

    public void SendTestPacket(uint deviceID, byte power, string syncWord, string testPacket)
    {
      if (this.TestParameters.TestDevice != 0)
        throw new Exception("Function only allowed for MinoConnect as test device");
      if (this.TestParameters.TestFrequency == 868.3)
      {
        this.MiCon.SendTestPacket(deviceID, RadioMode.Radio3, power, syncWord, testPacket);
      }
      else
      {
        if (this.TestParameters.TestFrequency != 868.95)
          throw new Exception("Not supported radio frequency");
        this.MiCon.SendTestPacket(deviceID, RadioMode.Radio3_868_95, power, syncWord, testPacket);
      }
    }

    public async Task<RadioTestResult> ReceiveOnePacketAsync(
      ProgressHandler progress,
      uint deviceID,
      ushort timeoutSeconds,
      byte[] testPacket)
    {
      this.NLOG_Trace("ReceiveOnePacketAsync started");
      if (this.TestParameters.TestDevice != RadioTestByDevice.RadioTestDevice.IUWS)
        throw new Exception("Function only allowed for IUWS as test device");
      if (timeoutSeconds > (ushort) byte.MaxValue)
        throw new ArgumentException("Timeout > 255 not allowed.");
      Exception localException = (Exception) null;
      RadioTestResult testResults = new RadioTestResult();
      try
      {
        List<byte> modeParameter = new List<byte>();
        modeParameter.Add((byte) testPacket.Length);
        modeParameter.Add((byte) ((uint) this.TestParameters.SyncWord >> 8));
        modeParameter.Add((byte) this.TestParameters.SyncWord);
        modeParameter.Add((byte) timeoutSeconds);
        await this.NfcCmd.SetModeAsync(S4_DeviceModes.RadioTestReceiveTestPacket, progress, this.CancelToken, modeParameter.ToArray());
        DateTime timeoutTime = DateTime.Now.AddSeconds((double) timeoutSeconds);
        while (DateTime.Now <= timeoutTime)
        {
          this.NLOG_Trace("IUWS state called");
          S4_SystemState iuwState = await this.NfcCmd.GetDeviceStatesAsync(progress, this.CancelToken);
          this.NLOG_Trace("IUWS state received");
          if (iuwState.DeviceMode != S4_DeviceModes.RadioTestReceiveTestPacket)
          {
            if (iuwState.DeviceMode == S4_DeviceModes.RadioTestReceiveTestPacketDone)
            {
              if (iuwState.ModeResultData != null && iuwState.ModeResultData.Length > 5)
              {
                byte[] receivedPacketBytes = new byte[iuwState.ModeResultData.Length - 4];
                Buffer.BlockCopy((Array) iuwState.ModeResultData, 2, (Array) receivedPacketBytes, 0, receivedPacketBytes.Length);
                if (((IEnumerable<byte>) receivedPacketBytes).SequenceEqual<byte>((IEnumerable<byte>) testPacket))
                {
                  this.NLOG_Debug("Received packet == testPacket");
                  testResults.RSSI = Util.RssiToRssi_dBm(iuwState.ModeResultData[0]);
                  testResults.Payload = receivedPacketBytes;
                }
                else
                  this.NLOG_Info("Received packet != testPacket");
                receivedPacketBytes = (byte[]) null;
                break;
              }
              progress.Report("Packet received but no data");
              break;
            }
            if (iuwState.DeviceMode == S4_DeviceModes.RadioTestReceiveTestPacketTimeout)
            {
              progress.Report("Receive timeout by device");
              break;
            }
            progress.Report("Illegal state change");
            break;
          }
          iuwState = (S4_SystemState) null;
        }
        modeParameter = (List<byte>) null;
      }
      catch (Exception ex)
      {
        localException = new Exception("Receive packed by TestDevice", ex);
      }
      this.NLOG_Trace("ReceiveOnePacketAsync finished");
      if (localException == null)
        return testResults.Payload == null ? (RadioTestResult) null : testResults;
      if (localException.InnerException is TaskCanceledException)
        return (RadioTestResult) null;
      throw localException;
    }

    public async Task SendTestPacketAsync(
      ProgressHandler progress,
      uint deviceID,
      ushort timeoutSeconds,
      byte[] sendPacketBytes)
    {
      this.NLOG_Trace("SendTestPacketAsync started");
      if (this.TestParameters.TestDevice != RadioTestByDevice.RadioTestDevice.IUWS)
        throw new Exception("Function only allowed for IUWS as test device");
      List<byte> modeParameter = new List<byte>();
      modeParameter.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) 0));
      modeParameter.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeoutSeconds));
      modeParameter.AddRange((IEnumerable<byte>) sendPacketBytes);
      await this.NfcCmd.SetModeAsync(S4_DeviceModes.RadioTestSendTestPacket, progress, this.CancelToken, modeParameter.ToArray());
      this.NLOG_Trace("SendTestPacketAsync finished");
      modeParameter = (List<byte>) null;
    }

    public async Task<DeviceIdentification> ReadDeviceIdentification(
      ProgressHandler progress,
      CancellationToken token)
    {
      if (this.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.IUWS && !this.NfcCmd.myNfcRepeater.CrcInitValue.HasValue)
      {
        DeviceIdentification deviceIdentification = await this.NfcCmd.ReadVersionAsync(progress, token);
        this.devIdent = deviceIdentification;
        deviceIdentification = (DeviceIdentification) null;
      }
      return this.devIdent;
    }

    public enum RadioTestDevice
    {
      MinoConnect,
      IUWS,
    }
  }
}
