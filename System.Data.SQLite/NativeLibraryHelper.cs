// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.NativeLibraryHelper
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  internal static class NativeLibraryHelper
  {
    private static IntPtr LoadLibraryWin32(string fileName)
    {
      return UnsafeNativeMethodsWin32.LoadLibrary(fileName);
    }

    private static IntPtr LoadLibraryPosix(string fileName)
    {
      return UnsafeNativeMethodsPosix.dlopen(fileName, 258);
    }

    public static IntPtr LoadLibrary(string fileName)
    {
      NativeLibraryHelper.LoadLibraryCallback loadLibraryCallback = new NativeLibraryHelper.LoadLibraryCallback(NativeLibraryHelper.LoadLibraryWin32);
      if (!HelperMethods.IsWindows())
        loadLibraryCallback = new NativeLibraryHelper.LoadLibraryCallback(NativeLibraryHelper.LoadLibraryPosix);
      return loadLibraryCallback(fileName);
    }

    private delegate IntPtr LoadLibraryCallback(string fileName);
  }
}
