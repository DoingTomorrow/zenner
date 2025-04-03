// Decompiled with JetBrains decompiler
// Type: S3_Handler.ControlWord1
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;

#nullable disable
namespace S3_Handler
{
  [Serializable]
  internal class ControlWord1
  {
    public ControlWord1()
    {
    }

    public ControlWord1(ushort controlWord) => this.ControlWord = controlWord;

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

    public bool IsSwitch
    {
      get => ((int) this.ControlWord & 16384) == 16384;
      set
      {
        if (value)
          this.ControlWord |= (ushort) 16384;
        else
          this.ControlWord &= (ushort) 49151;
      }
    }

    public int LoggerCycleCount
    {
      get => ((int) this.ControlWord & 16128) >> 8;
      set
      {
        this.ControlWord &= (ushort) 49407;
        this.ControlWord |= (ushort) (value << 8);
      }
    }

    public DecodeCommand DecodeCommand
    {
      get => (DecodeCommand) Enum.ToObject(typeof (DecodeCommand), (int) this.ControlWord & 240);
      set
      {
        this.ControlWord &= (ushort) 65295;
        this.ControlWord |= (ushort) value;
      }
    }

    public int LoggerNextList
    {
      get => (int) this.ControlWord & 15;
      set
      {
        this.ControlWord &= (ushort) 65520;
        this.ControlWord |= (ushort) value;
      }
    }

    public override string ToString()
    {
      return string.Format("LoggerCycleCount: {0}, {1}, LoggerNextList: {2}", (object) this.LoggerCycleCount, (object) this.DecodeCommand, (object) this.LoggerNextList);
    }
  }
}
