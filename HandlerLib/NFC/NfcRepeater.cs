// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NfcRepeater
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using GmmDbLib;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib.NFC
{
  public class NfcRepeater
  {
    private static Logger BaseNfcRepeaterLogger = LogManager.GetLogger(nameof (NfcRepeaterLogger));
    public ChannelLogger NfcRepeaterLogger;
    internal CommunicationPortFunctions ci;
    private ushort? CrcInitValueSaved;
    public readonly ConfigList myConfig;
    public Common32BitCommands IrDaCommands = (Common32BitCommands) null;
    public NfcRepeater.IrDaCommandWrapper IrDaWrapper = NfcRepeater.IrDaCommandWrapper.Non;
    public DownLinkTestWindow DownLinkWindow;

    public ushort? CrcInitValue { get; private set; }

    public int TotalDeviceRepeats { get; private set; }

    public NfcRepeater(CommunicationPortFunctions myPort)
    {
      this.ci = myPort;
      this.myConfig = myPort.GetReadoutConfiguration();
      this.NfcRepeaterLogger = new ChannelLogger(NfcRepeater.BaseNfcRepeaterLogger, this.myConfig);
    }

    public void ClearCrcInitValue()
    {
      this.CrcInitValue = new ushort?();
      this.TotalDeviceRepeats = 0;
    }

    public void SetCrcInitValue(uint MeterID)
    {
      this.CrcInitValue = new ushort?((ushort) (MeterID ^ MeterID >> 16));
      this.CrcInitValueSaved = this.CrcInitValue;
      this.TotalDeviceRepeats = 0;
    }

    public async Task GetResultFrameAsync(
      NfcFrame nfcFrame,
      ProgressHandler progress,
      CancellationToken cancelToken,
      int readTimeOffset = 0)
    {
      if (cancelToken.IsCancellationRequested)
        throw new TaskCanceledException();
      await Task.Run((Action) (() => this.GetResultFrame(nfcFrame, progress, cancelToken, readTimeOffset)));
    }

    public void GetResultFrame(
      NfcFrame nfcFrame,
      ProgressHandler progres,
      CancellationToken cancelToken,
      int readTimeOffset = 0)
    {
      if (readTimeOffset < 0)
        readTimeOffset = 0;
      if (this.IrDaCommands != null)
        this.GetIrDaResultFrame(nfcFrame, progres, cancelToken, readTimeOffset);
      else if (this.IrDaWrapper == NfcRepeater.IrDaCommandWrapper.DownlinkTest && this.DownLinkWindow != null)
      {
        byte[] numArray;
        if (nfcFrame.NfcCommand == NfcCommands.IrDa_Compatible_Command)
        {
          numArray = new byte[nfcFrame.NfcRequestFrame.Length - 4];
          Buffer.BlockCopy((Array) nfcFrame.NfcRequestFrame, 2, (Array) numArray, 0, numArray.Length);
        }
        else
        {
          byte[] frameData = NfcFrame.GetFrameData(nfcFrame.NfcRequestFrame);
          numArray = new byte[frameData.Length + 1];
          numArray[0] = (byte) 56;
          Buffer.BlockCopy((Array) frameData, 0, (Array) numArray, 1, frameData.Length);
        }
        if (!this.CrcInitValueSaved.HasValue)
          throw new Exception("CrcInitValueSaved not prepared for DownlinkTest");
        byte[] byteArray = Util.HexStringToByteArray("FFA55AF5AF");
        byte[] frameData1 = new byte[2 + byteArray.Length];
        frameData1[0] = (byte) 53;
        frameData1[1] = (byte) 9;
        byteArray.CopyTo((Array) frameData1, 2);
        NfcFrame nfcFrame1 = new NfcFrame(NfcCommands.IrDa_Compatible_Command, frameData1, "Send confirmed data", this.CrcInitValueSaved);
        byte[] uplinkBytes = this.DownLinkWindow.GetUplinkBytes(numArray, new DownLinkTestWindow.SendProtocol(this.TransmitAndGetResultFrame), nfcFrame1, progres, cancelToken);
        if (uplinkBytes == null)
          throw new OperationCanceledException();
        if (nfcFrame.NfcCommand == NfcCommands.IrDa_Compatible_Command)
        {
          nfcFrame.NfcResponseFrame = new byte[uplinkBytes.Length + 4];
          uplinkBytes.CopyTo((Array) nfcFrame.NfcResponseFrame, 2);
          nfcFrame.NfcResponseFrame[0] = (byte) (nfcFrame.NfcResponseFrame.Length - 3);
          nfcFrame.NfcResponseFrame[1] = (byte) 32;
          BitConverter.GetBytes(NfcFrame.createCRC(nfcFrame.NfcResponseFrame, this.CrcInitValue)).CopyTo((Array) nfcFrame.NfcResponseFrame, nfcFrame.NfcResponseFrame.Length - 2);
        }
        else
        {
          nfcFrame.NfcResponseFrame = new byte[uplinkBytes.Length + 2];
          nfcFrame.NfcResponseFrame[0] = (byte) (nfcFrame.NfcResponseFrame.Length - 3);
          Buffer.BlockCopy((Array) uplinkBytes, 1, (Array) nfcFrame.NfcResponseFrame, 1, uplinkBytes.Length - 1);
          BitConverter.GetBytes(NfcFrame.createCRC(nfcFrame.NfcResponseFrame, this.CrcInitValue)).CopyTo((Array) nfcFrame.NfcResponseFrame, nfcFrame.NfcResponseFrame.Length - 2);
        }
      }
      else
      {
        if (nfcFrame.NfcCommand != NfcCommands.IrDa_Compatible_Command)
        {
          NfcFrame nfcFrame2 = (NfcFrame) null;
          if (this.IrDaWrapper == NfcRepeater.IrDaCommandWrapper.SendNfcCommand)
          {
            byte[] frameData = NfcFrame.GetFrameData(nfcFrame.NfcRequestFrame);
            byte[] numArray = new byte[frameData.Length + 1];
            numArray[0] = (byte) 56;
            Buffer.BlockCopy((Array) frameData, 0, (Array) numArray, 1, frameData.Length);
            nfcFrame2 = frameData[0] != (byte) 1 ? new NfcFrame(NfcCommands.IrDa_Compatible_Command, numArray, this.myConfig.ReadingChannelIdentification, this.CrcInitValue) : new NfcFrame(NfcCommands.IrDa_Compatible_Command, numArray, this.myConfig.ReadingChannelIdentification, this.CrcInitValueSaved);
            nfcFrame2.ProgressParameter = nfcFrame.ProgressParameter;
          }
          else if (this.IrDaWrapper == NfcRepeater.IrDaCommandWrapper.SendToNfcDevice)
          {
            byte[] nfcRequestFrame = nfcFrame.NfcRequestFrame;
            byte[] numArray = new byte[nfcRequestFrame.Length + 2];
            numArray[0] = (byte) 54;
            numArray[1] = (byte) 11;
            Buffer.BlockCopy((Array) nfcRequestFrame, 0, (Array) numArray, 2, nfcRequestFrame.Length);
            nfcFrame2 = new NfcFrame(NfcCommands.IrDa_Compatible_Command, numArray, this.myConfig.ReadingChannelIdentification, this.CrcInitValue);
          }
          if (nfcFrame2 != null)
          {
            this.TransmitAndGetResultFrame(nfcFrame2, progres, cancelToken, readTimeOffset);
            nfcFrame.NfcResponseFrame = new byte[nfcFrame2.NfcResponseFrame.Length - 2];
            if (nfcFrame2.NfcResponseFrame[0] == byte.MaxValue)
            {
              ushort num = (ushort) ((uint) BitConverter.ToUInt16(nfcFrame2.NfcResponseFrame, 1) - 2U);
              nfcFrame.NfcResponseFrame[0] = byte.MaxValue;
              Buffer.BlockCopy((Array) BitConverter.GetBytes(num), 0, (Array) nfcFrame.NfcResponseFrame, 1, 2);
              Buffer.BlockCopy((Array) nfcFrame2.NfcResponseFrame, 5, (Array) nfcFrame.NfcResponseFrame, 3, nfcFrame.NfcResponseFrame.Length - 3);
              return;
            }
            nfcFrame.NfcResponseFrame[0] = (byte) ((uint) nfcFrame2.NfcResponseFrame[0] - 2U);
            Buffer.BlockCopy((Array) nfcFrame2.NfcResponseFrame, 3, (Array) nfcFrame.NfcResponseFrame, 1, nfcFrame.NfcResponseFrame.Length - 1);
            return;
          }
        }
        this.TransmitAndGetResultFrame(nfcFrame, progres, cancelToken, readTimeOffset);
      }
    }

    public void TransmitAndGetResultFrame(
      NfcFrame nfcFrame,
      ProgressHandler progres,
      CancellationToken cancelToken,
      int readTimeOffset = 0)
    {
      int? nullable = new int?();
      try
      {
        int num1 = (nfcFrame.NfcRequestFrame.Length - 64) * 2;
        if (num1 < 0)
          num1 = 0;
        if (num1 > 0 || readTimeOffset > 0)
        {
          nullable = new int?(num1 + readTimeOffset);
          this.ci.GetReadoutConfiguration().RecTime_BeforFirstByte += nullable.Value;
        }
        for (int index = 0; index < this.myConfig.MaxRequestRepeat; ++index)
        {
          if (cancelToken.IsCancellationRequested)
            throw new TaskCanceledException();
          try
          {
            this.NfcRepeaterLogger.Trace("NFC transmit: " + Util.ByteArrayToHexStringFormated(nfcFrame.NfcRequestFrame));
            this.ci.DiscardInBuffer();
            this.ci.Write(nfcFrame.NfcRequestFrame);
            int num2 = 3;
            byte[] numArray = this.ci.ReadHeader(num2);
            this.NfcRepeaterLogger.Trace("NFC header received: " + Util.ByteArrayToHexStringFormated(numArray));
            int length = numArray[0] == byte.MaxValue ? (int) BitConverter.ToUInt16(numArray, 1) + 5 : (int) numArray[0] + 3;
            byte[] src = this.ci.ReadEnd(length - num2);
            nfcFrame.NfcResponseFrame = new byte[length];
            Buffer.BlockCopy((Array) numArray, 0, (Array) nfcFrame.NfcResponseFrame, 0, numArray.Length);
            Buffer.BlockCopy((Array) src, 0, (Array) nfcFrame.NfcResponseFrame, num2, src.Length);
            this.NfcRepeaterLogger.Trace("NFC frame received: " + Util.ByteArrayToHexStringFormated(nfcFrame.NfcResponseFrame));
            nfcFrame.IsResponseErrorMsg();
            nfcFrame.IsResponseCrcOk();
            if (nfcFrame.ProgressParameter == null)
              break;
            progres.Report(nfcFrame.NfcCommand.ToString() + nfcFrame.ProgressParameter);
            break;
          }
          catch (TaskCanceledException ex)
          {
            throw ex;
          }
          catch (DeviceMessageException ex)
          {
            this.NfcRepeaterLogger.Error("DeviceMessageException");
            throw ex;
          }
          catch (NfcFrameException ex)
          {
            this.NfcRepeaterLogger.Error("NFC_Frame Exception");
            if (index >= this.myConfig.MaxRequestRepeat - 1)
              throw ex;
          }
          catch (TimeoutException ex)
          {
            this.NfcRepeaterLogger.Info("Timeout", (Exception) ex);
            if (index >= this.myConfig.MaxRequestRepeat - 1)
              throw new TimeoutException(Ot.Gtm(Tg.CommunicationLogic, "NfcReadTimeout", "Nfc read timeout"), (Exception) ex);
          }
          catch (Exception ex)
          {
            this.NfcRepeaterLogger.Trace("NfcRepeaterException: " + ex.ToString());
            if (index >= this.myConfig.MaxRequestRepeat - 1)
              throw new Exception("NfcRepeaterException", ex);
          }
          Thread.Sleep(this.myConfig.WaitBeforeRepeatTime);
          ++this.TotalDeviceRepeats;
        }
      }
      finally
      {
        if (nullable.HasValue)
          this.ci.GetReadoutConfiguration().RecTime_BeforFirstByte -= nullable.Value;
      }
    }

    public async Task TransmitFrameAsync(
      NfcFrame nfcFrame,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitFrameFromTask(nfcFrame, progress, cancelToken)));
    }

    private void TransmitFrameFromTask(
      NfcFrame nfcFrame,
      ProgressHandler progres,
      CancellationToken cancelToken)
    {
      this.NfcRepeaterLogger.Trace("NFC transmit: " + Util.ByteArrayToHexStringFormated(nfcFrame.NfcRequestFrame));
      this.ci.Write(nfcFrame.NfcRequestFrame);
    }

    private void GetIrDaResultFrame(
      NfcFrame nfcFrame,
      ProgressHandler progress,
      CancellationToken cancelToken,
      int readTimeOffset)
    {
      byte[] nfcRequestFrame = nfcFrame.NfcRequestFrame;
      if (this.IrDaCommands.DeviceCMD.ConnectedReducedID == null)
        this.IrDaCommands.DeviceCMD.ReadVersion(progress, cancelToken);
      byte[] data = this.IrDaCommands.TransmitAndGetData(Manufacturer_FC.SpecialCommands_0x36, new byte?((byte) 11), nfcRequestFrame, false, progress, cancelToken);
      nfcFrame.NfcResponseFrame = data;
    }

    private enum UsedConfigSettings
    {
      MaxRequestRepeat,
      WaitBeforeRepeatTime,
    }

    public enum IrDaCommandWrapper
    {
      Non,
      SendToNfcDevice,
      SendNfcCommand,
      DownlinkTest,
    }
  }
}
