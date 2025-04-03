// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBytes
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  internal static class SQLiteBytes
  {
    public static byte[] FromIntPtr(IntPtr pValue, int length)
    {
      if (pValue == IntPtr.Zero)
        return (byte[]) null;
      if (length == 0)
        return new byte[0];
      byte[] destination = new byte[length];
      Marshal.Copy(pValue, destination, 0, length);
      return destination;
    }

    public static IntPtr ToIntPtr(byte[] value)
    {
      if (value == null)
        return IntPtr.Zero;
      int length = value.Length;
      if (length == 0)
        return IntPtr.Zero;
      IntPtr destination = SQLiteMemory.Allocate(length);
      if (destination == IntPtr.Zero)
        return IntPtr.Zero;
      Marshal.Copy(value, 0, destination, length);
      return destination;
    }
  }
}
