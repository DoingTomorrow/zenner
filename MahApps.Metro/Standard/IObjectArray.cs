// Decompiled with JetBrains decompiler
// Type: Standard.IObjectArray
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
  [ComImport]
  internal interface IObjectArray
  {
    uint GetCount();

    [return: MarshalAs(UnmanagedType.IUnknown)]
    object GetAt([In] uint uiIndex, [In] ref Guid riid);
  }
}
