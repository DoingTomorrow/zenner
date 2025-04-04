// Decompiled with JetBrains decompiler
// Type: Standard.IShellItemArray
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
  [Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
  [ComImport]
  internal interface IShellItemArray
  {
    [return: MarshalAs(UnmanagedType.Interface)]
    object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetPropertyStore(int flags, [In] ref Guid riid);

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

    uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

    uint GetCount();

    IShellItem GetItemAt(uint dwIndex);

    [return: MarshalAs(UnmanagedType.Interface)]
    object EnumItems();
  }
}
