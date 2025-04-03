// Decompiled with JetBrains decompiler
// Type: HandlerLib.CRC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public static class CRC
  {
    public static ushort CRC_CCITT(byte[] buffer, ushort offset, ushort size)
    {
      byte[] numArray = new byte[(int) size];
      Buffer.BlockCopy((Array) buffer, (int) offset, (Array) numArray, 0, (int) size);
      return CRC.CRC_CCITT(numArray);
    }

    public static ushort CRC_CCITT(byte[] buffer)
    {
      ushort num1 = ushort.MaxValue;
      ushort num2 = 4129;
      for (int index1 = 0; index1 < buffer.Length; ++index1)
      {
        num1 ^= (ushort) ((uint) buffer[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((uint) num1 & 32768U) > 0U)
            num1 = (ushort) ((uint) num1 << 1 ^ (uint) num2);
          else
            num1 <<= 1;
        }
      }
      return num1;
    }

    public static ushort CRC_CCITT(byte[] buffer, ushort offset, ushort size, ushort initValue)
    {
      byte[] numArray = new byte[(int) size];
      Buffer.BlockCopy((Array) buffer, (int) offset, (Array) numArray, 0, (int) size);
      return CRC.CRC_CCITT(numArray, initValue);
    }

    public static ushort CRC_CCITT(byte[] buffer, ushort initValue)
    {
      ushort num1 = initValue;
      ushort num2 = 4129;
      for (int index1 = 0; index1 < buffer.Length; ++index1)
      {
        num1 ^= (ushort) ((uint) buffer[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((uint) num1 & 32768U) > 0U)
            num1 = (ushort) ((uint) num1 << 1 ^ (uint) num2);
          else
            num1 <<= 1;
        }
      }
      return num1;
    }

    public static ushort CRC_16(byte[] buffer, ushort offset, ushort size)
    {
      byte[] numArray = new byte[(int) size];
      Buffer.BlockCopy((Array) buffer, (int) offset, (Array) numArray, 0, (int) size);
      return CRC.CRC_16(numArray, ushort.MaxValue);
    }

    public static ushort CRC_16(byte[] buffer, ushort initValue)
    {
      ushort num1 = initValue;
      ushort num2 = 32773;
      for (int index1 = 0; index1 < buffer.Length; ++index1)
      {
        num1 ^= (ushort) ((uint) buffer[index1] << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((uint) num1 & 32768U) > 0U)
            num1 = (ushort) ((uint) num1 << 1 ^ (uint) num2);
          else
            num1 <<= 1;
        }
      }
      return num1;
    }

    public static ushort CRC_16(byte[] buffer) => CRC.CRC_16(buffer, ushort.MaxValue);

    public static uint CRC_32(byte[] buffer, ushort offset, ushort size)
    {
      byte[] numArray = new byte[(int) size];
      Buffer.BlockCopy((Array) buffer, (int) offset, (Array) numArray, 0, (int) size);
      return CRC.CRC_32(numArray);
    }

    public static uint CRC_32(byte[] buffer) => throw new NotImplementedException();
  }
}
