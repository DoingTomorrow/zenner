// Decompiled with JetBrains decompiler
// Type: Standard.ShellUtil
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard
{
  internal static class ShellUtil
  {
    public static string GetPathFromShellItem(IShellItem item)
    {
      return item.GetDisplayName(SIGDN.DESKTOPABSOLUTEPARSING);
    }

    public static IShellItem2 GetShellItemForPath(string path)
    {
      if (string.IsNullOrEmpty(path))
        return (IShellItem2) null;
      Guid riid = new Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93");
      object ppv;
      HRESULT hresult = NativeMethods.SHCreateItemFromParsingName(path, (IBindCtx) null, ref riid, out ppv);
      if (hresult == (HRESULT) Win32Error.ERROR_FILE_NOT_FOUND || hresult == (HRESULT) Win32Error.ERROR_PATH_NOT_FOUND)
      {
        hresult = HRESULT.S_OK;
        ppv = (object) null;
      }
      hresult.ThrowIfFailed();
      return (IShellItem2) ppv;
    }
  }
}
