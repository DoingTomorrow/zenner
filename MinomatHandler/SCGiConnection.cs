// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiConnection
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using AsyncCom;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class SCGiConnection : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (SCGiConnection));
    private IAsyncFunctions channel;

    public event EventHandler<SCGiPacket> OnPacketReceived;

    public event EventHandler<SCGiPacket> OnRequest;

    public event CommunicationEventHandler OnResponse;

    public SCGiConnection()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.AsyncCom);
      this.channel = (IAsyncFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.AsyncCom];
      this.SourceAddress = SCGiAddress.RS232;
    }

    public SCGiConnection(IAsyncFunctions MyCom)
    {
      this.channel = MyCom;
      this.SourceAddress = SCGiAddress.RS232;
    }

    public long InputBufferLength
    {
      get
      {
        return this.channel != null ? this.channel.InputBufferLength : throw new ArgumentNullException("Channel can not be null!");
      }
    }

    public int ReadTimeout_RecTime_OffsetPerBlock
    {
      get
      {
        if (this.channel == null)
          return 400;
        SortedList<AsyncComSettings, object> asyncComSettings = this.channel.GetAsyncComSettings();
        return !asyncComSettings.ContainsKey(AsyncComSettings.RecTime_OffsetPerBlock) || string.IsNullOrEmpty(asyncComSettings[AsyncComSettings.RecTime_OffsetPerBlock].ToString()) ? 400 : Convert.ToInt32(asyncComSettings[AsyncComSettings.RecTime_OffsetPerBlock]);
      }
      set
      {
        if (this.channel == null)
          return;
        this.channel.SingleParameter(CommParameter.RecTime_OffsetPerBlock, value.ToString());
      }
    }

    public SCGiAddress SourceAddress { get; set; }

    public bool Open()
    {
      if (this.channel == null)
        throw new ArgumentNullException("Channel can not be null!");
      this.channel.BreakRequest = false;
      if (this.channel.IsOpen)
        return true;
      bool flag = this.channel.Open();
      if (!flag)
        return false;
      return this.channel.Transceiver == TransceiverDevice.MinoHead ? this.channel.CallTransceiverFunction(TransceiverDeviceFunction.TransparentModeV3On) : flag;
    }

    public bool Close()
    {
      if (this.channel == null)
        throw new ArgumentNullException("Channel can not be null!");
      this.channel.BreakRequest = true;
      return this.channel.Close();
    }

    public bool Write(SCGiPacket packet)
    {
      if (packet == null)
        throw new ArgumentNullException("SCGi packet can not be null!");
      if (this.OnRequest != null)
        this.OnRequest((object) this, packet);
      return this.Write(new SCGiFrame(packet));
    }

    public bool Write(SCGiFrame frame)
    {
      if (frame == null)
        throw new ArgumentNullException("SCGi frame can not be null!");
      if (frame.Count == 0)
        throw new ArgumentNullException("SCGi frame must contain at least one SCGi packet!");
      foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) frame)
        scGiPacket.Header.SourceAddress = this.SourceAddress;
      this.channel.WaitToEarliestTransmitTime();
      foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) frame)
      {
        byte[] byteArray = scGiPacket.ToByteArray();
        if (byteArray == null)
          return false;
        if (SCGiConnection.logger.IsDebugEnabled)
          SCGiConnection.logger.Debug("Write SCGi: {0}", scGiPacket.ToString());
        if (SCGiConnection.logger.IsTraceEnabled)
          SCGiConnection.logger.Debug("Write SCGi: " + Util.ByteArrayToHexString(byteArray));
        if (!this.channel.TransmitBlock(byteArray))
          return false;
      }
      return true;
    }

    public SCGiFrame ReadSCGi2_0()
    {
      bool first = true;
      ushort maxValue;
      SCGiPacket scGiPacket;
      do
      {
        maxValue = ushort.MaxValue;
        scGiPacket = this.ReadPacket(ref maxValue, first);
        if (scGiPacket == null)
          return (SCGiFrame) null;
        first = false;
        if (scGiPacket.Header.MessageClass == SCGiMessageType.SCGI_1_9)
        {
          SCGiConnection.logger.Error<SCGiPacket>("Received SCGi 1.9 packet! Dispose it. \n{0}", scGiPacket);
          scGiPacket = (SCGiPacket) null;
        }
      }
      while (scGiPacket == null);
      scGiPacket.IsFirstPacketOfFrame = true;
      if (SCGiConnection.logger.IsDebugEnabled)
        SCGiConnection.logger.Debug("Read SCGi first packet: {0}", scGiPacket.ToString());
      if (scGiPacket.Header.PayloadLength == 0 && scGiPacket.Payload == null)
        return new SCGiFrame(scGiPacket);
      if (scGiPacket.Payload.Length < 2)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.AuthenticationFailed, "Authentication was failed!");
        SCGiConnection.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      SCGiFrame frame = new SCGiFrame(scGiPacket);
      if (this.OnPacketReceived != null)
        this.OnPacketReceived((object) this, scGiPacket);
      if (scGiPacket.Header.IsSequence)
      {
        SCGiPacket e;
        do
        {
          e = this.ReadPacket(ref maxValue, false);
          if (e == null)
            return (SCGiFrame) null;
          e.IsFirstPacketOfFrame = false;
          if (SCGiConnection.logger.IsDebugEnabled)
            SCGiConnection.logger.Debug("Read SCGi sequence packet: {0}", e.ToString());
          frame.Add(e);
          if (this.OnPacketReceived != null)
            this.OnPacketReceived((object) this, e);
        }
        while (e.Header.IsSequence);
      }
      if (this.OnResponse != null)
        this.OnResponse((object) this, frame);
      return frame;
    }

    private SCGiPacket ReadPacket(ref ushort crcInitValue, bool first)
    {
      byte? b;
      while (this.ReadByte(out b, first))
      {
        byte? nullable1 = b;
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        int num1 = 170;
        if (!(nullable2.GetValueOrDefault() == num1 & nullable2.HasValue))
          SCGiConnection.logger.Error("Wrong sync byte received! Value: 0x{0}", b.Value.ToString("X2"));
        nullable1 = b;
        int? nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        int num2 = 170;
        if (nullable3.GetValueOrDefault() == num2 & nullable3.HasValue)
        {
          byte[] stuffed1;
          byte[] unstuffed1;
          if (!this.InternalRead(5, out stuffed1, out unstuffed1, true, false) || unstuffed1 == null || unstuffed1.Length == 0)
          {
            ZR_ClassLibMessages.LastErrorInfo lastErrorInfo = ZR_ClassLibMessages.GetLastErrorInfo();
            if (stuffed1 != null && stuffed1.Length != 0)
              throw new SCGiError(SCGiErrorType.InvalidResponce, string.Format("Not enough bytes was received for SCGi header! Expected: 5, Actual {0}, Stuffed header buffer: {1}, GMM LastError: {2}", (object) stuffed1.Length, (object) Util.ByteArrayToHexString(stuffed1), (object) lastErrorInfo.LastErrorDescription));
            throw new SCGiError(SCGiErrorType.Communication, "Can not receive SCGi header! GMM LastError: " + lastErrorInfo.LastErrorDescription);
          }
          List<byte> byteList1 = new List<byte>((IEnumerable<byte>) unstuffed1);
          byteList1.Insert(0, b.Value);
          byte[] array = byteList1.ToArray();
          List<byte> byteList2 = new List<byte>((IEnumerable<byte>) stuffed1);
          byteList2.Insert(0, b.Value);
          stuffed1 = byteList2.ToArray();
          SCGiHeader header = SCGiHeader.Parse(array);
          if (header == null)
            return (SCGiPacket) null;
          if (header.SequenceHeaderType != 0)
            throw new NotImplementedException(string.Format("Handler for sequence header type is not implemented! Type: {0}", (object) header.SequenceHeaderType));
          byte[] unstuffed2 = new byte[0];
          byte[] stuffed2 = new byte[0];
          if (header.PayloadLength > 0)
          {
            if (!this.InternalRead(header.PayloadLength, out stuffed2, out unstuffed2, true, false) || unstuffed2 == null || unstuffed2.Length == 0)
            {
              ZR_ClassLibMessages.LastErrorInfo lastErrorInfo = ZR_ClassLibMessages.GetLastErrorInfo();
              if (stuffed2 == null || stuffed2.Length == 0)
                throw new SCGiError(SCGiErrorType.Communication, string.Format("Can not receive payload of SCGi packet! HEADER: {0}, GMM LastError: {1}", (object) header, (object) ZR_ClassLibMessages.GetLastError()));
              if (header.MessageClass == SCGiMessageType.SCGI_1_9)
              {
                SCGiConnection.logger.Error<string, string, string>("Invalid SCGi 1.9 responce received! \nStuffed buffer: {0}{1}, GMM LastError: {2}", Util.ByteArrayToHexString(stuffed1), Util.ByteArrayToHexString(stuffed2), lastErrorInfo.LastErrorDescription);
                return new SCGiPacket(header, (byte[]) null);
              }
              throw new SCGiError(SCGiErrorType.InvalidResponce, string.Format("Not enough bytes was received for SCGi payload! Expected: {0}, Actual {1}, HEADER: {2}, \nStuffed buffer: {3}{4}, GMM LastError: {5}", (object) header.PayloadLength, (object) stuffed2.Length, (object) header, (object) Util.ByteArrayToHexString(stuffed1), (object) Util.ByteArrayToHexString(stuffed2), (object) lastErrorInfo.LastErrorDescription));
            }
          }
          else if (header.MessageClass == SCGiMessageType.SCGI_1_9)
            return new SCGiPacket(header, (byte[]) null);
          byte[] buffer;
          if (!this.ReceiveBlock(out buffer, 2, false))
          {
            ZR_ClassLibMessages.LastErrorInfo lastErrorInfo = ZR_ClassLibMessages.GetLastErrorInfo();
            if (buffer != null && buffer.Length != 0)
            {
              SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidResponce, string.Format("Not enough bytes was received for SCGi CRC! Expected: 2, Actual {0}, HEADER: {1}, PAYLOAD: {2}, Stuffed CRC buffer; {3}, GMM LastError: {4}", (object) buffer.Length, (object) header, (object) Util.ByteArrayToHexString(unstuffed2), (object) Util.ByteArrayToHexString(buffer), (object) lastErrorInfo.LastErrorDescription));
              SCGiConnection.logger.Error<SCGiError>(scGiError);
              throw scGiError;
            }
            SCGiError scGiError1 = new SCGiError(SCGiErrorType.Communication, string.Format("Can not receive CRC of SCGi packet! HEADER: {0}, PAYLOAD: {1}, GMM LastError: {2}", (object) header, (object) Util.ByteArrayToHexString(unstuffed2), (object) ZR_ClassLibMessages.GetLastError()));
            SCGiConnection.logger.Error<SCGiError>(scGiError1);
            throw scGiError1;
          }
          byte[] numArray = new byte[stuffed1.Length + stuffed2.Length + buffer.Length];
          Buffer.BlockCopy((Array) stuffed1, 0, (Array) numArray, 0, stuffed1.Length);
          Buffer.BlockCopy((Array) stuffed2, 0, (Array) numArray, stuffed1.Length, stuffed2.Length);
          Buffer.BlockCopy((Array) buffer, 0, (Array) numArray, stuffed2.Length + stuffed1.Length, buffer.Length);
          ushort crc = CRC.CalculateCRC(numArray, 1, numArray.Length - 2, crcInitValue);
          if ((int) BitConverter.ToUInt16(buffer, 0) != (int) crc)
          {
            SCGiError scGiError = new SCGiError(SCGiErrorType.WrongCRC, "Received packet has invalid CRC (SCGi)!");
            SCGiConnection.logger.Error<SCGiError>(scGiError);
            throw scGiError;
          }
          if (SCGiConnection.logger.IsTraceEnabled)
            SCGiConnection.logger.Trace("Read SCGi:" + Util.ByteArrayToHexString(numArray));
          crcInitValue = crc;
          return new SCGiPacket(header, unstuffed2);
        }
      }
      throw new SCGiError(SCGiErrorType.Communication, ZR_ClassLibMessages.GetLastErrorInfo().LastErrorDescription);
    }

    private bool ReadByte(out byte? b, bool first)
    {
      b = new byte?();
      byte[] stuffed;
      bool flag = this.InternalRead(1, out stuffed, out byte[] _, false, first);
      if (flag)
        b = new byte?(stuffed[0]);
      return flag;
    }

    private bool InternalRead(
      int size,
      out byte[] stuffed,
      out byte[] unstuffed,
      bool checkSyncByte,
      bool first)
    {
      unstuffed = (byte[]) null;
      stuffed = (byte[]) null;
      List<byte> byteList1 = new List<byte>();
      List<byte> byteList2 = new List<byte>();
      byte[] buffer;
      while (this.ReceiveBlock(out buffer, size - byteList2.Count, first))
      {
        byteList1.AddRange((IEnumerable<byte>) buffer);
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (buffer[index] == (byte) 170 && index + 1 < buffer.Length && buffer[index + 1] == (byte) 170)
          {
            SCGiConnection.logger.Trace("Stuffed data byte detected!");
            byteList2.Add(buffer[index]);
            ++index;
          }
          else if (checkSyncByte && buffer[index] == (byte) 170 && index == buffer.Length - 1)
          {
            SCGiConnection.logger.Trace("Stuffed data byte on end of packet detected!");
            byteList2.Add(buffer[index]);
            byte? b;
            if (!this.ReadByte(out b, false))
              return false;
            byteList1.Add(b.Value);
            byte? nullable1 = b;
            int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
            int num = 170;
            if (!(nullable2.GetValueOrDefault() == num & nullable2.HasValue))
            {
              SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidResponce, string.Format("Invalid end of responce received! Detected 0xAA as last byte but next byte is not 0xAA. Buffer: {0}", (object) Util.ByteArrayToHexString(buffer)));
              SCGiConnection.logger.Error<SCGiError>(scGiError);
              throw scGiError;
            }
          }
          else
          {
            if (buffer[index] == (byte) 170 & checkSyncByte && index + 1 < buffer.Length && buffer[index + 1] != (byte) 170)
            {
              SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidResponce, string.Format("Invalid responce received! Sync byte only once. Buffer: {0}", (object) Util.ByteArrayToHexString(buffer)));
              SCGiConnection.logger.Error<SCGiError>(scGiError);
              throw scGiError;
            }
            byteList2.Add(buffer[index]);
          }
        }
        if (byteList2.Count >= size)
        {
          if (byteList2.Count != size)
            throw new Exception("FATAL INTERNAL ERROR 1");
          if (byteList1.Count < byteList2.Count)
            throw new Exception("FATAL INTERNAL ERROR 2");
          unstuffed = byteList2.ToArray();
          stuffed = byteList1.ToArray();
          return true;
        }
      }
      if (buffer != null && buffer.Length != 0)
        byteList1.AddRange((IEnumerable<byte>) buffer);
      stuffed = byteList1.ToArray();
      return false;
    }

    public SCGiFrame ReadSCGi1_9()
    {
      SCGiFrame scGiFrame = new SCGiFrame();
      bool first = true;
      while (true)
      {
        try
        {
          ushort maxValue = ushort.MaxValue;
          SCGiPacket e = this.ReadPacket(ref maxValue, first);
          if (e == null || e.Payload == null)
            return scGiFrame;
          first = false;
          if (SCGiConnection.logger.IsDebugEnabled)
            SCGiConnection.logger.Debug<SCGiPacket>("Read SCGi 1.9 packet: {0}", e);
          scGiFrame.Add(e);
          if (this.OnPacketReceived != null)
            this.OnPacketReceived((object) this, e);
        }
        catch (SCGiError ex)
        {
          if (ex.Type != SCGiErrorType.InvalidResponce)
          {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) scGiFrame)
              stringBuilder.Append(Encoding.ASCII.GetString(scGiPacket.Payload, 0, scGiPacket.Payload.Length).TrimEnd(new char[1]));
            throw new SCGiError(SCGiErrorType.Communication, string.Format("Error while receiving SCGi 1.9 responce! \n{0}, \n{1}", (object) ex.Message, (object) stringBuilder), (Exception) ex);
          }
          goto label_16;
        }
label_16:;
      }
    }

    public SortedList<AsyncComSettings, object> GetSettings() => this.channel.GetAsyncComSettings();

    public void Dispose()
    {
      if (this.channel == null)
        return;
      this.channel.Close();
    }

    private bool ReceiveBlock(out byte[] buffer, int size, bool first)
    {
      buffer = (byte[]) null;
      ByteField DataBlock = new ByteField();
      bool block = this.channel.ReceiveBlock(ref DataBlock, size, first);
      if (DataBlock.Count > 0)
      {
        byte[] destinationArray = new byte[DataBlock.Count];
        Array.Copy((Array) DataBlock.Data, (Array) destinationArray, DataBlock.Count);
        buffer = destinationArray;
      }
      return block && buffer != null && buffer.Length == size;
    }

    public void ClearCom()
    {
      if (this.channel != null)
      {
        SCGiConnection.logger.Debug("Clear COM called.");
        for (int index = 10000; index > 0 && this.InputBufferLength > 0L; index -= 1000)
        {
          byte[] buffer;
          if (!this.channel.TryReceiveBlock(out buffer))
            return;
          SCGiConnection.logger.Error<int, string>("Not expected data received! Size: {0}, Buffer: {1}", buffer.Length, Util.ByteArrayToHexString(buffer));
          SCGiConnection.logger.Trace("Wait 1000 ms after clear input buffer.");
          Thread.Sleep(1000);
        }
        if (this.InputBufferLength > 0L)
          throw new Exception("It was detected a sustained data transfer!");
      }
      this.channel.ClearCom();
    }
  }
}
