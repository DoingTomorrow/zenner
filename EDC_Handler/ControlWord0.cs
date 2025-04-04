// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ControlWord0
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  public sealed class ControlWord0
  {
    public ControlWord0(ushort controlWord) => this.ControlWord = controlWord;

    public ushort ControlWord { get; set; }

    public int ByteCount
    {
      get => ((int) this.ControlWord & 3840) >> 8;
      set
      {
        this.ControlWord &= (ushort) 61695;
        this.ControlWord |= (ushort) (value << 8);
      }
    }

    public int VifDifCount
    {
      get => (int) this.ControlWord & 15;
      set
      {
        this.ControlWord &= (ushort) 65520;
        this.ControlWord |= (ushort) value;
      }
    }

    public ParamCode ParamCode
    {
      get => (ParamCode) Enum.ToObject(typeof (ParamCode), (int) this.ControlWord & 240);
      set
      {
        this.ControlWord &= (ushort) 65295;
        this.ControlWord |= (ushort) value;
      }
    }

    public ControlLogger ControlLogger
    {
      get => (ControlLogger) Enum.ToObject(typeof (ControlLogger), (int) this.ControlWord & 28672);
      set
      {
        this.ControlWord &= (ushort) 36863;
        this.ControlWord |= (ushort) value;
      }
    }

    public bool IsInvalid => this.ControlWord == (ushort) 0;
  }
}
