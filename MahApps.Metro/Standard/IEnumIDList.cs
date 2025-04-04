// Decompiled with JetBrains decompiler
// Type: Standard.IEnumIDList
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("000214F2-0000-0000-C000-000000000046")]
  [ComImport]
  internal interface IEnumIDList
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT Skip(uint celt);

    void Reset();

    void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
  }
}
