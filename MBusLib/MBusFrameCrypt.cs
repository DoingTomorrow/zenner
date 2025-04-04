// Decompiled with JetBrains decompiler
// Type: MBusLib.MBusFrameCrypt
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.DataTypes;
using MBusLib.Entities;
using MBusLib.Utility;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  [Serializable]
  public sealed class MBusFrameCrypt : IPrintable
  {
    public Direction Direction { get; set; }

    public DateTime? ReadTime { get; set; }

    public FrameType Type { get; set; }

    public C_Field Control { get; set; }

    public byte Address { get; set; }

    public CI_Field ControlInfo { get; set; }

    public byte[] UserData { get; set; }

    public AFL AFL { get; set; }

    public int? EncryptionMode { get; set; }

    public byte[] EncryptionKey { get; set; }

    public MBusFrameCrypt PreviousFrame { get; set; }

    public string Print(int spaces = 0) => Util.PrintObject((object) this);

    public bool IsVariableDataStructure
    {
      get
      {
        return this.ControlInfo == CI_Field.MBusWithFullHeader || this.ControlInfo == CI_Field.MBusWithShortHeader || this.ControlInfo == CI_Field.Response_76h || this.ControlInfo == CI_Field.RequestReadoutOfCompleteRAMcontent;
      }
    }

    public DeviceVersion DeviceVersion
    {
      get
      {
        if (!this.IsVariableDataStructure)
          return (DeviceVersion) null;
        try
        {
          VariableDataStructure variableDataStructure = VariableDataStructure.Parse(this);
          if (variableDataStructure == null)
            return (DeviceVersion) null;
          ConfWord confWord = new ConfWord()
          {
            ConfigurationField = variableDataStructure.Header.Signature
          };
          return new DeviceVersion()
          {
            ID_BCD = variableDataStructure.Header.ID_BCD,
            Manufacturer = variableDataStructure.Header.Manufacturer,
            Generation = variableDataStructure.Header.Generation,
            Medium = (Medium) variableDataStructure.Header.Medium,
            MfgData = variableDataStructure.MfgData,
            EncryptionMode = confWord.EncryptionMode
          };
        }
        catch (Exception ex)
        {
          Util.Log((object) this, "DeviceVersion object can not be created!", ex);
          return (DeviceVersion) null;
        }
      }
    }

    public MBusFrameCrypt()
      : this((byte[]) null)
    {
    }

    public MBusFrameCrypt(byte[] userData)
    {
      this.UserData = userData;
      this.Type = FrameType.LongFrame;
      this.Control = C_Field.SND_UD_53h;
      this.Address = (byte) 254;
      this.ControlInfo = CI_Field.DataSendMode1;
    }

    public MBusFrameCrypt(
      Direction direction,
      byte[] userData,
      int? encryptionMode,
      byte[] encryptionKey,
      MBusFrameCrypt previousFrame)
      : this(userData)
    {
      this.Direction = direction;
      this.EncryptionMode = encryptionMode;
      this.EncryptionKey = encryptionKey;
      this.PreviousFrame = previousFrame;
      int? nullable = encryptionMode;
      int num = 7;
      if (!(nullable.GetValueOrDefault() == num & nullable.HasValue))
        return;
      this.ControlInfo = CI_Field.AFL;
    }

    public override string ToString()
    {
      if (this.Type == FrameType.ACK)
        return "ACK";
      return string.Format("{0} C:{1} ADDR:{2} CI:{3} IsVariableDataStructure:{4}", (object) this.Type, (object) this.Control, (object) this.Address, (object) this.ControlInfo, (object) this.IsVariableDataStructure);
    }

    public string Print(bool expert = true, int spaces = 0) => Util.PrintObject((object) this);

    public byte[] ToByteArray()
    {
      List<byte> buffer = this.Create();
      this.FinishFrame(buffer);
      return buffer.ToArray();
    }

    public static MBusFrameCrypt Parse(
      Direction direction,
      DateTime? readTime,
      byte buffer,
      byte[] key = null)
    {
      return MBusFrameCrypt.Parse(direction, readTime, new byte[1]
      {
        buffer
      }, key);
    }

    public static MBusFrameCrypt Parse(
      Direction direction,
      DateTime? readTime,
      byte[] buffer,
      byte[] key = null)
    {
      int offset = 0;
      return MBusFrameCrypt.Parse(direction, readTime, buffer, ref offset, key);
    }

    public static MBusFrameCrypt Parse(
      Direction direction,
      DateTime? readTime,
      byte[] buffer,
      ref int offset,
      byte[] key = null)
    {
      if (buffer == null || buffer.Length == 0)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length >= offset + 1 && buffer[offset] == (byte) 229)
      {
        ++offset;
        return new MBusFrameCrypt()
        {
          Direction = direction,
          ReadTime = readTime,
          Type = FrameType.ACK,
          Address = 0,
          Control = (C_Field) 0,
          ControlInfo = ~(CI_Field.OBIS_APL_ShortTPL | CI_Field.TPL_LongFromOtherDeviceToMeter)
        };
      }
      if (buffer.Length >= offset + 5 && buffer[offset] == (byte) 16)
      {
        MBusFrameCrypt.ValidateChecksum(buffer, offset);
        MBusFrameCrypt mbusFrameCrypt = new MBusFrameCrypt()
        {
          Direction = direction,
          ReadTime = readTime,
          Type = FrameType.ShortFrame,
          Control = (C_Field) buffer[offset + 1],
          Address = buffer[offset + 2]
        };
        offset += 5;
        return mbusFrameCrypt;
      }
      if (buffer.Length >= 9 && buffer[offset] == (byte) 104 && buffer[offset + 3] == (byte) 104 && (int) buffer[offset + 1] == (int) buffer[offset + 2] && buffer[offset + 1] == (byte) 3)
      {
        MBusFrameCrypt.ValidateChecksum(buffer, offset);
        MBusFrameCrypt mbusFrameCrypt = new MBusFrameCrypt()
        {
          Direction = direction,
          ReadTime = readTime,
          Type = FrameType.ControlFrame,
          Control = (C_Field) buffer[offset + 4],
          Address = buffer[offset + 5],
          ControlInfo = (CI_Field) buffer[offset + 6]
        };
        offset += 9;
        return mbusFrameCrypt;
      }
      if (buffer.Length <= 9 || buffer[offset] != (byte) 104 || buffer[offset + 3] != (byte) 104 || (int) buffer[offset + 1] != (int) buffer[offset + 2])
        throw new Exception("Unknown M-Bus frame! Buffer: " + Util.ByteArrayToHexString((IEnumerable<byte>) buffer));
      MBusFrameCrypt.ValidateChecksum(buffer, offset);
      byte num1 = buffer[offset + 1];
      offset += 4;
      C_Field cField = (C_Field) buffer[offset++];
      byte num2 = buffer[offset++];
      CI_Field ciField = (CI_Field) buffer[offset++];
      int length = (int) num1 - 3;
      AFL afl = (AFL) null;
      if (ciField == CI_Field.AFL)
      {
        afl = AFL.Decode(buffer, ref offset);
        int num3 = length - 1 - (int) afl.AFLL;
        ciField = (CI_Field) buffer[offset++];
        length = num3 - 1;
      }
      byte[] numArray = buffer.SubArray<byte>(offset, length);
      MBusFrameCrypt mbusFrameCrypt1 = new MBusFrameCrypt()
      {
        Direction = direction,
        ReadTime = readTime,
        Type = FrameType.LongFrame,
        Control = cField,
        Address = num2,
        ControlInfo = ciField,
        UserData = numArray,
        AFL = afl,
        EncryptionKey = key
      };
      offset += numArray.Length + 2;
      return mbusFrameCrypt1;
    }

    private static void ValidateChecksum(byte[] buffer, int offset)
    {
      byte count = buffer[offset] == (byte) 104 && buffer[offset + 3] == (byte) 104 && (int) buffer[offset + 1] == (int) buffer[offset + 2] ? buffer[offset + 1] : throw new Exception("Invalid M-Bus header! Expected (68 L L 68)");
      byte num = buffer[offset + (int) count + 4];
      if ((int) Util.CalculateChecksum(buffer, offset + 4, (int) count) == (int) num)
        return;
      Util.Log((object) "MBusFrame", "The checksum of M-Bus frame is invalid!");
    }

    private List<byte> Create()
    {
      switch (this.Type)
      {
        case FrameType.ACK:
          return new List<byte>((IEnumerable<byte>) new byte[1]
          {
            (byte) 229
          });
        case FrameType.ShortFrame:
          return new List<byte>((IEnumerable<byte>) new byte[3]
          {
            (byte) 16,
            (byte) this.Control,
            this.Address
          });
        case FrameType.ControlFrame:
        case FrameType.LongFrame:
          return this.CreateLongFrame();
        default:
          throw new NotImplementedException(this.Type.ToString());
      }
    }

    private void FinishFrame(List<byte> buffer)
    {
      if (this.Type == FrameType.ACK)
        return;
      int startIndex = this.Type == FrameType.ShortFrame ? 1 : 4;
      byte[] array = buffer.ToArray();
      byte checksum = Util.CalculateChecksum(array, startIndex, array.Length - startIndex);
      if (this.Type == FrameType.LongFrame)
      {
        buffer[1] = (byte) (buffer.Count - 4);
        buffer[2] = buffer[1];
      }
      buffer.Add(checksum);
      buffer.Add((byte) 22);
    }

    private List<byte> CreateLongFrame()
    {
      int? encryptionMode;
      int num1;
      if (this.EncryptionMode.HasValue)
      {
        encryptionMode = this.EncryptionMode;
        int num2 = 0;
        num1 = encryptionMode.GetValueOrDefault() == num2 & encryptionMode.HasValue ? 1 : 0;
      }
      else
        num1 = 1;
      if (num1 != 0)
      {
        List<byte> longFrame = new List<byte>((IEnumerable<byte>) new byte[7]
        {
          (byte) 104,
          (byte) 0,
          (byte) 0,
          (byte) 104,
          (byte) this.Control,
          this.Address,
          (byte) this.ControlInfo
        });
        if (this.Type != FrameType.LongFrame || this.UserData == null)
          return longFrame;
        longFrame.AddRange((IEnumerable<byte>) this.UserData);
        return longFrame;
      }
      encryptionMode = this.EncryptionMode;
      int num3 = 7;
      if (encryptionMode.GetValueOrDefault() == num3 & encryptionMode.HasValue)
        return this.CreateLongFrameForEncryptionMode7();
      throw new NotImplementedException(string.Format("The encryption mode {0} is not supported!", (object) this.EncryptionMode));
    }

    private List<byte> CreateLongFrameForEncryptionMode7()
    {
      uint num1 = 0;
      uint num2 = 0;
      byte num3 = 0;
      if (this.PreviousFrame != null)
      {
        if (this.PreviousFrame.AFL != null && this.PreviousFrame.AFL.MCR.HasValue)
          num2 = this.PreviousFrame.AFL.MCR.Value;
        if (this.PreviousFrame.IsVariableDataStructure)
        {
          FixedDataHeader fixedDataHeader = FixedDataHeader.Parse(this.PreviousFrame.UserData);
          if (fixedDataHeader != null)
          {
            if (fixedDataHeader.ID_BCD > 0U)
              num1 = fixedDataHeader.ID_BCD;
            if (fixedDataHeader.ACC > (byte) 0)
              num3 = fixedDataHeader.ACC;
          }
        }
      }
      List<byte> forEncryptionMode7 = new List<byte>((IEnumerable<byte>) new byte[7]
      {
        (byte) 104,
        (byte) 0,
        (byte) 0,
        (byte) 104,
        (byte) this.Control,
        this.Address,
        (byte) this.ControlInfo
      });
      this.AFL = new AFL()
      {
        AFLL = (byte) 15,
        FCL = new FCL((ushort) 11264),
        MCL = new byte?((byte) 37),
        MCR = new uint?(num2),
        MAC = new ulong?(0UL)
      };
      forEncryptionMode7.AddRange((IEnumerable<byte>) this.AFL.GetBytes());
      int count1 = forEncryptionMode7.Count;
      forEncryptionMode7.Add((byte) 122);
      forEncryptionMode7.Add(num3);
      forEncryptionMode7.Add((byte) 0);
      int num4 = this.UserData != null ? 16 - (this.UserData.Length + 2) % 16 : 0;
      ConfWord confWord = new ConfWord()
      {
        ConfigurationField = 1792,
        ConfigurationFieldExtension1 = new byte?((byte) 16)
      };
      confWord.CountOfBlocks = (byte) 0;
      int count2 = forEncryptionMode7.Count;
      forEncryptionMode7.AddRange((IEnumerable<byte>) confWord.GetBytes());
      int count3 = forEncryptionMode7.Count;
      forEncryptionMode7.Add((byte) 47);
      forEncryptionMode7.Add((byte) 47);
      if (this.UserData != null)
        forEncryptionMode7.AddRange((IEnumerable<byte>) this.UserData);
      int startIndex = 4;
      byte checksum = Util.CalculateChecksum(forEncryptionMode7.ToArray(), startIndex, forEncryptionMode7.Count - startIndex);
      forEncryptionMode7.Add(checksum);
      forEncryptionMode7.Add((byte) 22);
      confWord.CountOfBlocks = this.UserData != null ? (byte) ((this.UserData.Length + 2 + num4) / 16) : (byte) 0;
      byte[] bytes = confWord.GetBytes();
      for (int index = 0; index < bytes.Length; ++index)
        forEncryptionMode7[count2 + index] = bytes[index];
      for (int index = 0; index < num4; ++index)
        forEncryptionMode7.Add((byte) 47);
      List<byte> byteList1 = new List<byte>(16);
      byteList1.Add(this.Direction == Direction.DeviceToGateway ? (byte) 0 : (byte) 16);
      byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num2));
      byteList1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num1));
      byteList1.Add((byte) 7);
      byteList1.Add((byte) 7);
      byteList1.Add((byte) 7);
      byteList1.Add((byte) 7);
      byteList1.Add((byte) 7);
      byteList1.Add((byte) 7);
      byteList1.Add((byte) 7);
      byte[] key1 = AES.AESCMAC(this.EncryptionKey, byteList1.ToArray());
      byteList1[0] = this.Direction == Direction.DeviceToGateway ? (byte) 1 : (byte) 17;
      byte[] key2 = AES.AESCMAC(this.EncryptionKey, byteList1.ToArray());
      byte[] IV = new byte[16];
      for (int index = 0; index < 16; ++index)
        IV[index] = (byte) 0;
      byte[] decrypted = forEncryptionMode7.ToArray().SubArray<byte>(count3);
      byte[] numArray1 = Util.EncryptCBC_AES_128(key1, IV, decrypted);
      int index1 = count3;
      int index2 = 0;
      while (index1 < forEncryptionMode7.Count)
      {
        forEncryptionMode7[index1] = numArray1[index2];
        ++index1;
        ++index2;
      }
      byte[] collection = forEncryptionMode7.ToArray().SubArray<byte>(count1);
      List<byte> byteList2 = new List<byte>();
      byteList2.Add(this.AFL.MCL.Value);
      byteList2.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num2));
      byteList2.AddRange((IEnumerable<byte>) collection);
      byte[] numArray2 = AES.AESCMAC(key2, byteList2.ToArray());
      this.AFL.MAC = new ulong?(BitConverter.ToUInt64(numArray2, 0));
      forEncryptionMode7[15] = numArray2[0];
      forEncryptionMode7[16] = numArray2[1];
      forEncryptionMode7[17] = numArray2[2];
      forEncryptionMode7[18] = numArray2[3];
      forEncryptionMode7[19] = numArray2[4];
      forEncryptionMode7[20] = numArray2[5];
      forEncryptionMode7[21] = numArray2[6];
      forEncryptionMode7[22] = numArray2[7];
      return forEncryptionMode7;
    }
  }
}
