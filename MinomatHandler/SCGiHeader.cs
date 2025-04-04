// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiHeader
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
  public sealed class SCGiHeader
  {
    private static Logger logger = LogManager.GetLogger(nameof (SCGiHeader));
    public const byte SYNC = 170;
    public const int SIZE = 6;

    public int Version { get; set; }

    public SCGiAddress DestinationAddress { get; set; }

    public bool IsSequence { get; set; }

    public int PayloadLength { get; set; }

    public bool IsResponse { get; set; }

    public SCGiMessageType MessageClass { get; set; }

    public SCGiSequenceHeaderType SequenceHeaderType { get; set; }

    public List<SCGiHeaderEx> ExtendedHeaderList { get; set; }

    public SCGiAddress SourceAddress { get; set; }

    public byte Sequencenumber { get; set; }

    public SCGiHeader()
    {
      this.Version = 0;
      this.DestinationAddress = SCGiAddress.NodeGateway;
      this.IsSequence = false;
      this.IsResponse = false;
      this.SourceAddress = SCGiAddress.RS232;
      this.SequenceHeaderType = SCGiSequenceHeaderType.None;
      this.ExtendedHeaderList = new List<SCGiHeaderEx>();
      this.SequenceHeaderType = SCGiSequenceHeaderType.None;
      this.Sequencenumber = (byte) 1;
    }

    public SCGiHeader(SCGiMessageType messageType)
      : this()
    {
      this.MessageClass = messageType;
    }

    public SCGiHeader(SCGiMessageType messageType, SCGiHeaderEx extendedHeader)
      : this()
    {
      this.MessageClass = messageType;
      if (extendedHeader == null)
        return;
      this.ExtendedHeaderList.Add(extendedHeader);
    }

    public byte[] Create()
    {
      List<byte> byteList = new List<byte>();
      if (this.ExtendedHeaderList.Count > 0)
        this.SequenceHeaderType = this.ExtendedHeaderList[0].HeaderType;
      byte num1 = (byte) (this.Version << 5);
      byte destinationAddress = (byte) this.DestinationAddress;
      byte num2 = this.IsSequence ? (byte) 128 : (byte) 0;
      byte num3 = (byte) (this.PayloadLength / 2);
      byte num4 = this.IsResponse ? (byte) 128 : (byte) 0;
      byte messageClass = (byte) this.MessageClass;
      byte num5 = (byte) ((uint) (byte) this.SequenceHeaderType << 5);
      byte sourceAddress = (byte) this.SourceAddress;
      byteList.Add((byte) 170);
      byteList.Add((byte) ((uint) num1 | (uint) destinationAddress));
      byteList.Add((byte) ((uint) num2 | (uint) num3));
      byteList.Add((byte) ((uint) num4 | (uint) messageClass));
      byteList.Add((byte) ((uint) num5 | (uint) sourceAddress));
      byteList.Add(this.Sequencenumber);
      if (this.ExtendedHeaderList.Count > 0)
      {
        for (int index = 0; index < this.ExtendedHeaderList.Count; ++index)
        {
          if (index + 1 < this.ExtendedHeaderList.Count)
            byteList.AddRange((IEnumerable<byte>) this.ExtendedHeaderList[index].Create(this.ExtendedHeaderList[index + 1].HeaderType));
          else
            byteList.AddRange((IEnumerable<byte>) this.ExtendedHeaderList[index].Create());
        }
      }
      return byteList.ToArray();
    }

    public static SCGiHeader Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException("Header buffer is null!");
      if (buffer.Length != 6)
        throw new ArgumentException("Header length is not " + 6.ToString() + "!");
      if (buffer[0] != (byte) 170)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, "Can not parse the SCGi header! Wrong SYNC byte. Buffer: " + Util.ByteArrayToHexString(buffer));
        SCGiHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      int num1 = (int) buffer[1] & 31;
      int num2 = (int) buffer[3] & (int) sbyte.MaxValue;
      int num3 = (int) buffer[4] >> 5;
      int num4 = (int) buffer[4] & 31;
      if (!Enum.IsDefined(typeof (SCGiAddress), (object) num1))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the SCGi header! Unknown destination address of SCGi packet! Value: {0}, Buffer: {1}", (object) num1, (object) Util.ByteArrayToHexString(buffer)));
        SCGiHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (!Enum.IsDefined(typeof (SCGiMessageType), (object) num2))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the SCGi header! Unknown class of SCGi packet! Value: {0}, Buffer: {1}", (object) num2, (object) Util.ByteArrayToHexString(buffer)));
        SCGiHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (!Enum.IsDefined(typeof (SCGiSequenceHeaderType), (object) num3))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the SCGi header! Unknown type of sequence header in SCGi packet! Value: {0}, Buffer: {1}", (object) num3, (object) Util.ByteArrayToHexString(buffer)));
        SCGiHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (!Enum.IsDefined(typeof (SCGiAddress), (object) num4))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse the SCGi header! Unknown source address of SCGi packet! Value: {0}, Buffer: {1}", (object) num4, (object) Util.ByteArrayToHexString(buffer)));
        SCGiHeader.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new SCGiHeader()
      {
        Version = (int) buffer[1] >> 5,
        DestinationAddress = (SCGiAddress) Enum.ToObject(typeof (SCGiAddress), num1),
        IsSequence = ((int) buffer[2] & 128) == 128,
        PayloadLength = ((int) buffer[2] & (int) sbyte.MaxValue) * 2,
        IsResponse = ((int) buffer[3] & 128) == 128,
        MessageClass = (SCGiMessageType) Enum.ToObject(typeof (SCGiMessageType), num2),
        SequenceHeaderType = (SCGiSequenceHeaderType) Enum.ToObject(typeof (SCGiSequenceHeaderType), num3),
        SourceAddress = (SCGiAddress) Enum.ToObject(typeof (SCGiAddress), num4),
        Sequencenumber = buffer[5]
      };
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("VER: ").Append(this.Version).Append(", ");
      stringBuilder.Append("DEST_ADDR: ").Append((object) this.DestinationAddress).Append(", ");
      stringBuilder.Append("IS_SEQ: ").Append(this.IsSequence).Append(", ");
      stringBuilder.Append("LEN: ").Append(this.PayloadLength).Append(", ");
      stringBuilder.Append("IS_RSP: ").Append(this.IsResponse).Append(", ");
      stringBuilder.Append("CLASS: ").Append((object) this.MessageClass).Append(", ");
      stringBuilder.Append("SEQ_HED_TYPE: ").Append((object) this.SequenceHeaderType).Append(", ");
      stringBuilder.Append("SRS_ADR: ").Append((object) this.SourceAddress).Append(", ");
      stringBuilder.Append("SEQ_NR: ").Append(this.Sequencenumber);
      if (this.ExtendedHeaderList.Count > 0)
      {
        stringBuilder.Append("\nEXT HEADER: ");
        foreach (SCGiHeaderEx extendedHeader in this.ExtendedHeaderList)
        {
          stringBuilder.Append((object) extendedHeader.HeaderType).Append(", ");
          stringBuilder.Append("Can ignored: ").Append(extendedHeader.CanIgnored).Append(", ");
          stringBuilder.Append("Content: ").Append(Util.ByteArrayToHexString(extendedHeader.Content)).Append(", ");
          stringBuilder.Append("Length: ").Append(extendedHeader.BufferLength).Append(", ");
          stringBuilder.Append("Next extended header: ").Append((object) extendedHeader.NextHeaderType);
        }
      }
      return stringBuilder.ToString();
    }
  }
}
