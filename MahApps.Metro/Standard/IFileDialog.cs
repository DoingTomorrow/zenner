// Decompiled with JetBrains decompiler
// Type: Standard.IFileDialog
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
  [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
  [ComImport]
  internal interface IFileDialog : IModalWindow
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    new HRESULT Show(IntPtr parent);

    void SetFileTypes(uint cFileTypes, [In] ref COMDLG_FILTERSPEC rgFilterSpec);

    void SetFileTypeIndex(uint iFileType);

    uint GetFileTypeIndex();

    uint Advise(IFileDialogEvents pfde);

    void Unadvise(uint dwCookie);

    void SetOptions(FOS fos);

    FOS GetOptions();

    void SetDefaultFolder(IShellItem psi);

    void SetFolder(IShellItem psi);

    IShellItem GetFolder();

    IShellItem GetCurrentSelection();

    void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetFileName();

    void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

    void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

    void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

    IShellItem GetResult();

    void AddPlace(IShellItem psi, FDAP alignment);

    void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

    void Close([MarshalAs(UnmanagedType.Error)] int hr);

    void SetClientGuid([In] ref Guid guid);

    void ClearClientData();

    void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);
  }
}
