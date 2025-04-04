// Decompiled with JetBrains decompiler
// Type: Standard.IApplicationDocumentLists
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06")]
  [ComImport]
  internal interface IApplicationDocumentLists
  {
    void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

    [return: MarshalAs(UnmanagedType.IUnknown)]
    object GetList([In] APPDOCLISTTYPE listtype, [In] uint cItemsDesired, [In] ref Guid riid);
  }
}
