// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsFormattedUnicodeString
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Text;

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsFormattedUnicodeString
  {
    protected byte[] m_bytes;
    protected uint m_offset;

    public XlsFormattedUnicodeString(byte[] bytes, uint offset)
    {
      this.m_bytes = bytes;
      this.m_offset = offset;
    }

    public ushort CharacterCount => BitConverter.ToUInt16(this.m_bytes, (int) this.m_offset);

    public XlsFormattedUnicodeString.FormattedUnicodeStringFlags Flags
    {
      get
      {
        return (XlsFormattedUnicodeString.FormattedUnicodeStringFlags) Buffer.GetByte((Array) this.m_bytes, (int) this.m_offset + 2);
      }
    }

    public bool HasExtString => false;

    public bool HasFormatting
    {
      get
      {
        return (this.Flags & XlsFormattedUnicodeString.FormattedUnicodeStringFlags.HasFormatting) == XlsFormattedUnicodeString.FormattedUnicodeStringFlags.HasFormatting;
      }
    }

    public bool IsMultiByte
    {
      get
      {
        return (this.Flags & XlsFormattedUnicodeString.FormattedUnicodeStringFlags.MultiByte) == XlsFormattedUnicodeString.FormattedUnicodeStringFlags.MultiByte;
      }
    }

    private uint ByteCount => (uint) this.CharacterCount * (this.IsMultiByte ? 2U : 1U);

    public ushort FormatCount
    {
      get
      {
        return !this.HasFormatting ? (ushort) 0 : BitConverter.ToUInt16(this.m_bytes, (int) this.m_offset + 3);
      }
    }

    public uint ExtendedStringSize
    {
      get
      {
        return !this.HasExtString ? 0U : (uint) BitConverter.ToUInt16(this.m_bytes, (int) this.m_offset + (this.HasFormatting ? 5 : 3));
      }
    }

    public uint HeadSize => (uint) ((this.HasFormatting ? 2 : 0) + (this.HasExtString ? 4 : 0) + 3);

    public uint TailSize
    {
      get
      {
        return (uint) ((this.HasFormatting ? 4 * (int) this.FormatCount : 0) + (this.HasExtString ? (int) this.ExtendedStringSize : 0));
      }
    }

    public uint Size
    {
      get
      {
        uint num = (uint) ((this.HasFormatting ? 2 + (int) this.FormatCount * 4 : 0) + (this.HasExtString ? 4 + (int) this.ExtendedStringSize : 0) + 3);
        return !this.IsMultiByte ? num + (uint) this.CharacterCount : num + (uint) this.CharacterCount * 2U;
      }
    }

    public string Value
    {
      get
      {
        return !this.IsMultiByte ? Encoding.Default.GetString(this.m_bytes, (int) this.m_offset + (int) this.HeadSize, (int) this.ByteCount) : Encoding.Unicode.GetString(this.m_bytes, (int) this.m_offset + (int) this.HeadSize, (int) this.ByteCount);
      }
    }

    [System.Flags]
    public enum FormattedUnicodeStringFlags : byte
    {
      MultiByte = 1,
      HasExtendedString = 4,
      HasFormatting = 8,
    }
  }
}
