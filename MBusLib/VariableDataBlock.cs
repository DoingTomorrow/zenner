// Decompiled with JetBrains decompiler
// Type: MBusLib.VariableDataBlock
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class VariableDataBlock : IPrintable
  {
    public DIF DIF { get; set; }

    public List<MBusLib.DIFE> DIFE { get; set; }

    public VIF VIF { get; set; }

    public string VIF_0xFC { get; set; }

    public List<VIF> VIFE { get; set; }

    public byte[] Data { get; set; }

    public MBusValue Value => new MBusValue(this);

    public int Size
    {
      get
      {
        int size = 2;
        if (this.DIFE != null)
          size += this.DIFE.Count;
        if (this.VIFE != null)
          size += this.VIFE.Count;
        if (this.Data != null)
          size += this.Data.Length;
        if (this.VIF_0xFC != null)
          size += this.VIF_0xFC.Length + 1;
        return size;
      }
    }

    public override string ToString() => this.Print(0);

    public string Print(int spaces = 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.DIF != null)
        stringBuilder.Append(' ', spaces).Append("DIF:").Append((object) this.DIF).Append("(").Append((object) this.DIF.DataField).Append(")");
      if (this.DIFE != null && this.DIFE.Count > 0)
      {
        stringBuilder.Append("  DIFE:");
        foreach (MBusLib.DIFE dife in this.DIFE)
          stringBuilder.Append((object) dife).Append(' ');
      }
      if (this.VIF != null)
        stringBuilder.Append(" VIF:").Append((object) this.VIF);
      if (this.VIFE != null && this.VIFE.Count > 0)
      {
        stringBuilder.Append("  VIFE:");
        foreach (VIF vif in this.VIFE)
          stringBuilder.Append((object) vif).Append(' ');
      }
      if (this.Data != null && this.Data.Length != 0)
        stringBuilder.Append("  Data(HEX):").Append(Util.ByteArrayToHexString((IEnumerable<byte>) this.Data)).Append(' ');
      stringBuilder.Append((object) this.Value);
      return stringBuilder.ToString();
    }

    public byte[] ToByteArray()
    {
      List<byte> byteList = new List<byte>();
      byteList.Add(this.DIF.Value);
      if (this.DIFE != null)
      {
        foreach (MBusLib.DIFE dife in this.DIFE)
          byteList.Add(dife.Value);
      }
      byteList.Add(this.VIF.Value);
      if (this.VIFE != null)
      {
        foreach (VIF vif in this.VIFE)
          byteList.Add(vif.Value);
      }
      if (this.Data != null && this.Data.Length != 0)
        byteList.AddRange((IEnumerable<byte>) this.Data);
      return byteList.ToArray();
    }

    public static VariableDataBlock Parse(byte[] buffer, int startIndex)
    {
      if (buffer == null || buffer.Length < 2)
        throw new ArgumentException(nameof (buffer));
      if (startIndex >= buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (startIndex));
      int num1 = startIndex;
      VariableDataBlock vdb = new VariableDataBlock();
      VariableDataBlock variableDataBlock1 = vdb;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int srcOffset = index1 + 1;
      DIF dif = DIF.Parse(numArray1[index1]);
      variableDataBlock1.DIF = dif;
      if (vdb.DIF.HasExtension)
      {
        vdb.DIFE = new List<MBusLib.DIFE>();
        MBusLib.DIFE dife;
        do
        {
          dife = MBusLib.DIFE.Parse(buffer[srcOffset++]);
          vdb.DIFE.Add(dife);
          if (vdb.DIFE.Count > 10)
            throw new Exception("Too many DIFE! Expected maximum ten.");
        }
        while (dife.HasExtension);
      }
      if (vdb.DIF.Value == (byte) 15)
      {
        byte[] dst = new byte[buffer.Length - srcOffset];
        Buffer.BlockCopy((Array) buffer, srcOffset, (Array) dst, 0, dst.Length);
        vdb.Data = dst;
        return vdb;
      }
      VariableDataBlock variableDataBlock2 = vdb;
      byte[] numArray2 = buffer;
      int index2 = srcOffset;
      int offset = index2 + 1;
      VIF vif1 = VIF.Parse(numArray2[index2]);
      variableDataBlock2.VIF = vif1;
      if (vdb.VIF.UnitAndMultiplier == (byte) 124)
      {
        byte[] numArray3 = buffer;
        int index3 = offset;
        int index4 = index3 + 1;
        byte count = numArray3[index3];
        vdb.VIF_0xFC = Encoding.ASCII.GetString(buffer, index4, (int) count);
        offset = index4 + (int) count;
      }
      if (vdb.VIF.HasExtension)
      {
        vdb.VIFE = new List<VIF>();
        VIF vif2;
        do
        {
          vif2 = VIF.Parse(buffer[offset++]);
          vdb.VIFE.Add(vif2);
          if (vdb.VIFE.Count > 10)
            throw new Exception("Too many VIFE! Expected maximum ten.");
        }
        while (vif2.HasExtension);
      }
      VariableDataBlock.SetData(buffer, offset, vdb);
      if (vdb.Data != null)
      {
        int num2 = offset + vdb.Data.Length;
      }
      return vdb;
    }

    public void SetDIF(byte value) => this.DIF = DIF.Parse(value);

    public void SetVIF(byte value) => this.VIF = VIF.Parse(value);

    public void SetData(byte[] value) => this.Data = value;

    private static void SetData(byte[] buffer, int offset, VariableDataBlock vdb)
    {
      int length = vdb.DIF.DataField != DataField.VariableLength ? vdb.DIF.DataFieldLength : (int) buffer[offset] + 1;
      if (length == 0)
        return;
      byte[] dst = new byte[length];
      Buffer.BlockCopy((Array) buffer, offset, (Array) dst, 0, dst.Length);
      vdb.Data = dst;
    }
  }
}
