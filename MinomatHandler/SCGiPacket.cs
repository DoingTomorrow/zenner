// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiPacket
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class SCGiPacket : EventArgs
  {
    private static Logger logger = LogManager.GetLogger(nameof (SCGiPacket));

    public SCGiPacket(SCGiHeader header, byte[] payload)
    {
      this.SetPayload(payload);
      this.Header = header;
      this.IsFirstPacketOfFrame = true;
      this.InitCRC = ushort.MaxValue;
    }

    public SCGiHeader Header { get; private set; }

    public byte[] Payload { get; private set; }

    public bool IsFirstPacketOfFrame { get; set; }

    public ushort InitCRC { get; set; }

    public void SetPayload(byte[] payload)
    {
      if (payload != null && payload.Length % 2 != 0)
      {
        byte[] dst = new byte[payload.Length + 1];
        Buffer.BlockCopy((Array) payload, 0, (Array) dst, 0, payload.Length);
        this.Payload = dst;
      }
      else
        this.Payload = payload;
    }

    public byte[] ToByteArray()
    {
      if (this.Header == null)
        throw new ArgumentNullException("SCGi header can not be null!");
      this.Header.PayloadLength = this.Payload != null ? this.Payload.Length : throw new ArgumentNullException("SCGi payload can not be null!");
      byte[] src = this.Header.Create();
      byte[] numArray = new byte[src.Length + this.Payload.Length];
      Buffer.BlockCopy((Array) src, 0, (Array) numArray, 0, src.Length);
      Buffer.BlockCopy((Array) this.Payload, 0, (Array) numArray, src.Length, this.Payload.Length);
      byte[] stuffedBuffer = SCGiPacket.GetStuffedBuffer(numArray);
      byte[] bytes = BitConverter.GetBytes(CRC.CalculateCRC(stuffedBuffer, 1, stuffedBuffer.Length, this.InitCRC));
      byte[] dst = new byte[stuffedBuffer.Length + bytes.Length];
      Buffer.BlockCopy((Array) stuffedBuffer, 0, (Array) dst, 0, stuffedBuffer.Length);
      Buffer.BlockCopy((Array) bytes, 0, (Array) dst, stuffedBuffer.Length, bytes.Length);
      return dst;
    }

    public ushort? CalculateCRC()
    {
      if (this.Header == null)
        return new ushort?();
      if (this.Payload == null)
        return new ushort?();
      this.Header.PayloadLength = this.Payload.Length;
      byte[] src = this.Header.Create();
      byte[] numArray = new byte[src.Length + this.Payload.Length];
      Buffer.BlockCopy((Array) src, 0, (Array) numArray, 0, src.Length);
      Buffer.BlockCopy((Array) this.Payload, 0, (Array) numArray, src.Length, this.Payload.Length);
      byte[] stuffedBuffer = SCGiPacket.GetStuffedBuffer(numArray);
      return new ushort?(CRC.CalculateCRC(stuffedBuffer, 1, stuffedBuffer.Length, this.InitCRC));
    }

    public static SCGiPacket Parse(byte[] stuffedBuffer)
    {
      byte[] numArray1 = stuffedBuffer != null ? SCGiPacket.GetUnstuffedBuffer(stuffedBuffer) : throw new ArgumentNullException("Stuffed buffer can not be null!");
      if (numArray1.Length < 6)
        throw new ArgumentException("Internal error by GetUnstuffedBuffer!");
      byte[] numArray2 = new byte[6];
      Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, 6);
      SCGiHeader header = SCGiHeader.Parse(numArray2);
      if (header == null)
        return (SCGiPacket) null;
      List<SCGiHeaderEx> scGiHeaderExList = SCGiHeaderEx.Parse(header, numArray1);
      if (scGiHeaderExList == null)
        return (SCGiPacket) null;
      header.ExtendedHeaderList = scGiHeaderExList;
      int num = 0;
      foreach (SCGiHeaderEx extendedHeader in header.ExtendedHeaderList)
        num += extendedHeader.BufferLength;
      if (6 + num + header.PayloadLength + 2 != numArray1.Length)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, "Can not parse the SCGi packet! Wrong packet length.");
        SCGiPacket.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      byte[] numArray3 = new byte[header.PayloadLength];
      Buffer.BlockCopy((Array) numArray1, 6 + num, (Array) numArray3, 0, header.PayloadLength);
      if ((int) BitConverter.ToUInt16(stuffedBuffer, stuffedBuffer.Length - 2) != (int) CRC.CalculateCRC(stuffedBuffer, 1, stuffedBuffer.Length - 2, ushort.MaxValue))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the SCGi packet! Wrong CRC. \nHEADER: {0}, \nBUFFER: {1}", (object) header, (object) Util.ByteArrayToHexString(numArray1)));
        SCGiPacket.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new SCGiPacket(header, numArray3);
    }

    public static byte[] GetStuffedBuffer(byte[] unstuffedBuffer)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add(unstuffedBuffer[0]);
      for (int index = 1; index < unstuffedBuffer.Length; ++index)
      {
        if (unstuffedBuffer[index] == (byte) 170)
        {
          byteList.Add((byte) 170);
          byteList.Add((byte) 170);
        }
        else
          byteList.Add(unstuffedBuffer[index]);
      }
      return byteList.ToArray();
    }

    public static byte[] GetUnstuffedBuffer(byte[] stuffedBuffer)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add(stuffedBuffer[0]);
      for (int index = 1; index < stuffedBuffer.Length; ++index)
      {
        if (stuffedBuffer[index] == (byte) 170)
        {
          byteList.Add((byte) 170);
          ++index;
        }
        else
          byteList.Add(stuffedBuffer[index]);
      }
      return byteList.ToArray();
    }

    public override string ToString()
    {
      if (this.Header == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append((object) this.Header);
      if (this.Payload == null)
        return stringBuilder.ToString();
      if (this.Header.MessageClass == SCGiMessageType.SCGI_1_9)
        stringBuilder.Append(" ASCII: ").Append(Encoding.ASCII.GetString(this.Payload, 0, this.Payload.Length).TrimEnd(new char[1]));
      else
        stringBuilder.Append(" PAYLOAD: ").Append(Util.ByteArrayToHexString(this.Payload));
      return stringBuilder.ToString();
    }
  }
}
