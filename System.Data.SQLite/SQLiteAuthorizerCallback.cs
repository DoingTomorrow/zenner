// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteAuthorizerCallback
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  internal delegate SQLiteAuthorizerReturnCode SQLiteAuthorizerCallback(
    IntPtr pUserData,
    SQLiteAuthorizerActionCode actionCode,
    IntPtr pArgument1,
    IntPtr pArgument2,
    IntPtr pDatabase,
    IntPtr pAuthContext);
}
