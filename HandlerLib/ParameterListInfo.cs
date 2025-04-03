// Decompiled with JetBrains decompiler
// Type: HandlerLib.ParameterListInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace HandlerLib
{
  public sealed class ParameterListInfo
  {
    public ParameterListInfo.ParameterDevice S3_Device { get; set; }

    public ParameterListInfo.ParameterDevice Radio { get; set; }

    public ParameterListInfo()
    {
      this.S3_Device = new ParameterListInfo.ParameterDevice();
      this.Radio = new ParameterListInfo.ParameterDevice();
    }

    public static ParameterListInfo Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      ParameterListInfo parameterListInfo = new ParameterListInfo();
      parameterListInfo.S3_Device.MaxList = (int) buffer[0];
      parameterListInfo.S3_Device.SelectedList = (int) buffer[1];
      parameterListInfo.S3_Device.Sublists = (int) buffer[2];
      parameterListInfo.Radio.MaxList = (int) buffer[3];
      if (parameterListInfo.Radio.MaxList != (int) byte.MaxValue)
      {
        parameterListInfo.Radio.SelectedList = (int) buffer[4];
        if (parameterListInfo.Radio.SelectedList >= parameterListInfo.Radio.MaxList)
          throw new ArgumentException("selected lis > max list");
      }
      if (buffer.Length >= 8)
      {
        parameterListInfo.Radio.AES_EncMode = (AES_ENCRYPTION_MODE) ((int) buffer[5] & 15);
        if (!Enum.IsDefined(typeof (AES_ENCRYPTION_MODE), (object) parameterListInfo.Radio.AES_EncMode))
          throw new Exception("Undefined EncryptionMode received");
        parameterListInfo.Radio.RadioMode = new RADIO_MODE?((RADIO_MODE) (((int) buffer[5] & 240) >> 4));
        if (!Enum.IsDefined(typeof (RADIO_MODE), (object) parameterListInfo.Radio.RadioMode))
          throw new Exception("Undefined RadioMode received");
        parameterListInfo.Radio.Cycletime = new int?((int) buffer[7] << 8 | (int) buffer[6]);
      }
      return parameterListInfo;
    }

    public static byte[] GetCommandPayload(ushort list, bool isRadio)
    {
      List<byte> byteList = new List<byte>();
      byte num1 = 0;
      if (isRadio)
        num1 = (byte) 1;
      byte num2 = (byte) ((int) num1 << 4 | 8);
      if (((uint) list & 1U) > 0U)
        num2 |= (byte) 64;
      list >>= 1;
      if (list > (ushort) 0)
        num2 |= (byte) 128;
      byteList.Add(num2);
      while (((int) num2 & 128) == 128)
      {
        num2 = (byte) ((uint) list & 15U);
        list >>= 4;
        if (list > (ushort) 0)
          num2 |= (byte) 128;
        byteList.Add(num2);
      }
      return byteList.ToArray();
    }

    public static byte[] GetCommandPayload(ushort list, bool isRadio, ushort enc_mode)
    {
      List<byte> byteList = new List<byte>();
      byte num1 = 0;
      if (isRadio)
        num1 = (byte) 1;
      byte num2 = (byte) ((int) num1 << 4 | 8);
      if (((uint) list & 1U) > 0U)
        num2 |= (byte) 64;
      list >>= 1;
      if (list > (ushort) 0 || enc_mode > (ushort) 0)
        num2 |= (byte) 128;
      byteList.Add(num2);
      while (((int) num2 & 128) == 128)
      {
        num2 = (byte) ((int) list & 15 | ((int) enc_mode & 3) << 4);
        list >>= 4;
        enc_mode >>= 2;
        if (list > (ushort) 0 || enc_mode > (ushort) 0)
          num2 |= (byte) 128;
        byteList.Add(num2);
      }
      return byteList.ToArray();
    }

    public class ParameterDevice
    {
      public int MaxList { get; set; }

      public int SelectedList { get; set; }

      public int Sublists { get; set; }

      public AES_ENCRYPTION_MODE AES_EncMode { get; set; }

      public RADIO_MODE? RadioMode { get; set; }

      public int? Cycletime { get; set; }
    }
  }
}
