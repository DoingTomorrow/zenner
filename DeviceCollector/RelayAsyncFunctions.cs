// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RelayAsyncFunctions
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using System;
using System.Collections;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  internal sealed class RelayAsyncFunctions : 
    IAsyncFunctions,
    I_ZR_Component,
    ILockable,
    ICancelable,
    IReadoutConfig
  {
    private int offset;

    public MbusTelegram MbusTelegramToRead { get; set; }

    public bool BreakRequest { get; set; }

    public bool Open() => true;

    public bool Close()
    {
      this.MbusTelegramToRead = (MbusTelegram) null;
      this.offset = 0;
      return true;
    }

    public bool TransmitBlock(ref ByteField DataBlock)
    {
      this.offset = 0;
      return this.MbusTelegramToRead != null;
    }

    public bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first)
    {
      if (this.MbusTelegramToRead == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, "RelayAsyncFunction simulate time out exception!");
        return false;
      }
      for (int offset = this.offset; offset < MinByteNb + this.offset; ++offset)
        DataBlock.Add(this.MbusTelegramToRead.Buffer[offset]);
      this.offset += DataBlock.Count;
      return true;
    }

    public event System.EventHandler ConnectionLost;

    public event System.EventHandler BatterieLow;

    public void ResetLastTransmitEndTime()
    {
    }

    public long InputBufferLength => 0;

    public bool GetCurrentInputBuffer(out byte[] buffer)
    {
      buffer = (byte[]) null;
      return false;
    }

    public SortedList<AsyncComSettings, object> GetAsyncComSettings()
    {
      return (SortedList<AsyncComSettings, object>) null;
    }

    public string GetAsyncComSettingsAsString() => "";

    public bool SendMinoConnectCommand(string cmd) => false;

    public bool TransmitBlock(byte[] buffer) => false;

    public bool TransmitBlock(byte[] buffer, bool isLogDisabled) => false;

    public Dictionary<string, string> LoadAvailableCOMservers()
    {
      throw new NotImplementedException();
    }

    public bool IsOpen => true;

    public bool UpdateTransceiverFirmware(string pathToFirmware)
    {
      throw new NotImplementedException();
    }

    public DateTime FirstCalculatedEarliestTransmitTime { get; set; }

    public ZR_ClassLibrary.TransceiverDevice Transceiver { get; set; }

    public bool TryReceiveBlock(out byte[] buffer) => throw new NotImplementedException();

    public bool TryReceiveBlock(out byte[] buffer, int numberOfBytesToReceive)
    {
      throw new NotImplementedException();
    }

    public string ShowComWindow(string ComponentList) => throw new NotImplementedException();

    public void ShowComWindow() => throw new NotImplementedException();

    public bool ShowComWindowChanged() => throw new NotImplementedException();

    public void ClearCom() => throw new NotImplementedException();

    public bool SetBreak() => throw new NotImplementedException();

    public bool ClearBreak() => throw new NotImplementedException();

    public bool CallTransceiverFunction(TransceiverDeviceFunction function)
    {
      throw new NotImplementedException();
    }

    public bool CallTransceiverFunction(TransceiverDeviceFunction function, object param1)
    {
      throw new NotImplementedException();
    }

    public bool CallTransceiverFunction(
      TransceiverDeviceFunction function,
      object param1,
      object param2)
    {
      throw new NotImplementedException();
    }

    public bool GetComPortIds(out string strComPortIds, bool ForceRefresh)
    {
      throw new NotImplementedException();
    }

    public string GetTranceiverDeviceInfo() => throw new NotImplementedException();

    public void ShowErrorMessageBox(bool on) => throw new NotImplementedException();

    public string SingleParameter(string ParameterName, string ParameterValue)
    {
      throw new NotImplementedException();
    }

    public string SingleParameter(CommParameter Parameter, string ParameterValue) => string.Empty;

    public void GetCommParameter(ref ArrayList ParameterList)
    {
    }

    public bool SetCommParameter(ArrayList ParameterList) => true;

    public bool SetCommParameter(ArrayList ParameterList, bool ComWindowRefresh)
    {
      throw new NotImplementedException();
    }

    public void WaitToEarliestTransmitTime()
    {
    }

    public void ResetEarliestTransmitTime()
    {
    }

    public void ClearWakeup()
    {
    }

    public void TriggerWakeup()
    {
    }

    public bool SetHandshakeState(HandshakeStates HandshakeState)
    {
      throw new NotImplementedException();
    }

    public void SetAnswerOffsetTime(int NewAnswerOffsetTime) => throw new NotImplementedException();

    public bool TransmitBlock(string DataString) => throw new NotImplementedException();

    public bool SendBlock(ref ByteField DataBlock) => throw new NotImplementedException();

    public bool TransmitString(string DataString) => throw new NotImplementedException();

    public bool ReceiveString(out string DataString) => throw new NotImplementedException();

    public bool ReceiveBlock(ref ByteField DataBlock) => throw new NotImplementedException();

    public bool ReceiveLine(out string ReceivedData) => throw new NotImplementedException();

    public bool ReceiveCRLF_Line(out string ReceivedData) => throw new NotImplementedException();

    public bool ReceiveBlockToChar(ref ByteField DataBlock, byte EndChar)
    {
      throw new NotImplementedException();
    }

    public void ComWriteLoggerEvent(EventLogger.LoggerEvent Event)
    {
    }

    public void ComWriteLoggerData(EventLogger.LoggerEvent Event, ref ByteField data)
    {
      throw new NotImplementedException();
    }

    public event EventHandler<GMM_EventArgs> OnAsyncComMessage;

    public bool ChangeParameterAtList(
      ArrayList ParameterList,
      string ParameterName,
      string NewParameter)
    {
      throw new NotImplementedException();
    }

    public string GetParameterFromList(ArrayList ParameterList, string ParameterName)
    {
      throw new NotImplementedException();
    }

    public string CreateParameterString(ArrayList ParameterList)
    {
      throw new NotImplementedException();
    }

    public ArrayList CreateParameterList(string ParameterString)
    {
      throw new NotImplementedException();
    }

    public void GMM_Dispose()
    {
    }

    public bool IsLocked => false;

    public void Lock(string owner) => throw new NotImplementedException();

    public void Unlock() => throw new NotImplementedException();

    public bool ChangeDriverSettings() => throw new NotImplementedException();

    public bool WakeupTemporaryOff { get; set; }

    public string Owner => throw new NotImplementedException();

    public int MinoConnectIrDaPulseLength { get; set; }

    public bool SetAsyncComSettings(SortedList<string, string> asyncComSettings)
    {
      throw new NotImplementedException();
    }

    public void SetReadoutConfiguration(ConfigList configList)
    {
    }

    public ConfigList GetReadoutConfiguration() => (ConfigList) null;
  }
}
