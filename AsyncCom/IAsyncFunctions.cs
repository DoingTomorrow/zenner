// Decompiled with JetBrains decompiler
// Type: AsyncCom.IAsyncFunctions
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System;
using System.Collections;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  public interface IAsyncFunctions : I_ZR_Component, ILockable, ICancelable
  {
    event System.EventHandler ConnectionLost;

    event System.EventHandler BatterieLow;

    string ShowComWindow(string ComponentList);

    void ShowComWindow();

    bool ShowComWindowChanged();

    bool IsOpen { get; }

    bool Open();

    bool Close();

    void ClearCom();

    bool SetBreak();

    bool ClearBreak();

    bool CallTransceiverFunction(TransceiverDeviceFunction function);

    bool CallTransceiverFunction(TransceiverDeviceFunction function, object param1);

    bool CallTransceiverFunction(TransceiverDeviceFunction function, object param1, object param2);

    bool GetComPortIds(out string strComPortIds, bool ForceRefresh);

    string GetTranceiverDeviceInfo();

    void ShowErrorMessageBox(bool on);

    string SingleParameter(string ParameterName, string ParameterValue);

    string SingleParameter(CommParameter Parameter, string ParameterValue);

    void GetCommParameter(ref ArrayList ParameterList);

    SortedList<AsyncComSettings, object> GetAsyncComSettings();

    string GetAsyncComSettingsAsString();

    bool SetCommParameter(ArrayList ParameterList);

    bool SetCommParameter(ArrayList ParameterList, bool ComWindowRefresh);

    void WaitToEarliestTransmitTime();

    void ResetEarliestTransmitTime();

    void ResetLastTransmitEndTime();

    void ClearWakeup();

    void TriggerWakeup();

    bool WakeupTemporaryOff { get; set; }

    bool SetHandshakeState(HandshakeStates HandshakeState);

    void SetAnswerOffsetTime(int NewAnswerOffsetTime);

    bool TransmitBlock(string DataString);

    bool TransmitBlock(ref ByteField DataBlock);

    bool TransmitBlock(byte[] buffer);

    bool SendBlock(ref ByteField DataBlock);

    bool TransmitString(string DataString);

    bool ReceiveString(out string DataString);

    bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first);

    bool ReceiveBlock(ref ByteField DataBlock);

    bool TryReceiveBlock(out byte[] buffer);

    bool TryReceiveBlock(out byte[] buffer, int numberOfBytesToReceive);

    bool ReceiveLine(out string ReceivedData);

    bool ReceiveCRLF_Line(out string ReceivedData);

    bool ReceiveBlockToChar(ref ByteField DataBlock, byte EndChar);

    void ComWriteLoggerEvent(EventLogger.LoggerEvent Event);

    void ComWriteLoggerData(EventLogger.LoggerEvent Event, ref ByteField data);

    event EventHandler<GMM_EventArgs> OnAsyncComMessage;

    bool ChangeParameterAtList(ArrayList ParameterList, string ParameterName, string NewParameter);

    string GetParameterFromList(ArrayList ParameterList, string ParameterName);

    string CreateParameterString(ArrayList ParameterList);

    ArrayList CreateParameterList(string ParameterString);

    bool ChangeDriverSettings();

    int MinoConnectIrDaPulseLength { get; set; }

    TransceiverDevice Transceiver { get; set; }

    DateTime FirstCalculatedEarliestTransmitTime { get; set; }

    bool UpdateTransceiverFirmware(string pathToFirmware);

    Dictionary<string, string> LoadAvailableCOMservers();

    bool SendMinoConnectCommand(string cmd);

    long InputBufferLength { get; }

    bool GetCurrentInputBuffer(out byte[] buffer);

    bool SetAsyncComSettings(SortedList<string, string> asyncComSettings);
  }
}
