// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteMemory
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  internal static class SQLiteMemory
  {
    public static IntPtr Allocate(int size) => UnsafeNativeMethods.sqlite3_malloc(size);

    public static int Size(IntPtr pMemory)
    {
      return UnsafeNativeMethods.sqlite3_malloc_size_interop(pMemory);
    }

    public static void Free(IntPtr pMemory) => UnsafeNativeMethods.sqlite3_free(pMemory);
  }
}
