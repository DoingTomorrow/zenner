// Decompiled with JetBrains decompiler
// Type: MBusLib.FCL
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;

#nullable disable
namespace MBusLib
{
  [Serializable]
  public class FCL
  {
    private ushort fcl;

    public bool MF => ((uint) this.fcl & 16384U) > 0U;

    public bool MCL => ((uint) this.fcl & 8192U) > 0U;

    public bool ML => ((uint) this.fcl & 4096U) > 0U;

    public bool MCR => ((uint) this.fcl & 2048U) > 0U;

    public bool MAC => ((uint) this.fcl & 1024U) > 0U;

    public bool KI => ((uint) this.fcl & 512U) > 0U;

    public byte FID => (byte) ((uint) this.fcl & (uint) byte.MaxValue);

    public FCL(ushort fcl) => this.fcl = fcl;

    public static FCL Decode(byte[] buffer, ref int offset)
    {
      ushort uint16 = BitConverter.ToUInt16(buffer, offset);
      offset += 2;
      return new FCL(uint16);
    }

    public byte[] GetBytes() => BitConverter.GetBytes(this.fcl);
  }
}
