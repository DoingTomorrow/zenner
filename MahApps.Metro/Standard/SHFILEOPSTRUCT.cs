// Decompiled with JetBrains decompiler
// Type: Standard.SHFILEOPSTRUCT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
  internal struct SHFILEOPSTRUCT
  {
    public IntPtr hwnd;
    [MarshalAs(UnmanagedType.U4)]
    public FO wFunc;
    public string pFrom;
    public string pTo;
    [MarshalAs(UnmanagedType.U2)]
    public FOF fFlags;
    [MarshalAs(UnmanagedType.Bool)]
    public int fAnyOperationsAborted;
    public IntPtr hNameMappings;
    public string lpszProgressTitle;
  }
}
