// Decompiled with JetBrains decompiler
// Type: Standard.IShellLinkW
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("000214F9-0000-0000-C000-000000000046")]
  [ComImport]
  internal interface IShellLinkW
  {
    void GetPath([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszFile, int cchMaxPath, [In, Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

    void GetIDList(out IntPtr ppidl);

    void SetIDList(IntPtr pidl);

    void GetDescription([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszFile, int cchMaxName);

    void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

    void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszDir, int cchMaxPath);

    void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

    void GetArguments([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszArgs, int cchMaxPath);

    void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

    short GetHotKey();

    void SetHotKey(short wHotKey);

    uint GetShowCmd();

    void SetShowCmd(uint iShowCmd);

    void GetIconLocation([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

    void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

    void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

    void Resolve(IntPtr hwnd, uint fFlags);

    void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
  }
}
