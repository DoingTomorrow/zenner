// Decompiled with JetBrains decompiler
// Type: MinomatListener.MinomatAsynCom
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using AsyncCom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatListener
{
  public sealed class MinomatAsynCom : IAsyncFunctions, I_ZR_Component, ILockable, ICancelable
  {
    private Server server;
    private TcpClient client;
    private NetworkStream stream;
    private MinomatDevice minomat;
    private Queue<byte> inputBuffer;

    public MinomatAsynCom(
      Server server,
      TcpClient client,
      NetworkStream stream,
      MinomatDevice minomat)
    {
      this.server = server;
      this.client = client;
      this.stream = stream;
      this.minomat = minomat;
      this.inputBuffer = new Queue<byte>();
    }

    public bool TransmitBlock(byte[] buffer)
    {
      uint? challengeKey = this.minomat.ChallengeKey;
      int left = (int) challengeKey.Value;
      challengeKey = this.minomat.ChallengeKey;
      int right = (int) challengeKey.Value;
      byte[] bytes1 = BitConverter.GetBytes(Util.TwoUInt32ToUInt64((uint) left, (uint) right) ^ this.minomat.SessionKey.Value);
      ulong? sessionKey = this.minomat.SessionKey;
      long num1 = (long) sessionKey.Value;
      sessionKey = this.minomat.SessionKey;
      long num2 = (long) sessionKey.Value;
      byte[] bytes2 = BitConverter.GetBytes((ulong) (num1 ^ num2));
      Array.Reverse((Array) bytes1);
      Array.Reverse((Array) bytes2);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 2);
      byteList.AddRange((IEnumerable<byte>) bytes1);
      byteList.AddRange((IEnumerable<byte>) bytes2);
      byteList.AddRange((IEnumerable<byte>) buffer);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Content-Length: " + byteList.Count.ToString());
      stringBuilder.AppendLine();
      byteList.InsertRange(0, (IEnumerable<byte>) Encoding.ASCII.GetBytes(stringBuilder.ToString()));
      if (Server.logger.IsDebugEnabled)
        Server.logger.Debug("Write: " + Util.ByteArrayToHexString(byteList.ToArray()));
      this.stream.Write(byteList.ToArray(), 0, byteList.Count);
      this.inputBuffer.Clear();
      return true;
    }

    public bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first)
    {
      if (DataBlock == null)
        throw new ArgumentNullException(nameof (DataBlock));
      if (MinByteNb <= 0)
        throw new IndexOutOfRangeException(nameof (MinByteNb));
      if (this.inputBuffer.Count <= 0)
      {
        if (!this.stream.CanRead)
        {
          string message = "The stream can not be read!" + this.minomat?.ToString();
          Server.logger.Error(message);
          this.server.RaiseOnError((object) this, (Exception) new InvalidConnectionException(message));
          return false;
        }
        HttpPacket httpPacket = (HttpPacket) null;
        byte[] buffer1 = new byte[1024];
        using (MemoryStream memoryStream = new MemoryStream(1024))
        {
          try
          {
            do
            {
              int count = this.stream.Read(buffer1, 0, buffer1.Length);
              if (count > 0)
              {
                memoryStream.Write(buffer1, 0, count);
                if (memoryStream.Length > 200000L)
                  throw new Exception("Minomat sends too match data! Ignore this client. " + this.minomat?.ToString());
              }
              else
                break;
            }
            while (this.stream.DataAvailable);
          }
          catch (Exception ex)
          {
            string message = "Failed read data from Minomat! " + ex.Message;
            Server.logger.Error(message);
            this.server.RaiseOnError((object) this, (Exception) new InvalidConnectionException(message, memoryStream.ToArray()));
          }
          byte[] array = memoryStream.ToArray();
          if (array.Length == 0)
          {
            string message = "No response from Minomat!";
            Server.logger.Error(message);
            this.server.RaiseOnError((object) this, (Exception) new InvalidConnectionException(message));
            return false;
          }
          try
          {
            httpPacket = HttpPacket.TryParse(array);
          }
          catch (HttpPacketIsNotCompleteException ex)
          {
            Server.logger.Warn("Response: " + Util.ByteArrayToHexString(array));
            Server.logger.WarnException("Minomat sends HTTP response but " + ex.MissedBytes.ToString() + " bytes are missed! Try to read it again.", (Exception) ex);
            int missedBytes = ex.MissedBytes;
            byte[] buffer2 = new byte[missedBytes];
            do
            {
              int count = this.stream.Read(buffer2, 0, buffer2.Length);
              if (count > 0)
              {
                missedBytes -= count;
                memoryStream.Write(buffer2, 0, count);
              }
              else
                break;
            }
            while (missedBytes > 0);
            httpPacket = HttpPacket.TryParse(memoryStream.ToArray());
          }
          catch (Exception ex)
          {
            string message = "Minomat sends invalid HTTP packet! " + ex.Message;
            Server.logger.Error(message, ex);
            this.server.RaiseOnError((object) this, (Exception) new InvalidConnectionException(message, memoryStream.ToArray()));
            return false;
          }
          finally
          {
            if (Server.logger != null && memoryStream.Length > 0L && Server.logger.IsDebugEnabled)
              Server.logger.Debug("Read: " + Util.ByteArrayToHexString(memoryStream.ToArray()));
          }
          if (httpPacket == null)
          {
            string message = "Minomat response with invalid HTTP packet!";
            Server.logger.Error(message);
            this.server.RaiseOnError((object) this, (Exception) new InvalidConnectionException(message, memoryStream.ToArray()));
            return false;
          }
        }
        if (httpPacket.Type != HttpPacketType.RESP)
        {
          string message = "Minomat response is wrong! Expected: RESP, received: " + httpPacket.Type.ToString();
          Server.logger.Error(message);
          this.server.RaiseOnError((object) this, (Exception) new InvalidConnectionException(message, httpPacket.Content));
          return false;
        }
        ResponcePacket responcePacket = ResponcePacket.TryParse(httpPacket.Content);
        if (responcePacket == null)
        {
          string message = "Minomat sends invalid response packet! Content of HTTP request: " + Util.ByteArrayToHexString(httpPacket.Content);
          Server.logger.Error(message);
          this.server.RaiseOnError((object) this, (Exception) new InvalidResponceException(message, httpPacket.Content));
          return false;
        }
        if (responcePacket.SCGI == null || responcePacket.SCGI.Length == 0)
        {
          string message = "Minomat sends no SCGI content!";
          Server.logger.Error(message);
          this.server.RaiseOnError((object) this, (Exception) new InvalidResponceException(message, httpPacket.Content));
          return false;
        }
        foreach (byte num in responcePacket.SCGI)
          this.inputBuffer.Enqueue(num);
      }
      if (this.inputBuffer.Count == 0)
      {
        string message = "Minomat sends no SCGI response!";
        Server.logger.Error(message);
        this.server.RaiseOnError((object) this, (Exception) new InvalidResponceException(message));
        return false;
      }
      if (this.inputBuffer.Count < MinByteNb)
      {
        string message = "Minomat sends not all bytes of SCGI response!";
        Server.logger.Error(message);
        this.server.RaiseOnError((object) this, (Exception) new InvalidResponceException(message, this.inputBuffer.ToArray()));
        return false;
      }
      for (int index = 0; index < MinByteNb; ++index)
        DataBlock.Add(this.inputBuffer.Dequeue());
      return true;
    }

    public bool SetCommParameter(ArrayList parameterList)
    {
      this.ParameterList = parameterList;
      return true;
    }

    public void GetCommParameter(ref ArrayList parameterList) => this.ParameterList = parameterList;

    public SortedList<AsyncComSettings, object> GetAsyncComSettings()
    {
      ArrayList parameterList = new ArrayList();
      this.GetCommParameter(ref parameterList);
      if (parameterList == null || parameterList.Count % 2 != 0)
        return (SortedList<AsyncComSettings, object>) null;
      SortedList<AsyncComSettings, object> asyncComSettings = new SortedList<AsyncComSettings, object>();
      for (int index = 0; index < parameterList.Count; index += 2)
      {
        if (Enum.IsDefined(typeof (AsyncComSettings), parameterList[index]))
        {
          AsyncComSettings key = (AsyncComSettings) Enum.Parse(typeof (AsyncComSettings), parameterList[index].ToString(), true);
          object obj = parameterList[index + 1];
          asyncComSettings.Add(key, obj);
        }
      }
      return asyncComSettings;
    }

    public void GMM_Dispose()
    {
      if (this.ParameterList != null)
        this.ParameterList.Clear();
      this.ParameterList = (ArrayList) null;
    }

    public ArrayList ParameterList { get; set; }

    public bool BreakRequest { get; set; }

    public TransceiverDevice Transceiver { get; set; }

    public bool IsOpen => true;

    public event System.EventHandler ConnectionLost;

    public event System.EventHandler BatterieLow;

    public event EventHandler<GMM_EventArgs> OnAsyncComMessage;

    public void WaitToEarliestTransmitTime()
    {
    }

    public bool ChangeDriverSettings() => true;

    public string SingleParameter(CommParameter Parameter, string ParameterValue) => string.Empty;

    public string ShowComWindow(string ComponentList) => throw new NotImplementedException();

    public void ShowComWindow() => throw new NotImplementedException();

    public bool ShowComWindowChanged() => throw new NotImplementedException();

    public bool Open() => throw new NotImplementedException();

    public bool Close() => true;

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

    public string GetAsyncComSettingsAsString() => throw new NotImplementedException();

    public bool SetCommParameter(ArrayList ParameterList, bool ComWindowRefresh)
    {
      throw new NotImplementedException();
    }

    public void ResetEarliestTransmitTime() => throw new NotImplementedException();

    public void ResetLastTransmitEndTime() => throw new NotImplementedException();

    public void ClearWakeup() => throw new NotImplementedException();

    public void TriggerWakeup() => throw new NotImplementedException();

    public bool WakeupTemporaryOff
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public bool SetHandshakeState(HandshakeStates HandshakeState)
    {
      throw new NotImplementedException();
    }

    public void SetAnswerOffsetTime(int NewAnswerOffsetTime) => throw new NotImplementedException();

    public bool TransmitBlock(string DataString) => throw new NotImplementedException();

    public bool TransmitBlock(ref ByteField DataBlock) => throw new NotImplementedException();

    public bool SendBlock(ref ByteField DataBlock) => throw new NotImplementedException();

    public bool TransmitString(string DataString) => throw new NotImplementedException();

    public bool ReceiveString(out string DataString) => throw new NotImplementedException();

    public bool ReceiveBlock(ref ByteField DataBlock) => throw new NotImplementedException();

    public bool TryReceiveBlock(out byte[] buffer) => throw new NotImplementedException();

    public bool TryReceiveBlock(out byte[] buffer, int numberOfBytesToReceive)
    {
      throw new NotImplementedException();
    }

    public bool ReceiveLine(out string ReceivedData) => throw new NotImplementedException();

    public bool ReceiveCRLF_Line(out string ReceivedData) => throw new NotImplementedException();

    public bool ReceiveBlockToChar(ref ByteField DataBlock, byte EndChar)
    {
      throw new NotImplementedException();
    }

    public void ComWriteLoggerEvent(EventLogger.LoggerEvent Event)
    {
      throw new NotImplementedException();
    }

    public void ComWriteLoggerData(EventLogger.LoggerEvent Event, ref ByteField data)
    {
      throw new NotImplementedException();
    }

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

    public int MinoConnectIrDaPulseLength
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public DateTime FirstCalculatedEarliestTransmitTime
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public bool UpdateTransceiverFirmware(string pathToFirmware)
    {
      throw new NotImplementedException();
    }

    public Dictionary<string, string> LoadAvailableCOMservers()
    {
      throw new NotImplementedException();
    }

    public bool SendMinoConnectCommand(string cmd) => throw new NotImplementedException();

    public long InputBufferLength => throw new NotImplementedException();

    public bool GetCurrentInputBuffer(out byte[] buffer) => throw new NotImplementedException();

    public bool IsLocked => throw new NotImplementedException();

    public void Lock(string owner) => throw new NotImplementedException();

    public void Unlock() => throw new NotImplementedException();

    public string Owner => throw new NotImplementedException();

    public bool SetAsyncComSettings(SortedList<string, string> asyncComSettings)
    {
      throw new NotImplementedException();
    }
  }
}
