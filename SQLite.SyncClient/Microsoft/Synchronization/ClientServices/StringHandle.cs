// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.StringHandle
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class StringHandle
  {
    private static readonly string[] ConstStrings = new string[3]
    {
      nameof (type),
      "root",
      "item"
    };
    private readonly XmlBufferReader bufferReader;
    private int key;
    private int length;
    private int offset;
    private StringHandle.StringHandleType type;

    public StringHandle(XmlBufferReader bufferReader)
    {
      this.bufferReader = bufferReader;
      this.SetValue(0, 0);
    }

    public bool IsEmpty
    {
      get
      {
        return this.type == StringHandle.StringHandleType.UTF8 ? this.length == 0 : this.Equals2(string.Empty);
      }
    }

    public bool IsXmlns
    {
      get
      {
        if (this.type != 0)
          return this.Equals2("xmlns");
        if (this.length != 5)
          return false;
        byte[] buffer = this.bufferReader.Buffer;
        int offset = this.offset;
        return buffer[offset] == (byte) 120 && buffer[offset + 1] == (byte) 109 && buffer[offset + 2] == (byte) 108 && buffer[offset + 3] == (byte) 110 && buffer[offset + 4] == (byte) 115;
      }
    }

    public static bool operator ==(StringHandle s1, string s2) => s1.Equals2(s2);

    public static bool operator !=(StringHandle s1, string s2) => !s1.Equals2(s2);

    public static bool operator ==(StringHandle s1, StringHandle s2) => s1.Equals2(s2);

    public static bool operator !=(StringHandle s1, StringHandle s2) => !s1.Equals2(s2);

    public void SetValue(int iOffset, int iLength)
    {
      this.type = StringHandle.StringHandleType.UTF8;
      this.offset = iOffset;
      this.length = iLength;
    }

    public void SetConstantValue(StringHandleConstStringType constStringType)
    {
      this.type = StringHandle.StringHandleType.ConstString;
      this.key = (int) constStringType;
    }

    public void SetValue(int iOffset, int iLength, bool escaped)
    {
      this.type = escaped ? StringHandle.StringHandleType.EscapedUTF8 : StringHandle.StringHandleType.UTF8;
      this.offset = iOffset;
      this.length = iLength;
    }

    public void SetValue(StringHandle value)
    {
      this.type = value.type;
      this.key = value.key;
      this.offset = value.offset;
      this.length = value.length;
    }

    public string GetString(XmlNameTable nameTable)
    {
      switch (this.type)
      {
        case StringHandle.StringHandleType.UTF8:
          return this.bufferReader.GetString(this.offset, this.length, nameTable);
        case StringHandle.StringHandleType.ConstString:
          return nameTable.Add(StringHandle.ConstStrings[this.key]);
        default:
          return this.bufferReader.GetEscapedString(this.offset, this.length, nameTable);
      }
    }

    public string GetString()
    {
      switch (this.type)
      {
        case StringHandle.StringHandleType.UTF8:
          return this.bufferReader.GetString(this.offset, this.length);
        case StringHandle.StringHandleType.ConstString:
          return StringHandle.ConstStrings[this.key];
        default:
          return this.bufferReader.GetEscapedString(this.offset, this.length);
      }
    }

    public byte[] GetString(out int iOffset, out int iLength)
    {
      switch (this.type)
      {
        case StringHandle.StringHandleType.UTF8:
          iOffset = this.offset;
          iLength = this.length;
          return this.bufferReader.Buffer;
        case StringHandle.StringHandleType.ConstString:
          byte[] bytes1 = XmlConverter.ToBytes(StringHandle.ConstStrings[this.key]);
          iOffset = 0;
          iLength = bytes1.Length;
          return bytes1;
        default:
          byte[] bytes2 = XmlConverter.ToBytes(this.bufferReader.GetEscapedString(this.offset, this.length));
          iOffset = 0;
          iLength = bytes2.Length;
          return bytes2;
      }
    }

    public bool TryGetDictionaryString(out XmlDictionaryString value)
    {
      if (this.IsEmpty)
      {
        value = XmlDictionaryString.Empty;
        return true;
      }
      value = (XmlDictionaryString) null;
      return false;
    }

    public override string ToString() => this.GetString();

    private bool Equals2(string s2) => this.GetString() == s2;

    private bool Equals2(int offset2, int length2, XmlBufferReader bufferReader2)
    {
      return this.type == StringHandle.StringHandleType.UTF8 ? this.bufferReader.Equals2(this.offset, this.length, bufferReader2, offset2, length2) : this.GetString() == this.bufferReader.GetString(offset2, length2);
    }

    private bool Equals2(StringHandle s2)
    {
      return s2.type == StringHandle.StringHandleType.UTF8 ? this.Equals2(s2.offset, s2.length, s2.bufferReader) : this.Equals2(s2.GetString());
    }

    public int CompareTo(StringHandle that)
    {
      return this.type == StringHandle.StringHandleType.UTF8 && that.type == StringHandle.StringHandleType.UTF8 ? this.bufferReader.Compare(this.offset, this.length, that.offset, that.length) : string.Compare(this.GetString(), that.GetString(), StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
      StringHandle stringHandle = obj as StringHandle;
      return (object) stringHandle != null && this == stringHandle;
    }

    public override int GetHashCode() => this.GetString().GetHashCode();

    private enum StringHandleType
    {
      UTF8,
      EscapedUTF8,
      ConstString,
    }
  }
}
