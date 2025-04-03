// Decompiled with JetBrains decompiler
// Type: S3_Handler.ControlWord2
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;

#nullable disable
namespace S3_Handler
{
  [Serializable]
  internal class ControlWord2
  {
    public ControlWord2()
    {
    }

    public ControlWord2(ushort controlWord) => this.ControlWord = controlWord;

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

    public bool IsSavePtr
    {
      get => ((int) this.ControlWord & 256) == 256;
      set
      {
        if (value)
          this.ControlWord |= (ushort) 256;
        else
          this.ControlWord &= (ushort) 65279;
      }
    }

    public bool IsAllowLoggerEnd
    {
      get => ((int) this.ControlWord & 512) == 512;
      set
      {
        if (value)
          this.ControlWord |= (ushort) 512;
        else
          this.ControlWord &= (ushort) 65023;
      }
    }

    public int ListMaxCount
    {
      get => (int) this.ControlWord & (int) byte.MaxValue;
      set
      {
        this.ControlWord &= (ushort) 65280;
        this.ControlWord |= (ushort) value;
      }
    }

    public override string ToString()
    {
      return string.Format("{0} {1} ListMaxCount: {2}", this.IsSavePtr ? (object) "IsSavePtr" : (object) string.Empty, this.IsAllowLoggerEnd ? (object) "IsAllowLoggerEnd" : (object) string.Empty, (object) this.ListMaxCount).Trim();
    }
  }
}
