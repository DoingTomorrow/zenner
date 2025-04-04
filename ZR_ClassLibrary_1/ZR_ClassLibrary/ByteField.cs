// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ByteField
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ByteField
  {
    public byte[] Data;
    public int Count;

    public ByteField()
    {
      this.Data = new byte[270];
      this.Count = 0;
    }

    public ByteField(int Size)
    {
      this.Data = new byte[Size];
      this.Count = 0;
    }

    public ByteField(byte[] ByteArray)
    {
      this.Data = ByteArray;
      this.Count = ByteArray.Length;
    }

    public ByteField(List<byte> theList)
    {
      this.Data = theList.ToArray();
      this.Count = theList.Count;
    }

    public void Add(byte Byte) => this.Data[this.Count++] = Byte;

    public void Add(int IntToByte) => this.Data[this.Count++] = (byte) IntToByte;

    public void Add(ref ByteField FieldToAdd)
    {
      for (int index = 0; index < FieldToAdd.Count; ++index)
        this.Data[this.Count++] = FieldToAdd.Data[index];
    }

    public void Add(ByteField FieldToAdd)
    {
      for (int index = 0; index < FieldToAdd.Count; ++index)
        this.Data[this.Count++] = FieldToAdd.Data[index];
    }

    public void Add(byte[] buffer)
    {
      for (int index = 0; index < buffer.Length; ++index)
        this.Data[this.Count++] = buffer[index];
    }

    public void Add(string StringToAdd)
    {
      for (int index = 0; index < StringToAdd.Length; ++index)
        this.Data[this.Count++] = (byte) StringToAdd[index];
    }

    public void ToHexString(ref string DataString)
    {
      for (int index = 0; index < this.Count; ++index)
        DataString = DataString + "0x" + this.Data[index].ToString("x2") + " ";
    }

    public void GetOptimalField(out byte[] DataOut)
    {
      DataOut = new byte[this.Count];
      for (int index = 0; index < this.Count; ++index)
        DataOut[index] = this.Data[index];
    }

    public string GetTraceString()
    {
      StringBuilder stringBuilder = new StringBuilder(" ");
      for (int index = 0; index < this.Count; ++index)
        stringBuilder.Append(this.Data[index].ToString("x02"));
      return stringBuilder.ToString();
    }

    public byte[] GetByteArray()
    {
      byte[] byteArray = new byte[this.Count];
      for (int index = 0; index < this.Count; ++index)
        byteArray[index] = this.Data[index];
      return byteArray;
    }
  }
}
