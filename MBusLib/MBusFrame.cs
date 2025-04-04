// Decompiled with JetBrains decompiler
// Type: MBusLib.MBusFrame
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class MBusFrame : IPrintable
  {
    public FrameType Type { get; set; }

    public C_Field Control { get; set; }

    public byte Address { get; set; }

    public CI_Field ControlInfo { get; set; }

    public byte[] UserData { get; set; }

    public bool IsVariableDataStructure
    {
      get
      {
        return this.ControlInfo == CI_Field.MBusWithFullHeader || this.ControlInfo == CI_Field.Response_76h;
      }
    }

    public MBusFrame()
      : this((byte[]) null)
    {
    }

    public MBusFrame(byte[] userData)
    {
      this.UserData = userData;
      this.Type = FrameType.LongFrame;
      this.Control = C_Field.SND_UD_53h;
      this.Address = (byte) 254;
      this.ControlInfo = CI_Field.DataSendMode1;
    }

    public override string ToString()
    {
      if (this.Type == FrameType.ACK)
        return "ACK";
      return string.Format("{0} C:{1} A:{2} CI:{3}", (object) this.Type, (object) this.Control, (object) this.Address, (object) this.ControlInfo);
    }

    public string Print(int spaces = 0) => Util.PrintObject((object) this);

    public byte[] ToByteArray()
    {
      List<byte> buffer = this.Create();
      if (this.Type == FrameType.LongFrame && this.UserData != null)
        buffer.AddRange((IEnumerable<byte>) this.UserData);
      this.FinishFrame(buffer);
      return buffer.ToArray();
    }

    public static MBusFrame Parse(byte[] buffer)
    {
      if (buffer == null || buffer.Length == 0)
        throw new ArgumentNullException(nameof (buffer));
      if (buffer.Length == 1 && buffer[0] == (byte) 229)
        return new MBusFrame()
        {
          Type = FrameType.ACK,
          Address = 0,
          Control = (C_Field) 0,
          ControlInfo = ~(CI_Field.OBIS_APL_ShortTPL | CI_Field.TPL_LongFromOtherDeviceToMeter)
        };
      if (buffer.Length == 5 && buffer[0] == (byte) 16)
        return new MBusFrame()
        {
          Type = FrameType.ShortFrame,
          Control = (C_Field) buffer[1],
          Address = buffer[2]
        };
      if (buffer.Length == 9)
        return new MBusFrame()
        {
          Type = FrameType.ControlFrame,
          Control = (C_Field) buffer[4],
          Address = buffer[5],
          ControlInfo = (CI_Field) buffer[6]
        };
      return new MBusFrame()
      {
        Type = FrameType.LongFrame,
        Control = (C_Field) buffer[4],
        Address = buffer[5],
        ControlInfo = (CI_Field) buffer[6],
        UserData = MBusUtil.GetUserData(buffer, 7)
      };
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
          return new List<byte>((IEnumerable<byte>) new byte[7]
          {
            (byte) 104,
            (byte) 0,
            (byte) 0,
            (byte) 104,
            (byte) this.Control,
            this.Address,
            (byte) this.ControlInfo
          });
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
      byte checksum = MBusUtil.CalculateChecksum(array, startIndex, array.Length);
      if (this.Type == FrameType.LongFrame)
      {
        buffer[1] = (byte) (buffer.Count - 4);
        buffer[2] = buffer[1];
      }
      buffer.Add(checksum);
      buffer.Add((byte) 22);
    }
  }
}
