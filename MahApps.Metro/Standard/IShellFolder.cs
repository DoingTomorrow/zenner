﻿// Decompiled with JetBrains decompiler
// Type: Standard.IShellFolder
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("000214E6-0000-0000-C000-000000000046")]
  [ComImport]
  internal interface IShellFolder
  {
    void ParseDisplayName(
      [In] IntPtr hwnd,
      [In] IBindCtx pbc,
      [MarshalAs(UnmanagedType.LPWStr), In] string pszDisplayName,
      [In, Out] ref int pchEaten,
      out IntPtr ppidl,
      [In, Out] ref uint pdwAttributes);

    IEnumIDList EnumObjects([In] IntPtr hwnd, [In] SHCONTF grfFlags);

    [return: MarshalAs(UnmanagedType.Interface)]
    object BindToObject([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

    [return: MarshalAs(UnmanagedType.Interface)]
    object BindToStorage([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT CompareIDs([In] IntPtr lParam, [In] IntPtr pidl1, [In] IntPtr pidl2);

    [return: MarshalAs(UnmanagedType.Interface)]
    object CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid);

    void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref SFGAO rgfInOut);

    [return: MarshalAs(UnmanagedType.Interface)]
    object GetUIObjectOf(
      [In] IntPtr hwndOwner,
      [In] uint cidl,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2, ArraySubType = UnmanagedType.SysInt), In] IntPtr apidl,
      [In] ref Guid riid,
      [In, Out] ref uint rgfReserved);

    void GetDisplayNameOf([In] IntPtr pidl, [In] SHGDN uFlags, out IntPtr pName);

    void SetNameOf([In] IntPtr hwnd, [In] IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr), In] string pszName, [In] SHGDN uFlags, out IntPtr ppidlOut);
  }
}
