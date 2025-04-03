// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.UnsafeNativeMethodsWin32
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace System.Data.SQLite
{
  [SuppressUnmanagedCodeSecurity]
  internal static class UnsafeNativeMethodsWin32
  {
    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
    internal static extern IntPtr LoadLibrary(string fileName);
  }
}
