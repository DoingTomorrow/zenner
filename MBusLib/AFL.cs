// Decompiled with JetBrains decompiler
// Type: MBusLib.AFL
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MBusLib
{
  [Serializable]
  public class AFL
  {
    public byte AFLL { get; set; }

    public FCL FCL { get; set; }

    public byte? MCL { get; set; }

    public ushort? KI { get; set; }

    public uint? MCR { get; set; }

    public ulong? MAC { get; set; }

    public ushort? ML { get; set; }

    internal static AFL Decode(byte[] buffer, ref int offset)
    {
      AFL afl = new AFL();
      afl.AFLL = buffer[offset++];
      afl.FCL = FCL.Decode(buffer, ref offset);
      if (afl.FCL.MCL)
        afl.MCL = new byte?(buffer[offset++]);
      if (afl.FCL.KI)
      {
        afl.KI = new ushort?(BitConverter.ToUInt16(buffer, offset));
        offset += 2;
      }
      if (afl.FCL.MCR)
      {
        afl.MCR = new uint?(BitConverter.ToUInt32(buffer, offset));
        offset += 4;
      }
      if (afl.FCL.MAC)
      {
        afl.MAC = new ulong?(BitConverter.ToUInt64(buffer, offset));
        offset += 8;
      }
      if (afl.FCL.ML)
      {
        afl.ML = new ushort?(BitConverter.ToUInt16(buffer, offset));
        offset += 2;
      }
      return afl;
    }

    public byte[] GetBytes()
    {
      if (this.FCL == null)
        return (byte[]) null;
      List<byte> byteList = new List<byte>() { this.AFLL };
      byteList.AddRange((IEnumerable<byte>) this.FCL.GetBytes());
      if (this.MCL.HasValue)
        byteList.Add(this.MCL.Value);
      if (this.MCR.HasValue)
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.MCR.Value));
      if (this.MAC.HasValue)
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.MAC.Value));
      return byteList.ToArray();
    }
  }
}
