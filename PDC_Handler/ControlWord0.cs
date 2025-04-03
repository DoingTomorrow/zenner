// Decompiled with JetBrains decompiler
// Type: PDC_Handler.ControlWord0
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;

#nullable disable
namespace PDC_Handler
{
  public sealed class ControlWord0
  {
    public ushort ControlWord { get; set; }

    public ControlWord0(ushort controlWord) => this.ControlWord = controlWord;

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

    public Param Param
    {
      get => (Param) Enum.ToObject(typeof (Param), (int) this.ControlWord & 240);
      set
      {
        this.ControlWord &= (ushort) 65295;
        this.ControlWord = (ushort) ((Param) this.ControlWord | value);
      }
    }

    public ParamLog ParamLog
    {
      get => (ParamLog) Enum.ToObject(typeof (ParamLog), (int) this.ControlWord & 28672);
      set
      {
        this.ControlWord &= (ushort) 36863;
        this.ControlWord = (ushort) ((ParamLog) this.ControlWord | value);
      }
    }

    public override string ToString()
    {
      return string.Format("PRM: {0}, {1} bytes, {2} VIFDIF, PRMLOG: {3}", (object) this.Param, (object) this.ByteCount, (object) this.VifDifCount, (object) this.ParamLog);
    }
  }
}
