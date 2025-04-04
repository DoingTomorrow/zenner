// Decompiled with JetBrains decompiler
// Type: Standard.IFileDialogEvents
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("973510DB-7D7F-452B-8975-74A85828D354")]
  [ComImport]
  internal interface IFileDialogEvents
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT OnFileOk(IFileDialog pfd);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT OnFolderChanging(IFileDialog pfd, IShellItem psiFolder);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT OnFolderChange(IFileDialog pfd);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT OnSelectionChange(IFileDialog pfd);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT OnShareViolation(IFileDialog pfd, IShellItem psi, out FDESVR pResponse);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT OnTypeChange(IFileDialog pfd);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    HRESULT OnOverwrite(IFileDialog pfd, IShellItem psi, out FDEOR pResponse);
  }
}
