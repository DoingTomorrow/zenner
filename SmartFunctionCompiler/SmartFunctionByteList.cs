// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SmartFunctionByteList
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System;

#nullable disable
namespace SmartFunctionCompiler
{
  public class SmartFunctionByteList
  {
    public byte NumberOfBytes => (byte) this.DataBytes.Length;

    public byte[] DataBytes { get; private set; }

    public byte[] ValueBytes
    {
      get
      {
        byte[] valueBytes = new byte[(int) this.NumberOfBytes + 1];
        valueBytes[0] = this.NumberOfBytes;
        this.DataBytes.CopyTo((Array) valueBytes, 1);
        return valueBytes;
      }
    }

    public SmartFunctionByteList(byte[] dataBytes)
    {
      this.DataBytes = dataBytes != null && dataBytes.Length <= (int) byte.MaxValue ? new byte[dataBytes.Length] : throw new ArgumentException("Illegal ByteList data");
      dataBytes.CopyTo((Array) this.DataBytes, 0);
    }

    public SmartFunctionByteList(byte[] byteArray, int offset)
    {
      if (offset >= byteArray.Length)
        throw new Exception("offset out of byteArray range");
      byte count = byteArray[offset++];
      if (offset + (int) count >= byteArray.Length)
        throw new Exception("offset out of byteArray range");
      this.DataBytes = new byte[(int) count];
      Buffer.BlockCopy((Array) byteArray, offset, (Array) this.DataBytes, 0, (int) count);
    }

    public SmartFunctionByteList(string valueString)
    {
      valueString = valueString.Replace(" ", "");
      string[] strArray1 = valueString.Trim().Split(';');
      if (strArray1.Length == 2)
      {
        string[] strArray2 = strArray1[0].Trim().Split(':');
        byte num = strArray2.Length == 2 && strArray2[0].StartsWith("NoOfBytes") ? byte.Parse(strArray2[1]) : throw new Exception("Illegal ByteList bytes definition");
        this.DataBytes = ByteArrayScanner16Bit.HexStringToByteArray(strArray1[1]);
        if (this.DataBytes.Length != (int) num)
          throw new Exception("NoOfByte <> scanned bytes");
      }
      else
        this.DataBytes = strArray1.Length == 1 ? ByteArrayScanner16Bit.HexStringToByteArray(strArray1[0]) : throw new Exception("Illegal ByteList bytes definition");
    }

    public void ScanIn(byte[] byteArray, ref int offset)
    {
      byteArray[offset++] = this.NumberOfBytes;
      this.DataBytes.CopyTo((Array) byteArray, offset);
      offset += (int) this.NumberOfBytes;
    }

    public override string ToString()
    {
      string hexStringFromArray = Compiler.GetHexStringFromArray(this.DataBytes, splitBytes: true);
      return "NoOfBytes:" + this.NumberOfBytes.ToString() + ";" + hexStringFromArray;
    }
  }
}
