// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffSST
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffSST : XlsBiffRecord
  {
    private readonly List<uint> continues = new List<uint>();
    private readonly List<string> m_strings;
    private uint m_size;

    internal XlsBiffSST(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
      this.m_size = (uint) this.RecordSize;
      this.m_strings = new List<string>();
    }

    public uint Count => this.ReadUInt32(0);

    public uint UniqueCount => this.ReadUInt32(4);

    public void ReadStrings()
    {
      uint num1 = (uint) (this.m_readoffset + 8);
      uint num2 = (uint) this.m_readoffset + (uint) this.RecordSize;
      int index = 0;
      uint num3 = this.UniqueCount;
      while (num1 < num2)
      {
        XlsFormattedUnicodeString formattedUnicodeString = new XlsFormattedUnicodeString(this.m_bytes, num1);
        uint headSize = formattedUnicodeString.HeadSize;
        uint tailSize = formattedUnicodeString.TailSize;
        uint characterCount = (uint) formattedUnicodeString.CharacterCount;
        uint num4 = (uint) ((int) headSize + (int) tailSize + (int) characterCount + (formattedUnicodeString.IsMultiByte ? (int) characterCount : 0));
        if (num1 + num4 > num2)
        {
          if (index >= this.continues.Count)
            break;
          uint num5 = this.continues[index];
          byte num6 = Buffer.GetByte((Array) this.m_bytes, (int) num5 + 4);
          byte[] numArray = new byte[(IntPtr) (num4 * 2U)];
          Buffer.BlockCopy((Array) this.m_bytes, (int) num1, (Array) numArray, 0, (int) num2 - (int) num1);
          if (num6 == (byte) 0 && formattedUnicodeString.IsMultiByte)
          {
            uint count = characterCount - (num2 - headSize - num1) / 2U;
            byte[] bytes = Encoding.Unicode.GetBytes(Encoding.Default.GetString(this.m_bytes, (int) num5 + 5, (int) count));
            Buffer.BlockCopy((Array) bytes, 0, (Array) numArray, (int) num2 - (int) num1, bytes.Length);
            Buffer.BlockCopy((Array) this.m_bytes, (int) num5 + 5 + (int) count, (Array) numArray, (int) num2 - (int) num1 + (int) count + (int) count, (int) tailSize);
            num1 = num5 + 5U + count + tailSize;
          }
          else if (num6 == (byte) 1 && !formattedUnicodeString.IsMultiByte)
          {
            uint num7 = characterCount - (num2 - num1 - headSize);
            byte[] bytes = Encoding.Default.GetBytes(Encoding.Unicode.GetString(this.m_bytes, (int) num5 + 5, (int) num7 + (int) num7));
            Buffer.BlockCopy((Array) bytes, 0, (Array) numArray, (int) num2 - (int) num1, bytes.Length);
            Buffer.BlockCopy((Array) this.m_bytes, (int) num5 + 5 + (int) num7 + (int) num7, (Array) numArray, (int) num2 - (int) num1 + (int) num7, (int) tailSize);
            num1 = num5 + 5U + num7 + num7 + tailSize;
          }
          else
          {
            Buffer.BlockCopy((Array) this.m_bytes, (int) num5 + 5, (Array) numArray, (int) num2 - (int) num1, (int) num4 - (int) num2 + (int) num1);
            num1 = num5 + 5U + num4 - num2 + num1;
          }
          num2 = num5 + 4U + (uint) BitConverter.ToUInt16(this.m_bytes, (int) num5 + 2);
          ++index;
          formattedUnicodeString = new XlsFormattedUnicodeString(numArray, 0U);
        }
        else
        {
          num1 += num4;
          if ((int) num1 == (int) num2)
          {
            if (index < this.continues.Count)
            {
              uint num8 = this.continues[index];
              num1 = num8 + 4U;
              num2 = num1 + (uint) BitConverter.ToUInt16(this.m_bytes, (int) num8 + 2);
              ++index;
            }
            else
              num3 = 1U;
          }
        }
        this.m_strings.Add(formattedUnicodeString.Value);
        --num3;
        if (num3 == 0U)
          break;
      }
    }

    public string GetString(uint SSTIndex)
    {
      return (long) SSTIndex < (long) this.m_strings.Count ? this.m_strings[(int) SSTIndex] : string.Empty;
    }

    public void Append(XlsBiffContinue fragment)
    {
      this.continues.Add((uint) fragment.Offset);
      this.m_size += (uint) fragment.Size;
    }
  }
}
