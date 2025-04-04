// Decompiled with JetBrains decompiler
// Type: MBusLib.VariableDataStructure
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Entities;
using MBusLib.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class VariableDataStructure : IPrintable
  {
    public FixedDataHeader Header { get; set; }

    public List<VariableDataBlock> Records { get; set; }

    public byte MDH { get; set; }

    public byte[] MfgData { get; set; }

    public bool HasMoreRecordsInNextTelegram => this.MDH == (byte) 31;

    public string Print(int spaces = 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Header != null)
        stringBuilder.Append(' ', spaces).AppendLine("HEADER").Append(this.Header.Print(spaces + 6));
      if (this.Records != null)
      {
        stringBuilder.Append(' ', spaces).AppendLine("RECORDS");
        foreach (VariableDataBlock record in this.Records)
          stringBuilder.AppendLine(record.Print(spaces + 6));
      }
      stringBuilder.Append(' ', spaces).Append("MDH: 0x").AppendLine(this.MDH.ToString("X2"));
      if (this.MfgData != null && this.MfgData.Length != 0)
        stringBuilder.Append(' ', spaces).Append("MfgData: ").AppendLine(Util.ByteArrayToHexString((IEnumerable<byte>) this.MfgData));
      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Header != null)
        stringBuilder.Append((object) this.Header).Append(", ");
      if (this.Records != null)
        stringBuilder.Append(this.Records.Count).Append(" records, ");
      stringBuilder.Append("MDH: 0x").Append(this.MDH.ToString("X2")).Append(" ");
      if (this.MfgData != null && this.MfgData.Length != 0)
        stringBuilder.Append("MfgData: ").Append(Util.ByteArrayToHexString((IEnumerable<byte>) this.MfgData));
      return stringBuilder.ToString().TrimEnd();
    }

    public void AddRecord(VariableDataBlock block)
    {
      if (block == null)
        throw new ArgumentNullException(nameof (block));
      if (this.Records == null)
        this.Records = new List<VariableDataBlock>();
      this.Records.Add(block);
    }

    public static VariableDataStructure Parse(MBusFrame frame)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      FixedDataHeader fixedDataHeader = frame.ControlInfo == CI_Field.MBusWithFullHeader || frame.ControlInfo == CI_Field.Response_76h ? FixedDataHeader.Parse(frame.UserData) : throw new ArgumentException("Invalid M-Bus frame! Expected: VariableDataStructure, Actual: " + frame?.ToString());
      int startIndex = 12;
      VariableDataStructure variableDataStructure1 = new VariableDataStructure();
      variableDataStructure1.Header = fixedDataHeader;
      while (startIndex < frame.UserData.Length)
      {
        if (frame.UserData[startIndex] == (byte) 15)
        {
          byte[] dst = new byte[frame.UserData.Length - startIndex - 1];
          Buffer.BlockCopy((Array) frame.UserData, startIndex + 1, (Array) dst, 0, dst.Length);
          VariableDataStructure variableDataStructure2 = variableDataStructure1;
          byte[] userData = frame.UserData;
          int index = startIndex;
          int num1 = index + 1;
          int num2 = (int) userData[index];
          variableDataStructure2.MDH = (byte) num2;
          variableDataStructure1.MfgData = dst;
          startIndex = num1 + variableDataStructure1.MfgData.Length;
        }
        else if (startIndex == frame.UserData.Length - 1)
        {
          variableDataStructure1.MDH = frame.UserData[startIndex++];
        }
        else
        {
          VariableDataBlock block = VariableDataBlock.Parse(frame.UserData, startIndex);
          startIndex += block.Size;
          variableDataStructure1.AddRecord(block);
        }
      }
      return variableDataStructure1;
    }

    public static VariableDataStructure Parse(MBusFrameCrypt frame)
    {
      int offset = 0;
      return VariableDataStructure.Parse(frame, ref offset);
    }

    public static VariableDataStructure Parse(MBusFrameCrypt frame, ref int offset)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      if (frame.ControlInfo != CI_Field.MBusWithFullHeader && frame.ControlInfo != CI_Field.MBusWithShortHeader && frame.ControlInfo != CI_Field.Response_76h)
        throw new ArgumentException("Invalid M-Bus frame! Expected: VariableDataStructure, Actual: " + frame?.ToString());
      FixedDataHeader fixedDataHeader;
      if (frame.ControlInfo == CI_Field.MBusWithShortHeader)
      {
        fixedDataHeader = new FixedDataHeader()
        {
          ID_BCD = 0U,
          ACC = frame.UserData[0],
          Status = frame.UserData[1]
        };
        offset = 4;
      }
      else
      {
        fixedDataHeader = FixedDataHeader.Parse(frame.UserData);
        offset = 12;
      }
      VariableDataStructure variableDataStructure = new VariableDataStructure()
      {
        Header = fixedDataHeader
      };
      if (frame.AFL != null && (frame.ControlInfo == CI_Field.MBusWithFullHeader || frame.ControlInfo == CI_Field.MBusWithShortHeader))
      {
        offset -= 2;
        ConfWord confWord = ConfWord.Decode(frame.UserData, ref offset);
        frame.EncryptionMode = new int?(confWord.EncryptionMode);
        int? encryptionMode = frame.EncryptionMode;
        int num1 = 7;
        if (!(encryptionMode.GetValueOrDefault() == num1 & encryptionMode.HasValue) || frame.EncryptionKey == null)
          throw new Exception("Encryption is not set in this frame, please check frame!");
        byte num2 = frame.UserData[offset++];
        byte num3 = frame.UserData[offset++];
        if (num2 != (byte) 47 || num3 != (byte) 47)
        {
          List<byte> byteList1 = new List<byte>(16);
          byteList1.Add(frame.Direction == Direction.DeviceToGateway ? (byte) 0 : (byte) 16);
          byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(frame.AFL.MCR.Value));
          byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(fixedDataHeader.ID_BCD));
          byteList1.Add((byte) 7);
          byteList1.Add((byte) 7);
          byteList1.Add((byte) 7);
          byteList1.Add((byte) 7);
          byteList1.Add((byte) 7);
          byteList1.Add((byte) 7);
          byteList1.Add((byte) 7);
          byte[] key1 = AES.AESCMAC(frame.EncryptionKey, byteList1.ToArray());
          byteList1[0] = frame.Direction == Direction.DeviceToGateway ? (byte) 1 : (byte) 17;
          byte[] key2 = AES.AESCMAC(frame.EncryptionKey, byteList1.ToArray());
          List<byte> collection = new List<byte>();
          collection.Add((byte) frame.ControlInfo);
          collection.AddRange((IEnumerable<byte>) frame.UserData);
          List<byte> byteList2 = new List<byte>();
          byteList2.Add(frame.AFL.MCL.Value);
          byteList2.AddRange((IEnumerable<byte>) BitConverter.GetBytes(frame.AFL.MCR.Value));
          byteList2.AddRange((IEnumerable<byte>) collection);
          ulong uint64 = BitConverter.ToUInt64(AES.AESCMAC(key2, byteList2.ToArray()), 0);
          ulong? mac = frame.AFL.MAC;
          long num4 = (long) uint64;
          ulong? nullable = mac;
          long valueOrDefault = (long) nullable.GetValueOrDefault();
          if (num4 == valueOrDefault & nullable.HasValue)
            ;
          byte[] encrypted = frame.UserData.SubArray<byte>(offset - 2, confWord.Size);
          byte[] IV = new byte[16];
          byte[] numArray = Util.DecryptCBC_AES_128(key1, IV, encrypted);
          int index1 = offset;
          for (int index2 = 2; index2 < numArray.Length; ++index2)
          {
            frame.UserData[index1] = numArray[index2];
            ++index1;
          }
        }
      }
      for (int index = 0; offset < frame.UserData.Length && index <= 17; ++index)
      {
        if (frame.UserData[offset] == (byte) 15)
        {
          byte[] dst = new byte[frame.UserData.Length - offset - 1];
          Buffer.BlockCopy((Array) frame.UserData, offset + 1, (Array) dst, 0, dst.Length);
          variableDataStructure.MDH = frame.UserData[offset++];
          variableDataStructure.MfgData = dst;
          offset += variableDataStructure.MfgData.Length;
        }
        else if (offset == frame.UserData.Length - 1)
        {
          variableDataStructure.MDH = frame.UserData[offset++];
        }
        else
        {
          try
          {
            VariableDataBlock block = VariableDataBlock.Parse(frame.UserData, offset);
            if (block != null)
              variableDataStructure.AddRecord(block);
          }
          catch
          {
            Util.Log((object) nameof (VariableDataStructure), "Data information block (DIF, DIFE, VIF, VIFE, Data) is invalid!", frame.ToByteArray());
          }
        }
      }
      return variableDataStructure;
    }
  }
}
