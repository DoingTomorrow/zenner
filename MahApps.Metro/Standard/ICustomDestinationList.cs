// Decompiled with JetBrains decompiler
// Type: Standard.ICustomDestinationList
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
  [Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
  [ComImport]
  internal interface ICustomDestinationList
  {
    void SetAppID([MarshalAs(UnmanagedType.LPWStr), In] string pszAppID);

    [return: MarshalAs(UnmanagedType.Interface)]
    object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

    void AppendKnownCategory(KDC category);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT AddUserTasks(IObjectArray poa);

    void CommitList();

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetRemovedDestinations([In] ref Guid riid);

    void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

    void AbortList();
  }
}
