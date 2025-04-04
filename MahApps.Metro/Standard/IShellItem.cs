// Decompiled with JetBrains decompiler
// Type: Standard.IShellItem
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
  [ComImport]
  internal interface IShellItem
  {
    [return: MarshalAs(UnmanagedType.Interface)]
    object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

    IShellItem GetParent();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetDisplayName(SIGDN sigdnName);

    SFGAO GetAttributes(SFGAO sfgaoMask);

    int Compare(IShellItem psi, SICHINT hint);
  }
}
