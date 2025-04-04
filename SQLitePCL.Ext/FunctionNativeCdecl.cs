// Decompiled with JetBrains decompiler
// Type: SQLitePCL.FunctionNativeCdecl
// Assembly: SQLitePCL.Ext, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 28DC4D07-0E35-45C1-8EF3-CED69BFBD581
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.Ext.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace SQLitePCL
{
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  internal delegate void FunctionNativeCdecl(
    IntPtr context,
    int numberOfArguments,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] arguments);
}
