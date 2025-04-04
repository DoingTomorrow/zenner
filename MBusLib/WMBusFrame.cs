// Decompiled with JetBrains decompiler
// Type: MBusLib.WMBusFrame
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using NLog;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class WMBusFrame : IPrintable
  {
    private static Logger logger = LogManager.GetLogger(nameof (WMBusFrame));
    private static int HEADER_SIZE = 10;

    public byte Length { get; private set; }

    public C_Field Control { get; private set; }

    public ushort Manufacturer { get; private set; }

    public uint ID_BCD { get; private set; }

    public byte Generation { get; private set; }

    public byte Medium { get; private set; }

    public CI_Field ControlInfo { get; private set; }

    public byte ACC { get; private set; }

    public byte STS { get; private set; }

    public ushort ConfWord { get; private set; }

    public byte[] EncodedData { get; private set; }

    public byte[] DecodedData { get; private set; }

    public uint? ID_BCD_Secundary { get; private set; }

    public ushort? Manufacturer_Secundary { get; private set; }

    public byte? Generation_Secundary { get; private set; }

    public byte? Medium_Secundary { get; private set; }

    public uint ID => Util.ConvertBcdUInt32ToUInt32(this.ID_BCD);

    public string ManufacturerString => MBusUtil.GetManufacturer(this.Manufacturer);

    public string MediumString => MBusUtil.GetMedium(this.Medium);

    public uint? ID_Secundary
    {
      get
      {
        return this.ID_BCD_Secundary.HasValue ? new uint?(Util.ConvertBcdUInt32ToUInt32(this.ID_BCD_Secundary.Value)) : new uint?();
      }
    }

    public string ManufacturerString_Secundary
    {
      get
      {
        return this.Manufacturer_Secundary.HasValue ? MBusUtil.GetManufacturer(this.Manufacturer_Secundary.Value) : (string) null;
      }
    }

    public string MediumString_Secundary
    {
      get
      {
        return this.Medium_Secundary.HasValue ? MBusUtil.GetMedium(this.Medium_Secundary.Value) : (string) null;
      }
    }

    public int EncryptionMode => ((int) this.ConfWord & 3840) >> 8;

    public int CounOfBlocks => (((int) this.ConfWord & 240) >> 4) * 16;

    public bool Synchronous => Convert.ToBoolean(((int) this.ConfWord & 8192) >> 13);

    public override string ToString() => base.ToString();

    public string Print(int spaces = 0) => Util.PrintObject((object) this);

    public static WMBusFrame Parse(byte[] buffer, byte[] aes_key = null, int startIndex = 0)
    {
      if (buffer == null || buffer.Length == 0)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length < WMBusFrame.HEADER_SIZE)
        throw new InvalidFrameException("Wrong length", buffer);
      WMBusFrame wmBusFrame1 = new WMBusFrame();
      int num1 = startIndex;
      WMBusFrame wmBusFrame2 = wmBusFrame1;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = (int) numArray1[index1];
      wmBusFrame2.Length = (byte) num3;
      WMBusFrame wmBusFrame3 = wmBusFrame1;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int startIndex1 = index2 + 1;
      int num4 = (int) numArray2[index2];
      wmBusFrame3.Control = (C_Field) num4;
      wmBusFrame1.Manufacturer = BitConverter.ToUInt16(buffer, startIndex1);
      int startIndex2 = startIndex1 + 2;
      wmBusFrame1.ID_BCD = BitConverter.ToUInt32(buffer, startIndex2);
      int num5 = startIndex2 + 4;
      WMBusFrame wmBusFrame4 = wmBusFrame1;
      byte[] numArray3 = buffer;
      int index3 = num5;
      int num6 = index3 + 1;
      int num7 = (int) numArray3[index3];
      wmBusFrame4.Generation = (byte) num7;
      WMBusFrame wmBusFrame5 = wmBusFrame1;
      byte[] numArray4 = buffer;
      int index4 = num6;
      int num8 = index4 + 1;
      int num9 = (int) numArray4[index4];
      wmBusFrame5.Medium = (byte) num9;
      WMBusFrame wmBusFrame6 = wmBusFrame1;
      byte[] numArray5 = buffer;
      int index5 = num8;
      int startIndex3 = index5 + 1;
      int num10 = (int) numArray5[index5];
      wmBusFrame6.ControlInfo = (CI_Field) num10;
      if (wmBusFrame1.ControlInfo == CI_Field.MBusWithFullHeader)
      {
        wmBusFrame1.ID_BCD_Secundary = new uint?(BitConverter.ToUInt32(buffer, startIndex3));
        int startIndex4 = startIndex3 + 4;
        wmBusFrame1.Manufacturer_Secundary = new ushort?(BitConverter.ToUInt16(buffer, startIndex4));
        int num11 = startIndex4 + 2;
        WMBusFrame wmBusFrame7 = wmBusFrame1;
        byte[] numArray6 = buffer;
        int index6 = num11;
        int num12 = index6 + 1;
        byte? nullable1 = new byte?(numArray6[index6]);
        wmBusFrame7.Generation_Secundary = nullable1;
        WMBusFrame wmBusFrame8 = wmBusFrame1;
        byte[] numArray7 = buffer;
        int index7 = num12;
        startIndex3 = index7 + 1;
        byte? nullable2 = new byte?(numArray7[index7]);
        wmBusFrame8.Medium_Secundary = nullable2;
      }
      WMBusFrame wmBusFrame9 = wmBusFrame1;
      byte[] numArray8 = buffer;
      int index8 = startIndex3;
      int num13 = index8 + 1;
      int num14 = (int) numArray8[index8];
      wmBusFrame9.ACC = (byte) num14;
      WMBusFrame wmBusFrame10 = wmBusFrame1;
      byte[] numArray9 = buffer;
      int index9 = num13;
      int num15 = index9 + 1;
      int num16 = (int) numArray9[index9];
      wmBusFrame10.STS = (byte) num16;
      WMBusFrame wmBusFrame11 = wmBusFrame1;
      byte[] numArray10 = buffer;
      int index10 = num15;
      int num17 = index10 + 1;
      int num18 = (int) numArray10[index10];
      wmBusFrame11.ConfWord = (ushort) num18;
      WMBusFrame wmBusFrame12 = wmBusFrame1;
      int confWord = (int) wmBusFrame12.ConfWord;
      byte[] numArray11 = buffer;
      int index11 = num17;
      int srcOffset1 = index11 + 1;
      int num19 = (int) (ushort) ((uint) numArray11[index11] << 8);
      wmBusFrame12.ConfWord = (ushort) (confWord | num19);
      byte num20 = buffer[srcOffset1];
      byte num21 = buffer[srcOffset1 + 1];
      if (num20 == (byte) 47 && num21 == (byte) 47)
      {
        int srcOffset2 = srcOffset1 + 1 + 1;
        byte[] dst = new byte[(int) wmBusFrame1.Length - srcOffset2 + 1];
        Buffer.BlockCopy((Array) buffer, srcOffset2, (Array) dst, 0, dst.Length);
        wmBusFrame1.EncodedData = dst;
        wmBusFrame1.DecodedData = dst;
      }
      else
      {
        byte[] bytes1;
        byte[] bytes2;
        byte generation;
        byte medium;
        if (wmBusFrame1.ControlInfo == CI_Field.MBusWithFullHeader)
        {
          bytes1 = BitConverter.GetBytes(wmBusFrame1.ID_BCD_Secundary.Value);
          bytes2 = BitConverter.GetBytes(wmBusFrame1.Manufacturer_Secundary.Value);
          generation = wmBusFrame1.Generation_Secundary.Value;
          medium = wmBusFrame1.Medium_Secundary.Value;
        }
        else
        {
          bytes1 = BitConverter.GetBytes(wmBusFrame1.ID_BCD);
          bytes2 = BitConverter.GetBytes(wmBusFrame1.Manufacturer);
          generation = wmBusFrame1.Generation;
          medium = wmBusFrame1.Medium;
        }
        if (wmBusFrame1.EncryptionMode == 5)
        {
          byte[] IV = new byte[16]
          {
            bytes2[0],
            bytes2[1],
            bytes1[0],
            bytes1[1],
            bytes1[2],
            bytes1[3],
            generation,
            medium,
            wmBusFrame1.ACC,
            wmBusFrame1.ACC,
            wmBusFrame1.ACC,
            wmBusFrame1.ACC,
            wmBusFrame1.ACC,
            wmBusFrame1.ACC,
            wmBusFrame1.ACC,
            wmBusFrame1.ACC
          };
          try
          {
            byte[] dst = new byte[wmBusFrame1.CounOfBlocks];
            Buffer.BlockCopy((Array) buffer, srcOffset1, (Array) dst, 0, dst.Length);
            wmBusFrame1.EncodedData = dst;
            byte[] key = aes_key ?? Util.HexStringToByteArray("5A8470C4806F4A87CEF4D5F2D985AB18");
            wmBusFrame1.DecodedData = MBusUtil.DecryptCBC_AES_128(key, IV, wmBusFrame1.EncodedData);
            if (wmBusFrame1.DecodedData[0] == (byte) 47 && wmBusFrame1.DecodedData[1] == (byte) 47)
            {
              List<byte> byteList = new List<byte>((IEnumerable<byte>) wmBusFrame1.DecodedData);
              byteList.RemoveAt(0);
              byteList.RemoveAt(0);
              for (int index12 = byteList.Count - 1; index12 >= 0; --index12)
              {
                if (byteList[index12] == (byte) 47)
                  byteList.RemoveAt(index12);
              }
              wmBusFrame1.DecodedData = byteList.ToArray();
            }
            else
              wmBusFrame1.DecodedData = (byte[]) null;
          }
          catch (Exception ex)
          {
            wmBusFrame1.DecodedData = (byte[]) null;
            WMBusFrame.logger.Error(ex.Message);
          }
        }
      }
      return wmBusFrame1;
    }

    public List<MBusValue> GetValues()
    {
      int startIndex = 0;
      List<MBusValue> values = new List<MBusValue>();
      while (startIndex < this.DecodedData.Length)
      {
        VariableDataBlock variableDataBlock = VariableDataBlock.Parse(this.DecodedData, startIndex);
        startIndex += variableDataBlock.Size;
        values.Add(variableDataBlock.Value);
      }
      return values;
    }
  }
}
