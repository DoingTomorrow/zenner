// Decompiled with JetBrains decompiler
// Type: Standard.IEnumObjects
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("2c1c7e2e-2d0e-4059-831e-1e6f82335c2e")]
  [ComImport]
  internal interface IEnumObjects
  {
    void Next(uint celt, [In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown), Out] object[] rgelt, out uint pceltFetched);

    void Skip(uint celt);

    void Reset();

    IEnumObjects Clone();
  }
}
