// Decompiled with JetBrains decompiler
// Type: Standard.IServiceProvider
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
  [ComImport]
  internal interface IServiceProvider
  {
    [return: MarshalAs(UnmanagedType.IUnknown)]
    object QueryService(ref Guid guidService, ref Guid riid);
  }
}
