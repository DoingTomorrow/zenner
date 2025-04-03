// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteString
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace System.Data.SQLite
{
  internal static class SQLiteString
  {
    private static int ThirtyBits = 1073741823;
    private static readonly Encoding Utf8Encoding = Encoding.UTF8;

    public static byte[] GetUtf8BytesFromString(string value)
    {
      return value == null ? (byte[]) null : SQLiteString.Utf8Encoding.GetBytes(value);
    }

    public static string GetStringFromUtf8Bytes(byte[] bytes)
    {
      return bytes == null ? (string) null : SQLiteString.Utf8Encoding.GetString(bytes);
    }

    public static int ProbeForUtf8ByteLength(IntPtr pValue, int limit)
    {
      int ofs = 0;
      if (pValue != IntPtr.Zero && limit > 0)
      {
        while (Marshal.ReadByte(pValue, ofs) != (byte) 0 && ofs < limit)
          ++ofs;
      }
      return ofs;
    }

    public static string StringFromUtf8IntPtr(IntPtr pValue)
    {
      return SQLiteString.StringFromUtf8IntPtr(pValue, SQLiteString.ProbeForUtf8ByteLength(pValue, SQLiteString.ThirtyBits));
    }

    public static string StringFromUtf8IntPtr(IntPtr pValue, int length)
    {
      if (pValue == IntPtr.Zero)
        return (string) null;
      if (length <= 0)
        return string.Empty;
      byte[] numArray = new byte[length];
      Marshal.Copy(pValue, numArray, 0, length);
      return SQLiteString.GetStringFromUtf8Bytes(numArray);
    }

    public static IntPtr Utf8IntPtrFromString(string value)
    {
      if (value == null)
        return IntPtr.Zero;
      IntPtr zero = IntPtr.Zero;
      byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
      if (utf8BytesFromString == null)
        return IntPtr.Zero;
      int length = utf8BytesFromString.Length;
      IntPtr num = SQLiteMemory.Allocate(length + 1);
      if (num == IntPtr.Zero)
        return IntPtr.Zero;
      Marshal.Copy(utf8BytesFromString, 0, num, length);
      Marshal.WriteByte(num, length, (byte) 0);
      return num;
    }

    public static string[] StringArrayFromUtf8SizeAndIntPtr(int argc, IntPtr argv)
    {
      if (argc < 0)
        return (string[]) null;
      if (argv == IntPtr.Zero)
        return (string[]) null;
      string[] strArray = new string[argc];
      int index = 0;
      int offset = 0;
      while (index < strArray.Length)
      {
        IntPtr pValue = SQLiteMarshal.ReadIntPtr(argv, offset);
        strArray[index] = pValue != IntPtr.Zero ? SQLiteString.StringFromUtf8IntPtr(pValue) : (string) null;
        ++index;
        offset += IntPtr.Size;
      }
      return strArray;
    }

    public static IntPtr[] Utf8IntPtrArrayFromStringArray(string[] values)
    {
      if (values == null)
        return (IntPtr[]) null;
      IntPtr[] numArray = new IntPtr[values.Length];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = SQLiteString.Utf8IntPtrFromString(values[index]);
      return numArray;
    }
  }
}
