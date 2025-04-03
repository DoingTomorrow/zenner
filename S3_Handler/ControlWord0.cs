// Decompiled with JetBrains decompiler
// Type: S3_Handler.ControlWord0
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;

#nullable disable
namespace S3_Handler
{
  [Serializable]
  internal class ControlWord0
  {
    public ControlWord0()
    {
    }

    public ControlWord0(ushort controlWord) => this.ControlWord = controlWord;

    public ushort ControlWord { get; private set; }

    public bool ItFollowsNextControlWord
    {
      get => ((int) this.ControlWord & 32768) == 32768;
      set
      {
        if (value)
          this.ControlWord |= (ushort) 32768;
        else
          this.ControlWord &= (ushort) short.MaxValue;
      }
    }

    public ControlLogger ControlLogger
    {
      get => (ControlLogger) Enum.ToObject(typeof (ControlLogger), (int) this.ControlWord & 28672);
      set
      {
        this.ControlWord &= (ushort) 36863;
        this.ControlWord = (ushort) ((ControlLogger) this.ControlWord | value);
      }
    }

    public int DataCount
    {
      get => ((int) this.ControlWord & 3840) >> 8;
      set
      {
        if (this.DataCount < 0)
          throw new Exception("DataCount can not be less as 0!");
        this.ControlWord &= (ushort) 61695;
        this.ControlWord |= (ushort) (value << 8);
      }
    }

    public bool IsBCDByRadio
    {
      get => ((int) this.ControlWord & 128) == 128;
      set
      {
        if (value)
          this.ControlWord |= (ushort) 128;
        else
          this.ControlWord &= (ushort) 65407;
      }
    }

    public ParamCode ParamCode
    {
      get => (ParamCode) Enum.ToObject(typeof (ParamCode), (int) this.ControlWord & 112);
      set
      {
        this.ControlWord &= (ushort) 65423;
        this.ControlWord |= (ushort) value;
      }
    }

    public int DifVifCount
    {
      get => (int) this.ControlWord & 15;
      set
      {
        if (this.DataCount < 0)
          throw new Exception("DataCount can not be less as 0!");
        this.ControlWord &= (ushort) 65520;
        this.ControlWord |= (ushort) value;
      }
    }

    public bool IsInvalid => this.ControlWord == (ushort) 0;

    public override string ToString()
    {
      return string.Format("{0}, BCD:{1}, {2}, DataCount: {3}", (object) this.ControlLogger, (object) this.IsBCDByRadio, (object) this.ParamCode, (object) this.DataCount);
    }
  }
}
